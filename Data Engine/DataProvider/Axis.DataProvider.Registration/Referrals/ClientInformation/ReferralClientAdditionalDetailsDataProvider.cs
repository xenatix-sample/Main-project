using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Registration.Referrals.ClientInformation
{
    public class ReferralClientAdditionalDetailsDataProvider : IReferralClientAdditionalDetailsDataProvider
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork = null;

        public ReferralClientAdditionalDetailsDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Response<ReferralClientAdditionalDetailsModel> GetClientAdditionalDetail(long ReferralHeaderID)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralClientAdditionalDetailsModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ReferralheaderID", ReferralHeaderID) };
            return referralRepository.ExecuteStoredProc("usp_GetReferralAdditionalDetails", procParams);
        }

        /// <summary>
        /// Adds the client AdditionalDetail.
        /// </summary>
        /// <param name="referral">The referral AdditionalDetail .</param>
        /// <returns></returns>
        public Response<ReferralClientAdditionalDetailsModel> AddClientAdditionalDetail(ReferralClientAdditionalDetailsModel referralAdditionalDetails)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralClientAdditionalDetailsModel>(SchemaName.Registration);
            var procParams = BuildParams(referralAdditionalDetails);

            return unitOfWork.EnsureInTransaction(
                    referralRepository.ExecuteNQStoredProc,
                    "usp_AddReferralAdditionalDetails",
                    procParams,
                    forceRollback: referralAdditionalDetails.ForceRollback.GetValueOrDefault(false),
                    idResult: true
                );
        }

        /// <summary>
        /// Updates the client AdditionalDetail.
        /// </summary>
        /// <param name="referral">The referral AdditionalDetail .</param>
        /// <returns></returns>
        public Response<ReferralClientAdditionalDetailsModel> UpdateClientAdditionalDetail(ReferralClientAdditionalDetailsModel referralAdditionalDetails)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralClientAdditionalDetailsModel>(SchemaName.Registration);
            var procParams = BuildParams(referralAdditionalDetails);

            return unitOfWork.EnsureInTransaction(
                    referralRepository.ExecuteNQStoredProc,
                    "usp_UpdateReferralAdditionalDetails",
                    procParams,
                    forceRollback: referralAdditionalDetails.ForceRollback.GetValueOrDefault(false),
                    idResult: false
                );
        }

        #region Private Methods

        /// <summary>
        /// Builds the parameters.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        private List<SqlParameter> BuildParams(ReferralClientAdditionalDetailsModel referralAdditionalDetails)
        {
            var spParameters = new List<SqlParameter>();
            if (referralAdditionalDetails.ReferralAdditionalDetailID > 0)
                spParameters.Add(new SqlParameter("ReferralDispositionDetailID", referralAdditionalDetails.ReferralAdditionalDetailID));

            spParameters.Add(new SqlParameter("ReferralHeaderID", referralAdditionalDetails.ReferralHeaderID));
            spParameters.Add(new SqlParameter("ReferralID", referralAdditionalDetails.ContactID));
            spParameters.Add(new SqlParameter("ReasonforCare", (object)referralAdditionalDetails.ReasonforCare ?? DBNull.Value));
            spParameters.Add(new SqlParameter("IsTransferred", (object)referralAdditionalDetails.IsTransferred ?? DBNull.Value));
            spParameters.Add(new SqlParameter("IsHousingProgram", (object)referralAdditionalDetails.IsHousingProgram ?? DBNull.Value));
            spParameters.Add(new SqlParameter("HousingDescription", (object)referralAdditionalDetails.HousingDescription ?? DBNull.Value));
            spParameters.Add(new SqlParameter("IsEligibleforFurlough", (object)referralAdditionalDetails.IsEligibleforFurlough ?? DBNull.Value));
            spParameters.Add(new SqlParameter("IsReferralDischargeOrTransfer", (object)referralAdditionalDetails.IsReferralDischargeOrTransfer ?? DBNull.Value));
            spParameters.Add(new SqlParameter("IsConsentRequired", (object)referralAdditionalDetails.IsConsentRequired ?? DBNull.Value));
            spParameters.Add(new SqlParameter("Comments", (object)referralAdditionalDetails.Comments ?? DBNull.Value));
            spParameters.Add(new SqlParameter("AdditionalConcerns", DBNull.Value));
            spParameters.Add(new SqlParameter("ModifiedOn", referralAdditionalDetails.ModifiedOn ?? DateTime.Now));
            return spParameters;
        }

        #endregion Private Methods
    }
}
