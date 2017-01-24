using Axis.Model.Common;
using Axis.Model.Common.Lookups.PayorPlan;
using Axis.PresentationEngine.Areas.BusinessAdmin.Respository.PayorPlans;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Http;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.ApiControllers
{
    public class PayorPlansController : BaseApiController
    {
        #region Class Variables

        /// <summary>
        /// The payor plans repository
        /// </summary>
        private readonly IPayorPlansRepository _payorPlansRepository;

        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="PayorPlansController"/> class.
        /// </summary>
        /// <param name="payorPlansRepository">The payor plans repository.</param>
        public PayorPlansController(IPayorPlansRepository payorPlansRepository)
        {
            _payorPlansRepository = payorPlansRepository;
        }
        #endregion

        #region Public Methods         

        /// <summary>
        /// Gets the payor plans.
        /// </summary>
        /// <param name="payorId">The payor identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<PayorPlan> GetPayorPlans(int payorId)
        {
            return _payorPlansRepository.GetPayorPlans(payorId);
        }

        /// <summary>
        /// Gets the payor plan by identifier.
        /// </summary>
        /// <param name="payorPlanId">The payor plan identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<PayorPlan> GetPayorPlanByID(int payorPlanId)
        {
            return _payorPlansRepository.GetPayorPlanByID(payorPlanId);
        }

        /// <summary>
        /// Adds the payor plan.
        /// </summary>
        /// <param name="payorPlanDetails">The payor plan details.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<PayorPlan> AddPayorPlan(PayorPlan payorPlanDetails)
        {
            return _payorPlansRepository.AddPayorPlan(payorPlanDetails);
        }

        /// <summary>
        /// Updates the payor plan.
        /// </summary>
        /// <param name="payorPlanDetails">The payor plan details.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<PayorPlan> UpdatePayorPlan(PayorPlan payorPlanDetails)
        {
            return _payorPlansRepository.UpdatePayorPlan(payorPlanDetails);
        }

        #endregion

    }
}