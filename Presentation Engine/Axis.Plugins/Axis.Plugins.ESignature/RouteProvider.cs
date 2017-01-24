using System.Web.Mvc;
using Axis.PresentationEngine.Helpers.Routes;
using System.Web.Routing;
using Axis.Plugins.ESignature.Infrastructure;

namespace Axis.Plugins.ESignature
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            ViewEngines.Engines.Add(new CustomESigViewEngine());
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
