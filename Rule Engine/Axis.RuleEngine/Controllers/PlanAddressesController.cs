using Axis.Helpers.Validation;
using Axis.Model.BusinessAdmin;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.PayorPlan;
using Axis.RuleEngine.BusinessAdmin.ClientMerge;
using Axis.RuleEngine.BusinessAdmin.PayorPlans;
using Axis.RuleEngine.BusinessAdmin.Payors;
using Axis.RuleEngine.BusinessAdmin.PlanAddresses;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using System.Collections.Generic;
using System.Web.Http;

namespace Axis.RuleEngine.Service.Controllers
{
    /// <summary>
    /// Plan Addresses Controller
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class PlanAddressesController : ApiController
    {
        #region Class Variables        
        /// <summary>
        /// The plan addresses rule engine
        /// </summary>
        private readonly IPlanAddressesRuleEngine _planAddressesRuleEngine = null;

        #endregion

        #region Constructors        
        /// <summary>
        /// Initializes a new instance of the <see cref="PlanAddressesController"/> class.
        /// </summary>
        /// <param name="planAddressesRuleEngine">The plan addresses rule engine.</param>
        public PlanAddressesController(IPlanAddressesRuleEngine planAddressesRuleEngine)
        {
            _planAddressesRuleEngine = planAddressesRuleEngine;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the plan addresses.
        /// </summary>
        /// <param name="payorPlanID">The payor plan identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetPlanAddresses(int payorPlanID)
        {
            return new HttpResult<Response<PlanAddressesModel>>(_planAddressesRuleEngine.GetPlanAddresses(payorPlanID), Request);
        }

        /// <summary>
        /// Gets the plan address.
        /// </summary>
        /// <param name="payorAddressID">The payor address identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetPlanAddress(int payorAddressID)
        {
            return new HttpResult<Response<PlanAddressesModel>>(_planAddressesRuleEngine.GetPlanAddress(payorAddressID), Request);
        }

        /// <summary>
        /// Adds the plan address.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddPlanAddress(PlanAddressesModel payorDetails)
        {
            return new HttpResult<Response<PlanAddressesModel>>(_planAddressesRuleEngine.AddPlanAddress(payorDetails), Request);

        }

        /// <summary>
        /// Updates the plan address.
        /// </summary>
        /// <param name="payorDetails">The payor details.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdatePlanAddress(PlanAddressesModel payorDetails)
        {
            return new HttpResult<Response<PlanAddressesModel>>(_planAddressesRuleEngine.UpdatePlanAddress(payorDetails), Request);

        }

        #endregion
    }
}