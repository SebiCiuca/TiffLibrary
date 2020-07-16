﻿using System.Threading;
using System.Threading.Tasks;

namespace TiffLibrary
{
    public static partial class TiffTagReaderExtensions
    {
    
        #region Artist

        /// <summary>
        /// Read the values of <see cref="TiffTag.Artist"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<string?> ReadArtistAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<string?> valueTask = tagReader.ReadASCIIFieldFirstStringAsync(TiffTag.Artist, sizeLimit: -1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                return new ValueTask<string?>(valueTask.GetAwaiter().GetResult());
            }

            return new ValueTask<string?>(TransformValueTaskAsync(valueTask));

            static async Task<string?> TransformValueTaskAsync(ValueTask<string?> valueTask)
            {
                return await valueTask.ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.Artist"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static string? ReadArtist(this TiffTagReader tagReader)
        {
            return tagReader.ReadASCIIFieldFirstString(TiffTag.Artist, sizeLimit: -1);
        }

        #endregion
    
        #region BitsPerSample

        /// <summary>
        /// Read the values of <see cref="TiffTag.BitsPerSample"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<TiffValueCollection<ushort>> ReadBitsPerSampleAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ushort>> valueTask = tagReader.ReadShortFieldAsync(TiffTag.BitsPerSample, sizeLimit: -1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ushort> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<TiffValueCollection<ushort>>(result.IsEmpty ? TiffValueCollection.Single<ushort>(1) : result);
            }

            return new ValueTask<TiffValueCollection<ushort>>(TransformValueTaskAsync(valueTask));

            static async Task<TiffValueCollection<ushort>> TransformValueTaskAsync(ValueTask<TiffValueCollection<ushort>> valueTask)
            {
                TiffValueCollection<ushort> result = await valueTask.ConfigureAwait(false);
                return result.IsEmpty ? TiffValueCollection.Single<ushort>(1) : result;
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.BitsPerSample"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static TiffValueCollection<ushort> ReadBitsPerSample(this TiffTagReader tagReader)
        {
            TiffValueCollection<ushort> result = tagReader.ReadShortField(TiffTag.BitsPerSample, sizeLimit: -1);
            return result.IsEmpty ? TiffValueCollection.Single<ushort>(1) : result;
        }

        #endregion
    
        #region CellLength

        /// <summary>
        /// Read the values of <see cref="TiffTag.CellLength"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<ushort?> ReadCellLengthAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ushort>> valueTask = tagReader.ReadShortFieldAsync(TiffTag.CellLength, sizeLimit: 1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ushort> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<ushort?>(result.IsEmpty ? default(ushort?) : result.GetFirstOrDefault());
            }

            return new ValueTask<ushort?>(TransformValueTaskAsync(valueTask));

            static async Task<ushort?> TransformValueTaskAsync(ValueTask<TiffValueCollection<ushort>> valueTask)
            {
                TiffValueCollection<ushort> result = await valueTask.ConfigureAwait(false);
                return result.IsEmpty ? default(ushort?) : result.GetFirstOrDefault();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.CellLength"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static ushort? ReadCellLength(this TiffTagReader tagReader)
        {
            TiffValueCollection<ushort> result = tagReader.ReadShortField(TiffTag.CellLength, sizeLimit: 1);
            return result.IsEmpty ? default(ushort?) : result.GetFirstOrDefault();
        }

        #endregion
    
        #region CellWidth

        /// <summary>
        /// Read the values of <see cref="TiffTag.CellWidth"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<ushort?> ReadCellWidthAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ushort>> valueTask = tagReader.ReadShortFieldAsync(TiffTag.CellWidth, sizeLimit: 1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ushort> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<ushort?>(result.IsEmpty ? default(ushort?) : result.GetFirstOrDefault());
            }

            return new ValueTask<ushort?>(TransformValueTaskAsync(valueTask));

            static async Task<ushort?> TransformValueTaskAsync(ValueTask<TiffValueCollection<ushort>> valueTask)
            {
                TiffValueCollection<ushort> result = await valueTask.ConfigureAwait(false);
                return result.IsEmpty ? default(ushort?) : result.GetFirstOrDefault();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.CellWidth"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static ushort? ReadCellWidth(this TiffTagReader tagReader)
        {
            TiffValueCollection<ushort> result = tagReader.ReadShortField(TiffTag.CellWidth, sizeLimit: 1);
            return result.IsEmpty ? default(ushort?) : result.GetFirstOrDefault();
        }

        #endregion
    
        #region ColorMap

        /// <summary>
        /// Read the values of <see cref="TiffTag.ColorMap"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<ushort[]> ReadColorMapAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ushort>> valueTask = tagReader.ReadShortFieldAsync(TiffTag.ColorMap, sizeLimit: -1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ushort> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<ushort[]>(result.GetOrCreateArray());
            }

            return new ValueTask<ushort[]>(TransformValueTaskAsync(valueTask));

