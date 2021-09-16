﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TiffLibrary.PixelConverter;

namespace TiffLibrary.PixelFormats.Converters
{
    internal class TiffBgra32ToRgba32PixelConverter : TiffPixelConverter<TiffBgra32, TiffRgba32>
    {
        public TiffBgra32ToRgba32PixelConverter(ITiffPixelBufferWriter<TiffRgba32> writer) : base(writer) { }

        public override void Convert(ReadOnlySpan<TiffBgra32> source, Span<TiffRgba32> destination)
        {
            SpanConverter.Convert(source, destination);
        }

        public static Factory FactoryInstance { get; } = new Factory();
        public static Converter SpanConverter { get; } = new Converter();

        internal class Factory : ITiffPixelConverterFactory
        {
            public bool IsConvertible<TSource, TDestination>()
                where TSource : unmanaged
                where TDestination : unmanaged
                => typeof(TSource) == typeof(TiffBgra32) && typeof(TDestination) == typeof(TiffRgba32);
            public ITiffPixelBufferWriter<TSource> CreateConverter<TSource, TDestination>(ITiffPixelBufferWriter<TDestination> buffer)
                where TSource : unmanaged
                where TDestination : unmanaged
            {
                if (typeof(TSource) != typeof(TiffBgra32) || typeof(TDestination) != typeof(TiffRgba32))
                {
                    ThrowHelper.ThrowInvalidOperationException();
                }
                return Unsafe.As<ITiffPixelBufferWriter<TSource>>(new TiffBgra32ToRgba32PixelConverter(Unsafe.As<ITiffPixelBufferWriter<TiffRgba32>>(buffer)));
            }
        }

        internal class Converter : ITiffPixelSpanConverter<TiffBgra32, TiffRgba32>
        {
            public void Convert(ReadOnlySpan<TiffBgra32> source, Span<TiffRgba32> destination)
            {
                int length = source.Length;
                ref uint sourceRef = ref Unsafe.As<TiffBgra32, uint>(ref MemoryMarshal.GetReference(source));
                ref uint destinationRef = ref Unsafe.As<TiffRgba32, uint>(ref MemoryMarshal.GetReference(destination));

                if (BitConverter.IsLittleEndian)
                {
                    for (int i = 0; i < length; i++)
                    {
                        Unsafe.Add(ref destinationRef, i) = ToBgra32LittleEndian(Unsafe.Add(ref sourceRef, i));
                    }
                }
                else
                {
                    for (int i = 0; i < length; i++)
                    {
                        Unsafe.Add(ref destinationRef, i) = ToBgra32BigEndian(Unsafe.Add(ref sourceRef, i));
                    }
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]

            public static uint ToBgra32LittleEndian(uint packedRgba)
            {
                // packedRgba          = [aa bb gg rr]
                // tmp1                = [aa 00 gg 00]
                // tmp2                = [00 bb 00 rr]
                // tmp3=ROTL(16, tmp2) = [00 rr 00 bb]
                // tmp1 + tmp3         = [aa rr gg bb]
                uint tmp1 = packedRgba & 0xFF00FF00;
                uint tmp2 = packedRgba & 0x00FF00FF;
                uint tmp3 = (tmp2 << 16) | (tmp2 >> 16);
                return tmp1 + tmp3;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]

            public static uint ToBgra32BigEndian(uint packedRgba)
            {
                // packedRgba          = [rr gg bb aa]
                // tmp1                = [rr 00 bb 00]
                // tmp2                = [00 gg 00 aa]
                // tmp3=ROTL(16, tmp1) = [bb 00 rr 00]
                // tmp2 + tmp3         = [bb gg rr aa]
                uint tmp1 = packedRgba & 0xFF00FF00;
                uint tmp2 = packedRgba & 0x00FF00FF;
                uint tmp3 = (tmp1 << 16) | (tmp1 >> 16);
                return tmp2 + tmp3;
            }

        }
    }
}
