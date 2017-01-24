angular.module('xenatixApp')
.directive('autoFocus', ['$timeout', function ($timeout) {
    return {
        restrict: 'A',
        link: function (scope, $element, attributes) {
            scope.$parent.$watch(attributes['eventFocus'], function (value) {
                if (value == true && $element[0] != null) {
                    $timeout(function () {
                        $element[0].focus();
                        try {
                            scope.$parent[attributes['eventFocus']] = false;
                        } catch (e) { }
                    }, 1);
                }
            });
        }
    }
}]);