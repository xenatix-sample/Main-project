using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Plugins.Scheduling.Models;
using Axis.Plugins.Scheduling.Repository.GroupScheduling;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Threading.Tasks;

namespace Axis.Plugins.Scheduling.ApiControllers
{
    public class GroupSchedulingSearchController : BaseApiController
    {
        /// <summary>
        /// The group scheduling search repository
        /// </summary>
        private readonly IGroupSchedulingSearchRepository _groupSchedulingSearchRepository = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupSchedulingSearchController"/> class.
        /// </summary>
        /// <param name="GroupSchedulingSearchController">The group scheduling search repository.</param>
        public GroupSchedulingSearchController(IGroupSchedulingSearchRepository groupSchedulingSearchRepository)
        {
            _groupSchedulingSearchRepository = groupSchedulingSearchRepository;
        }

        /// <summary>
        /// Gets the Group Schedules based on search string.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <returns></returns>
        public async Task<Response<GroupSchedulingSearchModel>> GetGroupSchedules(string searchStr)
        {
            searchStr = searchStr != null ? searchStr : string.Empty;
            var result = await _groupSchedulingSearchRepository.GetGroupSchedulesAsync(searchStr);
            return result;
        }

        /// <summary>
        /// Cancels the whole GroupAppointment.
        /// </summary>
        /// <param name="model">AppointmentCancel Model.</param>
        /// <returns></returns>
        public Response<GroupSchedulingSearchModel> CancelGroupAppointment(GroupSchedulingSearchModel model)
        {
            return _groupSchedulingSearchRepository.CancelGroupAppointment(model);
        }
    }
}
