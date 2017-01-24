using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Clinical.Controllers
{
    public class MedicalHistoryController : BaseController
    {
        #region Action Results

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MedicalHistoryDetails()
        {
            return View();
        }

        #endregion
    }
}
