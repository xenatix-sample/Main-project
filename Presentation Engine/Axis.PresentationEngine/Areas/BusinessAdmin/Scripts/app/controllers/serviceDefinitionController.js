(function () {
    angular.module('xenatixApp')
    .controller('serviceDefinitionController', ['$filter', 'alertService', '$stateParams', '$state', 'formService', '$scope', '$q', '$rootScope', '$timeout', 'lookupService', 'helperService', 'serviceDefinitionService', '_',
        function ($filter, alertService, $stateParams, $state, formService, $scope, $q, $rootScope, $timeout, lookupService, helperService, serviceDefinitionService, _) {
            var programUnits = helperService.getOrganizationDetails('ProgramUnit');
            var serviceDefinitionState = "servicedefinition";

            var init = function () {

                $scope.permissionKey = $state.current.data.permissionKey;

                initProgramUnits();

                $scope.serviceDefinition = {
                    ServicesID: 0,
                    ServiceCode: "",
                    ServiceName: "",
                    ServiceConfigServiceTypeID: null,
                    EffectiveDate: "",
                    ExpirationDate: "",
                    ExpirationReason: "",
                    EncounterReportable: false,
                    ServiceDefinition: "",
                    ProgramUnitHierarchies: [],
                    Notes: ""
                }
                getAndReset();
            }


            var initProgramUnits = function () {
                $scope.programUnits = angular.copy(programUnits);
                $scope.ProgramUnitHierarchies = {
                    ProgramID: null,
                    ProgramNames: []
                };
            };

            var checkFormStatus = function (state) {
                $timeout(function () {
                    var stateDetail = { stateName: serviceDefinitionState, validationState: state ? 'valid' : 'invalid' };
                    $rootScope.$broadcast('rightNavigationServicesHandler', stateDetail);
                })
            };

            var getServiceDefinition = function () {
                if ($stateParams.ServicesID) {
                    return serviceDefinitionService.getServiceDefinitionByID($stateParams.ServicesID).then(function (data) {
                        if (hasData(data)) {
                            $scope.serviceDefinition = data.DataItems[0];
                            formatAllDates($scope.serviceDefinition);
                            $scope.pageSecurity = $scope.serviceDefinition.ServicesID;
                            checkFormStatus(true);
                        }
                    })
                }
                else {
                    $scope.pageSecurity = 0;
                    return $scope.promiseNoOp();
                }
            }

            var getAndReset = function () {
                getServiceDefinition().then(function () {
                    resetForm();
                });
            }

            var formatAllDates = function (serviceDefinition) {
                serviceDefinition.EffectiveDate = formattedDate(serviceDefinition.EffectiveDate);
                serviceDefinition.ExpirationDate = formattedDate(serviceDefinition.ExpirationDate);

                angular.forEach(serviceDefinition.ProgramUnitHierarchies, function (item) {
                    item.EffectiveDate = formattedDate(item.EffectiveDate);
                    item.ExpirationDate = formattedDate(item.ExpirationDate);
                })

            }

            var formattedDate = function (date, defaultValue) {
                return helperService.getFormattedDate(date, defaultValue);
            }

            var onProgramUnitFlyoutClosed = function () {
                $rootScope.defaultFormName = getDefaultFormName();
                $('.program-flyout').removeClass('active');
                resetProgramFlyout();

            }

            var resetForm = function () {
                if ($scope.serviceDefinitionForm)
                    $rootScope.formReset($scope.serviceDefinitionForm);
                if ($scope.addProgramUnitsForm)
                    $rootScope.formReset($scope.addProgramUnitsForm, $scope.addProgramUnitsForm.$name);
            }

            var resetProgramFlyout = function () {
                if ($scope.addProgramUnitsForm)
                    $rootScope.formReset($scope.addProgramUnitsForm, $scope.addProgramUnitsForm.$name);
                return $scope.promiseNoOp();
            }

            $scope.save = function (isNext, mandatory, hasErrors, keepForm) {
                if (formService.isDirty($scope.serviceDefinitionForm.$name)) {
                    formatAllDates($scope.serviceDefinition);
                    $scope.serviceDefinition.ModifiedOn = $filter('formatDate')(new Date());
                    return serviceDefinitionService.saveServiceDefinition($scope.serviceDefinition).then(function (data) {
                        if (data.data.ResultCode == 0) {
                            if (!$stateParams.ServicesID) {
                                alertService.success('Service Definition has been added');
                                resetForm();
                                if (isNext) {
                                    next(data.data.ID);
                                } else {
                                    $state.transitionTo(serviceDefinitionState, { ServicesID: data.data.ID });
                                }
                            }
                            else {
                                alertService.success('Service Definition has been updated');
                                resetForm();
                                if (isNext) {
                                    next($stateParams.ServicesID);
                                }
                                else {
                                    getAndReset();
                                }
                            }
                        }
                    });
                }
                else {
                    if (isNext) {
                        next($stateParams.ServicesID);
                    }
                    return $scope.promiseNoOp();
                }
            }

            var next = function (servicesID) {
                $timeout(function () {
                    $scope.Goto("servicedetails", {
                        ServicesID: servicesID
                    });
                });
            }

            $scope.saveProgramUnitAssignment = function (isNext, mandatory, hasErrors) {
                    $rootScope.defaultFormName = "None";
                    if ($scope.ProgramUnitHierarchies && hasDetails($scope.ProgramUnitHierarchies.ProgramNames)) {
                        bootbox.dialog({
                            title: 'Confirm Program Units',
                            message: "Are your selections correct?",
                            backdrop: true,
                            buttons: {
                                danger: {
                                    label: "No",
                                    className: "btn-danger",
                                    callback: function () {
                                    $rootScope.defaultFormName = 'addProgramUnitsForm';
                                    }
                                },
                                success: {
                                    label: "Yes",
                                    className: "btn-success",
                                    callback: function () {
                                        var newProgramUnit = $.map($scope.ProgramUnitHierarchies.ProgramNames, function (item, indx) {
                                            return {
                                                DetailID: item.value,
                                                Name: item.text,
                                                EffectiveDate: formattedDate(new Date())
                                            };
                                        });

                                        $scope.serviceDefinition.ProgramUnitHierarchies = $scope.serviceDefinition.ProgramUnitHierarchies.concat(newProgramUnit)
                                        resetProgramFlyout().then(function () {
                                            formService.initForm(true, $scope.serviceDefinitionForm.$name);
                                            $scope.closeProgramFlyout();
                                        })
                                    }
                                }
                            }
                        });
                    }
            };

            $scope.removeHierarchyAssignment = function (index) {
                $scope.serviceDefinition.ProgramUnitHierarchies.splice(index, 1);
            }


            $scope.openProgramFlyOut = function () {
                $rootScope.defaultFormName = 'addProgramUnitsForm';
                initProgramUnits();
                onProgramUnitAdd();
                resetProgramFlyout();
                $('.program-flyout').addClass('active');
            }

            var onProgramUnitAdd = function () {
                var programUnit = $.grep(programUnits, function (item, indx) {
                    var mappedDivision = _.find($scope.serviceDefinition.ProgramUnitHierarchies, {
                        DetailID: item.ID
                    })
                    if (!mappedDivision) {
                        return item;
                    }
                });
                $scope.programUnits = angular.copy(programUnit);
            }

            $scope.closeProgramFlyout = function () {
                if (formService.isDirty('addProgramUnitsForm')) {
                    bootbox.confirm("Any unsaved data will be lost. Do you want to continue?", function (result) {
                        if (result == true) {
                            onProgramUnitFlyoutClosed();
                        }
                    });
                } else {
                    onProgramUnitFlyoutClosed();
                }
            }


            $scope.repositionDatepicker = function (id) {
                var datepickerElement = angular.element(document.querySelector('#' + id + ' .dropdown-menu'));
                if (datepickerElement !== null && datepickerElement !== undefined && datepickerElement[0] !== undefined)
                    repositionElement(datepickerElement);
            }


            init();

        }]);
})();
