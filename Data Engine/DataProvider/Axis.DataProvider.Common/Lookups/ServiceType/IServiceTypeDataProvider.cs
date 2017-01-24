using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IServiceTypeDataProvider
    {
        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        /// <returns></returns>
        Response<ServiceTypeModel> GetServiceType();
    }
}
