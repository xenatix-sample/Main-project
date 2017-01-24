using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Respository.ServiceDetails
{
    public class ServiceDetailsRepository : IServiceDetailsRepository
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "ServiceDetails/";

        #endregion Class Variables

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDetailsRepository"/> class.
        /// </summary>
        public ServiceDetailsRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDetailsRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ServiceDetailsRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        #endregion Constructors

        #region Public Methods




        /// <summary>
        /// Gets the service workflows.
        /// </summary>
        /// <param name="servicesID">The services identifier.</param>
        /// <returns></returns>
        public Response<ServiceDetailsDataModel> GetServiceWorkflows(int servicesID)
        {
            const string apiUrl = BaseRoute + "GetServiceWorkflows";
            var requestXMLValueNvc = new NameValueCollection { { "servicesID", servicesID.ToString() } };
            return communicationManager.Get<Response<ServiceDetailsDataModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Saves the service details.
        /// </summary>
        /// <param name="serviceDetails">The service details.</param>
        /// <returns></returns>
        public Response<ServiceDetailsModel> SaveServiceDetails(ServiceDetailsModel serviceDetails)
        {
            const string apiUrl = BaseRoute + "SaveServiceDetails";
            return communicationManager.Post<ServiceDetailsModel, Response<ServiceDetailsModel>>(serviceDetails, apiUrl);
        }

        /// <summary>
        /// Gets the service details.
        /// </summary>
        /// <param name="servicesID">The services identifier.</param>
        /// <param name="moduleComponentID">The module component identifier.</param>
        /// <returns></returns>
        public Response<ServiceDetailsModel> GetServiceDetails(int servicesID,long moduleComponentID)
        {
            const string apiUrl = BaseRoute + "GetServiceDetails";
            var requestXMLValueNvc = new NameValueCollection { { "servicesID", servicesID.ToString() }, { "moduleComponentID", moduleComponentID.ToString() } };
            return communicationManager.Get<Response<ServiceDetailsModel>>(requestXMLValueNvc, apiUrl);
        }


        #endregion Public Methods
    }
}