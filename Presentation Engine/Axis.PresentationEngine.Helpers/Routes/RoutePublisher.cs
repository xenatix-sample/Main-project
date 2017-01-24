using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Axis.Helpers.Infrastructure;
using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Plugins;

namespace Axis.PresentationEngine.Helpers.Routes
{
    /// <summary>
    /// Route publisher
    /// </summary>
    public class RoutePublisher : AppStartPublisher, IRoutePublisher
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="typeFinder"></param>
        public RoutePublisher(ITypeFinder typeFinder) : base(typeFinder)
        {
        }

        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="routes">Routes</param>
        public virtual void RegisterRoutes(RouteCollection routes)
        {
            var routeProviderTypes = typeFinder.FindClassesOfType<IRouteProvider>();
            var routeProviders = new List<IRouteProvider>();
            foreach (var providerType in routeProviderTypes)
            {
                //Ignore not installed plugins
                var plugin = FindPlugin(providerType);
                if (plugin != null && !plugin.Installed)
                    continue;

                var provider = Activator.CreateInstance(providerType) as IRouteProvider;
                routeProviders.Add(provider);
            }

            routeProviders = routeProviders.OrderByDescending(rp => rp.Priority).ToList();
            System.Web.Mvc.ViewEngines.Engines.Clear();
            routeProviders.ForEach(rp => rp.RegisterRoutes(routes));
            System.Web.Mvc.ViewEngines.Engines.Add(new RazorViewEngine());
        }
    }
}
