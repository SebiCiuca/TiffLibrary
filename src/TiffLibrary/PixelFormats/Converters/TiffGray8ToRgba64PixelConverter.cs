﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TiffLibrary.PixelConverter;

namespace TiffLibrary.PixelFormats.Converters
{

    internal class TiffGray8ToRgba64PixelConverter : TiffPixelConverter<TiffGray8, TiffRgba64>
    {
        public TiffGray8ToRgba64PixelConverter(ITiffPixelBufferWriter<TiffRgba64> writer) : base(writer) { }

        public override void Convert(ReadOnlySpan<TiffGray8> source, Span<TiffRgba64> destination)
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
                => typeof(TSource) == typeof(TiffGray8) && typeof(TDestination) == typeof(TiffRgba64);
            public ITiffPixelBufferWriter<TSource> CreateConverter<TSource, TDestination>(ITiffPixelBufferWriter<TDestination> buffer)
                where TSource : unmanaged
                where TDestination : unmanaged
            {
                if (typeof(TSource) != typeof(TiffGray8) || typeof(TDestination) != typeof(TiffRgba64))
                {
                    ThrowHelper.ThrowInvalidOperationException();
                }
                return Unsafe.As<ITiffPixelBufferWriter<TSource>>(new TiffGray8ToRgba64PixelConverter(Unsafe.As<ITiffPixelBufferWriter<TiffRgba64>>(buffer)));
            }
        }

        internal class Converter : ITiffPixelSpanConverter<TiffGray8, TiffRgba64>
        {
            public void Convert(ReadOnlySpan<TiffGray8> source, Span<TiffRgba64> destination)
            {
                int length = source.Length;
                ref byte sourceRef = ref Unsafe.As<TiffGray8, byte>(ref MemoryMarshal.GetReference(source));
                ref ulong destinationRef = ref Unsafe.As<TiffRgba64, ulong>(ref MemoryMarshal.GetReference(destination));

                if (BitConverter.IsLittleEndian)
                {
                    for (int i = 0; i < length; i++)
                    {
                        ulong intensity = Unsafe.Add(ref sourceRef, i);
                        intensity = intensity << 8 | intensity;
                        intensity = intensity << 16 | intensity;
                        intensity = intensity << 32 | intensity;
                        intensity = 0xffff000000000000 | intensity;
                        Unsafe.Add(ref destinationRef, i) = intensity;
                    }
                }
                else
                {
                    for (int i = 0; i < length; i++)
                    {
                        ulong intensity = Unsafe.Add(ref sourceRef, i);
                        intensity = intensity << 8 | intensity;
                        intensity = intensity << 16 | intensity;
                        intensity = intensity << 32 | intensity;
                        intensity = 0xffff | intensity;
                        Unsafe.Add(ref destinationRef, i) = intensity;
                    }
                }
            }
        }
    }
}
