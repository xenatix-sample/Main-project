angular.module("xenatixApp")
    .config([
        '$stateProvider', 'lazyLoaderProvider', function ($stateProvider, lazyLoader) {
            $stateProvider
                .state('scheduling',
                    {
                        title: 'Scheduling',
                        url: '/Scheduling',
                        templateUrl: '/Plugins/Scheduling/Calendar',
                        controller: "calendarController as ctrl",
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Scheduling/Calendar');
                            }]
                        }
                    }
                )
                 .state('scheduling.groupschedulingsearch',
                    {
                        title: 'Group Scheduling Search',
                        url: '/GroupSchedulingSearch',
                        templateUrl: '/Plugins/GroupScheduling/Index',
                        controller: "groupSchedulingSearchController as ctrl",
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Scheduling/GroupSchedulingSearch');
                            }]
                        },
                        data: {
                            permissionKey: 'Scheduling-Appointment-GroupAppointment'
                        }
                    }
                )
                .state('scheduling.groupscheduling',
                    {
                        title: 'Group Scheduling',
                        url: '/GroupScheduling',
                        templateUrl: '/Plugins/GroupScheduling/GroupSchedulingMain',
                        resolve: {
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Scheduling/GroupScheduling');
                            }]
                        },
                        data: {
                            permissionKey: 'Scheduling-Appointment-GroupAppointment'
                        }
                    }
                )
                 .state('scheduling.groupscheduling.details',
                    {
                        url: '/{GroupID:int}/:ReadOnly',
                        views: {
                            '@scheduling.groupscheduling': {
                                template: '<div ui-view></div>'
                            },
                            'navigation@scheduling.groupscheduling': {
                                template: '<div class="container-fluid nomargin nopadding">' +
                                        '<div>' +
                                            '<div class="col-md-12 col-lg-12 right-nav">' +
                                                '<div work-flows-set work-flow-active-option="groupSchedulingComplete" work-flow-ready="workFlowReady" work-flow-model="workFlowModel">' +
                                      '</div></div></div></div>',
                                controller: "groupScheduleNavigationController as ctrl"
                            }
                        },
                        cache: false,
                        resolve: {
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Scheduling/GroupSchedulingNavigation');
                            }]
                        },
                        data: {
                            permissionKey: 'Scheduling-Appointment-GroupAppointment'
                        }
                    }
                )
                .state('scheduling.groupscheduling.details.groupschedule',
                    {
                        title: 'Group Schedule',
                        url: '/GroupSchedule/:IsRecurringAptEdit',
                        templateUrl: '/Plugins/GroupScheduling/GroupSchedule',
                        controller: "groupSchedulingController as ctrl",
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Scheduling/GroupScheduling');
                            }]
                        },
                        data: {
                            permissionKey: 'Scheduling-Appointment-GroupAppointment'
                        }
                    }
                )
                .state('scheduling.groupscheduling.details.groupnote',
                    {
                        title: 'Group Note',
                        url: '/GroupNote',
                        templateUrl: '/Plugins/GroupScheduling/GroupNote',
                        controller: "groupNoteController as ctrl",
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Scheduling/GroupNote');
                            }]
                        },
                        data: {
                            permissionKey: 'Scheduling-Appointment-GroupAppointment'
                        }
                    }
                )
                .state('patientprofile.appointments',
                    {
                        title: 'Appointments',
                        url: '/Appointments',
                        templateUrl: '/Plugins/Scheduling/Index',
                        controller: "appointmentDetailController as ctrl",
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Scheduling/Scheduling');
                            }]
                        },
                        data: {
                            permissionKey: 'Scheduling-Appointment-SingleAppointment'
                        }
                    }
                )
                .state('patientprofile.appointments.newAppointment',
                    {
                        title: 'Appointment',
                        url: '/New',
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Plugins/Scheduling/Schedule',
                                controller: "appointmentController as ctrl"
                            },
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Scheduling/Scheduling');
                            }],
                            securityAttribute: [function () {
                                return { module: 'scheduling', feature: 'appointment' };
                            }]
                        },
                        data: {
                            permissionKey: 'Scheduling-Appointment-SingleAppointment'
                        }
                    }
                )
              .state('patientprofile.appointments.editAppointment',
                    {
                        title: 'Appointment',
                        url: '/Edit/:AppointmentID/:IsRecurringAptEdit',
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Plugins/Scheduling/Schedule',
                                controller: "appointmentController as ctrl"
                            },
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Scheduling/Scheduling');
                            }],
                            securityAttribute: [function () {
                                return { module: 'scheduling', feature: 'appointment' };
                            }]
                        },
                        data: {
                            permissionKey: 'Scheduling-Appointment-SingleAppointment'
                        }
                    }
                )
                .state('patientprofile.appointments.viewAppointment',
                    {
                        title: 'View Appointment',
                        url: '/{ReadOnly}/view/:AppointmentID/:IsRecurringAptEdit',
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Plugins/Scheduling/Schedule',
                                controller: "appointmentController as ctrl"
                            },
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Scheduling/Scheduling');
                            }],
                            securityAttribute: [function () {
                                return { module: 'scheduling', feature: 'appointment' };
                            }]
                        },
                        data: {
                            permissionKey: 'Scheduling-Appointment-SingleAppointment'
                        }
                    }
                )
            .state('patientprofile.appointments.cancelAppointment',
                    {
                        title: 'Cancel Appointment',
                        url: '/{ReadOnly}/cancel/:AppointmentID/:IsRecurringAptEdit',
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Plugins/Scheduling/Schedule',
                                controller: "appointmentController as ctrl"
                            },
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Scheduling/Scheduling');
                            }],
                            securityAttribute: [function () {
                                return { module: 'scheduling', feature: 'appointment' };
                            }]
                        },
                        data: {
                            permissionKey: 'Scheduling-Appointment-SingleAppointment'
                        }
                    }
                );
        }
    ])
    .run([
         '$rootScope', '$timeout', '$http', '$document',
        function ($rootScope, $timeout, $http, $document) {
            $rootScope.groupSchedulingComplete = { workFlowEnableAction: null };

            $rootScope.$on("$stateChangeSuccess", function (event, toState, toParams, fromState, fromParams) {
                //Group Scheduling navigation validation handler
                if ((toState != null) && toState.name && (toState.name.indexOf('scheduling.groupscheduling.details') === 0)) {
                    if (((fromState != null) && fromState.name && ((fromState.name.indexOf('scheduling.groupscheduling.details.groupschedule') !== 0) || (fromParams.GroupID !== toParams.GroupID)) && (fromParams.GroupID === null || fromParams.GroupID === undefined || fromParams.GroupID === 0)) || ((fromState != null) && (!fromState.name))) {
                        var groupID = toParams.GroupID ? toParams.GroupID : 0;
                        $timeout(function () {
                            var workflowActions = $("ul li[data-state-name^=scheduling]").map(function () { return this.attributes['data-state-name'].value; }).get();
                            if (groupID !== 0) {
                                $http.get('/Plugins/GroupScheduling/NavigationValidationStates', { params: { groupID: groupID, workflowActions: workflowActions } })
                                    .then(function (response) {
                                        var data = eval(response.data);
                                        for (var state in data) {
                                            var stateObject = { stateName: state, validationState: data[state] };
                                            $rootScope.groupAppointmentRightNavigationHandler(stateObject);
                                            if (workflowActions.indexOf(state) >= 0)
                                                workflowActions.splice(workflowActions.indexOf(state), 1);
                                        }
                                    }).finally(function () { // whether it succeeded or not, stop any remaining spinners ...
                                        for (var iIdx = 0; iIdx < workflowActions.length; iIdx++) {
                                            $rootScope.$broadcast(workflowActions[iIdx], { validationState: ' ' });
                                        }
                                    });
                            }
                            else if (!((fromState != null) && fromState.name && (fromState.name === 'scheduling.groupscheduling.details.groupschedule'))) {
                                //default everything to a neutral state and stop all spinners
                                for (var iIdx = 0; iIdx < workflowActions.length; iIdx++) {
                                    $rootScope.$broadcast(workflowActions[iIdx], { validationState: ' ' });
                                }
                            }
                        });
                    }
                    $document.scrollTop(0);
                }
            });//End of StateChangeSuccess

            $rootScope.groupAppointmentRightNavigationHandler = function (stateDetail) {
                if ($rootScope.workflowActions != undefined && $rootScope.workflowActions.hasOwnProperty(stateDetail.stateName)) {
                    if (stateDetail.stateName === "scheduling.groupscheduling.details.groupschedule" && stateDetail.validationState === "valid") {
                        $rootScope.groupSchedulingComplete = { workFlowEnableAction: 0 };
                    }
                    $rootScope.workflowActions[stateDetail.stateName].validationState = stateDetail.validationState;
                }
                $rootScope.$broadcast(stateDetail.stateName, { validationState: stateDetail.validationState });
            };
        }]);//Run 