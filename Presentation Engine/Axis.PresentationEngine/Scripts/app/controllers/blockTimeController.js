angular.module( 'xenatixApp' )
    .controller( 'blockTimeController', ['$scope', '$q', '$filter', '$rootScope', '$injector', 'formService', 'alertService', '$stateParams', 'lookupService', '$timeout', '$rootScope', '$state',
        function ( $scope, $q, $filter, $rootScope, $injector, formService, alertService, $stateParams, lookupService, $timeout, $rootScope, $state ) {
            //var FACILITY_RESOURCE_TYPE = 6;
            var FACILITY_RESOURCE_TYPE;
            var APPT_TYPE = "Blocked Time";
            $scope.isFormDisabled = false;
            $scope.permissionKey = $state.current.data.permissionKey;
            var currentDate = new Date();
            currentDate.setHours( 0, 0, 0, 0 );

            var resetForm = function () {
                $rootScope.formReset( $scope.ctrl.scheduleForm, $scope.ctrl.scheduleForm.$name );
            };

            // Get scheduling plugin 'resourceService'
            if ( $injector.has( 'appointmentService' ) )
                $scope.appointmentService = $injector.get( 'appointmentService' );
            else {
                bootbox.alert( "Appointment Service is not available, please load the Scheduling plugin!" );
                return;
            }

            $scope.dateOptions = {
                formatYear: "yy",
                startingDay: 1,
                showWeeks: false
            };

            $scope.blockedTimesList = [];
            //$scope.LocationID = $stateParams.LocationID;
            if ( $stateParams.LocationID != null && $stateParams.LocationID !== undefined ) {
                $scope.ResourceID = $stateParams.LocationID;
                FACILITY_RESOURCE_TYPE = 6;
            }
            else {
                $scope.ResourceID = $stateParams.UserID;
                FACILITY_RESOURCE_TYPE = 2;
            }

            $scope.init = function () {
                var ampm = $filter( 'toMMDDYYYYDate' )( new Date(), 'hh:mm A', 'useLocal' ).indexOf( 'AM' ) > -1 ? "AM" : "PM";
                var dt = new Date();
                var dttime = $filter( 'toMMDDYYYYDate' )( new Date(), 'hhmm ', 'useLocal' );
                // Init stuff
                $scope.blockedTime = {
                    BlockedTimeID: 0,
                    Reason: "",
                    StartDate: dt,
                    StartTime: '',
                    StartAmPm: ampm,
                    EndDate: dt,
                    EndTime: '',
                    EndAmPm: ampm,
                    Comment: "",
                    AllDay: false
                };

                // Set some defaults
                $scope.blockedTime.ContactID = 0;
                $scope.blockedTime.IsCancelled = false;
                $scope.blockedTime.ProgramID = 1;
                $scope.blockedTime.SupervisionVisit = false;
                $scope.blockedTime.ReferredBy = '';
                $scope.blockedTime.CancelComment = '';
                $scope.isUpdate = false;
                $scope.isDisabled = false;
                $scope.userBlockTimeID = 0;
                // Set appt type
                $scope.getAppointmentType();

                // Get current blocked times for this location
                $scope.getBlockedTimes();

                $scope.NewRecurrence = $scope.Recurrence;
            }

            $scope.getAppointmentType = function () {
                var q = $q.defer();
                $scope.appointmentTypes = $filter( 'filter' )( lookupService.getLookupsByType( 'AppointmentTypes' ), { AppointmentType: APPT_TYPE }, true );
                q.resolve( $scope.appointmentTypes );
                if ( $scope.appointmentTypes.length != 0 ) {
                    $scope.apptType = $scope.appointmentTypes[0].AppointmentTypeID;
                }
                return q.promise;
            };

            $scope.getApptResourceModel = function ( appointmentID ) {
                var apptresmodel = {};
                apptresmodel.AppointmentID = appointmentID;
                apptresmodel.ResourceTypeID = FACILITY_RESOURCE_TYPE;
                apptresmodel.ResourceID = $scope.ResourceID; // $scope.LocationID;
                return apptresmodel;
            }

            $scope.getApptModel = function () {
                var apptmodel = {};
                if ( $scope.blockedTime.BlockedTimeID != 0 && $scope.isUpdate )
                    apptmodel.AppointmentID = $scope.blockedTime.BlockedTimeID;
                apptmodel.EndDate = $filter('formatDate')($scope.blockedTime.EndDate);
                apptmodel.ContactID = $scope.blockedTime.ContactID;
                apptmodel.ProgramID = $scope.blockedTime.ProgramID;
                apptmodel.AppointmentTypeID = $scope.apptType;
                apptmodel.AppointmentDate = $filter('formatDate')($scope.blockedTime.StartDate);
                    //new Date($scope.blockedTime.StartDate.getFullYear(), $scope.blockedTime.StartDate.getMonth(), $scope.blockedTime.StartDate.getDate());
                apptmodel.AppointmentStartTime = $scope.formatTime( $scope.blockedTime.StartTime, $scope.blockedTime.StartAmPm );
                apptmodel.AppointmentLength = $scope.getTimeDiffInMins( $scope.blockedTime );
                apptmodel.SupervisionVisit = $scope.blockedTime.SupervisionVisit;
                apptmodel.ReasonForVisit = $scope.blockedTime.Reason;

                if ( FACILITY_RESOURCE_TYPE === 6 )
                    apptmodel.FacilityID = $scope.ResourceID; //$scope.LocationID;

                apptmodel.Comments = $scope.blockedTime.Comment;

                apptmodel.Recurrence = $scope.Recurrence;
                if (apptmodel.Recurrence != null) {
                    apptmodel.Recurrence.StartDate = $filter('formatDate')(apptmodel.Recurrence.StartDate);
                    apptmodel.Recurrence.EndDate = $filter('formatDate')(apptmodel.Recurrence.EndDate);
                    apptmodel.Recurrence.RecurrenceID = $scope.Recurrence != null ? $scope.Recurrence.RecurrenceID : null;
                    apptmodel.RecurrenceID = $scope.Recurrence != null ? $scope.Recurrence.RecurrenceID : null;
                    apptmodel.IsRecurringAptEdit = true;
                }

                return apptmodel;
            }

            $scope.formatTime = function ( time, ampm ) {
                var hours = parseInt( time.toString().substring( 0, 2 ) );
                var mins = parseInt( time.toString().substring( 2 ) );
                if ( ampm == "PM" && hours < 12 ) hours = hours + 12;
                if ( ampm == "AM" && hours == 12 ) hours = hours - 12;
                var sHours = hours.toString();
                var sMinutes = mins.toString();
                if ( hours < 10 ) sHours = "0" + sHours;
                if ( mins < 10 ) sMinutes = "0" + sMinutes;
                return sHours + sMinutes;
            }

            $scope.validateStartVsEndDateTime = function () {
                var startdatetime = $scope.getStartDateTime( $scope.blockedTime );
                var enddatetime = $scope.getEndDateTime( $scope.blockedTime );

                //check for both fromdate and todate are equlal both time and date
                if ( startdatetime.toString() == enddatetime.toString() ) {
                    return "From and to Date and time Should not be the Same! Please correct the date fields and try again."
                }
                else if ( startdatetime > enddatetime ) {
                    return "From Date should not be greater than ToDate! Please correct the date fields and try again."
                }
            }

            $scope.getTimeDiffInMins = function ( blockedTime ) {
                var startdatetime = $scope.getStartDateTime( blockedTime );
                var enddatetime = $scope.getEndDateTime( blockedTime );
                var diff = enddatetime - startdatetime;
                return Math.round( diff / 60000 );
            }

            $scope.getStartDateTime = function ( blockedTime ) {
                var starttime = $scope.formatTime( blockedTime.StartTime, blockedTime.StartAmPm );
                var starthours = parseInt( starttime.toString().substring( 0, 2 ) );
                var startmins = parseInt( starttime.toString().substring( 2 ) );
                var startdatetime = new Date( blockedTime.StartDate.getFullYear(), blockedTime.StartDate.getMonth(),
                    blockedTime.StartDate.getDate(), starthours, startmins, 0, 0 );
                return startdatetime;
            }

            $scope.getEndDateTime = function ( blockedTime ) {
                var endtime = $scope.formatTime( blockedTime.EndTime, blockedTime.EndAmPm );
                var endhours = parseInt( endtime.toString().substring( 0, 2 ) );
                var endmins = parseInt( endtime.toString().substring( 2 ) );
                var enddatetime = new Date( blockedTime.EndDate.getFullYear(), blockedTime.EndDate.getMonth(),
                blockedTime.EndDate.getDate(), endhours, endmins, 0, 0 );
                return enddatetime;
            }

            $scope.getAmPmValue = function ( militarytime ) {
                var inttime = parseInt( militarytime );
                if ( inttime > 1200 )
                    return "PM";
                else
                    return "AM";
            }

            $scope.getStandardTime = function ( militarytime, isdisplay ) {
                militarytime = $scope.pad( militarytime, 4 );
                var hours = parseInt( militarytime.toString().substring( 0, 2 ) );
                var mins = parseInt( militarytime.toString().substring( 2 ) );
                var dt = new Date( 99, 1, 1, hours, mins, 0, 0 );
                var obj = new Object();
                obj.date = dt;
                obj.hrsmins = ( isdisplay ) ? $filter( 'toMMDDYYYYDate' )( dt, 'hh:mm', 'useLocal' ) :
                                            $filter( 'toMMDDYYYYDate' )( dt, 'hhmm', 'useLocal' );
                return obj;
            }

            $scope.addMinutes = function ( date, minutes ) {
                var dt = new Date( date );
                return new Date( dt.getTime() + minutes * 60000 );
            }

            $scope.pad = function ( num, size ) {
                var s = num + "";
                while ( s.length < size ) s = "0" + s;
                return s;
            }

            $scope.formatFacilityBlockedTimes = function ( data ) {
                // Format incmomming data to blocked times
                var tmpblockedlist = [];
                angular.forEach( data, function ( item, key ) {
                    var blockedTimeModel = {};
                    blockedTimeModel.BlockedTimeID = item.AppointmentID;
                    blockedTimeModel.StartDate = ( new moment( item.AppointmentDate ) )._d;
                    blockedTimeModel.StartAmPm = $scope.getAmPmValue( item.AppointmentStartTime );
                    blockedTimeModel.StartTime = $scope.getStandardTime( item.AppointmentStartTime ).hrsmins;
                    blockedTimeModel.StartTimeAmPm = $scope.getStandardTime( blockedTimeModel.StartTime, true ).hrsmins + ' ' + blockedTimeModel.StartAmPm;
                    var enddt = new Date( blockedTimeModel.StartDate.getFullYear(), blockedTimeModel.StartDate.getMonth(), blockedTimeModel.StartDate.getDate(), ( $scope.getStandardTime( item.AppointmentStartTime ) ).date.getHours(), ( $scope.getStandardTime( item.AppointmentStartTime ) ).date.getMinutes() );
                    blockedTimeModel.EndDate = $scope.addMinutes( enddt, item.AppointmentLength ); var endtimemilitary = $scope.pad( blockedTimeModel.EndDate.getHours(), 2 ).toString() + $scope.pad( blockedTimeModel.EndDate.getMinutes(), 2 ).toString();
                    blockedTimeModel.EndAmPm = $scope.getAmPmValue( endtimemilitary );
                    blockedTimeModel.EndTime = $scope.getStandardTime( endtimemilitary ).hrsmins;
                    blockedTimeModel.EndTimeAmPm = $scope.getStandardTime( blockedTimeModel.EndTime, true ).hrsmins + ' ' + blockedTimeModel.EndAmPm;
                    blockedTimeModel.Reason = item.ReasonForVisit;
                    blockedTimeModel.Comment = item.Comments;
                    blockedTimeModel.AllDay = $scope.getAllDayValue(blockedTimeModel);
                    blockedTimeModel.NumberOfOccurences = item.NumberOfOccurences;
                    blockedTimeModel.RecurrenceDay = item.RecurrenceDay;
                    blockedTimeModel.RecurrenceFrequency = item.RecurrenceFrequency;
                    if (item.RecurrenceEndDate != null) {
                        var dt = (new moment(item.RecurrenceEndDate))._d;
                        blockedTimeModel.RecurrenceEndDate = dt;
                    }

                    tmpblockedlist.push( blockedTimeModel );
                } );
                return tmpblockedlist;

                // TMP DEBUG
                //var tmpblockedlist = [];
                //var recurrence = {
                //    Occurence: 10,
                //    Day: 'WED, FRI',
                //    Frequency: 'Weekly',
                //    EndDate: new Date(17, 2, 2)
                //};
                //var tmpblockedTime = {
                //    BlockedTimeID: 33,
                //    Reason: "Holiday",
                //    StartDate: new Date(),
                //    StartTime: new Date(),
                //    StartAmPm: 'AM',
                //    EndDate: new Date(99, 1, 1),
                //        EndTime: new Date(99, 1, 1, 15, 35, 50),
                //    EndAmPm: "PM",
                //    Comment: "Saved comment"
                //};
                //tmpblockedTime.Recurrence = recurrence;
                //tmpblockedlist.push(tmpblockedTime);

                //var tmpbockedTime2 = {
                //    BlockedTimeID: 44,
                //    Reason: "Day Off",
                //    StartDate: new Date(),
                //    StartTime: new Date(),
                //    StartAmPm: 'PM',
                //    EndDate: new Date(91, 4, 7),
                //        EndTime: new Date(91, 4, 7, 13, 20, 50),
                //    EndAmPm: "PM",
                //    Comment: "Comment on year 91"
                //};
                //tmpbockedTime2.Recurrence = recurrence;
                //tmpblockedlist.push(tmpbockedTime2); 
                //return tmpblockedlist;
            }

            $scope.onAllDayClick = function () {
                // Disable time and ampm fields
                var alldayval = !$scope.blockedTime.AllDay;
                if ( alldayval ) {
                    $( '#startTime' ).prop( 'disabled', 'disabled' );
                    $( '#startAMPM' ).prop( 'disabled', 'disabled' );
                    $( '#endTime' ).prop( 'disabled', 'disabled' );
                    $( '#endAMPM' ).prop( 'disabled', 'disabled' );
                    $( '#endDate' ).prop( 'disabled', 'disabled' );
                    $scope.isDisabled = true;

                    // Set times from 12am to 11:59pm and enddate to same value as start date
                    $scope.blockedTime.EndDate = $scope.blockedTime.StartDate;
                    $scope.blockedTime.StartTime = '1200';
                    $scope.blockedTime.StartAmPm = 'AM';
                    $scope.blockedTime.EndTime = '1159';
                    $scope.blockedTime.EndAmPm = 'PM';
                } else {
                    $( '#startTime' ).removeAttr( 'disabled' );
                    $( '#startAMPM' ).removeAttr( 'disabled' );
                    $( '#endTime' ).removeAttr( 'disabled' );
                    $( '#endAMPM' ).removeAttr( 'disabled' );
                    $('#endDate').removeAttr('disabled');
                    $scope.blockedTime.StartTime = '';
                    $scope.blockedTime.StartAmPm = '';
                    $scope.blockedTime.EndTime = '';
                    $scope.blockedTime.EndAmPm = '';
                    $scope.isDisabled = false;
                }
                $scope.validateAppointmentTime();
            }

            $scope.validateAppointmentDate = function () {
                if ( $scope.blockedTime.StartDate !== undefined && $scope.blockedTime.StartDate != '' ) {
                    if ( $scope.blockedTime.StartDate >= currentDate ) {
                        $( "#startdateOfAppointmentError" ).addClass( 'ng-hide' );
                        $( "#startDateContainer" ).removeClass( "has-error" );
                        $scope.ctrl.scheduleForm.$valid = true;
                    }
                    else {
                        $( "#startdateOfAppointmentError" ).removeClass( 'ng-hide' );
                        $( "#startDateContainer" ).addClass( "has-error" );
                        $scope.ctrl.scheduleForm.$valid = false;
                    }
                }

                if ( $scope.blockedTime.EndDate !== undefined && $scope.blockedTime.EndDate != '' ) {
                    if ( $scope.blockedTime.EndDate >= currentDate ) {
                        $( "#enddateOfAppointmentError" ).addClass( 'ng-hide' );
                        $( "#endDateContainer" ).removeClass( "has-error" );
                        $scope.ctrl.scheduleForm.$valid = true;
                    }
                    else {
                        $( "#enddateOfAppointmentError" ).removeClass( 'ng-hide' );
                        $( "#endDateContainer" ).addClass( "has-error" );
                        $scope.ctrl.scheduleForm.$valid = false;
                    }
                }
            }

            $scope.validateAppointmentTime = function () {
                $timeout( function () {
                    var valid1 = $scope.validateTime( true );
                    var valid2 = $scope.validateTime( false );
                } );
            }

            $scope.validateTime = function ( isStartTime ) {
                var max = 2359;
                var min = 00;
                var format = ( isStartTime ) ? $scope.blockedTime.StartAmPm : $scope.blockedTime.EndAmPm;
                var time = 0;
                var isValid = false;

                if ( $scope.ctrl == undefined || $scope.ctrl.scheduleForm == undefined ||
                    format == undefined || format == '' ||
                    $scope.blockedTime.StartDate == undefined || $scope.blockedTime.StartDate == '' ||
                    $scope.blockedTime.EndDate == undefined || $scope.blockedTime.EndDate == '' ||
                    $scope.blockedTime.StartTime == undefined || $scope.blockedTime.StartTime == '' ||
                    $scope.blockedTime.EndTime == undefined || $scope.blockedTime.EndTime == ''
                  ) {
                    $scope.ctrl.scheduleForm.startTime.$setValidity( "startTimeError", true );
                    $scope.ctrl.scheduleForm.endTime.$setValidity( "startTimeError", true );
                    return false;
                }
                if ( $scope.blockedTime.StartTime == "0000" || $scope.blockedTime.StartTime == 0000 ) {
                    $scope.ctrl.scheduleForm.startTime.$setValidity( "startTimeError", false );
                    return false;
                }
                if ( $scope.blockedTime.EndTime == "0000" || $scope.blockedTime.EndTime == 0000 ) {
                    $scope.ctrl.scheduleForm.endTime.$setValidity( "startTimeError", false );
                    return false;
                }

                if ( isStartTime == true ) {
                    if ( $scope.blockedTime.StartTime > 1259 ) {
                        $scope.ctrl.scheduleForm.startTime.$setValidity( "startTimeError", false );
                        return false;
                    }
                    else {
                        time = $filter( 'toMilitaryTime' )( $scope.blockedTime.StartTime, format );
                        isValid = time >= min && time < max;
                        $scope.ctrl.scheduleForm.startTime.$setValidity( "startTimeError", isValid );
                        return isValid;
                    }
                }
                else {
                    if ( $scope.blockedTime.EndTime > 1259 ) {
                        $scope.ctrl.scheduleForm.endTime.$setValidity( "startTimeError", false );
                        return false;
                    }
                    else {
                        format = $scope.blockedTime.EndAmPm;
                        time = $filter( 'toMilitaryTime' )( $scope.blockedTime.EndTime, format );
                        isValid = time > min && time <= max;
                        $scope.ctrl.scheduleForm.endTime.$setValidity( "startTimeError", isValid );
                        return isValid;
                    }
                }
                return false;
            }

            $scope.syncDateAndLength = function () {
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

            $scope.getAllDayValue = function ( blockedTimeModel ) {
                if ( blockedTimeModel.StartTime == '12:00' && blockedTimeModel.StartAmPm == 'AM' &&
                   blockedTimeModel.EndTime == '11:59' && blockedTimeModel.EndAmPm == 'PM' )
                    return true;
                else
                    return false;
            }

            $scope.resetFields = function () {
                $scope.isUpdate = false;
                $scope.blockedTime.BlockedTimeID = 0;
                $scope.blockedTime.Reason = '';
                $scope.blockedTime.StartDate = new Date();
                $scope.blockedTime.StartTime = '';
                $scope.blockedTime.StartAmPm = '';
                $scope.blockedTime.EndDate = new Date();
                $scope.blockedTime.EndTime = '';
                $scope.blockedTime.EndAmPm = '';
                $scope.blockedTime.Comment = '';
                $scope.blockedTime.AllDay = false;
                $( '#startTime' ).removeAttr( 'disabled' );
                $( '#startAMPM' ).removeAttr( 'disabled' );
                $( '#endTime' ).removeAttr( 'disabled' );
                $( '#endAMPM' ).removeAttr( 'disabled' );
                $( '#endDate' ).removeAttr( 'disabled' );

                $scope.Recurrence = $scope.NewRecurrence;
                $scope.userBlockTimeID = 0;
                $scope.isDisabled = false;
                if ($scope.ctrl.scheduleForm)
                    $rootScope.formReset( $scope.ctrl.scheduleForm, $scope.ctrl.scheduleForm.$name );
            }

            $scope.editBlockedTime = function ( item ) {
                $scope.isUpdate = true;
                $scope.userBlockTimeID = item.BlockedTimeID;
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
                if ( item.AllDay == true ) {
                    $( '#startTime' ).prop( 'disabled', 'disabled' );
                    $( '#startAMPM' ).prop( 'disabled', 'disabled' );
                    $( '#endTime' ).prop( 'disabled', 'disabled' );
                    $( '#endAMPM' ).prop( 'disabled', 'disabled' );
                    $( '#endDate' ).prop( 'disabled', 'disabled' );
                }

                angular.forEach($scope.appointments, function (appointment) {
                    if (appointment.AppointmentID == item.BlockedTimeID) {
                        $scope.Recurrence = appointment.Recurrence;
                        if ($scope.Recurrence != null)
                            $scope.Recurrence.StartDate = $scope.blockedTime.StartDate;
                        if (appointment != null && $scope.Recurrence != null && $scope.Recurrence.RecurrenceID == null) {
                            $scope.Recurrence.RecurrenceID = appointment.RecurrID;
                        }
                    }
                });
            }

            $scope.getBlockedTimeCopy = function ( item ) {
                var cpyobj = {};
                cpyobj.BlockedTimeID = item.BlockedTimeID;
                cpyobj.Reason = item.Reason;
                cpyobj.StartDate = item.StartDate;
                cpyobj.StartTime = item.StartTime;
                cpyobj.StartAmPm = item.StartAmPm;
                cpyobj.StartTimeAmPm = $scope.getStandardTime( cpyobj.StartTime, true ).hrsmins + ' ' + cpyobj.StartAmPm;
                cpyobj.EndDate = item.EndDate;
                cpyobj.EndTime = item.EndTime;
                cpyobj.EndAmPm = item.EndAmPm;
                cpyobj.EndTimeAmPm = $scope.getStandardTime( cpyobj.EndTime, true ).hrsmins + ' ' + cpyobj.EndAmPm;
                cpyobj.Comment = item.Comment;
                cpyobj.AllDay = item.AllDay;

                // TODO: Add recurrence
                return cpyobj;
            }

            $scope.recurrenceChanged = function () {
                if ($scope.Recurrence.IsRecurring) {
                    $scope.Recurrence.StartDate = new Date($scope.blockedTime.StartDate.getFullYear(), $scope.blockedTime.StartDate.getMonth(), $scope.blockedTime.StartDate.getDate());
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

            $scope.removeFromModelAndGet = function(id){

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
                    return $scope.getBlockedTimes().then(function () {
                        $scope.resetFields();
                        alertService.success('Blocked time has been successfully removed.');
                    });
                }
                else {
                    alertService.error('There was an error in deleting the blocked time!');
                    return $scope.promiseNoOp();
                }
            }

            $scope.deleteBlockedTime = function ( id ) {
                bootbox.confirm( "Selected blocked time will be deleted. Do you want to continue?", function ( result ) {
                    if (result === true) {

                        var foundappt = $filter('filter')($scope.appointments, function (item, index) {
                            if (item.AppointmentID == id) {
                                return item;
                            }
                        })[0];
                        if (foundappt != null) {

                            if (foundappt.Recurrence == null) {
                                $scope.appointmentService.deleteAppointment(0, id).then(function (resp) {
                                    if (resp.ResultCode === 0)
                                        $scope.removeFromModelAndGet(id);
                                    else {
                                        alertService.error('There was an error in deleting the blocked time!');
                                        return $scope.promiseNoOp();
                                    }
                                });
                            }
                            else {

                                if (foundappt.RecurrenceID == null)
                                    foundappt.RecurrenceID = foundappt.RecurrID;

                                $scope.appointmentService.deleteAppointmentsByRecurrence(foundappt.RecurrenceID).then(function (resp) {
                                    if (resp.ResultCode === 0)
                                        $scope.removeFromModelAndGet(id);
                                    else {
                                        alertService.error('There was an error in deleting the blocked time!');
                                        return $scope.promiseNoOp();
                                    }
                                },
                                function (errorStatus) {
                                    alertService.error('There was an error in deleting the blocked time!');
                                    return $scope.promiseNoOp();
                                });
                            }
                        }
                        else {
                            alertService.error('There was an error in deleting the blocked time!');
                            return $scope.promiseNoOp();
                        }
                    }
                } );
            }

            $scope.isFormValid = function () {
                if ( $scope.blockedTime.StartDate == '' || $scope.blockedTime.EndDate == '' ||
                    $scope.blockedTime.StartAmPm == '' || $scope.blockedTime.EndAmPm == '' ||
                    $scope.blockedTime.Reason == '' )
                    return false;
                else
                    return true;
            }

            $scope.save = function ( isNext, mandatory, hasErrors ) {
                if ( !mandatory && isNext && hasErrors ) {
                    $scope.handleNextState();
                    return;
                }

                if (!$scope.ctrl.scheduleForm.$dirty && isNext && !hasErrors) {
                    $scope.handleNextState();
                    return;
                }

                if ( !$scope.isFormValid() ) {
                    bootbox.alert( 'Please fill out all required fields!' );
                    return;
                }

                var message = $scope.validateStartVsEndDateTime();

                if ( message != undefined ) {
                    alertService.error(message);
                    //$scope.ctrl.scheduleForm.startDate.$setValidity("date", false);
                    //$scope.ctrl.scheduleForm.startTime.$setValidity("startTimeError", false);
                    //$scope.ctrl.scheduleForm.endDate.$setValidity("date", false);
                    //$scope.ctrl.scheduleForm.startTime.$setValidity("startTimeError", false);


                    $("#startDateContainer").addClass("has-error");
                    $scope.ctrl.scheduleForm.startDate.$setValidity("date", false);

                    $("#startdateOfAppointmentError").addClass("has-error");
                    $scope.ctrl.scheduleForm.startTime.$setValidity("invalid", false);

                    $("#endDateContainer").addClass("has-error");
                    $scope.ctrl.scheduleForm.endDate.$setValidity("date", false);


                    $("#enddateOfAppointmentError").addClass("has-error");
                    $scope.ctrl.scheduleForm.endTime.$setValidity("invalid", false);
                    return;
                }

                if ( $scope.ctrl.scheduleForm.$dirty && !hasErrors ) {
                    var q = $q.defer();

                    if ( $scope.isUpdate ) {
                        $scope.appointmentService.updateAppointment( $scope.getApptModel() ).then( function ( response ) {
                            if ( response.ResultCode === 0 ) {
                                // Now add an appt resource if needed
                                $scope.appointments = response.DataItems;

                                if ($scope.Recurrence != null && $scope.Recurrence.RecurrenceID == null)
                                    $scope.Recurrence.RecurrenceID = response.DataItems[0].RecurrenceID;

                                var innerPromises = [];
                                angular.forEach(response.DataItems, function (appointment) {
                                    innerPromises.push($scope.appointmentService.getAppointmentResource(appointment.AppointmentID).then(function (resp) {
                                        if (resp.ResultCode === 0) {
                                            if (resp.DataItems.length == 0)
                                                $scope.addAppointmentResource(appointment.AppointmentID);
                                            $scope.blockedTime.BlockedTimeID = appointment.AppointmentID;
                                        } else {
                                            alertService.error('Error while retrieving an appointment resource! Please reload the page and try again.');
                                        }
                                    }));
                                });

                                $q.all(innerPromises).then(function (results) {
                                    $scope.getBlockedTimes().then(function (data) {
                                        alertService.success('Blocked time updated successfully!');
                                        $scope.resetFields();
                                        resetForm();

                                        if (isNext)
                                            $scope.handleNextState();
                                    });
                                });
                            } else {
                                alertService.error( 'Error while saving a blocked time! Please reload the page and try again.' );
                            }
                        } );
                    } else {
                        $scope.appointmentService.addAppointment( $scope.getApptModel() ).then( function ( response ) {
                            if ( response.ResultCode === 0 ) {
                                // Now add an appt resource if needed
                                $scope.appointments = response.DataItems;

                                if ( $scope.Recurrence != null  && $scope.Recurrence.RecurrenceID == null)
                                    $scope.Recurrence.RecurrenceID = response.DataItems[0].RecurrenceID;

                                var innerPromises = [];
                                angular.forEach(response.DataItems, function (appointment) {
                                    innerPromises.push($scope.appointmentService.getAppointmentResource( appointment.AppointmentID ).then( function ( resp ) {
                                        if ( resp.ResultCode === 0 ) {
                                            if ( resp.DataItems.length == 0 ) 
                                                $scope.addAppointmentResource( appointment.AppointmentID );
                                            $scope.blockedTime.BlockedTimeID = appointment.AppointmentID;
                                        } else {
                                            alertService.error( 'Error while retrieving an appointment resource! Please reload the page and try again.' );
                                        }
                                    }));
                                } );

                                $q.all(innerPromises).then(function(results) {
                                    $scope.getBlockedTimes().then(function (data) {
                                        alertService.success('Blocked time saved successfully!');
                                        $scope.resetFields();
                                        resetForm();

                                        if (isNext)
                                            $scope.handleNextState();
                                    });
                                });
                            } else {
                                alertService.error( 'Error while saving a blocked time! Please reload the page and try again.' );
                            }
                        } );
                    }
                    return q.promise;
                }
            }

            $scope.addAppointmentResource = function ( appointmentID ) {
                $scope.appointmentService.addAppointmentResource( $scope.getApptResourceModel( appointmentID ) ).then( function ( response ) {
                    if ( response.ResultCode === 0 ) {
                    } else {
                        alertService.error( 'Error while saving an appointment resource! Please reload the page and try again.' );
                    }
                } );
            }

            $scope.getBlockedTimes = function () {
                var obj = { stateName: $state.current.name, validationState: 'warning' };
                return $scope.appointmentService.getBlockedTimeAppointments($scope.ResourceID, FACILITY_RESOURCE_TYPE).then(function (data) {
                    if (data.ResultCode === 0 && data.DataItems != null && data.DataItems.length > 0) {
                        $scope.appointments = data.DataItems;
                        $scope.blockedTimesList = [];
                        $scope.blockedTimesList = ($scope.formatFacilityBlockedTimes(data.DataItems));
                        obj = { stateName: $state.current.name, validationState: 'valid' };

                        // Get appt recurrence info
                        $scope.appointmentService.getAppointmentByResource($scope.ResourceID, FACILITY_RESOURCE_TYPE).then(function (data) {
                            if (data.ResultCode === 0 && data.DataItems != null && data.DataItems.length > 0) {

                                angular.forEach(data.DataItems, function (dataitem) {
                                    if (dataitem.RecurrenceID != null) {
                                        var foundappt = $filter('filter')($scope.appointments, function (item, index) {
                                            if (item.AppointmentID == dataitem.AppointmentID) {
                                                return item;
                                            }
                                        })[0];
                                        if (foundappt != null)
                                            foundappt.Recurrence = dataitem.Recurrence;
                                    }
                                })
                            }
                        });

                        if (FACILITY_RESOURCE_TYPE === 2) {
                            $rootScope.staffManagementRightNavigationHandler(obj);
                        }
                    }
                    else {
                        var obj = { stateName: $state.current.name, validationState: 'warning' };
                        $rootScope.staffManagementRightNavigationHandler(obj);
                    }
                },
                function ( errorStatus ) {
                    alertService.error( 'Unable to connect to server' );
                } );
            };

            $scope.handleNextState = function () {
                var nextState = angular.element( "li[data-state-name].list-group-item.active" ).next( "li[data-state-name].list-group-item" );
                if ( nextState.length === 0 )
                    $scope.Goto( '^' );
                else {
                    $timeout( function () {
                        $rootScope.setform( false );
                        var nextStateName = nextState.attr( 'data-state-name' );
                        $scope.Goto( nextStateName, { UserID: $scope.ResourceID } );
                    } );
                }
            };

            $scope.init();
        }] );