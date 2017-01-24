using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Scheduling.GroupScheduling;
using System.Web.Http;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Scheduling
{
    public class GroupSchedulingSearchController : BaseApiController
    {
        /// <summary>
        /// The GroupScheduling search rule engine
        /// </summary>
        private readonly IGroupSchedulingSearchRuleEngine _groupSchedulingSearchRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupSchedulingSearchController"/> class.
        /// </summary>
        /// <param name="IGroupSchedulingSearchRuleEngine">The GroupScheduling search rule engine.</param>
        public GroupSchedulingSearchController(IGroupSchedulingSearchRuleEngine groupSchedulingSearchRuleEngine)
        {
            _groupSchedulingSearchRuleEngine = groupSchedulingSearchRuleEngine;
        }

        /// <summary>
        /// Gets the Group Schedules.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = SchedulingPermissionKey.Scheduling_Appointment_GroupAppointment, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetGroupSchedules(string searchStr)
        {
            return new HttpResult<Response<GroupSchedulingSearchModel>>(_groupSchedulingSearchRuleEngine.GetGroupSchedules(searchStr), Request);
        }

        /// <summary>
        /// Cancel the Group Schedules.
        /// </summary>
        /// <param name="model">Group Scheduling search model.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = SchedulingPermissionKey.Scheduling_Appointment_GroupAppointment, Permission = Permission.Delete)]
        [HttpPut]
        public IHttpActionResult CancelGroupAppointment(GroupSchedulingSearchModel model)
        {
            return new HttpResult<Response<GroupSchedulingSearchModel>>(_groupSchedulingSearchRuleEngine.CancelGroupAppointment(model), Request);
        }
    }
}
