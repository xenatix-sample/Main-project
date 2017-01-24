angular.module('xenatixApp')
    .controller('roleController', ['$scope', '$modal', '$state', 'roleService', 'alertService', '$rootScope', 'formService', '$filter', '$stateParams', '$compile', function ($scope, $modal, $state, roleService, alertService, $rootScope, formService, $filter, $stateParams, $compile) {
        $scope.init = function () {
            $scope.$parent['autoFocus'] = true;
            $scope.rolesTable = $('#rolesTable');
            $scope.initializeBootstrapTable();
            $scope.get();
            $scope.initialChildState = '.rolemanagement.roledetails';

        }

        $scope.isLoading = true;
        $scope.roles = [];
        $scope.rolesData = false;
        $scope.modules = [];
        $scope.modulesAssignedToRole = [];
        $scope.moduleFeatures = [];
        $scope.featurePermissionNames = [];
        $scope.modulePermissions = [];
        $scope.showAssignModule = false;
        $scope.stopSave = true;
        $scope.enterKeyStop = true;
        //For Focus
        $scope.detailsIndex = 0;
        $scope.assignmoduleIndex = 1;
        $scope.restIndex = 2;
        $scope.autoFocusEdit = [];
        $scope.roleName = '';
        $scope.roleId = 0;
        $scope.roleId = $stateParams.RoleId || 0;
        $scope.action = {
            Role: 1,
            AssignModule: 2,
            AssignPermission: 3
        };
        $scope.currentAction = $scope.action.Role;
        $scope.initrole = function () {
            $scope.currentAction = $scope.action.Role;
            $scope.moduleID = $stateParams.ModuleID || 0;
             
        }
        $scope.initModule = function () {
            $scope.currentAction = $scope.action.AssignModule;
            $scope.roleId = $stateParams.RoleId || 0;
            $scope.getModuleByRoleId(false);
            $scope.getRoleById();
        }
        $scope.currentWizardStep = 0;
        $scope.$parent['autoFocus'] = true;

        resetForm = function () {
            $rootScope.formReset($scope.ctrl.roleForm);
        };

        $scope.wizardSteps = [];

        var rolesTable = $("#rolesTable");

        $scope.setShortCutKeys = function (flag) {
            $scope.stopSave = flag;
            $scope.enterKeyStop = flag;
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

        $scope.initializeBootstrapTable = function () {



            $scope.tableoptions = {
                pagination: true,
                pageSize: 10,
                pageList: [10, 25, 50, 100],
                search: false,
                showColumns: true,
                data: [],
                undefinedText: '',
                columns: [
                    {
                        field: 'Name',
                        title: 'Name'
                    },
                    {
                        field: 'Description',
                        title: 'Description'
                    },
                    {
                        field: 'RoleID',
                        title: '',
                        formatter: function (value, row, index) {
                            return '<span class="text-nowrap"><a data-default-action href="javascript:void(0)" id="editRole" ng-click=edit(' + value + ') title="Edit Role" security permission-key="SiteAdministration-RoleManagement-RoleDetails" permission="update" space-key-press><i class="fa fa-pencil fa-fw padding-left-small padding-right-small" /></a>' +
                                    '<a data-default-no-action href="javascript:void(0)" ng-click="remove(' + value + ',$event)" id="removeRole" title="Delete Role" security permission-key="SiteAdministration-RoleManagement-RoleDetails" permission="delete" space-key-press ng-init="autoFocusEdit[assignmoduleIndex] = $last"><i class="fa fa-trash fa-fw padding-left-small padding-right-small"></i></a>' +
                                    '</span>';

                        }
                    }
                ]
            };
        };

        $scope.getRoles = function () {
            $scope.isLoading = true;
            $scope.rolesData = false;
            roleService.getRoles($scope.roleName).then(function (data) {
                $scope.roles = data.DataItems;
                if (data != null && data.DataItems != null) {
                    rolesTable.bootstrapTable('load', data.DataItems);
                } else {
                    rolesTable.bootstrapTable('removeAll');
                }

                $scope.rolesData = true;
                $scope.isLoading = false;
                resetForm();
            },

            function (errorStatus) {
                $scope.isLoading = false;
                alertService.error('Unable to connect to server');
            });
        };



        $scope.getRoleById = function () {
            $scope.isLoading = true;
            roleService.getRoleById($scope.roleId).then(function (data) {
                $scope.newRole = data.DataItems[0];
                $scope.isLoading = false;
                resetForm();
            },

            function (errorStatus) {
                $scope.isLoading = false;
                alertService.error('Unable to connect to server');
            });
        };

        $scope.getFeaturePermissionByRoleId = function () {
            $scope.isLoading = true;
            $scope.setAction('', $scope.action.AssignPermission);
            roleService.getFeaturePermissionByRoleId($scope.roleId).then(function (data) {
                $scope.moduleFeatures = data.DataItems;

                //get all feature permission name
                angular.forEach($scope.moduleFeatures, function (item) {
                    angular.forEach(item.Permissions, function (permission) {
                        var isUnique = true;
                        angular.forEach($scope.featurePermissionNames, function (uniquePermission) {
                            if (uniquePermission.ModuleID == item.ModuleId && uniquePermission.Name == permission.Name) {
                                isUnique = false;
                            }
                        });

                        if (isUnique) {
                            $scope.featurePermissionNames.push({ ModuleID: item.ModuleId, PermissionID: permission.PermissionID, Name: permission.Name });
                        }
                    });
                });

                // prepare unmapped permission to feature
                angular.forEach($scope.moduleFeatures, function (item) {
                    angular.forEach($scope.featurePermissionNames, function (uniquePermission) {
                        var isUnique = true;

                        angular.forEach(item.Permissions, function (permission) {
                            if (uniquePermission.Name == permission.Name) {
                                isUnique = false;
                            }
                        });

                        if (isUnique) {
                            item.Permissions.push({ PermissionID: uniquePermission.PermissionID, Name: uniquePermission.Name, NotMapped: true });
                        }
                    });
                });

                // sort permission
                angular.forEach($scope.moduleFeatures, function (item) {
                    item.Permissions.sort(function (left, right) {
                        return left.PermissionID < right.PermissionID ? -1 : 1;
                    });
                });

                $scope.getAssignedPermissionByModuleId(id);
                resetForm();
                $scope.isLoading = false;
            },
             function (errorStatus) {
                 $scope.isLoading = false;
                 alertService.error('Unable to connect to server');
             });
        };
        $scope.getModulePermissionByRoleId = function () {
            $scope.isLoading = true;
            roleService.getModulePermissionByRoleId($scope.roleId).then(function (data) {
                $scope.modulePermissions = data.DataItems;

                $scope.isLoading = false;
                $scope.autoFocusEdit[$scope.detailsIndex] = true;
                resetForm();
            },
            function (errorStatus) {
                $scope.isLoading = false;
                alertService.error('Unable to connect to server');
            });
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

                var el = angular.element(document.createElement('dynamic-navigation'));
                var linkarray = [];
                angular.forEach(roleModule, function (item) {
                    if (item.Selected == true) {
                        var param1 = '{ ModuleID:' + item.ModuleID + ' }';
                        var link1 = { state: 'siteadministration.rolemanagement.role.assignmoduledetails', title: '' + item.Name + '', paramarray: param1 };
                        linkarray.push(link1);
                    }
                });              
                el.attr('linkarray', JSON.stringify(linkarray));
                $compile(el)($scope);
                angular.element('div [ui-view="navigation"] ul li:nth-child(2)').append(el);

                if ($rootScope.dynamicLinks == null)
                    $rootScope.dynamicLinks = [];
                $rootScope.dynamicLinks["RoleDetails"] = linkarray;

                $scope.isLoading = false;
                alertService.success('Modules has been assigned to Role.');

                $scope.getModuleByRoleId($scope.next);
                $scope.getFeaturePermissionByRoleId();
                $scope.getModulePermissionByRoleId();
            },

            function (errorStatus) {
                $scope.isLoading = false;
                alertService.error('Unable to connect to server');
            });
        };

        $scope.getModuleByRoleId = function (isNext) {
            $scope.isLoading = true;
            roleService.getModuleByRoleId($scope.roleId).then(function (data) {
                $scope.modulesAssignedToRole = data.DataItems;
                $scope.modules = angular.copy($scope.modulesAssignedToRole);
                $scope.isLoading = false;

                //prepare wizard
                $scope.wizardSteps = [];
                var wizardStepCount = 0;

                $scope.wizardSteps.push({ actionName: 'roleDetails', action: $scope.action.Role, wizardStep: wizardStepCount++ });
                $scope.wizardSteps.push({ actionName: 'assignModule', action: $scope.action.AssignModule, wizardStep: wizardStepCount++ });
                angular.forEach($scope.modulesAssignedToRole, function (item) {
                    if (item.Selected == true)
                        $scope.wizardSteps.push({ actionName: 'assignModulePermission-' + item.ModuleId, action: $scope.action.AssignPermission, wizardStep: wizardStepCount++ });
                });

                setTimeout(function () {
                    if (isNext != null)
                        $scope.next();
                }, 1000);

                resetForm();
            },

            function (errorStatus) {
                $scope.isLoading = false;
                alertService.error('Unable to connect to server');
            });
        };

        $scope.assignRolePermission = function (isNext) {
            $scope.isLoading = true;

            //prepare request for assigning permission
            var moduleFeaturePermissions = [];
            var copyOfModuleFeatures = angular.copy($scope.moduleFeatures);
            var modulePermissions = angular.copy($scope.modules);
            var hasModulePermission = false;
            var hasFeaturePermission = false;
            var assignedModulePermissions = [];
            angular.forEach(modulePermissions, function (item) {


                if (item.Selected == true) {
                    assignedModulePermissions.push(permission);
                    hasModulePermission = true;
                }


            });
            modulePermissions = assignedModulePermissions;
            var assignPermission = {
                role: {
                    Role: { RoleID: $scope.roleId },
                    Modules: modulePermissions
                    // Features: moduleFeaturePermissions
                }
            };

            roleService.assignRolePermission(assignPermission).then(function (data) {
                alertService.success('Permissions has been assigned to Feature and Module.');
                $scope.isLoading = false;
                if (isNext != null)
                    $scope.next();
                resetForm();
            },
            function (errorStatus) {
                $scope.isLoading = false;
                alertService.error('Unable to connect to server');
            });
        };

        $scope.initRole = function () {
            $scope.newRole = {};
            $scope.newRole.Name = '';
            resetForm();
        };

        $scope.add = function () {
            $scope.Goto("siteadministration.rolemanagement.role.roledetails");
        };

        $scope.edit = function (roleID) {
            $scope.Goto("siteadministration.rolemanagement.role.roledetails", { RoleId: roleID });
        };

        $scope.cancel = function () {
            $rootScope.onCancel(function (result) {
                if (result) {
                    //$scope.initRole();
                    $scope.editMode = false;
                    $('#roleModel').modal('hide');
                    $scope.setShortCutKeys(true);
                    $scope.getRoles();
                }
            });
        };

        $scope.remove = function (id, $event) {
            $event.stopPropagation();
            bootbox.confirm("Are you sure you want to delete?", function (result) {
                if (result == true) {
                    roleService.remove(id).then(function (data) {
                        $scope.getRoles();
                        alertService.success('Role has been deleted.');
                    },

                    function (errorStatus) {
                        alertService.error('OOPS! Something went wrong');
                    });
                }
            });
        };

        $scope.setAction = function (actionName, action) {
            $scope.currentAction = action;
            if (actionName != '') {
                $scope.currentWizardStep = 0;
                var nextWizardStep = 0;
                var isFound = false;
                angular.forEach($scope.wizardSteps, function (item) {
                    if (!isFound) {
                        if (item.actionName == actionName) {
                            $scope.currentWizardStep = nextWizardStep;
                            isFound = true;
                        }
                        nextWizardStep++;
                    }
                });
            }

        }

        $scope.save = function (isNext, mandatory, hasErrors) {
            // if there are errors, the screen is optional, and the user is trying to go to the next screen, don't bother trying to save anything
            if (!mandatory && isNext && hasErrors) {
                $scope.next();
            }

            if ((!mandatory && !hasErrors && !isNext) || (mandatory && !hasErrors && !isNext) || (!mandatory && !hasErrors && isNext) || (mandatory && !hasErrors && isNext)) {
                //var isDirty = formService.isDirty();

                //    if (isDirty) {
                //    if ($scope.roleId == 0) {
                //        $scope.currentAction = $scope.action.Role;
                //    }

                if ($scope.currentAction == $scope.action.Role) {
                    $scope.saveRole(isNext);
                } else if ($scope.currentAction == $scope.action.AssignModule) {
                    $scope.assignModuleToRole(isNext);
                } else {
                    $scope.assignRolePermission(isNext);
                }
            } else
                if (isNext) {
                    $scope.next();
                }
        }
        // };

        $scope.saveRole = function (isNext) {
            $scope.isLoading = true;
            if ($scope.newRole.RoleID != null && $scope.newRole.RoleID != 0) {
                roleService.update($scope.newRole).then(function (data) {
                    $scope.getRoles();

                    $scope.getRoleById();
                    alertService.success('Role has been updated successfully.');
                    $scope.isLoading = false;

                    if (isNext != null)
                        $scope.next();

                },
                function (errorStatus) {
                    alertService.error('OOPS! Something went wrong');
                    $scope.isLoading = false;
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
                        $scope.roleId = xml;//$(xml).find("Identifier").text();
                        $scope.newRole.RoleID = $scope.roleId;

                        $scope.getRoles();

                        alertService.success('Role has been added successfully.');
                        $scope.isLoading = false;

                        if (isNext != null)
                            $scope.next();
                    }

                },

                function (errorStatus) {
                    alertService.error('OOPS! Something went wrong');
                    $scope.isLoading = false;
                });
            }
        };
        $scope.next = function () {
            $scope.Goto("siteadministration.rolemanagement.role.assignmodules", { RoleId: $scope.roleId });

        }
        
        $scope.getRoles();
        $scope.initializeBootstrapTable();
        setTimeout(function () { $scope.InitRightNav(); }, 50);
    }]);