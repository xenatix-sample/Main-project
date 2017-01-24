using System;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.Assessment;
using System.Threading.Tasks;

namespace Axis.Plugins.Clinical.Repository.Assessment
{
    public interface IClinicalAssessmentRepository
    {


        /// <summary>
        /// Gets the Assessments async
        /// </summary>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        Task<Response<ClinicalAssessmentViewModel>> GetClinicalAssessments(long ContactID);

        /// <summary>
        /// Gets the contact Assessments
        /// </summary>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        Task<Response<ClinicalAssessmentViewModel>> GetClinicalAssessmentsByContact(long ContactID);

       

        /// <summary>
        /// Adds the assessment.
        /// </summary>
        /// <param name="assessment">The Assessment.</param>
        /// <returns></returns>
        Response<ClinicalAssessmentViewModel> AddAssessment(ClinicalAssessmentViewModel assessment);

        /// <summary>
        /// Updates the assessment.
        /// </summary>
        /// <param name="assessment">The assessment.</param>
        /// <returns></returns>
        Response<ClinicalAssessmentViewModel> UpdateAssessment(ClinicalAssessmentViewModel assessment);

        /// <summary>
        /// Deletes the Assessment.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Response<ClinicalAssessmentViewModel> DeleteAssessment(long id, DateTime modifiedOn);
    }
}
