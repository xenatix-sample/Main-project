using Axis.Model.RecordedServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Model.Common;

namespace Axis.RuleEngine.RecordedServices.VoidService
{
    public interface IVoidServiceRuleEngine
    {
        Response<VoidServiceModel> VoidService(VoidServiceModel voidService);
        Response<VoidServiceModel> VoidServiceCallCenter(VoidServiceModel voidService);
        Response<VoidServiceModel> GetVoidService(int serviceRecordingID);
    }
}
