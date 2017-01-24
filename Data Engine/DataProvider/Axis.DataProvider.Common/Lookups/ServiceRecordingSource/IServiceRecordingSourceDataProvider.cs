using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IServiceRecordingSourceDataProvider
    {
        /// <summary>
        /// Gets the service recording source.
        /// </summary>
        /// <returns></returns>
        Response<ServiceRecordingSourceModel> GetServiceRecordingSource();
    }
}
