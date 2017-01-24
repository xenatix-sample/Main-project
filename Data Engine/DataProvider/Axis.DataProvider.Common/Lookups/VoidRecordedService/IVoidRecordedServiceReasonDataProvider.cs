using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IVoidRecordedServiceReasonDataProvider
    {
        /// <summary>
        /// Gets the ServiceRecordingVoidReasonDetails of the service.
        /// </summary>
        /// <returns></returns>
        Response<VoidRecordedServiceReasonModel> GetVoidServiceRecordingReasonDetails();
    }
}
