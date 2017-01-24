using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration;
using System.Web.Http;
using Axis.Helpers.Validation;
using System;
using Axis.Helpers;
using Axis.Constant;
using System.Web.Http.Cors;

namespace Axis.RuleEngine.Service.Controllers
{
    /// <summary>
    ///
    /// </summary>    
    public class RegistrationController : BaseApiController
    {
        /// <summary>
        /// The _registration rule engine
        /// </summary>
        private readonly IRegistrationRuleEngine _registrationRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationController"/> class.
        /// </summary>
        /// <param name="registrationRuleEngine">The registration rule engine.</param>
        public RegistrationController(IRegistrationRuleEngine registrationRuleEngine)
        {
            _registrationRuleEngine = registrationRuleEngine;
        }

        /// <summary>
        /// Gets the contact demographics.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>

        public IHttpActionResult GetContactDemographics(long contactID)
        {
            return new HttpResult<Response<ContactDemographicsModel>>(_registrationRuleEngine.GetContactDemographics(contactID), Request);
        }

        /// <summary>
        /// Gets the Contact Address.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Demography, CallCenterPermissionKey.CallCenter_CrisisLine, GeneralPermissionKey.General_General_Referral, ECIPermissionKey.ECI_Registration_Referral, CallCenterPermissionKey.CallCenter_LawLiaison, BusinessAdministrationPermissionKey.BusinessAdministration_ClientMerge_ClientMerge }, Permission = Permission.Read)]
        public IHttpActionResult GetContactAddress(long contactID)
        {
            return new HttpResult<Response<ContactAddressModel>>(_registrationRuleEngine.GetContactAddress(contactID), Request);
        }

        /// <summary>
        /// Adds the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Demography, CallCenterPermissionKey.CallCenter_CrisisLine, RegistrationPermissionKey.Registration_Registration_Referral, GeneralPermissionKey.General_General_Referral, ECIPermissionKey.ECI_Registration_Referral, CallCenterPermissionKey.CallCenter_LawLiaison }, Permissions = new string[] { Permission.Create, Permission.Update })]
        public IHttpActionResult AddContactDemographics(ContactDemographicsModel contact)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<ContactDemographicsModel>>(_registrationRuleEngine.AddContactDemographics(contact), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<ContactDemographicsModel>() { DataItems = new List<ContactDemographicsModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<ContactDemographicsModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Updates the contact demographics.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        [Authorization(Modules = new string[] { Module.General }, PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Demography, CallCenterPermissionKey.CallCenter_CrisisLine, RegistrationPermissionKey.Registration_Registration_Referral, GeneralPermissionKey.General_General_Referral, ECIPermissionKey.ECI_Registration_Referral, CallCenterPermissionKey.CallCenter_LawLiaison }, Permissions = new string[] { Permission.Create, Permission.Update })]
        public IHttpActionResult UpdateContactDemographics(ContactDemographicsModel contact)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<ContactDemographicsModel>>(_registrationRuleEngine.UpdateContactDemographics(contact), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<ContactDemographicsModel>() { DataItems = new List<ContactDemographicsModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<ContactDemographicsModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Get client records based on the search criteria
        /// </summary>
        /// <param name="SearchCriteria">Search Criteria entered by user</param>
        /// <param name="contactType">contact type of contact</param>
        /// <returns>
        /// ContactDemographicsModel
        /// </returns>
        [HttpGet]
        public IHttpActionResult GetClientSummary(string SearchCriteria, string ContactType)
        {
            if (string.IsNullOrEmpty(SearchCriteria) || string.IsNullOrWhiteSpace(SearchCriteria))
            {
                SearchCriteria = "";
            }
            return new HttpResult<Response<ContactDemographicsModel>>(_registrationRuleEngine.GetClientSummary(SearchCriteria, ContactType), Request);
        }

        /// <summary>
        /// Verifies the duplicate contacts.
        /// </summary>
        /// <param name="contact">The contact.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Demography, ECIPermissionKey.ECI_Registration_Demographics, GeneralPermissionKey.General_General_Referral, CallCenterPermissionKey.CallCenter_CrisisLine, CallCenterPermissionKey.CallCenter_LawLiaison }, Permission = Permission.Read)]
        [HttpPost]
        public IHttpActionResult VerifyDuplicateContacts(ContactDemographicsModel contact)
        {
            return new HttpResult<Response<ContactDemographicsModel>>(_registrationRuleEngine.VerifyDuplicateContacts(contact), Request);
        }

        /// <summary>
        /// Gets the SSN.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Demography, CallCenterPermissionKey.CallCenter_CrisisLine, ECIPermissionKey.ECI_Registration_Collateral, ECIPermissionKey.ECI_Registration_Demographics, CallCenterPermissionKey.CallCenter_LawLiaison, IntakeFormsPermissionKey.Intake_IDD_Forms, GeneralPermissionKey.General_General_Demographics, BusinessAdministrationPermissionKey.BusinessAdministration_ClientMerge_ClientMerge }, Permission = Permission.Read)]
        public IHttpActionResult GetSSN(long contactID)
        {
            return new HttpResult<Response<String>>(_registrationRuleEngine.GetSSN(contactID), Request);
        }
    }
}