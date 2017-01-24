using Axis.Data.Repository;
using Axis.Helpers;
using Axis.Constant;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Registration.Referrals.Forwarded
{
    /// <summary>
    ///
    /// </summary>
    public class ReferralForwardedDataProvider : IReferralForwardedDataProvider
    {
        #region Class Variables

        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork = null;

        #endregion  Class Variables

        #region  Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralForwardedDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReferralForwardedDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion  Constructors

        #region Public Methods

        /// <summary>
        /// Gets the referral followups.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        public Response<ReferralForwardedModel> GetReferralForwardedDetails(long ReferralHeaderID)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralForwardedModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ReferralHeaderID", ReferralHeaderID) };

            return referralRepository.ExecuteStoredProc("usp_GetReferralForwardedDetails", procParams);
        }

        /// <summary>
        /// Gets the referralForwarded.
        /// </summary>
        /// <param name="contactId">The Referral detail.</param>
        /// <returns></returns>
        public Response<ReferralForwardedModel> GetReferralForwardedDetail(long ReferralForwardedDetailID)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralForwardedModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ReferralForwardedDetailID", ReferralForwardedDetailID) };

            return referralRepository.ExecuteStoredProc("GetReferralForwardedDetail", procParams);
        }

        /// <summary>
        /// Adds the referralForwarded.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralForwardedModel> AddReferralForwardedDetail(ReferralForwardedModel referral)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralForwardedModel>(SchemaName.Registration);
            var procParams = BuildParams(referral);

            return unitOfWork.EnsureInTransaction(
                    referralRepository.ExecuteNQStoredProc,
                    "usp_AddReferralForwardedDetail",
                    procParams,
                    forceRollback: referral.ForceRollback.GetValueOrDefault(false)
                );
        }

        /// <summary>
        /// Updates the referralForwarded.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralForwardedModel> UpdateReferralForwardedDetail(ReferralForwardedModel referral)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralForwardedModel>(SchemaName.Registration);
            var procParams = BuildParams(referral);

            return unitOfWork.EnsureInTransaction(
                    referralRepository.ExecuteNQStoredProc,
                    "usp_UpdateReferralForwardedDetail",
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
        private List<SqlParameter> BuildParams(ReferralForwardedModel referral)
        {
            var spParameters = new List<SqlParameter>();
            if (referral.ReferralForwardedDetailID > 0)
            spParameters.Add(new SqlParameter("ReferralForwardedDetailID", referral.ReferralForwardedDetailID));
            spParameters.Add(new SqlParameter("ReferralHeaderID", referral.ReferralHeaderID));
            spParameters.Add(new SqlParameter("OrganizationID", referral.OrganizationID));
            spParameters.Add(new SqlParameter("SendingReferralToID", (object)referral.SendingReferralToID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("Comments", (object)referral.Comments ?? DBNull.Value));
            spParameters.Add(new SqlParameter("UserID", (object)referral.UserID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ReferralSentDate", (object)referral.ReferralSentDate ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModifiedOn", referral.ModifiedOn ?? DateTime.Now));

            return spParameters;
        }

        #endregion Private Methods
    }
}
