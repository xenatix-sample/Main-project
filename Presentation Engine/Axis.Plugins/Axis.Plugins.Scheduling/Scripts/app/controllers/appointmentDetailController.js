angular.module( 'xenatixApp' )
    .controller( 'appointmentDetailController', ['$scope', 'appointmentService', 'alertService', '$stateParams', '$state', '$filter', '$rootScope', 'resourceService', '$q', 'contactAddressService', 'lookupService'
    , function ( $scope, appointmentService, alertService, $stateParams, $state, $filter, $rootScope, resourceService, $q, contactAddressService, lookupService ) {
        $scope.isLoading = true;
        var undefined = 'undefined';
        var appointmentTable = $( "#appointmentTable" );
        $scope.externalSources = [];
        $scope.locations = [];
        $scope.rooms = [];
        $scope.appointmentResources = [];
        $scope.appointmentToEdit = {};
        var locationCounter = 0;

        $scope.initializeBootstrapTable = function () {
            $scope.tableoptions = {
                pagination: true,
                pageSize: 10,
                pageList: [10, 25, 50, 100],
                search: false,
                showColumns: true,
                data: [],
                undefinedText: '',
                //onClickRow: function (e, row, $element) {
                //    row.find("[data-default-action]").triggerHandler('click');
                //},
                columns: [
                     {
                         field: 'AppointmentStatusID',
                         title: 'Status',
                         formatter: function (value, row) {
                             if (row.IsCancelled)
                                 return "Cancelled";

                             if (value) {
                                 var txtStatus = $filter( 'filter' )( $scope.AppointmentStatusList, { AppointmentStatusID: value }, true )[0].AppointmentStatus
                                 if ( txtStatus != undefined )
                                     return txtStatus;
                                 else
                                     return ""
                             }
                             else {
                                 return "";
                             }
                         }
                     },
                    {
                        field: 'AppointmentDate',
                        title: 'Day',
                        formatter: function ( value, row, index ) {
                            return getDay( value );
                        }
                    },
                    {
                        field: 'AppointmentDate',
                        title: 'Date',
                        formatter: function ( value, row, index ) {
                            return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY', 'useLocal');;
                        }
                    },
                    {
                        field: 'AppointmentStartTime',
                        title: 'Start',
                        formatter: function ( value, row, index ) {
                            return getStart( value );
                        }
                    },
                    {
                        field: 'AppointmentLength',
                        title: 'End',
                        formatter: function ( value, row, index ) {
                            return getEnd( value, row );
                        }
                    },
                    {
                        field: 'AppointmentTypeID',
                        title: 'Appointment Type',
                        formatter: function (value, row, index) {
                            if (row.IsGroupAppointment)
                                return row.GroupType;
                            else
                                return getAppointmentType( value, row, index );
                        }
                    },
                    {
                        field: 'Providers',
                        title: 'Provider'
                    },
                    {
                        field: 'LocationName',
                        title: 'Location',
                        formatter: function ( value, row, index ) {
                            return getLocation( value, row, index );
                        }
                    },
                   {
                       field: 'AppointmentID',
                       title: '',
                       formatter: function (value, row, index) {
                           var path;
                           var apptStatus = '';
                           if (row != null && row.AppointmentStatusID != null)
                               apptStatus = $filter('filter')($scope.AppointmentStatusList, { AppointmentStatusID: row.AppointmentStatusID }, true)[0].AppointmentStatus
                           var isHidden = $scope.IsHidden(row);
                           if (row.IsGroupAppointment){ 
                               // Group appt: show 'edit' and 'cancel' states (selectively)
                               var editicon = '<a href="javascript:void(0)" data-default-action id="editAppointment" name="editAppointment" data-toggle="modal" ng-click="Goto(\'scheduling.groupscheduling.details.groupschedule\', { GroupID:' + row.GroupID + ',ReadOnly: \'edit\', IsRecurringAptEdit: \'false\' })" title="Edit" space-key-press><i class="fa fa-pencil fa-fw" /></a>';
                               if (row.RecurrenceID != null)
                                   editicon = '<a data-default-no-action ng-click="showSingleOrRecurrigApptModal(' + value + ')" alt="Edit" title="Edit"><i class="fa fa-pencil fa-fw" /></a>';
                               var viewicon = '<a data-default-no-action ui-sref="scheduling.groupscheduling.details.groupschedule({  GroupID: ' + row.GroupID + ', ReadOnly: \'view\' })" alt="View" title="View"><i class="fa fa-eye fa-fw" /></a>';
                               path = viewicon + editicon;
                               if (apptStatus.indexOf('Cancel') >= 0 || ($scope.IsDateInThePast(row) && row.RecurrenceID == null))
                                   path = viewicon;
                               return path + '<a href="javascript:void(0)" ng-if="' + isHidden + '" ng-click="deleteAppointmentBootbox(' + value + ')"   data-default-no-action id="cancelAppointment" name="cancelAppointment" data-toggle="modal"  title="Cancel Appointment" space-key-press><i class="fa fa-ban fa-fw" /></a>';
                           }
                           else {
                               // For recurring appts, show the modal
                               var editicon = '<a data-default-no-action  ui-sref="patientprofile.appointments.editAppointment({ AppointmentID:' + value + ', IsRecurringAptEdit: \'false\' })" alt="Edit" title="Edit"><i class="fa fa-pencil fa-fw" /></a>';
                               if (row.RecurrenceID != null)
                                   editicon = '<a data-default-no-action ng-click="showSingleOrRecurrigApptModal(' + value + ')" alt="Edit" title="Edit"><i class="fa fa-pencil fa-fw" /></a>';
                               var viewicon = '<a data-default-no-action ui-sref="patientprofile.appointments.viewAppointment({ ReadOnly: \'view\', AppointmentID:' + value + ' })" alt="View" title="View"><i class="fa fa-eye fa-fw" /></a>';
                               path = viewicon + editicon;
                               // NOTE: added condition: If this is a non-recurring appt and it's in the past, only allow ability to view
                               if (apptStatus == 'No Show' || apptStatus == 'Check In' || apptStatus == 'Check Out' || apptStatus.indexOf('Cancel') >= 0 || ($scope.IsDateInThePast(row) && row.RecurrenceID == null)){
                                   // For these states only show the 'view' icon
                                   path = viewicon;
                               }
                               return path + '<a href="javascript:void(0)" ng-if="' + isHidden + '" ng-click="deleteAppointmentBootbox(' + value + ')"   data-default-no-action id="cancelAppointment" name="cancelAppointment" data-toggle="modal"  title="Cancel Appointment" space-key-press><i class="fa fa-ban fa-fw" /></a>';
                           }
                           
                       }
                   }

                ]
            };
        };

        $scope.IsDateInThePast = function (row) {
            var curdate = new Date();

            var tmpaptdate = moment(row.AppointmentDate)._d;
            var aptdate = new Date(tmpaptdate.getFullYear(), tmpaptdate.getMonth(), tmpaptdate.getDate(), getTime(row.AppointmentStartTime).getHours(), getTime(row.AppointmentStartTime).getMinutes());

            if (aptdate < curdate)
                return true;
            return false;
        }

        $scope.IsHidden = function (row) {
            if(row.IsGroupAppointment || row.IsCancelled)
                return false;

            if (row.AppointmentStatusID == null || row.AppointmentStatusID == undefined)
                return true;
            else
                return false;
        }

        $scope.cancelSingleOrRecurrentModal = function () {
            $scope.appointmentToEdit = {};
            $('#editSingleOrRecurrentAptModal').modal('hide');
        }

        $scope.showSingleOrRecurrigApptModal = function (value) {

            $scope.appointmentToEdit = $scope.appointmentList.filter(function (obj) {
                return obj.AppointmentID == value;
            })[0];
            $scope.appointmentToEdit.IsRecurrentAppointment = false;
            $('#editSingleOrRecurrentAptModal').modal('show');
        }

        $scope.editSingleOrRecurrigAppt = function () {
            $('#editSingleOrRecurrentAptModal').modal('hide');
            $('body,html').removeClass('modal-open');

            if (!$scope.appointmentToEdit.IsGroupAppointment) {
                // Single instance of a recurring apt: view mode for past date - 
                if (!$scope.appointmentToEdit.IsRecurrentAppointment && $scope.IsDateInThePast($scope.appointmentToEdit))
                    $scope.Goto('patientprofile.appointments.viewAppointment', { ReadOnly: 'view', AppointmentID: $scope.appointmentToEdit.AppointmentID, IsRecurringAptEdit: $scope.appointmentToEdit.IsRecurrentAppointment });
                else
                    $scope.Goto('patientprofile.appointments.editAppointment', { AppointmentID: $scope.appointmentToEdit.AppointmentID, IsRecurringAptEdit: $scope.appointmentToEdit.IsRecurrentAppointment });
            } else {
                // Single instance of a recurring apt: view mode for past date - 
                if (!$scope.appointmentToEdit.IsRecurrentAppointment && $scope.IsDateInThePast($scope.appointmentToEdit))
                    $scope.Goto('scheduling.groupscheduling.details.groupschedule', {GroupID: $scope.appointmentToEdit.GroupID, ReadOnly: 'view', IsRecurringAptEdit: $scope.appointmentToEdit.IsRecurrentAppointment });
                else
                    $scope.Goto('scheduling.groupscheduling.details.groupschedule', { GroupID: $scope.appointmentToEdit.GroupID, ReadOnly: 'edit', IsRecurringAptEdit: $scope.appointmentToEdit.IsRecurrentAppointment });
}
        }

        $scope.deleteAppointmentBootbox = function ( value ) {
            //$event.stopPropagation();
            bootbox.confirm( "Are you sure that you want to cancel this appointment?", function ( result ) {
                if ( result === true ) {
                    //$state.go('patientprofile.appointments.cancelAppointment', { ReadOnly: 'view', AppointmentID: value })
                    $scope.appointmentToCancel = $scope.appointmentList.filter(function (obj) {
                        return obj.AppointmentID == value;
                    })[0];
                    $scope.appointmentToCancel.IsCancelAllAppoitment = false;
                    $scope.appointmentToCancel.CancelReasonID = 0;
                    $scope.appointmentToCancel.CancelComment = '';
                    if ($scope.appointmentToCancel.RecurrenceID === null)
                        $scope.appointmentToCancel.IsCancelAllAppoitment = false;
                    $scope.$digest();
                    $('#cancelGroupScheduleModel').modal('show');
                }
                else {
                    return;
                }
            } )
        }

        $scope.getAddress = function () {
            var q = $q.defer();
            $scope.isLoading = true;
            return contactAddressService.get( $stateParams.ContactID ).then( function ( data ) {
                if ( data.DataItems.length > 0 ) {
                    $scope.Address = data.DataItems;
                    angular.forEach( $scope.Address, function ( param ) {
                        var isExist = $filter( 'filter' )( $scope.locations, { LocationID: param.AddressID, type: 4 } )[0];
                        if ( !isExist ) {
                            $scope.locations.push( {
                                ID: locationCounter++,
                                LocationID: param.AddressID,
                                Name: param.Line1 + " " + param.Line2,
                                type: 4       //'Resource Type :'External' ID:4'
                            } );
                        }
                    } )
                }
                $scope.isLoading = false;
                q.resolve();
            },
             function ( errorStatus ) {
                 $scope.isLoading = false;
                 alertService.error( 'Unable to connect to server' );
                 q.reject();
             } );
            return q.promise;
        };

        $scope.getFacility = function ( typeName ) {
            $scope.facilityData = $rootScope.getLookupsByType( 'Facility' );
            angular.forEach( $scope.facilityData, function ( param ) {
                var isExist = $filter( 'filter' )( $scope.locations, { LocationID: param.ID, type: 1 } )[0];
                if ( !isExist ) {
                    $scope.locations.push( {
                        ID: locationCounter++,
                        LocationID: param.ID,
                        Name: param.Name,
                        type: 1
                    } );
                }
            } )
        };

        $scope.getLocations = function () {
            var q = $q.defer();
            $scope.getAddress().then( function () {
                $scope.getFacility();
                q.resolve();
            } );
            return q.promise;
        }

        $scope.init = function () {
            $scope.contactID = $stateParams.ContactID;
            $scope.$parent['autoFocus'] = true;
            $scope.AppointmentStatusList = lookupService.getLookupsByType( 'AppointmentStatus' );

            if ( ( $scope.contactID !== 0 ) && ( $scope.contactID != null ) && ( $scope.contactID != 'undefined' ) ) {
                $scope.getLocations().then( function () {
                    $scope.getAppointmentDetails( $scope.contactID );
                } );
            }
            $scope.initializeBootstrapTable();
        };

        $scope.addNew = function () {
            $state.go( 'patientprofile.appointments.newAppointment', { ContactID: $stateParams.ContactID } );
        }

        var getExternalSources = function () {
            resourceService.getResources( 2 ).then( function ( data ) {
                if ( data.DataItems != null && data.DataItems.length > 0 ) {
                    $scope.externalSources = data.DataItems;
                }
            }, function ( errorStatus ) { } );
        };

        var getResources = function ( appoitmentID ) {
            var q = $q.defer();
            var isExist = $filter( 'filter' )( $scope.appointmentResources, { AppointmentID: appoitmentID }, true )[0];
            if ( !isExist ) {
                appointmentService.getAppointmentResource( appoitmentID ).then( function ( res ) {
                    angular.forEach( res.DataItems, function ( item ) {
                        if ( item.IsActive )
                            $scope.appointmentResources.push( item );
                    } );

                    q.resolve( $scope.appointmentResources );
                },
              function ( errorStatus ) {
                  $scope.isLoading = false;
                  alertService.error( 'Unable to connect to server' );
                  q.reject();
              } );
            } else {
                q.resolve();
            }

            return q.promise;
        }

        var getRooms = function ( facilityID ) {
            var q = $q.defer();
            var Rooms = $filter( 'filter' )( $scope.rooms, { FacilityID: facilityID }, true );
            if ( facilityID && Rooms.length == 0 ) {
                resourceService.getRooms( facilityID ).then( function ( data ) {
                    if ( data.DataItems.length > 0 ) {
                        angular.forEach( data.DataItems, function ( item ) {
                            $scope.rooms.push( item );
                        } );
                    }
                    q.resolve( $scope.rooms );
                },
            function ( errorStatus ) {
                $scope.isLoading = false;
                alertService.error( 'Unable to connect to server' );
            } );
            } else {
                q.resolve();
            }
            return q.promise;
        }

        $scope.getAppointmentDetails = function ( contactID ) {
            $scope.isLoading = true;
            appointmentService.getAppointmentByResource( contactID, 7 ).then( function ( data ) {
                $scope.appointmentList = data.DataItems;                
                //$scope.appointmentList = $filter( 'filter' )( $scope.appointmentList, { IsCancelled: false }, true );
                var arr = [];
                angular.forEach( $scope.appointmentList, function ( item ) {
                    arr.push( getResources( item.AppointmentID ) );
                    arr.push( getRooms( item.FacilityID ) );
                } );
                $q.all( arr ).then( function () {
                    getExternalSources();
                    if ( $scope.appointmentList != null ) {
                        var promises = [];
                        angular.forEach( $scope.appointmentList, function ( item ) {
                            promises.push( provider( item ) );
                        } );
                        $q.all( promises ).then( function ( data ) {
                            appointmentTable.bootstrapTable( 'load', $scope.appointmentList );
                        } );
                    } else {
                        appointmentTable.bootstrapTable( 'removeAll' );
                    }
                    applyDropupOnGrid( true );
                    $scope.isLoading = false;
                } );
            },
            function ( errorStatus ) {
                $scope.isLoading = false;
                alertService.error( 'Unable to connect to server' );
            } );
        };

        $scope.edit = function ( appointmentId, index ) {
            console.log( appointmentId );
        };

        $scope.deleteAppointment = function ( appointmentId ) {
            bootbox.confirm( "Selected Appointment will be cancelled.\n Do you want to continue?", function ( result ) {
                if ( result == true ) {
                    appointmentService.deleteAppointment( $scope.contactID, appointmentId ).then( function ( data ) {
                        var appointment = $filter( 'filter' )( $scope.appointmentList, { AppointmentID: appointmentId }, true )[0];
                        var index = $scope.appointmentList.indexOf( appointment );
                        if ( index > -1 )
                            $scope.appointmentList.splice( index, 1 );

                        appointmentTable.bootstrapTable( 'load', $scope.appointmentList );
                    },
           function ( errorStatus ) {
               $scope.isLoading = false;
               alertService.error( 'Unable to connect to server' );
           } );
                }
            } );
        };

        getDay = function ( value ) {
            var dt = moment( value );
            return dt.format( 'ddd' );
        };

        getTime = function ( value ) {
            var year = '2000';
            var month = '01';
            var day = '01';

            var start = value.toString();
            var len_start = start.length

            var last2 = start.slice( -2 );
            var len_last = last2.length;

            var diff = len_start - len_last;

            var first = start.substr( 0, diff );;

            var time = new Date( year, month, day, first, last2 );

            return time;
        };

        formatTime = function ( tme ) {
            var h = tme.getHours(), m = ( '0' + tme.getMinutes() ).slice( -2 );
            var time = ( h > 12 ) ? ( ( '0' + ( h - 12 ) ).slice( -2 ) + ':' + m + ' PM' ) : ( ( h != 12 ) ? ( ( '0' + h ).slice( -2 ) + ':' + m + ' AM' ) : ( ( '0' + h ).slice( -2 ) + ':' + m + ' PM' ) );
            return time
        }

        getStart = function ( value ) {
            var time = getTime( value );
            var tme = formatTime( time );
            return tme;
        };

        getEnd = function ( value, row ) {
            var dt = getTime( row.AppointmentStartTime );
            dt.setMinutes( dt.getMinutes() + value );
            var tme = formatTime( dt );
            return tme;
        };

        getAppointmentType = function ( value, row, index ) {
            $scope.appointmentTypes = $filter( 'filter' )( $rootScope.getLookupsByType( 'AppointmentTypes' ), { ProgramID: row.ProgramID, AppointmentTypeID: row.AppointmentTypeID }, true )[0];
            return $scope.appointmentTypes != null ? $scope.appointmentTypes.AppointmentType : null;
        }

        var provider = function ( row ) {
            var q = $q.defer();
            var appointmentResources = $filter( 'filter' )( $scope.appointmentResources, function ( item ) {
                return item.ResourceTypeID == 2 && item.AppointmentID == row.AppointmentID && item.IsActive == true;
            } );
            var providers = '';
            resourceService.getCredentialByAppointmentType( row.AppointmentTypeID ).then( function ( data ) {
                var resource;
                var facility = '';
                angular.forEach( appointmentResources, function ( item ) {
                    if ( item.ParentID ) {
                        angular.forEach( data.DataItems, function ( credential ) {
                            angular.forEach( credential.Providers, function ( provider ) {
                                if ( provider.ProviderId == item.ResourceID ) {
                                    resource = provider.ProviderName;
                                    facility += facility ? ( ',' + credential.CredentialAbbreviation ) : credential.CredentialAbbreviation;
                                }
                            } );
                        } );
                    } else {
                        facility = '';
                        resource = $filter( 'filter' )( $scope.externalSources, { ResourceID: item.ResourceID }, true )[0].ResourceName;
                    }

                    resource = resource + ( facility ? '(' + facility + ')' : '' );
                    providers += providers ? ' <br />' + resource : resource;
                } );
                row.Providers = providers;
                q.resolve( row );
            } );
            return q.promise;
        }

        getLocation = function ( value, row, index ) {
            var locationName = '';
            if ( row.FacilityID ) {
                var resource = $filter( 'filter' )( $scope.appointmentResources, function ( item ) {
                    return item.ResourceTypeID == ResourceType.Room && item.AppointmentID == row.AppointmentID;
                } )[0];

                var location = $filter( 'filter' )( $scope.locations, function ( location ) {
                    return location.type == ResourceType.Room && location.LocationID == row.FacilityID;
                } )[0];

                var room = ''
                if ( resource ) {
                    room = $filter( 'filter' )( $scope.rooms, function ( room ) {
                        return room.RoomID == resource.ResourceID && room.FacilityID == row.FacilityID;
                    } )[0];
                }
                locationName = location ? location.Name + ( room ? '-' + room.RoomName : '' ) : '';
            } else {
                var address = $filter( 'filter' )( $scope.appointmentResources, function ( item ) {
                    return item.ResourceTypeID == ResourceType.External && item.AppointmentID == row.AppointmentID;
                } )[0];

                if ( address ) {
                    var location = $filter( 'filter' )( $scope.locations, function ( location ) {
                        return location.type == ResourceType.External && location.LocationID == address.ResourceID;
                    } )[0];
                    locationName = location ? location.Name : '';
                }
            }

            return locationName
        }

        var resetForm = function () {
            $rootScope.formReset($scope.ctrl.cancelAppointmentForm, $scope.ctrl.cancelAppointmentForm.$name);
        };

        $scope.cancelModal = function () {
            $scope.appointmentToCancel = {};
            $('#cancelGroupScheduleModel').modal('hide');
        }

        $scope.save = function (isNext, mandatory, hasErrors) {
            if (hasErrors) {
                alertService.error('Please correct the highlighted errors before submitting the form');
                return false;
            }

            if ($scope.ctrl.cancelAppointmentForm.$dirty && !hasErrors) {
                appointmentService.cancelAppointment($scope.appointmentToCancel).then(function (response) {
                    if (response.ResultCode === 0) {
                        $scope.appointmentToCancel.IsCancelled = true;
                        alertService.success('Appointment cancelled successfully');
                        $('#cancelGroupScheduleModel').modal('hide');
                        $scope.getAppointmentDetails($scope.contactID);
                        resetForm();
                    }
                    else {
                        alertService.error('Unable to cancel Appointment');
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

        $scope.init();
    }] );