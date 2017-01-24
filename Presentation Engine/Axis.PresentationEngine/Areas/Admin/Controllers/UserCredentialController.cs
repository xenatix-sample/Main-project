using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Controllers;

namespace Axis.PresentationEngine.Areas.Admin.Controllers
{
    public class UserCredentialController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}