            static async Task<ushort[]> TransformValueTaskAsync(ValueTask<TiffValueCollection<ushort>> valueTask)
            {
                TiffValueCollection<ushort> result = await valueTask.ConfigureAwait(false);
                return result.GetOrCreateArray();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.ColorMap"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static ushort[] ReadColorMap(this TiffTagReader tagReader)
        {
            TiffValueCollection<ushort> result = tagReader.ReadShortField(TiffTag.ColorMap, sizeLimit: -1);
            return result.GetOrCreateArray();
        }

        #endregion
    
        #region Compression

        /// <summary>
        /// Read the values of <see cref="TiffTag.Compression"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<TiffCompression> ReadCompressionAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ushort>> valueTask = tagReader.ReadShortFieldAsync(TiffTag.Compression, sizeLimit: 1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ushort> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<TiffCompression>(result.IsEmpty ? TiffCompression.Unspecified : (TiffCompression)result.GetFirstOrDefault());
            }

            return new ValueTask<TiffCompression>(TransformValueTaskAsync(valueTask));

            static async Task<TiffCompression> TransformValueTaskAsync(ValueTask<TiffValueCollection<ushort>> valueTask)
            {
                TiffValueCollection<ushort> result = await valueTask.ConfigureAwait(false);
                return result.IsEmpty ? TiffCompression.Unspecified : (TiffCompression)result.GetFirstOrDefault();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.Compression"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static TiffCompression ReadCompression(this TiffTagReader tagReader)
        {
            TiffValueCollection<ushort> result = tagReader.ReadShortField(TiffTag.Compression, sizeLimit: 1);
            return result.IsEmpty ? TiffCompression.Unspecified : (TiffCompression)result.GetFirstOrDefault();
        }

        #endregion
    
        #region Copyright

        /// <summary>
        /// Read the values of <see cref="TiffTag.Copyright"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<string?> ReadCopyrightAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<string?> valueTask = tagReader.ReadASCIIFieldFirstStringAsync(TiffTag.Copyright, sizeLimit: -1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                return new ValueTask<string?>(valueTask.GetAwaiter().GetResult());
            }

            return new ValueTask<string?>(TransformValueTaskAsync(valueTask));

            static async Task<string?> TransformValueTaskAsync(ValueTask<string?> valueTask)
            {
                return await valueTask.ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.Copyright"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static string? ReadCopyright(this TiffTagReader tagReader)
        {
            return tagReader.ReadASCIIFieldFirstString(TiffTag.Copyright, sizeLimit: -1);
        }

        #endregion
    
        #region DateTime

        /// <summary>
        /// Read the values of <see cref="TiffTag.DateTime"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<string?> ReadDateTimeAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<string?> valueTask = tagReader.ReadASCIIFieldFirstStringAsync(TiffTag.DateTime, sizeLimit: -1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                return new ValueTask<string?>(valueTask.GetAwaiter().GetResult());
            }

            return new ValueTask<string?>(TransformValueTaskAsync(valueTask));

            static async Task<string?> TransformValueTaskAsync(ValueTask<string?> valueTask)
            {
                return await valueTask.ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.DateTime"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static string? ReadDateTime(this TiffTagReader tagReader)
        {
            return tagReader.ReadASCIIFieldFirstString(TiffTag.DateTime, sizeLimit: -1);
        }

        #endregion
    
        #region ExtraSamples

        /// <summary>
        /// Read the values of <see cref="TiffTag.ExtraSamples"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<TiffValueCollection<TiffExtraSample>> ReadExtraSamplesAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ushort>> valueTask = tagReader.ReadShortFieldAsync(TiffTag.ExtraSamples, sizeLimit: -1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ushort> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<TiffValueCollection<TiffExtraSample>>(result.ConvertAll(i => (TiffExtraSample)i));
            }

            return new ValueTask<TiffValueCollection<TiffExtraSample>>(TransformValueTaskAsync(valueTask));

            static async Task<TiffValueCollection<TiffExtraSample>> TransformValueTaskAsync(ValueTask<TiffValueCollection<ushort>> valueTask)
            {
                TiffValueCollection<ushort> result = await valueTask.ConfigureAwait(false);
                return result.ConvertAll(i => (TiffExtraSample)i);
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.ExtraSamples"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static TiffValueCollection<TiffExtraSample> ReadExtraSamples(this TiffTagReader tagReader)
        {
            TiffValueCollection<ushort> result = tagReader.ReadShortField(TiffTag.ExtraSamples, sizeLimit: -1);
            return result.ConvertAll(i => (TiffExtraSample)i);
        }

        #endregion
    
        #region FillOrder

        /// <summary>
        /// Read the values of <see cref="TiffTag.FillOrder"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<TiffFillOrder> ReadFillOrderAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ushort>> valueTask = tagReader.ReadShortFieldAsync(TiffTag.FillOrder, sizeLimit: 1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ushort> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<TiffFillOrder>(result.IsEmpty ? TiffFillOrder.HigherOrderBitsFirst : (TiffFillOrder)result.GetFirstOrDefault());
            }

