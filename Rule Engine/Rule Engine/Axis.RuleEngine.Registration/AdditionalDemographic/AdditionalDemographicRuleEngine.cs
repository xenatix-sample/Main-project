using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.RuleEngine.Registration
{
    public class AdditionalDemographicRuleEngine : IAdditionalDemographicRuleEngine
    {
        private readonly IAdditionalDemographicService _additionalDemographicsService;

        public AdditionalDemographicRuleEngine(IAdditionalDemographicService additionalDemographicsService)
        {
            _additionalDemographicsService = additionalDemographicsService;
        }

        public Response<AdditionalDemographicsModel> GetAdditionalDemographic(long contactId)
        {
            return _additionalDemographicsService.GetAdditionalDemographic(contactId);
        }

        public Response<AdditionalDemographicsModel> AddAdditionalDemographic(AdditionalDemographicsModel additional)
        {
            return _additionalDemographicsService.AddAdditionalDemographic(additional);
        }

        public Response<AdditionalDemographicsModel> UpdateAdditionalDemographic(AdditionalDemographicsModel additional)
        {
            return _additionalDemographicsService.UpdateAdditionalDemographic(additional);
        }

        public Response<AdditionalDemographicsModel> DeleteAdditionalDemographic(long id, DateTime modifiedOn)
        {
            throw new NotImplementedException();
        }
    }
}
