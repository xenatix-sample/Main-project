using Axis.Helpers.Infrastructure;
using Axis.PresentationEngine.Helpers.Routes;
using System.Web.Mvc;
using System.Web.Routing;

namespace Axis.PresentationEngine
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "GetPartial",
                url: "GetPartial",
                defaults: new {controller = "Home", action = "GetPartial", id = UrlParameter.Optional}
                );

            routes.MapRoute(
                name: "XenatixHome",
                url: "xenatix",
                defaults: new {controller = "Home", action = "Xenatix", id = UrlParameter.Optional}
                );

            routes.MapRoute(
                name: "XenatixPing",
                url: "xping",
                defaults: new {controller = "Home", action = "Xping", id = UrlParameter.Optional}
                );

            //register custom routes (plugins, etc)
            var routePublisher = EngineContext.Current.Resolve<IRoutePublisher>();
            routePublisher.RegisterRoutes(routes);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional}
                );
        }
    }
}