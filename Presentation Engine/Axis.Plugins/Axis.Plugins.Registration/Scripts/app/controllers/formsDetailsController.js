(function () {
    angular.module('xenatixApp')
    .controller('formsDetailsController', ['$scope', '$rootScope', '$q', '$stateParams', '$state', '$filter', 'formService', 'alertService', 'navigationService', 'intakeFormsService', 'registrationService', 'contactBenefitService', 'lookupService', 'contactAddressService', '$controller', 'serviceRecordingService', '$timeout', 'assessmentPrintService', 'assessmentService', 'contactPhoneService', 'contactSSNService', 'recordingServicePrintService', 'cacheService','WorkflowHeaderService',
        function ($scope, $rootScope, $q, $stateParams, $state, $filter, formService, alertService, navigationService, intakeFormsService, registrationService, contactBenefitService, lookupService, contactAddressService, $controller, serviceRecordingService, $timeout, assessmentPrintService, assessmentService, contactPhoneService, contactSSNService, printService, cacheService, WorkflowHeaderService) {
            var Staff_Id;
            $controller('recordingServiceController', { $scope: $scope, ServiceRecordingSourceID: SERVICE_RECORDING_SOURCE.IDDForms, FormName: CREDENTIAL_ACTION_FORM.IDDFormServices });
            $scope.GET_DATE = $filter('toMMDDYYYYDate')(new Date, 'MM/DD/YYYY', 'useLocal');
            $scope.AGENCY_ADDRESS = AGENCY_Data.ADDRESS;
            $scope.AGENCY_CITY = AGENCY_Data.CITY;
            $scope.AGENCY_NAME = AGENCY_Data.NAME;
            $scope.AGENCY_STATE = AGENCY_Data.STATE;
            $scope.AGENCY_ZIP = AGENCY_Data.ZIP;
            $scope.MEDICAID_NUMBER = 'N/A';
            $scope.isInactive = false;
            var addendumSectionID = 82;
            if ($stateParams.ReadOnly == 'view' && $stateParams.SectionID == BAPN_ASSESSMENT_SECTION.Addedndum)
                $scope.enableSave = true;
            var serviceName = "formservice";
            var SSN;
            var formSectionURL = 'patientprofile.intake.formnavi.forms.formsnavi.formsSection';
            var moveNext = false;
            var documentStatusID = DOCUMENT_STATUS.Draft;
            var isOtherUser = false;
            var getNavigationDefaultData = function () {
                var dfd = $q.defer();
                navigationService.get().then(function (response) {
                    if (hasData(response)) {
                        $scope.CredentialList = $filter('orderBy')(response.DataItems[0].UserCredentials, 'CredentialName');
                        if ($scope.CredentialList && $scope.CredentialList.length == 1) {
                            $scope.CREDENTIAL_ID = $scope.CredentialList[0].CredentialID;
                        }
                        $scope.STAFF_NAME = response.DataItems[0].UserFullName;
                        Staff_Id = response.DataItems[0].UserID;
                        serviceRecordingService.isOtherUser($stateParams.ContactFormsID, SERVICE_RECORDING_SOURCE.IDDForms, Staff_Id).then(function (otherUser) {
                            if (otherUser && !($stateParams.SectionID == addendumSectionID)) {
                                $scope.STAFF_NAME = '';
                                $scope.GET_DATE = '';
                            }
                            isOtherUser = otherUser;
                            dfd.resolve(response);
                        });

                    }
                    else {
                        dfd.resolve(null);
                    }
                });
                return dfd.promise;
            };


            var getContactBenefitDefaultData = function (contactID) {
                var dfd = $q.defer();
                contactBenefitService.get(contactID).then(function (cbdata) {
                    if (hasData(cbdata)) {
                        var payors = $filter('filter')(cbdata.DataItems, function (itm) {
                            return itm.PayorName.toString().toLowerCase().indexOf('medicaid') > -1;
                        });
                        if (hasData(payors)) {
                            $scope.MEDICAID_NUMBER = payors[0].PolicyID;
                            dfd.resolve(cbdata);
                        }
                        else {
                            dfd.resolve(cbdata);
                        }
                    }
                    else {
                        dfd.resolve(null);
                    }
                });
                return dfd.promise;
            };

            var getRegistrationDefaultData = function (contactID) {
                var dfd = $q.defer();
                registrationService.get(contactID).then(function (regdata) {
                    if (hasData(regdata)) {
                        var contactDetails = regdata.DataItems[0];
                        $scope.MRN = (contactDetails.MRN) ? contactDetails.MRN : '';
                        $scope.CLIENT_FULLNAME = contactDetails.FirstName + ' ' + contactDetails.LastName;
                        $scope.CLIENT_NAME = contactDetails.FirstName + ' ' + contactDetails.LastName;
                        $scope.DOB = (contactDetails.DOB) ? $filter('toMMDDYYYYDate')(contactDetails.DOB, 'MM/DD/YYYY') : '';
                        var promises = [];
                        if (contactDetails.SSN && contactDetails.SSN.length > 0 && contactDetails.SSN.length < 9) {
                            promises.push(getContactSSN(contactID));
                        }
                        promises.push(getContactAddress(contactID, contactDetails.ContactTypeID));
                        promises.push(getContactPhone(contactID, contactDetails.ContactTypeID));
                        $q.all(promises).then(function (servicesData) {
                            dfd.resolve(regdata)
                        }, function (errorMessage) {
                            alertService.error(errorMessage);
                        });
                    }
                    else {
                        dfd.resolve(null);
                    }
                });
                return dfd.promise;
            };

            var getContactSSN = function (contactID) {
                var contactSSNDeferred = $q.defer();
                contactSSNService.get(contactID).then(function (ssnData) {
                    if (hasData(ssnData)) {
                        SSN = ssnData.DataItems[0];
                    }
                    contactSSNDeferred.resolve(ssnData)
                })
                return contactSSNDeferred.promise;
            };

            var getContactAddress = function (contactID, contactTypeID) {
                var contactAddressDeferred = $q.defer();
                contactAddressService.get(contactID, contactTypeID).then(function (addressdata) {
                    if (hasData(addressdata)) {
                        var contactAddresses = $filter('filter')(addressdata.DataItems, { IsPrimary: true }, true);
                        if (contactAddresses && contactAddresses.length == 0) {
                            contactAddresses = $filter('orderBy')(addressdata.DataItems, function (item) {
                                return new Date(item.ModifiedOn);
                            }, true);
                        }
                        var contactAddress = contactAddresses[0];
                        var line1 = contactAddress.Line1 ? contactAddress.Line1 : '';
                        var line2 = contactAddress.Line2 ? contactAddress.Line2 : '';
                        $scope.CLIENT_ADDRESS = line1 + (line1 ? ' ' : '') + line2;
                        var stateName = lookupService.getText('StateProvince', contactAddress.StateProvince);
                        var stateProvince = ((contactAddress.City && contactAddress.StateProvince) ? ', ' : '') + stateName;
                        var zipCode = (((contactAddress.StateProvince || contactAddress.City) && contactAddress.Zip) ? ', ' : '') + contactAddress.Zip
                        $scope.CLIENT_CITYSTATEZIP = (contactAddress.City ? contactAddress.City : '') + (stateName ? stateProvince : '') + (zipCode ? zipCode : '');
                        contactAddressDeferred.resolve(addressdata);
                    }
                    else {
                        contactAddressDeferred.resolve(null);
                    }
                });
                return contactAddressDeferred.promise;
            };

            var getContactPhone = function (contactID, contactTypeID) {
                var contactPhoneDeferred = $q.defer();
                contactPhoneService.get(contactID, contactTypeID).then(function (phonedata) {
                    if (hasData(phonedata)) {
                        var contactphones = $filter('filter')(phonedata.DataItems, { IsPrimary: true }, true);
                        if (contactphones && contactphones.length == 0) {
                            contactphones = $filter('orderBy')(phonedata.DataItems, function (item) {
                                return new Date(item.ModifiedOn);
                            }, true);
                        }
                        var contactphone = contactphones[0];
                        $scope.CLIENT_PHONE = contactphone.Number ? contactphone.Number : '';
                        contactPhoneDeferred.resolve(phonedata);
                    }
                    else {
                        contactPhoneDeferred.resolve(null);
                    }
                });
                return contactPhoneDeferred.promise;
            };

            var getNoteStartDate = function (contactFormsID) {
                var dfd = $q.defer();
                serviceRecordingService.getServiceRecording(contactFormsID, SERVICE_RECORDING_SOURCE.IDDForms).then(function (data) {
                    if (hasData(data)) {
                        $scope.SERVICE_START_DATE = data.DataItems[0].ServiceStartDate ? $filter('toMMDDYYYYDate')(data.DataItems[0].ServiceStartDate, 'MM/DD/YYYY') : '';
                    }
                    dfd.resolve(data);
                });
                return dfd.promise;
            };

            var prepopulatedDataFromServices = function () {
                var dfd = $q.defer();
                var servicesPromises = [];
                servicesPromises.push(getNavigationDefaultData());
                servicesPromises.push(getRegistrationDefaultData($stateParams.ContactID));
                servicesPromises.push(getContactBenefitDefaultData($stateParams.ContactID));
                servicesPromises.push(getNoteStartDate($stateParams.ContactFormsID, SERVICE_RECORDING_SOURCE.IDDForms));
                $q.all(servicesPromises).then(function (servicesData) {
                    dfd.resolve(servicesData);
                    disabledSignedForms();
                }, function (errorMessage) {
                    alertService.error(errorMessage);
                });
                return dfd.promise;
            };


            var disabledSignedForms = function () {
                assessmentService.getAssessmentResponseDetails($stateParams.ResponseID, $stateParams.SectionID).then(function (data) {
                    if (hasData(data.data)) {
                        var checkSignatures = $filter('filter')(data.data.DataItems, { OptionsID: 3701 }, true);
                        // disbale screen only when both signature are provided.
                        if (checkSignatures && checkSignatures.length > 0) {
                            $scope.disableAssessmentSave = $scope.noAccessToOther = (checkSignatures[0].SignatureBLOB) ? true : false;
                        }
                    }
                });
            }

            $scope.prepopulatedData = function () {
                return prepopulatedDataFromServices();
            };

            var hasSectionsData = function () {
                var dfd = $q.defer();
                assessmentService.getAssessmentSections($stateParams.AssessmentID).then(function (data) {
                    if (hasData(data)) {
                        var sectionID;
                        var promiseArr = [];
                        angular.forEach(data.DataItems, function (item) {
                            sectionID = item.AssessmentSectionID;
                            if (sectionID !== BAPN_ASSESSMENT_SECTION.Addedndum) {
                                promiseArr.push(assessmentService.getAssessmentResponseDetails($stateParams.ResponseID, sectionID));
                            }
                        });
                        $q.all(promiseArr).then(function (responses) {
                            var hasSigned = false;
                            if (responses.length > 0) {
                                for (var i = 0; i < responses.length; i++) {
                                    var retObj = $filter('filter')(responses[i].data.DataItems, function (item) { return (item.OptionsID == 3701 || item.OptionsID == 3702 || item.OptionsID == 3703) }, true);
                                    if (retObj.length > 0) {
                                        hasSigned = true;
                                        break;
                                    }
                                }
                                return dfd.resolve(hasSigned);
                            }
                            else {
                                return dfd.resolve(false);
                            }
                        });
                    }
                });
                return dfd.promise;
            }

            var resetForm = function () {
                $rootScope.formReset($scope.ctrl.serviceRecordingForm);
                $scope.formDetailsChanged = false;
            };
            var initTopazModel = function () {
                $scope.topazModel.b64ImageData = '';
                $scope.topazModel.hideSignatureBtns = false;
            };
            var getUserApproval = function () {
                var dfd = $q.defer();
                if ($scope.validateSignature()) {
                    bootbox.confirm("Are you sure you have completed all areas needed in your workflow?", function (result) {
                        if (result) {
                            if ($stateParams.ResponseID && $stateParams.ResponseID !== 0) {
                                //Check if any of the section has data
                                hasSectionsData().then(function (hasDetails) {
                                    if (hasDetails) {
                                        dfd.resolve(hasDetails);
                                    }
                                    else {
                                        $scope.topazModel.Clear();
                                        initTopazModel();
                                        alertService.error('Signature cannot be accepted without at least one note/form completed.');
                                        dfd.resolve(false);
                                    }
                                })
                            } else {
                                $scope.topazModel.Clear();
                                initTopazModel();
                                alertService.error('Signature cannot be accepted without at least one note/form completed.');
                                dfd.resolve(false);
                            }
                        }

                    });
                } else {
                    dfd.resolve(true);
                }
                return dfd.promise;
            };

            var getLetterModel = function () {
                var documentStatusID = (!$stateParams.DocumentStatusID || $stateParams.DocumentStatusID == 0 || $stateParams.DocumentStatusID == 1) ? ($scope.validateSignature() ? DOCUMENT_STATUS.Completed : DOCUMENT_STATUS.Draft) : $stateParams.DocumentStatusID;
                var lettersModel = {
                    ContactFormsID: $stateParams.ContactFormsID,
                    ContactID: $stateParams.ContactID,
                    AssessmentID: $stateParams.AssessmentID,
                    ResponseID: $stateParams.ResponseID ? $stateParams.ResponseID : null,
                    DocumentStatusID: (documentStatusID ? documentStatusID : DOCUMENT_STATUS.Draft),
                    UserID: Staff_Id,
                    ModifiedOn: $scope.ModifiedOn
                };
                angular.extend($stateParams, { DocumentStatusID: documentStatusID, ReadOnly: (documentStatusID == DOCUMENT_STATUS.Completed) ? "view" : "edit" });
                return lettersModel;
            }

            $scope.saveRecordedService = function (isNext, mandatory, hasErrors, keepForm, next) {
                var dfd = $q.defer();
                //Vaidate the Call Start/End Time
                if (!$scope.validateCallTime()) {
                    alertService.error("Invalid end time!");
                    dfd.resolve(null);
                    return false;
                }

                return getUserApproval().then(function (resp) {
                    if (resp) {
                        var isFormDirty = formService.isDirty();
                        moveNext = isNext;
                        if (isFormDirty) {
                            var lettersModel = getLetterModel();
                            if ($state.current.name.indexOf(formSectionURL) == -1) {
                                //No need to save FORM details if not in service screen
                                //Code to Save Form details
                                return saveFormDetails(lettersModel);
                            } else {
                                $scope.saveAssessment(isNext, mandatory, hasErrors, keepForm, next).then(function () {
                                    //required call to update responseId, no need to udpate if responseId is available
                                    checkFormStatus({ stateName: $state.current.name + $stateParams.SectionID, validationState: VALIDATION_STATE.Valid });
                                    if (!$stateParams.ResponseID || $stateParams.ResponseID === 0) {
                                        angular.extend($stateParams, { ResponseID: $scope.responseId });
                                        angular.extend(lettersModel, { ResponseID: $scope.responseId ? $scope.responseId : null, DocumentStatusID: $stateParams.DocumentStatusID });
                                        return saveFormDetails(lettersModel);
                                    }
                                    else {
                                        alertService.success('Forms updated successfully.');
                                        resetForm();
                                        if (isNext) {
                                            nextSection($stateParams);
                                        }
                                        dfd.resolve(1);
                                    }
                                });
                            }

                        }
                        else if (isNext) {
                            nextSection($stateParams);
                            dfd.resolve(1);
                        } else {
                            resetForm();
                            dfd.resolve(1);
                        }

                    }
                });

                return dfd.promise;
            };

            var saveFormDetails = function (lettersModel) {
                if ($stateParams.ContactFormsID && $stateParams.ContactFormsID > 0) {
                    return intakeFormsService.update(lettersModel).then(saveFormSuccess, saveFailure);
                }
                else {
                    return intakeFormsService.add(lettersModel).then(saveFormSuccess, saveFailure);
                }
            }


            var saveFormSuccess = function (response) {
                if (response.data.ResultCode !== 0) {
                    alertService.error(response.ResultMessage);
                } else {
                    if (!$stateParams.ContactFormsID || $stateParams.ContactFormsID == 0)
                        angular.extend($stateParams, { ContactFormsID: response.data.ID });

                    //save workflow Header details.
                    //$rootScope.$broadcast('onWorkflowHeaderSave', { workflowDataKey: $state.current.data.workflowDataKey, workflowHeaderID: $stateParams.ContactFormsID });
                    WorkflowHeaderService.AddWorkflowHeader({ WorkflowDataKey: $state.current.data.workflowDataKey, RecordHeaderID: $stateParams.ContactFormsID, ContactID: $stateParams.ContactID });
                    if ($state.current.name.indexOf(formSectionURL) == -1) {
                        return $scope.saveRecordingService($stateParams.ContactFormsID).then(saveServiceSuccess, saveFailure);
                    } else {
                        var msg = (response.ID) ? "saved" : "updated";
                        alertService.success('Form ' + msg + ' successfully.');
                        $state.transitionTo($state.current.name, $stateParams, { notify: true });
                        if (documentStatusID == DOCUMENT_STATUS.Completed) {
                            $rootScope.$broadcast('getComplete');
                        }
                        resetForm();
                        if (moveNext) {
                            nextSection($stateParams);
                        }
                        return $scope.promiseNoOp();
                    }
                }
            };

            var saveServiceSuccess = function (response) {
                if (response.ResultCode !== 0) {
                    alertService.error(response.ResultMessage);
                } else {
                    var msg = (response.ID) ? "saved" : "updated";
                    alertService.success('Form ' + msg + ' successfully.');
                    angular.extend($stateParams, {
                        ResponseID: $stateParams.ResponseID ? $stateParams.ResponseID : ($scope.responseId ? $scope.responseId : 0),
                        SectionID: $stateParams.SectionID ? $stateParams.SectionID : 0,
                        ReadOnly: ($stateParams.DocumentStatusID == DOCUMENT_STATUS.Completed) ? "view" : "edit"
                    });
                    $state.transitionTo(serviceName, $stateParams, { notify: true });
                    if (documentStatusID == DOCUMENT_STATUS.Completed) {
                        $rootScope.$broadcast('getComplete');
                        checkFormStatus({ stateName: serviceName, validationState: VALIDATION_STATE.Valid });
                    }
                    else {
                        checkFormStatus({ stateName: serviceName, validationState: VALIDATION_STATE.Invalid });
                    }
                }
                resetForm();
                if (moveNext) {
                    nextSection($stateParams);
                }
                return $scope.promiseNoOp();
            };

            var saveFailure = function (response) {
                    if (response.ResultCode !== 15)
                        alertService.error('OOPS! Something went wrong.');
            };

            var nextSection = function (statePrms) {
                var nextState = angular.element("li[data-state-name].list-group-item.active").next("li[data-state-name].list-group-item");
                if (nextState && nextState.length == 0) {
                    // for xen-workflow
                    nextState = angular.element("xen-workflow-action[data-state-name] > li.list-group-item.active").parents('.work-flow').next().find("xen-workflow-action[data-state-name]");
                }
                angular.extend($stateParams, statePrms);
                var stateParams = $stateParams;
                if (nextState.length !== 1) {
                    nextState = $scope.returnState;
                } else {
                    stateParams.SectionID = Math.abs(nextState.attr('data-state-key'));
                    nextState = nextState.attr('data-state-name');
                }

                if ($state.current.name.indexOf('initializeformservice') >= 0) {
                    $timeout(function () { $state.go(nextState, stateParams) });
                } else {
                    $state.go(nextState, stateParams, { notify: true });
                }
            };

            $scope.initFormsReport = function (response, wasDirty) {
                var lettersModel = getLetterModel();
                if (!$stateParams.ResponseID || $stateParams.ResponseID === 0) {
                    angular.extend($stateParams, { ResponseID: $scope.responseId });
                    angular.extend(lettersModel, { ResponseID: $scope.responseId ? $scope.responseId : null, DocumentStatusID: $stateParams.DocumentStatusID });
                }
                if (wasDirty) {
                    return saveFormDetails(lettersModel).then(function () {
                        return assessmentPrintService.initReports($stateParams.AssessmentID, $scope.responseId, $stateParams.SectionID, $state.current.data.workflowDataKey, $stateParams.ContactFormsID).then(onPrintReportReceived.bind(this, $rootScope));
                    });
                }
                else {
                    return assessmentPrintService.initReports($stateParams.AssessmentID, $scope.responseId, $stateParams.SectionID, $state.current.data.workflowDataKey, $stateParams.ContactFormsID).then(onPrintReportReceived.bind(this, undefined));
                }
            }

            var onPrintReportReceived = function (rootScope, data) {
                var dfd = $q.defer()
                var reportModel = data;
                reportModel[2585] = $scope.SERVICE_START_DATE;
                reportModel.contactSSN = SSN ? SSN : '';
                if (rootScope) {
                    reportModel.alreadyExecuted = true;
                    rootScope.reportModel = reportModel;
                    rootScope.reportModel.HasLoaded = true;
                    $('#reportModal').modal('show');
                }
                dfd.resolve(reportModel);
                return dfd.promise;
            }

            $scope.printReport = function (isNext, mandatory, hasErrors, keepForm, next) {
                $scope.saveRecordedService(isNext, mandatory, hasErrors, keepForm, next).then(function (response) {
                    printService.initPrint($stateParams.ContactFormsID, SERVICE_RECORDING_SOURCE.IDDForms, $scope.serviceRecording.ServiceRecordingID, $state.current.data.workflowDataKey, $stateParams.ContactFormsID).then(function (data) {

                        $rootScope.reportModel = data;
                        $rootScope.formReset($scope.ctrl.serviceRecordingForm);
                        $rootScope.reportModel.HasLoaded = true;
                        $('#reportModal').modal('show');
                    });
                });
            }
            $scope.$on('$destroy', function () {
                $rootScope.reportModel = null;
            });
            $scope.disableForm = function () {
                if ($stateParams.SectionID != BAPN_ASSESSMENT_SECTION.Addedndum || cacheService.get('IsVoidedRecord')) {
                    if (isOtherUser || cacheService.get('IsVoidedRecord') || $stateParams.DocumentStatusID == DOCUMENT_STATUS.Completed) {
                        $scope.noAccessToOther = true;
                        $scope.isInactive = true;
                        $scope.userCredentials = lookupService.getLookupsByType('Credential');
                        $scope.disableAssessmentSave = true;
                    }

                }
                else {
                    $scope.enableSave = true;
                    $scope.disableAssessmentSave = false;
                    $scope.hideNextAssessment = true;
                }
                // Disable Note screen if user signed it 
                if (!$scope.noAccessToOther && $scope.responses && $scope.responses[2589] && $scope.responses[2589][3701]) {
                    $scope.noAccessToOther = true;
                }

                 //Will fetch the date from Service Screen
                if ($scope.responses && $scope.responses[2585] && $scope.responses[2585][17] && $scope.SERVICE_START_DATE) {
                    $scope.responses[2585][17] = $scope.SERVICE_START_DATE;
                }
            };

            var checkFormStatus = function (state) {
                $rootScope.$broadcast('rightNavigationIntakeFormHandler', state);
            }

           
        }]);
}());
