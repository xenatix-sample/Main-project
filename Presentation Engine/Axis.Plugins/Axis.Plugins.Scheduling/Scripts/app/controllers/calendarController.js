angular.module('xenatixApp')
    .controller('calendarController', [
        '$scope', 'appointmentService', 'resourceService', 'lookupService', 'navigationService', 'registrationService', 'alertService', '$timeout', '$q', '$filter', '$interval', '$rootScope', 'groupSchedulingSearchService',
function ($scope, appointmentService, resourceService, lookupService, navigationService, registrationService, alertService, $timeout, $q, $filter, $interval, $rootScope, groupSchedulingSearchService) {
            $scope.init();

            $scope.init = function() {
                $scope.isLoading = false;
                $scope.resourceTypeID = 2; //TODO: Add to Navigation Service / Items
                $scope.resourceID = 0;
                $scope.CurrentUserID = 0;
                $scope.contactIDs = [];
                $scope.contactList = [];
                $scope.clientTypeList = lookupService.getLookupsByType('ClientType');
                $scope.users = lookupService.getLookupsByType('Users');
                $scope.AppointmentStatusList = lookupService.getLookupsByType('AppointmentStatus');
                $scope.appointmentTypes = [];
                $scope.appointmentTypeClasses = ['status-one', 'status-two', 'status-three', 'status-four'];
                $scope.resourceAvailabilities = [];
                $scope.appointmentToEdit = {};
                //TODO: Get the following from website/user configuration
                $scope.config = {
                    facilities: lookupService.getLookupsByType('Facility'),
                    users: lookupService.getLookupsByType('Users'),
                    calendarStartsOnDay: 1,
                    includeSaturday: true,
                    includeSunday: true,
                    baselineStartDate: new Date(new Date().toDateString()),
                    calendarStartHour: 8,
                    calendarEndHour: 18,
                    calendarTimeIncrement: 0.25, //TODO: Possible values should be 0.25, 0.5, and 1.0 only; perhaps a drop-down?
                    timeMarkerTop: undefined
                };
                $scope.initializeAppointmentDetails();
                appointmentService.getAppointmentType(0).then(function(data) {
                    if ((data !== undefined) && ('DataItems' in data) && (data.DataItems.length > 0)) {
                        $scope.appointmentTypes = data.DataItems;
                        for (var iIdx = 0; iIdx < data.DataItems.length; iIdx++) {
                            data.DataItems[iIdx].AppointmentTypeClass = (iIdx < $scope.appointmentTypeClasses.length) ? $scope.appointmentTypeClasses[iIdx] : 'date_appt';
                        }
                    }
                });
                navigationService.get().then(function(data) {
                    $scope.resourceID = $scope.currentUserID = data.DataItems[0].UserID;
                    $scope.config.selectedUserID = $scope.resourceID;
                    $scope.resourceName = data.DataItems[0].UserFullName;
                    //This loads all of the current user's data. This will need to be called everytime that the user dropdown is changed
                    $scope.setResourceBasedData($scope.resourceID, $scope.resourceTypeID).then(function() {
                        $scope.$watch('config.selectedFacilityID', function (newValue, oldValue) {
                            if ((newValue !== oldValue) && newValue !== 0)
                                $scope.filterApptsByFacility();
                        });
                        $scope.$watch('config.selectedUserID', function (newValue, oldValue) {
                            if ((newValue !== oldValue) && newValue !== 0)
                                $scope.filterAppointmentsByUser(newValue);
                        });
                        $scope.$watchGroup(['config.includeSaturday', 'config.includeSunday'], function (newValues, oldValues) {
                            if ((newValues[0] !== oldValues[0]) || (newValues[1] !== oldValues[1])) {
                                $scope.updateIncludedDays();
                                $scope.buildCalendarMatrix();
                            }
                        });
                        $scope.$watch('config.baselineStartDate', function () {
                            $scope.buildCalendar();
                        });
                        $interval($scope.calculateTimeMarkerTop, 60000);
                    });
                });
            };

            $scope.IsDateInThePast = function () {
                var curdate = new Date();

                var tmpaptdate = moment($scope.apptDetails.ApptDate)._d;
                var aptdate = new Date(tmpaptdate.getFullYear(), tmpaptdate.getMonth(), tmpaptdate.getDate(), getTime($scope.apptDetails.StartTime).getHours(), getTime($scope.apptDetails.StartTime).getMinutes());

                if (aptdate < curdate)
                    return true;
                return false;
            }

            $scope.cancelSingleOrRecurrentModal = function () {
                $scope.appointmentToEdit = {};
                $('#editSingleOrRecurrentAptModal').modal('hide');
            }

            $scope.showSingleOrRecurringApptModal = function () {

                $scope.appointmentToEdit = $scope.apptDetails;
                $scope.appointmentToEdit.IsRecurrentAppointment = false;
                $('#editSingleOrRecurrentAptModal').modal('show');
            }

            $scope.editSingleOrRecurrigAppt = function () {
                $('#editSingleOrRecurrentAptModal').modal('hide');
                $('body,html').removeClass('modal-open');

                if (!$scope.appointmentToEdit.IsGroupAppointment) {
                    // Single instance of a recurring apt: view mode for past date - 
                    if (!$scope.appointmentToEdit.IsRecurrentAppointment && $scope.IsDateInThePast())
                        $scope.Goto('patientprofile.appointments.viewAppointment', { ReadOnly: 'view', ContactID: $scope.appointmentToEdit.ContactID, AppointmentID: $scope.appointmentToEdit.AppointmentID, IsRecurringAptEdit: $scope.appointmentToEdit.IsRecurrentAppointment });
                    else
                        $scope.Goto('patientprofile.appointments.editAppointment', { ContactID: $scope.appointmentToEdit.ContactID, AppointmentID: $scope.appointmentToEdit.AppointmentID, IsRecurringAptEdit: $scope.appointmentToEdit.IsRecurrentAppointment });
                }
                else {
                    // Single instance of a recurring apt: view mode for past date - 
                    if (!$scope.appointmentToEdit.IsRecurrentAppointment && $scope.IsDateInThePast($scope.appointmentToEdit))
                        $scope.Goto('scheduling.groupscheduling.details.groupschedule', { GroupID: $scope.appointmentToEdit.GroupID, ReadOnly: 'view', IsRecurringAptEdit: $scope.appointmentToEdit.IsRecurrentAppointment });
                    else
                        $scope.Goto('scheduling.groupscheduling.details.groupschedule', { GroupID: $scope.appointmentToEdit.GroupID, ReadOnly: 'edit', IsRecurringAptEdit: $scope.appointmentToEdit.IsRecurrentAppointment });
                }
            }

            $scope.setResourceBasedData = function (resourceID, resourceTypeID) {
                $scope.resourceAvailabilities = [];
                return resourceService.getResourceAvailability(resourceID, resourceTypeID).then(function(availData) {
                    if ((availData !== undefined) && ('DataItems' in availData) && (availData.DataItems.length > 0)) {
                        $scope.resourceAvailabilities = availData.DataItems.map(function (availability) {
                            return {
                                FacilityID: availability.FacilityID,
                                DayOfTheWeek: availability.Days,
                                StartTime: $scope.normalizeApptTime(availability.AvailabilityStartTime),
                                EndTime: $scope.normalizeApptTime(availability.AvailabilityEndTime)
                            };
                        });
                        var startTimes = $scope.resourceAvailabilities.map(function (avail) { return avail.StartTime; }) || [];
                        startTimes.push($scope.config.calendarStartHour);
                        $scope.config.calendarStartHour = Math.min.apply(null, startTimes);
                        var endTimes = $scope.resourceAvailabilities.map(function (avail) { return avail.EndTime; }) || [];
                        endTimes.push($scope.config.calendarEndHour);
                        $scope.config.calendarEndHour = Math.max.apply(null, endTimes);
                    }
                });
            };

            $scope.initializeAppointmentDetails = function() {
                $scope.apptDetails = {
                    ContactID: 0,
                    FullName: '',
                    AppointmentID: 0,
                    ApptDate: '',
                    StartTime: '',
                    EndTime: '',
                    ApptType: '',
                    ApptTypeClass: '',
                    Alias: '',
                    ProgramName: '',
                    Note: '',
                    ProviderName: '',
                    Age: ''
                };
            };

            $scope.buildCalendar = function() {
                $scope.isLoading = true;
                $scope.config.calendarStartDate = new Date($scope.config.baselineStartDate.toDateString());
                while ($scope.config.calendarStartDate.getDay() !== $scope.config.calendarStartsOnDay)
                    $scope.config.calendarStartDate.setDate($scope.config.calendarStartDate.getDate() - 1);
                $scope.calendarDays = [];
                $scope.buildCalendarDays($scope.config.calendarStartDate);
                $scope.updateIncludedDays();
                $scope.calendarHours = [];
                $scope.buildCalendarHours($scope.config.calendarStartDate, $scope.config.calendarStartHour, $scope.config.calendarEndHour, $scope.config.calendarTimeIncrement);
                $scope.initializeCalendarMatrix();
                $scope.getAppointmentData().then(function(datas) {
                    $scope.processAppointmentData(datas).then(function() {
                        $scope.buildTimeSlots();
                        $scope.buildCalendarMatrix();
                        $scope.calculateTimeMarkerTop();
                        $scope.isLoading = false;
                        $timeout(offCanvasNav.init);
                    });
                });
            };

            $scope.buildCalendarDays = function(startDate) {
                var tempDate = new Date(startDate.toDateString());
                var tempDateStartDate = tempDate.getDate();
                for (var iIdx = 0; iIdx < 5; iIdx++) {
                    tempDate = new Date(startDate.toDateString());
                    tempDate.setDate(tempDateStartDate + iIdx);
                    $scope.calendarDays.push($scope.buildCalendarDay(tempDate, true));
                }
                tempDate = new Date(startDate.toDateString());
                tempDate.setDate(tempDateStartDate + 5);
                $scope.calendarDays.push($scope.buildCalendarDay(tempDate, $scope.config.includeSaturday));
                tempDate = new Date(startDate.toDateString());
                tempDate.setDate(tempDateStartDate + 6);
                $scope.calendarDays.push($scope.buildCalendarDay(tempDate, $scope.config.includeSunday));
            };

            $scope.buildCalendarDay = function (tempDate, isIncluded) {
                var tmpMoment = moment.utc(new Date(tempDate));
                return {
                    fullDate: new Date(tempDate),
                    dayOfTheWeek: tmpMoment.format('dddd'),
                    monthName: tmpMoment.format('MMMM'),
                    dateOfTheMonth: tmpMoment.format('M/D'),
                    shortDate: tmpMoment.format('M/D/YYYY'),
                    isIncluded: isIncluded
                };
            };

            $scope.buildCalendarHours = function(startDate, calendarStartHour, calendarEndHour, calendarTimeIncrement) {
                var rowSpan = parseInt(1 / calendarTimeIncrement);
                var tempDate = new Date(startDate);
                for (var iIdx = calendarStartHour; iIdx < calendarEndHour; iIdx += calendarTimeIncrement) {
                    tempDate.setHours(iIdx);
                    $scope.calendarHours.push({
                        hourHeader: (iIdx % 1) === 0 ? tempDate.toLocaleTimeString('en-US', { hour: 'numeric' }) : undefined,
                        rowSpan: (iIdx % 1) === 0 ? rowSpan : 1,
                        slot: iIdx
                    });
                }
            };

            $scope.initializeCarousel = function() {
                angular.element('#scheduling_calendar').owlCarousel({
                    navigation: true,
                    slideSpeed: 300,
                    pagination: false,
                    paginationSpeed: 400,
                    singleItem: true
                });
            };

            /// Calendar navigation
            $scope.changeMonth = function(delta) {
                $scope.config.baselineStartDate.setMonth($scope.config.baselineStartDate.getMonth() + delta);
                $scope.config.baselineStartDate = new Date($scope.config.baselineStartDate.toDateString());
            };

            $scope.changeDate = function(delta) {
                $scope.config.baselineStartDate.setDate($scope.config.baselineStartDate.getDate() + delta);
                $scope.config.baselineStartDate = new Date($scope.config.baselineStartDate.toDateString());
            };

            $scope.changeYear = function(delta) {
                $scope.config.baselineStartDate.setFullYear($scope.config.baselineStartDate.getFullYear() + delta);
                $scope.config.baselineStartDate = new Date($scope.config.baselineStartDate.toDateString());
            };

            /// Appointment-related
            $scope.getAppointmentData = function() {
                return appointmentService.getResourceAppointmentsByWeek($scope.resourceID, $scope.resourceTypeID, $scope.calendarDays[0].fullDate.toDateString());
            };

            $scope.normalizeApptTime = function(apptTime) {
                var remainder = apptTime % 100;
                var slot = (apptTime - remainder) / 100;
                remainder = remainder / 60;
                remainder = (remainder < 0.25) ? 0 : (remainder < 0.5) ? 0.25 : (remainder < 0.75) ? 0.5 : 0.75;
                return slot + remainder;
            };

            $scope.processAppointmentData = function(data) {
                $scope.appointmentData = [];
                var contactIDs = [];
                if ((data !== undefined) && ('DataItems' in data)) {
                    $scope.appointmentData =
                        data.DataItems.map(
                            function(appt) {
                                angular.forEach(appt.Contacts, function(contact) {
                                    if (($scope.contactIDs.indexOf(contact.ResourceID) < 0) && (contactIDs.indexOf(contact.ResourceID) < 0))
                                        contactIDs.push(contact.ResourceID);
                                });

                                return {
                                    ID: appt.AppointmentID,
                                    TypeID: appt.AppointmentTypeID,
                                    IsCancelled: appt.IsCancelled,
                                    IsGroupAppointment: appt.IsGroupAppointment,
                                    GroupID: appt.GroupID,
                                    GroupDetailID: appt.GroupDetailID,
                                    FullDate: appt.AppointmentDate,
                                    Date: moment.utc(appt.AppointmentDate).format('M/D/YYYY'),
                                    Time: appt.AppointmentStartTime,
                                    Length: appt.AppointmentLength,
                                    Slot: $scope.normalizeApptTime(appt.AppointmentStartTime),
                                    Span: Math.ceil(appt.AppointmentLength / ($scope.config.calendarTimeIncrement * 60)),
                                    FacID: null,
                                    Note: appt.ReasonForVisit,
                                    GroupComments:appt.GroupComments,
                                    Comments: appt.Comments,
                                    ProgramUnit: appt.ProgramName,
                                    FacilityName: appt.FacilityName,
                                    Resources: appt.Resources.map(function (res) {
                                        var resourceName = $filter('filter', true)($scope.users, function (user) { return res.ResourceID == user.ID; })[0];
                                        return { TypeID: res.ResourceTypeID, ID: res.ResourceID, Name: (resourceName !== undefined) ? resourceName.Name : '' };
                                    }),
                                    Contacts: appt.Contacts.map(function (contact) { return { ContactID: contact.ResourceID, AppointmentResourceID: contact.AppointmentResourceID, AppointmentStatusID: contact.AppointmentStatusID, AppointmentStatusDetailID: contact.AppointmentStatusDetailID }; }),
                                    ApptInfo: { Name: (appt.IsGroupAppointment) ? appt.GroupName : '', Type: (appt.IsGroupAppointment) ? appt.GroupType : ((appt.AppointmentType != '') ? appt.AppointmentType : ''), ServiceName: appt.ServiceName }
                                };
                            }
                        );

                    angular.forEach($scope.appointmentData, function(appointment) {
                        angular.forEach(appointment.Resources, function (resource) {
                            if (appointment.FacID === null || appointment.FacID === undefined) {
                                if (resource.TypeID === 1) {
                                    appointment.FacID = resource.ID;
                                }
                            }
                        });
                    });

                    var promises = contactIDs.map(function(contactId) { return registrationService.get(contactId); });
                    if (promises.length > 0) {
                        var deferred = $q.defer();
                        $q.all(promises).then(function(contactDatas) {
                            angular.forEach(contactDatas, function(contactData) {
                                if ((contactData !== undefined) && ('DataItems' in contactData) && (contactData.DataItems.length > 0)) {
                                    angular.forEach(contactData.DataItems, function(contact) {
                                        $scope.contactList.push(contact);
                                        $scope.contactIDs.push(contact.ContactID);
                                    });
                                }
                            });
                            deferred.resolve();
                        });
                        return deferred.promise;
                    } else {
                        return $q.when();
                    }
                } else {
                    return $q.when();
                }
            };

            $scope.buildTimeSlots = function() {
                angular.forEach($scope.calendarDays, function(calendarDay) {
                    calendarDay.timeSlots = [];
                    var currentHour = $scope.calendarHours[0].slot;
                    angular.forEach($scope.calendarHours, function(calendarHour) {
                        if (calendarHour.slot >= currentHour) {
                            var filter = { Date: calendarDay.shortDate, Slot: calendarHour.slot, Resources: { TypeID: $scope.resourceTypeID, ID: $scope.resourceID } };
                            if ($scope.config.selectedFacilityID > 0)
                                filter.FacID = $scope.config.selectedFacilityID;
                            var appts = $filter('filter', true)($scope.appointmentData, filter, true);
                            if ((appts !== undefined) && (appts.length > 0)) {
                                calendarDay.timeSlots.push({
                                    ID: appts[0].ID,
                                    TypeID: appts[0].TypeID,
                                    Slot: appts[0].Slot,
                                    Span: appts[0].Span,
                                    Contacts: appts[0].Contacts,
                                    ApptInfo: appts[0].ApptInfo
                                });
                                currentHour += (appts[0].Span * $scope.config.calendarTimeIncrement);
                            } else {
                                calendarDay.timeSlots.push({ ID: 0, Slot: calendarHour.slot, Span: 1 });
                                currentHour += $scope.config.calendarTimeIncrement;
                            }
                        }
                    });
                });
            };

            $scope.initializeCalendarMatrix = function() {
                $scope.calendarMatrix = {};
                angular.forEach($scope.calendarHours, function(calendarHour) {
                    $scope.calendarMatrix[calendarHour.slot.toString()] = ($scope.includedCalendarDays || []).map(function(calendarDay) {
                        return {
                            rowSpan: 1,
                            className: 'date_blocked',
                            title: '',
                            Contacts: [],
                            CID: undefined,
                            AID: undefined
                        };
                    });
                });
            };

            $scope.isAvailable = function (facilityId, dayOfTheWeek, slot) {
                var filter = { DayOfTheWeek: dayOfTheWeek };
                if (facilityId !== 0)
                    filter.FacilityID = facilityId;
                var availabilities = $filter('filter', true)($scope.resourceAvailabilities, filter);
                var isAvailable = false;
                angular.forEach(availabilities, function(availability) {
                    isAvailable = isAvailable || ((slot >= availability.StartTime) && (slot < availability.EndTime));
                });
                return isAvailable;
            };

            $scope.buildCalendarMatrix = function() {
                angular.forEach($scope.calendarHours, function(calendarHour) {
                    $scope.calendarMatrix[calendarHour.slot.toString()] = ($scope.includedCalendarDays || []).map(function(calendarDay) {
                        return $filter('filter', true)(calendarDay.timeSlots, { Slot: calendarHour.slot }, true).map(function(timeSlot) {
                            var contactNames = (timeSlot.ID === 0) ? '' : timeSlot.Contacts.map(function(contact) {
                                var contact = $filter('filter', true)($scope.contactList, { ContactID: contact.ContactID })[0];
                                return { First: contact.FirstName, Last: contact.LastName };
                            });
                            if (timeSlot.ApptInfo != null && timeSlot.ApptInfo.Name == '' && contactNames != "") {
                                timeSlot.ApptInfo.Name = contactNames[0].First + " " + contactNames[0].Last;
                            }
                            var apptType = $filter('filter')($scope.appointmentTypes, { AppointmentTypeID: timeSlot.TypeID })[0] || { AppointmentTypeClass: 'date_appt' };
                            //this is to fix issue where appointment don't have any contact, temp fix it should not happen
                            var timeSlotID = 0;
                            if (timeSlot.Contacts !== undefined && timeSlot.Contacts.length !== 0)
                                timeSlotID = timeSlot.Contacts[0].ContactID;
                            return {
                                rowSpan: timeSlot.Span,
                                className: timeSlot.ID !== 0 ? 'date_appt' : $scope.isAvailable($scope.config.selectedFacilityID || 0, calendarDay.dayOfTheWeek, timeSlot.Slot) ? 'date_avail' : 'date_blocked',
                                title: timeSlot.ID === 0 ? 'Available time slot' : '',
                                Contacts: timeSlot.ID === 0 ? [] : contactNames,
                                CID: timeSlot.ID === 0 ? undefined : timeSlotID,
                                AID: timeSlot.ID,
                                apptType: apptType.AppointmentTypeClass,
                                apptInfo: timeSlot.ApptInfo
                            };
                        })[0] || null;
                    });
                    $scope.calendarMatrix[calendarHour.slot.toString()] = $filter('filter', true)($scope.calendarMatrix[calendarHour.slot.toString()], function(n) { return n !== null; });
                });
            };

            $scope.getCalendarRowData = function(slot) {
                return $scope.calendarMatrix[slot.toString()] || [];
            };

            $scope.updateIncludedDays = function() {
                $scope.calendarDays[5].isIncluded = $scope.config.includeSaturday;
                $scope.calendarDays[6].isIncluded = $scope.config.includeSunday;
                $scope.includedCalendarDays = $filter('filter', true)($scope.calendarDays, function(calendarDay) { return calendarDay.isIncluded === true; });
            };

            $scope.filterAppointmentsByUser = function (selectedUserID) {
                $scope.config.selectedFacilityID = 0;
                $scope.resourceID = selectedUserID;
                $scope.setResourceBasedData($scope.resourceID, $scope.resourceTypeID);
                $scope.buildCalendar();
            };

            $scope.filterApptsByFacility = function () {
                $scope.initializeCalendarMatrix();
                $scope.buildTimeSlots();
                $scope.buildCalendarMatrix();
            };

            $scope.calculateTimeMarkerTop = function() {
                var currentTime = moment(new Date()).format('HH:mm').split(':');
                var hour = angular.element('th[hour=' + parseInt(currentTime[0]) + ']');
                if (hour.length === 1) {
                    $scope.config.timeMarkerTop = hour[0].offsetTop + ((parseInt(currentTime[1]) / 60) * hour[0].offsetHeight) - 9;
                } else $scope.config.timeMarkerTop = undefined;
            };

            $scope.loadAppointmentDetails = function(contactId, appointmentId) {
                if ((contactId > 0) && (appointmentId > 0)) {
                    $scope.initializeAppointmentDetails();
                    var contact = $filter('filter', true)($scope.contactList, { ContactID: contactId })[0];
                    var appt = $filter('filter', true)($scope.appointmentData, { ID: appointmentId })[0];
                    var apptType = $filter('filter', true)($scope.appointmentTypes, { AppointmentTypeID: appt.TypeID })[0] || { AppointmentTypeClass: 'date_appt', AppointmentType: 'Not found' };
                    var startTime = pad(appt.Time, 4);
                    startTime = startTime.substring(0, 2) + ':' + startTime.substring(2);
                    var endTime = new Date(appt.Date + ' ' + startTime);
                    endTime = new Date(endTime.setMinutes(endTime.getMinutes() + appt.Length));
                    var program = $filter('filter', true)($scope.clientTypeList, { ID: contact.ClientTypeID })[0] || { Name: '' };

                    $scope.apptDetails = {
                        ContactID: contactId,
                        FullName: contact.FirstName + ' ' + contact.LastName,
                        AppointmentID: appointmentId,
                        ApptDate: appt.Date,
                        StartTime: startTime,
                        EndTime: moment(endTime).format('HH:mm'),
                        ApptType: apptType.AppointmentType,
                        IsCancelled: appt.IsCancelled,
                        ApptTypeClass: apptType.AppointmentTypeClass,
                        Alias: contact.Alias,
                        ProgramName: program.Name,
                        Note: appt.Note,
                        ProviderName: $scope.resourceName,
                        Providers : $filter('filter', true)(appt.Resources, function (provider) { return provider.TypeID == 2; }),
                        Age: $filter('ageToShow', true)(contact.DOB),
                        IsGroupAppointment: appt.IsGroupAppointment,
                        Contact: appt.Contacts[0]
                    };
                    if (appt.IsGroupAppointment) {
                        $scope.apptDetails.contactsNameList = [];
                        angular.forEach(appt.Contacts, function (apptContact) {
                            var contactName = $filter('filter', true)($scope.contactList, function (listContact) { return apptContact.ContactID == listContact.ContactID; });
                            $scope.apptDetails.contactsNameList.push(contactName[0]);
                        });
                        $scope.apptDetails.Comments = appt.Comments;
                        $scope.apptDetails.ProgramUnit = appt.ProgramUnit;
                        $scope.apptDetails.FacilityName = appt.FacilityName;
                        $scope.apptDetails.GroupID = appt.GroupID;
                        $scope.apptDetails.GroupDetailID = appt.GroupDetailID;
                        $scope.apptDetails.ApptInfo = appt.ApptInfo;
                        $scope.apptDetails.GroupComments = appt.GroupComments;
                    }

                    // TODO: get contact resource type id here and use this instead of the harcoded value
                    appointmentService.getAppointmentByResource(contactId, 7).then(function (data) {
                        if (data != null && data.DataItems != null && data.DataItems.length > 0) {
                            var apt = $filter('filter', true)(data.DataItems, function (item) { return item.AppointmentID == appointmentId; })[0];
                            $scope.apptDetails.RecurrenceID = apt.RecurrenceID;
                        }
                        else {
                            alertService.error('There was a problem getting appointment recurrence information.');
                        }
                    });
                    angular.element('.row-offcanvas').addClass('active');
                }
            };

            $scope.UpdateStatus = function (statusID) {
                var txtStatus = $filter('filter', true)($scope.AppointmentStatusList, { AppointmentStatusID: statusID }, true)[0].AppointmentStatus;
                var message = 'Are you sure you want to ' + txtStatus + ' this appointment?';
                bootbox.confirm(message, function (result) {
                    if (result === true) {
                        var statusObj = {};
                        statusObj.AppointmentResourceID = $scope.apptDetails.Contact.AppointmentResourceID;
                        statusObj.AppointmentID = $scope.apptDetails.AppointmentID;
                        statusObj.AppointmentStatusID = statusID;

                        if ($scope.apptDetails.Contact.AppointmentStatusDetailID == 0 || $scope.apptDetails.Contact.AppointmentStatusDetailID == null) {

                            appointmentService.addAppointmentStatusDetail(statusObj).then(function (response) {
                                if (response.ResultCode === 0) {
                                    $scope.apptDetails.Contact.AppointmentStatusDetailID = response.ID;
                                    $scope.apptDetails.Contact.AppointmentStatusID = statusID;
                                    alertService.success('Appoinment updated successfully!');
                                } else {
                                    alertService.error('Error while updating appointment status.');
                                }
                            });
                        }
                        else {
                            statusObj.AppointmentStatusDetailID = $scope.apptDetails.Contact.AppointmentStatusDetailID;
                            appointmentService.updateAppointmentStatusDetail(statusObj).then(function (response) {
                                if (response.ResultCode === 0) {
                                    $scope.apptDetails.Contact.AppointmentStatusID = statusID;
                                    alertService.success('Appoinment updated successfully!');
                                } else {
                                    alertService.error('Error while updating an appointment status detail! Please reload the page and try again.');
                                }
                            });
                        }
                    }
                });
            };

            $scope.closeFlyout = function() {
                angular.element('.row-offcanvas').removeClass('active');
            };

            $scope.cancelAppointment = function () {
                $scope.groupScheduleToCancel = {};
                $('#cancelGroupScheduleModel').modal('show');
            }

            $scope.cancelModal = function () {
                $('#cancelGroupScheduleModel').modal('hide');
            }

            $scope.save = function (isNext, mandatory, hasErrors) {
                if (hasErrors) {
                    alertService.error('Please correct the highlighted errors before submitting the form');
                    return false;
                }

                if ($scope.ctrl.cancelGroupScheduleForm.$dirty && !hasErrors) {
                    $scope.groupScheduleToCancel.GroupDetailID = $scope.apptDetails.GroupDetailID;
                    $scope.groupScheduleToCancel.AppointmentID = $scope.apptDetails.AppointmentID;
                    groupSchedulingSearchService.cancelGroupAppointment($scope.groupScheduleToCancel).then(function (response) {
                        if (response.ResultCode === 0) {
                            $scope.apptDetails.IsCancelled = true;
                            var appt = $filter('filter', true)($scope.appointmentData, { ID: $scope.apptDetails.AppointmentID })[0];
                            appt.IsCancelled = true;
                            $('#cancelGroupScheduleModel').modal('hide');
                            resetForm();
                            alertService.success('Group Appointment cancelled successfully');
                        }
                        else {
                            alertService.error('Unable to cancel Group Appointment');
                        }
                    },
                        function (errorStatus) {
                            alertService.error('OOPS! Something went wrong');
                            deferred.reject();
                        },
                        function (notification) {
                            alertService.warning(notification);
                        }
                    ).finally(function () {
                        resetForm();
                    });
                }
            }

            $scope.goToChart = function () {
                $scope.Goto('patientprofile.chart', { ContactID: $scope.apptDetails.ContactID });
            };

            $scope.goToAppt = function () {
                if ($scope.apptDetails.RecurrenceID != null)
                    $scope.showSingleOrRecurringApptModal();
                else
                    $scope.Goto('patientprofile.appointments.editAppointment', { ContactID: $scope.apptDetails.ContactID, AppointmentID: $scope.apptDetails.AppointmentID, IsRecurringAptEdit: 'false' });
            };

            $scope.goToCancelAppt = function () {
                $scope.Goto('patientprofile.appointments.cancelAppointment', { ReadOnly: 'view', ContactID: $scope.apptDetails.ContactID, AppointmentID: $scope.apptDetails.AppointmentID, IsRecurringAptEdit: 'false' });
            };

            $scope.goToGroupNote = function () {
                $scope.Goto('scheduling.groupscheduling.details.groupnote', { GroupID: $scope.apptDetails.GroupID, ReadOnly: 'edit' });
            };

            $scope.goToGroupAppt = function () {
                if ($scope.apptDetails.RecurrenceID != null)
                    $scope.showSingleOrRecurringApptModal();
                else
                    $scope.Goto('scheduling.groupscheduling.details.groupschedule', { GroupID: $scope.apptDetails.GroupID, ReadOnly: 'edit', IsRecurringAptEdit: 'false' });
            };

           var resetForm = function () {
               $rootScope.formReset($scope.ctrl.cancelGroupScheduleForm, $scope.ctrl.cancelGroupScheduleForm.$name);
           };

           var getTime = function (value) {
               var year = '2000';
               var month = '01';
               var day = '01';

               var start = value.toString();
               var len_start = start.length

               var last2 = start.slice(-2);
               var len_last = last2.length;

               var diff = len_start - len_last;

               var first = start.substr(0, diff-1);

               var time = new Date(year, month, day, first, last2);

               return time;
           };
        }
    ]);