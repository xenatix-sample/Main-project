using Axis.Configuration;
using Axis.Model.CallCenter;
using Axis.Model.Common;
using Axis.Security;
using System;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.CallCenter.CallCenterSummary
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Axis.Service.CallCenter.CallCenterSummary.ICallCenterSummaryService" />
    public class CallCenterSummaryService : ICallCenterSummaryService
    {
        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "CallCenterSummary/";

        /// <summary>
        /// Initializes a new instance of the <see cref="CallCenterSummaryService" /> class.
        /// </summary>
        public CallCenterSummaryService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CallCenterSummaryService" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public CallCenterSummaryService(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the call center summary.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <param name="userID">The userID</param>
        /// <param name="searchTypeFilter"></param>
        /// <param name="callStatusID"></param>
        /// <returns></returns>
        public Response<CallCenterSummaryModel> GetCallCenterSummary(string searchStr, int userID, int searchTypeFilter, string callStatusID, int userIDFilter, bool isNormalView)
        {
            searchStr = string.IsNullOrWhiteSpace(searchStr) ? "" : searchStr;
            const string apiUrl = BaseRoute + "GetCallCenterSummary";
            var requestId = new NameValueCollection {
                { "searchStr", searchStr.ToString(CultureInfo.InvariantCulture) },
                { "userID", userID.ToString(CultureInfo.InvariantCulture) },
                { "searchTypeFilter", searchTypeFilter.ToString(CultureInfo.InvariantCulture) },
                { "callStatusID", !string.IsNullOrWhiteSpace(callStatusID) ? callStatusID.ToString(CultureInfo.InvariantCulture) : null },
                { "userIDFilter", userIDFilter.ToString(CultureInfo.InvariantCulture) },
                { "isNormalView", isNormalView.ToString(CultureInfo.InvariantCulture) }
            };
            return _communicationManager.Get<Response<CallCenterSummaryModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Gets the follow up summary.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        public Response<CallCenterSummaryModel> GetFollowUpSummary(long callCenterHeaderID)
        {
            const string apiUrl = BaseRoute + "GetFollowUpSummary";
            var requestId = new NameValueCollection {
                { "callCenterHeaderID", callCenterHeaderID.ToString(CultureInfo.InvariantCulture) },
            };
            return _communicationManager.Get<Response<CallCenterSummaryModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Gets the law liaison incident.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        public Response<LawLiaisonIncidentModel> GetLawLiaisonIncident(long callCenterHeaderID)
        {
            const string apiUrl = BaseRoute + "GetLawLiaisonIncident";
            var requestId = new NameValueCollection {
                { "callCenterHeaderID", callCenterHeaderID.ToString(CultureInfo.InvariantCulture) },
            };

            return _communicationManager.Get<Response<LawLiaisonIncidentModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<CallCenterSummaryModel> Delete(long id, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "Delete";
            var requestId = new NameValueCollection { { "id", id.ToString(CultureInfo.InvariantCulture) },
                                                      { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) }
                                                    };
            return _communicationManager.Delete<Response<CallCenterSummaryModel>>(requestId, apiUrl);
        }
    }
}
