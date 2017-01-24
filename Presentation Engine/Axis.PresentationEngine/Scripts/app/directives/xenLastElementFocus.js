angular.module('xenatixApp').directive('xenLastElementFocus', ['globalObjectsService', function (globalObjectsService) {
    return {
        link: function (scope, element, attrs) {
            var watchArray = attrs.xenLastElementFocus;
            scope.$watch(watchArray, function () {
                if (scope.$last && globalObjectsService.isViewContentLoaded) {
                    element[0].focus();
                }
            });
        }

    }
}]);
