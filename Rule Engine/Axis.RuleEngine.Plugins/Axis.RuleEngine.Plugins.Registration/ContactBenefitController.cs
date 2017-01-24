using System;
using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration;
using System.Web.Http;
using System.Web.Http.Cors;
using Axis.Helpers.Validation;
using Axis.Model.Common.Lookups.PayorPlanGroup;
using Axis.Model.Common.Lookups.PayorPlan;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Registration
{
    /// <summary>
    ///
    /// </summary>

    public class ContactBenefitController : ApiController
    {
        /// <summary>
        /// The contact benefit rule engine
        /// </summary>
        private readonly IContactBenefitRuleEngine contactBenefitRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactBenefitController"/> class.
        /// </summary>
        /// <param name="contactBenefitRuleEngine">The contact benefit rule engine.</param>
        public ContactBenefitController(IContactBenefitRuleEngine contactBenefitRuleEngine)
        {
            this.contactBenefitRuleEngine = contactBenefitRuleEngine;
        }

        /// <summary>
        /// Gets the contact benefits.
        /// </summary>
        /// <param name="ContactId">The contact identifier.</param>
        /// <returns></returns>
        [Authorization(Modules = new string[] { Module.General, Module.BusinessAdministration }, PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Payors, ECIPermissionKey.ECI_Registration_Payors, CallCenterPermissionKey.CallCenter_CrisisLine, BenefitsPermissionKey.Benefits_Payors_Payors, ConsentsPermissionKey.Consents_Assessment_Agency, LettersPermissionKey.Intake_IDD_Letters, IntakeFormsPermissionKey.Intake_IDD_Forms, CallCenterPermissionKey.CallCenter_LawLiaison, ChartPermissionKey.Chart_Chart_Assessment, ChartPermissionKey.Chart_Chart_RecordedServices, BusinessAdministrationPermissionKey.BusinessAdministration_ClientMerge_ClientMerge }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetContactBenefits(long ContactId)
        {
            return new HttpResult<Response<ContactBenefitModel>>(contactBenefitRuleEngine.GetContactBenefits(ContactId), Request);
        }


        /// <summary>
        /// Gets the payor details.
        /// </summary>
        /// <param name="Entered Data">The search field identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Payors, ECIPermissionKey.ECI_Registration_Payors, CallCenterPermissionKey.CallCenter_CrisisLine, BenefitsPermissionKey.Benefits_Payors_Payors }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetPayors(string searchText)
        {
            return new HttpResult<Response<PayorDetailModel>>(contactBenefitRuleEngine.GetPayors(searchText), Request);
        }

        /// <summary>
        /// Gets the payor group plan for payor.
        /// </summary>
        /// <param name="PayorId">The payor identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Payors, ECIPermissionKey.ECI_Registration_Payors, CallCenterPermissionKey.CallCenter_CrisisLine, BenefitsPermissionKey.Benefits_Payors_Payors }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetPayorPlans(int payorId)
        {
            return new HttpResult<Response<PayorPlan>>(contactBenefitRuleEngine.GetPayorPlans(payorId), Request);
        }

        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Payors, ECIPermissionKey.ECI_Registration_Payors, CallCenterPermissionKey.CallCenter_CrisisLine, BenefitsPermissionKey.Benefits_Payors_Payors }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetGroupsAndAddressForPlan(int planID)
        {
            return new HttpResult<Response<PlanGroupAndAddressesModel>>(contactBenefitRuleEngine.GetGroupsAndAddressForPlan(planID), Request);
        }

        /// <summary>
        /// Adds the contact benefit.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Payors, ECIPermissionKey.ECI_Registration_Payors, CallCenterPermissionKey.CallCenter_CrisisLine, BenefitsPermissionKey.Benefits_Payors_Payors, CallCenterPermissionKey.CallCenter_LawLiaison }, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddContactBenefit(ContactBenefitModel contact)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<ContactBenefitModel>>(contactBenefitRuleEngine.AddContactBenefit(contact), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<ContactBenefitModel>() { DataItems = new List<ContactBenefitModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<ContactBenefitModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Updates the contact benefit.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Payors, ECIPermissionKey.ECI_Registration_Payors, CallCenterPermissionKey.CallCenter_CrisisLine, BenefitsPermissionKey.Benefits_Payors_Payors, CallCenterPermissionKey.CallCenter_LawLiaison }, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateContactBenefit(ContactBenefitModel contact)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<ContactBenefitModel>>(contactBenefitRuleEngine.UpdateContactBenefit(contact), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<ContactBenefitModel>() { DataItems = new List<ContactBenefitModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<ContactBenefitModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Deletes the contact benefit.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Payors, ECIPermissionKey.ECI_Registration_Payors, CallCenterPermissionKey.CallCenter_CrisisLine, BenefitsPermissionKey.Benefits_Payors_Payors, CallCenterPermissionKey.CallCenter_LawLiaison }, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteContactBenefit(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<ContactBenefitModel>>(contactBenefitRuleEngine.DeleteContactBenefit(id, modifiedOn), Request);
        }
    }
}