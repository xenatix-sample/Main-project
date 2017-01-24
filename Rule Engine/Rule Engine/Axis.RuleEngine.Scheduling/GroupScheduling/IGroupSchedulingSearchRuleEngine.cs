
using Axis.Model.Common;
using Axis.Model.Scheduling;

namespace Axis.RuleEngine.Scheduling.GroupScheduling
{
    public interface IGroupSchedulingSearchRuleEngine
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
