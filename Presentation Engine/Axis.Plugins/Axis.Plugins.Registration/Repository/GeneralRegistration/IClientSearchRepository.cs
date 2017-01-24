
using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.Plugins.Registration.Repository
{
    public interface IClientSearchRepository
    {
        Response<ContactDemographicsModel> GetClientSummary(string searchCriteria, string contactType);
    }
}
