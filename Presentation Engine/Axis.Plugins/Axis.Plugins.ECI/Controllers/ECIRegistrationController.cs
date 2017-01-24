using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.ECI.Controllers
{
    public class ECIRegistrationController : BaseController
    {
        #region Action Results

        public ActionResult Main()
        {
            return View();
        }

        public ActionResult ECIRegistrationNavigation()
        {
            var sharedNavView = "../Shared/_ECIRegistrationNavigation";
            return View(sharedNavView);
        }

        #endregion
    }
}
