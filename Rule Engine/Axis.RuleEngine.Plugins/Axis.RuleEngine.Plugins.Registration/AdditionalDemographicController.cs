using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Axis.Helpers.Validation;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;
using System.Web.Http.Cors;

namespace Axis.RuleEngine.Plugins.Registration
{

    public class AdditionalDemographicController : BaseApiController
    {
        readonly IAdditionalDemographicRuleEngine _additionalDemographicsRuleEngine;

        public AdditionalDemographicController(IAdditionalDemographicRuleEngine additionalDemographicsRuleEngine)
        {
            _additionalDemographicsRuleEngine = additionalDemographicsRuleEngine;
        }

        [Authorization(PermissionKeys = new String[] { CallCenterPermissionKey.CallCenter_CrisisLine, RegistrationPermissionKey.Registration_AdditionalDemography, ConsentsPermissionKey.Consents_Assessment_Agency, CallCenterPermissionKey.CallCenter_LawLiaison,BusinessAdministrationPermissionKey.BusinessAdministration_ClientMerge_ClientMerge}, Modules = new string[] { Module.General, Module.Benefits, Module.Intake, Module.Consents, Module.Chart }, Permission = Permission.Read)]
        public IHttpActionResult GetAdditionalDemographic(long contactId)
        {
            return new HttpResult<Response<AdditionalDemographicsModel>>(_additionalDemographicsRuleEngine.GetAdditionalDemographic(contactId), Request);
        }

        [Authorization(PermissionKeys = new String[] { CallCenterPermissionKey.CallCenter_CrisisLine, RegistrationPermissionKey.Registration_AdditionalDemography, GeneralPermissionKey.General_General_AdditionalDemographics, CallCenterPermissionKey.CallCenter_LawLiaison }, Permissions = new string[] { Permission.Create,Permission.Update })]
        public IHttpActionResult AddAdditionalDemographic(AdditionalDemographicsModel additional)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<AdditionalDemographicsModel>>(_additionalDemographicsRuleEngine.AddAdditionalDemographic(additional), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<AdditionalDemographicsModel>() { DataItems = new List<AdditionalDemographicsModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<AdditionalDemographicsModel>>(validationResponse, Request);
            }
        }

        [Authorization(PermissionKeys = new String[] { CallCenterPermissionKey.CallCenter_CrisisLine, RegistrationPermissionKey.Registration_AdditionalDemography, GeneralPermissionKey.General_General_AdditionalDemographics, CallCenterPermissionKey.CallCenter_LawLiaison }, Permissions = new string[] { Permission.Create, Permission.Update })]
        public IHttpActionResult UpdateAdditionalDemographic(AdditionalDemographicsModel additional)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<AdditionalDemographicsModel>>(_additionalDemographicsRuleEngine.UpdateAdditionalDemographic(additional), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<AdditionalDemographicsModel>() { DataItems = new List<AdditionalDemographicsModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<AdditionalDemographicsModel>>(validationResponse, Request);
            }
        }

        [Authorization(PermissionKeys = new String[] { CallCenterPermissionKey.CallCenter_CrisisLine, RegistrationPermissionKey.Registration_AdditionalDemography, GeneralPermissionKey.General_General_AdditionalDemographics, CallCenterPermissionKey.CallCenter_LawLiaison }, Permission = Permission.Delete)]
        public IHttpActionResult DeleteAdditionalDemographic(long id, DateTime modifiedOn)
        {
            throw new NotImplementedException();
        }
    }
}
