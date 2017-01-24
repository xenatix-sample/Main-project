using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Areas.Admin.Controllers
{
    public class UserDirectReportsController : BaseController
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}