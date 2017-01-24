using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.CallCenter.CallCenterSummary;
using Axis.Model.CallCenter;
using Axis.Model.Common;
using System;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.CallCenter
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Axis.DataEngine.Helpers.Controllers.BaseApiController" />
    public class CallCenterSummaryController : BaseApiController
    {
        /// <summary>
        /// The _call center summary data provider
        /// </summary>
        private readonly ICallCenterSummaryDataProvider _callCenterSummaryDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallCenterSummaryController" /> class.
        /// </summary>
        /// <param name="callCenterSummaryDataProvider">The call center summary data provider.</param>
        public CallCenterSummaryController(ICallCenterSummaryDataProvider callCenterSummaryDataProvider)
        {
            _callCenterSummaryDataProvider = callCenterSummaryDataProvider;
        }

        /// <summary>
        /// Gets the call center summary.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <param name="userID">The userID.</param>
        /// <param name="searchTypeFilter">The search type filter.</param>
        /// <param name="callStatusID">The call status identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCallCenterSummary(string searchStr, int userID, int searchTypeFilter, string callStatusID = null, int userIDFilter = 0, bool isNormalView = true)
        {
            return new HttpResult<Response<CallCenterSummaryModel>>(_callCenterSummaryDataProvider.GetCallCenterSummary(searchStr, userID, searchTypeFilter, callStatusID, userIDFilter, isNormalView), Request);
        }

        /// <summary>
        /// Gets the follow up summary.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetFollowUpSummary(long callCenterHeaderID)
        {
            return new HttpResult<Response<CallCenterSummaryModel>>(_callCenterSummaryDataProvider.GetFollowUpSummary(callCenterHeaderID), Request);
        }

        /// <summary>
        /// Gets the law liaison incident.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetLawLiaisonIncident(long callCenterHeaderID)
        {
            return new HttpResult<Response<LawLiaisonIncidentModel>>(_callCenterSummaryDataProvider.GetLawLiaisonIncident(callCenterHeaderID), Request);
        }

        /// <summary>
        /// Deletes the referral.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult Delete(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<CallCenterSummaryModel>>(_callCenterSummaryDataProvider.Delete(id, modifiedOn), Request);
        }
    }
}
