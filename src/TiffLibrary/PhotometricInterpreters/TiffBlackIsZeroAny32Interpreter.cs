﻿using System;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TiffLibrary.ImageDecoder;
using TiffLibrary.PixelBuffer;
using TiffLibrary.PixelFormats;

namespace TiffLibrary.PhotometricInterpreters
{
    /// <summary>
    /// A middleware to read any bits (less than 32 bits) BlackIsZero pixels from uncompressed data to destination buffer writer.
    /// </summary>
    public sealed class TiffBlackIsZeroAny32Interpreter : ITiffImageDecoderMiddleware
    {
        private readonly int _bitCount;
        private readonly TiffFillOrder _fillOrder;

        /// <summary>
        /// Initialize the middleware with the specified bit count and fill order.
        /// </summary>
        /// <param name="bitCount">The bit count.</param>
        /// <param name="fillOrder">The FillOrder tag.</param>
        [CLSCompliant(false)]
        public TiffBlackIsZeroAny32Interpreter(int bitCount, TiffFillOrder fillOrder = TiffFillOrder.HigherOrderBitsFirst)
        {
            if ((uint)bitCount > 32)
            {
                throw new ArgumentOutOfRangeException(nameof(bitCount));
            }
            if (fillOrder == 0)
            {
                fillOrder = TiffFillOrder.HigherOrderBitsFirst;
            }
            _bitCount = bitCount;
            _fillOrder = fillOrder;
        }

        /// <inheritdoc />
        public ValueTask InvokeAsync(TiffImageDecoderContext context, ITiffImageDecoderPipelineNode next)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (next is null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            int bitCount = _bitCount;
            bool isHigherOrderBitsFirst = _fillOrder != TiffFillOrder.LowerOrderBitsFirst;

            int bytesPerScanline = (context.SourceImageSize.Width * bitCount + 7) / 8;
            Memory<byte> source = context.UncompressedData.Slice(context.SourceReadOffset.Y * bytesPerScanline);
            ReadOnlySpan<byte> sourceSpan = source.Span;

            using TiffPixelBufferWriter<TiffGray16> writer = context.GetWriter<TiffGray16>();

            int rows = context.ReadSize.Height;
            int cols = context.ReadSize.Width;

            // BitReader.Read reads bytes in big-endian way, we only need to reverse the endianness if the source is little-endian.
            bool reverseEndianness = context.IsLittleEndian && bitCount % 8 == 0;
            bool canDoFastPath = bitCount >= 16 && !reverseEndianness;

            for (int row = 0; row < rows; row++)
            {
                using TiffPixelSpanHandle<TiffGray16> pixelSpanHandle = writer.GetRowSpan(row);
                Span<TiffGray16> pixelSpan = pixelSpanHandle.GetSpan();
                var bitReader = new BitReader(sourceSpan.Slice(0, bytesPerScanline), isHigherOrderBitsFirst);
                bitReader.Advance(context.SourceReadOffset.X * bitCount);

                if (canDoFastPath)
                {
                    // Fast path for bits >= 16
                    for (int col = 0; col < cols; col++)
                    {
                        uint value = bitReader.Read(bitCount);
                        value = FastExpandBits(value, bitCount, 32);
                        pixelSpan[col] = new TiffGray16((ushort)(value >> 16));
                    }
                }
                else
                {
                    // Slow path
                    for (int col = 0; col < cols; col++)
                    {
                        uint value = bitReader.Read(bitCount);
                        value = (uint)ExpandBits(value, bitCount, 32, reverseEndianness);
                        pixelSpan[col] = new TiffGray16((ushort)(value >> 16));
                    }
                }

                sourceSpan = sourceSpan.Slice(bytesPerScanline);
            }

            return next.RunAsync(context);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint FastExpandBits(uint bits, int bitCount, int targetBitCount)
        {
            Debug.Assert(bitCount * 2 >= targetBitCount);
            int remainingBits = targetBitCount - bitCount;
            return (bits << remainingBits) | (bits & ((uint)(1 << remainingBits) - 1));
        }

        private static ulong ExpandBits(ulong bits, int bitCount, int targetBitCount, bool reverseEndianness)
        {
            if (reverseEndianness)
            {
                Debug.Assert(bitCount % 8 == 0);
                // Left-align
                bits = bits << (64 - bitCount);
                bits = BinaryPrimitives.ReverseEndianness(bits);
            }

            int currentBitCount = bitCount;
            while (currentBitCount < targetBitCount)
            {
                bits = (bits << bitCount) | bits;
                currentBitCount += bitCount;
            }

            if (currentBitCount > targetBitCount)
            {
                bits = bits >> bitCount;
                currentBitCount -= bitCount;
                return FastExpandBits((uint)bits, currentBitCount, targetBitCount);
            }

            return bits;
        }
    }
}
