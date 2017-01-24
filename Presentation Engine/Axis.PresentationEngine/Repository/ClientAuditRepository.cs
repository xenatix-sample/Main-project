using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Common.ClientAudit;
using Axis.PresentationEngine.Helpers.Model;
using Axis.Service;
using Axis.PresentationEngine.Helpers.Translator;
using System.Globalization;
using System.Collections.Specialized;
using Axis.Model.Registration;

namespace Axis.PresentationEngine.Repository
{
    public class ClientAuditRepository : IClientAuditRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "clientAudit/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientAuditRepository"/> class.
        /// </summary>
        public ClientAuditRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

         /// <summary>
        /// Initializes a new instance of the <see cref="ClientAuditRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ClientAuditRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Adds the client Audit log.
        /// </summary>
        /// <param name="clientAudit">The client Audit model.</param>
        /// <returns></returns>
        public Response<ClientAuditModel> AddClientAudit(ClientAuditModel clientAudit)
        {
            string apiUrl = baseRoute + "addClientAudit";
            return communicationManager.Post<ClientAuditModel, Response<ClientAuditModel>>(clientAudit, apiUrl);
        }

        public Response<ScreenAuditModel> AddScreenAudit(ScreenAuditModel screenAudit)
        {
            string apiUrl = baseRoute + "AddScreenAudit";
            return communicationManager.Post<ScreenAuditModel, Response<ScreenAuditModel>>(screenAudit, apiUrl);
        }


        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <returns></returns>
        public string GetUniqueId()
        {
            const string apiUrl = baseRoute + "GetUniqueId";
            return communicationManager.Get<string>(apiUrl);
        }


        /// <summary>
        /// Gets the history details.
        /// </summary>
        /// <param name="screenId">The screen identifier.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns></returns>
        public string GetHistoryDetails(long screenId, long primaryKey)
        {
            const string apiUrl = baseRoute + "GetHistoryDetails";
            var param = new NameValueCollection { { "screenId", screenId.ToString(CultureInfo.InvariantCulture) },
                                                    { "primaryKey", primaryKey.ToString(CultureInfo.InvariantCulture)}};
            return communicationManager.Get<string>(param, apiUrl);
        }

        /// <summary>
        /// Gets the demography history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<DemographyHistoryModel> GetDemographyHistory(long contactId)
        {
            const string apiUrl = baseRoute + "GetDemographyHistory";
            var param = new NameValueCollection { { "contactId", contactId.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<DemographyHistoryModel>>(param, apiUrl);
        }

        /// <summary>
        /// Gets the alias history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<AliasHistoryModel> GetAliasHistory(long contactId)
        {
            const string apiUrl = baseRoute + "GetAliasHistory";
            var param = new NameValueCollection { { "contactId", contactId.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<AliasHistoryModel>>(param, apiUrl);
        }

        /// <summary>
        /// Gets the identifier history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<IdHistoryModel> GetIdHistory(long contactId)
        {
            const string apiUrl = baseRoute + "GetIdHistory";
            var param = new NameValueCollection { { "contactId", contactId.ToString(CultureInfo.InvariantCulture) } };
            return communicationManager.Get<Response<IdHistoryModel>>(param, apiUrl);
        }
    }
}