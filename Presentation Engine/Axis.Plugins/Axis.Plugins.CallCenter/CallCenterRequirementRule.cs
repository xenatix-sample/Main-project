using Axis.PluginsEngine;
using System.Web.Routing;

namespace Axis.Plugins.CallCenter
{
    public class CallCenterRequirementRule : BasePlugin, IPluginRules
    {
        public override void Install()
        {
            base.Install();
        }

        public override void Uninstall()
        {
            base.Uninstall();
        }

        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Index";
            controllerName = "CallCenter";
            routeValues = new RouteValueDictionary { { "Namespaces", "Axis.Plugins.CallCenter.Controllers" }, { "area", null } };
        }
    }
}
