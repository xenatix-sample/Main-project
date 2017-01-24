angular.module("xenatixApp")
    .config(['$stateProvider', 'lazyLoaderProvider', function ($stateProvider, lazyLoader) {
        $stateProvider
            .state('callcenter', {
                title: 'Call Center',
                url: '/CallCenter/:ReadOnly',
                abstract: true,
                template: '<div ui-view></div>',
                cache: false,
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.CallCenter/CallCenter');
                    }],
                    scriptHeader: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/miniHeader');
                    }]
                }
            })
            .state('crisisline', {
                title: 'Crisis Line',
                url: '/CrisisLine',
                templateUrl: '/CrisisLine/Index',
                controller: 'crisisLineSummaryController as ctrl',
                cache: false,
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.CallCenter/CrisisLineSummary');
                    }]
                },
                onEnter: ['cacheService', function (cacheService) {
                    cacheService.remove('FollowUp');
                    cacheService.remove('approvalData');
                    cacheService.remove('currentApprovalIndex');
                    cacheService.remove('reviewFollowup');
                    cacheService.remove('lawLiaisonFollowUp');
                    cacheService.remove('IsReadOnlyScreens');
                }],
                onExist: ['cacheService', function (cacheService) {
                    cacheService.remove('IsManagerAccess');
                    cacheService.remove('IsCreatorAccess');
                }],
                data: {
                    permissionKey: 'CrisisLine-CrisisLine-CrisisLine'
                }

            })
            .state('callcenter.crisisline', {
                title: 'Crisis Line',
                url: '/CrisisLine',
                controller: "callCenterCrisislineController as ctrl",
                templateUrl: "/Plugins/CallCenter/CallCenterRegistration/CrisisLineNavigation",
                cache: false,
                abstract: true,
                data: {
                    approvalData: null
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.CallCenter/CallCenter');
                    }],
                    scriptQuickRegistration: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/QuickRegistration');
                    }]
                },
                onExit: function () {
                    this.data.approvalData = null;
                },
                data: {
                    permissionKey: 'CrisisLine-CrisisLine-CrisisLine',
                    workflowDataKey: 'CrisisLine-CrisisLine-CrisisLine'
                }
            })
            .state('callcenter.crisisline.initialcallerinformation', {
                title: 'Caller Information',
                url: '/CallerInformation/Initial',
                templateUrl: "/Plugins/CallCenter/CallerInformation/Index",
                resolve: {
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.CallCenter/CallerInformation');
                    }],
                    viewMode: function () {
                        return false;
                    },
                    managerOrCreator: ['roleSecurityService', 'cacheService', function (roleSecurityService, cacheService) {
                        if (roleSecurityService.hasPermission('CrisisLine-CrisisLine-Approver', PERMISSION.UPDATE))
                            cacheService.add('IsManagerAccess', true);
                        else
                            cacheService.add('IsManagerAccess', false);
                        cacheService.add('IsCreatorAccess', true);
                    }]
                },
                controller: "callerInformationController as ctrl",
                data: {
                    permissionKey: 'CrisisLine-CrisisLine-CrisisLine'
                }
            })
            .state('callcenter.crisisline.callerinformation', {
                title: 'Caller Information',
                url: '/CallerInformation/{CallCenterHeaderID:int}/{ContactID:int}',
                templateUrl: "/Plugins/CallCenter/CallerInformation/Index",
                resolve: {
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.CallCenter/CallerInformation');
                    }],
                    viewMode: ['$stateParams', 'callerInformationService', function ($stateParams, callerInformationService) {
                        return callerInformationService.get($stateParams.CallCenterHeaderID).then(function (response) {
                            return (response && response.DataItems && response.DataItems.length > 0 && (response.DataItems[0].CallStatusID == 1 || response.DataItems[0].CallStatusID == 6)) ? true : false;
                        });
                    }]
                },
                controller: "callerInformationController as ctrl",
                data: {
                    permissionKey: 'CrisisLine-CrisisLine-CrisisLine'
                }
            })
            .state('callcenter.crisisline.columbiasuicidescale', {
                title: 'Columbia Suicide Scale',
                url: '/ColumbiaSuicideScale/{CallCenterHeaderID:int}/{ContactID:int}',
                params: {
                    SectionID: null,
                    ResponseID: null
                },
                template: '<div class="row padding-left-small"><breadcrumbs min-breadcrumbs="2" goto="crisisline" /></div><assessment-section no-access-to-other="{{isDisabled}}" on-post-assessment-response="postAssessmentReponseDetails" permission-key="CrisisLine-CrisisLine-CrisisLine" data-return-state="" prepopulatedData="prepopulatedData" on-print-report="initReport"/>',
                controller: 'columbiaSuicideScaleController as ctrl',
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.CallCenter/ColumbiaSuicideScale');
                    }],
                    assessmentScripts: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/assessment');
                    }],
                    sectionID: ['assessmentService', function (assessmentService) {
                        return assessmentService.ensureAssessmentSectionID(ASSESSMENT_TYPE.ColumbiaSuicideSeverityRatingScale).then(function (data) {
                            return data;
                        });
                    }],
                    responseID: ['$stateParams', 'callerInformationService', function ($stateParams, callerInformationService) {
                        return callerInformationService.ensureResponseID($stateParams.CallCenterHeaderID, $stateParams.ContactID, ASSESSMENT_TYPE.ColumbiaSuicideSeverityRatingScale);
                    }],
                    callerInfoScripts: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.CallCenter/CallerInformation');
                    }],
                    signatureScripts: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Signature');
                    }]
                },
                data: {
                    credentialKey: 'Columbia Suicide Assessment',
                    permissionKey: 'CrisisLine-CrisisLine-CrisisLine'
                }
            })
            .state('callcenter.crisisline.crisisAssessment', {
                title: 'Crisis Assessment',
                url: '/CrisisAssessment/{CallCenterHeaderID:int}/{ContactID:int}',
                params: {
                    SectionID: null,
                    ResponseID: null
                },
                template: '<div class="row padding-left-small"><breadcrumbs min-breadcrumbs="2" goto="crisisline" /></div><assessment-section permission-key="CrisisLine-CrisisLine-CrisisLine" data-return-state="" no-access-to-other="{{isDisabled}}" on-post-assessment-response="postAssessmentReponseDetails" prepopulatedData="prepopulatedData" on-print-report="initReport"/>',
                controller: 'crisisAssessmentController as ctrl',
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.CallCenter/CrisisAssessment');
                    }],
                    assessmentScripts: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/assessment');
                    }],
                    sectionID: ['assessmentService', function (assessmentService) {
                        return assessmentService.ensureAssessmentSectionID(ASSESSMENT_TYPE.CrisisAssessment).then(function (data) {
                            return data;
                        });
                    }],
                    responseID: ['$stateParams', 'callerInformationService', function ($stateParams, callerInformationService) {
                        return callerInformationService.ensureResponseID($stateParams.CallCenterHeaderID, $stateParams.ContactID, ASSESSMENT_TYPE.CrisisAssessment);
                    }],
                    callerInfoScripts: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.CallCenter/CallerInformation');
                    }],
                    signatureScripts: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Signature');
                    }]
                },
                data: {
                    credentialKey: 'Crisis Assessment',
                    permissionKey: 'CrisisLine-CrisisLine-CrisisLine'
                }
            })
            .state('callcenter.crisisline.adultScreening', {
                title: 'Adult Screening',
                url: '/AdultScreening/{CallCenterHeaderID:int}/{ContactID:int}',
                params: {
                    SectionID: null,
                    ResponseID: null
                },

                //the template function will get replaced with dynamic schemas.Hard code fix for 90 days.
                template: function () {
                    return '<div class="row padding-left-small"><breadcrumbs min-breadcrumbs="2" goto="crisisline" /></div><assessment-section permission-key="CrisisLine-CrisisLine-CrisisLine" data-start-Date="' + new Date()
                        + '" data-end-Date = "' + new Date(new Date().setMonth(new Date().getMonth() + 8)) + '"  data-return-state="" prepopulatedData="prepopulatedData" no-access-to-other="{{isDisabled}}" on-post-assessment-response="postAssessmentReponseDetails"  on-print-report="initReport"/>';
                },

                //template: '<div class="row padding-left-small"><breadcrumbs min-breadcrumbs="2" goto="callcentersummary" /></div><assessment-section  data-return-state="" on-print-report="initReport"/>',
                controller: 'adultScreeningController as ctrl',
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.CallCenter/AdultScreening');
                    }],
                    assessmentScripts: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/assessment');
                    }],
                    sectionID: ['assessmentService', function (assessmentService) {
                        return assessmentService.ensureAssessmentSectionID(ASSESSMENT_TYPE.CrisisAdultScreening).then(function (data) {
                            return data;
                        });
                    }],
                    responseID: ['$stateParams', 'callerInformationService', function ($stateParams, callerInformationService) {
                        return callerInformationService.ensureResponseID($stateParams.CallCenterHeaderID, $stateParams.ContactID, ASSESSMENT_TYPE.CrisisAdultScreening);
                    }],
                    callerInfoScripts: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.CallCenter/CallerInformation');
                    }],
                    signatureScripts: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Signature');
                    }]
                },
                data: {
                    credentialKey: 'Crisis Adult Screening',
                    permissionKey: 'CrisisLine-CrisisLine-CrisisLine'
                }
            })
            .state('callcenter.crisisline.childScreening', {
                title: 'Child Screening',
                url: '/ChildScreening/{CallCenterHeaderID:int}/{ContactID:int}',
                params: {
                    SectionID: null,
                    ResponseID: null
                },
                template: function () {
                    return '<div class="row padding-left-small"><breadcrumbs min-breadcrumbs="2" goto="crisisline" /></div><assessment-section no-access-to-other="{{isDisabled}}" permission-key="CrisisLine-CrisisLine-CrisisLine" data-start-Date="' + new Date(new Date().setMonth(new Date().getMonth() - 6))
                        + '" data-end-Date = "' + new Date() + '"  data-return-state="" prepopulatedData="prepopulatedData" on-post-assessment-response="postAssessmentReponseDetails" on-print-report="initReport"/>';
                },

                //template: '<div class="row padding-left-small"><breadcrumbs min-breadcrumbs="2" goto="callcentersummary" /></div><assessment-section   data-return-state="" on-print-report="initReport"/>',
                controller: 'childScreeningController as ctrl',
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.CallCenter/ChildScreening');
                    }],
                    assessmentScripts: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/assessment');
                    }],
                    sectionID: ['assessmentService', function (assessmentService) {
                        return assessmentService.ensureAssessmentSectionID(ASSESSMENT_TYPE.CrisisChildScreening).then(function (data) {
                            return data;
                        });
                    }],
                    responseID: ['$stateParams', 'callerInformationService', function ($stateParams, callerInformationService) {
                        return callerInformationService.ensureResponseID($stateParams.CallCenterHeaderID, $stateParams.ContactID, ASSESSMENT_TYPE.CrisisChildScreening);
                    }],
                    callerInfoScripts: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.CallCenter/CallerInformation');
                    }],
                    signatureScripts: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Signature');
                    }]
                },
                data: {
                    credentialKey: 'Crisis Child Screening',
                    permissionKey: 'CrisisLine-CrisisLine-CrisisLine'
                }
            })
            .state('callcenter.crisisline.progressnotes', {
                title: 'Progress Notes',
                url: '/CallCenterNote/{CallCenterHeaderID:int}/{ContactID:int}',
                params: {
                    SectionID: null,
                    ResponseID: null
                },
                templateUrl: "/Plugins/CallCenter/CallCenterProgressNote/Index",
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.CallCenter/CallCenterProgressNote');
                    }],
                    assessmentScripts: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/assessment');
                    }],
                    sectionID: ['assessmentService', function (assessmentService) {
                        return assessmentService.ensureAssessmentSectionID(ASSESSMENT_TYPE.CrisisLineProgressNote).then(function (data) {
                            return data;
                        });
                    }],
                    responseID: ['$stateParams', 'callerInformationService', function ($stateParams, callerInformationService) {
                        return callerInformationService.ensureResponseID($stateParams.CallCenterHeaderID, $stateParams.ContactID, ASSESSMENT_TYPE.CrisisLineProgressNote);
                    }],
                    viewMode: function () {
                        return false;
                    }
                },
                controller: "callCenterProgressNoteController as ctrl",
                data: {
                    permissionKey: 'CrisisLine-CrisisLine-CrisisLine',
                }
            })
            .state('callcenter.crisisline.appointment', {
                title: 'Appointment',
                url: '/Appointment/{CallCenterHeaderID:int}/{ContactID:int}',
                templateUrl: '/Plugins/Scheduling/Schedule',
                controller: "appointmentController as ctrl",
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Scheduling/Scheduling');
                    }],
                    securityAttribute: [function () {
                        return { module: 'callcenter', feature: 'crisisline' };
                    }]
                },
                data: {
                    permissionKey: 'CrisisLine-CrisisLine-CrisisLine'
                }
            })
            .state('callcenter.crisisline.report', {
                title: 'Report',
                url: '/CallCenterReport/{CallCenterHeaderID:int}/{ContactID:int}',
                params: {
                    SectionID: null,
                    ResponseID: null
                },
                template: '<xen-report on-print-report="printCrisisLine()" goto-state="crisisline" />',
                controller: 'callCenterReportController as ctrl',
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.CallCenter/CallCenterReport');
                    }]
                },
                data: {
                    permissionKey: 'CrisisLine-CrisisLine-CrisisLine'
                }
            })
            .state('callcenter.crisisline.services', {
                title: 'Services',
                url: '/ServiceRecording/{CallCenterHeaderID:int}/{ContactID:int}',
                templateUrl: "/Plugins/CallCenter/ServiceRecording/Index",
                controller: "serviceRecordingController as ctrl",
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/ServiceRecording');
                    }],
                    ServiceRecordingSourceID: [function () {
                        return SERVICE_RECORDING_SOURCE.CallCenter;
                    }]
                },
                data: {
                    permissionKey: CallCenterPermissionKey.CallCenter_CrisisLine
                }
            })
            .state('callcenter.lawliaison', {
                title: 'Law Liaison',
                url: '/LawLiaison',
                templateUrl: "/Plugins/CallCenter/CallCenterRegistration/LawLiaisonNavigation",
                controller: "callCenterLawliaisonController as ctrl",
                cache: false,
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.CallCenter/CallCenter');
                    }],
                    scriptQuickRegistration: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/QuickRegistration');
                    }]
                },
                data: {
                    permissionKey: 'LawLiaison-LawLiaison-LawLiaison',
                    workflowDataKey: 'LawLiaison-LawLiaison-LawLiaison'
                }
            })
            .state('callcenter.lawliaison.initlawenforcement', {
                title: 'Law Enforcement',
                url: '/Lawenforcement/Initial/',
                templateUrl: '/Plugins/CallCenter/LawLiaisonEnforcement',
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Registration');
                    }],
                    lawliaison: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.CallCenter/LawLiaisonEnforcement');
                    }],
                    isReadOnlyAccess: ['cacheService', function (cacheService) {
                        cacheService.add('IsReadOnlyLLScreens', false);
                    }]
                },
                controller: "lawLiaisonEnforcementController as ctrl",
                data: {
                    permissionKey: 'LawLiaison-LawLiaison-LawLiaison'
                }
            })
            .state('callcenter.lawliaison.lawenforcement', {
                title: 'Law Enforcement',
                url: '/LawEnforcement/{CallCenterHeaderID:int}/{ContactID:int}',
                templateUrl: '/Plugins/CallCenter/LawLiaisonEnforcement',
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Registration');
                    }],
                    lawliaison: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.CallCenter/LawLiaisonEnforcement');
                    }]
                },
                controller: "lawLiaisonEnforcementController as ctrl",
                data: {
                    permissionKey: 'LawLiaison-LawLiaison-LawLiaison'
                }
            })
            .state('callcenter.lawliaison.screening', {
                title: 'Law Liaison Screening',
                url: '/LawLiaisonScreening/{CallCenterHeaderID:int}/{ContactID:int}',
                params: {
                    SectionID: null,
                    ResponseID: null
                },
                templateUrl: '/Plugins/CallCenter/LawLiaisonScreening/Index',
                controller: 'lawLiaisonscreeningController as ctrl',
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    regScriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Registration');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.CallCenter/LawLiaisonscreening');
                    }],
                    assessmentScripts: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/assessment');
                    }],
                    sectionID: ['assessmentService', function (assessmentService) {
                        return assessmentService.ensureAssessmentSectionID(ASSESSMENT_TYPE.LawLiaisonScreening).then(function (data) {
                            return data;
                        });
                    }],
                    responseID: ['$stateParams', 'callerInformationService', function ($stateParams, callerInformationService) {
                        return callerInformationService.ensureResponseID($stateParams.CallCenterHeaderID, $stateParams.ContactID, ASSESSMENT_TYPE.LawLiaisonScreening);
                    }],
                    parentResponseID: ['cacheService', 'callerInformationService', function (cacheService, callerInformationService) {
                        var lawLiaisonFollowUp = cacheService.get('lawLiaisonFollowUp');
                        if (lawLiaisonFollowUp && lawLiaisonFollowUp.followupRequired) {
                            return callerInformationService.getCallCenterAssessmentResponse(lawLiaisonFollowUp.parentCallCenterHeaderID, ASSESSMENT_TYPE.LawLiaisonScreening).then(function (assessmentData) {
                                if (hasData(assessmentData)) {
                                    return assessmentData.DataItems[0].ResponseID;
                                }
                                return null;
                            });
                        }
                        else
                            return null;
                    }],
                    callerInfoScripts: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.CallCenter/CallerInformation');
                    }],
                    viewMode: ['$stateParams', 'eSignatureService', function ($stateParams, eSignatureService) {
                        return eSignatureService.getDocumentSignatures(DOCUMENT_TYPE.CallCenter, $stateParams.CallCenterHeaderID).then(function (response) {
                            return (response && response.DataItems && response.DataItems.length > 0) ? true : false;
                        });
                    }],
                    signatureScripts: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Signature');
                    }]
                },
                data: {
                    credentialKey: 'Law Liaison Screening',
                    permissionKey: 'LawLiaison-LawLiaison-LawLiaison'
                }
            })
            .state('callcenter.lawliaison.services', {
                title: 'Services',
                url: '/ServiceRecording/{CallCenterHeaderID:int}/{ContactID:int}',
                templateUrl: "/Plugins/CallCenter/ServiceRecording/Index",
                controller: "serviceRecordingController as ctrl",
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/ServiceRecording');
                    }],
                    ServiceRecordingSourceID: [function () {
                        return SERVICE_RECORDING_SOURCE.LawLiaison;
                        //Above is to get ID from enum. Below is Method to get ID from lookup service and then filtering. Both working.
                        //var recordingSource = lookupService.getLookupsByType("ServiceRecordingSource");
                        //var val = $filter('filter')(recordingSource, function (recSource) {
                        //    return (recSource.ID == SERVICE_RECORDING_SOURCE.CallCenter)
                        //});
                        //return val[0].ID;
                    }]
                },
                data: {
                    permissionKey: CallCenterPermissionKey.CallCenter_LawLiaison
                }
            })
            .state('callcenter.lawliaison.progressnotes', {
                title: 'Progress Notes',
                url: '/CallCenterNote/{CallCenterHeaderID:int}/{ContactID:int}',
                params: {
                    SectionID: null,
                    ResponseID: null
                },
                templateUrl: "/Plugins/CallCenter/CallCenterProgressNote/Index",
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    regScriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Registration');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.CallCenter/CallCenterProgressNote');
                    }],
                    assessmentScripts: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/assessment');
                    }],
                    sectionID: ['assessmentService', function (assessmentService) {
                        return assessmentService.ensureAssessmentSectionID(ASSESSMENT_TYPE.LawLiaisonProgressNote).then(function (data) {
                            return data;
                        });
                    }],
                    responseID: ['$stateParams', 'callerInformationService', function ($stateParams, callerInformationService) {
                        return callerInformationService.ensureResponseID($stateParams.CallCenterHeaderID, $stateParams.ContactID, ASSESSMENT_TYPE.LawLiaisonProgressNote);
                    }],
                    viewMode: ['$stateParams', 'eSignatureService', function ($stateParams, eSignatureService) {
                        return eSignatureService.getDocumentSignatures(DOCUMENT_TYPE.CallCenter, $stateParams.CallCenterHeaderID).then(function (response) {
                            return (response && response.DataItems && response.DataItems.length > 0) ? true : false;
                        });
                    }]
                },
                controller: "callCenterProgressNoteController as ctrl",
                data: {
                    permissionKey: 'LawLiaison-LawLiaison-LawLiaison'

                }
            })
            .state('callcenter.lawliaison.appointment', {
                title: 'Appointment',
                url: '/Appointment/{CallCenterHeaderID:int}/{ContactID:int}',
                templateUrl: '/Plugins/Scheduling/Schedule',
                controller: "appointmentController as ctrl",
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Scheduling/Scheduling');
                    }],
                    securityAttribute: [function () {
                        return { module: 'callcenter', feature: 'lawliaison' };
                    }]
                },
                data: {
                    permissionKey: 'LawLiaison-LawLiaison-LawLiaison'
                }
            })
    }])
    .run(['$rootScope', '$state', '$document', '$http', '$q', '$injector', '$timeout',
        function ($rootScope, $state, $document, $http, $q, $injector, $timeout) {
            var defaultValidationState = ' ';
            var validationState = defaultValidationState;
            var getSignature = function (callCenterID, ServiceRecordingSourceID) {
                if ($injector.has('eSignatureService') && $injector.has('serviceRecordingService')) {
                    var eSignatureService = $injector.get('eSignatureService');
                    var serviceRecordingService = $injector.get('serviceRecordingService');
                    return serviceRecordingService.getServiceRecording(callCenterID, ServiceRecordingSourceID).then(function (serviceData) {
                        if (hasData(serviceData)) {
                            return eSignatureService.getDocumentSignatures(DOCUMENT_TYPE.ServiceRecording, serviceData.DataItems[0].ServiceRecordingID);
                        }
                        else {
                            return { DataItems: [] };
                        }
                    });
                }
                else {
                    return { DataItems: [] };
                }
            }

            $rootScope.$on("$stateChangeSuccess", function (event, toState, toParams, fromState, fromParams) {
                var getAssessementServiceResponse = function (assessmentTypeId, contactId, callCenterHeaderId) {
                    var assessmentService = $injector.get('assessmentService');
                    var callerInformationService = $injector.get('callerInformationService');
                    var deferred = $q.defer();
                    assessmentService.ensureAssessmentSectionID(assessmentTypeId).then(function (sectionId) {
                        callerInformationService.getCallCenterAssessmentResponse(callCenterHeaderId, assessmentTypeId).then(function (data) {
                            var responseId = 0;
                            if (hasData(data)) {
                                responseId = data.DataItems[0].ResponseID;
                            }
                            var obj = { sectionId: sectionId, responseId: responseId };
                            deferred.resolve(obj);
                        });
                    });
                    return deferred.promise;
                };

                var getAssessementResponse = function (assessmentTypeId, contactId, callCenterHeaderId) {
                    var assessmentService = $injector.get('assessmentService');
                    var callerInformationService = $injector.get('callerInformationService');
                    var deferred = $q.defer();
                    callerInformationService.getCallCenterAssessmentResponse(callCenterHeaderId, assessmentTypeId).then(function (data) {
                        if (hasData(data)) {
                            assessmentService.ensureAssessmentSectionID(assessmentTypeId).then(function (sectionId) {
                                assessmentService.getAssessmentResponseDetails(data.DataItems[0].ResponseID, sectionId).then(function (responseDetails) {
                                    if (responseDetails && hasData(responseDetails.data)) {
                                        deferred.resolve(responseDetails);
                                    }
                                    else {
                                        deferred.resolve(null);
                                    }
                                })
                            });
                        }
                        else {
                            deferred.resolve(null)
                        }
                    });
                    return deferred.promise;
                };

                var isReadOnlyParamChanged = function () {
                    if ((fromParams.ReadOnly
                            && toParams.ReadOnly
                            && fromParams.ReadOnly.toString().toLowerCase() === toParams.ReadOnly.toString().toLowerCase())) {
                        return true;
                    }

                    return false;
                }

                var getCenterProgressNote = function (callCenterHeaderID) {
                    var dfd = $q.defer();
                    this.get(callCenterHeaderID).then(function (response) {
                        if (hasData(response) && response.DataItems[0].ProgressNoteID) {
                            dfd.resolve(response)
                        }
                        else {
                            dfd.resolve(null);
                        }
                    });
                    return dfd.promise;
                };
                $rootScope.defaultFormName = getDefaultFormName();
                $('.row-offcanvas').removeClass('active');
                // call center law liason screen right navigation validation color
                if ((toState != null) && toState.name && (toState.name.indexOf('callcenter.lawliaison') === 0)) {
                    if (!((fromState != null) && fromState.name && (fromState.name.indexOf('callcenter.lawliaison') === 0) && (fromParams.ContactID === toParams.ContactID))) {
                        var contactId = toParams.ContactID ? toParams.ContactID : 0;
                        var callCenterHeaderID = toParams.CallCenterHeaderID ? toParams.CallCenterHeaderID : 0;
                        $timeout(function () {
                            var workflowActions = $("xen-workflow-action[data-state-name^='callcenter.lawliaison']").map(function () { return this.attributes['data-state-name'].value; }).get();

                            if (callCenterHeaderID != 0) {
                                var deferredLawLiaison = [];

                                if ($injector.has('assessmentService')) {
                                    var assessmentService = $injector.get('assessmentService');
                                    //1 lawlaison client demographics
                                    if ($injector.has('callerInformationService')) {
                                        var callCenterInformationService = $injector.get('callerInformationService');
                                        deferredLawLiaison.push(callCenterInformationService.get(callCenterHeaderID));
                                    }

                                    //2 lawlaison screening
                                    deferredLawLiaison.push(getAssessementResponse(ASSESSMENT_TYPE.LawLiaisonScreening, contactId, callCenterHeaderID));

                                    //3 law liaison services
                                    deferredLawLiaison.push(getSignature(callCenterHeaderID, SERVICE_RECORDING_SOURCE.LawLiaison));


                                    //4 lawlaison appointment screen 
                                    //deferredLawLiasion.push(assessmentService.getAssessmentResponseDetails(responseId, sectionId));

                                    //5 lawlaison progress note
                                    if ($injector.has('eSignatureService')) {
                                        var eSignatureService = $injector.get('eSignatureService');
                                        deferredLawLiaison.push(eSignatureService.getDocumentSignatures(DOCUMENT_TYPE.CallCenterProgressNote, callCenterHeaderID));
                                    }

                                    $q.all(deferredLawLiaison).then(function (processData) {
                                        var indx = 0;
                                        angular.forEach(workflowActions, function (state) {
                                            var data = (processData.slice(indx, indx + 1))[0];
                                            if (hasData(data) || (data && data.hasOwnProperty('ID') && data.ID !== 0) || (data && data.hasOwnProperty('data') && hasData(data.data))) {
                                                validationState = VALIDATION_STATE.Valid;
                                            }
                                            else {
                                                validationState = VALIDATION_STATE.Warning;
                                            }

                                            var stateObject = { stateName: state, validationState: validationState };
                                            $rootScope.$broadcast('rightNavigationCallCenterHandler', stateObject);
                                            indx = indx + 1;
                                        });
                                    });
                                }
                            }
                            else if (!((fromState != null) && fromState.name && (fromState.name === 'callcenter.lawliaison.initlawenforcement'))) {
                                //default everything to a neutral state and stop all spinners
                                for (var iIdx = 0; iIdx < workflowActions.length; iIdx++) {
                                    $rootScope.$broadcast(workflowActions[iIdx], {
                                        validationState: ' '
                                    });
                                }
                            }
                        });
                    }
                }

                // call center crisis line right navigation validation color
                if ((toState != null) && toState.name && (toState.name.indexOf('callcenter.crisisline') === 0)) {
                    if (!(fromState != null
                            && fromState.name
                            && (fromState.name.indexOf('callcenter.crisisline') === 0)
                            && (fromParams.ContactID === toParams.ContactID)
                            && isReadOnlyParamChanged())
                    ) {
                        var contactId = toParams.ContactID ? toParams.ContactID : 0;
                        var callCenterHeaderID = toParams.CallCenterHeaderID ? toParams.CallCenterHeaderID : 0;
                        $timeout(function () {
                            var workflowActions = $("xen-workflow-action[data-state-name^='callcenter.crisisline']").map(function () { return this.attributes['data-state-name'].value; }).get();
                            if (callCenterHeaderID != 0) {
                                var processQ = [];
                                if ($injector.has('assessmentService')) {
                                    var assessmentService = $injector.get('assessmentService');
                                    var assessmentServiceArray = [];
                                    assessmentServiceArray.push(getAssessementServiceResponse(ASSESSMENT_TYPE.ColumbiaSuicideSeverityRatingScale, contactId, callCenterHeaderID));
                                    assessmentServiceArray.push(getAssessementServiceResponse(ASSESSMENT_TYPE.CrisisAssessment, contactId, callCenterHeaderID));
                                    assessmentServiceArray.push(getAssessementServiceResponse(ASSESSMENT_TYPE.CrisisAdultScreening, contactId, callCenterHeaderID));
                                    assessmentServiceArray.push(getAssessementServiceResponse(ASSESSMENT_TYPE.CrisisChildScreening, contactId, callCenterHeaderID));
                                    assessmentServiceArray.push(getAssessementServiceResponse(ASSESSMENT_TYPE.CrisisLineProgressNote, contactId, callCenterHeaderID));
                                    $q.all(assessmentServiceArray).then(function (assessmentServiceArrayData) {
                                        //1 caller information screen
                                        if ($injector.has('callerInformationService')) {
                                            var callCenterInformationService = $injector.get('callerInformationService');
                                            processQ.push(callCenterInformationService.get(callCenterHeaderID));
                                        }

                                        //2 service recording screen
                                        processQ.push(getSignature(callCenterHeaderID, SERVICE_RECORDING_SOURCE.CallCenter));


                                        //3 columbia Suicide screen
                                        processQ.push(assessmentService.getAssessmentResponseDetails(assessmentServiceArrayData[0].responseId, assessmentServiceArrayData[0].sectionId));

                                        //4 crisis assessment screen
                                        processQ.push(assessmentService.getAssessmentResponseDetails(assessmentServiceArrayData[1].responseId, assessmentServiceArrayData[1].sectionId));

                                        //5 crisis adult screening
                                        processQ.push(assessmentService.getAssessmentResponseDetails(assessmentServiceArrayData[2].responseId, assessmentServiceArrayData[2].sectionId));

                                        //6 crisis child screening
                                        processQ.push(assessmentService.getAssessmentResponseDetails(assessmentServiceArrayData[3].responseId, assessmentServiceArrayData[3].sectionId));

                                        //7 crisis progress note screen
                                        if ($injector.has('eSignatureService')) {
                                            var eSignatureService = $injector.get('eSignatureService');
                                            processQ.push(eSignatureService.getDocumentSignatures(DOCUMENT_TYPE.CallCenterProgressNote, callCenterHeaderID));
                                        }

                                        $q.all(processQ).then(function (processData) {
                                            var indx = 0;
                                            angular.forEach(workflowActions, function (state) {
                                                var data = processData.slice(indx, indx + 1);
                                                validationState = VALIDATION_STATE.Valid;
                                                if ((hasDetails(data) && data[0] != null) && ((data != undefined && data[0].hasOwnProperty('DataItems') && data[0].DataItems.length > 0) || (data != undefined && data[0].hasOwnProperty('ID') && data[0].ID !== 0)) || (data != undefined && data[0].hasOwnProperty('data') && data[0].data.hasOwnProperty('DataItems') && data[0].data.DataItems.length > 0)) {
                                                    validationState = VALIDATION_STATE.Valid;
                                                }
                                                else {
                                                    validationState = VALIDATION_STATE.Warning;
                                                }
                                                var stateObject = { stateName: state, validationState: validationState };
                                                $rootScope.$broadcast('rightNavigationCallCenterHandler', stateObject);

                                                indx = indx + 1;
                                            });
                                        });
                                    });
                                }
                            }
                            else if (!((fromState != null) && fromState.name && (fromState.name === 'callcenter.crisisline.initialcallerinformation'))) {
                                //default everything to a neutral state and stop all spinners
                                for (var iIdx = 0; iIdx < workflowActions.length; iIdx++) {
                                    $rootScope.$broadcast(workflowActions[iIdx], { validationState: ' ' });
                                }
                            }
                        });
                    }
                }
            });
        }]);
