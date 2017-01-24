using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.DataProvider.CallCenter
{
    public class CallCenterProgressNoteDataProvider : ICallCenterProgressNoteDataProvider
    {
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallCenterProgressNoteDataProvider" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public CallCenterProgressNoteDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the call center progress note.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteModel> GetCallCenterProgressNote(long callCenterHeaderID)
        {
            var callCenterRepository = _unitOfWork.GetRepository<CallCenterProgressNoteModel>(SchemaName.CallCenter);
            var procParams = new List<SqlParameter> { new SqlParameter("CallCenterHeaderID", callCenterHeaderID) };
            return callCenterRepository.ExecuteStoredProc("usp_GetCrisisLineProgressNote", procParams);
        }

        /// <summary>
        /// Adds the caller information.
        /// </summary>
        /// <param name="callCenterProgressNode">The call center progress note.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteModel> AddCallCenterProgressNote(CallCenterProgressNoteModel callCenterProgressNode)
        {
            var callCenterRepository = _unitOfWork.GetRepository<CallCenterProgressNoteModel>(SchemaName.CallCenter);
            var procParams = BuildParams(callCenterProgressNode);

            return _unitOfWork.EnsureInTransaction(
                    callCenterRepository.ExecuteNQStoredProc,
                    "usp_AddCrisisLineProgressNote",
                    procParams,
                    forceRollback: callCenterProgressNode.ForceRollback.GetValueOrDefault(false)
                );
        }

        /// <summary>
        /// Updates the caller information.
        /// </summary>
        /// <param name="callCenterProgressNode">The call center progress note.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteModel> UpdateCallCenterProgressNote(CallCenterProgressNoteModel callCenterProgressNode)
        {
            var callCenterRepository = _unitOfWork.GetRepository<CallCenterProgressNoteModel>(SchemaName.CallCenter);
            var procParams = BuildParams(callCenterProgressNode);

            return _unitOfWork.EnsureInTransaction(
                    callCenterRepository.ExecuteNQStoredProc,
                    "usp_AddCrisisLineProgressNote",
                    procParams,
                    forceRollback: callCenterProgressNode.ForceRollback.GetValueOrDefault(false)
                );
        }


        private List<SqlParameter> BuildParams(CallCenterProgressNoteModel callCenterProgressNode)
        {
            var spParameters = new List<SqlParameter>();

            spParameters.AddRange(new List<SqlParameter>{
                new SqlParameter("CallCenterHeaderID", callCenterProgressNode.CallCenterHeaderID),
                new SqlParameter("NatureofCall", (object)callCenterProgressNode.NatureofCall ?? DBNull.Value),
                new SqlParameter("CallTypeID", (object)callCenterProgressNode.CallTypeID ?? DBNull.Value),
                new SqlParameter("CallTypeOther", (object)callCenterProgressNode.CallTypeOther ?? DBNull.Value),
                new SqlParameter("FollowupPlan", (object)callCenterProgressNode.FollowupPlan ?? DBNull.Value),
                new SqlParameter("Comments", (object)callCenterProgressNode.Comments ?? DBNull.Value),
                new SqlParameter("BehavioralCategoryID", (object)callCenterProgressNode.BehavioralCategoryID ?? DBNull.Value),
                new SqlParameter("ClientStatusID", (object)callCenterProgressNode.ClientStatusID ?? DBNull.Value),
                new SqlParameter("ClientProviderID", (object)callCenterProgressNode.ClientProviderID ?? DBNull.Value),
                new SqlParameter("Disposition", (object)callCenterProgressNode.Disposition ?? DBNull.Value),
                new SqlParameter("CallStartTime", (object)callCenterProgressNode.CallStartTime ?? DBNull.Value),
                new SqlParameter("CallEndTime", (object)callCenterProgressNode.CallEndTime ?? DBNull.Value),
                new SqlParameter("ModifiedOn", (object)callCenterProgressNode.ModifiedOn ?? DBNull.Value),
                new SqlParameter("IsCallerSame", (object)callCenterProgressNode.IsCallerSame ?? DBNull.Value),
                new SqlParameter("NewCallerID", (object)callCenterProgressNode.NewCallerID ?? DBNull.Value)
            });
            return spParameters;
        }

    }
}
