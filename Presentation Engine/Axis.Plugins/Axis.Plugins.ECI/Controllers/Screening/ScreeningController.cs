using System.Web.Mvc;

namespace Axis.Plugins.ECI.Controllers
{
    public class ScreeningController : Controller
    {
        #region Action Results

        public ActionResult Index()
        {                        
            return View();
        }

        public ActionResult ScreeningHeader()
        {
            return View();
        }

        public ActionResult ScreeningReport()
        {
            return View();
        }

        #endregion
    }
}
