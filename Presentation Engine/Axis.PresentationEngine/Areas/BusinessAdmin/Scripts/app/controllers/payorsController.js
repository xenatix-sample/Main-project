
(function () {
    angular.module('xenatixApp')
    .controller('payorsController', ['$filter', 'alertService', '$state', 'formService', '$scope', '$q', '$rootScope', 'payorsService', 'lookupService', 'roleSecurityService',
        function ($filter, alertService, $state, formService, $scope, $q, $rootScope, payorsService, lookupService, roleSecurityService) {
            $scope.permissionKey = $state.current.data.permissionKey;
            var hasEditPermission = roleSecurityService.hasPermission($scope.permissionKey, PERMISSION.UPDATE);
            $scope.searchText = "";
            var payorPlanState = "businessadministration.configuration.payors.payorplans"
            var payorPlanInitialState= "businessadministration.configuration.payors.initial"

            var init = function () {
                $scope.CallCenterSummaryTable = $("#payorListTable");
                $scope.initializeBootstrapTable();
                 $scope.$evalAsync(function () {
                    applyDropupOnGrid(true);
                });
                $scope.getPayors();
            };

            $scope.getPayors = function (searchText) {
                payorsService.getPayors(searchText).then(function (data) {
                        if (hasData(data)) {
                            $scope.payorList = data.DataItems;
                            $scope.CallCenterSummaryTable.bootstrapTable('load', $scope.payorList);
                        }
                        else {
                            alertService.warning("No matching records found");
                            $scope.CallCenterSummaryTable.bootstrapTable('removeAll');
                        }
                    })
            }

             $scope.addNew=function(){
                 $scope.Goto(payorPlanInitialState);
             }

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
                            field: 'PayorName',
                            title: 'Payor Name',
                            sortable: true
                        },
                        {
                            field: 'PayorCode',
                            title: 'Payor Code',
                            sortable: true
                        },
                        {
                            field: 'PayorTypeID',
                            title: 'Payor Type',
                            sortable: true,
                            formatter: function (value, row) {
                                if (value)
                                    return lookupService.getText(LOOKUPTYPE.PayorType, value);
                                else
                                    return "";
                            }
                        },

                        {
                            field: 'EffectiveDate',
                            title: 'Effective Date',
                            sortable: true,
                            formatter: function (value, row, index) {
                                if (value) {
                                    //toMMDDYYYYDate -> formateDate because its a date only field
                                    var formattedDate = $filter('formatDate')(value, 'MM/DD/YYYY');
                                    return formattedDate;
                                } else
                                    return '';
                            }
                        },
                        {
                            field: 'ExpirationDate',
                            title: 'Expiration Date',
                            sortable: true,
                            formatter: function (value, row, index) {
                                if (value) {
                                    //toMMDDYYYYDate -> formateDate because its a date only field
                                    var formattedDate = $filter('formatDate')(value, 'MM/DD/YYYY');
                                    return formattedDate;
                                } else
                                    return '';
                            }
                        },
                        {
                            field: 'CreatedBy',
                            title: 'Created By',
                            sortable: true,
                            formatter: function (value, row) {
                                if (value)
                                    return lookupService.getText(LOOKUPTYPE.Users, value);
                                else
                                    return "";
                            }
                        },
                        {
                            field: 'ModifiedBy',
                            title: 'Modified By',
                            sortable: true,
                            formatter: function (value, row) {
                                if (value)
                                    return lookupService.getText(LOOKUPTYPE.Users, value);
                                else
                                    return "";
                            }
                        },
                        {
                            field: 'ModifiedOn',
                            title: 'Modified Date',
                            sortable: true,
                            formatter: function (value, row, index) {
                                if (value) {
                                    //toMMDDYYYYDate -> formateDate because its a date only field
                                    var formattedDate = $filter('formatDate')(value, 'MM/DD/YYYY');
                                    return formattedDate;
                                } else
                                    return '';
                            }
                        },

                        {
                            field: 'PayorID',
                            title: 'Actions',
                            formatter: function (value, row, index) {
                                var isPayorExpired = isExpireDate(row.ExpirationDate);
                                return '<span class="text-nowrap">' +
                                     (!isPayorExpired && hasEditPermission ? '<a href="javascript:void(0)" data-default-action id="editPayor" name="editPayor" data-toggle="modal"  ng-click="edit(' + value + ',' + (row.PayorName ? '\'' + row.PayorName + '\'' : null) + ')" title="Edit" security permission-key="{{permissionKey}}" permission="update" space-key-press><i class="fa fa-pencil fa-fw border-left margin-left padding-left-small padding-right-small" /></a>' :
                                      '<a href="javascript:void(0)" data-default-action ng-click="edit(' + value + ',' + (row.PayorName ? '\'' + row.PayorName + '\'' : null) + ')" id="viewPayor" name="viewPayor" security permission-key="{{permissionKey}}" permission="read" space-key-press><i title="view" class="fa fa-eye fa-fw border-left margin-left padding-left-small padding-right-small" /></a>') + '</span>'
                            }
                        }
                    ]
                };
            }

            $scope.edit = function (contactPayorID, payorName) {
                $scope.Goto(payorPlanState, { PayorID: contactPayorID });
            };

            init();

        }]);
}());