angular.module('xenatixApp')
    .directive('chartJs', ['$compile', '$timeout',
            function ($compile, $timeout) {
                var optionOverrides = { multiTooltipTemplate: '<%= datasetLabel %>: <%= value %>' };
                var chartColors = [
                    '223,1,1',
                    '219,169,1',
                    '58,223,0',
                    '1,169,219',
                    '1,58,223',
                    '116,1,223',
                    '223,1,116',
                    '247,129,129',
                    '245,218,129',
                    '159,247,129',
                    '129,218,245',
                    '129,129,247',
                    '218,129,245',
                    '247,129,159'
                ];
                return {
                    restrict: 'E',
                    replace: true,
                    scope: {
                        chartId: '@',
                        totalWidth: '@',
                        totalHeight: '@',
                        chartWidth: '@',
                        chartHeight: '@',
                        chartData: '='
                    },
                    template: '<div style="width: {{totalWidth}}; height:{{totalHeight}};" class="divtable"><div class="divrow"><div class="divcell"><canvas id="{{chartId}}" width="{{chartWidth}}" height="{{chartHeight}}"></canvas></div><div class="divcell"><div class="chartjs-legend form-inline"></div></div></div></div>',
                    link: function (scope, el, attrs) {
                        scope.resizeChart = function () {
                            el.find('canvas').height(parseInt(scope.chartHeight)).width(parseInt(scope.chartWidth));
                            scope.chart.chart.width = parseInt(scope.chartWidth);
                            scope.chart.chart.height = parseInt(scope.chartHeight);
                            scope.chart.chart.aspectRatio = parseInt(scope.chartWidth) / parseInt(scope.chartHeight);
                            scope.chart.resize();
                            scope.chart.reflow();
                            scope.chart.update();
                        };
                        scope.initChart = function () {
                            scope.legendLabels = {};
                            var colorIdx = 0;
                            for (var dsIdx = 0; dsIdx < scope.chartData.datasets.length; dsIdx++) {
                                scope.legendLabels[scope.chartData.datasets[dsIdx].label] = true;
                                scope.chartData.datasets[dsIdx] = $.extend({}, scope.chartData.datasets[dsIdx], {
                                    fillColor: "rgba(" + chartColors[colorIdx] + ",0.2)",
                                    strokeColor: "rgba(" + chartColors[colorIdx] + ",1)",
                                    pointColor: "rgba(" + chartColors[colorIdx] + ",1)",
                                    pointStrokeColor: "#fff",
                                    pointHighlightFill: "#fff",
                                    pointHighlightStroke: "rgba(" + chartColors[colorIdx] + ",1)",
                                });
                                colorIdx++;
                                colorIdx = colorIdx === chartColors.length ? 0 : colorIdx;
                            }
                            var ctx = el.find('canvas').get(0).getContext('2d');
                            scope.chart = new Chart(ctx).Line(scope.chartData, optionOverrides);
                            scope.chart.options.legendTemplate = '<% for (var i=0; i<datasets.length; i++){%><div class="legend-row"><div class="legend-cell"><span style="background-color:<%=datasets[i].strokeColor%>"></span></div><div class="legend-cell"><xen-checkbox data-ng-model="legendLabels[\'<%= datasets[i].label %>\']" data-checkbox-id="legend_<%= i %>" data-label="<%=datasets[i].label%>"></xen-checkbox></div></div><%}%>';
                            el.find('.chartjs-legend').addClass('line-legend').html(scope.chart.generateLegend());
                            $timeout(function() {
                                scope.resizeChart();
                            }, 1000);
                            scope.$watch('legendLabels', function (newValue, oldValue) {
                                var data = angular.copy(scope.chartData);
                                data.datasets = [];
                                angular.forEach(scope.chartData.datasets, function (dataset) {
                                    if (scope.legendLabels[dataset.label] === true) {
                                        data.datasets.push(dataset);
                                    }
                                });
                                scope.chart.destroy();
                                scope.chart = new Chart(ctx).Line(data, optionOverrides);
                            }, true);
                            $compile(el)(scope);
                        };
                        scope.$watch('chartData', function (newValue) {
                            if ((scope.chartData !== undefined) && (scope.chartData.datasets !== undefined))
                                scope.initChart();
                        });
                    }
                };
            }
        ]
    );