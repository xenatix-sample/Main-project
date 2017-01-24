using Axis.Model.Common;
using Axis.Model.Common.ClientAudit;
using Axis.Model.Registration;
using Axis.Service.Common.ClientAudit;

namespace Axis.RuleEngine.Common.ClientAudit
{
    public class ClientAuditRuleEngine : IClientAuditRuleEngine
    {
        /// <summary>
        /// The ClientAudit service
        /// </summary>
        private IClientAuditService clientAuditService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientAuditRuleEngine"/> class.
        /// </summary>
        /// <param name="photoService">The client audit service.</param>
        public ClientAuditRuleEngine(IClientAuditService clientAuditService)
        {
            this.clientAuditService = clientAuditService;
        }

        /// <summary>
        /// Adds the client Audit log.
        /// </summary>
        /// <param name="clientAudit">The client Audit model.</param>
        /// <returns></returns>
        public Response<ClientAuditModel> AddClientAudit(ClientAuditModel clientAudit)
        {
            return clientAuditService.AddClientAudit(clientAudit);
        }

        public Response<ScreenAuditModel> AddScreenAudit(ScreenAuditModel screenAudit)
        {
            return clientAuditService.AddScreenAudit(screenAudit);
        }


        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <returns></returns>
        public string GetUniqueId()
        {
            return clientAuditService.GetUniqueId();
        }


        /// <summary>
        /// Gets the history details.
        /// </summary>
        /// <param name="screenId">The screen identifier.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns></returns>
        public string GetHistoryDetails(long screenId, long primaryKey)
        {
            return clientAuditService.GetHistoryDetails(screenId, primaryKey);
        }

        /// <summary>
        /// Gets the demography history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<DemographyHistoryModel> GetDemographyHistory(long contactId)
        {
            return clientAuditService.GetDemographyHistory(contactId);
        }

        /// <summary>
        /// Gets the alias history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<AliasHistoryModel> GetAliasHistory(long contactId)
        {
            return clientAuditService.GetAliasHistory(contactId);
        }

        /// <summary>
        /// Gets the identifier history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<IdHistoryModel> GetIdHistory(long contactId)
        {
            return clientAuditService.GetIdHistory(contactId);
        }
    }
}
