angular.module('xenatixApp')
    .controller('roleModulesController', ['$scope', '$modal', '$state', 'roleService', 'alertService', '$rootScope', 'formService', '$filter', '$stateParams', '$compile', '$timeout', '$injector', 'roleSecurityService',
        function ($scope, $modal, $state, roleService, alertService, $rootScope, formService, $filter, $stateParams, $compile, $timeout, $injector, roleSecurityService) {
        $scope.init = function () {
            $scope.$parent['autoFocus'] = true;
        }
        $scope.stopEnter = false;
        $scope.isLoading = true;
        $scope.roles = [];
        $scope.rolesData = false;
        $scope.modules = [];
        $scope.moduleFeatures = [];
        $scope.featurePermissionNames = [];
        $scope.modulePermissions = [];
        $scope.showAssignModule = false;
        $scope.stopSave = true;
        $scope.enterKeyStop = true;
        $scope.autoFocusEdit = [];
        $scope.roleId = $stateParams.RoleId || 0;
        $scope.roleModuleID = null;

        $scope.action = {
            Role: 1,
            AssignModule: 2,
            AssignPermission: 3
        };
        var linkarrayMain = [];
        $scope.currentAction = $scope.action.Role;
        $scope.initrole = function () {
            $scope.currentAction = $scope.action.Role;
            $scope.moduleID = $stateParams.ModuleID || 0;
            $rootScope.dynamicLinks = [];
            $rootScope.dynamicLinks["RoleDetails"] = [];

            $scope.getRoleById();
        }
        $scope.initModule = function () {
            $scope.currentAction = $scope.action.AssignModule;
            $scope.roleId = $stateParams.RoleId || 0;
            $scope.getModuleByRoleId(false);
            $scope.getRoleById();
        }
        $scope.$parent['autoFocus'] = true;

        resetForm = function () {
            $rootScope.formReset($scope.ctrl.roleForm);
        };

        var resetRoleForm = function () {
            $rootScope.formReset($scope.ctrl.roleForm);
        };

        var resetAssignForm = function () {
            $rootScope.formReset($scope.ctrl.assignForm);
        };

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1,
            showWeeks: 'false'
        };

        $scope.setShortCutKeys = function (flag) {
            $scope.stopSave = flag;
            $scope.enterKeyStop = flag;
        }

        $scope.InitRightNav = function () {
            if ($injector.has('roleSecurityService')) {
                if (!roleSecurityService.hasPermission('SiteAdministration-RoleManagement-Assignmodules', PERMISSION.READ)) {
                    $scope.stopEnter = true;
                }
            }
            if ($rootScope.dynamicLinks != null && !$state.is('siteadministration.rolemanagement')) {
                var linkarray = $rootScope.dynamicLinks["RoleDetails"];
                if (linkarray != null && linkarray != '') {
                    var el = angular.element(document.createElement('dynamic-navigation'));
                    el.attr('linkarray', JSON.stringify(linkarray));
                    $compile(el)($scope);
                    angular.element('div [ui-view="navigation"] ul li:nth-child(2)').append(el);
                }
            }
        }

        $scope.getRoleById = function () {
            if ($scope.roleId > 0) {
                $scope.isLoading = true;
                roleService.getRoleById($scope.roleId).then(function (data) {
                    if (hasData(data)) {
                        $scope.newRole = data.DataItems[0];
                        if ($scope.newRole.EffectiveDate !== undefined && $scope.newRole.EffectiveDate !== null) {
                            $scope.newRole.EffectiveDate = $filter('toMMDDYYYYDate')($scope.newRole.EffectiveDate, 'MM/DD/YYYY');
                        }
                        if ($scope.newRole.ExpirationDate !== undefined && $scope.newRole.ExpirationDate !== null) {
                            $scope.newRole.ExpirationDate = $filter('toMMDDYYYYDate')($scope.newRole.ExpirationDate, 'MM/DD/YYYY');
                        }
                    }
                    $scope.isLoading = false;
                    resetRoleForm();
                },

                function (errorStatus) {
                    $scope.isLoading = false;
                    alertService.error('Unable to connect to server');
                });
            }
            else
            {
                $scope.newRole = {};
                $scope.newRole.RoleID = 0;
            }
        };

        $scope.assignModuleToRole = function (isNext) {
            $scope.isLoading = true;

            var roleModule = [];
            //get all selected module
            angular.forEach($scope.modules, function (item) {
                if (item.Selected == true) {
                    item.RoleId = $scope.roleId;
                    roleModule.push(item);
                }
            });
            
            if (roleModule == null || roleModule.length == 0) {
                roleModule.push({ RoleId: $scope.roleId });
            }

            roleService.assignModuleToRole(roleModule).then(function (data) {
                $rootScope.dynamicLinks = [];
                $scope.isLoading = false;
                alertService.success('Modules has been assigned to Role.');
                $scope.getModuleByRoleId(isNext);
            },

            function (errorStatus) {
                $scope.isLoading = false;
                alertService.error('Unable to connect to server');
            });
        };

        $scope.getModuleByRoleId = function (isNext) {
            $scope.isLoading = true;
            roleService.getModuleByRoleId($scope.roleId).then(function (data) {
                $scope.modules = data.DataItems;

                // get first assigned role module id
                var result = $filter('filter')($scope.modules, { Selected: true }, true);
                if (result && result.length > 0) {
                    $scope.roleModuleID = result[0].RoleModuleID;
                }
                else {
                    $scope.roleModuleID = 0;
                }

                $scope.isLoading = false;
                $scope.PrepareRightNav();
                resetAssignForm();
            },

            function (errorStatus) {
                $scope.isLoading = false;
                alertService.error('Unable to connect to server');
            }).finally(function () {
                if (isNext)
                    $scope.handleNextState();
            });
        };

        $scope.handleNextState = function () {
            var nextState = angular.element("li[data-state-name].list-group-item").first("li[data-state-name].list-group-item");
            if (nextState.length === 0)
                $scope.Goto('^');
            else {
                $timeout(function () {
                    $rootScope.setform(false);
                    var nextStateName = nextState.attr('data-state-name');
                    var nextStateParams = nextState.attr('data-state-params');
                    $scope.Goto(nextStateName, eval("(" + nextStateParams + ')'));
                });
            }
        };

        $scope.PrepareRightNav = function () {
            if ($injector.has('roleSecurityService')) {
                var el = angular.element(document.createElement('dynamic-navigation'));
                var modulePermissionKey = 'SiteAdministration-RoleManagement-Assignmodules';
                var linkarray = [];
                angular.forEach($scope.modules, function (item) {
                    var isLink = true;
                    if (item.RoleModuleID !== 0 && roleSecurityService.hasPermission(modulePermissionKey, PERMISSION.READ)) {
                        var param1 = '{ ModuleID:' + item.RoleModuleID + ', RoleId:' + $scope.roleId + ' }';
                        var link1 = { state: 'siteadministration.rolemanagement.role.assignmoduledetails', title: '' + item.Name + '', paramarray: param1 };
                        linkarray.push(link1);
                    }
                });

                el.attr('linkarray', JSON.stringify(linkarray));
                $compile(el)($scope);

                angular.forEach(angular.element('.list-group'), function (value, key) {
                    if (key > 1) {
                        var data = angular.element(value);
                        data.remove();
                    }
                });
                angular.element('div [ui-view="navigation"] ul li:nth-child(2)').append(el);
                if ($rootScope.dynamicLinks == null || $rootScope.dynamicLinks == undefined)
                    $rootScope.dynamicLinks = [];
                $rootScope.dynamicLinks["RoleDetails"] = [];

                $rootScope.dynamicLinks["RoleDetails"] = linkarray;
            }
            // }
        }

        $scope.add = function () {
            $scope.Goto("siteadministration.rolemanagement.role.roledetails");
        };

        $scope.save = function (isNext, mandatory, hasErrors) {
            // if there are errors, the screen is optional, and the user is trying to go to the next screen, don't bother trying to save anything
            if (!formService.isDirty() && !mandatory && isNext && hasErrors) {
                if ($scope.roleId > 0)
                    $scope.next();
                else {
                    alertService.error('The required fields must be completed before moving to next screen ');
                    return;
                }
            }

            if (!formService.isDirty() && isNext) {
                $scope.next();
            } else {
                if (formService.isDirty() && !hasErrors) {
                    if ($scope.currentAction == $scope.action.Role) {
                        $scope.saveRole(isNext);
                    } else if ($scope.currentAction == $scope.action.AssignModule) {
                        $scope.assignModuleToRole(isNext);
                    }
                }
            }
        }

        $scope.endDateValidError = false;
        $scope.saveRole = function (isNext) {
            $scope.isLoading = true;
            var isValid1 = true;
            $scope.endDateValidError = false;
            if ($scope.newRole.ExpirationDate != null) {
                $scope.newRole.EffectiveDate = $filter('toMMDDYYYYDate')($scope.newRole.EffectiveDate, 'MM/DD/YYYY');
                var stime = $scope.newRole.EffectiveDate;
                if (stime.length == 0) {
                    stime = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY');
                }
                $scope.newRole.ExpirationDate = $filter('toMMDDYYYYDate')($scope.newRole.ExpirationDate, 'MM/DD/YYYY');
                var etime = $scope.newRole.ExpirationDate;
                if (Date.parse(etime) <= Date.parse(stime)) {
                    isValid1 = false;
                }
                if ($scope.ctrl.roleForm != undefined) {
                    $scope.endDateValidError = !isValid1;
                }
            }
            if (isValid1) {
                if ($scope.newRole.RoleID != null && $scope.newRole.RoleID != 0) {
                    roleService.update($scope.newRole).then(function (data) {
                        alertService.success('Role has been updated successfully.');
                        $scope.isLoading = false;
                        if ($scope.newRole.EffectiveDate == ''){
                            $scope.newRole.EffectiveDate = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY');
}
                        resetRoleForm();
                    },
                    function (errorStatus) {
                        alertService.error('OOPS! Something went wrong');
                        $scope.isLoading = false;
                    }).finally(function () {
                        if (isNext)
                            $scope.next();
                    });
                }
                else {
                    $scope.isLoading = true;
                    roleService.add($scope.newRole).then(function (data) {
                        if (data.ResultCode != 0) {
                            alertService.error(data.ResultMessage);
                        }
                        else {
                            var xml = data.ID;
                            $scope.roleId = xml;
                            $scope.newRole.RoleID = $scope.roleId;
                            roleService.getRoleById($scope.roleId).then(function () {
                                $state.transitionTo($state.current.name, { RoleId: $scope.roleId }, { notify: true, reload: false });
                                // $scope.updateHeader();
                                alertService.success('Role has been added successfully.');
                                resetRoleForm();
                                $scope.isLoading = false;
                            }).finally(function () {
                                if (isNext)
                                    $scope.next();
                            });
                        }
                    },

                    function (errorStatus) {
                        alertService.error('OOPS! Something went wrong');
                        $scope.isLoading = false;
                    });
                }
            }
        };
        $scope.next = function () {
            if ($scope.currentAction == $scope.action.Role) {
                $scope.Goto("siteadministration.rolemanagement.role.assignmodules", { RoleId: $scope.roleId });
            } else if ($scope.currentAction == $scope.action.AssignModule) {
                $scope.handleNextState();
            }
        }

        setTimeout(function () { $scope.InitRightNav(); }, 50);
    }]);