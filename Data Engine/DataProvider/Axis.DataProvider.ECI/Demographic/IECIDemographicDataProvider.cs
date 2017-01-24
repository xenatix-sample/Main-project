using Axis.Model.Common;
using Axis.Model.ECI;

namespace Axis.DataProvider.ECI.Demographic
{
    public interface IECIDemographicDataProvider
    {
        Response<ECIContactDemographicsModel> GetContactDemographics(long contactID);
        Response<ECIContactDemographicsModel> AddContactDemographics(ECIContactDemographicsModel contact);
        Response<ECIContactDemographicsModel> UpdateContactDemographics(ECIContactDemographicsModel contact);
    }
}
