using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Service.Scheduling.GroupScheduling;

namespace Axis.RuleEngine.Scheduling.GroupScheduling
{
    public class GroupSchedulingSearchRuleEngine : IGroupSchedulingSearchRuleEngine
    {
        /// <summary>
        /// The _referral search service
        /// </summary>
        private readonly IGroupSchedulingSearchService _groupSchedulingSearchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupSchedulingSearchRuleEngine"/> class.
        /// </summary>
        /// <param name="GroupSchedulingSearchService">The Group Scheduling search service.</param>
        public GroupSchedulingSearchRuleEngine(IGroupSchedulingSearchService groupSchedulingSearchService)
        {
            _groupSchedulingSearchService = groupSchedulingSearchService;
        }

        /// <summary>
        /// Gets the Group Schedules.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <returns></returns>
        public Response<GroupSchedulingSearchModel> GetGroupSchedules(string searchStr)
        {
            return _groupSchedulingSearchService.GetGroupSchedules(searchStr);
        }

        /// <summary>
        /// Cancel the Group Schedules.
        /// </summary>
        /// <param name="model">Group Scheduling search model.</param>
        /// <returns></returns>
        public Response<GroupSchedulingSearchModel> CancelGroupAppointment(GroupSchedulingSearchModel model)
        {
            return _groupSchedulingSearchService.CancelGroupAppointment(model);
        }
    }
}
