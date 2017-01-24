using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class PhotoController : BaseController
    {
        //
        // GET: /Common/Photo/
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult PhotoProfile()
        {
            return View();
        }
    }
}