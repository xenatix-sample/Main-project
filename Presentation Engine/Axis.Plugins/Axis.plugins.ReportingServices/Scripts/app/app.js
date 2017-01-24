angular.module("xenatixApp")
    .config(['$stateProvider', 'lazyLoaderProvider', function ($stateProvider, lazyLoader) {
            $stateProvider
                .state('ssrs', {
                    title: 'Reports',
                    url: '/ReportingServices',
                    templateUrl: '/Plugins/ReportingServices/Index',
                    controller: "ReportsController as ctrl",
                    resolve: {
                        pinkyPromise: [
                            '$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }
                        ],
                        scriptPromise: [
                            '$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.ReportingServices/ReportingServices');
                            }
                        ]
                    }
                })
                .state('ssrs.reports', {
                    title: 'Report',
                    url: '/ReportingServices/Reports',
                    views: {
                        '@reports': {
                            templateUrl: '/Plugins/ReportingServices/Reports'
                        }
                    }
                })
                .state('Reports', {
                    title: 'Reports',
                    url: '/ReportingServices/ListReports',
                    templateUrl: '/Plugins/ReportingServices/ListReports',
                    controller: "ListReportsController as ctrl",
                    resolve: {
                        pinkyPromise: [
                            '$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }
                        ],
                        scriptPromise: [
                            '$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.ReportingServices/ReportingServices');
                            }
                        ]
                    }
                })
                .state('Reports.Params', {
                    title: 'Report Parameters',
                    url: '/:reportName',
                    templateUrl: '/Plugins/ReportingServices/GetPartial?path=Partials/ReportParams'
                });
        }
    ])
    .run([
        '$rootScope', function ($rootScope) {

        }
    ]);