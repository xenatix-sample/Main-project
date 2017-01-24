using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.Plugins.CallCenter.Controllers
{
    public class LawLiaisonController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FollowUpHistory()
        {
            var sharedNavView = "../Shared/_LawLiaisonFollowUpHistory";
            return View(sharedNavView);
        }
    }
}