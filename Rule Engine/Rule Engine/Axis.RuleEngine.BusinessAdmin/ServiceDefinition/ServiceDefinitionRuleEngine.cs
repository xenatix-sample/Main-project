using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.RuleEngine.BusinessAdmin.ServiceDefinition;
using Axis.Service.BusinessAdmin.ServiceDefinition;

namespace Axis.RuleEngine.BusinessAdmin.Payors
{
    public class ServiceDefinitionRuleEngine : IServiceDefinitionRuleEngine
    {
        #region Class Variables                
        /// <summary>
        /// The service definition service
        /// </summary>
        private readonly IServiceDefinitionService _serviceDefinitionService;

        #endregion

        #region Constructors               
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDefinitionRuleEngine"/> class.
        /// </summary>
        /// <param name="serviceDefinitionService">The service definition service.</param>
        public ServiceDefinitionRuleEngine  (IServiceDefinitionService serviceDefinitionService)
        {
            _serviceDefinitionService = serviceDefinitionService;
        }

        #endregion

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        public Response<ServicesModel> GetServices(string searchText="")
        {
            return _serviceDefinitionService.GetServices(searchText);

        }

        /// <summary>
        /// Gets the service definition by identifier.
        /// </summary>
        /// <param name="serviceID">The service identifier.</param>
        /// <returns></returns>
        public Response<ServiceDefinitionModel> GetServiceDefinitionByID(int serviceID)
        {
            return _serviceDefinitionService.GetServiceDefinitionByID(serviceID);
        }

        /// <summary>
        /// Saves the service definition.
        /// </summary>
        /// <param name="serviceDefinition">The service definition.</param>
        /// <returns></returns>
        public Response<ServiceDefinitionModel> SaveServiceDefinition(ServiceDefinitionModel serviceDefinition)
        {
            return _serviceDefinitionService.SaveServiceDefinition(serviceDefinition);
        }

    }
}
