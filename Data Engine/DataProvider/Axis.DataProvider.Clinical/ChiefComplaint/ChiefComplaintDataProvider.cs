using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Clinical;

namespace Axis.DataProvider.Clinical.ChiefComplaint
{
    public class ChiefComplaintDataProvider : IChiefComplaintDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion Class Variables

        #region Constructors

        public ChiefComplaintDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Constructors

        #region Public Methods

        public Response<ChiefComplaintModel> GetChiefComplaint(long contactID)
        {
            var repository = _unitOfWork.GetRepository<ChiefComplaintModel>(SchemaName.Clinical);
            SqlParameter idParam = new SqlParameter("ContactID", contactID);
            List<SqlParameter> procParams = new List<SqlParameter>() { idParam };
            var result = repository.ExecuteStoredProc("usp_GetChiefComplaintbyContact", procParams);
            return result;
        }

        public Response<ChiefComplaintModel> GetChiefComplaints(long contactID)
        {
            var repository = _unitOfWork.GetRepository<ChiefComplaintModel>(SchemaName.Clinical);
            List<SqlParameter> procParams = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };

            var result = repository.ExecuteStoredProc("usp_GetChiefComplaintList", procParams);

            return result;
        }

        public Response<ChiefComplaintModel> AddChiefComplaint(ChiefComplaintModel chiefComplaint)
        {
            var repository = _unitOfWork.GetRepository<ChiefComplaintModel>(SchemaName.Clinical);
            var procParams = BuildAddUpdSpParams(chiefComplaint);
            return _unitOfWork.EnsureInTransaction(repository.ExecuteNQStoredProc, "usp_AddChiefComplaint", procParams, idResult: true,
                            forceRollback: chiefComplaint.ForceRollback.GetValueOrDefault(false));
        }

        public Response<ChiefComplaintModel> UpdateChiefComplaint(ChiefComplaintModel chiefComplaint)
        {
            var repository = _unitOfWork.GetRepository<ChiefComplaintModel>(SchemaName.Clinical);
            var procParams = BuildAddUpdSpParams(chiefComplaint);
            return _unitOfWork.EnsureInTransaction(repository.ExecuteNQStoredProc, "usp_UpdateChiefComplaint", procParams,
                            forceRollback: chiefComplaint.ForceRollback.GetValueOrDefault(false));
        }

        public Response<ChiefComplaintModel> DeleteChiefComplaint(long chiefComplaintID, DateTime modifiedOn)
        {
            var rosRepository = _unitOfWork.GetRepository<ChiefComplaintModel>(SchemaName.Clinical);
            var param = new SqlParameter("ChiefComplaintID", chiefComplaintID);
            var modifiedOnParam = new SqlParameter("ModifiedOn", modifiedOn);
            var procParams = new List<SqlParameter> { param, modifiedOnParam };
            var result = rosRepository.ExecuteNQStoredProc("usp_DeleteChiefComplaint", procParams);
            return result;
        }

        private List<SqlParameter> BuildAddUpdSpParams(ChiefComplaintModel chiefComplaint)
        {
            var spParameters = new List<SqlParameter>
            {
                new SqlParameter("ChiefComplaintID", chiefComplaint.ChiefComplaintID),
                new SqlParameter("ContactID", chiefComplaint.ContactID),
                new SqlParameter("ChiefComplaint", chiefComplaint.ChiefComplaint),
                new SqlParameter("TakenBy", chiefComplaint.TakenBy),
                new SqlParameter("TakenTime", chiefComplaint.TakenTime),
                new SqlParameter("EncounterID", (object) chiefComplaint.EncounterID ?? DBNull.Value),
                new SqlParameter("ModifiedOn", chiefComplaint.ModifiedOn ?? DateTime.Now)
            };
            return spParameters;
        }
        
        #endregion
    }
}
