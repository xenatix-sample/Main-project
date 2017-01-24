using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Respository.ServiceDefinition
{
    public class ServiceDefinitionRepository : IServiceDefinitionRepository
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "ServiceDefinition/";

        #endregion Class Variables

        #region Constructors

        public ServiceDefinitionRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDefinitionRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ServiceDefinitionRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <param name="searchText">The search text.</param>
        /// <returns></returns>
        public Response<ServicesModel> GetServices(string searchText)
        {
            if (searchText == null)
            {
                searchText = "";
            }
            string apiUrl = BaseRoute + "GetServices";
            var parameters = new NameValueCollection { { "searchText", searchText.ToString(CultureInfo.InvariantCulture) } };
            var response = communicationManager.Get<Response<ServicesModel>>(parameters, apiUrl);
            return response;
        }

        /// <summary>
        /// Gets the service definition by identifier.
        /// </summary>
        /// <param name="serviceID">The service identifier.</param>
        /// <returns></returns>
        public Response<ServiceDefinitionModel> GetServiceDefinitionByID(int serviceID)
        {
            const string apiUrl = BaseRoute + "GetServiceDefinitionByID";
            var requestXMLValueNvc = new NameValueCollection { { "serviceID", serviceID.ToString() } };
            return communicationManager.Get<Response<ServiceDefinitionModel>>(requestXMLValueNvc, apiUrl);
        }

        /// <summary>
        /// Saves the service definition.
        /// </summary>
        /// <param name="serviceDefinition">The service definition.</param>
        /// <returns></returns>
        public Response<ServiceDefinitionModel> SaveServiceDefinition(ServiceDefinitionModel serviceDefinition)
        {
            const string apiUrl = BaseRoute + "SaveServiceDefinition";
            return communicationManager.Post<ServiceDefinitionModel, Response<ServiceDefinitionModel>>(serviceDefinition, apiUrl);
        }

        #endregion Public Methods
    }
}