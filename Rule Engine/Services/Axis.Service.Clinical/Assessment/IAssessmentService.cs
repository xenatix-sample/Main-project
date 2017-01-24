using System;
using Axis.Model.Clinical.Assessment;
using Axis.Model.Common;

namespace Axis.Service.Clinical.Assessment
{
    public interface IAssessmentService
    {
        /// <summary>
        /// To get the assessment
        /// </summary>
        /// <param name="clinicalAssessmentID">Contact Id</param>
        /// <returns></returns>
        Response<ClinicalAssessmentModel> GetClinicalAssessments(long clinicalAssessmentID);
       
        /// <summary>
        /// To get the list of assessments
        /// </summary>
        /// <param name="contactID">Contact Id</param>
        /// <returns></returns>
        Response<ClinicalAssessmentModel> GetClinicalAssessmentsByContact(long contactID);

        
        /// <summary>
        /// To add assessment
        /// </summary>
        /// <param name="assessment"></param>
        /// <returns></returns>
        Response<ClinicalAssessmentModel> AddAssessment(ClinicalAssessmentModel assessment);

        /// <summary>
        /// To update assessment
        /// </summary>
        /// <param name="assessment"></param>
        /// <returns></returns>
        Response<ClinicalAssessmentModel> UpdateAssessment(ClinicalAssessmentModel assessment);

        /// <summary>
        /// To remove assessment
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ClinicalAssessmentModel> DeleteAssessment(long Id, DateTime modifiedOn);
    }
}
