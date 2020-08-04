﻿using System;
using System.Threading.Tasks;

namespace TiffLibrary.ImageEncoder
{
    /// <summary>
    /// A middleware that crops the input image into tiles.
    /// </summary>
    /// <typeparam name="TPixel">The pixel type.</typeparam>
    public sealed class TiffTiledImageEncoderEnumeratorMiddleware<TPixel> : ITiffImageEncoderMiddleware<TPixel> where TPixel : unmanaged
    {
        private readonly TiffSize _tileSize;

        /// <summary>
        /// Initialize the middleware with the specified tile size.
        /// </summary>
        /// <param name="tileSize">The size of each tile.</param>
        public TiffTiledImageEncoderEnumeratorMiddleware(TiffSize tileSize)
        {
            if (tileSize.Width < 16 || tileSize.Height < 16)
            {
                throw new ArgumentOutOfRangeException(nameof(tileSize));
            }
            if ((tileSize.Width % 16 != 0) || (tileSize.Height % 16 != 0))
            {
                throw new ArgumentOutOfRangeException(nameof(tileSize));
            }
            _tileSize = tileSize;
        }

        /// <summary>
        /// Crops the input image into tiles and runs the next middleware for each tile.
        /// </summary>
        /// <param name="context">The encoder context.</param>
        /// <param name="next">The next middleware.</param>
        /// <returns>A <see cref="Task"/> that completes when the image has been encoded.</returns>
        public async ValueTask InvokeAsync(TiffImageEncoderContext<TPixel> context, ITiffImageEncoderPipelineNode<TPixel> next)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (next is null)
            {
                throw new ArgumentNullException(nameof(next));
            }

            var state = context.GetService(typeof(TiffParallelEncodingState)) as TiffParallelEncodingState;
            TiffCroppedImageEncoderContext<TPixel>? wrappedContext = null;

            int width = context.ImageSize.Width, height = context.ImageSize.Height;
            int tileWidth = _tileSize.Width, tileHeight = _tileSize.Height;

            int tileAcross = (width + tileWidth - 1) / tileWidth;
            int tileDown = (height + tileHeight - 1) / tileHeight;
            int tileCount = tileAcross * tileDown;

            ulong[] tileOffsets = new ulong[tileCount];
            ulong[] tileByteCounts = new ulong[tileCount];
            int index = 0;

            for (int row = 0; row < tileDown; row++)
            {
                int offsetY = row * tileHeight;
                int imageHeight = Math.Min(height - offsetY, tileHeight);

                for (int col = 0; col < tileAcross; col++)
                {
                    int offsetX = col * tileWidth;
                    int imageWidth = Math.Min(width - offsetX, tileWidth);

                    wrappedContext ??= new TiffCroppedImageEncoderContext<TPixel>(context);

                    wrappedContext.ExposeIfdWriter = row == 0 && col == 0;
                    wrappedContext.OutputRegion = default;
                    wrappedContext.Crop(new TiffPoint(offsetX, offsetY), new TiffSize(imageWidth, imageHeight));

                    if (state is null)
                    {
                        await next.RunAsync(wrappedContext).ConfigureAwait(false);
                        tileOffsets[index] = (ulong)(long)wrappedContext.OutputRegion.Offset;
                        tileByteCounts[index] = (ulong)wrappedContext.OutputRegion.Length;
                    }
                    else
                    {
                        TiffCroppedImageEncoderContext<TPixel>? wContext = wrappedContext;
                        wrappedContext = null;
                        int currentIndex = index;
                        await state.DispatchAsync(async () =>
                        {
                            await next.RunAsync(wContext).ConfigureAwait(false);
                            tileOffsets[currentIndex] = (ulong)(long)wContext.OutputRegion.Offset;
                            tileByteCounts[currentIndex] = (ulong)wContext.OutputRegion.Length;
                        }, context.CancellationToken).ConfigureAwait(false);
                    }

                    index++;
                }
            }

            // Wait until all tiles are written.
            if (!(state is null))
            {
                await state.Complete.Task.ConfigureAwait(false);
            }

            TiffImageFileDirectoryWriter? ifdWriter = context.IfdWriter;
            if (!(ifdWriter is null))
            {
                using (await context.LockAsync().ConfigureAwait(false))
                {
                    await ifdWriter.WriteTagAsync(TiffTag.ImageWidth, TiffValueCollection.Single((uint)width)).ConfigureAwait(false);
                    await ifdWriter.WriteTagAsync(TiffTag.ImageLength, TiffValueCollection.Single((uint)height)).ConfigureAwait(false);
                    await ifdWriter.WriteTagAsync(TiffTag.TileWidth, TiffValueCollection.Single((ushort)tileWidth)).ConfigureAwait(false);
                    await ifdWriter.WriteTagAsync(TiffTag.TileLength, TiffValueCollection.Single((ushort)tileHeight)).ConfigureAwait(false);

                    if (context.FileWriter?.UseBigTiff ?? false)
                    {
                        await ifdWriter.WriteTagAsync(TiffTag.TileOffsets, TiffValueCollection.UnsafeWrap(tileOffsets)).ConfigureAwait(false);
                        await ifdWriter.WriteTagAsync(TiffTag.TileByteCounts, TiffValueCollection.UnsafeWrap(tileByteCounts)).ConfigureAwait(false);
                    }
                    else
                    {
                        uint[] tileOffsets32 = new uint[tileCount];
                        uint[] tileByteCounts32 = new uint[tileCount];

                        for (int i = 0; i < tileCount; i++)
                        {
                            tileOffsets32[i] = (uint)tileOffsets[i];
                            tileByteCounts32[i] = (uint)tileByteCounts[i];
                        }

                        await ifdWriter.WriteTagAsync(TiffTag.TileOffsets, TiffValueCollection.UnsafeWrap(tileOffsets32)).ConfigureAwait(false);
                        await ifdWriter.WriteTagAsync(TiffTag.TileByteCounts, TiffValueCollection.UnsafeWrap(tileByteCounts32)).ConfigureAwait(false);
                    }
                }
            }
        }
    }
}
