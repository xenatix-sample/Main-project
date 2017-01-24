angular.module('xenatixApp')
.directive('autoFocusModal', ['$timeout', function ($timeout) {
    return {
        restrict: 'A',
        scope: {
            eventFocus: "="
        },
        link: function (scope, $element, attributes) {
            scope.$watch('eventFocus', function (value) {
                if (value == true && $element[0] != null && $element[0].attributes['auto-focus-modal'] != null) {
                    $timeout(function () {
                        $element[0].focus();
                        scope.eventFocus = false;
                    })
                }
            });
        }
    }
}]);