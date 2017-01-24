angular.module('xenatixApp').
    directive('xenSignature', ['$http', '$templateCache', '$compile',
        function ($http, $templateCache, $compile) {
        return {
            restrict: 'E',
            replace: true,
            templateUrl: '/DigitalSignature/DigitalSignature',
            link: function (scope, ele, attrs) {
                //Define sign method to verify signature
            }
        }
    }]);