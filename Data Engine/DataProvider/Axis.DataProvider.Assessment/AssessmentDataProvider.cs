using Axis.Data.Repository;
using Axis.Model.Common;
using Axis.Model.Common.Assessment;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;

namespace Axis.DataProvider.Assessment
{
    /// <summary>
    /// 
    /// </summary>
    public class AssessmentDataProvider : IAssessmentDataProvider
    {
        #region Private Variables

        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork = null;

        #endregion Private Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentDataProvider" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public AssessmentDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the assessment.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        public Response<AssessmentModel> GetAssessment(long? assessmentId)
        {
            var assessmentRepository = unitOfWork.GetRepository<AssessmentModel>();

            SqlParameter assessmentIdParam = new SqlParameter("AssessmentID", (object)assessmentId ?? DBNull.Value);

            List<SqlParameter> procParams = new List<SqlParameter>() { assessmentIdParam };
            var assessments = assessmentRepository.ExecuteStoredProc("usp_GetAssessments", procParams);

            return assessments;
        }

        /// <summary>
        /// Gets the assessment sections.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        public Response<AssessmentSectionsModel> GetAssessmentSections(long? assessmentId)
        {
            var assessmentSectionsRepository = unitOfWork.GetRepository<AssessmentSectionsModel>();

            SqlParameter assessmentIdParam = new SqlParameter("AssessmentID", (object)assessmentId ?? DBNull.Value);
            List<SqlParameter> procParams = new List<SqlParameter>() { assessmentIdParam };

            var assessmentSections = assessmentSectionsRepository.ExecuteStoredProc("usp_GetAssessmentSections", procParams);
            return assessmentSections;
        }

        /// <summary>
        /// Gets the assessment questions.
        /// </summary>
        /// <param name="assessmentSectionId">The assessment section identifier.</param>
        /// <returns></returns>
        public Response<AssessmentQuestionModel> GetAssessmentQuestions(long? assessmentSectionId)
        {
            var assessmentQuestionRepository = unitOfWork.GetRepository<AssessmentQuestionModel>();

            SqlParameter sectionIdParam = new SqlParameter("AssessmentSectionID", (object)assessmentSectionId ?? DBNull.Value);
            List<SqlParameter> procParams = new List<SqlParameter>() { sectionIdParam };
            var assessmentQuestions = assessmentQuestionRepository.ExecuteStoredProc("usp_GetAssessmentQuestions", procParams);

            if ((assessmentQuestions != null) && (assessmentQuestions.DataItems.Count > 0))
            {
                //Get assessment question options
                var assessmentQuestionOptions = GetAssessmentQuestionOptions(assessmentSectionId);
                assessmentQuestions.DataItems.ForEach(delegate(AssessmentQuestionModel assessmentQuestion)
                {
                    assessmentQuestion.AssessmentQuestionOptions =
                        assessmentQuestionOptions.DataItems.Where(x => x.QuestionID == assessmentQuestion.QuestionID)
                            .ToList();
                });
            }

            return assessmentQuestions;
        }


        public Response<AssessmentQuestionLogicModel> GetAssessmentQuestionsLogic(long? assessmentSectionId)
        {
            var responseRepository = unitOfWork.GetRepository<AssessmentQuestionLogicModel>();


            SqlParameter sectionIdParam = new SqlParameter("AssessmentSectionID", assessmentSectionId);
            List<SqlParameter> procParams = new List<SqlParameter>() { sectionIdParam };

            var assessmentQuestionsLogic = responseRepository.ExecuteStoredProc("usp_GetAssessmentQuestionLogic", procParams);


            //if ((assessmentQuestionsLogic != null) && (assessmentQuestionsLogic.DataItems.Count > 0))
            //{
            //    ////Get assessment question options
            //    //var assessmentQuestionOptions = GetAssessmentQuestionOptions(assessmentSectionId);
            //    //assessmentQuestionsLogic.DataItems.ForEach(delegate(AssessmentQuestionLogicModel assessmentQuestion)
            //    //{
            //    //    assessmentQuestion.AssessmentQuestionOptions =
            //    //        assessmentQuestionOptions.DataItems.Where(x => x.QuestionID == assessmentQuestion.QuestionID)
            //    //            .ToList();
            //    //});
            //}
           
           
          

            return assessmentQuestionsLogic;
        }


        /// <summary>
        /// Gets the assessment responses for a given contact and assessment.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        public Response<AssessmentResponseModel> GetAssessmentResponses(long contactId, long assessmentId)
        {
            var assessmentResponseRepository = unitOfWork.GetRepository<AssessmentResponseModel>();

            SqlParameter contactIdParam = new SqlParameter("ContactID", contactId);
            SqlParameter assessmentIdParam = new SqlParameter("AssessmentID", assessmentId);

            List<SqlParameter> procParams = new List<SqlParameter>() { contactIdParam, assessmentIdParam };
            var assessmentResponses = assessmentResponseRepository.ExecuteStoredProc("usp_GetAssessmentResponses", procParams);

            return assessmentResponses;
        }

