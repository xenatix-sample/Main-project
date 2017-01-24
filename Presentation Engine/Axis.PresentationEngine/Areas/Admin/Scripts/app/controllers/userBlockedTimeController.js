angular.module('xenatixApp')
    .controller('userBlockedTimeController', ['$scope', '$q', '$filter', '$injector', 'formService', 'alertService', '$stateParams', 'lookupService', '$timeout', '$rootScope', '$state',
        function ($scope, $q, $filter, $injector, formService, alertService, $stateParams, lookupService, $timeout, $rootScope, $state) {

            var FACILITY_RESOURCE_TYPE = 6;
            var APPT_TYPE = "Blocked Time";
            $scope.permissionKey = $state.current.data.permissionKey;
            // Get scheduling plugin 'resourceService'
            if ($injector.has('appointmentService'))
                $scope.appointmentService = $injector.get('appointmentService');
            else {
                bootbox.alert("Appointment Service is not available, please load the Scheduling plugin!");
                return;
            }

            $scope.blockedTimesList = [];
            $scope.LocationID = $stateParams.LocationID;

            $scope.init = function () {

                // Init stuff
                $scope.blockedTime = {
                    BlockedTimeID: 0,
                    Reason: "",
                    StartDate: new Date(),
                    StartTime: new Date(),
                    StartAmPm: $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal').indexOf('AM') > -1 ? "AM" : "PM",
                    EndDate: '',
                    EndTime: '',
                    EndAmPm: "PM",
                    Comment: "My new comment here...",
                    AllDay: true
                };

                // Set some defaults
                $scope.blockedTime.ContactID = 0;
                $scope.blockedTime.IsCancelled = false;
                $scope.blockedTime.ProgramID = 1;
                $scope.blockedTime.SupervisionVisit = false;
                $scope.blockedTime.ReferredBy = '';
                $scope.blockedTime.CancelComment = '';
                $scope.isUpdate = false;

                // TMP DEBUG: set fake recurrence
                var recurrence = {};
                recurrence.EndDate = new Date(99, 4, 18);
                $scope.blockedTime.Recurrence = recurrence;

                // Set appt type
                $scope.getAppointmentType();

                // Get current blocked times for this location
                $scope.getBlockedTimes();
            }

            $scope.getAppointmentType = function () {
                var q = $q.defer();
                $scope.appointmentTypes = $filter('filter')(lookupService.getLookupsByType('AppointmentTypes'), { AppointmentType: APPT_TYPE }, true);
                q.resolve($scope.appointmentTypes);
                if ($scope.appointmentTypes.length != 0) {
                    $scope.apptType = $scope.appointmentTypes[0].AppointmentTypeID;
                }
                return q.promise;
            };

            $scope.getApptResourceModel = function () {
                var apptresmodel = {};
                apptresmodel.AppointmentID = $scope.blockedTime.BlockedTimeID;
                apptresmodel.ResourceTypeID = FACILITY_RESOURCE_TYPE;
                apptresmodel.ResourceID = $scope.LocationID;
                return apptresmodel;
            }

            $scope.getApptModel = function () {
                var apptmodel = {};
                if ($scope.blockedTime.BlockedTimeID != 0 && $scope.isUpdate)
                    apptmodel.AppointmentID = $scope.blockedTime.BlockedTimeID;
                apptmodel.ContactID = $scope.blockedTime.ContactID;
                apptmodel.ProgramID = $scope.blockedTime.ProgramID;
                apptmodel.AppointmentTypeID = $scope.apptType;
                apptmodel.AppointmentDate = $scope.blockedTime.StartDate;
                apptmodel.AppointmentStartTime = $scope.formatTime($scope.blockedTime.StartTime, $scope.blockedTime.StartAmPm);
                apptmodel.AppointmentLength = $scope.getTimeDiffInMins($scope.blockedTime);
                apptmodel.SupervisionVisit = $scope.blockedTime.SupervisionVisit;
                apptmodel.ReasonForVisit = $scope.blockedTime.Reason;
                apptmodel.FacilityID = $scope.LocationID;
                apptmodel.Comments = $scope.blockedTime.Comment;
                return apptmodel;
            }

            $scope.formatTime = function (time, ampm) {
                var hours = parseInt(time.toString().substring(0, 2));
                var mins = parseInt(time.toString().substring(3, 5));
                if (ampm == "PM" && hours < 12) hours = hours + 12;
                if (ampm == "AM" && hours == 12) hours = hours - 12;
                var sHours = hours.toString();
                var sMinutes = mins.toString();
                if (hours < 10) sHours = "0" + sHours;
                if (mins < 10) sMinutes = "0" + sMinutes;
                return sHours + sMinutes;
            }

            $scope.getTimeDiffInMins = function (blockedTime) {
                // TODO: Get time diff in mins from startDate/startTime to endDate/endTime
                return 60;
            }

            $scope.formatFacilityBlockedTimes = function (data) {

                // TODO: format incmomming data to blocked times  

                // TMP DEBUG
                var tmpblockedlist = [];
                var recurrence = {
                    Occurence: 10,
                    Day: 'WED, FRI',
                    Frequency: 'Weekly',
                    EndDate: new Date(17, 2, 2)
                };
                var tmpblockedTime = {
                    BlockedTimeID: 33,
                    Reason: "Holiday",
                    StartDate: new Date(),
                    StartTime: new Date(),
                    StartAmPm: 'AM',
                    EndDate: new Date(99, 1, 1),
                    EndTime: new Date(99, 1, 1, 15, 35, 50),
                    EndAmPm: "PM",
                    Comment: "Saved comment"
                };
                tmpblockedTime.Recurrence = recurrence;
                tmpblockedlist.push(tmpblockedTime);

                var tmpbockedTime2 = {
                    BlockedTimeID: 44,
                    Reason: "Day Off",
                    StartDate: new Date(),
                    StartTime: new Date(),
                    StartAmPm: 'PM',
                    EndDate: new Date(91, 4, 7),
                    EndTime: new Date(91, 4, 7, 13, 20, 50),
                    EndAmPm: "PM",
                    Comment: "Comment on year 91"
                };
                tmpbockedTime2.Recurrence = recurrence;
                tmpblockedlist.push(tmpbockedTime2);
                return tmpblockedlist;
            }

            $scope.onAllDayClick = function () {
                alert('All day click, value: ' + $scope.blockedTime.AllDay);
            }

            $scope.validateAppointmentTime = function () {
                $timeout(function () {
                    //$scope.setAmPmForAppointmentEndTime();
                    $scope.validateTime1(true);
                    $scope.validateTime(false);
                });

            }

            $scope.validateTime1 = function (isStartTime) {
                var format = $scope.blockedTime.StartAmPm;
                var time = $filter('toMilitaryTime')($scope.blockedTime.StartTime, format);
                var time1 = $filter('toStandardTime')($scope.blockedTime.StartTime, format);
                var time2 = $filter('toStandardTimeAMPM')($scope.blockedTime.StartTime, format);
            }

            $scope.validateTime = function (isStartTime) {
                var max = 1800;
                var min = 800;
                var format = $scope.blockedTime.StartAmPm;
                var time = 0;
                var isValid = false;

                if ($scope.ctrl == undefined || $scope.ctrl.scheduleForm == undefined)
                    return;

                if (format != undefined && $scope.blockedTime.StartTime && $scope.blockedTime.EndTime) {
                    if (isStartTime == true) {
                        time = $filter('toMilitaryTime')($scope.blockedTime.StartTime, format);
                        isValid = time >= min && time < max;
                        $scope.ctrl.scheduleForm.startTime.$setValidity("startTimeError", isValid);
                    }
                    else {
                        format = $scope.blockedTime.EndAmPm;
                        time = $filter('toMilitaryTime')($scope.blockedTime.EndTime, format);
                        isValid = time > min && time <= max;
                        $scope.ctrl.scheduleForm.endTime.$setValidity("startTimeError", isValid);
                    }
                }
            }

            $scope.syncDateAndLength = function (time) {

            }

            $scope.addNew = function () {
                $scope.resetFields();
                //if (formService.isDirty()) {
                //    bootbox.confirm("Current blocked time changes will be discarded, do you want to proceed?", function (result) {
                //        if (result === true) {
                //            $scope.resetFields();
                //        }
                //    });
                //}
                //else
                //    $scope.resetFields();
            }



            $scope.resetFields = function () {
                $scope.isUpdate = false;
                $scope.blockedTime.BlockedTimeID = 0;
                $scope.blockedTime.Reason = '';
                $scope.blockedTime.StartDate = new Date();
                $scope.blockedTime.StartTime = $scope.blockedTime.StartDate;
                $scope.blockedTime.StartAmPm = $filter('toMMDDYYYYDate')($scope.blockedTime.StartDate, 'hh:mm A', 'useLocal').indexOf('AM') > -1 ? "AM" : "PM";
                $scope.blockedTime.EndDate = '';
                $scope.blockedTime.EndTime = '';
                $scope.blockedTime.EndAmPm = '';
                $scope.blockedTime.Comment = '';
                $scope.blockedTime.AllDay = false;
            }

            $scope.editBlockedTime = function (item) {
                $scope.isUpdate = true;
                $scope.blockedTime.BlockedTimeID = item.BlockedTimeID;
                $scope.blockedTime.Reason = item.Reason;
                $scope.blockedTime.StartDate = item.StartDate;
                $scope.blockedTime.StartTime = item.StartTime;
                $scope.blockedTime.StartAmPm = item.StartAmPm;
                $scope.blockedTime.EndDate = item.EndDate;
                $scope.blockedTime.EndTime = item.EndTime;
                $scope.blockedTime.EndAmPm = item.EndAmPm;
                $scope.blockedTime.Comment = item.Comment;
                $scope.blockedTime.AllDay = item.AllDay;
            }



            $scope.deleteBlockedTime = function (id) {

                bootbox.confirm("Selected blocked time will be deleted. Do you want to continue?", function (result) {
                    if (result === true) {

                        $scope.appointmentService.deleteAppointment(0, id).then(function () {
                            alertService.success('Blocked time has been successfully removed.');

                            // Remove from model
                            var idx = 0;
                            var foundblockedtime = $filter('filter')($scope.blockedTimesList, function (item, index) {
                                if (item.BlockedTimeID == id) {
                                    idx = index;
                                    return item;
                                }
                            });
                            if (foundblockedtime != null) {
                                $scope.blockedTimesList.splice(idx, 1);
                            }

                        },

                        function (errorStatus) {
                            alertService.error('There was an error in deleting the blockedtime');
                        });
                    }
                });
            }

            $scope.save = function () {
                var q = $q.defer();

                if ($scope.isUpdate) {
                    $scope.appointmentService.updateAppointment($scope.getApptModel()).then(function (response) {
                        if (response.ResultCode === 0) {
                            alertService.success('Blocked time updated successfully!');
                        } else {
                            alertService.error('Error while saving a blocked time! Please reload the page and try again.');
                        }
                    });
                } else {
                    $scope.appointmentService.addAppointment($scope.getApptModel()).then(function (response) {
                        if (response.ResultCode === 0) {

                            // Now add an appt resource if needed
                            $scope.blockedTime.BlockedTimeID = response.ID;
                            $scope.appointmentService.getAppointmentResource($scope.blockedTime.BlockedTimeID).then(function (response) {
                                if (response.ResultCode === 0) {
                                    if (response.DataItems.length == 0)
                                        $scope.addAppointmentResource();
                                    else {
                                        alertService.success('Blocked time saved successfully!');
                                        $scope.blockedTimesList.push($scope.blockedTime);
                                        $scope.resetFields();
                                    }
                                } else {
                                    alertService.error('Error while retrieving an appointment resource! Please reload the page and try again.');
                                }
                            });
                        } else {
                            alertService.error('Error while saving a blocked time! Please reload the page and try again.');
                        }
                    });
                }
                return q.promise;

            }

            $scope.addAppointmentResource = function () {
                $scope.appointmentService.addAppointmentResource($scope.getApptResourceModel()).then(function (response) {
                    if (response.ResultCode === 0) {
                        alertService.success('Blocked time saved successfully!');
                        $scope.blockedTimesList.push($scope.blockedTime);
                        $scope.resetFields();
                    } else {
                        alertService.error('Error while saving an appointment resource! Please reload the page and try again.');
                    }
                });
            }

            $scope.getBlockedTimes = function () {
                return $scope.appointmentService.getAppointmentByResource($scope.LocationID, FACILITY_RESOURCE_TYPE).then(function (data) {
                    if (data.DataItems.length > 0) {
                        $scope.appointments = data.DataItems;
                        $scope.blockedTimesList = [];
                        $scope.blockedTimesList.push($scope.formatFacilityBlockedTimes(data.DataItems));
                    }

                    //TMP DEBUG for now
                    //$scope.blockedTimesList = $scope.formatFacilityBlockedTimes(null);
                },
                function (errorStatus) {
                    alertService.error('Unable to connect to server');
                });
            };

            $scope.next = function (one, two) {

            }

            $scope.init();

        }]);