            return new ValueTask<TiffFillOrder>(TransformValueTaskAsync(valueTask));

            static async Task<TiffFillOrder> TransformValueTaskAsync(ValueTask<TiffValueCollection<ushort>> valueTask)
            {
                TiffValueCollection<ushort> result = await valueTask.ConfigureAwait(false);
                return result.IsEmpty ? TiffFillOrder.HigherOrderBitsFirst : (TiffFillOrder)result.GetFirstOrDefault();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.FillOrder"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static TiffFillOrder ReadFillOrder(this TiffTagReader tagReader)
        {
            TiffValueCollection<ushort> result = tagReader.ReadShortField(TiffTag.FillOrder, sizeLimit: 1);
            return result.IsEmpty ? TiffFillOrder.HigherOrderBitsFirst : (TiffFillOrder)result.GetFirstOrDefault();
        }

        #endregion
    
        #region FreeByteCounts

        /// <summary>
        /// Read the values of <see cref="TiffTag.FreeByteCounts"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<uint?> ReadFreeByteCountsAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<uint>> valueTask = tagReader.ReadLongFieldAsync(TiffTag.FreeByteCounts, sizeLimit: 1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<uint> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<uint?>(result.IsEmpty ? default(uint?) : result.GetFirstOrDefault());
            }

            return new ValueTask<uint?>(TransformValueTaskAsync(valueTask));

            static async Task<uint?> TransformValueTaskAsync(ValueTask<TiffValueCollection<uint>> valueTask)
            {
                TiffValueCollection<uint> result = await valueTask.ConfigureAwait(false);
                return result.IsEmpty ? default(uint?) : result.GetFirstOrDefault();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.FreeByteCounts"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static uint? ReadFreeByteCounts(this TiffTagReader tagReader)
        {
            TiffValueCollection<uint> result = tagReader.ReadLongField(TiffTag.FreeByteCounts, sizeLimit: 1);
            return result.IsEmpty ? default(uint?) : result.GetFirstOrDefault();
        }

        #endregion
    
        #region FreeOffsets

        /// <summary>
        /// Read the values of <see cref="TiffTag.FreeOffsets"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<uint?> ReadFreeOffsetsAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<uint>> valueTask = tagReader.ReadLongFieldAsync(TiffTag.FreeOffsets, sizeLimit: 1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<uint> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<uint?>(result.IsEmpty ? default(uint?) : result.GetFirstOrDefault());
            }

            return new ValueTask<uint?>(TransformValueTaskAsync(valueTask));

            static async Task<uint?> TransformValueTaskAsync(ValueTask<TiffValueCollection<uint>> valueTask)
            {
                TiffValueCollection<uint> result = await valueTask.ConfigureAwait(false);
                return result.IsEmpty ? default(uint?) : result.GetFirstOrDefault();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.FreeOffsets"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static uint? ReadFreeOffsets(this TiffTagReader tagReader)
        {
            TiffValueCollection<uint> result = tagReader.ReadLongField(TiffTag.FreeOffsets, sizeLimit: 1);
            return result.IsEmpty ? default(uint?) : result.GetFirstOrDefault();
        }

        #endregion
    
        #region GrayResponseCurve

        /// <summary>
        /// Read the values of <see cref="TiffTag.GrayResponseCurve"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<TiffValueCollection<ushort>> ReadGrayResponseCurveAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ushort>> valueTask = tagReader.ReadShortFieldAsync(TiffTag.GrayResponseCurve, sizeLimit: -1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ushort> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<TiffValueCollection<ushort>>(result);
            }

            return new ValueTask<TiffValueCollection<ushort>>(TransformValueTaskAsync(valueTask));

            static async Task<TiffValueCollection<ushort>> TransformValueTaskAsync(ValueTask<TiffValueCollection<ushort>> valueTask)
            {
                TiffValueCollection<ushort> result = await valueTask.ConfigureAwait(false);
                return result;
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.GrayResponseCurve"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static TiffValueCollection<ushort> ReadGrayResponseCurve(this TiffTagReader tagReader)
        {
            TiffValueCollection<ushort> result = tagReader.ReadShortField(TiffTag.GrayResponseCurve, sizeLimit: -1);
            return result;
        }

        #endregion
    
        #region GrayResponseUnit

        /// <summary>
        /// Read the values of <see cref="TiffTag.GrayResponseUnit"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<TiffGrayResponseUnit> ReadGrayResponseUnitAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ushort>> valueTask = tagReader.ReadShortFieldAsync(TiffTag.GrayResponseUnit, sizeLimit: 1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ushort> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<TiffGrayResponseUnit>(result.IsEmpty ? TiffGrayResponseUnit.Hundredths : (TiffGrayResponseUnit)result.GetFirstOrDefault());
            }

            return new ValueTask<TiffGrayResponseUnit>(TransformValueTaskAsync(valueTask));

