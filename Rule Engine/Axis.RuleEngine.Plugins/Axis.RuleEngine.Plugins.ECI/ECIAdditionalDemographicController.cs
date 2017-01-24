using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.RuleEngine.ECI;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using System;
using System.Collections.Generic;
using System.Web.Http;

using Axis.Helpers.Validation;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.ECI
{
    public class ECIAdditionalDemographicController : BaseApiController
    {
        readonly IECIAdditionalDemographicRuleEngine _eciAdditionalDemographicsRuleEngine;

        public ECIAdditionalDemographicController(IECIAdditionalDemographicRuleEngine eciAdditionalDemographicsRuleEngine)
        {
            _eciAdditionalDemographicsRuleEngine = eciAdditionalDemographicsRuleEngine;
        }

        [Authorization(PermissionKeys = new string[] { ECIPermissionKey.ECI_Registration_AdditionalDemographics, ConsentsPermissionKey.Consents_Assessment_Agency }, Modules = new string[] { Module.General,Module.Benefits,Module.Intake,Module.Consents }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetAdditionalDemographic(long contactId)
        {
            return new HttpResult<Response<ECIAdditionalDemographicsModel>>(_eciAdditionalDemographicsRuleEngine.GetAdditionalDemographic(contactId), Request);
        }

        [Authorization(PermissionKeys = new string[] { ECIPermissionKey.ECI_Registration_AdditionalDemographics, GeneralPermissionKey.General_General_AdditionalDemographics }, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddAdditionalDemographic(ECIAdditionalDemographicsModel additional)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<ECIAdditionalDemographicsModel>>(_eciAdditionalDemographicsRuleEngine.AddAdditionalDemographic(additional), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<ECIAdditionalDemographicsModel>() { DataItems = new List<ECIAdditionalDemographicsModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<ECIAdditionalDemographicsModel>>(validationResponse, Request);
            }
        }

        [Authorization(PermissionKeys = new string[] { ECIPermissionKey.ECI_Registration_AdditionalDemographics, GeneralPermissionKey.General_General_AdditionalDemographics }, Permission = Permission.Update)]
        [HttpPost]
        public IHttpActionResult UpdateAdditionalDemographic(ECIAdditionalDemographicsModel additional)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<ECIAdditionalDemographicsModel>>(_eciAdditionalDemographicsRuleEngine.UpdateAdditionalDemographic(additional), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<ECIAdditionalDemographicsModel>() { DataItems = new List<ECIAdditionalDemographicsModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<ECIAdditionalDemographicsModel>>(validationResponse, Request);
            }
        }
        [Authorization(PermissionKeys = new string[] { ECIPermissionKey.ECI_Registration_AdditionalDemographics, GeneralPermissionKey.General_General_AdditionalDemographics }, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteAdditionalDemographic(long id, DateTime modifiedOn)
        {
            throw new NotImplementedException();
        }
    }
}
