using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.Plugins.ECI.Controllers
{
    public class EligibilityCalculationController : BaseController
    {
        #region Action Results

        public ActionResult Calculation()
        {
            return View();
        }

        #endregion
    }
}
