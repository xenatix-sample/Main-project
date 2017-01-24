using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.ECI.Referrals;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.ECI.Referrals
{
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
        /// Initializes a new instance of the <see cref="ReferralDataProvider"/> class.
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
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        public Response<ReferralAdditionalDetailModel> GetReferralAdditionalDetail(long contactID)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralAdditionalDetailModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ContactID", contactID) };
            return referralRepository.ExecuteStoredProc("usp_GetReferralAdditionalDetailsByContact", procParams);
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

        #endregion Public Methods


        #region Private Methods

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
                    new SqlParameter("AdditionalConcerns", (object)model.AdditionalConcerns ?? DBNull.Value)
            });
            return spParameters;
        }

        #endregion Private Methods
       
    }
}
