using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.Plugins.Registration.Models;

namespace Axis.Plugins.Registration.Repository
{
    public interface IConsentRepository
    {
        Response<ConsentViewModel> AddConsentSignature(ConsentViewModel consentViewModel);
        Response<ConsentViewModel> GetConsentSignature(long contactId);
        Task<Response<ConsentViewModel>> GetConsentSignatureAsync(long contactId);
    }
}
