using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.RuleEngine.BusinessAdmin.ClientMerge;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using System.Web.Http;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;

namespace Axis.RuleEngine.Service.Controllers
{
    public class ClientMergeController : BaseApiController
    {
        #region Class Variables

        private readonly IClientMergeRuleEngine _clientMergeRuleEngine = null;

        #endregion

        #region Constructors

        public ClientMergeController(IClientMergeRuleEngine clientMergeRuleEngine)
        {
            _clientMergeRuleEngine = clientMergeRuleEngine;
        }

        #endregion

        #region Public Methods
        [Authorization(PermissionKey = BusinessAdministrationPermissionKey.BusinessAdministration_ClientMerge_ClientMerge, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetClientMergeCounts()
        {
            return new HttpResult<Response<ClientMergeCountModel>>(_clientMergeRuleEngine.GetClientMergeCounts(), Request);
        }

        /// <summary>
        /// Gets the potential matches.
        /// </summary>
        /// <returns></returns>
        [Authorization(PermissionKey = BusinessAdministrationPermissionKey.BusinessAdministration_ClientMerge_ClientMerge, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetPotentialMatches()
        {
            return new HttpResult<Response<PotentialMatchesModel>>(_clientMergeRuleEngine.GetPotentialMatches(), Request);
        }

        [Authorization(PermissionKey = BusinessAdministrationPermissionKey.BusinessAdministration_ClientMerge_ClientMerge, Permission = Permission.Read)]
        [HttpPost]
        public IHttpActionResult MergeRecords(ClientMergeModel clientMerge)
        {
            var clientMergeResponse = _clientMergeRuleEngine.MergeRecords(clientMerge);
            return new HttpResult<Response<ClientMergeModel>>(clientMergeResponse, Request);
        }

        /// <summary>
        /// Gets the merged records.
        /// </summary>
        /// <returns></returns>
        [Authorization(PermissionKey = BusinessAdministrationPermissionKey.BusinessAdministration_ClientMerge_ClientMerge, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetMergedRecords()
        {
            return new HttpResult<Response<MergeContactModel>>(_clientMergeRuleEngine.GetMergedRecords(), Request);
        }

        /// <summary>
        /// Un merge contact
        /// </summary>
        /// <param name="mergedContactsMappingID"></param>
        /// <returns></returns>
        [Authorization(PermissionKey = BusinessAdministrationPermissionKey.BusinessAdministration_ClientMerge_ClientMerge, Permission = Permission.Read)]
        [HttpPost]
        public IHttpActionResult UnMergeRecords(long mergedContactsMappingID)
        {
            var clientUnMergeResponse = _clientMergeRuleEngine.UnMergeRecords(mergedContactsMappingID);
            return new HttpResult<Response<MergeContactModel>>(clientUnMergeResponse, Request);
        }

        /// <summary>
        /// Get refresh potentialMatches
        /// </summary>
        /// <returns></returns>
        [Authorization(PermissionKey = BusinessAdministrationPermissionKey.BusinessAdministration_ClientMerge_ClientMerge, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetRefreshPotentialMatches()
        {
            var potentialMatchesResponse = _clientMergeRuleEngine.GetRefreshPotentialMatches();
            return new HttpResult<Response<PotentialMatchesModel>>(potentialMatchesResponse, Request);
        }

        /// <summary>
        /// Gets potential matches last run.
        /// </summary>
        /// <returns></returns>
        [Authorization(PermissionKey = BusinessAdministrationPermissionKey.BusinessAdministration_ClientMerge_ClientMerge, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetPotentialMergeContactsLastRun()
        {
            var potentialMatchesLastRunResponse = _clientMergeRuleEngine.GetPotentialMergeContactsLastRun();
            return new HttpResult<Response<PotentialMergeContactsLastRunModel>>(potentialMatchesLastRunResponse, Request);
        }
        #endregion
    }
}