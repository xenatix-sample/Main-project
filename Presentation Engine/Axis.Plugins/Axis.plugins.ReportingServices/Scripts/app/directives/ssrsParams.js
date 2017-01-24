
// DEVELOPER: Chris Reed

(function () {
    angular.module('xenatixApp')
        .directive('ssrsParams', ['$compile', '$templateCache', '$timeout', 'settings', 'reportingService', 'alertService', '_', '$window', 'httpLoaderInterceptor',
            function ($compile, $templateCache, $timeout, settings, reportingService, alertService, _, $window, httpLoaderInterceptor) {
                httpLoaderInterceptor.ignore(true);

                // Default Option Values
                var defOptions = {
                    Cells: {
                        // scope.Op.Cells.PerRow
                        PerRow: 3,
                        // scope.Op.Cells.Classes
                        Classes: 'col-lg-4 col-md-4 col-sm-4 margin-bottom-small',
                    },
                    // scope.Op.PartialURLs
                    PartialURLs: {
                        Controller: 'Plugins/ReportingServices/',
                        DateTime: 'GetPartial?path=Partials/SsrsParams_DateTime',
                        Integer: 'GetPartial?path=Partials/SsrsParams_Integer',
                        String: 'GetPartial?path=Partials/SsrsParams_String',
                    },
                    // scope.Op.DateOptions
                    DateOptions: {
                        dateFormat: 'mm/dd/yyyy',
                        formatYear: 'yy',
                        startingDay: 1,
                        showWeeks: 'false'
                    },
                    // scope.Op.MomentOptinos
                    MomentOptinos: {
                        // scope.Op.MomentOptinos.SsrsFormat
                        SsrsFormat: 'MM/DD/YYYY'
                    }
                };

                return {
                    restrict: 'E',
                    replace: true,
                    scope: {
                        reportId: '=',
                        params: '=',
                        paramValues: '=?',
                        validate: '=?',
                        options: '=?'
                    },
                    templateUrl: 'Plugins/ReportingServices/GetPartial?path=Partials/SsrsParams',
                    link: function (scope, elem, attrs) {
                        var $w = angular.element($window);

                        if (!scope.params || scope.params.length < 1)
                            return;

                        //#region MODEL MODS/RESETS

                        var loadingDepVals = false;

                        scope.params = _.filter(scope.params, function (param) {
                            return !!param.Prompt;
                        });

                        // Options
                        scope.Op = angular.copy(defOptions);
                        if (scope.options)
                            _.merge(scope.Op, scope.options);

                        //#endregion


                        //#region PVT FNs

                        // Not Implemented.  Left in for later implementation.
                        var buildTemplate = function () {
                            return;

                            var param = null,
                                templateUrl = '',
                                html = '<div class="ssrs-params"><div class="row">';

                            for (var x = 0; x < scope.params.length; x++) {
                                param = scope.params[x];

                                if(!param) continue;

                                if (x > 0 && (x % scope.Op.CtrlsPerRow) == 0)
                                    html += '</div><div class="row">';

                                templateUrl = scope.Op.PartialURLs.Controller +
                                    scope.Op.PartialURLs[param.ParameterTypeName];
                                console.log('templateUrl: ' + templateUrl);

                                if (!templateUrl) continue;

                                //console.log('$templateCache: ' + $templateCache.get(templateUrl));

                                html += '<div class="{{Op.Cells.Classes}}">' +
                                    '<ng-include src="\'' + templateUrl + '\'"></ng-include>' +
                                    //$templateCache.get(templateUrl) +
                                    '</div>';

                            }

                            html += '</div>';

                            elem.html(html).show();
                            $compile(elem.contents())(scope);
                        }

                        var parseParamValues = function () {
                            scope.paramValues = [];

                            _.forEach(scope.params, function (param) {
                                if (param.ParameterTypeName == 'DateTime') {
                                    if (param.Value) {
                                        try {
                                            scope.paramValues.push({
                                                Name: param.Name,
                                                Value: moment(param.Value).format(scope.Op.MomentOptinos.SsrsFormat),
                                                Label: param.Name
                                            });
                                        }
                                        catch (ex) {
                                            alertService.error('Invalid Date Format - ' + param.Name);
                                            scope.paramValues = [];
                                            return false;
                                        }
                                    }
                                }
                                else if (param.MultiValue && param.Values && param.Values.length > 0)
                                    _.forEach(param.Values, function (pv) {
                                        scope.paramValues.push({ Name: param.Name, Value: pv.Value, Label: pv.Name });
                                    });
                                else
                                    scope.paramValues.push({ Name: param.Name, Value: param.Value, Label: param.Name });
                            });

                            return true;
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

                        var resize = function () {
                            // This will get replaced when we start storing field specific attributes in the database
                            var fgl = elem.find('.form-group-lg');
                            fgl.height(75);
                        };

                        //#endregion


                        //#region PUBLIC FNs

                        scope.GetDependValues = function () {
                            if (loadingDepVals || scope.params.length < 1) return;
                            if (!scope.reportId || !scope.params || scope.params.length < 1) return;

                            if (!parseParamValues()) return;

                            loadingDepVals = true;
                            reportingService.loadReportParams(scope.reportId, scope.paramValues)
                                .then(function (response) {
                                    loadingDepVals = false;

                                    if (response !== null && response !== undefined) {
                                        if (response.DataItems) {
                                            var op, np;

                                            for (var i = 0; i < response.DataItems.length; i++) {
                                                // np: New Param
                                                // op: Old Param

                                                np = response.DataItems[i];
                                                if (!np.Prompt) continue;

                                                op = _.firstWhere(scope.params, { Name: np.Name });

                                                if (!op) continue;

                                                op.DefaultValues = getDistinctValues(np.DefaultValues || []);
                                                op.ValidValues = getDistinctValues(np.ValidValues || []);

                                                if (op.MultiValue === true && op.ParameterTypeName != 'DateTime' && op.Values.length < 1)
                                                    op.Values = angular.copy(op.DefaultValues);
                                                else if (!op.MultiValue && op.ParameterTypeName != 'DateTime' &&
                                                    op.DefaultValues.length > 0 &&
                                                    !_.firstWhere(op.ValidValues, { Value: op.Value }))
                                                    op.Value = op.DefaultValues[0].Value;

                                                if (op.Values.length > 0)
                                                    op.Values = getDistinctValues(op.Values);
                                            }
                                        }

                                        if (scope.validate)
                                            scope.validate();

                                        parseParamValues();
                                    }
                                    else
                                        alertService.error('Error while loading dependent parameters!');
                                });
                        };

                        //#endregion


                        //#region WATCHERS/EVENTS

                        // Watch every param value to get dependent vlaues
                        // Not the way I wanted to do this, but a more elegant 
                        // solution takes more time than I have atm.
                        scope.$watch(function () {
                            return _.map(scope.params, function (param) {
                                return param.MultiValue ? param.Values : param.Value;
                            });
                        }, scope.GetDependValues, true);

                        scope.$watch('options', function () {
                            _.merge(scope.Op, scope.options);
                        });

                        scope.$watch(function () { return elem[0].innerHTML; }, resize, true);
                        $w.on('resize', resize);
                        resize();

                        //#endregion


                        // INIT
                        //buildTemplate();
                        if (scope.validate)
                            scope.validate();

                        parseParamValues();
                    }
                }
            }
        ]);
}());