(function () {
    angular.module('xenatixApp')
    .controller('adultScreeningController', ['$scope', '$filter', '$q', 'alertService', '$stateParams', '$state', '$rootScope', 'formService', 'assessmentService', 'registrationService', 'lookupService', 'callerInformationService', 'sectionID', 'responseID', 'credentialSecurityService', 'contactBenefitService', 'contactAddressService', 'additionalDemographyService', 'contactSSNService', 'contactRaceService', 'navigationService', 'assessmentPrintService', 'cacheService', 'serviceRecordingService', 'contactPhoneService', 'WorkflowHeaderService', 'callCenterAssessmentPrintService',
    function ($scope, $filter, $q, alertService, $stateParams, $state, $rootScope, formService, assessmentService, registrationService, lookupService, callerInformationService, sectionID, responseID, credentialSecurityService, contactBenefitService, contactAddressService, additionalDemographyService, contactSSNService, contactRaceService, navigationService, printService, cacheService, serviceRecordingService, contactPhoneService, WorkflowHeaderService, callCenterAssessmentPrintService) {
        $scope.ContactID = $stateParams.ContactID;
        $scope.AssessmentID = ASSESSMENT_TYPE.CrisisAdultScreening;
        var reportModel = null;
       

        $scope.inputType = {
            Button: 1,
            Checkbox: 2,
            Radio: 3,
            Textbox: 4,
            Select: 5,
            MultiSelect: 6,
            None: 7,
            DatePicker: 8,
            TextArea: 9
        };

        $scope.init = function () {
            angular.extend($stateParams, { SectionID: sectionID, ResponseID: responseID});
            $state.transitionTo($state.current.name, $stateParams);
        };

        $scope.prepopulatedData = function () {
            var dfd = $q.defer();
            $scope.GET_DATE = $filter('toMMDDYYYYDate')(new Date, 'MM/DD/YYYY', 'useLocal');
            navigationService.get().then(function (response) {
                if (response && response.DataItems && response.DataItems.length > 0) {
                    $scope.STAFF_NAME = response.DataItems[0].UserFullName;
                    credentialSecurityService.getUserCredentialSecurity().then(function (credData) {
                        if (hasData(credData)) {
                            $scope.CredentialList = $filter('filter')(credData.DataItems, { CredentialActionForm: "Crisis Adult Screening", CredentialAction: "Digital Signature" }, true);
                            if ($scope.CredentialList && $scope.CredentialList.length == 1) {
                                $scope.CREDENTIAL_ID = $scope.CredentialList[0].CredentialID;
                        }
                            dfd.resolve(credData);
                        }
                        else
                            dfd.resolve(response);
                    })
                }
            });
            return dfd.promise;
        };

        $scope.initReport = function (response) {
            if (response != "-1") {
                alertService.success('Assessment Response saved successfully.');
                //save workflow Header details.
                //$rootScope.$broadcast('onWorkflowHeaderSave', { workflowDataKey: $state.current.data.workflowDataKey, workflowHeaderID: $stateParams.CallCenterHeaderID });
                WorkflowHeaderService.AddWorkflowHeader({ WorkflowDataKey: $state.current.data.workflowDataKey, RecordHeaderID: $stateParams.CallCenterHeaderID, ContactID: $stateParams.ContactID });

            }
            return printService.initReports($scope.AssessmentID, responseID, sectionID).then(onPrintReportReceived.bind(this));
        };

        $scope.postAssessmentReponseDetails = function () {
            $scope.isDisabled = (cacheService.get('IsReadOnlyScreens')) ? true : false;
        };

        var onPrintReportReceived = function (resp) {
            reportModel = resp;
            reportModel.HasLoaded = false;
            reportModel.ReportHeader = 'Adult Screening';
            reportModel.ReportName = 'CrisisAdultScreening';

            var deferred = $q.defer();
            var promises = [];

            promises.push(registrationService.get($stateParams.ContactID));
            promises.push(callerInformationService.get($stateParams.CallCenterHeaderID));
            promises.push(contactBenefitService.getMedicaidNumber($stateParams.ContactID));
            promises.push(additionalDemographyService.getAdditionalDemographic($stateParams.ContactID));
            promises.push(callerInformationService.get($stateParams.CallCenterHeaderID));
            promises.push(contactRaceService.get($stateParams.ContactID));
            promises.push(serviceRecordingService.getServiceRecording($stateParams.CallCenterHeaderID, SERVICE_RECORDING_SOURCE.CallCenter));
            promises.push(WorkflowHeaderService.GetWorkflowHeader($state.current.data.workflowDataKey, $stateParams.CallCenterHeaderID));
            $q.all(promises).then(function (data) {

                if (hasData(data[7])) {
                    contactData = data[7].DataItems[0];
                    callCenterAssessmentPrintService.getCallCenterPrintHeaderDetails(reportModel, contactData);
                }

                 if (hasData(data[6])) {
                        var serviceRecording = data[6].DataItems[0];
                        if (serviceRecording.OrganizationID) {
                            reportModel.contactProgramUnit = lookupService.getText('ProgramUnit', serviceRecording.OrganizationID)
                        }
                    }

                reportModel.screener = lookupService.getText('Users', data[1].DataItems[0].ProviderID);
                reportModel.screeningDate = ($filter('toMMDDYYYYDate')(new Date(data[1].DataItems[0].CallStartTime), 'MM/DD/YYYY', 'useLocal')).toString();
                reportModel.screeningTime = ($filter('toMMDDYYYYDate')(data[1].DataItems[0].CallStartTime, 'hh:mm A', 'useLocal')).toString();
                reportModel.signatureDate = ($filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal')).toString();

                //if (hasData(data[4])) {
                //    reportModel.incidentID = data[4].DataItems[0].IncidentID ? data[4].DataItems[0].IncidentID : '';
                //}
                //var infoPromise = [];
                //infoPromise.push(contactAddressService.getReportModel($stateParams.ContactID, data[0].DataItems[0].ContactTypeID));
                //infoPromise.push(contactPhoneService.get($stateParams.ContactID, data[0].DataItems[0].ContactTypeID));
                //$q.all(infoPromise).then(function (infoData) {
                //    if (!$.isEmptyObject(infoData[0])) {
                //        angular.extend(reportModel, infoData[0]);
                //    }
                //    if (hasData(infoData[1])) {
                //        if (hasData(infoData[1]) && infoData[1].DataItems[0].Number)
                //            var primaryOrLatestData = getPrimaryOrLatestData(infoData[1].DataItems)[0];
                //        reportModel.contactPhone = $filter('toPhone')(primaryOrLatestData.Number);
                //    }
                //        return deferred.resolve(reportModel);
                //});

            }).finally(function () {
                deferred.resolve(reportModel);
            });

            return deferred.promise;
        }

        $scope.init();
    }]);
}());
