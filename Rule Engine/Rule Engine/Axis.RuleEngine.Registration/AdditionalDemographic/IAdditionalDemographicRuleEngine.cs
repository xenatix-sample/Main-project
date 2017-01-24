using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.RuleEngine.Registration
{
    public interface IAdditionalDemographicRuleEngine
    {
        Response<AdditionalDemographicsModel> GetAdditionalDemographic(long contactId);
        Response<AdditionalDemographicsModel> AddAdditionalDemographic(AdditionalDemographicsModel additional);
        Response<AdditionalDemographicsModel> UpdateAdditionalDemographic(AdditionalDemographicsModel additional);
        Response<AdditionalDemographicsModel> DeleteAdditionalDemographic(long id, DateTime modifiedOn);
    }
}
