using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Clinical.Controllers
{
    public class NoteController : BaseController
    {
        /// <summary>
        /// Returns default view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Note Details view
        /// </summary>
        /// <returns></returns>
        public ActionResult NoteDetail()
        {
            return View();
        }
    }
}
