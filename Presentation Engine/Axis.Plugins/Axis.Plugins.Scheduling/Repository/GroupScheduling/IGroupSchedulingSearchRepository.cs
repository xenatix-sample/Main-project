using Axis.Model.Common;
using Axis.Model.Scheduling;
using System.Threading.Tasks;

namespace Axis.Plugins.Scheduling.Repository.GroupScheduling
{
    public interface IGroupSchedulingSearchRepository
    {
        /// <summary>
        /// Gets the Group Schedules based on search string.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <returns></returns>
        Task<Response<GroupSchedulingSearchModel>> GetGroupSchedulesAsync(string searchStr);

        /// <summary>
        /// Cancels the whole GroupAppointment.
        /// </summary>
        /// <param name="model">AppointmentCancel Model.</param>
        /// <returns></returns>
        Response<GroupSchedulingSearchModel> CancelGroupAppointment(GroupSchedulingSearchModel model);
    }
}
