using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.Plugins.CallCenter.Controllers
{
    public class CallCenterSummaryController : BaseController
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FollowUpHistory()
        {
            var sharedNavView = "../Shared/_FollowUpHistory";
            return View(sharedNavView);
        }
    }
}
