using Axis.Model.Common;
using Axis.Model.ECI;

namespace Axis.Service.ECI.Demographic
{
    public interface IECIDemographicService
    {
        Response<ECIContactDemographicsModel> GetContactDemographics(long contactID);
        Response<ECIContactDemographicsModel> AddContactDemographics(ECIContactDemographicsModel contact);
        Response<ECIContactDemographicsModel> UpdateContactDemographics(ECIContactDemographicsModel contact);
    }
}
