angular.module('xenatixApp')
    .directive("passwordStrength", function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                scope.$watch(attrs.passwordStrength, function (value) {
                   
                    if (angular.isDefined(value) ) {
                        
                         if (value.length > 7 && /\d/.test(value) && /[a-z]/.test(value) && /[#$%!]/.test(value)) {
                            scope.strength = 'strong';
                        }
                        else if (value.length > 7 && /\d/.test(value) && /[A-Z]/.test(value) && /[#$%!]/.test(value)) {
                            scope.strength = 'strong';
                        }
                        else if (value.length > 7 && /[A-Z]/.test(value) && /[a-z]/.test(value) && /[#$%!]/.test(value)) {
                            scope.strength = 'strong';
                        } else if (value.length > 7 && /\d/.test(value) && /[a-z]/.test(value) && /[A-Z]/.test(value)) {
                            scope.strength = 'medium';
                        }
                        else if (value.length > 7) {
                            scope.strength = 'medium';
                        } else {
                            scope.strength = 'weak';
                        }
                    }
                });
            }
        }
    }).directive('passwordStrength', [

function () {

    return {

        require: 'ngModel',

        restrict: 'E',

        scope: {

            password: '=ngModel'

        },

        link: function (scope, elem, attrs, ctrl) {

            var strength = {

                colors: ['#ac233f', '#e56805', '#fa8222', '#81bc41', '#168e4c'],

                mesureStrength: function (p) {

                    var _force = 0;

                    var _regex = /[$-/:-?{-~!"^_`\[\]]/g;

                    var _lowerLetters = /[a-z]+/.test(p);

                    var _upperLetters = /[A-Z]+/.test(p);

                    var _numbers = /[0-9]+/.test(p);

                    var _symbols = _regex.test(p);

                    var _flags = [_lowerLetters, _upperLetters, _numbers, _symbols];

                    var _passedMatches = $.grep(_flags, function (el) { return el === true; }).length;

                    _force += 2 * p.length + ((p.length >= 10) ? 1 : 0);

                    _force += _passedMatches * 10;

                    // penality (short password)

                    _force = (p.length <= 6) ? Math.min(_force, 10) : _force;

                    // penality (poor variety of characters)

                    _force = (_passedMatches == 1) ? Math.min(_force, 10) : _force;

                    _force = (_passedMatches == 2) ? Math.min(_force, 20) : _force;

                    _force = (_passedMatches == 3) ? Math.min(_force, 40) : _force;

                    return _force;

                },

                getColor: function (s) {

                    var idx = 0;

                    if (s <= 10) { idx = 0; }

                    else if (s <= 20) { idx = 1; }

                    else if (s <= 30) { idx = 2; }

                    else if (s <= 40) { idx = 3; }

                    else { idx = 4; }

                    return { idx: idx + 1, col: this.colors[idx] };

                },

                getStrength: function (s) {

                var showtext = ' ';

                if (s <= 10) { showtext = 'weak'; }

                else if (s <= 20) { showtext = 'medium'; }

                else if (s <= 30) { showtext = 'strong'; }

                else if (s <= 40) { showtext = 'strongest'; }

                else { showtext = ' '; }

                return { showtext: showtext + 1, col: this.toString };

            }

            };

            scope.$watch('password', function (newVal) {

                if (newVal === '' || newVal == undefined) {

                    elem.css({ "display": "none" });

                } else {

                    var c = strength.getColor(strength.mesureStrength(newVal));

                    elem.css({ "display": "inline" });

                    elem.children('ul').children('li')

                    .css({ "background": "transparent" })

                    .slice(0, c.idx)

                    .css({ "background": c.col })

                }

                //scope.strength = isSatisfied(newVal && newVal.length >= 8) +

                // isSatisfied(newVal && /[A-z]/.test(newVal)) +

                // isSatisfied(newVal && /(?=.*\W)/.test(newVal)) +

                // isSatisfied(newVal && /\d/.test(newVal));

                //function isSatisfied(criteria) {

                // return criteria ? 1 : 0;

                //}

            });

        },

        // template: '<li class="point"></li><li class="point"></li><li class="point"></li><li class="point"></li><li class="point"></li>'

        template: '<ul id="strength">' +

        '<li class="point" ></li>' +

        '<li class="point"></li>' +

        '<li class="point"></li>' +

        '<li class="point"></li>' +

        '</ul>'

    }

}

    ])

.directive('patternValidator', [

function () {

    return {

        require: 'ngModel',

        restrict: 'A',

        link: function (scope, elem, attrs, ctrl) {

            ctrl.$parsers.unshift(function (viewValue) {
                if (viewValue == '' || viewValue == undefined)
                {
                    ctrl.$setValidity('passwordPattern', true);
                }
                else{
                var patt = new RegExp(attrs.patternValidator);

                var isValid = patt.test(viewValue);

                ctrl.$setValidity('passwordPattern', isValid);
                    }

                // angular does this with all validators -> return isValid ? viewValue : undefined;

                // But it means that the ng-model will have a value of undefined

                // So just return viewValue!

                return viewValue;

            });

        }

    };

}

]);