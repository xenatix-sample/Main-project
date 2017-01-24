using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Controllers
{
    public class HealthRecordsController : Controller
    {
        
        // GET: BusinessAdmin/HealthRecords        
        public ActionResult Index()
        {
            return View();
        }
    }
}