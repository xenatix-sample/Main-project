using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.ECI.Controllers
{
    public class ECIDemographicController : BaseController
    {
        #region Action Results

        public ActionResult Index()
        {
            return View();
        }

        #endregion
    }
}
