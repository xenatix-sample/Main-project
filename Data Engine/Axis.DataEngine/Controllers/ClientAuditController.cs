using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Common.ClientAudit;
using Axis.Model.Common.ClientAudit;
using System.Web.Http;
using Axis.Model.Registration;
using Axis.Model.Common;
using System;

namespace Axis.DataEngine.Service.Controllers
{
    public class ClientAuditController : BaseApiController
    {
        /// <summary>
        /// The ClientAudit data provider
        /// </summary>
        private readonly IClientAuditDataProvider clientAuditDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientAuditController"/> class.
        /// </summary>
        /// <param name="ClientAudit">The client audit data provider.</param>
        public ClientAuditController(IClientAuditDataProvider clientAuditDataProvider)
        {
            this.clientAuditDataProvider = clientAuditDataProvider;
        }

        /// <summary>
        /// Adds the client Audit log.
        /// </summary>
        /// <param name="clientAudit">The client Audit model.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddClientAudit(ClientAuditModel clientAudit)
        {
            return new HttpResult<Response<ClientAuditModel>>(clientAuditDataProvider.AddClientAudit(clientAudit), Request);
        }

        [HttpPost]
        public IHttpActionResult AddScreenAudit(ScreenAuditModel screenAudit)
        {
            //check if datakey is for client merge, then process following method for each MRN. eg: ParentMRN:12,ChildMRN:11|BusinessAdministration-ClientMerge-ClientMerge
            if (screenAudit.DataKey.Contains("BusinessAdministration-ClientMerge-ClientMerge") && screenAudit.DataKey.Contains("ParentMRN") && screenAudit.DataKey.Contains("ChildMRN"))
            {
                var dataToParse = screenAudit.DataKey;
                screenAudit.DataKey = dataToParse.Split('|')[1];
                var MRNs = dataToParse.Split('|')[0].Split(',');
                var index = 0;
                foreach(var mrn in MRNs)
                {
                    int contactID = 0;
                    screenAudit.ContactID = ( int.TryParse(mrn.Split(':')[1], out contactID)) ? contactID : 0;
                    index++;
                    if (index == 1)
                    {
                        new HttpResult<Response<ScreenAuditModel>>(clientAuditDataProvider.AddScreenAudit(screenAudit), Request);
                    }                 
                    else if (index == 2)
                    {
                        return new HttpResult<Response<ScreenAuditModel>>(clientAuditDataProvider.AddScreenAudit(screenAudit), Request);
                    }                     

                }
                return null;
            }
            else
            {
                return new HttpResult<Response<ScreenAuditModel>>(clientAuditDataProvider.AddScreenAudit(screenAudit), Request);
            }
           
        }

        [HttpGet]
        public string GetUniqueId()
        {
            return clientAuditDataProvider.GetUniqueId();
        }

        [HttpGet]
        public string GetHistoryDetails(long screenId, long primaryKey)
        {
            return clientAuditDataProvider.GetHistoryDetails(screenId, primaryKey);
        }

        /// <summary>
        /// Gets the demography history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetDemographyHistory(long contactId)
        {
            return new HttpResult<Response<DemographyHistoryModel>>(clientAuditDataProvider.GetDemographyHistory(contactId), Request);
        }

        /// <summary>
        /// Gets the alias history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAliasHistory(long contactId)
        {
            return new HttpResult<Response<AliasHistoryModel>>(clientAuditDataProvider.GetAliasHistory(contactId), Request);
        }

        /// <summary>
        /// Gets the identifier history.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetIdHistory(long contactId)
        {
            return new HttpResult<Response<IdHistoryModel>>(clientAuditDataProvider.GetIdHistory(contactId), Request);
        }
    }
}