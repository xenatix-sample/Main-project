using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.ECI.Controllers
{
    public class EligibilityDeterminationController : BaseController
    {
        #region Action Results

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EligibilityNavigation()
        {
            return View("_EligibilityNavigation");
        }

        public ActionResult EligibilityHeader()
        {
            return View();
        }

        public ActionResult EligibilitySignature()
        {
            return View();
        }

        public ActionResult Notes()
        {
            return View();
        }

        public ActionResult Report()
        {
            return View();
        }

        #endregion
    }
}
