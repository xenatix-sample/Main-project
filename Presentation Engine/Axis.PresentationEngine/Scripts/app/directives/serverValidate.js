angular.module('xenatixApp').directive('serverValidate', function() {
    return {
        link: function (scope, element, attr) {
            var form = element.inheritedData('$formController');
            if (!form) return;
            var validate = attr.serverValidate;
            scope.$watch(validate, function(errors) {
                form.$serverError = {};
                if (!errors) {
                    form.$serverInvalid = false;
                    return;
                }
                form.$serverInvalid = (errors.length > 0);
                angular.forEach(errors, function(error, i) {
                    form.$serverError[error.Key] = { $invalid: true, message: error.Value };
                });
            });
        }
    };
});
