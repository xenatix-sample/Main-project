angular.module('xenatixApp')
    .controller('userProfileController', [
        '$scope', '$state', '$modal', 'userProfileService', 'alertService', 'lookupService', '$filter', '$rootScope', 'formService', '$timeout', '$stateParams', 'navigationService',
function ($scope, $state, $modal, userProfileService, alertService, lookupService, $filter, $rootScope, formService, $timeout, $stateParams, navigationService) {
    var isMyProfile = false;
            $scope.emailPattern = /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/i;
            $scope.init = function () {
                if ($state.current.name == 'myprofile.nav.profile') 
                    isMyProfile = true;
                else
                    $scope.permissionKey = $state.current.data.permissionKey;
                $scope.initEmails();
                $scope.initUserProfile();
                $scope.initPhones();
                $scope.initAddresses();
                $scope.userID = 0;
                $scope.inStaffManagement = false;
                if ($stateParams.UserID !== undefined && $stateParams.UserID !== null) {
                    $scope.userID = $stateParams.UserID;
                    $scope.inStaffManagement = true;
                }
                
                $scope.get();
            };
           
            $scope.initUserProfile = function () {
                $scope.UserProfile = {};
            };

            $scope.initPhones = function () {
                $scope.Phones = [{
                    Index: 0,
                    PhoneTypeID: null,
                    Number: '',
                    PhonePermissionID: null,
                    IsPrimary: true
                }];
            };

            $scope.initEmails = function () {
                $scope.Emails = [{
                    EmailID: 0,
                    Email: '',
                    EmailPermissionID: null,
                    IsPrimary: true,
                    required: true
                }];
            };

            $scope.initAddresses = function () {
                var addressTypes = $scope.getLookupsByType('AddressType');
                var workAddressType = $filter('filter')(addressTypes, { Name: 'Work' })[0];
                var workAddressTypeID = (workAddressType !== null && workAddressType !== undefined) ? workAddressType.ID : 4;
                $scope.Addresses = [{
                    AddressTypeID: workAddressTypeID,
                    Line1: '',
                    Line2: '',
                    City: '',
                    StateProvince: 0,
                    County: 0,
                    Zip: '',
                    MailPermissionID: 1,
                    IsPrimary: true,
                    IsGateCode: true,
                    IsComplexName: true
                }];
            };

            $scope.getLookupsByType = function (typeName) {
                return lookupService.getLookupsByType(typeName);
            };

            resetForm = function () {
                $rootScope.formReset($scope.ctrl.userProfileForm);
            };

            $scope.postGet = function(data) {
                if (data.ResultCode !== 0) {
                    alertService.error('Error while getting data');
                } else {
                    $scope.userProfile = data.DataItems[0];

                    if ($scope.userProfile.Phones.length > 0) {
                        $scope.Phones = $scope.userProfile.Phones;
                    }
                    if ($scope.userProfile.Emails.length > 0) {
                        angular.forEach($scope.userProfile.Emails, function (emailObj) {
                            emailObj.required = true;
                        });
                        $scope.Emails = $scope.userProfile.Emails;
                    }
                    if ($scope.userProfile.Addresses.length > 0) {
                        $scope.Addresses = $scope.userProfile.Addresses;
                        angular.forEach($scope.Addresses, function (addressObj) {
                            addressObj.IsGateCode = true;
                            addressObj.IsComplexName = true;
                        });
                    }
                    resetForm();
                }
                $('#phoneType').focus();
            };

            $scope.get = function () {
                if ($scope.inStaffManagement) {
                    return userProfileService.getByID($scope.userID, isMyProfile).then(function (idData) {
                        $scope.postGet(idData);
                    });
                } else {
                    return userProfileService.get(isMyProfile).then(function (data) {
                        $scope.postGet(data);
                    });
                }
            };

            $scope.save = function (isNext, mandatory, hasErrors) {
                if (formService.isDirty() && !hasErrors) {

                    $scope.userProfile.Phones = $scope.Phones;
                    $scope.userProfile.Emails = $scope.Emails;

                   
                    $scope.userProfile.Addresses = $filter('filter')($scope.Addresses, function (item) {

                        
                        return item.Line1 !== '' || item.Line2 !== '' || item.City !== '' || (item.StateProvince !== '' && item.StateProvince != 0) || (item.County !== '' && item.County != 0) || item.Zip !== '';
                    });
                    if ($scope.Addresses[0].ComplexName !== '' && $scope.Addresses[0].ComplexName != undefined) {
                        if ($scope.Addresses[0].Line1 === '') {
                            $scope.addressLine1= true;
                            return;
                        }
                    }
                    $scope.addressLine1= false;


                    userProfileService.save($scope.userProfile, isMyProfile).then(function (data) {
                        if (data.ResultCode !== 0) {
                            alertService.error('Error while updating user profile');
                        } else {
                            alertService.success('User profile updated successfully');
                            $scope.postSave(isNext);
                        }
                    },
                    function(errorStatus) {
                        alertService.error('Error while saving data: ' + errorStatus);
                    });
                } else if (!formService.isDirty() && isNext) {
                    $scope.postSave(isNext);
                }
            };

            $scope.postSave = function (isNext) {
                navigationService.get(true);
                if (isNext) {
                    $scope.handleNextState();
                } else {
                    $scope.get().then(function() {
                        $('#phoneType').focus();
                    });
                }
            };

            $scope.handleNextState = function () {
                var nextState = angular.element("li[data-state-name].list-group-item.active").next("li[data-state-name].list-group-item");
                if (nextState.length === 0) {
                    $scope.Goto('^');
                } else {
                    $timeout(function () {
                        $rootScope.setform(false);
                        var nextStateName = nextState.attr('data-state-name');
                        $scope.Goto(nextStateName);
                    });
                }
            };

            $scope.init();
        }
    ]);