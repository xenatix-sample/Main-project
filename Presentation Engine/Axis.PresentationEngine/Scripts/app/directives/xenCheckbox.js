angular.module('xenatixApp')
    .directive('xenCheckbox', [
        function () {
            return {
                restrict: 'E',
                replace: true,
                scope: {
                    ngModel: '=',
                    checkboxId: '@',
                    label: '@',
                    enterToggles: '@',
                    onClick: '&',
                    className:'@'
                },
                template: '<a id="{{ checkboxId }}" href="javascript:void(0)" ng-click="checkUncheckMe($event)" ng-class="setMyClass()" class={{className}}><label for="{{ checkboxId }}">{{ label }}</label><i class="fa fa-fw" ng-class="setMyIconClass()"></i></a>',
                link: function (scope, el, attrs) {
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
                        scope.ngModel = !scope.ngModel;
                        if (scope.onClick)
                            scope.onClick();
                    };

                    scope.setMyIconClass = function () {
                        var rtn = scope.ngModel === true ? "fa-check-square" : "fa-square";
                        return rtn;
                    }

                    scope.setMyClass = function () {
                        var rtn = "xencheckbox";
                        rtn += (scope.ngModel === true ? " selected" : "");

                        if (attrs.disabled) {
                            rtn += " no-security disabled";
                        }
                        return rtn;
                    }
                }
            };
        }
    ]);
