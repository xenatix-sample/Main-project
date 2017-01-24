angular.module('xenatixApp')
    .controller('ReportsController', ['$scope', 'reportingService', '$sce', '$filter', 'alertService', '$window', 'settings', function ($scope, reportingService, $sce, $filter, alertService, $window, settings) {
  
    var reportType = 'ReportingServices';
    var iFrameWidthDifference = '30';
    var iFrameHeightDifference = '20';

    var init = function () {
        initializeVariables();
        getReportsByType();
    }

    var initializeVariables = function() {
        $scope.reports = [];
        $scope.groups = [];
        $scope.showReport = false;
        $scope.reportSource = $sce.trustAsResourceUrl(settings.reportViewerPath);
    };

    var setGroups = function () {
        var uniqueGroups = [];
        for (i = 0; i < $scope.reports.length; i++) {
            var existingGroup = $filter('filter')(uniqueGroups, { GroupName: $scope.reports[i].ReportGroup }, true);
            if (existingGroup.length === 0) {
                var matchingReports = $filter('filter')($scope.reports, { ReportGroup: $scope.reports[i].ReportGroup }, true);
                var tmpObj = { GroupName: $scope.reports[i].ReportGroup, Reports: matchingReports };
                uniqueGroups.push(tmpObj);
            }
        }

        $scope.groups = uniqueGroups;
    };

    var determineIFrameWidth = function () {
        var wrapperWidth = angular.element('#xenFrameWrapper').width();
        var iFrameWidth = (wrapperWidth - iFrameWidthDifference) + 'px';
        angular.element('.xen-frame').css('width', iFrameWidth);
    };

    angular.element($window).bind('resize', function () {
        var wrapperWidth = angular.element('#xenFrameWrapper').width();
        var iFrameWidth = (wrapperWidth - iFrameWidthDifference) + 'px';
        angular.element('.xen-frame').css('width', iFrameWidth);
    });

    var getReportsByType = function () {
        var hasError = false;
        reportingService.getReportsByType(reportType).then(function(response) {
            if (response !== null && response !== undefined) {
                if (response.DataItems) {
                    $scope.reports = response.DataItems;
                    setGroups();
                    determineIFrameWidth();
                } else {
                    hasError = true;
                }
            } else {
                hasError = true;
            }

            if (hasError) {
                alertService.error('Error while loading report data!');
            }
        });
    };

    $scope.setReport = function (reportName) {        
        reportingService.setReport(reportName).then(function () {
            $scope.showReport = true;
            $scope.activeMenu = reportName;
            $('#iframeReport')[0].contentWindow.location.reload(true);
        });
    };

    init();
}]);
