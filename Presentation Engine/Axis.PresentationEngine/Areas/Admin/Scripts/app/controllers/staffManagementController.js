angular.module('xenatixApp')
    .controller('staffManagementController', [
        '$http', '$scope', '$rootScope', '$filter', '$timeout', 'staffManagementService', 'alertService', '$injector', '$state', function ($http, $scope, $rootScope, $filter,$timeout, staffManagementService, alertService, $injector, $state) {

            $scope.init = function () {
                $scope.$parent['autoFocus'] = true;
                $scope.staffManagementTable = $('#staffManagementTable');
                $scope.initializeBootstrapTable();
                $scope.get();
            };

            var profileState ='details';

            $('#staffManagementTable').on('all.bs.table', function (e, name, args) {
                $('.fixed-table-body').scrollLeft(0);
            });

            resetForm = function () {
                $rootScope.formReset($scope.ctrl.staffManagementForm);
            };

            var getUser = function (userID) {
                return angular.copy($filter('filter')($scope.users, { UserID: userID }, true)[0]);
            };

            $scope.get = function () {
                if (typeof ($scope.staffSearch) == "undefined") {
                    $scope.staffSearch = '';
                }
                staffManagementService.get($scope.staffSearch).then(function (data) {
                    //If the ResultCode equals 0 it indicates that staff members were retrieved successfully
                    if (data.ResultCode === 0) {
                        if (data.DataItems != undefined && data.DataItems.length >= 0) {
                            $scope.users = data.DataItems;
                            $scope.staffManagementTable.bootstrapTable('load', data.DataItems);
                            if ($injector.has('roleSecurityService')) {
                                var roleSecurityService = $injector.get('roleSecurityService');
                                var profileNavigationState = [{ Name: 'UserDetails', State: 'details', PermissionKey: 'SiteAdministration-StaffManagement-UserDetails' },
                                                                { Name: 'UserRoles', State: 'roles', PermissionKey: 'SiteAdministration-StaffManagement-UserRoles' },
                                                                { Name: 'UserCredentials', State: 'credentials', PermissionKey: 'SiteAdministration-StaffManagement-Credentials' },
                                                                { Name: 'DivisionProgram', State: 'divisionprogram', PermissionKey: 'SiteAdministration-StaffManagement-DivisionPrograms' },
                                                                { Name: 'Scheduling', State: 'scheduling', PermissionKey: 'SiteAdministration-StaffManagement-Scheduling' },
                                                                { Name: 'UserBlockedTime', State: 'blockedtime', PermissionKey: 'SiteAdministration-StaffManagement-BlockedTime' },
                                                                { Name: 'DirectReports', State: 'directreports', PermissionKey: 'SiteAdministration-StaffManagement-DirectReports' },
                                                                { Name: 'UserProfile', State: 'profile', PermissionKey: 'SiteAdministration-StaffManagement-UserProfile' },
                                                                { Name: 'Photos', State: 'photos', PermissionKey: 'SiteAdministration-StaffManagement-UserPhoto' },
                                                                { Name: 'AdditionalDetails', State: 'additionaldetails', PermissionKey: 'SiteAdministration-StaffManagement-AdditionalDetails' }];
                                for (var i = 0; i < profileNavigationState.length; i++) {
                                    if (roleSecurityService.hasPermission(profileNavigationState[i].PermissionKey, PERMISSION.READ)) {
                                        profileState = profileNavigationState[i].State;
                                        break;
                                    }
                                }
                            } else {
                                $scope.staffManagementTable.bootstrapTable('removeAll');
                            }
                            resetForm();
                        } 
                    }
                    //If ResultCode is not 0 then it indicates problem loading data
                    else {
                        alertService.error('Error while loading staff members.');
                    }
                });
            };

            $scope.edit = function (userID) {
                
                $state.go('.user.' + profileState, { UserID: userID }).then(function () {
                    $timeout(function () {
                        var obj = { stateName: $state.current.name, EnableAllWorkFlow: true, validationState: 'valid' };
                        $rootScope.staffManagementRightNavigationHandler(obj);
                    });
                    
                });
            };

            $scope.add = function () {
                $scope.Goto('.user.'+profileState);
            };

            $scope.deleteUser = function(userID, $event) {
                $event.stopPropagation();
                bootbox.confirm("Are you sure that you want to deactivate this user?", function (result) {
                    if (result) {
                        staffManagementService.deleteUser(userID).then(function(data) {
                            if (data.ResultCode === 0) {
                                alertService.success('User deactivated successfully.');
                                $scope.get();
                            } else {
                                alertService.error('Error while dectivating user.');
                            }
                        });
                    }
                });
            };

            $scope.activate = function (userID, $event) {
                $event.stopPropagation();
                bootbox.confirm("Are you sure that you want to activate this user?", function (result) {
                    if (result) {
                        staffManagementService.activate(userID).then(function (response) {
                            if (response.data.ResultCode === 0) {
                                alertService.success('User activated successfully.');
                                $scope.get();
                            } else {
                                alertService.error('Error while activating user.');
                            }
                        });
                    }
                });
            };

            $scope.reset = function (userID, $event) {
                $event.stopPropagation();
                bootbox.confirm("Are you sure that you want to reset this user's password?", function (result) {
                    if (result) {
                        var user = getUser(userID);
                        var primaryEmail = user.PrimaryEmail;
                        staffManagementService.reset(primaryEmail).then(function (response) {
                            if (response.data.ResultCode === 0) {
                                alertService.success('User\'s password reset successfully.');
                                $scope.get();
                            } else {
                                alertService.error(response.data.ResultMessage);
                            }
                        });
                    }
                });
            };
            
            $scope.unlock = function (userID, $event) {
                $event.stopPropagation();
                bootbox.confirm("Are you sure that you want to unlock this user?", function (result) {
                    if (result) {
                        staffManagementService.unlock(userID).then(function (response) {
                            if (response.data.ResultCode === 0) {
                                alertService.success('User unlocked successfully.');
                                $scope.get();
                            } else {
                                alertService.error('Error while unlocking user.');
                            }
                        });
                    }
                });
            };

            $scope.initializeBootstrapTable = function () {
                $scope.tableoptions = {
                    pagination: true,
                    pageSize: 10,
                    pageList: [10, 25, 50, 100],
                    search: false,
                    showColumns: true,
                    data: [],
                    undefinedText: '',
                    onClickRow: function (e, row, $element) {
                    },
                    columns: [
                        {
                            field: 'UserName',
                            title: 'User Name'
                        },
                        {
                            field: 'FirstName',
                            title: 'First Name'
                        },
                        {
                            field: 'MiddleName',
                            title: 'Middle Name'
                        },
                        {
                            field: 'LastName',
                            title: 'Last Name'
                        },
                        {
                            field: 'PrimaryEmail',
                            title: 'Email'
                        },
                        {
                            field: 'ADFlag',
                            title: 'AD User',
                            formatter: function(value) {
                                if (value === false) {
                                    return 'No';
                                } else {
                                    return 'Yes';
                                }
                            }
                        },
                        {
                            field: 'IsActive',
                            title: 'Active',
                            formatter: function (value, row, index) {
                                var isActive = $filter('toYesNo')(value);
                                return isActive;
                            }
                        },
                        {
                            field: 'LastLogin',
                            title: 'Last Login',
                            formatter: function (value, row, index) {
                                if (value) {
                                    var expirationDate = $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY', 'useLocal');
                                    return expirationDate;
                                } else
                                    return '';
                            }
                        },
                        {
                            field: 'LoginAttempts',
                            title: 'Login Attempts'
                        },
                        {
                            field: 'UserID', //Controller doing templating like directive???
                            title: '',
                            formatter: function (value, row, index) {
                                return '<span class="text-nowrap">' +
                                            //Edit Button - Pencil Icon
                                            '<a data-default-action href="javascript:void(0)" ng-click=edit(' + value +
                                                ') security permission-key="SiteAdministration-StaffManagement-UserDetails" permission="update" id="aedit" name="aedit" title="Edit User"' +
                                                'space-key-press><i class="fa fa-pencil fa-fw padding-left-small padding-right-small"></i>' +
                                            '</a>' +
                                            //Delete Button - Trash Icon
                                            (row.IsActive ?
                                                '<a data-default-no-action href="javascript:void(0)" ' +
                                                    'ng-click=deleteUser(' + value + ',$event) id="aremove" name="aremove" title="Deactivate User"' +
                                                    'ng-show = "' + !row.ADFlag + '"' +
                                                    'security permission-key="SiteAdministration-StaffManagement-UserDetails" permission="delete"' +
                                                    'space-key-press><i class="fa fa-trash fa-fw padding-left-small padding-right-small"></i>' +
                                                '</a>'
                                            :
                                                '<a data-default-no-action ' +
                                                    '"href="javascript:void(0)" ng-click=activate(' + value + ',$event) id="aactivate" name="aactivate" title="Activate User" ' +
                                                    'ng-show = "' + !row.ADFlag + '"' +
                                                    'security permission-key="SiteAdministration-StaffManagement-UserDetails" permission="update"' +
                                                    'space-key-press><i class="fa fa-bolt fa-fw padding-left-small padding-right-small"></i>' +
                                                '</a>') +
                                            //Reset Password Button - Refresh Icon
                                            (row.IsActive ?
                                                '<a data-default-no-action ' +
                                                    '"href="javascript:void(0)" ng-click=reset(' + value + ',$event) id="areset" name="areset" title="Reset Password"' +
                                                    'ng-show = "' + !row.ADFlag + '"' +
                                                    'security permission-key="SiteAdministration-StaffManagement-UserDetails" permission="update"' +
                                                    'space-key-press><i class="fa fa-undo fa-fw padding-left-small padding-right-small"></i>' +
                                                '</a>' : '') +
                                            //Unlock Button - Unlock Icon
                                            (row.LoginAttempts > 3 ?
                                                '<a data-default-no-action href="javascript:void(0)" ng-click=unlock(' + value + ',$event) id="aunlock" name="aunlock" title="Unlock User"' +
                                                    'security permission-key="SiteAdministration-StaffManagement-UserDetails" permission="update"' +
                                                    'space-key-press><i class="fa fa-unlock fa-fw padding-left-small padding-right-small"></i>' +
                                                '</a>' : "") +
                                        '</span>';
                            }
                        }
                    ]
                };
            };

            $scope.init();
        }
    ]);
