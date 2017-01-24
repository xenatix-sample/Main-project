using Axis.Model.Common;
using Axis.Model.Registration;
using System;

namespace Axis.DataProvider.Registration
{
    public interface ILettersDataProvider
    {
        /// <summary>
        /// Gets the letters.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns>Response&lt;LettersModel&gt;.</returns>
        Response<LettersModel> GetLetters(long contactID);

        /// <summary>
        /// Adds the letters.
        /// </summary>
        /// <param name="lettersModel">The letters model.</param>
        /// <returns>Response&lt;LettersModel&gt;.</returns>
        Response<LettersModel> AddLetters(LettersModel lettersModel);

        /// <summary>
        /// Updates the letters.
        /// </summary>
        /// <param name="lettersModel">The letters model.</param>
        /// <returns>Response&lt;LettersModel&gt;.</returns>
        Response<LettersModel> UpdateLetters(LettersModel lettersModel);

        /// <summary>
        /// Deletes the letters.
        /// </summary>
        /// <param name="contactLettersID">The intake letters identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns>Response&lt;LettersModel&gt;.</returns>
        Response<LettersModel> DeleteLetters(long contactLettersID, DateTime modifiedOn);
    }
}
