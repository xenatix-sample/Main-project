using System.Threading.Tasks;
using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;


namespace Axis.Plugins.CallCenter.Controllers
{
    public class LawLiaisonEnforcementController : BaseController
    {
        /// <summary>
        /// Returns default view
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}
