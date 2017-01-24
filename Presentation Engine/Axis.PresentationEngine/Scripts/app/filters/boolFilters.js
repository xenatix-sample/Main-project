angular.module('xenatixApp')
.filter('toYesNo', function () {
    return function (value) {
        return value ? 'Yes' : 'No'
    }
})