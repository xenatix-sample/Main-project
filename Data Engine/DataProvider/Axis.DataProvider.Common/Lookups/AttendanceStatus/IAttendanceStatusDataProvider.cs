using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public interface IAttendanceStatusDataProvider
    {
        /// <summary>
        /// Gets the Attendence Statuses.
        /// </summary>
        /// <returns></returns>
        Response<AttendanceStatusModel> GetAttendanceStatuses();

        /// <summary>
        /// Gets the Configured Attendence Statuses.
        /// </summary>
        /// <returns></returns>
        Response<AttendanceStatusModel> GetAttendanceStatusesConfigured();
    }
}
