angular.module( 'xenatixApp' )
    .controller( 'groupSchedulingSearchController', ['$scope', '$filter', 'alertService', '$rootScope', '$stateParams', '$q', '$state', 'formService', 'lookupService', 'groupSchedulingSearchService',
        function ( $scope, $filter, alertService, $rootScope, $stateParams, $q, $state, formService, lookupService, groupSchedulingSearchService ) {
            var resetForm = function () {
                $rootScope.formReset( $scope.ctrl.cancelGroupScheduleForm, $scope.ctrl.cancelGroupScheduleForm.$name );
            };

            $scope.init = function () {
                $scope.$parent['autoFocus'] = true;
                $scope.isDisabled = false;
                $scope.groupSchedulingTable = $( "#groupSchedulingTable" );
                $scope.searchText = "";
                $scope.groupScheduleToCancel = {};
                $scope.initializeBootstrapTable();
                $scope.$evalAsync( function () {
                    applyDropupOnGrid( true );
                } );
            };

            $scope.getLookupsByType = function ( typeName ) {
                return lookupService.getLookupsByType( typeName );
            };

            $scope.searchGroupSchedule = function ( searchStr ) {
                $scope.isLoading = true;
                groupSchedulingSearchService.get( searchStr ).then( function ( data ) {
                    if ( data && data.DataItems ) {
                        $scope.groupSchedules = data.DataItems;
                        $scope.groupSchedulingTable.bootstrapTable( 'load', $scope.groupSchedules );
                    } else {
                        $scope.groupSchedules = [];
                        $scope.groupSchedulingTable.bootstrapTable( 'removeAll' );
                    }
                    applyDropupOnGrid( true );
                },
                    function ( errorStatus ) {
                        alertService.error( 'Unable to get clientInformation: ' + errorStatus );
                    } ).finally( function () {
                        $scope.isLoading = false;
                    } );
            }

            $scope.cancelAppointment = function (GroupID) {
                bootbox.confirm("Are you sure that you want to cancel this appointment?", function (result) {
                    if (result === true) {
                        $scope.groupScheduleToCancel = $scope.groupSchedules.filter(function (obj) {
                            return obj.GroupID == GroupID;
                        })[0];
                        $('#cancelGroupScheduleModel').modal('show');
                    }
                });
            }

            $scope.cancelModal = function () {
                $scope.GroupIDToCancel = 0;
                $( '#cancelGroupScheduleModel' ).modal( 'hide' );
            }

            $scope.save = function ( isNext, mandatory, hasErrors ) {
                if ( hasErrors ) {
                    alertService.error( 'Please correct the highlighted errors before submitting the form' );
                    return false;
                }

                if ( $scope.ctrl.cancelGroupScheduleForm.$dirty && !hasErrors ) {
                    groupSchedulingSearchService.cancelGroupAppointment( $scope.groupScheduleToCancel ).then( function ( response ) {
                        if ( response.ResultCode === 0 ) {
                            alertService.success( 'Group Appointment cancelled successfully' );

                            $( '#cancelGroupScheduleModel' ).modal( 'hide' );

                            $scope.searchGroupSchedule( $scope.searchText );
                            resetForm();
                        }
                        else {
                            alertService.error( 'Unable to cancel Group Appointment' );
                        }
                    },
                        function ( errorStatus ) {
                            alertService.error( 'OOPS! Something went wrong' );
                            deferred.reject();
                        },
                        function ( notification ) {
                            alertService.warning( notification );
                        }
                    ).finally( function () {
                        resetForm();
                    } );
                }
            }

            $scope.initializeBootstrapTable = function () {
                $scope.tableoptions = {
                    pagination: true,
                    pageSize: 10,
                    pageList: [10, 25, 50, 100],
                    search: false,
                    showColumns: true,
                    data: [],
                    undefinedText: '',
                    onClickRow: function ( e, row, $element ) {
                    },
                    columns: [
                        {
                            field: "GroupName",
                            title: "Group Name"
                        },
                        {
                            field: "GroupTypeID",
                            title: "Group Type"
                        },
                        {
                            field: "ProgramName",
                            title: "Program Unit"
                        },
                        {
                            field: "FacilityName",
                            title: "Location"
                        },
                        {
                            field: "AppointmentDate",
                            title: "Date",
                            formatter: function ( value, row, index ) {
                                if ( value ) {
                                    var formattedDate = $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY', 'useLocal');
                                    return formattedDate;
                                } else
                                    return '';
                            }
                        },
                        {
                            field: "AppointmentStartTime",
                            title: "Start Time",
                            formatter: function ( value, row, index ) {
                                if ( value ) {
                                    return $filter('toStandardTime')(value) + ' ' + $filter('toStandardTimeAMPM')(value);//Check This Attribute
                                } else
                                    return '';
                            }
                        },
                        {
                            field: "Recurring",
                            title: "Recurring",
                            formatter: function ( value, row ) {
                                return value == 1 ? "Yes" : "No";
                            },
                        },
                         {
                             field: "GroupID",
                             title: "",
                             formatter: function ( value, row, index ) {
                                 return (
                                     '<span class="text-nowrap"><a data-default-no-action ui-sref="scheduling.groupscheduling.details.groupschedule({  GroupID: ' + value + ', ReadOnly: \'view\' })" alt="View" ' +
                                             'security permission-key="Scheduling-Appointment-GroupAppointment" permission="read" space-key-press>' +
                                             '<i title="View" class="fa fa-eye fa-fw border-left margin-left padding-left-small padding-right-small" /></a>' +

                                     '<a data-default-action ui-sref="scheduling.groupscheduling.details.groupschedule({  GroupID: ' + value + ' , ReadOnly: \'edit\' })" alt="Edit" ' +
                                             'security permission-key="Scheduling-Appointment-GroupAppointment" permission="read" space-key-press>' +
                                             '<i  title="Edit" class="fa fa-pencil fa-fw border-left margin-left padding-left-small padding-right-small" /></a>' +

                                     '<a  data-default-no-action href="#" ng-click="cancelAppointment(' + value + ')" alt="Cancel" security permission-key="Scheduling-Appointment-GroupAppointment" permission="delete">' +
                                     '<i title="Cancel" class="fa fa-ban fa-fw border-left margin-left padding-left-small padding-right-small"></i></a>' +
                                     '</span>'
                                     );
                             }
                         }
                    ]
                };
            };

            $scope.init();
        }] );