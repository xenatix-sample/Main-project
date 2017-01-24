using System;
using System.Web.Http;
using Axis.Model.Clinical.PresentIllness;
using Axis.Model.Common;
using Axis.RuleEngine.Clinical.PresentIllness;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Clinical
{
    public class PresentIllnessController : BaseApiController
    {
        #region Class Variables

        readonly IPresentIllnessRuleEngine _PresentIllnessRuleEngine = null;

        #endregion

         #region Constructors

        public PresentIllnessController(IPresentIllnessRuleEngine PresentIllnessRuleEngine)
        {
            _PresentIllnessRuleEngine = PresentIllnessRuleEngine;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_PresentIllness_PresentIllness, Permission = Permission.Read)]
        public IHttpActionResult GetHPIBundle(long contactID)
        {
            return new HttpResult<Response<PresentIllnessModel>>(_PresentIllnessRuleEngine.GetHPIBundle(contactID), Request);
        }

        [HttpGet]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_PresentIllness_PresentIllness, Permission = Permission.Read)]
        public IHttpActionResult GetHPI(long HPIID)
        {
            return new HttpResult<Response<PresentIllnessModel>>(_PresentIllnessRuleEngine.GetHPI(HPIID), Request);
        }

        [HttpGet]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_PresentIllness_PresentIllness, Permission = Permission.Read)]
        public IHttpActionResult GetHPIDetail(long HPIID)
        {
            return new HttpResult<Response<PresentIllnessDetailModel>>(_PresentIllnessRuleEngine.GetHPIDetail(HPIID), Request);
        }

        [HttpDelete]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_PresentIllness_PresentIllness, Permission = Permission.Delete)]
        public IHttpActionResult DeleteHPI(long HPIID, DateTime modifiedOn)
        {
            return new HttpResult<Response<PresentIllnessModel>>(_PresentIllnessRuleEngine.DeleteHPI(HPIID, modifiedOn), Request);
        }

        [HttpDelete]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_PresentIllness_PresentIllness, Permission = Permission.Delete)]
        public IHttpActionResult DeleteHPIDetail(long HPIDetailID, DateTime modifiedOn)
        {
            return new HttpResult<Response<PresentIllnessDetailModel>>(_PresentIllnessRuleEngine.DeleteHPIDetail(HPIDetailID, modifiedOn), Request);
        }

        [HttpPost]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_PresentIllness_PresentIllness, Permission = Permission.Create)]
        public IHttpActionResult AddHPI(PresentIllnessModel HPI)
        {
            return new HttpResult<Response<PresentIllnessModel>>(_PresentIllnessRuleEngine.AddHPI(HPI), Request);
        }

        [HttpPost]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_PresentIllness_PresentIllness, Permission = Permission.Create)]
        public IHttpActionResult AddHPIDetail(PresentIllnessDetailModel HPI)
        {
            return new HttpResult<Response<PresentIllnessDetailModel>>(_PresentIllnessRuleEngine.AddHPIDetail(HPI), Request);
        }


        [HttpPut]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_PresentIllness_PresentIllness, Permission = Permission.Update)]
        public IHttpActionResult UpdateHPI(PresentIllnessModel HPI)
        {
            return new HttpResult<Response<PresentIllnessModel>>(_PresentIllnessRuleEngine.UpdateHPI(HPI), Request);
        }

        [HttpPut]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_PresentIllness_PresentIllness, Permission = Permission.Update)]
        public IHttpActionResult UpdateHPIDetail(PresentIllnessDetailModel HPI)
        {
            return new HttpResult<Response<PresentIllnessDetailModel>>(_PresentIllnessRuleEngine.UpdateHPIDetail(HPI), Request);
        }

        #endregion
        }

}
