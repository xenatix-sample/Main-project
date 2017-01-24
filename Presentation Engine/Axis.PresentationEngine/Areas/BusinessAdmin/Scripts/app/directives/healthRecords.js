/// <reference path="../templates/HealthRecords.html" />
/// <reference path="../../../Views/HealthRecords/HealthRecords.html" />
/// <reference path="../../../Views/HealthRecords/HealthRecords.html" />
(function () {
    angular.module('xenatixApp')
        .directive('healthRecords', function () {

            var controller = ["$scope", "$timeout", function ($scope, $timeout) {
                $scope.initializeBootstrapTable = function () {
                    $scope.tableoptions = {
                        pagination: true,
                        pageSize: 10,
                        pageList: [10, 25, 50, 100],
                        search: false,
                        showColumns: true,
                        data: [],
                        undefinedText: '',
                        columns: [
                            {
                                field: 'Status',
                                title: 'STATUS'
                            },
                            {
                                field: 'ContactName',
                                title: 'CONTACT NAME'
                            },
                            {
                                field: 'MRN',
                                title: 'MRN'
                            },
                            {
                                field: 'RequestedBy',
                                title: 'REQUESTED BY'
                            },
                             {
                                 field: 'DateRange',
                                 title: 'DATE RANGE'
                             },
                            {
                                field: 'SentVia',
                                title: 'SENT VIA'
                            },
                            {
                                field: 'DateSent',
                                title: 'DATE SENT'
                            },
                            {
                                field: 'PrintedBy',
                                title: 'PRINTED BY'
                            },
                             {
                                 field: 'DatePrinted',
                                 title: 'DATE PRINTED',
                                 formatter: function (value, row, index) {
                                     if (value) {
                                         var formattedDate = $filter('formatDate')(value, 'MM/DD/YYYY');
                                         return formattedDate;
                                     } else
                                         return '';
                                 }
                             },
                            {
                                field: 'Action',
                                title: '',
                                formatter: function (value, row, index) {
                                    //var hasEditPermission = roleSecurityService.hasPermission($scope.permissionKey, PERMISSION.UPDATE);
                                    //var mode = hasEditPermission ? "edit" : "view";
                                    return (
                                    '<span class="text-nowrap pull-right">' +
                                        '<a  data-default-action href="javascript:void(0)"  ng-click="" id="history" name="history" title="history"><i class="fa fa-history fa-fw padding-left-small padding-right-small"></i></a>' +
                                        '<a  data-default-action href="javascript:void(0)"  ng-click="" id="view" name="view" title="View"><i class="fa fa-eye fa-fw padding-left-small padding-right-small"></i></a>' +
                                        '<a  data-default-action href="javascript:void(0)"  ng-click="" id="recreate" name="recreate" title="recreate"><i class="fa fa-clone fa-fw padding-left-small padding-right-small"></i></a>' +
                                        '<a  data-default-action href="javascript:void(0)"  ng-click="" id="print" name="print" title="print"><i class="fa fa-print fa-fw padding-left-small padding-right-small"> </i></a>' +
                                    + '</span>');
                                }
                            }
                        ]
                    };
                };


                $scope.init = function () {
                    $scope.initializeBootstrapTable();

                    $scope.$watch("data", function (newValue, oldValue) {
                        if (newValue != null) {
                            $timeout(function () {
                                var healthRecordsTable = $("#" + $scope.elementId);
                                healthRecordsTable.bootstrapTable('load', newValue);
                            });
                        }
                    })
                }

                $scope.init();

            }];

            return {
                restrict: 'E',
                scope: {
                    elementId: "@",
                    enableStatus: "@",
                    enableSorting: "@",
                    data: "="
                },
                controller: controller,
                controllerAs: 'healthRecordVM',
                templateUrl: '/Areas/BusinessAdmin/Scripts/app/templates/HealthRecords.html'
            };
        });
}());