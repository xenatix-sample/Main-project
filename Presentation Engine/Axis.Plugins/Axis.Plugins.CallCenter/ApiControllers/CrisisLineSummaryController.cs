using Axis.Model.Common;
using Axis.Plugins.CallCenter.Models;
using Axis.Plugins.CallCenter.Repository.CrisisLineSummary;
using Axis.PresentationEngine.Helpers.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Axis.Plugins.CallCenter.ApiControllers
{
    public class CrisisLineSummaryController : BaseApiController
    {
        /// <summary>
        /// The _call center summary repository
        /// </summary>
        private readonly ICrisisLineSummaryRepository _callCenterSummaryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallCenterSummaryController"/> class.
        /// </summary>
        /// <param name="callCenterSummaryRepository">The call center summary repository.</param>
        public CrisisLineSummaryController(ICrisisLineSummaryRepository callCenterSummaryRepository)
        {
            _callCenterSummaryRepository = callCenterSummaryRepository;
        }

        [HttpGet]

        public async Task<Response<CallCenterSummaryViewModel>> GetCrisisLineCallCenterSummary(string searchStr, int userID, int searchTypeFilter, string callStatusID = null, int userIDFilter = 0, bool isNormalView = true)
        {
            searchStr = searchStr != null ? searchStr : string.Empty;
            var result = await _callCenterSummaryRepository.GetCrisisLineSummary(searchStr, userID, searchTypeFilter, callStatusID, userIDFilter, isNormalView);
            return result;
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<CallCenterSummaryViewModel> Delete(long id, DateTime modifiedOn)
        {
            return _callCenterSummaryRepository.Delete(id, modifiedOn);
        }

        [HttpGet]
        public Response<CallCenterSummaryViewModel> GetFollowUpSummary(long callCenterHeaderID)
        {
            return _callCenterSummaryRepository.GetFollowUpSummary(callCenterHeaderID);

        }

    }
}
