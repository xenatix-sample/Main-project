using System;
using Axis.Helpers.Validation;
using Axis.Model.Common;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Model.Clinical;
using Axis.RuleEngine.Helpers.Results;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Axis.RuleEngine.Clinical.ChiefComplaint;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Clinical
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ChiefComplaintController : ApiController
    {
        private readonly IChiefComplaintRuleEngine _ruleEngine;

        public ChiefComplaintController(IChiefComplaintRuleEngine ruleEngine)
        {
            this._ruleEngine = ruleEngine;
        }

        [HttpGet]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_ChiefComplaint_ChiefComplaint, Permission = Permission.Read)]
        public IHttpActionResult GetChiefComplaint(long contactID)
        {
            return new HttpResult<Response<ChiefComplaintModel>>(_ruleEngine.GetChiefComplaint(contactID), Request);
        }

        [HttpGet]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_ChiefComplaint_ChiefComplaint, Permission = Permission.Read)]
        public IHttpActionResult GetChiefComplaints(long contactID)
        {
            return new HttpResult<Response<ChiefComplaintModel>>(_ruleEngine.GetChiefComplaints(contactID), Request);
        }

        [HttpPost]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_ChiefComplaint_ChiefComplaint, Permission = Permission.Create)]
        public IHttpActionResult AddChiefComplaint(ChiefComplaintModel chiefComplaint)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<ChiefComplaintModel>>(_ruleEngine.AddChiefComplaint(chiefComplaint), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<ChiefComplaintModel>() { DataItems = new List<ChiefComplaintModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<ChiefComplaintModel>>(validationResponse, Request);
            }
        }

        [HttpPut]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_ChiefComplaint_ChiefComplaint, Permission = Permission.Update)]
        public IHttpActionResult UpdateChiefComplaint(ChiefComplaintModel chiefComplaint)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<ChiefComplaintModel>>(_ruleEngine.UpdateChiefComplaint(chiefComplaint), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<ChiefComplaintModel>() { DataItems = new List<ChiefComplaintModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<ChiefComplaintModel>>(validationResponse, Request);
            }
        }

        [HttpDelete]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_ChiefComplaint_ChiefComplaint, Permission = Permission.Delete)]
        public IHttpActionResult DeleteChiefComplaint(long chiefComplaintID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ChiefComplaintModel>>(_ruleEngine.DeleteChiefComplaint(chiefComplaintID, modifiedOn), Request);
        }

    }
}
