
(function () {
    angular.module('xenatixApp')
    .controller('planAddressesController', ['$filter', 'alertService', '$stateParams', '$state', 'formService', '$scope', '$q', '$rootScope', 'planAddressesService', 'lookupService', 'payorPlansService', 'helperService', 'payorsService', 'roleSecurityService', 'planDetailsData',
        function ($filter, alertService, $stateParams, $state, formService, $scope, $q, $rootScope, planAddressesService, lookupService, payorPlansService, helperService, payorsService, roleSecurityService, planDetailsData) {
            $scope.permissionKey = $state.current.data.permissionKey;
            var hasEditPermission = roleSecurityService.hasPermission($scope.permissionKey, PERMISSION.UPDATE);
            var planDetailsState = "businessadministration.configuration.payors.payorplans.plandetails";
            var payorAddressState = "businessadministration.configuration.payors.payorplans.plandetails.addressdetails";
            var payorEffectiveDate;
            var payorExpirationDate;

            var init = function () {
                $scope.showPlus = false;
                $scope.payorPlan = {
                    PlanName: '',
                    PayorPlanID: 0,
                    PlanID: '',
                    PayorID: '',
                    EffectiveDate: formattedDate(new Date()),
                    ExpirationDate: null,
                    IsActive: '1',
                    ModifiedOn: moment().toDate()
                }

                $scope.CallCenterSummaryTable = $("#planAddressesTable");
                $scope.initializeBootstrapTable();
                $scope.$evalAsync(function () {
                    applyDropupOnGrid(true);
                });
                get();
            };

            var setPayorEffectiveAndExpirationDate = function () {
                if (!payorEffectiveDate && $stateParams.PayorID) {
                    return payorsService.getPayorByID($stateParams.PayorID).then(function (data) {
                        if (hasData(data))
                            payorEffectiveDate = formattedDate(data.DataItems[0].EffectiveDate);
                        payorExpirationDate = formattedDate(data.DataItems[0].ExpirationDate);
                    })
                }
                else {
                    return $scope.promiseNoOp();
                }
            }
            var get = function () {
                var promise = [];
                promise.push(setPayorEffectiveAndExpirationDate());
                if ($stateParams.PayorPlanID) {
                    $scope.pageSecurity = $stateParams.PayorPlanID;
                    promise.push(planDetailsData);
                    promise.push(planAddressesService.getPlanAddresses($stateParams.PayorPlanID));
                }
                else {
                    $scope.pageSecurity = 0;
                }
                $q.all(promise).then(function (data) {
                    if (data.length > 1) {
                        if (hasData(data[1])) {
                            var payorPlanDetails = data[1].DataItems[0];
                            payorPlanDetails.ExpirationDate = formattedDate(payorPlanDetails.ExpirationDate);
                            payorPlanDetails.EffectiveDate = formattedDate(payorPlanDetails.EffectiveDate);
                            $scope.payorPlan = payorPlanDetails;
                            helperService.replaceStateTitle(planDetailsState, $scope.payorPlan.PlanName);
                            $scope.refreshBreadcrumbs();
                            if (isExpireDate($scope.payorPlan.ExpirationDate)) {
                                $scope.isPlanDisabled = true;
                            }
                            $scope.showPlus = !$scope.isPlanDisabled;
                        }
                         if (hasData(data[2])) {
                            $scope.CallCenterSummaryTable.bootstrapTable('load', data[2].DataItems);
                        }

                        resetForm();
                    }
                })
            };

            var resetForm = function () {
                if ($scope.payorDetailsForm)
                    $rootScope.formReset($scope.payorDetailsForm);
            }
            var formattedDate = function (date, defaultValue) {
                return helperService.getFormattedDate(date, defaultValue);
            }

            var addUpdPlan = function () {
                if (!$scope.payorPlan.PayorPlanID) {
                    return payorPlansService.addPayorPlan($scope.payorPlan).then(function (data) {
                        if (data.data.ResultCode == 0) {
                            alertService.success('Payor Plan has been added');
                            $state.transitionTo(planDetailsState, { PayorID: $stateParams.PayorID, PayorPlanID: data.data.ID }, { notify: true, reload: false });
                        }
                    }, function (errorStatus) {
                        alertService.error('OOPS! Something went wrong');
                    })
                }
                else {
                    $scope.payorPlan.PayorPlanID = $stateParams.PayorPlanID;
                    $scope.payorPlan.ModifiedOn = moment().toDate();
                    return payorPlansService.updatePayorPlan($scope.payorPlan).then(function (data) {
                        if (data.data.ResultCode == 0)
                            alertService.success('Payor Plan has been updated');
                        
                        get();
                    }, function (errorStatus) {
                        alertService.error('OOPS! Something went wrong');
                    })
                }
            }

            $scope.save = function (hasErrors) {
                if (!hasErrors && formService.isDirty($scope.payorDetailsForm.$name)) {
                    if ($stateParams.PayorID)
                        $scope.payorPlan.PayorID = $stateParams.PayorID;
                    $scope.payorPlan.EffectiveDate = formattedDate($scope.payorPlan.EffectiveDate); //$scope.payorPlan.EffectiveDate ? $filter('formatDate')($scope.payorPlan.EffectiveDate, 'MM/DD/YYYY') : '';
                    $scope.payorPlan.ExpirationDate = formattedDate($scope.payorPlan.ExpirationDate);// $scope.payorPlan.ExpirationDate ? $filter('formatDate')($scope.payorPlan.ExpirationDate, 'MM/DD/YYYY') : '';
                    if (isValidDateRange($scope.payorPlan.EffectiveDate, payorEffectiveDate, payorExpirationDate)) {
                        if ($scope.payorPlan.ExpirationDate && _.find($scope.payorDetailsForm.modifiedModels, '$name', 'expirationDate') && isExpireDate($scope.payorPlan.ExpirationDate)) {
                            bootbox.dialog({
                                message: "Are you sure you want to expire the payor plan? This will expire the plan addresses  attached to this payor plan?",
                                buttons: {
                                    success: {
                                        label: "Yes",
                                        className: "btn-success",
                                        callback: function () {
                                            return addUpdPlan();
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
                            return addUpdPlan();
                        }
                    }
                    else {
                        alertService.error('Payor Plan Effective date must be greater than or equal to corresponding payor effective date.');
                        return $scope.promiseNoOp();
                    }
                }
                else {
                    return $scope.promiseNoOp();
                }
            };


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
                            field: 'ElectronicPayorID',
                            title: 'ELECTRONIC PAYOR ID',
                            sortable: true
                        },
                        {
                            field: 'ContactID',
                            title: 'CONTACT ID',
                            sortable: true
                        },
                        {
                            field: 'Line1',
                            title: 'ADDRESS LINE 1',
                            sortable: true

                        },

                        {
                            field: 'Line2',
                            title: 'ADDRESS LINE 2',
                            sortable: true

                        },
                        {
                            field: 'City',
                            title: 'CITY',
                            sortable: true

                        },
                        {
                            field: 'StateProvince',
                            title: 'STATE',
                                sortable: true,
                            formatter: function (value, row) {
                                if (value)
                                    return lookupService.getText(LOOKUPTYPE.StateProvince, value);
                                else
                                    return "";
                            }

                        },
                        {
                            field: 'Zip',
                            title: 'POSTAL CODE',
                            sortable: true

                        },


                        {
                            field: 'PayorAddressID',
                            title: 'Actions',
                            formatter: function (value, row, index) {
                                var isAddressExpired = isExpireDate(row.ExpirationDate);
                                return '<span class="text-nowrap">' +
                                     (!isAddressExpired && hasEditPermission ? '<a href="javascript:void(0)" data-default-action id="editAddressDetails" name="editAddressDetails" data-toggle="modal"  ng-click="edit(' + value + ',' + (row.ElectronicPayorID ? '\'' + row.ElectronicPayorID + '\'' : null) + ')" title="Edit" security permission-key="{{permissionKey}}" permission="update" space-key-press><i class="fa fa-pencil fa-fw border-left margin-left padding-left-small padding-right-small" /></a>' :
                                      '<a href="javascript:void(0)" data-default-action ng-click="edit(' + value + ',' + (row.ElectronicPayorID ? '\'' + row.ElectronicPayorID + '\'' : null) + ')" id="viewAddressDetails" name="viewAddressDetails" security permission-key="{{permissionKey}}" permission="read" space-key-press><i title="view" class="fa fa-eye fa-fw border-left margin-left padding-left-small padding-right-small" /></a>') + '</span>'
                            }
                        }
                    ]
                };
            }



            $scope.edit = function (payorAddressID, electronicPayorID) {
                $scope.Goto(payorAddressState, { PayorAddressID: payorAddressID });
            };

            init();

        }]);
}());