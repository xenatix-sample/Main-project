using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.Plugins.Registration.Controllers.Referrals
{
    /// <summary>
    ///
    /// </summary>
    public class ReferralFollowupController : BaseController
    {
        #region Action Results

        /// <summary>
        /// Referral followup view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #endregion Action Results
    }
}