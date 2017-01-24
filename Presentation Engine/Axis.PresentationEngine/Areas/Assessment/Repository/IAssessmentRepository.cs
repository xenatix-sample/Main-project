using System;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Assessment.Models;
using System.Collections.Generic;
using Axis.Model.Common.Assessment;

namespace Axis.PresentationEngine.Areas.Assessment.Repository
{
    /// <summary>
    ///
    /// </summary>
    public interface IAssessmentRepository
    {
        /// <summary>
        /// Gets assessment.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        Response<AssessmentViewModel> GetAssessment(long? assessmentId);

        /// <summary>
        /// Get AssessmentLogicMapping ID.
        /// </summary>
        /// <param name="logicMappingId"></param>
        /// <returns></returns>
        Response<AssessmentQuestionLogicViewModel> GetAssessmentQuestionsLogic(long? assessmentSectionId);
        

        /// <summary>
        /// Gets the assessment sections.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        Response<AssessmentSectionsViewModel> GetAssessmentSections(long? assessmentId);

        /// <summary>
        /// Gets the assessment questions.
        /// </summary>
        /// <param name="assessmentSectionId">The assessment section identifier.</param>
        /// <returns></returns>
        Response<AssessmentQuestionViewModel> GetAssessmentQuestions(long? assessmentSectionId);

        /// <summary>
        /// Gets the assessment responses.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        Response<AssessmentResponseViewModel> GetAssessmentResponses(long contactId, long assessmentId);

        /// <summary>
        /// Gets the assessment response.
        /// </summary>
        /// <param name="responseId">The response identifier.</param>
        /// <returns></returns>
        Response<AssessmentResponseViewModel> GetAssessmentResponse(long responseId);

        /// <summary>
        /// Gets the assessment response details.
        /// </summary>
        /// <param name="responseId">The response identifier.</param>
        /// <param name="sectionId">The section identifier.</param>
        /// <returns></returns>
        Response<AssessmentResponseDetailsViewModel> GetAssessmentResponseDetails(long responseId, long sectionId);

        /// <summary>
        /// Adds the assessment response.
        /// </summary>
        /// <param name="assessmentResponse">The assessment response.</param>
        /// <returns></returns>
        Response<AssessmentResponseViewModel> AddAssessmentResponse(AssessmentResponseViewModel assessmentResponse);

        /// <summary>
        /// Adds the assessment response details.
        /// </summary>
        /// <param name="assessmentResponseDetails">The assessment question.</param>
        /// <returns></returns>
        Response<AssessmentResponseDetail> SaveAssessmentResponseDetails(AssessmentResponseDetail assessmentResponseDetails);

        /// <summary>
        /// Updates the assessment response.
        /// </summary>
        /// <param name="assessmentResponse">The assessment response.</param>
        /// <returns></returns>
        Response<AssessmentResponseViewModel> UpdateAssessmentResponse(AssessmentResponseViewModel assessmentResponse);

        /// <summary>
        /// Gets the assessment response list.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        Response<AssessmentResponseList> GetAssessmentResponseListByContactID(long contactID);

        /// <summary>
        /// Delete assessment.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
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
