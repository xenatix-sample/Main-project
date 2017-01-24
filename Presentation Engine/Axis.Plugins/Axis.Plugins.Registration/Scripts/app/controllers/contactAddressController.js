(function () {

    angular.module('xenatixApp')
    .controller('contactAddressController', ['$scope', '$filter', 'contactAddressService', 'alertService', 'lookupService', 'formService', '$rootScope', '$stateParams', '$state','$controller',
    function ($scope, $filter, contactAddressService, alertService, lookupService, formService, $rootScope, $stateParams, $state, $controller) {
        $controller('baseContactController', { $scope: $scope });
        var addressTypeList = lookupService.getLookupsByType('AddressType');
        var stateList = lookupService.getLookupsByType('StateProvince');
        var countyList = lookupService.getLookupsByType('County');
        var addressTable = $("#addressTable");
        var initIndex = 0;
        $scope.init = function () {
            $scope.contactID = $stateParams.ContactID;
            initAddress();
            $scope.addressList = [];
            $scope.initializeBootstrapTable();

            if ($state.current.name.toLowerCase().indexOf('patientprofile') >= 0) {
                $scope.enterKeyStop = true;
                $scope.stopNext = false;
                $scope.saveOnEnter = true;

            }
            else {
                $scope.enterKeyStop = false;
                $scope.stopNext = false;
                $scope.saveOnEnter = false;

            }
        }

        initAddress = function () {
            $scope.address = {
                ContactAddressID: 0
            };

            $scope.Addresses = [{
                ContactAddressID: 0,
                AddressID: 0,
                AddressTypeID: null,
                Line1: '',
                Line2: '',
                City: '',
                StateProvince: $scope.defaultStateProvinceID,
                County: $scope.defaultCountyID,
                Zip: '',
                IsGateCode: true,
                IsComplexName: true,
                ShowPrimary: true,
                IsPrimary: false,
                IsEffectiveDate: true,
                IsExpirationDate: true,
                EffectiveDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
                ExpirationDate: '',
                MailPermissionID: '',
                IsAddressPermissions: true
            }];
            resetForm();
        };

        $scope.reset = function () {
            initAddress();
        }

        resetForm = function () {
            $scope.addressAutoFocus = true;
            $rootScope.formReset($scope.ctrl.addressForm);
        };

        getText = function (value, list) {
            if (value) {
                var formattedValue = lookupService.getSelectedTextById(value, list);
                if (formattedValue != undefined && formattedValue.length > 0)
                    return formattedValue[initIndex].Name
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
                        field: 'AddressTypeID',
                        title: 'Address Type',
                        formatter: function (value, row, index) {
                            return getText(value, addressTypeList);
                        }
                    },
                    {
                        field: 'Line1',
                        title: 'Address Line1'
                    },
                    {
                        field: 'Line2',
                        title: 'Address Line2'
                    },
                    {
                        field: 'IsPrimary',
                        title: 'Primary',
                        formatter: function (value, row, index) {
                            return (value) ? 'Yes' : 'No';
                        }
                    },
                    {
                        field: 'City',
                        title: 'City'
                    },
                    {
                        field: 'StateProvince',
                        title: 'State',
                        formatter: function (value, row, index) {
                            return getText(value, stateList);
                        }

                    },
                    {
                        field: 'County',
                        title: 'County',
                        formatter: function (value, row, index) {
                            return getText(value, countyList);
                        }
                    },
                    {
                        field: 'Zip',
                        title: 'Postal Code'
                    },
                    {
                        field: 'ContactAddressID',
                        title: '',
                        formatter: function (value, row, index) {
                            return '<a href="javascript:void(0)" data-default-action security permission-key="General-General-Address" permission="update" id="editBenefit" name="editBenefit" data-toggle="modal" ng-click="edit(' + value + ')" title="View/Edit" space-key-press><i class="fa fa-pencil fa-fw" /></a>' +
                                    '<a href="javascript:void(0)" data-default-no-action ng-click="remove(' + value + ')" id="deactivateBenefit" security permission-key="General-General-Address" permission="delete" name="deactivateBenefit" title="Deactivate" space-key-press><i class="fa fa-trash fa-fw"></i></a>';
                        }
                    }
                ]
            };
        };

        $scope.get = function (contactId) {
            $scope.isLoading = true;
            $scope.init();
            return contactAddressService.get(contactId).then(function (data) {
                var activeAddressList = $filter('filter')(data.DataItems, function (item) {
                    return !item.ExpirationDate || $filter('formatDate')(item.ExpirationDate, 'MM/DD/YYYY') >= $filter('formatDate')(new Date(), 'MM/DD/YYYY')
                }, true);
                var expiredAddressList = $filter('filter')(data.DataItems, function (item) {
                    return item.ExpirationDate && $filter('formatDate')(item.ExpirationDate, 'MM/DD/YYYY') < $filter('formatDate')(new Date(), 'MM/DD/YYYY')
                }, true);
                if (expiredAddressList.length > 0) {
                    activeAddressList = activeAddressList.concat(expiredAddressList);
                }
                $scope.addressList = activeAddressList;
                if ($scope.addressList != null) {
                    addressTable.bootstrapTable('load', $scope.addressList);
                } else {
                    addressTable.bootstrapTable('removeAll');
                }
                $scope.isLoading = false;
            },
            function (errorStatus) {
                $scope.isLoading = false;
                alertService.error('Unable to connect to server');
            });
        };

        $scope.edit = function (contactAddressID) {
            $scope.Addresses = [];
            var address = angular.copy($filter('filter')($scope.addressList, { ContactAddressID: contactAddressID }, true)[initIndex]);
            $scope.address.ContactAddressID = address.ContactAddressID;
            address.IsGateCode = true;
            address.IsComplexName = true;
            address.ShowPrimary = true;
            address.IsEffectiveDate = true;
            address.IsExpirationDate = true;
            address.EffectiveDate = address.EffectiveDate ? $filter('toMMDDYYYYDate')(address.EffectiveDate, 'MM/DD/YYYY') : $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal');
            $scope.baseEffectiveDate = address.EffectiveDate;
            address.ExpirationDate = address.ExpirationDate ? $filter('toMMDDYYYYDate')(address.ExpirationDate, 'MM/DD/YYYY') : '';
            address.IsAddressPermissions = true;
            $scope.Addresses.push(address);
            resetForm();
        };

        $scope.save = function () {
            var isDirty = formService.isDirty();
            var modelToSave = angular.copy($scope.Addresses);
            var modelToValidate = modelToSave[initIndex];
            var requiredToSave = false;
            if (modelToValidate.Line1 != '' || modelToValidate.Line2 != '' || modelToValidate.City != '' || modelToValidate.StateProvince != undefined || modelToValidate.County != undefined || modelToValidate.Zip != '') {
                requiredToSave = true;
            }
            else {
                alertService.error("No data to save.")
            }
            if (isDirty && requiredToSave) {
                modelToSave[initIndex].ContactID = $scope.contactID;
                modelToSave[initIndex].EffectiveDate = $filter('formatDate')(modelToSave[initIndex].EffectiveDate);
                modelToSave[initIndex].ExpirationDate = $filter('formatDate')(modelToSave[initIndex].ExpirationDate);
                var prevcontactAddress = $filter('filter')($scope.addressList, { ContactAddressID: modelToSave[initIndex].ContactAddressID });

                var addressPrimary = $filter('filter')($scope.addressList, function (item) {
                    if (item.IsPrimary == true && item.ContactAddressID != modelToSave[initIndex].ContactAddressID) {
                        return true;
                    }
                    else {
                        return false;
                    }
                });

                if (modelToSave[initIndex].IsPrimary == true && addressPrimary.length > 0) {
                    bootbox.confirm("You already have primary contact address. Do you want to override it?", function (result) {
                        if (result === true) {
                            $scope.saveAddress(modelToSave);
                        }
                    });
                }
                else if (modelToSave[initIndex].IsPrimary == false && prevcontactAddress.length > 0 && prevcontactAddress[0].IsPrimary == true) {
                    bootbox.confirm("You have unchecked primary and no other contact address is marked as primary. Are you sure you want to save this?", function (result) {
                        if (result === true) {
                            $scope.saveAddress(modelToSave);
                        }
                    });
                }
                else {
                    $scope.saveAddress(modelToSave);                   
                }
            }
        };

        $scope.saveAddress = function (modelToSave) {
            $scope.isLoading = true;
            var model = modelToSave[0];
            model.ModifiedOn = moment.utc();
            contactAddressService.addUpdate(model)
                .then(
                    function (response) {
                        $scope.get($scope.contactID).then(function () {
                            if (model.AddressID > 0) {
                                alertService.success('Contact address has been updated.');                                
                            }
                            else {
                                alertService.success('Contact address has been saved.');                               
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
        };

        $scope.remove = function (contactAddressID) {
            bootbox.confirm("Selected contact address will be deactivated.\n Do you want to continue?", function (result) {
                if (result == true) {
                    contactAddressService.remove(contactAddressID, $scope.contactID).then(function () {
                        $scope.get($scope.contactID).then(function () {
                            alertService.success('Contact address has been deactivated.');
                        });
                    },
                    function (errorStatus) {
                        alertService.error('OOPS! Something went wrong');
                    });
                }
            });
        };

        $scope.init();

        $scope.get($scope.contactID);

        $scope.$on('showDetails', function (event, args) {
            $scope.get($scope.contactID).then(function () {
                setGridItem(addressTable, 'ContactAddressID', args.id);
            });
        });
    }]);
}());
