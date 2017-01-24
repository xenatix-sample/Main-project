angular.module( 'xenatixApp' )
    .controller( 'appointmentController', ['$scope', '$q', '$filter', 'registrationService', 'appointmentService',
                                           'resourceService', 'alertService', '$stateParams',
                                            'lookupService', '$timeout', 'contactAddressService', 'additionalDemographyService',
                                            'formService', '$rootScope', '$state', 'referralClientInformationService', 'securityAttribute',

        function ( $scope, $q, $filter, registrationService, appointmentService, resourceService, alertService, $stateParams, lookupService, $timeout, contactAddressService,
                                               additionalDemographyService, formService, $rootScope, $state, referralClientInformationService, securityAttribute) {
            var pendingAddUpdates = [];
            $scope.module = securityAttribute.module;
            $scope.feature = securityAttribute.feature;
            $scope.isCancel = false;
            $scope.isLoading = true;
            $scope.ContactID = 0;
            $scope.contactName = '';
            $scope.appointment = {};
            $scope.appointmentContact = {};
            $scope.appointments = [];
            $scope.appointmentResources = [];
            $scope.appointmentTypes = [];
            $scope.locations = [];
            $scope.rooms = [];
            $scope.selectedRoomId = 0;
            $scope.selectedRoomName = '';
            $scope.credentials = [];
            $scope.resources = [];
            $scope.selectedTimeSlots = [];
            $scope.selectedLocation = {};
            $scope.conflicts = [];
            $scope.startTimeAmPm = "";
            $scope.endTimeAmPm = "";
            $scope.externalSources = [];
            $scope.serviceData = [];
            $scope.serviceAllData = '';
            $scope.hideShowRequired = false;
            $scope.checkMH = false;
            $scope.programID = 0;
            $scope.requiredIsTrue = true;
            $scope.Recurrence = '';
            $scope.allAppointmentTypes = [];
            var appointmentExistingResources = [];
            $scope.permissionKey = $state.current.data.permissionKey;
            var locationCounter = 0;
            var selectedTimeSlotIndex = -1;

            $scope.appointment.AppointmentLength = 30;
            $scope.appointment.AppointmentLengthPeriod = "min";
            $scope.appointmentStartDate = new Date();
            $scope.appointment.AppointmentDate = $filter('toMMDDYYYYDate')($scope.appointmentStartDate, 'MM/DD/YYYY', 'useLocal');
            $scope.IsRecurringAptEdit = ($stateParams.IsRecurringAptEdit != null && $stateParams.IsRecurringAptEdit === 'false') ? false : true;
            $scope.appointment.IsRecurringAptEdit = $scope.IsRecurringAptEdit;

            var counter = 0;
            $scope.nonSpecialistProviders = []; //Array containing list of non-specialist provider
            $scope.providers = [];              //list of providers or resources having resourceTypeId = 2  without credential (non-specialist)
            var nonSpecialistIndex = 0;
            $scope.$parent['autoFocus'] = true; //for focusing program

            $scope.opened = false;
            $scope.dobName = 'appointmentDate';
            $scope.dateOptions = {
                formatYear: "yy",
                startingDay: 1,
                showWeeks: false
            };

            var currentDate = new Date();
            currentDate.setHours( 0, 0, 0, 0 );
            var selectedDate = null;

            // Calander Starts
            var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
            var weekDays = ["SUNDAY", "MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY", "SATURDAY"];

            $scope.days = [];
            $scope.hours = [];
            $scope.timeSlots = [];

            var today = new Date();
            $scope.selectedYear = today.getFullYear();
            $scope.selectedMonth = today.getMonth() + 1;
            $scope.currentMonthName = monthNames[$scope.selectedMonth - 1];
            $scope.weekStartDate = today.getDate();
            $scope.weekEndDate = 0;
            $scope.totalMonthDays = getTotalMonthDay( $scope.selectedMonth, $scope.selectedYear );

            if ( $state.current.name == "patientprofile.appointments.cancelAppointment" ) {
                $scope.isCancel = true;
                $scope.preventStopSave = true;
            }

            // In cancel/view state, disable the form and hide the scheduling assistant
            if ($scope.isCancel || ($stateParams.ReadOnly && $stateParams.ReadOnly.toString().toLowerCase() == "view"))
                $scope.isFormDisabled = true;
            else
                $scope.isFormDisabled = false;

            resetForm = function () {
               $rootScope.formReset($scope.ctrl.scheduleForm);
               
            };

            $scope.initCalender = function () {
                if ( $scope.weekStartDate + 6 <= $scope.totalMonthDays )
                    $scope.weekEndDate = $scope.weekStartDate + 6;
                else
                    $scope.weekEndDate = $scope.weekStartDate + ( $scope.totalMonthDays - $scope.weekStartDate );

                generateCalanderForContact();
            }

            $scope.changeCalander = function ( direction, type ) {
                $scope.selectedTimeSlots = [];
                switch ( type ) {
                    case "year":
                        if ( direction == "left" )
                            $scope.selectedYear = $scope.selectedYear - 1;
                        else
                            $scope.selectedYear = $scope.selectedYear + 1;

                        /*************Code to check if week start and end date do not exceed total month days in case of Feb month ***************/
                        if ( $scope.selectedMonth == 2 ) {
                            $scope.totalMonthDays = getTotalMonthDay( $scope.selectedMonth, $scope.selectedYear );

                            if ( $scope.weekEndDate > $scope.totalMonthDays ) {
                                $scope.weekEndDate = $scope.totalMonthDays;
                                $scope.weekStartDate = $scope.weekEndDate - 6;
                            }
                        }

                        break;

                    case "month":
                        if ( direction == "left" ) {
                            if ( ( $scope.selectedMonth - 1 ) >= 1 ) {
                                $scope.selectedMonth = $scope.selectedMonth - 1;
                            }
                            else {
                                $scope.selectedMonth = 12;
                                $scope.selectedYear = $scope.selectedYear - 1;
                            }
                        }
                        else {
                            if ( ( $scope.selectedMonth + 1 ) <= 12 ) {
                                $scope.selectedMonth = $scope.selectedMonth + 1;
                            }
                            else {
                                $scope.selectedMonth = 1;
                                $scope.selectedYear = $scope.selectedYear + 1;
                            }
                        }

                        $scope.totalMonthDays = getTotalMonthDay( $scope.selectedMonth, $scope.selectedYear );
                        if ( $scope.weekEndDate > $scope.totalMonthDays ) {
                            $scope.weekStartDate = $scope.totalMonthDays - 6;
                            $scope.weekEndDate = $scope.totalMonthDays;
                        }

                        break;

                    case "week":
                        if ( direction == "left" ) {
                            if ( ( $scope.weekStartDate - 6 ) > 0 ) {
                                $scope.weekEndDate = $scope.weekStartDate - 1;
                                $scope.weekStartDate = $scope.weekStartDate - 6;
                            }
                            else {
                                if ( $scope.weekStartDate == 1 ) { //code to check if week start date is 1 if yes then set weekStartDate and weekEndDate of previous month .
                                    if ( ( $scope.selectedMonth - 1 ) >= 1 )
                                        $scope.selectedMonth = $scope.selectedMonth - 1;
                                    else {
                                        $scope.selectedMonth = 12;
                                        $scope.selectedYear = $scope.selectedYear - 1;
                                    }
                                    $scope.totalMonthDays = getTotalMonthDay( $scope.selectedMonth, $scope.selectedYear );
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
                            if ( ( $scope.weekEndDate + 6 ) <= $scope.totalMonthDays ) {
                                $scope.weekStartDate = $scope.weekEndDate + 1;
                                $scope.weekEndDate = $scope.weekEndDate + 6
                            }
                            else {
                                //code to check if week end date is the last date of current month if yes then set weekStartDate and weekEndDate of next month accordingly .
                                if ( $scope.weekEndDate == $scope.totalMonthDays ) {
                                    if ( ( $scope.selectedMonth + 1 ) <= 12 )
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
                                    $scope.weekEndDate = $scope.totalMonthDays
                                }
                            }
                        }
                        break;
                    case 'date':
                        if ( $scope.appointment.AppointmentDate != undefined ) {
                            var appointmentDate = new Date( $scope.appointment.AppointmentDate );
                            $scope.selectedMonth = appointmentDate.getMonth() + 1;

                            $scope.selectedYear = appointmentDate.getFullYear();

                            $scope.weekStartDate = appointmentDate.getDate();

                            if ( $scope.weekStartDate > ( $scope.totalMonthDays - 6 ) ) {
                                $scope.weekEndDate = $scope.totalMonthDays;
                            }
                            else {
                                $scope.weekEndDate = $scope.weekStartDate + 6;
                            }
                        }
                        break;
                }

                $scope.totalMonthDays = getTotalMonthDay( $scope.selectedMonth, $scope.selectedYear );
                $scope.currentMonthName = monthNames[$scope.selectedMonth - 1];
                generateCalanderForContact();

                if ( $scope.credentials.length > 0 ) {
                    generateCalenderForCredentials();
                    $scope.bindResourceAvailability( 1 ).then( function () {
                        highlightFirstAvailableTimeSlot();
                    } );
                }

                if ( $scope.selectedRoomId > 0 ) {
                    generateCalenderForRoom();
                    $scope.getResourceAvailability( 1, $scope.selectedRoomId, 3 );
                }
                generateCalenderForNonSpecialistProvider();
                $scope.bindResourceAvailability( 2 ).then( function () {
                    highlightFirstAvailableTimeSlot();
                    $( "div.owl-wrapper" ).css( "transform", "" );
                } );
            };

            function getTotalMonthDay( month, year ) {
                var totalMonthDay = new Date( year, month, 0 ).getDate();
                return totalMonthDay;
            }
            // Calander Ends

            var generateCalanderForContact = function () {
                $scope.days = [];
                $scope.hours = [];
                $scope.timeSlots = [];
                counter = 0;

                for ( var startDayIndex = $scope.weekStartDate; startDayIndex <= $scope.weekEndDate; startDayIndex++ ) {
                    var date = pad( $scope.selectedMonth, 2 ) + "/" + pad( startDayIndex, 2 ) + "/" + $scope.selectedYear;
                    var dayOfWeek = new Date( date ).getDay();

                    for ( var timeSlotIndex = 800; timeSlotIndex < 1800; timeSlotIndex += 100 ) {
                        var timeSlot = timeSlotIndex / 100;
                        if ( timeSlot > 12 )
                            timeSlot = timeSlot - 12;

                        $scope.hours.push( {
                            sNo: counter++,
                            hour: timeSlotIndex == 800 ? timeSlot + ' AM' : timeSlotIndex == 1200 ? timeSlot + ' PM' : timeSlot
                        } );

                        $scope.timeSlots.push( {
                            sNo: counter++,
                            startTime: timeSlotIndex,
                            endTime: timeSlotIndex + 30,
                            date: date,
                            dayOfWeek: dayOfWeek,
                            resourceId: 0,
                            groupId: 0,
                            contactId: $scope.ContactID,
                            status: 'available',
                            type: 0
                        } );

                        $scope.timeSlots.push( {
                            sNo: counter++,
                            startTime: timeSlotIndex + 30,
                            endTime: timeSlotIndex + 100,
                            date: date,
                            dayOfWeek: dayOfWeek,
                            resourceId: 0,
                            groupId: 0,
                            contactId: $scope.ContactID,
                            status: 'available',
                            type: 0
                        } );
                    }

                    $scope.days.push( {
                        calenderDate: date,
                        calenderDay: weekDays[dayOfWeek]
                    } );

                    // prepare time slots for contact
                    angular.forEach( $scope.appointments, function ( appointment ) {
                        angular.forEach( $scope.appointmentResources, function ( appointmentResource ) {
                            if ( appointment.AppointmentID == appointmentResource.AppointmentID && appointmentResource.IsActive == true ) {
                                var appointmentStartTime = appointment.AppointmentStartTime;
                                var appointmentEndTime = calculateAppointmentEndTime( appointment.AppointmentStartTime, appointment.AppointmentLength );

                                var filterTimeSlot = $filter( 'filter' )( $scope.timeSlots, function ( timeSlot ) {
                                    return $filter( 'toMMDDYYYYDate' )( appointment.AppointmentDate ) == timeSlot.date
                                            && calculateTimeDifference( timeSlot.endTime, appointmentStartTime ) >= 0
                                           && calculateTimeDifference( appointmentEndTime, timeSlot.startTime ) >= 0
                                            && timeSlot.contactId == $scope.ContactID;
                                } );
                                if ( filterTimeSlot != null && filterTimeSlot.length > 0 ) {
                                    angular.forEach( filterTimeSlot, function ( updateTimeSlot ) {
                                        if ( ( calculateTimeDifference( updateTimeSlot.endTime, appointmentStartTime ) >= 30 ) && ( calculateTimeDifference( appointmentEndTime, updateTimeSlot.startTime ) >= 30 ) ) {
                                            updateTimeSlot.status = "appointment";
                                        }
                                        else {
                                            updateTimeSlot.status = "available";
                                        }

                                        updateTimeSlot.resourceId = appointmentResource.ResourceID;
                                        updateTimeSlot.appointmentStartTime = appointmentStartTime;
                                        updateTimeSlot.appointmentEndTime = appointmentEndTime;
                                        updateTimeSlot.appointmentId = appointmentResource.AppointmentID;
                                    } );
                                }
                            }
                        } );
                    } );
                }
                $scope.$broadcast( 'getComplete' );
            };

            var generateCalenderForCredentials = function () {
                angular.forEach( $scope.credentials, function ( selectedCredential ) {
                    $scope.generateTimeSlots( selectedCredential.selectedProviderId, selectedCredential.CredentialID, 1 );
                } );
            }

            var generateCalenderForNonSpecialistProvider = function ( nonSpecialist ) {
                if ( nonSpecialist != null ) {
                    $scope.generateTimeSlots( nonSpecialist.selectedProviderId, nonSpecialist.nonSpecialistID, 2 );
                }
                else {
                    angular.forEach( $scope.nonSpecialistProviders, function ( nonSpecialistProvider ) {
                        $scope.generateTimeSlots( nonSpecialistProvider.selectedProviderId, nonSpecialistProvider.nonSpecialistID, 2 );
                    } );
                }
            };

            var generateCalenderForRoom = function () {
                if ( $scope.selectedRoomId > 0 ) {
                    $scope.generateTimeSlots( 0, $scope.selectedRoomId, 3, false );
                }
            }

            $scope.generateTimeSlots = function ( resourceId, groupId, timeSlotType, isHeader ) {
                for ( var startDayIndex = $scope.weekStartDate; startDayIndex <= $scope.weekEndDate; startDayIndex++ ) {
                    var date = pad( $scope.selectedMonth, 2 ) + "/" + pad( startDayIndex, 2 ) + "/" + $scope.selectedYear;
                    var dayOfWeek = new Date( date ).getDay();

                    for ( var timeSlotIndex = 800; timeSlotIndex < 1800; timeSlotIndex += 100 ) {
                        $scope.timeSlots.push( {
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
                        } );

                        $scope.timeSlots.push( {
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
                        } );
                    }
                }
            }

            var setContactID = function () {
                var deferred = $q.defer();
                $scope.ContactID = $stateParams.ContactID;
                if ($state.current.name.toLowerCase().indexOf('referrals.appointment') >= 0) {
                    referralClientInformationService.getReferralClientInformation($stateParams.ReferralHeaderID).then(function (data) {
                        if (data && data.DataItems && data.DataItems.length > 0) {
                            $scope.ContactID = data.DataItems[0].clientDemographicsModel.ContactID;
                            deferred.resolve(-1);
                        }
                    });
                }
                else
                    deferred.resolve(-1);
                return deferred.promise;
            }

            $scope.init = function () {
                setContactID().then(function () {
                $scope.enterKeyStop = true;
                $scope.stopNext = false;
                    $scope.saveOnEnter = $scope.isCancel ? true : !($stateParams.ReadOnly && $stateParams.ReadOnly.toString().toLowerCase() == "view");
                    $scope.appointment.StartAMPM = $filter('toStandardTimeAMPM')(getCurrentTime());
                    $scope.appointment.AMPM = $filter('toStandardTimeAMPM')(getCurrentTime());
                    registrationService.get($scope.ContactID).then(function (data) {
                        if (data != undefined && data.DataItems[0] != undefined)
                        $scope.contactName = data.DataItems[0].FirstName + ' ' + data.DataItems[0].LastName;
                    });

                    additionalDemographyService.getAdditionalDemographic($scope.ContactID).then(function (data) {
                        if (data != undefined && data.DataItems[0] != undefined)
                        $scope.interpreterRequired = data.DataItems[0].InterpreterRequired;
                    });
                    getServiceProgramData();
                    $scope.getLocations().then(function () {
                        $scope.getAppointments().then(function () {
                            if ($stateParams.AppointmentID != undefined) {
                                $scope.appointment.AppointmentID = $stateParams.AppointmentID;
                                $scope.getAppointment($stateParams.AppointmentID);
                            }
                            else {
                                $scope.appointment.AppointmentID = 0;
                            }
                        });
                    });
                getExternalSources();

                // scheduling calander
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
                        },
                        afterAction: function () {
                                if (this.itemsAmount > this.visibleItems.length) {
                                    $('.next').show();
                                    $('.prev').show();

                                    $('.next').removeClass('disabled');
                                    $('.prev').removeClass('disabled');
                                    if (this.currentItem == 0) {
                                        $('.prev').addClass('disabled');
                                }
                                    if (this.currentItem == this.maximumItem) {
                                        $('.next').addClass('disabled');
                                }
                            } else {
                                    $('.next').hide();
                                    $('.prev').hide();
                            }
                        }
                        });

                    //$("#scheduling_assistant").data('owlCarousel').reinit({
                    //    touchDrag: true,
                    //    mouseDrag: true
                    //});

                        if (!$scope.isCancel) {
                            $(document).undelegate('.scheduling_assist td', 'mouseover');
                            $(document).delegate(".scheduling_assist td", "mouseover", function () {
                            var targetIndex, elements;
                                targetIndex = $(this).index() + 1;
                                elements = $(".scheduling_assist td");
                                elements.filter(getCalanderArea(targetIndex)).addClass("col_hover");
                            });

                            $(document).undelegate('.scheduling_assist td', 'mouseleave');
                            $(document).delegate('.scheduling_assist td', 'mouseleave', function () {
                                $(".scheduling_assist td").removeClass("col_hover");
                            });

                            $(document).undelegate('.scheduling_assist td', 'click');
                            $(document).delegate('.scheduling_assist td', 'click', function () {
                            var targetIndex;
                                targetIndex = $(this).index() + 1;
                                highlightCalander(targetIndex);
                            });
                    } else {
                            $(document).undelegate('.scheduling_assist td', 'click');
                            $(document).undelegate('.scheduling_assist td', 'mouseleave');
                            $(document).undelegate('.scheduling_assist td', 'mouseover');
                            $(".scheduling_assist td").removeClass("col_hover");
                    }
                    });

                // Service dropdwons- single update calender.
                })
            };

            var getCalanderArea = function ( index ) {
                var appointmentLength = $scope.appointment.AppointmentLengthPeriod == 'min' ? $scope.appointment.AppointmentLength : $filter( 'tomin' )( $scope.appointment.AppointmentLength );

                appointmentLength = parseInt( appointmentLength );

                if ( isNaN( appointmentLength ) ) {
                    appointmentLength = 30;
                }

                var noOfSlot = parseInt( appointmentLength / 30 );
                if ( ( parseInt( appointmentLength ) % 30 ) > 0 )
                    noOfSlot = noOfSlot + 1;
                var selector = ":nth-child(n+" + index + "):nth-child(-n+" + ( index + ( noOfSlot - 1 ) ) + ")";
                return selector;
            };

            var highlightCalander = function ( index ) {
                if (!$scope.validateAppointmentLength())
                    return;
                $scope.validateStartAppointmentTime();
                $scope.validateAppointmentTime();
                var elements = $( ".scheduling_assist td" );
                var selector = getCalanderArea( index );

                if ( selectedTimeSlotIndex !== index ) {
                    elements.removeClass( "col_select" );
                }
                if ($scope.appointment.AppointmentEndTime !== undefined) {
                    elements.filter(selector).addClass("col_select");
                }
                elements.not( selector ).removeClass( "col_select" );
                selectedTimeSlotIndex = index;
            };

            $scope.getLookupsByType = function ( typeName ) {
                return lookupService.getLookupsByType( typeName );
            };

            $scope.getAddress = function () {
                var q = $q.defer();
                $scope.isLoading = true;
                return contactAddressService.get($scope.ContactID).then(function (data) {
                    if ( data.DataItems.length > 0 ) {
                        $scope.Address = data.DataItems;
                        angular.forEach( $scope.Address, function ( param ) {
                            $scope.locations.push( {
                                ID: locationCounter++,
                                LocationID: param.AddressID,
                                Name: param.Line1 + " " + param.Line2,
                                type: 4       //'Resource Type :'External' ID:4'
                            } );
                        } )
                    }
                    $scope.isLoading = false;
                    q.resolve();
                },
                 function ( errorStatus ) {
                     $scope.isLoading = false;
                     alertService.error( 'Unable to connect to server ' );
                     q.reject();
                 } );
                return q.promise;
            };

            $scope.getFacility = function ( typeName ) {
                $scope.facilityData = lookupService.getLookupsByType( 'Facility' );
                angular.forEach( $scope.facilityData, function ( param ) {
                    $scope.locations.push( {
                        ID: locationCounter++,
                        LocationID: param.ID,
                        Name: param.Name,
                        type: 1
                    } );
                } )
            };

            $scope.getLocations = function () {
                var q = $q.defer();
                $scope.getAddress().then( function () {
                    $scope.getFacility();
                    q.resolve();
                    resetForm();
                } );
                return q.promise;
            }

            $scope.populateRooms = function () {
                var location = $filter( 'filter' )( $scope.locations, { ID: $scope.selectedLocation.ID }, true )[0];
                $scope.selectedLocation.type = location != null ? location.type : 0;
                $scope.selectedLocation.LocationID = location != null ? location.LocationID : 0;

                if ( location != null && location.type == 1 ) {
                    $scope.appointment.FacilityID = location.LocationID;
                    $scope.getRooms();
                }
                else {
                    $scope.timeSlots = $filter( 'filter' )( $scope.timeSlots, function ( item ) {
                        return item.type != 3;
                    } );
                    $scope.selectedRoomId = 0;

                    $scope.rooms = [];
                    $scope.appointment.FacilityID = null;
                }

                highlightFirstAvailableTimeSlot();
            }

            $scope.getAppointments = function () {
                $scope.isLoading = true;
                return appointmentService.getAppointmentByResource( $scope.ContactID, 7 ).then( function ( data ) {
                    if (data.DataItems.length > 0) {
                        $scope.appointments = data.DataItems;
                        if ($state.includes('callcenter') || $state.includes('referrals'))
                            $stateParams.AppointmentID = $scope.appointments[0].AppointmentID;                        
                     
                        $scope.isLoading = false;
                    }
                    return $scope.getAppointmentResourceByContact();
                },
                function ( errorStatus ) {
                    $scope.isLoading = false;
                    alertService.error( 'Unable to connect to server ' );
                } );
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

            $scope.getAppointment = function ( appointmentID ) {
                $scope.isLoading = true;
                appointmentService.getAppointment( $scope.ContactID, appointmentID ).then( function ( data ) {
                    $scope.appointment = data.DataItems[0];
                    if ( data.DataItems[0].Recurrence != null ) {
                        $scope.Recurrence = data.DataItems[0].Recurrence;
                        $scope.Recurrence.RecurrenceID = ( data.DataItems.length > 0 && data.DataItems[0].RecurrenceID != null ) ? data.DataItems[0].RecurrenceID : 0;
                        $scope.Recurrence.StartDate = $filter( 'toMMDDYYYYDate' )( $scope.Recurrence.StartDate, 'MM/DD/YYYY' );
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

                    $scope.appointment.AppointmentStartTime = pad($scope.appointment.AppointmentStartTime, 4);
                    $scope.filterServiceData( $scope.appointment.ProgramID );
                    $scope.adjustAppointmentPeriod();
                    $scope.appointment.AppointmentEndTime = $scope.adjustAppointmentEndTime();
                    $scope.appointment.StartAMPM = $filter( 'toStandardTimeAMPM' )( $scope.appointment.AppointmentStartTime );
                    $scope.appointment.AMPM = $filter( 'toStandardTimeAMPM' )( $scope.appointment.AppointmentStartTime );
                    if ( $scope.appointment.AppointmentLength == 0 ) {
                        $scope.appointment.AppointmentLength = 30;
                        $scope.appointment.AppointmentLengthPeriod = "min";
                    }

                    $scope.appointment.AppointmentDate = $filter('toMMDDYYYYDate')($scope.appointment.AppointmentDate);
                    $scope.appointment.IsRecurringAptEdit = $scope.IsRecurringAptEdit;
                    var appointmentDate = moment.utc( $scope.appointment.AppointmentDate );

                    $scope.selectedYear = appointmentDate.year();
                    $scope.selectedMonth = appointmentDate.month() + 1;

                    $scope.currentMonthName = monthNames[$scope.selectedMonth - 1];
                    $scope.weekStartDate = appointmentDate.date();
                    $scope.totalMonthDays = getTotalMonthDay( $scope.selectedMonth, $scope.selectedYear );

                    $scope.timeSlots = [];
                    $scope.initCalender();
                    $scope.getAppointmentResource( $scope.appointment.AppointmentID ).then( function () {
                        $scope.getAppointmentType();
                        $scope.processByAppointmentType($scope.appointment.AppointmentTypeID);
                        $scope.isLoading = false;
                        resetForm();
                    } );
                },
                function ( errorStatus ) {
                    $scope.isLoading = false;
                    alertService.error( 'Unable to connect to server ' );
                } );
            };

            $scope.getAppointmentResource = function ( appointmentID ) {
                var q = $q.defer();
                $scope.isLoading = true;
                appointmentService.getAppointmentResource( appointmentID ).then( function ( data ) {
                    $scope.appointmentResources = $filter( 'filter' )( data.DataItems, { IsActive: true } );
                    appointmentExistingResources = $filter( 'filter' )( data.DataItems, { IsActive: true } );
                    var resource = $filter( 'filter' )( $scope.appointmentResources, { ResourceTypeID: 1, AppointmentID: appointmentID }, true )[0];
                    if ( resource != null && resource != undefined ) {
                        $scope.getRooms().then( function () {
                            var location = $filter( 'filter' )( $scope.locations, function ( item ) {
                                return item.LocationID == $scope.appointment.FacilityID && item.type == 1
                            }, true )[0];
                            if ( location )
                                $scope.selectedLocation = location;

                            $scope.selectedRoomId = resource.ResourceID;
                            var filteredRooms = $filter( 'filter' )( $scope.rooms, { RoomID: $scope.selectedRoomId }, true )[0];
                            $scope.selectedRoomName = filteredRooms.RoomName;
                        } );
                    } else {
                        var resource = $filter( 'filter' )( $scope.appointmentResources, { ResourceTypeID: 4, AppointmentID: appointmentID }, true )[0];
                        if ( resource != null && resource != undefined ) {
                            var location = $filter( 'filter' )( $scope.locations, function ( item ) {
                                return item.LocationID == resource.ResourceID && item.type == 4
                            }, true )[0];
                            if ( location )
                                $scope.selectedLocation = location;
                        }
                    }

                    $scope.initCalender();
                    $scope.isLoading = false;
                    q.resolve();
                },
                function ( errorStatus ) {
                    $scope.isLoading = false;
                    alertService.error( 'Unable to connect to server ' );
                } );

                return q.promise;
            };

            $scope.getAppointmentResourceByContact = function () {
                $scope.isLoading = true;

                return appointmentService.getAppointmentByResource( $scope.ContactID, 7 ).then( function ( data ) {
                    $scope.appointmentResources = data.DataItems;
                    $scope.isLoading = false;
                    if ( $scope.appointment.AppointmentID == undefined || $scope.appointment.AppointmentID == 0 )
                        $scope.initCalender();

                    resetForm();
                },
                 function ( errorStatus ) {
                     $scope.isLoading = false;
                     alertService.error( 'Unable to connect to server ' );
                 } );
            };

            $scope.getAppointmentByResource = function (resourceId, resourceTypeId) {
                var q = $q.defer();
                q.resolve(appointmentService.getAppointmentByResource(resourceId, resourceTypeId));
                return q.promise;
            };
            
            //the following function collects the data from lookupservice -kind of select * from
            var getServiceProgramData = function () {
                $scope.programs = $filter('securityFilter')(lookupService.getOrganizationByDataKey('ProgramUnit'), 'ProgramUnit', 'OrganizationDetailID', SchedulingPermissionKey.Scheduling_Appointment_SingleAppointment);
                $scope.serviceAllData = lookupService.getLookupsByType( 'Service' );
                $scope.allAppointmentTypes = lookupService.getLookupsByType( 'AppointmentTypes' );
                $scope.appointmentMapping = lookupService.getLookupsByType( 'AppointmentMapping' );
            }

            // filters based on the selection from program drop down.
            $scope.filterServiceData = function ( id ) {
                $scope.isNonMHMR = false;
                $scope.selectedValue = id;
                $scope.programID = id;

                $scope.requiredIsTrue = true;
                if ( id === 2 ) { //MH Data
                    $scope.hideShowRequired = true;
                }

                $scope.getAppoinmentDetails();
            }
            //buffer call - may be needed - if req chnage.
            $scope.filterServiceStatusData = function ( id ) {
                // $scope.appointment.ServicesID = id;
            }

            //The following call will filter the data based on program drop down selection.
            $scope.getAppoinmentDetails = function () {
                $scope.appointmentTypes = [];
                for ( var i = 0; i < $scope.allAppointmentTypes.length; i++ ) {
                    if ( $scope.allAppointmentTypes[i].ProgramID === $scope.programID ) {
                        $scope.appointmentTypes.push( $scope.allAppointmentTypes[i] );
                    }
                }
                if ( $scope.appointment.AppointmentTypeID == APPOINTMENT_TYPE.NonMHMRAppointment ) {
                    $scope.isNonMHMR = true;
                }
            }

            $scope.getAppointmentType = function () {
                //code to remove existing credential providers from UI in case of when no AppointmentTypes found
                if ( $scope.appointmentTypes.length == 0 || $scope.appointmentTypes[0].AppointmentTypeID == undefined ) {
                    $scope.credentials = [];
                    $scope.timeSlots = $filter( 'filter' )( $scope.timeSlots, function ( timeSlot ) {
                        return timeSlot.type != 1;
                    } );
                }
            };

            //
            $scope.getRooms = function () {
                return resourceService.getRooms( $scope.appointment.FacilityID ).then( function ( data ) {
                    $scope.rooms = data.DataItems;
                    if ( data.DataItems.length == 0 ) {
                        alertService.error( 'No Rooms available' );
                    }
                },
                function ( errorStatus ) {
                    $scope.isLoading = false;
                    alertService.error( 'Unable to connect to server' );
                } );
            };

            $scope.adjustAppointmentPeriod = function () {
                var appointmentLength = $scope.appointment.AppointmentLengthPeriod == 'hour' ? $filter( 'tomin' )( $scope.appointment.AppointmentLength ) : $scope.appointment.AppointmentLength;
                if ( isNaN( appointmentLength ) ) {
                    appointmentLength = 30;
                }

                if ( appointmentLength % 60 == 0 ) {
                    // show as hours
                    $scope.appointment.AppointmentLengthPeriod = "hour";
                    $scope.appointment.AppointmentLength = $filter( 'tohour' )( $scope.appointment.AppointmentLength );
                }
                else {
                    $scope.appointment.AppointmentLengthPeriod = "min";
                    $scope.appointment.AppointmentLength = $scope.appointment.AppointmentLength;
                }
            }

            $scope.adjustAppointmentEndTime = function () {
                var appointmentLength = $scope.appointment.AppointmentLengthPeriod == 'hour' ? $scope.appointment.AppointmentLength * 60 : $scope.appointment.AppointmentLength;

                if ( $scope.appointment.AppointmentLengthPeriod == 'hour' ) {
                    appointmentLength = $filter( 'tomin' )( $scope.appointment.AppointmentLength );
                } else {
                    appointmentLength = $scope.appointment.AppointmentLength;
                }

                if ( isNaN( appointmentLength ) ) {
                    appointmentLength = 30;
                }
                $scope.validateAppointmentTime();
                return calculateAppointmentEndTime($scope.appointment.AppointmentStartTime, appointmentLength, $scope.appointment.StartAMPM);
            }

            var calculateAppointmentEndTime = function (startTime, length, startampm) {
                //ToDo: Pass the date and use that to determine the logic
                if (startTime !== undefined && startTime !== null) {
                    if (startampm !== undefined)
                        startTime = $filter('toMilitaryTime')(startTime, startampm);
                    var standardTime = moment( pad( startTime, 4 ), 'HH:mm' ).add( length, 'minute' ).format( 'HH:mm' );
                    var ampm = $filter('toStandardTimeAMPM')(standardTime);
                    if (startampm !== undefined)
                        $scope.appointment.AMPM = ampm;
                    return $filter('toMilitaryTime')(standardTime, ampm);
                }
            }

            var calculateTimeDifference = function ( startTime, endTime ) {
                return moment.duration( moment( pad( startTime, 4 ), "HH:mm" ).diff( moment( pad( endTime, 4 ), "HH:mm" ) ) ).asMinutes();
            };

            $scope.getAppointmentLength = function () {
                if ( $scope.appointment.AppointmentTypeID != undefined )
                    return appointmentService.getAppointmentLength( $scope.appointment.AppointmentTypeID ).then( function ( data ) {
                        if ( data.DataItems.length > 0 ) {
                            $scope.appointment.AppointmentLength = data.DataItems[0].AppointmentLength;
                            $scope.adjustAppointmentPeriod();
                        }
                    },
                    function ( errorStatus ) {
                        $scope.isLoading = false;
                        alertService.error( 'Unable to connect to server ' );
                    } );
            };

            $scope.getCredentialByAppointmentType = function () {
                if ($scope.appointment.AppointmentTypeID == undefined) {
                    $scope.credentials = [];
                    return $q.all([]);
                }

                return resourceService.getCredentialByAppointmentType($scope.appointment.AppointmentTypeID).then(function (data) {
                    angular.forEach(data.DataItems, function (item) {
                        if (item.Providers.length > 0)
                            $scope.credentials.push(item);
                    })
                    // reset time slots when appointment type is changed
                    $scope.timeSlots = $filter( 'filter' )( $scope.timeSlots, function ( timeSlot ) {
                        return timeSlot.type != 1;
                    } );

                    var defer = $q.defer();
                    var promises = [];

                    if ( $stateParams.AppointmentID != undefined ) {
                        var selectedResources = $filter( 'filter' )( $scope.appointmentResources, function ( item ) {
                            return item.AppointmentID == $scope.appointment.AppointmentID && item.ResourceTypeID == 2
                        } );

                        var credentials = [];
                        angular.forEach( $scope.credentials, function ( credential ) {
                            var isExists = $filter( 'filter' )( selectedResources, function ( resource ) {
                                return resource.ParentID == credential.CredentialID;
                            } ).length > 0;

                            if ( isExists ) {
                                credentials.push( credential );
                            }
                        } );

                        $scope.credentials = credentials;
                    }

                    angular.forEach( $scope.credentials, function ( credential ) {
                        if ( credential != null && credential.Providers.length > 0 ) {
                            credential.selectedProviderId = credential.Providers[0].ProviderId;
                            promises.push( $scope.getResourceAvailability( 2, credential.CredentialID, 1 ) );
                        }
                    } );

                    generateCalenderForCredentials();
                    bindSelectedResources();
                    return $q.all( promises );
                },
                function ( errorStatus ) {
                    $scope.isLoading = false;
                    alertService.error( 'Unable to connect to server' );
                } );
            };

            //Render selected appointment data
            function bindSelectedResources() {
                if ( $scope.appointment.AppointmentID != null && $scope.appointment.AppointmentID != undefined ) {
                    var facility = $filter( 'filter' )( $scope.appointmentResources, function ( item ) {
                        return item.ResourceTypeID == 1 && item.AppointmentID == $scope.appointment.AppointmentID;
                    } )[0];

                    if ( $scope.appointment.FacilityID ) {
                        var location = $filter( 'filter' )( $scope.locations, function ( location ) {
                            return location.type == 1 && location.LocationID == $scope.appointment.FacilityID;
                        } )[0];

                        $scope.selectedLocation = { ID: location.ID, LocationID: location.LocationID, type: 1 };
                        $scope.getRooms().then( function () {
                            $scope.selectedRoomId = facility ? facility.ResourceID : '';
                            var filteredRooms = $filter( 'filter' )( $scope.rooms, function ( room ) { return room.RoomID == $scope.selectedRoomId } );
                            if ( filteredRooms.length > 0 ) {
                                $scope.selectedRoomName = filteredRooms[0].RoomName;
                                generateCalenderForRoom();

                                $scope.getResourceAvailability( 1, $scope.selectedRoomId, 3 ).then( function () {
                                    highlightFirstAvailableTimeSlot();
                                } );
                            }
                            else {
                                $scope.selectedRoomId = 0;
                            }
                        } );
                    } else {
                        var address = $filter( 'filter' )( $scope.appointmentResources, function ( item ) {
                            return item.AppointmentID == $scope.appointment.AppointmentID && item.ResourceTypeID == 4
                        } )[0];
                        if ( address != null ) {
                            var location = $filter( 'filter' )( $scope.locations, function ( location ) {
                                return location.type == 4 && location.LocationID == address.ResourceID;
                            } )[0];
                            $scope.selectedLocation = location ? { ID: location.ID, LocationID: location.LocationID, type: 4 } : '';
                        }
                    }

                    var selectedResources = $filter( 'filter' )( $scope.appointmentResources, function ( item ) {
                        return item.AppointmentID == $scope.appointment.AppointmentID && item.ResourceTypeID == 2
                    } );

                    angular.forEach( selectedResources, function ( res ) {
                        if ( res.ResourceID > 0 ) {
                            var preProvider = $filter( 'filter' )( $scope.credentials, function ( credential ) {
                                return credential.CredentialID == res.ParentID;
                            } )[0];

                            if ( preProvider == null || preProvider == undefined ) {
                                $scope.addResource( res.ResourceID );
                            } else {
                                preProvider.selectedProviderId = res.ResourceID;
                            }
                        }
                    } );
                }
            }

            $scope.getServiceDataBasedOnAppointmentTypeId = function ( appointmentTypeId ) {
                $scope.serviceData = [];

                for ( var i = 0; i < $scope.appointmentMapping.length; i++ ) {
                    if ( $scope.appointmentMapping[i].AppointmentTypeID == appointmentTypeId ) {
                        $scope.serviceData.push( $scope.appointmentMapping[i] );
                    }
                }
            }

            $scope.processByAppointmentType = function (appointmentTypeId) {
                // here we need to process all actions related to setting the Appointment Type
                $scope.nonSpecialistProviders = [];

                $scope.credentials = [];

                $scope.getServiceDataBasedOnAppointmentTypeId(appointmentTypeId);

                $scope.getCredentialByAppointmentType().then( function () {
                    highlightFirstAvailableTimeSlot();
                } );
                $scope.getAppointmentLength();

                if ( $scope.appointment.AppointmentTypeID == APPOINTMENT_TYPE.NonMHMRAppointment ) {
                    $scope.isNonMHMR = true;

                    $scope.appointment.FacilityID = null;

                    //remove resource if nonMHMR is selected
                    $scope.timeSlots = $filter( 'filter' )( $scope.timeSlots, function ( item ) {
                        return item.groupId == 0 && item.type == 0;
                    } );
                }
                else {
                    $scope.isNonMHMR = false;
                }
            };

            $scope.adjustAppointmentLength = function ( period ) {
                if ( period == "min" )    // we are switching the appointment time to minutes
                    $scope.appointment.AppointmentLength = $filter( 'tomin' )( $scope.appointment.AppointmentLength );
                else   // we are switching the appointment time to hours
                    $scope.appointment.AppointmentLength = $filter( 'tohour' )( $scope.appointment.AppointmentLength );

                $scope.appointment.AppointmentLengthPeriod = period;
            };

            $scope.bindResourceAvailability = function ( timeSlotType ) {
                var defer = $q.defer();
                var promises = [];
                var count = 0;

                if ( timeSlotType == 1 ) {
                    angular.forEach( $scope.credentials, function ( credential ) {
                        if ( credential.selectedProviderId != null && credential.selectedProviderId > 0 ) {
                            promises.push( $scope.getResourceAvailability( 2, credential.CredentialID, 1 ) );
                        }
                    } );
                }
                else if ( timeSlotType == 2 ) {
                    angular.forEach( $scope.nonSpecialistProviders, function ( nonSpecialistProvider ) {
                        if ( nonSpecialistProvider.selectedProviderId != null && nonSpecialistProvider.selectedProviderId > 0 ) {
                            promises.push( $scope.getResourceAvailability( 2, nonSpecialistProvider.nonSpecialistID, 2 ) );
                        }
                    } );
                }

                return $q.all( promises );
            }

            $scope.getResourceAvailability = function (resourceTypeID, identifier, timeSlotType) {
                var resourceId = 0;
                if ( timeSlotType == 1 ) {
                    var selectedCredential = $filter( 'filter' )( $scope.credentials, function ( item ) { return item.CredentialID == identifier && item.selectedProviderId != null; } );

                    if ( selectedCredential.length == 0 )
                        return $scope.promiseNoOp();
                    resourceId = selectedCredential[0].selectedProviderId;
                }
                else if ( timeSlotType == 2 ) {
                    var selectedCredential = $filter( 'filter' )( $scope.nonSpecialistProviders, function ( item ) { return item.nonSpecialistID == identifier && item.selectedProviderId != null; } );
                    if ( selectedCredential.length == 0 )
                        return $scope.promiseNoOp();

                    resourceId = selectedCredential[0].selectedProviderId;
                }
                else if ( timeSlotType == 3 ) {
                    resourceId = identifier;
                }

                var filteredResource = $filter( 'filter' )( $scope.resources, function ( item ) {
                    return item.ResourceID == resourceId && item.ResourceTypeID == resourceTypeID;
                } );

                if ( filteredResource.length > 0 ) {
                    return processTimeSlots( resourceId, resourceTypeID, identifier, timeSlotType ).then(function() {
                        $timeout(function () {
                            $scope.getAppointmentStatusOnProviderChange();
                        }, 0);
                    });
                }
                else {
                    // Get all resource availabilities of current provider selected by user
                    return resourceService.getResourceDetails( resourceId, resourceTypeID ).then( function ( data ) {
                        Array.prototype.push.apply( $scope.resources, data.DataItems );

                        return processTimeSlots( resourceId, resourceTypeID, identifier, timeSlotType ).then(function() {
                            $timeout(function () {
                                $scope.getAppointmentStatusOnProviderChange();
                            }, 0);
                        });
                    },
                    function ( errorStatus ) {
                        $scope.isLoading = false;
                        alertService.error( 'Unable to connect to server' );
                    } );
                }
            }

            var processTimeSlots = function ( resourceId, resourceTypeID, identifier, timeSlotType ) {
                //Get all the provider appointments for the selected provider or resource
                $scope.isLoading = true;
                return $scope.getAppointmentByResource(resourceId, resourceTypeID).then(function (response) {
                    $scope.isLoading = false;
                    var providerAppointments = [];
                    if (response && 'DataItems' in response) {
                        providerAppointments = response.DataItems;
                    }
                    //filter appointments of current provider selected based on start calender date and  end calender date
                    var filteredAppointments = $filter( 'filter' )( providerAppointments, function ( item ) {
                        var dateOfAppointment = moment.utc( item.AppointmentDate );
                        var appointmentDate = dateOfAppointment.date();
                        if ( dateOfAppointment.year() == $scope.selectedYear && dateOfAppointment.month() + 1 == $scope.selectedMonth ) {
                            if ( appointmentDate >= $scope.weekStartDate && appointmentDate <= $scope.weekEndDate ) {
                                return item;
                            }
                        }
                    } );

                    //filter timeslots for selected provider based on identifier
                    var filteredTimeSlots = $filter( 'filter' )( $scope.timeSlots, function ( item ) {
                        return item.groupId == identifier && item.type == timeSlotType;
                    } );

                    angular.forEach( filteredTimeSlots, function ( timeSlot ) {
                        var filteredResourceAppointments = $filter( 'filter' )( filteredAppointments, function ( resourceAppointment ) {
                            var appointmentStartTime = resourceAppointment.AppointmentStartTime;
                            var appointmentEndTime = calculateAppointmentEndTime( resourceAppointment.AppointmentStartTime, resourceAppointment.AppointmentLength );

                            return ( $filter( 'toMMDDYYYYDate' )( resourceAppointment.AppointmentDate ) == timeSlot.date
                                    && calculateTimeDifference( timeSlot.endTime, appointmentStartTime ) >= 0
                                    && calculateTimeDifference( appointmentEndTime, timeSlot.startTime ) >= 0 );
                        } );

                        timeSlot.resourceId = resourceId;
                        //code to check if current appointment in filteredResourceAppointments is for current contact

                        if ( filteredResourceAppointments.length > 0 ) {
                            // appontment exists in current time slot
                            var appointment = filteredResourceAppointments[0];
                            var appointmentStartTime = appointment.AppointmentStartTime;
                            var appointmentEndTime = calculateAppointmentEndTime( appointment.AppointmentStartTime, appointment.AppointmentLength );

                            if ( ( calculateTimeDifference( timeSlot.endTime, appointmentStartTime ) >= 30 ) && ( calculateTimeDifference( appointmentEndTime, timeSlot.startTime ) >= 30 && !filteredResourceAppointments[0].IsCancelled ) ) {
                                if ( filteredResourceAppointments[0].AppointmentTypeID == 3 ) {
                                    timeSlot.status = "blocked";
                                }
                                else {
                                    timeSlot.status = "appointment";
                                }
                            }
                            else {
                                timeSlot.status = "available";
                            }

                            timeSlot.appointmentStartTime = appointmentStartTime;
                            timeSlot.appointmentEndTime = appointmentEndTime;
                            timeSlot.appointmentId = appointment.AppointmentID;
                        }
                        else {
                            timeSlot.appointmentStartTime = null;
                            timeSlot.appointmentEndTime = null;
                            timeSlot.appointmentId = null;
                            timeSlot.appointment = null;

                            var timeSlotDayOfWeek = weekDays[timeSlot.dayOfWeek];
                            var resourceAvailabilities, resourcesOverrides;

                            var resource = $filter( 'filter' )( $scope.resources, function ( resourceAvailability ) {
                                return resourceAvailability.ResourceID == resourceId && resourceAvailability.ResourceTypeID == resourceTypeID;
                            } );

                            if ( resource.length > 0 ) {
                                resourceAvailabilities = resource[0].ResourceAvailabilities;
                                resourcesOverrides = resource[0].ResourceOverrides;
                            }

                            // resource availability
                            var isResourceAvailable = $filter( 'filter' )( resourceAvailabilities, function ( resourceAvailability ) {
                                return resourceAvailability.Days.toUpperCase() == timeSlotDayOfWeek
                                && ( timeSlot.startTime >= resourceAvailability.AvailabilityStartTime
                                    && timeSlot.endTime <= resourceAvailability.AvailabilityEndTime );
                            } ).length > 0 ? true : false;

                            // resource override
                            var hasResourceOverride = $filter( 'filter' )( resourcesOverrides, function ( override ) {
                                return override.ResourceID == resourceId
                                    && override.ResourceTypeID == resourceTypeID
                                    && $filter( 'toMMDDYYYYDate' )( override.OverrideDate ) == timeSlot.date
                            } ).length > 0 ? true : false;

                            if ( isResourceAvailable && !hasResourceOverride ) {
                                timeSlot.status = "available";
                            }
                            else {
                                timeSlot.status = "blocked";
                            }
                        }
                    });
                } );
            };

            var setTimeSlot = function ( timeSlot, flag ) {
                if (flag) {
                    $scope.appointment.AppointmentStartTime = pad(timeSlot.startTime, 4);
                    $scope.appointment.AppointmentEndTime = $scope.adjustAppointmentEndTime();
                    $scope.appointment.AppointmentDate = timeSlot.date;
                    $scope.appointment.StartAMPM = $filter('toStandardTimeAMPM')(timeSlot.startTime);
                    $scope.appointment.AMPM = $filter('toStandardTimeAMPM')(timeSlot.endTime);
                }
                

                //ToDo: This is an experiment....test all scenarios before checking this in
               
                $scope.selectedTimeSlots = $filter( 'filter' )( $scope.timeSlots, function ( item ) {
                    return item.startTime == timeSlot.startTime && item.endTime == timeSlot.endTime && item.date == timeSlot.date;
                } );
            }

            $scope.getAppointmentStatusOnProviderChange = function() {
                // conflicts & suggested appointment date
                $timeout(function() {
                    $scope.conflicts = [];
                    var conflictTimeSlot = $filter('filter')($scope.selectedTimeSlots, function (conflict) {
                        return conflict.appointmentStartTime != undefined
                            && conflict.appointmentEndTime != undefined
                            && conflict.resourceId != undefined
                            && conflict.resourceId !== 0
                            && conflict.appointmentId !== $scope.appointment.AppointmentID
                            && conflict.status != 'available' // this is for cancelled appointments, they are there, but ready available
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
                });
            };

            $scope.getAppointmentStatus = function ( timeSlot, flag ) {
                setTimeSlot( timeSlot, flag );

                // conflicts & suggested appointment date
                $scope.conflicts = [];

                var conflictTimeSlot = $filter('filter')($scope.selectedTimeSlots, function (conflict) {
                    return conflict.appointmentStartTime != undefined
                        && conflict.appointmentEndTime != undefined
                        && conflict.resourceId != undefined
                        && conflict.resourceId != 0
                        && conflict.appointmentId != $scope.appointment.AppointmentID
                        && conflict.status != 'available' // this is for cancelled appointments, they are there, but ready available
                        && ( ( calculateTimeDifference( conflict.endTime, conflict.appointmentStartTime ) > 0 && calculateTimeDifference( conflict.endTime, conflict.appointmentStartTime ) <= 30 ) || ( calculateTimeDifference( conflict.appointmentEndTime, conflict.startTime ) > 0 && calculateTimeDifference( conflict.appointmentEndTime, conflict.startTime ) <= 30 ) );
                } );

                angular.forEach( conflictTimeSlot, function ( conflict ) {
                    var resourceName = '';
                    var resourceTypeId = getResourceType( conflict.type );

                    resourceService.getResourceById( conflict.resourceId, resourceTypeId ).then( function ( response ) {
                        if ( response.DataItems.length > 0 ) {
                            resourceName = response.DataItems[0].ResourceName;
                            $scope.conflicts.push( { resourceId: conflict.resourceId, resourceTypeId: conflict.type, resourceName: resourceName, suggestedAppointmentTime: '' } );
                        }
                    } );
                } );
            };

            var getResourceType = function ( timeSlotType ) {
                switch ( timeSlotType ) {
                    case 1:
                        return 2;
                    case 2:
                        return 2;
                    case 3:
                        return 1;
                    default:
                        break;
                }
            }

            $scope.removeResource = function ( identifier, type ) {
                var resourceId = 0, resourceTypeID = 2;

                //remove resource from appointmentResources
                switch ( type ) {
                    case 1:
                        var selectedCredential = $filter( 'filter' )( $scope.credentials, function ( item ) { return item.CredentialID == identifier && item.selectedProviderId != null; } );

                        $scope.credentials = $filter( 'filter' )( $scope.credentials, function ( item ) {
                            return item.CredentialID != identifier
                        } );

                        $scope.timeSlots = $filter( 'filter' )( $scope.timeSlots, function ( timeslot ) {
                            return timeslot.groupId != identifier;
                        } );

                        $scope.selectedTimeSlots = $filter( 'filter' )( $scope.selectedTimeSlots, function ( item ) {
                            return item.groupId != identifier;;
                        } );

                        $scope.ctrl.scheduleForm.modified = true;
                        if ( selectedCredential.length == 0 )
                            return;
                        resourceId = selectedCredential[0].selectedProviderId;
                        break;
                    case 2:

                        $scope.nonSpecialistProviders = $filter( 'filter' )( $scope.nonSpecialistProviders, function ( item ) {
                            return item.nonSpecialistID != identifier
                        } );

                        $scope.providers = $filter( 'filter' )( $scope.providers, function ( item ) {
                            return item.nonSpecialistID != identifier
                        } );

                        $scope.timeSlots = $filter( 'filter' )( $scope.timeSlots, function ( timeslot ) {
                            return timeslot.groupId != identifier;
                        } );

                        $scope.selectedTimeSlots = $filter( 'filter' )( $scope.selectedTimeSlots, function ( item ) {
                            return item.groupId != identifier;;
                        } );

                        var selectedCredential = $filter( 'filter' )( $scope.nonSpecialistProviders, function ( item ) { return item.nonSpecialistID == identifier && item.selectedProviderId != null; } );
                        $scope.ctrl.scheduleForm.modified = true;
                        if ( selectedCredential.length == 0 )
                            return;

                        resourceId = selectedCredential[0].selectedProviderId;

                        break;

                    default:
                        break;
                }

                var filteredResource = $filter( 'filter' )( $scope.appointmentResources, function ( item ) {
                    return !( item.ResourceID == resourceId && item.ResourceTypeID == resourceTypeID );
                } );

                var filteredTimeSlot = $filter( 'filter' )( $scope.timeSlots, function ( item ) {
                    return !( item.resourceId == resourceId && item.ResourceTypeID == 2 );
                } );

                $scope.appointmentResources = filteredResource;
                $scope.timeSlots = filteredTimeSlot;

                highlightFirstAvailableTimeSlot();
            };

            $scope.saveAppointment = function ( isUpdate ) {
                $scope.appointment.Recurrence = $scope.Recurrence;
                if ($scope.appointment != null && $scope.appointment.Recurrence != null)
                    $scope.appointment.Recurrence.RecurrenceID = $scope.appointment.RecurrenceID;
                var q = $q.defer();
                $scope.appointment.ContactID = $scope.ContactID;
                $scope.appointment.IsCancelled = $scope.isCancel;
                var apptcopy = angular.copy($scope.appointment);
                if ($scope.appointment.AppointmentLengthPeriod == "hour") {
                    apptcopy.AppointmentLength = $filter('tomin')($scope.appointment.AppointmentLength);
                }

                if (isUpdate) {
                    q.resolve(appointmentService.updateAppointment(apptcopy));

                } else {
                    q.resolve(appointmentService.addAppointment(apptcopy));                    
                }
                return q.promise;
            };

            $scope.saveAppointmentContact = function ( appointmentID, isUpdate ) {
                $scope.appointmentContact.AppointmentID = appointmentID;
                $scope.appointmentContact.ResourceID = $scope.ContactID;
                $scope.appointmentContact.ResourceTypeID = 7;
                $scope.AppointmentResourceID = 0;
                //$scope.appointmentContact.ContactID = $scope.ContactID;
                if ( isUpdate ) {
                    pendingAddUpdates.push(appointmentService.updateAppointmentResource($scope.appointmentContact));
                } else {
                    pendingAddUpdates.push(appointmentService.addAppointmentResource($scope.appointmentContact));
                }
            };

            $scope.availableTimeSlots = function ( appointmentId ) {
                return $filter( 'filter' )( $scope.selectedTimeSlots, function ( slot ) {
                    return ( appointmentId == undefined || slot.appointmentId == appointmentId ) || slot.status == 'available';
                } );
            }

            var resources = function ( appointmentID ) {
                var appointmentResources = [];
                var appointmentResource = {};
                // resource type - room, provider
                var timeSlots = $scope.availableTimeSlots( appointmentID );
                angular.forEach( timeSlots, function ( timeSlot ) {
                    if ( !( timeSlot.type == 0 || timeSlot.resourceId == 0 ) ) {
                        var resourceTypeID = getResourceType( timeSlot.type );

                        var existingAppointmentResource = $filter( 'filter' )( $scope.appointmentResources, function ( resource ) {
                            return resource.AppointmentID == appointmentID && resource.ResourceTypeID == resourceTypeID && resource.ResourceID == timeSlot.resourceId;
                        } );

                        if ( existingAppointmentResource.length == 0 ) {
                            appointmentResource = { AppointmentResourceID: 0, AppointmentID: appointmentID, ResourceTypeID: resourceTypeID, ResourceID: timeSlot.resourceId, ParentID: ( timeSlot.type == 1 ? timeSlot.groupId : null ) };
                        }
                        else {
                            appointmentResource = existingAppointmentResource[0];
                        }
                        if ( !isAppointmentResourceExists( appointmentResource, appointmentResources ) )
                            appointmentResources.push( appointmentResource );
                    }
                } );

                // resource type - external

                if ( $scope.selectedLocation != undefined && $scope.selectedLocation.type == 4 && $scope.selectedLocation.ID != null ) {
                    var existingAppointmentResource = $filter( 'filter' )( $scope.appointmentResources, function ( resource ) {
                        return resource.AppointmentID == appointmentID && resource.ResourceTypeID == 4 && resource.ResourceID == $scope.selectedLocation.LocationID;
                    } );

                    if ( existingAppointmentResource.length == 0 ) {
                        appointmentResource = {
                            AppointmentResourceID: 0, AppointmentID: appointmentID, ResourceTypeID: 4, ResourceID: $scope.selectedLocation.LocationID
                        };
                    }
                    else {
                        appointmentResource = existingAppointmentResource[0];
                    }

                    if ( !isAppointmentResourceExists( appointmentResource, appointmentResources ) )
                        appointmentResources.push( appointmentResource );
                }
                return appointmentResources;
            }

            $scope.saveAppointmentResource = function ( appointmentID, isUpdate, isrecur, newapptid ) {
                if ( !$scope.isNonMHMR ) {
                    var appointmentResources = resources( appointmentID );

                    var newResources = $filter('filter')(appointmentResources, function (item) {
                        return (item.AppointmentResourceID == 0 || isrecur);
                    });

                    angular.forEach( newResources, function ( item ) {
                        item.IsActive = true;
                        item.AppointmentID = newapptid;
                        item.AppointmentResourceID = 0;
                        pendingAddUpdates.push(appointmentService.addAppointmentResource(item));
                    } );

                    if ($scope.appointment.AppointmentID != undefined && $scope.appointment.AppointmentID != 0 && appointmentExistingResources.length > 0 && isrecur == false) {
                        angular.forEach( appointmentExistingResources, function ( item ) {
                            var removedOrModified = $filter( 'filter' )( appointmentResources, { AppointmentResourceID: item.AppointmentResourceID }, true )[0];
                            if ( !removedOrModified ) { //Resource removed
                                item.IsActive = false;
                                pendingAddUpdates.push(appointmentService.updateAppointmentResource(item));
                            } else {
                                if ( removedOrModified.ResourceID != item.ResourceID ) { //Resource changed
                                    item.IsActive = true;
                                    pendingAddUpdates.push(appointmentService.updateAppointmentResource(item));
                                }
                            }
                        } );
                    }
                }

                // save appointment contact
                $scope.saveAppointmentContact( (isrecur) ? newapptid : appointmentID, isUpdate );
            };

            var isAppointmentResourceExists = function ( appointment, appointmentResources ) {
                for ( var i = 0; i < appointmentResources.length; i++ ) {
                    if ( appointmentResources[i].ResourceID == appointment.ResourceID && appointmentResources[i].ResourceTypeID == appointment.ResourceTypeID ) {
                        return true;
                    }
                }

                return false;
            };


            $scope.validateStartTimeFromSave = function () {
                var max = 1800;
                var min = 800;
                var formatStart = $scope.appointment.StartAMPM;

                var time = 0;
                var isValid = false;

                if (formatStart != undefined  && $scope.appointment.AppointmentStartTime ) {
                   
                        time = $filter('toMilitaryTime')($scope.appointment.AppointmentStartTime, formatStart);
                    $scope.appointment.AppointmentStartTime = time;
                        isValid = time >= min && time < max;

                        if ($scope.ctrl.scheduleForm != undefined && (!isValid)) {
                                     return "Please enter valid start time ";
                        }

                
                }
                
            }

            $scope.validateEndTimeFromSave = function () {
                var max = 1800;
                var min = 800;
               var formatEnd = $scope.appointment.AMPM;
                var time = 0;
                var isValid = false;

                if (formatEnd != undefined  && $scope.appointment.AppointmentEndTime ) {
                    formatEnd = $scope.endTimeAmPm;
                    time = $filter('toMilitaryTime')($scope.appointment.AppointmentEndTime, formatEnd);
                    $scope.appointment.AppointmentEndTime = time;
                        isValid = time > min && time <= max;
                        if ($scope.ctrl.scheduleForm != undefined && (!isValid)) {
                                return "Please enter valid end time ";
                        }
   
                }
               
                }
            

            $scope.save = function (isNext, mandatory, hasErrors) {                

                if (!$scope.ctrl.scheduleForm.$valid) {
                    alertService.error("Please correct the errors on the page before proceeding");
                    return false;
                }
                if ( ( !mandatory && !hasErrors && !isNext ) || ( mandatory && !hasErrors && !isNext ) || ( !mandatory && !hasErrors && isNext ) || ( mandatory && !hasErrors && isNext ) ) {
                    var isDirty = formService.isDirty();
                    // check time slot if it is blocked or conflicts
                    // conflicts
                    if ( isDirty ) {
                        if ( !$scope.isCancel ) {
                            var currentTime = getCurrentTime();
                            var todayDate = getCurrentDate();

                            if ( !$scope.isNonMHMR ) {
                                if ( $scope.selectedTimeSlots == undefined || $scope.selectedTimeSlots.length == 0 ) {
                                    alertService.error( 'No time slot is selected.' );
                                    return false;
                                }

                                var isTimeSlotAvailable = false;
                                angular.forEach( $scope.selectedTimeSlots, function ( timeSlot ) {
                                    if ( ( timeSlot.status != "blocked" && timeSlot.status != "appointment" ) || ( timeSlot.appointmentId && timeSlot.appointmentId == $scope.appointment.AppointmentID ) ) {
                                        isTimeSlotAvailable = true;
                                    }
                                } );

                                // Make sure there isn't any 'blocked' time slot
                                angular.forEach($scope.selectedTimeSlots, function (timeSlot) {
                                    if (timeSlot.status == "blocked")
                                        isTimeSlotAvailable = false;
                                 });

                                if ( !isTimeSlotAvailable ) {
                                    alertService.error( 'No time slot is available.' );
                                    return false;
                                }

                                // Custom validation to ensure that at least 1 provider has been selected for appts OTHER than NON MHMR
                                if ($scope.nonSpecialistProviders.length < 1 && $scope.credentials.length < 1) {
                                    alertService.error('The appointment must contain a provider');
                                    return $scope.promiseNoOp();
                                }

                                //var timeSlots = $scope.availableTimeSlots( $scope.appointment.AppointmentID );
                                //if (timeSlots.length == 1 && timeSlots[0].contactId == $scope.ContactID && timeSlots[0].type == 0 && timeSlots[0].resourceId == 0) //if no resouces are available.(only Contact is selected)
                                //{
                                //    alertService.error( 'No resources are available.' );
                                //    return false;
                                //}
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

                            //temp fix:
                            $scope.appointment.ProgramID = 0;
                            $scope.appointment.ProgramID = $scope.programID;
                            $scope.appointment.ServicesID = $scope.appointment.ServicesID;
                            //$stateParams.programIDRouteValue = $scope.programID;

                            //

                            var startTime = $filter( "toMilitaryTime" )( $scope.appointment.AppointmentStartTime, $scope.appointment.AMPM );
                            var difference = calculateTimeDifference( startTime, currentTime );
                            var currentTimeSlotDate = $filter( 'toMMDDYYYYDate' )( $scope.selectedTimeSlots[0].date, 'MM/DD/YYYY' );

                            if (new Date(currentTimeSlotDate).getTime() < new Date(todayDate).getTime() || (new Date(currentTimeSlotDate).getTime() == new Date(todayDate).getTime() && difference < 0)) {
                                alertService.error('Appointment can\'t be scheduled for past date.');
                                return;
                            }
                        }            
                        

                        var validateStartTimeFromSaveCheck = $scope.validateStartTimeFromSave();
                        if (validateStartTimeFromSaveCheck != undefined) {
                            alertService.error(validateStartTimeFromSaveCheck);
                            return false;
                        }


                        var validateEndTimeFromSaveCheck = $scope.validateEndTimeFromSave();
                        if (validateEndTimeFromSaveCheck != undefined) {
                            alertService.error(validateEndTimeFromSaveCheck);
                            return false;

                        }

                        if ($scope.Recurrence != null && $scope.Recurrence.IsRecurring) {
                            if ($scope.Recurrence.WeeklyRecurrence !== undefined && $scope.Recurrence.WeeklyRecurrence !== null) {
                                if (!ValidateWeeklyRecurrence()) {
                                    alertService.error('Atleast one weekday must be provided for Recurrence');
                                    return;
                                }
                            }
                        }


                        var isUpdate = $scope.appointment.AppointmentID != undefined && $scope.appointment.AppointmentID != 0;                       

                        // Confirm with a message for modifying recurrent appts
                        if ($scope.IsRecurringAptEdit && isUpdate && $scope.Recurrence != null && $scope.Recurrence != '' && $scope.Recurrence.IsRecurring) {
                            bootbox.confirm("If you changed specific appointments in the series, your changes will be cancelled and those appointments will match the series again.", function (result) {
                                if (result === true)
                                    $scope.finishSave(isUpdate);
                            });
                        }
                        else
                            $scope.finishSave(isUpdate);                        
                    }
                }
            };

            $scope.finishSave = function (isUpdate) {
                var successMessage = '';
                var errorMessage = '';
                var warningMessage = '';
                var addUpdate = $scope.isCancel ? 'cancelled' : (isUpdate ? 'updated' : 'added');

                $scope.saveAppointment(isUpdate).then(function (response) {
                    var data = response;
                    if (data.ResultCode === 0 && data.DataItems.length > 0 && data.DataItems[0].AppointmentID != null && data.DataItems[0].AppointmentID > 0) {
                        if (!isUpdate && data.DataItems.length > 0)
                            if ($scope.appointment != null) {
                                $scope.appointment = angular.copy(data.DataItems[0]);
                                if ($scope.appointment.AppointmentStartTime < 1000) {
                                    $scope.appointment.AppointmentStartTime = "0" + $scope.appointment.AppointmentStartTime;
                                }
                                $scope.appointment.AppointmentEndTime = $scope.adjustAppointmentEndTime();
                                $scope.appointment.StartAMPM = $filter('toStandardTimeAMPM')($scope.appointment.AppointmentStartTime);
                                $scope.appointment.AMPM = $filter('toStandardTimeAMPM')($scope.appointment.AppointmentStartTime);
                                $scope.appointment.AppointmentLengthPeriod = "min";
                                $scope.appointment.IsRecurringAptEdit = $stateParams.IsRecurringAptEdit != null ? $stateParams.IsRecurringAptEdit : true;
                            }
                        if (!$scope.isCancel) {
                            //if ($scope.appointment.AppointmentLengthPeriod == "hour") {
                            //    $scope.appointment.AppointmentLength = $filter('tohour')($scope.appointment.AppointmentLength);
                            //}

                            var appointmentID = data.DataItems[0].AppointmentID;
                            $stateParams.AppointmentID = appointmentID;

                            if (data.DataItems.length > 0) {
                                var apptid = $scope.appointment.AppointmentID;
                                angular.forEach(data.DataItems, function (appt) {
                                    var isrecur = $scope.appointment.IsRecurringAptEdit && $scope.appointment.RecurrenceID != null && $scope.appointment.Recurrence != null;
                                    apptid = (isrecur) ? $scope.appointment.AppointmentID : appt.AppointmentID;
                                    // save appointment resources
                                    if (!isUpdate ||
                                        data.DataItems.length > 1 ||
                                        (isrecur && !$scope.appointment.Recurrence.IsRecurring)) { // we are dealing w/ a recurrence situation, so do an add instead of updating appointment resources and contacts
                                        $scope.saveAppointmentResource(apptid, false, isrecur, appt.AppointmentID);
                                    } else {
                                        $scope.saveAppointmentResource(apptid, true, isrecur, appt.AppointmentID);
                                    }
                                });

                                $q.all(pendingAddUpdates).then(function (response) {
                                    if (errorMessage != '') {
                                        alertService.error(errorMessage);
                                    }
                                    else if (warningMessage != '') {
                                        alertService.warning(warningMessage);
                                    } else {
                                        successMessage = 'Appointment has been ' + addUpdate + ' successfully.';
                                        alertService.success(successMessage);
                                        setValidStateForAppointment();
                                        appointmentService.getAppointmentResource(apptid).then(function (data) {
                                            $scope.appointmentResources = $filter('filter')(data.DataItems, { IsActive: true });
                                            appointmentExistingResources = $filter('filter')(data.DataItems, { IsActive: true });
                                        });
                                        resetForm();
                                    }
                                });
                            }
                        } else {
                            alertService.success('Appointment has been ' + addUpdate + ' successfully.');
                            setValidStateForAppointment();
                            resetForm();
                        }
                    } else
                        alertService.error('The recurrence pattern is not valid.');
                }, function (errorStatus) { alertService.error('There was a problem saving the appointment.'); });
            }

            var setValidStateForAppointment = function () {
                if ($state.current.name.toLowerCase().indexOf('referrals.appointment') >= 0) {
                    $rootScope.$broadcast($state.current.name, { validationState: 'valid' });
                }
            };

            var getExternalSources = function () {
                resourceService.getResources( 2, null, true ).then( function ( data ) {
                    if ( data.DataItems != null && data.DataItems.length > 0 ) {
                        $scope.externalSources = data.DataItems;
                    }
                }, function ( errorStatus ) { } );
            };

            $scope.addResource = function ( selectedProviderId ) {
                if ( $scope.providers == undefined || $scope.providers.length == 0 ) {
                    Array.prototype.push.apply( $scope.providers, $scope.externalSources );

                    if ( selectedProviderId )
                        addNewResource( selectedProviderId );
                    else
                        addNewResource( $scope.providers[0].ResourceID );
                }
                else {
                    if ( selectedProviderId )
                        addNewResource( selectedProviderId );
                    else
                        addNewResource( $scope.providers[0].ResourceID );
                }
            };

            var addNewResource = function ( selectedProviderId ) {
                nonSpecialistIndex++;

                var provider = {
                    nonSpecialistID: nonSpecialistIndex, selectedProviderId: selectedProviderId
                };
                $scope.nonSpecialistProviders.push( provider );

                generateCalenderForNonSpecialistProvider( provider );
                $scope.getResourceAvailability( 2, nonSpecialistIndex, 2 ).then( function () {
                    highlightFirstAvailableTimeSlot();
                } );
            }

            $scope.addRoom = function () {
                $scope.timeSlots = $filter( 'filter' )( $scope.timeSlots, function ( timeSlot ) {
                    return timeSlot.type != 3
                } );

                var filteredRooms = [];
                $scope.timeSlots = $filter( 'filter' )( $scope.timeSlots, function ( timeSlot ) {
                    return timeSlot.type != 3 && timeSlot.type != 4;
                } );

                if ( $scope.selectedRoomId != null && $scope.selectedRoomId > 0 )
                    filteredRooms = $filter( 'filter' )( $scope.rooms, function ( room ) { return room.RoomID == $scope.selectedRoomId } );
                if ( filteredRooms.length > 0 ) {
                    $scope.selectedRoomName = filteredRooms[0].RoomName;

                    generateCalenderForRoom();

                    $scope.getResourceAvailability( 1, $scope.selectedRoomId, 3 ).then( function () {
                        highlightFirstAvailableTimeSlot();
                    } );
                }
                else {
                    $scope.selectedRoomId = 0;
                }
            }

            var highlightAppointment = function () {
                var contactTimeSlots = $filter( 'filter' )( $scope.timeSlots, function ( item ) {
                    return item.contactId == $scope.ContactID;
                } );

                var flag = false;
                var selectedColumnIndex = -1;
                var timeSlot = {};

                for ( var i = 0; i < contactTimeSlots.length; i++ ) {
                    if ( contactTimeSlots[i].date == $filter( 'toMMDDYYYYDate' )(( new Date( $scope.appointment.AppointmentDate ) ), 'MM/DD/YYYY') &&
                        $filter( "toMilitaryTime" )( contactTimeSlots[i].startTime, $scope.appointment.StartAMPM ) == $scope.appointment.AppointmentStartTime ) {
                        selectedColumnIndex = i;
                        timeSlot = contactTimeSlots[i];
                        break;
                    }
                }

                var targetIndex;
                targetIndex = selectedColumnIndex + 2;
                highlightCalander( targetIndex );
                $scope.getAppointmentStatus( timeSlot, true );
            };

            var getCurrentDate = function () {
                var todayDate = new Date();
                return todayDate = $filter( 'toMMDDYYYYDate' )( todayDate, 'MM/DD/YYYY', 'useLocal' );
            };

            var getCurrentTime = function () {
                var todayDate = new Date();
                var currentHour = todayDate.getHours();
                var currentMinute = todayDate.getMinutes();

                var period = currentHour >= 12 ? 'pm' : 'am';
                return $filter( 'toMilitaryTime' )( pad( currentHour, 2 ) + ":" + pad( currentMinute, 2 ), period );
            };

            var highlightFirstAvailableTimeSlot = function () {
                if($scope.isFormDisabled)
                    return;
                var contactTimeSlots = $filter( 'filter' )( $scope.timeSlots, function ( item ) {
                    return item.contactId == $scope.ContactID;
                } );

                var currentTime = getCurrentTime();
                var todayDate = getCurrentDate();

                var flag = false;
                var selectedColumnIndex = -1;
                if ( $scope.appointment.AppointmentID != undefined ) {
                    highlightAppointment();
                }
                else {
                    for ( var i = 0; i < contactTimeSlots.length; i++ ) {
                        var difference = calculateTimeDifference( contactTimeSlots[i].startTime, currentTime );
                        var currentTimeSlotDate = $filter( 'toMMDDYYYYDate' )( contactTimeSlots[i].date, 'MM/DD/YYYY' );

                        if ( contactTimeSlots[i].status == 'available' && currentTimeSlotDate >= todayDate ) {
                            if ( currentTimeSlotDate == todayDate && difference < 0 ) {
                                continue;
                            }

                            if ( $scope.credentials.length > 0 || $scope.nonSpecialistProviders.length > 0 || $scope.selectedRoomId > 0 ) {
                                flag = true;
                            }
                            for ( var j = 0; j < $scope.credentials.length; j++ ) {
                                var credentialId = $scope.credentials[j].CredentialID;

                                if ( $filter( 'filter' )( $scope.timeSlots, function ( item ) {
                                    return item.groupId == credentialId
                                            && contactTimeSlots[i].date == item.date
                                            && item.startTime == contactTimeSlots[i].startTime
                                            && item.status == 'available';
                                } ).length > 0 ) {
                                    flag = true;
                                }
                                else {
                                    flag = false;
                                    break;
                                }
                            }

                            if ( flag == true ) {
                                for ( var index = 0; index < $scope.nonSpecialistProviders.length; index++ ) {
                                    var nonSpecialistID = $scope.nonSpecialistProviders[index].nonSpecialistID;

                                    if ( $filter( 'filter' )( $scope.timeSlots, function ( item ) {
                                    return item.groupId == nonSpecialistID && item.startTime == contactTimeSlots[i].startTime && item.status == 'available';
                                    } ).length > 0 ) {
                                        flag = true;
                                    }
                                    else {
                                        flag = false;
                                        break;
                                    }
                                }
                            }

                            if ( flag == true && $scope.selectedRoomId > 0 ) {
                                if ( $filter( 'filter' )( $scope.timeSlots, function ( item ) {
                                return item.type == 3 && item.groupId == $scope.selectedRoomId && item.startTime == contactTimeSlots[i].startTime && item.status == 'available';
                                } ).length > 0 ) {
                                    flag = true;
                                }
                                else {
                                    flag = false;
                                }
                            }
                        }

                        if ( flag == true ) {
                            selectedColumnIndex = i;
                            $scope.getAppointmentStatus( $scope.timeSlots[i], true );

                            var targetIndex;
                            targetIndex = selectedColumnIndex + 2;
                            highlightCalander( targetIndex );

                            break;
                        }
                        else {
                            $( '.scheduling_assist td' ).removeClass( 'col_select' );
                        }
                    }
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

           
            $scope.$watch("[appointment.AppointmentDate,appointment.AppointmentLength,appointment.AppointmentStartTime,appointment.AppointmentEndTime,appointment.StartAMPM,appointment.AMPM]",
                function (newValues, oldValues) {
                    if (!$scope.isCancel && (selectedDate >= currentDate)) {
                        var isValueChanged = false;
                        if ( oldValues[0] != newValues[0] ) {
                            if ( $scope.appointment.AppointmentDate != undefined && newValues[0] != undefined ) {
                                // If date belongs to calander then donot re-render
                                var appointmentDate = new Date($scope.appointment.AppointmentDate);
                                if ( appointmentDate != 'Invalid Date' ) {
                                    var newAppointmentDate = new Date( appointmentDate.getFullYear(), appointmentDate.getMonth(), appointmentDate.getDate() );

                                    var startDate = new Date( $scope.selectedYear, $scope.selectedMonth - 1, $scope.weekStartDate );
                                    var endDate = new Date($scope.selectedYear, $scope.selectedMonth - 1, $scope.weekEndDate);

                                    if ( !( newAppointmentDate >= startDate && newAppointmentDate <= endDate ) ) {
                                        $scope.changeCalander( null, 'date' );
                                    }
                                }

                                isValueChanged = true;
                            }
                        }
                        else if ( oldValues[1] != newValues[1] ) {
                            if ( changeLength ) {
                                $scope.appointment.AppointmentEndTime = $scope.adjustAppointmentEndTime();
                                isValueChanged = true;
                                changeLength = false;
                            }
                        }
                        else if (oldValues[2] !== newValues[2]) {
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
                        else if (oldValues[3] !== newValues[3]) {
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
                        else if ( oldValues[4] != newValues[4] ) {
                            isValueChanged = true;
                            //validation of the start ampm button...start time AMPM can only be changed via the radio buttons themselves
                            var formatStart = $scope.appointment.StartAMPM;
                            var time;
                            var isValid = true;
                            if ($scope.appointment.AppointmentStartTime !== undefined && $scope.appointment.AppointmentStartTime !== null) {
                                time = $filter('toMilitaryTime') ($scope.appointment.AppointmentStartTime, formatStart);
                                isValid = time >= 600 && time < 1800;
                        }

                            if (!isValid) {
                                isValueChanged = false;
                                $scope.appointment.StartAMPM = oldValues[4];
                                alertService.warning('Invalid AM/PM selection!');
                            }
                        }

                        if ( isValueChanged == true )
                            adjustCalanderSelection();
                        $scope.setAmPmForAppointmentStartTime();
                        $scope.setAmPmForAppointmentEndTime();
                    }
                } );

            $scope.setAmPmForAppointmentStartTime = function () {
                if ( $scope.appointment.AppointmentStartTime ) {
                    var endTime = 0;

                    var appointmentLength = $scope.appointment.AppointmentLengthPeriod == 'hour' ? $scope.appointment.AppointmentLength * 60 : $scope.appointment.AppointmentLength;

                    if ( isNaN( appointmentLength ) ) {
                        appointmentLength = 30;
                    }
                    var startTime = $filter( "toMilitaryTime" )( $scope.appointment.AppointmentStartTime, $scope.appointment.StartAMPM );

                    var standardTime = moment( pad( startTime, 4 ), 'HH:mm' ).add( appointmentLength, 'minute' ).format( 'HH:mm' );
                    var ampm = $filter( 'toStandardTimeAMPM' )( standardTime );
                    startTime = $filter( 'toMilitaryTime' )( standardTime, ampm );
                    $scope.startTimeAmPm = $filter( 'toStandardTimeAMPM' )( startTime );
                }
            }

            $scope.setAmPmForAppointmentEndTime = function() {
                if ($scope.appointment.AppointmentEndTime) {
                    var endTime = 0;

                    var appointmentLength = $scope.appointment.AppointmentLengthPeriod == 'hour' ? $scope.appointment.AppointmentLength * 60 : $scope.appointment.AppointmentLength;

                    if (isNaN(appointmentLength)) {
                        appointmentLength = 30;
                    }

                    //logic to determine what the end time's AM/PM should be set to
                    var startTime = $filter("toMilitaryTime")($scope.appointment.AppointmentStartTime, $scope.appointment.StartAMPM);
                    var calculatedEndTime = moment(pad(startTime, 4), 'HH:mm').add(appointmentLength, 'minute').format('HH:mm');
                    $scope.endTimeAmPm = $filter('toStandardTimeAMPM')(calculatedEndTime);
                    if ($scope.appointment.AMPM !== $scope.endTimeAmPm) {
                        $scope.appointment.AMPM = $scope.endTimeAmPm;
                        alertService.warning('End time adjusted to ' + $scope.endTimeAmPm);
                }
            }
            };

            var adjustCalanderSelection = function () {
                if ( $scope.appointment.AppointmentDate != undefined ) {
                    if ( $scope.appointment.StartAMPM == 'AM' && $scope.appointment.AppointmentStartTime > 1200 ) {
                        $scope.appointment.AppointmentStartTime = pad(( $scope.appointment.AppointmentStartTime - 1200 ), 4 );
                    }
                    if ( $scope.appointment.AMPM == 'AM' && $scope.appointment.AppointmentEndTime > 1200 ) {
                        $scope.appointment.AppointmentEndTime = pad(( $scope.appointment.AppointmentEndTime - 1200 ), 4 );
                    }
                     var startTime = $filter("toMilitaryTime")($scope.appointment.AppointmentStartTime, $scope.appointment.StartAMPM);
                     var time = $filter("toMilitaryTime")($scope.appointment.AppointmentEndTime, $scope.appointment.AMPM);

                    if ( startTime ) {
                        var selectedColumnIndex = -1;
                        var contactTimeSlots = $filter( 'filter' )( $scope.timeSlots, function ( item ) {
                            return item.contactId == $scope.ContactID;
                        } );

                        var selectedTimeSlot = $filter( 'filter' )( contactTimeSlots, function ( slot ) {
                            return (slot.date == $filter("formatDate")($scope.appointment.AppointmentDate) && (calculateTimeDifference(slot.endTime, startTime) > 0 &&
                                calculateTimeDifference(slot.endTime, startTime) <= 30));
                        } )[0];

                        if ( selectedTimeSlot ) {
                            for ( var i = 0; i < contactTimeSlots.length; i++ ) {
                                if ( contactTimeSlots[i].sNo == selectedTimeSlot.sNo ) {
                                    selectedColumnIndex = i;
                                    timeSlot = contactTimeSlots[i];
                                    break;
                                }
                            }

                            if ( selectedColumnIndex >= 0 ) {
                                var targetIndex;
                                targetIndex = selectedColumnIndex + 2;
                                highlightCalander( targetIndex );
                                $scope.getAppointmentStatus( timeSlot, false );
                            }
                            else {
                                $( '.scheduling_assist td' ).removeClass( 'col_select' );
                            }
                        }
                    }
                }
            };

            $scope.changeMinHour = function ( value, preventAction ) {
                
                if ( preventAction ) {
                    if ( value == "min" ) {
                        // min = value * 60 (minutes)
                        $scope.appointment.AppointmentLength = $filter( 'tomin' )( $scope.appointment.AppointmentLength );;
                    }
                    else {
                        // hours = value / 60 (minutes)
                        $scope.appointment.AppointmentLength = $filter( 'tohour' )( $scope.appointment.AppointmentLength );
                    }
                    $scope.appointment.AppointmentLengthPeriod = value;
                    $scope.appointment.AppointmentEndTime = $scope.adjustAppointmentEndTime();
                }
            }

            $scope.validateStartAppointmentTime = function () {
                $timeout( function () {
                    $scope.setAmPmForAppointmentStartTime();
                    $scope.validateTime( true );
                    changeLength = true;
                } );
            }

            $scope.validateAppointmentTime = function () {
                $timeout( function () {
                    $scope.setAmPmForAppointmentEndTime();
                    $scope.validateTime( false );

                } );
            }

            $scope.validateStartTimeWatch = function() {
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

            $scope.validateEndTimeWatch = function() {
                var max = 1800;
                var min = 800;
                var format = $scope.appointment.AMPM;

                if (format != undefined && !$scope.isCancel && $scope.appointment.AppointmentEndTime) {
                    var time = $filter('toMilitaryTime') ($scope.appointment.AppointmentEndTime, format);
                    var isValid = time > min && time <= max;

                    return isValid;
                }

                return true;
            };

            $scope.getCursorPosition = function (id) {
                test = $('#' + id);
                var input = $('#' + id).get(0);

                if (!input) return; // No (input) element found

                if ('selectionStart' in input) {
                    // Standard-compliant browsers
                    return input.selectionStart;
                } else if(document.selection) {
                    // IE
                    input.focus();
                    var sel = document.selection.createRange();
                    var selLen = document.selection.createRange().text.length;
                    sel.moveStart('character', - input.value.length);
                    return sel.text.length -selLen;
                }
            }

            $scope.resetTimeField = function (id, prev, hour, min) {
                var time = hour + ':' + min;
                
                if (hour == null || min == null || hour == "00")
                    time = prev;

                $('#' +id).val(time);
                
                return time;
            }

            $scope.pad = function (num, n, size, cursorpos) {
                var s = (n != null) ? n + "" : num + "";
                while (s.length < size) s = (cursorpos == 0 || cursorpos == 3) ? "0" + s : s + "0";
                return s;
            }

            $scope.selectRange = function (id, start, end) {
                if (end === undefined) {
                    end = start;
                }
                return $('#' + id).each(function () {
                    if ('selectionStart' in $('#' + id)[0]) {
                        $('#' + id)[0].selectionStart = start;
                        $('#' + id)[0].selectionEnd = end;
                    } else if ($('#' + id)[0].setSelectionRange) {
                        $('#' + id)[0].setSelectionRange(start, end);
                    } else if ($('#' + id)[0].createTextRange) {
                        var range = $('#' + id)[0].createTextRange();
                        range.collapse(true);
                        range.moveEnd('character', end);
                        range.moveStart('character', start);
                        range.select();
                    }
                });
            }

            $scope.validateStandardTime = function (isStartTime) {
                $scope.ctrl.scheduleForm.$dirty = true;
                if (isStartTime) id = 'startTime';
                else id = 'endTime';

                var hourcursorpos = $scope.getCursorPosition(id);
                var prev = $('#' + id).data("prev");
                var val = $('#' + id).val();
                var idx = val.toString().indexOf(':');
                var newtime = '';

                if (idx < 0) {
                    newtime = $scope.resetTimeField(id, prev);
                } else {
                    var shour = val.toString().substring(0, (idx < 2) ? idx : 2); //108
                    var smin = val.toString().substring(idx + 1, (val.toString().substring(idx + 1).length > 2) ? val.toString().length - 1 : val.toString().length);

                    if (idx == hourcursorpos && idx == 3) {
                        var shourrem = val.toString().substring(2, 3);
                        smin = shourrem + smin.substring(1);
                    }

                    var hour = ($.isNumeric(shour) && shour.indexOf('o') < 0) ? parseInt(shour.substring(0, 2)) : null;
                    var min = ($.isNumeric(smin) && smin.indexOf('o') < 0) ? parseInt(smin.substring(0, 2)) : null;

                    if (hour == null || isNaN(hour) || hour > 12 || hour < 0 || shour == '' || (shour.indexOf('.') > 0))
                        hour = null;
                    if (min == null || isNaN(min) || min > 59 || min < 0 || smin == '' || (smin.indexOf('.') > 0))
                        min = null;
                    if (idx == hourcursorpos && idx == 3 && min != null)
                        hourcursorpos++;

                    newtime = $scope.resetTimeField(id, prev, hour == null ? null : $scope.pad(shour, (shour.length > 2) ? hour : null, 2, hourcursorpos), min == null ? null : $scope.pad(smin, (smin.length > 2) ? min : null, 2, hourcursorpos));
                }

                // Set cursor
                $scope.selectRange(id, hourcursorpos);
                $('#' + id).data("prev", newtime);
            }

            $scope.validateTime = function ( isStartTime ) {
                var max = 1800;
                var min = 800;
                var formatStart = $scope.appointment.StartAMPM;
                var format = $scope.appointment.AMPM;
                var time = 0;
                var isValid = false;

                if ( formatStart != undefined && format != undefined && !$scope.isCancel && $scope.appointment.AppointmentStartTime && $scope.appointment.AppointmentStartTime ) {
                    if ( isStartTime == true ) {
                        time = $filter( 'toMilitaryTime' )( $scope.appointment.AppointmentStartTime, formatStart );
                        isValid = time >= min && time < max;

                        if ($scope.ctrl.scheduleForm != undefined) {

                            $scope.ctrl.scheduleForm.startTime.$setValidity("startTimeError", isValid);
                           
                            if (!isValid) {
                                $('#startTimeContainer').addClass('has-error');
                                $('#appointmentStartTimeError1').removeClass('ng-hide').addClass('ng-show');
                                $scope.ctrl.scheduleForm.startTime.$invalid = true;
                                $scope.ctrl.scheduleForm.startTime.$valid = false;
                            }
                            else
                            {
                                $('#startTimeContainer').removeClass('has-error');
                                $('#appointmentStartTimeError1').removeClass('ng-show').addClass('ng-hide');
                                $scope.ctrl.scheduleForm.startTime.$invalid = false;
                                $scope.ctrl.scheduleForm.startTime.$valid = true;
                            }
                        }
                    }
                    else {
                        format = $scope.endTimeAmPm;
                        time = $filter( 'toMilitaryTime' )( $scope.appointment.AppointmentEndTime, format );
                        isValid = time > min && time <= max;
                        if ($scope.ctrl.scheduleForm != undefined) {
                            $scope.ctrl.scheduleForm.endTime.$setValidity("endTimeError", isValid);

                            if (!isValid) {
                                $('#endTimeContainer').addClass('has-error');
                                $('#appointmentEndTimeError1').removeClass('ng-hide').addClass('ng-show');
                                $scope.ctrl.scheduleForm.endTime.$invalid = true;
                                $scope.ctrl.scheduleForm.endTime.$valid = false;
                            }
                            else {
                                $('#endTimeContainer').removeClass('has-error');
                                $('#appointmentEndTimeError1').removeClass('ng-show').addClass('ng-hide');
                                $scope.ctrl.scheduleForm.endTime.$invalid = true;
                                $scope.ctrl.scheduleForm.endTime.$valid = false;
                            }

                          
                        }

                        var isValid1 = true;
                        var stime = $filter( 'toMilitaryTime' )( $scope.appointment.AppointmentStartTime, formatStart );
                        var etime = $filter( 'toMilitaryTime' )( $scope.appointment.AppointmentEndTime, format );
                        if ( etime <= stime ) {
                            isValid1 = false;
                        }
                        if ($scope.ctrl.scheduleForm != undefined) {
                            $scope.ctrl.scheduleForm.endTime.$setValidity("endTimeValidError", isValid1);
                        }
                    }
                }
            }

            $scope.validateAppointmentDate = function () {
                if ($scope.isFormDisabled)
                    return;

                if ( !$scope.isCancel ) {
                    if ( $scope.appointment.AppointmentID > 0 ) {
                        $( "#dateOfAppointmentError" ).addClass( 'ng-hide' );
                        $( "#dateOfAppointment" ).removeClass( "has-error" );
                    }

                    if ( $scope.appointment.AppointmentDate != undefined ) {
                        selectedDate = new Date( $scope.appointment.AppointmentDate );

                        if (selectedDate >= currentDate) {
                            $( "#dateOfAppointmentError" ).addClass( 'ng-hide' );
                            $( "#dateOfAppointment" ).removeClass( "has-error" );
                            $scope.ctrl.scheduleForm.$valid = true;
                        }
                        else if ($scope.ctrl.scheduleForm.$pristine) {
                            $("#dateOfAppointmentError").addClass('ng-hide');
                            $("#dateOfAppointment").removeClass("has-error");
                            $scope.ctrl.scheduleForm.$valid = true;
                        }
                        else {
                            $( "#dateOfAppointmentError" ).removeClass( 'ng-hide' );
                            $( "#dateOfAppointment" ).addClass( "has-error" );
                            $scope.ctrl.scheduleForm.$valid = false;
                        }
                    }
                }
            }

            $scope.validateAppointmentLength = function () {
                if ( $scope.appointment.AppointmentLengthPeriod != undefined && $scope.appointment.AppointmentLength != undefined ) {
                    var appointmentLength = $scope.appointment.AppointmentLength;
                    var timeToCheck;
                    var milTimeStart = $filter('toMilitaryTime')($scope.appointment.AppointmentStartTime, $scope.appointment.StartAMPM);
                    if ( isNaN( appointmentLength ) ) {
                        $( "#lengthOfAppointment" ).addClass( 'has-error' );
                        $("#lengthOfAppointmentError").removeClass('ng-hide');
                        return false;
                    }
                    if ($scope.appointment.AppointmentLengthPeriod == "min") {
                        if (appointmentLength > 0 && appointmentLength <= 600) {
                            timeToCheck = moment(pad(milTimeStart, 4), 'HH:mm').add(appointmentLength, 'minute').format('HH:mm');
                            $( "#lengthOfAppointmentError" ).addClass( 'ng-hide' );
                            $( "#lengthOfAppointment" ).removeClass( "has-error" );
                        }
                        else {
                            $( "#lengthOfAppointment" ).addClass( 'has-error' );
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

            var ValidateWeeklyRecurrence = function () {
                var retVal = true;
                if (($scope.Recurrence.WeekDayMon == undefined || $scope.Recurrence.WeekDayMon == false) && ($scope.Recurrence.WeekDayTue == undefined || $scope.Recurrence.WeekDayTue == false)
                     && ($scope.Recurrence.WeekDayWed == undefined || $scope.Recurrence.WeekDayWed == false) && ($scope.Recurrence.WeekDayThur == undefined || $scope.Recurrence.WeekDayThur == false)
                     && ($scope.Recurrence.WeekDayFri == undefined || $scope.Recurrence.WeekDayFri == false) && ($scope.Recurrence.WeekDaySat == undefined || $scope.Recurrence.WeekDaySat == false) 
                     && ($scope.Recurrence.WeekDaySun == undefined || $scope.Recurrence.WeekDaySun == false)) {
                        retVal = false;
                }
                return retVal;
            }

            var changeLength = false;
            $scope.init();
        }] );