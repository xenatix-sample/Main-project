angular.module('xenatixApp')
    .controller('userCredentialController', [
        '$http', '$scope', '$state', '$rootScope', '$filter', '$stateParams', 'userCredentialService', 'alertService', 'formService', '$timeout', 'navigationService',
        function ($http, $scope, $state, $rootScope, $filter, $stateParams, userCredentialService, alertService, formService, $timeout, navigationService) {
            var isMyProfile = false;
            $scope.init = function () {
                if ($state.current.name == 'myprofile.nav.credentials')
                    isMyProfile = true;
                else
                    $scope.permissionKey = $state.current.data.permissionKey;
                $scope.userID = $stateParams.UserID || 0;
                $scope.defaultCredentialList = [{ CredentialObj: null, CredentialID: null, Acronym: '', LicenseNumber: null, LicenseIssueDate: null, LicenseExpirationDate: null, StateIssuedByID: null }];
                $scope.defaultCredentialObject = { CredentialObj: null, CredentialID: null, Acronym: '', LicenseNumber: null, LicenseIssueDate: null, LicenseExpirationDate: null, StateIssuedByID: null };
                $scope.initDateOptions();
                $scope.inProfile = false;
                $timeout(function () {
                    $scope.reset();
                });

                $scope.prepareUserData($scope.userID).then(function () {
                    $scope.get($scope.userID);
                });
            };
       
            $scope.prepareUserData = function (userID) {
                if (userID === 0) {
                    $scope.inProfile = true;
                    return navigationService.get().then(function (response) {
                        $scope.userID = response.DataItems[0].UserID;
                    });
                }

                return $scope.promiseNoOp();
            };

            

            $scope.initDateOptions = function () {
                $scope.dateOptions = {
                    formatYear: 'yy',
                    startingDay: 1,
                    showWeeks: false
                };
            };

            $scope.reset = function () {
                if ($scope.ctrl) {
                    $rootScope.formReset($scope.ctrl.userCredentialForm);
                    $scope.ctrl.userCredentialForm.modified = false;
                }
            };

            $scope.triggerTypeaheadCustom = function (index) {
                var nameTag = 'credentialName' + index;
                $('[name="' + nameTag + '"]').focus();
                $('[name="' + nameTag + '"]').trigger('forcelyOpenTypeaheadPopup');
                setTimeout(function () { $scope.reposTypeahead(index); }, 500);
            };

            $scope.typeaheadChange = function (index) {
                setTimeout(function () { $scope.reposTypeahead(index); }, 500);
            };

            $scope.reposTypeahead = function (index) {
                var typeaheadElement = angular.element(document.querySelector('#inputgroup' + index + ' .dropdown-menu'));
                if (typeaheadElement !== null && typeaheadElement !== undefined && typeaheadElement[0] !== undefined)
                    $scope.repositionElement(typeaheadElement);
            }

            $scope.get = function (userID) {
                $scope.user = {};
                $scope.user.UserID = $scope.userID;
                var isValid = false;
                return userCredentialService.get(userID, isMyProfile).then(function (response) {
                    if (response.ResultCode === 0) {
                        if (response.DataItems.length > 0) {
                            isValid = true;
                            var obj = { stateName: $state.current.name, validationState: 'valid' };
                            $rootScope.staffManagementRightNavigationHandler(obj);
                            $scope.user.Credentials = response.DataItems;
                            $scope.userCredentialID = $scope.user.Credentials[0].UserCredentialID;
                            var credentialList = $scope.getLookupsByType('Credential');
                            var idx = 0;
                            angular.forEach($scope.user.Credentials, function (credential) {
                                if (credential.LicenseIssueDate !== undefined && credential.LicenseIssueDate !== null) {
                                    credential.LicenseIssueDate = $filter('toMMDDYYYYDate')(credential.LicenseIssueDate, 'MM/DD/YYYY');
                                }
                                if (credential.LicenseExpirationDate !== undefined && credential.LicenseExpirationDate !== null) {
                                    credential.LicenseExpirationDate = $filter('toMMDDYYYYDate')(credential.LicenseExpirationDate, 'MM/DD/YYYY');
                                }

                                if ($scope.user.Credentials.length > 1) {
                                   $scope.setMinusButton = true;
                                }

                                if (credential.CredentialID !== null) {
                                    //ToDo: replace with $filter
                                    angular.forEach(credentialList, function (item) {
                                        if (item.CredentialID === credential.CredentialID) {
                                            $scope.user.Credentials[idx].CredentialObj = item;
                                            $scope.user.Credentials[idx].Acronym = item.CredentialAbbreviation;
                                        }
                                    });
                                }
                                idx++;
                            });

                            $scope.reset();
                        } else {
                            $scope.userCredentialID = 0;
                            $scope.user.Credentials = $scope.defaultCredentialList;
                        }
                    } else {
                        $scope.user.Credentials = $scope.defaultCredentialList;
                        alertService.error('Error while loading the user\'s credentials');
                    }

                    if (!isValid) {
                        var invalidObj = { stateName: $state.current.name, validationState: 'warning' };
                        $rootScope.staffManagementRightNavigationHandler(invalidObj);
                    }
                    $scope.reset();
                });
            };

            $scope.setAcronym = function (item, index) {
                var loopIndex = 0;
                var alreadyAdded = false;
                angular.forEach($scope.user.Credentials, function (cred) {
                    if (loopIndex !== index) {
                        if (item.CredentialID === cred.CredentialObj.CredentialID) {
                            $scope.user.Credentials[index].CredentialObj = '';
                            $scope.user.Credentials[index].Acronym = '';
                            alreadyAdded = true;
                            alertService.error('Credential has already been added!');
                        }
                    }
                    
                    loopIndex++;
                });

                if (!alreadyAdded) {
                    $scope.user.Credentials[index].Acronym = item.CredentialAbbreviation;
                }
            };

            $scope.handleNextState = function () {
                var nextState = angular.element("li[data-state-name].list-group-item.active").next("li[data-state-name].list-group-item");
                if (nextState.length === 0)
                    $scope.Goto('^');
                else {
                    $timeout(function () {
                        $rootScope.setform(false);
                        var nextStateName = nextState.attr('data-state-name');
                        $scope.Goto(nextStateName, { UserID: $scope.userID });
                    });
                }
            };

            $scope.postSave = function (isNext) {
                $scope.reset();
                var isValid = false;
                if ($scope.user.Credentials.length > 0) {
                    isValid = true;
                }
                var stateDetail = { stateName: $state.current.name, validationState: (isValid ? 'valid' : 'warning') };
                $rootScope.staffManagementRightNavigationHandler(stateDetail);

                if (isNext) {
                    $scope.handleNextState();
                } else {
                    $scope.get($scope.userID);
                }
            };

            $scope.save = function (isNext, mandatory, hasErrors) {
                if (formService.isDirty() && !hasErrors && !$scope.inProfile) {
                    var noCreds = false;
                    angular.forEach($scope.user.Credentials, function (credential) {
                    		if (credential.CredentialObj !== null && credential.CredentialObj !== undefined) {
                            credential.CredentialID = credential.CredentialObj.CredentialID;
                            credential.LicenseExpirationDate =  $filter('toMMDDYYYYDate')(credential.LicenseExpirationDate, 'MM/DD/YYYY');
                            credential.LicenseIssueDate = $filter('toMMDDYYYYDate')(credential.LicenseIssueDate, 'MM/DD/YYYY');
                        } else {
                            credential.CredentialID = null;
                            noCreds = true;
                        }
                    });

                    if (noCreds) {
                        alertService.error('Please select a Credential Name');
                        return $scope.promiseNoOp();
                    }

                    return userCredentialService.update($scope.user, isMyProfile).then(function (response) {
                        if (response.ResultCode === 0) {
                            alertService.success('User credentials saved successfully');
                            $scope.postSave(isNext);
                        } else {
                            alertService.error('Error while saving user credentials');
                        }
                    });
                } else if (!formService.isDirty() && isNext) {
                    $scope.handleNextState();
                } else {
                    return $scope.promiseNoOp();
                }
            };

            $scope.addRow = function (event) {
                if (event !== null && event !== undefined) {
                    event.stopPropagation();
                }

                var tmpDefaultCredentialObject = angular.copy($scope.defaultCredentialObject);
                $scope.user.Credentials.push(tmpDefaultCredentialObject);
                $scope.setMinusButton = true;
            };

            $scope.deleteRow = function (credential, event) {
                if (event !== null && event !== undefined) {
                    event.stopPropagation();
                }

                $scope.user.Credentials.splice($scope.user.Credentials.indexOf(credential), 1);

                if ($scope.user.Credentials.length == 1)
                {
                    $scope.setMinusButton = false;
                }

                if ($scope.ctrl.userCredentialForm != undefined)
                    $scope.ctrl.userCredentialForm.$setDirty();
            };

            $scope.validateEffectiveDateLessThanExpirationDate = function (index) {
                var effectiveDate = new Date($scope.user.Credentials[index].LicenseIssueDate);
                var expirationDate = new Date($scope.user.Credentials[index].LicenseExpirationDate);
                if ($scope.user.Credentials[index].LicenseExpirationDate && $scope.user.Credentials[index].LicenseIssueDate) {
                    if (effectiveDate > expirationDate) {
                        //[Bug:11797] change validation error message.
                        alertService.error('Effective date cannot be greater than Expiration date.');
                        $scope.user.Credentials[index].LicenseExpirationDate = null;
                    }
                }
            };

            $scope.reposDatepicker = function (id) {
                var datepickerElement = angular.element(document.querySelector('#' + id + ' .dropdown-menu'));
                if (datepickerElement !== null && datepickerElement !== undefined && datepickerElement[0] !== undefined)
                    $scope.repositionElement(datepickerElement);
            }

            $scope.repositionElement = function (reposElement) {
                windowHeight = $(window)[0].screen.availHeight;
                elementHeight = reposElement[0].getBoundingClientRect().height;
                var top = reposElement.parent()[0].getBoundingClientRect().top + 37;
                var left = reposElement.parent()[0].getBoundingClientRect().left;

                if (top + elementHeight < windowHeight) {
                    reposElement.css('top', top + 'px');
                    reposElement.css('left', left + 'px');
                }
                else {
                    var bottom = reposElement.parent()[0].getBoundingClientRect().top - elementHeight;
                    reposElement.css('top', bottom + 'px');
                    reposElement.css('left', left + 'px');
                }
            }

            $scope.init();
        }
    ]);
