using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Registration.Controllers
{
    /// <summary>
    /// Admission Controller
    /// </summary>
    public class AdmissionController : BaseController
    {
        /// <summary>
        /// View Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

    }
}