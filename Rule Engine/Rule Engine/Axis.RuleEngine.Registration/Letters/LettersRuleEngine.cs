using System;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration;

namespace Axis.RuleEngine.Registration
{
    public class LettersRuleEngine : ILettersRuleEngine
    {
        private readonly ILettersService _lettersService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LettersRuleEngine"/> class.
        /// </summary>
        /// <param name="lettersService">The letters service.</param>
        public LettersRuleEngine(ILettersService lettersService)
        {
            this._lettersService = lettersService;
        }

        /// <summary>
        /// Gets the letters.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns>Response&lt;LettersModel&gt;.</returns>
        public Response<LettersModel> GetLetters(long contactID)
        {
            return _lettersService.GetLetters(contactID);
        }

        /// <summary>
        /// Adds the letters.
        /// </summary>
        /// <param name="lettersModel">The letters model.</param>
        /// <returns>Response&lt;LettersModel&gt;.</returns>
        public Response<LettersModel> AddLetters(LettersModel lettersModel)
        {
            return _lettersService.AddLetters(lettersModel);
        }

        /// <summary>
        /// Updates the letters.
        /// </summary>
        /// <param name="lettersModel">The letters model.</param>
        /// <returns>Response&lt;LettersModel&gt;.</returns>
        public Response<LettersModel> UpdateLetters(LettersModel lettersModel)
        {
            return _lettersService.UpdateLetters(lettersModel);
        }

        /// <summary>
        /// Deletes the letters.
        /// </summary>
        /// <param name="contactLettersID">The contact letters identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns>Response&lt;LettersModel&gt;.</returns>
        public Response<LettersModel> DeleteLetters(long contactLettersID, DateTime modifiedOn)
        {
            return _lettersService.DeleteLetters(contactLettersID, modifiedOn);
        }
    }
}
