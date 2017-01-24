angular.module('xenatixApp')
.directive('number', function () {
    return {
        require: '?ngModel',
        link: function (scope, el, attrs, ctrl) {
            if (!ctrl) {
                return;
            }

            ctrl.$parsers.push(function (val) {
                if (angular.isUndefined(val)) {
                    val = '';
                }
                var clean = val.replace(/[^0-9\.]/g, '');
                var parseDecimal = clean.split('.');

                if (!angular.isUndefined(parseDecimal[1])) {
                    parseDecimal[1] = parseDecimal[1].slice(0, 2);
                    clean = parseDecimal[0] + '.' + parseDecimal[1];
                }

                if (val !== clean) {
                    ctrl.$setViewValue(clean);
                    ctrl.$render();
                }

                //ensure that the value is converted to numeric
                return parseFloat(clean);
            });

            //prevent the spacebar's default action
            el.bind('keypress', function (event) {
                if (event.keyCode === 32) {
                    event.preventDefault();
                }
            });
        }
    };
});


