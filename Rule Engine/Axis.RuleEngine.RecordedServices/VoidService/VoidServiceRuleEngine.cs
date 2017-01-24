using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Model.RecordedServices;
using Axis.Service.RecordedServices.VoidService;
using Axis.Model.Common;

namespace Axis.RuleEngine.RecordedServices.VoidService
{
    public class VoidServiceRuleEngine : IVoidServiceRuleEngine
    {
        private IVoidRecordedService voidRecordedService;
        public VoidServiceRuleEngine(IVoidRecordedService voidRecordedService)
        {
            this.voidRecordedService = voidRecordedService;
        }

        public Response<VoidServiceModel> VoidService(VoidServiceModel voidService)
        {
            return voidRecordedService.VoidService(voidService);
        }

        public Response<VoidServiceModel> VoidServiceCallCenter(VoidServiceModel voidService)
        {
            return voidRecordedService.VoidServiceCallCenter(voidService);
        }

        public Response<VoidServiceModel> GetVoidService(int serviceRecordingID)
        {
            return voidRecordedService.GetVoidService(serviceRecordingID);
        }
    }
}
