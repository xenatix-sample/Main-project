using Axis.PresentationEngine.Helpers.Bundles;
using System.Web.Optimization;

namespace Axis.Plugins.ReportingServices
{
    public class BundleProvider : IBundleProvider
    {
        public void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.ReportingServices/ReportingServices")
                .Include(
                    "~/Plugins/Axis.Plugins.ReportingServices/Scripts/app/controllers/ListReportsController.js",
                    "~/Plugins/Axis.Plugins.ReportingServices/Scripts/app/controllers/ReportsController.js",
                    "~/Plugins/Axis.Plugins.ReportingServices/Scripts/app/services/reportingService.js",
                    "~/Plugins/Axis.Plugins.ReportingServices/Scripts/app/directives/ssrsParams.js",
                    "~/Plugins/Axis.Plugins.ReportingServices/Scripts/MvcReportViewer.js",
                    "~/Plugins/Axis.Plugins.ReportingServices/Scripts/url.js",
                    "~/Scripts/app/directives/breadcrumbs.js"
                ));

            var angularBundle = bundles.GetBundleFor("~/bundles/angular");
            if (angularBundle != null)
            {
                if (bundles.Remove(angularBundle))
                {
                    angularBundle.Include("~/Plugins/Axis.Plugins.ReportingServices/Scripts/app/app.js");
                    bundles.Add(angularBundle);
                }
            }
        }
    }
}
