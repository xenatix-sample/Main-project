using System;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Areas.Dashboard.Controllers
{
    public class DashboardController : BaseController
    {
        // GET: Dashboard/Dashboard
        public ActionResult Index()
        {
            var hourOfTheDay = DateTime.Now.Hour;
            ViewBag.Greeting = string.Format("Good {0}, {1}.",
                hourOfTheDay < 12 ? "Morning" :
                hourOfTheDay < 17 ? "Afternoon" : "Evening",
                Session["UserFirstName"]);
            return View();
        }
    }
}