angular.module('xenatixApp')
    .controller('lawLiaisonscreeningController', ['$scope', '$q', '_', 'alertService', 'lookupService', '$filter', '$stateParams', '$rootScope', 'formService', '$state', 'sectionID', 'responseID', 'parentResponseID', 'viewMode', 'registrationService', 'callerInformationService', 'contactPhoneService', 'navigationService', 'cacheService', 'dateTimeValidatorService', 'lawLiaisonscreeningService', 'helperService', 'WorkflowHeaderService',
        function ($scope, $q, _, alertService, lookupService, $filter, $stateParams, $rootScope, formService, $state, sectionID, responseID, parentResponseID, viewMode, registrationService, callerInformationService, contactPhoneService, navigationService, cacheService, dateTimeValidatorService, lawLiaisonscreeningService, helperService, WorkflowHeaderService) {
            var self = this;
            var callCenterTypeID = 2;
            var defaultCallStatusID = 2;
            var contactID = $stateParams.ContactID;
            var assessmentID = ASSESSMENT_TYPE.LawLiaisonScreening;
            var reportModel = null;
            var defaultTime = "00:00:00";
            var OTHER_TXT = 'Other';
            var inputType = {
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
            if ($state.current.data && $state.current.data.credentialKey)
                $scope.credentialKey = $state.current.data.credentialKey;

            self.otherReferralAgency = REFERRAL_AGENCY.Other;

            var init = function () {
                self.dateOptions = {
                    formatYear: 'yy',
                    startingDay: 1,
                    showWeeks: false
                };
                $scope.isScreeningSigned = false;
                self.endDate = new Date();
                self.startDate = $filter('calculate120years')();
                self.enterKeyStop = false;

                var lawLiaisonFollowUp = cacheService.get('lawLiaisonFollowUp');
                if (lawLiaisonFollowUp) {
                    $scope.isFollowup = lawLiaisonFollowUp.followupRequired;
                }
                $scope.isReadOnly = cacheService.get('IsReadOnlyLLScreens');
                $scope.assessmentNoAccess = ($scope.isReadOnly || $scope.isFollowup) ? true : false;
                $scope.isAssessmentIntegrated = true;
                $scope.isOther = false;
                self.headerID = $stateParams.CallCenterHeaderID;
                initFormDetails();
                if (self.headerID && self.headerID != 0) {
                    if ($scope.isFollowup) {
                        self.headerID = lawLiaisonFollowUp.parentCallCenterHeaderID;
                        get(lawLiaisonFollowUp.parentCallCenterHeaderID);
                    }
                    else {
                        get(self.headerID);
                    }
                }
                else
                    setFormStatus(false);

                if (parentResponseID) {
                    transitTo(parentResponseID);
                }
                else {
                    transitTo(responseID);
                }
                $scope.$parent['autoFocus'] = true;

            };
            function transitTo(responseID) {
                angular.extend($stateParams, {
                    SectionID: sectionID,
                    ResponseID: responseID
                });
                $state.transitionTo($state.current.name, $stateParams);
            };

            var initFormDetails = function () {
                self.callerInformation = {
                    ContactID: 0,
                    FirstName: '',
                    LastName: '',
                    DOB: null,
                    ContactTypeID: 9,
                    ModifiedOn: new Date()
                };

                self.Phones = [];
                self.Phones.push(objPhone());
                self.callerDetails = {
                    CallCenterHeaderID: 0,
                    ProviderID: null,
                    ReasonCalled: '',
                    DateOfIncident: $filter('formatDate')(new Date(), 'MM/DD/YYYY'),
                    Disposition: '',
                    OtherInformation: '',
                    CallStatusID: null,
                    Comments: '',
                    ProgramUnitID: null,
                    SuicideHomicideID: 2,
                    CallCenterPriorityID: 1,
                    CallCenterTypeID: callCenterTypeID,
                    CallerID: null,
                    ContactID: null,
                    ReferralAgencyID: null,
                    ReferralAgencyName: null,
                    OtherReferralAgency: null,
                    CallStartTime: $filter('formatDate')(new Date(), 'MM/DD/YYYY hh:mm:ss A'),
                    CallEndTime: null,
                    CountyID: null,
                    ModifiedOn: new Date(),
                    IsCallerClientSame: false,
                    FollowUpRequired: false,
                    CallStatusID: defaultCallStatusID,
                };

                self.providerDetail = {
                    ProviderDate: $filter('formatDate')(new Date(), 'MM/DD/YYYY'),
                    ProviderStartTime: $filter('formatDate')(new Date(), 'hh:mm'),
                    CallStartAMPM: $filter('toStandardTimeAMPM')(dateTimeValidatorService.getCurrentMeridian(new Date())),
                    ProviderEndTime: '',
                    disableEndTime: false
                };

                if (!self.UserID) {
                    navigationService.get().then(function (data) {
                        if (hasData(data)) {
                            self.UserID = data.DataItems[0].UserID;
                            self.providerDetail.ProviderBy = self.UserID;      //default taken by to the logged in user
                            self.providerDetail.Provider = lookupService.getText("Users", self.UserID);
                            //if user don't have QMHP permission then readOnly Screen.
                            $scope.hasQMHPPermission = !(data.DataItems[0].UserCredentials.filter(
                                     function (obj, value) {
                                         return (obj.CredentialID == CREDENTIAL_PERMISSION.QMHP);
                                     }).length > 0);
                        }
                    }).finally(function () {
                        resetForm();
                    });
                }
                else {
                    self.providerDetail.ProviderBy = self.UserID;         //default taken by to the logged in user
                    self.providerDetail.Provider = lookupService.getText("Users", self.UserID);
                }
            };

            var objPhone = function () {
                var obj = {
                    ContactPhoneID: 0,
                    ContactID: null,
                    PhoneTypeID: null,
                    Number: '',
                    PhonePermissionID: null,
                    IsPrimary: true,
                    IsActive: true
                };
                return obj;
            };

            var resetForm = function () {
                if (self.lawLiaisonScreeningForm) {
                    $rootScope.formReset(self.lawLiaisonScreeningForm);
                    self.lawLiaisonScreeningForm.modified = false;
                }
                resetHeader();
                resetCaller();
                resetPhone();
                resetAssessmentForm();
                if (self.callerInformationForm.lawLiaisonForm) {
                    $rootScope.formReset(self.callerInformationForm.lawLiaisonForm, self.callerInformationForm.lawLiaisonForm.$name);
                    self.callerInformationForm.lawLiaisonForm.modified = false;
                }

            };

            var resetCaller = function () {
                if (self.callerDetailsForm.callerForm)
                    $rootScope.formReset(self.callerDetailsForm.callerForm, self.callerDetailsForm.callerForm.$name);
            };

            var resetPhone = function () {
                if (self.callerInformationForm.phoneForm)
                    $rootScope.formReset(self.callerInformationForm.phoneForm, self.callerInformationForm.phoneForm.$name);
            };

            var resetHeader = function () {
                if (self.callerInformationForm.takenDetailsForm) {
                    $rootScope.formReset(self.callerInformationForm.takenDetailsForm, self.callerInformationForm.takenDetailsForm.$name);
                    self.callerInformationForm.takenDetailsForm.modified = false;
                }
            };

            var resetAssessmentForm = function () {
                if (self.assessmentForm)
                    $rootScope.formReset(self.assessmentForm, self.assessmentForm.$name);
            };

            var setFormStatus = function (value) {
                var stateDetail = { stateName: 'callcenter.lawliaison.screening', validationState: value ? 'valid' : 'warning' };
                $rootScope.$broadcast('rightNavigationCallCenterHandler', stateDetail);
            };

            $scope.changeAgency = function () {
                if (self.callerDetails.ReferralAgencyName !== OTHER_TXT) {
                    resetOtherReferral();
                }
            }

            $scope.checkOther = function (value) {
                self.callerDetails.ReferralAgencyName = value.Name;
                self.callerDetails.ReferralAgencyID = value.ID;
                if (self.callerDetails.ReferralAgencyName == OTHER_TXT) {
                    $scope.isOther = true;
                }
                else {
                    resetOtherReferral();
                }
            };

            var resetOtherReferral = function () {
                $scope.isOther = false;
                self.callerDetails.OtherReferralAgency = '';
            }

            $scope.saveLawLiaisonScreening = function (isNext, mandatory, hasErrors, keepForm, next) {
                if (helperService.validateSignature(self.assessmentForm.signatureForm, $scope.dSignature)) {
                    return $scope.saveLawLiaison(isNext, mandatory, hasErrors, keepForm, next)
                        .then(function (response) {
                            var msg = self.isNewCaller ? 'saved' : 'updated';
                            if (isNext && !hasErrors) {
                                if (response && response != -1) {
                                    successMessage(msg);
                                }
                                resetForm();
                                self.next();
                            }
                            else if (response && response != -1) {
                                successMessage(msg);
                                get($stateParams.CallCenterHeaderID);
                            }
                        })
                }
            };

            var successMessage = function (message) {
                alertService.success('Law Liaison screening ' + message + ' successfully.');
                //save workflow Header details.
                //$rootScope.$broadcast('onWorkflowHeaderSave', { workflowDataKey: $state.current.data.workflowDataKey, workflowHeaderID: $stateParams.CallCenterHeaderID });
                WorkflowHeaderService.AddWorkflowHeader({ WorkflowDataKey: $state.current.data.workflowDataKey, RecordHeaderID: $stateParams.CallCenterHeaderID, ContactID: $stateParams.ContactID });
            }

            $scope.saveLawLiaison = function (isNext, mandatory, hasErrors, keepForm, next) {
                if (self.callerDetails.ReferralAgencyID != self.otherReferralAgency) {
                    self.callerDetails.OtherReferralAgency = null;
                }
                if (!mandatory && isNext && hasErrors) {
                    self.next();
                }

                self.isNext = isNext;
                self.mandatory = mandatory;
                self.hasErrors = hasErrors;
                self.keepForm = keepForm;

                if ((formService.isDirty(self.callerInformationForm.takenDetailsForm.$name) ||
                    formService.isDirty(self.callerDetailsForm.callerForm.$name) ||
                    formService.isDirty(self.callerInformationForm.phoneForm.$name) ||
                    formService.isDirty(self.callerInformationForm.lawLiaisonForm.$name) ||
                    formService.isDirty())
                    && !hasErrors) {

                    return saveContact();;
                }

                return $rootScope.promiseNoOp();
            };

            var saveContact = function () {
                if (!formService.isDirty(self.callerDetailsForm.callerForm.$name) && self.callerInformation.ContactID != 0) {
                    var response = {};
                    response.data = {};
                    response.data.ResultCode = 0;

                    return saveContactSuccess(response);
                }
                else if (formService.isDirty(self.callerDetailsForm.callerForm.$name)) {
                    //Saves caller as contact
                    self.callerInformation.ModifiedBy = self.UserID;
                    self.callerInformation.ModifiedOn = new Date();

                    if (self.callerInformation.ContactID != 0)
                        return registrationService.update(self.callerInformation).then(saveContactSuccess, errorMethod, notifyMethod);
                    else
                        return registrationService.add(self.callerInformation).then(saveContactSuccess, errorMethod, notifyMethod);
                }
            };

            var saveContactSuccess = function (resp) {
                if ((resp && resp.data && resp.data.ID) || (self.callerInformation.ContactID != 0 && resp && resp.data.ResultCode == 0)) {
                    var callerID;

                    if (self.callerInformation.ContactID != 0)       //if update
                    {
                        callerID = self.callerInformation.ContactID;
                        self.isNewCaller = false;
                    }
                    else {
                        callerID = resp.data.ID;
                        self.isNewCaller = true;
                    }

                    self.callerInformation.ContactID = self.callerDetails.CallerContactID = self.callerDetails.CallerID = callerID;

                    savePhone(callerID);

                    var dateVal = moment(self.providerDetail.ProviderDate).toDate();
                    var timeVal = $filter('toStandardTime')(self.providerDetail.ProviderStartTime);
                    var hr = timeVal.substring(0, timeVal.indexOf(':'));

                    if (self.providerDetail.CallStartAMPM == "PM" && hr != 12)      //checks if PM, adds 12 hours
                        hr = +hr + +12;
                    else if (self.providerDetail.CallStartAMPM == "AM" && hr == 12) { // BUGFIX - 13633
                        hr = "0";
                    }

                    var min = timeVal.substring(timeVal.indexOf(':') + 1, timeVal.length);
                    var dateTime = dateVal.setHours(hr, min);

                    self.callerDetails.DateOfIncident = $filter('formatDate')(moment(self.callerDetails.DateOfIncident).toDate());
                    self.callerDetails.CallStartTime = $filter('formatDate')(moment(dateTime).toDate(), 'MM/DD/YYYY hh:mm A');
                    self.callerDetails.ProviderID = self.providerDetail.ProviderBy;
                    self.callerDetails.ModifiedOn = new Date();

                    // ReferralAgency is selected by type-ahead, but ReferralAgencyID is needed
                    if (self.callerDetails.ReferralAgency)
                        self.callerDetails.ReferralAgencyID = self.callerDetails.ReferralAgency.ID;

                    if (self.callerDetails.CallCenterHeaderID != 0)
                        return callerInformationService.update(self.callerDetails, true).then(saveCallerInfoSuccess, errorMethod, notifyMethod);
                    else
                        return callerInformationService.add(self.callerDetails).then(saveCallerInfoSuccess, errorMethod, notifyMethod);
                }

                return $rootScope.promiseNoOp();
            };

            var savePhone = function (callerID) {
                //Save Phone details
                if (formService.isDirty(self.callerInformationForm.phoneForm.$name)) {
                    var taskArray = [];

                    angular.forEach(self.Phones, function (phone) {
                        phone.ModifiedBy = self.UserID;
                        phone.ModifiedOn = new Date();
                        phone.ContactID = callerID;

                        if (phone.Number != '' || phone.ContactPhoneID != 0) {
                            if (phone.ContactPhoneID != 0 && (phone.Number == '' || phone.Number == undefined || phone.Number == null))
                                phone.IsActive = false;
                            if (phone.IsActive == true) {
                                taskArray.push([contactPhoneService.save, [phone]]);
                            }
                            else if (phone.ContactPhoneID != 0) {
                                taskArray.push([contactPhoneService.remove, [phone.ContactID, phone.ContactPhoneID]]);
                            }
                        }
                    });

                    return $q.serial(taskArray);
                }

                return $rootScope.promiseNoOp();
            };

            var saveCallerInfoSuccess = function (resp) {
                if ((resp && resp.ID) || (self.callerDetails.CallCenterHeaderID != 0 && resp && resp.ResultCode == 0)) {
                    if (self.callerDetails.CallCenterHeaderID == 0)
                        self.callerDetails.CallCenterHeaderID = resp.ID;

                    return callerInformationService
                        .updateModifiedOn($stateParams.CallCenterHeaderID)
                        .finally(function () {
                            if ((formService.isDirty())) {
                                return $scope.saveAssessment(self.isNext, self.mandatory, self.hasErrors, self.keepForm, self.next);
                            }
                        });
                }

                return $rootScope.promiseNoOp();
            };

            var get = function () {

                callerInformationService.get(self.headerID)
                    .then(getCallerInfoSuccess, failureMethod, notifyMethod)
                    .finally(function () {
                        resetForm();
                    });
            };

            var getCallerInfoSuccess = function (data) {
                if (data && data.DataItems && data.DataItems.length > 0) {
                    self.callerDetails = data.DataItems[0];

                    if (self.callerDetails.ReferralAgencyID != null) {
                        self.callerDetails.ReferralAgencyName = lookupService.getText('ReferralAgency', self.callerDetails.ReferralAgencyID);
                        if (self.callerDetails.ReferralAgencyName == OTHER_TXT) {
                            $scope.isOther = true;
                        }
                    }
                    // provider, crisis date & time
                    self.providerDetail.ProviderStartTime = $filter('formatDate')(self.callerDetails.CallStartTime, 'hh:mm');
                    self.providerDetail.ProviderDate = $filter('formatDate')(self.callerDetails.CallStartTime, 'MM/DD/YYYY');
                    self.providerDetail.CallStartAMPM = searchString(self.callerDetails.CallStartTime, defaultTime) ? "AM" : $filter('toStandardTimeAMPM')(dateTimeValidatorService.getCurrentMeridian(moment(self.callerDetails.CallStartTime).toDate()));
                    if (self.callerDetails.ProviderID > 0) {
                        self.providerDetail.ProviderBy = self.callerDetails.ProviderID;
                        self.providerDetail.Provider = lookupService.getText("Users", self.providerDetail.ProviderBy);
                    }

                    //Gets caller details
                    if (self.callerDetails.CallerContactID != undefined && self.callerDetails.CallerContactID != null)
                        registrationService.get(self.callerDetails.CallerContactID).then(getContactSuccess, errorMethod, notifyMethod);
                    else { // Fix for offline-- incase callerContactID not exists then try to get it again with isCaller flag true
                        callerInformationService.get(self.headerID, true).then(function (callerData) {
                            if (callerData && callerData.DataItems && callerData.DataItems.length > 0) {
                                self.callerDetails.CallerContactID = callerData.DataItems[0].CallerContactID;
                                self.callerDetails.ReasonCalled = callerData.DataItems[0].ReasonCalled;
                                if (self.callerDetails.CallerContactID != undefined && self.callerDetails.CallerContactID != null)
                                    registrationService.get(self.callerDetails.CallerContactID).then(getContactSuccess, errorMethod, notifyMethod);
                            }
                        }, failureMethod, notifyMethod);
                    }
                    if (self.callerDetails.DateOfIncident) {
                        self.callerDetails.DateOfIncident = $filter('formatDate')(self.callerDetails.DateOfIncident, 'MM/DD/YYYY');
                    }
                    else {
                        self.callerDetails.DateOfIncident = $filter('formatDate')(new Date(), 'MM/DD/YYYY');
                    }
                } else {
                    alertService.error('Unable to get caller information!');
                    resetForm();
                };
            };

            var getContactSuccess = function (data) {
                if (data && data.DataItems && data.DataItems.length > 0) {
                    //Gets contact phone details
                    self.callerInformation = data.DataItems[0];
                    contactPhoneService.get(self.callerInformation.ContactID, self.callerInformation.ContactTypeID).then(getPhoneSuccess, errorMethod, notifyMethod);
                    resetForm();
                }
                else {
                    self.callerDetails.ReasonCalled = '';
                }
            };

            var getPhoneSuccess = function (data) {
                if (data && data.DataItems && data.DataItems.length > 0) {
                    self.Phones = data.DataItems;
                    resetForm();
                }
            };

            var failureMethod = function (data) {
                alertService.error('Unable to get caller information: ' + errorStatus);
                setFormStatus(false);
            };

            var errorMethod = function (err) {
                isSaving = false;
                alertService.error('OOPs something went wrong');
            };

            var notifyMethod = function (notify) {
            };

            self.next = function () {
                angular.extend($stateParams, {
                    ContactID: $stateParams.ContactID,
                    CallCenterHeaderID: $stateParams.CallCenterHeaderID
                });
                $state.go('callcenter.lawliaison.services', $stateParams);
            };

            self.selectProvider = function (item) {
                self.providerDetail.ProviderBy = item.ID;
            };

            self.getLookupsByType = function (typeName) {
                return lookupService.getLookupsByType(typeName);
            };

            self.initReport = function () {
                var reportModel = {}, promiseArr = [], dfd = $q.defer();
                $scope.saveLawLiaison(false, false, false).then(function (response) {
                    var msg = self.isNewCaller ? 'saved' : 'updated';
                    if (response && response != -1) {
                        successMessage(msg);
                        get($stateParams.CallCenterHeaderID);
                    }                
                    responseID = cacheService.get('lawLiaisonFollowUp') ? parentResponseID : responseID;

                    self.callerInformation.Phones = $scope.phoneItems;
                    lawLiaisonscreeningService.initReport($scope.AssessmentID, responseID, sectionID, self.headerID, contactID, self.providerDetail.ProviderDate, self.callerInformation, self.callerDetails, self.providerDetail.ProviderStartTime, self.providerDetail.CallStartAMPM, self.providerDetail.Provider).then(function (reportModel) {
                        dfd.resolve(reportModel);
                        $scope.reportModel = reportModel;
                        $('#reportModal').modal('show');
                    });
                });
                return dfd.promise;
            };

            $scope.validateFutureDateTime = function () {
                var errorControlBlock = angular.element("#StartTimeErrortd");
                var errorControl = angular.element("#startTimeFutureError");
                var formControl = self.callerInformationForm.takenDetailsForm;
                var formName = self.callerInformationForm.takenDetailsForm;
                var selector = 'StartTime';
                var dateControl = self.providerDetail.ProviderDate;
                dateTimeValidatorService.validateFutureDateTime(errorControlBlock, errorControl, formControl, dateControl, self.providerDetail.ProviderStartTime, self.providerDetail.CallStartAMPM, selector, formName);
            };

            $scope.postAssessmentReponseDetails = function () {
                if (hasDetails($scope.assessmentResponseDetails)) {
                    $scope.isScreeningSigned = $filter('filter')($scope.assessmentResponseDetails, { OptionsID: 3701 }, true).length > 0 ? true : false;
                    setFormStatus(true);
                }
                else
                    setFormStatus(false);
            };

            init();
        }]);