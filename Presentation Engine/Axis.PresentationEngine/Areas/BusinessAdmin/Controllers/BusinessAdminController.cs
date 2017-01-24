using Axis.PresentationEngine.Helpers.Controllers;
using System.Web;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Controllers
{
    public class BusinessAdminController : BaseController
    {
        // GET: BusinessAdmin/BusinessMain
        public ActionResult Index()
        {
            return View();
        }
    }
}