using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.PresentationEngine.Areas.Security.Controllers
{
    public class RoleManagementController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}