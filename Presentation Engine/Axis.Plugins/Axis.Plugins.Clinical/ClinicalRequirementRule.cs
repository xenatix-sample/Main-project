using Axis.PluginsEngine;
using System.Web.Routing;

namespace Axis.Plugins.Clinical
{
    public class ClinicalRequirementRule : BasePlugin, IPluginRules
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
            controllerName = "Clinical";
            routeValues = new RouteValueDictionary { { "Namespaces", "Axis.Plugins.Clinical.Controllers" }, { "area", null } };
        }
    }
}
