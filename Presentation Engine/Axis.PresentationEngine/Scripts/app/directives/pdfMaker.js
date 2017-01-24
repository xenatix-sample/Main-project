angular.module('xenatixApp')
    .directive('pdfMaker', ['$sce', '$compile', 'reportService', 'auditService', '$stateParams',
        function ($sce, $compile, reportService, auditService,$stateParams) {
            var OtherBrowserTemplate = '<iframe id="{{reportId}}" class="center-block" ng-src="{{reportUrl}}" type="application/pdf" style="width: {{reportWidth}}; height: {{reportHeight}};" />';
            var IETemplate = '<div id="{{reportId}}" class="center-block" ng-bind-html="reportUrl" style="width: {{reportWidth}}; height: {{reportHeight}};"></div>';
            return {
                restrict: 'E',
                replace: true,
                scope: {
                    reportId: '@',
                    reportHeight: '@',
                    reportWidth: '@',
                    reportName: '@',
                    reportData: '=',
                    reportPreview: '@'
                },
                link: function (scope, element, attrs) {

                    angular.element(".printPreview").on('click', function () {
                        scope.reportPreview = false;                        
                        scope.refreshPdf();                      
                        
                    });

                    scope.refreshPdf = function () {                       
                        scope.addPrintScreenAudit(scope.reportData);
                        var isIEBrowser = (window.navigator.userAgent.indexOf("MSIE") > 0 || (!!navigator.userAgent.match(/Trident\/7\./)));
                        if (isIEBrowser) {
                            if (!scope.reportPreview)
                                pdfMake.createPdf(reportService.getReport(scope.reportName)(scope.reportData)).download(scope.reportName);
                            scope.reportUrl = $sce.trustAsHtml("<p>" + scope.reportName + " report downloaded successfully" + "</p>");
                            element.html(IETemplate);
                            $compile(element.contents())(scope);

                        }
                        else {
                            pdfMake.createPdf(reportService.getReport(scope.reportName)(scope.reportData)).getDataUrl(function (reportUrl) {
                                element.html(OtherBrowserTemplate);
                                scope.reportUrl = $sce.trustAsResourceUrl(reportUrl);
                                $compile(element.contents())(scope);
                            });
                        }
                    };

                    scope.addPrintScreenAudit = function (reportData) {  
                        auditService.auditScreenModel.ContactID = $stateParams.ContactID;
                        auditService.auditScreenModel.TransactionLogID = reportData.TransactionID;
                        auditService.auditScreenModel.ActionTypeID = SCREEN_ACTIONTYPES.PrintView;                       
                        auditService.addScreenAudit(auditService.auditScreenModel);
                    };

                    scope.$watch('reportData', scope.refreshPdf);
                   
                }
            };

            
        }
    ]);
