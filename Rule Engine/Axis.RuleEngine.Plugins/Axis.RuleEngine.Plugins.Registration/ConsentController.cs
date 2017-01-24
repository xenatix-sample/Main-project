using System.Web.Http;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration.Consent;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Registration
{
    public class ConsentController : BaseApiController
    {
        #region Class Variables

        IConsentRuleEngine consentRuleEngine = null;

        #endregion

        #region Constructors

        public ConsentController(IConsentRuleEngine consentRuleEngine)
        {
            this.consentRuleEngine = consentRuleEngine;
        }

        #endregion

        #region Public Methods

        [HttpPost]
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Demography, Permission = Permission.Create)]
        public IHttpActionResult AddConsentSignature(ConsentModel consent)
        {            
            return new HttpResult<Response<ConsentModel>>(consentRuleEngine.AddConsentSignature(consent), Request);
        }

        [HttpGet]
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Demography, Permission = Permission.Read)]
        public IHttpActionResult GetConsentSignature(long contactId)
        {
            return new HttpResult<Response<ConsentModel>>(consentRuleEngine.GetConsentSignature(contactId), Request);
        }

        #endregion
    }
}
