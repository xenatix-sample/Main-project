using Axis.Helpers.Validation;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.PayorPlan;
using Axis.RuleEngine.BusinessAdmin.PayorPlans;
using Axis.RuleEngine.Helpers.Results;
using System.Collections.Generic;
using System.Web.Http;

namespace Axis.RuleEngine.Service.Controllers
{
    public class PayorPlansController : ApiController
    {
        #region Class Variables        
        /// <summary>
        /// The payor plans rule engine
        /// </summary>
        private readonly IPayorPlansRuleEngine _payorPlansRuleEngine = null;

        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="PayorPlansController"/> class.
        /// </summary>
        /// <param name="payorPlansRuleEngine">The payor plans rule engine.</param>
        public PayorPlansController(IPayorPlansRuleEngine payorPlansRuleEngine)
        {
            _payorPlansRuleEngine = payorPlansRuleEngine;
        }

        #endregion

        #region Public Methods


        /// <summary>
        /// Gets the payor plans.
        /// </summary>
        /// <param name="payorId">The payor identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetPayorPlans(int payorId)
        {
            return new HttpResult<Response<PayorPlan>>(_payorPlansRuleEngine.GetPayorPlans(payorId), Request);
        }


        /// <summary>
        /// Gets the payor plan by identifier.
        /// </summary>
        /// <param name="payorPlanId">The payor plan identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetPayorPlanByID(int payorPlanId)
        {
            return new HttpResult<Response<PayorPlan>>(_payorPlansRuleEngine.GetPayorPlanByID(payorPlanId), Request);
        }

        /// <summary>
        /// Adds the payor plan.
        /// </summary>
        /// <param name="payorPlanDetails">The payor plan details.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddPayorPlan(PayorPlan payorPlanDetails)
        {
            return new HttpResult<Response<PayorPlan>>(_payorPlansRuleEngine.AddPayorPlan(payorPlanDetails), Request);

        }

        /// <summary>
        /// Updates the payor plan.
        /// </summary>
        /// <param name="payorPlanDetails">The payor plan details.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdatePayorPlan(PayorPlan payorPlanDetails)
        {
            return new HttpResult<Response<PayorPlan>>(_payorPlansRuleEngine.UpdatePayorPlan(payorPlanDetails), Request);
        }


        #endregion
    }
}