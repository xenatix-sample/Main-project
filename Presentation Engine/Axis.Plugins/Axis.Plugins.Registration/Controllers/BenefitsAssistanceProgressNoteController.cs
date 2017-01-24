using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.Plugins.Registration.Controllers
{
    public class BenefitsAssistanceProgressNoteController : BaseController
    {
        /// <summary>
        /// Controller for Benefits Assistance Progress Note
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BAPNNavigation()
        {
            return View();
        }
    }
}
