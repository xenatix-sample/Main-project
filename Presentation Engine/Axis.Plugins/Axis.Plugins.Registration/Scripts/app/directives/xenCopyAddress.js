var app = angular.module('xenatixApp');

app.directive('xenCopyAddress', ['$timeout', 'contactAddressService', 'alertService', '$filter',
function ($timeout, contactAddressService, alertService, $filter) {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            ngChecked: '=',
            checkboxId: '@',
            label: '@',
            enterToggles: '@',
            disableAddress: '=',
            dirtyAddress: '=',
            formName: '@',
            isDisabled: '='
        },
        template: '<a id="{{ checkboxId }}" href="javascript:void(0)" ng-class="isDisabled===\'false\' ? \'xencheckbox disabled\' : \'xencheckbox\'" ng-click="checkUncheckMe()"><label for="{{ checkboxId }}">{{ label }}</label><i class="fa fa-fw" ng-class="{\'fa-check-square\': ngChecked, \'fa-square\': !ngChecked }"></i></a>',
        link: function (scope, element, attrs, ctrl) {
            element.bind('keydown', function ($event) {
                if (($event.keyCode === 32) || (scope.enterToggles && $event.keyCode === 13)) {
                    $event.preventDefault();
                    $event.currentTarget.click();
                    return false;
                } else {
                    return true;
                }
            });

            scope.checkUncheckMe = function () {
                if (scope.isDisabled==="false") {
                    return;
                }
                scope.ngChecked = !scope.ngChecked;
            };

            scope.$watch('ngChecked', function (value) {
                if (value) {
                    scope.$parent.isEdit = false;
                    if (!scope.$parent.alreadyChecked) {
                        scope.copyPrevious = scope.$parent.Addresses;
                        return contactAddressService.get(scope.$parent.contactID).then(function (data) {
                            if (data.ResultCode == 0) {
                                if (hasData(data)) {
                                    scope.$parent.Addresses = getPrimaryOrLatestData(data.DataItems);
                                    scope.$parent.Addresses[0].IsComplexName = false;
                                    scope.$parent.Addresses[0].IsGateCode = false;
                                    scope.$parent.Addresses[0].IsExpirationDate = false;
                                    scope.$parent.Addresses[0].IsEffectiveDate = false;
                                    scope.$parent.Addresses[0].IsAddressPermissions = true;
                                    scope.$parent.contactAddress = angular.copy(scope.$parent.Addresses);
                                    ExplicitlyModifiedTheForm(scope.$parent, scope.formName);
                                    scope.disableAddress = true;
                                    scope.dirtyAddress = true;
                                }
                                else {
                                    scope.$parent.isChecked = false;
                                    scope.disableAddress = scope.$parent.setDisableAddress();
                                    scope.$parent.initAddresses();
                                    alertService.warning('No address is currently on file');
                                }
                            }
                            else {
                                scope.$parent.isChecked = false;
                                scope.$parent.initAddresses();
                                scope.disableAddress = scope.$parent.setDisableAddress();
                                alertService.error('OOPS! Something went wrong');
                            }

                        },
                       function (errorStatus) {
                           scope.$parent.isChecked = false;
                           scope.$parent.initAddresses();
                           scope.disableAddress = scope.$parent.setDisableAddress();
                           alertService.error('Unable to connect to server');
                       });
                    } else {
                        scope.copyPrevious = [{
                            AddressTypeID: null,
                            Line1: '',
                            Line2: '',
                            City: '',
                            StateProvince: null,
                            County: null,
                            Zip: '',
                            MailPermissionID: '',
                            IsPrimary: true,
                            IsGateCode: false,
                            IsComplexName: false,
                            IsAddressPermissions: true
                        }];
                    }
                }
                else {
                    if (!scope.copyPrevious) {
                        scope.$parent.initAddresses();
                    }
                    else if (scope.$parent.Addresses[0].AddressID == scope.$parent.parentAddressID) {
                        //Is Primary address of current contact
                        scope.$parent.initAddresses();
                        if (!scope.$parent.isEdit) {
                            scope.$parent.Addresses = scope.copyPrevious;
                        }
                    }
                    else if (!scope.$parent.isEdit) {
                        scope.$parent.Addresses = scope.copyPrevious;
                    }
                    scope.$parent.isEdit = false;
                    ExplicitlyModifiedTheForm(scope.$parent, scope.formName);
                    scope.disableAddress = scope.$parent.setDisableAddress();
                    if (scope.$parent.alreadyChecked)
                        scope.$parent.alreadyChecked = false;
                }
            });
        }
    }
}
]);

app.directive('disableAddressDiv', ['$timeout', function ($timeout) {
    return {
        restrict: 'A',
        scope: {
            disableAddress: '='
        },
        require:'^form',
        link: function (scope, element, attr, ctrl) {
            scope.$watch(function () { return ctrl; }, function () {
                if (scope.disableAddress) {
                    angular.element('input', element).attr('disabled', 'disabled');
                    angular.element('select', element).attr('disabled', 'disabled');
                    angular.element('select', element).attr('readonly', 'true');
                    angular.element('input', element).attr('readonly', 'true');
                }
                else {
                    angular.element('input', element).removeAttr('disabled');
                    angular.element('select', element).removeAttr('disabled');
                    angular.element('select', element).removeAttr('readonly');
                    angular.element('input', element).removeAttr('readonly');
                }
            },true);
        }
    }
}]);