        /// <summary>
        /// Gets the assessment response.
        /// </summary>
        /// <param name="responseId">The response identifier.</param>
        /// <returns></returns>
        public Response<AssessmentResponseModel> GetAssessmentResponse(long responseId)
        {
            var assessmentResponseRepository = unitOfWork.GetRepository<AssessmentResponseModel>();

            SqlParameter responseIdParam = new SqlParameter("ResponseID", responseId);

            List<SqlParameter> procParams = new List<SqlParameter>() { responseIdParam };
            var assessmentResponses = assessmentResponseRepository.ExecuteStoredProc("usp_GetAssessmentResponse", procParams);

            return assessmentResponses;
        }

        /// <summary>
        /// Gets the assessment response details.
        /// </summary>
        /// <param name="responseId">The response identifier.</param>
        /// <param name="sectionId">The section identifier.</param>
        /// <returns></returns>
        public Response<AssessmentResponseDetailsModel> GetAssessmentResponseDetails(long responseId, long sectionId)
        {
            var assessmentResponseDetailsRepository = unitOfWork.GetRepository<AssessmentResponseDetailsModel>();

            SqlParameter responseIdParam = new SqlParameter("ResponseID", responseId);
            SqlParameter sectionIdParam = new SqlParameter("AssessmentSectionID", sectionId);

            List<SqlParameter> procParams = new List<SqlParameter>() { responseIdParam, sectionIdParam };
            var assessmentResponseDetails = assessmentResponseDetailsRepository.ExecuteStoredProc("usp_GetAssessmentResponseDetails", procParams);

            return assessmentResponseDetails;
        }

        /// <summary>
        /// Gets the assessment question options.
        /// </summary>
        /// <param name="sectionId">The section identifier.</param>
        /// <returns></returns>
        public Response<AssessmentQuestionOptionModel> GetAssessmentQuestionOptions(long? sectionId)
        {
            var assessmentQuestionOptionRepository = unitOfWork.GetRepository<AssessmentQuestionOptionModel>();

            SqlParameter sectionIdParam = new SqlParameter("AssessmentSectionID", (object)sectionId ?? DBNull.Value);
            List<SqlParameter> procParams = new List<SqlParameter>() { sectionIdParam };
            var assessmentQuestionOptions = assessmentQuestionOptionRepository.ExecuteStoredProc("usp_GetAssessmentQuestionOptions", procParams);

            return assessmentQuestionOptions;
        }

        /// <summary>
        /// Adds the assessment response.
        /// </summary>
        /// <param name="assessmentResponse">The assessment response.</param>
        /// <returns></returns>
        public Response<AssessmentResponseModel> AddAssessmentResponse(AssessmentResponseModel assessmentResponse)
        {
            var responseRepository = unitOfWork.GetRepository<AssessmentResponseModel>();

            var responseIDValueParam = new SqlParameter("ResponseID", assessmentResponse.ResponseID);
            var contactIDValueParam = new SqlParameter("ContactID", assessmentResponse.ContactID);
            var assessmentIDValueParam = new SqlParameter("AssessmentID", assessmentResponse.AssessmentID);
            var enterDateValueParam = new SqlParameter("EnterDate", DateTime.Now);
            var modifiedOnParam = new SqlParameter("ModifiedOn", assessmentResponse.ModifiedOn ?? DateTime.Now);
            var spParameters = new List<SqlParameter>() { responseIDValueParam, contactIDValueParam, assessmentIDValueParam, enterDateValueParam, modifiedOnParam };
            var responseResult = unitOfWork.EnsureInTransaction<Response<AssessmentResponseModel>>(responseRepository.ExecuteNQStoredProc, "usp_AddAssessmentResponses", spParameters, forceRollback: assessmentResponse.ForceRollback.GetValueOrDefault(false), idResult: true);

            return responseResult;
        }

        /// <summary>
        /// Saves the assessment response details.
        /// </summary>
        /// <param name="assessmentResponseDetails">The assessment response details.</param>
        /// <returns></returns>
        public Response<AssessmentResponseDetail> SaveAssessmentResponseDetails(AssessmentResponseDetail assessmentResponseDetails)
        {
            var responseDetailsRepository = unitOfWork.GetRepository<AssessmentResponseDetail>();

            var requestXMLValueParam = new SqlParameter("AssessmentXML", GenerateAssessmentXml(assessmentResponseDetails));
            var responseID = new SqlParameter("ResponseID", assessmentResponseDetails.ResponseID);
            var sectionID = new SqlParameter("SectionID", assessmentResponseDetails.SectionID);
            var modifiedOn = new SqlParameter("ModifiedOn", assessmentResponseDetails.ModifiedOn);
            var responseDetailsSpParameters = new List<SqlParameter>() { requestXMLValueParam, responseID, sectionID, modifiedOn };
            var responseDetailsResult = unitOfWork.EnsureInTransaction<Response<AssessmentResponseDetail>>(responseDetailsRepository.ExecuteNQStoredProc, "usp_SaveAssessmentResponseDetails", responseDetailsSpParameters, forceRollback: assessmentResponseDetails.ForceRollback.GetValueOrDefault(false));

            return responseDetailsResult;
        }

