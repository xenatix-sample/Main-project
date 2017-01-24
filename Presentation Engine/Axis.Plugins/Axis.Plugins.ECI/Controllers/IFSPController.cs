using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.ECI.Controllers
{
    public class IFSPController :  BaseController
    {
        /// <summary>
        /// Returns default view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IFSPs()
        {
            return View();
        }

        public ActionResult IFSPHeader()
        {
            return View();
        }

        public ActionResult IFSPReport()
        {
            return View();
        }
    }
}