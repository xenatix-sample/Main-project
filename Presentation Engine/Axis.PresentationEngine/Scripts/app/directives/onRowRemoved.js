angular.module('xenatixApp')
.directive('onRowRemoved', ['formService', '$rootScope', function (formService, $rootScope) {
    return {        
        link: function ($scope, element, attrs) {
            $scope.$on('$destroy', function () {
                $rootScope.setform(true);
            });
        }
    };
}]);