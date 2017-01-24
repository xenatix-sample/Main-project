(function () {
    angular.module('xenatixApp')
    .controller('programConfigurationController', ['$filter', 'alertService', '$state', "_", 'formService', '$scope', '$q', '$rootScope', 'programConfigurationService', 'organizationStructureService', 'lookupService', 'helperService', '$controller',
        function ($filter, alertService, $state, _, formService, $scope, $q, $rootScope, programConfigurationService, organizationStructureService, lookupService, helperService, $controller) {
            var programsTable = $("#programsTable");
            var companyID = 1; // default company is "MHMR Tarrant"

            var divisions = helperService.getOrganizationDetails('Division');

            $scope.companies = helperService.getOrganizationDetails('Company');

            var initProgramDivisionAssignment = function () {
                $scope.divisions = $.map(divisions, function (item, indx) {
                    var mappedDivision = _.find($scope.programDetails.ProgramHierarchies, { DivisionID: item.ID })
                    if (!mappedDivision) {
                        return {
                            text: item.Name,
                            value: item.ID
                        }
                    }
                });

                $scope.programHierarchy = {
                    ProgramID: null,
                    Divisions: []
                };
            }

            var init = function () {
                initializeBootstrapTable();
                $scope.initProgram();
                var promise = [];
                promise.push(getPrograms());
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
                            title: 'Program Name',
                            sortable: true
                        },
                        {
                            field: 'Divisions',
                            title: 'Divisions',
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
                                return '<span class="text-nowrap"><a href="javascript:void(0)" data-default-action id="edit" name="edit" data-toggle="modal"  ng-click="edit(' + value + ')" title="Edit" security permission-key="BusinessAdministration-Configuration-Program" permission="update" space-key-press><i class="fa fa-pencil fa-fw" /></a></span>'
                            }
                        }
                    ]
                };
            }

            var resetForm = function () {
                if ($scope.programForm)
                    $rootScope.formReset($scope.programForm, $scope.programForm.$name);
                formService.reset();
            };

            var resetDivisionFlyout = function () {
                var dfd = $q.defer();
                $rootScope.formReset($scope.addDivisionForm, $scope.addDivisionForm.$name);
                dfd.resolve();
                return dfd.promise;
            }

            var formatAllDates = function (programDetails) {
                angular.forEach(programDetails.ProgramHierarchies, function (item) {
                    item.EffectiveDate = formattedDate(item.EffectiveDate);
                    item.ExpirationDate = formattedDate(item.ExpirationDate);
                })
                angular.forEach(programDetails.DivisionHierarchies, function (item) {
                    item.EffectiveDate = formattedDate(item.EffectiveDate);
                    item.ExpirationDate = formattedDate(item.ExpirationDate);
                })
                programDetails.Program.EffectiveDate = formattedDate(programDetails.Program.EffectiveDate);
                programDetails.Program.ExpirationDate = formattedDate(programDetails.Program.ExpirationDate);
            }

            var formattedDate = function (date, defaultValue) {
                return helperService.getFormattedDate(date, defaultValue);
            }

            var getPrograms = function () {
                return programConfigurationService.getPrograms().then(function (data) {
                    if (hasData(data)) {
                        var programs = data.DataItems;
                        programsTable.bootstrapTable('load', programs);
                        applyDropupOnGrid();
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

            var onDivisionFlyoutClosed = function () {
                $rootScope.defaultFormName = getDefaultFormName();
                $('.division-flyout').removeClass('active');
            }

            var onEdit = function (programID) {
                return programConfigurationService.getProgramByID(programID).then(function (data) {
                    if (hasData(data)) {
                        $scope.programDetails = data.DataItems[0];
                        formatAllDates($scope.programDetails);
                        $scope.pageSecurity = 1;
                        resetForm();
                    }
                }, function (errorStatus) {
                    alertService.error('OOPS! Something went wrong');
                });
            }

            $scope.initProgram = function () {
                $scope.pageSecurity = 0;
                $scope.programDetails = {
                    Program: {
                        EffectiveDate: formattedDate(new Date())
                    },
                    ProgramHierarchies: [],
                    DivisionHierarchies: []
                }
                resetForm();
            };

            $scope.save = function (isNext, mandatory, hasErrors) {
                if (!hasErrors && (formService.isDirty() || formService.isDirty($scope.programForm.$name))) {
                    formatAllDates($scope.programDetails);
                    if ($scope.programDetails.Program.ExpirationDate) {
                        bootbox.confirm("The program will be expired from the program unit. Do you want to continue?", function (result) {
                            if (result == true) {
                                saveProgram();
                            }
                        })
                    }
                    else {
                        saveProgram();
                    }
                }
                else {
                    return $scope.promiseNoOp();
                }
            };

            var saveProgram = function () {
                return programConfigurationService.saveProgram($scope.programDetails).then(function (data) {
                    if (data.data.ResultCode == 0) {
                        if (!$scope.programDetails.Program.DetailID) {
                            alertService.success('Program has been added');
                        }
                        else {
                            alertService.success('Program has been updated');
                        }
                        init();
                    }
                }, function (errorStatus) {
                    alertService.error('OOPS! Something went wrong');
                });
            }

            $scope.edit = function (programID) {
                if (formService.isAnyFormDirty()) {
                    bootbox.confirm("Any unsaved data will be lost. Do you want to continue?", function (result) {
                        if (result == true) {
                            onEdit(programID);
                        }
                    })
                }
                else {
                    onEdit(programID);
                }
            };

            $scope.saveProgramDivisionAssignment = function (isNext, mandatory, hasErrors) {
                if ($scope.programHierarchy && $scope.programHierarchy.Divisions && $scope.programHierarchy.Divisions.length > 0) {
                    $rootScope.defaultFormName = null;
                    bootbox.dialog({
                        message: "Are your selections correct?",
                        buttons: {
                            danger: {
                                label: "No",
                                className: "btn-danger",
                                callback: function () {
                                    $rootScope.defaultFormName = 'addDivisionForm';
                                }
                            },
                            success: {
                                label: "Yes",
                                className: "btn-success",
                                callback: function () {
                                    var newHierarchyAssignment = $.map($scope.programHierarchy.Divisions, function (item, indx) {
                                        return {
                                            ProgramID: $scope.programDetails.Program.DetailID,
                                            DivisionID: item.value,
                                            DivisionName: helperService.getOrganizationDetails('Division', item.value),
                                            EffectiveDate: formattedDate(new Date())
                                        };
                                    });

                                    $scope.programDetails.ProgramHierarchies = $scope.programDetails.ProgramHierarchies.concat(newHierarchyAssignment)
                                    resetDivisionFlyout().then(function () {
                                        formService.initForm(true);
                                        $scope.closeDivisionFlyout();
                                    })
                                }
                            }
                        }
                    });
                }
            };

            $scope.removeHierarchyAssignment = function (index) {
                $scope.programDetails.ProgramHierarchies.splice(index, 1);
            }

            $scope.openDivisionFlyOut = function () {
                $rootScope.defaultFormName = 'addDivisionForm';
                initProgramDivisionAssignment();
                resetDivisionFlyout();
                $('.division-flyout').addClass('active');
            }

            $scope.closeDivisionFlyout = function () {
                if (formService.isDirty('addDivisionForm')) {
                    bootbox.confirm("Any unsaved data will be lost. Do you want to continue?", function (result) {
                        if (result == true) {
                            onDivisionFlyoutClosed();
                        }
                    });
                } else {
                    onDivisionFlyoutClosed();
                }
            }

            $scope.repositionDatepicker = function (id) {
                var datepickerElement = angular.element(document.querySelector('#' + id + ' .dropdown-menu'));
                if (datepickerElement !== null && datepickerElement !== undefined && datepickerElement[0] !== undefined)
                    repositionElement(datepickerElement);
            }

            init();
        }]);
}());