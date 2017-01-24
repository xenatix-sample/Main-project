using Axis.Plugins.Registration.Repository.Referrals;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Axis.Plugins.Registration.Controllers.Referrals
{
    /// <summary>
    /// This class is use to perform action for referral search screen
    /// </summary>
    /// <seealso cref="Axis.PresentationEngine.Helpers.Controllers.BaseController" />
    public class ReferralSearchController : BaseController
    {
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Returns referral view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ReferralNavigation()
        {
            var strView = "../Shared/_ReferralNavigation";
            return View(strView);
        }
    }
}
