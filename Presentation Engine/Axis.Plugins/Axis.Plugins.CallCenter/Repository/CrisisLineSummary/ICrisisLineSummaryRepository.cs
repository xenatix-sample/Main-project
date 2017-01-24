using Axis.Model.Common;
using Axis.Plugins.CallCenter.Models;
using System;
using System.Threading.Tasks;

namespace Axis.Plugins.CallCenter.Repository.CrisisLineSummary
{
    /// <summary>
    /// Repository for call center summary
    /// </summary>
    public interface ICrisisLineSummaryRepository
    {
        /// <summary>
        /// Gets the call center summary.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <param name="searchStr">The search type filter.</param>
        /// <param name="userID">The userID</param>
        /// <returns></returns>
        Task<Response<CallCenterSummaryViewModel>> GetCrisisLineSummary(string searchStr, int userID, int searchTypeFilter, string callStatusID, int userIDFilter, bool isNormalView);

        Response<CallCenterSummaryViewModel> GetFollowUpSummary(long callCenterHeaderID);

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        Response<CallCenterSummaryViewModel> Delete(long id, DateTime modifiedOn);
    }
}
