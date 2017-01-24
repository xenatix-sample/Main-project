using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Registration.Controllers
{
    public class ProgramController : BaseController
    {
        #region Constructors

        public ProgramController()
        {

        }

        #endregion

        #region Action Results

        public ActionResult Index()
        {
            return View();
        }

        #endregion
    }
}
