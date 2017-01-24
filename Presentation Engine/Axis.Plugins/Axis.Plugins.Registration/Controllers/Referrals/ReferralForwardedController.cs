using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.Plugins.Registration.Controllers.Referrals
{
    /// <summary>
    ///
    /// </summary>
    public class ReferralForwardedController : BaseController
    {
        #region Action Results

        /// <summary>
        /// Referral forwarded view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #endregion Action Results
    }
}