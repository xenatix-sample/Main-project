using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;
namespace Axis.PresentationEngine.Controllers
{
    public class AssessmentGridController : BaseController
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}