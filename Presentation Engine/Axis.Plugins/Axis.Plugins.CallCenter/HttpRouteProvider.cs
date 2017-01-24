using Axis.PresentationEngine.Helpers.Routes;
using System.Web.Http;

namespace Axis.Plugins.CallCenter
{
    public class HttpRouteProvider : IHttpRouteProvider
    {
        public void RegisterHttpRoutes(HttpRouteCollection routes)
        {
            routes.MapHttpRoute(
                name: "CallCenterApi",
                routeTemplate: "data/plugins/callcenter/{controller}/{action}"
                );
        }

        public int Priority
        {
            get
            {
                return 4;
            }
        }
    }
}