        /// <summary>
        /// Updates the assessment response.
        /// </summary>
        /// <param name="assessmentResponse">The assessment response.</param>
        /// <returns></returns>
        public Response<AssessmentResponseModel> UpdateAssessmentResponse(AssessmentResponseModel assessmentResponse)
        {
            var responseRepository = unitOfWork.GetRepository<AssessmentResponseModel>();

            var responseIdValueParam = new SqlParameter("ResponseID", assessmentResponse.ResponseID);
            var enterDateValueParam = new SqlParameter("EnterDate", DateTime.Now);
            var isActiveValueParam = new SqlParameter("IsActive", assessmentResponse.IsActive.GetValueOrDefault(true));
            var modifiedOnParam = new SqlParameter("ModifiedOn", assessmentResponse.ModifiedOn ?? DateTime.Now);
            var spParameters = new List<SqlParameter>() { responseIdValueParam, enterDateValueParam, isActiveValueParam, modifiedOnParam };
            var responseResult = unitOfWork.EnsureInTransaction<Response<AssessmentResponseModel>>(responseRepository.ExecuteNQStoredProc, "usp_UpdateAssessmentResponses", spParameters, forceRollback: assessmentResponse.ForceRollback.GetValueOrDefault(false));

            return responseResult;
        }



        /// <summary>
        /// Gets the assessment list.
        /// </summary>
        /// <param name="responseId">The contact identifier.</param>
        /// <returns></returns>
        public Response<AssessmentResponseList> GetAssessmentResponseListByContactID(long contactID)
        {
            var assessmentResponseDetailsRepository = unitOfWork.GetRepository<AssessmentResponseList>();
            SqlParameter contactIdParam = new SqlParameter("ContactID", contactID);
            List<SqlParameter> procParams = new List<SqlParameter>() { contactIdParam };
            var assessmentResponseList = assessmentResponseDetailsRepository.ExecuteStoredProc("usp_GetAssessmentsByContactID", procParams);
            return assessmentResponseList;
        }
      

        /// <summary>
        /// Deletes the assessment.
        /// </summary>
        /// <param name="assessmentID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
       
        public Response<bool> DeleteAssessment(long assessmentID, DateTime modifiedOn)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the Assessment Document Type ID.
        /// </summary>
        /// <param name="assessmentId">The assessment identifier.</param>
        /// <returns></returns>
        public Response<ScalarResult<int>> GetAssessmentDocumentTypeID(long? assessmentId)
        {
            var assessmentSectionsRepository = unitOfWork.GetRepository<ScalarResult<int>>();

            SqlParameter assessmentIdParam = new SqlParameter("AssessmentID", (object)assessmentId ?? DBNull.Value);
            List<SqlParameter> procParams = new List<SqlParameter>() { assessmentIdParam };

            var assessmentSections = assessmentSectionsRepository.ExecuteStoredProc("usp_GetAssessmentDocumentType", procParams);
            return assessmentSections;
        }

        #endregion Methods

        #region Private Methods

        /// <summary>
        /// Generates the assessment XML.
        /// </summary>
        /// <param name="assessmentResponseDetails">The assessment.</param>
        /// <returns></returns>
        private string GenerateAssessmentXml(AssessmentResponseDetail assessmentResponseDetails)
        {
            var xmlString =
                    new XElement("Assessment",
                            from assessmentResponseDetail in assessmentResponseDetails.AssessmentDetails
                            select new XElement("AssessmentResponseDetails",GetXMLNode(assessmentResponseDetail)));
            return xmlString.ToString();
        }

        private List<XElement> GetXMLNode(AssessmentResponseDetailsModel model)
        {
            List<XElement> lstElement = new List<XElement>();
            lstElement.Add(new XElement("ResponseDetailsID", model.ResponseDetailsID));
            lstElement.Add(new XElement("ResponseDetailsID", model.ResponseDetailsID));
            lstElement.Add(new XElement("QuestionID", model.QuestionID));
            lstElement.Add(new XElement("OptionsID", model.OptionsID));
            lstElement.Add(new XElement("ResponseText", model.ResponseText));
            lstElement.Add(new XElement("Rating", model.Rating));
            if (model.SignatureBLOB != null && model.SignatureBLOB.Length>0)
            {
                lstElement.Add(new XElement("SignatureBLOB",Convert.ToBase64String(model.SignatureBLOB)));
            }
            if (model.CredentialID > 0)
            {
                lstElement.Add(new XElement("DateSigned", model.DateSigned));
                lstElement.Add(new XElement("CredentialID", model.CredentialID));

            }

            return lstElement;
        }
        #endregion Private Methods
    }
}
