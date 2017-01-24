using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Service.ECI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.RuleEngine.ECI
{
    public class ECIAdditionalDemographicRuleEngine:IECIAdditionalDemographicRuleEngine
    {
         private readonly IECIAdditionalDemographicService _eciAdditionalDemographicsService;

         public ECIAdditionalDemographicRuleEngine(IECIAdditionalDemographicService eciAdditionalDemographicsService)
        {
            _eciAdditionalDemographicsService = eciAdditionalDemographicsService;
        }

        public Response<ECIAdditionalDemographicsModel> GetAdditionalDemographic(long contactId)
        {
            return _eciAdditionalDemographicsService.GetAdditionalDemographic(contactId);
        }

        public Response<ECIAdditionalDemographicsModel> AddAdditionalDemographic(ECIAdditionalDemographicsModel additional)
        {
            return _eciAdditionalDemographicsService.AddAdditionalDemographic(additional);
        }

        public Response<ECIAdditionalDemographicsModel> UpdateAdditionalDemographic(ECIAdditionalDemographicsModel additional)
        {
            return _eciAdditionalDemographicsService.UpdateAdditionalDemographic(additional);
        }

        public Response<ECIAdditionalDemographicsModel> DeleteAdditionalDemographic(long id, DateTime modifiedOn)
        {
            throw new NotImplementedException();
        }
    }
}
