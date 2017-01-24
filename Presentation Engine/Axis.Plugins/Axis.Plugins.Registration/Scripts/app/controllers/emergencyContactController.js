angular.module('xenatixApp')
    .controller('emergencyContactController', ['$scope', '$modal', '$filter', 'emergencyContactService', 'alertService', 'settings', 'lookupService', '$stateParams', '$state', '$rootScope', 'formService',
        function ($scope, $modal, $filter, emergencyContactService, alertService, settings, lookupService, $stateParams, $state, $rootScope, formService) {
            $scope.isLoading = true;
            $scope.emergencyContact = {
            };
            $scope.dateOptions = {
                formatYear: 'yy',
                startingDay: 1,
            };

            var collateralGroupID = RELATIONSHIP_TYPE_GROUPID.Collateral;

            $scope.contactID = $stateParams.ContactID;
            var ageLimit = 120;
            $scope.$parent['autoFocus'] = true; //for Focus
            $scope.endDate = new Date();
            $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'MM/dd/yyyy'];
            $scope.format = $scope.formats[3];
            $scope.emailPattern = /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/i;
            $scope.selIdx = -1;
            resetForm = function () {
                $rootScope.formReset($scope.ctrl.contactForm);
            };

            $scope.getLookupsByType = function (typeName) {
                return lookupService.getLookupsByType(typeName);
            };

            var stateList = $scope.getLookupsByType('StateProvince');
            var countyList = $scope.getLookupsByType('County');

            getIdByText = function (text) {
                var ContactTypeDetails = $scope.getLookupsByType('ContactType')
                return ContactTypeDetails.filter(
                      function (obj, value) {
                          return (obj.Name == text);
                      }
                  );
            };

            var contactTypeId = getIdByText('Emergency')[0].ID;

            $scope.initEmergencyContact = function () {
                $scope.emergencyContact = {};
                $scope.initAddresses();
                $scope.initPhones();
                $scope.initEmails();

                var relationshipLookups = getLookupsByType('RelationshipType');
                $scope.emergencyRelationshipLookups = $filter('filter')(relationshipLookups, { RelationshipGroupID: collateralGroupID });
            };

            $scope.initAddresses = function () {
                $scope.Addresses = [{
                    AddressTypeID: null,
                    Line1: '',
                    Line2: '',
                    City: '',
                    StateProvince: 0,
                    County: 0,
                    Zip: '',
                    MailPermissionID: 1,
                    IsPrimary: true,
                    IsGateCode: false,
                    IsComplexName: false
                }];
            };

            $scope.initPhones = function () {
                $scope.Phones = [{
                    Index: 0,
                    PhoneTypeID: null,
                    Number: '',
                    PhonePermissionID: null,
                    IsPrimary: false
                }];
            };

            $scope.initEmails = function () {
                $scope.Emails = [{
                    Email: '',
                    EmailPermissionID: null,
                    IsPrimary: true
                }];
            };
            // Get Emergency Records
            $scope.get = function (contactID, contactTypeID) {
                $scope.isLoading = true;
                return emergencyContactService.get(contactID, contactTypeID).then(function(data) {
                        bindDataModel(data.DataItems);
                        checkFormStatus();
                    }).then(function() {
                        $scope.isLoading = false;
                    });
            };

            checkFormStatus = function() {
                $scope.$watch('ctrl.contactForm.$valid', function(newValue) {
                    if (newValue !== undefined)
                        $rootScope.$broadcast('registration.emergcontacts',
                        { validationState: (($scope.emergencyContactList != null && Object.keys($scope.emergencyContactList).length > 0) ? 'valid' : 'warning') });
                });
            };


            // Bind js model with data that got from server
            bindDataModel = function (model) {
                $scope.initEmergencyContact();
                $scope.emergencyContactList = model;
                var fullAddress = '';

                angular.forEach($scope.emergencyContactList, function (emergencyContact) {
                    emergencyContact.DOB = $filter('toMMDDYYYYDate')(emergencyContact.DOB, 'MM/DD/YYYY', 'useLocal');
                    var adrs = emergencyContact.Addresses[0];
                    fullAddress = '';
                    if (adrs != undefined) {
                        fullAddress = adrs.Line1;
                        fullAddress = concatWithSeparator(fullAddress, adrs.Line2, ',');
                        fullAddress = concatWithSeparator(fullAddress, adrs.City, ',');

                        var lstTxtById = lookupService.getSelectedTextById(adrs.StateProvince, stateList);
                        if (lstTxtById != undefined && lstTxtById.length > 0)
                            fullAddress = concatWithSeparator(fullAddress, lstTxtById[0].Name, ',');

                        lstTxtById = lookupService.getSelectedTextById(adrs.County, countyList);
                        if (lstTxtById != undefined && lstTxtById.length > 0)
                            fullAddress = concatWithSeparator(fullAddress, lstTxtById[0].Name, ',');

                        fullAddress = concatWithSeparator(fullAddress, adrs.Zip, ',');
                    }
                    emergencyContact.FullAddress = fullAddress;
                });
                resetForm();
            };

            // Add Emergency Record
            $scope.add = function(isNext, modelToSave) {
                $scope.isLoading = true;
                emergencyContactService.add(modelToSave)
                    .then(
                        function(response) {
                            var data = response.data;
                            if (data.ResultCode == 0) {
                                alertService.success('Emergency contact has been successfully added.');
                                $scope.get($scope.contactID, $scope.emergencyContact.ContactTypeID).then(function() {
                                    if (isNext)
                                        $scope.next();
                                });
                            } else
                            alertService.error('OOPS! Something went wrong');
                        },
                        function(errorStatus) {
                            $scope.isLoading = false;
                            alertService.error('OOPS! Something went wrong');
                        },
                        function(notification) {
                            alertService.warning(notification);
                        }).then(function() {
                        $scope.isLoading = false;
                    });
            };

            // Reset Emergency Model
            $scope.reset = function () {
                resetForm();
                $scope.initEmergencyContact();
                $scope.$parent['autoFocus'] = true;
            };

            // Perform save operation
            $scope.save = function (isNext, mandatory, hasErrors) {
                // if there are errors, the screen is optional, and the user is trying to go to the next screen, don't bother trying to save anything
                // Vipul Singhal - As per Json if user modified the optional screen and click enter then it should display the validation error 
                // and if user don't don any modification then user can move to next screen.
                if (!mandatory && isNext && hasErrors) {
                    $scope.next();
                }

                var isDirty = formService.isDirty();

                if (isDirty && !hasErrors) {
                    $scope.emergencyContact.ContactTypeID = contactTypeId;
                    var modelToSave = angular.copy($scope.emergencyContact);

                    // filter out blank address
                    modelToSave.Addresses = $filter('filter')($scope.Addresses, function (item) {
                        return item.Line1 != '' || item.Line2 != '' || item.City != '' || item.StateProvince != 0 || item.County != 0 || item.Zip != '';
                    });
                    // filter out blank phone
                    modelToSave.Phones = $filter('filter')($scope.Phones, function (item) {
                        return item.Number != '';
                    });
                    // filter out blank email
                    modelToSave.Emails = $filter('filter')($scope.Emails, function (item) {
                        return item.Email != '';
                    });

                    if ($scope.emergencyContact.ContactRelationshipID > 0) {
                        $scope.update(isNext, modelToSave);
                    }
                    else {
                        //modelToSave.ContactID = $scope.contactID;
                        modelToSave.ParentContactID = $scope.contactID;
                        $scope.add(isNext, modelToSave);
                    }

                }

                $scope.selIdx = -1;

                if (isNext) {
                    $scope.next();
                }
            };

            $scope.edit = function (index, rowIndex) {
                $scope.selectedID = index;
                $scope.selIdx = rowIndex;
                angular.forEach($scope.emergencyContactList, function (emergencyContact) {
                    if (emergencyContact.ContactRelationshipID == index) {
                        $scope.emergencyContact = angular.copy(emergencyContact);
                        $scope.Addresses = angular.copy(emergencyContact.Addresses);
                        if ($scope.Addresses.length == 0) {
                            $scope.initAddresses();
                        }
                        $scope.Phones = angular.copy(emergencyContact.Phones);
                        if ($scope.Phones.length == 0) {
                            $scope.initPhones();
                        }
                        $scope.Emails = angular.copy(emergencyContact.Emails);
                        if ($scope.Emails.length == 0) {
                            $scope.initEmails();
                        }
                        $scope.$parent['autoFocus'] = true;
                        resetForm();
                        return;
                    }

                });
            };

            // Update Emergency contact
            $scope.update = function(isNext, modelToSave) {
                $scope.isLoading = true;
                emergencyContactService.update(modelToSave)
                    .then(
                        function(response) {
                            var data = response.data;
                            if (data.ResultCode == 0) {
                                alertService.success('Emergency contact has been successfully Updated.');
                                $scope.get($scope.contactID, $scope.emergencyContact.ContactTypeID).then(function() {
                                    if (isNext)
                                        $scope.next();
                                });
                            } else {
                                alertService.error('OOPS! Something went wrong');
                            }
                        },
                        function(errorStatus) {
                            $scope.isLoading = false;
                            alertService.error('OOPS! Something went wrong');
                        },
                        function(notification) {
                            alertService.warning(notification);
                        }).then(function() {
                        $scope.isLoading = false;
                    });
            };

            $scope.remove = function (id) {
                bootbox.confirm("Selected emergency contact will be removed.\n Do you want to continue?", function (result) {
                    if (result == true) {
                        emergencyContactService.remove($scope.contactID, id).then(function(data) {
                                $scope.isLoading = false;
                                alertService.success('Emergency contact has been deleted.');
                                $scope.get($scope.contactID, contactTypeId).then(function() {
                                    $scope.selIdx = -1;
                                });
                            },
                            function(errorStatus) {
                                alertService.error('OOPS! Something went wrong');
                            }).then(function() {
                                $scope.$apply();
                            }
                        );
                    }
                });
            };

            $scope.cancel = function () {
                bootbox.confirm("You will lose the information entered.\n Do you want to continue?", function (result) {
                    if (result == true) {
                        $scope.initEmergencyContact();
                        $scope.editMode = false;
                        $scope.ctrl.contactForm.$setPristine();
                        $scope.$apply();
                        $scope.$parent['autoFocus'] = true;
                    }
                });
            };

            $scope.next = function () {
                $state.go("registration.collateral", {
                    ContactID: $scope.contactID
                });
            };

            $scope.calculateAge = function () {
                if ($scope.emergencyContact.DOB != null) {
                    var date = new Date($scope.emergencyContact.DOB);
                    if (date <= $scope.endDate) {
                        $scope.emergencyContact.Age = parseInt($filter('toAge')($scope.emergencyContact.DOB));
                        var isDatePastLimit = $filter('isDateMaxLimit')($scope.emergencyContact.DOB, ageLimit);
                        if (isDatePastLimit) {
                            $scope.emergencyContact.Age = $scope.emergencyContact.DOB = null;
                            alertService.error("Age Can't Be greater than "+ ageLimit +" years.");
                        }
                        else {
                            $scope.emergencyContact.Age = $filter('ageToShow')($scope.emergencyContact.DOB);
                        }

                        $('#doberrortd').removeClass('has-error');
                        $('#doberror').addClass('ng-hide');
                    }
                    else {
                        $scope.emergencyContact.Age = null;
                        $('#doberror').removeClass('ng-hide');
                        $('#doberrortd').addClass('has-error');
                    }
                }
                else {
                    $scope.emergencyContact.Age = '';
                }
            };

            $scope.formatPhone = function (phoneText) {
                if (phoneText == null || phoneText == undefined)
                    return '';
                return phoneText.replace(/(\d{3})(\d{3})(\d{4})/, '$1-$2-$3');
            }

            $scope.get($scope.contactID, contactTypeId);

        }]);