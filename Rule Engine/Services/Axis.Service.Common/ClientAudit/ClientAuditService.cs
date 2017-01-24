using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Common.ClientAudit;
using Axis.Model.Registration;
using Axis.Security;
using System.Collections.Specialized;

namespace Axis.Service.Common.ClientAudit
{
    public class ClientAuditService : IClientAuditService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "clientAudit/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientAuditService"/> class.
        /// </summary>
        public ClientAuditService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Adds the client Audit log.
        /// </summary>
        /// <param name="clientAudit">The client Audit model.</param>
        /// <returns></returns>
        public Response<ClientAuditModel> AddClientAudit(ClientAuditModel clientAudit)
        {
            var apiUrl = baseRoute + "AddClientAudit";
            return communicationManager.Post<ClientAuditModel, Response<ClientAuditModel>>(clientAudit, apiUrl);
        }

        public Response<ScreenAuditModel> AddScreenAudit(ScreenAuditModel screenAudit)
        {
            var apiUrl = baseRoute + "AddScreenAudit";
            return communicationManager.Post<ScreenAuditModel, Response<ScreenAuditModel>>(screenAudit, apiUrl);
        }

        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <returns></returns>
        public string GetUniqueId()
        {
            var apiUrl = baseRoute + "GetUniqueId";
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
            var apiUrl = baseRoute + "GetHistoryDetails";
            var param = new NameValueCollection();
            param.Add("screenId", screenId.ToString());
            param.Add("primaryKey", primaryKey.ToString());
            return communicationManager.Get<string>(param, apiUrl);
        }

        /// <summary>
        /// Gets the demography history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<DemographyHistoryModel> GetDemographyHistory(long contactId)
        {
            var apiUrl = baseRoute + "GetDemographyHistory";
            var param = new NameValueCollection { { "contactId", contactId.ToString() } };
            return communicationManager.Get<Response<DemographyHistoryModel>>(param, apiUrl);
        }

        /// <summary>
        /// Gets the alias history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<AliasHistoryModel> GetAliasHistory(long contactId)
        {
            var apiUrl = baseRoute + "GetAliasHistory";
            var param = new NameValueCollection { { "contactId", contactId.ToString() } };
            return communicationManager.Get<Response<AliasHistoryModel>>(param, apiUrl);
        }

        /// <summary>
        /// Gets the identifier history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<IdHistoryModel> GetIdHistory(long contactId)
        {
            var apiUrl = baseRoute + "GetIdHistory";
            var param = new NameValueCollection { { "contactId", contactId.ToString() } };
            return communicationManager.Get<Response<IdHistoryModel>>(param, apiUrl);
        }
    }
}
