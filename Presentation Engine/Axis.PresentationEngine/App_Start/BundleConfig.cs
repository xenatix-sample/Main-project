using Axis.Helpers.Infrastructure;
using Axis.PresentationEngine.Helpers.Bundles;
using System.Web.Optimization;

namespace Axis.PresentationEngine
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/Library/JQuery/jquery-{version}.js",
                "~/Scripts/Library/JQuery/perfect-scrollbar.jquery.min.js",
                "~/Scripts/Library/JQuery/jquery.lazyload.js",
                "~/Scripts/Library/JQuery/jquery.waypoints.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval")
                .Include("~/Scripts/Library/JQuery/jquery.validate*")
                );

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                "~/Scripts/library/angular/angular.js",
                "~/Scripts/library/angular-ui-router/angular-ui-router.min.js",
                "~/Scripts/library/angular/angular-http-batch.min.js",
                "~/Scripts/plugins/angular-input-modified.js",
                "~/Scripts/plugins/moment-with-locales.js",
                "~/Scripts/plugins/moment-timezone-with-data.js",
                "~/Scripts/library/angular/ui-mask.js",
                "~/Scripts/plugins/jquery.maskedinput.js",
                "~/Scripts/app/app.js",
                "~/Scripts/app/providers/idleTimeout.js",
                "~/Scripts/app/services/providersService.js",
                "~/Scripts/app/services/formService.js",
                "~/Scripts/app/services/globalObjectsService.js",
                "~/Scripts/app/services/navigationService.js",
                "~/Scripts/app/controllers/navigationController.js",
                "~/Scripts/app/directives/setFormStatus.js",
                "~/Scripts/app/providers/lazyLoader.js",
                "~/Scripts/plugins/toastr/toastr.js",
                "~/Scripts/app/services/alertService.js", // requires toastr
                "~/Scripts/library/angular/ngMask.js",
                "~/Scripts/app/helpers/common.js",
                "~/Scripts/app/helpers/enum.js",
                 "~/Scripts/app/helpers/enum_ScreenActionType.js",
                "~/Scripts/app/services/auditService.js",
                "~/Scripts/app/services/workflowHeaderService.js",
                "~/Areas/Security/Scripts/app/services/credentialSecurityService.js",
                "~/Scripts/app/services/scopesService.js",
                "~/Scripts/library/angular/ngStorage.js",
                "~/Scripts/app/services/cacheService.js",
                "~/Scripts/app/services/dateTimeValidatorService.js",
                "~/Scripts/app/services/xLog.js",
                "~/Scripts/plugins/md5.js",
                "~/Scripts/app/services/helperService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/angular-forms").Include(
                "~/Scripts/app/filters/bitwiseFilters.js",
                "~/Scripts/app/filters/commonFilters.js",
                "~/Scripts/app/filters/boolFilters.js",
                "~/Scripts/app/filters/toDateFilters.js",
                "~/Scripts/app/filters/xenUnique.js",
                "~/Scripts/app/filters/toPhone.js",
                "~/Scripts/app/filters/toSSN.js",
                "~/Scripts/app/directives/password.js",
                "~/Scripts/app/directives/autoFocus.js",
                "~/Scripts/app/directives/autoFocusModal.js",
                "~/Scripts/app/directives/xenProviders.js",
                "~/Scripts/app/directives/checkForm.js",
                "~/Scripts/app/directives/xenCheckForm.js",
                "~/Scripts/app/directives/xenSubmit.js",
                "~/Scripts/app/directives/inputMask.js",
                "~/Scripts/app/directives/mydatepicker.js",
                "~/Scripts/app/directives/xenDatePicker.js",
                "~/Scripts/app/directives/xenTimePicker.js",
                "~/Scripts/app/directives/xenDateRangePicker.js",
                "~/Scripts/app/directives/xenMultiSelect.js",
                "~/Scripts/app/directives/xenMultiSelectTypeahead.js",
                "~/Scripts/app/directives/number.js",
                "~/Scripts/app/directives/pageShortcuts.js",
                "~/Scripts/app/directives/pdfMaker.js",
                "~/Scripts/app/directives/plusButton.js",
                "~/Scripts/app/directives/plusButtonGridClear.js",
                "~/Scripts/app/directives/postalCode.js",
                "~/Scripts/app/directives/serverValidate.js",
                "~/Scripts/app/directives/tabAction.js",
                "~/Scripts/app/directives/toDateDirective.js",
                "~/Scripts/app/directives/toggleColumnView.js",
                "~/Scripts/app/directives/typeahead.js",
                "~/Scripts/app/directives/workflowAction.js",
                "~/Scripts/app/directives/xenCheckbox.js",
                "~/Scripts/app/directives/xenButtonCheckbox.js",
                "~/Scripts/app/directives/xenRadioButton.js",
                "~/Scripts/app/directives/xenMemoBox.js",
                "~/Scripts/app/directives/xenMaxlength.js",
                "~/Scripts/app/directives/xenLastElementFocus.js",
                "~/Scripts/app/directives/workFlowsSet.js",
                "~/Scripts/app/directives/xenWorkflows.js",
                "~/Scripts/app/directives/xenWorkflowAction.js",
                "~/Scripts/app/directives/auditOn.js",
                "~/Scripts/app/directives/checkboxDropdown.js",
                "~/Scripts/app/directives/inputLabel.js",
                "~/Scripts/app/directives/multiSelect.js",
                "~/Scripts/app/directives/xenFrame.js",
                "~/Scripts/app/services/contactSSNService.js",
                "~/Scripts/app/controllers/baseController.js",
                "~/Scripts/app/controllers/baseContactController.js",
                "~/Scripts/app/directives/xenSignature.js",
                "~/Scripts/app/directives/xenDigitalSignature.js",
                "~/Scripts/app/directives/xenComment.js",
                "~/Scripts/app/directives/flyoutContact.js",
                "~/Scripts/app/filters/securityFilter.js",
                "~/Scripts/app/filters/filterBy.js",
                "~/Scripts/library/angular/ngStorage.js",
                "~/Scripts/app/services/cacheService.js",
                "~/Scripts/app/services/dateTimeValidatorService.js",
                "~/Scripts/app/filters/timeFilters.js",
                "~/Scripts/app/directives/toolTip.js",
                "~/Scripts/app/directives/onRowRemoved.js",
                "~/Areas/RecordedServices/Scripts/app/services/voidService.js",
                "~/Scripts/app/directives/flyoutRecordedServiceVoid.js",
                "~/Scripts/app/services/auditService.js",
                "~/Scripts/app/services/workflowHeaderService.js",
                "~/Scripts/app/directives/xenReport.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/assessmentsGrid").Include(
                "~/Scripts/app/controllers/assessmentsGridController.js",
                "~/Plugins/Axis.Plugins.CallCenter/Scripts/app/services/callerInformationService.js",
                "~/Plugins/Axis.Plugins.CallCenter/Scripts/app/services/callCenterAssessmentPrintService.js",
                "~/Scripts/app/services/assessmentsGridService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactRaceService.js",
                "~/Areas/Assessment/Scripts/app/services/assessmentPrintService.js"
                
                ));

            bundles.Add(new ScriptBundle("~/bundles/chartJs").Include(
                "~/Scripts/library/Chart.min.js",
                "~/Scripts/app/directives/chartJs.js",
                "~/Scripts/app/directives/xenCheckbox.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/httpLoader").Include(
                "~/Scripts/app/directives/xenLoader.js",
                "~/Scripts/app/providers/httpLoaderInterceptor.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/anonymous").Include(
                "~/Scripts/library/angular/angular.js",
                "~/Scripts/library/angular-ui-router/angular-ui-router.min.js",
                "~/Scripts/app/anonymousApp.js",
                "~/Scripts/plugins/toastr/toastr.js",
                "~/Scripts/app/services/alertService.js",
                "~/Scripts/app/services/auditService.js",
                "~/Scripts/app/services/workflowHeaderService.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/angular-ui")
                .Include("~/Scripts/library/angular-ui/ui-bootstrap-tpls.js")
                .Include("~/Scripts/library/angular-ui/ui-bootstrap-tpls-extended.js")                
                );

            bundles.Add(new ScriptBundle("~/bundles/toastr")
                .Include("~/Scripts/plugins/toastr/toastr.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/lodash").Include(
                "~/Scripts/library/lodash/lodash.js",
                "~/Scripts/app/services/lodashService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/resize").Include(
                "~/Scripts/plugins/enquire.min.js",
                "~/Scripts/plugins/respondmore.min.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/site").Include(
                "~/Scripts/site.js",
                "~/Scripts/app/directives/security.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/registrationService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/login").Include(
                "~/Areas/Account/Scripts/app/helpers/loginHelper.js"
                ));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr")
                .Include("~/Scripts/modernizr-*")
                );

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/Library/Bootstrap/bootstrap.js",
                 "~/Scripts/Library/Bootstrap/daterangepicker.js",
                "~/Scripts/Library/Bootstrap/bootbox.js",
                "~/Scripts/Library/Bootstrap/bootstrap-table-all.js",
                "~/Scripts/Library/Bootstrap/bootstrap-timepicker.js",
                "~/Scripts/app/directives/bootstrapTable.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/offline").Include(
                 "~/Scripts/Library/sjcl.js",
                 "~/Scripts/app/services/connectionState.js",
                 "~/Scripts/app/providers/indexedDb.js",
                 "~/Scripts/app/providers/offlineData.js"
                 ));

            bundles.Add(new ScriptBundle("~/bundles/sync").Include(
                "~/Scripts/app/directives/syncProgress.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                "~/Areas/Admin/Scripts/app/controllers/adminController.js",
                "~/Areas/Admin/Scripts/app/services/adminService.js",
                "~/Scripts/app/directives/breadcrumbs.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/role").Include(
                "~/Areas/Security/Scripts/app/controllers/roleController.js",
                "~/Areas/Security/Scripts/app/controllers/roleModulesController.js",
                "~/Areas/Security/Scripts/app/controllers/RoleNavigationController.js",
                "~/Areas/Security/Scripts/app/services/roleService.js",
                "~/Scripts/app/directives/dynamicNavigation.js",
                "~/Areas/Security/Scripts/app/controllers/roleManagementController.js",
                "~/Areas/Security/Scripts/app/services/roleManagementService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/settings").Include(
                "~/Areas/Configuration/Scripts/app/controllers/settingsController.js",
                "~/Areas/Configuration/Scripts/app/services/settingsService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/userProfile").Include(
                "~/Areas/Account/Scripts/app/controllers/userProfileController.js",
                "~/Areas/Account/Scripts/app/services/userProfileService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/assessment").Include(
                "~/Areas/Assessment/Scripts/app/controllers/assessmentController.js",
                "~/Areas/Assessment/Scripts/app/directives/assessmentSection.js",
                "~/Areas/Assessment/Scripts/app/services/assessmentPrintService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/pdfmake").Include(
                "~/Scripts/library/pdfmake/pdfmake.min.js",
                "~/Scripts/library/pdfmake/vfs_fonts.js"
                ));

            //Reviewed and Discussed with Karl Jablonski
            //Major challenge is I can not see the history to see what has changed because I have been told that it was working in the past
            bundles.Add(new ScriptBundle("~/bundles/tileController")
            );
            bundles.Add(new ScriptBundle("~/Home/Datepicker")
            );

            bundles.Add(new ScriptBundle("~/bundles/chartTileController").Include(
                "~/Scripts/app/controllers/chartTileController.js",
                "~/Scripts/app/services/serviceRecordingService.js",
                "~/Areas/BusinessAdmin/Scripts/app/services/organizationStructureService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/photo").Include(
                "~/Scripts/app/directives/photoProfile.js",
                "~/Scripts/app/services/photoService.js",
                "~/Scripts/plugins/imageBuilder.js",
                "~/Scripts/plugins/webcam.js",
                "~/Scripts/plugins/owl.carousel.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/dashboard").Include(
                "~/Areas/Dashboard/Scripts/app/controllers/dashboardController.js",
                "~/Areas/Dashboard/Scripts/app/services/dashboardService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/staffManagement").Include(
                "~/Areas/Admin/Scripts/app/controllers/staffManagementController.js",
                "~/Areas/Admin/Scripts/app/services/staffManagementService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/staffManagementNavigation").Include(
                "~/Areas/Admin/Scripts/app/controllers/staffManagementNavigationController.js",
                "~/Areas/Admin/Scripts/app/services/userRoleService.js",
                "~/Areas/Admin/Scripts/app/services/userCredentialService.js",
                "~/Areas/Admin/Scripts/app/controllers/userHeaderController.js",
                "~/Areas/Admin/Scripts/app/services/userHeaderService.js",
                "~/Areas/Admin/Scripts/app/services/userDetailService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/userDetail").Include(
                "~/Areas/Admin/Scripts/app/controllers/userDetailController.js",
                "~/Areas/Admin/Scripts/app/services/userDetailService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/userRole").Include(
                "~/Areas/Admin/Scripts/app/controllers/userRoleController.js",
                "~/Areas/Admin/Scripts/app/services/userRoleService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/userCredential").Include(
                "~/Areas/Admin/Scripts/app/controllers/userCredentialController.js",
                "~/Areas/Admin/Scripts/app/services/userCredentialService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/configuration")
                .Include("~/Areas/Configuration/Scripts/app/controllers/configController.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/locations")
                .Include("~/Areas/Configuration/Scripts/app/controllers/locationsController.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/locationsNavigation")
                .Include("~/Areas/Configuration/Scripts/app/controllers/locationsNavigationController.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/locationInfo")
                .Include("~/Areas/Configuration/Scripts/app/controllers/locationInfoController.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/blockedtime").Include(
               "~/Scripts/app/controllers/blockTimeController.js",
               "~/Scripts/app/filters/timeFilters.js",
               "~/Scripts/app/directives/toTimeNoPad.js",
               "~/Scripts/plugins/owl.carousel.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/roomschedule")
                .Include("~/Areas/Configuration/Scripts/app/controllers/roomScheduleController.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/divisionprogram").Include(
               "~/Scripts/app/controllers/divisionProgramController.js",
               "~/Scripts/app/services/divisionProgramService.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/userscheduling").Include(
                "~/Scripts/app/controllers/userSchedulingController.js",
                "~/Scripts/app/services/userSchedulingService.js",
                "~/Scripts/app/filters/timeFilters.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/UserDirectReports").Include(
                "~/Areas/Admin/Scripts/app/controllers/userDirectReportsController.js",
                "~/Areas/Admin/Scripts/app/services/userDirectReportsService.js",
                "~/Areas/Account/Scripts/app/services/userProfileService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/userPhoto").Include(
                "~/Areas/Admin/Scripts/app/controllers/userPhotoController.js",
                "~/Areas/Admin/Scripts/app/services/userPhotoService.js",
                "~/Areas/Account/Scripts/app/controllers/myProfileNavigationController.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/UserAdditionalDetails")
                .Include("~/Areas/Admin/Scripts/app/controllers/userAdditionalDetailsController.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/ServiceList").Include(
                "~/Scripts/app/controllers/recordingServiceListController.js",
                "~/Scripts/app/services/serviceRecordingService.js",
                "~/Areas/RecordedServices/Scripts/app/services/recordingServicePrintService.js",
                "~/Areas/Account/Scripts/app/services/userProfileService.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/services/eSignatureService.js",
                "~/Scripts/app/directives/pdfMaker.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/ContactBenefitService.js",
                "~/Scripts/app/directives/serviceFlyout.js",
                "~/Areas/Assessment/Scripts/app/services/assessmentPrintService.js",
                "~/Plugins/Axis.Plugins.CallCenter/Scripts/app/services/callCenterProgressNoteService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/benefitsAssistanceProgressNoteService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/intakeFormsService.js",
                 "~/Plugins/Axis.Plugins.CallCenter/Scripts/app/services/lawLiaisonscreeningService.js",
                 "~/Plugins/Axis.Plugins.CallCenter/Scripts/app/services/callCenterCrisislineService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/ServiceRecording").Include(
                "~/Scripts/app/controllers/serviceRecordingController.js",
                "~/Scripts/app/services/serviceRecordingService.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/services/eSignatureService.js",
                "~/Areas/Admin/Scripts/app/services/userCredentialService.js",
                "~/Areas/RecordedServices/Scripts/app/services/voidService.js",
                "~/Areas/RecordedServices/Scripts/app/services/recordingServicePrintService.js",
                "~/Areas/Account/Scripts/app/services/userProfileService.js"
              ));

            bundles.Add(new ScriptBundle("~/bundles/RecordingService").Include(
                "~/Scripts/app/controllers/recordingServiceController.js",
                "~/Scripts/app/services/serviceRecordingService.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/services/eSignatureService.js",
                "~/Areas/Admin/Scripts/app/services/userCredentialService.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/eSignature.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/jSignature.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/jSignature.UndoButton.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/flashcanvas.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/Topaz/SigWebTablet.js",
                "~/Areas/RecordedServices/Scripts/app/services/voidService.js",
                "~/Areas/RecordedServices/Scripts/app/services/recordingServicePrintService.js",
                "~/Areas/Account/Scripts/app/services/userProfileService.js",
                "~/Areas/BusinessAdmin/Scripts/app/services/organizationStructureService.js"
              ));

            bundles.Add(new ScriptBundle("~/bundles/CallCenterSignature").Include(
                "~/Plugins/Axis.Plugins.CallCenter/Scripts/app/controllers/signatureController.js",
                "~/Areas/Admin/Scripts/app/services/userCredentialService.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/services/eSignatureService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/myProfile")
                .Include("~/Areas/Account/Scripts/app/directives/profileBreadcrumbs.js")
                );

            bundles.Add(new ScriptBundle("~/bundles/myProfileNav").Include(
                "~/Areas/Account/Scripts/app/controllers/myProfileNavigationController.js",
                "~/Areas/Account/Scripts/app/controllers/profileHeaderController.js",
                "~/Areas/Account/Scripts/app/services/userProfileService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/usersecurity").Include(
                "~/Areas/Account/Scripts/app/controllers/userSecurityController.js",
                "~/Areas/Account/Scripts/app/services/userSecurityService.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/services/eSignatureService.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/eSignature.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/jSignature.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/jSignature.UndoButton.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/flashcanvas.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/Topaz/SigWebTablet.js",
                "~/Areas/Account/Scripts/app/filters/excludeSelect.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/userDigitalSignatue").Include(
              "~/Areas/Account/Scripts/app/controllers/userDigitalSignatureController.js",
              "~/Areas/Account/Scripts/app/services/userSecurityService.js",
              "~/Plugins/Axis.Plugins.ESignature/Scripts/app/services/eSignatureService.js",
              "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/eSignature.js",
              "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/jSignature.js",
              "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/jSignature.UndoButton.js",
              "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/flashcanvas.js",
              "~/Plugins/Axis.Plugins.ESignature/Scripts/Topaz/SigWebTablet.js",
              "~/Areas/Account/Scripts/app/filters/excludeSelect.js"
              ));

            bundles.Add(new ScriptBundle("~/bundles/consents").Include(
                "~/Areas/Consents/Scripts/app/controllers/consentsController.js",
                "~/Areas/Consents/Scripts/app/controllers/consentsNavigationController.js",
                "~/Areas/Consents/Scripts/app/services/consentsService.js",
                "~/Areas/Consents/Scripts/app/services/consentsPrintService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/consentDetails").Include(
                "~/Areas/Consents/Scripts/app/controllers/consentDetailsController.js",
                "~/Areas/Consents/Scripts/app/services/consentsPrintService.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/Topaz/SigWebTablet.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/eSignature.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/jSignature.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/jSignature.UndoButton.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/flashcanvas.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/services/eSignatureService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactRelationshipService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/consentUI").Include(
                "~/Areas/Consents/Scripts/app/controllers/consentHIPAAController.js",
                "~/Areas/Consents/Scripts/app/services/consentsPrintService.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/Topaz/SigWebTablet.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/services/eSignatureService.js",
                "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/ContactBenefitService.js",
                "~/Scripts/app/services/navigationService.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/eSignature.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/jSignature.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/jSignature.UndoButton.js",
                "~/Plugins/Axis.Plugins.ESignature/Scripts/app/directives/flashcanvas.js",
                "~/Scripts/app/directives/pdfMaker.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/miniHeader")
                .Include("~/Scripts/app/controllers/miniHeaderController.js")
                );

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/animate.css",
                "~/Content/daterangepicker.css",
                "~/Content/datepicker.css",
                "~/Content/style.min.css",
                "~/Content/font-awesome.min.css",
                "~/Content/toastr.min.css",
                "~/Content/xenatix.min.css",
                "~/Content/checkbox-dropdown.css",
                "~/Content/float-label.css",
                "~/Content/timepicker.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/clientMerge").Include(
                "~/Areas/BusinessAdmin/Scripts/app/controllers/clientMergeController.js",
                "~/Areas/BusinessAdmin/Scripts/app/services/clientMergeService.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/healthRecords").Include(
               "~/Areas/BusinessAdmin/Scripts/app/controllers/healthRecordsController.js",
               "~/Areas/BusinessAdmin/Scripts/app/services/healthRecordsService.js",
               "~/Areas/BusinessAdmin/Scripts/app/directives/healthRecords.js"
               ));
            bundles.Add(new ScriptBundle("~/bundles/configuration").Include(
               "~/Areas/BusinessAdmin/Scripts/app/controllers/configurationController.js"
               ));
            bundles.Add(new ScriptBundle("~/bundles/payors").Include(
               "~/Areas/BusinessAdmin/Scripts/app/controllers/payorsNavigationController.js",
               "~/Areas/BusinessAdmin/Scripts/app/controllers/payorsController.js",
               "~/Areas/BusinessAdmin/Scripts/app/services/payorsService.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/companyConfiguration").Include(
               "~/Areas/BusinessAdmin/Scripts/app/controllers/companyConfigurationController.js",
               "~/Areas/BusinessAdmin/Scripts/app/services/companyConfigurationService.js",
               "~/Areas/BusinessAdmin/Scripts/app/services/organizationStructureService.js"
           ));

            bundles.Add(new ScriptBundle("~/bundles/divisionConfiguration").Include(
               "~/Areas/BusinessAdmin/Scripts/app/controllers/divisionConfigurationController.js",
               "~/Areas/BusinessAdmin/Scripts/app/services/divisionConfigurationService.js",
               "~/Areas/BusinessAdmin/Scripts/app/services/organizationStructureService.js"
           ));

            bundles.Add(new ScriptBundle("~/bundles/programConfiguration").Include(
               "~/Areas/BusinessAdmin/Scripts/app/controllers/programConfigurationController.js",
               "~/Areas/BusinessAdmin/Scripts/app/services/programConfigurationService.js",
               "~/Areas/BusinessAdmin/Scripts/app/services/organizationStructureService.js"
           ));

            bundles.Add(new ScriptBundle("~/bundles/programUnitConfiguration").Include(
               "~/Areas/BusinessAdmin/Scripts/app/controllers/programUnitConfigurationController.js",
               "~/Areas/BusinessAdmin/Scripts/app/services/programUnitConfigurationService.js",
               "~/Areas/BusinessAdmin/Scripts/app/services/organizationStructureService.js"
           ));
            bundles.Add(new ScriptBundle("~/bundles/payorPlan").Include(
               "~/Areas/BusinessAdmin/Scripts/app/controllers/payorPlansController.js",
               "~/Areas/BusinessAdmin/Scripts/app/controllers/payorsNavigationController.js",
               "~/Areas/BusinessAdmin/Scripts/app/services/payorPlansService.js"
               ));
            bundles.Add(new ScriptBundle("~/bundles/planDetails").Include(
              "~/Areas/BusinessAdmin/Scripts/app/controllers/planAddressesController.js",
              "~/Areas/BusinessAdmin/Scripts/app/services/planAddressesService.js",
              "~/Areas/BusinessAdmin/Scripts/app/services/payorPlansService.js",
              "~/Areas/BusinessAdmin/Scripts/app/services/payorsService.js"
              ));
            bundles.Add(new ScriptBundle("~/bundles/planAddressDetails").Include(
              "~/Areas/BusinessAdmin/Scripts/app/controllers/planAddressController.js",
              "~/Areas/BusinessAdmin/Scripts/app/services/planAddressesService.js",
               "~/Areas/BusinessAdmin/Scripts/app/services/payorPlansService.js"
              ));

            bundles.Add(new ScriptBundle("~/bundles/organizationStructure").Include(
            "~/Areas/BusinessAdmin/Scripts/app/controllers/organizationStructureNavigationController.js"
            ));
            
            bundles.Add(new ScriptBundle("~/bundles/services").Include(
               "~/Areas/BusinessAdmin/Scripts/app/controllers/servicesNavigationController.js",
               "~/Areas/BusinessAdmin/Scripts/app/controllers/servicesController.js",
               "~/Areas/BusinessAdmin/Scripts/app/services/serviceDefinitionService.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/servicedefinition").Include(
               "~/Areas/BusinessAdmin/Scripts/app/controllers/serviceDefinitionController.js",
               "~/Areas/BusinessAdmin/Scripts/app/services/serviceDefinitionService.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/servicedetails").Include(
              "~/Areas/BusinessAdmin/Scripts/app/controllers/serviceDetailsController.js",
              "~/Areas/BusinessAdmin/Scripts/app/services/serviceDetailsService.js",
               "~/Areas/BusinessAdmin/Scripts/app/services/serviceDefinitionService.js"
              ));

            bundles.Add(new ScriptBundle("~/bundles/clientmergeNavigation").Include(
            "~/Areas/BusinessAdmin/Scripts/app/controllers/clientMergeNavigationController.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/PotentialMatches").Include(
                "~/Areas/BusinessAdmin/Scripts/app/controllers/potentialMatchesController.js",
                "~/Areas/BusinessAdmin/Scripts/app/services/clientMergeService.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/CompareRecords").Include(
               "~/Areas/BusinessAdmin/Scripts/app/controllers/compareRecordsController.js",
               "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactAddressService.js",
               "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactPhoneService.js",
               "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactEmailService.js",
               "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/collateralService.js",
               "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactBenefitService.js",
               "~/Plugins/Axis.Plugins.Registration/Scripts/app/services/contactRelationshipService.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/MergedContacts").Include(
                "~/Areas/BusinessAdmin/Scripts/app/controllers/mergedContactsController.js"
                ));
            //register custom bundles (plugins, etc)
            var bundlePublisher = EngineContext.Current.Resolve<IBundlePublisher>();
            bundlePublisher.RegisterBundles(bundles);
            BundleTable.EnableOptimizations = true;
        }
    }
}
