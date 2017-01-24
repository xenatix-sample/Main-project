(function () {
    angular.module('xenatixApp')
    .controller('programUnitConfigurationController', ['$filter', 'alertService', '$state', "_", 'formService', '$scope', '$q', '$rootScope', 'programUnitConfigurationService', 'organizationStructureService', 'lookupService', 'helperService', '$controller',
        function ($filter, alertService, $state, _, formService, $scope, $q, $rootScope, programUnitConfigurationService, organizationStructureService, lookupService, helperService, $controller) {
            $controller('baseContactController', { $scope: $scope });

            var programUnitsTable = $("#programUnitsTable");
            var companyID = 1; // default company is "MHMR Tarrant"
            var divisions = helperService.getOrganizationDetails('Division');
            var services = lookupService.getLookupsByType('ServiceDetails');
            var serviceWorkflows = lookupService.getLookupsByType('ServiceWorkflowType');

            $scope.companies = helperService.getOrganizationDetails('Company');
            $scope.programs = helperService.getOrganizationDetails('Program');
            $scope.AddressAccessCode = $scope.ADDRESS_ACCESS.Required | $scope.ADDRESS_ACCESS.EffectiveDate;

            var Address = function () {
                return {
                    AddressID: 0,
                    AddressTypeID: 4,
                    Line1: '',
                    Line2: '',
                    City: '',
                    StateProvince: 0,
                    County: 0,
                    Zip: '',
                    MailPermissionID: 1,
                    ShowPrimary: true,
                    IsEffectiveDate: true,
                    IsExpirationDate: true,
                    ShowPlusButton: true,
                    IsMultiAddress: true
                };
            }

            var initializeAddress = function () {
                $scope.Addresses = [];
                $scope.Addresses.push(new Address());
            }

            var initProgramHierarchyAssignment = function () {
                $scope.divisions = $.map(divisions, function (item, indx) {
                    return {
                        text: item.Name,
                        value: item.ID
                    };
                });

                $scope.programUnitHierarchy = {
                    ProgramID: null,
                    Divisions: []
                };
            }

            var initProgramUnitServiceAssignment = function () {
                $scope.services = $.map(services, function (item, indx) {
                    var mappedService = _.find($scope.programUnitDetails.ProgramUnitServices, { ServiceID: item.ID })
                    if (!mappedService) {
                        return {
                            text: item.Name,
                            value: item.ID
                        }
                    }
                });

                $scope.programUnitService = {
                    DetailID: null,
                    Services: []
                };
            }

            var initProgramUnitServiceWorkflowAssignment = function () {
                $scope.serviceWorkflows = $.map(serviceWorkflows, function (item, indx) {
                    var mappedServiceWorklows = _.find($scope.programUnitDetails.ProgramUnitServiceWorkflows, {
                        ModuleComponentID: item.ID
                    });
                    if (!mappedServiceWorklows) {
                        return {
                            text: item.Name,
                            value: item.ID
                        }
                    }
                });

                $scope.programUnitServiceWorkflow = {
                    DetailID: null,
                    ServiceWorkflows: []
                };
            }

            var init = function () {
                initializeBootstrapTable();
                $scope.initProgramUnit();
                var promise = [];
                promise.push(getProgramUnits());
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
                            title: 'Program Unit Name',
                            sortable: true
                        },
                        {
                            field: 'Programs',
                            title: 'Programs',
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
                                return '<span class="text-nowrap"><a href="javascript:void(0)" data-default-action id="edit" name="edit" data-toggle="modal"  ng-click="edit(' + value + ')" title="Edit" security permission-key="BusinessAdministration-Configuration-ProgramUnit" permission="update" space-key-press><i class="fa fa-pencil fa-fw" /></a></span>'
                            }
                        }
                    ]
                };
            }

            var resetForm = function () {
                if ($scope.programUnitForm)
                    $rootScope.formReset($scope.programUnitForm, $scope.programUnitForm.$name);
                if ($scope.addressForm)
                    $rootScope.formReset($scope.addressForm, $scope.addressForm.$name);
                formService.reset();
            };

            var resetServiceFlyout = function () {
                var dfd = $q.defer();
                $rootScope.formReset($scope.addServiceForm, $scope.addServiceForm.$name);
                dfd.resolve();
                return dfd.promise;
            }

            var resetServiceWorkflowFlyout = function () {
                var dfd = $q.defer();
                $rootScope.formReset($scope.addServiceWorkflowForm, $scope.addServiceWorkflowForm.$name);
                dfd.resolve();
                return dfd.promise;
            }

            var resetProgramFlyout = function () {
                var dfd = $q.defer();
                $rootScope.formReset($scope.addProgramForm, $scope.addProgramForm.$name);
                dfd.resolve();
                return dfd.promise;
            }

            var formatAllDates = function (programUnitDetails) {
                angular.forEach(programUnitDetails.Addresses, function (item) {
                    item.EffectiveDate = formattedDate(item.EffectiveDate);
                    item.ExpirationDate = formattedDate(item.ExpirationDate);
                    item.IsExpirationDate = true;
                    item.IsEffectiveDate = true;
                    item.ShowPrimary = true;
                })
                angular.forEach(programUnitDetails.ProgramUnitHierarchies, function (item) {
                    item.EffectiveDate = formattedDate(item.EffectiveDate);
                    item.ExpirationDate = formattedDate(item.ExpirationDate);
                })
                angular.forEach(programUnitDetails.ProgramUnitServices, function (item) {
                    item.EffectiveDate = formattedDate(item.EffectiveDate);
                    item.ExpirationDate = formattedDate(item.ExpirationDate);
                })
                angular.forEach(programUnitDetails.ProgramUnitServiceWorkflows, function (item) {
                    item.EffectiveDate = formattedDate(item.EffectiveDate);
                    item.ExpirationDate = formattedDate(item.ExpirationDate);
                })
                programUnitDetails.ProgramUnit.EffectiveDate = formattedDate(programUnitDetails.ProgramUnit.EffectiveDate);
                programUnitDetails.ProgramUnit.ExpirationDate = formattedDate(programUnitDetails.ProgramUnit.ExpirationDate);
            }

            var formattedDate = function (date, defaultValue) {
                return helperService.getFormattedDate(date, defaultValue);
            }

            var getProgramUnits = function () {
                return programUnitConfigurationService.getProgramUnits().then(function (data) {
                    if (hasData(data)) {
                        var programUnits = data.DataItems;
                        programUnitsTable.bootstrapTable('load', programUnits);
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

            var onProgramUnitFlyoutClosed = function () {
                $rootScope.defaultFormName = getDefaultFormName();
                $('.program-flyout').removeClass('active');
            }

            var onServiceFlyoutClosed = function () {
                $rootScope.defaultFormName = getDefaultFormName();
                $('.service-flyout').removeClass('active');
            }

            var onServiceWorkflowFlyoutClosed = function () {
                $rootScope.defaultFormName = getDefaultFormName();
                $('.serviceWorkflow-flyout').removeClass('active');
            }

            var setPlusMinusButtons = function (items) {
                angular.forEach(items, function (data, index) {
                    data.IsMultiAddress = true;
                    if (index !== items.length - 1) {
                        data.ShowPlusButton = false;

                    }
                    else {
                        data.ShowPlusButton = true;
                    }
                });
            }

            var onEdit = function (programUnitID) {
                return programUnitConfigurationService.getProgramUnitByID(programUnitID).then(function (data) {
                    if (hasData(data)) {
                        $scope.programUnitDetails = data.DataItems[0];
                        formatAllDates($scope.programUnitDetails);
                        if ($scope.programUnitDetails.Addresses.length > 0) {
                            $scope.Addresses = $scope.programUnitDetails.Addresses;
                            setPlusMinusButtons($scope.Addresses);
                        }
                        else {
                            initializeAddress();
                        }
                        $scope.pageSecurity = 1;
                        resetForm();
                    }
                }, function (errorStatus) {
                    alertService.error('OOPS! Something went wrong');
                });
            }

            $scope.initProgramUnit = function () {
                $scope.pageSecurity = 0;
                $scope.programUnitDetails = {
                    ProgramUnit: {
                        EffectiveDate: formattedDate(new Date())
                    },
                    ReportingUnit: {
                        OrganizationIdentifierTypeID: 5
                    },
                    ProgramUnitHierarchies: [],
                    ProgramUnitServices: [],
                    ProgramUnitServiceWorkflows: []
                }
                initializeAddress();
                resetForm();
            };

            $scope.addNewAddress = function () {
                $scope.Addresses.push(new Address());
                setPlusMinusButtons($scope.Addresses);
            }

            $scope.removeAddress = function (index) {
                $scope.Addresses.splice(index, 1);
                setPlusMinusButtons($scope.Addresses);
            }

            $scope.save = function (isNext, mandatory, hasErrors) {
                if (!hasErrors && (formService.isDirty() || formService.isDirty($scope.programUnitForm.$name) || formService.isDirty($scope.addressForm.$name))) {
                    $scope.programUnitDetails.Addresses = $scope.Addresses;
                    formatAllDates($scope.programUnitDetails);
                    return programUnitConfigurationService.saveProgramUnit($scope.programUnitDetails).then(function (data) {
                        if (data.data.ResultCode == 0) {
                            if (!$scope.programUnitDetails.ProgramUnit.DetailID) {
                                alertService.success('Program Unit has been added');
                            }
                            else {
                                alertService.success('Program Unit has been updated');
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


            $scope.edit = function (programUnitID) {
                if (formService.isAnyFormDirty()) {
                    bootbox.confirm("Any unsaved data will be lost. Do you want to continue?", function (result) {
                        if (result == true) {
                            onEdit(programUnitID);
                        }
                    })
                }
                else {
                    onEdit(programUnitID);
                }
            };

            $scope.saveProgramUnitAssignment = function (isNext, mandatory, hasErrors) {
                if ($scope.programUnitHierarchy && $scope.programUnitHierarchy.Divisions && $scope.programUnitHierarchy.Divisions.length > 0) {
                    $rootScope.defaultFormName = null;
                    bootbox.dialog({
                        message: "Are your selections correct?",
                        buttons: {
                            danger: {
                                label: "No",
                                className: "btn-danger",
                                callback: function () {
                                    $rootScope.defaultFormName = 'addProgramForm';
                                }
                            },
                            success: {
                                label: "Yes",
                                className: "btn-success",
                                callback: function () {
                                    var newHierarchyAssignment = $.map($scope.programUnitHierarchy.Divisions, function (item, indx) {
                                        return {
                                            ProgramUnitID: $scope.programUnitDetails.ProgramUnit.DetailID,
                                            ProgramID: $scope.programUnitHierarchy.ProgramID,
                                            ProgramName: helperService.getOrganizationDetails('Program', $scope.programUnitHierarchy.ProgramID),
                                            DivisionID: item.value,
                                            DivisionName: helperService.getOrganizationDetails('Division', item.value),
                                            EffectiveDate: formattedDate(new Date())
                                        };
                                    });

                                    $scope.programUnitDetails.ProgramUnitHierarchies = $scope.programUnitDetails.ProgramUnitHierarchies.concat(newHierarchyAssignment)
                                    resetProgramFlyout().then(function () {
                                        formService.initForm(true);
                                        $scope.closeProgramFlyout();
                                    })
                                }
                            }
                        }
                    });
                }
            };

            $scope.saveProgramUnitServicesAssignment = function (isNext, mandatory, hasErrors) {
                if ($scope.programUnitService && $scope.programUnitService.Services && $scope.programUnitService.Services.length > 0) {
                    $rootScope.defaultFormName = null;
                    bootbox.dialog({
                        message: "Are your selections correct?",
                        buttons: {
                            danger: {
                                label: "No",
                                className: "btn-danger",
                                callback: function () {
                                    $rootScope.defaultFormName = 'addServiceForm';
                                }
                            },
                            success: {
                                label: "Yes",
                                className: "btn-success",
                                callback: function () {
                                    var newServiceAssignment = $.map($scope.programUnitService.Services, function (item, indx) {
                                        return {
                                            DetailID: $scope.programUnitDetails.ProgramUnit.DetailID,
                                            ServiceID: item.value,
                                            ServiceName: lookupService.getText(LOOKUPTYPE.ServiceDetails, item.value),
                                            EffectiveDate: formattedDate(new Date())
                                        };
                                    });
                                    $scope.programUnitDetails.ProgramUnitServices = $scope.programUnitDetails.ProgramUnitServices.concat(newServiceAssignment);
                                    resetServiceFlyout().then(function () {
                                        formService.initForm(true);
                                        $scope.closeServiceFlyout();
                                    })
                                }
                            }
                        }
                    });
                }
            };

            $scope.saveProgramUnitServiceWorkflowAssignment = function (isNext, mandatory, hasErrors) {
                if ($scope.programUnitServiceWorkflow && $scope.programUnitServiceWorkflow.ServiceWorkflows && $scope.programUnitServiceWorkflow.ServiceWorkflows.length > 0) {
                    $rootScope.defaultFormName = null;
                    bootbox.dialog({
                        message: "Are your selections correct?",
                        buttons: {
                            danger: {
                                label: "No",
                                className: "btn-danger",
                                callback: function () {
                                    $rootScope.defaultFormName = 'addServiceWorkflowForm';
                                }
                            },
                            success: {
                                label: "Yes",
                                className: "btn-success",
                                callback: function () {
                                    var newServiceWorkflowAssignment = $.map($scope.programUnitServiceWorkflow.ServiceWorkflows, function (item, indx) {
                                        return {
                                            DetailID: $scope.programUnitDetails.ProgramUnit.DetailID,
                                            ModuleComponentID: item.value,
                                            Feature: lookupService.getText(LOOKUPTYPE.ServiceWorkflowType, item.value),
                                            EffectiveDate: formattedDate(new Date())
                                        };
                                    });
                                    $scope.programUnitDetails.ProgramUnitServiceWorkflows = $scope.programUnitDetails.ProgramUnitServiceWorkflows.concat(newServiceWorkflowAssignment);
                                    resetServiceWorkflowFlyout().then(function () {
                                        formService.initForm(true);
                                        $scope.closeServiceWorkflowFlyout();
                                    })
                                }
                            }
                        }
                    });
                }
            };

            $scope.removeHierarchyAssignment = function (index) {
                $scope.programUnitDetails.ProgramUnitHierarchies.splice(index, 1);
            }

            $scope.removeServiceAssignment = function (index) {
                $scope.programUnitDetails.ProgramUnitServices.splice(index, 1);
            }

            $scope.removeServiceWorkflowAssignment = function (index) {
                formService.initForm(true);
                $scope.programUnitDetails.ProgramUnitServiceWorkflows.splice(index, 1);
            }

            $scope.onProgramChange = function () {
                $scope.divisions = $.map(divisions, function (item, indx) {
                    var mappedDivision = _.find($scope.programUnitDetails.ProgramUnitHierarchies, { DivisionID: item.ID, ProgramID: $scope.programUnitHierarchy.ProgramID })
                    if (!mappedDivision) {
                        return {
                            text: item.Name,
                            value: item.ID
                        }
                    }
                });
            }

            $scope.openProgramFlyOut = function () {
                $rootScope.defaultFormName = 'addProgramForm';
                initProgramHierarchyAssignment();
                resetProgramFlyout();
                $('.program-flyout').addClass('active');
            }

            $scope.closeProgramFlyout = function () {
                if (formService.isDirty('addProgramForm')) {
                    bootbox.confirm("Any unsaved data will be lost. Do you want to continue?", function (result) {
                        if (result == true) {
                            onProgramUnitFlyoutClosed();
                        }
                    });
                } else {
                    onProgramUnitFlyoutClosed();
                }
            }

            $scope.openServiceFlyOut = function () {
                $rootScope.defaultFormName = 'addServiceForm';
                initProgramUnitServiceAssignment();
                resetServiceFlyout();
                $('.service-flyout').addClass('active');
            }

            $scope.closeServiceFlyout = function () {
                if (formService.isDirty('addServiceForm')) {
                    bootbox.confirm("Any unsaved data will be lost. Do you want to continue?", function (result) {
                        if (result == true) {
                            onServiceFlyoutClosed();
                        }
                    });
                } else {
                    onServiceFlyoutClosed();
                }
            }
            
            $scope.openServiceWorkflowFlyOut = function () {
                $rootScope.defaultFormName = 'addServiceWorkflowForm';
                initProgramUnitServiceWorkflowAssignment();
                resetServiceWorkflowFlyout();
                $('.serviceWorkflow-flyout').addClass('active');
            }
            
            $scope.closeServiceWorkflowFlyout = function () {
                if (formService.isDirty('addServiceWorkflowForm')) {
                    bootbox.confirm("Any unsaved data will be lost. Do you want to continue?", function (result) {
                        if (result == true) {
                            onServiceWorkflowFlyoutClosed();
                        }
                    });
                } else {
                    onServiceWorkflowFlyoutClosed();
                }
            }

            $scope.repositionDatepicker = function (id) {
                var datepickerElement = angular.element(document.querySelector('#' + id + ' .dropdown-menu'));
                if (datepickerElement !== null && datepickerElement !== undefined && datepickerElement[0] !== undefined)
                    repositionElement(datepickerElement);
            }

            $scope.setPrimary = function (addressModel, index) {
                if (addressModel.length > 1) {
                    angular.forEach(addressModel, function (item) {
                        if (item.IsPrimary == true) {
                            item.IsPrimary = false;
                        }
                    })
                }
            }

            init();
        }]);
}());