
using System;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Service.BusinessAdmin.ClientMerge;
using Axis.Service.BusinessAdmin.Payors;
using Axis.RuleEngine.BusinessAdmin.PayorPlans;
using Axis.Model.Common.Lookups.PayorPlan;
using Axis.Service.BusinessAdmin.PayorPlans;

namespace Axis.RuleEngine.BusinessAdmin.PayorPlans
{
    public class PayorPlansRuleEngine : IPayorPlansRuleEngine
    {
        #region Class Variables        
        /// <summary>
        /// The payor plans service
        /// </summary>
        private readonly IPayorPlansService _payorPlansService;

        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="PayorPlansRuleEngine"/> class.
        /// </summary>
        /// <param name="payorPlansService">The payor plans service.</param>
        public PayorPlansRuleEngine(IPayorPlansService payorPlansService)
        {
            _payorPlansService = payorPlansService;
        }

        #endregion


        /// <summary>
        /// Gets the payor plans.
        /// </summary>
        /// <param name="payorID">The payor identifier.</param>
        /// <returns></returns>
        public Response<PayorPlan> GetPayorPlans(int payorID)
        {
            return _payorPlansService.GetPayorPlans(payorID);
        }

        /// <summary>
        /// Gets the payor plan by identifier.
        /// </summary>
        /// <param name="payorPlanId">The payor plan identifier.</param>
        /// <returns></returns>
        public Response<PayorPlan> GetPayorPlanByID(int payorPlanId)
        {
            return _payorPlansService.GetPayorPlanByID(payorPlanId);
        }

        /// <summary>
        /// Adds the payor plan.
        /// </summary>
        /// <param name="payorPlanDetails">The payor plan details.</param>
        /// <returns></returns>
        public Response<PayorPlan> AddPayorPlan(PayorPlan payorPlanDetails)
        {
            return _payorPlansService.AddPayorPlan(payorPlanDetails);
        }

        /// <summary>
        /// Updates the payor plan.
        /// </summary>
        /// <param name="payorPlanDetails">The payor plan details.</param>
        /// <returns></returns>
        public Response<PayorPlan> UpdatePayorPlan(PayorPlan payorPlanDetails)
        {
            return _payorPlansService.UpdatePayorPlan(payorPlanDetails);
        }


    }
}
