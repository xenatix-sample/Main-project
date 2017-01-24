angular.module('xenatixApp')
    .directive('bootstrapTable', [
        '$compile', 'roleSecurityService', 'httpLoaderInterceptor', function ($compile, roleSecurityService, httpLoaderInterceptor) {
            return {
                restrict: 'A',

                link: function (scope, el, attrs) {
                    if (angular.element(el).is(':visible') && $('[security][dual-permission="true"]').length == 0) {
                        httpLoaderInterceptor.ignore(true);
                    }
                    var opts = scope.$eval(attrs.bootstrapTable);

                    opts.onPostBody = function () {
                        $compile(el.contents())(scope);
                    };

                    el.bootstrapTable(opts);
                }
            };
        }
    ]);