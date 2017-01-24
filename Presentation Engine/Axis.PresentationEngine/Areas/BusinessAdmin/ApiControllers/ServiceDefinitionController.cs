using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.ServiceDefinition;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.ApiControllers
{
    /// <summary>
    /// Service Definition Controller
    /// </summary>
    /// <seealso cref="Axis.PresentationEngine.Helpers.Controllers.BaseApiController" />
    public class ServiceDefinitionController : BaseApiController
    {
        #region Class Variables        

        /// <summary>
        /// The service definition repository
        /// </summary>
        private readonly IServiceDefinitionRepository _serviceDefinitionRepository;

        #endregion

        #region Constructors  
              
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDefinitionController"/> class.
        /// </summary>
        /// <param name="serviceDefinitionRepository">The service definition repository.</param>
        public ServiceDefinitionController(IServiceDefinitionRepository serviceDefinitionRepository)
        {
            _serviceDefinitionRepository = serviceDefinitionRepository;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        public Response<ServicesModel> GetServices(string searchText)
        {
            return _serviceDefinitionRepository.GetServices(searchText);
        }

        /// <summary>
        /// Gets the service definition by identifier.
        /// </summary>
        /// <param name="serviceID">The service identifier.</param>
        /// <returns></returns>
        public Response<ServiceDefinitionModel> GetServiceDefinitionByID(int serviceID)
        {
            return _serviceDefinitionRepository.GetServiceDefinitionByID(serviceID);
        }

        /// <summary>
        /// Saves the service definition.
        /// </summary>
        /// <param name="serviceDefinition">The service definition.</param>
        /// <returns></returns>
        public Response<ServiceDefinitionModel> SaveServiceDefinition(ServiceDefinitionModel serviceDefinition)
        {
            return _serviceDefinitionRepository.SaveServiceDefinition(serviceDefinition);
        }



        #endregion




    }
}