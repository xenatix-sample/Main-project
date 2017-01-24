(function () {
    angular.module('xenatixApp')
        .controller('userDirectReportsController', [
        '$http', '$scope', '$rootScope', '$filter', '$stateParams', '$state', 'userDirectReportsService', 'alertService', 'formService', 'userProfileService', '$timeout',
        function ($http, $scope, $rootScope, $filter, $stateParams, $state, userDirectReportsService, alertService, formService, userProfileService, $timeout) {
            var self = this;
            var usersTable = $("#usersTable");
            var directReportTable = $("#directReportTable");
            self.userProfile = ($state.current.name.indexOf('myprofile.nav.directreports') > -1);
            var isMyProfile = false;
            if (self.userProfile)
                isMyProfile = true;
            else
                $scope.permissionKey = $state.current.data.permissionKey;
            var init = function () {
                initFormDetails();
                get();    //gets grid data on page load
            };
            var disableEnableShortcutKeys = function (enterKeyStop, saveOnEnter) {
                $scope.enterKeyStop = enterKeyStop;
                $scope.stopNext = false;
                $scope.saveOnEnter = saveOnEnter;
            }
            var initFormDetails = function () {
                self.userDetail = {
                    MappingID: 0,
                    ParentID: self.CurrentUserID,
                    FirstName: '',
                    MiddleName: '',
                    LastName: '',
                    UserName: '',
                    Email: '',
                    IsSupervisor: false,
                    Program: '',
                    ProgramUnit: '',
                    ModifiedOn: new Date()
                };
            };
            self.searchStaff = function (searchText) {
                if (searchText && searchText.length > 0) {
                    disableEnableShortcutKeys(true, false);
                    self.getUsersByCriteria(searchText);
                }
                else {
                    disableEnableShortcutKeys(false, true);
                }
            }
            var getCurrentUserID = function () {
                userProfileService.get(isMyProfile).then(function (response) {
                    if (response && response.ResultCode == 0 && response.DataItems.length > 0) {
                        self.CurrentUserID = response.DataItems[0].UserID;
                        init();
                    }
                }, function (errorStatus) {
                    alertService.error('Error while getting data: ' + errorStatus);
                });
            };

            var resetForm = function () {
                $rootScope.formReset();
            };

            var setState = function () {
                var isValid = (self.reportList.length > 0) ? true : false;
                var stateDetail = { stateName: $state.current.name, validationState: (isValid ? 'valid' : 'warning') };
                $rootScope.staffManagementRightNavigationHandler(stateDetail);
            }

            self.stopEnterKey = function () {
                if (!$('#userListModel').hasClass('in')) {
                    if ($state.current.name.toLowerCase().indexOf('directreports') >= 0) {
                        disableEnableShortcutKeys(true, true);
                    }
                    else {
                        disableEnableShortcutKeys(false,true);
                    }
                }
                else
                    disableEnableShortcutKeys(true, false);
            };

            $scope.cancelModel = function () {
                $('#userListModel').modal('hide');
                self.searchText = '';
                disableEnableShortcutKeys(false, true);
            };

            var get = function () {
                userDirectReportsService.get(self.CurrentUserID, isMyProfile).then(getSuccessMethod, failureMethod, notificationMethod);
            };

            var getSuccessMethod = function (data) {
                var isValid = false;
                if (data && data.data && data.data.DataItems) {
                    //Populate directReportTable table
                    self.reportList = data.data.DataItems;
                    if (self.reportList) {
                        directReportTable.bootstrapTable('load', self.reportList);
                        isValid = true;
                        var obj = { stateName: $state.current.name, validationState: 'valid' };
                        $rootScope.staffManagementRightNavigationHandler(obj);
                    } else {
                        directReportTable.bootstrapTable('removeAll');
                    }
                    setState();
                    applyDropupOnGrid(false);
                }

                if (!isValid) {
                    var obj = { stateName: $state.current.name, validationState: 'warning' };
                    $rootScope.staffManagementRightNavigationHandler(obj);
                }
            };

            self.getUsersByCriteria = function (searchText) {
                if (searchText && searchText != '') {
                    userDirectReportsService.getUsersByCriteria(searchText, isMyProfile).then(searchSuccess, failureMethod, notificationMethod);
                } else {
                    self.stopEnterKey();
                }
            };

            var searchSuccess = function (data) {
                if (data && data.DataItems) {
                    //filter users having no supervisor and is not the current user
                    self.usersList = $filter('filter')(data.DataItems, function (resp) {
                        return (!resp.HasSupervisor && (resp.UserID !== self.CurrentUserID));
                    });

                    $('#userListModel').on('hidden.bs.modal', function () {
                        var focus = $('#FirstName').is(":focus");
                        if (!focus) {
                            $('#txtClientSearch').focus();
                        }
                    });

                    angular.forEach(self.usersList, function (item) {
                        item.DOB = item.DOB ? $filter('toMMDDYYYYDate')(item.DOB, 'MM/DD/YYYY') : "";
                    });
                    //Populate usersTable table
                    if (self.usersList) {
                        usersTable.bootstrapTable('load', self.usersList);
                        $('#userListModel').modal('show');
                        $('#userListModel').on('shown.bs.modal', function () {
                            disableEnableShortcutKeys(true, false);
                        });
                    } else {
                        usersTable.bootstrapTable('removeAll');
                    }
                    // This code is commented since Column Selector should be drop down ,so no need to call  applyDropupOnGrid method
                    // Now you can see all columns FirstName, Last Name, MI, Gender, program , Program Unit  
                    // applyDropupOnGrid(false);

                }
            };

            $scope.save = function (isNext, mandatory, hasErrors, keepForm, next) {
                if (!hasErrors && self.userDetail && formService.isDirty() && !self.userProfile) {
                    ////There will be no update...only insert or delete
                    userDirectReportsService.add(self.userDetail, isMyProfile).then(function (response) {
                        if (response && response.ResultCode === 0) {
                            alertService.success('User Direct Report saved successfully.');
                            if (isNext) {
                                $scope.handleNextState();
                            }
                            else {
                                initFormDetails();
                                resetForm();
                                get();
                            }
                        }
                        else if (response && response.ResultCode < 0 && response.ResultMessage === 'user has reached maximum supervisor limit') {
                            alertService.error('User already has supervisor.');
                            initFormDetails();
                            resetForm();
                        }
                        else if (response && response.ResultCode !== 0) {
                            failureMethod();
                        }
                    });
                }
                else if (!formService.isDirty() && isNext) {
                    $scope.handleNextState();
                }
            }

            $scope.handleNextState = function () {
                var nextState = angular.element("li[data-state-name].list-group-item.active").next("li[data-state-name].list-group-item");
                if (nextState.length === 0)
                    $scope.Goto('^');
                else {
                    $timeout(function () {
                        $rootScope.setform(false);
                        var nextStateName = nextState.attr('data-state-name');
                        $scope.Goto(nextStateName, { UserID: self.CurrentUserID });
                    });
                }
            };

            self.remove = function (mappingID) {
                bootbox.confirm("Selected record will be removed.\n Do you want to continue?", function (result) {
                    if (result) {
                        userDirectReportsService.remove(mappingID, isMyProfile).then(deleteSuccess, failureMethod, notificationMethod);
                    }
                });
            };

            var deleteSuccess = function (data) {
                if (data && data.ResultCode == 0) {
                    alertService.success('User Direct Report has been deleted successfully.');
                    get();
                }
                else {
                    failureMethod();
                }
            };

            self.edit = function (val) {
                if (val) {
                    //if user already exists, return with the message.
                    if (self.reportList && $filter('filter')(self.reportList, function (resp) { return (resp.UserID === val); }).length > 0) {
                        alertService.error('This user already exists on this direct report list.');
                        return;
                    }
                    //filter and get val from list bind with search Users list
                    var myUser = $filter('filter')(self.usersList, function (resp) { return (resp.UserID === val); });
                    if (myUser.length > 0) {
                        self.userDetail = myUser[0];
                        myUser[0].IsSupervisor = false;
                        self.userDetail.ParentID = self.CurrentUserID;
                    }
                    self.userDetail.MappingID = 0;
                    $scope.cancelModel();
                }
            }

            var failureMethod = function () {
                alertService.error('OOPS! Something went wrong');
                var obj = { stateName: $state.current.name, validationState: 'warning' };
                $rootScope.staffManagementRightNavigationHandler(obj);
            };

            var notificationMethod = function () {
                //Implement if required
            };

            var initializeBootstrapTable = function () {
                self.tableoptions = {
                    pagination: true,
                    pageSize: 10,
                    pageList: [10, 25, 50, 100],
                    search: false,
                    showColumns: true,
                    data: [],
                    undefinedText: '',
                    columns: [
                        {
                            field: 'FirstName',
                            title: 'First Name'
                        },
                        {
                            field: 'LastName',
                            title: 'Last Name'
                        },
                        {
                            field: 'MiddleName',
                            title: 'MI'
                        },
                        {
                            field: 'DOB',
                            title: 'Date Of Birth',
                            formatter: function (value, row, index) {
                                return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY', 'useLocal');
                            }
                        },
                        {
                            field: 'Gender',
                            title: 'Gender',
                            formatter: function (value, row, index) {
                                return (value ? ((value === 'Male') ? 'M' : 'F') : '');
                            }
                        },
                        {
                            field: 'Program',
                            title: 'Program'
                        },
                        {
                            field: 'ProgramUnit',
                            title: 'Program Unit'
                        },
                        {
                            field: 'UserID',
                            title: '',
                            formatter: function (value, row, index) {
                                return '<a href="javascript:void(0)" data-default-action id="fillStaff" security permission-key="SiteAdministration-StaffManagement-DirectReports" permission="update" name="fillStaff" data-toggle="modal" ng-click="ctrl.edit(' + value + ')" title="View/Edit" space-key-press><i class="fa fa-plus-circle" /></a>';
                            }
                        }
                    ]
                };

                self.reportTableoptions = {
                    pagination: true,
                    pageSize: 10,
                    pageList: [10, 25, 50, 100],
                    search: false,
                    showColumns: true,
                    data: [],
                    undefinedText: '',
                    columns: [
                        {
                            field: 'FirstName',
                            title: 'Name',
                            formatter: function (value, row, index) {
                                return row.FirstName + ' ' + row.LastName;
                            }
                        },
                        {
                            field: 'UserName',
                            title: 'User Name'
                        },
                        {
                            field: 'Email',
                            title: 'Email'
                        },
                        {
                            field: 'IsSupervisor',
                            title: 'Role',
                            formatter: function (value, row, index) {
                                return value ? "Supervisor" : "Direct Report";
                            }
                        },
                        {
                            field: 'Program',
                            title: 'Program'
                        },
                        {
                            field: 'MappingID',
                            title: '',
                            visible: !self.userProfile,
                            formatter: function (value, row, index) {
                                return '<a href="javascript:void(0)" data-default-no-action ng-click="ctrl.remove(' + value + ')" security permission-key="SiteAdministration-StaffManagement-DirectReports" permission="delete" id="delete" name="delete" title="Delete" space-key-press><i class="fa fa-trash fa-fw"></i></a>';
                            }
                        }
                    ]
                };
            };

            initializeBootstrapTable();

            if (!self.userProfile) {
                self.CurrentUserID = $stateParams.UserID;
                init();
            }
            else {
                getCurrentUserID();
            }

        }]);
}());