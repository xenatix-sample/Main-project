using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// IService ConfigType DataProvider
    /// </summary>
    public interface IServiceConfigTypeDataProvider
    {
        /// <summary>
        /// Gets the service configuration types.
        /// </summary>
        /// <returns></returns>
        Response<ServiceConfigTypeModel> GetServiceConfigTypes();
    }
}