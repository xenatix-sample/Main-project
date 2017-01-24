using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IServiceStatusDataProvider
    {
        /// <summary>
        /// Gets the Service Statuses.
        /// </summary>
        /// <returns></returns>
        Response<ServiceStatusModel> GetServiceStatuses();

        /// <summary>
        /// Gets the service to service statuses.
        /// </summary>
        /// <returns></returns>
        Response<ServiceStatusModel> GetServiceToServiceStatuses();

        /// <summary>
        /// Gets the configured service statuses.
        /// </summary>
        /// <returns></returns>
        Response<ServiceStatusModel> GetServiceStatusesConfigured();
    }
}
