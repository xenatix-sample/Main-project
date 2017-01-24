using System;
using Axis.Model.Common;
using Axis.Model.Common.Assessment;
using Axis.RuleEngine.Assessment;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using System.Collections.Generic;
using System.Web.Http;

namespace Axis.RuleEngine.Service.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AssessmentController : BaseApiController
    {
        /// <summary>
        /// The assessment data provider
        /// </summary>
        private readonly IAssessmentRuleEngine assessmentRuleEngine = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentController" /> class.
        /// </summary>
        /// <param name="assessmentRuleEngine">The assessment data provider.</param>
        public AssessmentController(IAssessmentRuleEngine assessmentRuleEngine)
        {
            this.assessmentRuleEngine = assessmentRuleEngine;
        }

        /// <summary>
        /// Gets the assessment.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAssessment(long? assessmentId)
        {
            return new HttpResult<Response<AssessmentModel>>(assessmentRuleEngine.GetAssessment(assessmentId), Request);
        }

        /// <summary>
        /// Gets the assessment sections.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAssessmentSections(long? assessmentId)
        {
            return new HttpResult<Response<AssessmentSectionsModel>>(assessmentRuleEngine.GetAssessmentSections(assessmentId), Request);
        }

        /// <summary>
        /// Gets the assessment questions.
        /// </summary>
        /// <param name="assessmentSectionId">The assessment section identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAssessmentQuestions(long? assessmentSectionId)
        {
            return new HttpResult<Response<AssessmentQuestionModel>>(assessmentRuleEngine.GetAssessmentQuestions(assessmentSectionId), Request);
        }


        [HttpGet]
        public IHttpActionResult GetAssessmentQuestionsLogic(long? assessmentSectionId)
        {
            return new HttpResult<Response<AssessmentQuestionLogicModel>>(assessmentRuleEngine.GetAssessmentQuestionsLogic(assessmentSectionId), Request);
        }


        /// <summary>
        /// Gets the assessment responses.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAssessmentResponses(long contactId, long assessmentId)
        {
            return new HttpResult<Response<AssessmentResponseModel>>(assessmentRuleEngine.GetAssessmentResponses(contactId, assessmentId), Request);
        }

        /// <summary>
        /// Gets the assessment response.
        /// </summary>
        /// <param name="responseId">The response identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAssessmentResponse(long responseId)
        {
            return new HttpResult<Response<AssessmentResponseModel>>(assessmentRuleEngine.GetAssessmentResponse(responseId), Request);
        }

        /// <summary>
        /// Gets the assessment response details.
        /// </summary>
        /// <param name="responseId">The response identifier.</param>
        /// <param name="sectionId">The section identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAssessmentResponseDetails(long responseId, long sectionId)
        {
            return new HttpResult<Response<AssessmentResponseDetailsModel>>(assessmentRuleEngine.GetAssessmentResponseDetails(responseId, sectionId), Request);
        }

        /// <summary>
        /// Adds the assessment response.
        /// </summary>
        /// <param name="assessmentResponse">The assessment response.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddAssessmentResponse(AssessmentResponseModel assessmentResponse)
        {
            return new HttpResult<Response<AssessmentResponseModel>>(assessmentRuleEngine.AddAssessmentResponse(assessmentResponse), Request);
        }

        /// <summary>
        /// Saves the assessment response details.
        /// </summary>
        /// <param name="assessmentResponseDetails">The assessment response details.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveAssessmentResponseDetails(AssessmentResponseDetail assessmentResponseDetails)
        {
            return new HttpResult<Response<AssessmentResponseDetail>>(assessmentRuleEngine.SaveAssessmentResponseDetails(assessmentResponseDetails), Request);
        }

        /// <summary>
        /// Updates the assessment response.
        /// </summary>
        /// <param name="assessmentResponse">The assessment response.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateAssessmentResponse(AssessmentResponseModel assessmentResponse)
        {
            return new HttpResult<Response<AssessmentResponseModel>>(assessmentRuleEngine.UpdateAssessmentResponse(assessmentResponse), Request);
        }

        /// <summary>
        /// Gets the assessment response list.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAssessmentResponseListByContactID(long contactID)
        {
            return new HttpResult<Response<AssessmentResponseList>>(assessmentRuleEngine.GetAssessmentResponseListByContactID(contactID), Request);
        }

        /// <summary>
        /// Deletes the assessment.
        /// </summary>
        /// <param name="assessmentId"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteAssessment(long assessmentId, DateTime modifiedOn)
        {
            return new HttpResult<Response<bool>>(assessmentRuleEngine.DeleteAssessment(assessmentId, modifiedOn), Request);
        }

        /// <summary>
        /// Gets the Assessment Document Type ID.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAssessmentDocumentTypeID(long? assessmentId)
        {
            return new HttpResult<Response<ScalarResult<int>>>(assessmentRuleEngine.GetAssessmentDocumentTypeID(assessmentId), Request);
        }
    }
}
