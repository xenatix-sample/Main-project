using Axis.Model.Common;
using Axis.Model.Common.ClientAudit;
using Axis.Model.Registration;
using Axis.RuleEngine.Common.ClientAudit;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.Security;
using System.Web.Http;

namespace Axis.RuleEngine.Service.Controllers
{
    public class ClientAuditController : BaseApiController
    {
        /// <summary>
        /// The client audit rule engine
        /// </summary>
        private readonly IClientAuditRuleEngine clientAuditRuleEngine = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientAuditController"/> class.
        /// </summary>
        /// <param name="clientAuditRuleEngine">The client audit rule engine.</param>
        public ClientAuditController(IClientAuditRuleEngine clientAuditRuleEngine)
        {
            this.clientAuditRuleEngine = clientAuditRuleEngine;
        }

        /// <summary>
        /// Adds the client audit.
        /// </summary>
        /// <param name="photo">The client audit model.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddClientAudit(ClientAuditModel clientAudit)
        {
            clientAudit.CreatedBy = AuthContext.Auth.User.UserID;
            return new HttpResult<Response<ClientAuditModel>>(clientAuditRuleEngine.AddClientAudit(clientAudit), Request);
        }

        [HttpPost]
        public IHttpActionResult AddScreenAudit(ScreenAuditModel screenAudit)
        {
            return new HttpResult<Response<ScreenAuditModel>>(clientAuditRuleEngine.AddScreenAudit(screenAudit), Request);
        }

        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetUniqueId()
        {
            return clientAuditRuleEngine.GetUniqueId();
        }

        /// <summary>
        /// Gets the history details.
        /// </summary>
        /// <param name="screenId">The screen identifier.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns></returns>
        [HttpGet]
        public string GetHistoryDetails(long screenId, long primaryKey)
        {
            return clientAuditRuleEngine.GetHistoryDetails(screenId, primaryKey);
        }

        /// <summary>
        /// Gets the demography history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetDemographyHistory(long contactId)
        {
            return new HttpResult<Response<DemographyHistoryModel>>(clientAuditRuleEngine.GetDemographyHistory(contactId), Request);
        }

        /// <summary>
        /// Gets the alias history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAliasHistory(long contactId)
        {
            return new HttpResult<Response<AliasHistoryModel>>(clientAuditRuleEngine.GetAliasHistory(contactId), Request);
        }

        /// <summary>
        /// Gets the identifier history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetIdHistory(long contactId)
        {
            return new HttpResult<Response<IdHistoryModel>>(clientAuditRuleEngine.GetIdHistory(contactId), Request);
        }
    }
}