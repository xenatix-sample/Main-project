using Axis.PresentationEngine.Helpers.Bundles;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace Axis.Plugins.Scheduling
{
    public class BundleProvider : IBundleProvider
    {
        public void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Scheduling/Scheduling")
                .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralClientInformationService.js")
                .Include("~/Plugins/Axis.Plugins.Scheduling/Scripts/app/controllers/appointmentController.js")
                .Include("~/Plugins/Axis.Plugins.Scheduling/Scripts/app/controllers/appointmentDetailController.js")
                .Include("~/Plugins/Axis.Plugins.Scheduling/Scripts/app/services/appointmentService.js")
                .Include("~/Scripts/app/filters/timeFilters.js")
                .Include("~/Scripts/app/directives/toTime.js")
                .Include("~/Scripts/plugins/owl.carousel.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Scheduling/Calendar")
                .Include("~/Plugins/Axis.Plugins.Scheduling/Scripts/app/controllers/calendarController.js")
                .Include("~/Plugins/Axis.Plugins.Scheduling/Scripts/app/services/appointmentService.js")
                .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/services/registrationService.js")
                .Include("~/Plugins/Axis.Plugins.Scheduling/Scripts/app/services/groupSchedulingSearchService.js")
                .Include("~/Scripts/app/services/navigationService.js")
                .Include("~/Scripts/app/filters/timeFilters.js")
                .Include("~/Scripts/app/directives/toTime.js")
                .Include("~/Scripts/plugins/owl.carousel.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Scheduling/GroupSchedulingSearch")
               .Include("~/Plugins/Axis.Plugins.Scheduling/Scripts/app/controllers/groupSchedulingSearchController.js")
               .Include("~/Plugins/Axis.Plugins.Scheduling/Scripts/app/services/groupSchedulingSearchService.js")
               .Include("~/Scripts/app/directives/breadcrumbs.js")
           );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Scheduling/GroupSchedulingNavigation")
                .Include("~/Plugins/Axis.Plugins.Scheduling/Scripts/app/controllers/groupScheduleNavigationController.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Scheduling/GroupScheduling")
                .Include("~/Plugins/Axis.Plugins.Scheduling/Scripts/app/controllers/groupSchedulingController.js")
                .Include("~/Plugins/Axis.Plugins.Scheduling/Scripts/app/services/groupSchedulingService.js")
                .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/services/clientSearchService.js")         
            );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Scheduling/GroupNote")
                .Include("~/Plugins/Axis.Plugins.Scheduling/Scripts/app/controllers/groupNoteController.js")
                .Include("~/Areas/Admin/Scripts/app/services/userDetailService.js")
                .Include("~/Plugins/Axis.Plugins.ESignature/Scripts/app/services/eSignatureService.js")
            );

            var angularBundle = bundles.GetBundleFor("~/bundles/angular");
            if (angularBundle != null)
            {
                if (bundles.Remove(angularBundle))
                {
                    angularBundle.Include("~/Plugins/Axis.Plugins.Scheduling/Scripts/app/app.js");
                    angularBundle.Include("~/Plugins/Axis.Plugins.Scheduling/Scripts/app/services/appointmentService.js");
                    bundles.Add(angularBundle);
                }//TODO: Else, scream as loud as you can!
            }
        }
    }
}
