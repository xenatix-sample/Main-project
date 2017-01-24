angular.module('xenatixApp')
    .controller('referralDispositionController', [
        '$scope', 'referralDispositionService', '$q', '$stateParams', '$state', 'alertService', 'lookupService', '$filter', '$rootScope', 'formService', 'navigationService',
        function ($scope, referralDispositionService, $q, $stateParams, $state, alertService, lookupService, $filter, $rootScope, formService, navigationService) {
            $scope.newReferralDispositionDetail = function () {
                $scope.initTakenDetail().then(function () {
                    $scope.referralDispositionDetail = {
                        ReferralDispositionDetailID: 0,
                        ReferralHeaderID: $stateParams.ReferralHeaderID,
                        ReferralDispositionID: "",
                        ReferralDispositionOutcomeID: "",
                        ReasonforDenial: '',
                        AdditionalNotes: '',
                        DispositionDate: new Date(),
                        UserID: $scope.TakenById,
                        TakenBy: $scope.TakenBy
                    };
                    resetForm();
                });
            };
            $scope.permissionKey = $state.current.data.permissionKey;
            $scope.get = function () {
                $scope.isLoading = true;
                return referralDispositionService.getReferralDispositionDetail($scope.ReferralHeaderID).then(function (data) {
                    if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                        $scope.referralDispositionDetail = data.DataItems[0];
                        $scope.referralDispositionDetail.TakenBy = lookupService.getText("Users", $scope.referralDispositionDetail.UserID);
                        $scope.permissionID = $scope.referralDispositionDetail.ReferralDispositionDetailID;
                        resetForm();
                    } else {
                        $scope.newReferralDispositionDetail();
                        $scope.permissionID = 0;
                    };
                },
                    function (errorStatus) {
                        alertService.error('Unable to get referralDisposition: ' + errorStatus);
                    }).finally(function () {
                        $rootScope.$broadcast('referrals.disposition',
                           { validationState: (($scope.referralDispositionDetail != null && $scope.referralDispositionDetail.ReferralDispositionDetailID !=undefined && $scope.referralDispositionDetail.ReferralDispositionDetailID != 0) ? 'valid' : 'warning') });
                        $scope.isLoading = false;
                    });
            };

            var resetForm = function () {
                $rootScope.formReset($scope.ctrl.referralDispositionForm);
            };

            $scope.next = function () {
                var params = {
                    ReferralHeaderID: $scope.ReferralHeaderID,
                    ContactID: $stateParams.ContactID
                }
                $state.go("referrals.forwardedto", params);
            };

            $scope.save = function (isNext, mandatory, hasErrors, keepForm) {
                var isDirty = formService.isDirty();
                if (isDirty && !hasErrors) {
                    var deferred = $q.defer();
                    var isUpdate = $scope.referralDispositionDetail.ReferralDispositionDetailID != undefined && $scope.referralDispositionDetail.ReferralDispositionDetailID !== 0;
                    $scope.saveReferralDispositionDetail(isUpdate).then(function (response) {
                        var action = isUpdate ? 'updated' : 'added';
                        $scope.get().finally(function () {
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
                    return $scope.promiseNoOp();
                } else {
                    alertService.success('Referral Disposition has been ' + action + ' successfully.');
                    $scope.referralDispositionDetail.ReferralDispositionDetailID =
                        (($scope.referralDispositionDetail !== undefined) && ($scope.referralDispositionDetail.ReferralDispositionDetailID !== undefined) && ($scope.referralDispositionDetail.ReferralDispositionDetailID != 0))
                        ? $scope.referralDispositionDetail.ReferralDispositionDetailID
                        : response.ID;

                    if (isNext) {
                        $scope.next();
                    }
                    else {
                        $scope.init();
                        return true;
                    }
                }
            };

            $scope.saveReferralDispositionDetail = function (isUpdate) {
                $scope.referralDispositionDetail.DispositionDate = $filter('toMMDDYYYYDate')($scope.referralDispositionDetail.DispositionDate, 'MM/DD/YYYY');
                $scope.referralDispositionDetail.ContactID = $stateParams.ContactID;
                if ($scope.referralDispositionDetail.ReferralDispositionID != 2) {
                    $scope.referralDispositionDetail.ReasonforDenial = '';
                }
                if (!isUpdate) {
                    return referralDispositionService.addReferralDispositionDetail($scope.referralDispositionDetail);
                }
                else {
                    return referralDispositionService.updateReferralDispositionDetail($scope.referralDispositionDetail);
                }
            };


            $scope.initTakenDetail = function () {
                return navigationService.get().then(function (data) {
                    $scope.TakenBy = data.DataItems[0].UserFullName;
                    $scope.TakenById = data.DataItems[0].UserID;                   
                });
            };

            $scope.initReferralDispositionDetail = function () {
                $scope.$parent['autoFocus'] = true;
                $scope.get();
            };

            $scope.init = function () {
                $scope.ReferralHeaderID = $stateParams.ReferralHeaderID;
                $scope.referralDispositionDetail = {};
                $scope.initReferralDispositionDetail();
                $scope.providers = lookupService.getLookupsByType("Users");
                $scope.referralStatus = lookupService.getLookupsByType("ReferralStatus");
                $scope.referralDispositionType = lookupService.getLookupsByType("ReferralDispositionType");
                $scope.referralDispositionOutcomeType = lookupService.getLookupsByType("ReferralDispositionOutcome");
                $scope.get();
                $scope.initReferralParent($scope.ReferralHeaderID, $stateParams.ContactID);
                $scope.$parent['autoFocus'] = true;
            };

            $scope.init();
        }
    ]);
