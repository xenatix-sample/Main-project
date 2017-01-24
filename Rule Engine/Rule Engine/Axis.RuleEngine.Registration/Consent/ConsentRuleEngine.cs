using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration.Consent;

namespace Axis.RuleEngine.Registration.Consent
{
    public class ConsentRuleEngine : IConsentRuleEngine
    {
        #region Class Variables

        private IConsentService consentService;

        #endregion

        #region Constructors

        public ConsentRuleEngine(IConsentService consentService)
        {
            this.consentService = consentService;
        }

        #endregion

        #region Public Methods

        public Response<ConsentModel> AddConsentSignature(ConsentModel consent)
        {
            return consentService.AddConsentSignature(consent);
        }

        public Response<ConsentModel> GetConsentSignature(long contactId)
        {
            return consentService.GetConsentSignature(contactId);
        }

        #endregion
    }
}
