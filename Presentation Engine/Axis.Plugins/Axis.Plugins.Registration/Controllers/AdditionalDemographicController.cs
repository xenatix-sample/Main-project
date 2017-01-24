using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Registration.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class AdditionalDemographicController : BaseController
    {
        /// <summary>
        /// Indexes the specified contact identifier.
        /// </summary>
        /// <param name="ContactID">The contact identifier.</param>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Adds this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }
    }
}