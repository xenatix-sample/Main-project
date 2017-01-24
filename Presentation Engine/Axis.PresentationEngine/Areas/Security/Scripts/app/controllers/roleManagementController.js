angular.module('xenatixApp')
    .controller('roleManagementController', ['$scope', '$filter', 'alertService', '$stateParams', '$timeout', '$rootScope', '$q', '$state', 'formService', 'lookupService', 'roleManagementService', 'navigationService', '$compile',
        function ($scope, $filter, alertService, $stateParams, $timeout, $rootScope, $q, $state, formService, lookupService, roleManagementService, navigationService, $compile) {
            var isDirty = false;
            var CREATE = 1;
            var READ = 2;
            var DELETE = 3;
            var UPDATE = 4;
            var resetForm = function () {
                $rootScope.formReset($scope.ctrl.roleManagementForm, $scope.ctrl.roleManagementForm.$name);
            };

            $scope.getLookupsByType = function (typeName) {
                return lookupService.getLookupsByType(typeName);
            };

            $scope.init = function () {
                $scope.$parent['autoFocus'] = true;
                $scope.ModuleID = $stateParams.ModuleID || 0;
                $scope.IsNext = true;
                $scope.get();
            };

            $scope.get = function () {
                $scope.Permissions = [];
                $scope.roleModules = [];
                $scope.roleModuleComponents = [];
                $scope.getPermissions();
            }

            $scope.InitRightNav = function () {
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

            $scope.getPermissions = function () {
                $scope.isLoading = true;
                return roleManagementService.getPermissions().then(function (data) {
                    if (data.ResultCode === 0 && data.DataItems != undefined && data.DataItems.length > 0) {
                        $scope.Permissions = data.DataItems;
                        $scope.getRoleModuleDetails();
                    }
                    else {
                        alertService.error('Error while loading Role Management');
                    }
                },
                function (errorStatus) {
                    alertService.error('Error while loading Role Management');
                }).finally(function () {
                    $scope.isLoading = false;
                    resetForm();
                });
            };

            $scope.getRoleModuleDetails = function () {
                $scope.isLoading = true;
                return roleManagementService.getRoleModuleDetails($scope.ModuleID).then(function (data) {
                    if (data.ResultCode === 0 && data.DataItems != undefined && data.DataItems.length > 0) {
                        $scope.roleModules = data.DataItems;
                        $state.current.title = $scope.roleModules[0].Name;

                        // check whether permission has created
                        var result = $filter('filter')($scope.roleModules[0].ModulePermissions, function (item) {
                            return item.PermissionLevelID != null;
                        }, true);
                        if (result && result.length > 0) {
                            $scope.roleModulePermissionID = result[0].RoleModulePermissionID
                        }
                        else {
                            $scope.roleModulePermissionID = ($scope.roleModulePermissionID || 0);
                        }

                        $scope.refreshBreadcrumbs();
                        $scope.getRoleModuleComponentDetails();
                    }
                    else {
                        alertService.error('Error while loading Role Management');
                    }
                },
                function (errorStatus) {
                    alertService.error('Error while loading Role Management');
                });
            };

            $scope.getRoleModuleComponentDetails = function () {
                $scope.isLoading = true;
                return roleManagementService.getRoleModuleComponentDetails($scope.ModuleID).then(function (data) {
                    if (data.ResultCode === 0 && data.DataItems != undefined && data.DataItems.length > 0) {
                        $scope.roleModuleComponents = data.DataItems;

                        // check whether permission has created
                        var isFound = false;
                        if ($scope.roleModulePermissionID) {
                            for (var i = 0; i < $scope.roleModuleComponents.length; i++) {
                                for (var j = 0; j < $scope.roleModuleComponents[i].ModuleComponents.length; j++) {
                                    var result = $filter('filter')($scope.roleModuleComponents[i].ModuleComponents[j].ModuleComponentPermissions, function (item) {
                                        return item.PermissionLevelID != null;
                                    }, true);
                                    if (result && result.length > 0) {
                                        $scope.roleModulePermissionID = result[0].RoleModuleComponentID;
                                        isFound = true;
                                        break;
                                    }
                                }
                            }
                            if (!isFound)
                                $scope.roleModulePermissionID = ($scope.roleModulePermissionID || 0);
                        }

                        resetForm();
                    }
                },
                function (errorStatus) {
                    alertService.error('Error while loading Role Management');
                }).finally(function () {
                    var nextState = angular.element("li[data-state-name].list-group-item.active").next("li[data-state-name].list-group-item");
                    if (nextState.length === 0)
                        $scope.IsNext = false;
                    resetForm();
                });
            };

            $scope.CheckAllColumn = function (permissionID, permissionLevelID, permissionLevel) {
                angular.forEach($scope.roleModuleComponents, function (roleModuleComponent) {
                    angular.forEach(roleModuleComponent.ModuleComponents, function (moduleComponent) {
                        angular.forEach(moduleComponent.ModuleComponentPermissions, function (moduleComponentPermission) {
                            if (moduleComponentPermission.PermissionID == permissionID)
                                moduleComponentPermission.PermissionLevelID = permissionLevelID;
                        })
                    })
                })

                //check for read
                if (permissionID == READ && permissionLevelID == null) {
                    var isWrite = false;
                    for (var i = 0; i < $scope.roleModules[0].ModulePermissions.length; i++) {
                        if ($scope.roleModules[0].ModulePermissions[i].PermissionID != READ && $scope.roleModules[0].ModulePermissions[i].PermissionLevelID != null) {
                            isWrite = true;
                            break;
                        }
                    }

                    if (isWrite) {
                        var roleModuleRead = $scope.roleModules[0].ModulePermissions.filter(function (obj) {
                            return obj.PermissionID == READ;
                        })[0];

                        roleModuleRead.PermissionLevelID = permissionLevel;
                        $scope.CheckAllColumn(roleModuleRead.PermissionID, permissionLevel, null);
                    }
                }
                else if (permissionID == CREATE || permissionID == UPDATE || permissionID == DELETE) {
                    if (permissionLevelID != null) {
                       
                        var roleModuleRead = $scope.roleModules[0].ModulePermissions.filter(function (obj) {
                            return obj.PermissionID == READ;
                        })[0];
                        roleModuleRead.PermissionLevelID = permissionLevel;
                        $scope.CheckAllColumn(roleModuleRead.PermissionID, permissionLevel, null);
                    }
                }
                
            }

            $scope.CheckAllRow = function (ComponentID, permissionLevelID, index) {
                var roleModuleComponent = $scope.roleModuleComponents[index].ModuleComponents.filter(function (obj) {
                    return obj.ComponentID == ComponentID;
                })[0];
                angular.forEach(roleModuleComponent.ModuleComponentPermissions, function (moduleComponentPermission) {
                    moduleComponentPermission.PermissionLevelID = permissionLevelID;
                });
            }

            $scope.CheckAll = function (permissionLevelID) {
                angular.forEach($scope.roleModules[0].ModulePermissions, function (modulePermission) {
                    modulePermission.PermissionLevelID = permissionLevelID;
                })

                angular.forEach($scope.roleModuleComponents, function (roleModuleComponent) {
                    angular.forEach(roleModuleComponent.ModuleComponents, function (moduleComponent) {
                        moduleComponent.PermissionLevelID = permissionLevelID;
                        angular.forEach(moduleComponent.ModuleComponentPermissions, function (moduleComponentPermission) {
                            moduleComponentPermission.PermissionLevelID = permissionLevelID;
                        })
                    })
                })
            }

            $scope.CheckReadComponent = function (ComponentID, permissionLevelID, permissionID, permissionLevel, index) {
                if (permissionID == READ && permissionLevelID == null) {
                    var roleModuleComponent = $scope.roleModuleComponents[index].ModuleComponents.filter(function (obj) {
                        return obj.ComponentID == ComponentID;
                    })[0];

                    var isWrite = false;
                    for (var i = 0; i < roleModuleComponent.ModuleComponentPermissions.length; i++) {
                        if (roleModuleComponent.ModuleComponentPermissions[i].PermissionID != READ && roleModuleComponent.ModuleComponentPermissions[i].PermissionLevelID != null) {
                            isWrite = true;
                            break;
                        }
                    }

                    if (isWrite) {
                        var roleModuleComponentRead = roleModuleComponent.ModuleComponentPermissions.filter(function (obj) {
                            return obj.PermissionID == READ;
                        })[0];

                        roleModuleComponentRead.PermissionLevelID = permissionLevel;
                    }
                }

                if (permissionID == CREATE || permissionID == UPDATE || permissionID == DELETE) {
                    if (permissionLevelID != null) {
                        var roleModuleComponent = $scope.roleModuleComponents[index].ModuleComponents.filter(function (obj) {
                            return obj.ComponentID == ComponentID;
                        })[0];

                        angular.forEach(roleModuleComponent.ModuleComponentPermissions, function (moduleComponentPermission) {
                            if (moduleComponentPermission.PermissionID == READ && moduleComponentPermission.PermissionLevelID == null)
                                moduleComponentPermission.PermissionLevelID = permissionLevelID;
                        });
                    }
                }
                
               
            }

            $scope.save = function (isNext, mandatory, hasErrors) {

                if (!mandatory && isNext && hasErrors) {
                    $scope.postSave(isNext);
                }

                if (!formService.isDirty() && isNext) {
                    $scope.postSave(isNext);
                }

                if ($scope.ctrl.roleManagementForm.$dirty && !hasErrors) {
                    var roleModuleToSave = {};
                    roleModuleToSave.roleModule = $scope.roleModules[0];
                    roleModuleToSave.roleModuleComponents = angular.copy($scope.roleModuleComponents);
                    roleManagementService.saveModulePermissions(roleModuleToSave).then(function (response) {
                        if (response.ResultCode === 0) {
                            alertService.success('Role Permissions saved successfully');
                            resetForm();
                            $scope.postSave(isNext);
                        }
                        else {
                            alertService.error('Unable to save Role Permissions');
                        }
                    },
                    function (errorStatus) {
                        alertService.error('Error while saving Role Permissionst');
                    });
                }
            }

            $scope.postSave = function (isNext) {
                if (isNext) {
                    $scope.handleNextState();
                } else {
                    $scope.get();
                }
            };

            $scope.handleNextState = function () {
                var nextState = angular.element("li[data-state-name].list-group-item.active").next("li[data-state-name].list-group-item");
                if (nextState.length != 0) {
                    $timeout(function () {
                        $rootScope.setform(false);
                        var nextStateName = nextState.attr('data-state-name');
                        var nextStateParams = nextState.attr('data-state-params');
                        $scope.Goto(nextStateName, eval("(" + nextStateParams + ')'));
                    });
                }
            };

            setTimeout(function () { $scope.InitRightNav(); }, 50);

            $scope.init();

        }]);
