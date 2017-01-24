using System;
using Axis.Model.Common;
using Axis.Model.Common.Assessment;
using Axis.Service.Assessment;
using System.Collections.Generic;

namespace Axis.RuleEngine.Assessment
{
    /// <summary>
    /// 
    /// </summary>
    public class AssessmentRuleEngine : IAssessmentRuleEngine
    {
        /// <summary>
        /// The assessment service
        /// </summary>
        private IAssessmentService assessmentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentRuleEngine" /> class.
        /// </summary>
        /// <param name="assessmentService">The assessment service.</param>
        public AssessmentRuleEngine(IAssessmentService assessmentService)
        {
            this.assessmentService = assessmentService;
        }

        /// <summary>
        /// Gets the assessment.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        public Response<AssessmentModel> GetAssessment(long? assessmentId)
        {
            return assessmentService.GetAssessment(assessmentId);
        }


        public Response<AssessmentQuestionLogicModel> GetAssessmentQuestionsLogic(long? assessmentSectionId)
        {
            return assessmentService.GetAssessmentQuestionsLogic(assessmentSectionId);
        }


        /// <summary>
        /// Gets the assessment sections.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        public Response<AssessmentSectionsModel> GetAssessmentSections(long? assessmentId)
        {
            return assessmentService.GetAssessmentSections(assessmentId);
        }

        /// <summary>
        /// Gets the assessment questions.
        /// </summary>
        /// <param name="assessmentSectionId">The assessment section identifier.</param>
        /// <returns></returns>
        public Response<AssessmentQuestionModel> GetAssessmentQuestions(long? assessmentSectionId)
        {
            return assessmentService.GetAssessmentQuestions(assessmentSectionId);
        }

        /// <summary>
        /// Gets the assessment responses.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        public Response<AssessmentResponseModel> GetAssessmentResponses(long contactId, long assessmentId)
        {
            return assessmentService.GetAssessmentResponses(contactId, assessmentId);
        }

        /// <summary>
        /// Gets the assessment response.
        /// </summary>
        /// <param name="responseId">The response identifier.</param>
        /// <returns></returns>
        public Response<AssessmentResponseModel> GetAssessmentResponse(long responseId)
        {
            return assessmentService.GetAssessmentResponse(responseId);
        }

        /// <summary>
        /// Gets the assessment response details.
        /// </summary>
        /// <param name="responseId">The response identifier.</param>
        /// <param name="sectionId">The section identifier.</param>
        /// <returns></returns>
        public Response<AssessmentResponseDetailsModel> GetAssessmentResponseDetails(long responseId, long sectionId)
        {
            return assessmentService.GetAssessmentResponseDetails(responseId, sectionId);
        }

        /// <summary>
        /// Adds the assessment response.
        /// </summary>
        /// <param name="assessmentResponse">The assessment response.</param>
        /// <returns></returns>
        public Response<AssessmentResponseModel> AddAssessmentResponse(AssessmentResponseModel assessmentResponse)
        {
            return assessmentService.AddAssessmentResponse(assessmentResponse);
        }

        /// <summary>
        /// Saves the assessment response details.
        /// </summary>
        /// <param name="assessmentResponseDetails">The assessment question.</param>
        /// <returns></returns>
        public Response<AssessmentResponseDetail> SaveAssessmentResponseDetails(AssessmentResponseDetail assessmentResponseDetails)
        {
            return assessmentService.SaveAssessmentResponseDetails(assessmentResponseDetails);
        }

        /// <summary>
        /// Updates the assessment response.
        /// </summary>
        /// <param name="assessmentResponse">The assessment response.</param>
        /// <returns></returns>
        public Response<AssessmentResponseModel> UpdateAssessmentResponse(AssessmentResponseModel assessmentResponse)
        {
            return assessmentService.UpdateAssessmentResponse(assessmentResponse);
        }

        /// <summary>
        /// Gets the assessment response list.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        public Response<AssessmentResponseList> GetAssessmentResponseListByContactID(long contactID)
        {
            return assessmentService.GetAssessmentResponseListByContactID(contactID);
        }

        /// <summary>
        /// Deletes the assessment.
        /// </summary>
        /// <param name="assessmentId"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<bool> DeleteAssessment(long assessmentId, DateTime modifiedOn)
        {
            return assessmentService.DeleteAssessment(assessmentId, modifiedOn);
        }

        /// <summary>
        /// Gets the Assessment Document Type ID.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        public Response<ScalarResult<int>> GetAssessmentDocumentTypeID(long? assessmentId)
        {
            return assessmentService.GetAssessmentDocumentTypeID(assessmentId);
        }
    }
}
