using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.Plugins.Clinical.Controllers
{
    public class ClinicalAssessmentController : BaseController
    {
        /// <summary>
        /// Returns default view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ClinicalAssessmentHeader()
        {
            return View();
        }

        public ActionResult ClinicalAssessmentReport()
        {
            return View();
        }
    }
}
