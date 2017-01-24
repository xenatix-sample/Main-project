using System.Web.Optimization;
using Axis.PresentationEngine.Helpers.Bundles;

namespace Axis.Plugins.Registration
{
    public class BundleProvider : IBundleProvider
    {
        public void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/Registration").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/registrationNavigationController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/registrationController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/directives/contactSearchDirective.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/registrationService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactAliasService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/admissionService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactRaceService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/AdditionalDemographic").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/additionalDemographyController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/additionalDemographyService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactRaceService.js",
                "~/Scripts/app/controllers/raceDetailsController.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/EmergencyContacts").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/emergencyContactController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/emergencyContactService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/directives/xenCopyAddress.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/Benefits").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/ContactBenefitController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/collateralService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/ContactBenefitService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/registrationService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactRelationshipService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactAddressService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/Referral").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/referralDetailController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralAdditionalDetailService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralClientInformationService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralConcernDetailService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/registrationService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactRelationshipService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/directives/contactSearchDirective.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralHeaderService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactAddressService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactPhoneService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactEmailService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/Financial").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/financialAssessmentController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/financialDetailsController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/financialAssessmentService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/directives/financialassessmentdirective.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/FinancialAssessments").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/financialAssessmentsController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/financialSummaryService.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/eSignature.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/Topaz/SigWebTablet.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/jSignature.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/FinancialSummary").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/financialSummaryController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/financialSummaryService.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/eSignature.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/Topaz/SigWebTablet.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/jSignature.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactRelationshipService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/Collateral").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/collateralController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/directives/contactSearchDirective.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/collateralService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/clientSearchService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactAddressService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/directives/xenCopyAddress.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactRelationshipService.js",
                 "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactBenefitService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/Consent").Include(
                "~/Scripts/app/services/alertService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/consentController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/consentService.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/topazSignature.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/Topaz/SigWebTablet.js",
                "~/Areas/Consents/Scripts/app/services/consentsPrintService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/ClientSearch").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/clientSearchController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/clientSearchService.js",
                // From here on out, needed for offline caching
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/registrationService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/additionalDemographyService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralAdditionalDetailService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/emergencyContactService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/ContactBenefitService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/financialAssessmentService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/collateralService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/consentService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactAddressService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactPhoneService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactEmailService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralHeaderService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralConcernDetailService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/admissionService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactPhotoService.js",
                "~/Scripts/app/services/photoService.js",
                "~/Scripts/app/controllers/baseFlyoutController.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/PatientProfile").Include(
                "~/Scripts/app/directives/breadcrumbs.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/patientProfileController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactPhotoService.js",
                "~/Scripts/app/directives/toggleColumnView.js",
                "~/Scripts/app/directives/tileFlyout.js",
                "~/Scripts/app/directives/loadView.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactAliasService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactRaceService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralAdditionalDetailService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/selfPayService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/financialAssessmentService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralHeaderService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactRelationshipService.js",
                //bundles that will potentially be needed to load the eci patient header
                "~/Plugins/Axis.Plugins.ECI/Scripts/app/services/eciDemographicService.js",
                "~/Plugins/Axis.Plugins.ECI/Scripts/app/services/eciAdditionalDemographicService.js",
                "~/Areas/Consents/Scripts/app/services/consentsService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactBenefitService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/dischargeService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/ContactEmail").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/contactEmailController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactEmailService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/ContactPhone").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/contactPhoneController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactPhoneService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/Address").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactAddressService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/contactAddressController.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/RegistrationTile").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/registrationTileController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/additionalDemographyService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralAdditionalDetailService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/financialAssessmentService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/registrationService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/ContactBenefitService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/collateralService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactAddressService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactPhoneService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactEmailService.js",
                //ToDo: Add logic to check for the existence of a file before including it in the bundle, I know that missing files are ignored, but I think it's bad practice
                "~/Plugins/Axis.Plugins.ECI/Scripts/app/services/eciDemographicService.js",
                "~/Plugins/Axis.Plugins.ECI/Scripts/app/services/eciAdditionalDemographicService.js",
                "~/Plugins/Axis.Plugins.ECI/Scripts/app/services/eciRegistrationTileService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralAdditionalDetailService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralHeaderService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralConcernDetailService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/admissionService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactRaceService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/ConsentTile").Include(
               "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/ConsentTileController.js",
               "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/consentService.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/ReferralCommon").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/referralParentController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralHeaderService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralDispositionService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralFollowupService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralReferredInformationService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralForwardedService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralClientInformationService.js",
                "~/Scripts/app/filters/timeFilters.js",
                "~/Scripts/app/directives/breadcrumbs.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/ReferralSearch").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/referralSearchController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralSearchService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/registrationService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactAddressService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactPhoneService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactEmailService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/ReferredInformation").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralDispositionService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/referralReferredInformationController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralReferredInformationService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/ReferralFollowup").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/referralFollowupController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralFollowupService.js",
                "~/Scripts/app/filters/timeFilters.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/ReferralDisposition").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/referralDispositionController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralDispositionService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/ReferralClientInformation").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/referralClientController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralClientInformationService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/ReferralRequestor").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/referralRequestorController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralHeaderService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/registrationService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactAddressService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactPhoneService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactEmailService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/ReferralForwarded").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/referralForwardedController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralForwardedService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/ReferralProgressNote").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/referralProgressNoteController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/referralProgressNoteService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/Program").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/programController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/directives/registrationNavigator.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/DischargeCompany").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/dischargeCompanyController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/admissionsDischargeController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/dischargeProgramUnitController.js",
                "~/Plugins/Axis.Plugins.CallCenter/Scripts/app/controllers/signatureController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/admisionService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/dischargeService.js",
                "~/Areas/Admin/Scripts/app/services/userCredentialService.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/services/eSignatureService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/Admission").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/admissionController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/admissionService.js",
                "~/Scripts/app/filters/timeFilters.js",
                "~/Scripts/app/directives/toTime.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/SelfPay").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/selfPayController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/selfPayService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/financialAssessmentService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/BenefitsAssistanceProgressNote").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/benefitsAssistanceProgressNoteController.js",
                "~/Areas/Assessment/Scripts/app/services/assessmentPrintService.js",
                "~/Scripts/app/services/serviceRecordingService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/registrationService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/ContactBenefitService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/benefitsAssistanceProgressNoteService.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/Topaz/SigWebTablet.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/eSignature.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/jSignature.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/jSignature.UndoButton.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/flashcanvas.js",
                "~/Scripts/app/directives/pdfMaker.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/services/eSignatureService.js",
                "~/Areas/RecordedServices/Scripts/app/services/voidService.js",
                "~/Areas/BusinessAdmin/Scripts/app/services/organizationStructureService.js",
                "~/Areas/RecordedServices/Scripts/app/services/recordingServicePrintService.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/bapnDetails").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/bapnDetailsController.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/BapnNavigation").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/bapnNavigationController.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/BAPN").Include(
               "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/bapnController.js",
               "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/benefitsAssistanceProgressNoteService.js",
               "~/Plugins/Axis.Plugins.Registration/Scripts/app/directives/bapnNavigation.js",
               "~/Plugins/Axis.Plugins.ESignature/Scripts/app/services/eSignatureService.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/BenefitsTile").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/benefitsTileController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/selfPayService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/financialSummaryService.js",
                "~/Areas/Assessment/Scripts/app/services/assessmentPrintService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/benefitsAssistanceProgressNoteService.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/services/eSignatureService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/IntakeTile").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/intakeTileController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/lettersService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/intakeFormsService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/registrationService.js",
                "~/Areas/BusinessAdmin/Scripts/app/services/organizationStructureService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/Letters").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/lettersController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/directives/letterNavigation.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/directives/intakeformsNavigation.js",
                "~/Areas/Assessment/Scripts/app/services/assessmentPrintService.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/Topaz/SigWebTablet.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/eSignature.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/jSignature.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/jSignature.UndoButton.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/flashcanvas.js",
                "~/Scripts/app/directives/pdfMaker.js",
                "~/Scripts/app/services/serviceRecordingService.js",
                "~/Areas/RecordedServices/Scripts/app/services/voidService.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/services/eSignatureService.js",
                "~/Areas/RecordedServices/Scripts/app/services/recordingServicePrintService.js"
                ));
            
            bundles.Add(new ScriptBundle("~/bundles/historyLog").Include(
               "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/historyLogController.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/LettersDetails").Include(
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/lettersDetailsController.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactAddressService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/registrationService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactBenefitService.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/Plugins/Axis.Plugins.Registration/FormsDetails").Include(
               "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/formsDetailsController.js",
               "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactAddressService.js",
               "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactPhoneService.js",
               "~/Plugins/Axis.Plugins.Registration/Scripts/app/controllers/intakeFormNavigationController.js"
                ));

            var angularBundle = bundles.GetBundleFor("~/bundles/angular");
            if (angularBundle != null)
            {
                if (bundles.Remove(angularBundle))
                {
                    angularBundle.Include("~/Plugins/Axis.Plugins.Registration/Scripts/app/app.js");
                    bundles.Add(angularBundle);
                }//TODO: Else, scream as loud as you can!
            }
            var consentDetailBundle = bundles.GetBundleFor("~/bundles/consentDetails");
            if (consentDetailBundle != null && bundles.Remove(consentDetailBundle))
            {
                consentDetailBundle.Include(
                    "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/collateralService.js",
                    "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/registrationService.js",
                    "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactAddressService.js",
                    "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/additionalDemographyService.js"
                    );

                bundles.Add(consentDetailBundle);
            }

            var miniHeaderBundle = bundles.GetBundleFor("~/bundles/miniHeader");
            if (miniHeaderBundle != null && bundles.Remove(miniHeaderBundle))
            {
                miniHeaderBundle.Include(
                    "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/ContactBenefitService.js",
                    "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/registrationService.js",
                    "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactPhotoService.js"
                    );

                bundles.Add(miniHeaderBundle);
            }
        }
    }
}