            static async Task<TiffGrayResponseUnit> TransformValueTaskAsync(ValueTask<TiffValueCollection<ushort>> valueTask)
            {
                TiffValueCollection<ushort> result = await valueTask.ConfigureAwait(false);
                return result.IsEmpty ? TiffGrayResponseUnit.Hundredths : (TiffGrayResponseUnit)result.GetFirstOrDefault();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.GrayResponseUnit"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static TiffGrayResponseUnit ReadGrayResponseUnit(this TiffTagReader tagReader)
        {
            TiffValueCollection<ushort> result = tagReader.ReadShortField(TiffTag.GrayResponseUnit, sizeLimit: 1);
            return result.IsEmpty ? TiffGrayResponseUnit.Hundredths : (TiffGrayResponseUnit)result.GetFirstOrDefault();
        }

        #endregion
    
        #region HostComputer

        /// <summary>
        /// Read the values of <see cref="TiffTag.HostComputer"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<string?> ReadHostComputerAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<string?> valueTask = tagReader.ReadASCIIFieldFirstStringAsync(TiffTag.HostComputer, sizeLimit: -1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                return new ValueTask<string?>(valueTask.GetAwaiter().GetResult());
            }

            return new ValueTask<string?>(TransformValueTaskAsync(valueTask));

            static async Task<string?> TransformValueTaskAsync(ValueTask<string?> valueTask)
            {
                return await valueTask.ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.HostComputer"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static string? ReadHostComputer(this TiffTagReader tagReader)
        {
            return tagReader.ReadASCIIFieldFirstString(TiffTag.HostComputer, sizeLimit: -1);
        }

        #endregion
    
        #region ImageDescription

        /// <summary>
        /// Read the values of <see cref="TiffTag.ImageDescription"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<string?> ReadImageDescriptionAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<string?> valueTask = tagReader.ReadASCIIFieldFirstStringAsync(TiffTag.ImageDescription, sizeLimit: -1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                return new ValueTask<string?>(valueTask.GetAwaiter().GetResult());
            }

            return new ValueTask<string?>(TransformValueTaskAsync(valueTask));

            static async Task<string?> TransformValueTaskAsync(ValueTask<string?> valueTask)
            {
                return await valueTask.ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.ImageDescription"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static string? ReadImageDescription(this TiffTagReader tagReader)
        {
            return tagReader.ReadASCIIFieldFirstString(TiffTag.ImageDescription, sizeLimit: -1);
        }

        #endregion
    
        #region ImageLength

        /// <summary>
        /// Read the values of <see cref="TiffTag.ImageLength"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<ulong> ReadImageLengthAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ulong>> valueTask = tagReader.ReadLong8FieldAsync(TiffTag.ImageLength, sizeLimit: -1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ulong> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<ulong>(result.GetFirstOrDefault());
            }

            return new ValueTask<ulong>(TransformValueTaskAsync(valueTask));

            static async Task<ulong> TransformValueTaskAsync(ValueTask<TiffValueCollection<ulong>> valueTask)
            {
                TiffValueCollection<ulong> result = await valueTask.ConfigureAwait(false);
                return result.GetFirstOrDefault();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.ImageLength"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static ulong ReadImageLength(this TiffTagReader tagReader)
        {
            TiffValueCollection<ulong> result = tagReader.ReadLong8Field(TiffTag.ImageLength, sizeLimit: -1);
            return result.GetFirstOrDefault();
        }

        #endregion
    
        #region ImageWidth

        /// <summary>
        /// Read the values of <see cref="TiffTag.ImageWidth"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<ulong> ReadImageWidthAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ulong>> valueTask = tagReader.ReadLong8FieldAsync(TiffTag.ImageWidth, sizeLimit: -1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ulong> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<ulong>(result.GetFirstOrDefault());
            }

            return new ValueTask<ulong>(TransformValueTaskAsync(valueTask));

            static async Task<ulong> TransformValueTaskAsync(ValueTask<TiffValueCollection<ulong>> valueTask)
            {
                TiffValueCollection<ulong> result = await valueTask.ConfigureAwait(false);
                return result.GetFirstOrDefault();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.ImageWidth"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static ulong ReadImageWidth(this TiffTagReader tagReader)
        {
            TiffValueCollection<ulong> result = tagReader.ReadLong8Field(TiffTag.ImageWidth, sizeLimit: -1);
            return result.GetFirstOrDefault();
        }

        #endregion
    
        #region Make

        /// <summary>
        /// Read the values of <see cref="TiffTag.Make"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<string?> ReadMakeAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<string?> valueTask = tagReader.ReadASCIIFieldFirstStringAsync(TiffTag.Make, sizeLimit: -1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                return new ValueTask<string?>(valueTask.GetAwaiter().GetResult());
            }

            return new ValueTask<string?>(TransformValueTaskAsync(valueTask));

