﻿using System.Threading;
using System.Threading.Tasks;

namespace TiffLibrary
{
    public static partial class TiffTagReaderExtensions
    {
    
        #region Predictor

        /// <summary>
        /// Read the values of <see cref="TiffTag.Predictor"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> that fires if the user want to stop the current task.</param>
        /// <returns>A <see cref="ValueTask{TiffValueCollection}"/> that completes when the value of the tag is read and return the read values.</returns>
        public static ValueTask<TiffPredictor> ReadPredictorAsync(this TiffTagReader tagReader, CancellationToken cancellationToken = default)
        {
            ValueTask<TiffValueCollection<ushort>> valueTask = tagReader.ReadShortFieldAsync(TiffTag.Predictor, sizeLimit: 1, cancellationToken);
            if (valueTask.IsCompletedSuccessfully)
            {
                TiffValueCollection<ushort> result = valueTask.GetAwaiter().GetResult();
                return new ValueTask<TiffPredictor>(result.IsEmpty ? TiffPredictor.None : (TiffPredictor)result.GetFirstOrDefault());
            }

            return new ValueTask<TiffPredictor>(TransformValueTaskAsync(valueTask));

            static async Task<TiffPredictor> TransformValueTaskAsync(ValueTask<TiffValueCollection<ushort>> valueTask)
            {
                TiffValueCollection<ushort> result = await valueTask.ConfigureAwait(false);
                return result.IsEmpty ? TiffPredictor.None : (TiffPredictor)result.GetFirstOrDefault();
            }
        }

        /// <summary>
        /// Read the values of <see cref="TiffTag.Predictor"/>.
        /// </summary>
        /// <param name="tagReader">The tag reader to use.</param>
        /// <returns>The values read.</returns>
        public static TiffPredictor ReadPredictor(this TiffTagReader tagReader)
        {
            TiffValueCollection<ushort> result = tagReader.ReadShortField(TiffTag.Predictor, sizeLimit: 1);
            return result.IsEmpty ? TiffPredictor.None : (TiffPredictor)result.GetFirstOrDefault();
        }

        #endregion
    
    }
}
