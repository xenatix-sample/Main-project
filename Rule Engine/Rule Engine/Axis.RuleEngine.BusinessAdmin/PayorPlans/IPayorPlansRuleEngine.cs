
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.PayorPlan;

namespace Axis.RuleEngine.BusinessAdmin.PayorPlans
{
    public interface IPayorPlansRuleEngine
    {

        /// <summary>
        /// Gets the payor plans.
        /// </summary>
        /// <param name="PayorID">The payor identifier.</param>
        /// <returns></returns>
        Response<PayorPlan> GetPayorPlans(int PayorID);

        /// <summary>
        /// Gets the payor plan by identifier.
        /// </summary>
        /// <param name="payorPlanId">The payor plan identifier.</param>
        /// <returns></returns>
        Response<PayorPlan> GetPayorPlanByID(int payorPlanId);

        /// <summary>
        /// Adds the payor plan.
        /// </summary>
        /// <param name="payorPlanDetails">The payor plan details.</param>
        /// <returns></returns>
        Response<PayorPlan> AddPayorPlan(PayorPlan payorPlanDetails);

        /// <summary>
        /// Updates the payor plan.
        /// </summary>
        /// <param name="payorPlanDetails">The payor plan details.</param>
        /// <returns></returns>
        Response<PayorPlan> UpdatePayorPlan(PayorPlan payorPlanDetails);


    }


}
