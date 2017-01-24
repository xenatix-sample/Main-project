using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Registration.Controllers.Referrals
{
    public class ReferralClientInformationController : BaseController
    {
        /// <summary>
        /// Returns Referrals Client Information view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}
