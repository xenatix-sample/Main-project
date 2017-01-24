using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.PresentationEngine.Helpers.Controllers.BaseController" />
    public class MergedContactsController : BaseController
    {
        // GET: BusinessAdmin/PotentialMatches
        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}