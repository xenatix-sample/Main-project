using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.Plugins.Registration.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class ContactPhotoController : BaseController
    {
        #region Action Method

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        #endregion Action Method
    }
}