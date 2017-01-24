using System.Web.Http;

namespace Axis.PresentationEngine.Helpers.Routes
{
    public interface IHttpRouteProvider
    {
        void RegisterHttpRoutes(HttpRouteCollection routes);

        int Priority { get; }
    }
}
