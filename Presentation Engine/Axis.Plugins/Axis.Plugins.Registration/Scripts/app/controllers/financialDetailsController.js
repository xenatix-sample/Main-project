angular.module('xenatixApp')
    .controller('financialDetailsController', ['$scope', 'financialAssessmentService', 'lookupService', '$filter', '$stateParams', '$state',
        function ($scope, financialAssessmentService, lookupService, $filter, $stateParams, $state) {
            var gridTable = $("#gridTable");
            $scope.permissionKey = $state.current.data.permissionKey;
            $scope.initializeBootstrapTable = function () {
                $scope.gridOptions = {
                    pagination: true,
                    pageSize: 10,
                    pageList: [10, 25, 50, 100],
                    search: false,
                    showColumns: true,
                    data: [],
                    undefinedText: '',
                    columns: [
                        {
                            field: 'AdjustedGrossIncome',
                            title: 'ADJUSTED GROSS INCOME',
                            formatter: function (value) {
                                return (checkModel(value) ? $filter('currency')(value) : '');
                                }                          
                        },
                        {
                            field: 'FamilySize',
                            title: 'FAMILY SIZE'
                        },
                        {
                            field: 'CreatedBy',
                            title: 'ENTERED BY',
                            formatter: function (value, row) {
                                if (value)
                                    return lookupService.getText(LOOKUPTYPE.Users, value);
                                else
                                    return "";
                            }
                        },
                        {
                            field: 'AssessmentDate',
                            title: 'ASSESSMENT DATE',
                            formatter: function (value, row) {
                                if (value) {
                                    return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY');
                                } else
                                    return '';
                            }
                        },
                        {
                            field: 'ExpirationDate',
                            title: 'EXPIRATION DATE',
                            formatter: function (value, row) {
                                if (value) {
                                    return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY');
                                } else
                                    return '';
                            }
                        },
                        {
                            field: 'ExpirationReasonID',
                            title: 'EXPIRATION REASON',
                            formatter: function (value, row) {
                                if (value)
                                    return lookupService.getText(LOOKUPTYPE.ExpirationReason, value);
                                else
                                    return "";
                            }
                        },
                        {
                            field: 'FinancialAssessmentDetailsID',
                            title: 'Actions',
                            formatter: function (value, row, index) {
                                var isView = (isHouseholdExpired(row.ExpirationDate) || row.IsViewFinanicalAssessment);
                                return '<span class="text-nowrap">' +
                                     (!isView ? '<a href="javascript:void(0)" data-default-action id="editAssessment" name="editAssessment" data-toggle="modal" data-target="#adminModel" ng-click="editAssessment(' + row.FinancialAssessmentID + ',\'edit\')" title="Edit" security permission-key="{{permissionKey}}" permission="update" space-key-press><i class="fa fa-pencil fa-fw border-left margin-left padding-left-small padding-right-small" /></a>' +
                                      '<a href="javascript:void(0)" data-default-no-action ng-click="editAssessment(' + row.FinancialAssessmentID + ',\'view\')" id="viewAssessment" name="viewAssessment" security permission-key="{{permissionKey}}" permission="read" space-key-press><i title="view" class="fa fa-eye fa-fw border-left margin-left padding-left-small padding-right-small" /></a>' :
                                      '<a href="javascript:void(0)" data-default-action ng-click="editAssessment(' + row.FinancialAssessmentID + ',\'view\')" id="viewAssessment" name="viewAssessment" security permission-key="{{permissionKey}}" permission="read" space-key-press><i title="view" class="fa fa-eye fa-fw border-left margin-left padding-left-small padding-right-small" /></a>') + '</span>'

                            }
                        }
                    ]
                };



            };

            //To get Financial Assessment 
            $scope.getFinancialAssessment = function () {
                $scope.isLoading = true;
                financialAssessmentService.get($scope.contactID, 0).then(function (data) {
                    if (hasData(data)) {
                    		gridData = ($filter('orderBy')(data.DataItems, ['AssessmentDate', 'AssessmentDate'], true));
                        $scope.activeFinancialAssessmentID = gridData[0].FinancialAssessmentID;
                        gridTable.bootstrapTable($scope.gridOptions);
                        gridTable.bootstrapTable('load', gridData);
                    }
                    else {
                        gridTable.bootstrapTable('removeAll');
                    }
                },
                function (errorStatus) {
                    $scope.isLoading = false;
                });
            };



            //To edit the income and expense data
            $scope.editAssessment = function (FinancialAssessmentID, mode) {
                $state.go('patientprofile.benefits.financial.financialdetails', { FinancialAssessmentID: FinancialAssessmentID, ReadOnly: mode });
            };


            $scope.newAssessment = function () {
                $state.go('patientprofile.benefits.financial.initial', { FinancialAssessmentID: $scope.activeFinancialAssessmentID });
            };


            $scope.init = function () {
                $scope.contactID = $stateParams.ContactID;
                $scope.financialAssessmentID = $stateParams.FinancialAssessmentID;
                $scope.$parent['autoFocus'] = true; //for Focus
                $scope.$parent['autoFocusMember'] = false;
                $scope.getFinancialAssessment();
                $scope.initializeBootstrapTable();

            };



            $scope.init();
        }]);
