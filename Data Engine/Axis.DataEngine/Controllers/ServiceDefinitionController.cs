using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.BusinessAdmin.Payors;
using Axis.DataProvider.BusinessAdmin.ServiceDefinition;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{
    /// <summary>
    /// Service Definition Controller
    /// </summary>
    /// <seealso cref="Axis.DataEngine.Helpers.Controllers.BaseApiController" />
    public class ServiceDefinitionController : BaseApiController
    {
        #region Class Variables               
        /// <summary>
        /// The service definition data provider
        /// </summary>
        readonly IServiceDefinitionDataProvider _serviceDefinitionDataProvider = null;

        #endregion

        #region Constructors    
                    
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDefinitionController"/> class.
        /// </summary>
        /// <param name="serviceDefinitionDataProvider">The service definition data provider.</param>
        public ServiceDefinitionController(IServiceDefinitionDataProvider serviceDefinitionDataProvider)
        {
            _serviceDefinitionDataProvider = serviceDefinitionDataProvider;
        }

        #endregion

        #region Public Methods         
        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetServices(string searchText)
        {
            return new HttpResult<Response<ServicesModel>>(_serviceDefinitionDataProvider.GetServices(searchText), Request);
        }

        /// <summary>
        /// Gets the service definition by identifier.
        /// </summary>
        /// <param name="serviceID">The service identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetServiceDefinitionByID(int serviceID)
        {
            return new HttpResult<Response<ServiceDefinitionModel>>(_serviceDefinitionDataProvider.GetServiceDefinitionByID(serviceID), Request);
        }

        /// <summary>
        /// Saves the service definition.
        /// </summary>
        /// <param name="serviceDefinition">The service definition.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveServiceDefinition(ServiceDefinitionModel serviceDefinition)
        {
            return new HttpResult<Response<ServiceDefinitionModel>>(_serviceDefinitionDataProvider.SaveServiceDefinition(serviceDefinition), Request);
        }


        #endregion
    }
}