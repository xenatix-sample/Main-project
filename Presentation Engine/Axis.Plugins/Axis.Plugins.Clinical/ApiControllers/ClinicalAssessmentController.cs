using System;
using Axis.Plugins.Clinical.Models.Assessment;
using Axis.Plugins.Clinical.Repository.Assessment;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Threading.Tasks;
using System.Web.Http;
using Axis.Model.Common;

namespace Axis.Plugins.Clinical.ApiControllers
{
    public class ClinicalAssessmentController : BaseApiController
    {
        readonly IClinicalAssessmentRepository _assessmentRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="assessmentRepository"></param>
        public ClinicalAssessmentController(IClinicalAssessmentRepository assessmentRepository)
        {
            _assessmentRepository = assessmentRepository;
        }

        /// <summary>
        /// Gets the list of Assessments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ClinicalAssessmentViewModel>> GetClinicalAssessments(long clinicalAssessmentID)
        {
            var result = await _assessmentRepository.GetClinicalAssessments(clinicalAssessmentID);
            return result;
        }

        /// <summary>
        /// Gets the list of Assessments
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<ClinicalAssessmentViewModel>> GetClinicalAssessmentsByContact(long ContactID)
        {
            var result = await _assessmentRepository.GetClinicalAssessmentsByContact(ContactID);
            return result;
        }

        /// <summary>
        /// Adds the assessment.
        /// </summary>
        /// <param name="assessment">The assessment.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ClinicalAssessmentViewModel> AddAssessment(ClinicalAssessmentViewModel assessment)
        {
            assessment.TakenTime = assessment.TakenTime.ToUniversalTime();
            return _assessmentRepository.AddAssessment(assessment);
        }

        /// <summary>
        /// Updates the Assessment.
        /// </summary>
        /// <param name="assessment">The assessment.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<ClinicalAssessmentViewModel> UpdateAssessment(ClinicalAssessmentViewModel assessment)
        {
            assessment.TakenTime = assessment.TakenTime.ToUniversalTime();
            return _assessmentRepository.UpdateAssessment(assessment);
        }

        /// <summary>
        /// Deletes the Assessment.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<ClinicalAssessmentViewModel> DeleteAssessment(long Id, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _assessmentRepository.DeleteAssessment(Id, modifiedOn);
        }
    }
}
