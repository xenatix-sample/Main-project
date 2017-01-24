using Axis.Constant;
using Axis.Model.CallCenter;
using Axis.Model.Common;
using Axis.RuleEngine.CallCenter.CallCenterSummary;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Axis.RuleEngine.Plugins.CallCenter
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CallCenterSummaryController : ApiController
    {
        /// <summary>
        /// The _call center summary rule engine
        /// </summary>
        private readonly ICallCenterSummaryRuleEngine _callCenterSummaryRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallCenterSummaryController" /> class.
        /// </summary>
        /// <param name="callCenterSummaryRuleEngine">The call center summary rule engine.</param>
        public CallCenterSummaryController(ICallCenterSummaryRuleEngine callCenterSummaryRuleEngine)
        {
            _callCenterSummaryRuleEngine = callCenterSummaryRuleEngine;
        }

        /// <summary>
        /// Gets the call center summary.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <param name="userID">The user identifier.</param>
        /// <param name="searchTypeFilter">The search type filter.</param>
        /// <param name="callStatusID">The call status identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { CallCenterPermissionKey.CallCenter_CrisisLine, CallCenterPermissionKey.CallCenter_LawLiaison, RegistrationPermissionKey.Registration_Demography }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetCallCenterSummary(string searchStr, int userID, int searchTypeFilter, string callStatusID = null, int userIDFilter = 0, bool isNormalView = true)
        {
            return new HttpResult<Response<CallCenterSummaryModel>>(_callCenterSummaryRuleEngine.GetCallCenterSummary(searchStr, userID, searchTypeFilter, callStatusID, userIDFilter, isNormalView), Request);
        }

        /// <summary>
        /// Gets the follow up summary.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = CallCenterPermissionKey.CallCenter_CrisisLine, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetFollowUpSummary(long callCenterHeaderID)
        {
            return new HttpResult<Response<CallCenterSummaryModel>>(_callCenterSummaryRuleEngine.GetFollowUpSummary(callCenterHeaderID), Request);
        }

        /// <summary>
        /// Gets the law liaison incident.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = CallCenterPermissionKey.CallCenter_LawLiaison, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetLawLiaisonIncident(long callCenterHeaderID)
        {
            return new HttpResult<Response<LawLiaisonIncidentModel>>(_callCenterSummaryRuleEngine.GetLawLiaisonIncident(callCenterHeaderID), Request);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { CallCenterPermissionKey.CallCenter_CrisisLine, CallCenterPermissionKey.CallCenter_LawLiaison, RegistrationPermissionKey.Registration_Demography }, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult Delete(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<CallCenterSummaryModel>>(_callCenterSummaryRuleEngine.Delete(id, modifiedOn), Request);
        }
    }
}
