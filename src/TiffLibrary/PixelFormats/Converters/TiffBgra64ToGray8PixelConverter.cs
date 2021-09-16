﻿using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using TiffLibrary.PixelConverter;

namespace TiffLibrary.PixelFormats.Converters
{

    internal class TiffBgra64ToGray8PixelConverter : TiffPixelConverter<TiffBgra64, TiffGray8>
    {
        public TiffBgra64ToGray8PixelConverter(ITiffPixelBufferWriter<TiffGray8> writer) : base(writer) { }

        public override void Convert(ReadOnlySpan<TiffBgra64> source, Span<TiffGray8> destination)
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
                => typeof(TSource) == typeof(TiffBgra64) && typeof(TDestination) == typeof(TiffGray8);
            public ITiffPixelBufferWriter<TSource> CreateConverter<TSource, TDestination>(ITiffPixelBufferWriter<TDestination> buffer)
                where TSource : unmanaged
                where TDestination : unmanaged
            {
                if (typeof(TSource) != typeof(TiffBgra64) || typeof(TDestination) != typeof(TiffGray8))
                {
                    ThrowHelper.ThrowInvalidOperationException();
                }
                return Unsafe.As<ITiffPixelBufferWriter<TSource>>(new TiffBgra64ToGray8PixelConverter(Unsafe.As<ITiffPixelBufferWriter<TiffGray8>>(buffer)));
            }
        }

        internal class Converter : ITiffPixelSpanConverter<TiffBgra64, TiffGray8>
        {
            public void Convert(ReadOnlySpan<TiffBgra64> source, Span<TiffGray8> destination)
            {
                int length = source.Length;
                ref TiffBgra64 sourceRef = ref MemoryMarshal.GetReference(source);
                ref byte destinationRef = ref Unsafe.As<TiffGray8, byte>(ref MemoryMarshal.GetReference(destination));

                for (int i = 0; i < length; i++)
                {
                    TiffBgra64 pixel = Unsafe.Add(ref sourceRef, i);

                    ushort a = pixel.A;
                    pixel.B = (ushort)(pixel.B * a / ushort.MaxValue);
                    pixel.G = (ushort)(pixel.G * a / ushort.MaxValue);
                    pixel.R = (ushort)(pixel.R * a / ushort.MaxValue);

                    byte gray = (byte)((pixel.R * 38 + pixel.G * 75 + pixel.B * 15) >> 7);
                    Unsafe.Add(ref destinationRef, i) = gray;
                }
            }
        }
    }
}
