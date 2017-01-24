(function () {
    angular.module('xenatixApp')
    .controller('companyConfigurationController', ['$filter', 'alertService', '$state', 'formService', '$scope', '$q', '$rootScope', 'companyConfigurationService', 'organizationStructureService', 'lookupService', 'helperService', '$controller',
        function ($filter, alertService, $state, formService, $scope, $q, $rootScope, companyConfigurationService, organizationStructureService, lookupService, helperService, $controller) {
            $controller('baseContactController', { $scope: $scope });

            var companyID = 1; // default company is "MHMR Tarrant"

            var Address = function () {
                return {
                    AddressID: 0,
                    AddressTypeID: 4,
                    Line1: '',
                    Line2: '',
                    City: '',
                    StateProvince: 0,
                    County: 0,
                    Zip: '',
                    MailPermissionID: 1,
                    ShowPrimary: true,
                    IsEffectiveDate: true,
                    IsExpirationDate: true,
                    ShowPlusButton: true,
                    IsMultiAddress: true
                };
            }

            var initializeAddress = function () {
                $scope.Addresses = [];
                $scope.Addresses.push(new Address());
            }

            var init = function () {
                getCompanyByID(companyID).then(function () {
                    resetForm();
                });
            }

            var resetForm = function () {
                if ($scope.companyForm)
                    $rootScope.formReset($scope.companyForm, $scope.companyForm.$name);
                if ($scope.addressForm)
                    $rootScope.formReset($scope.addressForm, $scope.addressForm.$name);
                formService.reset();
            };

            var formatAllDates = function (companyDetails) {
                angular.forEach(companyDetails.Addresses, function (item) {
                    item.EffectiveDate = formattedDate(item.EffectiveDate);
                    item.ExpirationDate = formattedDate(item.ExpirationDate);
                    item.IsExpirationDate = true;
                    item.IsEffectiveDate = true;
                    item.ShowPrimary = true;
                })
                angular.forEach(companyDetails.CompanyIdentifiers, function (item) {
                    item.EffectiveDate = formattedDate(item.EffectiveDate);
                    item.ExpirationDate = formattedDate(item.ExpirationDate);
                })

                companyDetails.Company.EffectiveDate = formattedDate(companyDetails.Company.EffectiveDate);
                companyDetails.Company.ExpirationDate = formattedDate(companyDetails.Company.ExpirationDate);
            }

            var formattedDate = function (date, defaultValue) {
                return helperService.getFormattedDate(date, defaultValue);
            }

            var getCompanyByID = function (companyID) {
                return companyConfigurationService.getCompanyByID(companyID).then(function (data) {
                    if (hasData(data)) {
                        $scope.companyDetails = data.DataItems[0];
                        formatAllDates($scope.companyDetails);
                        if ($scope.companyDetails.Addresses.length > 0) {
                            $scope.Addresses = $scope.companyDetails.Addresses;
                            setPlusMinusButtons($scope.Addresses);
                        }
                        else {
                            initializeAddress();
                        }

                        angular.forEach($scope.companyDetails.CompanyIdentifiers, function (item) {
                            if (item.OrganizationIdentifierTypeID == 2)
                            {
                                item.pattern = /^[0-9]+$/;
                            }
                            if (item.OrganizationIdentifierTypeID == 3)
                            {
                                item.maxLength = 100;
                            }
                            else
                            {
                                item.maxLength = 20;
                            }
                        })

                        $scope.pageSecurity = 1;
                        resetForm();
                    }
                }, function (errorStatus) {
                    alertService.error('OOPS! Something went wrong');
                });
            }

            var setPlusMinusButtons = function (items) {
                angular.forEach(items, function (data, index) {
                    data.IsMultiAddress = true;
                    if (index !== items.length - 1) {
                        data.ShowPlusButton = false;

                    }
                    else {
                        data.ShowPlusButton = true;
                    }
                });
            }

            $scope.AddressAccessCode = $scope.ADDRESS_ACCESS.Required | $scope.ADDRESS_ACCESS.EffectiveDate;

            $scope.addNewAddress = function () {
                $scope.Addresses.push(new Address());
                setPlusMinusButtons($scope.Addresses);
            }

            $scope.removeAddress = function (index) {
                $scope.Addresses.splice(index, 1);
                setPlusMinusButtons($scope.Addresses);
            }

            $scope.save = function (isNext, mandatory, hasErrors) {
                if (!hasErrors && (formService.isDirty() || formService.isDirty($scope.companyForm.$name) || formService.isDirty($scope.addressForm.$name))) {
                    $scope.companyDetails.Addresses = $scope.Addresses;
                    formatAllDates($scope.companyDetails);
                    return companyConfigurationService.saveCompany($scope.companyDetails).then(function (data) {
                        if (data.data.ResultCode == 0) {
                            if (!$scope.companyDetails.Company.DetailID) {
                                alertService.success('Company has been added');
                            }
                            else {
                                alertService.success('Company has been updated');
                            }
                            init();
                        }
                    }, function (errorStatus) {
                        alertService.error('OOPS! Something went wrong');
                    });
                }
                else {
                    return $scope.promiseNoOp();
                }
            };

            $scope.repositionDatepicker = function (id) {
                var datepickerElement = angular.element(document.querySelector('#' + id + ' .dropdown-menu'));
                if (datepickerElement !== null && datepickerElement !== undefined && datepickerElement[0] !== undefined)
                    repositionElement(datepickerElement);
            }

            $scope.setPrimary = function (addressModel, index) {
                if (addressModel.length > 1) {
                    angular.forEach(addressModel, function (item) {
                        if (item.IsPrimary == true) {
                            item.IsPrimary = false;
                        }
                    })
                }
            }

            init();
        }]);
}());