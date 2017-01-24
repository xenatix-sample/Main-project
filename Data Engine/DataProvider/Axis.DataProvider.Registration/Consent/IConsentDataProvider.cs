using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.DataProvider.Registration.Consent
{
    public interface IConsentDataProvider
    {
        Response<ConsentModel> AddConsentSignature(ConsentModel consent);
        Response<ConsentModel> GetConsentSignature(long contactId);
    }
}
