using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Clinical.Controllers
{
    public class ChiefComplaintController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
