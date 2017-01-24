using System.Web.Optimization;
using Axis.PresentationEngine.Helpers.Bundles;

namespace Axis.Plugins.ECI
{
    public class BundleProvider : IBundleProvider
    {
        public void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.ECI/ECIRegistration")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/controllers/eciRegistrationController.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.ECI/ECIDemographic")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/controllers/eciDemographicController.js")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/eciDemographicService.js")
                //services needed for the initial navigation validation states under eci registration
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/eciAdditionalDemographicService.js")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/referralAdditionalDetailService.js")
                .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/services/collateralService.js")
                .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactBenefitService.js")
                .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/services/financialAssessmentService.js")
                .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactAliasService.js")
                );
            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.ECI/ECIAdditionalDemographic")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/controllers/eciAdditionalDemographicController.js")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/eciAdditionalDemographicService.js")
                .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactRaceService.js")
                .Include("~/Scripts/app/controllers/raceDetailsController.js")
                );
            

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.ECI/Screening")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/controllers/screeningController.js")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/screeningService.js")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/directives/screeningNavigation.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.ECI/EligibilityDetermination")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/controllers/eligibilityDeterminationController.js")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/eligibilityDeterminationService.js")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/eciDemographicService.js")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/eligibilityCalculationService.js")
                .Include("~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/topazSignature.js")
                .Include("~/Plugins/Axis.Plugins.ESignature/Scripts/Topaz/SigWebTablet.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.ECI/EligibilityCalculation")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/controllers/eligibilityCalculationController.js")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/eligibilityCalculationService.js")
                .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/services/registrationService.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.ECI/IFSP")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/controllers/ifspController.js")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/ifspService.js")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/eligibilityDeterminationService.js")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/directives/ifspNavigation.js")
                .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/services/collateralService.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.ECI/ProgressNote")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/controllers/progressNoteController.js")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/progressNoteService.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.ECI/Referral")
                .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/referralDetailController.js")
                .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralAdditionalDetailService.js")
                .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralConcernDetailService.js")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/eciDemographicService.js")
                .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralHeaderService.js")
                .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactAddressService.js")
               .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactPhoneService.js")
               .Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactEmailService.js")
                );

            var angularBundle = bundles.GetBundleFor("~/bundles/angular");
            if (angularBundle != null && bundles.Remove(angularBundle))
            {
                angularBundle.Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/app.js");
                bundles.Add(angularBundle);
            }
            var tileBundle = bundles.GetBundleFor("~/bundles/tileController");
            if (tileBundle != null && bundles.Remove(tileBundle))
            {
                tileBundle.Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/screeningService.js");
                tileBundle.Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/eligibilityDeterminationService.js");
                tileBundle.Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/ifspService.js");
                tileBundle.Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/eciTileService.js");
                tileBundle.Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/progressNoteService.js");
                bundles.Add(tileBundle);
            }

            //Inject the ECI services needed by client search for downloading a client's data for offline use
            var clientSearchBundle = bundles.GetBundleFor("~/bundles/Plugins/Axis.Plugins.Registration/ClientSearch");
            if (clientSearchBundle != null && bundles.Remove(clientSearchBundle))
            {
                clientSearchBundle.Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/screeningService.js")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/eligibilityDeterminationService.js")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/eligibilityCalculationService.js")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/ifspService.js")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/eciDemographicService.js")
                .Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/eciAdditionalDemographicService.js");
                bundles.Add(clientSearchBundle);
            }
            
            var consentDetailBundle = bundles.GetBundleFor("~/bundles/consentDetails");
            if (consentDetailBundle != null && bundles.Remove(consentDetailBundle))
            {
                consentDetailBundle.Include("~/Plugins/Axis.Plugins.ECI/Scripts/app/services/eciAdditionalDemographicService.js");
                bundles.Add(consentDetailBundle);
            }
        }

    }
}
