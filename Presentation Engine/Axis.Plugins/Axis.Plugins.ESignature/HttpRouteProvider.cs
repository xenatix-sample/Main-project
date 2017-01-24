using Axis.PresentationEngine.Helpers.Routes;
using System.Web.Http;

namespace Axis.Plugins.ESignature
{
    public partial class HttpRouteProvider : IHttpRouteProvider
    {
        public void RegisterHttpRoutes(HttpRouteCollection routes)
        {
            routes.MapHttpRoute(
                name: "ESignatureApi",
                routeTemplate: "data/plugins/esignature/{controller}/{action}"
                );
        }

        public int Priority
        {
            get
            {
                return 1;
            }
        }
    }
}