            static async Task<string?> TransformValueTaskAsync(ValueTask<string?> valueTask)
            {
                return await valueTask.ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.Make"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static string? ReadMake(this TiffTagReader tagReader)
        {
            return tagReader.ReadASCIIFieldFirstString(TiffTag.Make, sizeLimit: -1);
        }

        #endregion
    
        #region MaxSampleValue

        /// <summary>
        /// Read the values of <see cref="TiffTag.MaxSampleValue"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<TiffValueCollection<ushort>> ReadMaxSampleValueAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ushort>> valueTask = tagReader.ReadShortFieldAsync(TiffTag.MaxSampleValue, sizeLimit: -1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ushort> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<TiffValueCollection<ushort>>(result);
            }

            return new ValueTask<TiffValueCollection<ushort>>(TransformValueTaskAsync(valueTask));

            static async Task<TiffValueCollection<ushort>> TransformValueTaskAsync(ValueTask<TiffValueCollection<ushort>> valueTask)
            {
                TiffValueCollection<ushort> result = await valueTask.ConfigureAwait(false);
                return result;
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.MaxSampleValue"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static TiffValueCollection<ushort> ReadMaxSampleValue(this TiffTagReader tagReader)
        {
            TiffValueCollection<ushort> result = tagReader.ReadShortField(TiffTag.MaxSampleValue, sizeLimit: -1);
            return result;
        }

        #endregion
    
        #region MinSampleValue

        /// <summary>
        /// Read the values of <see cref="TiffTag.MinSampleValue"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<TiffValueCollection<ushort>> ReadMinSampleValueAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ushort>> valueTask = tagReader.ReadShortFieldAsync(TiffTag.MinSampleValue, sizeLimit: -1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ushort> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<TiffValueCollection<ushort>>(result);
            }

            return new ValueTask<TiffValueCollection<ushort>>(TransformValueTaskAsync(valueTask));

            static async Task<TiffValueCollection<ushort>> TransformValueTaskAsync(ValueTask<TiffValueCollection<ushort>> valueTask)
            {
                TiffValueCollection<ushort> result = await valueTask.ConfigureAwait(false);
                return result;
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.MinSampleValue"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static TiffValueCollection<ushort> ReadMinSampleValue(this TiffTagReader tagReader)
        {
            TiffValueCollection<ushort> result = tagReader.ReadShortField(TiffTag.MinSampleValue, sizeLimit: -1);
            return result;
        }

        #endregion
    
        #region Model

        /// <summary>
        /// Read the values of <see cref="TiffTag.Model"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<string?> ReadModelAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<string?> valueTask = tagReader.ReadASCIIFieldFirstStringAsync(TiffTag.Model, sizeLimit: -1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                return new ValueTask<string?>(valueTask.GetAwaiter().GetResult());
            }

            return new ValueTask<string?>(TransformValueTaskAsync(valueTask));

            static async Task<string?> TransformValueTaskAsync(ValueTask<string?> valueTask)
            {
                return await valueTask.ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.Model"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static string? ReadModel(this TiffTagReader tagReader)
        {
            return tagReader.ReadASCIIFieldFirstString(TiffTag.Model, sizeLimit: -1);
        }

        #endregion
    
        #region NewSubfileType

        /// <summary>
        /// Read the values of <see cref="TiffTag.NewSubfileType"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<TiffNewSubfileType> ReadNewSubfileTypeAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<uint>> valueTask = tagReader.ReadLongFieldAsync(TiffTag.NewSubfileType, sizeLimit: 1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<uint> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<TiffNewSubfileType>((TiffNewSubfileType)result.GetFirstOrDefault());
            }

            return new ValueTask<TiffNewSubfileType>(TransformValueTaskAsync(valueTask));

            static async Task<TiffNewSubfileType> TransformValueTaskAsync(ValueTask<TiffValueCollection<uint>> valueTask)
            {
                TiffValueCollection<uint> result = await valueTask.ConfigureAwait(false);
                return (TiffNewSubfileType)result.GetFirstOrDefault();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.NewSubfileType"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static TiffNewSubfileType ReadNewSubfileType(this TiffTagReader tagReader)
        {
            TiffValueCollection<uint> result = tagReader.ReadLongField(TiffTag.NewSubfileType, sizeLimit: 1);
            return (TiffNewSubfileType)result.GetFirstOrDefault();
        }

        #endregion
    
        #region Orientation

        /// <summary>
        /// Read the values of <see cref="TiffTag.Orientation"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<TiffOrientation> ReadOrientationAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ushort>> valueTask = tagReader.ReadShortFieldAsync(TiffTag.Orientation, sizeLimit: 1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ushort> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<TiffOrientation>(result.IsEmpty ? TiffOrientation.TopLeft : (TiffOrientation)result.GetFirstOrDefault());
            }

            return new ValueTask<TiffOrientation>(TransformValueTaskAsync(valueTask));

