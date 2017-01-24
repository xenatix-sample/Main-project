

using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Scheduling.GroupScheduling;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Scheduling
{
    public class GroupSchedulingSearchController : BaseApiController
    {
        /// <summary>
        /// The GroupScheduling search data provider
        /// </summary>
        private readonly IGroupSchedulingSearchDataProvider _groupSchedulingSearchDataProvider;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="IGroupSchedulingSearchDataProvider"></param>
        public GroupSchedulingSearchController(IGroupSchedulingSearchDataProvider groupSchedulingSearchDataProvider)
        {
            _groupSchedulingSearchDataProvider = groupSchedulingSearchDataProvider;
        }

        /// <summary>
        /// Gets the referrals.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetGroupSchedules(string searchStr)
        {
            return new HttpResult<Response<GroupSchedulingSearchModel>>(_groupSchedulingSearchDataProvider.GetGroupSchedules(searchStr), Request);
        }

        /// <summary>
        /// Cancel the Group Schedules.
        /// </summary>
        /// <param name="model">Group Scheduling search model.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult CancelGroupAppointment(GroupSchedulingSearchModel model)
        {
            return new HttpResult<Response<GroupSchedulingSearchModel>>(_groupSchedulingSearchDataProvider.CancelGroupAppointment(model), Request);
        }
    }
}
