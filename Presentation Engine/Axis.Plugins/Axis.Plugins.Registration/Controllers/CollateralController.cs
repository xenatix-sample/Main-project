using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Registration.Controllers
{
    /// <summary>
    /// Controller for Collateral screen
    /// </summary>
    public class CollateralController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
