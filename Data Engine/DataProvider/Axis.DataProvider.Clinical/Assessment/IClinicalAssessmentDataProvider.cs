using System;
using Axis.Model.Clinical.Assessment;
using Axis.Model.Common;

namespace Axis.DataProvider.Clinical.Assessment
{
   public interface IClinicalAssessmentDataProvider
    {

        /// <summary>
        /// Get list of Assessments for clinical assessment ID
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns></returns>
        Response<ClinicalAssessmentModel> GetClinicalAssessments(long clinicalAssessmentID);


        /// <summary>
        /// Get list of Assessments for contact
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns></returns>
        Response<ClinicalAssessmentModel> GetClinicalAssessmentsByContact(long contactID);

        /// <summary>
        /// Add Assessment for contact
        /// </summary>
        /// <param name="Assessment"></param>
        /// <returns></returns>
        Response<ClinicalAssessmentModel> AddAssessment(ClinicalAssessmentModel Assessment);

        /// <summary>
        /// Update Assessment
        /// </summary>
        /// <param name="Assessment"></param>
        /// <returns></returns>
        Response<ClinicalAssessmentModel> UpdateAssessment(ClinicalAssessmentModel Assessment);

        /// <summary>
        /// Remove Assessment for contact
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ClinicalAssessmentModel> DeleteAssessment(long Id, DateTime modifiedOn);
    }
}
