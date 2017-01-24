using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.Service.Registration.Consent
{
    public interface IConsentService
    {
        Response<ConsentModel> AddConsentSignature(ConsentModel consent);
        Response<ConsentModel> GetConsentSignature(long contactId);
    }
}
