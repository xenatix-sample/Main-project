using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.RuleEngine.BusinessAdmin.ServiceDefinition;
using Axis.RuleEngine.Helpers.Results;
using System.Web.Http;

namespace Axis.RuleEngine.Service.Controllers
{
    /// <summary>
    /// Service Definition Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class ServiceDefinitionController : ApiController
    {
        #region Class Variables

        /// <summary>
        /// The service definition rule engine
        /// </summary>
        private readonly IServiceDefinitionRuleEngine _serviceDefinitionRuleEngine = null;

        #endregion

        #region Constructors                
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDefinitionController"/> class.
        /// </summary>
        /// <param name="serviceDefinitionRuleEngine">The service definition rule engine.</param>
        public ServiceDefinitionController(IServiceDefinitionRuleEngine serviceDefinitionRuleEngine)
        {
            _serviceDefinitionRuleEngine = serviceDefinitionRuleEngine;
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
            return new HttpResult<Response<ServicesModel>>(_serviceDefinitionRuleEngine.GetServices(searchText), Request);
        }

        /// <summary>
        /// Gets the service definition by identifier.
        /// </summary>
        /// <param name="serviceID">The service identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetServiceDefinitionByID(int serviceID)
        {
            return new HttpResult<Response<ServiceDefinitionModel>>(_serviceDefinitionRuleEngine.GetServiceDefinitionByID(serviceID), Request);
        }


        /// <summary>
        /// Saves the service definition.
        /// </summary>
        /// <param name="serviceDefinition">The service definition.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveServiceDefinition(ServiceDefinitionModel serviceDefinition)
        {
            return new HttpResult<Response<ServiceDefinitionModel>>(_serviceDefinitionRuleEngine.SaveServiceDefinition(serviceDefinition), Request);
        }



        #endregion
    }
}