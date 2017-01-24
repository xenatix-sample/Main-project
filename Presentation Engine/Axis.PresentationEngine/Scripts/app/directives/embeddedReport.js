angular.module('xenatixApp')
    .directive('embeddedReport', [
        function() {
            return {
                restrict: 'E',
                replace: true,
                scope: {
                    reportId: '@',
                    reportUrl: '@',
                    reportParams: '@',
                    reportHeight: '@',
                    reportWidth: '@'
                },
                template: '<embed id="{{reportId}}" style="width: {{reportWidth}}; height: {{reportHeight}};" src="{{ buildReportUrl() }}" type="application/pdf"/>',
                link: function(scope, el, attrs) {
                    scope.reportHeight = scope.reportHeight || '500px';
                    scope.reportWidth = scope.reportWidth || '900px';

                    scope.buildReportUrl = function() {
                        return scope.reportUrl + '?' + $.param(JSON.parse(scope.reportParams));
                    };
                }
            };
        }
    ]);
