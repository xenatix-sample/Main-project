using Axis.PresentationEngine.Helpers.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Areas.Consents.Controllers
{
    public class ConsentsController : BaseController
    {
        /// <summary>
        /// Controller for consents screen
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult HIPAAConsent()
        {
            return View();
        }
    }
}