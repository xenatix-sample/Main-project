using System.Web.Optimization;
using Axis.PresentationEngine.Helpers.Bundles;

namespace Axis.Plugins.Clinical
{
    public class BundleProvider : IBundleProvider
    {
        public void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Clinical/Allergy")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/controllers/allergyController.js")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/allergyService.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Clinical/MedicalHistory")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/controllers/medicalHistoryController.js")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/medicalHistoryService.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Clinical/MedicalHistoryDetails")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/controllers/medicalHistoryDetailsController.js")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/medicalHistoryService.js")
                .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/services/registrationService.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Clinical/Note")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/controllers/noteController.js")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/noteService.js")
            );
            
            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Clinical/NoteDetail")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/controllers/noteDetailController.js")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/noteService.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Clinical/ReviewOfSystems")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/controllers/reviewOfSystemsController.js")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/reviewOfSystemsService.js")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/directives/rosNavigation.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Clinical/Vital")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/controllers/vitalController.js")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/vitalService.js")
                .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/services/registrationService.js")
            );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Clinical.SocialRelationshipHistory")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/controllers/socialRelationshipHistoryController.js")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/socialRelationshipHistoryService.js")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/socialRelationshipService.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Clinical/ChiefComplaint")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/controllers/chiefComplaintController.js")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/chiefComplaintService.js")
                .Include("~/Scripts/app/services/alertService.js")
                .Include("~/Scripts/app/filters/toDateFilters.js")
                .Include("~/Scripts/app/directives/toDateDirective.js")
                .Include("~/Scripts/app/directives/pageShortcuts.js")
                .Include("~/Scripts/app/directives/toDateDirective.js")
                .Include("~/Scripts/app/directives/mydatepicker.js")
                .Include("~/Scripts/app/directives/inputMask.js")
                .Include("~/Scripts/app/directives/serverValidate.js")
                .Include("~/Scripts/app/directives/workflowAction.js")
                .Include("~/Scripts/app/directives/autoFocus.js")
                .Include("~/Scripts/app/directives/checkForm.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Clinical/clinicalAssessment")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/controllers/clinicalAssessmentController.js")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/clinicalAssessmentService.js")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/directives/clinicalAssessmentNavigation.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Clinical/SocialRelationship")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/controllers/socialRelationshipController.js")
                .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/socialRelationshipService.js")
                );


            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Clinical/PresentIllness")
               .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/controllers/presentIllnessController.js")
               .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/presentIllnessService.js")
               );

            var angularBundle = bundles.GetBundleFor("~/bundles/angular");
            if (angularBundle != null && bundles.Remove(angularBundle))
            {
                angularBundle.Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/app.js");
                bundles.Add(angularBundle);
            }

            var tileBundle = bundles.GetBundleFor("~/bundles/tileController");
            if (tileBundle != null && bundles.Remove(tileBundle))
            {
                tileBundle.Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/clinicalTileService.js")
                    .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/noteService.js")
                    .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/reviewOfSystemsService.js")
                    .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/chiefComplaintService.js")
                    .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/clinicalAssessmentService.js")
                    .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/vitalService.js")
                    .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/allergyService.js")
                    .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/socialRelationshipService.js")
                    .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/presentIllnessService.js")
                    .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/medicalHistoryService.js");
                bundles.Add(tileBundle);
            }

            //Inject the clinical services needed by client search for downloading a client's data for offline use
            var clientSearchBundle = bundles.GetBundleFor("~/bundles/Plugins/Axis.Plugins.Registration/ClientSearch");
            if (clientSearchBundle != null && bundles.Remove(clientSearchBundle))
            {
                clientSearchBundle.Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/allergyService.js")
                    .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/vitalService.js")
                    .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/socialRelationshipService.js")
                    .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/socialRelationshipHistoryService.js")
                    .Include("~/Plugins/Axis.Plugins.Clinical/Scripts/app/services/clinicalAssessmentService.js");
                bundles.Add(clientSearchBundle);
            }
        }
    }
}
