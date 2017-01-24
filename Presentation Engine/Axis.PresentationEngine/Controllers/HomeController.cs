using Axis.Configuration;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.PresentationEngine.Helpers.Filters;
using Axis.PresentationEngine.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Dynamic;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Controllers
{
    public class HomeController : BaseController
    {
        [Authorization(AllowAnonymous = true)]
        public ActionResult Index()
        {
            return RedirectToAction("Login", "Account", new { area = "Account" });
        }

        [Authorization(AllowAnonymous = false)]
        public ActionResult Xenatix()
        {
            ViewBag.UserFullName = Session["UserFullName"];
            ViewBag.UserRolePrimary = Session["UserRolePrimary"];
            return View();
        }

        [Authorization(AllowAnonymous = false)]
        public JsonResult Xping()
        {
            return Json(System.DateTime.Now, JsonRequestBehavior.AllowGet);
        }

        [Authorization(AllowAnonymous = true)]
        public ActionResult Settings(string angularModuleName = "xenatixApp.settings")
        {
            dynamic settings = new ExpandoObject();
            settings.WebApiBaseUrl = ApplicationSettings.PresentationApiUrl;
            settings.ReportViewerPath = ApplicationSettings.ReportViewerPath;
            settings.EnableSsrsTest = ApplicationSettings.EnableSsrsTest;
            settings.ReportPath = ApplicationSettings.ReportPath;
           
            //add additional config values as needed

            settings.EnableJsDebug = ApplicationSettings.EnableDebugMessages;

            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var settingsJson = JsonConvert.SerializeObject(settings, Formatting.Indented, serializerSettings);

            var settingsViewModel = new SettingsViewModel()
            {
                SettingsJson = settingsJson,
                AngularModuleName = angularModuleName
            };

            Response.ContentType = "text/javascript";
            return View(settingsViewModel);
        }
    }
}