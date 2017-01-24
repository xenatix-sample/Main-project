using System.Web.Mvc;

namespace Axis.Plugins.ReportingServices.Infrastructure
{
    public class CustomReportingServicesViewEngine : RazorViewEngine
    {
        public CustomReportingServicesViewEngine()
            : base()
        {
            ViewLocationFormats = new string[] {
                "~/Plugins/Axis.Plugins.ReportingServices/Views/{1}/{0}.cshtml",
                "~/Plugins/Axis.Plugins.ReportingServices/Views/Shared/{0}.cshtml"
            };

            PartialViewLocationFormats = new[] { "~/Plugins/Axis.Plugins.ReportingServices/Views/{1}/{0}.cshtml", "~/Plugins/Axis.Plugins.ReportingServices/Views/Shared/{0}.cshtml" };
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return base.FindView(controllerContext, viewName, masterName, useCache);
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            return base.FindPartialView(controllerContext, partialViewName, useCache);
        }
    }
}
