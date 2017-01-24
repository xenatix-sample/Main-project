angular.module('xenatixApp').directive('standardTimeNoPad',['$filter', function ($filter) {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelController) {
            ngModelController.$parsers.push(function (data) {
                // convert data from view format to model format
                if (data == undefined) {
                    return "";
                }
                return data;
            });

            ngModelController.$formatters.push(function (data) {
                // convert data from model format to view format
                return $filter('toStandardTimeNoPad')(data);
            });
        }
    }
}]);