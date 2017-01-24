using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.PresentationEngine.Areas.Admin.Respository.Provider
{
    public class ProvidersRepository : IProvidersRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "providers/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ProvidersRepository"/> class.
        /// </summary>
        public ProvidersRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Gets the providers.
        /// </summary>
        /// <param name="filterCriteria">The filter criteria.</param>
        /// <returns></returns>
        public Response<ProvidersModel> GetProviders(int filterCriteria)
        {
            string apiUrl = baseRoute + "GetProviders";
            var param = new NameValueCollection { { "filterCriteria", filterCriteria.ToString(CultureInfo.InvariantCulture) } };
            var response = communicationManager.Get<Response<ProvidersModel>>(param, apiUrl);
            return response;
        }
        /// <summary>
        /// Gets the provider by id.
        /// </summary>
        /// <param name="providerID">The provider ID.</param>
        /// <returns></returns>
        public Response<ProvidersModel> GetProviderByid(int providerID)
        {
            string apiUrl = baseRoute + "GetProviderByid";
            var param = new NameValueCollection { { "providerID", providerID.ToString(CultureInfo.InvariantCulture) } };
            var response = communicationManager.Get<Response<ProvidersModel>>(param, apiUrl);
            return response;
        }
    }
}