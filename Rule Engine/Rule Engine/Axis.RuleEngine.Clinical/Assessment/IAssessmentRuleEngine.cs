using System;
using Axis.Model.Clinical.Assessment;
using Axis.Model.Common;

namespace Axis.RuleEngine.Clinical.Assessment
{
   public interface IAssessmentRuleEngine
    {

        /// <summary>
        /// To get the assessment
        /// </summary>
        /// <param name="clinicalAssessmentID">Clinical Assessment ID </param>
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
        /// <returns></returns>
        Response<ClinicalAssessmentModel> DeleteAssessment(long Id, DateTime modifiedOn);

    }
}
