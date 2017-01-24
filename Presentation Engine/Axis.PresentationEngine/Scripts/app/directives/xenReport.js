angular.module('xenatixApp')
.directive('xenReport', ['$parse', '$rootScope', function ($parse, $rootScope) {
    return {
        scope: {
            onPrintReport: '&',
            gotoState: '@',
            hideBreadcrumb: '@',
            onCloseReport:'&'
        },
        templateUrl: '/Scripts/app/Template/Report.html',
        link: function (scope, element, attrs) {
            if (attrs.onPrintReport && scope.onPrintReport) {
                scope.onPrintReport().then(function (data) {
                    scope.reportModel = data;
                    scope.reportModel.HasLoaded = true;
                    $('#xenReportModal').modal('show');
                });
            }

            $('#xenReportModal').on('hidden.bs.modal', function () {
                if (attrs.onCloseReport && scope.onCloseReport) {
                    scope.onCloseReport();
                    scope.$apply();
                 }
            })
            scope.Goto = function (goto) {
                $rootScope.Goto(goto);
            };
            
        }
    }
}]);
