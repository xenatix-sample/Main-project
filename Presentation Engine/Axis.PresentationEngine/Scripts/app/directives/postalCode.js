angular.module('xenatixApp').directive('postalCode', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            $(element).keyup(function () {
                var val = $(this).val().replace('-', '').trim();
                if (val.length > 5) {
                    $(element).val(val.substring(0, 5) + '-' + val.substring(5, val.length));
                }
            });
        }
    }
});