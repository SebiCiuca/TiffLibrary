﻿using System.Threading;
using System.Threading.Tasks;

namespace TiffLibrary
{
    public static partial class TiffTagReaderExtensions
    {
    
        #region YCbCrCoefficients

        /// <summary>
        /// Read the values of <see cref="TiffTag.YCbCrCoefficients"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<TiffRational[]> ReadYCbCrCoefficientsAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<TiffRational>> valueTask = tagReader.ReadRationalFieldAsync(TiffTag.YCbCrCoefficients, sizeLimit: -1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<TiffRational> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<TiffRational[]>(result.GetOrCreateArray());
            }

            return new ValueTask<TiffRational[]>(TransformValueTaskAsync(valueTask));

            static async Task<TiffRational[]> TransformValueTaskAsync(ValueTask<TiffValueCollection<TiffRational>> valueTask)
            {
                TiffValueCollection<TiffRational> result = await valueTask.ConfigureAwait(false);
                return result.GetOrCreateArray();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.YCbCrCoefficients"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static TiffRational[] ReadYCbCrCoefficients(this TiffTagReader tagReader)
        {
            TiffValueCollection<TiffRational> result = tagReader.ReadRationalField(TiffTag.YCbCrCoefficients, sizeLimit: -1);
            return result.GetOrCreateArray();
        }

        #endregion
    
        #region YCbCrSubSampling

        /// <summary>
        /// Read the values of <see cref="TiffTag.YCbCrSubSampling"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<ushort[]> ReadYCbCrSubSamplingAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ushort>> valueTask = tagReader.ReadShortFieldAsync(TiffTag.YCbCrSubSampling, sizeLimit: -1, cancellationToken);
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
        /// Read the values of <see cref="TiffTag.YCbCrSubSampling"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static ushort[] ReadYCbCrSubSampling(this TiffTagReader tagReader)
        {
            TiffValueCollection<ushort> result = tagReader.ReadShortField(TiffTag.YCbCrSubSampling, sizeLimit: -1);
            return result.GetOrCreateArray();
        }

        #endregion
    
        #region YCbCrPositioning

        /// <summary>
        /// Read the values of <see cref="TiffTag.YCbCrPositioning"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<TiffYCbCrPositioning> ReadYCbCrPositioningAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ushort>> valueTask = tagReader.ReadShortFieldAsync(TiffTag.YCbCrPositioning, sizeLimit: 1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ushort> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<TiffYCbCrPositioning>(result.IsEmpty ? TiffYCbCrPositioning.Unspecified : (TiffYCbCrPositioning)result.GetFirstOrDefault());
            }

            return new ValueTask<TiffYCbCrPositioning>(TransformValueTaskAsync(valueTask));

            static async Task<TiffYCbCrPositioning> TransformValueTaskAsync(ValueTask<TiffValueCollection<ushort>> valueTask)
            {
                TiffValueCollection<ushort> result = await valueTask.ConfigureAwait(false);
                return result.IsEmpty ? TiffYCbCrPositioning.Unspecified : (TiffYCbCrPositioning)result.GetFirstOrDefault();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.YCbCrPositioning"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static TiffYCbCrPositioning ReadYCbCrPositioning(this TiffTagReader tagReader)
        {
            TiffValueCollection<ushort> result = tagReader.ReadShortField(TiffTag.YCbCrPositioning, sizeLimit: 1);
            return result.IsEmpty ? TiffYCbCrPositioning.Unspecified : (TiffYCbCrPositioning)result.GetFirstOrDefault();
        }

        #endregion
    
    }
}
