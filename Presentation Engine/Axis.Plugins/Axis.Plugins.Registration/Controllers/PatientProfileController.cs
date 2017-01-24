using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Registration.Controllers
{
    public class PatientProfileController : BaseController
    {
        #region Action Results

        public ActionResult Main()
        {
            return View();
        }

        public ActionResult General()
        {
            return View();
        }

        public ActionResult Navigation()
        {
            return View("../Shared/_PatientProfileNavigation");
        }

        #endregion
    }
}
