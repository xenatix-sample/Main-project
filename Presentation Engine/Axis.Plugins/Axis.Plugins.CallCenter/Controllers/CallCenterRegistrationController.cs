using Axis.PresentationEngine.Helpers.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Axis.Plugins.CallCenter.Controllers
{
    public class CallCenterRegistrationController : BaseController
    {
        public ActionResult Main()
        {
            return View();
        }

        public ActionResult CrisisLineNavigation()
        {
            var sharedNavView = "../Shared/_CrisisLineNavigation";
            return View(sharedNavView);
        }

        public ActionResult LawLiaisonNavigation()
        {
            var sharedNavView = "../Shared/_LawLiaisonNavigation";
            return View(sharedNavView);
        }
    }
}
