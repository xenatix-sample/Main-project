using System;
using Axis.Model.Common;
using Axis.Model.Common.Assessment;
using System.Collections.Generic;

namespace Axis.Service.Assessment
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAssessmentService
    {
        /// <summary>
        /// Gets the assessment.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        Response<AssessmentModel> GetAssessment(long? assessmentId);

        /// <summary>
        /// Gets the assessment sections.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        Response<AssessmentSectionsModel> GetAssessmentSections(long? assessmentId);

        /// <summary>
        /// Get assessment based on logic table.
        /// </summary>
        /// <param name="assessmentSectionId"></param>
        /// <returns></returns>
        Response<AssessmentQuestionLogicModel> GetAssessmentQuestionsLogic(long? assessmentSectionId);

        /// <summary>
        /// Gets the assessment questions.
        /// </summary>
        /// <param name="assessmentSectionId">The assessment section identifier.</param>
        /// <returns></returns>
        Response<AssessmentQuestionModel> GetAssessmentQuestions(long? assessmentSectionId);

        /// <summary>
        /// Gets the assessment responses.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        Response<AssessmentResponseModel> GetAssessmentResponses(long contactId, long assessmentId);

        /// <summary>
        /// Gets the assessment response.
        /// </summary>
        /// <param name="responseId">The response identifier.</param>
        /// <returns></returns>
        Response<AssessmentResponseModel> GetAssessmentResponse(long responseId);

        /// <summary>
        /// Gets the assessment response details.
        /// </summary>
        /// <param name="responseId">The response identifier.</param>
        /// <param name="sectionId">The section identifier.</param>
        /// <returns></returns>
        Response<AssessmentResponseDetailsModel> GetAssessmentResponseDetails(long responseId, long sectionId);

        /// <summary>
        /// Adds the assessment response.
        /// </summary>
        /// <param name="assessmentResponse">The assessment response.</param>
        /// <returns></returns>
        Response<AssessmentResponseModel> AddAssessmentResponse(AssessmentResponseModel assessmentResponse);

        /// <summary>
        /// Saves the assessment response details.
        /// </summary>
        /// <param name="assessmentResponseDetails">The assessment response details.</param>
        /// <returns></returns>
        Response<AssessmentResponseDetail> SaveAssessmentResponseDetails(AssessmentResponseDetail assessmentResponseDetails);

        /// <summary>
        /// Updates the assessment response.
        /// </summary>
        /// <param name="assessmentResponse">The assessment response.</param>
        /// <returns></returns>
        Response<AssessmentResponseModel> UpdateAssessmentResponse(AssessmentResponseModel assessmentResponse);

        /// <summary>
        /// Gets the assessment response list.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        Response<AssessmentResponseList> GetAssessmentResponseListByContactID(long contactID);

        /// <summary>
        /// Deletes the assessment.
        /// </summary>
        /// <param name="assessmentId"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<bool> DeleteAssessment(long assessmentId, DateTime modifiedOn);

        /// <summary>
        /// Gets the Assessment Document Type ID.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        Response<ScalarResult<int>> GetAssessmentDocumentTypeID(long? assessmentId);
    }
}
