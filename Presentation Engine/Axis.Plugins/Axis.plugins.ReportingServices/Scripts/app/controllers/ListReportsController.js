
// DEVELOPER: Chris Reed

(function () {
    angular.module('xenatixApp')
        .controller('ListReportsController', ['$scope', '$stateParams', 'reportingService', '$sce', '$timeout', 'alertService', '$window', 'settings', '_', 'httpLoaderInterceptor',
            function ($scope, $stateParams, reportingService, $sce, $timeout, alertService, $window, settings, _, httpLoaderInterceptor) {
                httpLoaderInterceptor.ignore(true);

                //Remove Before Production Publish
                $scope.RL = {
                    // $scope.RL.Type
                    Type: 'G',
                    // $scope.RL.Toggle
                    Toggle: function (type) {
                        type = type || '';

                        switch (type) {
                            case 'G':
                            case 'F':
                                $scope.RL.Type = type;
                                break;
                            default:
                                $scope.RL.Type = ($scope.RL.Type == 'G') ? 'F' : 'G';
                                break;
                        }
                    }
                };

                //#region MODELS

                $scope.Loading = null;

                $scope.Reports = {
                    // $scope.Reports.UserID
                    UserID: [],
                    // $scope.Reports.Flat
                    Flat: [],
                    // $scope.Reports.Folders
                    Folders: [],
                    // $scope.Reports.Groups
                    Groups: [],
                    // $scope.Reports.Selected
                    Selected: {
                        // $scope.Reports.Selected.ReportId
                        ReportId: null,
                        // $scope.Reports.Selected.Name
                        Name: null,
                        // $scope.Reports.Selected.Params
                        Params: [],
                        // $scope.Reports.Selected.ParamValues
                        ParamValues: [],
                        // $scope.Reports.Selected.ReportUrl
                        ReportUrl: null,
                        // $scope.Reports.Selected.ParamsComplete
                        ParamsComplete: false,
                        // $scope.Reports.Selected.ValidateParams
                        ValidateParams: function () {
                            var valid = true;

                            $scope.Loading = 'Validating';
                            _.forEach($scope.Reports.Selected.Params, function (param) {
                                if (param.Required && param.Prompt) {
                                    if (param.MultiValue && param.Values.length < 1) {
                                        valid = false;
                                    }
                                    else if (param.ParameterTypeName == "Boolean" &&
                                        (param.Value !== true && param.Value !== false)) {
                                        valid = false;
                                    }
                                    else if (!param.MultiValue && !param.Value) {
                                        valid = false;
                                    }
                                }
                            });
                            $scope.Loading = null;

                            $scope.Reports.Selected.ParamsComplete = valid;
                        },
                        // $scope.Reports.Selected.Reset
                        Reset: function () {
                            $scope.Reports.Selected.ReportId = null;
                            $scope.Reports.Selected.Name = null;
                            $scope.Reports.Selected.Params = [];
                            $scope.Reports.Selected.ParamValues = [];
                            $scope.Reports.Selected.ParamsComplete = false;
                            $scope.Reports.Selected.ReportUrl = null;
                        }
                    }
                };

                $scope.Tab = {
                    Title: {
                        // $scope.Tab.Title.Value
                        Value: 'Reports',
                        // $scope.Tab.Title.Set
                        Set: function () {
                            if ($scope.Reports.Selected && $scope.Reports.Selected.Name)
                                $scope.Tab.Title.Value = 'Reports / ' + $scope.Reports.Selected.Name;
                            else
                                $scope.Tab.Title.Value = 'Reports';

                            console.log('Tab.Title: ' + $scope.Tab.Title.Value);
                        }
                    },
                    // $scope.Tab.Selected
                    Selected: 0,
                    // $scope.Tab.Set
                    Set: function (tab) {
                        $scope.Tab.Selected = tab || 0;

                        if ($scope.Tab.Selected == 1)
                            $scope.Reports.Selected.Reset();

                        $scope.Tab.Title.Set();

                        console.log('Tab.Selected: ' + $scope.Tab.Selected);
                    }
                };

                //#endregion


                //#region PVT FNs

                var setGroups = function () {
                    // getReportsFromFolders
                    var gRFF = function (folders) {
                        var r = [];
                        _.forEach(folders, function (folder) {
                            _.merge(r, folder.Reports);

                            if (folder.SubFolders.length > 0)
                                _.merge(r, gRFF(folder.SubFolders));
                        });

                        return r;
                    };

                    var reports = gRFF($scope.Reports.Folders);

                    $scope.Reports.Flat = [];
                    $scope.Reports.Groups = [];

                    var group = null;
                    _.forEach(reports, function (report) {
                        if (!report.ReportGroup) return;

                        $scope.Reports.Flat.push(report);

                        group = _.firstWhere($scope.Reports.Groups, { ReportGroup: report.ReportGroup });

                        if (!group) {
                            group = {
                                ReportGroup: report.ReportGroup,
                                GroupID: report.ReportGroupID,
                                Reports: []
                            };
                            group.Reports.push(report);
                            $scope.Reports.Groups.push(group);
                        }
                        else
                            group.Reports.push(report);
                    });

                    $scope.Reports.Groups = _.sortBy($scope.Reports.Groups, function (g) { return g.GroupID; });

                    console.log('Report Count: ' + $scope.Reports.Flat.length);
                };

                var getReportById = function (reportId) {
                    if (!reportId) return null;

                    var folder = null;
                    for (var x = 0; x < $scope.Reports.Folders.length; x++) {
                        folder = $scope.Reports.Folders[x];

                        for (var y = 0; y < folder.Reports.length; y++) {
                            if (folder.Reports[y].ID == reportId) {
                                return folder.Reports[y];
                            }
                        }
                    }

                    return null;
                };

                var getReportByName = function (reportName) {
                    if (!reportName) return null;

                    var folder = null;
                    for (var x = 0; x < $scope.Reports.Folders.length; x++) {
                        folder = $scope.Reports.Folders[x];

                        for (var y = 0; y < folder.Reports.length; y++) {
                            if (folder.Reports[y].ReportName == reportName) {
                                return folder.Reports[y];
                            }
                        }
                    }

                    return null;
                };

                // Remove duplicate items from architectural bug
                var getDistinctValues = function (values) {
                    var vList = [];
                    _.forEach(values, function (val) {
                        if (_.findIndex(vList, { Value: val.Value }) >= 0)
                            return;

                        vList.push(val);
                    });

                    return vList;
                };

                var parseParams = function (params) {
                    console.log('Param Count: ' + params.length);

                    _.forEach(params, function (param) {
                        if (param.MultiValue) {
                            param.DefaultValues = getDistinctValues(param.DefaultValues);
                            param.ValidValues = getDistinctValues(param.ValidValues);
                            param.Values = getDistinctValues(param.Values);
                        }

                        switch (param.ParameterTypeName) {
                            case 'Integer':
                                if (param.Value != null && param.Value != undefined)
                                    param.Value = parseInt(param.Value);
                                break;
                            case 'DateTime':
                                if (param.Value)
                                    param.Value = new Date(param.Value);
                                break;
                            case 'String':
                                break;
                        }
                    });

                    $scope.Reports.Selected.Params = params;
                };

                //#endregion


                //#region PUBLIC FNs

                $scope.GetAllReports = function () {
                    $scope.Loading = 'Reports';
                    reportingService.getAllReports()
                        .then(function (response) {
                            $scope.Loading = null;

                            if (response !== null && response !== undefined) {
                                if (response.DataItems) {
                                    $scope.Reports.Folders = response.DataItems;
                                    setGroups();

                                    return;
                                }
                            }

                            alertService.error('Error while loading report data!');
                        });
                };

                $scope.LoadReportParams = function (reportName) {
                    reportName = reportName || $scope.Reports.Selected.Name;

                    if (!reportName) {
                        $scope.Goto('Reports');
                        return;
                    }

                    var report = getReportByName(reportName);

                    if (!report) return;

                    $scope.Reports.Selected.ReportId = report.ID;
                    $scope.Reports.Selected.Name = report.ReportName;
                    $scope.Reports.Selected.Params = [];

                    $scope.Loading = 'Params';
                    reportingService.loadReportParams($scope.Reports.Selected.ReportId)
                        .then(function (response) {
                            $scope.Loading = null;

                            if (response !== null && response !== undefined) {
                                if (response.DataItems) {
                                    if (response.DataItems.length < 1)
                                        $scope.RunReport($scope.Reports.Selected.ReportId);
                                    else
                                        parseParams(response.DataItems);

                                    $scope.Goto('Reports.Params', { reportName: $scope.Reports.Selected.Name });
                                    return;
                                }
                            }

                            alertService.error('Error while loading report parameters!');
                        });
                };

                $scope.RunReport = function (reportId) {
                    if (reportId && reportId != $scope.Reports.Selected.ReportId)
                        $scope.Reports.Selected = {
                            ReportId: reportId,
                            Params: [],
                            ReportUrl: null
                        };

                    $scope.Reports.Selected.ReportUrl = '';

                    $scope.Loading = 'RunReport';
                    reportingService.runReport({
                        ReportId: $scope.Reports.Selected.ReportId,
                        ReportParameters: $scope.Reports.Selected.Params,
                        //ParamValues: $scope.Reports.Selected.ParamValues,
                        Format: 'PDF',
                        Export: false
                    })
                    .then(function (response) {
                        $scope.Loading = null;

                        if (response !== null && response !== undefined) {
                            if (response.length > 4 && response.substr(0, 4) == 'url:') {
                                $scope.Reports.Selected.ReportUrl = $sce.
                                    trustAsResourceUrl(settings.webApiBaseUrl + response.substr(4, response.length - 4));
                                console.log('ReportUrl: ' + $scope.Reports.Selected.ReportUrl);
                            }
                            else
                                alertService.error('We encountered an error while running your report!');

                            return;
                        }

                        alertService.error('We encountered an error while running your report!');
                    });
                };

                $scope.ExportReport = function (format) {
                    format = format || 'PDF';

                    if (!$scope.Reports.Selected || !$scope.Reports.Selected.ReportId) {
                        alertService.error('Invalid report.  Please select a report to export.!');
                        return;
                    }

                    $scope.Loading = 'ExportReport';
                    reportingService.runReport({
                        ReportId: $scope.Reports.Selected.ReportId,
                        ReportParameters: $scope.Reports.Selected.Params,
                        Format: format,
                        Export: true
                    })
                    .then(function (response) {
                        $scope.Loading = null;

                        if (response !== null && response !== undefined) {
                            if (response.length > 4 && response.substr(0, 4) == 'url:') {
                                var url = $sce.trustAsResourceUrl(settings.webApiBaseUrl + response.substr(4, response.length - 4));

                                console.log('Export URL: ' + url);
                                $window.open(url);
                            }
                            else
                                alertService.error('We encountered an error while running your report!');
                        }
                        else
                            alertService.error('We encountered an error while running your report!');
                    });
                };

                //#endregion


                // INIT
                $scope.GetAllReports();

                if ($stateParams.reportName)
                    $scope.LoadReportParams(reportName);
                else
                    $scope.Goto('Reports', $stateParams);
            }]
    );
}());



