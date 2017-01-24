
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;

namespace Axis.Service.BusinessAdmin.ServiceDefinition
{
    /// <summary>
    /// IService Definition Service
    /// </summary>
    public interface IServiceDefinitionService
    {
        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        Response<ServicesModel> GetServices(string searchText);

        /// <summary>
        /// Gets the service definition by identifier.
        /// </summary>
        /// <param name="serviceID">The service identifier.</param>
        /// <returns></returns>
        Response<ServiceDefinitionModel> GetServiceDefinitionByID(int servicesID);

        /// <summary>
        /// Saves the service definition.
        /// </summary>
        /// <param name="serviceDefinition">The service definition.</param>
        /// <returns></returns>
        Response<ServiceDefinitionModel> SaveServiceDefinition(ServiceDefinitionModel serviceDefinition);


    }
}
