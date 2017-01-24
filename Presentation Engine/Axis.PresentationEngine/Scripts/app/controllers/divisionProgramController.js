angular.module('xenatixApp')
    .controller('divisionProgramController', ['$scope', '$filter', 'alertService', '$stateParams', '$timeout', '$rootScope', '$q', '$state', 'formService', 'lookupService', 'divisionProgramService', 'navigationService',
        function ($scope, $filter, alertService, $stateParams, $timeout, $rootScope, $q, $state, formService, lookupService, divisionProgramService, navigationService) {
            var resetForm = function () {
                $rootScope.formReset($scope.ctrl.divisionProgramForm, $scope.ctrl.divisionProgramForm.$name);
            };

            $scope.getLookupsByType = function (typeName) {
                return lookupService.getLookupsByType(typeName);
            };
            var isMyProfile = false;
            $scope.init = function () {
                if ($state.current.name == 'myprofile.nav.divisionprograms') 
                    isMyProfile = true;
                else
                    $scope.permissionKey = $state.current.data.permissionKey;
                $scope.$parent['autoFocus'] = true;
                $scope.userID = 0;
                $scope.inProfile = false;
                if ($stateParams.UserID !== undefined && $stateParams.UserID !== null)
                    $scope.userID = $stateParams.UserID;

                $scope.organizations = lookupService.getLookupsByType("Organizations");
                $scope.userProgramDivisions = [];
                $scope.selectedProgramUnits = [];
                $scope.initFields();
                $scope.prepareUserData($scope.userID).then(function () {
                    $scope.getDivisionPrograms();
                });
            };

            $scope.prepareUserData = function (userID) {
                if (userID === 0) {
                    $scope.inProfile = true;
                    return navigationService.get().then(function (response) {
                        $scope.userID = response.DataItems[0].UserID;
                    });
                }

                return $scope.promiseNoOp();
            };

            $scope.initFields = function () {
                $scope.Division = {};
                $scope.Program = {};
                $scope.ProgramUnit = {};
                $scope.selectedProgramUnits = [];
                $scope.IsEdit = false;
                $scope.userDivisionProgramID = 0;
            }

            $scope.addNew = function () {
                $scope.initFields();
                resetForm();
                $scope.ctrl.divisionProgramForm.$setPristine();
            }

            $scope.editUserDivisionProgram = function (divisionID, programID) {
                $scope.IsEdit = true;
                var organizations = $scope.getLookupsByType('Organizations');
                var programDivision = $scope.userProgramDivisions.filter(function (obj) {
                    return obj.MappingID == divisionID;
                })[0];

                $scope.Division = organizations.filter(function (obj) {
                    return obj.ID == divisionID;
                })[0];

                $scope.Program = organizations.filter(function (obj) {
                    return obj.ID == programID;
                })[0];

                var userProgram = programDivision.Programs.filter(function (obj) {
                    return obj.MappingID == programID;
                })[0];
                $scope.userDivisionProgramID = programID;
                $scope.selectedProgramUnits = angular.copy(userProgram.ProgramUnits);
                resetForm();
            };

            $scope.selectDivision = function (item) {
                $scope.Division = { ID: item.ID, Name: item.Name };
                $scope.Program = {};
                $scope.ProgramUnit = {};
                $scope.selectedProgramUnits = [];
            }

            $scope.selectProgram = function (item) {
                $scope.Program = { ID: item.ID, Name: item.Name };
                $scope.ProgramUnit = {};
                $scope.selectedProgramUnits = [];
            }

            $scope.selectProgramUnit = function (item) {
                var idx = -1;
                for (var i = 0; i < $scope.selectedProgramUnits.length; i++) {
                    if ($scope.selectedProgramUnits[i].MappingID === item.ID) {
                        idx = i;
                    }
                }
                if (idx === -1) {
                    $scope.selectedProgramUnits.push({ MappingID: item.ID, Name: item.Name, IsActive: true });
                }
                else if ($scope.selectedProgramUnits[idx].IsActive == undefined || $scope.selectedProgramUnits[idx].IsActive == null || $scope.selectedProgramUnits[idx].IsActive == false) {
                    $scope.selectedProgramUnits[idx].IsActive = true;
                }

                if (!$scope.ctrl.divisionProgramForm.$dirty)
                    $scope.ctrl.divisionProgramForm.$setDirty();

                $scope.ctrl.divisionProgramForm.modified = true;

                $scope.ProgramUnit = {};

            };

            $scope.removeProgramUnit = function (programUnit) {
                var idx = -1;
                for (var i = 0; i < $scope.selectedProgramUnits.length; i++) {
                    if ($scope.selectedProgramUnits[i].MappingID === programUnit.MappingID) {
                        idx = i;
                        break;
                    }
                }
                if (idx > -1) {
                    $scope.selectedProgramUnits[idx].IsActive = false;
                    if (!$scope.ctrl.divisionProgramForm.$dirty)
                        $scope.ctrl.divisionProgramForm.$setDirty();

                    $scope.ctrl.divisionProgramForm.modified = true;
                }
            };

            $scope.getDivisionPrograms = function () {
                $scope.isLoading = true;
                $scope.userProgramDivisions = [];
                var isValid = false;
                return divisionProgramService.getDivisionPrograms($scope.userID, isMyProfile).then(function (data) {
                    if ($scope.userID === 0)
                        $scope.userID = data.ID;
                    if (data.ResultCode === 0) {
                        if (data.DataItems != undefined && data.DataItems.length > 0) {
                            isValid = true;
                            var obj = { stateName: $state.current.name, validationState: 'valid' };
                            $rootScope.staffManagementRightNavigationHandler(obj);
                            $scope.userProgramDivisions = data.DataItems;
                            $scope.Company = data.DataItems[0].CompanyName;
                        } else {
                            $scope.userProgramDivisions = [];
                            if ($scope.scheduleSummaryTable)
                                $scope.scheduleSummaryTable.bootstrapTable('removeAll');
                        }
                    } else {
                        alertService.error('Error while loading User Divisions and Programs');
                    }

                    if (!isValid) {
                        var obj = { stateName: $state.current.name, validationState: 'warning' };
                        $rootScope.staffManagementRightNavigationHandler(obj);
                    }
                },
                function (errorStatus) {
                    alertService.error('Error while loading User Divisions and Programs: ' + errorStatus);
                }).finally(function () {
                    $scope.isLoading = false;
                    resetForm();
                });
            };

            $scope.deleteUserDivisionProgram = function (divisionID, programID) {
                bootbox.confirm("Are you sure that you want to delete this program?", function (result) {
                    if (result === true) {
                        var programDivision = $scope.userProgramDivisions.filter(function (obj) {
                            return obj.MappingID == divisionID;
                        })[0];

                        var programDivisionToDelete = {};
                        programDivisionToDelete.MappingID = programDivision.MappingID;
                        programDivisionToDelete.userID = $scope.userID;
                        if (programDivision.Programs.length == 1)
                            programDivision.IsActive = false;
                        else
                            programDivision.IsActive = true;

                        var program = programDivision.Programs.filter(function (obj) {
                            return obj.MappingID == programID;
                        })[0];
                        angular.forEach(program.ProgramUnits, function (programUnit) {
                            programUnit.IsActive = false;
                        });
                        programDivisionToDelete.Programs = [program];

                        divisionProgramService.saveDivisionProgram(programDivisionToDelete, isMyProfile).then(function (response) {
                            if (response.ResultCode === 0) {
                                alertService.success('User Program Division deleted successfully');
                                $scope.getDivisionPrograms();
                            }
                        });
                    }
                });
            }

            $scope.save = function (isNext, mandatory, hasErrors) {
                if (!mandatory && isNext && hasErrors) {
                    $scope.postSave(isNext);
                }

                if (!formService.isDirty() && isNext && !hasErrors) {
                    $scope.postSave(isNext);
                }

                if ($scope.ctrl.divisionProgramForm.$dirty && !hasErrors && !$scope.inProfile) {

                    var activeProgramUnits = $scope.selectedProgramUnits.filter(function (obj) {
                        return obj.IsActive == true;
                    });

                    if (activeProgramUnits.length <= 0) {
                        alertService.error('Please add Program Unit');
                        return;
                    }

                    var programDivisionToSave = {};
                    programDivisionToSave.MappingID = $scope.Division.ID;
                    programDivisionToSave.userID = $scope.userID;
                    programDivisionToSave.IsActive = true;
                    var program = { MappingID: $scope.Program.ID, IsActive: true };
                    programDivisionToSave.Programs = [program];
                    programDivisionToSave.Programs[0].ProgramUnits = angular.copy($scope.selectedProgramUnits);

                    divisionProgramService.saveDivisionProgram(programDivisionToSave, isMyProfile).then(function (response) {
                        if (response.ResultCode === 0) {
                            alertService.success('User Program Division saved successfully');
                            $scope.initFields();
                            $scope.ctrl.divisionProgramForm.$setPristine();
                            $scope.postSave(isNext);
                        }
                        else {
                            alertService.error('Unable to save Division and Program');
                        }
                    });
                }
            }

            $scope.postSave = function (isNext) {
                if (isNext) {
                    $scope.handleNextState();
                } else {
                    $scope.getDivisionPrograms();
                }
            };

            $scope.handleNextState = function () {
                var nextState = angular.element("li[data-state-name].list-group-item.active").next("li[data-state-name].list-group-item");
                if (nextState.length === 0)
                    $scope.Goto('^');
                else {
                    $timeout(function () {
                        $rootScope.setform(false);
                        var nextStateName = nextState.attr('data-state-name');
                        $scope.Goto(nextStateName, { UserID: $scope.userID });
                    });
                }
            };

            $scope.init();

        }]);