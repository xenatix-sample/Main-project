using System;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Configuration;
using Axis.Security;
using Axis.Service.BusinessAdmin.Payors;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.BusinessAdmin.ServiceDefinition
{
    public class ServiceDefinitionService : IServiceDefinitionService
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

        #endregion

        #region Constructors                
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDefinitionService"/> class.
        /// </summary>
        public ServiceDefinitionService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
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
            if (searchText == null)
            {
                searchText = "";
            }
            const string apiUrl = BaseRoute + "GetServices";
            var requestXMLValueNvc = new NameValueCollection { { "searchText", searchText } };
            return communicationManager.Get<Response<ServicesModel>>(requestXMLValueNvc, apiUrl);

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

        #endregion
    }
}
