using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Registration.Referrals.Followup
{
    /// <summary>
    ///
    /// </summary>
    public class ReferralFollowupDataProvider : IReferralFollowupDataProvider
    {
        #region Class Variables

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralFollowupDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReferralFollowupDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the referral followups.
        /// </summary>
        /// <param name="referralHeaderID">The referral identifier.</param>
        /// <returns></returns>
        public Response<ReferralOutcomeDetailsModel> GetReferralFollowups(long referralHeaderID)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralOutcomeDetailsModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ReferralHeaderID", referralHeaderID) };

            return referralRepository.ExecuteStoredProc("usp_GetReferralOutcomeDetails", procParams);
        }

        /// <summary>
        /// Gets the referral followup.
        /// </summary>
        /// <param name="referralOutcomeDetailID">The referral outcome detail identifier.</param>
        /// <returns></returns>
        public Response<ReferralOutcomeDetailsModel> GetReferralFollowup(long referralOutcomeDetailID)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralOutcomeDetailsModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ReferralOutcomeDetailID", referralOutcomeDetailID) };

            return referralRepository.ExecuteStoredProc("usp_GetReferralOutcomeDetail", procParams);
        }

        /// <summary>
        /// Adds the referral followup.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralOutcomeDetailsModel> AddReferralFollowup(ReferralOutcomeDetailsModel referral)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralOutcomeDetailsModel>(SchemaName.Registration);
            var procParams = BuildParams(referral);

            return unitOfWork.EnsureInTransaction(
                    referralRepository.ExecuteNQStoredProc,
                    "usp_AddReferralOutcomeDetail",
                    procParams,
                    forceRollback: referral.ForceRollback.GetValueOrDefault(false), 
                    idResult: true
                );
        }

        /// <summary>
        /// Updates the referral followup.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralOutcomeDetailsModel> UpdateReferralFollowup(ReferralOutcomeDetailsModel referral)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralOutcomeDetailsModel>(SchemaName.Registration);
            var procParams = BuildParams(referral);

            return unitOfWork.EnsureInTransaction(
                    referralRepository.ExecuteNQStoredProc,
                    "usp_UpdateReferralOutcomeDetail",
                    procParams,
                    forceRollback: referral.ForceRollback.GetValueOrDefault(false)
                );
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Builds the parameters.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        private List<SqlParameter> BuildParams(ReferralOutcomeDetailsModel referral)
        {
            var spParameters = new List<SqlParameter>();
            if (referral.ReferralOutcomeDetailID > 0)
                spParameters.Add(new SqlParameter("ReferralOutcomeDetailID", referral.ReferralOutcomeDetailID));

            spParameters.Add(new SqlParameter("ReferralHeaderID", referral.ReferralHeaderID));
            spParameters.Add(new SqlParameter("FollowupExpected", (object)referral.FollowupExpected ?? DBNull.Value));
            spParameters.Add(new SqlParameter("FollowupProviderID", (object)referral.FollowupProviderID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("FollowupDate", (object)referral.FollowupDate ?? DBNull.Value));
            spParameters.Add(new SqlParameter("FollowupOutcome", (object)referral.FollowupOutcome ?? DBNull.Value));
            spParameters.Add(new SqlParameter("IsAppointmentNotified", (object)referral.IsAppointmentNotified ?? DBNull.Value));
            spParameters.Add(new SqlParameter("AppointmentNotificationMethod", (object)referral.AppointmentNotificationMethod ?? DBNull.Value));
            spParameters.Add(new SqlParameter("Comments", (object)referral.Comments ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModifiedOn", referral.ModifiedOn ?? DateTime.Now));

            return spParameters;
        }

        #endregion Private Methods
    }
}
