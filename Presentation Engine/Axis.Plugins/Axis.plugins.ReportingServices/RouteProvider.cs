using Axis.Plugins.ReportingServices.Infrastructure;
using Axis.PresentationEngine.Helpers.Routes;
using System.Web.Mvc;
using System.Web.Routing;

namespace Axis.Plugins.ReportingServices
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.ReportingServices.ReportingServices",
              "Plugins/ReportingServices/{action}",
              new { controller = "ReportingServices", action = "Index" },
              new[] { "Axis.Plugins.ReportingServices.Controllers" }
              );

            //routes.MapRoute("Plugin.ReportingServices.Reports",
            //  "Plugins/ReportingServices/Reports/{action}",
            //  new { controller = "ReportingServices", action = "Reports" },
            //  new[] { "Axis.Plugins.ReportingServices.Controllers" }
            //  );

            routes.MapRoute("Plugin.ReportingServices.ViewPdfRaw",
              "Plugins/ReportingServices/ViewPdfRaw/{key}",
              new { controller = "ReportingServices", action = "ViewPdfRaw" },
              new[] { "Axis.Plugins.ReportingServices.Controllers" }
              );

            routes.MapRoute("Plugin.ReportingServices.ViewReportExported",
              "Plugins/ReportingServices/ViewReportExported/{format}/{key}",
              new { controller = "ReportingServices", action = "ViewReportExported" },
              new[] { "Axis.Plugins.ReportingServices.Controllers" }
              );

            ViewEngines.Engines.Add(new CustomReportingServicesViewEngine());           
        }

        public int Priority
        {
            get
            {
                return 6;
            }
        }
    }
}
