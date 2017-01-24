angular.module('xenatixApp')
    .directive('xenRadioButton', [
        function () {
            return {
                restrict: 'E',
                replace: true,
                scope: {
                    ngModel: '=',
                    ngValue: '=',
                    radioButtonId: '@',
                    label: '@',
                    enterToggles: '@',
                    onClick: '&',
                    onChange :'&',
                    preventAction: '@'
                },
                template: '<a id="{{ radioButtonId }}" href="javascript:void(0)" ng-class="setMyClass()" ng-click="checkUncheckMe($event)"><i class="fa fa-fw" ng-class="setMyIconClass()"></i><label for="{{ radioButtonId }}">{{ label }}</label></a>',
                link: function (scope, el, attrs) {
                    if (scope.onChange != undefined) {
                        scope.$watch('ngModel', function (newValue, oldValue) {
                            if (newValue && newValue != oldValue)
                                scope.onChange();
                        });
                    }
                    el.bind('keydown', function ($event) {
                        if (($event.keyCode === 32) || (scope.enterToggles && $event.keyCode === 13)) {
                            $event.preventDefault();
                            $event.currentTarget.click();
                            return false;
                        } else
                            return true;
                    });


                    scope.checkUncheckMe = function ($event) {
                        if (attrs.disabled || $($event.currentTarget).attr('disabled')) {
                            return;
                        }
                        scope.ngModel = scope.ngValue;
                        scope.onClick();
                    };

                    scope.setMyIconClass = function () {
                        return scope.ngModel == scope.ngValue ? "fa-dot-circle-o" : "fa-circle-o";
                    }

                    scope.setMyClass = function () {
                        var rtn = "xenradiobutton";
                        rtn += (scope.ngModel == scope.ngValue ? " selected" : "");

                        if (attrs.disabled) {
                            rtn += " no-security disabled";
                        }
                        return rtn;
                    }
                }
            };
        }
]);