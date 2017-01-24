angular.module('xenatixApp')
    .directive('tileFlyout', [
        function () {
            return {
                restrict: 'A',
                scope: {
                    tileFlyout: '@'
                },
                link: function (scope, el, attrs) {
                    el.on('click', function ($event) {
                        el.closest(".tile-flyout").find("[tile-flyout].active").not(el).trigger('click');
                        $(scope.tileFlyout).toggleClass('in').toggleClass('collapse');
                        el.toggleClass('active');
                    });
                }
            };
        }
    ]);
