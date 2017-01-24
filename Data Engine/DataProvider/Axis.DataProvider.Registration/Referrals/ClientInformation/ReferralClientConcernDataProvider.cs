using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Axis.DataProvider.Registration.Referrals.ClientInformation
{
    public class ReferralClientConcernDataProvider : IReferralClientConcernDataProvider
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork = null;

        public ReferralClientConcernDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        
        public Response<ReferralClientConcernsModel> GetClientConcern(long ReferralAdditionalDetailID)
        {
            var referralRepository = unitOfWork.GetRepository<ReferralClientConcernsModel>(SchemaName.Registration);
            var procParams = new List<SqlParameter> { new SqlParameter("ReferralAdditionalDetailID", ReferralAdditionalDetailID) };
            return referralRepository.ExecuteStoredProc("usp_GetReferralConcernDetails", procParams);
        }

         /// <summary>
        /// Add/Update the Referral Client Concerns
        /// </summary>
        /// <param name="ReferralAdditionalDetailID">The referral Client Concerns.</param>
        /// <returns></returns>
        public Response<ReferralClientConcernsModel> AddUpdateClientConcern(ReferralClientConcernsModel clientConcern)
        {
            if (clientConcern.ReferralConcernDetailID > 0)
                return UpdateClientConcern(clientConcern);
            else
                return AddClientConcern(clientConcern);
        }

        /// <summary>
        /// Adds the Referral Client Concern
        /// </summary>
        /// <param name="ReferralAdditionalDetailID">The referral Client Concerns.</param>
        /// <returns></returns>
        Response<ReferralClientConcernsModel> AddClientConcern(ReferralClientConcernsModel clientConcern)
        {
            var repository = unitOfWork.GetRepository<ReferralClientConcernsModel>(SchemaName.Registration);
            var spParameters = BuildParams(clientConcern);
            return unitOfWork.EnsureInTransaction(repository.ExecuteNQStoredProc, "usp_AddReferralConcernDetails",
                spParameters, forceRollback: clientConcern.ForceRollback.GetValueOrDefault(), idResult: true);
        }

        /// <summary>
        /// Update the Referral Client Concern.
        /// </summary>
        /// <param name="concernToAdd">The referral client concern model to add.</param>
        /// <returns></returns>
        Response<ReferralClientConcernsModel> UpdateClientConcern(ReferralClientConcernsModel clientConcern)
        {
            var repository = unitOfWork.GetRepository<ReferralClientConcernsModel>(SchemaName.Registration);
            var spParameters = BuildParams(clientConcern);
            return unitOfWork.EnsureInTransaction(repository.ExecuteNQStoredProc, "usp_UpdateReferralConcernDetails",
                spParameters, forceRollback: clientConcern.ForceRollback.GetValueOrDefault());
        }

        /// <summary>
        /// Builds the parameters.
        /// </summary>
        /// <param name="model">The referral.</param>
        /// <returns></returns>
        private List<SqlParameter> BuildParams(ReferralClientConcernsModel model)
        {
            var spParameters = new List<SqlParameter>();
            if (model.ReferralConcernDetailID > 0)
                spParameters.Add(new SqlParameter("ReferralConcernDetailID", model.ReferralConcernDetailID));

            spParameters.Add(new SqlParameter("ReferralAdditionalDetailID", (object)model.ReferralAdditionalDetailID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ReferralConcernID", (object)model.ReferralConcernID?? DBNull.Value));
            spParameters.Add(new SqlParameter("Diagnosis", (object)model.Diagnosis ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ReferralPriorityID", (object)model.ReferralPriorityID ?? DBNull.Value));
            spParameters.Add(new SqlParameter("ModifiedOn", model.ModifiedOn ?? DateTime.Now));

            return spParameters;
        }
    }
}
