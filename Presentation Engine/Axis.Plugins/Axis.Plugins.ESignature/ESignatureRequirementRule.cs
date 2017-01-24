using Axis.PluginsEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace Axis.Plugins.ESignature
{
    public class ESignatureRequirementRule : BasePlugin, IPluginRules
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
            controllerName = "ESignature";
            routeValues = new RouteValueDictionary { { "Namespaces", "Axis.Plugins.ESignature.Controllers" }, { "area", null } };
        }
    }
}