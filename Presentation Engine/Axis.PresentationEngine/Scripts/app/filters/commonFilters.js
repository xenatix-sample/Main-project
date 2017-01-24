var app = angular.module('xenatixApp');
app.filter('map', function () {
    return function (input, propName) {
        return input.map(function (item) {
            return item[propName];
        });
    };
});