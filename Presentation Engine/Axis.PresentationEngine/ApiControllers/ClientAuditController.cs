using System.Web.Http;
using Axis.Model.Common;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.PresentationEngine.Repository;
using Axis.PresentationEngine.Helpers.Model;
using Axis.Model.Common.ClientAudit;
using Axis.Model.Registration;

namespace Axis.PresentationEngine.ApiControllers
{
    public class ClientAuditController : BaseApiController
    {
        /// <summary>
        /// The client audit repository
        /// </summary>
        private readonly IClientAuditRepository clientAuditRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientAuditController"/> class.
        /// </summary>
        /// <param name="clientAuditRepository">The clientAudit repository.</param>
        public ClientAuditController(IClientAuditRepository clientAuditRepository) 
        {
            this.clientAuditRepository = clientAuditRepository;
        }

        /// <summary>
        /// Adds the client audit.
        /// </summary>
        /// <param name="clientAudit">The client audit model.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ClientAuditModel> AddClientAudit(ClientAuditModel clientAudit)
        {
            return clientAuditRepository.AddClientAudit(clientAudit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientAudit"></param>
        /// <returns></returns>
        [HttpPost]
        public Response<ScreenAuditModel> AddScreenAudit(ScreenAuditModel screenAudit)
        {
            return clientAuditRepository.AddScreenAudit(screenAudit);
        }

        /// <summary>
        /// Gets the unique identifier.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetUniqueId()
        {
            return clientAuditRepository.GetUniqueId();
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
            return clientAuditRepository.GetHistoryDetails(screenId, primaryKey);
        }

        /// <summary>
        /// Gets the demography history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<DemographyHistoryModel> GetDemographyHistory(long contactId)
        {
            return clientAuditRepository.GetDemographyHistory(contactId);
        }

        /// <summary>
        /// Gets the alias history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<AliasHistoryModel> GetAliasHistory(long contactId)
        {
            return clientAuditRepository.GetAliasHistory(contactId);
        }

        /// <summary>
        /// Gets the identifier history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<IdHistoryModel> GetIdHistory(long contactId)
        {
            return clientAuditRepository.GetIdHistory(contactId);
        }

    }
}