using System;
using Axis.Model.Common;
using Axis.Model.Scheduling;
using Axis.Data.Repository;
using System.Collections.Generic;
using System.Data.SqlClient;
using Axis.Data.Repository.Schema;

namespace Axis.DataProvider.Scheduling.GroupScheduling
{
    public class GroupSchedulingSearchDataProvider : IGroupSchedulingSearchDataProvider
    {
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        public GroupSchedulingSearchDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupSchedulingSearchDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public Response<GroupSchedulingSearchModel> GetGroupSchedules(string searchStr)
        {
            var sprocName = "usp_GetGroupSchedulingSearchDetails";
            var procParams = new List<SqlParameter>();
            procParams.Add(new SqlParameter("GroupName", searchStr));

            
            var groupScheduleSearchRepository = _unitOfWork.GetRepository<GroupSchedulingSearchModel>(SchemaName.Scheduling);
            return groupScheduleSearchRepository.ExecuteStoredProc(sprocName, procParams);
        }

        /// <summary>
        /// Cancel the Group Schedules.
        /// </summary>
        /// <param name="model">Group Scheduling search model.</param>
        /// <returns></returns>
        public Response<GroupSchedulingSearchModel> CancelGroupAppointment(GroupSchedulingSearchModel model)
        {
            var groupScheduleSearchRepository = _unitOfWork.GetRepository<GroupSchedulingSearchModel>(SchemaName.Scheduling);
            if (model.IsCancelAllAppoitment)
                model.AppointmentID = null;

            var procParams = BuildParams(model);

            return _unitOfWork.EnsureInTransaction(
                    groupScheduleSearchRepository.ExecuteNQStoredProc,
                    "usp_CancelAppointmentsForGroup",
                    procParams,
                    forceRollback: model.ForceRollback.GetValueOrDefault(false)
                );
        }

        #region Private Methods

        private List<SqlParameter> BuildParams(GroupSchedulingSearchModel model)
        {
            var spParameters = new List<SqlParameter>();

            spParameters.AddRange(new List<SqlParameter>{
                new SqlParameter("GroupDetailID", model.GroupDetailID),
                new SqlParameter("CancelReasonID", model.CancelReasonID),
                new SqlParameter("CancelComment", (object)model.CancelComment ?? DBNull.Value),
                new SqlParameter("AppointmentID", (object)model.AppointmentID ?? DBNull.Value),
                new SqlParameter("ModifiedOn", (object)model.ModifiedOn ?? DateTime.Now)
            });
            return spParameters;
        }

        #endregion Private Methods
    }
}
