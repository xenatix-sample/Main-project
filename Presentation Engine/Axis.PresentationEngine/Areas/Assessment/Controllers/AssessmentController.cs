using Axis.PresentationEngine.Areas.Assessment.Models;
using Axis.PresentationEngine.Areas.Assessment.Repository;
using Axis.PresentationEngine.Helpers.Controllers;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Areas.Assessment.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class AssessmentController : BaseController
    {
        /// <summary>
        /// The assessment repository
        /// </summary>
        private readonly IAssessmentRepository assessmentRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssessmentController" /> class.
        /// </summary>
        /// <param name="assessmentRepository">The assessment repository.</param>
        public AssessmentController(IAssessmentRepository assessmentRepository)
        {
            this.assessmentRepository = assessmentRepository;
        }

        /// <summary>
        /// Screenings this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Section()
        {
            return View("_AssessmentSection");
        }

        /// <summary>
        /// Assessments the service.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult AssessmentService()
        {
            var assessments = assessmentRepository.GetAssessment(null);
            var assessmentSections = assessmentRepository.GetAssessmentSections(null);
            var assessmentQuestions = assessmentRepository.GetAssessmentQuestions(null);

            var assessmentLogicmapping = assessmentRepository.GetAssessmentQuestionsLogic(null);


            var assessmentCacheViewModel = new AssessmentCacheViewModel()
            {
                Assessments = JsonConvert.SerializeObject(assessments.DataItems),
                AssessmentSections = JsonConvert.SerializeObject(assessmentSections.DataItems),
                AssessmentQuestions = JsonConvert.SerializeObject(assessmentQuestions.DataItems),

                AssessmentLogicMapping = JsonConvert.SerializeObject(assessmentLogicmapping.DataItems)
            };

            Response.ContentType = "text/javascript";
            return View(assessmentCacheViewModel);
        }
    }
}