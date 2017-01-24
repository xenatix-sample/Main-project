using System.Web.Mvc;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Controllers
{
    public class OrganizationStructureController : Controller
    {

        /// <summary>
        /// Companies this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Company()
        {
            return View();
        }
        /// <summary>
        /// Divisions this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Division()
        {
            return View();
        }

        /// <summary>
        /// Programses this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Programs()
        {
            return View();
        }

        /// <summary>
        /// Programs the units.
        /// </summary>
        /// <returns></returns>
        public ActionResult ProgramUnits()
        {
            return View();
        }
    }
}