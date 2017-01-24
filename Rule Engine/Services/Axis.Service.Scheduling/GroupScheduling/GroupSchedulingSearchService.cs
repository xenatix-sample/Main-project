using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Scheduling.GroupScheduling
{
    public class GroupSchedulingSearchService : IGroupSchedulingSearchService
    {
        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;
        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "GroupSchedulingSearch/";

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupSchedulingSearchService"/> class.
        /// </summary>
        public GroupSchedulingSearchService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupSchedulingSearchService"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public GroupSchedulingSearchService(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the Group Schedules.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <returns></returns>
        public Response<GroupSchedulingSearchModel> GetGroupSchedules(string searchStr)
        {
            searchStr = string.IsNullOrWhiteSpace(searchStr) ? "" : searchStr;
            const string apiUrl = BaseRoute + "GetGroupSchedules";
            var requestId = new NameValueCollection { { "searchStr", searchStr.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<GroupSchedulingSearchModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Cancel the Group Schedules.
        /// </summary>
        /// <param name="model">Group Scheduling search model.</param>
        /// <returns></returns>
        public Response<GroupSchedulingSearchModel> CancelGroupAppointment(GroupSchedulingSearchModel model)
        {
            const string apiUrl = BaseRoute + "CancelGroupAppointment";
            return _communicationManager.Put<GroupSchedulingSearchModel, Response<GroupSchedulingSearchModel>>(model, apiUrl);
        }
    }
}
