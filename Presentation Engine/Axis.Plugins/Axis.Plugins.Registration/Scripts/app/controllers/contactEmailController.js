angular.module('xenatixApp')
    .controller('contactEmailController', ['$scope', '$modal', '$filter', 'contactEmailService', 'alertService', 'settings', 'lookupService', '$stateParams', '$state', '$rootScope', 'formService','$controller',
        function ($scope, $modal, $filter, contactEmailService, alertService, settings, lookupService, $stateParams, $state, $rootScope, formService,$controller) {
            $controller('baseContactController', { $scope: $scope });
            var emailPermissionType = lookupService.getLookupsByType('EmailPermission');
            $scope.isLoading = true;
            $scope.emailPattern = /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/i;
            $scope.Email = {};
            var emailTable = $("#emailTable");
            var todayDate = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal');
            var date = new Date(todayDate);
            $scope.EmailAccessCode = $scope.EMAIL_ACCESS.Required | $scope.EMAIL_ACCESS.Email;
            resetForm = function () {
                $scope.emailAutoFocus = true;
                $rootScope.formReset($scope.ctrl.emailForm);
            };


            $scope.reset = function () {
                $scope.initEmails();
            }

            //Lookups for email types
            $scope.getLookupsByType = function (typeName) {
                return lookupService.getLookupsByType(typeName);
            };

            $scope.initEmails = function () {
                $scope.email = { ContactEmailID: 0 };

                $scope.Emails = [{
                    Email: '',
                    EmailID: 0,
                    EmailPermissionID: null,
                    IsPrimary: false,
                    EffectiveDate: todayDate,
                    ExpirationDate: null,
                    ContactEmailID: 0,
                    required: true
                }];
                $scope.ShowPrimaryEmailCheckbox = true;
                $scope.ShowEmailExpirationDates = true;
                resetForm();
            };

            $scope.init = function () {
                $scope.contactID = $stateParams.ContactID;
                $scope.initEmails();
                $scope.emailList = [];
                $scope.initializeBootstrapTable();

                if ($state.current.name.toLowerCase().indexOf('patientprofile') >= 0) {
                    $scope.enterKeyStop = true;
                    $scope.stopNext = false;
                    $scope.saveOnEnter = true;
                }
                else {
                    $scope.stopNext = false;
                    $scope.saveOnEnter = false;
                }
            }

            var getText = function (value, list) {
                if (value) {
                    var formattedValue = lookupService.getSelectedTextById(value, emailPermissionType);
                    if (formattedValue != undefined && formattedValue.length > 0)
                        return formattedValue[0].Name
                    else
                        return '';
                } else

                    return '';
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
                    //onClickRow: function (e, row, $element) {
                    //    row.find("[data-default-action]").triggerHandler('click');
                    //},
                    columns: [
                        {
                            field: 'Email',
                            title: 'Email'
                        },
                        {
                            field: 'EmailPermissionID',
                            title: 'Permission Type',
                            formatter: function (value, row, index) {
                                return getText(value, emailPermissionType);
                            }
                        },
                        {
                            field: 'IsPrimary',
                            title: 'Primary',
                            formatter: function (value, row, index) {
                                return value ? "Yes" : "No"
                            }
                        },
                        {
                            field: 'EffectiveDate',
                            title: 'Effective Date',
                            formatter: function (value, row, index) {
                                if (value) {
                                    var formattedDate = $filter('formatDate')(value, 'MM/DD/YYYY');
                                    return formattedDate;
                                } else
                                    return '';
                            }
                        },
                        {
                            field: 'ExpirationDate',
                            title: 'Expiration Date',
                            formatter: function (value, row, index) {
                                if (value) {
                                    var formattedDate = $filter('formatDate')(value, 'MM/DD/YYYY');
                                    return formattedDate;
                                } else
                                    return '';
                            }
                        },
                        {
                            field: 'ContactEmailID',
                            title: '',
                            formatter: function (value, row, index) {
                                return '<a href="javascript:void(0)" data-default-action id="editEmail" security permission-key="General-General-Email" permission="update" name="editEmail" data-toggle="modal" ng-click="edit(' + value + ')" title="View/Edit" space-key-press><i class="fa fa-pencil fa-fw" /></a>' +
                                       '<a href="javascript:void(0)" data-default-no-action ng-click="remove(' + value + ',$event)" security permission-key="General-General-Email" permission="delete" id="deleteEmail" name="deleteEmail" title="Deactivate" space-key-press><i class="fa fa-trash fa-fw"></i></a>';
                            }
                        }
                    ]
                };
            };

            $scope.get = function (contactId) {
                $scope.isLoading = true;
                $scope.init();
                return contactEmailService.get(contactId).then(function (data) {
                    $scope.emailList = data.DataItems;

                    if ($scope.emailList != null) {
                        emailTable.bootstrapTable('load', $scope.emailList);                        
                    } else {
                        emailTable.bootstrapTable('removeAll');
                    }
                    $scope.reset();
                    $scope.isLoading = false;
                },
                function (errorStatus) {
                    $scope.isLoading = false;
                    alertService.error('Unable to connect to server');
                });
            };

            $scope.edit = function (ContactEmailID) {
                $scope.Emails = [];
                var email = angular.copy($filter('filter')($scope.emailList, { ContactEmailID: ContactEmailID })[0]);
                email.EffectiveDate = email.EffectiveDate
                                                ? $filter('formatDate')(email.EffectiveDate, 'MM/DD/YYYY')
                                                : null;
                email.ExpirationDate = email.ExpirationDate
                                                ? $filter('formatDate')(email.ExpirationDate, 'MM/DD/YYYY')
                                                : null;
                $scope.email.ContactEmailID = ContactEmailID;
                $scope.Emails.push(email);
                resetForm();
            };

            $scope.remove = function (id, $event) {
                $event.stopPropagation();
                bootbox.confirm("Selected contact email will be deactivated.\n Do you want to continue?", function (result) {
                    if (result === true) {
                        contactEmailService.remove(id, $scope.contactID).then(function (data) {
                            $scope.get($scope.contactID).then(function () {
                                alertService.success('Contact email has been deactivated.');
                            });
                        },

                        function (errorStatus) {
                            alertService.error('Error while delete the user: ' + errorStatus);
                        });
                    }
                });
            };

            $scope.addUpdate = function (contactEmailModal) {
                contactEmailModal.ContactID = $scope.contactID;
                delete contactEmailModal.required
                $scope.isLoading = true;
                contactEmailModal.ModifiedOn = moment.utc();
                contactEmailService.addUpdate(contactEmailModal)
                    .then(
                        function (response) {
                            $scope.get($scope.contactID).then(function () {
                                if (contactEmailModal.ContactEmailID > 0) {
                                    alertService.success('Contact email has been updated.');
                                }
                                else {
                                    alertService.success('Contact email has been saved.');
                                }
                            });
                        },
                        function (errorStatus) {
                            $scope.isLoading = false;
                            alertService.error('OOPS! Something went wrong');
                        },
                        function (notification) {
                            alertService.warning(notification);
                        }).then(function () {
                            $scope.isLoading = false;
                        });
            }

            function showErrorExpireDate() {
                angular.element('#expirationDateError').addClass('has-error');
                angular.element('#isValidError').removeClass('ng-hide').addClass('ng-show');
            }

            function hideErrorExpireDate() {
                angular.element('#expirationDateError').removeClass('has-error');
                angular.element('#isValidError').removeClass('ng-show').addClass('ng-hide');
            }

            $scope.expirationDate = function (expiredate) {
                if (expiredate) {
                    var isValidDate = moment(expiredate).isValid();
                    var isValidEffectDate = moment($scope.Emails[0].EffectiveDate).isValid();
                    if (!isValidDate || !isValidEffectDate) {
                        alertService.error("Expiration date can't be less than effective date");
                        $scope.Emails[0].ExpirationDate = '';
                        return;
                    }

                    var expdate = new Date(expiredate);
                    var effDate = new Date($scope.Emails[0].EffectiveDate);
                    if (expdate < effDate && isValidDate && isValidEffectDate) {
                        showErrorExpireDate();
                        alertService.error("Expiration date can't be less than effective date");
                        $scope.Emails[0].ExpirationDate = '';
                    }
                    else {
                        hideErrorExpireDate();
                    }
                }
                else {
                    hideErrorExpireDate();
                }
            };

            $scope.save = function () {
                var isDirty = formService.isDirty();
                var modelToSave = angular.copy($scope.Emails);
                var emailList = angular.copy($scope.emailList);
                var contactEmailModal = modelToSave[0];
                var requiredToSave = false;

                angular.forEach(emailList, function (item, index) { //update list with updated email item
                    if (item.ContactEmailID == contactEmailModal.ContactEmailID) {
                        item = contactEmailModal;
                    }
                });
                //filter previous contact with primary key
                var prevcontactEmail = $filter('filter')(emailList, { ContactID: contactEmailModal.ContactID, IsPrimary: true });
                var emailPrimary = $filter('filter')(emailList, function (item) {
                    if (item.IsPrimary == true && item.EmailID != contactEmailModal.EmailID)
                        return true;
                    else
                        return false;
                });

                if (contactEmailModal.Email && contactEmailModal.Email != '') { requiredToSave = true; }
                else { alertService.error("Please provide a valid email address.") }
                if (isDirty && requiredToSave) {

                    var email = angular.copy($filter('filter')(emailList, { IsPrimary: true })[0]);

                    //Update the Effective and Expiration dates
                    contactEmailModal.EffectiveDate = $filter('formatDate')(contactEmailModal.EffectiveDate);
                    contactEmailModal.ExpirationDate = $filter('formatDate')(contactEmailModal.ExpirationDate);

                    if (contactEmailModal.IsPrimary && emailPrimary.length > 0) {
                        bootbox.confirm("You already have primary contact email. Do you want to override it?", function (result) {
                            if (result)
                                $scope.addUpdate(contactEmailModal);
                        });
                    }
                    else if (emailList.length == 1 && contactEmailModal.EmailID == emailList[0].EmailID) {
                        $scope.addUpdate(contactEmailModal);
                    }
                    else if (contactEmailModal.ContactEmailID && emailList.length > 0 && !contactEmailModal.IsPrimary && emailPrimary.length == 0) {
                            bootbox.confirm("You have unchecked primary and no other contact email is marked as primary. Are you sure you want to save this?", function (result) {
                                if (result)
                                    $scope.addUpdate(contactEmailModal);
                            });
                        } else {
                            $scope.addUpdate(contactEmailModal);
                        }
                    
                }
            };


            $scope.init();
            $scope.get($scope.contactID);
            $scope.$on('showDetails', function (event, args) {
                $scope.get($scope.contactID).then(function () {
                    setGridItem(emailTable, 'ContactEmailID', args.id);
                });
            });
        }]);