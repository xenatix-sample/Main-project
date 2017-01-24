angular.module('xenatixApp')
    .controller('userSchedulingController', ['$scope', '$filter', 'alertService', '$stateParams', '$timeout', '$rootScope', '$q', '$state', 'formService', 'lookupService', 'userSchedulingService', 'navigationService',
        function ($scope, $filter, alertService, $stateParams, $timeout, $rootScope, $q, $state, formService, lookupService, userSchedulingService, navigationService) {
            var isDirty = false;
            var resetForm = function () {
                $rootScope.formReset($scope.ctrl.schedulingForm, $scope.ctrl.schedulingForm.$name);
            };
            $scope.getLookupsByType = function (typeName) {
                return lookupService.getLookupsByType(typeName);
            };
            var isMyProfile = false;
            if ($state.current.name == 'myprofile.nav.scheduling') {
                isMyProfile = true;
                $scope.preventDisable = 'no-security';
            }
            else
                $scope.permissionKey = $state.current.data.permissionKey;
            $scope.init = function () {
                $scope.$parent['autoFocus'] = true;
                $scope.userID = 0;
                $scope.inProfile = false;
                if ($stateParams.UserID !== undefined && $stateParams.UserID !== null)
                    $scope.userID = $stateParams.UserID;
                $scope.facilities = lookupService.getLookupsByType("Facility");
                $scope.scheduleSummaryTable = $('#scheduleSummaryTable');
                $scope.userWeekSchedule = [];
                $scope.sameAsFacility = false;
                $scope.prepareUserData($scope.userID).then(function () {
                    $scope.getUserFacilities();
                });
            };

            $scope.scheduleTypeOptions = [
                   { ID: 1, Label: "Same as facility" },
                   { ID: 4, Label: "Set Time" }
            ];

            $scope.prepareUserData = function (userID) {
                if (userID === 0) {
                    $scope.inProfile = true;
                    return navigationService.get().then(function (response) {
                        $scope.userID = response.DataItems[0].UserID;
                    });
                }

                return $scope.promiseNoOp();
            };

            //get all the facilities schedule configured for the user
            $scope.getUserFacilities = function () {
                $scope.isLoading = true;
                $scope.userWeekSchedule = [];
                var isValid = false;
                return userSchedulingService.getUserFacilities($scope.userID, isMyProfile).then(function (data) {
                    if ($scope.userID === 0)
                        $scope.userID = data.ID;
                    if (data.ResultCode === 0) {
                        if (data.DataItems != undefined && data.DataItems.length > 0) {
                            isValid = true;
                            var obj = { stateName: $state.current.name, validationState: 'valid' };
                            $rootScope.staffManagementRightNavigationHandler(obj);
                            $scope.userFacilities = data.DataItems;
                            $scope.facilityID = data.DataItems[0].FacilityID;
                            angular.forEach($scope.userFacilities, function (userFacility) {
                                angular.forEach(userFacility.UserFacilitySchedule, function (userFacilitySchedule) {
                                    angular.forEach(userFacilitySchedule.UserFacilityTimeSchedule, function (timeSchedule) {
                                        var startTime = $scope.getTime(timeSchedule.AvailabilityStartTime);
                                        timeSchedule.AvailabilityStartTime = startTime.hrmin;
                                        timeSchedule.startTimeAmPm = startTime.ampm;

                                        var endTime = $scope.getTime(timeSchedule.AvailabilityEndTime);
                                        timeSchedule.AvailabilityEndTime = endTime.hrmin;
                                        timeSchedule.endTimeAmPm = endTime.ampm;
                                    });
                                });
                            });
                            $scope.getUserFacilitySchedule($scope.facilityID);
                        } else {
                            $scope.userSchedulingID = 0;
                            $scope.userFacilities = [];
                            $scope.scheduleSummaryTable.bootstrapTable('removeAll');
                        }
                    } else {
                        alertService.error('Error while loading User Schedule');
                    }

                    if (!isValid) {
                        var obj = { stateName: $state.current.name, validationState: 'warning' };
                        $rootScope.staffManagementRightNavigationHandler(obj);
                    }
                },
                function (errorStatus) {
                    alertService.error('Error while loading User Schedule: ' + errorStatus);
                }).finally(function () {
                    $scope.isLoading = false;
                    resetForm();
                });
            };

            //get specific user facility schedule
            $scope.getUserFacilitySchedule = function (facilityID) {
                $scope.userWeekSchedule = [];
                $scope.facilityID = facilityID;
                if (facilityID === undefined || facilityID === null)
                    return;

                $scope.isLoading = true;
                isDirty = false;
                return userSchedulingService.getUserFacilitySchedule($scope.userID, facilityID, isMyProfile).then(function (data) {
                    if (data.ResultCode === 0) {
                        if (data.DataItems != undefined && data.DataItems.length > 0) {
                            $scope.userData = data.DataItems[0];
                            $scope.userData.sameAsFacility = false;
                            $scope.facilityID = data.DataItems[0].FacilityID;
                            $scope.PrepareData();
                        }
                    } else {
                        alertService.error('Error while loading User Schedule');
                    }
                },
                function (errorStatus) {
                    alertService.error('Error while loading User Schedule: ' + errorStatus);
                }).finally(function () {
                    $scope.isLoading = false;
                    if (isDirty) {
                        $scope.ctrl.schedulingForm.$setDirty();
                    }
                    else {
                        resetForm();
                    }

                });
            }

            $scope.editUserFacilitySchedule = function (facilityID) {
                $scope.getUserFacilitySchedule(facilityID).then(function () {
                    $scope.userSchedulingID = facilityID;
                });
            }

            $scope.PrepareData = function () {
                //here we are merging already saved schedule of user with the facility schedule(only for the days for which schedule is not saved)
                if ($scope.userData.UserFacilitySchedule == null || $scope.userData.UserFacilitySchedule.length == 0) {
                    $scope.SetFacilitySchedule();
                    $scope.userSchedulingID = 0;
                    return;
                }
                for (var i = 1; i <= 7; i++) {
                    var userDaySchedule = $scope.userData.UserFacilitySchedule.filter(function (obj) {
                        return obj.DayOfWeekID == i;
                    })[0];

                    if (userDaySchedule != null && userDaySchedule != undefined) {
                        var userDayScheduleToCopy = angular.copy(userDaySchedule);
                        $scope.userSchedulingID = 0;
                        angular.forEach(userDayScheduleToCopy.UserFacilityTimeSchedule, function (timeSchedule) {
                            var startTime = $scope.getTime(timeSchedule.AvailabilityStartTime);
                            timeSchedule.AvailabilityStartTime = startTime.hrmin;
                            timeSchedule.startTimeAmPm = startTime.ampm;

                            var endTime = $scope.getTime(timeSchedule.AvailabilityEndTime);
                            timeSchedule.AvailabilityEndTime = endTime.hrmin;
                            timeSchedule.endTimeAmPm = endTime.ampm;

                            timeSchedule.IsChecked = true;

                        });
                        $scope.userWeekSchedule.push(userDayScheduleToCopy);
                        continue;
                    }

                    var facilityScheduleToPush = $scope.GetFacilitySchedule(i);
                    $scope.userWeekSchedule.push(facilityScheduleToPush);

                }
            }

            $scope.GetFacilitySchedule = function (i) {
                if ($scope.userData.FacilitySchedule != null || $scope.userData.FacilitySchedule.length != 0) {
                    var facilitySchedule = $scope.userData.FacilitySchedule.filter(function (obj) {
                        return obj.DayOfWeekID == i;
                    })[0];

                    if (facilitySchedule != null && facilitySchedule != undefined) {
                        var facilityScheduleToCopy = angular.copy(facilitySchedule);
                        facilityScheduleToCopy.ScheduleTypeID = 1;
                        angular.forEach(facilityScheduleToCopy.UserFacilityTimeSchedule, function (timeSchedule) {
                            var startTime = $scope.getTime(timeSchedule.AvailabilityStartTime);
                            timeSchedule.AvailabilityStartTime = startTime.hrmin;
                            timeSchedule.startTimeAmPm = startTime.ampm;

                            var endTime = $scope.getTime(timeSchedule.AvailabilityEndTime);
                            timeSchedule.AvailabilityEndTime = endTime.hrmin;
                            timeSchedule.endTimeAmPm = endTime.ampm;
                            timeSchedule.ResourceAvailabilityID = 0;
                        });
                        isDirty = true;
                        return facilityScheduleToCopy;
                    }
                    else {
                        var defaultSchedule = {
                            DayOfWeekID: i,
                            Days: $scope.GetDayName(i),
                            ScheduleTypeID: 1,
                            RowCount: 1,
                            UserFacilityTimeSchedule: [{
                                ResourceAvailabilityID: 0,
                                AvailabilityStartTime: 'hh:mm',
                                AvailabilityEndTime: 'hh:mm',
                                startTimeAmPm: 'AM',
                                endTimeAmPm: 'PM',
                                IsFirst: true,
                                IsLast: true,
                                IsActive: true,
                                RowNumber: 0,
                                IsChecked: false
                            }]

                        };
                        return defaultSchedule;
                    }
                }
            }

            $scope.initializescheduleSummaryTable = function () {
                $scope.tableoptions = {
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
                            field: 'FacilityID',
                            title: 'Location',
                            formatter: function (value, row) {
                                return getFacilityName(value, $scope.facilities);
                            }
                        },
                        {
                            field: 'Days',
                            title: 'Day',
                            formatter: function (value, row) {
                                return value;
                            }
                        },

                        {
                            field: 'FacilityID',
                            title: '',
                            formatter: function (value, row) {
                                return (
                                    '<a href="javascript:void(0)" data-default-no-action ng-click="editUserFacilitySchedule(' + value + ');" ' +
                                    'id="editUserFacilitySchedule" name="editUserFacilitySchedule" title="Edit Schedule"' +
                                    'space-key-press><i class="fa fa-file-text fa-fw"></i></a>' +

                                    '<a href="javascript:void(0)" data-default-no-action ng-click="deleteUserFacility(' + value + ')" id="deleteUserFacility" name="deleteUserFacility" title="Deactivate" ' +
                                    'space-key-press><i class="fa fa-trash fa-fw"></i></a>'
                                    );
                            }
                        }
                    ]
                };
            };

            $scope.SetSameAsFacility = function () {
                if ($scope.userData.sameAsFacility)
                    return;

                bootbox.confirm("Are you sure that you want to set same as facility? This will remove any current and pending schedules for this facility", function (result) {
                    if (result === true) {
                        $scope.SetFacilitySchedule();
                    }
                    else {
                        $scope.userData.sameAsFacility = false;
                        $scope.$digest();
                    }
                });
            }

            $scope.SetFacilitySchedule = function () {
                if ($scope.userData.FacilitySchedule == null || $scope.userData.FacilitySchedule.length == 0) {
                    alertService.error('Facility Schedule not available');
                    return;
                }

                $scope.userWeekSchedule = [];
                for (var i = 1; i <= 7; i++) {
                    var facilityScheduleToPush = $scope.GetFacilitySchedule(i);
                    $scope.userWeekSchedule.push(facilityScheduleToPush);
                }
            }

            $scope.scheduleTypeChanged = function (userDaySchedule) {
                if (userDaySchedule.ScheduleTypeID === 1) {
                    var facilityScheduleToPush = $scope.GetFacilitySchedule(userDaySchedule.DayOfWeekID);
                    userDaySchedule.UserFacilityTimeSchedule = facilityScheduleToPush.UserFacilityTimeSchedule;
                }
                else {
                    $scope.userData.sameAsFacility = false;
                    angular.forEach(userDaySchedule.UserFacilityTimeSchedule, function (timeSchedule) {
                        timeSchedule.IsChecked = true;
                    });
                }
            }

            $scope.getFacilityName = function (value, list) {
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

            $scope.getTime = function (militaryTime) {
                var obj = new Object();
                obj.hrmin = $filter('toStandardTime')(militaryTime);
                obj.ampm = $filter('toStandardTimeAMPM')(militaryTime);
                return obj;
            };

            $scope.pad = function (num, width, char) {
                char = char || '0';
                num = num + '';
                return num.length >= width ? num : new Array(width - num.length + 1).join(char) + num;
            }

            $scope.CBClicked = function () {
                $scope.ctrl.schedulingForm.$setDirty();
            }

            $scope.validateStartTime = function (userSchedule, ampm) {
                userSchedule.startTimeAmPm = ampm;
                $scope.validateTime(userSchedule);

            }

            $scope.validateEndTime = function (userSchedule, ampm) {
                userSchedule.endTimeAmPm = ampm;
                $scope.validateTime(userSchedule);
                $scope.ctrl.schedulingForm.$setDirty();
            }

            $scope.validateTime = function (userSchedule) {

                if (userSchedule.AvailabilityStartTime > 1259 || userSchedule.AvailabilityStartTime == '' || userSchedule.AvailabilityStartTime == undefined || userSchedule.AvailabilityStartTime == 0000)
                    userSchedule.StartTimeInvalid = true;
                else
                    userSchedule.StartTimeInvalid = false;

                if (userSchedule.AvailabilityEndTime > 1259 || userSchedule.AvailabilityEndTime == '' || userSchedule.AvailabilityEndTime == undefined || userSchedule.AvailabilityEndTime == 0000)
                    userSchedule.EndTimeInvalid = true;
                else
                    userSchedule.EndTimeInvalid = false;

                if (userSchedule.StartTimeInvalid == true || userSchedule.EndTimeInvalid == true)
                    return;

                var max = 2359;
                var min = 00;
                var starttime = $filter('toMilitaryTime')(userSchedule.AvailabilityStartTime, userSchedule.startTimeAmPm);
                var startTimeValid = starttime >= min && starttime < max;
                if (!startTimeValid)
                    userSchedule.StartTimeInvalid = true;

                var endtime = $filter('toMilitaryTime')(userSchedule.AvailabilityEndTime, userSchedule.endTimeAmPm);
                var endTimeValid = endtime >= min && endtime < max;
                if (!endTimeValid)
                    userSchedule.EndTimeInvalid = true;

                if (userSchedule.startTimeAmPm == 'PM' && userSchedule.endTimeAmPm == 'AM') {
                    userSchedule.StartTimeInvalid = true;
                    userSchedule.EndTimeInvalid = true;
                    return;
                }

                if (userSchedule.startTimeAmPm == userSchedule.endTimeAmPm && starttime > endtime) {
                    userSchedule.StartTimeInvalid = true;
                    userSchedule.EndTimeInvalid = true;
                }

            }

            $scope.GetDayName = function (dayWeekId) {
                switch (dayWeekId) {
                    case 1:
                        return 'Monday';
                    case 2:
                        return 'Tuesday';
                    case 3:
                        return 'Wednesday';
                    case 4:
                        return 'Thursday';
                    case 5:
                        return 'Friday';
                    case 6:
                        return 'Saturday';
                    case 7:
                        return 'Sunday';
                }
            }

            $scope.addDaySchedule = function (dayWeekId) {
                var userDaySchedule = $scope.userWeekSchedule.filter(function (obj) {
                    return obj.DayOfWeekID == dayWeekId;
                })[0];

                var newUserFacilityTimeSchedule = {
                    ResourceAvailabilityID: 0,
                    AvailabilityStartTime: 'hh:mm',
                    AvailabilityEndTime: 'hh:mm',
                    startTimeAmPm: 'AM',
                    endTimeAmPm: 'PM',
                    IsFirst: false,
                    IsLast: true,
                    IsActive: true,
                    RowNumber: userDaySchedule.UserFacilityTimeSchedule.length,
                    IsChecked: (userDaySchedule.ScheduleTypeID != 1) ? true : false
                };

                userDaySchedule.UserFacilityTimeSchedule.push(newUserFacilityTimeSchedule);
                userDaySchedule.RowCount++;
                userDaySchedule.UserFacilityTimeSchedule[userDaySchedule.UserFacilityTimeSchedule.length - 2].IsLast = false;
                $scope.ctrl.schedulingForm.$setDirty();
            }

            $scope.deleteDaySchedule = function (dayWeekId, rowNumber) {
                var userDaySchedule = $scope.userWeekSchedule.filter(function (obj) {
                    return obj.DayOfWeekID == dayWeekId;
                })[0];

                if (userDaySchedule.UserFacilityTimeSchedule[rowNumber].ResourceAvailabilityID != 0) {   //existing record in DB
                    userDaySchedule.UserFacilityTimeSchedule[rowNumber].IsActive = false;
                } else {  //not in DB yet, so remove it
                    userDaySchedule.UserFacilityTimeSchedule.splice(rowNumber, 1);
                }

                var scheduleInactive = true;
                angular.forEach(userDaySchedule.UserFacilityTimeSchedule, function (timeSchedule) {
                    if (timeSchedule.IsActive) {
                        scheduleInactive = false;
                    }
                });

                if (userDaySchedule.UserFacilityTimeSchedule.length === 0 || scheduleInactive) {
                    var newUserFacilityTimeSchedule = {
                        ResourceAvailabilityID: 0,
                        AvailabilityStartTime: 'hh:mm',
                        AvailabilityEndTime: 'hh:mm',
                        startTimeAmPm: 'AM',
                        endTimeAmPm: 'PM',
                        IsFirst: false,
                        IsLast: true,
                        IsActive: true,
                        RowNumber: userDaySchedule.UserFacilityTimeSchedule.length,
                        IsChecked: (userDaySchedule.ScheduleTypeID != 1) ? true : false
                    };

                    userDaySchedule.UserFacilityTimeSchedule.push(newUserFacilityTimeSchedule);
                } else {
                    userDaySchedule.UserFacilityTimeSchedule[userDaySchedule.UserFacilityTimeSchedule.length - 1].IsLast = true;
                }

                var rowIndex = 0;
                var firstFound = false;
                angular.forEach(userDaySchedule.UserFacilityTimeSchedule, function (timeSchedule) {
                    timeSchedule.RowNumber = rowIndex;
                    if (timeSchedule.IsActive && !firstFound) {
                        timeSchedule.IsFirst = true;
                        firstFound = true;
                    }
                    rowIndex++;
                });
                $scope.ctrl.schedulingForm.$setDirty();
            }

            $scope.isOverlapping = function () {
                var retValue = false;
                angular.forEach($scope.userWeekSchedule, function (schedule) {
                    if (schedule.UserFacilityTimeSchedule.length > 1) {
                        tempUserFacilityTimeSchedule = $filter('filter')(angular.copy(schedule.UserFacilityTimeSchedule), { IsActive: true, IsChecked: true });
                        var sortedTimeSchedule = tempUserFacilityTimeSchedule.sort(function (prev, current) {
                            previousTime = $filter('toMilitaryTime')(prev.AvailabilityStartTime, prev.startTimeAmPm);
                            currentTime = $filter('toMilitaryTime')(current.AvailabilityStartTime, current.startTimeAmPm);
                            if (previousTime < currentTime) {
                                return -1;
                            }
                            if (previousTime === currentTime) {
                                return 0;
                            }
                            return 1;
                        });

                        for (var i = 1; i < sortedTimeSchedule.length; i++) {
                            var previous = sortedTimeSchedule[i - 1];
                            var current = sortedTimeSchedule[i];
                            var previousEndTime = $filter('toMilitaryTime')(previous.AvailabilityEndTime, previous.endTimeAmPm);
                            var currentStartTime = $filter('toMilitaryTime')(current.AvailabilityStartTime, current.startTimeAmPm);
                            if (previousEndTime >= currentStartTime) {
                                retValue = true;
                                return;
                            }
                        }
                    }
                });
                return retValue;
            }


            $scope.save = function (isNext, mandatory, hasErrors) {

                var hasError = false;
                angular.forEach($scope.userWeekSchedule, function (schedule) {
                    angular.forEach(schedule.UserFacilityTimeSchedule, function (timeSchedule) {
                        if (timeSchedule.StartTimeInvalid == true || timeSchedule.EndTimeInvalid == true) {
                            hasError = true;
                        }
                    });
                });

                if (hasError) {
                    alertService.error('Please correct the highlighted errors before submitting the form');
                    return false;
                }

                if (!mandatory && isNext && hasErrors) {
                    $scope.postSave(isNext);
                }

                if (!formService.isDirty() && isNext) {
                    $scope.postSave(isNext);
                }

                //we don't want to check $dirty here, as there can be facility schedule merged with user schedule and user can save this data without changing anything in form
                if ($scope.ctrl.schedulingForm.$dirty && !hasErrors && !$scope.inProfile) {
                    var overlapping = $scope.isOverlapping();
                    if (overlapping) {
                        alertService.error('You cannot overlap time for a single day');
                        return;
                    }
                    var userScheduleToSave = {};
                    userScheduleToSave.FacilityID = $scope.facilityID;
                    userScheduleToSave.ResourceID = $scope.userID;

                    var userFacilitySchedulesToSave = [];

                    angular.forEach($scope.userWeekSchedule, function (schedule) {
                        var facilityScheduleToSave = angular.copy(schedule);
                        facilityScheduleToSave.UserFacilityTimeSchedule = [];

                        angular.forEach(schedule.UserFacilityTimeSchedule, function (timeSchedule) {
                            if (timeSchedule.IsActive == true && timeSchedule.IsChecked && timeSchedule.AvailabilityStartTime != 'hh:mm' && timeSchedule.AvailabilityEndTime != 'hh:mm') {
                                timeScheduleToPush = angular.copy(timeSchedule);
                                timeScheduleToPush.AvailabilityStartTime = $filter('toMilitaryTime')(timeScheduleToPush.AvailabilityStartTime, timeScheduleToPush.startTimeAmPm);
                                timeScheduleToPush.AvailabilityEndTime = $filter('toMilitaryTime')(timeScheduleToPush.AvailabilityEndTime, timeScheduleToPush.endTimeAmPm);
                                facilityScheduleToSave.UserFacilityTimeSchedule.push(timeScheduleToPush);
                            }

                        });
                        userFacilitySchedulesToSave.push(facilityScheduleToSave);
                    });

                    userScheduleToSave.UserFacilitySchedule = userFacilitySchedulesToSave;

                    userSchedulingService.saveUserFacilitySchedule(userScheduleToSave, isMyProfile).then(function (response) {
                        if (response.ResultCode === 0) {
                            alertService.success('User Schedule saved successfully');
                            $scope.ctrl.schedulingForm.$setPristine();
                            $scope.postSave(isNext);
                        }
                    });

                }
            }

            $scope.postSave = function (isNext) {
                if (isNext) {
                    $scope.handleNextState();
                } else {
                    $scope.getUserFacilities();
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
