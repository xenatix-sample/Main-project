(function () {
    angular.module('xenatixApp').controller('benefitSelfPayController', [
        '$scope', '$filter', 'alertService', 'lookupService', '$q', '$stateParams', '$rootScope', '$document', 'formService', 'financialAssessmentService', 'selfPayService', 'financialSummaryService', 'roleSecurityService',
        function ($scope, $filter, alertService, lookupService, $q, $stateParams, $rootScope, $document, formService, financialAssessmentService, selfPayService, financialSummaryService, roleSecurityService) {


            $scope.ExpirationDate = 'ExpirationDate';
            $scope.EffectiveDate = 'EffectiveDate';
            $scope.endDate = new Date();
            $scope.startDate = new Date();
            $scope.isReadOnly = false;
            $scope.dateOptions = {
                formatYear: 'yy',
                startingDay: 1,
                showWeeks: false
            };
            var selfPayTable = $("#selfPayTable");

            var resetForm = function () {
                $rootScope.formReset($scope.ctrl.selfPayForm);
                $rootScope.formReset($scope.ctrl.selfPayForm, 'ctrl.selfPayForm');
                if ($scope.ctrl.selfPayForm) {
                    $scope.ctrl.selfPayForm.modifiedModels = [];
                }
            };

            $scope.amountTypes = [{ value: false, label: 'Dollar' }, { value: true, label: 'Percentage' }];
            var dollarMaxValue = 9999999999999;
            var percentagemaxValue = 100;
            $scope.minValue = 0;
            $scope.maxValue = dollarMaxValue;


            $scope.onAmountTypeChange = function (isParcentage) {
                $scope.maxValue = (isParcentage) ? percentagemaxValue : dollarMaxValue;
            }

            var getOrganizationText = function (type, id) {
                if (id != null && id != undefined)
                    return $filter('filter')($rootScope.getOrganizationByDataKey(type), { ID: id }, true)[0].Name;
            };

            var setDefaultDatePickerSettings = function () {
                $scope.opened = false;
                $scope.dateOptions = {
                    formatYear: 'yy',
                    startingDay: 1,
                    showWeeks: false
                };
                $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'MM/dd/yyyy'];
                $scope.format = $scope.formats[3];
            };
            var initializeSelPay = function () {
                var todayDate = new Date();
                var expirationDate = todayDate.setDate(todayDate.getDate() + 365);
                $scope.selfPay = { EffectiveDate: new Date(), IsPercent: false, ContactID: $stateParams.ContactID, ExpirationDate: new Date(expirationDate), SelfPayID: 0 };
                $scope.minValue = 0;
                $scope.maxValue = dollarMaxValue;
                resetForm();
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
                    onClickRow: function (e, row, $element) {
                        //$scope.prepRowEditData(e);
                    },
                    columns: [
                        {
                            field: "OrganizationDetailID",
                            title: "Division",
                            formatter: function (value, row, index) {
                                return getOrganizationText('Division', value);
                            }
                        },
                        {
                            field: "SelfPayAmount",
                            title: "Amount",
                            formatter: function (value, row, index) {
                                return $filter('number')(value, 2);
                            }
                        },
                        {
                            field: "IsPercent",
                            title: "AMOUNT TYPE",
                            formatter: function (value, row, index) {
                                if (value == 0)
                                    return "Dollar";
                                else if (value == 1)
                                    return "Percentage";
                                else
                                    return null;

                            }
                        },
                        {
                            field: "EffectiveDate",
                            title: "Effective Date",
                            formatter: function (value, row, index) {
                                if (value) {
                                    var formattedDate = $filter('formatDate')(value, 'MM/DD/YYYY');
                                    return formattedDate;
                                } else
                                    return '';
                            }
                        },
                        {
                            field: "ExpirationDate",
                            title: "Expiration Date",
                            formatter: function (value, row, index) {
                                if (value) {
                                    var formattedDate = $filter('formatDate')(value, 'MM/DD/YYYY');
                                    return formattedDate;
                                } else
                                    return '';
                            }
                        },
                        {
                            field: "OverrideReason",
                            title: "Override Reason",
                            formatter: function (value, row, index) {

                                var overrideReasons = "";
                                for (var key in CheckBox_Flags) {
                                    if (CheckBox_Flags.hasOwnProperty(key)) {
                                        if (row[key] == true) {
                                            overrideReasons += CheckBox_Flags[key] + " , " + "<br>";
                                        }
                                    }
                                };
                                return overrideReasons.substring(0, overrideReasons.length - 6);
                            }

                        },
                        {
                            field: "SelfPayID",
                            title: "",
                            formatter: function (value, row, index) {
                                var isView = ((row.ExpirationDate ? isExpireDate(row.ExpirationDate) : false) || row.IsViewSelfPay);
                                return '<span class="text-nowrap">' +
                                    (isView ? '<a href="javascript:void(0)" data-default-action security permission-key="Benefits-SelfPay-SelfPay" permission="read"  ng-click="editSelfPay(' + value + ',' + isView + ', $event)" title="View" space-key-press><i class="fa fa-eye fa-fw"></i></a>' :
                                         '<a href="javascript:void(0)" data-default-action ng-click="editSelfPay(' + value + ',' + isView + ', $event)" title="Edit" ' + 'security permission-key="Benefits-SelfPay-SelfPay" permission="update" space-key-press><i class="fa fa-pencil fa-fw" /></a>' +
                                         '<a href="javascript:void(0)" data-default-no-action ng-click="removeSelfPay(' + value + ', $event)" title="Deactivate" security permission-key="Benefits-SelfPay-SelfPay" permission="delete" space-key-press><i title="Remove" class="fa fa-trash fa-fw border-left margin-left padding-left-small padding-right-small" /></a>' +
                                         '</span>');
                            }
                        }
                    ]
                };
            };

            $scope.divisionChange = function () {
                if ($scope.ctrl.selfPayForm) {
                    $scope.ctrl.selfPayForm.modified = true;
                    $rootScope.setform(true);
                }
            };

            $scope.newSelfPay = function () {
                initializeSelPay();
                $scope.$parent['autoFocus'] = true;
                $scope.isReadOnly = false;
                $document.scrollTop(0);
            };

            $scope.editSelfPay = function (selfPayID, isView, $event) {
                $scope.selfPay = $filter('filter')($scope.selfPayList, { SelfPayID: selfPayID }, true)[0];
                $scope.isReadOnly = isView ? $scope.hasUpdatePermission : false;
                $scope.selfPay.EffectiveDate = (!$scope.selfPay.EffectiveDate) ? '' : $filter('formatDate')($scope.selfPay.EffectiveDate, 'MM/DD/YYYY');
                $scope.selfPay.ExpirationDate = (!$scope.selfPay.ExpirationDate) ? '' : $filter('formatDate')($scope.selfPay.ExpirationDate, 'MM/DD/YYYY');
                $scope.$parent['autoFocus'] = true;
                $scope.selfPay.SelfPayID = selfPayID;
                resetForm();
            };

            $scope.removeSelfPay = function (selfPayID, $event) {
                bootbox.confirm("Selected self Pay will be deactivated. Do you want to continue?", function (result) {
                    if (result === true) {
                        selfPayService.deleteSelfPay($stateParams.ContactID, selfPayID).then(function (response) {
                            var data = response;
                            if (data.ResultCode === 0) {
                                alertService.success('Self Pay has been deactivated.');
                                initializeSelPay();
                                formService.initForm(true, $scope.ctrl.selfPayForm.$name);
                                getSelfPayData();

                            } else {
                                alertService.error('OOPS! Something went wrong');
                            }
                        },
                            function (errorStatus) {
                                alertService.error('OOPS! Something went wrong');
                            },
                            function (notification) {
                                alertService.warning(notification);
                            }
                        ).finally(function () {
                        });
                        $scope.$apply();
                    }
                });
            };

            var CheckBox_Flags = {
                ISChildInConservatorship: "Child is in the Conservatorship of the State (Includes Foster Care)",
                IsNotAttested: "ECI Family Choice not to attest",
                IsEnrolledInPublicBenefits: "Enrolled Public Benefits: WIC, PHC, CSHCN, CIHCP, SKIP, Medic, CHIP, SNAP, TANF",
                IsRequestingReconsideration: "Family Choice declining consent requesting reconsideration",
                IsNotGivingConsent: "Family Choice not to give consent",
                IsOtherChildEnrolled: "Family has 2nd Child Enrolled in ECI Services with a Family Cost Share Amount",
                IsApplyingForPublicBenefits: "Family will apply for public benefits",
                IsReconsiderationOfAdjustment: "Reconsideration of Adjustment Made By Program"
            };

            var getSelfPayData = function () {
                $scope.selfPay.SelfPayID = 0;

                return selfPayService.getSelfPay($stateParams.ContactID).then(function (data) {
                    if (data.DataItems && data.DataItems != undefined) {
                        $scope.selfPayList = data.DataItems;
                        selfPayTable.bootstrapTable('load', $scope.selfPayList);
                    } else {
                        $scope.selfPayList = [];
                        selfPayTable.bootstrapTable('removeAll');
                    }
                    applyDropupOnGrid(true);
                    initializeSelPay();
                });

            };

            //To get Financial Assessment 
            var getFinancialAssessment = function () {
                $scope.isLoading = true;
                return financialAssessmentService.getActiveFA($scope.contactID).then(function (data) {
                    if (data.DataItems.length > 0) {
                        $scope.financialAssessment = data.DataItems[0];
                        $scope.financialAssessment.AssessmentDate = $filter('toMMDDYYYYDate')($scope.financialAssessment.AssessmentDate, 'MM/DD/YYYY');
                        $scope.financialAssessment.ExpirationDate = $filter('toMMDDYYYYDate')($scope.financialAssessment.ExpirationDate, 'MM/DD/YYYY');
                    }
                    else {
                        $scope.financialAssessment = { FamilySize: 1, AssessmentDate: new Date(), ExpirationDate: $filter('toMMDDYYYYDate')((new Date()).setFullYear((new Date()).getFullYear() + 1), 'MM/DD/YYYY', 'useLocal') };
                    }
                },
                function (errorStatus) {
                    $scope.isLoading = false;
                    alertService.error('Unable to connect to server');
                }).finally(function () {
                    setTimeout(function () {
                        var btn = $($(".datepicker")[1]).find("button");
                        btn.click();
                        $($(".datepicker")[1]).find("ul").hide()

                        setTimeout(function () {
                            btn.click();
                        }, 0);

                    }, 0);
                });
            };

            var recordExists = function () {
                var gridData = selfPayTable.bootstrapTable('getData');
                var selfPayPrevious;
                //Beacuase IDD and MH divisions share same self pay - should be treated as one division
                if ($scope.selfPay.OrganizationDetailID == Division.BHMH || $scope.selfPay.OrganizationDetailID == Division.IDD) {
                    selfPayPrevious = $filter('filter')(gridData, function (item) {
                        return (
                             item.SelfPayID != $scope.selfPay.SelfPayID &&      
                            (item.OrganizationDetailID == Division.BHMH ||
                            item.OrganizationDetailID == Division.IDD) &&
                            (!item.ExpirationDate || (moment(item.ExpirationDate).startOf('day').toDate() > moment(new Date()).startOf('day').toDate()))
                            );

                    })[0];
                }
                else {
                    selfPayPrevious = $filter('filter')(gridData, function (item) {
                        return (
                             item.SelfPayID != $scope.selfPay.SelfPayID &&
                            (item.OrganizationDetailID == $scope.selfPay.OrganizationDetailID) &&
                            (!item.ExpirationDate || (moment(item.ExpirationDate).startOf('day').toDate() > moment(new Date()).startOf('day').toDate()))
                            );
                    })[0];
                }

                if (selfPayPrevious) {
                    selfPayPrevious.EffectiveDate = $filter('toMMDDYYYYDate')(selfPayPrevious.EffectiveDate, 'MM/DD/YYYY');
                    selfPayPrevious.ExpirationDate = (!$scope.selfPay.EffectiveDate) ? $filter('toMMDDYYYYDate')(new Date(moment().add(-1, 'days')), 'MM/DD/YYYY') : $filter('toMMDDYYYYDate')(new Date(moment($scope.selfPay.EffectiveDate).add(-1, 'days')), 'MM/DD/YYYY');
                }
                return selfPayPrevious;
            }

            $scope.save = function (isNext, mandatory, hasErrors, keepForm) {
                var isDirty = formService.isDirty();
                if (isDirty && !hasErrors) {
                    var deferred = $q.defer();
                    var isUpdate = $scope.selfPay.SelfPayID != undefined && $scope.selfPay.SelfPayID !== 0;
                    $scope.addSelfPay(isUpdate).then(function (response) {
                        var action = isUpdate ? 'updated' : 'added';
                        getSelfPayData().finally(function () {
                            $scope.postSave(response, action, isNext);
                            deferred.resolve(response);
                        });
                    },
                     function (errorStatus) {
                         alertService.error('OOPS! Something went wrong');
                         deferred.reject();
                     },
                     function (notification) {
                         alertService.warning(notification);
                     });
                    return deferred.promise;
                }
            };

            $scope.postSave = function (response, action, isNext) {
                var data = response;
                if (data.ResultCode !== 0) {
                    alertService.error(data.ResultMessage);
                    return $scope.promiseNoOp();
                } else {
                    alertService.success('Self Pay has been ' + action + ' successfully.');
                    init();
                }
            };

            $scope.addSelfPay = function (isUpdate) {
                $scope.selfPay.EffectiveDate = $filter('toMMDDYYYYDate')($scope.selfPay.EffectiveDate, 'MM/DD/YYYY');
                $scope.selfPay.ExpirationDate = $filter('toMMDDYYYYDate')($scope.selfPay.ExpirationDate, 'MM/DD/YYYY');
                if (!isUpdate) {
                    var selfPayPrevious = recordExists();
                    if (selfPayPrevious) {
                        alertService.warning("There is already a self-pay for this division (BH-MH and IDD share the same Self Pay record)");
                        return selfPayService.updateSelfPay(selfPayPrevious).finally(function (data) {
                            selfPayService.addSelfPay($scope.selfPay);
                        });
                    }
                    else
                        return selfPayService.addSelfPay($scope.selfPay);
                }
                else {
                    var selfPayPrevious = recordExists();
                    if (selfPayPrevious) {
                        alertService.warning("There is already a self-pay for this division (BH-MH and IDD share the same Self Pay record)");
                        return selfPayService.updateSelfPay(selfPayPrevious).finally(function (data) {
                            selfPayService.updateSelfPay($scope.selfPay);
                        });
                    }
                    else {
                        return selfPayService.updateSelfPay($scope.selfPay);
                    }
                }
            };
            var init = function () {
                $scope.$parent['autoFocus'] = true;
                setDefaultDatePickerSettings();
                initializeSelPay();
                $scope.initializeBootstrapTable();
                $scope.hasUpdatePermission = roleSecurityService.hasPermission(BenifitsPermissionKey.Benefits_SelfPay_SelfPay, PERMISSION.UPDATE);
                getSelfPayData().then(function () {
                    if ($stateParams.SelfPayID) {
                        setGridItem(selfPayTable, 'SelfPayID', parseInt($stateParams.SelfPayID));
                        $stateParams.SelfPayID = null;
                    }
                });
                getFinancialAssessment();
            }
            init();
        }
    ]);

}());
