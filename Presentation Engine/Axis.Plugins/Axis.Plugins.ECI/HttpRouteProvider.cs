using Axis.PresentationEngine.Helpers.Routes;
using System.Web.Http;

namespace Axis.Plugins.ECI
{
    public partial class HttpRouteProvider : IHttpRouteProvider
    {
        public void RegisterHttpRoutes(HttpRouteCollection routes)
        {
            routes.MapHttpRoute(
                name: "ECIApi",
                routeTemplate: "data/plugins/eci/{controller}/{action}"
                );
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
