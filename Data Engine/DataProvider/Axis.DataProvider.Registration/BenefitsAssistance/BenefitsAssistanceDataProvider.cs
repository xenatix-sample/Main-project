using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Registration
{
    public class BenefitsAssistanceDataProvider : IBenefitsAssistanceDataProvider
    {
        #region initializations

        private IUnitOfWork unitOfWork = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="BenefitsAssistanceDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public BenefitsAssistanceDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceID">The benefits assistance identifier.</param>
        /// <returns></returns>
        public Response<BenefitsAssistanceModel> GetBenefitsAssistance(long benefitsAssistanceID)
        {
            var benefitsAssistanceRepository = unitOfWork.GetRepository<BenefitsAssistanceModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("BenefitsAssistanceID", benefitsAssistanceID) };

            return benefitsAssistanceRepository.ExecuteStoredProc("usp_GetBenefitsAssistance", procParams);
        }

        /// <summary>
        /// Gets the benefits assistance by contact identifier.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<BenefitsAssistanceModel> GetBenefitsAssistanceByContactID(long contactID)
        {
            var benefitsAssistanceRepository = unitOfWork.GetRepository<BenefitsAssistanceModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };

            var model = benefitsAssistanceRepository.ExecuteStoredProc("usp_GetBenefitsAssistanceByContactID", procParams);
            return model;
        }

        /// <summary>
        /// Adds the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceModel">The benefits assistance model.</param>
        /// <returns></returns>
        public Response<BenefitsAssistanceModel> AddBenefitsAssistance(BenefitsAssistanceModel benefitsAssistanceModel)
        {
            var benefitsAssistanceRepository = unitOfWork.GetRepository<BenefitsAssistanceModel>(SchemaName.Registration);
            var procParams = BuildSpParams(benefitsAssistanceModel);

            return unitOfWork.EnsureInTransaction(benefitsAssistanceRepository.ExecuteNQStoredProc, "usp_AddBenefitsAssistance", procParams, idResult: true,
                forceRollback: benefitsAssistanceModel.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Updates the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceModel">The benefits assistance model.</param>
        /// <returns></returns>
        public Response<BenefitsAssistanceModel> UpdateBenefitsAssistance(BenefitsAssistanceModel benefitsAssistanceModel)
        {
            var benefitsAssistanceRepository = unitOfWork.GetRepository<BenefitsAssistanceModel>(SchemaName.Registration);
            var procParams = BuildSpParams(benefitsAssistanceModel);

            return unitOfWork.EnsureInTransaction(benefitsAssistanceRepository.ExecuteNQStoredProc, "usp_UpdateBenefitsAssistance", procParams,
                forceRollback: benefitsAssistanceModel.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Deletes the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceID">The benefits assistance identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        public Response<BenefitsAssistanceModel> DeleteBenefitsAssistance(long benefitsAssistanceID, DateTime modifiedOn)
        {
            var benefitsAssistanceRepository = unitOfWork.GetRepository<BenefitsAssistanceModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("BenefitsAssistanceID", benefitsAssistanceID), new SqlParameter("ModifiedOn", modifiedOn) };
            return unitOfWork.EnsureInTransaction(benefitsAssistanceRepository.ExecuteNQStoredProc, "usp_DeleteBenefitsAssistance", procParams);
        }

        #endregion exposed functionality

        #region Helpers

        private List<SqlParameter> BuildSpParams(BenefitsAssistanceModel benefitsAssistanceModel)
        {
            var spParameters = new List<SqlParameter>();

            if (benefitsAssistanceModel.BenefitsAssistanceID > 0) // Only in case of Update
                spParameters.Add(new SqlParameter("BenefitsAssistanceID", benefitsAssistanceModel.BenefitsAssistanceID));
            
            spParameters.AddRange(new List<SqlParameter> {
                new SqlParameter("ContactID", (object)benefitsAssistanceModel.ContactID),
                new SqlParameter("DateEntered", (object)benefitsAssistanceModel.DateEntered ?? DBNull.Value),
                new SqlParameter("UserID", (object)benefitsAssistanceModel.UserID ?? DBNull.Value),
                new SqlParameter("AssessmentID", (object)benefitsAssistanceModel.AssessmentID ?? DBNull.Value),
                new SqlParameter("ResponseID", (object)benefitsAssistanceModel.ResponseID ?? DBNull.Value),
                new SqlParameter("DocumentStatusID", (object)benefitsAssistanceModel.DocumentStatusID ?? DBNull.Value),
                new SqlParameter("ModifiedOn", benefitsAssistanceModel.ModifiedOn ?? DateTime.Now)
                });
            return spParameters;
        }

        #endregion
    }
}
