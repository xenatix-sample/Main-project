using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Routes;
using System.Web.Routing;
using Axis.Plugins.CallCenter.Infrastructure;

namespace Axis.Plugins.CallCenter
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.CallCenter.LawLiaison",
              "Plugins/CallCenter/LawLiaison/{action}",
              new { controller = "LawLiaison", action = "Index" },
              new[] { "Axis.Plugins.CallCenter.Controllers" }
              );
            routes.MapRoute("Plugin.CallCenter.CrisisLine",
              "Plugins/CallCenter/CrisisLine/{action}",
              new { controller = "CrisisLine", action = "Index" },
              new[] { "Axis.Plugins.CallCenter.Controllers" }
              );

            routes.MapRoute("Plugin.CallCenter.CallCenter",
              "Plugins/CallCenter/CallCenter/{action}",
              new { controller = "CallCenter", action = "Index" },
              new[] { "Axis.Plugins.CallCenter.Controllers" }
              );
            routes.MapRoute("Plugin.CallCenter.Registration",
                "Plugins/CallCenter/CallCenterRegistration/{action}",
                new { controller = "CallCenterRegistration", action = "Main" },
                new[] { "Axis.Plugins.CallCenter.Controllers" }
                );

            routes.MapRoute("Plugin.CallCenter.LawLiaisonSummary",
                "Plugins/CallCenter/LawLiaisonSummary/{action}",
                new { controller = "LawLiaisonSummary", action = "Index" },
                new[] { "Axis.Plugins.CallCenter.Controllers" });

            routes.MapRoute("Plugin.CallCenter.LawLiaisonEnforcement",
                "Plugins/CallCenter/LawLiaisonEnforcement/{action}",
               new { controller = "LawLiaisonEnforcement", action = "Index" },
               new[] { "Axis.Plugins.CallCenter.Controllers" });

            routes.MapRoute("Plugin.CallCenter.CrisisLineSummary",
                "Plugins/CallCenter/CrisisLineSummary/{action}",
                new { controller = "CrisisLineSummary", action = "Index" },
                new[] { "Axis.Plugins.CallCenter.Controllers" });

            routes.MapRoute("Plugin.CallCenter.CallerInformation",
                "Plugins/CallCenter/CallerInformation/{action}",
                new { controller = "CallerInformation", action = "Index" },
                new[] { "Axis.Plugins.CallCenter.Controllers" }
                );

            routes.MapRoute("Plugin.CallCenter.ServiceRecording",
               "Plugins/CallCenter/ServiceRecording/{action}",
               new { controller = "ServiceRecording", action = "Index" },
               new[] { "Axis.Plugins.CallCenter.Controllers" }
               );

            routes.MapRoute("Plugin.CallCenter.CallCenterProgressNote",
            "Plugins/CallCenter/CallCenterProgressNote/{action}",
            new { controller = "CallCenterProgressNote", action = "Index" },
            new[] { "Axis.Plugins.CallCenter.Controllers" }
            );

            routes.MapRoute("Plugin.CallCenter.LawLiaisonScreening",
                "Plugins/CallCenter/LawLiaisonScreening/{action}",
                new { controller = "LawLiaisonScreening", action = "Index" },
                new[] { "Axis.Plugins.CallCenter.Controllers" }
            );

            routes.MapRoute("Plugin.CallCenter.Signature",
                "Plugins/CallCenter/Signature/{action}",
                new { controller = "Signature", action = "Index" },
                new[] { "Axis.Plugins.CallCenter.Controllers" }
            );

            ViewEngines.Engines.Add(new CustomCallCenterViewEngine());

        }

        public int Priority
        {
            get
            {
                return 4;
            }
        }
    }
}
