using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Registration.Controllers
{
    /// <summary>
    /// Controller for Email screen
    /// </summary>
    public class ContactEmailController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
