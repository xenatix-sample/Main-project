using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Registration.Referrals.Disposition
{
    public class ReferralDispositionDataProvider : IReferralDispositionDataProvider
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
        public ReferralDispositionDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Gets the referral disposition detail.
        /// </summary>
        /// <param name="referralOutcomeDetailID">The referral header identifier.</param>
        /// <returns></returns>
        public Response<ReferralDispositionModel> GetReferralDispositionDetail(long referralHeaderID)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralDispositionModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ReferralHeaderID", referralHeaderID) };

            return referralRepository.ExecuteStoredProc("usp_GetReferralDispositionDetails", procParams);
        }

        /// <summary>
        /// Adds the referral disposition.
        /// </summary>
        /// <param name="referral">The referral disposition .</param>
        /// <returns></returns>
        public Response<ReferralDispositionModel> AddReferralDisposition(ReferralDispositionModel referralDisposition)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralDispositionModel>(SchemaName.Registration);
            var procParams = BuildParams(referralDisposition);

            return unitOfWork.EnsureInTransaction(
                    referralRepository.ExecuteNQStoredProc,
                    "usp_AddReferralDispositionDetail",
                    procParams,
                    forceRollback: referralDisposition.ForceRollback.GetValueOrDefault(false),
                    idResult: true
                );
        }

        /// <summary>
        /// Updates the referral disposition.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralDispositionModel> UpdateReferralDisposition(ReferralDispositionModel referralDisposition)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralDispositionModel>(SchemaName.Registration);
            var procParams = BuildParams(referralDisposition);

            return unitOfWork.EnsureInTransaction(
                    referralRepository.ExecuteNQStoredProc,
                    "usp_UpdateReferralDispositionDetail",
                    procParams,
                    forceRollback: referralDisposition.ForceRollback.GetValueOrDefault(false)
                );
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Builds the parameters.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        private List<SqlParameter> BuildParams(ReferralDispositionModel referralDisposition)
        {
            var spParameters = new List<SqlParameter>();
            if (referralDisposition.ReferralDispositionDetailID > 0)
                spParameters.Add(new SqlParameter("ReferralDispositionDetailID", referralDisposition.ReferralDispositionDetailID));

            spParameters.Add(new SqlParameter("ReferralHeaderID", referralDisposition.ReferralHeaderID));
            spParameters.Add(new SqlParameter("ReferralDispositionID", (object)referralDisposition.ReferralDispositionID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ReasonforDenial", (object)referralDisposition.ReasonforDenial ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ReferralDispositionOutcomeID", (object)referralDisposition.ReferralDispositionOutcomeID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("AdditionalNotes", (object)referralDisposition.AdditionalNotes ?? DBNull.Value));
            spParameters.Add(new SqlParameter("UserID", referralDisposition.UserID));
            spParameters.Add(new SqlParameter("DispositionDate", referralDisposition.DispositionDate));
            spParameters.Add(new SqlParameter("ModifiedOn", referralDisposition.ModifiedOn ?? DateTime.Now));
            return spParameters;
        }

        #endregion Private Methods
    }
}
