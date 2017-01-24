using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.Plugins.Registration.Controllers
{
    public class DischargeCompanyController : BaseController
    {

        #region Action Method

        /// <summary>
        /// Index view.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Main Registration Screen Method
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Main()
        {
            return View();
        }

       

        #endregion Action Method
    }
}
