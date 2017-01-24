using Axis.Model.Common;
using Axis.Model.ECI;

namespace Axis.RuleEngine.ECI.Demographic
{
    public interface IECIDemographicRuleEngine
    {
        Response<ECIContactDemographicsModel> GetContactDemographics(long contactID);
        Response<ECIContactDemographicsModel> AddContactDemographics(ECIContactDemographicsModel contact);
        Response<ECIContactDemographicsModel> UpdateContactDemographics(ECIContactDemographicsModel contact);
    }
}
