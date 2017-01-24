(function () {
    angular.module('xenatixApp')
    .controller('callerInformationController', ['$filter', 'alertService', '$stateParams', 'formService', 'callerInformationService', 'registrationService', 'navigationService', 'contactPhoneService', '$rootScope', '$scope', '$q', '$state', '$timeout', 'lookupService', 'contactAddressService', 'additionalDemographyService', 'contactSSNService', 'serviceRecordingService', 'viewMode', 'cacheService', 'scopesService', 'dateTimeValidatorService', 'roleSecurityService','WorkflowHeaderService',
        function ($filter, alertService, $stateParams, formService, callerInformationService, registrationService, navigationService, contactPhoneService, $rootScope, $scope, $q, $state, $timeout, lookupService, contactAddressService, additionalDemographyService, contactSSNService, serviceRecordingService, viewMode, cacheService, scopesService, dateTimeValidatorService, roleSecurityService, WorkflowHeaderService) {
            var self = this;
            var callCenterTypeID = 1; //Crisis Line
            self.anonymousAdult = 11;
            self.anonymousChild = 12;
            self.requiredCallStatusID = CALL_STATUS.NEEDS_REVIEW;
            $scope.voidStatusID = CALL_STATUS.VOID;
            $scope.isneedReviewAgain = true;
            var anonymousText = "Anonymous";
            var isSearched;
            var adjustedTime = "00:00:01";
            var defaultTime = "00:00:00";
            var savedContactStatusID;
            var defaultCallStatusID = CALL_STATUS.PENDING; // default status new
            var hasEditPermission = roleSecurityService.hasPermission('CrisisLine-CrisisLine-CrisisLine', PERMISSION.UPDATE);
            var hasCreatePermission = roleSecurityService.hasPermission('CrisisLine-CrisisLine-CrisisLine', PERMISSION.CREATE);
            $scope.isApprover = roleSecurityService.hasPermission('CrisisLine-CrisisLine-Approver', PERMISSION.CREATE);
            $scope.isCreatorOrApproverAccess = $scope.isApprover || cacheService.get('IsCreatorAccess');
            $scope.preventStopSave = $scope.isCreatorOrApproverAccess;
            var readOnly = function () {
                if (self.pageSecurity == 0) {
                    return hasCreatePermission ? 'edit' : 'view';
                }
                else {
                    return hasEditPermission ? ((cacheService.get('IsManagerAccess') || (cacheService.get('IsCreatorAccess') && !viewMode)) ? 'edit' : 'view') : 'view'
                }
            };
            var init = function () {
                setFormStatus({ valid: false, isNotAnonymous: true });
                self.dateOptions = {
                    formatYear: 'yy',
                    startingDay: 1,
                    showWeeks: false
                };
                if (cacheService.get('reviewFollowup')) {
                    $scope.isContactSearch = true;
                }
                else {
                    $scope.isContactSearch = false;
                }
                self.opened = false;
                self.endDate = new Date();
                self.startDate = $filter('calculate120years')();
                self.dobDateName = 'DateofBirth';

                $scope.enterKeyStop = false;

                $scope.changeContactType = function () {
                    if (self.callerDetails.ContactTypeID == CONTACT_TYPE.Anonymous || self.callerDetails.ContactTypeID == CONTACT_TYPE.Anonymous_Adult || self.callerDetails.ContactTypeID == CONTACT_TYPE.Anonymous_Child) {
                        if (self.clientDetails.FirstName.length <= 0) {
                            self.clientDetails.FirstName = anonymousText;
                        }
                        if (self.clientDetails.LastName.length <= 0) {
                            self.clientDetails.LastName = anonymousText;
                        }
                    }
                };

                initFormDetails();
                var headerID = $stateParams.CallCenterHeaderID;
                if (headerID && headerID != 0) {
                    get(headerID);
                }
                else {
                    self.pageSecurity = 0;
                    //Add watch for any model changes
                    clearScopeServiceData();
                    $scope.$watch('ctrl.clientDetails', function (newvalue, oldValue) {
                        scopesService.store(CRISISLINE_QUICK_REG_DATA.ClientData, angular.copy(self.clientDetails));
                    }, true);

                    $scope.$watch('ctrl.Phones', function (newvalue, oldValue) {
                        scopesService.store(CRISISLINE_QUICK_REG_DATA.PhoneData, angular.copy(self.Phones[0]));
                    }, true);
                }
                angular.extend($stateParams, {
                    ReadOnly: readOnly()
                });
                self.enableFollowUpFields = cacheService.get('FollowUp') ? true : ($stateParams.ReadOnly == 'view' ? false : true);
                $state.transitionTo($state.current.name, $stateParams);

            };

            var clearScopeServiceData = function () {
                scopesService.clear(CRISISLINE_QUICK_REG_DATA.ClientData);
                scopesService.clear(CRISISLINE_QUICK_REG_DATA.PhoneData);
            };
            var initCallerInformationDetails = function () {
                self.callerInformation = {
                    ContactID: undefined,
                    FirstName: '',
                    LastName: '',
                    DOB: null,
                    ContactTypeID: CONTACT_TYPE.Crisis_Line,
                    ModifiedOn: new Date()
                };
                self.CallerPhones = [];
                self.CallerPhones.push(objPhone());
            };

            var initFormDetails = function () {
                initCallerInformationDetails();
                initContactDetails();
                self.callerDetails = {
                    CallCenterHeaderID: 0,
                    ProviderID: null,
                    ReasonCalled: '',
                    Disposition: '',
                    OtherInformation: '',
                    CallStatusID: null,
                    preComments: '',
                    newComment: '',
                    ProgramUnitID: null,
                    SuicideHomicideID: 2,
                    CallCenterPriorityID: 1,
                    CallCenterTypeID: callCenterTypeID,
                    CallerID: null,
                    ContactID: null,
                    ContactTypeID: CONTACT_TYPE.New,
                    CallStartTime: new Date(),
                    CallEndTime: null,
                    CountyID: null,
                    ModifiedOn: new Date(),
                    IsCallerClientSame: false,
                    FollowUpRequired: false,
                    CallStatusID: defaultCallStatusID
                };
                self.providerDetail = {
                    ProviderDate: $filter('formatDate')(new Date(), 'MM/DD/YYYY'),
                    ProviderStartTime: $filter('formatDate')(new Date(), 'hh:mm'),
                    ProviderEndTime: '',
                    disableEndTime: false,
                    CallEndAMPM: null,
                    CallStartAMPM: $filter('toStandardTimeAMPM')(dateTimeValidatorService.getCurrentMeridian(new Date())),
                    enableProviderDate: false,
                    enableProviderStartTime: false,
                    enableProviderEndTime: false
                };
                if (!self.UserID) {
                    navigationService.get().then(function (data) {
                        self.UserID = data.DataItems[0].UserID;
                        self.cmtReporter = data.DataItems[0].UserFullName;
                        self.providerDetail.ProviderBy = self.UserID;      //default taken by to the logged in user
                    }).finally(function () {
                        resetForm();
                    });
                }
                else {
                    self.providerDetail.ProviderBy = self.UserID;         //default taken by to the logged in user
                }
            };
            var initContactDetails = function () {
                self.clientDetails = {
                    ContactID: 0,
                    ContactTypeID: null,
                    FirstName: '',
                    LastName: '',
                    ClientTypeID: null,
                    DOB: null,
                    ModifiedOn: new Date(),
                    GenderID: null,
                    SSN: null
                };
                self.additionalDemoDetails = {
                    AdditionalDemographicID: null,
                    ContactID: 0,
                    MaritalStatusID: null,
                    GenerateMRN: null

                };
                self.Phones = [];
                self.Phones.push(objPhone());

                $scope.Addresses = [{
                    AddressTypeID: ADDRESS_TYPE.ResidenceHome,
                    Line1: '',
                    Line2: '',
                    City: '',
                    StateProvince: null,
                    County: null,
                    Zip: '',
                    MailPermissionID: 1,
                    IsPrimary: true,
                    IsGateCode: true,
                    IsComplexName: true
                }];
            }

            var resetForm = function () {
                if (self.callerInformationForm && (typeof self.callerInformationForm.$name) != "undefined") {
                    self.callerInformationForm.$setPristine();
                    formService.reset();
                }
                resetHeader();
                resetTakenDetails();
                resetCaller();
                resetClient();
                resetContactType();
                resetPhone();
                resetAddress();
                resetAdditionalDemo();
            };

            var resetTakenDetails = function () {
                if (self.callerInformationForm.takenDetails)
                    $rootScope.formReset(self.callerInformationForm.takenDetails, self.callerInformationForm.takenDetails.$name);
            };

            var resetCaller = function () {
                if (self.callerDetailsForm.callerForm)
                    $rootScope.formReset(self.callerDetailsForm.callerForm, self.callerDetailsForm.callerForm.$name);
            };

            var resetClient = function () {
                if (self.callerInformationForm.clientForm)
                    $rootScope.formReset(self.callerInformationForm.clientForm, self.callerInformationForm.clientForm.$name);
            };

            var resetContactType = function () {
                if (self.callerInformationForm.clientForm.contactTypeForm)
                    $rootScope.formReset(self.callerInformationForm.clientForm.contactTypeForm, self.callerInformationForm.clientForm.contactTypeForm.$name);
            };

            var resetPhone = function (isQuickRegCall) {
                if (self.callerInformationForm.phoneForm)
                    $rootScope.formReset(self.callerInformationForm.phoneForm, self.callerInformationForm.phoneForm.$name);

                if (!isQuickRegCall)
                    resetClientPhone();
            };

            var resetAddress = function () {
                if (self.callerInformationForm.AddressForm)
                    $rootScope.formReset(self.callerInformationForm.AddressForm, self.callerInformationForm.AddressForm.$name);
            };

            var resetClientPhone = function () {
                if (self.clientPhoneForm)
                    $rootScope.formReset(self.clientPhoneForm, self.clientPhoneForm.$name);
            };

            var resetHeader = function () {
                if (self.callerInformationForm.callerInfo)
                    $rootScope.formReset(self.callerInformationForm.callerInfo, self.callerInformationForm.callerInfo.$name);
            };

            var resetAdditionalDemo = function () {
                if (self.callerInformationForm.additionalDemoForm)
                    $rootScope.formReset(self.callerInformationForm.additionalDemoForm, self.callerInformationForm.additionalDemoForm.$name);
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
            $scope.phoneThreeDigits = function () {
                phoneThreeDigitsCheck(self.clientPhoneForm.clientPhoneForm.phoneNumber, self.CallerPhones[0]);
            };
            var get = function (headerID) {
                self.isLoading = true;
                //var headerID = $stateParams.CallCenterHeaderID;
                callerInformationService.get(headerID).then(successMethod, failureMethod, notifyMethod).finally(function () {
                    self.isLoading = false;
                    resetForm();
                });
            };

            var setFormStatus = function (stateInformation) {
                $timeout(function () {
                    if (stateInformation.isNotAnonymous != undefined)
                        var stateDetail = { stateName: 'callcenter.crisisline.callerinformation', validationState: stateInformation.valid ? 'valid' : 'warning', isNotAnonymous: stateInformation.isNotAnonymous };
                    else
                        var stateDetail = { stateName: 'callcenter.crisisline.callerinformation', validationState: stateInformation.valid ? 'valid' : 'warning' };
                    $rootScope.$broadcast('rightNavigationCallCenterHandler', stateDetail);
                });
            };

            self.next = function () {
                angular.extend($stateParams, {
                    ContactID: self.clientDetails.ContactID,
                    CallCenterHeaderID: self.callerDetails.CallCenterHeaderID,
                    ReadOnly: readOnly()
                });

                self.enableFollowUpFields = cacheService.get('FollowUp');
                $scope.Goto('callcenter.crisisline.services', $stateParams);
            };

            $scope.save = function (isNext, mandatory, hasErrors, keepForm, next) {
                //Save Caller details
                if (isNext && next === undefined) {
                    next = function () {
                        next();
                    }
                }
                if (!mandatory && isNext && hasErrors) {
                    self.next();
                }
                self.IsNext = isNext;
                if ((formService.isDirty() ||
                    formService.isDirty(self.callerDetailsForm.callerForm.$name) ||
                    formService.isDirty(self.callerInformationForm.takenDetails.$name) ||
                    formService.isDirty(self.callerInformationForm.phoneForm.$name) ||
                    formService.isDirty(self.callerInformationForm.clientForm.$name) ||
                    formService.isDirty(self.callerInformationForm.clientForm.contactTypeForm.$name) ||
                    formService.isDirty(self.callerInformationForm.callerInfo.$name) ||
                    formService.isDirty(self.clientPhoneForm.$name) ||
                    formService.isDirty(self.callerInformationForm.additionalDemoForm.$name) ||
                    formService.isDirty(self.callerInformationForm.AddressForm.$name))
                    && !hasErrors) {
                    var isAnonymousContact = ((self.callerDetails.ContactTypeID === CONTACT_TYPE.Anonymous || self.callerDetails.ContactTypeID === CONTACT_TYPE.Anonymous_Adult || self.callerDetails.ContactTypeID === CONTACT_TYPE.Anonymous_Child)
                        && ((self.clientDetails.FirstName === anonymousText || !self.clientDetails.FirstName) && (self.clientDetails.LastName === anonymousText || !self.clientDetails.LastName)));
                    if ((!self.clientDetails.ContactID) && !isAnonymousContact) {
                        registrationService.verifyDuplicateContacts(self.clientDetails).then(function (data) {
                            if (hasData(data)) {
                                $scope.callDuplicateContactList = data.DataItems;
                            }
                            else
                                $scope.saveDetails();
                        });
                    }
                    else
                        $scope.saveDetails();
                }
                else if (self.IsNext) {
                    self.next();
                }
            }

            self.UpdateClientDetails = function () {
                if (self.callerDetails.IsCallerClientSame) {
                    if (self.clientDetails.FirstName && self.clientDetails.FirstName.length > 0)
                        self.callerInformation.FirstName = self.clientDetails.FirstName;
                    else
                        self.callerInformation.FirstName = anonymousText;
                    if (self.clientDetails.LastName && self.clientDetails.LastName.length > 0)
                        self.callerInformation.LastName = self.clientDetails.LastName;
                    else
                        self.callerInformation.LastName = anonymousText;
                }
            };

            $scope.SetCallerAsClient = function (isContactSearched) {
                if (!self.callerDetails.IsCallerClientSame || isContactSearched) {
                    if (!self.callerDetails.IsLinkedToContact && (((self.callerInformation.LastName != '' || self.callerInformation.FirstName != '')
                        && (self.clientDetails.FirstName == '' && self.clientDetails.LastName == '')) ||
                        ((self.callerInformation.LastName != '' || self.callerInformation.FirstName != '')
                        && (self.clientDetails.FirstName != '' || self.clientDetails.LastName != '')) ||
                        hasPhoneDetails('caller'))) {
                        self.clientDetails.FirstName = self.callerInformation.FirstName;
                        self.clientDetails.LastName = self.callerInformation.LastName;
                        self.Phones = self.CallerPhones;
                        self.callerDetails.ClientContactID = self.callerDetails.CallerContactID;
                        self.callerDetails.ContactID = self.callerDetails.CallerID;
                    }
                    else if (self.callerDetails.IsLinkedToContact || ((self.clientDetails.FirstName != '' || self.clientDetails.LastName != '') && (self.callerInformation.LastName == '' && self.callerInformation.FirstName == '') || hasPhoneDetails('contact'))) {
                        self.callerInformation.FirstName = self.clientDetails.FirstName;
                        self.callerInformation.LastName = self.clientDetails.LastName;
                        self.CallerPhones = self.Phones;
                        self.callerDetails.CallerContactID = self.callerDetails.ClientContactID;
                        self.callerDetails.CallerID = self.callerDetails.ContactID;
                    }
                    if (self.clientDetails.FirstName == '') {
                        self.clientDetails.FirstName = anonymousText;
                    }
                    if (self.clientDetails.LastName == '') {
                        self.clientDetails.LastName = anonymousText;
                    }
                    if (self.clientPhoneForm)
                        $rootScope.formReset(self.clientPhoneForm, self.clientPhoneForm.$name);
                }
                else {
                    if (angular.equals(self.CallerPhones, self.Phones)) {
                        self.CallerPhones = angular.copy(self.Phones);
                        angular.forEach(self.CallerPhones, function (callerPhones) {
                            callerPhones.ContactPhoneID = 0;
                        });
                    }
                    //self.CallerPhones.push(objPhone());
                    self.callerInformation.ContactID = self.callerDetails.CallerContactID = 0;
                    self.callerInformation.ContactTypeID = CONTACT_TYPE.Crisis_Line;
                    if (!self.callerDetails.IsLinkedToContact) {
                        initContactBasicDetails();
                    }
                    if (self.clientPhoneForm)
                        formService.initForm(true, self.clientPhoneForm.$name);

                }
            };
            var initContactBasicDetails = function () {
                self.clientDetails.FirstName = '';
                self.clientDetails.LastName = '';
                self.Phones = [];
                self.Phones.push(objPhone());
            }
            var hasPhoneDetails = function (modeuleName) {
                if (modeuleName == 'caller') {
                    return (self.CallerPhones.length > 0 && self.CallerPhones[0].Number != '');
                } else {
                    return (self.Phones.length > 0 && self.Phones[0].Number != '');
                }
            }

            $scope.saveDetails = function () {
                if ((formService.isDirty(self.callerInformationForm.clientForm.$name) || formService.isDirty(self.callerInformationForm.clientForm.contactTypeForm.$name)) && !self.callerDetails.IsLinkedToContact) {
                    //Save Client details
                    if (!isSearched && self.clientDetails.ContactTypeID == CONTACT_TYPE.Patient && $scope.LinkedContactID) {
                        self.clientDetails.ContactTypeID = self.callerDetails.ContactTypeID;
                    }
                    else if (!isSearched && self.clientDetails.ContactTypeID != CONTACT_TYPE.Patient) {
                        self.clientDetails.ContactTypeID = self.callerDetails.ContactTypeID;
                    }
                    else if (isSearched) {
                        self.clientDetails.ContactTypeID = self.callerDetails.ContactTypeID;
                    }
                    angular.forEach(self.Phones, function (phone) {
                        phone.ContactPhoneID = 0;
                    });
                    angular.forEach($scope.Addresses, function (address) {
                        address.ContactAddressID = 0;
                        address.AddressID = 0;
                    });

                    self.callerDetails.ContactTypeID = self.callerDetails.ContactTypeID || CONTACT_TYPE.Crisis_Line;
                    self.clientDetails.ModifiedBy = self.providerDetail.ProviderBy;
                    self.clientDetails.ModifiedOn = new Date();
                    self.clientDetails.DOB = self.clientDetails.DOB ? $filter('formatDate')(self.clientDetails.DOB) : null;
                    if (self.clientDetails.ContactID != 0)
                        registrationService.update(self.clientDetails).then(saveClientSuccessMethod, errorMethod, notifyMethod);
                    else
                        registrationService.add(self.clientDetails).then(saveClientSuccessMethod, errorMethod, notifyMethod);
                }
                else {
                    var response = {};
                    response.data = {};
                    response.data.ResultCode = 0;
                    if (self.callerDetails.IsLinkedToContact && $scope.LinkedContactID)
                        self.clientDetails.ContactID = $scope.LinkedContactID;
                    saveClientSuccessMethod(response);
                }
            };
            var saveCallerSuccessMethod = function (resp) {
                if ((resp && resp.data && resp.data.ID) || (self.callerInformation.ContactID != 0 && resp && resp.data.ResultCode == 0)) {
                    var callerID;
                    if (self.callerDetails.IsCallerClientSame) {
                        callerID = self.clientDetails.ContactID;
                    }
                    else {
                        if (self.callerInformation.ContactID && self.callerInformation.ContactID != 0)       //if update
                            callerID = self.callerInformation.ContactID;
                        else
                            callerID = resp.data.ID;
                    }
                    self.callerInformation.ContactID = self.callerDetails.CallerID = self.callerDetails.CallerContactID = callerID;
                    //Save Phone details
                    //[Bug:11945] consider phone changes to be saved when choose to link.
                    if (!self.callerDetails.IsCallerClientSame) {
                        if (formService.isDirty(self.clientPhoneForm.$name)) {
                            var taskArray = [];
                            angular.forEach(self.CallerPhones, function (phone) {
                                phone.ModifiedBy = self.providerDetail.ProviderBy;
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
                            $q.serial(taskArray);
                        }
                    }
                    var clientID;
                    if (self.clientDetails.ContactID != 0)
                        clientID = self.clientDetails.ContactID;
                    else
                        clientID = resp.data.ID;

                    self.clientDetails.ContactID = self.callerDetails.ContactID = self.callerDetails.ClientContactID = clientID;

                    var dateVal = moment(self.providerDetail.ProviderDate).toDate();
                    var timeVal = $filter('toStandardTime')(self.providerDetail.ProviderStartTime);
                    var hr = timeVal.substring(0, timeVal.indexOf(':'));
                    if (self.providerDetail.CallStartAMPM == "PM" && hr != 12) {     //checks if PM, adds 12 hours
                        hr = +hr + 12;
                    }
                    else if (self.providerDetail.CallStartAMPM == "AM" && hr == 12) { // BUGFIX - 13633
                        hr = "0";
                    }
                    timeVal = timeVal.substring(timeVal.indexOf(':') + 1, timeVal.length)
                    var min = timeVal.substring(0, timeVal.length);

                    var dateTime = dateVal.setHours(hr, min);
                    self.callerDetails.CallStartTime = $filter('formatDate')(moment(dateTime).toDate(), 'MM/DD/YYYY hh:mm A');

                    if (self.providerDetail.ProviderEndTime) {
                        var timeValEnd = $filter('toStandardTime')(self.providerDetail.ProviderEndTime || "00:00");
                        var hr1 = timeValEnd.substring(0, timeValEnd.indexOf(':'));
                        if (self.providerDetail.CallEndAMPM == "PM" && hr1 != 12) {     //checks if PM, adds 12 hours
                            hr1 = +hr1 + 12;
                        }
                        else if (self.providerDetail.CallStartAMPM == "AM" && hr1 == 12) { // BUGFIX - 13633
                            hr1 = "0";
                        }
                        timeValEnd = timeValEnd.substring(timeValEnd.indexOf(':') + 1, timeValEnd.length)
                        var minEnd = timeValEnd.substring(0, timeValEnd.length);
                        var dateTimeEnd = dateVal.setHours(hr1, minEnd);

                        self.callerDetails.CallEndTime = $filter('formatDate')(dateTimeEnd, 'MM/DD/YYYY hh:mm A');
                    }

                    self.callerDetails.ProviderID = self.providerDetail.ProviderBy;
                    self.callerDetails.ModifiedOn = new Date();

                    self.callerDetails.Comments = prepareXenCommentHistory(self.callerDetails.preComments, self.callerDetails.newComment, self.cmtReporter);

                    if (self.callerDetails.CallCenterHeaderID != 0)
                        callerInformationService.update(self.callerDetails).then(saveCallerInfoSuccessMethod, errorMethod, notifyMethod);
                    else
                        callerInformationService.add(self.callerDetails).then(saveCallerInfoSuccessMethod, errorMethod, notifyMethod);
                }
            };

            var saveClientSuccessMethod = function (resp) {
                //Save Call Center details
                if ((resp && resp.data && resp.data.ID) || (self.clientDetails.ContactID != 0 && resp && resp.data && resp.data.ResultCode == 0)) {
                    var clientID;
                    if (self.clientDetails.ContactID != 0)       //if update
                        clientID = self.clientDetails.ContactID;
                    else {
                        clientID = resp.data.ID;
                        self.clientDetails.ContactID = clientID;
                    }
                    if (!self.callerDetails.IsLinkedToContact) {
                        //Save Client Phone details
                        if (formService.isDirty(self.callerInformationForm.phoneForm.$name)) {
                            var taskArray = [];
                            angular.forEach(self.Phones, function (phone) {
                                phone.ModifiedBy = self.providerDetail.ProviderBy;
                                phone.ModifiedOn = new Date();
                                phone.ContactID = clientID;
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
                            $q.serial(taskArray);
                        }

                        //Save Address details
                        if (formService.isDirty(self.callerInformationForm.AddressForm.$name)) {
                            var model = $scope.Addresses[0];
                            model.ContactID = clientID;
                            //Adding then to check it throws message if save fails
                            contactAddressService.addUpdate(model).then(addressSaveSuccess, errorMethod, notifyMethod);
                        }
                        //Save Additional Demography details(Marital status)
                        if (formService.isDirty(self.callerInformationForm.additionalDemoForm.$name)) {
                            self.additionalDemoDetails.ContactID = clientID;
                            //Adding then to check it throws message if save fails
                            if (self.additionalDemoDetails.AdditionalDemographicID && self.additionalDemoDetails.AdditionalDemographicID > 0) {
                                if (!self.additionalDemoDetails.MRN) {
                                    self.additionalDemoDetails.GenerateMRN = false;
                                }
                                additionalDemographyService.updateAdditionalDemographic(self.additionalDemoDetails).then(additionalDemoSaveSuccess, errorMethod, notifyMethod);
                            }
                            else {
                                self.additionalDemoDetails.AdditionalDemographicID = 0;
                                self.additionalDemoDetails.ModifiedOn = new Date();
                                self.additionalDemoDetails.GenerateMRN = false;
                                additionalDemographyService.addAdditionalDemographic(self.additionalDemoDetails).then(additionalDemoSaveSuccess, errorMethod, notifyMethod);
                            }
                        }
                    }

                    if (self.callerDetails.IsCallerClientSame || (!formService.isDirty(self.callerDetailsForm.callerForm.$name) && !formService.isDirty(self.callerInformationForm.clientForm.$name) && self.callerInformation.ContactID)) {
                        var response = {
                        };
                        response.data = {
                        };
                        response.data.ResultCode = 0;
                        response.data.ID = clientID;
                        saveCallerSuccessMethod(response);
                    }
                    else if (formService.isDirty(self.callerDetailsForm.callerForm.$name) || formService.isDirty(self.callerInformationForm.clientForm.$name) || formService.isDirty(self.clientPhoneForm.$name)) {
                        //Saves caller as contact
                        self.callerInformation.ModifiedBy = self.providerDetail.ProviderBy;
                        if (self.callerInformation.FirstName.length == 0) {
                            self.callerInformation.FirstName = anonymousText;
                        }
                        if (self.callerInformation.LastName.length == 0) {
                            self.callerInformation.LastName = anonymousText;
                        }
                        self.callerInformation.ModifiedOn = new Date();
                        self.callerInformation.DOB = $filter('formatDate')(self.clientDetails.DOB);
                        if (self.callerInformation.ContactID && self.callerInformation.ContactID != 0)
                            registrationService.update(self.callerInformation).then(saveCallerSuccessMethod, errorMethod, notifyMethod);
                        else
                            registrationService.add(self.callerInformation).then(saveCallerSuccessMethod, errorMethod, notifyMethod);
                    }
                }
            };

            var addressSaveSuccess = function (resp) {
                //We need not to do anything
            };

            var additionalDemoSaveSuccess = function (resp) {
                //We need not to do anything
            }

            var saveCallerInfoSuccessMethod = function (resp) {
                if ((resp && resp.ID) || (self.callerDetails.CallCenterHeaderID != 0 && resp && resp.ResultCode == 0)) {
                    resetForm();
                    var isAdd = (self.callerDetails.CallCenterHeaderID && self.callerDetails.CallCenterHeaderID != 0) ? false : true;
                    var callerID;
                    if (self.callerDetails.CallCenterHeaderID != 0)
                        callCenterHeaderID = self.callerDetails.CallCenterHeaderID;
                    else
                        self.callerDetails.CallCenterHeaderID = callCenterHeaderID = resp.ID;
                    var msg = isAdd ? 'saved' : 'updated';
                    alertService.success('Caller Information ' + msg + ' successfully.');

                    //save workflow Header details.
                    //$rootScope.$broadcast('onWorkflowHeaderSave', { workflowDataKey: $state.current.data.workflowDataKey, workflowHeaderID: callCenterHeaderID });
                    WorkflowHeaderService.AddWorkflowHeader({ WorkflowDataKey: $state.current.data.workflowDataKey, RecordHeaderID: callCenterHeaderID, ContactID: self.clientDetails.ContactID });
                    $rootScope.$broadcast('isCallEnded', false);
                    get(callCenterHeaderID);
                    if (self.IsNext) {
                        self.next();
                    }
                    else if ($state.current.name.toLowerCase().indexOf('init') >= 0) {
                        angular.extend($stateParams, {
                            ContactID: self.clientDetails.ContactID,
                            CallCenterHeaderID: callCenterHeaderID,
                            ReadOnly: (self.callerDetails.CallStatusID === 1) ? "view" : "edit"
                        });

                        $state.transitionTo('callcenter.crisisline.callerinformation', $stateParams);
                    }
                    else {
                        //Set the Form to ReadOnly if the Call Status is Complete
                        if ($stateParams.ContactID !== self.callerDetails.ContactID) {
                            $stateParams.ContactID = self.callerDetails.ContactID;
                            $state.transitionTo('callcenter.crisisline.callerinformation', $stateParams);
                        }
                        setFormToReadOnlyForComplete();
                    }
                }
                else if (self.IsNext)
                    self.next();
            };

            var setFormToReadOnlyForComplete = function () {
                if (self.callerDetails.CallStatusID === 1) {
                    angular.extend($stateParams, {
                        ReadOnly: "view"
                    });

                    $state.transitionTo($state.current.name, $stateParams);
                }
            }

            var successMethod = function (data) {
                if (data && data.DataItems && data.DataItems.length > 0) {
                    self.callerDetails = data.DataItems[0];
                    savedContactStatusID = self.callerDetails.CallStatusID;
                    if (self.callerDetails.CallStatusID == CALL_STATUS.VOID)
                        $scope.isVoided = true;
                    else
                        $scope.isVoided = false;

                    $scope.isFollowUpRecord = self.callerDetails.ParentCallCenterHeaderID || cacheService.get('FollowUp') ? true : false;
                    if ($scope.isFollowUpRecord) {
                        callerInformationService.get(self.callerDetails.ParentCallCenterHeaderID).then(function (followUpResponse) {
                            if (hasData(followUpResponse)) {
                                var callerData = followUpResponse.DataItems[0];
                                self.callerDetails.ProgramUnitID = callerData.ProgramUnitID;
                                self.callerDetails.CountyID = callerData.CountyID;
                                self.callerDetails.SuicideHomicideID = callerData.SuicideHomicideID;
                                self.callerDetails.CallPriorityID = callerData.CallPriorityID;
                                self.callerDetails.ReasonCalled = callerData.ReasonCalled;
                                self.callerDetails.ContactTypeID = callerData.ContactTypeID;
                                self.callerDetails.IsLinkedToContact = callerData.IsLinkedToContact;
                                if (self.callerDetails.IsLinkedToContact && self.callerDetails.IsLinkedToContact === true) {
                                    $scope.LinkedContactID = contactID;
                                }
                                registrationService.get(callerData.CallerContactID).then(callerSuccess, errorMethod, notifyMethod);
                                //Gets client details
                                registrationService.get(callerData.ClientContactID).then(contactSuccess, errorMethod, notifyMethod);
                            }
                        });
                        $rootScope.$broadcast('onFollowUpCall', { data: true });
                    }
                    if (cacheService.get('FollowUp')) {
                        self.callerDetails.ReasonCalled = "";
                        self.callerDetails.FollowUpRequired = false;
                    }

                    if (self.callerDetails.Comments) {
                        self.callerDetails.preComments = parseJSON(self.callerDetails.Comments);
                    }
                    if (self.callerDetails.CallStatusID != CALL_STATUS.NEEDS_REVIEW) {
                        $scope.isneedReviewAgain = true;
                    }
                    if (self.callerDetails.CallStatusID == CALL_STATUS.NEEDS_REVIEW) {
                        $scope.isneedReviewAgain = false;
                    }
                    var contactID = self.callerDetails.ClientContactID;
                    var callerID = self.callerDetails.CallerContactID;
                    self.callerInformation.ContactID = contactID;
                    self.pageSecurity = contactID;
                    if (self.callerDetails.IsLinkedToContact && self.callerDetails.IsLinkedToContact === true && !$scope.isFollowUpRecord) {
                        $scope.LinkedContactID = contactID;
                    }
                    else
                        $scope.LinkedContactID = undefined;
                    $scope.DisableLinkedDetails = $scope.isFollowUpRecord ? true : self.callerDetails.IsLinkedToContact;
                    $scope.isLinkToExistingContact = $scope.isFollowUpRecord ? true : self.callerDetails.IsLinkedToContact;
                    getHeader();
                    if (!$scope.isFollowUpRecord) {
                        //Gets caller details
                        registrationService.get(callerID).then(callerSuccess, errorMethod, notifyMethod);
                        //Gets client details
                        registrationService.get(contactID).then(contactSuccess, errorMethod, notifyMethod);
                    }
                    setFormStatus({
                        valid: true
                    });
                } else {
                    alertService.error('Unable to get caller information!');
                    resetForm();
                    setFormStatus({
                        valid: false
                    });
                };
            };

            getHeader = function () {
                if (self.callerDetails.CallStartTime) {
                    self.providerDetail.ProviderStartTime = searchString(self.callerDetails.CallStartTime, adjustedTime) ? "" : $filter('formatDate')(self.callerDetails.CallStartTime, 'hh:mm');
                    self.providerDetail.CallStartAMPM = searchString(self.callerDetails.CallStartTime, defaultTime) ? "AM" : $filter('toStandardTimeAMPM')(dateTimeValidatorService.getCurrentMeridian(moment(self.callerDetails.CallStartTime).toDate()));
                    self.providerDetail.ProviderDate = $filter('formatDate')(self.callerDetails.CallStartTime, 'MM/DD/YYYY');
                }
                if (self.callerDetails.CallEndTime) {
                    self.providerDetail.ProviderEndTime = searchString(self.callerDetails.CallEndTime, adjustedTime) ? "" : $filter('formatDate')(self.callerDetails.CallEndTime, 'hh:mm');
                    self.providerDetail.CallEndAMPM = searchString(self.callerDetails.CallEndTime, defaultTime) ? "AM" : $filter('toStandardTimeAMPM')(dateTimeValidatorService.getCurrentMeridian(moment(self.callerDetails.CallEndTime).toDate()));
                }
                self.providerDetail.ProviderBy = self.callerDetails.ProviderID;
            }

            var callerSuccess = function (data) {
                if (data && data.DataItems && data.DataItems.length > 0) {
                    //Gets contact phone details
                    self.callerInformation = data.DataItems[0];
                    if (!(self.callerInformation.FirstName === anonymousText && self.callerInformation.LastName === anonymousText)) {
                        setFormStatus({
                            valid: true, isNotAnonymous: true
                        });
                    }
                    else {
                        setFormStatus({
                            valid: true, isNotAnonymous: false
                        });
                    }
                    if ($scope.$parent)
                        $scope.$parent.$parent['autoFocus'] = true;
                    contactPhoneService.get(self.callerInformation.ContactID, self.callerInformation.ContactTypeID).then(phoneSuccess, errorMethod, notifyMethod);
                    resetForm();
                }
            };

            var contactSuccess = function (data) {
                var deferred = $q.defer();
                if (data && data.DataItems && data.DataItems.length > 0) {
                    var promises = [];

                    //self.clientDetails = data.DataItems[0];
                    // if duplicate contact(16971) disabled quick registration and contact type become existing.
                    if (data.DataItems[0].MRN && self.callerDetails.IsLinkedToContact) {
                        $scope.isCallCenterConvertToRegistration = true;
                        self.callerDetails.ContactTypeID = CONTACT_TYPE.Existing;
                    }

                    //Get Address
                    promises.push(contactAddressService.get(data.DataItems[0].ContactID, data.DataItems[0].ContactTypeID).then(addressGetSuccess, errorMethod, notifyMethod));
                    if (self.callerDetails.IsLinkedToContact || !self.callerDetails.IsCallerClientSame) {
                        promises.push(contactPhoneService.get(data.DataItems[0].ContactID, data.DataItems[0].ContactTypeID).then(clientPhoneSuccess, errorMethod, notifyMethod));
                        //self.callerDetails.ContactTypeID = CONTACT_TYPE.Existing;
                    }
                    //Get Additional Demo
                    promises.push(additionalDemographyService.getAdditionalDemographic(data.DataItems[0].ContactID).then(additionalDemoGetSuccess, errorMethod, notifyMethod));
                    if (data.DataItems[0].DOB)
                        data.DataItems[0].DOB = $filter('formatDate')(data.DataItems[0].DOB, 'MM/DD/YYYY');
                    $rootScope.$broadcast('updateRegistrationData', {
                        Data: data.DataItems[0]
                    });
                    if (data.DataItems[0].SSN && data.DataItems[0].SSN.length > 0 && data.DataItems[0].SSN.length < 9) {
                        promises.push(contactSSNService.refreshSSN(data.DataItems[0].ContactID, data.DataItems[0]));
                    }
                    //if additional demographics are added above, then MRN number is generated which is causing "convert 
                    //to registration" to be disabled since it assumes it is an existing contact, so re enable link.
                    //if (!self.callerDetails.IsLinkedToContact )
                    //$rootScope.$broadcast('quickRegMRN', { Data: false });
                    resetForm();
                    $q.all(promises).then(function () {
                        self.clientDetails = data.DataItems[0];
                        deferred.resolve();
                    });
                }
                return deferred.promise;
            };

            var phoneSuccess = function (data) {
                if (data && data.DataItems && data.DataItems.length > 0) {
                    self.CallerPhones = getPrimaryOrLatestData(data.DataItems);
                    if (self.callerDetails.IsCallerClientSame) {
                        self.Phones = getPrimaryOrLatestData(self.CallerPhones);
                    };
                    resetForm();
                }
            };

            var addressGetSuccess = function (data) {
                if (data && data.DataItems && data.DataItems.length > 0) {
                    $scope.Addresses = [];
                    $scope.Addresses.push(getPrimaryOrLatestData(data.DataItems)[0]);
                    $scope.Addresses[0].IsGateCode = true;
                    $scope.Addresses[0].IsComplexName = true;
                }
                else {
                    $scope.Addresses = [{
                        AddressTypeID: ADDRESS_TYPE.ResidenceHome,
                        Line1: '',
                        Line2: '',
                        City: '',
                        StateProvince: null,
                        County: null,
                        Zip: '',
                        MailPermissionID: '',
                        IsPrimary: true,
                        IsGateCode: true,
                        IsComplexName: true
                    }];
                }
            };

            var additionalDemoGetSuccess = function (data) {
                if (data && data.DataItems && data.DataItems.length > 0) {
                    self.additionalDemoDetails = data.DataItems[0];
                    resetForm();
                }
            };

            var clientPhoneSuccess = function (data) {
                if (data && data.DataItems && data.DataItems.length > 0) {
                    self.Phones = [];
                    self.Phones.push(getPrimaryOrLatestData(data.DataItems)[0]);
                    resetForm();
                }
            };

            var convertToRegistrationPhoneSuccess = function (data) {
                if (hasData(data)) {
                    self.Phones = [];
                    self.Phones.push(data.DataItems[0]);
                    resetPhone(true);
                }
                else {
                    self.Phones[0].Number = null;
                    self.Phones[0].PhoneTypeID = null;
                    self.Phones[0].PhonePermissionID = null;
                }
            };



            var failureMethod = function (errorStatus) {
                alertService.error('Unable to get caller information: ' + errorStatus + '.');
                setFormStatus({
                    valid: false
                });
            };

            var errorMethod = function (err) {
                alertService.error('OOPs something went wrong.');
            };

            var notifyMethod = function (notify) {
                //TODO: if needed then implement for notifications
            };

            $scope.setShortcutKey = function (enterKeyStop, stopNext, saveOnEnter, stopSave) {
                $scope.enterKeyStop = enterKeyStop;
                $scope.stopNext = stopNext;
                $scope.saveOnEnter = saveOnEnter;
                $scope.stopSave = stopSave;
            };

            $scope.setFocus = function (autoFocus) {
                $rootScope.setFocusToGrid('contactsTable');
                if (autoFocus) {
                    $('#txtClientSearch').focus();
                    $scope.setShortcutKey(true, false, false, true);
                }
            };

            $scope.setCancelFunction = function (cancelFunction) {
                if (cancelFunction)
                    $scope.CancelCallerInformation = cancelFunction;
            };

            $scope.onContactSelect = function (contactID) {
                isSearched = true;
                $scope.LinkedContactID = contactID;
                $scope.GetLinkedContact(true);
                $scope.$parent.$parent['autoFocus'] = true;
            }

            $scope.GetLinkedContact = function (isContactSearched) {
                if (isContactSearched || (!self.callerDetails.IsLinkedToContact && $scope.LinkedContactID)) {
                    $scope.isNotExisting = false;
                    $scope.DisableLinkedDetails = true;
                    self.callerDetails.IsLinkedToContact = true;
                    registrationService.get($scope.LinkedContactID).then(linkedContactSuccess, errorMethod, notifyMethod);
                }
                else {
                    $scope.DisableLinkedDetails = false;
                    self.clientDetails.ContactID = 0;
                    initContactDetails();
                    self.callerDetails.IsCallerClientSame = false;
                    initCallerInformationDetails();
                    self.callerDetails.ContactTypeID = CONTACT_TYPE.New;
                }
            }

            var linkedContactSuccess = function (data) {
                if (hasData(data)) {
                    var contactDetails = data.DataItems[0];
                    self.clientDetails.ContactID = contactDetails.ContactID;
                    self.clientDetails.ContactTypeID = contactDetails.ContactTypeID;
                    self.clientDetails.FirstName = contactDetails.FirstName;
                    self.clientDetails.LastName = contactDetails.LastName;
                    self.clientDetails.ModifiedOn = new Date();
                    self.clientDetails.GenderID = contactDetails.GenderID;
                    //Fill the Full SSN from the DB Else assign the value from the search record
                    if (contactDetails.SSN && contactDetails.SSN.length > 0 && contactDetails.SSN.length < 9) {
                        contactSSNService.refreshSSN($scope.LinkedContactID, self.clientDetails);
                    }
                    else {
                        self.clientDetails.SSN = contactDetails.SSN;
                    }
                    // Get Phone
                    contactPhoneService.get($scope.LinkedContactID, self.clientDetails.ContactTypeID).then(linkedClientPhoneSuccess, errorMethod, notifyMethod);

                    //Get Address
                    contactAddressService.get($scope.LinkedContactID, self.clientDetails.ContactTypeID).then(addressGetSuccess, errorMethod, notifyMethod);

                    //Get Additional Demo
                    additionalDemographyService.getAdditionalDemographic(self.clientDetails.ContactID).then(linkedAdditionalDemoGetSuccess, errorMethod, notifyMethod);
                    if (contactDetails.DOB) {
                        contactDetails.DOB = $filter('formatDate')(contactDetails.DOB, 'MM/DD/YYYY');
                        self.clientDetails.DOB = contactDetails.DOB;
                    }
                    $rootScope.$broadcast('updateRegistrationData', {
                        Data: contactDetails
                    });
                    self.callerDetails.ContactTypeID = CONTACT_TYPE.Existing;
                }
            };

            var linkedAdditionalDemoGetSuccess = function (data) {
                if (data && data.DataItems && data.DataItems.length > 0) {
                    self.additionalDemoDetails = data.DataItems[0];
                }
            };

            var linkedClientPhoneSuccess = function (data) {
                self.Phones = [];
                self.Phones.push(objPhone());
                if (hasData(data)) {
                    self.Phones[0] = getPrimaryOrLatestData(data.DataItems)[0];
                }
                else {
                    self.Phones[0].Number = null;
                    self.Phones[0].PhoneTypeID = null;
                    self.Phones[0].PhonePermissionID = null;
                }
                if (self.callerDetails.IsCallerClientSame)
                    $scope.SetCallerAsClient(true);
            };

            $scope.changeCallStatus = function (status) {
                if (!cacheService.get('IsManagerAccess') && cacheService.get('IsCreatorAccess') && !($stateParams.ReadOnly == 'view') && (status === CALL_STATUS.COMPLETE || status === CALL_STATUS.NEEDS_REVIEW)) {
                    alertService.error('You are not authorized to set call status to any of the following statuses: COMPLETE, or NEEDS REVIEW.');
                    self.callerDetails.CallStatusID = savedContactStatusID;
                    return;
                }
                $stateParams.CallCenterHeaderID && status === CALL_STATUS.COMPLETE && serviceRecordingService.getServiceRecording($stateParams.CallCenterHeaderID, SERVICE_RECORDING_SOURCE.CallCenter).then(function (data) {
                    var isCallEndDate = (data && data.DataItems.length > 0 && data.DataItems[0].ServiceEndDate) ? true : false;
                    if (!isCallEndDate) {
                        callCompeleteWarning();
                    }
                });
                if (!$stateParams.CallCenterHeaderID && status === CALL_STATUS.COMPLETE) {
                    callCompeleteWarning();
                }
            };

            var callCompeleteWarning = function () {
                alertService.error("Call must end prior to setting call status to complete");
                self.callerDetails.CallStatusID = savedContactStatusID;
            };

            init();
            $rootScope.$on('updateData', function (event, args) {
                if (self.callerInformationForm.clientForm) {
                    var isContactDirty = formService.isDirty(self.callerInformationForm.clientForm.$name);
                }
                self.clientDetails = args.Data;
                if (!isContactDirty) {
                    resetClient();
                }
                $rootScope.$broadcast('updateRegistrationData', {
                    Data: self.clientDetails
                });
                contactPhoneService.get(self.clientDetails.ContactID, self.clientDetails.ContactTypeID).then(convertToRegistrationPhoneSuccess, errorMethod, notifyMethod);
            });

            $rootScope.$on('callCenterEndCall', function (event, callEndDateTime) {
                if (callEndDateTime != null) {
                    self.providerDetail.ProviderEndTime = $filter('formatDate')(callEndDateTime, 'hh:mm');
                    self.providerDetail.CallEndAMPM = $filter('toStandardTimeAMPM')(dateTimeValidatorService.getCurrentMeridian(moment(callEndDateTime).toDate()));
                }
            });

            //duplicat contact detection callback 
            $scope.callBackDuplicate = function (contactID) {
                $scope.LinkedContactID = contactID;
                self.callerDetails.IsLinkedToContact = true;
                self.clientDetails.ContactID = contactID;
                registrationService.get(contactID).then(function (data) {
                    contactSuccess(data).then(function () {
                        $timeout(function () {
                            formService.initForm(true)
                            formService.initForm(true, $scope.ctrl.callerInformationForm.clientForm.$name);
                            formService.initForm(true, self.clientPhoneForm.$name);
                        });
                    });
                }, errorMethod, notifyMethod);
            };

        }]);
}());
