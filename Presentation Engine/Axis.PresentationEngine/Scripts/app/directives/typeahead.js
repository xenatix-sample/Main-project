angular.module('xenatixApp')
    .directive('typeaheadTrigger', ['$timeout', function ($timeout) {
    return {
        require: 'ngModel',
        scope: {
            ngModel: '='
        },
        link: function (scope, element, attr, ngModel) {

            //trigger the popup on 'change' because 'focus'
            //is also triggered after the item selection
            element.bind('forcelyOpenTypeaheadPopup', function () {
                var viewValue = ngModel.$viewValue;
                //restore to null value so that the typeahead can detect a change
                if (ngModel.$viewValue == ' ') {
                    ngModel.$setViewValue(null);
                }

                //force trigger the popup
                ngModel.$setViewValue(' ');

                //set the actual value in case there was already a value in the input
                ngModel.$setViewValue(viewValue || ' ');
                if (scope.ngModel == null || scope.ngModel == "" || scope.ngModel == ' ') {
                    $timeout(function () {
                        scope.ngModel = undefined;
                    }, 500);
                }
            });
        }
    };
}]);
