using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Axis.DataProvider.Registration
{
    public class SelfPayDataProvider : ISelfPayDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork unitOfWork;

        #endregion

        #region Constructors

        public SelfPayDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods


        /// <summary>
        /// Gets the self pay list.
        /// </summary>
        /// <param name="selfPayHeaderID">The self pay identifier.</param>
        /// <returns></returns>
        public Response<SelfPayModel> GetSelfPayDetails(long selfPayID)
        {
            var contactDischargeNoteRepository = unitOfWork.GetRepository<SelfPayModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ContactID", selfPayID) };

            return contactDischargeNoteRepository.ExecuteStoredProc("usp_GetSelfPay", procParams);
        }


        /// <summary>
        /// add the self pay.
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        public Response<SelfPayModel> AddSelfPay(SelfPayModel selfPay)
        {
            var selfPayRepository = unitOfWork.GetRepository<SelfPayModel>(SchemaName.Registration);
            var procParams = BuildSpParams(selfPay);

            return unitOfWork.EnsureInTransaction(selfPayRepository.ExecuteNQStoredProc, "usp_AddSelfPay", procParams, idResult: true,
                forceRollback: selfPay.ForceRollback.GetValueOrDefault(false));
        }


        /// <summary>
        /// add the self pay header.
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        public Response<SelfPayModel> AddSelfPayHeader(SelfPayModel selfPay)
        {
            var selfPayRepository = unitOfWork.GetRepository<SelfPayModel>(SchemaName.Registration);
            var procParams = BuildSpParams(selfPay);

            return unitOfWork.EnsureInTransaction(selfPayRepository.ExecuteNQStoredProc, "usp_AddSelfPayHeader", procParams, idResult: true,
                forceRollback: selfPay.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Updates the self pay.
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        public Response<SelfPayModel> UpdateSelfPay(SelfPayModel selfPay)
        {
            var selfPayRepository = unitOfWork.GetRepository<SelfPayModel>(SchemaName.Registration);
            var procParams = BuildSpParams(selfPay);

            return unitOfWork.EnsureInTransaction(selfPayRepository.ExecuteNQStoredProc, "usp_UpdateSelfPay", procParams,
                forceRollback: selfPay.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// delete the self pay.
        /// </summary>
        /// <param name="selfPayID">The self pay identifier.</param>
        /// <param name="modifiedOn">The modified on</param>
        /// <returns></returns>
        public Response<SelfPayModel> DeleteSelfPay(long selfPayID, DateTime modifiedOn)
        {
            var contactDischargeNoteRepository = unitOfWork.GetRepository<SelfPayModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("SelfPayID", selfPayID), new SqlParameter("ModifiedOn", modifiedOn) };
            return unitOfWork.EnsureInTransaction(contactDischargeNoteRepository.ExecuteNQStoredProc, "usp_DeleteSelfPay", procParams);
        }

        #endregion

        #region Helpers

        private List<SqlParameter> BuildSpParams(SelfPayModel selfPay)
        {
            var spParameters = new List<SqlParameter>();

            if (selfPay.SelfPayID > 0) // Only in case of Update
                spParameters.Add(new SqlParameter("SelfPayID", selfPay.SelfPayID));
            else
                spParameters.Add(new SqlParameter("ContactID", selfPay.ContactID));

            spParameters.AddRange(new List<SqlParameter> {
                new SqlParameter("OrganizationDetailID", (object)selfPay.OrganizationDetailID ?? DBNull.Value),
                new SqlParameter("SelfPayAmount", (object)selfPay.SelfPayAmount ?? DBNull.Value),
                new SqlParameter("IsPercent", (object)selfPay.IsPercent ?? DBNull.Value),
                new SqlParameter("EffectiveDate", (object)selfPay.EffectiveDate ?? DBNull.Value),
                new SqlParameter("ExpirationDate", (object)selfPay.ExpirationDate ?? DBNull.Value),
                new SqlParameter("ISChildInConservatorship", (object)selfPay.ISChildInConservatorship ?? DBNull.Value),
                new SqlParameter("IsNotAttested", (object)selfPay.IsNotAttested ?? DBNull.Value),
                new SqlParameter("IsApplyingForPublicBenefits", (object)selfPay.IsApplyingForPublicBenefits ?? DBNull.Value),
                new SqlParameter("IsEnrolledInPublicBenefits", (object)selfPay.IsEnrolledInPublicBenefits ?? DBNull.Value),
                new SqlParameter("IsRequestingReconsideration", (object)selfPay.IsRequestingReconsideration ?? DBNull.Value),
                new SqlParameter("IsNotGivingConsent", (object)selfPay.IsNotGivingConsent ?? DBNull.Value),
                new SqlParameter("IsOtherChildEnrolled", (object)selfPay.IsOtherChildEnrolled ?? DBNull.Value),
                new SqlParameter("IsReconsiderationOfAdjustment", (object)selfPay.IsReconsiderationOfAdjustment ?? DBNull.Value),
                new SqlParameter("ModifiedOn", selfPay.ModifiedOn ?? DateTime.Now)
                });
            return spParameters;
        }

        #endregion
    }
}
