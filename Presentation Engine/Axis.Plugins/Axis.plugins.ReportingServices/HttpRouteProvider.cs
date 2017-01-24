using System.Web.Http;
using Axis.PresentationEngine.Helpers.Routes;

namespace Axis.plugins.ReportingServices
{
    public class HttpRouteProvider : IHttpRouteProvider
    {
        public void RegisterHttpRoutes(HttpRouteCollection routes)
        {
            routes.MapHttpRoute(
                name: "ReportingServicesApi",
                routeTemplate: "data/plugins/reportingservices/{controller}/{action}"
                );
        }

        public int Priority
        {
            get
            {
                return 5;
            }
        }
    }
}
