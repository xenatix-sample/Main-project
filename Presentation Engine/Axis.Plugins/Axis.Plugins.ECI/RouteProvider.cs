using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Routes;
using System.Web.Routing;
using Axis.Plugins.ECI.Infrastructure;

namespace Axis.Plugins.ECI
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.ECIPlugins",
                "Plugins/ECI/{action}",
                new {controller = "ECI", action = "Index"},
                new[] {"Axis.Plugins.ECI.Controllers"}
                );
            routes.MapRoute("Plugin.ECIPlugins.Screening",
                "Plugins/ECI/Screening/{action}",
                new {controller = "Screening", action = "Index"},
                new[] {"Axis.Plugins.ECI.Controllers"}
                );
            routes.MapRoute("Plugin.ECIPlugins.ScreeningHeader",
                "Plugins/ECI/ScreeningHeader/{action}",
                new { controller = "Screening", action = "ScreeningHeader" },
                new[] { "Axis.Plugins.ECI.Controllers" }
                );
            routes.MapRoute("Plugin.ECIPlugins.ScreeningSignature",
                "Plugins/ECI/ScreeningSignature/{action}",
                new { controller = "Screening", action = "ScreeningSignature" },
                new[] { "Axis.Plugins.ECI.Controllers" }
                );
            routes.MapRoute("Plugin.ECIPlugins.EligibilityDeterminationMain",
                "Plugins/ECI/EligibilityDetermination/{action}",
                new { controller = "EligibilityDetermination", action = "Main" },
                new[] { "Axis.Plugins.ECI.Controllers" }
                );
            routes.MapRoute("Plugin.ECIPlugins.EligibilityDetermination",
                "Plugins/ECI/EligibilityDetermination/{action}",
                new { controller = "EligibilityDetermination", action = "Index" },
                new[] { "Axis.Plugins.ECI.Controllers" }
                );
            routes.MapRoute("Plugin.ECIPlugins.EligibilityCalculation",
                "Plugins/ECI/EligibilityCalculation/{action}",
                new { controller = "EligibilityCalculation", action = "Calculation" },
                new[] { "Axis.Plugins.ECI.Controllers" }
                );
            routes.MapRoute("Plugin.ECIPlugins.IFSP",
                "Plugins/ECI/IFSP/{action}",
                new {controller = "IFSP", action = "Index"},
                new[] {"Axis.Plugins.ECI.Controllers"}
                );
            routes.MapRoute("Plugin.ECIPlugins.IFSPHeader",
                "Plugins/ECI/IFSPHeader/{action}",
                new { controller = "IFSP", action = "IFSPHeader" },
                new[] { "Axis.Plugins.ECI.Controllers" }
                );
            routes.MapRoute("Plugin.ECIPlugins.IFSPReport",
                "Plugins/ECI/IFSPReport/{action}",
                new { controller = "IFSP", action = "IFSPReport" },
                new[] { "Axis.Plugins.ECI.Controllers" }
                );
            routes.MapRoute("Plugin.ECIPlugins.Registration",
                "Plugins/ECI/ECIRegistration/{action}",
                new { controller = "ECIRegistration", action = "Main" },
                new[] { "Axis.Plugins.ECI.Controllers" }
                );
            routes.MapRoute("Plugin.ECIPlugins.ECIDemographic",
                "Plugins/ECI/ECIDemographic/{action}",
                new { controller = "ECIDemographic", action = "Index" },
                new[] { "Axis.Plugins.ECI.Controllers" }
                );

            routes.MapRoute("Plugin.ECIPlugins.ECIAdditionalDemographic",
               "Plugins/ECI/ECIAdditionalDemographic/{action}",
               new { controller = "ECIAdditionalDemographic", action = "Index" },
               new[] { "Axis.Plugins.ECI.Controllers" }
               );
            routes.MapRoute("Plugin.ECIPlugins.ProgressNote",
               "Plugins/ECI/ProgressNote/{action}",
               new { controller = "ProgressNote", action = "Index" },
               new[] { "Axis.Plugins.ECI.Controllers" }
               );

            ViewEngines.Engines.Add(new CustomECIViewEngine());
        }

        public int Priority
        {
            get
            {
                return 2;
            }
        }
    }
}
