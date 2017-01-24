angular.module('xenatixApp')
.controller('medicalHistoryController', [
    '$scope', '$filter', 'medicalHistoryService', 'alertService', 'lookupService', 'navigationService', '$stateParams', '$state', '$rootScope', 'formService', '$q',
    function ($scope, $filter, medicalHistoryService, alertService, lookupService, navigationService, $stateParams, $state, $rootScope, formService, $q) {

        $scope.MedicalHistoryList = [];

        $scope.init = function() {
            $scope.$parent['autoFocus'] = true;
            $scope.initLookups();
            $scope.setDefaultDatePickerSettings();
            $scope.medicalHistoryTable = $('#medicalHistoryTable');
            $scope.endDate = new Date();
            $scope.initializeBootstrapTable();
            $scope.newMedicalHistory();
            $scope.get();
            $scope.medicalHistory = { MedicalHistoryID: 0 };
            $scope.ContactID = $stateParams.ContactID;
            $('#takenTime').timepicker({
                minuteStep: 1,
                showInputs: false,
                //disableFocus: true
            });
        };

        $scope.resetForm = function () {
            if ($scope != null && $scope.ctrl != null && $scope.ctrl.medicalHistoryForm != null) {
                $scope.formReset($scope.ctrl.medicalHistoryForm);
            }
        };

        $scope.newMedicalHistory = function () {
            $scope.medicalHistory = {
                MedicalHistoryID: 0,
                Conditions: [],
                ContactID: $scope.ContactID,
                EncounterID: null,
                TakenBy: null,
                TakenTime: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal')
            };

            $scope.initTakenDetails();
        };

        $scope.initTakenDetails = function () {
            $scope.takenOnDetail = {
                TakenBy: 0,
                TakenOnDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
                TakenOnTime: $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal')
            };

            navigationService.get().then(function (data) {
                $scope.takenOnDetail = {
                    TakenBy: data.DataItems[0].UserID,
                    TakenOnDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
                    TakenOnTime: $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal')
                };

                $scope.UserID = $scope.takenOnDetail.TakenBy;
                $scope.resetForm();
            });
        };

        $scope.initLookups = function () {
            $scope.UserList = lookupService.getLookupsByType('Users');
        };

        $scope.setDefaultDatePickerSettings = function () {
            $scope.opened = false;

            $scope.dateOptions = {
                formatYear: 'yy',
                startingDay: 1,
                showWeeks: false
            };

            $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'MM/dd/yyyy'];
            $scope.format = $scope.formats[3];
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
                onClickRow: function (e, row, $element) {
                    $scope.rowEdit(e);
                },
                columns: [
                    {
                        field: 'TakenBy',
                        title: 'Taken By',
                        formatter: function (value, row) {
                            return getText(value, $scope.UserList);
                        }
                    },
                    {
                        field: 'TakenTime',
                        title: 'Taken Date',
                        formatter: function (value, row) {
                            return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY hh:mm A', 'useLocal');
                        }
                    },
                    
                    {
                        field: 'MedicalHistoryID',
                        title: '',
                        formatter: function (value, row) {
                            return (
                                '<a href="javascript:void(0)" data-default-action id="editMedicalHistory" ng-click="editMedicalHistoryDetails(' + value + ');" name="editMedicalHistory" data-toggle="modal" data-target="#MedicalHistoryModel" ' +
                                'security permission-key="Clinical-MedicalHistory-MedicalHistory" permission="update" title="Edit" space-key-press><i class="fa fa-pencil fa-fw" /></a> ' +
                                '<a href="javascript:void(0)" data-default-no-action ng-click="delete(' + value + ')" id="deleteAllergy" security permission-key="Clinical-MedicalHistory-MedicalHistory" permission="delete" name="deleteAllergy" title="Deactivate" ' +
                                'space-key-press><i class="fa fa-trash fa-fw"></i></a>'
                                );
                        }
                    }
                ]
            };
        };

        $scope.editMedicalHistoryDetails = function (medicalHistoryID) {
            if (event)
                event.stopPropagation();

            $scope.next(medicalHistoryID);
        }

        $scope.next = function (medicalHistoryID) {
            var params = {
                ContactID: $stateParams.ContactID,
                MedicalHistoryID: medicalHistoryID
            }

            $state.go('patientprofile.chart.intake.medicalhistory.medicalhistorydetails', params);
        }

        $scope.get = function () {
            return medicalHistoryService.getMedicalHistoryBundle($scope.contactID).then(function (data) {
                if (data.ResultCode === 0) {
                    if (data.DataItems != undefined && data.DataItems.length > 0) {
                        $scope.MedicalHistoryList = data.DataItems;
                        $scope.medicalHistoryTable.bootstrapTable('load', data.DataItems);
                    } else {
                        $scope.medicalHistory.MedicalHistoryID = 0;
                        $scope.medicalHistoryTable.bootstrapTable('removeAll');
                    }
                } else {
                    alertService.error('Error while loading medical history details');
                }

                    $scope.resetForm();
                },
           function (errorStatus) {
               alertService.error('Error while getting medical history list: ' + errorStatus);
           });
        };

        $scope.delete = function (medicalHistoryID) {
            event.stopPropagation();
            bootbox.confirm('Are you sure you want to deactivate?', function (confirmed) {
                if (confirmed) {
                    medicalHistoryService.deleteMedicalHistory(medicalHistoryID, $scope.ContactID).then(function (data) {
                        if (data.ResultCode === 0) {
                            alertService.success('Medical History deactivated successfully');
                            if ($scope.medicalHistory.MedicalHistoryID == medicalHistoryID)
                            {
                                $scope.newMedicalHistory();
                            }
                            $scope.get();
                        } else {
                            alertService.error('Error while deactivating Medical History');
                        }
                    });
                }
            });
        };

        $scope.save = function (isNext, mandatory, hasErrors) {
            if (!mandatory && isNext && hasErrors) {
                next();
            }
            var isAdd = ($scope.medicalHistory.MedicalHistoryID === 0 || $scope.medicalHistory.MedicalHistoryID === undefined);

            if (!hasErrors) {
                if (isAdd || formService.isDirty($scope.ctrl.medicalHistoryForm.name)) {
                    var dateVal = new Date($scope.takenOnDetail.TakenOnDate);
                    var timeVal = $scope.takenOnDetail.TakenOnTime;
                    var hr = timeVal.substring(0, timeVal.indexOf(':'));
                    if (timeVal.substring(timeVal.indexOf(' ') +1, timeVal.length) == "PM" && hr != 12)      //checks if PM, adds 12 hours
                        hr = +hr + +12;
                    var min = timeVal.substring(timeVal.indexOf(':') + 1, timeVal.indexOf(' '));
                    var dateTime = dateVal.setHours(hr, min);

                    $scope.medicalHistory.TakenTime = new Date(dateTime);
                    $scope.medicalHistory.ContactID = $stateParams.ContactID;
                    $scope.medicalHistory.TakenBy = $scope.takenOnDetail.TakenBy;
                    $scope.saveMedicalHistory(isAdd, isNext);
                } else if (isNext) {
                    $scope.next($scope.medicalHistory.MedicalHistoryID);
                }
            }
        };

        // when saving a medical history, there currently is no updating, only adding a new medical history, then associating it to copies of another 
        $scope.saveMedicalHistory = function (isAdd, isNext) {
            var newMedicalHistory = angular.copy($scope.medicalHistory);
            var newMedicalHistoryID;
            medicalHistoryService.addMedicalHistory(newMedicalHistory).then(function (dataNewMedicalHistory) {
                newMedicalHistoryID = newMedicalHistory.MedicalHistoryID = dataNewMedicalHistory.data.ID;
                var sourceMedicalHistoryID = 0;
                if (isAdd) {
                    // adding, so get the latest medical history to pull the details from
                    var latestMedicalHistory = null;
                    if ($scope.MedicalHistoryList.length > 0) {
                        angular.forEach($scope.MedicalHistoryList, function(medicalHistory) {
                            if (latestMedicalHistory == null) {
                                latestMedicalHistory = medicalHistory;
                            } else {
                                if (medicalHistory.TakenTime.valueOf() > latestMedicalHistory.TakenTime.valueOf()) {
                                    latestMedicalHistory = medicalHistory;
                                }
                            }
                        });
                    }

                    if (latestMedicalHistory != null)
                        sourceMedicalHistoryID = latestMedicalHistory.MedicalHistoryID;
                } else {
                    // updating, so get the currently selected medical history to pull the details from
                    sourceMedicalHistoryID = $scope.medicalHistory.MedicalHistoryID;
                }

                if (sourceMedicalHistoryID != 0) {
                    // get the details for which we will be copying
                    medicalHistoryService.getMedicalHistoryConditionDetails(sourceMedicalHistoryID, $scope.ContactID).then(function (sourceDetails) {
                        if(sourceDetails && sourceDetails.DataItems) {

                            newMedicalHistory = angular.copy(sourceDetails.DataItems);
                            angular.forEach(newMedicalHistory.Conditions, function (newDetailCondition) {
                                angular.forEach(sourceDetails.DataItems, function (sourceDetailCondition) {
                                    if(sourceDetailCondition.MedicalHistoryConditionID == newDetailCondition.MedicalHistoryConditionID) {
                                        newDetailCondition.Details = angular.copy(sourceDetailCondition.Details);

                                        angular.forEach(newDetailCondition.Details, function (newCondtionDetail) {
                                            newCondtionDetail.MedicalHistoryConditionDetailID = 0;
                                            newCondtionDetail.MedicalHistoryConditionID = 0;
                                        });

                                    }
                                });

                                newDetailCondition.MedicalHistoryID = newMedicalHistory.MedicalHistoryID;
                                newDetailCondition.MedicalHistoryConditionID = 0;
                            });

                            return medicalHistoryService.saveMedicalHistoryConditions(newMedicalHistory).then(function (response) {
                                if(response.data.ResultCode === 0) {
                                    if ($scope.medicalHistory != undefined && $scope.medicalHistory.MedicalHistoryID != undefined) {
                                        var successMessage = 'Medical History has been ' +(isAdd ? 'added': 'updated') + ' successfully.';
                                    }
                                    alertService.success(successMessage);

                                    if (isNext) {
                                        $scope.next(newMedicalHistoryID);
                                    } else {
                                        $scope.newMedicalHistory();
                                        $scope.get();
                                    }
                                } else {
                                    alertService.error('Error while adding Medical History');
                                }
                            });
                        }
                    });
                } else {
                    if (isNext) {
                        $scope.next(newMedicalHistory.MedicalHistoryID);
                    }
                    $scope.newMedicalHistory();
                    $scope.get();
                }
            });
        };

        $scope.rowEdit = function (row) {
            $scope.prepRowEditData(row);
        };

        $scope.prepRowEditData = function (row) {
            $scope.medicalHistory = row;
            $scope.takenOnDetail.TakenBy = $scope.medicalHistory.TakenBy;
            $scope.takenOnDetail.TakenOnDate = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal');
            $scope.takenOnDetail.TakenOnTime = $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal');
            $scope.resetForm();
        };

        $scope.init();

        function getText(value, list) {
            if (value) {
                var formattedValue = lookupService.getSelectedTextById(value, list);
                if (formattedValue != undefined && formattedValue.length > 0) {
                    return formattedValue[0].Name;
                } else {
                    return '';
                }
            } else {
                return '';
            }
        };
    }
]);