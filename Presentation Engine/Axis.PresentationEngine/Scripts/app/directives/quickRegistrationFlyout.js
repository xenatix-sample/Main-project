angular.module('xenatixApp')
.directive('quickRegistrationFlyout', ['$rootScope', function ($rootScope) {
    return {
        restrict: 'E',
        scope: false,
        link: function (scope, element, attrs) {
            var flyoutElement = $('.row-offcanvas');
            scope.closeFlyout = function () {
                scope.cancelFlyout();
            }
        },
        controller: "quickRegistrationController",
        controllerAs: "quickRegistrationController",
        templateUrl: '/Scripts/app/Template/RegistrationProfile.html'
    };
}]);