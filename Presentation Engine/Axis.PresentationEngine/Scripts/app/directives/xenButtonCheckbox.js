angular.module('xenatixApp')
    .directive('xenButtonCheckbox', [
        function () {
            return {
                restrict: 'E',
                replace: true,
                scope: {
                    ngModel: '=',
                    buttonId: '@',
                    label: '@',
                    title: '@',
                    onClick: '&'
                },
                template: '<button type="button" id="{{ buttonId }}" name="{{ buttonId }}" ng-click="checkUncheckMe()" title="{{ title }}" class="btn btn-default"><i class="fa fa-square" ng-class="setMyIconClass()"></i> {{ label }}</button>',
                link: function (scope, el, attrs) {
                    scope.checkUncheckMe = function () {
                        scope.ngModel = !scope.ngModel;
                        scope.onClick();
                    };

                    scope.setMyIconClass = function () {
                        var rtn = scope.ngModel === true ? "fa-check-square" : "fa-square";
                        return rtn;
                    }
                }
            };
        }
    ]);
