angular.module('xenatixApp')
    .controller('adminController', ['$scope', '$modal', 'adminService', 'alertService', 'lookupService', '$filter', '$rootScope', 'formService',
function ($scope, $modal, adminService, alertService, lookupService, $filter, $rootScope, formService) {
    $scope.isLoading = true;
    $scope.users = [];
    $scope.userroles = [];
    $scope.userCredentials = [];
    $scope.activesch = true;
    $scope.selectedRoles = [];
    $scope.selectedCredentials = [];
    $scope.$parent['autoFocus'] = true;
    $scope.emailPattern = /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/i;
    $scope.userSch = '';
    $scope.usersData = false;
    $scope.userId = 0;
    $scope.currentWizardStep = 1;
    $scope.wizardSteps = [];
    $scope.userCredential = {};
    $scope.credentials = [];
    $scope.user = {};
    $scope.stopSave = true;
    $scope.enterKeyStop = true;
    //For Focus
    $scope.userNameIndex = 1;
    $scope.rolesIndex = 2;
    $scope.crediantialsIndex = 3;
    $scope.autoFocusEdit = [];

    // Date picker settings
    $scope.opened = false;
    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1,
        showWeeks: false
    };
    $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'MM/dd/yyyy'];
    $scope.format = $scope.formats[3];
    $scope.endDate = new Date();
    $scope.startDate = new Date();
    $scope.$parent['autoFocus'] = true;

    var usersTable = $("#usersTable");

    // Scroll the table horizontally back to the left after pagination
    $('#usersTable').on('all.bs.table', function (e, name, args) {
        $('.fixed-table-body').scrollLeft(0);
    });


    $scope.setShortCutKeys = function (flag) {
        $scope.stopSave = flag;
        $scope.enterKeyStop = flag;
    }

    //This is for display and doesn't save anything updating the date and time values to display using 'useLocal'
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
                    field: 'UserName',
                    title: 'User Name'
                },
                {
                    field: 'FirstName',
                    title: 'First Name'
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
                    field: 'IsActive',
                    title: 'Active',
                    formatter: function (value, row, index) {
                        var isActive = $filter('toYesNo')(value);
                        return isActive;

                    }
                },
                {
                    field: 'EffectiveToDate',
                    title: 'Expiration Date',
                    formatter: function (value, row, index) {
                        if (value) {
                            var expirationDate = $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY', 'useLocal');
                            return expirationDate;
                        } else
                            return '';
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
                    title: 'LoginAttempts'
                },
                {
                    field: 'UserID',
                    title: '',
                    formatter: function (value, row, index) {
                        return '<span class="text-nowrap"><a data-default-action href="javascript:void(0)" ng-click=edit(' + value + ')  security permission-key="SiteAdministration-StaffManagement-UserDetails" permission="update" id="aedit" name="aedit"  title="Edit User"  space-key-press><i class="fa fa-pencil fa-fw padding-left-small padding-right-small"></i></a>' +
                            (row.IsActive?
                               '<a  data-default-no-action href="javascript:void(0)" ng-click=remove(' + value + ',$event) id="aremove" name="aremove" title="Deactivate User"  security permission-key="SiteAdministration-StaffManagement-UserDetails" permission="delete"  space-key-press><i class="fa fa-trash fa-fw padding-left-small padding-right-small"></i></a>'
                               :
                               '<a data-default-no-action href="javascript:void(0)" ng-click=activate(' + value + ',$event) id="aactivate" name="aactivate" title="Activate User" security permission-key="SiteAdministration-StaffManagement-UserDetails" permission="update"  space-key-press><i class="fa fa-bolt fa-fw padding-left-small padding-right-small"></i></a>') +

                               '<a data-default-no-action href="javascript:void(0)" ' + (row.IsActive ? 'ng-click=reset(' + value + ',$event,' + row.IsActive + ')' : '') + ' id="areset" name="areset" title="Reset Password" security permission-key="SiteAdministration-StaffManagement-UserDetails" permission="update"  space-key-press><i class="fa fa-undo fa-fw padding-left-small padding-right-small"></i></a>' +
                            (row.LoginAttempts>3 ?
                               '<a data-default-no-action href="javascript:void(0)" ng-click=unlock(' + value + ',$event) id="aunlock" name="aunlock" title="Unlock User"  security permission-key="SiteAdministration-StaffManagement-UserDetails" permission="update"  space-key-press><i class="fa fa-unlock fa-fw padding-left-small padding-right-small"></i></a>' : "") +
                            '</span>';
                    }
                }
            ]
        };
    };

    $scope.get = function () {
        $scope.isLoading = true;
        if (typeof ($scope.userSch) == "undefined") {
            $scope.userSch = '';
        }

        $scope.usersData = false;
        adminService.get($scope.userSch).then(function (data) {
            if (data.ResultCode != 0) {
                alertService.error('Error while getting user data');
            }

            $scope.users = data.DataItems;

            if (data != null && data.DataItems != null) {
                usersTable.bootstrapTable('load', data.DataItems);
                $scope.$parent['autoFocus'] = true;
            } else {
                usersTable.bootstrapTable('removeAll');
                $scope.$parent['autoFocus'] = true;
            }

            $scope.usersData = true;

            $scope.isLoading = false;
            resetForm();
        },

        function (errorStatus) {
            $scope.isLoading = false;
            alertService.error('Error while getting user data: ' + errorStatus);
        });
    };

    $scope.initUser = function () {
        $scope.newUser = {
            UserID: '',
            UserName: '',
            IsActive: true,
            EffectiveToDate: '',
            LoginAttempts: '',
            Roles: [],
            Credentials: [],
            PrimaryEmail: '',
            EmailID: ''
        };
    };

    function resetCredentialFields() {
        $scope.userCredential.UserCredentialID = null;
        $scope.userCredential.Name = '';
        $scope.userCredential.LicenseNbr = '';
        $scope.userCredential.EffectiveDate = '';
        $scope.userCredential.ExpirationDate = '';
        $scope.selectedCredentials = [];
        adminService.getUserCredentials($scope.newUser).then(function (data) {
            $scope.userCredentials = data.DataItems;
            for (var i = 0; i < $scope.userCredentials.length; i++) {
                if ($scope.userCredentials[i].UserID > 0) {
                    $scope.selectedCredentials.push({
                        CredentialID: $scope.userCredentials[i].CredentialID,
                        Name: $scope.userCredentials[i].Name,
                        Description: $scope.userCredentials[i].Description
                    });
                }
            }
            $scope.isLoading = false;
            resetForm();
        });
        //setTimeout(function () {
        //    $('#credentialName').focus();
        //}, 500);
    }

    $scope.add = function () {
        //$('#adminModel').on('shown.bs.modal', function () {
        //    setTimeout(function () {
        //        $('#username').focus();
        //    }, 500);
        //});

        $scope.currentWizardStep = 1;
        $scope.wizardSteps = [];
        var wizardStepCount = 0;
        $scope.wizardSteps.push({ actionName: 'userDetails', wizardStep: ++wizardStepCount });
        $scope.wizardSteps.push({ actionName: 'roles', wizardStep: ++wizardStepCount });
        $scope.wizardSteps.push({ actionName: 'credentials', wizardStep: ++wizardStepCount });

        $scope.selectedRoles = [];
        $scope.selectedCredentials = [];
        $scope.currentWizardStep = 1;
        $scope.editMode = false;
        $scope.initUser();
        $scope.newUser.UserID = 0;
        $scope.userId = 0;
        adminService.getUserRoles($scope.newUser).then(function (data) {
            $scope.userroles = data.DataItems;
            for (var i = 0; i < $scope.userroles.length; i++) {
                if ($scope.userroles[i].UserID > 0) {
                    $scope.selectedRoles.push({ RoleID: $scope.userroles[i].RoleID, Name: $scope.userroles[i].Name, Description: $scope.userroles[i].Description });
                }
            }
            $scope.isLoading = false;
            resetForm();
            $scope.autoFocusEdit[$scope.userNameIndex] = true;
        });

        $('#adminModel').modal('show').on('shown.bs.modal', function () {
            $('#username').focus();
            $scope.userCredentials = [];            
        });
        $('.nav-tabs a[href="#userdetails"]').tab('show');


    };

    var getUser = function (UserID) {
        return angular.copy($filter('filter')($scope.users, { UserID: UserID }, true)[0]);
    };

    $scope.edit = function (UserID) {
        //$('#adminModel').on('shown.bs.modal', function () {
        //    setTimeout(function () {
        //        $('#username').focus();
        //    }, 500);
        //});

        //Prepare the wizard when the modal is opened

        var user = getUser(UserID);
        $scope.currentWizardStep = 1;
        $scope.wizardSteps = [];
        var wizardStepCount = 0;
        $scope.wizardSteps.push({ actionName: 'userDetails', wizardStep: ++wizardStepCount });
        $scope.wizardSteps.push({ actionName: 'roles', wizardStep: ++wizardStepCount });
        $scope.wizardSteps.push({ actionName: 'credentials', wizardStep: ++wizardStepCount });

        $scope.selectedRoles = [];
        $scope.selectedCredentials = [];
        $scope.editMode = true;
        $scope.newUser = user;
        $scope.userId = $scope.newUser.UserID;
        $scope.userCredential.UserID = user.UserID;
        $scope.newUser.EffectiveToDate = $filter('toMMDDYYYYDate')($scope.newUser.EffectiveToDate, 'MM/DD/YYYY');
        adminService.getUserRoles($scope.newUser).then(function (data) {
            $scope.userroles = data.DataItems;
            for (var i = 0; i < $scope.userroles.length; i++) {
                if ($scope.userroles[i].UserID > 0) {
                    var role = $filter('filter')($scope.selectedRoles, { RoleID: $scope.userroles[i].RoleID })[0];
                    if (!role) {
                        $scope.selectedRoles.push({ RoleID: $scope.userroles[i].RoleID, Name: $scope.userroles[i].Name, Description: $scope.userroles[i].Description });
                    }

                }
            }
            $scope.isLoading = false;
            resetForm();
        });
        adminService.getUserCredentials($scope.newUser).then(function (data) {
            $scope.userCredentials = data.DataItems;
            for (var i = 0; i < $scope.userCredentials.length; i++) {
                if ($scope.userCredentials[i].UserID > 0) {
                    $scope.selectedCredentials.push({
                        CredentialID: $scope.userCredentials[i].CredentialID,
                        Name: $scope.userCredentials[i].Name,
                        Description: $scope.userCredentials[i].Description,
                        LicenseNbr: $scope.userCredentials[i].LicenseNbr,
                        EffectiveDate: $scope.userCredentials[i].EffectiveDate,
                        ExpirationDate: $scope.userCredentials[i].ExpirationDate
                    });
                }
            }
            $scope.isLoading = false;
            resetForm();
            $scope.autoFocusEdit[$scope.userNameIndex] = true;

            $('#adminModel').modal('show');
        });

        $('#adminModel').modal('show').on('shown.bs.modal', function () {
            $('#username').focus();
            $scope.setShortCutKeys(false);
        });
        $('.nav-tabs a[href="#userdetails"]').tab('show');

    };

    $scope.getLookupsByType = function (typeName) {
        return lookupService.getLookupsByType(typeName);
    };

    $scope.editUserCredential = function (credential) {
        $scope.userCredential.UserCredentialID = credential.UserCredentialID;
        $scope.userCredential.CredentialID = credential.CredentialID;
        $scope.userCredential.Name = credential.Name;
        $scope.userCredential.LicenseNbr = credential.LicenseNbr;
        $scope.userCredential.EffectiveDate = $filter('toMMDDYYYYDate')(credential.EffectiveDate, 'MM/DD/YYYY');
        $scope.userCredential.ExpirationDate = $filter('toMMDDYYYYDate')(credential.ExpirationDate, 'MM/DD/YYYY');

        $scope.isLoading = false;
    };

    $scope.removeUserCredential = function (userCredentialID) {
        var result = window.confirm("Selected User's Credential will be deleted.Do you want to continue?");
        if (result == false) {
            return;
        }
        adminService.removeUserCredential(userCredentialID).then(function () {
            alertService.success('User Credential has been removed.');
            resetCredentialFields();
        },

        function (errorStatus) {
            alertService.error('OOPS! Something went wrong');
        });
    };

    $scope.cancel = function () {

        $rootScope.onCancel(function (result) {
            if (result) {
                $scope.initUser();
                $scope.editMode = false;
                $scope.roleEditMode = true;
                $scope.credentialEditMode = true;
                $scope.selectedRoles = [];
                $scope.get();
                $('#txtusernamesch').focus();
                $('#adminModel').modal('hide');
                $scope.setShortCutKeys(true);
            }
        });
    };

    $scope.setAction = function (actionName) {
        if ($scope.userId > 0) {
            $scope.currentWizardStep = 1;
            var nextWizardStep = 1;
            var isFound = false;
            angular.forEach($scope.wizardSteps, function (item) {
                if (!isFound) {
                    if (item.actionName === actionName) {
                        $scope.currentWizardStep = nextWizardStep;
                        isFound = true;
                    }
                    nextWizardStep++;
                }
            });
        }

        if (actionName == 'credentials') {
            $scope.currentWizardStep = 3;
            setTimeout(function () {
                $('#credentialName').focus();
            }, 500);
        }

        $('.nav-tabs a[href="#' + actionName + '"]').tab('show');
        $scope.autoFocusEdit[$scope.crediantialsIndex] = true;
    }

    resetForm = function () {
        $rootScope.formReset($scope.ctrl.userForm);

    };

    $scope.next = function () {
        var nextWizardStep = $scope.currentWizardStep + 1;
        if ($scope.currentWizardStep === $scope.wizardSteps.length) {
            $('#adminModel').modal('hide');
            $scope.setShortCutKeys(true);
        }
        else {
            angular.forEach($scope.wizardSteps, function (item) {
                if (item.wizardStep === nextWizardStep) {
                    $scope.currentWizardStep = nextWizardStep;
                    $('.nav-tabs a[href="#' + item.actionName + '"]').tab('show');
                    $scope.$evalAsync(function () {
                        $scope.autoFocusEdit[nextWizardStep] = true;
                    });
                }
            });
        }
    };

    var isError = false; //Check: if an error or a warning comes from server do not close or move next tab on modal.
    $scope.save = function (isNext, mandatory, hasErrors) {
        var isDirty = formService.isDirty();

        if (isDirty && !hasErrors) {
            if ($scope.editMode) {
                $scope.newUser.Roles = $scope.selectedRoles;
                adminService.update($scope.newUser).then(function (data) {
                    $scope.editMode = true;

                    // only add or update a user credential, if a credential is selected
                    if ($scope.currentWizardStep == 3) {
                        //validation for expiration date and mandatory fields
                        if ($scope.checkExpiration()) { 
                            if ($scope.userCredential.UserCredentialID && $scope.userCredential.UserCredentialID > 0) {

                                adminService.updateUserCredential($scope.userCredential).then(function (data) {
                                    if (data.ResultCode != 0) {
                                        alertService.error(data.ResultMessage);
                                        isError = true;
                                    } else {
                                        alertService.success('Credential has been updated.');
                                        isError = false;
                                        resetCredentialFields();
                                    }
                                });
                            } else {
                                adminService.addUserCredential($scope.userCredential).then(function (data) {
                                    if (data.ResultCode != 0) {
                                        alertService.error(data.ResultMessage);
                                        isError = true;
                                    } else {
                                        alertService.success('Credential has been added.');
                                        isError = false;
                                        resetCredentialFields();

                                    }
                                });
                            }
                        }
                    } else {
                        if (data.ResultCode != 0) {
                            alertService.error(data.ResultMessage);
                            isError = true;
                        }
                        else {
                            alertService.success('User has been updated.');
                            isError = false;
                            $scope.get();
                        }
                    }


                    if (!isError && isNext) {
                        $scope.next();
                    }
                },

                function (errorStatus) {
                    alertService.error('Error while saving the user: ' + errorStatus);
                });
            }
            else {
                $scope.newUser.Roles = $scope.selectedRoles;
                adminService.add($scope.newUser).then(function (data) {
                    if (data.ResultCode !== 0) {
                        alertService.error(data.ResultMessage);
                        isError = true;
                    }
                    else if (data.ResultCode === 0 && data.ResultMessage.indexOf("Error while sending the email") > -1) {
                        alertService.success('User has been added. Email failed to send.');
                        isError = false;
                        // reset form so when tabs are changed or something is saved, we can call the $rootScope.formReset so we know when something has been changed again 
                        resetForm();
                    }
                    else {
                        alertService.success('User has been added.');

                        var xml = data.AdditionalResult;
                        $scope.userId = $(xml).find("UserIdentifier").text();
                        $scope.newUser.UserID = $scope.userId;
                        $scope.userCredential.UserID = $scope.userId;
                        $scope.newUser.EmailID = $(xml).find("EmailIdentifier").text();

                        $scope.editMode = true;
                        isError = false;
                        $scope.get();

                    }

                    if (!isError && isNext)
                        $scope.next();
                },

                function (errorStatus) {
                    alertService.error('Error while adding the user: ' + errorStatus);
                });
            }
        } else {
            if (isNext)
                $scope.next();
        }
    };

    $scope.initUserCredential = function () {
        $scope.userCredential = {};
        $scope.userCredential.CredentialName.autoFocus = true;
    }

    $scope.selectCredential = function (credential) {
        $scope.userCredential.CredentialID = credential.CredentialID;
    }

    $scope.addUserCredential = function () {
        if ($scope.checkExpiration()) {
            adminService.addUserCredential($scope.userCredential).then(function (data) {
                if (data.ResultCode != 0) {
                    alertService.error(data.ResultMessage);
                } else {
                    alertService.success('Credential has been added.');
                    resetCredentialFields();
                }
            });
        }
    }

    $scope.remove = function (UserID, $event) {
        $event.stopPropagation();
        bootbox.confirm("Are you sure that you want to deactivate this user?", function (result) {
            if (result === true) {
                var user = getUser(UserID);
                adminService.remove(user).then(function (data) {
                    $scope.editMode = false;
                    $scope.get();

                    if (data.ResultCode != 0) {
                        alertService.error('Error while deactivating the user.');
                    }
                    else {
                        alertService.success('User has been deactivated.');
                    }
                },

                function (errorStatus) {
                    alertService.error('Error while deactivating the user: ' + errorStatus);
                });
            }
        });
    };

    $scope.activate = function (UserID, $event) {
        $event.stopPropagation();
        bootbox.confirm("Are you sure that you want to activate this user?", function (result) {
            if (result === true) {
                var user = getUser(UserID);
                adminService.activate(user).then(function (data) {
                    $scope.editMode = false;
                    $scope.get();

                    if (data.ResultCode != 0) {
                        alertService.error('Error while activating the user');
                    }
                    else {
                        alertService.success('User has been activated.');
                    }
                },

            function (errorStatus) {
                alertService.error('Error while activating the user');
            });
            }
        });
    };

    $scope.reset = function (UserID, $event, isActive) {
        if (isActive) {
            $event.stopPropagation();
            bootbox.confirm("Are you sure that you want to reset this user's password?", function (result) {
                if (result == true) {
                    var user = getUser(UserID);
                    adminService.reset(user.PrimaryEmail).then(function (data) {
                        $scope.editMode = false;
                        $scope.get();

                        if (data.ResultCode != 0) {
                            alertService.error(data.ResultMessage);
                        }
                        else {
                            alertService.success('User reset email has been sent.');
                        }
                    },

                function (errorStatus) {
                    alertService.error('Error while resetting the password');
                });
                }
            });
        }
        else
            alertService.error('Unable to reset password because user is inactive');
    };

    $scope.unlock = function (UserID, $event) {
        $event.stopPropagation();
        bootbox.confirm("Are you sure that you want to unlock this user?", function (result) {
            if (result == true) {
                var user = getUser(UserID);
                adminService.unlock(user).then(function (data) {
                    $scope.editMode = false;
                    $scope.get();

                    if (data.ResultCode != 0) {
                        alertService.error('Error while unlocking the account');
                    }
                    else {
                        alertService.success('The login attempts have been reset.');
                    }
                },

               function (errorStatus) {
                   alertService.error('Error while unlocking the account: ' + errorStatus);
               });
            }
        });
    };

    $scope.toggleSelection = function toggleSelection(roleID) {
        var idx = -1;

        for (var i = 0; i < $scope.selectedRoles.length; i++) {
            if ($scope.selectedRoles[i].RoleID == roleID) {
                idx = i;
            }
        }

        if (idx > -1) {
            $scope.selectedRoles.splice(idx, 1);
        }
        else {
            $scope.selectedRoles.push({ RoleID: roleID, Name: '', Description: '' });
        }
    }

    $scope.today = function () {
        $scope.dt = new Date();
    };

    $scope.today();

    $scope.clear = function () {
        $scope.dt = null;
    };

    // Disable weekend selection
    $scope.disabled = function (date, mode) {
        return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
    };

    $scope.toggleMin = function () {
        $scope.minDate = $scope.minDate ? null : new Date();
    };
    $scope.toggleMin();

    $scope.open = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.opened = true;
    };

    $scope.open = function ($event, which) {
        $event.preventDefault();
        $event.stopPropagation();

        $scope.datepickers.isopen[which] = true;
    };

    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1
    };

    var tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);
    var afterTomorrow = new Date();
    afterTomorrow.setDate(tomorrow.getDate() + 2);
    $scope.events =
      [
        {
            date: tomorrow,
            status: 'full'
        },
        {
            date: afterTomorrow,
            status: 'partially'
        }
      ];

    $scope.getDayClass = function (date, mode) {
        if (mode === 'day') {
            var dayToCheck = new Date(date).setHours(0, 0, 0, 0);

            for (var i = 0; i < $scope.events.length; i++) {
                var currentDay = new Date($scope.events[i].date).setHours(0, 0, 0, 0);

                if (dayToCheck === currentDay) {
                    return $scope.events[i].status;
                }
            }
        }

        return '';
    };

    $scope.checkExpiration = function () {
        if ($scope.userCredential != null) {
            var currentDate = new Date().getTime();
            var expiryDate = new Date($scope.userCredential.ExpirationDate).getTime();
            var effectiveDate = new Date($scope.userCredential.EffectiveDate).getTime();
            if ($scope.userCredential.ExpirationDate != null && $scope.userCredential.ExpirationDate != '' && (expiryDate < effectiveDate || expiryDate < currentDate)) {
                $scope.userCredential.ExpirationDate = null;
                alertService.error("Expiration Date can't be less than Effective Date or Today's Date");
                return false;
            }
            else if (typeof $scope.userCredential.Name === 'undefined') {
                alertService.error("Please enter valid credential name");
                return false;
            }
            else if ($scope.userCredential.Name == "" || $scope.userCredential.Name == null) {
                alertService.error("Please enter credential name");
                return false;
            }
            else
                return true;
        }
    };

    $scope.get();
    $scope.initializeBootstrapTable();
}]);