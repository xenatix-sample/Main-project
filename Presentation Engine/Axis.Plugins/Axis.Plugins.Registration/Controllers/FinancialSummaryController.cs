using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Registration.Controllers
{
    /// <summary>
    /// Controller for Financial Assessment screen
    /// </summary>
    public class FinancialSummaryController : BaseController
    {
        /// <summary>
        /// To show the index view 
        /// </summary>
        /// <returns>index view</returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}
