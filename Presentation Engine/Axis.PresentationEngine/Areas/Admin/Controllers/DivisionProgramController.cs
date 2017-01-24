using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.PresentationEngine.Areas.Admin.Controllers
{
    public class DivisionProgramController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}