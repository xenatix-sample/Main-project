// by using a directive for form valiation, we are making a declarative, rather than imperative, solution to error handling
angular.module('xenatixApp')
    .directive('xenCheckForm', ['$rootScope', '$timeout', 'alertService', 'formService', function ($rootScope, $timeout, alertService, formService) {
        return {
            restrict: 'A',
            replace: false,
            scope: {
                onSave: '&',
                onPrint: '&',
                name: '@',
                stopSave: '=',
                stopNext: '=',
                isDefault: '=',
                permissionKey: '@',
                credentialKey: '@',
                permission: '@',
                isReadOnly: '@'
            },
            require: '^form',
            link: function (scope, elem, attr, controller) {
                if (scope.isDefault) {
                    $rootScope.defaultFormName = scope.name;
                }
                scope.formController = controller;
            },
            controller: ['$scope', function ($scope) {
                this.scope = $scope;
                var timer;

                var save = function (isNext, isMandatory, hasErrors, keepForm, isPrint) {

                    if (!isPrint && !isNext && !hasErrors && !formService.isAnyFormDirty()) {
                        alertService.warning("No change detected! Please make some changes before saving.");
                        return;
                    }

                    if (isPrint && $scope.onPrint !== undefined && !$rootScope.isRunning && ($scope.isReadOnly || !hasErrors)) {
                        $scope.onPrint({ isNext: isNext, isMandatory: isMandatory, hasErrors: hasErrors, keepForm: keepForm });
                    }
                    else if ($scope.onSave !== undefined && !$rootScope.isRunning) {
                        $scope.onSave({
                            isNext: isNext, isMandatory: isMandatory, hasErrors: hasErrors, keepForm: keepForm
                        });
                    }
                    return true;
                };

                this.submit = function (isNext, isMandatory, keepForm, isPrint) {
                    if (timer) {
                        return;
                    }

                    timer = $timeout(function () {
                        $timeout.cancel(timer);
                        timer = null;
                    }, 1000);
                    if ($scope.formController !== undefined && ($scope.formController.$invalid || Object.keys($scope.formController.$error).length)) {
                        // we don't want to display validation errors on screens that are optional, and the user is moving away from the next screen
                        // Vipul Singhal - As per Json if user modified the optional screen and click enter then it should display the validation error 
                        // and if user don't do any modification then user can move to next screen.
                        if (!isMandatory && isNext && !$scope.formController.modified) {
                            // we still need to let the screen's save handle moving to the next screen
                            save(isNext, isMandatory, true, keepForm, isPrint);

                            return true;
                        }

                        // make sure we can check the errors to know what to display
                        if ($scope.formController.$error === undefined || $scope.formController.$error === null) {
                            return false; // unable to determine if there is an error, so don't continue
                        }

                        if (!isMandatory && !$scope.formController.modified) {
                            save(isNext, isMandatory, true, keepForm, isPrint);
                        }

                        if (!eval($scope.isReadOnly)) {
                            var fieldToFocus = null;
                            iterateErrorFields($scope.formController.$error.required, fieldToFocus, 'required', alertService);
                            iterateErrorFields($scope.formController.$error.date, fieldToFocus, 'date', alertService);
                            iterateErrorFields($scope.formController.$error.invalidDate, fieldToFocus, 'invalidDate', alertService);
                            iterateErrorFields($scope.formController.$error.lessThanMinValidDate, fieldToFocus, 'lessThanMinValidDate', alertService);
                            iterateErrorFields($scope.formController.$error.futureDate, fieldToFocus, 'futureDate', alertService);
                            iterateErrorFields($scope.formController.$error.pastDate, fieldToFocus, 'pastDate', alertService);
                            iterateErrorFields($scope.formController.$error.greaterThanDate, fieldToFocus, 'greaterThanDate', alertService);
                            iterateErrorFields($scope.formController.$error.lessThanDate, fieldToFocus, 'lessThanDate', alertService);
                            iterateErrorFields($scope.formController.$error.invalidTime, fieldToFocus, 'invalidTime', alertService);
                            iterateErrorFields($scope.formController.$error.mask, fieldToFocus, 'mask', alertService);
                            iterateErrorFields($scope.formController.$error.pattern, fieldToFocus, 'pattern', alertService);
                            iterateErrorFields($scope.formController.$error.passwordPattern, fieldToFocus, 'passwordPattern', alertService);
                            iterateErrorFields($scope.formController.$error.min, fieldToFocus, 'min', alertService);
                            iterateErrorFields($scope.formController.$error.max, fieldToFocus, 'max', alertService);
                            iterateErrorFields($scope.formController.$error.minimum, fieldToFocus, 'minimum', alertService);
                            iterateErrorFields($scope.formController.$error.minlength, fieldToFocus, 'minlength', alertService);
                            iterateErrorFields($scope.formController.$error.maximum, fieldToFocus, 'maximum', alertService);
                            iterateErrorFields($scope.formController.$error.maxlength, fieldToFocus, 'maxlength', alertService);
                            iterateErrorFields($scope.formController.$error.editable, fieldToFocus, 'editable', alertService);      //For typeahead errors
                            iterateErrorFields($scope.formController.$error.maxLimit, fieldToFocus, 'maxLimit', alertService);
                            iterateErrorFields($scope.formController.$error.customErrorMessage, fieldToFocus, 'customErrorMessage', alertService);
                            iterateErrorFields($scope.formController.$error.invalidSearch, fieldToFocus, 'invalidSearch', alertService);

                            // Check for errors on disabled fields...if all errors come from disabled fields, save the formctrl.quickRegForm.dob.$error.maxLimit
                            // NOTE: this is for 'date' only for now, need to add more with specific selector logic
                            // NOTE: by only checking for date errors, isAllDisabled is true for other errors, allowing save to be called w/o errors in some cases
                            var isAllDisabled = checkForAllDisabledCtrls($scope.formController.$error.date) &&
                                                checkForAllDisabledCtrls($scope.formController.$error.invalidDate) &&
                                                checkForAllDisabledCtrls($scope.formController.$error.lessThanMinValidDate) &&
                                                checkForAllDisabledCtrls($scope.formController.$error.futureDate) &&
                                                checkForAllDisabledCtrls($scope.formController.$error.pastDate) &&
                                                checkForAllDisabledCtrls($scope.formController.$error.greaterThanDate) &&
                                                checkForAllDisabledCtrls($scope.formController.$error.lessThanDate) &&
                                                checkForAllDisabledCtrls($scope.formController.$error.required) &&
                                                checkForAllDisabledCtrls($scope.formController.$error.invalidTime) &&
                                                checkForAllDisabledCtrls($scope.formController.$error.mask) &&
                                                checkForAllDisabledCtrls($scope.formController.$error.pattern) &&
                                                checkForAllDisabledCtrls($scope.formController.$error.passwordPattern) &&
                                                checkForAllDisabledCtrls($scope.formController.$error.min) &&
                                                checkForAllDisabledCtrls($scope.formController.$error.max) &&
                                                checkForAllDisabledCtrls($scope.formController.$error.minimum) &&
                                                checkForAllDisabledCtrls($scope.formController.$error.minlength) &&
                                                checkForAllDisabledCtrls($scope.formController.$error.maximum) &&
                                                checkForAllDisabledCtrls($scope.formController.$error.maxlength) &&
                                                checkForAllDisabledCtrls($scope.formController.$error.editable) &&
                                                checkForAllDisabledCtrls($scope.formController.$error.maxLimit) &&
                                                checkForAllDisabledCtrls($scope.formController.$error.customErrorMessage) &&
                                                checkForAllDisabledCtrls($scope.formController.$error.invalidSearch);

                            if (isAllDisabled) {
                                save(isNext, isMandatory, false, keepForm, isPrint);
                            }
                        }
                        else { //will call when form is in readonly mode so, can move to next screeen.
                            save(isNext, isMandatory, false, keepForm, isPrint);
                        }
                    }

                    else if ($scope.formController !== undefined && $scope.formController.$valid) {
                        save(isNext, isMandatory, false, keepForm, isPrint);
                        return true;
                    }
                    return false;
                }

            }]
        }
    }]);

