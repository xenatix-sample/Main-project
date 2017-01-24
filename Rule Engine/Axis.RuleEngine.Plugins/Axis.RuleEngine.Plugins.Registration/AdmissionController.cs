using System;
using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration;
using System.Web.Http;
using Axis.Helpers.Validation;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Registration.Admission;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Registration
{
    /// <summary>
    /// Controller for admission
    /// </summary>
   
    public class AdmissionController : BaseApiController
    {
        readonly IAdmissionRuleEngine _admissionRuleEngine;

        public AdmissionController(IAdmissionRuleEngine admissionRuleEngine)
        {
            this._admissionRuleEngine = admissionRuleEngine;
        }

        /// <summary>
        /// Gets the Admission.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [Authorization(Modules = new string[] { Module.Benefits, Module.Intake }, PermissionKeys = new string[] { GeneralPermissionKey.General_General_Admission, GeneralPermissionKey.General_General_Admission_Discharge, RegistrationPermissionKey.Registration_Demography, ECIPermissionKey.ECI_Registration_Demographics, CallCenterPermissionKey.CallCenter_LawLiaison }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetAdmission(long contactID)
        {
            return new HttpResult<Response<AdmissionModal>>(_admissionRuleEngine.GetAdmission(contactID), Request);
        }

        /// <summary>
        /// Add Admission.
        /// </summary>
        /// <param name="admission">The Admission.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { GeneralPermissionKey.General_General_Admission, RegistrationPermissionKey.Registration_Demography, ECIPermissionKey.ECI_Registration_Demographics, CallCenterPermissionKey.CallCenter_LawLiaison, CallCenterPermissionKey.CallCenter_CrisisLine }, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddAdmission(AdmissionModal collateralModel)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<AdmissionModal>>(_admissionRuleEngine.AddAdmission(collateralModel), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<AdmissionModal>() { DataItems = new List<AdmissionModal>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<AdmissionModal>>(validationResponse, Request);
            }
        }
        /// <summary>
        /// Updates admission.
        /// </summary>
        /// <param name="admission">The admission.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { GeneralPermissionKey.General_General_Admission, RegistrationPermissionKey.Registration_Demography, ECIPermissionKey.ECI_Registration_Demographics, CallCenterPermissionKey.CallCenter_LawLiaison }, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateAdmission(AdmissionModal collateralModel)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<AdmissionModal>>(_admissionRuleEngine.UpdateAdmission(collateralModel), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<AdmissionModal>() { DataItems = new List<AdmissionModal>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<AdmissionModal>>(validationResponse, Request);
            }
        }

    }
}
