using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.CallCenter;
using Axis.Model.Common;
using Axis.Plugins.CallCenter.Models;
using Axis.Plugins.CallCenter.Translator;
using Axis.Service;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;


namespace Axis.Plugins.CallCenter.Repository.CrisisLineSummary
{
    public class CrisisLineSummaryRepository : ICrisisLineSummaryRepository
    {
        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "CallCenterSummary/";

        /// <summary>
        /// Initializes a new instance of the <see cref="CallCenterSummaryRepository"/> class.
        /// </summary>
        public CrisisLineSummaryRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CrisisLineSummaryRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public CrisisLineSummaryRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the call center summary.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <param name="searchStr">The search type filter.</param>
        /// <param name="userID">The userID</param>
        /// <returns></returns>
        public async Task<Response<CallCenterSummaryViewModel>> GetCrisisLineSummary(string searchStr, int userID, int searchTypeFilter, string callStatusID, int userIDFilter, bool isNormalView)
        {
            const string apiUrl = baseRoute + "GetCallCenterSummary";
            var param = new NameValueCollection {
                { "searchStr", searchStr.ToString(CultureInfo.InvariantCulture) },
                { "userID", userID.ToString(CultureInfo.InvariantCulture) },
                { "searchTypeFilter", searchTypeFilter.ToString(CultureInfo.InvariantCulture) },
                { "callStatusID", !string.IsNullOrWhiteSpace(callStatusID)  ? callStatusID.ToString(CultureInfo.InvariantCulture) : null },
                { "userIDFilter", userIDFilter.ToString(CultureInfo.InvariantCulture) },
                { "isNormalView", isNormalView.ToString(CultureInfo.InvariantCulture) }
            };
            var response = await _communicationManager.GetAsync<Response<CallCenterSummaryModel>>(param, apiUrl);
            return response.ToViewModel();
        }


        public Response<CallCenterSummaryViewModel> GetFollowUpSummary(long callCenterHeaderID)
        {
            const string apiUrl = baseRoute + "GetFollowUpSummary";
            var param = new NameValueCollection {

                { "callCenterHeaderID", callCenterHeaderID.ToString(CultureInfo.InvariantCulture) }

            };
            var response = _communicationManager.Get<Response<CallCenterSummaryModel>>(param, apiUrl).ToViewModel();
            return response;
        }
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<CallCenterSummaryViewModel> Delete(long id, DateTime modifiedOn)
        {
            const string apiUrl = baseRoute + "Delete";
            var requestId = new NameValueCollection { { "id", id.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            var response = _communicationManager.Delete<Response<CallCenterSummaryModel>>(requestId, apiUrl);
            return response.ToViewModel();
        }
    }
}
