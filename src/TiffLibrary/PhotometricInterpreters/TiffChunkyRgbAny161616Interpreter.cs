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
    /// A middleware to read any bits (less than 16 bits) RGB pixels from uncompressed data to destination buffer writer.
    /// </summary>
    public sealed class TiffChunkyRgbAny161616Interpreter : ITiffImageDecoderMiddleware
    {
        private readonly TiffValueCollection<ushort> _bitsPerSample;
        private readonly TiffFillOrder _fillOrder;

        /// <summary>
        /// Initialize the middleware with the specified bits per sample and fill order.
        /// </summary>
        /// <param name="bitsPerSample">The BitsPerSample flags.</param>
        /// <param name="fillOrder">The FillOrder tag.</param>
        [CLSCompliant(false)]
        public TiffChunkyRgbAny161616Interpreter(TiffValueCollection<ushort> bitsPerSample, TiffFillOrder fillOrder = TiffFillOrder.HigherOrderBitsFirst)
        {
            if (bitsPerSample.Count != 3)
            {
                throw new ArgumentOutOfRangeException(nameof(bitsPerSample));
            }
            if ((uint)bitsPerSample[0] > 16 || (uint)bitsPerSample[1] > 16 || (uint)bitsPerSample[2] > 16)
            {
                throw new ArgumentOutOfRangeException(nameof(bitsPerSample));
            }
            if (fillOrder == 0)
            {
                fillOrder = TiffFillOrder.HigherOrderBitsFirst;
            }
            _bitsPerSample = bitsPerSample;
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

            Span<ushort> bitsPerSample = stackalloc ushort[3];
            _bitsPerSample.CopyTo(bitsPerSample);
            bool isHigherOrderBitsFirst = _fillOrder != TiffFillOrder.LowerOrderBitsFirst;
            int totalBitsPerSample = bitsPerSample[0] + bitsPerSample[1] + bitsPerSample[2];

            int bytesPerScanline = (context.SourceImageSize.Width * totalBitsPerSample + 7) / 8;
            Memory<byte> source = context.UncompressedData.Slice(context.SourceReadOffset.Y * bytesPerScanline);
            ReadOnlySpan<byte> sourceSpan = source.Span;

            using TiffPixelBufferWriter<TiffRgba64> writer = context.GetWriter<TiffRgba64>();

            int rows = context.ReadSize.Height;
            int cols = context.ReadSize.Width;

            // BitReader.Read reads bytes in big-endian way, we only need to reverse the endianness if the source is little-endian.
            bool isLittleEndian = context.IsLittleEndian;
            bool reverseEndiannessR = isLittleEndian && bitsPerSample[0] % 8 == 0;
            bool reverseEndiannessG = isLittleEndian && bitsPerSample[1] % 8 == 0;
            bool reverseEndiannessB = isLittleEndian && bitsPerSample[2] % 8 == 0;
            bool canDoFastPath = bitsPerSample[0] >= 8 && bitsPerSample[1] >= 8 && bitsPerSample[2] >= 8
                                 && !reverseEndiannessR & !reverseEndiannessG & !reverseEndiannessB;

            TiffRgba64 pixel = default;
            pixel.A = ushort.MaxValue;

            for (int row = 0; row < rows; row++)
            {
                using TiffPixelSpanHandle<TiffRgba64> pixelSpanHandle = writer.GetRowSpan(row);
                Span<TiffRgba64> pixelSpan = pixelSpanHandle.GetSpan();
                var bitReader = new BitReader(sourceSpan.Slice(0, bytesPerScanline), isHigherOrderBitsFirst);
                bitReader.Advance(context.SourceReadOffset.X * totalBitsPerSample);

                if (canDoFastPath)
                {
                    // Fast path for bits >= 8
                    for (int col = 0; col < cols; col++)
                    {
                        pixel.R = (ushort)FastExpandBits(bitReader.Read(bitsPerSample[0]), bitsPerSample[0], 16);
                        pixel.G = (ushort)FastExpandBits(bitReader.Read(bitsPerSample[1]), bitsPerSample[1], 16);
                        pixel.B = (ushort)FastExpandBits(bitReader.Read(bitsPerSample[2]), bitsPerSample[2], 16);
                        pixelSpan[col] = pixel;
                    }
                }
                else
                {
                    // Slow path
                    for (int col = 0; col < cols; col++)
                    {
                        pixel.R = (ushort)ExpandBits(bitReader.Read(bitsPerSample[0]), bitsPerSample[0], 16, reverseEndiannessR);
                        pixel.G = (ushort)ExpandBits(bitReader.Read(bitsPerSample[1]), bitsPerSample[1], 16, reverseEndiannessG);
                        pixel.B = (ushort)ExpandBits(bitReader.Read(bitsPerSample[2]), bitsPerSample[2], 16, reverseEndiannessB);
                        pixelSpan[col] = pixel;
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

        private static uint ExpandBits(uint bits, int bitCount, int targetBitCount, bool reverseEndianness)
        {
            if (reverseEndianness)
            {
                Debug.Assert(bitCount % 8 == 0);
                // Left-align
                bits = bits << (32 - bitCount);
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
                return FastExpandBits(bits, currentBitCount, targetBitCount);
            }

            return bits;
        }
    }
}
