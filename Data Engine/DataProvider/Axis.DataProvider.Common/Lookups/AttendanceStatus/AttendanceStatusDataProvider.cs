using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class AttendanceStatusDataProvider : IAttendanceStatusDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttendanceStatusDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public AttendanceStatusDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the Attendance Statuses.
        /// </summary>
        /// <returns></returns>
        public Response<AttendanceStatusModel> GetAttendanceStatuses()
        {
            var repository = _unitOfWork.GetRepository<AttendanceStatusModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetAttendanceStatusDetails");

            return results;
        }

        /// <summary>
        /// Gets the Configured Attendance Statuses.
        /// </summary>
        /// <returns></returns>
        public Response<AttendanceStatusModel> GetAttendanceStatusesConfigured()
        {
            var repository = _unitOfWork.GetRepository<AttendanceStatusModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetAttendanceStatusModuleComponentDetails");

            return results;
        }

        #endregion exposed functionality
    }
}
