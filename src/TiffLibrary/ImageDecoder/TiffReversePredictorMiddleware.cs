﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace TiffLibrary.ImageDecoder
{
    /// <summary>
    /// A middleware that reverse predictor in the source image.
    /// </summary>
    public sealed class TiffReversePredictorMiddleware : ITiffImageDecoderMiddleware
    {
        private readonly TiffValueCollection<int> _bytesPerScanlines;
        private readonly TiffValueCollection<ushort> _bitsPerSample;
        private readonly TiffPredictor _predictor;

        /// <summary>
        /// Initialize the middleware.
        /// </summary>
        /// <param name="bytesPerScanlines">Byte count per scanline.</param>
        /// <param name="bitsPerSample">Bits per sample.</param>
        /// <param name="predictor">The predictor tag.</param>
        [CLSCompliant(false)]
        public TiffReversePredictorMiddleware(TiffValueCollection<int> bytesPerScanlines, TiffValueCollection<ushort> bitsPerSample, TiffPredictor predictor)
        {
            _bytesPerScanlines = bytesPerScanlines;
            _bitsPerSample = bitsPerSample;
            _predictor = predictor;
        }

        /// <inheritdoc />
        public ValueTask InvokeAsync(TiffImageDecoderContext context, ITiffImageDecoderPipelineNode next)
        {
            ThrowHelper.ThrowIfNull(context);
            ThrowHelper.ThrowIfNull(next);

            if (_predictor == TiffPredictor.None)
            {
                return next.RunAsync(context);
            }
            if (_predictor != TiffPredictor.HorizontalDifferencing)
            {
                ThrowHelper.ThrowNotSupportedException("Predictor not supportted.");
            }

            int skipped = 0;
            bool isMultiplePlanar = _bytesPerScanlines.Count > 1;
            for (int planarIndex = 0; planarIndex < _bytesPerScanlines.Count; planarIndex++)
            {
                int bytesPerScanline = _bytesPerScanlines[planarIndex];

                // Current plane buffer
                Span<byte> plane = context.UncompressedData.Span.Slice(skipped, bytesPerScanline * context.SourceImageSize.Height);

                // Skip scanlines that are not to be decoded
                plane = plane.Slice(bytesPerScanline * context.SourceReadOffset.Y);

                TiffValueCollection<ushort> bitsPerSample = isMultiplePlanar ? TiffValueCollection.Single(_bitsPerSample[planarIndex]) : _bitsPerSample;

                for (int row = 0; row < context.ReadSize.Height; row++)
                {
                    // Process every scanline
                    Span<byte> scanline = plane.Slice(row * bytesPerScanline, bytesPerScanline);
                    UndoHorizontalDifferencingForScanline(scanline, bitsPerSample, context.SourceImageSize.Width);
                }

                skipped += bytesPerScanline * context.SourceImageSize.Height;
            }

            return next.RunAsync(context);
        }

        private static void UndoHorizontalDifferencingForScanline(Span<byte> scanline, TiffValueCollection<ushort> bitsPerSample, int width)
        {
            if (width <= 1)
            {
                return;
            }
            int sampleCount = bitsPerSample.Count;
            if (sampleCount > 8)
            {
                ThrowHelper.ThrowNotSupportedException("Too many samples.");
            }

            Span<ushort> bitsPerSampleSpan = stackalloc ushort[8];
            ref ushort bitsPerSampleSpanRef = ref MemoryMarshal.GetReference(bitsPerSampleSpan);
            bitsPerSample.CopyTo(bitsPerSampleSpan);

            Span<uint> samples = stackalloc uint[8];
            ref uint samplesRef = ref MemoryMarshal.GetReference(samples);

            var reader = new BitReader(scanline);
            var writer = new BitWriter(scanline);

            for (int sampleIndex = 0; sampleIndex < sampleCount; sampleIndex++)
            {
                int bits = Unsafe.Add(ref bitsPerSampleSpanRef, sampleIndex);
                if (bits > 32)
                {
                    ThrowHelper.ThrowNotSupportedException("Bits too large.");
                }
                uint value = reader.Read(bits);
                Unsafe.Add(ref samplesRef, sampleIndex) = value;
                writer.Write(value, bits);
            }

            for (int col = 1; col < width; col++)
            {
                for (int sampleIndex = 0; sampleIndex < sampleCount; sampleIndex++)
                {
                    int bits = Unsafe.Add(ref bitsPerSampleSpanRef, sampleIndex);
                    uint value = reader.Read(bits);
                    value += Unsafe.Add(ref samplesRef, sampleIndex);
                    writer.Write(value, bits);
                    Unsafe.Add(ref samplesRef, sampleIndex) = value;
                }
            }

            writer.Flush();
        }
    }
}
