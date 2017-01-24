angular.module('xenatixApp')
    .controller('referralReferredInformationController',
        ['$scope', '$state', '$q', '$stateParams', '$modal', 'referralReferredInformationService', 'alertService', 'lookupService', '$filter', '$rootScope', 'formService', '$timeout', 'navigationService', 'referralDispositionService',
        function ($scope, $state, $q, $stateParams, $modal, referralReferredInformationService, alertService, lookupService, $filter, $rootScope, formService, $timeout, navigationService, referralDispositionService) {

            $scope.referral = {
                ReferredToDetailID: 0,
                ReferralHeaderID: $stateParams.ReferralHeaderID,
                Date: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
                Time: $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal'),
                OrganizationID: "",
                ActionTaken: "",
                Comments: "",
                UserID: 0,
                AMPM: $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal').indexOf('AM') > -1 ? "AM" : "PM"
            };
            $scope.permissionKey = $state.current.data.permissionKey;
            $scope.disposition = {
                ReferralDispositionID: "",
                ReasonforDenial: "",
                ReferralDispositionDetailID: 0,
                AdditionalNotes: "",
                ReferralDispositionOutcomeID: ""
            };

            /*$scope.$watch("referral.Date", function (oldValue, newValue) {
                if (oldValue || newValue)
                    $scope.validateDatetime();
            });*/

            $scope.validateDatetime = function () {
                if ($scope.referral.Time) {
                    var time = $scope.referral.Time.replace(':', '');
                    var date = $filter('toMMDDYYYYDate')($scope.referral.Date, 'MM/DD/YYYY','useLocal');

                    if (!$scope.referral.Date) {
                        $scope.ctrl.referralInformationForm.referredToDate.$invalid = true;
                        $scope.ctrl.referralInformationForm.referredToDate.$valid = false;

                        $scope.ctrl.referralInformationForm.$invalid = true;
                        $scope.ctrl.referralInformationForm.$valid = false;
                    }

                    var hour = time.substr(0, 2);
                    var minute = time.substr(2, 2);
                    var datetime = date + ' ' + hour + ':' + minute + ' ' + $scope.referral.AMPM;
                    var isValidTime = moment(datetime, "MM/DD/YYYY LT", true).isValid();

                    if (!isValidTime) {

                        $timeout(function () {
                            $scope.ctrl.referralInformationForm.referredToTime.$invalid = true;
                            $scope.ctrl.referralInformationForm.referredToTime.$valid = false;
                            $scope.ctrl.referralInformationForm.$invalid = true;
                            $scope.ctrl.referralInformationForm.$valid = false;

                        });
                        return false;
                    } else {
                        $scope.ctrl.referralInformationForm.referredToTime.$valid = true;
                        return true;
                    }
                };
            }


            var resetForm = function () {
                $rootScope.formReset($scope.ctrl.referralInformationForm);
            };

            $scope.saveDisposition = function () {

                $scope.referralDispositionDetail = {
                    ReferralHeaderID: $stateParams.ReferralHeaderID,
                    ReferralDispositionID: $scope.disposition.ReferralDispositionID,
                    ReferralDispositionOutcomeID: $scope.disposition.ReferralDispositionOutcomeID,
                    ReasonforDenial: $scope.disposition.ReasonforDenial,
                    AdditionalNotes: $scope.disposition.AdditionalNotes,
                    DispositionDate: new Date(),
                    UserID: $scope.referral.UserID
                };

                $scope.referralDispositionDetail.ReferralDispositionDetailID = $scope.disposition.ReferralDispositionDetailID;

                $scope.referralDispositionDetail.ReferralDispositionDetailID > 0 ?
                                    referralDispositionService.updateReferralDispositionDetail($scope.referralDispositionDetail)
                                   : referralDispositionService.addReferralDispositionDetail($scope.referralDispositionDetail);

            };

            $scope.saveReferredTo = function (isUpdate) {
                $scope.saveDisposition();
                $scope.referral.ContactID = $stateParams.ContactID;
                if (!isUpdate) {
                    return referralReferredInformationService.add($scope.referral)
                }
                else {
                    return referralReferredInformationService.update($scope.referral);
                }
            };

            $scope.save = function (isNext, mandatory, hasErrors, keepForm, next) {
                var isDirty = formService.isDirty();
                if (isDirty && !hasErrors) {
                    var deferred = $q.defer();
                    var datePart = $filter('toMMDDYYYYDate')($scope.referral.Date, 'MM/DD/YYYY');
                    $scope.referral.ReferredDateTime = $filter('toMMDDYYYYDate')(datePart + ' ' + $filter('toStandardTime')($scope.referral.Time) + ' ' + $scope.referral.AMPM, 'MM/DD/YYYY HH:mm');
                    var isUpdate = $scope.referral.ReferredToDetailID != undefined && $scope.referral.ReferredToDetailID !== 0;
                    if ($scope.disposition.ReferralDispositionID == 1) {
                        $scope.disposition.ReasonForDenial = "";
                    }
                    $scope.saveReferredTo(isUpdate).then(function (response) {
                        $scope.get().finally(function () {
                            var action = isUpdate ? 'updated' : 'added';
                            $scope.postSave(response, action, isNext);
                            deferred.resolve(response);
                        });
                    },
                     function (errorStatus) {
                         alertService.error('OOPS! Something went wrong');
                         deferred.reject();
                     },
                     function (notification) {
                         alertService.warning(notification);
                     });
                    return deferred.promise;
                }
                else if (!isDirty && isNext) {
                    $scope.next();
                }
            };

            $scope.postSave = function (response, action, isNext) {
                var data = response;
                if (data.ResultCode !== 0) {
                    alertService.error(data.ResultMessage);
                } else {
                    alertService.success('Referral referred to information has been ' + action + ' successfully.');
                    $scope.referral.ReferredToDetailID =
                        (($scope.referral !== undefined) && ($scope.referral.ReferredToDetailID !== undefined) && ($scope.referral.ReferredToDetailID != 0))
                        ? $scope.referral.ReferredToDetailID : response.ID;

                    if (isNext) {
                        $scope.next();
                    }
                    else {
                        $scope.init();

                    }
                }
            };

            $scope.next = function () {
                var params = {
                    ReferralHeaderID: $stateParams.ReferralHeaderID,
                    ContactID: $stateParams.ContactID
                }
                $state.go("referrals.followup", params);

            };

            $scope.setDefaultDatePickerSettings = function () {
                $scope.opened = false;
                $scope.dateOptions = {
                    formatYear: 'yy',
                    startingDay: 1,
                    showWeeks: false
                };
                $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'MM/dd/yyyy'];
                $scope.format = $scope.formats[3];
            };

            $scope.getLookups = function (type) {
                return lookupService.getLookupsByType(type);
            }

            $scope.getLoggedUser = function () {
                navigationService.get().then(function (data) {
                    if (data.DataItems != undefined && data.DataItems.length > 0) {
                        var user = data.DataItems[0];
                        $scope.staffName = user.UserFullName;
                        $scope.contactMethod = user.ContactNumber;
                        $scope.referral.UserID = user.UserID;
                    }
                    resetForm();
                }, function (errorStatus) {
                    resetForm();
                });
            }

            $scope.getDisposition = function () {
                $scope.isLoading = true;
                return referralDispositionService.getReferralDispositionDetail($scope.referral.ReferralHeaderID).then(function (data) {
                    if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                        $scope.disposition = data.DataItems[0];
                        resetForm();
                    }
                    $("input[name='referredToDate']").trigger("focus");

                },
                    function (errorStatus) {
                        alertService.error('Unable to get referralDisposition: ' + errorStatus);
                    }).finally(function () {
                        $scope.isLoading = false;
                    });
            };

            $scope.get = function () {
                $scope.isLoading = true;
                return referralReferredInformationService.get($scope.referral.ReferralHeaderID).then(function (data) {
                    if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                        $scope.referral = data.DataItems[0];
                        $scope.permissionID = $scope.referral.ReferredToDetailID;
                        var time = $filter('toMMDDYYYYDate')($scope.referral.ReferredDateTime, 'hh:mm A');
                        $scope.referral.Date = $filter('toMMDDYYYYDate')($scope.referral.ReferredDateTime, 'MM/DD/YYYY');
                        $scope.referral.Time = time;
                        $scope.referral.AMPM = time.indexOf('AM') > -1 ? "AM" : "PM";
                        $rootScope.$broadcast('referrals.referredto', { validationState: 'valid' });
                    }
                    else {
                        $rootScope.$broadcast('referrals.referredto', { validationState: 'warning' });
                        $scope.permissionID = 0;
                        if ($scope.referral && !$scope.$parent.isReadOnlyForm) {
                            $scope.referral.Date = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal');
                            $scope.referral.Time = $filter('toMMDDYYYYDate')(new Date(), 'hh:mm', 'useLocal');
                            $scope.referral.AMPM = $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal').indexOf('AM') > -1 ? "AM" : "PM"
                        }
                    }
                    $scope.getDisposition();
                },
                    function (errorStatus) {
                        alertService.error('Unable to get referral referred to information: ' + errorStatus);
                    }).finally(function () {
                        $scope.isLoading = false;
                        resetForm();
                    });
            };

            $scope.init = function () {
                $scope.setDefaultDatePickerSettings();
                $scope.getLoggedUser();
                //$scope.programs = $scope.getLookups('Program');
                $scope.referralDispositionType = lookupService.getLookupsByType("ReferralDispositionType");
                $scope.initReferralParent($scope.referral.ReferralHeaderID, $stateParams.ContactID);
                
                $scope.get();                
            };

            $scope.init();

        }]);
