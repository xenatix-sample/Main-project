var app = angular.module('xenatixApp');

app.directive('tabAction', function () {
    return {
        restrict: 'A',
        link: function ($scope, element, attrs) {
            element.attr('tabindex', '0');
            element.on('keydown', function (e) {
                if (e.keyCode == 32) {
                    $(this).click();
                }
                
            });
        }
    };
});