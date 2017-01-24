(function () {
    angular.module('xenatixApp')
    .controller('planAddressController', ['$filter', 'alertService', '$stateParams', '$state', 'formService', '$scope', '$q', '$rootScope', 'planAddressesService', 'lookupService', '$controller', 'payorPlansService', 'helperService',
        function ($filter, alertService, $stateParams, $state, formService, $scope, $q, $rootScope, planAddressesService, lookupService, $controller, payorPlansService, helperService) {
            $controller('baseContactController', { $scope: $scope });
            $scope.permissionKey = $state.current.data.permissionKey;
            var payorPlanEffectiveDate;
            var payorPlanExpirationDate;
            var payorAddressState = "businessadministration.configuration.payors.payorplans.plandetails.addressdetails";
            var resetForm = function () {
                if ($scope.planAddressForm)
                    $rootScope.formReset($scope.planAddressForm);
            }

            var formattedDate = function (date, defaultValue) {
                return helperService.getFormattedDate(date, defaultValue);
            }
            var init = function () {
                $scope.initAddresses();
                get();
            };


            var setPayorPlanEffectiveAndExpirationDate = function () {
                if (!payorPlanEffectiveDate && $stateParams.PayorID) {
                    return payorPlansService.getPayorPlanByID($stateParams.PayorPlanID).then(function (data) {
                        if (hasData(data))
                            payorPlanEffectiveDate = formattedDate(data.DataItems[0].EffectiveDate);
                        payorPlanExpirationDate = formattedDate(data.DataItems[0].ExpirationDate);
                    })
                }
                else {
                    return $scope.promiseNoOp();
                }
            }

            var get = function () {
                var promise = [];
                promise.push(setPayorPlanEffectiveAndExpirationDate());
                if ($stateParams.PayorAddressID) {
                    $scope.pageSecurity = $stateParams.PayorAddressID;
                    promise.push(planAddressesService.getPlanAddress($stateParams.PayorAddressID));
                }
                else {
                    $scope.pageSecurity = 0;
                }
                return $q.all(promise).then(function (promiseData) {
                    if (promiseData.length > 1) {
                        if (hasData(promiseData[1])) {
                            var planAddressDetails = promiseData[1].DataItems[0];
                            helperService.replaceStateTitle(payorAddressState, planAddressDetails.ContactID);
                            $scope.refreshBreadcrumbs();
                            $scope.planAddress.PayorAddressID = planAddressDetails.PayorAddressID;
                            $scope.planAddress.PayorPlanID = planAddressDetails.PayorPlanID;
                            $scope.planAddress.ElectronicPayorID = planAddressDetails.ElectronicPayorID;
                            $scope.planAddress.ContactID = planAddressDetails.ContactID;
                            $scope.planAddress.EffectiveDate = formattedDate(planAddressDetails.EffectiveDate);
                            $scope.planAddress.ExpirationDate = formattedDate(planAddressDetails.ExpirationDate);

                            var addressModel = $scope.Addresses[0];

                            addressModel.AddressID = planAddressDetails.AddressID;
                            addressModel.AddressTypeID = planAddressDetails.AddressTypeID;
                            addressModel.Line1 = planAddressDetails.Line1;
                            addressModel.Line2 = planAddressDetails.Line2;
                            addressModel.City = planAddressDetails.City;
                            addressModel.StateProvince = planAddressDetails.StateProvince;
                            addressModel.County = planAddressDetails.County;
                            addressModel.Zip = planAddressDetails.Zip;

                            if (isExpireDate($scope.planAddress.ExpirationDate)) {
                                $scope.isAddressDisabled = true;
                            }
                        }
                        resetForm();
                    }
                })
            }

            $scope.AddressAccessCode = $scope.ADDRESS_ACCESS.Required | $scope.ADDRESS_ACCESS.Line1;

            $scope.planAddress = {
                PayorAddressID: 0,
                PayorPlanID: 0,
                ElectronicPayorID: "",
                ContactID: null,
                EffectiveDate: formattedDate(new Date()),
                ExpirationDate: null,
                ModifiedOn: moment().toDate()
            }


            $scope.initAddresses = function () {
                $scope.Addresses = [{
                    AddressID: 0,
                    AddressTypeID: ADDRESS_TYPE.BusinessAddress,
                    Line1: '',
                    Line2: '',
                    City: '',
                    StateProvince: 0,
                    County: 0,
                    Zip: '',
                    MailPermissionID: 1
                }];
            };


            $scope.save = function (hasErrors) {
                if (!hasErrors && formService.isDirty($scope.planAddressForm.$name)) {
                    $scope.planAddress.EffectiveDate = formattedDate($scope.planAddress.EffectiveDate);
                    $scope.planAddress.ExpirationDate = formattedDate($scope.planAddress.ExpirationDate);
                    angular.extend($scope.planAddress, $scope.Addresses[0]);
                    if (isValidDateRange($scope.planAddress.EffectiveDate, payorPlanEffectiveDate, payorPlanExpirationDate)) {
                        if (!$stateParams.PayorAddressID) {
                            if ($stateParams.PayorPlanID) {
                                $scope.planAddress.PayorPlanID = $stateParams.PayorPlanID;
                                return planAddressesService.addPlanAddress($scope.planAddress).then(function (data) {
                                    if (data.data.ResultCode == 0) {
                                        alertService.success('Plan Address has been added');
                                        $state.transitionTo("businessadministration.configuration.payors.payorplans.plandetails.addressdetails", { PayorID: $stateParams.PayorID, PayorPlanID: $stateParams.PayorPlanID, PayorAddressID: data.data.ID });

                                    }
                                }, function (errorStatus) {
                                    alertService.error('OOPS! Something went wrong');
                                })
                            }
                        }
                        else {
                            if ($scope.planAddress.ElectronicPayorID == null) {
                                $scope.planAddress.ElectronicPayorID = "";
                            }
                            $scope.planAddress.ModifiedOn = moment().toDate();
                            return planAddressesService.updatePlanAddress($scope.planAddress).then(function (data) {
                                if (data.data.ResultCode == 0)
                                    alertService.success('Plan Address has been updated');
                                return get();
                            }, function (errorStatus) {
                                alertService.error('OOPS! Something went wrong');
                            })
                        }
                    }
                    else {
                        alertService.error('Plan Address Effective date must be greater than or equal to corresponding Plan effective date.');
                        return $scope.promiseNoOp();
                    }
                }
                else {
                    return $scope.promiseNoOp();
                }
            };

            $scope.cancel = function () {
                $scope.Goto("businessadministration.configuration.payors.payorplans.plandetails", { PayorPlanID: $stateParams.PayorPlanID });
            }




            init();


        }]);
}());