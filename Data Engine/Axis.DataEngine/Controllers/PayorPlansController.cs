using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.BusinessAdmin.Payors;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.PayorPlan;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{
    /// <summary>
    /// Payor Plans Controller
    /// </summary>
    /// <seealso cref="Axis.DataEngine.Helpers.Controllers.BaseApiController" />
    public class PayorPlansController : BaseApiController
    {
        #region Class Variables        
        /// <summary>
        /// The payors data provider
        /// </summary>
        readonly IPayorPlansDataProvider _payorsDataProvider = null;

        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="PayorPlansController"/> class.
        /// </summary>
        /// <param name="payorsDataProvider">The payors data provider.</param>
        public PayorPlansController(IPayorPlansDataProvider payorsDataProvider)
        {
            _payorsDataProvider = payorsDataProvider;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the payor plans.
        /// </summary>
        /// <param name="payorID">The payor identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetPayorPlans(int payorID)
        {
            return new HttpResult<Response<PayorPlan>>(_payorsDataProvider.GetPayorPlans(payorID), Request);
        }

        /// <summary>
        /// Adds the payor plan.
        /// </summary>
        /// <param name="payorPlanDetails">The payor plan details.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddPayorPlan(PayorPlan payorPlanDetails)
        {
            return new HttpResult<Response<PayorPlan>>(_payorsDataProvider.AddPayorPlan(payorPlanDetails), Request);
        }


        /// <summary>
        /// Updates the payor plan.
        /// </summary>
        /// <param name="payorPlanDetails">The payor plan details.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdatePayorPlan(PayorPlan payorPlanDetails)
        {
            return new HttpResult<Response<PayorPlan>>(_payorsDataProvider.UpdatePayorPlan(payorPlanDetails), Request);
        }

        /// <summary>
        /// Gets the payor plan by identifier.
        /// </summary>
        /// <param name="payorPlanId">The payor plan identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetPayorPlanByID(int payorPlanId)
        {
            return new HttpResult<Response<PayorPlan>>(_payorsDataProvider.GetPayorPlanByID(payorPlanId), Request);
        }


        #endregion
    }
}