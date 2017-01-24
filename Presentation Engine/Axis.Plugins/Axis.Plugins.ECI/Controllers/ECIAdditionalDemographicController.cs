using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.Plugins.ECI.Controllers
{
    public class ECIAdditionalDemographicController : BaseController
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
