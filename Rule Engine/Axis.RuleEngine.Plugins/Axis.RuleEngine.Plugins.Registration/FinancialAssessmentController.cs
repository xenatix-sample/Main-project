using System;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration;
using System.Web.Http;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Registration
{
    /// <summary>
    /// Controller class for Financial Assessment
    /// </summary>
    public class FinancialAssessmentController : BaseApiController
    {
        readonly IFinancialAssessmentRuleEngine financialAssessmentRuleEngine;

        public FinancialAssessmentController(IFinancialAssessmentRuleEngine financialAssessmentRuleEngine)
        {
            this.financialAssessmentRuleEngine = financialAssessmentRuleEngine;
        }

        /// <summary>
        /// To get the financial assessment details of contact id 
        /// </summary>
        /// <param name="contactID">contact id for which financial assessment data will fetch</param> 
        /// <param name="financialAssessmentID">financialAssessmentID</param>
        /// <returns>IHttpActionResult of financial assesment model type</returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_HouseholdIncome, ECIPermissionKey.ECI_Registration_HouseholdIncome, BenefitsPermissionKey.Benefits_HouseholdIncome_HouseholdIncome, BenefitsPermissionKey.Benefits_SelfPay_SelfPay }, Permission = Permission.Read)]
        public IHttpActionResult GetFinancialAssessment(long contactID, long financialAssessmentID)
        {
            return new HttpResult<Response<FinancialAssessmentModel>>(financialAssessmentRuleEngine.GetFinancialAssessment(contactID,financialAssessmentID), Request);
        }

        /// <summary>
        /// To add the financial assessment for contact id 
        /// </summary>
        /// <param name="financialAssessment">model of FinancialAssessment </param>
        /// <returns>IHttpActionResult of financial assesment model type</returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_HouseholdIncome, ECIPermissionKey.ECI_Registration_HouseholdIncome, BenefitsPermissionKey.Benefits_HouseholdIncome_HouseholdIncome }, Permissions = new string[] { Permission.Create, Permission.Update })]
        public IHttpActionResult AddFinancialAssessment(FinancialAssessmentModel financialAssessment)
        {
            return new HttpResult<Response<FinancialAssessmentModel>>(financialAssessmentRuleEngine.AddFinancialAssessment(financialAssessment), Request);
        }

        /// <summary>
        /// To update the financial assessment for contact id 
        /// </summary>
        /// <param name="financialAssessment">model of FinancialAssessment </param>
        /// <returns>IHttpActionResult of financial assesment model type</returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_HouseholdIncome, ECIPermissionKey.ECI_Registration_HouseholdIncome, BenefitsPermissionKey.Benefits_HouseholdIncome_HouseholdIncome }, Permissions = new string[] { Permission.Create, Permission.Update })]
        public IHttpActionResult UpdateFinancialAssessment(FinancialAssessmentModel financialAssessment)
        {
            return new HttpResult<Response<FinancialAssessmentModel>>(financialAssessmentRuleEngine.UpdateFinancialAssessment(financialAssessment), Request);
        }

        /// <summary>
        /// To add the financial assessment details for contact id 
        /// </summary>
        /// <param name="financialAssessmentDetails">model of FinancialAssessment </param>
        /// <returns>IHttpActionResult of financial assesment model type</returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_HouseholdIncome, ECIPermissionKey.ECI_Registration_HouseholdIncome, BenefitsPermissionKey.Benefits_HouseholdIncome_HouseholdIncome }, Permission = Permission.Create)]
        public IHttpActionResult AddFinancialAssessmentDetails(FinancialAssessmentDetailsModel financialAssessmentDetails)
        {
            return new HttpResult<Response<FinancialAssessmentDetailsModel>>(financialAssessmentRuleEngine.AddFinancialAssessmentDetails(financialAssessmentDetails), Request);
        }

        /// <summary>
        /// To update the financial assessment details for contact id 
        /// </summary>
        /// <param name="financialAssessmentDetails">model of FinancialAssessment </param>
        /// <returns>IHttpActionResult of financial assesment model type</returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_HouseholdIncome, ECIPermissionKey.ECI_Registration_HouseholdIncome, BenefitsPermissionKey.Benefits_HouseholdIncome_HouseholdIncome }, Permission = Permission.Update)]
        public IHttpActionResult UpdateFinancialAssessmentDetails(FinancialAssessmentDetailsModel financialAssessmentDetails)
        {
            return new HttpResult<Response<FinancialAssessmentDetailsModel>>(financialAssessmentRuleEngine.UpdateFinancialAssessmentDetails(financialAssessmentDetails), Request);
        }

        /// <summary>
        /// Delete Financial Assessment Detail
        /// </summary>
        /// <param name="financialAssessmentDetailID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_HouseholdIncome, ECIPermissionKey.ECI_Registration_HouseholdIncome, BenefitsPermissionKey.Benefits_HouseholdIncome_HouseholdIncome }, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteFinancialAssessmentDetail(long financialAssessmentDetailID, DateTime modifiedOn)
        {
            return new HttpResult<Response<bool>>(financialAssessmentRuleEngine.DeleteFinancialAssessmentDetail(financialAssessmentDetailID, modifiedOn), Request);
        }

    }
}
