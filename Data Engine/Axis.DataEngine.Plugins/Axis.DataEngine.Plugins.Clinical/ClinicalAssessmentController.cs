using System;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Clinical.Assessment;
using Axis.Model.Clinical.Assessment;
using Axis.Model.Common;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Clinical
{
    public class ClinicalAssessmentController : BaseApiController
    {

        readonly IClinicalAssessmentDataProvider _assessmentDataProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="assessmentDataProvider"></param>
        public ClinicalAssessmentController(IClinicalAssessmentDataProvider assessmentDataProvider)
        {
            this._assessmentDataProvider = assessmentDataProvider;
        }

        /// <summary>
        /// To get  Assessment
        /// </summary>
        /// <param name="clinicalAssessmentID">clinical Assessment Id</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetClinicalAssessments(long clinicalAssessmentID)
        {
            return new HttpResult<Response<ClinicalAssessmentModel>>(_assessmentDataProvider.GetClinicalAssessments(clinicalAssessmentID), Request);
        }

        
        /// <summary>
        /// To get  Assessment list
        /// </summary>
        /// <param name="contactID">Contact Id</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetClinicalAssessmentsByContact(long contactID)
        {
            return new HttpResult<Response<ClinicalAssessmentModel>>(_assessmentDataProvider.GetClinicalAssessmentsByContact(contactID), Request);
        }
        /// <summary>
        /// To add assessment
        /// </summary>
        /// <param name=" ClinicalAssessmentModel">Assessment Model</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddAssessment(ClinicalAssessmentModel assessmentModel)
        {
            return new HttpResult<Response<ClinicalAssessmentModel>>(_assessmentDataProvider.AddAssessment(assessmentModel), Request);
        }

        /// <summary>
        /// To update assessment
        /// </summary>
        /// <param name=" ClinicalAssessmentModel">Assessment Model</param>
        /// <returns>Response of type  ClinicalAssessmentModel</returns>
        [HttpPut]
        public IHttpActionResult UpdateAssessment(ClinicalAssessmentModel assessmentModel)
        {
            return new HttpResult<Response<ClinicalAssessmentModel>>(_assessmentDataProvider.UpdateAssessment(assessmentModel), Request);
        }

        /// <summary>
        /// To remove assessment
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteAssessment(long Id, DateTime modifiedOn)
        {
            return new HttpResult<Response<ClinicalAssessmentModel>>(_assessmentDataProvider.DeleteAssessment(Id, modifiedOn), Request);
        }
    }
}
