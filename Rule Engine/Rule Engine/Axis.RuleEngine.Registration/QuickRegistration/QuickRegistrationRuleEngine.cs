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
    public class QuickRegistrationRuleEngine : IQuickRegistrationRuleEngine
    {
        private readonly IQuickRegistrationService quickRegistrationService;

        public QuickRegistrationRuleEngine(IQuickRegistrationService quickRegistrationService)
        {
            this.quickRegistrationService = quickRegistrationService;
        }

        public QuickRegistrationRuleEngine(string token)
        {
        }

        public Response<QuickRegistrationModel> GetQuickRegistration()
        {
            return quickRegistrationService.GetQuickRegistration();
        }

        public Response<QuickRegistrationModel> AddQuickRegistration(QuickRegistrationModel patient)
        {
            return quickRegistrationService.Add(patient);
        }

        public Response<QuickRegistrationModel> GetMrnMpi()
        {
            return quickRegistrationService.GetMrnMpi();
        }
    }
}
