using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IServiceDataProvider
    {
        /// <summary>
        /// Gets the service location.
        /// </summary>
        /// <returns></returns>
        Response<ServiceModel> GetService();

        /// <summary>
        /// Gets the service details.
        /// </summary>
        /// <returns></returns>
        Response<ServiceModel> GetServiceDetails();
    }
}