﻿using System;
using System.Buffers.Binary;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TiffLibrary
{
    /// <summary>
    /// A reader class to read field value from IFD.
    /// </summary>
    public sealed partial class TiffFieldReader : IDisposable, IAsyncDisposable
    {
        private TiffFileContentReader? _reader;
        private TiffOperationContext? _context;
        private bool _leaveOpen;
        private readonly bool _reverseEndianNeeded;

        internal TiffFieldReader(TiffFileContentReader reader, TiffOperationContext context, bool leaveOpen = false)
        {
            _reader = reader;
            _context = context;
            _leaveOpen = leaveOpen;
            _reverseEndianNeeded = BitConverter.IsLittleEndian != context.IsLittleEndian;
        }

        private TiffFileContentReader GetAsyncReader()
        {
            TiffFileContentReader? reader = _reader;
            if (reader is null)
            {
                ThrowObjectDisposedException();
            }
            return reader;
        }

        private TiffFileContentReader GetSyncReader()
        {
            TiffFileContentReader? reader = _reader;
            if (reader is null)
            {
                ThrowObjectDisposedException();
            }
            return TiffSyncFileContentSource.WrapReader(reader);
        }

        [DoesNotReturn]
        private static void ThrowObjectDisposedException()
        {
            ThrowHelper.ThrowObjectDisposedException(nameof(TiffFieldReader));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _context = null;
            if (_reader is not null && !_leaveOpen)
            {
                _reader.Dispose();
            }
            _reader = null;
        }

        /// <inheritdoc />
        public async ValueTask DisposeAsync()
        {
            _context = null;
            if (_reader is not null && !_leaveOpen)
            {
                await _reader.DisposeAsync().ConfigureAwait(false);
            }
            _reader = null;
        }

        #region Copy values

        private void InternalCopyInt64Values<TDest>(ReadOnlySpan<byte> buffer, Span<TDest> values, Func<long, TDest>? convertFunc = null) where TDest : struct
        {
            ReadOnlySpan<byte> src = buffer.Slice(0, sizeof(long) * values.Length);
            bool reverseEndianNeeded = _reverseEndianNeeded;

            if ((typeof(TDest) == typeof(long) || typeof(TDest) == typeof(ulong)) && convertFunc is null)
            {
                if (!reverseEndianNeeded)
                {
                    MemoryMarshal.Cast<byte, TDest>(src).CopyTo(values);
                }
                else
                {
                    Span<long> dest = MemoryMarshal.Cast<TDest, long>(values);
                    for (int i = 0; i < values.Length; i++)
                    {
                        long temp = MemoryMarshal.Read<long>(src);
                        temp = BinaryPrimitives.ReverseEndianness(temp);
                        dest[i] = temp;
                        src = src.Slice(sizeof(long));
                    }
                }
            }
            else
            {
                ThrowHelper.ThrowIfNull(convertFunc);

                for (int i = 0; i < values.Length; i++)
                {
                    long temp = MemoryMarshal.Read<long>(src);
                    if (reverseEndianNeeded)
                    {
                        temp = BinaryPrimitives.ReverseEndianness(temp);
                    }
                    values[i] = convertFunc(temp);
                    src = src.Slice(sizeof(long));
                }
            }
        }

        private void InternalCopyInt32Values<TDest>(ReadOnlySpan<byte> buffer, Span<TDest> values, Func<int, TDest>? convertFunc = null) where TDest : struct
        {
            ReadOnlySpan<byte> src = buffer.Slice(0, sizeof(int) * values.Length);
            bool reverseEndianNeeded = _reverseEndianNeeded;

            if ((typeof(TDest) == typeof(int) || typeof(TDest) == typeof(uint)) && convertFunc is null)
            {
                if (!reverseEndianNeeded)
                {
                    MemoryMarshal.Cast<byte, TDest>(src).CopyTo(values);
                }
                else
                {
                    Span<int> dest = MemoryMarshal.Cast<TDest, int>(values);
                    for (int i = 0; i < values.Length; i++)
                    {
                        int temp = MemoryMarshal.Read<int>(src);
                        temp = BinaryPrimitives.ReverseEndianness(temp);
                        dest[i] = temp;
                        src = src.Slice(sizeof(int));
                    }
                }
            }
            else
            {
                ThrowHelper.ThrowIfNull(convertFunc);

                for (int i = 0; i < values.Length; i++)
                {
                    int temp = MemoryMarshal.Read<int>(src);
                    if (reverseEndianNeeded)
                    {
                        temp = BinaryPrimitives.ReverseEndianness(temp);
                    }
                    values[i] = convertFunc(temp);
                    src = src.Slice(sizeof(int));
                }
            }
        }

        private void InternalCopyInt16Values<TDest>(ReadOnlySpan<byte> buffer, Span<TDest> values, Func<short, TDest>? convertFunc = null) where TDest : struct
        {
            ReadOnlySpan<byte> src = buffer.Slice(0, sizeof(short) * values.Length);
            bool reverseEndianNeeded = _reverseEndianNeeded;

            if ((typeof(TDest) == typeof(short) || typeof(TDest) == typeof(ushort)) && convertFunc is null)
            {
                if (!reverseEndianNeeded)
                {
                    MemoryMarshal.Cast<byte, TDest>(src).CopyTo(values);
                }
                else
                {
                    Span<short> dest = MemoryMarshal.Cast<TDest, short>(values);
                    for (int i = 0; i < values.Length; i++)
                    {
                        short temp = MemoryMarshal.Read<short>(src);
                        temp = BinaryPrimitives.ReverseEndianness(temp);
                        dest[i] = temp;
                        src = src.Slice(sizeof(short));
                    }
                }
            }
            else
            {
                ThrowHelper.ThrowIfNull(convertFunc);

                for (int i = 0; i < values.Length; i++)
                {
                    short temp = MemoryMarshal.Read<short>(src);
                    if (reverseEndianNeeded)
                    {
                        temp = BinaryPrimitives.ReverseEndianness(temp);
                    }
                    values[i] = convertFunc(temp);
                    src = src.Slice(sizeof(short));
                }
            }
        }

        private static void InternalCopyByteValues<TDest>(ReadOnlySpan<byte> buffer, Span<TDest> values, Func<byte, TDest>? convertFunc) where TDest : struct
        {
            ReadOnlySpan<byte> src = buffer.Slice(0, values.Length);

            if ((typeof(TDest) == typeof(byte) || typeof(TDest) == typeof(sbyte)) && convertFunc is null)
            {
                MemoryMarshal.Cast<byte, TDest>(src).CopyTo(values);
            }
            else
            {
                ThrowHelper.ThrowIfNull(convertFunc);

                for (int i = 0; i < values.Length; i++)
                {
                    byte temp = src[i];
                    values[i] = convertFunc(temp);
                }
            }
        }

        private void InternalCopyDoubleValues<TDest>(ReadOnlySpan<byte> buffer, Span<TDest> values, Func<double, TDest>? convertFunc) where TDest : struct
        {
            ReadOnlySpan<byte> src = buffer.Slice(0, sizeof(double) * values.Length);
            bool reverseEndianNeeded = _reverseEndianNeeded;

            if (typeof(TDest) == typeof(double) && convertFunc is null)
            {
                if (!reverseEndianNeeded)
                {
                    MemoryMarshal.Cast<byte, TDest>(src).CopyTo(values);
                }
                else
                {
                    Span<double> dest = MemoryMarshal.Cast<TDest, double>(values);
                    for (int i = 0; i < values.Length; i++)
                    {
                        long temp = MemoryMarshal.Read<long>(src);
                        temp = BinaryPrimitives.ReverseEndianness(temp);
                        dest[i] = Int64BitsToDouble(temp);
                        src = src.Slice(sizeof(double));
                    }
                }
            }
            else
            {
                ThrowHelper.ThrowIfNull(convertFunc);

                for (int i = 0; i < values.Length; i++)
                {
                    long temp = MemoryMarshal.Read<long>(src);
                    if (reverseEndianNeeded)
                    {
                        temp = BinaryPrimitives.ReverseEndianness(temp);
                    }
                    values[i] = convertFunc(Int64BitsToDouble(temp));
                    src = src.Slice(sizeof(double));
                }
            }
        }

        private void InternalCopyFloatValues<TDest>(ReadOnlySpan<byte> buffer, Span<TDest> values, Func<float, TDest>? convertFunc = null) where TDest : struct
        {
            ReadOnlySpan<byte> src = buffer.Slice(0, sizeof(float) * values.Length);
            bool reverseEndianNeeded = _reverseEndianNeeded;

            if (typeof(TDest) == typeof(float) && convertFunc is null)
            {
                if (!reverseEndianNeeded)
                {
                    MemoryMarshal.Cast<byte, TDest>(src).CopyTo(values);
                }
                else
                {
                    Span<float> dest = MemoryMarshal.Cast<TDest, float>(values);
                    for (int i = 0; i < values.Length; i++)
                    {
                        int temp = MemoryMarshal.Read<int>(src);
                        temp = BinaryPrimitives.ReverseEndianness(temp);
                        dest[i] = Int32BitsToSingle(temp);
                        src = src.Slice(sizeof(float));
                    }
                }
            }
            else
            {
                ThrowHelper.ThrowIfNull(convertFunc);

                for (int i = 0; i < values.Length; i++)
                {
                    int temp = MemoryMarshal.Read<int>(src);
                    if (reverseEndianNeeded)
                    {
                        temp = BinaryPrimitives.ReverseEndianness(temp);
                    }
                    values[i] = convertFunc(Int32BitsToSingle(temp));
                    src = src.Slice(sizeof(float));
                }
            }
        }

        private static float Int32BitsToSingle(int value)
        {
            return Unsafe.As<int, float>(ref value);
        }

        private static double Int64BitsToDouble(long value)
        {
            return Unsafe.As<long, double>(ref value);
        }

        private void InternalCopyRationalValues<TDest>(ReadOnlySpan<byte> buffer, Span<TDest> values, Func<TiffRational, TDest>? convertFunc = null) where TDest : struct
        {
            ReadOnlySpan<byte> src = buffer.Slice(0, 8 * values.Length);
            bool reverseEndianNeeded = _reverseEndianNeeded;

            if (typeof(TDest) == typeof(TiffRational) && convertFunc is null)
            {
                Span<TiffRational> dest = MemoryMarshal.Cast<TDest, TiffRational>(values);

                for (int i = 0; i < values.Length; i++)
                {
                    uint numerator = MemoryMarshal.Read<uint>(src);
                    uint denominator = MemoryMarshal.Read<uint>(src.Slice(4));
                    if (reverseEndianNeeded)
                    {
                        numerator = BinaryPrimitives.ReverseEndianness(numerator);
                        denominator = BinaryPrimitives.ReverseEndianness(denominator);
                    }

                    dest[i] = new TiffRational(numerator, denominator);
                    src = src.Slice(8);
                }
            }
            else if (typeof(TDest) == typeof(TiffSRational) && convertFunc is null)
            {
                Span<TiffSRational> dest = MemoryMarshal.Cast<TDest, TiffSRational>(values);

                for (int i = 0; i < values.Length; i++)
                {
                    int numerator = MemoryMarshal.Read<int>(src);
                    int denominator = MemoryMarshal.Read<int>(src.Slice(4));
                    if (reverseEndianNeeded)
                    {
                        numerator = BinaryPrimitives.ReverseEndianness(numerator);
                        denominator = BinaryPrimitives.ReverseEndianness(denominator);
                    }

                    dest[i] = new TiffSRational(numerator, denominator);
                    src = src.Slice(8);
                }
            }
            else
            {
                ThrowHelper.ThrowIfNull(convertFunc);

                for (int i = 0; i < values.Length; i++)
                {
                    uint numerator = MemoryMarshal.Read<uint>(src);
                    uint denominator = MemoryMarshal.Read<uint>(src.Slice(4));
                    if (reverseEndianNeeded)
                    {
                        numerator = BinaryPrimitives.ReverseEndianness(numerator);
                        denominator = BinaryPrimitives.ReverseEndianness(denominator);
                    }

                    values[i] = convertFunc(new TiffRational(numerator, denominator));
                    src = src.Slice(8);
                }
            }
        }

        #endregion

        #region ASCII Parsing

        private static string ParseASCIIString(ReadOnlySpan<byte> buffer)
        {
            int pos = buffer.IndexOf((byte)0);
            if (pos == 0)
            {
                return string.Empty;
            }
            else if (pos > 0)
            {
                buffer = buffer.Slice(0, pos);
            }
            return EncodingASCIIGetString(buffer);
        }

        private static TiffValueCollection<string> ParseASCIIArray(ReadOnlySpan<byte> buffer)
        {
            int startIndex = 0;
            int count = buffer.Length;

            if (count == 0)
            {
                return TiffValueCollection.Empty<string>();
            }

            // Find the null terminator of the first string.
            int endIndex = startIndex + count - 1;
            int nullIndex = endIndex + 1;
            for (int i = startIndex; i <= endIndex; i++)
            {
                if (buffer[i] == 0)
                {
                    nullIndex = i;
                    break;
                }
            }

            string firstValue = EncodingASCIIGetString(buffer.Slice(startIndex, nullIndex - startIndex));

            if (nullIndex >= endIndex)
            {
                return TiffValueCollection.Single(firstValue);
            }

            // Find the count of all strings.
            startIndex = nullIndex + 1;
            int strCount = 1;
            for (int i = startIndex; i <= endIndex; i++)
            {
                if (buffer[i] == 0)
                {
                    strCount++;
                }
            }
            if (buffer[endIndex] != 0)
            {
                strCount++;
            }

            // Read all strings.
            string[] values = new string[strCount];
            values[0] = firstValue;

            int index = 1;
            for (int i = startIndex; i <= endIndex; i++)
            {
                if (buffer[i] == 0)
                {
                    values[index] = EncodingASCIIGetString(buffer.Slice(startIndex, i - startIndex));
                    index++;
                    startIndex = i + 1;
                }
            }
            if (buffer[endIndex] != 0)
            {
                values[index] = EncodingASCIIGetString(buffer.Slice(startIndex, endIndex - startIndex + 1));
            }

            return TiffValueCollection.UnsafeWrap(values);
        }

        private static unsafe string EncodingASCIIGetString(ReadOnlySpan<byte> data)
        {
#if NO_FAST_SPAN
            fixed (byte* pData = data)
            {
                return Encoding.ASCII.GetString(pData, data.Length);
            }
#else
            return Encoding.ASCII.GetString(data);
#endif
        }


        #endregion
    }
}
