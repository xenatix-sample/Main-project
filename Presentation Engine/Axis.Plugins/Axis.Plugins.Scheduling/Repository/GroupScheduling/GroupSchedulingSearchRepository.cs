using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Service;
using System.Threading.Tasks;
using System.Collections.Generic;
using Axis.Model.Scheduling;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Plugins.Scheduling.Repository.GroupScheduling
{
    public class GroupSchedulingSearchRepository : IGroupSchedulingSearchRepository
    {
        private readonly CommunicationManager _communicationManager;
        private const string baseRoute = "GroupSchedulingSearch/";

        public GroupSchedulingSearchRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Gets the Group Schedules based on search string.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <returns></returns>
        public async Task<Response<GroupSchedulingSearchModel>> GetGroupSchedulesAsync(string searchStr)
        {
            const string apiUrl = baseRoute + "GetGroupSchedules";
            var param = new NameValueCollection { { "searchStr", searchStr.ToString(CultureInfo.InvariantCulture) } };
            var response = await _communicationManager.GetAsync<Response<GroupSchedulingSearchModel>>(param, apiUrl);
            return response;
        }

        /// <summary>
        /// Cancels the whole GroupAppointment.
        /// </summary>
        /// <param name="model">AppointmentCancel Model.</param>
        /// <returns></returns>
        public Response<GroupSchedulingSearchModel> CancelGroupAppointment(GroupSchedulingSearchModel model)
        {
            const string apiUrl = baseRoute + "CancelGroupAppointment";
            return _communicationManager.Put<GroupSchedulingSearchModel, Response<GroupSchedulingSearchModel>>(model, apiUrl);
        }

    }
}
