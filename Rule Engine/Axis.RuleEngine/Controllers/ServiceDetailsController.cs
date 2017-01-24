using Axis.Helpers.Validation;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.RuleEngine.BusinessAdmin.Payors;
using Axis.RuleEngine.BusinessAdmin.ServiceDetails;
using Axis.RuleEngine.Helpers.Results;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Http;

namespace Axis.RuleEngine.Service.Controllers
{
    /// <summary>
    /// Service Details Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class ServiceDetailsController : ApiController
    {
        #region Class Variables

        /// <summary>
        /// The service details rule engine
        /// </summary>
        private readonly IServiceDetailsRuleEngine _serviceDetailsRuleEngine = null;

        #endregion

        #region Constructors                
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDetailsController"/> class.
        /// </summary>
        /// <param name="serviceDetailsRuleEngine">The service details rule engine.</param>
        public ServiceDetailsController(IServiceDetailsRuleEngine serviceDetailsRuleEngine)
        {
            _serviceDetailsRuleEngine = serviceDetailsRuleEngine;
        }

        #endregion

        #region Public Methods


        /// <summary>
        /// Gets the service workflows.
        /// </summary>
        /// <param name="servicesID">The services identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetServiceWorkflows(int servicesID)
        {
            return new HttpResult<Response<ServiceDetailsDataModel>>(_serviceDetailsRuleEngine.GetServiceWorkflows(servicesID), Request);
        }

        /// <summary>
        /// Saves the service details.
        /// </summary>
        /// <param name="serviceDetails">The service details.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveServiceDetails(ServiceDetailsModel serviceDetails)
        {
            return new HttpResult<Response<ServiceDetailsModel>>(_serviceDetailsRuleEngine.SaveServiceDetails(serviceDetails), Request);
        }

        /// <summary>
        /// Gets the service details.
        /// </summary>
        /// <param name="servicesID">The services identifier.</param>
        /// <param name="moduleComponentID">The module component identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetServiceDetails(int servicesID, long moduleComponentID)
        {
            return new HttpResult<Response<ServiceDetailsModel>>(_serviceDetailsRuleEngine.GetServiceDetails(servicesID,moduleComponentID), Request);
        }


        #endregion
    }
}