using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.ECI;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.ECI
{
    public class DischargeDataProvider : IDischargeDataProvider
    {
       /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        public DischargeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the dischages.
        /// </summary>
        /// <param name="progressNoteID">The progress note identifier.</param>
        /// <returns></returns>
        public Response<DischargeModel> GetDischages(long contactID, int noteTypeID)
        {
            var dischargeRepository = _unitOfWork.GetRepository<DischargeModel>(SchemaName.ECI);
            List<SqlParameter> procParams = new List<SqlParameter> { new SqlParameter("ContactID", contactID),
                                                                     new SqlParameter("NoteTypeID", noteTypeID)
                                                                    };
            return dischargeRepository.ExecuteStoredProc("usp_GetDischarges", procParams);
        }

        /// <summary>
        /// Gets the dischage.
        /// </summary>
        /// <param name="noteHeaderId">The note header identifier.</param>
        /// <returns></returns>
        public Response<DischargeModel> GetDischage(long dischargeID)
        {
            var dischargeRepository = _unitOfWork.GetRepository<DischargeModel>(SchemaName.ECI);
            List<SqlParameter> procParams = new List<SqlParameter> { new SqlParameter("DischargeID", dischargeID) };
            return dischargeRepository.ExecuteStoredProc("usp_GetDischarge", procParams);
        }

        /// <summary>
        /// Adds the dischage.
        /// </summary>
        /// <param name="discharge">The discharge.</param>
        /// <returns></returns>
        public Response<DischargeModel> AddDischage(DischargeModel discharge)
        {
            var dischargeRepository = _unitOfWork.GetRepository<DischargeModel>(SchemaName.ECI);
            var procParams = BuildSpParams(discharge);
            return _unitOfWork.EnsureInTransaction(dischargeRepository.ExecuteNQStoredProc, "usp_AddDischarge", procParams, idResult: true,
                            forceRollback: discharge.ForceRollback.GetValueOrDefault(false));      
        }

        /// <summary>
        /// Updates the dischage.
        /// </summary>
        /// <param name="discharge">The discharge.</param>
        /// <returns></returns>
        public Response<DischargeModel> UpdateDischage(DischargeModel discharge)
        {
            var dischargeRepository = _unitOfWork.GetRepository<DischargeModel>(SchemaName.ECI);
            var procParams = BuildSpParams(discharge);
            return _unitOfWork.EnsureInTransaction(dischargeRepository.ExecuteNQStoredProc, "usp_UpdateDischarge", procParams,
                            forceRollback: discharge.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Deletes the dischage.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<DischargeModel> DeleteDischage(long Id, DateTime modifiedOn)
        {
            var dischargeRepository = _unitOfWork.GetRepository<DischargeModel>(SchemaName.ECI);
            var param = new List<SqlParameter> { new SqlParameter("DischargeID", Id), new SqlParameter("ModifiedOn", modifiedOn) };
            return dischargeRepository.ExecuteNQStoredProc("usp_DeleteDischarge", param);
        }


        private List<SqlParameter> BuildSpParams(DischargeModel discharge)
        {
            var spParameters = new List<SqlParameter>();

            if (discharge.DischargeID > 0) // Only in case of Update
                spParameters.Add(new SqlParameter("DischargeID", discharge.DischargeID));

            spParameters.AddRange(new List<SqlParameter> {
                new SqlParameter("ProgressNoteID", (object)discharge.ProgressNoteID ?? DBNull.Value),
                new SqlParameter("DischargeTypeID", (object)discharge.DischargeTypeID ?? DBNull.Value),
                new SqlParameter("DischargeDate", (object)discharge.DischargeDate ?? DBNull.Value),
                new SqlParameter("TakenBy", (object)discharge.TakenBy ?? DBNull.Value),
                new SqlParameter("DischargeReasonID", (object)discharge.DischargeReasonID ?? DBNull.Value),
                new SqlParameter("ModifiedOn", discharge.ModifiedOn ?? DateTime.Now)
            });
            return spParameters;
        }
    }
}
