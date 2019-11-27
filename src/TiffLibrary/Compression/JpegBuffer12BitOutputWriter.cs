﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using JpegLibrary;
using TiffLibrary.Utils;

namespace TiffLibrary.Compression
{
    internal sealed class JpegBuffer12BitOutputWriter : JpegBlockOutputWriter
    {
        private int _width;
        private int _skippedScanlines;
        private int _height;
        private int _componentCount;
        private Memory<byte> _output;

        public JpegBuffer12BitOutputWriter(int width, int skippedScanlines, int height, int componentsCount, Memory<byte> output)
        {
            if (output.Length < (width * height * componentsCount))
            {
                throw new ArgumentException("Destination buffer is too small.");
            }

            _width = width;
            _skippedScanlines = skippedScanlines / 8 * 8; // Align to block
            _height = height;
            _componentCount = componentsCount;
            _output = output;
        }

        public void Reset()
        {
            _width = default;
            _height = default;
            _componentCount = default;
            _output = default;
        }

        public override void WriteBlock(in JpegBlock8x8 block, int componentIndex, int x, int y)
        {
            int componentCount = _componentCount;
            int width = _width;
            int height = _height;

            if (x > width || y > _height)
            {
                return;
            }
            if ((y + 8) <= _skippedScanlines)
            {
                // No need to decode region before the fist requested scanline.
                return;
            }

            int writeWidth = Math.Min(width - x, 8);
            int writeHeight = Math.Min(height - y, 8);

            ref short blockRef = ref Unsafe.As<JpegBlock8x8, short>(ref Unsafe.AsRef(block));
            ref ushort destinationRef = ref Unsafe.As<byte, ushort>(ref MemoryMarshal.GetReference(_output.Span));

            for (int destY = 0; destY < writeHeight; destY++)
            {
                ref short blockRowRef = ref Unsafe.Add(ref blockRef, destY * 8);
                ref ushort destinationRowRef = ref Unsafe.Add(ref destinationRef, ((y + destY) * width + x) * componentCount + componentIndex);
                for (int destX = 0; destX < writeWidth; destX++)
                {
                    Unsafe.Add(ref destinationRowRef, destX * componentCount) = (ushort)FastExpandBits(TiffMathHelper.ClampTo12Bit(Unsafe.Add(ref blockRowRef, destX)));
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static uint FastExpandBits(uint bits)
        {
            const int bitCount = 12;
            const int targetBitCount = 16;
            const int remainingBits = targetBitCount - bitCount;
            return (bits << remainingBits) | (bits & ((uint)(1 << remainingBits) - 1));
        }
    }
}