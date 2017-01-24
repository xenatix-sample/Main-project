using Axis.Model.Common;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Repository;
using Axis.PresentationEngine.Helpers.Controllers;
using System;
using System.Web.Http;

namespace Axis.Plugins.Registration.ApiControllers
{
    public class lettersController : BaseApiController
    {
        private readonly ILettersRepository _lettersRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="lettersController"/> class.
        /// </summary>
        /// <param name="lettersRepository">The letters repository.</param>
        public lettersController(ILettersRepository lettersRepository)
        {
            this._lettersRepository = lettersRepository;
        }

        /// <summary>
        /// Gets the letters.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns>Response&lt;LettersViewModel&gt;.</returns>
        [HttpGet]
        public Response<LettersViewModel> GetLetters(long contactID)
        {
            return _lettersRepository.GetLetters(contactID);
        }

        /// <summary>
        /// Adds the letters.
        /// </summary>
        /// <param name="lettersModel">The letters model.</param>
        /// <returns>Response&lt;LettersViewModel&gt;.</returns>
        [HttpPost]
        public Response<LettersViewModel> AddLetters(LettersViewModel lettersModel)
        {
            return _lettersRepository.AddLetters(lettersModel);
        }

        /// <summary>
        /// Updates the letters.
        /// </summary>
        /// <param name="lettersModel">The letters model.</param>
        /// <returns>Response&lt;LettersViewModel&gt;.</returns>
        [HttpPut]
        public Response<LettersViewModel> UpdateLetters(LettersViewModel lettersModel)
        {
            return _lettersRepository.UpdateLetters(lettersModel);
        }

        /// <summary>
        /// Deletes the letters.
        /// </summary>
        /// <param name="contactLettersID">The contact letters identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns>Response&lt;LettersViewModel&gt;.</returns>
        [HttpDelete]
        public Response<LettersViewModel> DeleteLetters(long contactLettersID, DateTime modifiedOn)
        {
            return _lettersRepository.DeleteLetters(contactLettersID, modifiedOn);
        }
    }
}
