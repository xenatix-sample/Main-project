using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.PresentationEngine.Areas.Admin.Controllers
{
    public class UserHeaderController : BaseController
    {
        public ActionResult UserHeader()
        {
            var headerView = "../Shared/_UserHeader";
            return View(headerView);
        }
    }
}