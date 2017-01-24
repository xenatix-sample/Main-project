using System.Web.Mvc;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Controllers
{
    public class PayorsConfigurationController : Controller
    {
        /// <summary>
        /// Payors 
        /// </summary>
        /// <returns></returns>
        public ActionResult Payors()
        {
            return View();
        }

        /// <summary>
        /// Payors plan.
        /// </summary>
        /// <returns></returns>
        public ActionResult PayorPlan()
        {
            return View();
        }

        /// <summary>
        /// Plans details.
        /// </summary>
        /// <returns></returns>
        public ActionResult PlanDetails()
        {
            return View();
        }

        /// <summary>
        /// Plan address.
        /// </summary>
        /// <returns></returns>
        public ActionResult PlanAddress()
        {
            return View();
        }
    }
}