(function () {
     angular.module('xenatixApp')
    .directive('serviceFlyout', function () {
        return {
            scope: {
                serviceRecording: '=',
                navItems: '=',
                printReportClick: '&'
            },
            templateUrl: '/Scripts/app/Template/ServiceFlyout.html',
            link: function (scope, ele, attr) {
                scope.closeServiceFlyout = function () {
                    $('#serviceFlyoutCanvas').removeClass('active');
                };
                
                scope.printReport = function (currentIndex) {
                    var printObj = {
                        currentIndex: currentIndex
                    };
                    scope.printReportClick(printObj);
                }
            }
        }
    })
})();
