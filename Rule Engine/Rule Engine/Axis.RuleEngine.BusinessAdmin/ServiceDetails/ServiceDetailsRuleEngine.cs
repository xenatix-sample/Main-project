using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.RuleEngine.BusinessAdmin.ServiceDetails;
using Axis.Service.BusinessAdmin.ServiceDetails;

namespace Axis.RuleEngine.BusinessAdmin.Payors
{
    public class ServiceDetailsRuleEngine : IServiceDetailsRuleEngine
    {
        #region Class Variables                
        /// <summary>
        /// The service definition service
        /// </summary>
        private readonly IServiceDetailsService _serviceDetailsService;

        #endregion

        #region Constructors               
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDefinitionRuleEngine"/> class.
        /// </summary>
        /// <param name="serviceDefinitionService">The service definition service.</param>
        public ServiceDetailsRuleEngine(IServiceDetailsService serviceDetailsService)
        {
            _serviceDetailsService = serviceDetailsService;
        }

        #endregion


        /// <summary>
        /// Gets the service workflows.
        /// </summary>
        /// <param name="servicesID">The services identifier.</param>
        /// <returns></returns>
        public Response<ServiceDetailsDataModel> GetServiceWorkflows(int servicesID)
        {
            return _serviceDetailsService.GetServiceWorkflows(servicesID);
        }

        /// <summary>
        /// Saves the service details.
        /// </summary>
        /// <param name="serviceDetails">The service details.</param>
        /// <returns></returns>
        public Response<ServiceDetailsModel> SaveServiceDetails(ServiceDetailsModel serviceDetails)
        {
            return _serviceDetailsService.SaveServiceDetails(serviceDetails);
        }

        /// <summary>
        /// Gets the service details.
        /// </summary>
        /// <param name="servicesID">The services identifier.</param>
        /// <param name="moduleComponentID">The module component identifier.</param>
        /// <returns></returns>
        public Response<ServiceDetailsModel> GetServiceDetails(int servicesID, long moduleComponentID)
        {
            return _serviceDetailsService.GetServiceDetails(servicesID,moduleComponentID);
        }


    }
}
