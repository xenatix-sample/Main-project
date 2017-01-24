var app = angular.module('xenatixApp');
//This is a light weight directive cutomized for the user to pass URL of the view passing attribute as viewURL
app.directive('loadView',['$http', '$templateCache', '$compile', '$parse',
    function ($http, $templateCache, $compile, $parse) {
    return {
        restrict: 'E',
        link: function (scope, ele, attrs) {
            scope.$watch(attrs.data, function () {
                var viewURL = $parse(attrs.data)(scope);
                var callback = $parse(attrs.callback)(scope);
                if (viewURL && callback) {
                    $http.get(viewURL, { cache: $templateCache }).success(function (response) {
                        ele.html($compile(response)(scope));
                        callback();
                    });
                }
            });
        }
    }
}]);

