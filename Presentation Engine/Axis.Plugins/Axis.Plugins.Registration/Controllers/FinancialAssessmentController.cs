using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Registration.Controllers
{
    /// <summary>
    /// Controller for Financial Assessment screen
    /// </summary>
    public class FinancialAssessmentController : BaseController
    {
        /// <summary>
        /// To show the index view 
        /// </summary>
        /// <returns>index view</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// To show Household info
        /// </summary>
        /// <returns></returns>
        public ActionResult HouseholdInfo()
        {
            return View();
        }

        /// <summary>
        /// To Get financial assesment partial view
        /// </summary>
        /// <returns></returns>

        public ActionResult _FinancialAssessment()
        {
            return View();
        }
    }
}
