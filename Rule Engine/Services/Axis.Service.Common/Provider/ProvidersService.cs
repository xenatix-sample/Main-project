using Axis.Configuration;
using Axis.Model.Common;
using Axis.Security;
using System.Collections.Specialized;

namespace Axis.Service.Common.Provider
{
    public class ProvidersService : IProvidersService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "Providers/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvidersService" /> class.
        /// </summary>
        public ProvidersService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <param name="filterCriteria">The filter criteria.</param>
        /// <returns></returns>
        public Response<ProvidersModel> GetProviders(int filterCriteria)
        {
            var apiUrl = baseRoute + "GetProviders";
            var param = new NameValueCollection();
            param.Add("filterCriteria", filterCriteria.ToString());
            return communicationManager.Get<Response<ProvidersModel>>(param, apiUrl);
        }
        /// <summary>
        /// Gets the provider by id.
        /// </summary>
        /// <param name="providerID">The provider ID.</param>
        /// <returns></returns>
        public Response<ProvidersModel> GetProviderByid(int providerID)
        {
            var apiUrl = baseRoute + "GetProviderByid";
            var param = new NameValueCollection();
            param.Add("providerID", providerID.ToString());
            return communicationManager.Get<Response<ProvidersModel>>(param, apiUrl);
        }
    }
}