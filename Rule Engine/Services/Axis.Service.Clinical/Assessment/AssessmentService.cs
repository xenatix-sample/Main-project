using System;
using Axis.Configuration;
using Axis.Model.Clinical.Assessment;
using Axis.Model.Common;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Clinical.Assessment
{
    public class AssessmentService : IAssessmentService
    {
        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "ClinicalAssessment/";

        /// <summary>
        /// Constructor
        /// </summary>
        public AssessmentService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token"></param>
        public AssessmentService(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// To get the assessment
        /// </summary>
        /// <param name="clinicalAssessmentID">Contact Id</param>
        /// <returns></returns>
        public Response<ClinicalAssessmentModel> GetClinicalAssessments(long clinicalAssessmentID)
        {
            const string apiUrl = BaseRoute + "GetClinicalAssessments";
            var requestId = new NameValueCollection { { "clinicalAssessmentID", clinicalAssessmentID.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<ClinicalAssessmentModel>>(requestId, apiUrl);
        }


        /// <summary>
        /// To get the list of assessments
        /// </summary>
        /// <param name="contactID">Contact Id</param>
        /// <returns></returns>
        public Response<ClinicalAssessmentModel> GetClinicalAssessmentsByContact(long contactID)
        {
            const string apiUrl = BaseRoute + "GetClinicalAssessmentsByContact";
            var requestId = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<ClinicalAssessmentModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// To add assessment
        /// </summary>
        /// <param name="assessment"></param>
        /// <returns></returns>
        public Response<ClinicalAssessmentModel> AddAssessment(ClinicalAssessmentModel assessment)
        {
            const string apiUrl = BaseRoute + "AddAssessment";
            return _communicationManager.Post<ClinicalAssessmentModel, Response<ClinicalAssessmentModel>>(assessment, apiUrl);
        }

        /// <summary>
        /// To update assessment
        /// </summary>
        /// <param name="assessment"></param>
        /// <returns></returns>
        public Response<ClinicalAssessmentModel> UpdateAssessment(ClinicalAssessmentModel assessment)
        {
            const string apiUrl = BaseRoute + "UpdateAssessment";
            return _communicationManager.Put<ClinicalAssessmentModel, Response<ClinicalAssessmentModel>>(assessment, apiUrl);
        }

        /// <summary>
        /// To remove assessment
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Response<ClinicalAssessmentModel> DeleteAssessment(long Id, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteAssessment";
            var requestId = new NameValueCollection { { "Id", Id.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Delete<Response<ClinicalAssessmentModel>>(requestId, apiUrl);
        }
    }
}
