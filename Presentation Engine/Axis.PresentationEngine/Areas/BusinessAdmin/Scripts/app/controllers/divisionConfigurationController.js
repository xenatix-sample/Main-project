(function () {
    angular.module('xenatixApp')
    .controller('divisionConfigurationController', ['$filter', 'alertService', '$state', 'formService', '$scope', '$q', '$rootScope', 'divisionConfigurationService', 'organizationStructureService', 'lookupService', 'helperService', '$controller',
        function ($filter, alertService, $state, formService, $scope, $q, $rootScope, divisionConfigurationService, organizationStructureService, lookupService, helperService, $controller) {
            var divisionsTable = $("#divisionsTable");
            var companyID = 1; // default company is "MHMR Tarrant"
            $scope.pageSecurity = undefined;

            var init = function () {
                initializeBootstrapTable();
                $scope.initDivisionUnit();
                var promise = [];
                promise.push(getDivisions());
                promise.push(getCompany());
                $q.all(promise).then(function () {
                    resetForm();
                })
            }

            var initializeBootstrapTable = function () {
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
                            field: 'Name',
                            title: 'Division Name',
                            sortable: true
                        },
                        {
                            field: 'Acronym',
                            title: 'Division Acronym',
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
                                    return formattedDate(value);
                                } else
                                    return '';
                            }
                        },

                        {
                            field: 'DetailID',
                            title: 'Actions',
                            formatter: function (value, row, index) {
                                return '<span class="text-nowrap"><a href="javascript:void(0)" data-default-action id="edit" name="edit" data-toggle="modal"  ng-click="edit(' + value + ')" title="Edit" security permission-key="BusinessAdministration-Configuration-Division" permission="update" space-key-press><i class="fa fa-eye fa-fw" /></a></span>'
                            }
                        }
                    ]
                };
            }

            var resetForm = function () {
                if ($scope.divisionForm)
                    $rootScope.formReset($scope.divisionForm, $scope.divisionForm.$name);
                if ($scope.addressForm)
                    $rootScope.formReset($scope.addressForm, $scope.addressForm.$name);
                formService.reset();
            };

            var formatAllDates = function (divisionDetails) {
                angular.forEach(divisionDetails.DivisionHierarchies, function (item) {
                    item.EffectiveDate = formattedDate(item.EffectiveDate);
                    item.ExpirationDate = formattedDate(item.ExpirationDate);
                })                
                divisionDetails.Division.EffectiveDate = formattedDate(divisionDetails.Division.EffectiveDate);
                divisionDetails.Division.ExpirationDate = formattedDate(divisionDetails.Division.ExpirationDate);
            }

            var formattedDate = function (date, defaultValue) {
                return helperService.getFormattedDate(date, defaultValue);
            }

            var getDivisions = function () {
                return divisionConfigurationService.getDivisions().then(function (data) {
                    if (hasData(data)) {
                        var divisions = data.DataItems;
                        divisionsTable.bootstrapTable('load', divisions);
                    }
                }, function (errorStatus) {
                    alertService.error('OOPS! Something went wrong');
                });
            };

            var getCompany = function () {
                return organizationStructureService.getOrganizationStructureByID(companyID).then(function (data) {
                    if (hasData(data)) {
                        $scope.company = data.DataItems[0];
                        $scope.company.EffectiveDate = formattedDate($scope.company.EffectiveDate);
                        $scope.company.ExpirationDate = formattedDate($scope.company.ExpirationDate);
                    }
                }, function (errorStatus) {
                    alertService.error('OOPS! Something went wrong');
                });
            }
            
            var onEdit = function (divisionID) {
                return divisionConfigurationService.getDivisionByID(divisionID).then(function (data) {
                    if (hasData(data)) {
                        $scope.divisionDetails = data.DataItems[0];
                        formatAllDates($scope.divisionDetails);                        
                        resetForm();
                    }
                }, function (errorStatus) {
                    alertService.error('OOPS! Something went wrong');
                });
            }

            $scope.companies = helperService.getOrganizationDetails('Company');

            $scope.initDivisionUnit = function () {
                $scope.divisionDetails = {
                    Division: {
                        EffectiveDate: formattedDate(new Date())
                    },                    
                    DivisionHierarchies: []
                }
                resetForm();
            };

            $scope.save = function (isNext, mandatory, hasErrors) {
                if (!hasErrors && (formService.isDirty() || formService.isDirty($scope.divisionForm.$name))) {
                    formatAllDates($scope.divisionDetails);
                    return divisionConfigurationService.saveDivision($scope.divisionDetails).then(function (data) {
                        if (data.data.ResultCode == 0) {
                            if (!$scope.divisionDetails.Division.DetailID) {
                                alertService.success('Division has been added');
                            }
                            else {
                                alertService.success('Division has been updated');
                            }
                            init();
                        }
                    }, function (errorStatus) {
                        alertService.error('OOPS! Something went wrong');
                    });
                }
                else {
                    return $scope.promiseNoOp();
                }
            };

            $scope.edit = function (divisionID) {
                if (formService.isAnyFormDirty()) {
                    bootbox.confirm("Any unsaved data will be lost. Do you want to continue?", function (result) {
                        if (result == true) {
                            onEdit(divisionID);
                        }
                    })
                }
                else {
                    onEdit(divisionID);
                }
            };

            $scope.repositionDatepicker = function (id) {
                var datepickerElement = angular.element(document.querySelector('#' + id + ' .dropdown-menu'));
                if (datepickerElement !== null && datepickerElement !== undefined && datepickerElement[0] !== undefined)
                    repositionElement(datepickerElement);
            }

            init();
        }]);
}());