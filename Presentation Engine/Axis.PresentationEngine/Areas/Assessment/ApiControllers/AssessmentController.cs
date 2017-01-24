using System;
using Axis.PresentationEngine.Areas.Assessment.Models;
using Axis.PresentationEngine.Areas.Assessment.Repository;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Collections.Generic;
using System.Web.Http;
using Axis.Model.Common;
using Newtonsoft.Json.Linq;
using Axis.Model.Common.Assessment;

namespace Axis.PresentationEngine.Areas.Assessment.ApiControllers
{
    /// <summary>
    ///
    /// </summary>
    public class AssessmentController : BaseApiController
    {
        /// <summary>
        /// The assessment repository
        /// </summary>
        private readonly IAssessmentRepository assessmentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentController" /> class.
        /// </summary>
        /// <param name="assessmentRepository">The assessment repository.</param>
        public AssessmentController(IAssessmentRepository assessmentRepository)
        {
            this.assessmentRepository = assessmentRepository;
        }

        /// <summary>
        /// Get assessment.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<AssessmentViewModel> GetAssessment(long? assessmentId)
        {
            return assessmentRepository.GetAssessment(assessmentId);
        }

        /// <summary>
        /// Gets the assessment sections.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<AssessmentSectionsViewModel> GetAssessmentSections(long? assessmentId)
        {
            return assessmentRepository.GetAssessmentSections(assessmentId);
        }

        /// <summary>
        /// Gets the assessment questions.
        /// </summary>
        /// <param name="assessmentSectionId">The assessment section identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<AssessmentQuestionViewModel> GetAssessmentQuestions(long? assessmentSectionId)
        {
            return assessmentRepository.GetAssessmentQuestions(assessmentSectionId);
        }

        /// <summary>
        /// Gets the assessment response.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<AssessmentResponseViewModel> GetAssessmentResponses(long contactId, long assessmentId)
        {
            return assessmentRepository.GetAssessmentResponses(contactId, assessmentId);
        }

        /// <summary>
        /// Gets the assessment response.
        /// </summary>
        /// <param name="responseId">The response identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<AssessmentResponseViewModel> GetAssessmentResponse(long responseId)
        {
            return assessmentRepository.GetAssessmentResponse(responseId);
        }

        /// <summary>
        /// Gets the assessment response details.
        /// </summary>
        /// <param name="responseId">The response identifier.</param>
        /// <param name="sectionId">The section identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<AssessmentResponseDetailsViewModel> GetAssessmentResponseDetails(long responseId, long sectionId)
        {
            return assessmentRepository.GetAssessmentResponseDetails(responseId, sectionId);
        }


        /// <summary>
        /// Gets the assessment Logic Mapping.
        /// </summary>
        /// <param name="logicMappingId"></param>
        /// <returns></returns>
        [HttpGet]
        public Response<AssessmentQuestionLogicViewModel> GetAssessmentQuestionsLogic(long assesmentSectionId)
        {
            return assessmentRepository.GetAssessmentQuestionsLogic(assesmentSectionId);
        }


        /// <summary>
        /// Adds the assessment response.
        /// </summary>
        /// <param name="assessmentResponse">The assessment response.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<AssessmentResponseViewModel> AddAssessmentResponse(AssessmentResponseViewModel assessmentResponse)
        {
            return assessmentRepository.AddAssessmentResponse(assessmentResponse);
        }

        /// <summary>
        /// Adds the assessment response details.
        /// </summary>
        /// <param name="responseId">The response identifier.</param>
        /// <param name="jsonData">The assessment response details.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<AssessmentResponseDetail> SaveAssessmentResponseDetails(AssessmentResponseDetail assessmentResponseDetail) 
        {
            return assessmentRepository.SaveAssessmentResponseDetails(assessmentResponseDetail);
        }

        /// <summary>
        /// Updates the assessment response.
        /// </summary>
        /// <param name="assessmentResponse">The assessment response.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<AssessmentResponseViewModel> UpdateAssessmentResponse(AssessmentResponseViewModel assessmentResponse)
        {
            return assessmentRepository.UpdateAssessmentResponse(assessmentResponse);
        }

        /// <summary>
        /// Gets the assessment response list.
        /// </summary>
        /// <param name="responseId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<AssessmentResponseList> GetAssessmentResponseListByContactID(long contactID)
        {
            return assessmentRepository.GetAssessmentResponseListByContactID(contactID);
        }


        /// <summary>
        /// Delete assessment
        /// </summary>
        /// <param name="assessmentId"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public Response<bool> DeleteAssessment(long assessmentId, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return assessmentRepository.DeleteAssessment(assessmentId, modifiedOn);
        }


        /// <summary>
        /// Gets the Assessment Document Type ID.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ScalarResult<int>> GetAssessmentDocumentTypeID(long? assessmentId)
        {
            return assessmentRepository.GetAssessmentDocumentTypeID(assessmentId);
        }
    }
}
