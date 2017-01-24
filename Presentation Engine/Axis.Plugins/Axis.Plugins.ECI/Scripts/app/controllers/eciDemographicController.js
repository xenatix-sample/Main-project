angular.module('xenatixApp')
    .controller('eciDemographicController', [
        '$http', '$scope', '$rootScope', '$state', '$stateParams', '$timeout', 'alertService', '$filter', 'lookupService', 'formService', 'eciDemographicService', 'contactEmailService', 'contactPhoneService', 'contactAddressService', '$q', 'globalObjectsService', 'registrationService', 'contactAliasService', '$controller', 'contactSSNService', '$injector', 'auditService', 'applicationSettingService',
        function ($http, $scope, $rootScope, $state, $stateParams, $timeout, alertService, $filter, lookupService, formService, eciDemographicService, contactEmailService, contactPhoneService, contactAddressService, $q, globalObjectsService, registrationService, contactAliasService, $controller, contactSSNService, $injector, auditService, applicationSettingService) {

            $controller('baseContactController', { $scope: $scope });
            var declinedSsnStatus = [1, 2, 3];
            $scope.permissionKey = $state.current.data.permissionKey;
            var duplicateContactsTable = $("#duplicateContactsTable");
            var isDuplicateCheckRequired = true;
            var isContactAliasDirty = false;
            var action = null;
            var isClientIdentifierDirty = false;
            var demographics;
            $scope.effectiveDate = 'EffectiveDate';
            $scope.expirationDate = 'ExpirationDate';
            $scope.screenID = SCREEN.Demography;
            var uniqueID;
            $scope.presentingProblemEffectiveDate = 'presentingProblemEffectiveDate';
            $scope.presentingProblemExpirationDate = 'presentingProblemExpirationDate';
            $scope.changeLogSetting = applicationSettingService.getSettingByName('ChangeLog');
            var eciAgeRestriction = applicationSettingService.getSettingByName('ECIAgeRestriction');
            var eciMaxAgeInMonths = applicationSettingService.getSettingByName('ECIMaxAgeInMonths');
            //$scope.adultAgeValue = 18;
            $scope.initializeBootstrapTable = function () {

                $scope.tableoptions = {
                    pagination: true,
                    pageSize: 10,
                    pageList: [10, 25, 50, 100],
                    search: false,
                    showColumns: true,
                    data: [],
                    undefinedText: '',

                    columns: [
                        {
                            field: 'FirstName',
                            title: 'First Name'
                        },
                        {
                            field: 'LastName',
                            title: 'Last Name'
                        },
                        {
                            field: 'Middle',
                            title: 'Middle Name'
                        },
                         {
                             field: 'GenderText',
                             title: 'Gender'
                         },
                        {
                            field: 'DOB',
                            title: 'DOB',
                            formatter: function (value, row, index) {
                                if (value) {
                                    var formattedDate = $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY');
                                    return formattedDate;
                                } else
                                    return '';
                            }
                        },
                        {
                            field: 'SSN',
                            title: 'SSN',
                            formatter: function (value, row, index) {
                                var formattedSNN = $filter('toMaskSSN')(value);
                                return formattedSNN;
                            }
                        },
                        {
                            field: 'ContactID',
                            title: 'Actions',
                            formatter: function (value, row, index) {
                                return '<a href="javascript:void(0)" data-default-action data-ng-click="populateContact(' + value + ')" alt="View Contact" security permission-key="ECI-Registration-Demographics" permission="update" title="Edit"><i class="fa fa-pencil fa-fw padding-left-small padding-right-small" /></a>';
                            }
                        }
                    ]
                };
            };

            var resetContactDemographics = function () {
                if ($scope.contactInformationForm)
                    $rootScope.formReset($scope.contactInformationForm, $scope.contactInformationForm.$name);

                if ($scope.additionalContactForm)
                    $rootScope.formReset($scope.additionalContactForm, $scope.additionalContactForm.$name);

                if ($scope.ctrl.contactForm)
                    $rootScope.formReset($scope.ctrl.contactForm, $scope.ctrl.contactForm.$name);

                if ($scope.presentingProblemForm)
                    $rootScope.formReset($scope.presentingProblemForm, $scope.presentingProblemForm.$name);

                if ($scope.ctrl.contactForm.contactDemographicsForm)
                    $rootScope.formReset($scope.ctrl.contactForm.contactDemographicsForm, $scope.ctrl.contactForm.contactDemographicsForm.$name);

            }

            var resetPhone = function () {
                if ($scope.ctrl.contactForm.registrationPhoneForm)
                    $rootScope.formReset($scope.ctrl.contactForm.registrationPhoneForm, $scope.ctrl.contactForm.registrationPhoneForm.$name);
            }

            var resetEmail = function () {
                if ($scope.ctrl.contactForm.registrationEmailForm)
                    $rootScope.formReset($scope.ctrl.contactForm.registrationEmailForm, $scope.ctrl.contactForm.registrationEmailForm.$name);
            }
            var resetAddress = function () {
                if ($scope.ctrl.contactForm.registrationAddressForm)
                    $rootScope.formReset($scope.ctrl.contactForm.registrationAddressForm, $scope.ctrl.contactForm.registrationAddressForm.$name);
            }

            var resetContactAlias = function () {

                if ($scope.ContactAliasForm)
                    $rootScope.formReset($scope.ContactAliasForm, $scope.ContactAliasForm.$name);
            }

            var resetcontactMethodForm = function () {
                if ($scope.contactMethodForm)
                    $rootScope.formReset($scope.contactMethodForm, $scope.contactMethodForm.$name);
            }

            var resetForm = function () {
                resetContactDemographics();
                resetcontactMethodForm();
                resetAddress();
                resetPhone();
                resetEmail();
                resetContactAlias();
                if ($scope.ctrl.contactForm) {
                    angular.forEach($scope.ctrl.contactForm.modifiedModels, function (obj) {
                        obj.$setPristine();
                    });
                }
                isContactAliasDirty = false;
                isClientIdentifierDirty = false;
            };
            var checkFormStatus = function (state) {
                var stateDetail = { stateName: "eciregistration.demographics", validationState: state.valid ? 'valid' : 'invalid' };
                $rootScope.$broadcast('rightNavigationECIRegistrationHandler', stateDetail);
            };

            $scope.isPreferredContact = function () {
                switch ($scope.newDemography.ContactMethodID) {
                    case PREFERRED_CONTACT_METHOD.Mail:
                        $scope.AddressAccessCode = $scope.ADDRESS_ACCESS.ConditionalRequired | $scope.ADDRESS_ACCESS.Line1 | $scope.ADDRESS_ACCESS.City | $scope.ADDRESS_ACCESS.State | $scope.ADDRESS_ACCESS.PostalCode;
                        $scope.PhoneAccessCode = 0;
                        $scope.EmailAccessCode = 0;
                        break;
                    case PREFERRED_CONTACT_METHOD.Phone:
                        $scope.PhoneAccessCode = $scope.PHONE_ACCESS.ConditionalRequired | $scope.PHONE_ACCESS.Number;
                        $scope.AddressAccessCode = 0;
                        $scope.EmailAccessCode = 0;
                        break;
                    case PREFERRED_CONTACT_METHOD.Email:
                        $scope.EmailAccessCode = $scope.EMAIL_ACCESS.ConditionalRequired | $scope.EMAIL_ACCESS.Email;
                        $scope.AddressAccessCode = 0;
                        $scope.PhoneAccessCode = 0;
                        break;
                    default:
                        $scope.AddressAccessCode = 0;
                        $scope.PhoneAccessCode = 0;
                        $scope.EmailAccessCode = 0;
                }
            };

            $scope.initVariables = function () {
                $scope.isLoading = true;
                $scope.opened = false;
                $scope.dobName = 'dateOfBirth';
                $scope.dateOptions = {
                    formatYear: 'yy',
                    startingDay: 1,
                    showWeeks: false
                };
                $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'MM/dd/yyyy'];
                $scope.format = $scope.formats[4];
                $scope.endDate = new Date();
                $scope.startDate = $filter('calculate120years')();
                $scope.ServerValidationErrors = []; // initialization toward sample use of server validation
                $scope.controlsVisible = true;
                $scope.undefinedValue = 'undefined';

                $scope.zeroValue = "0";

                $scope.femaleOptions = '2';
                $scope.emailPattern = /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/i;
            };

            $scope.init = function () {
                $scope.initializeBootstrapTable();
                $scope.$parent['autoFocus'] = true; //for Focus
                //hide the program field if you are in a registration process
                var currentState = $state.current.name;
                if (currentState.indexOf('registration') === 0 || currentState.indexOf('eciregistration') === 0) {
                    $scope.isRegistrationState = true;
                } else {
                    $scope.isRegistrationState = false;
                }
                $scope.initSharedItems();
                $scope.$parent.autoFocus = true;
                $scope.initVariables();

                $scope.contactID = $stateParams.ContactID;
                if ($stateParams.OtherContactID != undefined && $stateParams.OtherContactID != null && $stateParams.OtherContactID != "")
                    $scope.contactID = $stateParams.OtherContactID;
                $scope.ClientTypeID = $stateParams.ClientTypeID;
                $scope.ProgramClientRequiredIdentifiers = lookupService.getLookupsByType("ProgramClientIdentifier");
                $scope.initDemographics();
                initContactAliases();

                if (($scope.contactID !== 0) && ($scope.contactID != null) && ($scope.contactID !== $scope.undefinedValue)) {
                    $scope.get($scope.contactID);
                }
                else {
                    $scope.permissionId = $scope.newDemography.ContactID = 0;
                    checkFormStatus({ valid: false });
                }

                $scope.formatContactData();
                if ($state.current.name.toLowerCase().indexOf('patientprofile') >= 0) {
                    $scope.controlsVisible = false;
                    $scope.enterKeyStop = true;
                    $scope.stopNext = false;
                    $scope.saveOnEnter = true;
                }
                else {
                    $scope.controlsVisible = true;
                    $scope.enterKeyStop = false;
                    $scope.stopNext = false;
                    $scope.saveOnEnter = false;
                    $timeout(function () {
                        var nextState = angular.element("xen-workflow-action[data-state-name] > li.list-group-item.active").parents('.work-flow').next().find("xen-workflow-action[data-state-name]");
                        $scope.controlsVisible = (nextState.length > 0);
                    });
                }

                $scope.isPatientProfileState = false;
                if ($state.current.name.indexOf('patientprofile') >= 0) {
                    $scope.isPatientProfileState = true;
                }

                $scope.formatAlternateIDs();

                $scope.ssnStatusFilter = function (ssnStatusItem) {
                				if ($scope.additionalContactForm && $scope.additionalContactForm.SSN) {
                								var isValidSSN = (
																												$scope.additionalContactForm.SSN.$valid && $scope.additionalContactForm.SSN.$viewValue != "" &&
																												$scope.additionalContactForm.SSN.$viewValue != undefined);
                        return !((isValidSSN) ? (ssnStatusItem.ID <= 3) : (ssnStatusItem.ID > 3));
                    }
                };

                //To display the Primary, Effective, Expiration Dates for Phone/Email
                $scope.ShowPrimaryCheckbox = true;
                $scope.ShowPrimaryEmailCheckbox = true;
                $scope.ShowPhoneExpirationDates = true;
                $scope.ShowEmailExpirationDates = true;
            };

            $scope.initSharedItems = function () {
                $scope.initEmails();
                $scope.initAddresses();
                $scope.initPhones();
            };

            $scope.resetDeceasedDate = function () {
                    $scope.newDemography.DeceasedDate = null;
                    $scope.newDemography.CauseOfDeath = null;
                    $scope.ctrl.contactForm.contactDemographicsForm.contactInformationForm["dateOfDeath"].$setValidity("lessThanDate", true);
                    $scope.ctrl.contactForm.contactDemographicsForm.contactInformationForm["dateOfDeath"].$setValidity("futureDate", true);
                    $scope.ctrl.contactForm.contactDemographicsForm.additionalContactForm["dateOfBirth"].$setValidity("greaterThanDate", true);
                    formService.initForm(true, $scope.contactInformationForm.$name);
            };

            $scope.initDemographics = function () {
                $scope.newDemography = {};
                if ($stateParams.ClientTypeID !== undefined && $stateParams.ClientTypeID !== null) {
                    $scope.newDemography.ClientTypeID = $stateParams.ClientTypeID;
                }

                $scope.initAddresses();
                $scope.initPhones(true);
                $scope.initEmails();
                $scope.initAlternateIDs();
                $scope.initcontactPresentingProblem();
            };

            $scope.initcontactPresentingProblem = function () {
                $scope.contactPresentingProblem = {
                    EffectiveDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
                    PresentingProblemTypeID: PRESENTING_PROBLEM_TYPE.ECI
                };
            };

            $scope.initEmails = function () {
                $scope.Emails = [{
                    Email: '',
                    EmailPermissionID: null,
                    EffectiveDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
                    ExpirationDate: null,
                    IsPrimary: true
                }];
            };

            $scope.initAddresses = function () {
                $scope.Addresses = [{
                    AddressTypeID: ADDRESS_TYPE.ResidenceHome,//Adress default to residence/home as per Bug 13914
                    Line1: '',
                    Line2: '',
                    City: '',
                    StateProvince: $scope.defaultStateProvinceID,
                    County: $scope.defaultCountyID,
                    Zip: '',
                    MailPermissionID: '',
                    IsPrimary: true,
                    IsGateCode: true,
                    IsComplexName: true,
                    IsEffectiveDate: true,
                    IsExpirationDate: true,
                    EffectiveDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
                    ExpirationDate: '',
                    IsAddressPermissions: true,
                    ShowPrimary: true
                }];
            };

            $scope.initPhones = function (isNew) {
                $scope.Phones = [{
                    Index: 0,
                    PhoneTypeID: null,
                    Number: '',
                    PhonePermissionID: null,
                    EffectiveDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
                    ExpirationDate: null,
                    IsPrimary: true
                }];
            };

            $scope.initAlternateIDs = function () {
                $scope.ClientAlternateIDs = [];
            };

            var objAlternateID = function () {
                var obj = {
                    Index: 0,
                    ContactClientIdentifierID: 0,
                    ClientIdentifierTypeID: null,
                    EffectiveDate: new Date(),
                    ExpirationDate: null,
                    AlternateID: '',
                    ExpirationReasonID: null,
                    IsRequired: false,
                    IsActive: true,
                    ShowPlusButton: true,
                    ShowMinusButton: true
                };
                return obj;
            }

            $scope.getLookupsByType = function (typeName) {
                return lookupService.getLookupsByType(typeName);
            };

            $scope.getCountiesByStateProvince = function (stateProvinceId) {
                return lookupService.getCountiesByStateProvince(stateProvinceId);
            };

            $scope.formatContactData = function () {
                if (typeof $scope.newDemography.MailPermission != $scope.undefinedValue && $scope.newDemography.MailPermission != null) {
                    $scope.newDemography.MailPermission = $scope.newDemography.MailPermission.toString();
                }
                else {
                    $scope.newDemography.MailPermission = $scope.zeroValue;
                }
            };

            $scope.$watch('newDemography.ClientTypeID', function () {
                $scope.formatAlternateIDs();
            });

            $scope.formatAlternateIDs = function () {
                var clientRequiredIdentifiers;
                if ($scope.ClientTypeID != $scope.undefinedValue && $scope.ClientTypeID != null) {
                    clientRequiredIdentifiers = $scope.ProgramClientRequiredIdentifiers.filter(function (obj) {
                        return obj.ProgramID == $scope.ClientTypeID;
                    });
                }
                if (clientRequiredIdentifiers != $scope.undefinedValue && clientRequiredIdentifiers != null) {
                    angular.forEach(clientRequiredIdentifiers, function (requiredIdentifier) {
                        var found = false;
                        angular.forEach($scope.ClientAlternateIDs, function (alternateIdentifier) {
                            if (requiredIdentifier.ClientIdentifierTypeID === alternateIdentifier.ClientIdentifierTypeID) {
                                found = true;
                                alternateIdentifier.IsRequired = true;

                                if (alternateIdentifier.EffectiveDate)
                                    alternateIdentifier.EffectiveDate = $filter('toMMDDYYYYDate')(alternateIdentifier.EffectiveDate, 'MM/DD/YYYY');
                                if (alternateIdentifier.ExpirationDate)
                                    alternateIdentifier.ExpirationDate = $filter('toMMDDYYYYDate')(alternateIdentifier.ExpirationDate, 'MM/DD/YYYY');
                            }
                        });
                        if (found == false) {
                            var objToPush = objAlternateID();
                            objToPush.ClientIdentifierTypeID = requiredIdentifier.ClientIdentifierTypeID;
                            objToPush.IsRequired = true;
                            $scope.ClientAlternateIDs.push(objToPush);
                        }
                    });
                }

                if ($scope.ClientAlternateIDs.length == 0)
                    $scope.ClientAlternateIDs.push(objAlternateID());

                //convert/format existing other type effect and expire dates to date types. this is required by date picker.
                if ($scope.ClientAlternateIDs && $scope.ClientAlternateIDs.length > 0) {
                    angular.forEach($scope.ClientAlternateIDs, function (alternateIdentifier) {
                        if (alternateIdentifier.EffectiveDate)
                            alternateIdentifier.EffectiveDate = $filter('toMMDDYYYYDate')(alternateIdentifier.EffectiveDate, 'MM/DD/YYYY');
                        if (alternateIdentifier.ExpirationDate)
                            alternateIdentifier.ExpirationDate = $filter('toMMDDYYYYDate')(alternateIdentifier.ExpirationDate, 'MM/DD/YYYY');
                    });
                }

                $scope.ClientAlternateIDs = setAddMinusButtons($scope.ClientAlternateIDs);
            }

            $scope.addNewAlternateID = function (index) {
                globalObjectsService.setViewContent();
                $scope.ClientAlternateIDs = $filter("filter")($scope.ClientAlternateIDs, function (obj) {
                    obj.ShowPlusButton = false;
                    return obj;
                });
                $("#clientAlternativeContainer").children('div:lt(' + (index + 1) + ')').find(":input , textarea, button").prop("disabled", false);
                $scope.ClientAlternateIDs.push(objAlternateID());
            }

            $scope.removeAlternateID = function (removeIndex, isLastRow) {
                globalObjectsService.setViewContent();
                $scope.ClientAlternateIDs = removeControl($scope.ClientAlternateIDs, removeIndex, 'ContactClientIdentifierID');
                $scope.ClientAlternateIDs = setAddMinusButtons($scope.ClientAlternateIDs);
                var newCollection = $filter('filter')($scope.ClientAlternateIDs, function (data) {
                    return data.IsActive === true;
                });
                $("#clientAlternativeContainer").children('div').eq(isLastRow ? (removeIndex - 1) : (newCollection.length - 1)).find(":input , textarea, button").prop("disabled", false);
            }

            var formatEffExpirationDates = function (items) {
                angular.forEach(items, function (item) {
                    item.EffectiveDate = item.EffectiveDate
                                                         ? $filter('toMMDDYYYYDate')(item.EffectiveDate, 'MM/DD/YYYY')
                                                         : null;
                    item.ExpirationDate = item.ExpirationDate
                                                    ? $filter('toMMDDYYYYDate')(item.ExpirationDate, 'MM/DD/YYYY')
                                                    : null;
                });
            };

            $scope.getPhones = function (contactID, contactTypeID) {
                contactPhoneService.get(contactID, contactTypeID).then(function (data) {
                    if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                        $scope.newDemography.Phones = data.DataItems;
                        formatEffExpirationDates($scope.newDemography.Phones);
                        $scope.Phones = getPrimaryOrLatestData($scope.newDemography.Phones);
                        if ($scope.Phones.length === 0) {
                            $scope.initPhones();
                        }
                    }
                }).finally(function () {
                    resetForm();
                });
            };

            $scope.getEmails = function (contactID, contactTypeID) {
                contactEmailService.get(contactID, contactTypeID).then(function (data) {
                    if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                        $scope.newDemography.Emails = data.DataItems;
                        formatEffExpirationDates($scope.newDemography.Emails);
                        $scope.Emails = getPrimaryOrLatestData($scope.newDemography.Emails);
                        if ($scope.Emails.length === 0) {
                            $scope.initEmails();
                        }

                    }
                }).finally(function () {
                    resetForm();
                });
            };

            $scope.getAddress = function (contactID, contactTypeID) {
                contactAddressService.get(contactID, contactTypeID).then(function (data) {
                    if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                        $scope.newDemography.Addresses = data.DataItems;
                        $scope.Addresses = getPrimaryOrLatestData($scope.newDemography.Addresses);
                        if ($scope.Addresses.length === 0) {
                            $scope.initAddresses();
                        } else {
                            $scope.Addresses[0].IsGateCode = true;
                            $scope.Addresses[0].IsComplexName = true;
                            $scope.Addresses[0].IsEffectiveDate = true;
                            $scope.Addresses[0].IsExpirationDate = true;
                            $scope.Addresses[0].EffectiveDate = $scope.Addresses[0].EffectiveDate ? $filter('toMMDDYYYYDate')($scope.Addresses[0].EffectiveDate, 'MM/DD/YYYY') : $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal');
                            $scope.baseEffectiveDate = $scope.Addresses[0].EffectiveDate;

                            $scope.Addresses[0].ExpirationDate = $scope.Addresses[0].ExpirationDate ? $filter('toMMDDYYYYDate')($scope.Addresses[0].ExpirationDate, 'MM/DD/YYYY') : '';
                            $scope.Addresses[0].IsAddressPermissions = true;
                            $scope.Addresses[0].ShowPrimary = true;
                        }
                    }
                }).finally(function () {
                    resetForm();
                });
            };
            var ageLimitInMonths = parseInt(eciMaxAgeInMonths.value);
            var ageLimit = 120;
            var getAgeError = function (ageLimitInMonths) {
                $scope.newDemography.Age = null;
                $scope.newDemography.DOB = null;
                var ageToShow = ageLimitInMonths ? ageLimitInMonths / 12 : ageLimit
                alertService.error("Age can't be greater than " + ageToShow + " years.");
            };


            $scope.calculateAge = function () {
                if ($scope.newDemography.DOB != null) {
                    var date = new Date($scope.newDemography.DOB);
                    if ($scope.newDemography.IsDeceased && $scope.newDemography.DeceasedDate != null) {
                        var deceasedDate = new Date($scope.newDemography.DeceasedDate);
                        if (deceasedDate < date) {
                            $('#lessthandoberror').removeClass('ng-hide');
                            $('#dateOfDeathtd').addClass('has-error');
                        }
                        else {
                            $('#lessthandoberror').addClass('ng-hide');
                            $('#dateOfDeathtd').removeClass('has-error');
                        }
                    }
                    if (date <= $scope.endDate) {
                        var isDatePastLimit = false;
                        var isDatePastLimitInMonths = false;
                        $scope.newDemography.Age = parseInt($filter('toAge')($scope.newDemography.DOB));
                        if ($scope.newDemography.IsDeceased && $scope.newDemography.DeceasedDate != null) {
                            isDatePastLimit = $filter('isDateMaxLimit')($scope.newDemography.DOB, ageLimit, $scope.newDemography.DeceasedDate);
                            isDatePastLimitInMonths = $filter('isDateMaxLimitInMonths')($scope.newDemography.DOB, ageLimitInMonths, $scope.newDemography.DeceasedDate);
                        }
                        else {
                            isDatePastLimit = $filter('isDateMaxLimit')($scope.newDemography.DOB, ageLimit);
                            isDatePastLimitInMonths = $filter('isDateMaxLimitInMonths')($scope.newDemography.DOB, ageLimitInMonths);
                        }
                        if (JSON.parse(eciAgeRestriction.value)) {
                            if (!$scope.newDemography.ContactID && isDatePastLimitInMonths) {
                                getAgeError(ageLimitInMonths);
                            }
                            else if (isDatePastLimit) {
                                getAgeError();
                            }

                        }
                        else if (!JSON.parse(eciAgeRestriction.value)) {
                            if (!$scope.newDemography.ContactID) {
                                if (isDatePastLimit) {
                                    getAgeError();
                                }
                                else if (isDatePastLimitInMonths) {
                                    alertService.warning("This contact is at least three years of age.");
                                }
                            }
                            else if (isDatePastLimit) {
                                getAgeError();
                            }
                        }
                        else {
                            if ($scope.newDemography.IsDeceased && $scope.newDemography.DeceasedDate != null) {
                                $scope.newDemography.Age = $filter('ageToShow')($scope.newDemography.DeceasedDate);
                            }
                            else {
                                $scope.newDemography.Age = $filter('ageToShow')($scope.newDemography.DOB);
                            }
                        }
                        $('#doberrortd').removeClass('has-error');
                        $('#doberror').addClass('ng-hide');
                    }
                    else {
                        $scope.newDemography.Age = null;
                        $('#doberror').removeClass('ng-hide');
                        $('#doberrortd').addClass('has-error');
                    }
                }
                else {
                    $scope.newDemography.Age = '';
                }
            };

            $scope.eciFilter = function (item) {
                return (item.Name.toLowerCase().indexOf('tkids') === 0);
            };

            $scope.verifyDuplicateContacts = function () {
                var deferred = $q.defer();
                if (isDuplicateCheckRequired == true && $scope.isLawLiaison == undefined) {
                    registrationService.verifyDuplicateContacts($scope.newDemography).then(function (data) {
                        return deferred.resolve(data);
                    }, function (errorStatus) {
                        alertService.error('Unable to connect to server');
                    });
                }
                else
                    deferred.resolve();

                return deferred.promise;
            };

            var bindDataModel = function (model, showCurrentUser) {
                var listToBind = model;
                angular.forEach(listToBind, function (contact) {
                    contact.DOB = contact.DOB ? $filter('toMMDDYYYYDate')(contact.DOB, 'MM/DD/YYYY') : "";
                    if (contact.GenderID > 0)
                        contact.GenderText = lookupService.getSelectedText('Gender', contact.GenderID)[0].Name;
                });
                return listToBind;
            };

            $scope.populateContact = function (contactID) {
                $scope.get(contactID).finally(function () {
                    $('#duplicateContactListModel').modal('hide');
                    $('#FirstName').focus();
                    $stateParams.ContactID = contactID;
                    $scope.setShortcutKey(false, false, false, false);
                })
            };

            $scope.continueWithRegistration = function () {
                $('#duplicateContactListModel').modal('hide');

                isDuplicateCheckRequired = false;
                $scope.save(action, true);
            };

            $scope.cancelModal = function () {
                $('#duplicateContactListModel').modal('hide');
            };

            $scope.onChangePresentingProblem = function () {
                if ($scope.ctrl.contactForm) {
                    $scope.ctrl.contactForm.modified = true;
                    $rootScope.setform(true);
                }
            };

            $scope.get = function (contactID) {
                $scope.isLoading = true;
                return eciDemographicService.get(contactID).then(function (data) {
                    demographics = data.DataItems[0];
                    $scope.permissionId = contactID;
                    $scope.getAddress(contactID, demographics.ContactTypeID);
                    $scope.getPhones(contactID, demographics.ContactTypeID);
                    $scope.getEmails(contactID, demographics.ContactTypeID);
                    getContactAliases(contactID);

                    isDuplicateCheckRequired = false;
                    $scope.ClientAlternateIDs = demographics.ClientAlternateIDs;
                    $scope.ClientTypeID = demographics.ClientTypeID;
                    $scope.formatAlternateIDs();
                    if (demographics.DOB)
                        demographics.DOB = $filter('toMMDDYYYYDate')(demographics.DOB, 'MM/DD/YYYY');
                    $rootScope.$broadcast('updateRegistrationData', { Data: demographics });
                    $scope.contactPresentingProblem = demographics.ContactPresentingProblem;
                    if ($scope.contactPresentingProblem && $scope.contactPresentingProblem.EffectiveDate)
                        $scope.contactPresentingProblem.EffectiveDate = $filter('toMMDDYYYYDate')($scope.contactPresentingProblem.EffectiveDate, 'MM/DD/YYYY');
                    if ($scope.contactPresentingProblem && $scope.contactPresentingProblem.ExpirationDate)
                        $scope.contactPresentingProblem.ExpirationDate = $filter('toMMDDYYYYDate')($scope.contactPresentingProblem.ExpirationDate, 'MM/DD/YYYY');

                    if ($scope.contactPresentingProblem && !$scope.contactPresentingProblem.PresentingProblemTypeID)
                        $scope.contactPresentingProblem.PresentingProblemTypeID = PRESENTING_PROBLEM_TYPE.ECI;

                    if (demographics.IsDeceased && demographics.DeceasedDate)
                        demographics.DeceasedDate = $filter('toMMDDYYYYDate')(demographics.DeceasedDate, 'MM/DD/YYYY');

                    if (demographics.SSN && demographics.SSN.length > 0 && demographics.SSN.length < 9) {
                        contactSSNService.refreshSSN(contactID, demographics).then(function () {
                            $scope.newDemography = demographics;
                            $scope.isPreferredContact();
                            $scope.formatContactData();
                            resetForm();
                            demographics = undefined;
                        });
                    }
                    else {
                        $scope.newDemography = demographics;
                        $scope.isPreferredContact();
                        $scope.formatContactData();
                        demographics = undefined;
                    }

                    checkFormStatus({ valid: true });
                },
                    function (errorStatus) {
                        $scope.isLoading = false;
                        checkFormStatus({ valid: false });
                        alertService.error('Unable to connect to server');
                    }).finally(function () {
                        $scope.isLoading = false;
                        resetForm();
                    });
            };

            var addUpdateDetails = function (contactID) {
                var deferred = $q.defer();
                var isDirtyPhone = formService.isDirty($scope.ctrl.contactForm.registrationPhoneForm.$name);
                var isDirtyEmail = formService.isDirty($scope.ctrl.contactForm.registrationEmailForm.$name);
                var isDirtyAddress = formService.isDirty($scope.ctrl.contactForm.registrationAddressForm.$name);
                var isDirtyContactAlias = formService.isDirty($scope.ContactAliasForm.$name);

                var promiseArray = [];
                // filter out blank address
                var addresses = $filter('filter')($scope.Addresses, function (item) {
                    if ((item.ContactAddressID != null && (item.ContactAddressID == 0 || item.ContactAddressID != 0)) || item.Line1 != '' || item.Line2 != '' || item.City != '' || (item.StateProvince != null && item.StateProvince != 0) || (item.County != null && item.County != 0) || item.Zip != '') {
                        return true;
                    }
                    else {
                        return false;
                    }
                });

                if (isDirtyAddress && addresses != undefined && addresses != null && addresses.length > 0) {
                    addresses[0].ContactID = contactID;
                    addresses[0].EffectiveDate = addresses[0].EffectiveDate ? $filter('formatDate')(addresses[0].EffectiveDate) : null;
                    addresses[0].ExpirationDate = addresses[0].ExpirationDate ? $filter('formatDate')(addresses[0].ExpirationDate) : null;
                    promiseArray.push(contactAddressService.addUpdate(addresses[0]));
                }

                // filter out blank email
                var emails = $filter('filter')($scope.Emails, function (item) {
                    if (item.Email != '' || (item.ContactEmailID != null && item.ContactEmailID != 0)) {
                        return true;
                    }
                    else {
                        return false;
                    }
                });
                if (isDirtyEmail && emails != undefined && emails != null && emails.length > 0) {
                    emails[0].ContactID = contactID;
                    emails[0].EffectiveDate = emails[0].EffectiveDate ? $filter('formatDate')(emails[0].EffectiveDate) : null;
                    emails[0].ExpirationDate = emails[0].ExpirationDate ? $filter('formatDate')(emails[0].ExpirationDate) : null;
                    promiseArray.push(contactEmailService.addUpdate(emails[0]));
                }

                // filter out blank phone
                var phones = $filter('filter')($scope.Phones, function (item) {
                    if (item.Number != '' || (item.ContactPhoneID != null && item.ContactPhoneID != 0)) {
                        return true;
                    }
                    else {
                        return false;
                    }
                });

                if (isDirtyPhone && phones != undefined && phones != null && phones.length > 0) {
                    phones[0].ContactID = contactID;
                    phones[0].EffectiveDate = phones[0].EffectiveDate ? $filter('formatDate')(phones[0].EffectiveDate) : null;
                    phones[0].ExpirationDate = phones[0].ExpirationDate ? $filter('formatDate')(phones[0].ExpirationDate) : null;
                    promiseArray.push(contactPhoneService.save(phones[0]));
                }

                // filter out blank contact alais
                var aliases = $filter('filter')($scope.ContactAliases, function (item) {
                    if (item.AliasFirstName != '' || item.AliasLastName != '') {
                        item.ModifiedOn = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY hh:mm:ss.SSS A');
                        return true;
                    }
                    else {
                        return false;
                    }
                });

                if ((isDirtyContactAlias || isContactAliasDirty) && aliases != undefined && aliases != null && aliases.length > 0) {
                    angular.forEach(aliases, function (alias) {
                        alias.ContactID = contactID;
                        alias.TransactionID = uniqueID;
                        alias.ScreenID = $scope.screenID;
                        if (alias.IsActive == true) {
                            if (alias.ContactAliasID == 0)
                                promiseArray.push(contactAliasService.addContactAlias(alias));
                            else
                                promiseArray.push(contactAliasService.updateContactAlias(alias));
                        }
                        else if (alias.ContactAliasID != 0)
                            promiseArray.push(contactAliasService.remove(alias.ContactAliasID, alias.ContactID));
                    });
                }

                /************* for company addmission************/
                var admissionService = $injector.get('admissionService');
                if (!$stateParams.ContactID)
                    promiseArray.push(admissionService.admissionToCompany($scope.newDemography.ContactID));

                /*********** end company admission*************/

                $q.all(promiseArray).then(function () {
                    deferred.resolve();
                    isContactAliasDirty = false;
                    isClientIdentifierDirty = false;
                },
                function (error) {
                    alertService.error('OOPs something went wrong ' + error);
                    deferred.reject();
                }).finally(function () {
                    $timeout(function () {
                        isSaving = false;
                    });
                });;

                return deferred.promise;
            }

            $scope.add = function (isNext) {
                $scope.newDemography.ContactID = 0;
                $scope.newDemography.ModifiedOn = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY hh:mm:ss.SSS A')
                eciDemographicService.add($scope.newDemography).then(function (response) {
                    var data = response.data;
                    $scope.newDemography.ContactID = data.ID;
                    $scope.ServerValidationErrors = data.ServerValidationErrors;
                    if (data.ResultCode === 0) {
                        addUpdateDetails($scope.newDemography.ContactID).then(function () {
                            //ToDo: Use a substring to check for patientprofile in case the state names ever change
                            if ($scope.isPatientProfileState) {
                                $scope.$parent.getPatientProfileData();
                            }

                            alertService.success('Demographics has been successfully added.');
                            resetForm();
                            $scope.get($scope.newDemography.ContactID).then(function () {
                                if (isNext) {
                                    $scope.next();
                                }
                                else {
                                    var firstWorkflowState = angular.element("xen-workflow-action[data-state-name] > li.list-group-item.active").parents('.work-flow').find("xen-workflow-action[data-state-name]");
                                    $state.transitionTo(firstWorkflowState.attr('data-state-name'), { ContactID: $scope.newDemography.ContactID }, {
                                        notify: true
                                    });
                                }
                            });
                        });

                    } else {
                        alertService.error('OOPS! Something went wrong: ', data.ResultMessage);
                    }
                },
                function (errorStatus) {
                    alertService.error('OOPS! Something went wrong');
                },
                function (notification) {
                    alertService.warning(notification);
                });
            }

            $scope.edit = function (isNext) {
                $scope.editMode = true;
                $scope.newDemography.isContactNotDirty = !(formService.isDirty('additionalContactForm') || formService.isDirty('contactInformationForm') || formService.isDirty('contactMethodForm') || (formService.isDirty('presentingProblemForm')));;
                $scope.newDemography.ModifiedOn = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY hh:mm:ss.SSS A')
                var isDirtyDemographics = false;
                if (formService.isDirty('additionalContactForm') || formService.isDirty('contactInformationForm') || formService.isDirty('contactMethodForm') || formService.isDirty('alternateIDForm') || formService.isDirty('presentingProblemForm')) {
                    isDirtyDemographics = true;
                }
                if (isDirtyDemographics || isClientIdentifierDirty) {
                    eciDemographicService.update($scope.newDemography).then(function (response) {
                        var data = response.data;
                        if ($scope.newDemography.ContactID === 0 ||
                            $scope.newDemography.ContactID == undefined ||
                            $scope.newDemography.ContactID == null) {
                            $scope.newDemography.ContactID = data.ID;
                        }
                        if (data.ResultCode === 0) {
                            addUpdateDetails($scope.newDemography.ContactID).then(function () {
                                alertService.success('Demographics has been successfully Updated.');

                                if ($scope.isPatientProfileState) {
                                    $scope.$parent.getPatientProfileData();
                                }

                                resetForm();
                                $scope.get($scope.newDemography.ContactID).then(function () {
                                    if (isNext)
                                        $scope.next();
                                });
                            });

                        } else {
                            alertService.error('OOPS! Something went wrong');
                        }
                    },
                        function (errorStatus) {
                            alertService.error('OOPS! Something went wrong');
                        },
                        function (notification) {
                            alertService.warning(notification);
                        });
                }
                else {
                    addUpdateDetails($scope.newDemography.ContactID).then(function () {
                        alertService.success('Demographics has been successfully Updated.');

                        if ($scope.isPatientProfileState) {
                            $scope.$parent.getPatientProfileData();
                        }

                        resetForm();
                        $scope.get($scope.newDemography.ContactID).then(function () {
                            if (isNext)
                                $scope.next();
                        });
                    });
                }
            };

            $scope.next = function () {
                if ($scope.newDemography.ContactID !== 0 &&
                    $scope.newDemography.ContactID != undefined &&
                    $scope.newDemography.ContactID != null) {
                    var nextState = angular.element("xen-workflow-action[data-state-name] > li.list-group-item.active").parents('.work-flow').next().find("xen-workflow-action[data-state-name]");
                    if (nextState.length > 0) {
                        $timeout(function () {
                            $rootScope.setform(false);
                            $scope.Goto(nextState.attr('data-state-name'), { ContactID: $scope.newDemography.ContactID });
                        });
                    }
                } else {
                    alertService.error('Please register before proceeding to next screen');
                }
            };

            $scope.save = function (isNext, mandatory, hasErrors) {
                var hasBlankAlternateID = false;
                if (formService.isDirty($scope.alternateIDForm.$name)) {
                    $scope.newDemography.ClientAlternateIDs = $filter('filter')($scope.ClientAlternateIDs, function (item) {
                        if (item.EffectiveDate) {
                            item.EffectiveDate = $filter("formatDate")(item.EffectiveDate);
                        }
                        if (item.ExpirationDate) {
                            item.ExpirationDate = $filter("formatDate")(item.ExpirationDate);
                        }
                        if (item.ClientIdentifierTypeID && item.ClientIdentifierTypeID != 0 && (!item.AlternateID || item.AlternateID == "")) {
                            hasBlankAlternateID = true;
                        }
                        item.ModifiedOn = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY hh:mm:ss.SSS A');
                        return item.AlternateID !== '';
                    });
                }
                else {
                    delete $scope.newDemography.ClientAlternateIDs;
                }
                if (hasBlankAlternateID) {
                    bootbox.confirm("Other ID will not be saved without other ID Number, Are you sure you want to proceed?", function (result) {
                        if (result) {
                            saveContactData(isNext, mandatory, hasErrors);
                        }
                    });
                }
                else {
                    saveContactData(isNext, mandatory, hasErrors);
                }
            };

            var saveContactData = function (isNext, mandatory, hasErrors) {
                action = isNext;

                if (!mandatory && isNext && hasErrors) {
                    $scope.next();
                }

                return auditService.getUniqueId().then(function (data) {
                    if (data != null || data != undefined) {
                        uniqueID = data;
                    }
                    else { uniqueID = 0; }


                    if ((!mandatory && !hasErrors && !isNext) || (mandatory && !hasErrors && !isNext) || (!mandatory && !hasErrors && isNext) || (mandatory && !hasErrors && isNext) || isContactAliasDirty || isClientIdentifierDirty) {
                        // save
                        var isDirty = formService.isDirty();
                        // format DOB for offline mode
                        $scope.newDemography.DOB = $filter("formatDate")($scope.newDemography.DOB);
                        if ($scope.newDemography.IsDeceased && $scope.newDemography.DeceasedDate)
                            $scope.newDemography.DeceasedDate = $filter('formatDate')($scope.newDemography.DeceasedDate);
                        var dob = new Date($scope.newDemography.DOB);
                        if (dob >= $scope.endDate) {
                            alertService.error('Please check date of birth.');
                            return false;
                        }

                        if (!(isDirty || isContactAliasDirty || isClientIdentifierDirty) && isNext) {
                            $scope.next();
                        } else if (isDirty || isContactAliasDirty || isClientIdentifierDirty) {
                            $scope.newDemography.ClientAlternateIDs = $filter('filter')($scope.newDemography.ClientAlternateIDs, function (item) {
                                item.TransactionID = uniqueID;
                                item.ScreenID = $scope.screenID;
                                return item;
                            });
                            $scope.verifyDuplicateContacts().then(function (response) {
                                if (response == undefined || response.DataItems == undefined || response.DataItems.length === 0) {
                                    // format presenting problem effective & expiration date
                                    if ($scope.contactPresentingProblem) {

                                        if ($scope.contactPresentingProblem && $scope.contactPresentingProblem.EffectiveDate) {
                                            $scope.contactPresentingProblem.EffectiveDate = $filter("formatDate")($scope.contactPresentingProblem.EffectiveDate);
                                        }
                                        if ($scope.contactPresentingProblem && $scope.contactPresentingProblem.ExpirationDate) {
                                            $scope.contactPresentingProblem.ExpirationDate = $filter("formatDate")($scope.contactPresentingProblem.ExpirationDate);
                                        }

                                        $scope.newDemography.ContactPresentingProblem = $scope.contactPresentingProblem;
                                    }
                                    else {
                                        delete $scope.newDemography.ContactPresentingProblem;
                                    }
                                    $scope.newDemography.TransactionID = uniqueID;
                                    $scope.newDemography.ScreenID = $scope.screenID;
                                    //Set contact type to 1(patient)
                                    $scope.newDemography.ContactTypeID = 1;
                                    //$scope.newDemography.ClientTypeID = $scope.ClientTypeID;
                                    if ($scope.newDemography.SSNStatusID == 0)
                                        $scope.newDemography.SSNStatusID = null;
                                    if ($scope.newDemography.ContactID !== 0 &&
                                        $scope.newDemography.ContactID != undefined &&
                                        $scope.newDemography.ContactID != null) {
                                        $scope.edit(isNext);
                                    } else {
                                        $scope.add(isNext);
                                    }
                                    $scope.setShortcutKey(false, false, false, false);
                                }
                                else {
                                    $scope.duplicateContactList = bindDataModel(response.DataItems, false);
                                    if ($scope.duplicateContactList != null && $scope.duplicateContactList.length > 0) {
                                        duplicateContactsTable.bootstrapTable('load', $scope.duplicateContactList);
                                        $('#duplicateContactListModel').modal('show');
                                        $('#duplicateContactListModel').on('shown.bs.modal', function () {
                                            $scope.setShortcutKey(true, false, false, true);
                                            $rootScope.setFocusToGrid('duplicateContactsTable');
                                        });
                                    }
                                    else {
                                        duplicateContactsTable.bootstrapTable('removeAll');
                                    }

                                }

                            });
                        };
                    }
                });
            };

            $scope.setShortcutKey = function (enterKeyStop, stopNext, saveOnEnter, stopSave) {
                $scope.enterKeyStop = enterKeyStop;
                $scope.stopNext = stopNext;
                $scope.saveOnEnter = saveOnEnter;
                $scope.stopSave = stopSave;
            };

            $scope.checkSSN = function () {
                if ($scope.newDemography.SSN && $scope.newDemography.SSN != "") {
                    var items = $filter('filter')(declinedSsnStatus, function (item) {
                        return item == $scope.newDemography.SSNStatusID;
                    });
                    if (items.length > 0) {
                        alertService.error("Invalid SSN Status because SSN is already provided");
                        $scope.newDemography.SSNStatusID = 0;
                        return false;
                    }
                }
            }



            /********************************************* Start Email Functions ********************************************/

            var getContactAliases = function (contactID) {

                contactAliasService.get(contactID).then(function (data) {
                    if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                        $scope.ContactAliases = data.DataItems;
                        setAddMinusButtons($scope.ContactAliases);
                    }
                }).finally(function () {
                    resetContactAlias();
                    if ($scope.isReadOnlyForm) {
                        $scope.ContactAliases = $filter("filter")($scope.ContactAliases, function (obj) {
                            obj.ShowPlusButton = false;
                            obj.ShowMinusButton = false;
                            return obj;
                        });
                    }
                });
            }

            var initContactAliases = function () {
                $scope.ContactAliases = [];
                $scope.ContactAliases.push(objContactAlias());
            };

            var objContactAlias = function () {
                var obj = {
                    ContactAliasID: 0,
                    ContactID: 0,
                    AliasFirstName: '',
                    AliasMiddle: '',
                    AliasLastName: '',
                    SuffixID: null,
                    ShowPlusButton: true,
                    ShowMinusButton: true,
                    IsActive: true
                };
                return obj;
            }

            $scope.addNewAlias = function () {
                globalObjectsService.setViewContent();
                $scope.ContactAliases = $filter("filter")($scope.ContactAliases, function (obj) {
                    obj.ShowPlusButton = false;
                    return obj;
                });
                $scope.ContactAliases.push(objContactAlias());
            }

            $scope.removeAlias = function (index) {
                globalObjectsService.setViewContent();
                $scope.ContactAliases = removeControl($scope.ContactAliases, index, 'ContactAliasID');
                $scope.ContactAliases = setAddMinusButtons($scope.ContactAliases);
            }

            var removeControl = function (collection, index, pkID) {
                var newCollection = $filter('filter')(collection, function (data) {
                    return data.IsActive === true;
                });
                var obj = newCollection[index];

                if (eval('obj.' + pkID) === 0) {
                    newCollection.splice(index, 1);
                }
                else {
                    obj.IsActive = false;

                    if (pkID.indexOf('ContactAlias') >= 0) {
                        isContactAliasDirty = true;
                    }
                    else if (pkID.indexOf('ClientIdentifier') >= 0) {
                        isClientIdentifierDirty = true;
                    }
                }
                return mergedCollection(collection, newCollection);
            }

            var setAddMinusButtons = function (model) {
                var activeCollection = $filter('filter')(model, function (data) {
                    return data.IsActive === true;
                });
                if (activeCollection.length == 1) {
                    activeCollection[0].ShowPlusButton = true;
                    activeCollection[0].ShowMinusButton = true;
                }
                else if (activeCollection.length > 1) {
                    angular.forEach(activeCollection, function (data, index) {
                        if (index == 0) {
                            data.ShowMinusButton = true;
                        }
                        else if (index == activeCollection.length - 1) {
                            data.ShowMinusButton = true;
                            data.ShowPlusButton = true;
                        }
                        else
                            data.ShowMinusButton = true;
                    });
                }
                return mergedCollection(model, activeCollection);
            }

            var mergedCollection = function (collection, activeCollection) {
                var inActiveCollection = $filter('filter')(collection, function (data) {
                    return data.IsActive === false;
                });
                if (inActiveCollection.length > 0) {
                    // remove obj from activeCollection which exists in inActiveCollection
                    activeCollection = activeCollection.filter(function (val) {
                        return inActiveCollection.indexOf(val) == -1;
                    });
                    activeCollection = activeCollection.concat(inActiveCollection);
                }
                return activeCollection;
            }

            var resetContactAlias = function () {
                if ($scope.ContactAliasForm)
                    $rootScope.formReset($scope.ContactAliasForm, $scope.ContactAliasForm.$name);
            }

            /* End Email Functions */

            $scope.init();
        }
    ]);
