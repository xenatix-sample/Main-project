using Axis.Plugins.Scheduling.Infrastructure;
using Axis.PresentationEngine.Helpers.Routes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Axis.Plugins.Scheduling
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.SchedulingPlugins",
                "Plugins/Scheduling/{action}",
                new { controller = "Appointment", action = "Schedule" },
                new[] { "Axis.Plugins.Scheduling.Controllers" }
                );

            routes.MapRoute("Plugin.Scheduling.ResourcePlugins",
                "Plugins/Scheduling/Resource/{action}",
                new { controller = "Resource", action = "Index" },
                new[] { "Axis.Plugins.Scheduling.Controllers" }
                );

            routes.MapRoute("Plugin.Scheduling.RecurrencePlugins",
                "Plugins/Scheduling/Recurrence/{action}",
                new { controller = "Recurrence", action = "Index" },
                new[] { "Axis.Plugins.Scheduling.Controllers" }
                );

            routes.MapRoute("Plugin.Schedule.GroupSchedulingPlugins",
                "Plugins/GroupScheduling/{action}",
                new { controller = "GroupScheduling", action = "Index" },
                new[] { "Axis.Plugins.Scheduling.Controllers" }
                );

            ViewEngines.Engines.Add(new CustomViewEngine());
        }

        public int Priority
        {
            get
            {
                return 3;
            }
        }
    }
}
