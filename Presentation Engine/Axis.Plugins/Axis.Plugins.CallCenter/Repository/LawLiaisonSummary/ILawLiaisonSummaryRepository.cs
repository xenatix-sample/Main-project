using Axis.Model.CallCenter;
using Axis.Model.Common;
using Axis.Plugins.CallCenter.Models;
using System;
using System.Threading.Tasks;

namespace Axis.Plugins.CallCenter.Repository.LawLiaisonSummary
{
    /// <summary>
    /// Repository for call center summary
    /// </summary>
    public interface ILawLiaisonSummaryRepository
    {
        /// <summary>
        /// Gets the call center summary.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <param name="userID">The userID</param>
        /// <param name="searchTypeFilter">The search type filter.</param>
        /// <param name="callStatusID">The call status identifier.</param>
        /// <returns></returns>
        Task<Response<CallCenterSummaryViewModel>> GetLawLiaisonSummary(string searchStr, int userID, int searchTypeFilter, string callStatusID, int userIDFilter, bool isNormalView);

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        Response<CallCenterSummaryViewModel> Delete(long id, DateTime modifiedOn);

        /// <summary>
        /// Gets the law liaison incident.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        Response<LawLiaisonIncidentModel> GetLawLiaisonIncident(long callCenterHeaderID);
    }
}
