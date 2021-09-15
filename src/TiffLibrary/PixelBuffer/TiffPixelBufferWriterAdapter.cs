﻿using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace TiffLibrary.PixelBuffer
{
    /// <summary>
    /// Uses <see cref="ITiffPixelBuffer{TPixel}"/> as the underlying storage. Provides <see cref="ITiffPixelBufferWriter{TPixel}"/> API to write pixels into <see cref="ITiffPixelBuffer{TPixel}"/>.
    /// </summary>
    /// <typeparam name="TPixel"></typeparam>
    public sealed class TiffPixelBufferWriterAdapter<TPixel> : ITiffPixelBufferWriter<TPixel> where TPixel : unmanaged
    {
        private readonly ITiffPixelBuffer<TPixel> _buffer;
        private readonly TiffSize _size;

        private RowSpanHandle? _cachedRowHandle;
        private ColumnSpanHandle? _cachedColHandle;

        /// <summary>
        /// Initialize the object to wrap <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">The pixel buffer to wrap.</param>
        public TiffPixelBufferWriterAdapter(ITiffPixelBuffer<TPixel> buffer)
        {
            _buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
            _size = new TiffSize(buffer.Width, buffer.Height);
        }

        /// <inheritdoc />
        public int Width => _size.Width;

        /// <inheritdoc />
        public int Height => _size.Height;

        /// <inheritdoc />
        public TiffPixelSpanHandle<TPixel> GetRowSpan(int rowIndex, int start, int length)
        {
            int width = _size.Width;
            int height = _size.Height;
            if ((uint)rowIndex >= (uint)height)
            {
                throw new ArgumentOutOfRangeException(nameof(rowIndex));
            }
            if ((uint)start > (uint)width)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }
            if (length < 0 || (uint)(start + length) > (uint)width)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            RowSpanHandle? handle = Interlocked.Exchange(ref _cachedRowHandle, null);
            if (handle is null)
            {
                handle = new RowSpanHandle();
            }
            handle.SetHandle(this, _buffer, rowIndex * _size.Width + start, length);
            return handle;
        }

        /// <inheritdoc />
        public TiffPixelSpanHandle<TPixel> GetColumnSpan(int colIndex, int start, int length)
        {
            int width = _size.Width;
            int height = _size.Height;
            if ((uint)colIndex >= (uint)width)
            {
                throw new ArgumentOutOfRangeException(nameof(colIndex));
            }
            if ((uint)start > (uint)height)
            {
                throw new ArgumentOutOfRangeException(nameof(start));
            }
            if (length < 0 || (uint)(start + length) > (uint)height)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            ColumnSpanHandle? handle = Interlocked.Exchange(ref _cachedColHandle, null);
            if (handle is null)
            {
                handle = new ColumnSpanHandle();
            }
            handle.SetHandle(this, _buffer, colIndex, start, length);
            return handle;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_cachedColHandle is not null)
            {
                _cachedColHandle.ReleaseBuffer();
            }
        }

        #region RowSpanHandle

        private class RowSpanHandle : TiffPixelSpanHandle<TPixel>
        {
            private TiffPixelBufferWriterAdapter<TPixel>? _parent;
            private ITiffPixelBuffer<TPixel>? _pixelBuffer;
            private int _start;
            private int _length;

            internal void SetHandle(TiffPixelBufferWriterAdapter<TPixel> parent, ITiffPixelBuffer<TPixel> buffer, int start, int length)
            {
                _parent = parent;
                _pixelBuffer = buffer;
                _start = start;
                _length = length;
            }

            public override Span<TPixel> GetSpan()
            {
                if (_pixelBuffer is null)
                {
                    throw new ObjectDisposedException(GetType().FullName);
                }
                return _pixelBuffer.GetSpan().Slice(_start, _length);
            }

            public override void Dispose()
            {
                _pixelBuffer = null;
                if (_parent != null)
                {
                    TiffPixelBufferWriterAdapter<TPixel> parent = _parent;
                    _parent = null;
                    Interlocked.CompareExchange(ref parent._cachedRowHandle, this, null);
                }
            }
        }

        #endregion

        #region ColumnSpanHandle

        private class ColumnSpanHandle : TiffPixelSpanHandle<TPixel>
        {
            private TiffPixelBufferWriterAdapter<TPixel>? _parent;
            private ITiffPixelBuffer<TPixel>? _pixelBuffer;
            private byte[]? _buffer;
            private int _colIndex;
            private int _start;
            private int _length;

            internal void SetHandle(TiffPixelBufferWriterAdapter<TPixel> parent, ITiffPixelBuffer<TPixel> buffer, int colIndex, int start, int length)
            {
                _parent = parent;
                _pixelBuffer = buffer;
                _colIndex = colIndex;
                _start = start;
                _length = length;

                EnsureBufferSize(_length * Unsafe.SizeOf<TPixel>());
            }

            internal void EnsureBufferSize(int size)
            {
                if (_buffer is null)
                {
                    _buffer = ArrayPool<byte>.Shared.Rent(size);
                    return;
                }
                if (_buffer.Length < size)
                {
                    ArrayPool<byte>.Shared.Return(_buffer);
                    _buffer = ArrayPool<byte>.Shared.Rent(size);
                }
            }

            internal void ReleaseBuffer()
            {
                if (_buffer is not null)
                {
                    ArrayPool<byte>.Shared.Return(_buffer);
                    _buffer = null;
                }
            }

            public override Span<TPixel> GetSpan()
            {
                if (_pixelBuffer is null)
                {
                    throw new ObjectDisposedException(GetType().FullName);
                }
                return MemoryMarshal.Cast<byte, TPixel>(_buffer.AsSpan(0, _length * Unsafe.SizeOf<TPixel>()));
            }

            public override void Dispose()
            {
                if (_pixelBuffer is null)
                {
                    return;
                }

                // Copy pixels into this column
                int colIndex = _colIndex;
                int width = _pixelBuffer.Width;
                Span<TPixel> sourceSpan = MemoryMarshal.Cast<byte, TPixel>(_buffer.AsSpan(0, _length * Unsafe.SizeOf<TPixel>()));
                Span<TPixel> destinationSpan = _pixelBuffer.GetSpan().Slice(_start * width);
                for (int i = 0; i < sourceSpan.Length; i++)
                {
                    destinationSpan[colIndex + i * width] = sourceSpan[i];
                }

                _pixelBuffer = null;
                if (_parent != null)
                {
                    TiffPixelBufferWriterAdapter<TPixel> parent = _parent;
                    _parent = null;
                    if (Interlocked.CompareExchange(ref parent._cachedColHandle, this, null) != null)
                    {
                        ReleaseBuffer();
                    }
                }
            }


        }

        #endregion
    }
}
