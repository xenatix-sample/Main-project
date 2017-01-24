using System.Web.Routing;

namespace Axis.PresentationEngine.Helpers.Routes
{
    public interface IRouteProvider
    {
        void RegisterRoutes(RouteCollection routes);

        int Priority { get; }
    }
}
