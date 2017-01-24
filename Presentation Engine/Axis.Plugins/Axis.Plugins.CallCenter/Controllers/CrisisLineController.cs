using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.CallCenter.Controllers
{
    public class CrisisLineController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
