(function () {
    angular.module('xenatixApp')
    .controller('payorPlansController', ['$scope', '$q', '$rootScope', 'alertService', '$stateParams', '$state', 'formService', '$timeout', 'lookupService', 'payorsService', 'payorPlansService', 'helperService', 'roleSecurityService', 'payorDetailsData',
        function ($scope, $q, $rootScope, alertService, $stateParams, $state, formService, $timeout, lookupService, payorsService, payorPlansService, helperService, roleSecurityService, payorDetailsData) {
            $scope.permissionKey = $state.current.data.permissionKey;
            var hasEditPermission = roleSecurityService.hasPermission($scope.permissionKey, PERMISSION.UPDATE);
            var payorPlanState = "businessadministration.configuration.payors.payorplans";
            var planDetailsState = "businessadministration.configuration.payors.payorplans.plandetails";
            var get = function () {
                if ($stateParams.PayorID) {
                    $scope.pageSecurity = $stateParams.PayorID;
                    var promise = [];
                    promise.push(payorDetailsData);
                    promise.push(payorPlansService.getPayorPlans($stateParams.PayorID));
                    return $q.all(promise).then(function (data) {
                        if (hasData(data[0])) {
                            var payorDetails = data[0].DataItems[0];
                            payorDetails.ExpirationDate = formattedDate(payorDetails.ExpirationDate);
                            payorDetails.EffectiveDate = formattedDate(payorDetails.EffectiveDate);
                            $scope.payor = payorDetails;
                            helperService.replaceStateTitle(payorPlanState, $scope.payor.PayorName);
                            $scope.refreshBreadcrumbs();
                            if (isExpireDate($scope.payor.ExpirationDate)) {
                                $scope.isPayorDisabled = true;
                            }
                            $scope.showPlus = !$scope.isPayorDisabled;
                        }
                        if (hasData(data[1])) {
                            $scope.CallCenterSummaryTable.bootstrapTable('load', data[1].DataItems);
                        }
                        resetForm();
                    })
                }
                else {
                    $scope.pageSecurity = 0;
                    return $scope.promiseNoOp();
                }
            };

            var init = function () {
                $scope.pageSecurity;
                $scope.showPlus = false;
                $scope.payor = {
                    PayorID: 0,
                    PayorCode: '',
                    PayorName: '',
                    PayorTypeID: '',
                    EffectiveDate: null,
                    ExpirationDate: null,
                    ModifiedOn: moment().toDate()
                }
                $scope.CallCenterSummaryTable = $("#payorPlanTable");
                $scope.initializeBootstrapTable();
                $scope.$evalAsync(function () {
                    applyDropupOnGrid(true);
                });
                get();
            };

            var resetForm = function () {
                if ($scope.payorPlanForm)
                    $rootScope.formReset($scope.payorPlanForm);
            };

            var formattedDate = function (date, defaultValue) {
                return helperService.getFormattedDate(date, defaultValue);
            }

            var addUpdPayor = function () {
                if (!$scope.payor.PayorID) {
                    return payorsService.addPayor($scope.payor).then(function (data) {
                        if (data.data.ResultCode == 0) {
                            alertService.success('Payor has been added');
                            $state.transitionTo(payorPlanState, { PayorID: data.data.ID });
                        }
                    }, function (errorStatus) {
                        alertService.error('OOPS! Something went wrong');
                    });
                }
                else {
                    $scope.payor.ModifiedOn = moment().toDate();
                    return payorsService.updatePayor($scope.payor).then(function (data) {
                        if (data.data.ResultCode == 0)
                            alertService.success('Payor has been updated');
                        get();
                    }, function (errorSttaus) {
                        alertService.error('OOPS! Something went wrong');
                    })
                }
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
                            field: 'PlanName',
                            title: 'Payor Plan Name',
                            sortable: true
                        },
                        {
                            field: 'PlanID',
                            title: 'Payor Plan ID',
                            sortable: true
                        },
                        {
                            field: 'EffectiveDate',
                            title: 'Effective Date',
                            sortable: true,
                            formatter: function (value, row, index) {
                                if (value) {
                                    return formattedDate(value);
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
                                    return formattedDate(value);
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
                                return (row.CreatedOn < row.ModifiedOn) ? (lookupService.getText(LOOKUPTYPE.Users, value)) : '';
                            }
                        },
                        {
                            field: 'ModifiedOn',
                            title: 'Modified Date',
                            sortable: true,
                            formatter: function (value, row, index) {
                                return (row.CreatedOn < row.ModifiedOn) ? formattedDate(value) : '';

                            }
                        },
                        {
                            field: 'PayorPlanID',
                            title: 'Actions',
                            formatter: function (value, row, index) {
                                var isPlanExpired = isExpireDate(row.ExpirationDate);
                                return '<span class="text-nowrap">' +
                                     (!isPlanExpired && hasEditPermission ? '<a href="javascript:void(0)" data-default-action id="editPlanDetails" name="editPlanDetails" data-toggle="modal"  ng-click="edit(' + value + ',' + (row.PlanName ? '\'' + row.PlanName + '\'' : null) + ')" title="Edit" security permission-key="{{permissionKey}}" permission="update" space-key-press><i class="fa fa-pencil fa-fw border-left margin-left padding-left-small padding-right-small" /></a>' :
                                      '<a href="javascript:void(0)" data-default-action ng-click="edit(' + value + ',' + (row.PlanName ? '\'' + row.PlanName + '\'' : null) + ')" id="viewPlanDetails" name="viewPlanDetails" security permission-key="{{permissionKey}}" permission="read" space-key-press><i title="view" class="fa fa-eye fa-fw border-left margin-left padding-left-small padding-right-small" /></a>') + '</span>'
                            }
                        }
                    ]
                };
            }

            $scope.save = function (hasErrors) {
                if (!hasErrors && formService.isDirty($scope.payorPlanForm.$name)) {
                    $scope.payor.EffectiveDate = formattedDate($scope.payor.EffectiveDate);
                    $scope.payor.ExpirationDate = formattedDate($scope.payor.ExpirationDate);
                    if ($scope.payor.ExpirationDate && _.find($scope.payorPlanForm.modifiedModels, '$name', 'expirationDate') && isExpireDate($scope.payor.ExpirationDate)) {
                        bootbox.dialog({
                            message: "Are you sure you want to expire the payor? This will expire the plans and addresses attached to this payor?",
                            buttons: {
                                success: {
                                    label: "Yes",
                                    className: "btn-success",
                                    callback: function () {
                                        return addUpdPayor();
                                    }
                                },
                                danger: {
                                    label: "No",
                                    className: "btn-danger",
                                    callback: function () {
                                        return $scope.promiseNoOp();
                                    }
                                }
                            }
                        });
                    }
                    else {
                        return addUpdPayor();
                    }

                }
                else {
                    return $scope.promiseNoOp();
                }
            };

            $scope.edit = function (payorPlanID, planName) {
                $scope.Goto(planDetailsState, { PayorPlanID: payorPlanID });
            };

            init();
        }]);
}());