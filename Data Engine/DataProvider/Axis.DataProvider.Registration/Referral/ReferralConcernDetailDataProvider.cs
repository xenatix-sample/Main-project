using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Registration.Referral
{
    public class ReferralConcernDetailDataProvider : IReferralConcernDetailDataProvider
    {
        #region Class Variables

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralConcernDetailDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReferralConcernDetailDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion Constructors

        #region Public Methods
        /// <summary>
        /// Gets the referral ConcernDetail.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        public Response<ReferralConcernDetailModel> GetReferralConcernDetail(long referralAdditionalDetailID)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralConcernDetailModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ReferralAdditionalDetailID", referralAdditionalDetailID) };
            return referralRepository.ExecuteStoredProc("usp_GetReferralConcernDetails", procParams);
        }

        /// <summary>
        /// Adds the referral ConcernDetail.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public Response<ReferralConcernDetailModel> AddReferralConcernDetail(ReferralConcernDetailModel model)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralConcernDetailModel>(SchemaName.Registration);
            var procParams = BuildParams(model);

            return unitOfWork.EnsureInTransaction(
                    referralRepository.ExecuteNQStoredProc,
                    "usp_AddReferralConcernDetails",
                    procParams, idResult: true,
                    forceRollback: model.ForceRollback.GetValueOrDefault(false)
                );
        }

        /// <summary>
        /// Updates the referral ConcernDetail.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public Response<ReferralConcernDetailModel> UpdateReferralConcernDetail(ReferralConcernDetailModel model)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralConcernDetailModel>(SchemaName.Registration);
            var procParams = BuildParams(model);

            return unitOfWork.EnsureInTransaction(
                    referralRepository.ExecuteNQStoredProc,
                    "usp_UpdateReferralConcernDetails",
                    procParams,
                    forceRollback: model.ForceRollback.GetValueOrDefault(false)
                );
        }

        /// <summary>
        /// Delete the referral ConcernDetail.
        /// </summary>
        /// <param name="referralConcernDetailID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ReferralConcernDetailModel> DeleteReferralConcernDetail(long referralConcernDetailID, DateTime modifiedOn)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralConcernDetailModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ReferralConcernDetailID", referralConcernDetailID), new SqlParameter("ModifiedOn", modifiedOn) };
            return referralRepository.ExecuteNQStoredProc("usp_DeleteReferralConcernDetails", procParams);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Builds the parameters.
        /// </summary>
        /// <param name="model">The referral.</param>
        /// <returns></returns>
        private List<SqlParameter> BuildParams(ReferralConcernDetailModel model)
        {
            var spParameters = new List<SqlParameter>();
            if (model.ReferralConcernDetailID > 0)
                spParameters.Add(new SqlParameter("ReferralConcernDetailID", model.ReferralConcernDetailID));
            spParameters.AddRange(new List<SqlParameter>
                {
                    new SqlParameter("ReferralAdditionalDetailID", (object)model.ReferralAdditionalDetailID ?? DBNull.Value),
                    new SqlParameter("ReferralConcernID", (object)model.ReferralConcernID ?? DBNull.Value),
                    new SqlParameter("Diagnosis", (object)model.Diagnosis ?? DBNull.Value),
                    new SqlParameter("ReferralPriorityID", (object)model.ReferralPriorityID ?? DBNull.Value),
                    new SqlParameter("ModifiedOn", model.ModifiedOn ?? DateTime.Now)
                });

            return spParameters;
        }

        #endregion Private Methods
    }
}
