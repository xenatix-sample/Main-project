using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.PresentationEngine.Areas.Admin.Controllers
{
    public class StaffManagementController : BaseController
    {
        #region Action Results

        public ActionResult Index()
        {
            return View();
        }

        #endregion
    }
}