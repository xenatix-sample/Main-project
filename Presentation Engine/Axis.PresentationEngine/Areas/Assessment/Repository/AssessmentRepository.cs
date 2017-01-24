using System;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Common.Assessment;
using Axis.PresentationEngine.Areas.Assessment.Models;
using Axis.PresentationEngine.Areas.Assessment.Translator;
using Axis.Service;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.PresentationEngine.Areas.Assessment.Repository
{
    /// <summary>
    ///
    /// </summary>
    public class AssessmentRepository : IAssessmentRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "assessment/";

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentRepository" /> class.
        /// </summary>
        public AssessmentRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentRepository" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public AssessmentRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Get assessment.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        public Response<AssessmentViewModel> GetAssessment(long? assessmentId)
        {
            var apiUrl = baseRoute + "getAssessment";
            var param = new NameValueCollection();

            param.Add("assessmentId", assessmentId.ToString());

            var response = communicationManager.Get<Response<AssessmentModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the assessment sections.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        public Response<AssessmentSectionsViewModel> GetAssessmentSections(long? assessmentId)
        {
            var apiUrl = baseRoute + "getAssessmentSections";
            var param = new NameValueCollection();

            param.Add("assessmentId", assessmentId.ToString());

            var response = communicationManager.Get<Response<AssessmentSectionsModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Get the Logic Mapping Id
        /// </summary>
        /// <param name="logicMappingId">Logic Mapping Identifier</param>
        /// <returns></returns>
        public Response<AssessmentQuestionLogicViewModel> GetAssessmentQuestionsLogic(long? assessmentSectionId)
        {
            assessmentSectionId = 40; //test as of 04 02 2016
            
            var apiUrl = baseRoute + "getAssessmentQuestionsLogic";
            var param = new NameValueCollection();

            param.Add("AssessmentSectionID", assessmentSectionId.ToString());

            var response = communicationManager.Get<Response<AssessmentQuestionLogicModel>>(param, apiUrl);

            return response.ToViewModel();
        }
      

        /// <summary>
        /// Gets the assessment questions.
        /// </summary>
        /// <param name="assessmentSectionId">The assessment section identifier.</param>
        /// <returns></returns>
        public Response<AssessmentQuestionViewModel> GetAssessmentQuestions(long? assessmentSectionId)
        {
            var apiUrl = baseRoute + "getAssessmentQuestions";
            var param = new NameValueCollection();

            param.Add("assessmentSectionId", assessmentSectionId.ToString());

            var response = communicationManager.Get<Response<AssessmentQuestionModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the assessment responses.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        public Response<AssessmentResponseViewModel> GetAssessmentResponses(long contactId, long assessmentId)
        {
            var apiUrl = baseRoute + "getAssessmentResponses";
            var param = new NameValueCollection();

            param.Add("contactId", contactId.ToString());
            param.Add("assessmentId", assessmentId.ToString());

            var response = communicationManager.Get<Response<AssessmentResponseModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the assessment response.
        /// </summary>
        /// <param name="responseId">The response identifier.</param>
        /// <returns></returns>
        public Response<AssessmentResponseViewModel> GetAssessmentResponse(long responseId)
        {
            var apiUrl = baseRoute + "getAssessmentResponse";
            var param = new NameValueCollection();

            param.Add("responseId", responseId.ToString());

            var response = communicationManager.Get<Response<AssessmentResponseModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the assessment response details.
        /// </summary>
        /// <param name="responseId">The response identifier.</param>
        /// <param name="sectionId">The section identifier.</param>
        /// <returns></returns>
        public Response<AssessmentResponseDetailsViewModel> GetAssessmentResponseDetails(long responseId, long sectionId)
        {
            var apiUrl = baseRoute + "getAssessmentResponseDetails";
            var param = new NameValueCollection();

            param.Add("responseId", responseId.ToString());
            param.Add("sectionId", sectionId.ToString());

            var response = communicationManager.Get<Response<AssessmentResponseDetailsModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Adds the assessment response.
        /// </summary>
        /// <param name="assessmentResponse">The assessment response.</param>
        /// <returns></returns>
        public Response<AssessmentResponseViewModel> AddAssessmentResponse(AssessmentResponseViewModel assessmentResponse)
        {
            string apiUrl = baseRoute + "addAssessmentResponse";
            var response = communicationManager.Post<AssessmentResponseModel, Response<AssessmentResponseModel>>(assessmentResponse.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Adds the assessment response details.
        /// </summary>
        /// <param name="assessmentResponseDetails">The assessment question.</param>
        /// <returns></returns>
        public Response<AssessmentResponseDetail> SaveAssessmentResponseDetails(AssessmentResponseDetail assessmentResponseDetails)
        {
            string apiUrl = baseRoute + "saveAssessmentResponseDetails";
            return communicationManager.Post<AssessmentResponseDetail, Response<AssessmentResponseDetail>>(assessmentResponseDetails, apiUrl);
        }

        /// <summary>
        /// Updates the assessment response.
        /// </summary>
        /// <param name="assessment">The assessment.</param>
        /// <returns></returns>
        public Response<AssessmentResponseViewModel> UpdateAssessmentResponse(AssessmentResponseViewModel assessment)
        {
            string apiUrl = baseRoute + "updateAssessmentResponse";
            var response = communicationManager.Put<AssessmentResponseModel, Response<AssessmentResponseModel>>(assessment.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the assessment response list.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<AssessmentResponseList> GetAssessmentResponseListByContactID(long contactID)
        {
            var apiUrl = baseRoute + "getAssessmentResponseListByContactID";
            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());
            var response = communicationManager.Get<Response<AssessmentResponseList>>(param, apiUrl);
            return response;
        }

        /// <summary>
        /// Delete assessment.
        /// </summary>
        /// <param name="assessmentID">The assessment identifier.</param>
        /// <returns></returns>
        public Response<bool> DeleteAssessment(long assessmentID, DateTime modifiedOn)
        {
            //string apiUrl = BaseRoute + "DeleteAllergyDetail";
            var param = new NameValueCollection
            {
                {"assessmentID", assessmentID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };

            string apiUrl = baseRoute + "deleteAssessment";
            var response = communicationManager.Delete<Response<bool>>(param, apiUrl);
            return response;
        }

        /// <summary>
        /// Gets the Assessment Document Type ID.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        public Response<ScalarResult<int>> GetAssessmentDocumentTypeID(long? assessmentId)
        {
            var apiUrl = baseRoute + "GetAssessmentDocumentTypeID";
            var param = new NameValueCollection();

            param.Add("assessmentId", assessmentId.ToString());

            var response = communicationManager.Get<Response<ScalarResult<int>>>(param, apiUrl);
            return response;
        }

    }
}
