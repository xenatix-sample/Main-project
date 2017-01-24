using Axis.PresentationEngine.Helpers.Routes;
using System.Web.Http;

namespace Axis.Plugins.Registration
{
    public partial class HttpRouteProvider : IHttpRouteProvider
    {
        public void RegisterHttpRoutes(HttpRouteCollection routes)
        {
            routes.MapHttpRoute(
                name: "RegistrationApi",
                routeTemplate: "data/plugins/registration/{controller}/{action}"
                );
        }

        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}
