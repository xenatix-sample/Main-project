angular.module('xenatixApp')
.factory('reportingService', ["$http", "$q", 'settings', 'offlineData', '$rootScope',
    function ($http, $q, settings, offlineData, $rootScope) {
    var CONFIG = {
        apiControllerAction: "/Data/Plugins/ReportingServices/ReportingServices/",
        controllerAction: "/Plugins/ReportingServices/",
    };

    function getAllReports() {
        var dfd = $q.defer();
        $http.get(settings.webApiBaseUrl + CONFIG.apiControllerAction + 'GetAllReports')
        .success(function (data, status, header, config) {
            dfd.resolve(data);
        })
        .error(function (data, status, header, config) {
            dfd.reject(status);
        });

        return dfd.promise;
    };

    function getReportByID(reportId) {
        var dfd = $q.defer();
        $http.post(settings.webApiBaseUrl + CONFIG.apiControllerAction + 'GetReportByID',
            { reportId: reportId })
        .success(function (data, status, header, config) {
            dfd.resolve(data);
        })
        .error(function (data, status, header, config) {
            dfd.reject(status);
        });

        return dfd.promise;
    };

    function loadReportParams(reportId, paramValues) {
        var reportParams = {
            ReportId: reportId,
            ParamValues: paramValues
        };

        var dfd = $q.defer();
        $http.post(settings.webApiBaseUrl + CONFIG.apiControllerAction + 'LoadReportParams',
            reportParams)
        .success(function (data, status, header, config) {
            dfd.resolve(data);
        })
        .error(function (data, status, header, config) {
            dfd.reject(status);
        });

        return dfd.promise;
    };

    function runReport(reportParams) {
        var dfd = $q.defer();
        $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'RunReport',
            reportParams)
        .success(function (data, status, header, config) {
            dfd.resolve(data);
        })
        .error(function (data, status, header, config) {
            dfd.reject(status);
        });

        return dfd.promise;
    };

    function getReportsByType(reportTypeName) {
        var dfd = $q.defer();
        $http.get(settings.webApiBaseUrl + CONFIG.apiControllerAction + 'GetReportsByType',
            { params: { ReportTypeName: reportTypeName } })
        .success(function (data, status, header, config) {
            dfd.resolve(data);
        })
        .error(function (data, status, header, config) {
            dfd.reject(status);
        });

        return dfd.promise;
    };

    function setReport(reportName) {
        var reportPath = settings.reportPath +'/'+ reportName;
        var dfd = $q.defer();
        $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'SetReport',
            { params: { reportPath: reportPath } })
        .success(function (data, status, header, config) {
            dfd.resolve(data);
        })
        .error(function (data, status, header, config) {
            dfd.reject(status);
        });

        return dfd.promise;
    };

    return {
        getAllReports: getAllReports,
        getReportByID: getReportByID,
        loadReportParams: loadReportParams,
        runReport: runReport,
        getReportsByType: getReportsByType,
        setReport: setReport
    };
}]);


