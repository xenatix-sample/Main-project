using Axis.Constant;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration;
using System;
using System.Web.Http;

namespace Axis.RuleEngine.Plugins.Registration
{
    public class BenefitsAssistanceController : BaseApiController
    {
        private readonly IBenefitsAssistanceRuleEngine benefitsAssistanceRuleEngine;


        /// <summary>
        /// Initializes a new instance of the <see cref="BenefitsAssistanceController"/> class.
        /// </summary>
        /// <param name="benefitsAssistanceRuleEngine">The benefits assistance rule engine.</param>
        public BenefitsAssistanceController(IBenefitsAssistanceRuleEngine benefitsAssistanceRuleEngine)
        {
            this.benefitsAssistanceRuleEngine = benefitsAssistanceRuleEngine;
        }

        /// <summary>
        /// Gets the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceID">The benefits assistance identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { BenefitsPermissionKey.Benefits_BAPN_BAPN,ChartPermissionKey.Chart_Chart_RecordedServices }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetBenefitsAssistance(long benefitsAssistanceID)
        {
            return new HttpResult<Response<BenefitsAssistanceModel>>(benefitsAssistanceRuleEngine.GetBenefitsAssistance(benefitsAssistanceID), Request);
        }

        /// <summary>
        /// Gets the benefits assistance by contact identifier.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = BenefitsPermissionKey.Benefits_BAPN_BAPN, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetBenefitsAssistanceByContactID(long contactID)
        {
            return new HttpResult<Response<BenefitsAssistanceModel>>(benefitsAssistanceRuleEngine.GetBenefitsAssistanceByContactID(contactID), Request);
        }

        /// <summary>
        /// Adds the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceModel">The benefits assistance model.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = BenefitsPermissionKey.Benefits_BAPN_BAPN, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddBenefitsAssistance(BenefitsAssistanceModel benefitsAssistanceModel)
        {
            return new HttpResult<Response<BenefitsAssistanceModel>>(benefitsAssistanceRuleEngine.AddBenefitsAssistance(benefitsAssistanceModel), Request);
        }

        /// <summary>
        /// Updates the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceModel">The benefits assistance model.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = BenefitsPermissionKey.Benefits_BAPN_BAPN, Permissions = new string[] { Permission.Create, Permission.Update })]
        [HttpPut]
        public IHttpActionResult UpdateBenefitsAssistance(BenefitsAssistanceModel benefitsAssistanceModel)
        {
            return new HttpResult<Response<BenefitsAssistanceModel>>(benefitsAssistanceRuleEngine.UpdateBenefitsAssistance(benefitsAssistanceModel), Request);
        }

        /// <summary>
        /// Deletes the benefits assistance.
        /// </summary>
        /// <param name="benefitsAssistanceID">The benefits assistance identifier.</param>
        /// <param name="modifiedOn">The modified on.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = BenefitsPermissionKey.Benefits_BAPN_BAPN, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteBenefitsAssistance(long benefitsAssistanceID, DateTime modifiedOn)
        {
            return new HttpResult<Response<BenefitsAssistanceModel>>(benefitsAssistanceRuleEngine.DeleteBenefitsAssistance(benefitsAssistanceID, modifiedOn), Request);
        }
    }
}