            static async Task<TiffOrientation> TransformValueTaskAsync(ValueTask<TiffValueCollection<ushort>> valueTask)
            {
                TiffValueCollection<ushort> result = await valueTask.ConfigureAwait(false);
                return result.IsEmpty ? TiffOrientation.TopLeft : (TiffOrientation)result.GetFirstOrDefault();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.Orientation"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static TiffOrientation ReadOrientation(this TiffTagReader tagReader)
        {
            TiffValueCollection<ushort> result = tagReader.ReadShortField(TiffTag.Orientation, sizeLimit: 1);
            return result.IsEmpty ? TiffOrientation.TopLeft : (TiffOrientation)result.GetFirstOrDefault();
        }

        #endregion
    
        #region PhotometricInterpretation

        /// <summary>
        /// Read the values of <see cref="TiffTag.PhotometricInterpretation"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<TiffPhotometricInterpretation?> ReadPhotometricInterpretationAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ushort>> valueTask = tagReader.ReadShortFieldAsync(TiffTag.PhotometricInterpretation, sizeLimit: 1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ushort> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<TiffPhotometricInterpretation?>(result.IsEmpty ? default(TiffPhotometricInterpretation?) : (TiffPhotometricInterpretation)result.GetFirstOrDefault());
            }

            return new ValueTask<TiffPhotometricInterpretation?>(TransformValueTaskAsync(valueTask));

            static async Task<TiffPhotometricInterpretation?> TransformValueTaskAsync(ValueTask<TiffValueCollection<ushort>> valueTask)
            {
                TiffValueCollection<ushort> result = await valueTask.ConfigureAwait(false);
                return result.IsEmpty ? default(TiffPhotometricInterpretation?) : (TiffPhotometricInterpretation)result.GetFirstOrDefault();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.PhotometricInterpretation"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static TiffPhotometricInterpretation? ReadPhotometricInterpretation(this TiffTagReader tagReader)
        {
            TiffValueCollection<ushort> result = tagReader.ReadShortField(TiffTag.PhotometricInterpretation, sizeLimit: 1);
            return result.IsEmpty ? default(TiffPhotometricInterpretation?) : (TiffPhotometricInterpretation)result.GetFirstOrDefault();
        }

        #endregion
    
        #region PlanarConfiguration

        /// <summary>
        /// Read the values of <see cref="TiffTag.PlanarConfiguration"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<TiffPlanarConfiguration> ReadPlanarConfigurationAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ushort>> valueTask = tagReader.ReadShortFieldAsync(TiffTag.PlanarConfiguration, sizeLimit: 1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ushort> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<TiffPlanarConfiguration>(result.IsEmpty ? TiffPlanarConfiguration.Chunky : (TiffPlanarConfiguration)result.GetFirstOrDefault());
            }

            return new ValueTask<TiffPlanarConfiguration>(TransformValueTaskAsync(valueTask));

            static async Task<TiffPlanarConfiguration> TransformValueTaskAsync(ValueTask<TiffValueCollection<ushort>> valueTask)
            {
                TiffValueCollection<ushort> result = await valueTask.ConfigureAwait(false);
                return result.IsEmpty ? TiffPlanarConfiguration.Chunky : (TiffPlanarConfiguration)result.GetFirstOrDefault();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.PlanarConfiguration"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static TiffPlanarConfiguration ReadPlanarConfiguration(this TiffTagReader tagReader)
        {
            TiffValueCollection<ushort> result = tagReader.ReadShortField(TiffTag.PlanarConfiguration, sizeLimit: 1);
            return result.IsEmpty ? TiffPlanarConfiguration.Chunky : (TiffPlanarConfiguration)result.GetFirstOrDefault();
        }

        #endregion
    
        #region ResolutionUnit

        /// <summary>
        /// Read the values of <see cref="TiffTag.ResolutionUnit"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<TiffResolutionUnit> ReadResolutionUnitAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ushort>> valueTask = tagReader.ReadShortFieldAsync(TiffTag.ResolutionUnit, sizeLimit: 1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ushort> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<TiffResolutionUnit>(result.IsEmpty ? TiffResolutionUnit.Inch : (TiffResolutionUnit)result.GetFirstOrDefault());
            }

            return new ValueTask<TiffResolutionUnit>(TransformValueTaskAsync(valueTask));

            static async Task<TiffResolutionUnit> TransformValueTaskAsync(ValueTask<TiffValueCollection<ushort>> valueTask)
            {
                TiffValueCollection<ushort> result = await valueTask.ConfigureAwait(false);
                return result.IsEmpty ? TiffResolutionUnit.Inch : (TiffResolutionUnit)result.GetFirstOrDefault();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.ResolutionUnit"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static TiffResolutionUnit ReadResolutionUnit(this TiffTagReader tagReader)
        {
            TiffValueCollection<ushort> result = tagReader.ReadShortField(TiffTag.ResolutionUnit, sizeLimit: 1);
            return result.IsEmpty ? TiffResolutionUnit.Inch : (TiffResolutionUnit)result.GetFirstOrDefault();
        }

        #endregion
    
        #region RowsPerStrip

        /// <summary>
        /// Read the values of <see cref="TiffTag.RowsPerStrip"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<uint> ReadRowsPerStripAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<uint>> valueTask = tagReader.ReadLongFieldAsync(TiffTag.RowsPerStrip, sizeLimit: 1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<uint> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<uint>(result.GetFirstOrDefault());
            }

