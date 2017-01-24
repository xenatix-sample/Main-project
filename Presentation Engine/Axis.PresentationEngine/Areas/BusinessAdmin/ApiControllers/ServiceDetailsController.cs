using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.Payors;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.ServiceDetails;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Http;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.ApiControllers
{
    /// <summary>
    /// Service Details Controller
    /// </summary>
    /// <seealso cref="Axis.PresentationEngine.Helpers.Controllers.BaseApiController" />
    public class ServiceDetailsController : BaseApiController
    {
        #region Class Variables        

        /// <summary>
        /// The service details repository
        /// </summary>
        private readonly IServiceDetailsRepository _serviceDetailsRepository;

        #endregion

        #region Constructors  
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDetailsController"/> class.
        /// </summary>
        /// <param name="serviceDetailsRepository">The service details repository.</param>
        public ServiceDetailsController(IServiceDetailsRepository serviceDetailsRepository)
        {
            _serviceDetailsRepository = serviceDetailsRepository;
        }
        #endregion

        #region Public Methods




        /// <summary>
        /// Gets the service workflows.
        /// </summary>
        /// <param name="servicesID">The services identifier.</param>
        /// <returns></returns>
        public Response<ServiceDetailsDataModel> GetServiceWorkflows(int servicesID)
        {
            return _serviceDetailsRepository.GetServiceWorkflows(servicesID);
        }

        /// <summary>
        /// Saves the service details.
        /// </summary>
        /// <param name="serviceDetails">The service details.</param>
        /// <returns></returns>
        public Response<ServiceDetailsModel> SaveServiceDetails(ServiceDetailsModel serviceDetails)
        {
            return _serviceDetailsRepository.SaveServiceDetails(serviceDetails);
        }

        /// <summary>
        /// Gets the service details.
        /// </summary>
        /// <param name="servicesID">The services identifier.</param>
        /// <param name="moduleComponentID">The module component identifier.</param>
        /// <returns></returns>
        public Response<ServiceDetailsModel> GetServiceDetails(int servicesID, long moduleComponentID)
        {
            return _serviceDetailsRepository.GetServiceDetails(servicesID, moduleComponentID);
        }


        #endregion

    }
}