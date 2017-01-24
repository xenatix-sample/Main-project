
using System;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Data.Repository;
using System.Data.SqlClient;
using System.Collections.Generic;
using Axis.Data.Repository.Schema;
using Axis.DataProvider.BusinessAdmin.Payors;
using Axis.Model.Common.Lookups.PayorPlan;

namespace Axis.DataProvider.BusinessAdmin.ClientMerge
{
    public class PayorPlansDataProvider : IPayorPlansDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion Class 

        #region Constructors

        public PayorPlansDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Public Methods

        
        public Response<PayorPlan> GetPayorPlans(int PayorId)
        {

            // Get Payor Plans
            var benefitPlanRepository = _unitOfWork.GetRepository<PayorPlan>(SchemaName.Reference);
            SqlParameter payorIdParam = new SqlParameter("PayorId", PayorId);
            List<SqlParameter> procParams = new List<SqlParameter>() { payorIdParam };
            return benefitPlanRepository.ExecuteStoredProc("usp_GetPayorPlanDetailsByPayorID", procParams);

        }

        
        public Response<PayorPlan> GetPayorPlanByID(int payorPlanId)
        {

            // Get Payor Plans
            var benefitPlanRepository = _unitOfWork.GetRepository<PayorPlan>(SchemaName.Reference);
            SqlParameter payorIdParam = new SqlParameter("PayorPlanId", payorPlanId);
            List<SqlParameter> procParams = new List<SqlParameter>() { payorIdParam };
            return benefitPlanRepository.ExecuteStoredProc("usp_GetPayorPlanDetailsByPayorPlanID", procParams);

        }

       
        public Response<PayorPlan> AddPayorPlan(PayorPlan payorPlanDetails)
        {
            var payorsParameters = BuildPayorPlanSpParams(payorPlanDetails);
            var payorsRepository = _unitOfWork.GetRepository<PayorPlan>(SchemaName.Reference);
            return payorsRepository.ExecuteNQStoredProc("usp_AddPayorPlanDetails", payorsParameters, idResult: true);
        }

        
        public Response<PayorPlan> UpdatePayorPlan(PayorPlan payorPlanDetails)
        {
            var payorsParameters = BuildPayorPlanSpParams(payorPlanDetails);
            var payorsRepository = _unitOfWork.GetRepository<PayorPlan>(SchemaName.Reference);
            return payorsRepository.ExecuteNQStoredProc("usp_UpdatePayorPlanDetails", payorsParameters);
        }




        #endregion

        private List<SqlParameter> BuildPayorPlanSpParams(PayorPlan payorPlanDetails)
        {
            var spParameters = new List<SqlParameter>();
            if (payorPlanDetails.PayorPlanID > 0)
                spParameters.Add(new SqlParameter("PayorPlanID", payorPlanDetails.PayorPlanID));

                spParameters.AddRange(new List<SqlParameter>{
                new SqlParameter("PayorID", (object) payorPlanDetails.PayorID ?? DBNull.Value),
                new SqlParameter("PlanName", (object) payorPlanDetails.PlanName ?? DBNull.Value),
                new SqlParameter("PlanID", (object) payorPlanDetails.PlanID ?? DBNull.Value),
                new SqlParameter("EffectiveDate", (object) payorPlanDetails.EffectiveDate ?? DBNull.Value),
                new SqlParameter("ExpirationDate", (object) payorPlanDetails.ExpirationDate ?? DBNull.Value),
                new SqlParameter("IsActive", (object) payorPlanDetails.IsActive ?? DBNull.Value),
                new SqlParameter("ModifiedOn", (object) payorPlanDetails.ModifiedOn ?? DBNull.Value)
            });

            return spParameters;
        }
       
    }
}
