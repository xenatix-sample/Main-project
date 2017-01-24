using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Model.Registration.Referrals;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Registration.Referral
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.DataProvider.Registration.Referral.IReferralAdditionalDetailDataProvider" />
    public class ReferralAdditionalDetailDataProvider : IReferralAdditionalDetailDataProvider
    {
        #region Class Variables

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralDataProvider" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReferralAdditionalDetailDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion Constructors

        #region Public Methods
        /// <summary>
        /// Gets the referral .
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ReferralAdditionalDetailModel> GetReferralAdditionalDetail(long contactID)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralAdditionalDetailModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };
            return referralRepository.ExecuteStoredProc("usp_GetReferralAdditionalDetailsByContact", procParams);
        }

        /// <summary>
        /// Gets the referral .
        /// </summary>
        /// <param name="contactID">The referral identifier.</param>
        /// <returns></returns>
        public Response<ReferralDetailsModel> GetReferralsDetails(long contactID)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralDetailsModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };
            return referralRepository.ExecuteStoredProc("usp_GetReferralsDetails", procParams);
        }

        /// <summary>
        /// Adds the referral .
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public Response<ReferralAdditionalDetailModel> AddReferralAdditionalDetail(ReferralAdditionalDetailModel model)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralAdditionalDetailModel>(SchemaName.Registration);
            var procParams = BuildParams(model);

            return unitOfWork.EnsureInTransaction(
                    referralRepository.ExecuteNQStoredProc,
                    "usp_AddReferralAdditionalDetails",
                    procParams, idResult: true,
                    forceRollback: model.ForceRollback.GetValueOrDefault(false)
                );
        }

        /// <summary>
        /// Updates the referral .
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public Response<ReferralAdditionalDetailModel> UpdateReferralAdditionalDetail(ReferralAdditionalDetailModel model)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralAdditionalDetailModel>(SchemaName.Registration);
            var procParams = BuildParams(model);

            return unitOfWork.EnsureInTransaction(
                    referralRepository.ExecuteNQStoredProc,
                    "usp_UpdateReferralAdditionalDetails",
                    procParams,
                    forceRollback: model.ForceRollback.GetValueOrDefault(false)
                );
        }

        /// <summary>
        /// Delete the referral .
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ReferralDetailsModel> DeleteReferralDetails(long contactID)
        {
            var procsParameters = new List<SqlParameter> { new SqlParameter("ContactID", contactID), new SqlParameter("ModifiedOn", DateTime.UtcNow) };
            var contactRepository = unitOfWork.GetRepository<ReferralDetailsModel>(SchemaName.Registration);
            return contactRepository.ExecuteNQStoredProc("usp_DeleteReferral", procsParameters);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Builds the parameters.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        private List<SqlParameter> BuildParams(ReferralAdditionalDetailModel model)
        {
            var spParameters = new List<SqlParameter>();
            if (model.ReferralAdditionalDetailID > 0)
                spParameters.Add(new SqlParameter("ReferralAdditionalDetailID", model.ReferralAdditionalDetailID));
            
            spParameters.AddRange(new List<SqlParameter>
                {
                    new SqlParameter("ReferralHeaderID", model.ReferralHeaderID),
                    new SqlParameter("ContactID", model.ContactID),
                    new SqlParameter("ReasonforCare", (object)model.ReasonforCare ?? DBNull.Value),
                    new SqlParameter("IsTransferred", (object)model.IsTransferred ?? DBNull.Value),
                    new SqlParameter("IsHousingProgram", (object)model.IsHousingProgram ?? DBNull.Value),
                    new SqlParameter("HousingDescription", (object)model.HousingDescription ?? DBNull.Value),
                    new SqlParameter("IsEligibleforFurlough", (object)model.IsEligibleforFurlough ?? DBNull.Value),
                    new SqlParameter("IsReferralDischargeOrTransfer", (object)model.IsReferralDischargeOrTransfer ?? DBNull.Value),
                    new SqlParameter("IsConsentRequired", (object)model.IsConsentRequired ?? DBNull.Value),
                    new SqlParameter("Comments", (object)model.Comments ?? DBNull.Value),
                    new SqlParameter("AdditionalConcerns", (object)model.AdditionalConcerns ?? DBNull.Value),
                    new SqlParameter("ModifiedOn", model.ModifiedOn ?? DateTime.Now)
            });
            return spParameters;
        }

        #endregion Private Methods

    }
}