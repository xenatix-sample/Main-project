
using Axis.Model.Common;
using Axis.Model.Scheduling;

namespace Axis.DataProvider.Scheduling.GroupScheduling
{
    public interface IGroupSchedulingSearchDataProvider
    {
        /// <summary>
        /// Gets the Group Schedules.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <returns></returns>
        Response<GroupSchedulingSearchModel> GetGroupSchedules(string searchStr);

        /// <summary>
        /// Cancel the Group Schedules.
        /// </summary>
        /// <param name="model">Group Scheduling search model.</param>
        /// <returns></returns>
        Response<GroupSchedulingSearchModel> CancelGroupAppointment(GroupSchedulingSearchModel model);
    }
}
