angular.module('xenatixApp')
    .controller('contactPhoneController', ['$scope', '$modal', '$filter', 'contactPhoneService', 'alertService', 'settings', 'lookupService', '$stateParams', '$state', '$rootScope', 'formService', '$controller',
         function ($scope, $modal, $filter, contactPhoneService, alertService, settings, lookupService, $stateParams, $state, $rootScope, formService, $controller) {
             $controller('baseContactController', { $scope: $scope });
             var loading = false;
             $scope.isLoading = true;
             var undefined = 'undefined';
             var phoneTypes = lookupService.getLookupsByType('PhoneType');
             var PhonePermissions = lookupService.getLookupsByType('PhonePermission');
             var phoneTable = $("#phoneTable");
             var todayDate = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal');
             var date = new Date(todayDate);

             initPhone = function () {
                 $scope.phone = {
                     ContactPhoneID: 0
                 };

                 $scope.Phones = [{
                     ContactPhoneID: 0,
                     PhoneTypeID: null,
                     PhonePermissionID: null,
                     Number: '',
                     Extension:'',
                     IsPrimary: false,
                     EffectiveDate: todayDate,
                     ExpirationDate: null
                 }];
                 $scope.ShowPrimaryCheckbox = true;
                 $scope.ShowPhoneExpirationDates = true;
                 $scope.autoContactPhoneFocus = true;
                 $scope.PhoneAccessCode = $scope.PHONE_ACCESS.Required | $scope.PHONE_ACCESS.Type | $scope.PHONE_ACCESS.Number;
                 resetForm();
             };

             $scope.reset = function () {
                 initPhone();
             }

             resetForm = function () {
                 $rootScope.formReset($scope.ctrl.contactPhoneForm);
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
                             field: 'Number',
                             title: 'Phone Number',
                             formatter: function (value, row, index) {
                                 return $filter('toPhone')(value);
                             }
                         },
                         {
                             field: 'PhoneTypeID',
                             title: 'Phone Type',
                             formatter: function (value, row, index) {
                                 return getText(value, phoneTypes);
                             }
                         },
                         {
                             field: 'PhonePermissionID',
                             title: 'Phone Permission',
                             formatter: function (value, row, index) {
                                 return getText(value, PhonePermissions);
                             }
                         },
                         {
                             field: 'IsPrimary',
                             title: 'Primary',
                             formatter: function (value, row, index) {
                                 return (value) ? 'Yes' : 'No';
                             }
                         },
                         {
                             field: 'EffectiveDate',
                             title: 'EffectiveDate',
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
                             title: 'ExpirationDate',
                             formatter: function (value, row, index) {
                                 if (value) {
                                     var formattedDate = $filter('formatDate')(value, 'MM/DD/YYYY');
                                     return formattedDate;
                                 } else
                                     return '';
                             }
                         },
                         {
                             field: 'ContactPhoneID',
                             title: '',
                             formatter: function (value, row, index) {
                                 return '<a href="javascript:void(0)" data-default-action security permission-key="General-General-Phone" permission="update" id="editPhone" name="editPhone" data-toggle="modal" ng-click="edit(' + value + ')" title="View/Edit" space-key-press><i class="fa fa-pencil fa-fw" /></a>' +
                                        '<a href="javascript:void(0)" data-default-no-action ng-click="remove(' + value + ')" security permission-key="General-General-Phone" permission="delete" id="deactivatePhone" name="deactivatePhone" title="Deactivate" space-key-press><i class="fa fa-trash fa-fw"></i></a>';
                             }
                         }
                     ]
                 };
             };

             $scope.init = function () {
                 $scope.isLoading = true;

                 $scope.contactID = $stateParams.ContactID;
                 initPhone();
                 if (($scope.contactID !== 0) && ($scope.contactID != null) && ($scope.contactID != 'undefined')) {
                     $scope.get($scope.contactID, 1);
                 }

                 $scope.initializeBootstrapTable();
                 loading = true;

                 if ($state.current.name.toLowerCase().indexOf('patientprofile') >= 0) {
                     $scope.enterKeyStop = true;
                     $scope.stopNext = false;
                     $scope.saveOnEnter = true;
                 }
                 else {
                     $scope.stopNext = false;
                     $scope.saveOnEnter = false;
                 }
             };

             getText = function (value, list) {
                 if (value) {
                     var formattedValue = lookupService.getSelectedTextById(value, list);
                     if (formattedValue != undefined && formattedValue.length > 0)
                         return formattedValue[0].Name
                     else
                         return '';
                 } else

                     return '';
             };

             $scope.get = function (contactID, contactTypeID) {
                 $scope.isLoading = true;

                 contactPhoneService.get(contactID, contactTypeID).then(function (data) {
                     $scope.phoneList = data.DataItems;
                     if ($scope.phoneList != null) {
                         phoneTable.bootstrapTable('load', $scope.phoneList);
                     } else {
                         phoneTable.bootstrapTable('removeAll');
                     }
                     resetForm();
                     $scope.isLoading = false;
                 },
                 function (errorStatus) {
                     $scope.isLoading = false;
                     alertService.error('Unable to connect to server');
                 });
             };

             $scope.edit = function (contactPhoneId, index) {
                 $scope.Phones = [];
                 contactPhone = angular.copy($filter('filter')($scope.phoneList, { ContactPhoneID: contactPhoneId })[0]);
                 contactPhone.EffectiveDate = contactPhone.EffectiveDate
                                                 ? $filter('formatDate')(contactPhone.EffectiveDate, 'MM/DD/YYYY')
                                                 : null;
                 contactPhone.ExpirationDate = contactPhone.ExpirationDate
                                                 ? $filter('formatDate')(contactPhone.ExpirationDate, 'MM/DD/YYYY')
                                                 : null;
                 $scope.phone.ContactPhoneID = contactPhoneId;
                 $scope.Phones.push(contactPhone);
                 $scope.Phones[0].IsActive = true;
                 resetForm();
             };

             var isAnyChildFormDirty = function (childForm) {
                 var dirtyFormControls = [];

                 angular.forEach(childForm, function (value, key) {
                     if (typeof value === 'object' && value.hasOwnProperty('$modelValue') && value.modified)
                         dirtyFormControls.push(value)
                 });

                 return dirtyFormControls.length > 0 ? true : false;
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
                     var isValidEffectDate = moment($scope.Phones[0].EffectiveDate).isValid();
                     if (!isValidDate || !isValidEffectDate) {
                         alertService.error("Expiration date can't be less than effective date");
                         $scope.Phones[0].ExpirationDate = '';
                         return;
                     }

                     var expdate = new Date(expiredate);
                     var effDate = new Date($scope.Phones[0].EffectiveDate);
                     if (expdate < effDate && isValidDate && isValidEffectDate) {
                         showErrorExpireDate();
                         alertService.error("Expiration date can't be less than effective date");
                         $scope.Phones[0].ExpirationDate = '';
                     }
                     else {
                         hideErrorExpireDate();
                     }
                 }
                 else {
                     hideErrorExpireDate();
                 }
             };

             $scope.save = function (isNext, isMandatory, hasErrors) {

                 var isDirty = formService.isDirty();
                 $scope.Phones[0].ContactID = $stateParams.ContactID;
                 var contactPhoneModel = angular.copy($scope.Phones[0]);
                 var contactPhoneList = angular.copy($scope.phoneList);

                 if (!('PhoneID' in contactPhoneModel))
                 { contactPhoneModel.PhoneID = 0; }
                 //var requiredToSave = false;

                 //if (contactPhoneModel.Number != '' && contactPhoneModel.PhoneTypeID != null) {
                 //    requiredToSave = true;
                 //}
                 //else {
                 //    alertService.error("Please provide Number and Phone Type.")
                 //}

                 if ((isDirty || isAnyChildFormDirty($scope.ctrl.contactPhoneForm.phoneForm)) && !hasErrors) {


                     angular.forEach(contactPhoneList, function (item, index) { //update list with phone item to be updated
                         if (item.ContactPhoneID == contactPhoneModel.ContactPhoneID) {
                             item = contactPhoneModel;
                         }
                     });
                     //filter previous contact with primary key
                     var prevcontactPhone = $filter('filter')(contactPhoneList, { ContactID: contactPhoneModel.ContactID, IsPrimary: true });

                     var modifiedObjects = $scope.ctrl.contactPhoneForm.phoneForm.modifiedModels;
                     var modifiedIsPrimary = $filter('filter')(modifiedObjects, function (item) { if (item.$name.indexOf('isPrimary') >= 0) return true; })

                     var phonePrimary = $filter('filter')(contactPhoneList, function (item) {
                         if (item.IsPrimary == true && item.PhoneID != contactPhoneModel.PhoneID) {
                             return true;
                         }
                         else {
                             return false;
                         }
                     });

                     //Update the Effective and Expiration dates
                     contactPhoneModel.EffectiveDate = $filter('formatDate')(contactPhoneModel.EffectiveDate);
                     contactPhoneModel.ExpirationDate = $filter('formatDate')(contactPhoneModel.ExpirationDate);

                     if (contactPhoneModel.IsPrimary == true && phonePrimary.length > 0) {
                         bootbox.confirm("You already have primary contact phone. Do you want to override it?", function (result) {
                             if (result === true) {
                                 $scope.savePhone(contactPhoneModel);
                             }
                         });
                     }
                     else if (!contactPhoneModel.IsPrimary && phonePrimary.length == 0 && modifiedIsPrimary && modifiedIsPrimary.length > 0) {
                         bootbox.confirm("You have unchecked primary and no other contact phone is marked as primary. Are you sure you want to save this?", function (result) {
                             if (result === true) {
                                 $scope.savePhone(contactPhoneModel);
                             }
                         });
                     }
                     else if (contactPhoneList.length == 1 && contactPhoneModel.PhoneID == contactPhoneList[0].PhoneID) {
                         $scope.savePhone(contactPhoneModel);
                     }

                     else {
                         $scope.savePhone(contactPhoneModel);
                     }
                 }
             };

             $scope.savePhone = function (contactPhoneModel) {
                 contactPhoneModel.ModifiedOn = moment.utc();
                 contactPhoneService.save(contactPhoneModel).then(
                        function (response) {
                            var data = response.data;
                            if (response.ResultCode == 0) {
                                if (contactPhoneModel.ContactPhoneID > 0) {
                                    alertService.success('Contact Phone has been updated.');
                                }
                                else {
                                    alertService.success('Contact Phone has been saved.');
                                }
                                $scope.reset();
                                $scope.get($scope.contactID, 1);
                            } else
                                alertService.error('OOPS! Something went wrong');
                        },
                        function (errorStatus) {
                            $scope.isLoading = false;
                            alertService.error('OOPS! Something went wrong');
                        },
                        function (notification) {
                            alertService.warning(notification);
                        }).then(function () {
                            $scope.isLoading = false;
                            resetForm();
                        });
             };

             $scope.remove = function (contactPhoneId) {
                 bootbox.confirm("Selected contact phone will be deactivated.\n Do you want to continue?", function (result) {
                     if (result == true) {
                         var contactPhone = angular.copy($filter('filter')($scope.phoneList, { ContactPhoneID: contactPhoneId })[0]);
                         $scope.reset();
                         contactPhone.IsActive = false;
                         //contactPhoneService.save(contactPhone).then(
                         contactPhoneService.remove($scope.contactID, contactPhoneId).then(
                         function (response) {
                             var data = response.data;
                             if (response.ResultCode == 0) {
                                 alertService.success('Contact Phone has been deactivated.');
                                 $scope.get($scope.contactID, 1);
                             }
                         },
                         function (errorStatus) {
                             $scope.isLoading = false;
                             alertService.error('OOPS! Something went wrong');
                         });
                     }
                 });
             }

             $scope.init();

             $scope.$on('showDetails', function (event, args) {
                 contactPhoneService.get($scope.contactID, 1).then(function () {
                     setGridItem(phoneTable, 'ContactPhoneID', args.id);
                 },
                 function (errorStatus) {
                     $scope.isLoading = false;
                     alertService.error('Unable to connect to server');
                 });
             });
         }]);