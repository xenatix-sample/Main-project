using System.Web.Mvc;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Controllers
{
    public class ServiceConfigurationController : Controller
    {
        /// <summary>
        /// Serviceses this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Services()
        {
            return View();
        }

        /// <summary>
        /// Services the definition.
        /// </summary>
        /// <returns></returns>
        public ActionResult ServiceDefinition()
        {
            return View();
        }

        /// <summary>
        /// Services the details.
        /// </summary>
        /// <returns></returns>
        public ActionResult ServiceDetails()
        {
            return View();
        }

        
    }
}