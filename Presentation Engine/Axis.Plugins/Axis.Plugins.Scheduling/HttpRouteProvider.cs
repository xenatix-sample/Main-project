using Axis.PresentationEngine.Helpers.Routes;
using System.Web.Http;

namespace Axis.Plugins.Scheduling
{
    public partial class HttpRouteProvider : IHttpRouteProvider
    {
        public void RegisterHttpRoutes(HttpRouteCollection routes)
        {
            routes.MapHttpRoute(
                name: "SchedulingApi",
                routeTemplate: "data/plugins/scheduling/{controller}/{action}"
                );
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
