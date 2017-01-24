angular.module('xenatixApp')
    .controller('groupSchedulingController', ['$scope', 'formService', 'alertService', '$stateParams', 'lookupService', '$rootScope', '$state', '$filter',
        'clientSearchService', 'resourceService', '$timeout', 'groupSchedulingService', 'appointmentService', 'navigationService', '$q',
        function ($scope, formService, alertService, $stateParams, lookupService, $rootScope, $state, $filter,
            clientSearchService, resourceService, $timeout, groupSchedulingService, appointmentService, navigationService, $q) {

            var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
            var weekDays = ["SUNDAY", "MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY", "SATURDAY"];
            var contactTypeSearch = '1,4';
            var locationCounter = 0;
            var defaultAppointmentLength = 30;
            var defaultAppointmentLengthPeriod = 'min';
            var nonSpecialistIndex = 0;
            var selectedTimeSlotIndex = -1;
            var counter = 0;
            var selectedDate = null;
            var currentDate = new Date();
            currentDate.setHours(0, 0, 0, 0);
            var changeLength = false;

            var highlightCalander = function (index) {
                if(!$scope.validateAppointmentLength())
                    return;
                $scope.validateStartAppointmentTime();
                $scope.validateAppointmentTime();
                var elements = $(".scheduling_assist td");
                var selector = getCalanderArea(index);

                if (selectedTimeSlotIndex !== index) {
                    elements.removeClass("col_select");
                }
                if ($scope.appointment.AppointmentEndTime !== undefined) {
                elements.filter(selector).addClass("col_select");
                }
                elements.not(selector).removeClass("col_select");
                selectedTimeSlotIndex = index;
            };

            var getCalanderArea = function (index) {
                var appointmentLength = $scope.appointment.AppointmentLengthPeriod === 'min' ? $scope.appointment.AppointmentLength : $filter('tomin')($scope.appointment.AppointmentLength);
                appointmentLength = parseInt(appointmentLength);
                if (isNaN(appointmentLength)) {
                    appointmentLength = 30;
                }

                var noOfSlot = parseInt(appointmentLength / 30);
                if ((parseInt(appointmentLength) % 30) > 0)
                    noOfSlot = noOfSlot + 1;
                var selector = ":nth-child(n+" + index + "):nth-child(-n+" + (index + (noOfSlot - 1)) + ")";
                return selector;
            };

            function initLookups() {
                $scope.groupServices = lookupService.getLookupsByType('GroupService');
                $scope.filteredGroupServices = $scope.groupServices;
            }

            resetForm = function () {
                $rootScope.formReset($scope.ctrl.groupSchedulingForm);
            };

            function getTotalMonthDay(month, year) {
                var totalMonthDay = new Date(year, month, 0).getDate();
                return totalMonthDay;
            }

            $scope.prepareContactData = function() {
                $scope.groupContacts = [];
            };

            $scope.init = function () {
                $scope.groupID = $stateParams.GroupID || 0;
                $scope.isReadOnly = $stateParams.ReadOnly === 'ReadOnly'; 
                $scope.groupScheduling = { Comments: '' };
                initLookups();
                $scope.enterKeyStop = true;
                $scope.isFormDisabled = ($stateParams.ReadOnly && $stateParams.ReadOnly.toString().toLowerCase() == "view") ? true : false;
                $scope.IsRecurringAptEdit = ($stateParams.IsRecurringAptEdit != null && $stateParams.IsRecurringAptEdit === 'false') ? false : true;
                $scope.isCancel = false;
                $scope.stopNext = false;
                $scope.saveOnEnter = true;
                $scope.contactsTable = $("#contactsTable");
                $scope.initializeBootstrapTable();
                //$scope.setupCustomValidationWatchers();
                $scope.prepareContactData();
                $scope.prepareDateData();
                $scope.prepareLocationData();
                $scope.prepareAppointmentData();
                $scope.prepareProviderData();
                navigationService.get().then(function (data) {
                    //$scope.currentProviderName = ($scope.groupID === 0) ? data.DataItems[0].UserFullName : null;
                    $scope.currentProviderID = ($scope.groupID === 0) ? data.DataItems[0].UserID : null;
                    $scope.getGroupData($scope.groupID).then(function (response) {
                        //load all of the members in the group and get the current user's name         
                        $scope.getLocations().then(function () {
                            $scope.getAppointments().then(function () {
                                if ($scope.groupID !== 0) {
                                    $scope.getAppointment($scope.groupID).then(function (apptData) {
                                        var obj = { stateName: $state.current.name, validationState: 'valid' };
                                        $rootScope.groupAppointmentRightNavigationHandler(obj);
                                    });
                                }
                                else {
                                    //load data needed when creating a new group
                                    $scope.newAppointmentInit();
                                    var obj = { stateName: $state.current.name, validationState: 'warning' };
                                    $rootScope.groupAppointmentRightNavigationHandler(obj);
                                    $scope.addResource($scope.currentProviderID);
                                    $scope.isLoading = false;
                                    resetForm();
                                }
                            });
                        });
                    });
                });

                getExternalSources();
                $timeout(function () {
                    $("#scheduling_assistant").owlCarousel({
                        navigation: true,
                        slideSpeed: 300,
                        paginationSpeed: 400,
                        singleItem: true,
                        afterMove: function (elem) {
                            var visibleItems = angular.element('.owl-page:visible').length;
                            var activeDisplay = angular.element('.owl-page.active span').css('display');
                            if (activeDisplay === 'none') {
                                angular.element('.owl-page.active').prev().trigger('owl.jumpTo', visibleItems - 1);
                            }
                        }
                    });

                    if (!$scope.isCancel) {
                        $(document).undelegate('.scheduling_assist td', 'mouseover');
                        $(document).delegate(".scheduling_assist td", "mouseover", function () {
                            var targetIndex = $(this).index() + 1;
                            var elements = $(".scheduling_assist td");
                            elements.filter(getCalanderArea(targetIndex)).addClass("col_hover");
                        });

                        $(document).undelegate('.scheduling_assist td', 'mouseleave');
                        $(document).delegate('.scheduling_assist td', 'mouseleave', function () {
                            $(".scheduling_assist td").removeClass("col_hover");
                        });

                        $(document).undelegate('.scheduling_assist td', 'click');

                        $(document).delegate('.scheduling_assist td', 'click', function () {
                            var targetIndex = $(this).index() + 1;
                            highlightCalander(targetIndex);
                        });
                    } else {
                        $(document).undelegate('.scheduling_assist td', 'click');
                        $(document).undelegate('.scheduling_assist td', 'mouseleave');
                        $(document).undelegate('.scheduling_assist td', 'mouseover');
                        $(".scheduling_assist td").removeClass("col_hover");
                    }

                });
            };
            $scope.ValidateAttendees = function () {
                if ($scope.groupScheduling.GroupCapacity < 2) {
                    alertService.error('Attendees must be more than 1');
                    $scope.ctrl.groupSchedulingForm.maxAttendees.$setValidity("maxAttendeesError", false);
                } else {
                    if ($scope.groupScheduling.GroupCapacity < $scope.groupContacts.length) {
                        alertService.error('Attendees count cannot be less than the number of attendees already added');
                        $scope.groupScheduling.GroupCapacity = undefined;
                        $scope.ctrl.groupSchedulingForm.maxAttendees.$setValidity("maxAttendeesError", false);
                    }
                    else {
                    $scope.ctrl.groupSchedulingForm.maxAttendees.$setValidity("maxAttendeesError", true);
                }
                }
            };
            var adjustCalanderSelection = function () {
                if ($scope.appointment.AppointmentDate != undefined) {
                    if ($scope.appointment.StartAMPM == 'AM' && $scope.appointment.AppointmentStartTime > 1200) {
                        $scope.appointment.AppointmentStartTime = pad(($scope.appointment.AppointmentStartTime - 1200), 4);
                    }
                    if ($scope.appointment.AMPM == 'AM' && $scope.appointment.AppointmentEndTime > 1200) {
                        $scope.appointment.AppointmentEndTime = pad(($scope.appointment.AppointmentEndTime - 1200), 4);
                    }
                    var startTime = $filter("toMilitaryTime")($scope.appointment.AppointmentStartTime, $scope.appointment.StartAMPM);
                    var time = $filter("toMilitaryTime")($scope.appointment.AppointmentEndTime, $scope.appointment.AMPM);

                    if (startTime) {
                        var selectedColumnIndex = -1;
                        var contactTimeSlots = $filter('filter')($scope.timeSlots, function (item) {
                            return item.contactId == $scope.ContactID;
                        });

                        var selectedTimeSlot = $filter('filter')(contactTimeSlots, function (slot) {
                            return (slot.date == $filter("formatDate")($scope.appointment.AppointmentDate) && (calculateTimeDifference(slot.endTime, startTime) > 0 &&
                                calculateTimeDifference(slot.endTime, startTime) <= 30));
                        })[0];

                        if (selectedTimeSlot) {
                            for (var i = 0; i < contactTimeSlots.length; i++) {
                                if (contactTimeSlots[i].sNo == selectedTimeSlot.sNo) {
                                    selectedColumnIndex = i;
                                    timeSlot = contactTimeSlots[i];
                                    break;
                                }
                            }

                            if (selectedColumnIndex >= 0) {
                                var targetIndex;
                                targetIndex = selectedColumnIndex + 2;
                                highlightCalander(targetIndex);
                                $scope.getAppointmentStatus(timeSlot, false);
                            }
                            else {
                                $('.scheduling_assist td').removeClass('col_select');
                            }
                        }
                    }
                }
            };

            $scope.$watch("[appointment.AppointmentDate, appointment.AppointmentLength,appointment.AppointmentStartTime,appointment.AppointmentEndTime,appointment.StartAMPM, appointment.AMPM]", 
                function (newValues, oldValues) {
                if (!$scope.isCancel && (selectedDate >= currentDate)) {
                    var isValueChanged = false;
                    if (oldValues[0] !== newValues[0]) {
                        if ($scope.appointment.AppointmentDate != undefined && newValues[0] != undefined) {
                            // If date belongs to calander then donot re-render
                            var appointmentDate = new Date($scope.appointment.AppointmentDate);
                            if (appointmentDate != 'Invalid Date') {
                                var newAppointmentDate = new Date(appointmentDate.getFullYear(), appointmentDate.getMonth(), appointmentDate.getDate());

                                var startDate = new Date($scope.selectedYear, $scope.selectedMonth - 1, $scope.weekStartDate);
                                var endDate = new Date($scope.selectedYear, $scope.selectedMonth - 1, $scope.weekEndDate);

                                if (!(newAppointmentDate >= startDate && newAppointmentDate <= endDate)) {
                                    $scope.changeCalander(null, 'date');
                                }
                            }

                            isValueChanged = true;
                        }
                    }
                    else if (oldValues[1] != newValues[1]) {
                        if (changeLength) {
                            $scope.appointment.AppointmentEndTime = $scope.adjustAppointmentEndTime();
                            isValueChanged = true;
                            changeLength = false;
                        }
                    }
                    else if (oldValues[2] != newValues[2]) {
                        //watch for appointment start time
                        var isValid = $scope.validateStartTimeWatch();
                        if (isValid) {
                        $scope.appointment.AppointmentEndTime = $scope.adjustAppointmentEndTime();
                        isValueChanged = true;
                        } else {
                            isValueChanged = false;
                            $scope.appointment.AppointmentStartTime = undefined;
                            $('.scheduling_assist td').removeClass('col_select');
                            alertService.warning('Invalid start time!');
                    }
                    }
                    else if (oldValues[3] != newValues[3]) {
                        //watch for appointment end time
                        var isValid = $scope.validateEndTimeWatch();
                        if (isValid) {
                            if (changeLength) {
                        isValueChanged = true;
                                $scope.syncDateAndLength('endtime');
                    }
                        } else {
                            isValueChanged = false;
                            $scope.appointment.AppointmentEndTime = undefined;
                            $('.scheduling_assist td').removeClass('col_select');
                            alertService.warning('Invalid end time!');
                        }
                    }
                    else if (oldValues[4] != newValues[4]) {
                        isValueChanged = true;
                        //validation of the start ampm button...start time AMPM can only be changed via the radio buttons themselves
                        var formatStart = $scope.appointment.StartAMPM;
                        var time;
                        var isValid = true;
                        if ($scope.appointment.AppointmentStartTime !== undefined && $scope.appointment.AppointmentStartTime !== null) {
                            time = $filter('toMilitaryTime')($scope.appointment.AppointmentStartTime, formatStart);
                            isValid = time >= 600 && time < 1800;
                    }

                        if (!isValid) {
                            isValueChanged = false;
                            $scope.appointment.StartAMPM = oldValues[4];
                            alertService.warning('Invalid AM/PM selection!');
                        }
                    }

                    if (isValueChanged == true)
                        adjustCalanderSelection();
                    $scope.setAmPmForAppointmentStartTime();
                    $scope.setAmPmForAppointmentEndTime();
                }
            });

            $scope.adjustAppointmentPeriod = function() {
                var appointmentLength = $scope.appointment.AppointmentLengthPeriod === 'hour' ? $filter('tomin')($scope.appointment.AppointmentLength) : $scope.appointment.AppointmentLength;
                if (isNaN(appointmentLength)) {
                    appointmentLength = 30;
                }

                if (appointmentLength % 60 === 0) {
                    // show as hours
                    $scope.appointment.AppointmentLengthPeriod = 'hour';
                    $scope.appointment.AppointmentLength = $filter('tohour')($scope.appointment.AppointmentLength);
                } else {
                    $scope.appointment.AppointmentLengthPeriod = 'min';
                    $scope.appointment.AppointmentLength = $scope.appointment.AppointmentLength;
                }
            };

            $scope.initCalender = function() {
                if ($scope.weekStartDate + 6 <= $scope.totalMonthDays)
                    $scope.weekEndDate = $scope.weekStartDate + 6;
                else
                    $scope.weekEndDate = $scope.weekStartDate + ($scope.totalMonthDays - $scope.weekStartDate);

                generateCalanderForContact();
               
            };

            $scope.handleRoomExternalResources = function (appointmentID) {
                var q = $q.defer();
                $scope.isLoading = true;
                var roomResource = $filter('filter')($scope.appointmentResources, { ResourceTypeID: 1 }, true)[0];
                if (roomResource != null && roomResource != undefined) {
                    $scope.getRooms().then(function () {
                        var location = $filter('filter')($scope.locations, function (item) {
                            return item.LocationID === $scope.appointment.FacilityID && item.type === 1;
                        }, true)[0];
                        if (location)
                            $scope.selectedLocation = location;

                        $scope.selectedRoomId = roomResource.ResourceID;
                        var filteredRooms = $filter('filter')($scope.rooms, { RoomID: $scope.selectedRoomId }, true)[0];
                        $scope.selectedRoomName = filteredRooms.RoomName;
                    });
                } else {
                    var externalResource = $filter('filter')($scope.appointmentResources, { ResourceTypeID: 4 }, true)[0];
                    if (externalResource != null && externalResource != undefined) {
                        var location = $filter('filter')($scope.locations, function (item) {
                            return item.LocationID === externalResource.ResourceID && item.type === 4;
                        }, true)[0];
                        if (location)
                            $scope.selectedLocation = location;
                    }
                }

                $scope.initCalender();
                $scope.isLoading = false;
                q.resolve();

                return q.promise;
            };

            var highlightFirstAvailableTimeSlot = function () {
                //ToDo: need to update to handle multiple contact ids
                var contactTimeSlots = $filter('filter')($scope.timeSlots, function (item) {
                    return item.contactId === $scope.ContactID;
                });

                var currentTime = getCurrentTime();
                var todayDate = getCurrentDate();
                var flag = false;
                var selectedColumnIndex = -1;
                if ($scope.appointment.AppointmentID != undefined) {
                    highlightAppointment();
                }
                else {
                    for (var i = 0; i < contactTimeSlots.length; i++) {
                        var difference = calculateTimeDifference(contactTimeSlots[i].startTime, currentTime);
                        var currentTimeSlotDate = $filter('toMMDDYYYYDate')(contactTimeSlots[i].date, 'MM/DD/YYYY');

                        if (contactTimeSlots[i].status === 'available' && currentTimeSlotDate >= todayDate) {
                            if (currentTimeSlotDate === todayDate && difference < 0) {
                                continue;
                            }

                            if ($scope.credentials.length > 0 || $scope.nonSpecialistProviders.length > 0 || $scope.selectedRoomId > 0) {
                                flag = true;
                            }
                            for (var j = 0; j < $scope.credentials.length; j++) {
                                var credentialId = $scope.credentials[j].CredentialID;

                                if ($filter('filter')($scope.timeSlots, function (item) {
                                    return item.groupId === credentialId
                                            && contactTimeSlots[i].date === item.date
                                            && item.startTime === contactTimeSlots[i].startTime
                                            && item.status === 'available';
                                }).length > 0) {
                                    flag = true;
                                }
                                else {
                                    flag = false;
                                    break;
                                }
                            }

                            if (flag === true) {
                                for (var index = 0; index < $scope.nonSpecialistProviders.length; index++) {
                                    var nonSpecialistID = $scope.nonSpecialistProviders[index].nonSpecialistID;

                                    if ($filter('filter')($scope.timeSlots, function (item) {
                                        return item.groupId === nonSpecialistID && item.startTime === contactTimeSlots[i].startTime && item.status === 'available';
                                    }).length > 0) {
                                        flag = true;
                                    }
                                    else {
                                        flag = false;
                                        break;
                                    }
                                }
                            }

                            if (flag === true && $scope.selectedRoomId > 0) {
                                if ($filter('filter')($scope.timeSlots, function (item) {
                                return item.type === 3 && item.groupId === $scope.selectedRoomId && item.startTime === contactTimeSlots[i].startTime && item.status === 'available';
                                }).length > 0) {
                                    flag = true;
                                }
                                else {
                                    flag = false;
                                }
                            }
                        }

                        if (flag === true) {
                            selectedColumnIndex = i;
                            $scope.getAppointmentStatus($scope.timeSlots[i], true);

                            var targetIndex;
                            targetIndex = selectedColumnIndex + 2;
                            highlightCalander(targetIndex);
                            break;
                        }
                        else {
                            $('.scheduling_assist td').removeClass('col_select');
                        }
                    }
                }
            };

            $scope.getAppointmentLength = function () {
                return appointmentService.getAppointmentLength($scope.appointment.AppointmentTypeID).then(function (data) {
                    if (data.DataItems.length > 0) {
                        $scope.appointment.AppointmentLength = data.DataItems[0].AppointmentLength;
                        $scope.adjustAppointmentPeriod();
                    }
                },
                function (errorStatus) {
                    $scope.isLoading = false;
                    alertService.error('Unable to connect to server');
                });
            };

            var generateCalenderForNonSpecialistProvider = function (nonSpecialist) {
                if (nonSpecialist != null) {
                    $scope.generateTimeSlots(nonSpecialist.selectedProviderId, nonSpecialist.nonSpecialistID, 2);
                }
                else {
                    angular.forEach($scope.nonSpecialistProviders, function (nonSpecialistProvider) {
                        $scope.generateTimeSlots(nonSpecialistProvider.selectedProviderId, nonSpecialistProvider.nonSpecialistID, 2);
                    });
                }
            };

            var addNewResource = function(selectedProviderId, isnew) {
                nonSpecialistIndex++;

                var provider = {
                    nonSpecialistID: nonSpecialistIndex,
                    selectedProviderId: selectedProviderId
                };
                provider.IsNew = isnew;
                $scope.nonSpecialistProviders.push(provider);

                generateCalenderForNonSpecialistProvider(provider);
                $scope.getResourceAvailability(2, nonSpecialistIndex, 2).then(function() {
                    highlightFirstAvailableTimeSlot();
                });
            };

            $scope.addResource = function (selectedProviderId, isnew) {
                if ($scope.providers == undefined || $scope.providers.length === 0) {
                    Array.prototype.push.apply($scope.providers, $scope.externalSources);

                    if (selectedProviderId)
                        addNewResource(selectedProviderId, isnew);
                    else
                        addNewResource($scope.providers[0].ResourceID, isnew);
                }
                else {
                    if (selectedProviderId)
                        addNewResource(selectedProviderId, isnew);
                    else
                        addNewResource($scope.providers[0].ResourceID, isnew);
                }
            };

            //Render selected appointment data
            function bindSelectedResources() {
                if ($scope.appointment.AppointmentID != null && $scope.appointment.AppointmentID != undefined) {
                    var facility = $filter('filter')($scope.appointmentResources, function (item) {
                        return item.ResourceTypeID === 1;
                    })[0];

                    if ($scope.appointment.FacilityID) {
                        var location = $filter('filter')($scope.locations, function (location) {
                            return location.type === 1 && location.LocationID === $scope.appointment.FacilityID;
                        })[0];

                        $scope.selectedLocation = { ID: location.ID, LocationID: location.LocationID, type: 1 };
                        $scope.getRooms().then(function () {
                            $scope.selectedRoomId = facility ? facility.ResourceID : '';
                            var filteredRooms = $filter('filter')($scope.rooms, function (room) { return room.RoomID === $scope.selectedRoomId });
                            if (filteredRooms.length > 0) {
                                $scope.selectedRoomName = filteredRooms[0].RoomName;
                                generateCalenderForRoom();

                                $scope.getResourceAvailability(1, $scope.selectedRoomId, 3).then(function () {
                                    highlightFirstAvailableTimeSlot();
                                });
                            }
                            else {
                                $scope.selectedRoomId = 0;
                            }
                        });
                    } else {
                        var address = $filter('filter')($scope.appointmentResources, function (item) {
                            return item.ResourceTypeID === 4;
                        })[0];
                        if (address != null) {
                            var location = $filter('filter')($scope.locations, function (location) {
                                return location.type === 4 && location.LocationID === address.ResourceID;
                            })[0];
                            $scope.selectedLocation = location ? { ID: location.ID, LocationID: location.LocationID, type: 4 } : '';
                        }
                    }

                    var selectedResources = $filter('filter')($scope.appointmentResources, function (item) {
                        return item.ResourceTypeID === 2;
                    });

                    //ToDo: Review this for group scheduling
                    angular.forEach(selectedResources, function (res) {
                        if (res.ResourceID > 0) {
                            var preProvider = null;
                            //var preProvider = $filter('filter')($scope.credentials, function (credential) {
                            //    return credential.CredentialID === res.ParentID;
                            //})[0];

                            if (preProvider == null || preProvider == undefined) {
                                $scope.addResource(res.ResourceID, false);
                            } else {
                                preProvider.selectedProviderId = res.ResourceID;
                            }
                        }
                    });
                }
            };

            $scope.getCredentialByAppointmentType = function () {
                //ToDo: Review this call to see how it applies to group appointments
                return resourceService.getCredentialByAppointmentType($scope.appointment.AppointmentTypeID).then(function (data) {
                    $scope.credentials = data.DataItems;
                    // reset time slots when appointment type is changed
                    $scope.timeSlots = $filter('filter')($scope.timeSlots, function (timeSlot) {
                        return timeSlot.type !== 1;
                    });

                    var defer = $q.defer();
                    var promises = [];

                    if ($scope.appointment.AppointmentID != undefined) {
                        //var selectedResources = $filter('filter')($scope.appointmentResources, function (item) {
                        //    return item.AppointmentID === $scope.appointment.AppointmentID && item.ResourceTypeID === 2;
                        //});
                        var selectedResources = $filter('filter')($scope.appointmentResources, { ResourceTypeID: 2 });

                        var credentials = [];
                        angular.forEach($scope.credentials, function(credential) {
                            credentials.push(credential);
                        });
                        //angular.forEach($scope.credentials, function (credential) {
                        //    var isExists = $filter('filter')(selectedResources, function (resource) {
                        //        return resource.ParentID === credential.CredentialID;
                        //    }).length > 0;

                        //    if (isExists) {
                        //        credentials.push(credential);
                        //    }
                        //});

                        $scope.credentials = credentials;
                    }
                    
                    //ToDo: Review if this is needed for group scheduling
                    //angular.forEach($scope.credentials, function (credential) {
                    //    if (credential != null && credential.Providers.length > 0) {
                    //        credential.selectedProviderId = credential.Providers[0].ProviderId;
                    //        promises.push($scope.getResourceAvailability(2, credential.CredentialID, 1));
                    //    }
                    //});

                    generateCalenderForCredentials();
                    bindSelectedResources();
                    return $q.all(promises);
                },
                function (errorStatus) {
                    $scope.isLoading = false;
                    alertService.error('Unable to connect to server');
                });
            };

            $scope.processByAppointment = function() {
                // here we need to process all actions related to setting the Appointment Type
                $scope.getCredentialByAppointmentType().then(function () {
                    highlightFirstAvailableTimeSlot();
                });
                $scope.getAppointmentLength();
            };

            $scope.newAppointmentInit = function () {
                var appointmentDate = moment(new Date());
                $scope.selectedYear = appointmentDate.year();
                $scope.selectedMonth = appointmentDate.month() + 1;
                $scope.currentMonthName = monthNames[$scope.selectedMonth - 1];
                $scope.weekStartDate = appointmentDate.date();
                if ($scope.weekStartDate + 6 <= $scope.totalMonthDays)
                    $scope.weekEndDate = $scope.weekStartDate + 6;
                else
                    $scope.weekEndDate = $scope.weekStartDate + ($scope.totalMonthDays - $scope.weekStartDate);
                $scope.totalMonthDays = getTotalMonthDay($scope.selectedMonth, $scope.selectedYear);
                $scope.timeSlots = [];
                $scope.initCalender();
            };

            $scope.getAppointment = function(groupID) {
                return groupSchedulingService.getAppointmentByGroupID(groupID).then(function(response) {
                    $scope.appointment = response.DataItems[0];

                    $scope.appointment.IsRecurringAptEdit = $scope.IsRecurringAptEdit;

                    if (response.DataItems[0].Recurrence != null) {
                        $scope.Recurrence = response.DataItems[0].Recurrence;
                        $scope.Recurrence.RecurrenceID = (response.DataItems.length > 0 && response.DataItems[0].RecurrenceID != null) ? response.DataItems[0].RecurrenceID: 0;
                        $scope.Recurrence.StartDate = $filter('toMMDDYYYYDate') ($scope.Recurrence.StartDate, 'MM/DD/YYYY');
                        $scope.Recurrence.EndDate = $filter('toMMDDYYYYDate')($scope.Recurrence.EndDate, 'MM/DD/YYYY');

                        // Only for non-view mode
                        if (!$scope.isFormDisabled) {
                            // Set the recurrence start date to today if it's in the past
                            var curdate = new Date(new Date().getFullYear(), new Date().getMonth(), new Date().getDate());
                            var recurdate = moment($scope.Recurrence.StartDate)._d;
                            if (recurdate < curdate)
                                $scope.Recurrence.StartDate = $filter('toMMDDYYYYDate')(curdate, 'MM/DD/YYYY');
                    }

                    }

                    //load all of the contact resource names by
                    groupSchedulingService.getAllContactResourceNamesByAppointment($scope.appointment.AppointmentID).then(function(response) {
                        var contacts = response.DataItems;
                        angular.forEach(contacts, function (contact) {
                            $scope.addContact(contact.ContactID, contact.FirstName, contact.LastName, null, null, true);
                        });
                    });

                    $scope.appointment.AppointmentStartTime = pad($scope.appointment.AppointmentStartTime, 4);
                    $scope.adjustAppointmentPeriod();
                    $scope.appointment.AppointmentEndTime = $scope.adjustAppointmentEndTime();
                    $scope.appointment.StartAMPM = $filter('toStandardTimeAMPM')($scope.appointment.AppointmentStartTime);
                    $scope.appointment.AMPM = $filter('toStandardTimeAMPM')($scope.appointment.AppointmentStartTime);
                    if ($scope.appointment.AppointmentLength === 0) {
                        $scope.appointment.AppointmentLength = 30;
                        $scope.appointment.AppointmentLengthPeriod = "min";
                    }

                    $scope.appointment.AppointmentDate = $filter('toMMDDYYYYDate')($scope.appointment.AppointmentDate);
                    var appointmentDate = moment.utc($scope.appointment.AppointmentDate);

                    $scope.selectedYear = appointmentDate.year();
                    $scope.selectedMonth = appointmentDate.month() + 1;
                    $scope.currentMonthName = monthNames[$scope.selectedMonth - 1];
                    $scope.weekStartDate = appointmentDate.date();
                    $scope.totalMonthDays = getTotalMonthDay($scope.selectedMonth, $scope.selectedYear);
                    $scope.timeSlots = [];
                    $scope.initCalender();
                    $scope.handleRoomExternalResources($scope.appointment.AppointmentID).then(function () {
                        //$scope.processByAppointmentType().then(function(resp) {
                        $scope.processByAppointment();
                        $scope.isLoading = false;
                        resetForm();
                    });
                });
            };

            $scope.getAppointments = function() {
                if ($scope.groupID !== 0) {
                    //get all resources and then loop through each one to get appointments by resource
                    return groupSchedulingService.getGroupSchedulingResource($scope.groupID).then(function(response) {
                        if (response.ResultCode === 0) {
                            if (response.DataItems.length > 0) {
                                //every resource will be returned with an inner list of all of their appointments -> $scope.appointmentResources.Appointments
                                $scope.appointmentResources = response.DataItems;
                                angular.forEach($scope.appointmentResources, function (resource) {
                                    angular.forEach(resource.Appointments, function(appt) {
                                        $scope.appointments.push(appt);
                                    });
                                });
                            } else {
                                alertService.error('No resources found');
                            }
                        } else {
                            alertService.error('Error while loading group resources');
                        }
                    });
                } else {
                    return $scope.promiseNoOp();
                }
            };

            var getExternalSources = function () {
                resourceService.getResources(2).then(function (data) {
                    if (data.DataItems != null && data.DataItems.length > 0) {
                        $scope.externalSources = data.DataItems;
                    }
                }, function (errorStatus) { });
            };

            $scope.prepareDateData = function () {
                $scope.opened = false;
                $scope.dobName = 'appointmentDate';
                $scope.dateOptions = {
                    formatYear: "yy",
                    startingDay: 1,
                    showWeeks: false
                };
                var today = new Date();
                $scope.selectedYear = today.getFullYear();
                $scope.selectedMonth = today.getMonth() + 1;
                $scope.currentMonthName = monthNames[$scope.selectedMonth - 1];
                $scope.weekStartDate = today.getDate();
                $scope.weekEndDate = 0;
                $scope.totalMonthDays = getTotalMonthDay($scope.selectedMonth, $scope.selectedYear);
                $scope.days = [];
                $scope.hours = [];
                $scope.timeSlots = [];
                $scope.selectedTimeSlots = [];
            };

            $scope.prepareLocationData = function() {
                $scope.locations = [];
                $scope.rooms = [];
                $scope.selectedLocation = {};
                $scope.selectedRoomID = 0;
            };

            var getCurrentTime = function () {
                var todayDate = new Date();
                var currentHour = todayDate.getHours();
                var currentMinute = todayDate.getMinutes();

                var period = currentHour >= 12 ? 'pm' : 'am';
                return $filter('toMilitaryTime')(pad(currentHour, 2) + ":" + pad(currentMinute, 2), period);
            };

            var getCurrentDate = function () {
                var todayDate = new Date();
                return $filter('toMMDDYYYYDate')(todayDate, 'MM/DD/YYYY', 'useLocal');
            };

            $scope.prepareAppointmentData = function () {
                $scope.conflicts = [];
                $scope.providerAppointments = [];
                $scope.appointmentResources = [];
                $scope.appointment = {};
                $scope.appointment.IsRecurringAptEdit = $scope.IsRecurringAptEdit;
                $scope.appointments = [];
                $scope.appointmentStartDate = new Date();
                $scope.appointment.AppointmentDate = $filter('toMMDDYYYYDate')($scope.appointmentStartDate, 'MM/DD/YYYY', 'useLocal');
                $scope.appointment.AppointmentLength = defaultAppointmentLength;
                $scope.appointment.AppointmentLengthPeriod = defaultAppointmentLengthPeriod;
                $scope.appointment.AMPM = $filter('toStandardTimeAMPM')(getCurrentTime());
                $scope.appointment.StartAMPM = $filter('toStandardTimeAMPM')(getCurrentTime());
            };

            $scope.prepareProviderData = function() {
                $scope.providers = [];
                $scope.nonSpecialistProviders = [];
                $scope.externalSources = [];
                getExternalSources();
                $scope.credentials = [];
                $scope.resources = [];
            };

            $scope.setAmPmForAppointmentEndTime = function () {
                if ($scope.appointment.AppointmentStartTime !== null && $scope.appointment.AppointmentStartTime !== undefined && $scope.appointment.AppointmentStartTime !== 'undefined') {
                    var endTime = 0;
                    var appointmentLength = $scope.appointment.AppointmentLengthPeriod === 'hour' ? $scope.appointment.AppointmentLength * 60 : $scope.appointment.AppointmentLength;
                    if (isNaN(appointmentLength)) {
                        appointmentLength = 30;
                    }

                    var startTime = $filter("toMilitaryTime")($scope.appointment.AppointmentStartTime, $scope.appointment.StartAMPM);
                    var calculatedEndTime = moment(pad(startTime, 4), 'HH:mm').add(appointmentLength, 'minute').format('HH:mm');
                    $scope.endTimeAmPm = $filter('toStandardTimeAMPM')(calculatedEndTime);
                    if ($scope.appointment.AMPM !== $scope.endTimeAmPm) {
                        $scope.appointment.AMPM = $scope.endTimeAmPm;
                        alertService.warning('End time adjusted to ' + $scope.endTimeAmPm);
                }
            }
            }

            $scope.validateAppointmentDate = function () {

                if ($scope.isFormDisabled)
                    return;

                if (!$scope.isCancel) {
                    if ($scope.appointment.AppointmentID > 0) {
                        $("#dateOfAppointmentError").addClass('ng-hide');
                        $("#dateOfAppointment").removeClass("has-error");
                    }
                    if ($scope.appointment.AppointmentDate != undefined) {
                        selectedDate = new Date($scope.appointment.AppointmentDate);

                        if (selectedDate >= currentDate) {
                            $("#dateOfAppointmentError").addClass('ng-hide');
                            $("#dateOfAppointment").removeClass("has-error");
                            $scope.ctrl.groupSchedulingForm.$valid = true;
                        }
                        else if ($scope.ctrl.groupSchedulingForm.$pristine) {
                            $("#dateOfAppointmentError").addClass('ng-hide');
                            $("#dateOfAppointment").removeClass("has-error");
                            $scope.ctrl.groupSchedulingForm.$valid = true;
                        } else {
                            $("#dateOfAppointmentError").removeClass('ng-hide');
                            $("#dateOfAppointment").addClass("has-error");
                            $scope.ctrl.groupSchedulingForm.$valid = false;
                        }
                    }
                }
            };

            $scope.validateTime = function (isStartTime) {
                var max = 1800;
                var min = 800;
                var formatStart = $scope.appointment.StartAMPM;
                var format = $scope.appointment.AMPM;
                var time = 0;
                var isValid = false;

                if (formatStart != undefined && format != undefined && !$scope.isCancel && $scope.appointment.AppointmentStartTime && $scope.appointment.AppointmentStartTime) {
                    if (isStartTime == true) {
                        time = $filter('toMilitaryTime')($scope.appointment.AppointmentStartTime, formatStart);
                        isValid = time >= min && time < max;

                        if ($scope.ctrl.groupSchedulingForm != undefined) {

                            $scope.ctrl.groupSchedulingForm.startTime.$setValidity("startTimeError", isValid);

                            if (!isValid) {
                                $('#startTimeContainer').addClass('has-error');
                                $('#appointmentStartTimeError1').removeClass('ng-hide').addClass('ng-show');
                                $scope.ctrl.groupSchedulingForm.startTime.$invalid = true;
                                $scope.ctrl.groupSchedulingForm.startTime.$valid = false;
                            }
                            else {
                                $('#startTimeContainer').removeClass('has-error');
                                $('#appointmentStartTimeError1').removeClass('ng-show').addClass('ng-hide');
                                $scope.ctrl.groupSchedulingForm.startTime.$invalid = false;
                                $scope.ctrl.groupSchedulingForm.startTime.$valid = true;
                            }
                        }



                    }
                    else {
                        format = $scope.endTimeAmPm;
                        time = $filter('toMilitaryTime')($scope.appointment.AppointmentEndTime, format);
                        isValid = time > min && time <= max;
                        if ($scope.ctrl.groupSchedulingForm != undefined) {
                            $scope.ctrl.groupSchedulingForm.endTime.$setValidity("endTimeError", isValid);

                            if (!isValid) {
                                $('#endTimeContainer').addClass('has-error');
                                $('#appointmentEndTimeError1').removeClass('ng-hide').addClass('ng-show');
                                $scope.ctrl.groupSchedulingForm.endTime.$invalid = true;
                                $scope.ctrl.groupSchedulingForm.endTime.$valid = false;
                    }
                            else {
                                $('#endTimeContainer').removeClass('has-error');
                                $('#appointmentEndTimeError1').removeClass('ng-show').addClass('ng-hide');
                                $scope.ctrl.groupSchedulingForm.endTime.$invalid = true;
                                $scope.ctrl.groupSchedulingForm.endTime.$valid = false;
                }


                        }

                        var isValid1 = true;
                        var stime = $filter('toMilitaryTime')($scope.appointment.AppointmentStartTime, formatStart);
                        var etime = $filter('toMilitaryTime')($scope.appointment.AppointmentEndTime, format);
                        if (etime <= stime) {
                            isValid1 = false;
                        }
                        if ($scope.ctrl.groupSchedulingForm != undefined) {
                            $scope.ctrl.groupSchedulingForm.endTime.$setValidity("endTimeValidError", isValid1);
                        }
                    }
                }
            };
    
            $scope.validateAppointmentLength = function () {
                if ($scope.appointment.AppointmentLengthPeriod != undefined && $scope.appointment.AppointmentLength != undefined) {
                    var appointmentLength = $scope.appointment.AppointmentLength;
                    var timeToCheck;
                    var milTimeStart = $filter('toMilitaryTime')($scope.appointment.AppointmentStartTime, $scope.appointment.StartAMPM);
                    if (isNaN(appointmentLength)) {
                        $("#lengthOfAppointment").addClass('has-error');
                        $("#lengthOfAppointmentError").removeClass('ng-hide');
                        return false;
                    }
                    if ($scope.appointment.AppointmentLengthPeriod == "min") {
                        if (appointmentLength > 0 && appointmentLength <= 600) {
                            timeToCheck = moment(pad(milTimeStart, 4), 'HH:mm').add(appointmentLength, 'minute').format('HH:mm');
                            $("#lengthOfAppointmentError").addClass('ng-hide');
                            $("#lengthOfAppointment").removeClass("has-error");
                        }
                        else {
                            $("#lengthOfAppointment").addClass('has-error');
                            $("#lengthOfAppointmentError").removeClass('ng-hide');
                            return false;
                        }
                    }
                    else if ($scope.appointment.AppointmentLengthPeriod == "hour") {
                        if (appointmentLength > 0 && appointmentLength <= 10) {
                            timeToCheck = moment(pad(milTimeStart, 4), 'HH:mm').add(appointmentLength, 'hour').format('HH:mm');
                            $("#lengthOfAppointment").removeClass('has-error');
                            $("#lengthOfAppointmentError").addClass('ng-hide');
                        }
                        else {
                            $("#lengthOfAppointment").addClass('has-error');
                            $("#lengthOfAppointmentError").removeClass('ng-hide');
                            return false;
                        }
                        }

                    var ampm = $filter('toStandardTimeAMPM')(timeToCheck);
                    var milTime = $filter('toMilitaryTime')(timeToCheck, ampm);
                    if ((milTime > 1800 || milTime < 800 || milTimeStart > milTime)) {
                        $("#lengthOfAppointment").addClass('has-error');
                        $("#lengthOfAppointmentError").removeClass('ng-hide');
                        return false;
                    }
                }
                changeLength = true;
                return true;
            };

            var setTimeSlot = function (timeSlot, flag) {
                if (flag) {
                    $scope.appointment.AppointmentStartTime = pad(timeSlot.startTime, 4);
                    $scope.appointment.AppointmentEndTime = $scope.adjustAppointmentEndTime();
                    $scope.appointment.AppointmentDate = timeSlot.date;
                    $scope.appointment.StartAMPM = $filter('toStandardTimeAMPM')(timeSlot.startTime);
                    $scope.appointment.AMPM = $filter('toStandardTimeAMPM')(timeSlot.endTime);
                }

                $scope.selectedTimeSlots = $filter('filter')($scope.timeSlots, function (item) {
                    return item.startTime === timeSlot.startTime && item.endTime === timeSlot.endTime && item.date === timeSlot.date;
                });
            };

            var calculateTimeDifference = function (startTime, endTime) {
                return moment.duration(moment(pad(startTime, 4), "HH:mm").diff(moment(pad(endTime, 4), "HH:mm"))).asMinutes();
            };

            var getResourceType = function (timeSlotType) {
                switch (timeSlotType) {
                    case 1:
                        return 2;
                    case 2:
                        return 2;
                    case 3:
                        return 1;
                    default:
                        return 0;
                }
            };

            $scope.getAppointmentStatus = function (timeSlot, flag) {
                setTimeSlot(timeSlot, flag);

                // conflicts & suggested appointment date
                $scope.conflicts = [];
                var conflictTimeSlot = $filter('filter')($scope.selectedTimeSlots, function (conflict) {
                    return conflict.appointmentStartTime != undefined
                        && conflict.appointmentEndTime != undefined
                        && conflict.resourceId != undefined
                        && conflict.resourceId !== 0
                        && conflict.appointmentId !== $scope.appointment.AppointmentID
                        && ((calculateTimeDifference(conflict.endTime, conflict.appointmentStartTime) > 0 && calculateTimeDifference(conflict.endTime, conflict.appointmentStartTime) <= 30) || (calculateTimeDifference(conflict.appointmentEndTime, conflict.startTime) > 0 && calculateTimeDifference(conflict.appointmentEndTime, conflict.startTime) <= 30));
                });

                angular.forEach(conflictTimeSlot, function (conflict) {
                    var resourceName = '';
                    var resourceTypeId = getResourceType(conflict.type);
                    resourceService.getResourceById(conflict.resourceId, resourceTypeId).then(function (response) {
                        if (response.DataItems.length > 0) {
                            resourceName = response.DataItems[0].ResourceName;
                            $scope.conflicts.push({ resourceId: conflict.resourceId, resourceTypeId: conflict.type, resourceName: resourceName, suggestedAppointmentTime: '' });
                        }
                    });
                });
            };

            var highlightAppointment = function () {
                var contactTimeSlots = $filter('filter')($scope.timeSlots, function (item) {
                    return item.contactId === $scope.ContactID;
                });

                var selectedColumnIndex = -1;
                var timeSlot = null;
                for (var i = 0; i < contactTimeSlots.length; i++) {
                    if (contactTimeSlots[i].date === $filter('toMMDDYYYYDate')((new Date($scope.appointment.AppointmentDate)), 'MM/DD/YYYY', 'useLocal') && $filter("toMilitaryTime")(contactTimeSlots[i].startTime, $scope.appointment.AMPM) === $scope.appointment.AppointmentStartTime) {
                        selectedColumnIndex = i;
                        timeSlot = contactTimeSlots[i];
                        break;
                    }
                }

                var targetIndex = selectedColumnIndex + 2;
                highlightCalander(targetIndex);
                if (timeSlot !== null)
                    $scope.getAppointmentStatus(timeSlot, true);
            };

            $scope.getAppointmentByResource = function (resourceId, resourceTypeId) {
                $scope.isLoading = true;
                if ($scope.appointmentResources.length > 0) {
                    var filteredResource = $filter('filter') ($scope.appointmentResources, { ResourceID: resourceId, ResourceTypeID: resourceTypeId });
                    if (filteredResource.length > 0) {
                        $scope.providerAppointments = filteredResource[0].Appointments;
                        $scope.isLoading = false;
                        return $scope.promiseNoOp();
                    }
                    else {
                        return appointmentService.getAppointmentByResource(resourceId, resourceTypeId).then(function (data) {
                            $scope.providerAppointments = data.DataItems;
                            $scope.isLoading = false;
                        });
                    }
                } else {
                    //load provider appointments for the current user
                    return appointmentService.getAppointmentByResource(resourceId, resourceTypeId).then(function (data) {
                        $scope.providerAppointments = data.DataItems;
                        $scope.isLoading = false;
                    });
                }
               // $scope.isLoading = false;
                //return $scope.promiseNoOp();
            };

            var processTimeSlots = function (resourceId, resourceTypeID, identifier, timeSlotType) {
                //Get all the provider appointments for the selected provider or resource
                return $scope.getAppointmentByResource(resourceId, resourceTypeID).then(function (response) {
                    //filter appointments of current provider selected based on start calender date and  end calender date
                    var filteredAppointments = $filter('filter')($scope.providerAppointments, function (item) {
                        var dateOfAppointment = moment.utc(item.AppointmentDate);
                        var appointmentDate = dateOfAppointment.date();
                        if (dateOfAppointment.year() === $scope.selectedYear && dateOfAppointment.month() + 1 === $scope.selectedMonth) {
                            if (appointmentDate >= $scope.weekStartDate && appointmentDate <= $scope.weekEndDate) {
                                return item;
                            }
                        }
                    });

                    //filter timeslots for selected provider based on identifier
                    var filteredTimeSlots = $filter('filter')($scope.timeSlots, function (item) {
                        return item.groupId === identifier && item.type === timeSlotType;
                    });

                    angular.forEach(filteredTimeSlots, function (timeSlot) {
                        var filteredResourceAppointments = $filter('filter')(filteredAppointments, function (resourceAppointment) {
                            var appointmentStartTime = resourceAppointment.AppointmentStartTime;
                            var appointmentEndTime = $scope.calculateAppointmentEndTime(resourceAppointment.AppointmentStartTime, resourceAppointment.AppointmentLength);

                            return ($filter('toMMDDYYYYDate')(resourceAppointment.AppointmentDate) === timeSlot.date
                                    && calculateTimeDifference(timeSlot.endTime, appointmentStartTime) >= 0
                                    && calculateTimeDifference(appointmentEndTime, timeSlot.startTime) >= 0);
                        });

                        timeSlot.resourceId = resourceId;
                        //code to check if current appointment in filteredResourceAppointments is for current contact

                        if (filteredResourceAppointments.length > 0) {
                            // appontment exists in current time slot
                            var appointment = filteredResourceAppointments[0];
                            var appointmentStartTime = appointment.AppointmentStartTime;
                            var appointmentEndTime = $scope.calculateAppointmentEndTime(appointment.AppointmentStartTime, appointment.AppointmentLength);

                            if ((calculateTimeDifference(timeSlot.endTime, appointmentStartTime) >= 30) && (calculateTimeDifference(appointmentEndTime, timeSlot.startTime) >= 30)) {
                                timeSlot.status = "appointment";
                            }
                            else {
                                timeSlot.status = "available";
                            }

                            timeSlot.appointmentStartTime = appointmentStartTime;
                            timeSlot.appointmentEndTime = appointmentEndTime;
                            timeSlot.appointmentId = appointment.AppointmentID;
                        }
                        else {
                            var timeSlotDayOfWeek = weekDays[timeSlot.dayOfWeek];
                            var resourceAvailabilities, resourcesOverrides;

                            var resource = $filter('filter')($scope.resources, function (resourceAvailability) {
                                return resourceAvailability.ResourceID === resourceId && resourceAvailability.ResourceTypeID === resourceTypeID;
                            });

                            if (resource.length > 0) {
                                resourceAvailabilities = resource[0].ResourceAvailabilities;
                                resourcesOverrides = resource[0].ResourceOverrides;
                            }

                            // resource availability
                            var isResourceAvailable = $filter('filter')(resourceAvailabilities, function (resourceAvailability) {
                                return resourceAvailability.Days.toUpperCase() === timeSlotDayOfWeek
                                    && (timeSlot.startTime >= resourceAvailability.AvailabilityStartTime
                                        && timeSlot.endTime <= resourceAvailability.AvailabilityEndTime);
                            }).length > 0 ? true : false;

                            // resource override
                            var hasResourceOverride = $filter('filter')(resourcesOverrides, function (override) {
                                return override.ResourceID === resourceId
                                    && override.ResourceTypeID === resourceTypeID
                                    && $filter('toMMDDYYYYDate')(override.OverrideDate) === timeSlot.date;
                            }).length > 0 ? true : false;

                            if (isResourceAvailable && !hasResourceOverride) {
                                timeSlot.status = "available";
                            }
                            else {
                                timeSlot.status = "blocked";
                            }
                        }
                    });
                });
            };

            $scope.generateTimeSlots = function (resourceId, groupId, timeSlotType) {
                for (var startDayIndex = $scope.weekStartDate; startDayIndex <= $scope.weekEndDate; startDayIndex++) {
                    var date = pad($scope.selectedMonth, 2) + "/" + pad(startDayIndex, 2) + "/" + $scope.selectedYear;
                    var dayOfWeek = new Date(date).getDay();

                    for (var timeSlotIndex = 800; timeSlotIndex < 1800; timeSlotIndex += 100) {
                        $scope.timeSlots.push({
                            sNo: counter++,
                            startTime: timeSlotIndex,
                            endTime: timeSlotIndex + 30,
                            date: date,
                            dayOfWeek: dayOfWeek,
                            resourceId: resourceId,
                            contactId: 0,
                            groupId: groupId,
                            status: 'blocked',
                            type: timeSlotType
                        });

                        $scope.timeSlots.push({
                            sNo: counter++,
                            startTime: timeSlotIndex + 30,
                            endTime: timeSlotIndex + 100,
                            date: date,
                            dayOfWeek: dayOfWeek,
                            resourceId: resourceId,
                            contactId: 0,
                            groupId: groupId,
                            status: 'blocked',
                            type: timeSlotType
                        });
                    }
                }
            };

            $scope.getResourceAvailability = function (resourceTypeID, identifier, timeSlotType) {
                var resourceId = 0;
                if (timeSlotType === 1) {
                    var selectedCredential = $filter('filter')($scope.credentials, function (item) { return item.CredentialID === identifier && item.selectedProviderId != null; });

                    if (selectedCredential.length === 0)
                        return $scope.promiseNoOp();
                    resourceId = selectedCredential[0].selectedProviderId;
                } else if (timeSlotType === 2) {
                    var selectedCredential = $filter('filter')($scope.nonSpecialistProviders, function (item) { return item.nonSpecialistID === identifier && item.selectedProviderId != null; });
                    if (selectedCredential.length === 0)
                        return $scope.promiseNoOp();
                    resourceId = selectedCredential[0].selectedProviderId;
                } else if (timeSlotType === 3) {
                    resourceId = identifier;
                }

                var filteredResource = $filter('filter')($scope.resources, function (item) {
                    return item.ResourceID === resourceId && item.ResourceTypeID === resourceTypeID;
                });

                if (filteredResource.length > 0) {
                    return processTimeSlots(resourceId, resourceTypeID, identifier, timeSlotType).then(function () {
                        $timeout(function () {
                             $scope.conflicts = [];
                        }, 0);
                    });
                } else {
                    // Get all resource availabilities of current provider selected by user
                    return resourceService.getResourceDetails(resourceId, resourceTypeID).then(function (data) {
                        Array.prototype.push.apply($scope.resources, data.DataItems);
                        return processTimeSlots(resourceId, resourceTypeID, identifier, timeSlotType).then(function () {
                            $timeout(function () {
                               $scope.conflicts = [];
                            }, 0);
                        });
                    },
                        function (errorStatus) {
                            $scope.isLoading = false;
                            alertService.error('Unable to connect to server');
                        });
                }
            };


            $scope.getRooms = function () {
                return resourceService.getRooms($scope.appointment.FacilityID).then(function (data) {
                    $scope.rooms = data.DataItems;
                },
                function (errorStatus) {
                    $scope.isLoading = false;
                    alertService.error('Unable to connect to server');
                });
            };

            $scope.populateRooms = function() {
                var location = $filter('filter')($scope.locations, { ID: $scope.selectedLocation.ID }, true)[0];
                $scope.selectedLocation.type = location != null ? location.type : 0;
                $scope.selectedLocation.LocationID = location != null ? location.LocationID : 0;

                if (location != null && location.type === 1) {
                    $scope.appointment.FacilityID = location.LocationID;
                    return $scope.getRooms().then(function () {
                        if ($scope.rooms.length === 0) {
                            alertService.error('No rooms are available for this location');
                        }
                        highlightFirstAvailableTimeSlot();
                    });
                }
                else {
                    $scope.timeSlots = $filter('filter')($scope.timeSlots, function (item) {
                        return item.type !== 3;
                    });
                    $scope.selectedRoomId = 0;
                    $scope.rooms = [];
                    $scope.appointment.FacilityID = null;
                    highlightFirstAvailableTimeSlot();
                    return $scope.promiseNoOp();
                }            
            };

            $scope.filterServices = function() {
                $scope.filteredGroupServices = $filter('filter')($scope.groupServices, { GroupTypeID: $scope.groupScheduling.GroupTypeID });
                $scope.groupScheduling.ServiceID = 0;
                $scope.groupScheduling.ServiceStatusID = 0;
            };

            var generateCalenderForRoom = function () {
                if ($scope.selectedRoomId > 0) {
                    $scope.generateTimeSlots(0, $scope.selectedRoomId, 3, false);
                }
            };
            
            $scope.addRoom = function() {
                $scope.timeSlots = $filter('filter')($scope.timeSlots, function (timeSlot) {
                    return timeSlot.type !== 3;
                });

                var filteredRooms = [];
                $scope.timeSlots = $filter('filter')($scope.timeSlots, function (timeSlot) {
                    return timeSlot.type !== 3 && timeSlot.type !== 4;
                });

                if ($scope.selectedRoomId != null && $scope.selectedRoomId > 0)
                    filteredRooms = $filter('filter')($scope.rooms, function (room) { return room.RoomID === $scope.selectedRoomId });
                if (filteredRooms.length > 0) {
                    $scope.selectedRoomName = filteredRooms[0].RoomName;
                    generateCalenderForRoom();

                    $scope.getResourceAvailability(1, $scope.selectedRoomId, 3).then(function () {
                        highlightFirstAvailableTimeSlot();
                    });
                }
                else {
                    $scope.selectedRoomId = 0;
                }
            };

            $scope.syncDateAndLength = function(type) {
                if (type == 'starttime') {
                    $scope.appointment.AppointmentEndTime = $scope.adjustAppointmentEndTime();
                }

                var starttimeMil = $filter('toMilitaryTime')($scope.appointment.AppointmentStartTime, $scope.appointment.StartAMPM);
                var endtimeMil = $filter('toMilitaryTime')($scope.appointment.AppointmentEndTime, $scope.appointment.AMPM);

                var starttime = moment(starttimeMil, 'HH:mm');

                var endtime = moment(endtimeMil, 'HH:mm');

                var diff = moment.duration(endtime - starttime).asMinutes();

                if (diff > 0) {
                    if ($scope.appointment.AppointmentLengthPeriod == 'hour')
                        $scope.appointment.AppointmentLength = $filter('tohour')(diff);
                    else
                        $scope.appointment.AppointmentLength = diff;
                }
            };

            $scope.calculateAppointmentEndTime = function (startTime, length, startampm) {
                if (startTime !== undefined && startTime !== null) {
                    if (startampm !== undefined) 
                        startTime = $filter('toMilitaryTime')(startTime, startampm);
                    var standardTime = moment(pad(startTime, 4), 'HH:mm').add(length, 'minute').format('HH:mm');
                    var ampm = $filter('toStandardTimeAMPM')(standardTime);
                    if (startampm !== undefined)
                        $scope.appointment.AMPM = ampm;
                    return $filter('toMilitaryTime')(standardTime, ampm);
                }
            }
            
            $scope.adjustAppointmentEndTime = function () {
                var appointmentLength = $scope.appointment.AppointmentLengthPeriod == 'hour' ? $scope.appointment.AppointmentLength * 60 : $scope.appointment.AppointmentLength;

                if ($scope.appointment.AppointmentLengthPeriod == 'hour') {
                    appointmentLength = $filter('tomin')($scope.appointment.AppointmentLength);
                } else {
                    appointmentLength = $scope.appointment.AppointmentLength;
                }

                if (isNaN(appointmentLength)) {
                    appointmentLength = 30;
                }
                $scope.validateAppointmentTime();
                return $scope.calculateAppointmentEndTime($scope.appointment.AppointmentStartTime, appointmentLength, $scope.appointment.StartAMPM);
            }

            $scope.changeMinHour = function (value, preventAction) {
                if (preventAction) {
                    if (value === 'min') {
                        $scope.appointment.AppointmentLength = $filter('tomin')($scope.appointment.AppointmentLength);;
                    }
                    else {
                        $scope.appointment.AppointmentLength = $filter('tohour')($scope.appointment.AppointmentLength);
                    }
                    $scope.appointment.AppointmentLengthPeriod = value;
                    $scope.appointment.AppointmentEndTime = $scope.adjustAppointmentEndTime();
                }
            };      

            $scope.validateStartAppointmentTime = function () {
                $timeout(function () {
                    $scope.setAmPmForAppointmentStartTime();
                    $scope.validateTime(true);
                    changeLength = true;
                });
            }

            $scope.setAmPmForAppointmentStartTime = function () {
                if ($scope.appointment.AppointmentStartTime) {
                    var endTime = 0;

                    var appointmentLength = $scope.appointment.AppointmentLengthPeriod == 'hour' ? $scope.appointment.AppointmentLength * 60 : $scope.appointment.AppointmentLength;

                    if (isNaN(appointmentLength)) {
                        appointmentLength = 30;
                    }
                    var startTime = $filter("toMilitaryTime")($scope.appointment.AppointmentStartTime, $scope.appointment.StartAMPM);

                    var standardTime = moment(pad(startTime, 4), 'HH:mm').add(appointmentLength, 'minute').format('HH:mm');
                    var ampm = $filter('toStandardTimeAMPM')(standardTime);
                    startTime = $filter('toMilitaryTime')(standardTime, ampm);
                    $scope.startTimeAmPm = $filter('toStandardTimeAMPM')(startTime);
                }
            }

            $scope.addProviderResource = function (selectedProviderId) {
                if ($scope.providers == undefined || $scope.providers.length === 0) {
                    Array.prototype.push.apply($scope.providers, $scope.externalSources);
                }
                if (selectedProviderId)
                    addNewResource(selectedProviderId, true);
                else
                    addNewResource($scope.providers[0].ResourceID, true);
                $scope.conflicts = [];
            };
            
            var generateCalanderForNewContact = function (contactID, fullName, isInit) {
                //$scope.days = [];
                //$scope.hours = [];
                //$scope.timeSlots = [];
                counter = 0;
                var contactObject = { ContactID: contactID, FullName: fullName };
                var hasContact = $filter('filter')($scope.groupContacts, { ContactID: contactID }, true);
                if (hasContact.length === 0) {
                    var parsedCapacity = parseInt($scope.groupScheduling.GroupCapacity);
                    if (($scope.groupContacts.length > 0) && (parsedCapacity !== NaN)) {
                        if ($scope.groupContacts.length + 1 > parsedCapacity) {
                            alertService.warning('Contact cannot be added. Group capacity has been reached');
                            return null;
                        }
                    }

                    contactObject.IsNew = !isInit;
                    $scope.groupContacts.push(contactObject);
                    for (var startDayIndex = $scope.weekStartDate; startDayIndex <= $scope.weekEndDate; startDayIndex++) {
                        var date = pad($scope.selectedMonth, 2) + "/" + pad(startDayIndex, 2) + "/" + $scope.selectedYear;
                    var dayOfWeek = new Date(date).getDay();

                    for (var timeSlotIndex = 800; timeSlotIndex < 1800; timeSlotIndex += 100) {
                        var timeSlot = timeSlotIndex / 100;
                        if (timeSlot > 12)
                                timeSlot = timeSlot - 12;

                        $scope.timeSlots.push({
                                sNo: counter++,
                                startTime: timeSlotIndex,
                                endTime: timeSlotIndex + 30,
                            date: date,
                                dayOfWeek: dayOfWeek,
                                    resourceId: 0,
                                    groupId: 0,
                                    contactId: contactID,
                                    status: 'available',
                                    type: 0
                                });

                                    $scope.timeSlots.push({
                                    sNo: counter++,
                                startTime: timeSlotIndex + 30,
                                endTime: timeSlotIndex + 100,
                                    date: date,
                                dayOfWeek: dayOfWeek,
                                    resourceId: 0,
                                    groupId: 0,
                                    contactId: contactID,
                                    status: 'available',
                                    type: 0
                        });
                        }

                    //Updates the new contact resource's availability by loading all of their appointments
                    appointmentService.getAppointmentByResource(contactID, 7).then(function (response) {
                        var contactAppointments = response.DataItems;
                        angular.forEach(contactAppointments, function(appt) {
                            var appointmentStartTime = appt.AppointmentStartTime;
                            var appointmentEndTime = $scope.calculateAppointmentEndTime(appt.AppointmentStartTime, appt.AppointmentLength);

                            var filterTimeSlot = $filter('filter')($scope.timeSlots, function (timeSlot) {
                                return $filter('toMMDDYYYYDate')(appt.AppointmentDate) === timeSlot.date
                                                && calculateTimeDifference(timeSlot.endTime, appointmentStartTime) >= 0
                                                && calculateTimeDifference(appointmentEndTime, timeSlot.startTime) >= 0
                                    && timeSlot.contactId === contactID;
                                                });

                                    if (filterTimeSlot != null && filterTimeSlot.length > 0) {
                                angular.forEach(filterTimeSlot, function (updateTimeSlot) {
                                            if ((calculateTimeDifference(updateTimeSlot.endTime, appointmentStartTime) >= 30) && (calculateTimeDifference(appointmentEndTime, updateTimeSlot.startTime) >= 30)) {
                                                updateTimeSlot.status = "appointment";
                                            } else {
                                                updateTimeSlot.status = "available";
                                                }

                                    updateTimeSlot.resourceId = contactID;
                                            updateTimeSlot.appointmentStartTime = appointmentStartTime;
                                            updateTimeSlot.appointmentEndTime = appointmentEndTime;
                                    updateTimeSlot.appointmentId = appt.AppointmentID;
                                        });
                                     }
                        });
                        highlightFirstAvailableTimeSlot();
                        });
                    }
                    //$scope.$broadcast('getComplete'); 
                    if(!isInit)
                    alertService.success('Contact added successfully');
                } else {
                    alertService.warning('Contact has already been assigned to the group');
                }
            };
            
            var generateCalanderForContact = function (contactID) {
                $scope.days = [];
                $scope.hours = [];
                counter = 0;

                for (var startDayIndex = $scope.weekStartDate; startDayIndex <= $scope.weekEndDate; startDayIndex++) {
                    var date = pad($scope.selectedMonth, 2) + "/" + pad(startDayIndex, 2) + "/" + $scope.selectedYear;
                    var dayOfWeek = new Date(date).getDay();

                    for (var timeSlotIndex = 800; timeSlotIndex < 1800; timeSlotIndex += 100) {
                        var timeSlot = timeSlotIndex / 100;
                        if (timeSlot > 12)
                            timeSlot = timeSlot - 12;

                        $scope.hours.push({
                            sNo: counter++,
                            hour: timeSlotIndex === 800 ? timeSlot + ' AM' : timeSlotIndex === 1200 ? timeSlot + ' PM' : timeSlot
                        });

                        $scope.timeSlots.push({
                            sNo: counter++,
                            startTime: timeSlotIndex,
                            endTime: timeSlotIndex + 30,
                            date: date,
                            dayOfWeek: dayOfWeek,
                            resourceId: 0,
                            groupId: 0,
                            contactId: contactID,
                            status: 'available',
                            type: 0
                        });

                        $scope.timeSlots.push({
                            sNo: counter++,
                            startTime: timeSlotIndex + 30,
                            endTime: timeSlotIndex + 100,
                            date: date,
                            dayOfWeek: dayOfWeek,
                            resourceId: 0,
                            groupId: 0,
                            contactId: contactID,
                            status: 'available',
                            type: 0
                        });
                    }

                    $scope.days.push({
                        calenderDate: date,
                        calenderDay: weekDays[dayOfWeek]
                    });

                    // prepare time slots for contact
                    angular.forEach($scope.appointments, function (appointment) {
                        angular.forEach($scope.appointmentResources, function (appointmentResource) {
                            if (appointment.AppointmentID === appointmentResource.AppointmentID && appointmentResource.IsActive === true) {
                                var appointmentStartTime = appointment.AppointmentStartTime;
                                var appointmentEndTime = $scope.calculateAppointmentEndTime(appointment.AppointmentStartTime, appointment.AppointmentLength);

                                var filterTimeSlot = $filter('filter')($scope.timeSlots, function (timeSlot) {
                                    return $filter('toMMDDYYYYDate')(appointment.AppointmentDate) === timeSlot.date
                                            && calculateTimeDifference(timeSlot.endTime, appointmentStartTime) >= 0
                                            && calculateTimeDifference(appointmentEndTime, timeSlot.startTime) >= 0
                                            && timeSlot.contactId === $scope.ContactID;
                                });
                                if (filterTimeSlot != null && filterTimeSlot.length > 0) {
                                    angular.forEach(filterTimeSlot, function (updateTimeSlot) {
                                        if ((calculateTimeDifference(updateTimeSlot.endTime, appointmentStartTime) >= 30) && (calculateTimeDifference(appointmentEndTime, updateTimeSlot.startTime) >= 30)) {
                                            updateTimeSlot.status = "appointment";
                                        }
                                        else {
                                            updateTimeSlot.status = "available";
                                        }

                                        updateTimeSlot.resourceId = appointmentResource.ResourceID;
                                        updateTimeSlot.appointmentStartTime = appointmentStartTime;
                                        updateTimeSlot.appointmentEndTime = appointmentEndTime;
                                        updateTimeSlot.appointmentId = appointmentResource.AppointmentID;
                                    });
                                }
                            }
                        });
                    });
                }
                //$scope.$broadcast('getComplete'); 
            };

            var generateCalenderForCredentials = function () {
                angular.forEach($scope.credentials, function (selectedCredential) {
                    $scope.generateTimeSlots(selectedCredential.selectedProviderId, selectedCredential.CredentialID, 1);
                });
            };

            $scope.bindResourceAvailability = function (timeSlotType) {
                var defer = $q.defer();
                var promises = [];

                if (timeSlotType === 1) {
                    angular.forEach($scope.credentials, function (credential) {
                        if (credential.selectedProviderId != null && credential.selectedProviderId > 0) {
                            promises.push($scope.getResourceAvailability(2, credential.CredentialID, 1));
                        }
                    });
                }
                else if (timeSlotType === 2) {
                    angular.forEach($scope.nonSpecialistProviders, function (nonSpecialistProvider) {
                        if (nonSpecialistProvider.selectedProviderId != null && nonSpecialistProvider.selectedProviderId > 0) {
                            promises.push($scope.getResourceAvailability(2, nonSpecialistProvider.nonSpecialistID, 2));
                        }
                    });
                }

                return $q.all(promises);
            }
            
            $scope.changeCalander = function (direction, type) {
                $scope.selectedTimeSlots = [];
                switch (type) {
                    case "year":
                        if (direction === "left")
                            $scope.selectedYear = $scope.selectedYear - 1;
                        else
                            $scope.selectedYear = $scope.selectedYear + 1;

                        /*************Code to check if week start and end date do not exceed total month days in case of Feb month ***************/
                        if ($scope.selectedMonth === 2) {
                            $scope.totalMonthDays = getTotalMonthDay($scope.selectedMonth, $scope.selectedYear);

                            if ($scope.weekEndDate > $scope.totalMonthDays) {
                                $scope.weekEndDate = $scope.totalMonthDays;
                                $scope.weekStartDate = $scope.weekEndDate - 6;
                            }
                        }
                        break;

                    case "month":
                        if (direction === "left") {
                            if (($scope.selectedMonth - 1) >= 1) {
                                $scope.selectedMonth = $scope.selectedMonth - 1;
                            }
                            else {
                                $scope.selectedMonth = 12;
                                $scope.selectedYear = $scope.selectedYear - 1;
                            }
                        }
                        else {
                            if (($scope.selectedMonth + 1) <= 12) {
                                $scope.selectedMonth = $scope.selectedMonth + 1;
                            }
                            else {
                                $scope.selectedMonth = 1;
                                $scope.selectedYear = $scope.selectedYear + 1;
                            }
                        }

                        $scope.totalMonthDays = getTotalMonthDay($scope.selectedMonth, $scope.selectedYear);
                        if ($scope.weekEndDate > $scope.totalMonthDays) {
                            $scope.weekStartDate = $scope.totalMonthDays - 6;
                            $scope.weekEndDate = $scope.totalMonthDays;
                        }
                        break;

                    case "week":
                        if (direction === "left") {
                            if (($scope.weekStartDate - 6) > 0) {
                                $scope.weekEndDate = $scope.weekStartDate - 1;
                                $scope.weekStartDate = $scope.weekStartDate - 6;
                            }
                            else {
                                if ($scope.weekStartDate === 1) { //code to check if week start date is 1 if yes then set weekStartDate and weekEndDate of previous month .
                                    if (($scope.selectedMonth - 1) >= 1)
                                        $scope.selectedMonth = $scope.selectedMonth - 1;
                                    else {
                                        $scope.selectedMonth = 12;
                                        $scope.selectedYear = $scope.selectedYear - 1;
                                    }
                                    $scope.totalMonthDays = getTotalMonthDay($scope.selectedMonth, $scope.selectedYear);
                                    $scope.weekEndDate = $scope.totalMonthDays;
                                    $scope.weekStartDate = $scope.totalMonthDays - 6;
                                }
                                else {
                                    $scope.weekEndDate = $scope.weekStartDate - 1;
                                    $scope.weekStartDate = 1;
                                }
                            }
                        }

                        else {
                            if (($scope.weekEndDate + 6) <= $scope.totalMonthDays) {
                                $scope.weekStartDate = $scope.weekEndDate + 1;
                                $scope.weekEndDate = $scope.weekEndDate + 6;
                            }
                            else {
                                //code to check if week end date is the last date of current month if yes then set weekStartDate and weekEndDate of next month accordingly .
                                if ($scope.weekEndDate === $scope.totalMonthDays) {
                                    if (($scope.selectedMonth + 1) <= 12)
                                        $scope.selectedMonth = $scope.selectedMonth + 1;
                                    else {
                                        $scope.selectedMonth = 1;
                                        $scope.selectedYear = $scope.selectedYear + 1;
                                    }
                                    $scope.weekStartDate = 1;
                                    $scope.weekEndDate = $scope.weekStartDate + 6;
                                }
                                else {
                                    $scope.weekStartDate = $scope.weekEndDate + 1;
                                    $scope.weekEndDate = $scope.totalMonthDays;
                                }
                            }
                        }
                        break;

                    case 'date':
                        if ($scope.appointment.AppointmentDate != undefined) {

                            var appointmentDate = new Date($scope.appointment.AppointmentDate);
                            $scope.selectedMonth = appointmentDate.getMonth() + 1;

                            $scope.selectedYear = appointmentDate.getFullYear();

                            $scope.weekStartDate = appointmentDate.getDate();

                            if ($scope.weekStartDate > ($scope.totalMonthDays - 6)) {
                                $scope.weekEndDate = $scope.totalMonthDays;
                            }
                            else {
                                $scope.weekEndDate = $scope.weekStartDate + 6;
                            }
                        }
                        break;
                }

                $scope.totalMonthDays = getTotalMonthDay($scope.selectedMonth, $scope.selectedYear);
                $scope.currentMonthName = monthNames[$scope.selectedMonth - 1];
                $scope.timeSlots = [];
                angular.forEach($scope.groupContacts, function (contact) {
                    generateCalanderForContact(contact.ContactID);
                });

                if ($scope.credentials.length > 0) {
                    generateCalenderForCredentials();
                    $scope.bindResourceAvailability(1).then(function () {
                        highlightFirstAvailableTimeSlot();
                    });
                }

                if ($scope.selectedRoomId > 0) {
                    generateCalenderForRoom();
                    $scope.getResourceAvailability(1, $scope.selectedRoomId, 3);
                }
                generateCalenderForNonSpecialistProvider();
                $scope.bindResourceAvailability(2).then(function () {
                    highlightFirstAvailableTimeSlot();
                    $("div.owl-wrapper").css("transform", "");
                });
            };

            $scope.initializeBootstrapTable = function() {
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
                            field: 'FirstName',
                            title: 'First Name'
                        },
                        {
                            field: 'LastName',
                            title: 'Last Name'
                        },
                        {
                            field: 'Middle',
                            title: 'Middle Name'
                        },
                        {
                            field: 'DOB',
                            title: 'Date Of Birth',
                            formatter: function (value, row, index) {
                                if (value) {
                                    var formattedDate = $filter('formatDate')(value, 'MM/DD/YYYY');
                                    return formattedDate;
                                } else
                                    return '';
                            }
                        },
                        {
                            field: 'GenderText',
                            title: 'Gender'
                        },
                        {
                            field: 'SSN',
                            title: 'SSN',
                            formatter: function (value, row, index) {
                                var formattedSSN = $filter('toMaskSSN')(value);
                                return formattedSSN;
                            }
                        },
                        {
                            field: 'ContactID',
                            title: 'Actions',
                            formatter: function (value, row) {
                                return '<plus-button title="Add Contact" permission-key="Scheduling-Appointment-GroupAppointment" permission="create" data-ignore-dirty="true" data-ng-click="addContact(' + value + ', &quot;' + row.FirstName + '&quot;, &quot;' + row.LastName + '&quot;, ' + row.ContactTypeID + ',$event,false)" space-key-press data-default-action></plus-button>';
                            }
                        }
                    ]
                };
            };

            $scope.resetEnterKey = function() {
                $scope.enterKeyStop = true;
                $scope.stopNext = false;
                $scope.saveOnEnter = false;
            };

            $scope.stopEnterKey = function() {
                if (!$('#contactListModel').hasClass('in')) {      
                    $scope.enterKeyStop = false;
                    $scope.stopNext = false;
                    $scope.saveOnEnter = true;
                } else
                    $scope.resetEnterKey();
            };
            
            var getGenderText = function(genderID) {
                var result = $scope.GenderList.filter(
                    function(obj) {
                        return (obj.ID === genderID);
                    }
                );
                return result;
            };

            var removeContactFromList = function(contactList, contactId) {
                contactList.filter(function(obj) {
                    if (obj.ContactID === contactId) {
                        var index = contactList.indexOf(obj);
                        if (index !== -1) {
                            contactList.splice(index, 1);
                        }
                    }
                });

                return contactList;
            };

            var bindDataModel = function (model, showCurrentUser) {
                //$scope.initCollateral();
                var listToBind = model;
                if ($scope.GenderList == undefined || $scope.GenderList == null)
                    $scope.GenderList = $scope.getLookupsByType('Gender');
                if (!showCurrentUser) {
                    listToBind = removeContactFromList(listToBind, $scope.contactID);
                }
                angular.forEach(listToBind, function (collateral) {
                    collateral.DOB = collateral.DOB ? $filter('formatDate')(collateral.DOB, 'MM/DD/YYYY') : "";
                    if (collateral.GenderID > 0)
                        collateral.GenderText = getGenderText(collateral.GenderID)[0].Name;
                });

                return listToBind;
            };

            $scope.getClientSummary = function(searchText) {
                if (searchText != undefined && searchText !== '') {
                    prevContact = "";
                    clientSearchService.getClientSummary(searchText, contactTypeSearch).then(function (data) {
                        $scope.contactList = bindDataModel(data.DataItems, false);
                        if ($scope.contactList != null && $scope.contactList.length > 0) {
                            $('#contactListModel').on('hidden.bs.modal', function() {
                                $scope.stopEnterKey();
                                var focus = $('#contactSearch').is(":focus");
                                if (!focus) {
                                    $('#contactSearch').focus();
                                }
                                $scope.groupSchedulingSearch = '';
                            });
                            if ($scope.contactList != null && $scope.contactList.length > 0) {
                                $scope.contactsTable.bootstrapTable('load', $scope.contactList);
                                $('#contactListModel').modal('show');
                                $('#contactListModel').on('shown.bs.modal', function() {
                                    $scope.resetEnterKey();
                                    $rootScope.setFocusToGrid('contactsTable');
                                });
                            }
                            else {
                                $scope.contactsTable.bootstrapTable('removeAll');
                            }
                        }
                        else {
                            $scope.contactsTable.bootstrapTable('removeAll');
                        }
                    }, function (errorStatus) {
                        alertService.error('Unable to connect to server');
                    });
                } else {
                    $scope.stopEnterKey();
                }
            };

            $scope.EnableDisableEnterKey = function (searchText, isSearch) {
                if (searchText != null && searchText !== "") {
                    $scope.resetEnterKey();
                    if (isSearch) {
                        $scope.getClientSummary(searchText);
                    }
                }
            };
            var prevContact = "";
            var prevcontactcount = 0;

            $scope.addContact = function(contactID, firstName, lastName, contactTypeID, $event, isInit) {
                if($event !== undefined && $event !== null)
                    $event.stopPropagation();
                var fullName = firstName + ' ' + lastName;
                if (prevcontactcount === 1) {
                    prevContact = "";
                    prevcontactcount = 0;
                }
                if (prevContact === "" || prevContact !== contactID) {
                    prevContact = contactID;
                    generateCalanderForNewContact(contactID, fullName, isInit);
                } else prevcontactcount = 1;
                
            };

            $scope.getFacility = function () {
                $scope.facilityData = lookupService.getLookupsByType('Facility');
                angular.forEach($scope.facilityData, function (param) {
                    $scope.locations.push({
                        ID: locationCounter++,
                        LocationID: param.ID,
                        Name: param.Name,
                        type: 1
                    });
                });
            };

            $scope.getLocations = function () {
                return $scope.promiseNoOp().then(function() {
                    $scope.getFacility();
                });
            };

            $scope.cancelModal = function () {
                        $('#contactListModel').modal('hide');
                        $scope.groupSchedulingSearch = '';
            };

            $scope.validateAppointmentTime = function() {
                $timeout(function() {
                    $scope.setAmPmForAppointmentEndTime();
                    $scope.validateTime(false);
                });
            };

            $scope.validateStartTimeWatch = function () {
                var max = 1800;
                var min = 800;
                var formatStart = $scope.appointment.StartAMPM;
                var format = $scope.appointment.AMPM;
                var time;
                var isValid;

                if (formatStart != undefined && format != undefined && !$scope.isCancel && $scope.appointment.AppointmentStartTime) {
                    time = $filter('toMilitaryTime')($scope.appointment.AppointmentStartTime, formatStart);
                    isValid = time >= min && time < max;

                    return isValid;
                }

                return true;
            };

            $scope.validateEndTimeWatch = function () {
                var max = 1800;
                var min = 800;
                var format = $scope.appointment.AMPM;

                if (format != undefined && !$scope.isCancel && $scope.appointment.AppointmentEndTime) {
                    var time = $filter('toMilitaryTime')($scope.appointment.AppointmentEndTime, format);
                    var isValid = time > min && time <= max;

                    return isValid;
                }

                return true;
            };
            
            $scope.removeResourceFromUI = function (identifier, type) {
                var resourceId = 0, resourceTypeID = 2;
                if (type === 0) {
                    resourceTypeID = type;
                }

                //remove resource from appointmentResources
                switch (type) {
                    case 0:
                        $scope.groupContacts = $filter('filter')($scope.groupContacts, function (item) {
                            return item.ContactID !== identifier;
                        });

                        $scope.timeSlots = $filter('filter')($scope.timeSlots, function (timeslot) {
                            return timeslot.ContactId !== identifier;
                        });

                        //selectedtimeslots?
                        resourceId = identifier;
                        $scope.ctrl.groupSchedulingForm.modified = true;
                        break;
                    case 1:
                        var selectedCredential = $filter('filter')($scope.credentials, function (item) { return item.CredentialID === identifier && item.selectedProviderId != null; });

                        $scope.credentials = $filter('filter')($scope.credentials, function (item) {
                            return item.CredentialID !== identifier;
                        });

                        $scope.timeSlots = $filter('filter')($scope.timeSlots, function (timeslot) {
                            return timeslot.groupId !== identifier;
                        });

                        $scope.selectedTimeSlots = $filter('filter')($scope.selectedTimeSlots, function (item) {
                            return item.groupId !== identifier;
                        });

                        $scope.ctrl.groupSchedulingForm.modified = true;
                        if (selectedCredential.length === 0)
                            return;
                        resourceId = selectedCredential[0].selectedProviderId;
                        break;
                    case 2:

                        $scope.nonSpecialistProviders = $filter('filter')($scope.nonSpecialistProviders, function (item) {
                            return item.nonSpecialistID !== identifier;
                        });

                        $scope.providers = $filter('filter')($scope.providers, function (item) {
                            return item.nonSpecialistID !== identifier;
                        });

                        $scope.timeSlots = $filter('filter')($scope.timeSlots, function (timeslot) {
                            return timeslot.groupId !== identifier;
                        });

                        $scope.selectedTimeSlots = $filter('filter')($scope.selectedTimeSlots, function (item) {
                            return item.groupId !== identifier;;
                        });

                        var selectedCredential = $filter('filter')($scope.nonSpecialistProviders, function (item) { return item.nonSpecialistID === identifier && item.selectedProviderId != null; });
                        $scope.ctrl.groupSchedulingForm.modified = true;
                        if (selectedCredential.length === 0)
                            return;

                        resourceId = selectedCredential[0].selectedProviderId;
                        break;

                    default:
                        break;
                }

                var filteredResource = $filter('filter')($scope.appointmentResources, function (item) {
                    return !(item.ResourceID === resourceId && item.ResourceTypeID === resourceTypeID);
                });

                var filteredTimeSlot = $filter('filter')($scope.timeSlots, function (item) {
                    return !(item.resourceId === resourceId && item.ResourceTypeID === 2);
                });

                $scope.appointmentResources = filteredResource;
                $scope.timeSlots = filteredTimeSlot;

                highlightFirstAvailableTimeSlot();

            }

            $scope.removeResourceFromDB = function (member, identifier, type, fullname) {

                // Set flyout vars
                var name = (type == 0) ? fullname : $filter('filter')($scope.providers, function (item) {
                    return item.ResourceID == member.selectedProviderId;
                })[0].ResourceName;
                $scope.removeResourceName = name.toUpperCase();
                $scope.removeResourceID = (type == 0) ? member.ContactID : member.selectedProviderId;
                $scope.removeResourceType = (type == 0) ? 7 : type;
                $scope.removeResourceSaved = false;
                $scope.removeMember = member;
                $scope.removeType = type;


                // Left side fly-out to remove the member
                $('[ui-view="navigation"]').css('display', 'none')
                angular.element('.row-offcanvas').addClass('active');

            };

            $scope.removeResource = function (identifier, type, fullName) {
                bootbox.confirm("Are you sure that you want to remove this member?", function (result) {
                    if (result === true) {
                        var foundmember = null;
                        if (type == 0)
                            foundmember = $filter('filter')($scope.groupContacts, function (item) {
                                return item.ContactID == identifier
                            })[0];
                        else if (type == 2)
                            foundmember = $filter('filter')($scope.nonSpecialistProviders, function (item) {
                                return item.nonSpecialistID == identifier
                            })[0];
                        if ($scope.groupID == 0 || ($scope.groupID != 0 && foundmember != null && foundmember.IsNew == true))
                            $scope.removeResourceFromUI(identifier, type);
                        else
                            $scope.removeResourceFromDB(foundmember, identifier, type, fullName);
                    }
                });
            };

            $scope.closeFlyout = function () {
                $('[ui-view="navigation"]').css('display', '')
                angular.element('.row-offcanvas').removeClass('active');
                $scope.removeResourceName = '';
                $scope.removeResourceID = 0;
                $scope.removeResourceType = 0;
                $scope.removeResourceSaved = false;
                $scope.removeResourceComment = '';
                $scope.removeResourceCancelReasonID = 0;
            };

            $scope.finishRemoveResource = function () {

                if ($scope.removeResourceCancelReasonID == null || $scope.removeResourceCancelReasonID == 0) {
                    bootbox.alert('Please select a valid cancel reason.');
                    return;
                }

                // Find this member's IDs first 
                var resource = $filter('filter')($scope.appointmentResources, function (item) {
                    return item.ResourceID == $scope.removeResourceID && item.ResourceTypeID == $scope.removeResourceType;
                });
                $scope.ItemsToSave = 3;
                $scope.SaveItemsCounter = 0;

                // Remove from appt resource table
                appointmentService.deleteAppointmentResource(resource[0].AppointmentResourceID).then(function (response) {
                    if (response.ResultCode === 0) {
                        $scope.SaveItemsCounter++;
                        $scope.VerifySuccess();
                    } else {
                        alertService.error('Error while removing an appointment resource! Please reload the page and try again.');
                    }
                });

                // Remove from group scheduling resource table
                groupSchedulingService.deleteGroupSchedulingResource(resource[0].GroupHeaderID, resource[0].GroupResourceID).then(function (response) {
                    if (response.ResultCode === 0) {
                        $scope.SaveItemsCounter++;
                        $scope.VerifySuccess();
                    } else {
                        alertService.error('Error while removing an group scheduling resource! Please reload the page and try again.');
                    }
                });

                // Add appt status detail w/comments and reason
                appointmentService.addAppointmentStatusDetail($scope.getApptStatus(resource[0])).then(function (response) {
                    if (response.ResultCode === 0) {
                        $scope.SaveItemsCounter++;
                        $scope.VerifySuccess();
                    } else {
                        alertService.error('Error while adding an appointment status detail! Please reload the page and try again.');
                    }
                });
                if ($scope.removeType == 0)
                    $scope.removeResourceFromUI($scope.removeResourceID, $scope.removeType);
                else 
                    $scope.removeResourceFromUI($scope.removeMember.nonSpecialistID, $scope.removeType);
            }

            $scope.getApptStatus = function (item) {
                var apptstatus = {};
                apptstatus.AppointmentResourceID = item.AppointmentResourceID;
                apptstatus.AppointmentID = $scope.AppointmentID;
                apptstatus.AppointmentStatusID = $filter('filter')(lookupService.getLookupsByType('AppointmentStatus'), { AppointmentStatus: 'Cancel' }, true)[0].AppointmentStatusID;
                apptstatus.IsCancelled = true;
                apptstatus.CancelReasonID = $scope.removeResourceCancelReasonID;
                apptstatus.Comments = $scope.removeResourceComment;
                return apptstatus;
            }

            $scope.VerifySuccess = function () {
                if ($scope.SaveItemsCounter == $scope.ItemsToSave) {
                    alertService.success('Group member removed successfully!');
                    $scope.SaveItemsCounter = 0;
                    $scope.ItemsToSave = 0;
                    $scope.removeResourceSaved = true;
                    //resetForm();
                }
            }

            $scope.getGroupData = function (groupID) {
                if (groupID !== 0) {
                    return groupSchedulingService.getGroupByID(groupID).then(function (response) {
                        if (response.ResultCode === 0 && response.DataItems.length === 1) {
                            $scope.groupScheduling = $.extend(true, {}, response.DataItems[0], response.DataItems[0].GroupDetails[0]);
                        } else {
                            alertService.error('Error while loading group data');
                        }
                    });
                } else {
                    return $scope.promiseNoOp();
                }
            };

            var handleNextState = function () {
                var nextState = angular.element("li[data-state-name].list-group-item.active").next("li[data-state-name].list-group-item");
                if (nextState.length === 0) {
                    $scope.Goto('^');
                } else {
                    $timeout(function () {
                        $rootScope.setform(false);
                        var nextStateName = nextState.attr('data-state-name');
                        
                        $scope.Goto(nextStateName, { GroupID: $scope.groupID, ReadOnly: $scope.isReadOnly ? 'ReadOnly' : 'Edit' });
                    });
                }
            };

            var saveGroupData = function (isUpdate) {
                
                var q = $q.defer();
                var groupHeaderObject = {};
                var groupDetails = [];
                groupDetails.push($scope.groupScheduling);
                groupHeaderObject = { GroupHeaderID: null, GroupDetailID: null, Comments: $scope.groupScheduling.Comments, GroupDetails: groupDetails};
                if (isUpdate) {
                    groupHeaderObject.GroupHeaderID = $scope.groupID;
                    groupHeaderObject.GroupDetailID = $scope.groupScheduling.GroupDetailID;
                    q.resolve(groupSchedulingService.updateGroupData(groupHeaderObject));
                } else {
                     
                    q.resolve(groupSchedulingService.addGroupData(groupHeaderObject));
                }

                return q.promise;
            };

            var saveGroupResources = function(isUpdate, groupID, appointmentID) {
                var q = $q.defer();
                //single selectedRoomId, nonSpecialistProviders, groupContacts = 3 types of potential resources
                //ToDo: Set the appointmentresourceid via a filter for each resource type
                var resourceBaseObj = { GroupHeaderID: groupID, IsPrimary: false, PrimaryAppointmentID: appointmentID };
                var finalResources = [];
                if ($scope.selectedRoomId !== null && $scope.selectedRoomId !== undefined && $scope.selectedRoomId !== 0) {
                    var roomObj = { ResourceID: $scope.selectedRoomId, ResourceTypeID: 1 };
                    if (isUpdate) {
                        var roomApptResourceID = $filter('filter')($scope.appointmentResources, { ResourceID: $scope.selectedRoomId, ResourceTypeID: 1 });
                        if (roomApptResourceID !== null && roomApptResourceID !== undefined) {
                            roomObj.AppointmentResourceID = roomApptResourceID[0].AppointmentResourceID;
                            roomObj.GroupResourceID = roomApptResourceID[0].GroupResourceID;
                        }
                    }
                    finalResources.push($.extend(true, {}, resourceBaseObj, roomObj));
                }
                angular.forEach($scope.nonSpecialistProviders, function (provider) {
                    var providerObj = { ResourceID: provider.selectedProviderId, ResourceTypeID: 2 };
                    if (isUpdate) {
                        var providerApptResourceID = $filter('filter')($scope.appointmentResources, { ResourceID: provider.selectedProviderId, ResourceTypeID: 2 }, true);
                        if (providerApptResourceID !== null && providerApptResourceID !== undefined && providerApptResourceID.length > 0) {
                            providerObj.AppointmentResourceID = providerApptResourceID[0].AppointmentResourceID;
                            providerObj.GroupResourceID = providerApptResourceID[0].GroupResourceID;
                        }
                    }
                    finalResources.push($.extend(true, {}, resourceBaseObj, providerObj));
                });
                angular.forEach($scope.groupContacts, function (contact) {
                    var contactObj = { ResourceID: contact.ContactID, ResourceTypeID: 7 };
                    if (isUpdate) {
                        var contactApptResourceID = $filter('filter')($scope.appointmentResources, { ResourceID: contact.ContactID, ResourceTypeID: 7 });
                        if (contactApptResourceID !== null && contactApptResourceID !== undefined) {
                            contactObj.AppointmentResourceID = contactApptResourceID[0].AppointmentResourceID;
                            contactObj.GroupResourceID = contactApptResourceID[0].GroupResourceID;
                        }
                    }
                    finalResources.push($.extend(true, {}, resourceBaseObj, contactObj));
                });

                var groupResourceObj = { ResourceID: groupID, ResourceTypeID: 5 };
                if (isUpdate) {
                    var groupApptResourceID = $filter('filter')($scope.appointmentResources, { ResourceID: groupID, ResourceTypeID: 5 });
                    if (groupApptResourceID !== null && groupApptResourceID !== undefined) {
                        groupResourceObj.AppointmentResourceID = groupApptResourceID[0].AppointmentResourceID;
                        groupResourceObj.GroupResourceID = groupApptResourceID[0].GroupResourceID;
                    }
                }
                finalResources.push($.extend(true, {}, resourceBaseObj, groupResourceObj));

                if (isUpdate) {
                    q.resolve(groupSchedulingService.updateResources(finalResources));
                } else {
                    q.resolve(groupSchedulingService.addResources(finalResources));
                }

                return q.promise;
            };

            var saveAppointment = function(isUpdate) {
                var q = $q.defer();

                //ToDo: Recurrence in progress
                $scope.appointment.Recurrence = $scope.Recurrence;
                if ($scope.appointment != null && $scope.appointment.Recurrence != null)
                    $scope.appointment.Recurrence.RecurrenceID = $scope.appointment.RecurrenceID;

                var appObj = $scope.appointment;
                appObj.FacilityID = $scope.selectedLocation.LocationID;
                appObj.AppointmentTypeID = $scope.groupScheduling.GroupTypeID;
                appObj.IsGroupAppointment = true;

                if (isUpdate) {
                    q.resolve(appointmentService.updateAppointment(appObj));
                } else {
                    q.resolve(appointmentService.addAppointment(appObj));
                }

                return q.promise;
            };

            $scope.recurrenceChanged = function () {
                if ($scope.Recurrence.IsRecurring) {
                    $scope.Recurrence.StartDate = $scope.appointment.AppointmentDate;
                }
                else {
                    $scope.Recurrence.StartDate = null;
                }

                if ($scope.Recurrence.Ending == null && $scope.Recurrence.IsRecurring) {
                    $scope.Recurrence.Ending = 3;
                } else if (!$scope.Recurrence.IsRecurring) {
                    $scope.Recurrence.Ending = null;
                }
            }

            $scope.save = function (isNext, isMandatory, hasErrors) {
                if (!$scope.ctrl.groupSchedulingForm.$valid) {
                    alertService.error("Please correct the errors on the page before proceeding");
                    return;
                }
                if (formService.isDirty() && !hasErrors) {
                    //validation,save, and then postsave
                    var currentTime = getCurrentTime();
                    var todayDate = getCurrentDate();
                    var startTime = $filter("toMilitaryTime")($scope.appointment.AppointmentStartTime, $scope.appointment.AMPM);
                    var difference = calculateTimeDifference(startTime, currentTime);
                    var currentTimeSlotDate = $filter('toMMDDYYYYDate')($scope.selectedTimeSlots[0].date, 'MM/DD/YYYY');
                    if ($scope.selectedTimeSlots == undefined || $scope.selectedTimeSlots.length == 0) {
                        alertService.error('No time slot is selected.');
                        return $scope.promiseNoOp();
                    }
                    if (currentTimeSlotDate < todayDate || (currentTimeSlotDate === todayDate && difference < 0)) {
                        alertService.error('Appointment can\'t be scheduled for past date.');
                        return $scope.promiseNoOp();
                    }
                    var isTimeSlotAvailable = false;
                    angular.forEach($scope.selectedTimeSlots, function (timeSlot) {
                        if ((timeSlot.type !== 0 && timeSlot.resourceId !== 0 && timeSlot.status !== "blocked" && timeSlot.status !== "appointment") || (timeSlot.appointmentId && timeSlot.appointmentId === $scope.appointment.AppointmentID)) {
                            isTimeSlotAvailable = true;
                        }
                    });
                    if (!isTimeSlotAvailable) {
                        alertService.error('No time slot is available.');
                        return $scope.promiseNoOp();
                    }

                    //custom validation to ensure that at least 1 provider and 2 contacts have been assigned to the group
                    if (($scope.groupContacts.length < 2) || $scope.nonSpecialistProviders.length < 1) {
                        alertService.error('The appointment must contain a provider and at least two contacts');
                        return $scope.promiseNoOp();
                    }

                    //The same provider cannot be added multiple times
                    var sortedNonSpecialists = $scope.nonSpecialistProviders.slice().sort();
                    for (var i = 0; i < $scope.nonSpecialistProviders.length; i++) {
                        var nextSpecialist = sortedNonSpecialists[i + 1];
                        if (nextSpecialist !== undefined && nextSpecialist !== null) {
                            if (nextSpecialist.selectedProviderId === sortedNonSpecialists[i].selectedProviderId) {
                                alertService.error('The same provider cannot be added multiple times');
                                return $scope.promiseNoOp();
                            }
                        }
                    }

                    var saveTypeMessage = $scope.groupID > 0 ? 'updated' : 'added';
                    var isUpdate = $scope.groupID > 0 ? true : false;
                    if ($scope.IsRecurringAptEdit && isUpdate && $scope.Recurrence != null && $scope.Recurrence != '' && $scope.Recurrence.IsRecurring) {
                        bootbox.confirm("If you changed specific appointments in the series, your changes will be cancelled and those appointments will match the series again.", function (result) {
                            if (result === true)
                                $scope.finishSave(isUpdate, isNext);
                        });
                    }
                    else
                        $scope.finishSave(isUpdate, isNext);
                    
                } else if (!formService.isDirty() && isNext) {
                    return handleNextState();
                } else {
                    return $scope.promiseNoOp();
                }
            };

            $scope.finishSave = function (isUpdate, isNext) {

                    //ToDo: service calls to save the data in the correct order
                    return saveGroupData(isUpdate).then(function(groupResponse) {
                        if (groupResponse.ResultCode === 0) {
                            
                            var currentGroupID = isUpdate ? $scope.groupID : groupResponse.ID;
                            if (!isUpdate) {
                                $scope.groupID = currentGroupID;
                            }
                            saveAppointment(isUpdate).then(function (apptResponse) {
                                if (apptResponse.ResultCode === 0) {
                                    var currentAppointmentID = isUpdate ? $scope.appointment.AppointmentID : apptResponse.ID;
                                    if (!isUpdate) {
                                        $scope.appointment.AppointmentID = currentAppointmentID;
                                    }
                                    saveGroupResources(isUpdate, currentGroupID, currentAppointmentID).then(function (resourceResponse) {
                                        if (resourceResponse.ResultCode === 0) {
                                            alertService.success('Group appointment saved successfully');
                                            $scope.postSave(isNext, isUpdate, currentGroupID);
                                        } else {
                                            alertService.error('Error while saving the group appointment');
                                        }
                                    });
                                } else {
                                    alertService.error('Error while saving the group appointment');
                                }
                            });   
                        } else {
                            alertService.error('Error while saving the group appointment');
                        }
                    });
                }

            $scope.postSave = function(isNext, isUpdate, groupID) {
                if (isNext) {
                    handleNextState();
                } else {
                    if (isUpdate) {
                        $scope.init();
                    } else {
                        $state.transitionTo($state.current.name, { GroupID: groupID, ReadOnly: $stateParams.ReadOnly }, { notify: true });
                    }
                }
            };

            $scope.init();
        }]);