using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Consents;
using Axis.Model.Common;
using Axis.Model.Consents;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{
    public class ConsentsController : BaseApiController
    {
        /// <summary>
        /// The consents data provider
        /// </summary>
        private readonly IConsentsDataProvider _consentsDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsentsController"/> class.
        /// </summary>
        /// <param name="consents">The consent data provider.</param>
        public ConsentsController(IConsentsDataProvider consentsDataProvider)
        {
            this._consentsDataProvider = consentsDataProvider;
        }

        /// <summary>
        /// Get Consents
        /// </summary>
        /// <param name="contactID">contactID</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetConsents(long contactID)
        {
            return new HttpResult<Response<ConsentsModel>>(_consentsDataProvider.GetConsents(contactID), Request);
        }

        /// <summary>
        /// Adds the consent.
        /// </summary>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddConsent(ConsentsModel consent)
        {
            return new HttpResult<Response<ConsentsModel>>(_consentsDataProvider.AddConsent(consent), Request);
        }

        /// <summary>
        /// Updates the consent.
        /// </summary>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateConsent(ConsentsModel consent)
        {
            return new HttpResult<Response<ConsentsModel>>(_consentsDataProvider.UpdateConsent(consent), Request);
        }

    }
}