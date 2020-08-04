using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace TiffLibrary
{
    /// <summary>
    /// A writer class that write IFD entries into TIFF stream.
    /// </summary>
    public sealed partial class TiffImageFileDirectoryWriter : IDisposable
    {
        private TiffFileWriter? _writer;
        private List<TiffImageFileDirectoryEntry> _entries;

        /// <summary>
        /// Gets the TIFF file writer.
        /// </summary>
        public TiffFileWriter FileWriter => _writer ?? ThrowObjectDisposedException<TiffFileWriter>();

        internal TiffImageFileDirectoryWriter(TiffFileWriter writer)
        {
            _writer = writer;
            _entries = new List<TiffImageFileDirectoryEntry>();
        }

        /// <summary>
        /// Writes the IFD into the TIFF stream.
        /// </summary>
        /// <returns>The offset of the IFD in the stream.</returns>
        public async Task<TiffStreamOffset> FlushAsync()
        {
            EnsureNotDisposed();

            Debug.Assert(_writer != null);
            await _writer!.AlignToWordBoundaryAsync().ConfigureAwait(false);
            TiffStreamOffset position = _writer.Position;

            await WriteEntries().ConfigureAwait(false);

            return position;
        }

        /// <summary>
        /// Writes the IFD into the TIFF stream. Update the specified IFD to point its "Next IFD Offset" field to the IFD just written.
        /// </summary>
        /// <param name="previousIfdOffset">The specified IFD to update.</param>
        /// <returns>The offset of the IFD in the stream.</returns>
        public async Task<TiffStreamOffset> FlushAsync(TiffStreamOffset previousIfdOffset)
        {
            EnsureNotDisposed();

            Debug.Assert(_writer != null);
            await _writer!.AlignToWordBoundaryAsync().ConfigureAwait(false);
            TiffStreamOffset position = _writer.Position;

            await WriteEntries().ConfigureAwait(false);

            if (previousIfdOffset.IsZero)
            {
                _writer.SetFirstImageFileDirectoryOffset(position);
            }
            else
            {
                await _writer.UpdateImageFileDirectoryNextOffsetFieldAsync(previousIfdOffset, position).ConfigureAwait(false);
            }

            return position;
        }

        private async Task WriteEntries()
        {
            Debug.Assert(_writer != null);
            _entries.Sort(TiffImageFileDirectoryEntryComparer.Instance);

            byte[] buffer = ArrayPool<byte>.Shared.Rent(32);
            try
            {
                TiffFileContentReaderWriter writer = _writer!.InnerWriter;
                long position = _writer.Position;

                if (_writer.UseBigTiff)
                {
                    Unsafe.WriteUnaligned(ref buffer[0], (long)(uint)_entries.Count);
                }
                else
                {
                    Unsafe.WriteUnaligned(ref buffer[0], checked((ushort)_entries.Count));
                }
                await writer.WriteAsync(position, new ArraySegment<byte>(buffer, 0, _writer.OperationContext.ByteCountOfImageFileDirectoryCountField), CancellationToken.None).ConfigureAwait(false);
                position = _writer.AdvancePosition(_writer.OperationContext.ByteCountOfImageFileDirectoryCountField);

                foreach (TiffImageFileDirectoryEntry entry in _entries)
                {
                    int bytesWritten = entry.Write(_writer.OperationContext, buffer);
                    await writer.WriteAsync(position, new ArraySegment<byte>(buffer, 0, bytesWritten), CancellationToken.None).ConfigureAwait(false);
                    position = _writer.AdvancePosition(bytesWritten);
                }

                Unsafe.WriteUnaligned(ref buffer[0], (long)0);

                await writer.WriteAsync(position, new ArraySegment<byte>(buffer, 0, _writer.OperationContext.ByteCountOfValueOffsetField), CancellationToken.None).ConfigureAwait(false);
                _writer.AdvancePosition(_writer.OperationContext.ByteCountOfValueOffsetField);
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }


        #region Disposiable support

        private void EnsureNotDisposed()
        {
            if (_writer is null)
            {
                ThrowObjectDisposedException();
            }
        }

        [DoesNotReturn]
        private static void ThrowObjectDisposedException()
        {
            throw new ObjectDisposedException(nameof(TiffImageFileDirectoryWriter));
        }

        [DoesNotReturn]
        private static T ThrowObjectDisposedException<T>()
        {
            throw new ObjectDisposedException(nameof(TiffImageFileDirectoryWriter));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _writer = null;
        }

        #endregion
    }
}
