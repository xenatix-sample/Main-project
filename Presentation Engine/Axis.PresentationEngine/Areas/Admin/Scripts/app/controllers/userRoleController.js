angular.module('xenatixApp')
    .controller('userRoleController', [
        '$http', '$scope', '$rootScope', '$filter', '$state', '$stateParams', '$timeout', 'userRoleService', 'alertService', 'formService',
        function ($http, $scope, $rootScope, $filter, $state, $stateParams, $timeout, userRoleService, alertService, formService) {

            $scope.init = function () {
                $scope.userID = $stateParams.UserID;
                $scope.get($scope.userID);
            };

            $scope.reset = function () {
                if ($scope.ctrl.userRoleForm !== undefined && $scope.ctrl.userRoleForm !== null) {
                    $rootScope.formReset($scope.ctrl.userRoleForm, $scope.ctrl.userRoleForm.name);
                }
            };

            $scope.get = function (userID) {
                $scope.selectedRoles = [];
                $scope.user = {};
                $scope.user.UserID = $scope.userID;
                return userRoleService.get(userID).then(function (response) {
                    if (response.ResultCode === 0) {
                        $scope.userRoles = response.DataItems;

                        for (var i = 0; i < $scope.userRoles.length; i++) {
                            if ($scope.userRoles[i].RoleGUID !== null && $scope.userRoles[i].RoleGUID !== undefined) {
                                $scope.userRoles[i].Disabled = true;
                            } else {
                                $scope.userRoles[i].Disabled = false;
                            }
                            if ($scope.userRoles[i].UserID > 0) {
                                var role = $filter('filter')($scope.selectedRoles, { RoleID: $scope.userRoles[i].RoleID }, true)[0];
                                if (!role) {
                                    $scope.selectedRoles.push({ RoleID: $scope.userRoles[i].RoleID, Name: $scope.userRoles[i].Name, Description: $scope.userRoles[i].Description });
                                }
                            }
                        }

                        if ($scope.selectedRoles.length > 0) {
                            var obj = { stateName: $state.current.name, validationState: 'valid' };
                            $rootScope.staffManagementRightNavigationHandler(obj);
                            $scope.userRoleID = $scope.selectedRoles[0].RoleID;
                        } else {
                            var obj = { stateName: $state.current.name, validationState: 'invalid' };
                            $rootScope.staffManagementRightNavigationHandler(obj);
                            $scope.userRoleID = 0;
                        }

                        $scope.reset();
                    } else {
                        alertService.error('Error while loading the user\'s roles');
                    }
                });
            };

            $scope.toggleSelection = function toggleSelection(roleID) {
                var idx = -1;
                for (var i = 0; i < $scope.selectedRoles.length; i++) {
                    if ($scope.selectedRoles[i].RoleID === roleID) {
                        idx = i;
                    }
                }
                if (idx > -1) {
                    $scope.selectedRoles.splice(idx, 1);
                } else {
                    $scope.selectedRoles.push({ RoleID: roleID, Name: '', Description: '' });
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
                var isValid = false;
                if ($scope.user.Roles.length > 0) {
                    isValid = true;
                }
                var stateDetail = { stateName: $state.current.name, validationState: (isValid ? 'valid' : 'invalid') };
                $rootScope.staffManagementRightNavigationHandler(stateDetail);

                if (isNext) {
                    $scope.handleNextState();
                } else {
                    $scope.get($scope.userID);
                    window.scrollTo(0,0);
                }
            };

            $scope.save = function (isNext, mandatory, hasErrors) {
                if (formService.isDirty() && !hasErrors) {
                    $scope.user.Roles = $scope.selectedRoles;
                    return userRoleService.update($scope.user).then(function (response) {
                        if (response.ResultCode === 0) {
                            alertService.success('User roles saved successfully');
                            $scope.postSave(isNext);
                        } else {
                            alertService.error('Error while saving user roles');
                        }
                    });
                } else if (!formService.isDirty() && isNext) {
                    $scope.handleNextState();
                } else {
                    return $scope.promiseNoOp();
                }
            };

            $scope.init();
        }
    ]);