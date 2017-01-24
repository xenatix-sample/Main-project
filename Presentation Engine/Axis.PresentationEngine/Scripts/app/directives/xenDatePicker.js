angular.module('xenatixApp')
    .directive("xenDatePicker", ['alertService', '$timeout',
        '$compile', '$filter', function (alertService, $timeout, $compile, $filter) {
            return {
                restrict: "E",
                scope: {
                    ngModel: "=",
                    dateOptions: "=",
                    opened: "=?",
                    dateRequired: '=',
                    callFunctionOnChange: '&',
                    startDate: '=',
                    endDate: '=',
                    name: '@',
                    eventFocus: '@',
                    isDisabled: '=?',
                    isHidden: '=?',
                    futureDateValidate: '@',
                    pastDateValidate: '@',
                    greaterThanDateValidation: '@',
                    lessThanDateValidation: '@',
                    compareElementForm: '=',
                    ignoreTime: '@',
                    maxLimit: '@'
                },
                require: 'ngModel',
                link: {
                    pre: function ($scope, element, attrs, ctrl) {
                        $scope.defaultOptions = {
                            formatYear: 'yy',
                            startingDay: 1,
                            showWeeks: false
                        };
                        $.extend($scope.defaultOptions, $scope.dateOptions);
                    },
                    post: function ($scope, element, attrs, ctrl) {

                        var getDate = function (dateObj) {
                            if ($scope.ignoreTime) {
                                dateObj = new Date(dateObj.setHours(0, 0, 0, 0));
                            }
                            return dateObj;
                        }
                        $scope.opencalendar = function (event) {
                            event.preventDefault();
                            $scope.opened = true;
                            $("input[name='" + $scope.name + "'] ~ .dropdown-menu").show();
                            $('.datepicker_cal').on('keydown', function (e) {
                                if (e.which == 9) {
                                    $("input[name='" + $scope.name + "'] ~ .dropdown-menu").hide();
                                }
                            });
                        };

                        $scope.clear = function () {
                            $scope.ngModel = null;
                        };

                        //Clear the date validation fields
                        var clearAllDateValidations = function (elementName, compareElementName) {
                            //Clear the validations of the element and compareElementName
                            setDateFieldValidity(elementName, true);
                            clearCompareElementValidations(compareElementName);
                        }

                        //Checks and clear the validations of the compareElementName field
                        var clearCompareElementValidations = function (compareElementName) {
                            var compareElement = compareElementName && $("[name='" + compareElementName + "']");
                            if (compareElement && compareElement.length > 0) {
                                setCompareFieldValidity(compareElementName, true);
                            }
                        }

                        var setDateFieldValidity = function (elemName, isValid) {
                            ctrl.$$parentForm[elemName].$setValidity('futureDate', isValid);
                            ctrl.$$parentForm[elemName].$setValidity('pastDate', isValid);
                            ctrl.$$parentForm[elemName].$setValidity('invalidDate', isValid);
                            ctrl.$$parentForm[elemName].$setValidity('maxLimit', isValid);
                            ctrl.$$parentForm[elemName].$setValidity('lessThanDate', isValid);
                            ctrl.$$parentForm[elemName].$setValidity('greaterThanDate', isValid);
                            ctrl.$$parentForm[elemName].$setValidity('lessThanMinValidDate', isValid);
                        }

                        var setCompareFieldValidity = function (elemName, isValid) {
                            if ($scope.compareElementForm) {
                                $scope.compareElementForm[elemName].$setValidity('greaterThanDate', isValid);
                                $scope.compareElementForm[elemName].$setValidity('lessThanDate', isValid);
                            }
                            else {
                                ctrl.$$parentForm[elemName].$setValidity('greaterThanDate', isValid);
                                ctrl.$$parentForm[elemName].$setValidity('lessThanDate', isValid);
                            }

                        }
                        $scope.changeDate = function () {
                            // Check for future/past date validation
                            var selectedDate = new moment($scope.ngModel).toDate();
                            if ($scope.ngModel && selectedDate !== 'Invalid Date') {
                                //Clear the existing validations
                                clearAllDateValidations(ctrl.$name, attrs.compareElementName);

                                //Validate the Date field
                                selectedDate = getDate(selectedDate);
                                var today = new Date();
                                var toDate = new Date(today.getFullYear(), today.getMonth(), today.getDate());
                                toDate = getDate(toDate);
                                var isDatePastLimit = $filter('isDateMaxLimit')(selectedDate, $scope.maxLimit)
                                var lowestPossibleDate = new moment('01/01/1753').toDate();    
                                if (selectedDate < lowestPossibleDate) {
                                    ctrl.$$parentForm[ctrl.$name].$setValidity('invalidDate', false);
                                    return;
                                }
                                else {
                                    ctrl.$$parentForm[ctrl.$name].$setValidity('invalidDate', true);
                                }

                                if ($scope.maxLimit) {
                                    if (isDatePastLimit) {
                                        ctrl.$$parentForm[ctrl.$name].$setValidity('maxLimit', false);
                                        return;
                                    }
                                    else {
                                        ctrl.$$parentForm[ctrl.$name].$setValidity('maxLimit', true);
                                    }
                                }

                                if ($scope.futureDateValidate && $scope.futureDateValidate == 'true') {
                                    if (selectedDate > toDate) {
                                        ctrl.$$parentForm[ctrl.$name].$setValidity('futureDate', false);
                                        return;
                                    }
                                    else {
                                        ctrl.$$parentForm[ctrl.$name].$setValidity('futureDate', true);
                                    }
                                }
                                if ($scope.pastDateValidate && $scope.pastDateValidate == 'true') {
                                    if (selectedDate < toDate) {
                                        ctrl.$$parentForm[ctrl.$name].$setValidity('pastDate', false);
                                        return;
                                    }
                                    else {
                                        ctrl.$$parentForm[ctrl.$name].$setValidity('pastDate', true);
                                    }
                                }
                                if ($scope.greaterThanDateValidation && $scope.greaterThanDateValidation == 'true') {
                                    //Clear the validations of the compareElementName
                                    clearCompareElementValidations(attrs.compareElementName);
                                    if ($scope.endDate) {
                                        var endDate = new Date($scope.endDate);
                                        endDate = getDate(endDate);
                                        if (selectedDate > endDate) {
                                            ctrl.$$parentForm[ctrl.$name].$setValidity('greaterThanDate', false);
                                            return;
                                        }
                                        else {
                                            ctrl.$$parentForm[ctrl.$name].$setValidity('greaterThanDate', true);
                                        }
                                    }
                                    else {
                                        ctrl.$$parentForm[ctrl.$name].$setValidity('greaterThanDate', true);
                                    }
                                }
                                if ($scope.lessThanDateValidation && $scope.lessThanDateValidation == 'true') {
                                    //Clear the validations of the compareElementName
                                    clearCompareElementValidations(attrs.compareElementName);
                                    if ($scope.startDate) {
                                        var startDate = new Date($scope.startDate);
                                        startDate = getDate(startDate);
                                        if (selectedDate < startDate) {
                                            ctrl.$$parentForm[ctrl.$name].$setValidity('lessThanDate', false);
                                            return;
                                        }
                                        else {
                                            ctrl.$$parentForm[ctrl.$name].$setValidity('lessThanDate', true);
                                        }
                                    }
                                    else {
                                        ctrl.$$parentForm[ctrl.$name].$setValidity('lessThanDate', true);
                                    }
                                }
                            }
                            else {
                                //Clear the existing validations
                                clearAllDateValidations(ctrl.$name, attrs.compareElementName);
                            }

                            if (typeof $scope.callFunctionOnChange == 'function') {
                                $timeout(function () {
                                    $scope.callFunctionOnChange();
                                });
                            }
                        }
                    }
                },
                template:
                '<div class="datepicker" ng-class="{ \'input-group\' : !isHidden}">' +
                    '<input type="text" class="form-control datepicker_cal input-min-width" datepicker-popup="MM/dd/yyyy" min-date="startDate" name="{{name}}" max-date="endDate" ng-model="ngModel" is-open="opened" datepicker-options="defaultOptions" ng-change="changeDate()" ng-required="dateRequired" auto-focus event-focus="{{eventFocus}}" ng-click="opencalendar($event)" on-open-focus="false" input-mask="{mask: \'99/99/9999\'}" ng-disabled="isDisabled" />' +
                    '<span class="input-group-btn" style="">' +
                        '<button type="button" class="btn btn-info" ng-click="opencalendar($event)" tabindex="-1" title="Calendar" ng-disabled="isDisabled" ng-hide="isHidden"><i class="fa fa-calendar"></i></button>' +
                    '</span>' +
                '</div>'
            }
        }
    ]);
