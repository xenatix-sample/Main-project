(function () {
    angular.module('xenatixApp')
        .controller('quickRegistrationController', ['$scope', '$state', 'formService', '$controller', '$filter', '$stateParams', '$rootScope', 'registrationService', 'additionalDemographyService', 'contactPhoneService', 'contactSSNService', '$q', 'alertService', '$injector','eciAdditionalDemographicService', function ($scope, $state, formService, $controller, $filter, $stateParams, $rootScope, registrationService, additionalDemographyService, contactPhoneService, contactSSNService, $q, alertService, $injector,eciAdditionalDemographicService) {
            $controller('raceDetailsController', { $scope: $scope });
            $controller('callCenterQuickRegistrationController', { $scope: $scope });
            $scope.permissionKey = $state.current.data.permissionKey;
            var ageLimit = DOB_AGE.MaxAge;
            $scope.additionalDemographicQuickReg = {};
            $scope.newDemographyQuickReg =
                {
                    ContactID: 0
                };

            if (!$stateParams.CallCenterHeaderID)
                $scope.permissionRegID = 0;
            if ($state.current.name.indexOf('lawliaison') >= 0) {
                $scope.isLawLiaison = true;
            }
            $scope.cancelFlyout = function () {
                if (formService.isDirty('ctrl.quickRegForm')) {
                    bootbox.confirm("Any unsaved data will be lost. Do you want to continue?", function (result) {
                        if (result == true) {
                            cancelFlyout();
                        }
                    });
                } else {
                    cancelFlyout();
                }

               
            }

            var cancelFlyout = function () {
                $rootScope.defaultFormName = getDefaultFormName();
                $('.row-offcanvas').removeClass('active');
                resetForm();
            }

            $scope.setShortcutKey = function (enterKeyStop, stopNext, saveOnEnter, stopSave) {
                $scope.enterKeyStop = enterKeyStop;
                $scope.stopNext = stopNext;
                $scope.saveOnEnter = saveOnEnter;
                $scope.stopSave = stopSave;
            };

            var resetForm = function () {
                $rootScope.formReset($scope.ctrl.quickRegForm, $scope.ctrl.quickRegForm.$name);
                $scope.raceDetailsChanged = false;
            };

            $scope.get = function (contactID) {
                $scope.promises = [];
                var deferred = $q.defer();
                return registrationService.get(contactID).then(function (data) {
                    if (hasData(data)) {
                        $scope.permissionRegID = contactID;
                        var newDemographyQuickRegData = data.DataItems[0];
                        if (newDemographyQuickRegData && newDemographyQuickRegData.DOB)
                            newDemographyQuickRegData.DOB = $filter('toMMDDYYYYDate')(newDemographyQuickRegData.DOB, 'MM/DD/YYYY');
                        // if SSN exists and it's length is less then 9 then get it again
                        if (newDemographyQuickRegData.SSN && newDemographyQuickRegData.SSN.length > 0 && newDemographyQuickRegData.SSN.length < 9) {
                            contactSSNService.refreshSSN(contactID, newDemographyQuickRegData);
                        }
                        var additionalDemographyDeferred = $q.defer();
                        $scope.promises.push(additionalDemographyDeferred.promise);

                        if (newDemographyQuickRegData.ClientTypeID == 1) {
                            eciAdditionalDemographicService.getAdditionalDemographic(contactID).then(function (data) {
                                $scope.additionalDemographicQuickReg = {};
                                if (hasData(data)) {
                                    $scope.additionalDemographicQuickReg = data.DataItems[0];
                                }
                                additionalDemographyDeferred.resolve(data);
                            });
                        }
                        else {
                            additionalDemographyService.getAdditionalDemographic(contactID).then(function (data) {
                                $scope.additionalDemographicQuickReg = {};
                                if (hasData(data)) {
                                    $scope.additionalDemographicQuickReg = data.DataItems[0];
                                }
                                additionalDemographyDeferred.resolve(data);
                            });
                        }
                        $scope.getRace(contactID);
                        var phoneDeferred = $q.defer();
                        $scope.promises.push(phoneDeferred.promise);
                        contactPhoneService.get(contactID, newDemographyQuickRegData.ContactTypeID).then(function (data) {
                            $scope.phone = {};
                            if (hasData(data)) {
                                $scope.phone = data.DataItems[0];
                            }
                            phoneDeferred.resolve(data);
                        });
                        $scope.isDuplicateCheckRequired = false;
                        $q.all($scope.promises).then(function (data) {
                            resetForm();
                            $scope.newDemographyQuickReg = newDemographyQuickRegData;
                            $rootScope.$broadcast('updateData', { Data: angular.copy($scope.newDemographyQuickReg) });
                            return deferred.resolve(data);
                        },
                        function (errorStatus) {
                            alertService.error(errorStatus);
                        });
                        return deferred.promise;
                    }
                    else {
                        $scope.permissionRegID = 0;
                    }
                },
                function (errorStatus) {
                    alertService.error('Unable to get demographics: ' + errorStatus);
                });
            };

            $scope.register = function (isNext, mandatory, hasErrors) {
                var isDirty = formService.isDirty();
                if ($scope.newDemographyQuickReg && $scope.newDemographyQuickReg.DOB) {
                    $scope.newDemographyQuickReg.DOB = $filter("formatDate")($scope.newDemographyQuickReg.DOB);
                }
                if ($scope.newDemographyQuickReg)
                    $scope.newDemographyQuickReg.ContactTypeID = 1;
                if ($scope.phone)
                    $scope.phone.IsPrimary = true;
                var activeRace = $filter('filter')($scope.selectedRaceDetails, function (item) {
                    return item.IsActive;
                });
                setRaceErrorClass();
                if (activeRace.length == 0) {
                    alertService.error('Please select atleast one Race.');
                    return;
                }
                if (isDirty && !hasErrors || $scope.raceDetailsChanged) {
                    if ($stateParams.CallCenterHeaderID) {
                        update();
                    }
                    else {
                        return $scope.verifyDuplicateContacts().then(function (response) {
                            if (response == undefined || response.DataItems == undefined || response.DataItems.length === 0) {
                                if ($scope.newDemographyQuickReg.ContactID !== 0 &&
                               $scope.newDemographyQuickReg.ContactID != undefined &&
                               $scope.newDemographyQuickReg.ContactID != null) {
                                    update();
                                }
                                else {
                                    add();
                                }
                            }
                            else {
                                $scope.showPotentialDuplicates(response.DataItems);
                            }
                        });
                    }
                }
                $scope.setShortcutKey(false, false, false, false);
            };

            var add = function () {
                var promise = []
                $scope.newDemographyQuickReg.ContactID = 0;
                $scope.additionalDemographicQuickReg.AdditionalDemographicID = 0;
                registrationService.add($scope.newDemographyQuickReg).then(function (response) {
                    $scope.newDemographyQuickReg.ContactID = $scope.additionalDemographicQuickReg.ContactID = $scope.phone.ContactID = response.data.ID;
                    if ($scope.newDemographyQuickReg.ClientTypeID == 1) {
                        $scope.additionalDemographicQuickReg.IsCPSInvolved = false;
                        promise.push(eciAdditionalDemographicService.addAdditionalDemographic($scope.additionalDemographicQuickReg));
                    }
                    else {
                        promise.push(additionalDemographyService.addAdditionalDemographic($scope.additionalDemographicQuickReg));
                    }
                    
                    promise.push(contactPhoneService.save($scope.phone));
                    /************* for company addmission************/
                    var admissionService = $injector.get('admissionService');
                    if ($scope.newDemographyQuickReg.ContactID)
                        promise.push((admissionService.admissionToCompany($scope.newDemographyQuickReg.ContactID)));
                    /*********** end company admission*************/
                    promise.push($scope.saveRace($scope.newDemographyQuickReg.ContactID));
                    $q.all(promise).then(function (response) {
                        $q.serial($scope.taskArray).then(function () {
                            $scope.get($scope.newDemographyQuickReg.ContactID).then(function () {
                                alertService.success('Demographics has been successfully added.');
                                $scope.raceDetailsChanged = false;
                                resetForm();

                                //Close the flyout on the successfull Registration
                                $scope.cancelFlyout();
                            });
                        });
                    });
                });
            };

            var update = function () {
                var promise = []
                registrationService.update($scope.newDemographyQuickReg).then(function (response) {
                    $scope.additionalDemographicQuickReg.ContactID = $scope.phone.ContactID = $scope.newDemographyQuickReg.ContactID;

                    if ($scope.newDemographyQuickReg.ClientTypeID == 1) {
                        $scope.additionalDemographicQuickReg.IsCPSInvolved = false;
                        if ($scope.additionalDemographicQuickReg.AdditionalDemographicID != null && $scope.additionalDemographicQuickReg.AdditionalDemographicID !== 0)
                            promise.push(eciAdditionalDemographicService.updateAdditionalDemographic($scope.additionalDemographicQuickReg));
                        else {
                            promise.push(eciAdditionalDemographicService.addAdditionalDemographic($scope.additionalDemographicQuickReg));
                        }
                    }
                    else {
                        if ($scope.additionalDemographicQuickReg.AdditionalDemographicID != null && $scope.additionalDemographicQuickReg.AdditionalDemographicID !== 0)
                            promise.push(additionalDemographyService.updateAdditionalDemographic($scope.additionalDemographicQuickReg));
                    else {
                            promise.push(additionalDemographyService.addAdditionalDemographic($scope.additionalDemographicQuickReg));
                        }
                    }
                    promise.push(contactPhoneService.save($scope.phone));
                    /************* for company addmission************/
                    var admissionService = $injector.get('admissionService');
                    if ($scope.newDemographyQuickReg.ContactID)
                        promise.push((admissionService.admissionToCompany($scope.newDemographyQuickReg.ContactID)));
                    /*********** end company admission*************/
                    promise.push($scope.saveRace($scope.newDemographyQuickReg.ContactID));
                    $q.all(promise).then(function () {
                        $q.serial($scope.taskArray).then(function () {
                            $scope.get($scope.newDemographyQuickReg.ContactID).then(function () {
                                alertService.success('Demographics has been successfully updated.');
                                $scope.raceDetailsChanged = false;
                                resetForm();
                                //Close the flyout on the successfull Registration
                                $scope.cancelFlyout();
                            });
                        });
                    });
                });
            };

        }]);
})();
