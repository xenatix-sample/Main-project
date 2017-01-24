using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.Plugins.Registration.Controllers
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Axis.PresentationEngine.Helpers.Controllers.BaseController" />
    public class ContactAliasController : BaseController
    {
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