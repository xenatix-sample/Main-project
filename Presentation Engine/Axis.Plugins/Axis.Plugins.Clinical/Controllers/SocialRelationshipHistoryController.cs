using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.Plugins.Clinical.Controllers
{
    public class SocialRelationshipHistoryController : BaseController
    {
        #region Action Results

        /// <summary>
        /// Returns default view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #endregion
    }
}
