using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Repository;
using System.Threading.Tasks;
using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.Plugins.Registration.Controllers
{
    /// <summary>
    /// Controller for Emergency Controller screen
    /// </summary>
    public class EmergencyContactController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
