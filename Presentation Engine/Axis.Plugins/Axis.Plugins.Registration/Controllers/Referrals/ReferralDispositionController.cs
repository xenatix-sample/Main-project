using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.Plugins.Registration.Controllers.Referrals
{
    public class ReferralDispositionController : BaseController
    {
        /// <summary>
        /// Referral disposition view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}
