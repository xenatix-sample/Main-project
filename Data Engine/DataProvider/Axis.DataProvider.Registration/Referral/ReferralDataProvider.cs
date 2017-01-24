using System;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Registration
{
    /// <summary>
    /// Referral data provider
    /// </summary>
    public class ReferralDataProvider : IReferralDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork unitOfWork;

        #endregion

        #region Constructors

        public ReferralDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets Referral
        /// </summary>
        /// <param name="contactId">Contact Id</param>
        /// <returns></returns>
        public Response<ReferralModel> GetReferrals(long contactId)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ContactID", contactId) };

            return referralRepository.ExecuteStoredProc("usp_GetReferral", procParams);
        }

        /// <summary>
        /// Adds referral
        /// </summary>
        /// <param name="referral">referral model</param>
        /// <returns></returns>
        public Response<ReferralModel> AddReferral(ReferralModel referral)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralModel>(SchemaName.Registration);
            var procParams = BuildContactAddUpdSpParams(referral);

            return unitOfWork.EnsureInTransaction(referralRepository.ExecuteNQStoredProc, "usp_AddReferral", procParams,
                forceRollback: referral.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Updates referral
        /// </summary>
        /// <param name="referral">referral model</param>
        /// <returns></returns>
        public Response<ReferralModel> UpdateReferral(ReferralModel referral)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralModel>(SchemaName.Registration);
            var procParams = BuildContactAddUpdSpParams(referral);

            return unitOfWork.EnsureInTransaction(referralRepository.ExecuteNQStoredProc, "usp_UpdateReferral", procParams,
                forceRollback: referral.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Deletes referral
        /// </summary>
        /// <param name="id">Referral Id</param>
        /// <returns></returns>
        public Response<ReferralModel> DeleteReferral(long id)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> {new SqlParameter("ReferralId", id)};
            //TODO: Ensure transaction for delete
            return unitOfWork.EnsureInTransaction(referralRepository.ExecuteNQStoredProc, "usp_DeleteReferral", procParams);
        }

        /// <summary>
        /// Update Referral Contact
        /// </summary>
        /// <param name="referralContact"></param>
        /// <returns></returns>
        public Response<ReferralContactModel> UpdateReferralContact(ReferralContactModel referralContact)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralContactModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter>
            {
                new SqlParameter("ReferralContactID", referralContact.ReferralContactID),
                new SqlParameter("ReferralID", referralContact.ReferralID),
                new SqlParameter("ContactID", referralContact.ContactID),
                new SqlParameter("ModifiedOn", referralContact.ModifiedOn ?? DateTime.Now)
            };

            return unitOfWork.EnsureInTransaction(referralRepository.ExecuteNQStoredProc, "usp_UpdateReferralContact", procParams);
        }

        /// <summary>
        /// Delete Referal Contact
        /// </summary>
        /// <param name="referralContactId"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ReferralContactModel> DeleteReferalContact(long referralContactId, DateTime modifiedOn)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralContactModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ReferralContactID", referralContactId), new SqlParameter("ModifiedOn", modifiedOn) };
            //TODO: Ensure transaction for delete
            return unitOfWork.EnsureInTransaction(referralRepository.ExecuteNQStoredProc, "usp_DeleteReferralContact", procParams);
        }

        #endregion

        #region Helpers

        private List<SqlParameter> BuildContactAddUpdSpParams(ReferralModel referral)
        {   
            if (referral.ReferralContactID <= 0)
            {
                var spParams = new List<SqlParameter>
                {
                    new SqlParameter("ReferralID", referral.ReferralID),
                    new SqlParameter("ReferralContactID", referral.ReferralContactID),
                    new SqlParameter("ContactID", referral.ContactID),
                    new SqlParameter("ReferralName", referral.ReferralName),
                    new SqlParameter("ReferralOrganization", (object) referral.ReferralOrganization ?? DBNull.Value),
                    new SqlParameter("ReferralCategoryID", (object) referral.ReferralCategoryID ?? DBNull.Value),
                    new SqlParameter("ReferralSourceID", referral.ReferralSourceID),
                    new SqlParameter("ReferralOriginID", (object) referral.ReferralOriginID ?? DBNull.Value),
                    new SqlParameter("ReferralProgramID", (object) referral.ReferralProgramID ?? DBNull.Value),
                    new SqlParameter("ReferralClosureReasonID",
                        (object) referral.ReferralClosureReasonID ?? DBNull.Value),
                    new SqlParameter("ReferralConcern", (object) referral.ReferralConcern ?? DBNull.Value),
                    new SqlParameter("ReferredDate", referral.ReferredDate)
                };
                return spParams;
            }
            else
            {
                var spParams = new List<SqlParameter>
                {
                    new SqlParameter("ReferralID", referral.ReferralID),
                    new SqlParameter("ReferralName", referral.ReferralName),
                    new SqlParameter("ReferralOrganization", (object) referral.ReferralOrganization ?? DBNull.Value),
                    new SqlParameter("ReferralCategoryID", (object) referral.ReferralCategoryID ?? DBNull.Value),
                    new SqlParameter("ReferralSourceID", referral.ReferralSourceID),
                    new SqlParameter("ReferralOriginID", (object) referral.ReferralOriginID ?? DBNull.Value),
                    new SqlParameter("ReferralProgramID", (object) referral.ReferralProgramID ?? DBNull.Value),
                    new SqlParameter("ReferralClosureReasonID",
                        (object) referral.ReferralClosureReasonID ?? DBNull.Value),
                    new SqlParameter("ReferralConcern", (object) referral.ReferralConcern ?? DBNull.Value),
                    new SqlParameter("ReferredDate", referral.ReferredDate),
                    new SqlParameter("ModifiedOn", referral.ModifiedOn ?? DateTime.Now)
                };
                return spParams;
            }
        }

        #endregion
    }
}
