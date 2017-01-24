using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.RuleEngine.Registration.Consent
{
    public interface IConsentRuleEngine
    {
        Response<ConsentModel> AddConsentSignature(ConsentModel consent);
        Response<ConsentModel> GetConsentSignature(long contactId);
    }
}