            return new ValueTask<uint>(TransformValueTaskAsync(valueTask));

            static async Task<uint> TransformValueTaskAsync(ValueTask<TiffValueCollection<uint>> valueTask)
            {
                TiffValueCollection<uint> result = await valueTask.ConfigureAwait(false);
                return result.GetFirstOrDefault();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.RowsPerStrip"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static uint ReadRowsPerStrip(this TiffTagReader tagReader)
        {
            TiffValueCollection<uint> result = tagReader.ReadLongField(TiffTag.RowsPerStrip, sizeLimit: 1);
            return result.GetFirstOrDefault();
        }

        #endregion
    
        #region SamplesPerPixel

        /// <summary>
        /// Read the values of <see cref="TiffTag.SamplesPerPixel"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<ushort> ReadSamplesPerPixelAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ushort>> valueTask = tagReader.ReadShortFieldAsync(TiffTag.SamplesPerPixel, sizeLimit: 1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ushort> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<ushort>(result.IsEmpty ? (ushort)1 : result.GetFirstOrDefault());
            }

            return new ValueTask<ushort>(TransformValueTaskAsync(valueTask));

            static async Task<ushort> TransformValueTaskAsync(ValueTask<TiffValueCollection<ushort>> valueTask)
            {
                TiffValueCollection<ushort> result = await valueTask.ConfigureAwait(false);
                return result.IsEmpty ? (ushort)1 : result.GetFirstOrDefault();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.SamplesPerPixel"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static ushort ReadSamplesPerPixel(this TiffTagReader tagReader)
        {
            TiffValueCollection<ushort> result = tagReader.ReadShortField(TiffTag.SamplesPerPixel, sizeLimit: 1);
            return result.IsEmpty ? (ushort)1 : result.GetFirstOrDefault();
        }

        #endregion
    
        #region Software

        /// <summary>
        /// Read the values of <see cref="TiffTag.Software"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<string?> ReadSoftwareAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<string?> valueTask = tagReader.ReadASCIIFieldFirstStringAsync(TiffTag.Software, sizeLimit: -1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                return new ValueTask<string?>(valueTask.GetAwaiter().GetResult());
            }

            return new ValueTask<string?>(TransformValueTaskAsync(valueTask));

            static async Task<string?> TransformValueTaskAsync(ValueTask<string?> valueTask)
            {
                return await valueTask.ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.Software"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static string? ReadSoftware(this TiffTagReader tagReader)
        {
            return tagReader.ReadASCIIFieldFirstString(TiffTag.Software, sizeLimit: -1);
        }

        #endregion
    
        #region StripByteCounts

        /// <summary>
        /// Read the values of <see cref="TiffTag.StripByteCounts"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<TiffValueCollection<ulong>> ReadStripByteCountsAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ulong>> valueTask = tagReader.ReadLong8FieldAsync(TiffTag.StripByteCounts, sizeLimit: -1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ulong> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<TiffValueCollection<ulong>>(result);
            }

            return new ValueTask<TiffValueCollection<ulong>>(TransformValueTaskAsync(valueTask));

            static async Task<TiffValueCollection<ulong>> TransformValueTaskAsync(ValueTask<TiffValueCollection<ulong>> valueTask)
            {
                TiffValueCollection<ulong> result = await valueTask.ConfigureAwait(false);
                return result;
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.StripByteCounts"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static TiffValueCollection<ulong> ReadStripByteCounts(this TiffTagReader tagReader)
        {
            TiffValueCollection<ulong> result = tagReader.ReadLong8Field(TiffTag.StripByteCounts, sizeLimit: -1);
            return result;
        }

        #endregion
    
        #region StripOffsets

        /// <summary>
        /// Read the values of <see cref="TiffTag.StripOffsets"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<TiffValueCollection<ulong>> ReadStripOffsetsAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ulong>> valueTask = tagReader.ReadLong8FieldAsync(TiffTag.StripOffsets, sizeLimit: -1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ulong> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<TiffValueCollection<ulong>>(result);
            }

            return new ValueTask<TiffValueCollection<ulong>>(TransformValueTaskAsync(valueTask));

            static async Task<TiffValueCollection<ulong>> TransformValueTaskAsync(ValueTask<TiffValueCollection<ulong>> valueTask)
            {
                TiffValueCollection<ulong> result = await valueTask.ConfigureAwait(false);
                return result;
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.StripOffsets"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static TiffValueCollection<ulong> ReadStripOffsets(this TiffTagReader tagReader)
        {
            TiffValueCollection<ulong> result = tagReader.ReadLong8Field(TiffTag.StripOffsets, sizeLimit: -1);
            return result;
        }

        #endregion
    
        #region SubFileType

        /// <summary>
        /// Read the values of <see cref="TiffTag.SubFileType"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<TiffSubfileType?> ReadSubFileTypeAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ushort>> valueTask = tagReader.ReadShortFieldAsync(TiffTag.SubFileType, sizeLimit: 1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ushort> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<TiffSubfileType?>(result.IsEmpty ? default(TiffSubfileType?) : (TiffSubfileType)result.GetFirstOrDefault());
            }

