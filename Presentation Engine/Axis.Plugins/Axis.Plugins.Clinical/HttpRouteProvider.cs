using Axis.PresentationEngine.Helpers.Routes;
using System.Web.Http;

namespace Axis.Plugins.Clinical
{
    public partial class HttpRouteProvider : IHttpRouteProvider
    {
        public void RegisterHttpRoutes(HttpRouteCollection routes)
        {
            routes.MapHttpRoute(
                name: "ClinicalApi",
                routeTemplate: "data/plugins/clinical/{controller}/{action}"
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
