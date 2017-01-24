using Axis.PresentationEngine.Areas.RecordedServices.Models;
using Axis.Model.Common;

namespace Axis.PresentationEngine.Areas.RecordedServices.Repository
{
    public interface IVoidServiceRepository
    {
        Response<VoidServiceViewModel> VoidService(VoidServiceViewModel voidService);
        Response<VoidServiceViewModel> VoidServiceCallCenter(VoidServiceViewModel voidService);
        Response<VoidServiceViewModel> GetVoidService(int serviceRecordingID);
        
    }
}
