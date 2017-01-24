using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IServiceDurationDataProvider
    {
        Response<ServiceDurationModel> GetServiceDurations();
    }
}