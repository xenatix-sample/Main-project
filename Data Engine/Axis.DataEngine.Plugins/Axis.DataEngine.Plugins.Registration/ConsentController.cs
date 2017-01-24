using System.Collections.Generic;
using System.Web.Http;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration.Consent;
using Axis.Helpers.Caching;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Model.Setting;

namespace Axis.DataEngine.Plugins.Registration
{
    public class ConsentController : BaseApiController
    {
        #region Class Variables

        IConsentDataProvider consentDataProvider = null;

        #endregion

        #region Constructors

        public ConsentController(IConsentDataProvider consentDataProvider)
        {
            this.consentDataProvider = consentDataProvider;
        }

        #endregion

        #region Public Methods

        [HttpPost]
        public IHttpActionResult AddConsentSignature(ConsentModel consent)
        {
            return new HttpResult<Response<ConsentModel>>(consentDataProvider.AddConsentSignature(consent), Request);
        }

        [HttpGet]
        public IHttpActionResult GetConsentSignature(long contactId)
        {
            return new HttpResult<Response<ConsentModel>>(consentDataProvider.GetConsentSignature(contactId), Request);
        }

        #endregion
    }
}
