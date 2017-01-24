using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.BusinessAdmin.ServiceDetails;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{
    /// <summary>
    /// Service Details Controller
    /// </summary>
    /// <seealso cref="Axis.DataEngine.Helpers.Controllers.BaseApiController" />
    public class ServiceDetailsController : BaseApiController
    {
        #region Class Variables               
        /// <summary>
        /// The service details data provider
        /// </summary>
        readonly IServiceDetailsDataProvider _serviceDetailsDataProvider = null;

        #endregion

        #region Constructors    
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDetailsController"/> class.
        /// </summary>
        /// <param name="serviceDetailsDataProvider">The service details data provider.</param>
        public ServiceDetailsController(IServiceDetailsDataProvider serviceDetailsDataProvider)
        {
            _serviceDetailsDataProvider = serviceDetailsDataProvider;
        }

        #endregion

        #region Public Methods         


        /// <summary>
        /// Gets the service definition by identifier.
        /// </summary>
        /// <param name="servicesID">The services identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetServiceWorkflows(int servicesID)
        {
            return new HttpResult<Response<ServiceDetailsDataModel>>(_serviceDetailsDataProvider.GetServiceWorkflows(servicesID), Request);
        }

        /// <summary>
        /// Saves the service details.
        /// </summary>
        /// <param name="serviceDetails">The service details.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveServiceDetails(ServiceDetailsModel serviceDetails)
        {
            return new HttpResult<Response<ServiceDetailsModel>>(_serviceDetailsDataProvider.SaveServiceDetails(serviceDetails), Request);
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
            return new HttpResult<Response<ServiceDetailsModel>>(_serviceDetailsDataProvider.GetServiceDetails(servicesID, moduleComponentID), Request);
        }


        #endregion
    }
}