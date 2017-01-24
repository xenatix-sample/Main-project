using Axis.Model.CallCenter;
using Axis.Model.Common;
using Axis.Plugins.CallCenter.Models;
using Axis.Plugins.CallCenter.Repository.LawLiaisonSummary;
using Axis.PresentationEngine.Helpers.Controllers;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Axis.Plugins.CallCenter.ApiControllers
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Axis.PresentationEngine.Helpers.Controllers.BaseApiController" />
    public class LawLiaisonSummaryController : BaseApiController
    {
        /// <summary>
        /// The _call center summary repository
        /// </summary>
        private readonly ILawLiaisonSummaryRepository _callCenterSummaryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallCenterSummaryController" /> class.
        /// </summary>
        /// <param name="callCenterSummaryRepository">The call center summary repository.</param>
        public LawLiaisonSummaryController(ILawLiaisonSummaryRepository callCenterSummaryRepository)
        {
            _callCenterSummaryRepository = callCenterSummaryRepository;
        }

        /// <summary>
        /// Gets the call center summary.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <param name="userID">The user id.</param>
        /// <param name="searchTypeFilter">The search type filter.</param>
        /// <param name="callStatusID">The call status identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<CallCenterSummaryViewModel>> GetLawLiaisonCallCenterSummary(string searchStr, int userID, int searchTypeFilter, string callStatusID = null, int userIDFilter = 0, bool isNormalView = true)
        {
            searchStr = searchStr != null ? searchStr : string.Empty;
            var result = await _callCenterSummaryRepository.GetLawLiaisonSummary(searchStr, userID, searchTypeFilter, callStatusID, userIDFilter, isNormalView);
            return result;
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<CallCenterSummaryViewModel> Delete(long id, DateTime modifiedOn)
        {
            return _callCenterSummaryRepository.Delete(id, modifiedOn);
        }

        /// <summary>
        /// Gets the law liaison incident.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<LawLiaisonIncidentModel> GetLawLiaisonIncident(long callCenterHeaderID)
        {
            return _callCenterSummaryRepository.GetLawLiaisonIncident(callCenterHeaderID);
        }
    }
}
