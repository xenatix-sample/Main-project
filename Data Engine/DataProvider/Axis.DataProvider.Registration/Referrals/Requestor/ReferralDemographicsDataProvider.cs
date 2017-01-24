using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Registration.Referrals.Requestor
{
    public class ReferralDemographicsDataProvider : IReferralDemographicsDataProvider
    {
        #region Class Variables

        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralDemographicsDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReferralDemographicsDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion Constructors

        #region Public Methods
        /// <summary>
        /// Gets the referral demographics.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        public Response<ReferralDemographicsModel> GetReferralDemographics(long referralID)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralDemographicsModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("referralID", referralID) };
            return referralRepository.ExecuteStoredProc("usp_GetReferralDemographics", procParams);
        }

        /// <summary>
        /// Adds the referral demographics.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public Response<ReferralDemographicsModel> AddReferralDemographics(ReferralDemographicsModel model)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralDemographicsModel>(SchemaName.Registration);
            var procParams = BuildParams(model);

            return unitOfWork.EnsureInTransaction(
                    referralRepository.ExecuteNQStoredProc,
                    "usp_AddReferralDemographics",
                    procParams, idResult: true,
                    forceRollback: model.ForceRollback.GetValueOrDefault(false)
                );
        }

        /// <summary>
        /// Updates the referral demographics.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public Response<ReferralDemographicsModel> UpdateReferralDemographics(ReferralDemographicsModel model)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralDemographicsModel>(SchemaName.Registration);
            var procParams = BuildParams(model);

            return unitOfWork.EnsureInTransaction(
                    referralRepository.ExecuteNQStoredProc,
                    "usp_UpdateReferralDemographics",
                    procParams,
                    forceRollback: model.ForceRollback.GetValueOrDefault(false)
                );
        }

        /// <summary>
        /// Deletes the referral demographics.
        /// </summary>
        /// <param name="referralID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ReferralDemographicsModel> DeleteReferralDemographics(long referralID, DateTime modifiedOn)
        {
            //TODO:
            throw new NotImplementedException();
        }
        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Builds the parameters.
        /// </summary>
        /// <param name="model">The referral.</param>
        /// <returns></returns>
        private List<SqlParameter> BuildParams(ReferralDemographicsModel model)
        {
            var spParameters = new List<SqlParameter>();
            if (model.ReferralID > 0)
                spParameters.Add(new SqlParameter("ReferralID", model.ReferralID));
            spParameters.Add(new SqlParameter("ContactTypeID", (object)model.ContactTypeID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("FirstName", model.FirstName));
            spParameters.Add(new SqlParameter("Middle", (object)model.Middle ?? DBNull.Value));
            spParameters.Add(new SqlParameter("LastName", model.LastName));

            spParameters.Add(new SqlParameter("SuffixID", (object)model.SuffixID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("TitleID", (object)model.TitleID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("PreferredContactMethodID", (object)model.PreferredContactMethodID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("GestationalAge", (object)model.GestationalAge ?? DBNull.Value));
            spParameters.Add(new SqlParameter("OrganizationName", (object)model.OrganizationName ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModifiedOn", model.ModifiedOn ?? DateTime.Now));

            return spParameters;
        }

        #endregion Private Methods     
    }
}
