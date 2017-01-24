using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.Plugins.ECI.Controllers
{
    public class ProgressNoteController : BaseController
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Attempts the index.
        /// </summary>
        /// <returns></returns>
        public ActionResult AttemptIndex()
        {
            return View();
        }

        /// <summary>
        /// Notes the navigation.
        /// </summary>
        /// <returns></returns>
        public ActionResult NoteNavigation()
        {
            var viewPath = "../Shared/_NoteNavigation";
            return View(viewPath);
        }
    }
}
