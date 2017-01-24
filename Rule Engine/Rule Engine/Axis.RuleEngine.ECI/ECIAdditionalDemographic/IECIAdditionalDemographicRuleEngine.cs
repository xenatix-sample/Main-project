using Axis.Model.Common;
using Axis.Model.ECI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.RuleEngine.ECI
{
    public interface IECIAdditionalDemographicRuleEngine
    {
        Response<ECIAdditionalDemographicsModel> GetAdditionalDemographic(long contactId);
        Response<ECIAdditionalDemographicsModel> AddAdditionalDemographic(ECIAdditionalDemographicsModel additional);
        Response<ECIAdditionalDemographicsModel> UpdateAdditionalDemographic(ECIAdditionalDemographicsModel additional);
        Response<ECIAdditionalDemographicsModel> DeleteAdditionalDemographic(long id, DateTime modifiedOn);
    }
}
