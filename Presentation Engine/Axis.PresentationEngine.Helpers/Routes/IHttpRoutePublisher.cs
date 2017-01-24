using System.Web.Http;

namespace Axis.PresentationEngine.Helpers.Routes
{
    /// <summary>
    /// Route publisher
    /// </summary>
    public interface IHttpRoutePublisher
    {
        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="routes">Routes</param>
        void RegisterHttpRoutes(HttpRouteCollection routes);
    }
}
