using System;
using System.Linq;
using System.Web.Http;
using Axis.Helpers.Infrastructure;
using Axis.PresentationEngine.Helpers.Plugins;

namespace Axis.PresentationEngine.Helpers.Routes
{
    /// <summary>
    /// Route publisher
    /// </summary>
    public class HttpRoutePublisher : AppStartPublisher, IHttpRoutePublisher
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="typeFinder"></param>
        public HttpRoutePublisher(ITypeFinder typeFinder) : base(typeFinder)
        {
        }

        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="routes">Routes</param>
        public virtual void RegisterHttpRoutes(HttpRouteCollection routes)
        {
            (
                from providerType in typeFinder.FindClassesOfType<IHttpRouteProvider>()
                let plugin = FindPlugin(providerType)
                where plugin == null || plugin.Installed
                select Activator.CreateInstance(providerType) as IHttpRouteProvider
                ).OrderByDescending(rp => rp.Priority).ToList().ForEach(rp => rp.RegisterHttpRoutes(routes));
        }
    }
}
