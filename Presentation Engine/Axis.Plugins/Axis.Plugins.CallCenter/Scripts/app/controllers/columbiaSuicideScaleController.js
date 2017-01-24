(function () {
    angular.module('xenatixApp')
    .controller('columbiaSuicideScaleController', ['$scope', '$filter', '$q', 'alertService', '$stateParams', '$state', '$rootScope', 'formService', 'assessmentService', 'registrationService', 'lookupService', 'callerInformationService', 'sectionID', 'responseID', 'credentialSecurityService', 'contactBenefitService', 'additionalDemographyService', 'contactAddressService', 'contactRaceService', 'contactPhoneService', 'contactSSNService', 'navigationService', 'assessmentPrintService', 'cacheService', 'serviceRecordingService', 'WorkflowHeaderService', 'callCenterAssessmentPrintService',
        function ($scope, $filter, $q, alertService, $stateParams, $state, $rootScope, formService, assessmentService, registrationService, lookupService, callerInformationService, sectionID, responseID, credentialSecurityService, contactBenefitService, additionalDemographyService, contactAddressService, contactRaceService, contactPhoneService, contactSSNService, navigationService, printService, cacheService, serviceRecordingService, WorkflowHeaderService, callCenterAssessmentPrintService) {
            $scope.ContactID = $stateParams.ContactID;            
            $scope.AssessmentID = ASSESSMENT_TYPE.ColumbiaSuicideSeverityRatingScale;
            var reportModel = null;
            var adjustedTime = '12:00:01';
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
                angular.extend($stateParams, { SectionID: sectionID, ResponseID: responseID });
                $state.transitionTo($state.current.name, $stateParams);
            };

            $scope.prepopulatedData = function () {
                var dfd = $q.defer();
                $scope.GET_DATE = $filter('toMMDDYYYYDate') (new Date, 'MM/DD/YYYY', 'useLocal');
                    navigationService.get().then(function (response) {
                    if (hasData(response)) {
                        $scope.STAFF_NAME = response.DataItems[0].UserFullName;
                        credentialSecurityService.getUserCredentialSecurity().then(function (credData) {
                            if (hasData(credData)) {
                                $scope.CredentialList = $filter('filter')(credData.DataItems, { CredentialActionForm: "Columbia Suicide Assessment", CredentialAction: "Digital Signature" }, true);
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

            $scope.postAssessmentReponseDetails = function () {
                $scope.isDisabled = (cacheService.get('IsReadOnlyScreens')) ? true : false;
            };

            $scope.initReport = function (response) {
                if (response != "-1") {
                    alertService.success('Assessment Response saved successfully.');
                    //save workflow Header details.
                    //$rootScope.$broadcast('onWorkflowHeaderSave', { workflowDataKey: $state.current.data.workflowDataKey, workflowHeaderID: $stateParams.CallCenterHeaderID });
                    WorkflowHeaderService.AddWorkflowHeader({ WorkflowDataKey: $state.current.data.workflowDataKey, RecordHeaderID: $stateParams.CallCenterHeaderID, ContactID: $stateParams.ContactID });
                }
                return printService.initReports($scope.AssessmentID, responseID, sectionID).then(onPrintReportReceived.bind(this));
            }

            var onPrintReportReceived = function (resp) {
                var deferred = $q.defer();
                reportModel = resp;
                reportModel.HasLoaded = false;
                reportModel.ReportHeader = 'Columbia Suicide Scale';
                reportModel.ReportName = 'Columbia-SuicideSeverityRatingScale';
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

                    reportModel.screener = lookupService.getText('Users', data[1].DataItems[0].ProviderID);
                    reportModel.screeningDate = ($filter('toMMDDYYYYDate')(new Date(data[1].DataItems[0].CallStartTime), 'MM/DD/YYYY', 'useLocal')).toString();
                    reportModel.screeningTime = ($filter('toMMDDYYYYDate')(data[1].DataItems[0].CallStartTime, 'hh:mm A', 'useLocal')).toString();
                    reportModel.signatureDate = ($filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal')).toString();

                    if (hasData(data[6])) {
                        var serviceRecording = data[6].DataItems[0];
                        if (serviceRecording.OrganizationID) {
                            reportModel.contactProgramUnit = lookupService.getText('ProgramUnit', serviceRecording.OrganizationID)
                        }
                    }
                    callerInformationService.getCallCenterAssessmentResponse($stateParams.CallCenterHeaderID, ASSESSMENT_TYPE.CrisisAssessment).then(function (response) {
                        if (hasData(response)) {
                            assessmentService.getAssessmentResponseDetails(response.DataItems[0].ResponseID, ASSESSMENT_SECTION.RiskFactors).then(function (responseDetails) {
                                var rData = responseDetails.data;
                                if (hasData(rData)) {
                                    $filter('filter')(rData.DataItems, function (question) { if (question.QuestionID == '3530') reportModel.sentToMCOTDate = question.ResponseText ? ($filter('toMMDDYYYYDate')(question.ResponseText, 'MM/DD/YYYY')).toString() : ($filter('toMMDDYYYYDate')(data[6].DataItems[0].ServiceEndDate, 'MM/DD/YYYY')).toString() });
                                    $filter('filter')(rData.DataItems, function (question) {
                                        if (question.QuestionID == '3531')
                                            reportModel.sentToMCOTTime = searchString(question.ResponseText, adjustedTime) ? "" : ($filter('toMMDDYYYYDate')(question.ResponseText, 'hh:mm A', 'useLocal')).toString();
                                    });
                                }
                            });
                        }
                    });
                   

                }).finally(function () {
                    deferred.resolve(reportModel);
                });
                return deferred.promise;

            }
            $scope.init();
        }]);
}());
