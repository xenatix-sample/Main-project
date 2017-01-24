using System.Web.Optimization;
using Axis.PresentationEngine.Helpers.Bundles;

namespace Axis.Plugins.CallCenter
{
    public class BundleProvider : IBundleProvider
    {
        public void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.CallCenter/LawLiaisonSummary")
                .Include("~/Areas/BusinessAdmin/Scripts/app/services/organizationStructureService.js")
                .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/services/callCenterAssessmentPrintService.js")
                .Include("~/Scripts/app/services/serviceRecordingService.js")
                .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/controllers/lawLiaisonSummaryController.js")
                .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/services/lawLiaisonSummaryService.js")
                .Include("~/Areas/RecordedServices/Scripts/app/services/voidService.js")
                .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/services/lawLiaisonFollowUpService.js")
                .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/services/callerInformationService.js")
                .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/services/registrationService.js")
                .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/services/callCenterProgressNoteService.js")
                .Include("~/Areas/Assessment/Scripts/app/services/assessmentPrintService.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.CallCenter/CrisisLineSummary")
                .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/controllers/crisisLineSummaryController.js")
                .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/services/crisisLineSummaryService.js")
                .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/services/callerInformationService.js")
                .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/services/crisisLineFollowUpService.js")
                .Include("~/Areas/RecordedServices/Scripts/app/services/voidService.js")
                );
            
            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.CallCenter/CallCenter")
                .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/controllers/callCenterCrisislineController.js",
                "~/Plugins/Axis.Plugins.CallCenter/Scripts/app/controllers/callCenterLawliaisonController.js",
                "~/Plugins/Axis.Plugins.CallCenter/Scripts/app/services/callerInformationService.js",
                "~/Plugins/Axis.Plugins.CallCenter/Scripts/app/services/callCenterProgressNoteService.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/services/eSignatureService.js",
                "~/Scripts/app/services/serviceRecordingService.js",
                "~/Areas/BusinessAdmin/Scripts/app/services/organizationStructureService.js",
                "~/Areas/Admin/Scripts/app/services/userPhotoService.js",
                "~/Areas/Assessment/Scripts/app/services/assessmentPrintService.js",
                "~/Plugins/Axis.Plugins.CallCenter/Scripts/app/services/callCenterAssessmentPrintService.js"));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.CallCenter/ColumbiaSuicideScale")
              .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/controllers/columbiaSuicideScaleController.js")
              .Include("~/Areas/Assessment/Scripts/app/services/assessmentPrintService.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/QuickRegistration")
             .Include("~/Scripts/app/controllers/quickRegistrationController.js")
             .Include("~/Scripts/app/controllers/raceDetailsController.js")
             .Include("~/Scripts/app/directives/quickRegistrationFlyout.js")
             .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/controllers/callCenterQuickRegistrationController.js")
             .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactRaceService.js")
             .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/services/admissionService.js")
             );

            bundles.Add(new ScriptBundle("~/bundles/Signature")
              .Include("~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/eSignature.js")
              .Include("~/Plugins/Axis.Plugins.ESignature/Scripts/Topaz/SigWebTablet.js")
              .Include("~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/jSignature.js")
             );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.CallCenter/CrisisAssessment")
              .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/controllers/crisisAssessmentController.js",
                        "~/Areas/Assessment/Scripts/app/services/assessmentPrintService.js"));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.CallCenter/AdultScreening")
              .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/controllers/adultScreeningController.js",
                        "~/Areas/Assessment/Scripts/app/services/assessmentPrintService.js"));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.CallCenter/ChildScreening")
              .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/controllers/childScreeningController.js",
                        "~/Areas/Assessment/Scripts/app/services/assessmentPrintService.js"));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.CallCenter/LawLiaisonscreening")
              .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/controllers/lawLiaisonscreeningController.js",
                        "~/Areas/Assessment/Scripts/app/services/assessmentPrintService.js",
                        "~/Plugins/Axis.Plugins.CallCenter/Scripts/app/services/lawLiaisonscreeningService.js"));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.CallCenter/CallerInformation")
                .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/controllers/callerInformationController.js")
                .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/directives/contactSearchDirective.js")
                .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/services/callerInformationService.js")
                .Include("~/Scripts/app/directives/breadcrumbs.js")
                .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/directives/duplicateContactDetection.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.CallCenter/CallCenterProgressNote")
              .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/controllers/callCenterProgressNoteController.js")
              .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/services/callCenterProgressNoteService.js")
              .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/services/callerInformationService.js")
              .Include("~/Scripts/app/services/contactSSNService.js")
              .Include("~/Areas/Account/Scripts/app/services/userProfileService.js")
              .Include("~/Areas/Assessment/Scripts/app/services/assessmentPrintService.js")
              );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.CallCenter/CallCenterReport").Include(
                "~/Plugins/Axis.Plugins.CallCenter/Scripts/app/controllers/callCenterReportController.js",
                "~/Areas/Assessment/Scripts/app/services/assessmentPrintService.js",
                "~/Plugins/Axis.Plugins.CallCenter/Scripts/app/services/callCenterAssessmentPrintService.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.CallCenter/LawLiaisonEnforcement")
              .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/controllers/lawLiaisonEnforcementController.js")
              .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/services/callerInformationService.js")
              .Include("~/Scripts/app/directives/breadcrumbs.js")
              .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/directives/programInfo.js")
              .Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/directives/duplicateContactDetection.js")
              );

            var angularBundle = bundles.GetBundleFor("~/bundles/angular");
            if (angularBundle != null)
            {
                if (bundles.Remove(angularBundle))
                {
                    angularBundle.Include("~/Plugins/Axis.Plugins.CallCenter/Scripts/app/app.js");
                    bundles.Add(angularBundle);
                }//TODO: Else, scream as loud as you can!
            }
        }
    }
}
