using Axis.Model.RecordedServices;
using Axis.Model.Common;

namespace Axis.Service.RecordedServices.VoidService
{
    public interface IVoidRecordedService
    {
        Response<VoidServiceModel> VoidService(VoidServiceModel voidService);
        Response<VoidServiceModel> VoidServiceCallCenter(VoidServiceModel voidService);
        Response<VoidServiceModel> GetVoidService(int serviceRecordingID);
    }
}