using System;
using Axis.Model.Clinical.Assessment;
using Axis.Model.Common;
using Axis.Service.Clinical.Assessment;

namespace Axis.RuleEngine.Clinical.Assessment
{
    public class AssessmentRuleEngine : IAssessmentRuleEngine
    {
         readonly IAssessmentService _assessmentService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="assessmentService"></param>
        public AssessmentRuleEngine(IAssessmentService assessmentService)
        {
            _assessmentService = assessmentService;
        }

        /// <summary>
        /// To get assessment
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns></returns>
        public Response<ClinicalAssessmentModel> GetClinicalAssessments(long clinicalAssessmentID)
        {
            return _assessmentService.GetClinicalAssessments(clinicalAssessmentID);
        }


        /// <summary>
        /// To get list of assessments 
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns></returns>
        public Response<ClinicalAssessmentModel> GetClinicalAssessmentsByContact(long contactID)
        {
            return _assessmentService.GetClinicalAssessmentsByContact(contactID);
        }

        /// <summary>
        /// To add new assessment 
        /// </summary>
        /// <param name="assessment"></param>
        /// <returns></returns>
        public Response<ClinicalAssessmentModel> AddAssessment(ClinicalAssessmentModel assessment)
        {
            return _assessmentService.AddAssessment(assessment);
        }

        /// <summary>
        /// To update assessment 
        /// </summary>
        /// <param name="assessment"></param>
        /// <returns></returns>
        public Response<ClinicalAssessmentModel> UpdateAssessment(ClinicalAssessmentModel assessment)
        {
            return _assessmentService.UpdateAssessment(assessment);
        }

        /// <summary>
        /// To remove assessment 
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Response<ClinicalAssessmentModel> DeleteAssessment(long Id, DateTime modifiedOn)
        {
            return _assessmentService.DeleteAssessment(Id, modifiedOn);
        }
    }
}
