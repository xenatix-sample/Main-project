angular.module('xenatixApp')
    .controller('userDetailController', [
        '$http', '$scope', '$rootScope', '$filter', '$stateParams', '$state', '$timeout', 'userDetailService', 'alertService', 'formService',
        function ($http, $scope, $rootScope, $filter, $stateParams, $state, $timeout, userDetailService, alertService, formService) {

            $scope.init = function () {
                $scope.emailPattern = /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/i;
                $scope.$parent['autoFocus'] = true;
                $scope.userID = $stateParams.UserID || 0;
                $scope.get($scope.userID);
                $scope.userDetail = {};
                $scope.userDetail.EffectiveFromDate = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY');
            };

            $scope.reset = function () {
                if ($scope.ctrl.userDetailForm !== undefined && $scope.ctrl.userDetailForm !== null) {
                    $rootScope.formReset($scope.ctrl.userDetailForm, $scope.ctrl.userDetailForm.name);
                }
            };
            $scope.dateOptions = {
                formatYear: 'yy',
                startingDay: 1,
                showWeeks: 'false'
            };
            $scope.get = function (userID) {
                $scope.disableFields = false;
                if (userID != null && userID !== 0) {
                    return userDetailService.get(userID).then(function (response) {
                        if (response.ResultCode === 0 && response.DataItems !== null && response.DataItems.length === 1) {
                            var obj = { stateName: $state.current.name, validationState: 'valid' };
                            $rootScope.staffManagementRightNavigationHandler(obj);
                            isValid = true;
                            $scope.userDetail = response.DataItems[0];

                            if ($scope.userDetail.EffectiveToDate !== undefined && $scope.userDetail.EffectiveToDate !== null) {
                                $scope.userDetail.EffectiveToDate = $filter('toMMDDYYYYDate')($scope.userDetail.EffectiveToDate, 'MM/DD/YYYY');
                            }

                            if ($scope.userDetail.EffectiveFromDate == undefined || $scope.userDetail.EffectiveFromDate == null) {
                                $scope.userDetail.EffectiveFromDate = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY');
                            }

                            $scope.userDetail.EffectiveFromDate = $filter('toMMDDYYYYDate')($scope.userDetail.EffectiveFromDate, 'MM/DD/YYYY');
                            $scope.disableFields = $scope.userDetail.UserGUID != null && $scope.userDetail.UserGUID != undefined;

                            $scope.reset();
                        } else {
                            $scope.userDetail = { IsActive: true };
                            alertService.error('Error while loading the user\'s details');
                            var obj = { stateName: $state.current.name, validationState: 'invalid' };
                            $rootScope.staffManagementRightNavigationHandler(obj);
                        }
                    });
                } else {
                    $scope.userDetail = {
                        IsActive: true,
                        UserID: 0
                    };
                    return $scope.promiseNoOp();
                }
            };

            $scope.updateHeader = function () {
                $scope.$parent.$broadcast('updateuserheader', $scope.userID);
            };

            $scope.update = function (isNext) {
                return userDetailService.update($scope.userDetail).then(function (response) {
                    if (response.ResultCode === 0) {
                        alertService.success('User updated successfully');
                        $scope.postSave(isNext);
                    } else {
                        alertService.error('Error while updating user');
                    }
                });
            };

            $scope.validateEffectiveDateLessThanExpirationDate = function () {
                if ($scope.userDetail && $scope.userDetail.EffectiveFromDate && $scope.userDetail.EffectiveToDate) {
                    var effectiveDate = $filter('formatDate')($scope.userDetail.EffectiveFromDate);
                    var expirationDate = $filter('formatDate')($scope.userDetail.EffectiveToDate);

                    if (Date.parse(effectiveDate) > Date.parse(expirationDate)) {
                        alertService.error('Effective date cannot be greater than Expiration date');
                        $scope.userDetail.EffectiveToDate = null;
                    }
                }
            }


            $scope.add = function (isNext) {
                return userDetailService.add($scope.userDetail).then(function (response) {
                    if (response.ResultCode === 0) {
                        alertService.success('User added successfully');
                        var xml = response.AdditionalResult;
                        $scope.userID = $(xml).find("UserIdentifier").text();
                        $scope.get($scope.userID).then(function () {
                            $state.transitionTo($state.current.name, { UserID: $scope.userID }, { notify: true, reload: false });
                            $scope.updateHeader();
                            if (isNext) {
                                $scope.handleNextState();
                            }
                        });
                    } else {
                        if (response.ResultCode === 2) {
                            alertService.error('Error while adding user:Email already registered');
                        } else
                            alertService.error('Error while adding user');
                    }
                });
            };

            $scope.handleNextState = function () {
                var nextState = angular.element("li[data-state-name].list-group-item.active").next("li[data-state-name].list-group-item");
                if (nextState.length === 0) {
                    $scope.Goto('^');
                } else {
                    $timeout(function () {
                        $rootScope.setform(false);
                        var nextStateName = nextState.attr('data-state-name');
                        $scope.Goto(nextStateName, { UserID: $scope.userID });
                    });
                }
            };

            $scope.save = function (isNext, mandatory, hasErrors) {
                if (formService.isDirty() && !hasErrors) {
                    $scope.userDetail.EffectiveFromDate = $filter('formatDate')($scope.userDetail.EffectiveFromDate);
                    $scope.userDetail.EffectiveToDate = $filter('formatDate')($scope.userDetail.EffectiveToDate);
                    if ($scope.userID !== null && $scope.userID !== undefined && $scope.userID !== 0) {
                        $scope.update(isNext);
                    } else {
                        $scope.add(isNext);
                    }
                } else if (!formService.isDirty() && isNext) {
                    $scope.handleNextState();
                } else {
                    return $scope.promiseNoOp();
                }
            };

            $scope.postSave = function (isNext) {
                $scope.updateHeader();
                if (isNext) {
                    $scope.handleNextState();
                } else {
                    $scope.get($scope.userID);
                }
            };

            $scope.init();
        }
    ]);