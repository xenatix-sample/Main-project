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
    /// <summary>
    /// Controller class for Financial Summary
    /// </summary>
    public class FinancialSummaryController : BaseApiController
    {
        private readonly IFinancialSummaryRuleEngine financialSummaryRuleEngine;

        public FinancialSummaryController(IFinancialSummaryRuleEngine financialSummaryRuleEngine)
        {
            this.financialSummaryRuleEngine = financialSummaryRuleEngine;
        }

        /// <summary>
        /// Gets the financial summary.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = BenefitsPermissionKey.Benefits_FinancialAssessment_FinancialAssessment, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetFinancialSummaryById(long financialSummaryID)
        {
            return new HttpResult<Response<FinancialSummaryModel>>(financialSummaryRuleEngine.GetFinancialSummaryById(financialSummaryID), Request);
        }

        /// <summary>
        /// Get all the financial summary.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = BenefitsPermissionKey.Benefits_FinancialAssessment_FinancialAssessment, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetFinancialSummaries(long contactID)
        {
            return new HttpResult<Response<FinancialSummaryModel>>(financialSummaryRuleEngine.GetFinancialSummaries(contactID), Request);
        }

        /// <summary>
        /// Adds the financial summary.
        /// </summary>
        /// <param name="financialSummary">The financial assessment.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = BenefitsPermissionKey.Benefits_FinancialAssessment_FinancialAssessment, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddFinancialSummary(FinancialSummaryModel financialSummary)
        {
            return new HttpResult<Response<FinancialSummaryModel>>(financialSummaryRuleEngine.AddFinancialSummary(financialSummary), Request);
        }

        /// <summary>
        /// Updates the financial summary.
        /// </summary>
        /// <param name="financialSummary">The financial assessment.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = BenefitsPermissionKey.Benefits_FinancialAssessment_FinancialAssessment, Permissions = new String[] { Permission.Create, Permission.Update })]
        [HttpPut]
        public IHttpActionResult UpdateFinancialSummary(FinancialSummaryModel financialSummary)
        {
            return new HttpResult<Response<FinancialSummaryModel>>(financialSummaryRuleEngine.UpdateFinancialSummary(financialSummary), Request);
        }
    }
}