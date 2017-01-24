using System;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Clinical.Assessment;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.Assessment;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;
using Axis.Plugins.Clinical.Translator;


namespace Axis.Plugins.Clinical.Repository.Assessment
{
   public class ClinicalAssessmentRepository:IClinicalAssessmentRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "clinicalAssessment/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ClinicalAssessmentRepository" /> class.
        /// </summary>
        public ClinicalAssessmentRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClinicalAssessmentRepository" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ClinicalAssessmentRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

      

        /// <summary>
        /// Gets the assessments asynchronously
        /// </summary>
        /// <param name="ContactID">The  clinical Assessment ID identifier.</param>
        /// <returns></returns>
        public async Task<Response<ClinicalAssessmentViewModel>> GetClinicalAssessments(long clinicalAssessmentID)
        {
            string apiUrl = baseRoute + "GetClinicalAssessments";
            var parameters = new NameValueCollection { { "clinicalAssessmentID", clinicalAssessmentID.ToString(CultureInfo.InvariantCulture) } };
            var response = await _communicationManager.GetAsync<Response<ClinicalAssessmentModel>>(parameters, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Gets the list of assessments asynchronously
        /// </summary>
        /// <param name="ContactID">The contact identifier.</param>
        /// <returns></returns>
        public async Task<Response<ClinicalAssessmentViewModel>> GetClinicalAssessmentsByContact(long ContactID)
        {
            string apiUrl = baseRoute + "GetClinicalAssessmentsByContact";
            var parameters = new NameValueCollection { { "ContactId", ContactID.ToString(CultureInfo.InvariantCulture) } };
            var response = await _communicationManager.GetAsync<Response<ClinicalAssessmentModel>>(parameters, apiUrl);
            return response.ToViewModel();
        }


        /// <summary>
        /// Add assessment
        /// </summary>
        /// <param name="assessment">The assessment.</param>
        /// <returns></returns>
        public Response<ClinicalAssessmentViewModel> AddAssessment(ClinicalAssessmentViewModel assessment)
        {
            string apiUrl = baseRoute + "AddAssessment";
            var response = _communicationManager.Post<ClinicalAssessmentModel, Response<ClinicalAssessmentModel>>(assessment.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Update Assessment
        /// </summary>
        /// <param name="assessment">The assessment.</param>
        /// <returns></returns>
        public Response<ClinicalAssessmentViewModel> UpdateAssessment(ClinicalAssessmentViewModel assessment)
        {
            string apiUrl = baseRoute + "UpdateAssessment";
            var response = _communicationManager.Put<ClinicalAssessmentModel, Response<ClinicalAssessmentModel>>(assessment.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Delete Assessment
        /// </summary>
        /// <param name="Id">The identifier</param>
        /// <returns></returns>
        public Response<ClinicalAssessmentViewModel> DeleteAssessment(long Id, DateTime modifiedOn)
        {
            string apiUrl = baseRoute;
            var param = new NameValueCollection
            {
                {"Id", Id.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };

            return _communicationManager.Delete<Response<ClinicalAssessmentModel>>(param, apiUrl).ToViewModel();
        }
    }
}
