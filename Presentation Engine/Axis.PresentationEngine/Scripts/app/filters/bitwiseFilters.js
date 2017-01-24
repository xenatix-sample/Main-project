
var app = angular.module('xenatixApp');
app.filter('bitwiseAnd', function () {
    return function (firstNumber, secondNumber) {
        return ((parseInt(firstNumber) & parseInt(secondNumber)) === parseInt(secondNumber));
    };
});

app.filter('bitwiseOr', function () {
    return function (firstNumber, secondNumber) {
        return ((parseInt(firstNumber) | parseInt(secondNumber)) === parseInt(secondNumber));
    };
});
