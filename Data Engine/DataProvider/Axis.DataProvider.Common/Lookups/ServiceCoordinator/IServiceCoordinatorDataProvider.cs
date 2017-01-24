using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IServiceCoordinatorDataProvider
    {
        Response<ServiceCoordinatorModel> GetServiceCoordinators();
    }
}