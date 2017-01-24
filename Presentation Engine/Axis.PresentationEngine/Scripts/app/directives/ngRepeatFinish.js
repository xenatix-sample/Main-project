angular.module('xenatixApp')
.directive('ngRepeatFinished', ['$timeout', function ($timeout) {
    return {
        restrict: 'A',        
        scope: {
            onRepeatFinished: "@"
        },
        link: function (scope, element, attrs) {
            if (scope.$last === true) {
                scope.onRepeatFinished();
            }
        }
    };
}]);