using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Registration.Controllers
{
    /// <summary>
    /// This class is use to perform action for client search screen
    /// </summary>
    public class ClientSearchController : BaseController
    {
        #region Action methods

        /// <summary>
        /// Returns the client search view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Searches the contact.
        /// </summary>
        /// <returns></returns>
        public ActionResult ContactSearch()
        {
            return View();
        }

        /// <summary>
        /// This controller will redirect to the dashboard.
        /// </summary>
        /// <returns></returns>
        public ActionResult GoToHomePage()
        {
            return RedirectToAction("Index", "Dashboard", new { area = "Dashboard" });
        }
        #endregion
    }
}
