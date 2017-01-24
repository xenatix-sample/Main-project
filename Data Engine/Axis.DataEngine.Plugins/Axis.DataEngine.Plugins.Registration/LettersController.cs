using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration
{
    public class LettersController : BaseApiController
    {
        readonly ILettersDataProvider _lettersDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="LettersController"/> class.
        /// </summary>
        /// <param name="lettersDataProvider">The letters data provider.</param>
        public LettersController(ILettersDataProvider lettersDataProvider)
        {
            _lettersDataProvider = lettersDataProvider;
        }

        /// <summary>
        /// Gets the letters.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpGet]
        public IHttpActionResult GetLetters(long contactID)
        {
            return new HttpResult<Response<LettersModel>>(_lettersDataProvider.GetLetters(contactID), Request);
        }

        /// <summary>
        /// Adds the letters.
        /// </summary>
        /// <param name="lettersModel">The letters model.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpPost]
        public IHttpActionResult AddLetters(LettersModel lettersModel)
        {
            return new HttpResult<Response<LettersModel>>(_lettersDataProvider.AddLetters(lettersModel), Request);
        }

        /// <summary>
        /// Updates the letters.
        /// </summary>
        /// <param name="lettersModel">The letters model.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpPut]
        public IHttpActionResult UpdateLetters(LettersModel lettersModel)
        {
            return new HttpResult<Response<LettersModel>>(_lettersDataProvider.UpdateLetters(lettersModel), Request);
        }

        /// <summary>
        /// Deletes the letters.
        /// </summary>
        /// <param name="contactLettersID">The contact letters identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns>IHttpActionResult.</returns>
        [HttpDelete]
        public IHttpActionResult DeleteLetters(long contactLettersID, DateTime modifiedOn)
        {
            return new HttpResult<Response<LettersModel>>(_lettersDataProvider.DeleteLetters(contactLettersID, modifiedOn), Request);
        }
    }
}
