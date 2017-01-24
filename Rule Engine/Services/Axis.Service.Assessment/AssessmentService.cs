using System;
using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Common.Assessment;
using Axis.Security;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Assessment
{
    /// <summary>
    /// 
    /// </summary>
    public class AssessmentService : IAssessmentService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "assessment/";

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentService" /> class.
        /// </summary>
        public AssessmentService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Gets the assessment.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        public Response<AssessmentModel> GetAssessment(long? assessmentId)
        {
            var apiUrl = baseRoute + "GetAssessment";
            var param = new NameValueCollection();
            param.Add("assessmentId", assessmentId.ToString());

            return communicationManager.Get<Response<AssessmentModel>>(param, apiUrl);
        }

        /// <summary>
        /// Gets the assessment sections.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        public Response<AssessmentSectionsModel> GetAssessmentSections(long? assessmentId)
        {
            var apiUrl = baseRoute + "GetAssessmentSections";
            var param = new NameValueCollection();
            param.Add("assessmentId", assessmentId.ToString());

            return communicationManager.Get<Response<AssessmentSectionsModel>>(param, apiUrl);
        }

        /// <summary>
        /// Gets the assessment questions.
        /// </summary>
        /// <param name="assessmentSectionId">The assessment section identifier.</param>
        /// <returns></returns>
        public Response<AssessmentQuestionModel> GetAssessmentQuestions(long? assessmentSectionId)
        {
            var apiUrl = baseRoute + "GetAssessmentQuestions";
            var param = new NameValueCollection();
            param.Add("assessmentSectionId", assessmentSectionId.ToString());

            return communicationManager.Get<Response<AssessmentQuestionModel>>(param, apiUrl);
        }


        public Response<AssessmentQuestionLogicModel> GetAssessmentQuestionsLogic(long? assessmentSectionId)
        {
            var apiUrl = baseRoute + "GetAssessmentQuestionsLogic";
            var param = new NameValueCollection();
            param.Add("assessmentSectionId", assessmentSectionId.ToString());

            return communicationManager.Get<Response<AssessmentQuestionLogicModel>>(param, apiUrl);
        }


        /// <summary>
        /// Gets the assessment response.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        public Response<AssessmentResponseModel> GetAssessmentResponses(long contactId, long assessmentId)
        {
            var apiUrl = baseRoute + "GetAssessmentResponses";
            var param = new NameValueCollection();
            param.Add("contactId", contactId.ToString());
            param.Add("assessmentId", assessmentId.ToString());

            return communicationManager.Get<Response<AssessmentResponseModel>>(param, apiUrl);
        }

        /// <summary>
        /// Gets the assessment response.
        /// </summary>
        /// <param name="responseId">The response identifier.</param>
        /// <returns></returns>
        public Response<AssessmentResponseModel> GetAssessmentResponse(long responseId)
        {
            var apiUrl = baseRoute + "GetAssessmentResponse";
            var param = new NameValueCollection();
            param.Add("responseId", responseId.ToString());

            return communicationManager.Get<Response<AssessmentResponseModel>>(param, apiUrl);
        }

        /// <summary>
        /// Gets the assessment response details.
        /// </summary>
        /// <param name="responseId">The response identifier.</param>
        /// <param name="sectionId">The section identifier.</param>
        /// <returns></returns>
        public Response<AssessmentResponseDetailsModel> GetAssessmentResponseDetails(long responseId, long sectionId)
        {
            var apiUrl = baseRoute + "GetAssessmentResponseDetails";
            var param = new NameValueCollection();
            param.Add("responseId", responseId.ToString());
            param.Add("sectionId", sectionId.ToString());

            return communicationManager.Get<Response<AssessmentResponseDetailsModel>>(param, apiUrl);
        }

        /// <summary>
        /// Adds the assessment response.
        /// </summary>
        /// <param name="assessmentResponse">The assessment response.</param>
        /// <returns></returns>
        public Response<AssessmentResponseModel> AddAssessmentResponse(AssessmentResponseModel assessmentResponse)
        {
            var apiUrl = baseRoute + "AddAssessmentResponse";
            return communicationManager.Post<AssessmentResponseModel, Response<AssessmentResponseModel>>(assessmentResponse, apiUrl);
        }

        /// <summary>
        /// Saves the assessment response details.
        /// </summary>
        /// <param name="assessmentResponseDetails">The assessment question.</param>
        /// <returns></returns>
        public Response<AssessmentResponseDetail> SaveAssessmentResponseDetails(AssessmentResponseDetail assessmentResponseDetails)
        {
            var apiUrl = baseRoute + "SaveAssessmentResponseDetails";
            return communicationManager.Post<AssessmentResponseDetail, Response<AssessmentResponseDetail>>(assessmentResponseDetails, apiUrl);
        }

        /// <summary>
        /// Updates the assessment response.
        /// </summary>
        /// <param name="assessmentResponse">The assessment response.</param>
        /// <returns></returns>
        public Response<AssessmentResponseModel> UpdateAssessmentResponse(AssessmentResponseModel assessmentResponse)
        {
            var apiUrl = baseRoute + "UpdateAssessmentResponse";
            return communicationManager.Put<AssessmentResponseModel, Response<AssessmentResponseModel>>(assessmentResponse, apiUrl);
        }


        /// <summary>
        /// Gets the assessment response list.
        /// </summary>
        /// <param name="responseId">The response identifier.</param>
        /// <param name="sectionId">The section identifier.</param>
        /// <returns></returns>
        public Response<AssessmentResponseList> GetAssessmentResponseListByContactID(long contactID)
        {
            var apiUrl = baseRoute + "GetAssessmentResponseListByContactID";
            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());
            return communicationManager.Get<Response<AssessmentResponseList>>(param, apiUrl);
        }

        /// <summary>
        /// Deletes the assessment.
        /// </summary>
        /// <param name="assessmentId"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<bool> DeleteAssessment(long assessmentId, DateTime modifiedOn)
        {
            var apiUrl = baseRoute + "DeleteAssessment";
            var param = new NameValueCollection
            {
                {"assessmentId", assessmentId.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };

            return communicationManager.Delete<Response<bool>>(param, apiUrl);
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

            return communicationManager.Get<Response<ScalarResult<int>>>(param, apiUrl);
        }
    }
}
