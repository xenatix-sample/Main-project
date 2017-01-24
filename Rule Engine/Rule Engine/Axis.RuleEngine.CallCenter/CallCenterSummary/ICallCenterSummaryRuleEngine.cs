using Axis.Model.CallCenter;
using Axis.Model.Common;
using System;

namespace Axis.RuleEngine.CallCenter.CallCenterSummary
{
    /// <summary>
    ///
    /// </summary>
    public interface ICallCenterSummaryRuleEngine
    {
        /// <summary>
        /// Gets the call center summary.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <param name="userID">The userID</param>
        /// <param name="searchTypeFilter">The search type filter.</param>
        /// <param name="callStatusID">The call status identifier.</param>
        /// <returns></returns>
        Response<CallCenterSummaryModel> GetCallCenterSummary(string searchStr, int userID, int searchTypeFilter, string callStatusID, int userIDFilter, bool isNormalView);

        /// <summary>
        /// Gets the follow up summary.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        Response<CallCenterSummaryModel> GetFollowUpSummary(long callCenterHeaderID);

        /// <summary>
        /// Gets the law liaison incident.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        Response<LawLiaisonIncidentModel> GetLawLiaisonIncident(long callCenterHeaderID);

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        Response<CallCenterSummaryModel> Delete(long id, DateTime modifiedOn);
    }
}
