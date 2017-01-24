using Axis.Model.Common;
using Axis.Plugins.Registration.Models;
using System;

namespace Axis.Plugins.Registration.Repository
{
    public interface ILettersRepository
    {
        /// <summary>
        /// Gets the letters.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns>Response&lt;LettersViewModel&gt;.</returns>
        Response<LettersViewModel> GetLetters(long contactID);

        /// <summary>
        /// Adds the letters.
        /// </summary>
        /// <param name="lettersModel">The letters model.</param>
        /// <returns>Response&lt;LettersViewModel&gt;.</returns>
        Response<LettersViewModel> AddLetters(LettersViewModel lettersModel);

        /// <summary>
        /// Updates the letters.
        /// </summary>
        /// <param name="lettersModel">The letters model.</param>
        /// <returns>Response&lt;LettersViewModel&gt;.</returns>
        Response<LettersViewModel> UpdateLetters(LettersViewModel lettersModel);

        /// <summary>
        /// Deletes the letters.
        /// </summary>
        /// <param name="contactLettersID">The contact letters identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns>Response&lt;LettersViewModel&gt;.</returns>
        Response<LettersViewModel> DeleteLetters(long contactLettersID, DateTime modifiedOn);
    }
}
