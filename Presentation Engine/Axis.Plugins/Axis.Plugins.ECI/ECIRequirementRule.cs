using Axis.PluginsEngine;
using System.Web.Routing;

namespace Axis.Plugins.ECI
{
    public class ECIRequirementRule : BasePlugin, IPluginRules
    {
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Index";
            controllerName = "ECI";
            routeValues = new RouteValueDictionary { { "Namespaces", "Axis.Plugins.ECI.Controllers" }, { "area", null } };
        }
    }
}
