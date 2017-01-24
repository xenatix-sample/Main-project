angular.module('xenatixApp')
    .directive('tooltip', ['$compile', function ($compile) {
        return {
            restrict: 'A',
            link: {
                post: function (scope, elem, attrs) {
                    //Run the Bootsrap tooltip function
                    $(elem).tooltip();

                    //Bind the Hover event to Show/Hide the Tooltip on mouse hover
                    elem.hover(function () {
                        elem.tooltip('show');
                    }, function () {
                        elem.tooltip('hide');
                    });
                }
            }
        }
    }]);