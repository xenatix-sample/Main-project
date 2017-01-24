using Axis.Model.CallCenter;
using Axis.Model.Common;
using Axis.Service.CallCenter.CallCenterSummary;
using System;

namespace Axis.RuleEngine.CallCenter.CallCenterSummary
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Axis.RuleEngine.CallCenter.CallCenterSummary.ICallCenterSummaryRuleEngine" />
    public class CallCenterSummaryRuleEngine : ICallCenterSummaryRuleEngine
    {
        /// <summary>
        /// The _call center summary service
        /// </summary>
        private readonly ICallCenterSummaryService _callCenterSummaryService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallCenterSummaryRuleEngine" /> class.
        /// </summary>
        /// <param name="callCenterSummaryService">The call center summary service.</param>
        public CallCenterSummaryRuleEngine(ICallCenterSummaryService callCenterSummaryService)
        {
            _callCenterSummaryService = callCenterSummaryService;
        }

        /// <summary>
        /// Gets the call center summary.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <param name="userID">The user id.</param>
        /// <param name="searchTypeFilter"></param>
        /// <param name="callStatusID"></param>
        /// <returns></returns>
        public Response<CallCenterSummaryModel> GetCallCenterSummary(string searchStr, int userID, int searchTypeFilter, string callStatusID, int userIDFilter, bool isNormalView)
        {
            return _callCenterSummaryService.GetCallCenterSummary(searchStr, userID, searchTypeFilter, callStatusID, userIDFilter, isNormalView);
        }

        /// <summary>
        /// Gets the law liaison incident.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        public Response<LawLiaisonIncidentModel> GetLawLiaisonIncident(long callCenterHeaderID)
        {
            return _callCenterSummaryService.GetLawLiaisonIncident(callCenterHeaderID);
        }

        /// <summary>
        /// Gets the follow up summary.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        public Response<CallCenterSummaryModel> GetFollowUpSummary(long callCenterHeaderID)
        {
            return _callCenterSummaryService.GetFollowUpSummary(callCenterHeaderID);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<CallCenterSummaryModel> Delete(long id, DateTime modifiedOn)
        {
            return _callCenterSummaryService.Delete(id, modifiedOn);
        }
    }
}
