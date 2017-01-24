(function () {
    angular.module('xenatixApp')
    .controller('bapnDetailsController', ['$rootScope', '$q', '$timeout', '$scope', '$stateParams', 'alertService', '$state', '$filter', 'navigationService', '$controller', 'benefitsAssistanceProgressNoteService', 'formService', 'serviceRecordingService', 'assessmentPrintService', 'registrationService', 'contactBenefitService', 'recordingServicePrintService', 'cacheService', 'lookupService', 'WorkflowHeaderService',
    function ($rootScope, $q, $timeout, $scope, $stateParams, alertService, $state, $filter, navigationService, $controller, benefitsAssistanceProgressNoteService, formService, serviceRecordingService, assessmentPrintService, registrationService, contactBenefitService, printService, cacheService, lookupService, WorkflowHeaderService) {

        //Sections for BAPN
        //60	Benefits Screening
        //61	Fee Assessment & Benefits Research
        //62	SSI & SSDI
        //63	Medicare
        //64	Traditional Medicaid
        //65	SNAP, CHIP, JPS, & TANF
        //66	Other
        //67	Signature
        var ServiceRecordingSourceID = SERVICE_RECORDING_SOURCE.BAPN;
        var addendumSectionID = 80;
        $controller('recordingServiceController', { $scope: $scope, ServiceRecordingSourceID: ServiceRecordingSourceID, FormName: CREDENTIAL_ACTION_FORM.BAPNServices });

        if ($stateParams.ReadOnly == 'view' && $stateParams.SectionID == 80)
            $scope.enableSave = true;

        $scope.permissionKey = $state.current.data.permissionKey;
        var isOtherUser = false;
        navigationService.get().then(function (response) {
            if (hasData(response)) {
                $scope.STAFF_NAME = response.DataItems[0].UserFullName;
                $scope.STAFF_ID = response.DataItems[0].UserID;
                $scope.CredentialList = $filter('orderBy')(response.DataItems[0].UserCredentials, 'CredentialName');
                if ($scope.CredentialList && $scope.CredentialList.length == 1) {
                    $scope.CREDENTIAL_ID = $scope.CredentialList[0].CredentialID;
                }
            }
        });
        var resetForm = function () {
            $rootScope.formReset($scope.ctrl.serviceRecordingForm);
        };
        $scope.prepopulatedData = function () {
            var dfd = $q.defer();
            $scope.isReadOnlyParam = ($stateParams.ReadOnly && $stateParams.ReadOnly == 'view') ? true : false;
            $scope.GET_DATE = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal');
            serviceRecordingService.getServiceRecording($stateParams.BenefitsAssistanceID, SERVICE_RECORDING_SOURCE.BAPN).then(function (data) {
                if (hasData(data)) {
                    $scope.SERVICE_START_DATE = data.DataItems[0].ServiceStartDate ? $filter('toMMDDYYYYDate')(data.DataItems[0].ServiceStartDate, 'MM/DD/YYYY') : '';
                    isOtherUser = data.DataItems[0].UserID != $scope.STAFF_ID;
                    if (isOtherUser && !($stateParams.SectionID == addendumSectionID)) {
                        $scope.STAFF_NAME = '';
                        $scope.GET_DATE = '';
                    }
                }
                dfd.resolve(data);
            });
            return dfd.promise;
        };

        var moveNext = false;
        var documentStatusID = DOCUMENT_STATUS.Draft;
        var serviceName = "bapnService";

        var getUserApproval = function () {
            var dfd = $q.defer();
            if ($scope.validateSignature()) {
                bootbox.confirm("Are you sure you have completed all areas needed in your workflow?", function (result) {
                    dfd.resolve(result);
                });
            } else {
                dfd.resolve(true);
            }
            return dfd.promise;
        };

        var getBapnModel = function () {
            documentStatusID = (!$stateParams.DocumentStatusID || $stateParams.DocumentStatusID == 0 || $stateParams.DocumentStatusID == 1) ?
                                ($scope.validateSignature() ? DOCUMENT_STATUS.Completed : DOCUMENT_STATUS.Draft) : $stateParams.DocumentStatusID;
            var bapnModel = {
                BenefitsAssistanceID: $stateParams.BenefitsAssistanceID,
                ContactID: $stateParams.ContactID,
                DateEntered: $filter('formatDate')(new Date(), 'MM/DD/YYYY hh:mm A'),
                UserID: $scope.STAFF_ID,
                AssessmentID: ASSESSMENT_TYPE.BenefitAssessmentsProgressNote,
                ResponseID: $stateParams.ResponseID ? $stateParams.ResponseID : null,
                DocumentStatusID: documentStatusID ? documentStatusID : DOCUMENT_STATUS.Draft,
                ModifiedOn: new Date(),
                ServiceStartDate: $scope.serviceRecording.ServiceStartDate,
                ServiceEndDate: $scope.serviceRecording.ServiceEndDate,
                ServiceItemID: $scope.serviceRecording.ServiceItemID,
                TrackingFieldID: $scope.serviceRecording.TrackingFieldID
            };
            return bapnModel;
        };

        $scope.saveRecordedService = function (isNext, mandatory, hasErrors, keepForm, next) {
            //Vaidate the Call Start/End Time
            var dfd = $q.defer();
            if (!$scope.validateCallTime()) {
                alertService.error("Invalid end time!");
                dfd.resolve(null);
                return false;
            }

            return getUserApproval().then(function (resp) {
                if (resp) {
                    var isFormDirty = formService.isDirty();
                    moveNext = isNext;
                    if (isCheckBoxListSelected()) {
                        if (isFormDirty) {
                            var bapnModel = getBapnModel();
                            angular.extend($stateParams, { DocumentStatusID: documentStatusID, ReadOnly: (documentStatusID == DOCUMENT_STATUS.Completed) ? "view" : "edit" });

                            if ($state.current.name.indexOf('patientprofile.benefits.bapn.benefitsAssistanceProgressNote.bapnNavigation.section') == -1) {
                                //No need to save BAPN details if not in service screen
                                //Code to Save bapn details
                                return saveBAPNDetails(bapnModel);
                            }
                            else {
                                return $scope.saveAssessment(isNext, mandatory, hasErrors, keepForm, next).then(function () {
                                    checkFormStatus({ stateName: $state.current.name + $stateParams.SectionID, validationState: VALIDATION_STATE.Valid });

                                    //required call to update responseId, no need to udpate if responseId is available
                                    if (!$stateParams.ResponseID || $stateParams.ResponseID === 0) {
                                        angular.extend($stateParams, { ResponseID: $scope.responseId });

                                        angular.extend(bapnModel, {
                                            ResponseID: $scope.responseId ? $scope.responseId : null, DocumentStatusID: $stateParams.DocumentStatusID
                                        });
                                        return saveBAPNDetails(bapnModel);
                                    }
                                    else {
                                        $scope.signatureFormReset();
                                        //TODO: update section name
                                        alertService.success('Benefits Assistance Progress Note updated successfully.');
                                        WorkflowHeaderService.AddWorkflowHeader({ WorkflowDataKey: $state.current.data.workflowDataKey, RecordHeaderID: $stateParams.BenefitsAssistanceID, ContactID: $stateParams.ContactID });
                                        resetForm();
                                        if (isNext)
                                            nextSection($stateParams);
                                        dfd.resolve(1);
                                    }
                                });
                            }
                        }
                        else if (isNext) {
                            nextSection($stateParams);
                            dfd.resolve(1);
                        }
                    }
                    else {
                        dfd.resolve(1);
                    }
                }
            });
            return dfd.promise;
        };

        var nextSection = function (stateParams) {
            var nextState = angular.element("xen-workflow-action[data-state-name] > li.list-group-item.active").parents('.work-flow').next().find("xen-workflow-action[data-state-name]");
            if (hasDetails(nextState)) {
                // for xen-workflow
                stateParams = angular.extend($stateParams, stateParams);

                if (nextState.length === 1) {
                    var sectionID = Math.abs(nextState.attr('data-state-key'));

                    if (sectionID)
                        stateParams.SectionID = sectionID;

                    nextState = nextState.attr('data-state-name');
                }
                else
                    nextState = $scope.returnState;

                $scope.Goto(nextState, stateParams);
            }
            else {
                var nextOnWorkflowClick = angular.element("xen-workflow-action[data-state-name] > li.list-group-item.active").parents('.work-flow').next().find("xen-workflow-action[on-workflow-click]");
                $timeout(function () {
                    nextOnWorkflowClick.find("a").trigger('click');
                });
            }

        };

        var saveBAPNDetails = function (bapnModel) {
            if ($stateParams.BenefitsAssistanceID && $stateParams.BenefitsAssistanceID > 0) {
                return benefitsAssistanceProgressNoteService.update(bapnModel).then(saveBAPNSuccess, saveFailure);
            }
            else {
                return benefitsAssistanceProgressNoteService.add(bapnModel).then(saveBAPNSuccess, saveFailure);
            }
        };

        var saveBAPNSuccess = function (response) {
            if (response.data.ResultCode !== 0) {
                alertService.error(response.ResultMessage);
            }
            else {

                if (!$stateParams.BenefitsAssistanceID || $stateParams.BenefitsAssistanceID == 0)
                    angular.extend($stateParams, { BenefitsAssistanceID: response.data.ID });
                //save workflow Header details.
                //$rootScope.$broadcast('onWorkflowHeaderSave', { workflowDataKey: $state.current.data.workflowDataKey, workflowHeaderID: $stateParams.BenefitsAssistanceID });
                WorkflowHeaderService.AddWorkflowHeader({ WorkflowDataKey: $state.current.data.workflowDataKey, RecordHeaderID: $stateParams.BenefitsAssistanceID, ContactID: $stateParams.ContactID });
                if ($state.current.name.indexOf('patientprofile.benefits.bapn.benefitsAssistanceProgressNote.bapnNavigation.section') == -1) {
                    return $scope.saveRecordingService($stateParams.BenefitsAssistanceID).then(function (response) {
                        if (response.ResultCode !== 0) {
                            alertService.error(response.ResultMessage);
                        }
                        else {
                            var msg = (response.ID) ? "saved" : "updated";
                            $scope.signatureFormReset();
                            alertService.success('Benefits Assistance Progress Note ' + msg + ' successfully.');
                            resetForm();

                            angular.extend($stateParams, {
                                SectionID: $stateParams.SectionID ? $stateParams.SectionID : 0,
                                ReadOnly: ($stateParams.DocumentStatusID == DOCUMENT_STATUS.Completed) ? "view" : "edit"
                            });

                            $state.transitionTo(serviceName, $stateParams, { notify: true });

                            if (this.documentStatusID == DOCUMENT_STATUS.Completed) {
                                $scope.noAccess = true;
                                $rootScope.$broadcast('getComplete');
                                checkFormStatus({ stateName: serviceName, validationState: VALIDATION_STATE.Valid });
                            }
                            else {
                                checkFormStatus({ stateName: serviceName, validationState: VALIDATION_STATE.Invalid });
                            }
                        }

                        if (moveNext)
                            nextSection($stateParams);
                    }, saveFailure);
                }
                else {
                    $scope.signatureFormReset();

                    var msg = (response.ID) ? "saved" : "updated";

                    alertService.success('Benefits Assistance Progress Note ' + msg + ' successfully.');

                    resetForm();

                    $state.transitionTo($state.current.name, $stateParams, { notify: true });

                    if (documentStatusID == DOCUMENT_STATUS.Completed) {
                        $rootScope.$broadcast('getComplete');
                    }

                    if (moveNext)
                        nextSection($stateParams);
                }
            }
        };

        var saveFailure = function (response) {
            if (response.ResultCode !== 15)
                alertService.error('OOPS! Something went wrong.');

            resetForm();
        };

        var isCheckBoxListSelected = function () {
            var val = false;
            var elements = angular.element($("[multi-Checkbox]"));
            var messageText;
            if (elements.length == 0) {
                val = true;
            } else {
                angular.forEach(elements, function (item) {
                    messageText = item.attributes["data-validation-name"].value;
                    if (!messageText) {
                        messageText = item.attributes["validation-name"].value;
                    }
                    if (item.attributes["disabled"] || item.attributes["class"].value.indexOf("selected") > -1) {
                        //If item is disaled or is checked
                        val = true;
                    }
                });
            }
            if (!val) {
                if (!messageText) {
                    messageText = 'Select';
                }
                alertService.error('Please fill out the ' + messageText + ' field.');
            }

            return val;
        };

        $scope.initNoteReport = function (response, wasDirty) {
            var responseID = $scope.responseId ? $scope.responseId : $stateParams.ResponseID;
            if (!$stateParams.ResponseID || $stateParams.ResponseID === 0) {
                angular.extend($stateParams, { ResponseID: $scope.responseId });
                $state.transitionTo($state.current.name, $stateParams, { notify: true });
            }
            if (wasDirty) {
                var bapnModel = getBapnModel();
                return saveBAPNDetails(bapnModel).then(function () {
                    return benefitsAssistanceProgressNoteService.initReport(ASSESSMENT_TYPE.BenefitAssessmentsProgressNote, responseID, $stateParams.SectionID, $stateParams.ContactID, $scope.SERVICE_START_DATE).then(function (reportModel) {
                        showReport(reportModel);
                        return reportModel;
                    });
                });
            } else {
                return benefitsAssistanceProgressNoteService.initReport(ASSESSMENT_TYPE.BenefitAssessmentsProgressNote, responseID, $stateParams.SectionID, $stateParams.ContactID, $scope.SERVICE_START_DATE).then(function (reportModel) {
                    showReport(reportModel);
                    return reportModel;
                });;
            }
        }

        var showReport = function (reportModel) {
            WorkflowHeaderService.GetWorkflowHeader($state.current.data.workflowDataKey,  $stateParams.BenefitsAssistanceID).then(function (headerDetails) {
                if (headerDetails) {
                    reportModel.mrn = headerDetails.MRN;
                    var suffix = lookupService.getText("Suffix", headerDetails.SuffixID);
                    reportModel.clientName = headerDetails.FirstName + (headerDetails.Middle ? ' ' + headerDetails.Middle : '') + ' ' + headerDetails.LastName + (suffix ? ' ' + suffix : '');;
                    reportModel.dob = ($filter('formatDate')(headerDetails.DOB, 'MM/DD/YYYY')).toString();
                    reportModel.medicaidNumber = headerDetails.MedicaidID || 'N/A';
                }
            });
            reportModel.HasLoaded = true;
            reportModel.alreadyExecuted = true;
            $rootScope.reportModel = reportModel;
            $('#reportModal').modal('show');
        }

        //var onPrintReportReceived = function (data) {
        //    $scope.reportModel = data;
        //    $scope.reportModel.medicaidNumber = 'N/A';
        //    $scope.reportModel[1800] = $scope.SERVICE_START_DATE;
        //    var dfd = $q.defer();
        //    var servicesPromises = [];
        //    servicesPromises.push(getRegitrationData($stateParams.ContactID));
        //    servicesPromises.push(getContactBenefitData($stateParams.ContactID));
        //    $q.all(servicesPromises).then(function () {
        //        $rootScope.reportModel = $scope.reportModel;
        //        $rootScope.reportModel.HasLoaded = true;
        //        $rootScope.reportModel.alreadyExecuted = true;
        //        $('#reportModal').modal('show');
        //        dfd.resolve($scope.reportModel);
        //    }, function (errorMessage) {
        //        alertService.error(errorMessage);
        //    });
        //    return dfd.promise;
        //}

        //var getRegitrationData = function (contactID) {
        //    var dfd = $q.defer();
        //    registrationService.get(contactID).then(function (data) {
        //        if (hasData(data)) {
        //            var registrationData = data.DataItems[0];
        //            $scope.reportModel.mrn = registrationData.MRN ? registrationData.MRN.toString() : '';
        //            $scope.reportModel.clientName = registrationData.FirstName + ' ' + registrationData.LastName;
        //            $scope.reportModel.dob = registrationData.DOB ? $filter('toMMDDYYYYDate')(registrationData.DOB, 'MM/DD/YYYY') : '';
        //            dfd.resolve(data);
        //        }
        //        else {
        //            dfd.resolve(null);
        //        }
        //    });
        //    return dfd.promise;
        //}

        //var getContactBenefitData = function (contactID) {
        //    var dfd = $q.defer();
        //    contactBenefitService.get($scope.contactID).then(function (data) {
        //        if (hasData(data)) {
        //            var payors = $filter('filter')(data.DataItems, function (itm) {
        //                return itm.PayorName.toString().toLowerCase().indexOf('medicaid') > -1;
        //            });
        //            if (hasData(payors)) {
        //                $scope.reportModel.medicaidNumber = payors[0].PolicyID;
        //            }
        //            dfd.resolve(data);
        //        }
        //        else {
        //            dfd.resolve(null);
        //        }
        //    });
        //    return dfd.promise;
        //}

        $scope.disableBAPN = function () {
            if ($stateParams.SectionID != addendumSectionID || cacheService.get('IsVoidedRecord')) {
                if (isOtherUser || cacheService.get('IsVoidedRecord') || $stateParams.DocumentStatusID == DOCUMENT_STATUS.Completed) {
                    $scope.noAccessToOther = true;
                    $scope.disableAssessmentSave = true;
                }
            }
            else {
                $scope.enableSave = true;
                $scope.disableAssessmentSave = false;
            };
            // Disable Note screen if user signed it 
            if (!$scope.noAccessToOther && $scope.responses && $scope.responses[1961] && $scope.responses[1961][3701]) {
                $scope.noAccessToOther = true;
            }

            //Will fetch the date from Service Screen
            if ($scope.responses && $scope.responses[1800] && $scope.responses[1800][17] && $scope.SERVICE_START_DATE) {
                $scope.responses[1800][17] = $scope.SERVICE_START_DATE;
            }

        }

        $scope.printReport = function (isNext, mandatory, hasErrors, keepForm, next) {
            $scope.saveRecordedService(isNext, mandatory, hasErrors, keepForm, next).then(function (response) {
                printService.initPrint($stateParams.BenefitsAssistanceID, ServiceRecordingSourceID, $scope.serviceRecording.ServiceRecordingID, $state.current.data.workflowDataKey).then(function (data) {
                    $rootScope.reportModel = data;
                    $rootScope.formReset($scope.ctrl.serviceRecordingForm);
                    $rootScope.reportModel.HasLoaded = true;
                    $('#reportModal').modal('show');
                });
            });
        };

        $scope.$on('$destroy', function () {
            $rootScope.reportModel = null;
        });

        var checkFormStatus = function (state) {
            $rootScope.$broadcast('rightNavigationBAPNHandler', state);
        }
    }])
}());
