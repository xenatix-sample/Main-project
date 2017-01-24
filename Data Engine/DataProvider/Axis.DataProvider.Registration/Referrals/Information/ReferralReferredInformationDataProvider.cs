using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Axis.DataProvider.Registration.Referrals.Information
{
    /// <summary>
    /// Referral referred to information Data provider
    /// </summary>
    public class ReferralReferredInformationDataProvider : IReferralReferredInformationDataProvider
    {
        #region initializations

        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralReferredInformationDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReferralReferredInformationDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the referral referred information
        /// </summary>
        /// <param name="referralHeaderID">The referralHeader ID.</param>
        /// <returns></returns>
        public Response<ReferralReferredInformationModel> GetReferredInformation(long referralHeaderID)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralReferredInformationModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ReferralHeaderID", referralHeaderID) };
            return referralRepository.ExecuteStoredProc("usp_GetReferralReferredToDetails", procParams);
        }

        /// <summary>
        /// Adds the referral referred information
        /// </summary>
        /// <param name="Referral">The referral referred information.</param>
        /// <returns></returns>
        public Response<ReferralReferredInformationModel> AddReferredInformation(ReferralReferredInformationModel Referral)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralReferredInformationModel>(SchemaName.Registration);
            var procParams = BuildParams(Referral);

            return unitOfWork.EnsureInTransaction(
                    referralRepository.ExecuteNQStoredProc,
                    "usp_AddReferralReferredToDetail",
                    procParams,
                    forceRollback: Referral.ForceRollback.GetValueOrDefault(false),
                    idResult: true
                );
        }

        /// <summary>
        /// Updates the referral referred information.
        /// </summary>
        /// <param name="referral">The referred information.</param>
        /// <returns></returns>
        public Response<ReferralReferredInformationModel> UpdateReferredInformation(ReferralReferredInformationModel Referral)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralReferredInformationModel>(SchemaName.Registration);
            var procParams = BuildParams(Referral);
            return unitOfWork.EnsureInTransaction(
                    referralRepository.ExecuteNQStoredProc,
                    "usp_UpdateReferralReferredToDetail",
                    procParams,
                    forceRollback: Referral.ForceRollback.GetValueOrDefault(false),
                    idResult: false
                );
        }

        #endregion exposed functionality
        
        #region Helpers

        /// <summary>
        /// Builds the Referral sp parameters.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        private List<SqlParameter> BuildParams(ReferralReferredInformationModel referral)
        {
            var spParameters = new List<SqlParameter>();
            if (referral.ReferredToDetailID > 0)
                spParameters.Add(new SqlParameter("ReferredToInformationID", referral.ReferredToDetailID));

            spParameters.Add(new SqlParameter("ReferralHeaderID", referral.ReferralHeaderID));
            spParameters.Add(new SqlParameter("OrganizationID", (object)referral.OrganizationID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ReferredDateTime", (object)referral.ReferredDateTime ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ActionTaken", (object)referral.ActionTaken ?? DBNull.Value));
            spParameters.Add(new SqlParameter("Comments", (object)referral.Comments ?? DBNull.Value));
            spParameters.Add(new SqlParameter("UserID", (object)referral.UserID));
            spParameters.Add(new SqlParameter("ModifiedOn", referral.ModifiedOn ?? DateTime.Now));
            return spParameters;
        }

        #endregion Helpers    
    }
}
