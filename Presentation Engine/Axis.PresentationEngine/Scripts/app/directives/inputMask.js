angular.module('xenatixApp')
    .directive('inputMask', function() {
        return {
            restrict: 'A',
            link: function(scope, el, attrs) {
                $(el).inputmask(scope.$eval(attrs.inputMask));
                $(el).on('change', function() {
                    scope.$eval(attrs.ngModel + "='" + el.val() + "'");
                });
            }
        };
    });