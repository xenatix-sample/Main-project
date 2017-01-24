using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.Plugins.Registration.Controllers.Referrals
{
    /// <summary>
    /// Referral referred to information controller
    /// </summary>
    public class ReferralReferredInformationController : BaseController
    {
        #region Action Results

        /// <summary>
        /// Referral referred information view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #endregion Action Results
    }
}