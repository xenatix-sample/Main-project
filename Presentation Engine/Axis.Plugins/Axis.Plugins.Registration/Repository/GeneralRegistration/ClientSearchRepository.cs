using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Registration;
using Axis.Service;
using Axis.Model.Common;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Constant;

namespace Axis.Plugins.Registration.Repository
{
    public class ClientSearchRepository : IClientSearchRepository
    {
        #region Private Variables

        /// <summary>
        /// Privates variables 
        /// </summary>
        private readonly CommunicationManager communicationManager = null;
        private const string baseRoute = "Registration/";
        private const string clientSummary = "GetClientSummary";

        #endregion

        #region Constructor

        /// <summary>
        /// Creating object of Communication Manager
        /// </summary>
        public ClientSearchRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        #endregion

        /// <summary>
        /// Creating object of Communication Manager for unit test case
        /// </summary>
        public ClientSearchRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        #region Interface Implimentation

        /// <summary>
        /// Get the patient details based on search criteria
        /// </summary>
        /// <param name="SearchCriteria">Text to Search</param>
        /// <param name="contactType">contact type of contact</param>
        /// <returns></returns>
        public Response<ContactDemographicsModel> GetClientSummary(string searchCriteria, string contactType)
        {
            string apiUrl = string.Format("{0}{1}", baseRoute, clientSummary);
            var request = new NameValueCollection { { "SearchCriteria", searchCriteria }, { "ContactType", contactType ?? string.Empty } };  //.ToString(CultureInfo.InvariantCulture)
            return communicationManager.Get<Response<ContactDemographicsModel>>(request, apiUrl);
        }

        #endregion
    }
}