using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Logging;

namespace Axis.DataProvider.ECI.Screening
{
    public class ScreeningDataProvider : IScreeningDataProvider
    {
        #region initializations

        IUnitOfWork _unitOfWork;

        ILogger _logger;

        public ScreeningDataProvider(IUnitOfWork unitOfWork,ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion initializations

        #region Public Methods

        public Response<ScreeningModel> AddScreening(ScreeningModel screening)
        {
            var screeningRepository = _unitOfWork.GetRepository<ScreeningModel>(SchemaName.ECI);
            List<SqlParameter> procParams = BuildSqlParameters(screening);
            Response<ScreeningModel> spResults = new Response<ScreeningModel>();
            using (var transactionScope = _unitOfWork.BeginTransactionScope())
            {
                try
                {
                    spResults = screeningRepository.ExecuteNQStoredProc("usp_AddScreening", procParams, idResult: true);
                    if (spResults.ResultCode != 0)
                    {
                        spResults.ResultCode = spResults.ResultCode;
                        spResults.ResultMessage = spResults.ResultMessage;
                        return spResults;
                    }
                    if (!screening.ForceRollback.GetValueOrDefault(false))
                        _unitOfWork.TransactionScopeComplete(transactionScope);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    spResults.ResultCode = -1;
                    spResults.ResultMessage = "An unexpected error has occurred!";
                }
            }

            return spResults;
        }

        public Response<ScreeningModel> GetScreenings(long contactID)
        {
            var screeningRepository = _unitOfWork.GetRepository<ScreeningModel>(SchemaName.ECI);
            List<SqlParameter> procParams = new List<SqlParameter> { new SqlParameter("contactID", contactID) };

            var screeningResult = screeningRepository.ExecuteStoredProc("usp_GetContactScreeningsList", procParams);

            return screeningResult;
        }

        public Response<ScreeningModel> GetScreening(long screeningID)
        {
            var screeningRepository = _unitOfWork.GetRepository<ScreeningModel>(SchemaName.ECI);
            SqlParameter screeningIDParam = new SqlParameter("screeningID", screeningID);

            List<SqlParameter> procParams = new List<SqlParameter>() { screeningIDParam };

            var screeningResult = screeningRepository.ExecuteStoredProc("usp_GetScreeningDetails", procParams);

            return screeningResult;
        }

        public Response<ScreeningModel> UpdateScreening(ScreeningModel screening)
        {
            var screeningRepository = _unitOfWork.GetRepository<ScreeningModel>(SchemaName.ECI);
            List<SqlParameter> procParams = BuildSqlParameters(screening);
            Response<ScreeningModel> spResults = new Response<ScreeningModel>();
            using (var transactionScope = _unitOfWork.BeginTransactionScope())
            {
                try
                {

                    spResults = screeningRepository.ExecuteNQStoredProc("usp_UpdateScreening", procParams);
                    if (spResults.ResultCode != 0)
                    {
                        spResults.ResultCode = spResults.ResultCode;
                        spResults.ResultMessage = spResults.ResultMessage;
                    }
                    if (!screening.ForceRollback.GetValueOrDefault(false))
                        _unitOfWork.TransactionScopeComplete(transactionScope);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    spResults.ResultCode = -1;
                    spResults.ResultMessage = "An unexpected error has occurred!";
                }
            }
            return spResults;
        }

        public Response<bool> RemoveScreening(long screeningID, DateTime modifiedOn)
        {
            var procsParameters = new List<SqlParameter> { new SqlParameter("ScreeningID", screeningID), new SqlParameter("ModifiedOn", modifiedOn) };
            var screeningRepository = _unitOfWork.GetRepository<ScreeningModel>(SchemaName.ECI);
            var spResults = screeningRepository.ExecuteNQStoredProc("usp_DeleteScreening", procsParameters);
            Response<bool> resultSet = new Response<bool>();
            resultSet.ResultCode = spResults.ResultCode;
            resultSet.ResultMessage = spResults.ResultMessage;
            return resultSet;
        }

        public List<SqlParameter> BuildSqlParameters(ScreeningModel screening)
        {
            List<SqlParameter> procParams = new List<SqlParameter>
            {
                new SqlParameter("ContactID", screening.ContactID),
                new SqlParameter("InitialContactDate", screening.InitialContactDate),
                new SqlParameter("InitialServiceCoordinatorID",
                    screening.InitialServiceCoordinatorID ?? (object) DBNull.Value),
                new SqlParameter("PrimaryServiceCoordinatorID",
                    screening.PrimaryServiceCoordinatorID ?? (object) DBNull.Value),
                new SqlParameter("ScreeningDate", screening.ScreeningDate),
                new SqlParameter("ScreeningTypeID", screening.ScreeningTypeID ?? (object) DBNull.Value),
                new SqlParameter("AssessmentID", screening.AssessmentID ?? (object) DBNull.Value),
                new SqlParameter("ScreeningScore", screening.ScreeningScore ?? (object) DBNull.Value),
                new SqlParameter("ScreeningResultID", screening.ScreeningResultsID ?? (object) DBNull.Value),
                new SqlParameter("ScreeningStatusID", screening.ScreeningStatusID ?? (object) DBNull.Value)                
            };

            if (screening.ScreeningID <= 0)
            {
                procParams.Insert(1, new SqlParameter("ProgramID", screening.ProgramID));
            }
            else
            {
                procParams.Insert(0, new SqlParameter("ScreeningID", screening.ScreeningID));
                procParams.Add(new SqlParameter("SubmittedByID", screening.SubmittedByID ?? (object)DBNull.Value));
            }
            procParams.Add(new SqlParameter("ResponseID", screening.ResponseID.GetValueOrDefault(0) <= 0 ? (object) DBNull.Value : screening.ResponseID));
            procParams.Add(new SqlParameter("ModifiedOn", screening.ModifiedOn ?? DateTime.Now));

            return procParams;
        }

        #endregion
    }
}
