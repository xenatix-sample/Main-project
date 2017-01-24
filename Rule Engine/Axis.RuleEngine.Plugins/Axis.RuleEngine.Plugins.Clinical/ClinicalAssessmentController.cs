using System;
using Axis.Model.Clinical.Assessment;
using Axis.Model.Common;
using Axis.RuleEngine.Clinical.Assessment;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using System.Collections.Generic;
using System.Web.Http;
using Axis.Helpers.Validation;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Clinical
{
    public class ClinicalAssessmentController : BaseApiController
    {
        readonly IAssessmentRuleEngine _assessmentRuleEngine;

        public ClinicalAssessmentController(IAssessmentRuleEngine assessmentRuleEngine)
        {
            this._assessmentRuleEngine = assessmentRuleEngine;
        }

        /// <summary>
        /// Get assessment  
        /// </summary>
        /// <param name="clinicalAssessmentID">Contact Id</param>
        /// <returns></returns>
        [HttpGet]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Assessment_Assessment, Permission = Permission.Read)]
        public IHttpActionResult GetClinicalAssessments(long clinicalAssessmentID)
        {
            return new HttpResult<Response<ClinicalAssessmentModel>>(_assessmentRuleEngine.GetClinicalAssessments(clinicalAssessmentID), Request);
        }

        /// <summary>
        /// Get assessment list 
        /// </summary>
        /// <param name="contactID">Contact Id</param>
        /// <returns></returns>
        [HttpGet]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Assessment_Assessment, Permission = Permission.Read)]
        public IHttpActionResult GetClinicalAssessmentsByContact(long contactID)
        {
            return new HttpResult<Response<ClinicalAssessmentModel>>(_assessmentRuleEngine.GetClinicalAssessmentsByContact(contactID), Request);
        }



        /// <summary>
        /// Add assessment 
        /// </summary>
        /// <param name="assessmentModel">assessment model</param>
        /// <returns></returns>
        [HttpPost]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Assessment_Assessment, Permission = Permission.Create)]
        public IHttpActionResult AddAssessment(ClinicalAssessmentModel assessmentModel)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<ClinicalAssessmentModel>>(_assessmentRuleEngine.AddAssessment(assessmentModel), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<ClinicalAssessmentModel>() { DataItems = new List<ClinicalAssessmentModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<ClinicalAssessmentModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Update assessment for model
        /// </summary>
        /// <param name="assessment">assessment model</param>
        /// <returns></returns>
        [HttpPut]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Assessment_Assessment, Permission = Permission.Update)]
        public IHttpActionResult UpdateAssessment(ClinicalAssessmentModel assessment)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<ClinicalAssessmentModel>>(_assessmentRuleEngine.UpdateAssessment(assessment), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<ClinicalAssessmentModel>() { DataItems = new List<ClinicalAssessmentModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<ClinicalAssessmentModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Delete assessment
        /// </summary>
        /// <param name="Id">Contact Id of assessment</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Assessment_Assessment, Permission = Permission.Delete)]
        public IHttpActionResult DeleteAssessment(long Id, DateTime modifiedOn)
        {
            return new HttpResult<Response<ClinicalAssessmentModel>>(_assessmentRuleEngine.DeleteAssessment(Id, modifiedOn), Request);
        }
    }
}