            return new ValueTask<TiffSubfileType?>(TransformValueTaskAsync(valueTask));

            static async Task<TiffSubfileType?> TransformValueTaskAsync(ValueTask<TiffValueCollection<ushort>> valueTask)
            {
                TiffValueCollection<ushort> result = await valueTask.ConfigureAwait(false);
                return result.IsEmpty ? default(TiffSubfileType?) : (TiffSubfileType)result.GetFirstOrDefault();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.SubFileType"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static TiffSubfileType? ReadSubFileType(this TiffTagReader tagReader)
        {
            TiffValueCollection<ushort> result = tagReader.ReadShortField(TiffTag.SubFileType, sizeLimit: 1);
            return result.IsEmpty ? default(TiffSubfileType?) : (TiffSubfileType)result.GetFirstOrDefault();
        }

        #endregion
    
        #region Threshholding

        /// <summary>
        /// Read the values of <see cref="TiffTag.Threshholding"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<TiffThreshholding> ReadThreshholdingAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ushort>> valueTask = tagReader.ReadShortFieldAsync(TiffTag.Threshholding, sizeLimit: 1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ushort> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<TiffThreshholding>(result.IsEmpty ? TiffThreshholding.NoThreshholding : (TiffThreshholding)result.GetFirstOrDefault());
            }

            return new ValueTask<TiffThreshholding>(TransformValueTaskAsync(valueTask));

            static async Task<TiffThreshholding> TransformValueTaskAsync(ValueTask<TiffValueCollection<ushort>> valueTask)
            {
                TiffValueCollection<ushort> result = await valueTask.ConfigureAwait(false);
                return result.IsEmpty ? TiffThreshholding.NoThreshholding : (TiffThreshholding)result.GetFirstOrDefault();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.Threshholding"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static TiffThreshholding ReadThreshholding(this TiffTagReader tagReader)
        {
            TiffValueCollection<ushort> result = tagReader.ReadShortField(TiffTag.Threshholding, sizeLimit: 1);
            return result.IsEmpty ? TiffThreshholding.NoThreshholding : (TiffThreshholding)result.GetFirstOrDefault();
        }

        #endregion
    
        #region XResolution

        /// <summary>
        /// Read the values of <see cref="TiffTag.XResolution"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<TiffRational?> ReadXResolutionAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<TiffRational>> valueTask = tagReader.ReadRationalFieldAsync(TiffTag.XResolution, sizeLimit: 1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<TiffRational> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<TiffRational?>(result.IsEmpty ? default(TiffRational?) : result.GetFirstOrDefault());
            }

            return new ValueTask<TiffRational?>(TransformValueTaskAsync(valueTask));

            static async Task<TiffRational?> TransformValueTaskAsync(ValueTask<TiffValueCollection<TiffRational>> valueTask)
            {
                TiffValueCollection<TiffRational> result = await valueTask.ConfigureAwait(false);
                return result.IsEmpty ? default(TiffRational?) : result.GetFirstOrDefault();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.XResolution"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static TiffRational? ReadXResolution(this TiffTagReader tagReader)
        {
            TiffValueCollection<TiffRational> result = tagReader.ReadRationalField(TiffTag.XResolution, sizeLimit: 1);
            return result.IsEmpty ? default(TiffRational?) : result.GetFirstOrDefault();
        }

        #endregion
    
        #region YResolution

        /// <summary>
        /// Read the values of <see cref="TiffTag.YResolution"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<TiffRational?> ReadYResolutionAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<TiffRational>> valueTask = tagReader.ReadRationalFieldAsync(TiffTag.YResolution, sizeLimit: 1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<TiffRational> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<TiffRational?>(result.IsEmpty ? default(TiffRational?) : result.GetFirstOrDefault());
            }

            return new ValueTask<TiffRational?>(TransformValueTaskAsync(valueTask));

            static async Task<TiffRational?> TransformValueTaskAsync(ValueTask<TiffValueCollection<TiffRational>> valueTask)
            {
                TiffValueCollection<TiffRational> result = await valueTask.ConfigureAwait(false);
                return result.IsEmpty ? default(TiffRational?) : result.GetFirstOrDefault();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.YResolution"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static TiffRational? ReadYResolution(this TiffTagReader tagReader)
        {
            TiffValueCollection<TiffRational> result = tagReader.ReadRationalField(TiffTag.YResolution, sizeLimit: 1);
            return result.IsEmpty ? default(TiffRational?) : result.GetFirstOrDefault();
        }

        #endregion
    
    }
}
