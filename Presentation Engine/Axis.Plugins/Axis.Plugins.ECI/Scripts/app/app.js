angular.module("xenatixApp")
    .config([
        '$stateProvider', 'lazyLoaderProvider', function ($stateProvider, lazyLoader) {
            $stateProvider
                //Registration-based states
                .state('eciregistration',
                    {
                        title: 'Registration',
                        url: '/Registration/ECI',
                        templateUrl: '/Plugins/ECI/ECIRegistration/ECIRegistrationNavigation',
                        controller: "eciRegistrationController as ctrl",
                        cache: false,
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.ECI/ECIRegistration');
                            }],
                            scriptHeader: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/miniHeader');
                            }]
                        }
                    }
                )
                .state('eciregistration.initialdemographics',
                    {
                        title: 'Demographics',
                        url: '/Demographics/Initial/{ClientTypeID:int}/:OtherContactID',
                        templateUrl: '/Plugins/ECI/ECIDemographic/Index',
                        controller: "eciDemographicController as ctrl",
                        cache: false,
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.ECI/ECIDemographic');
                            }],
                            regScriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Registration');
                            }]
                        },
                        data: {
                            permissionKey: 'ECI-Registration-Demographics'
                        }
                    }
                )
                .state('eciregistration.demographics',
                    {
                        title: 'Demographics',
                        url: '/Demographics/{ContactID:int}',
                        templateUrl: '/Plugins/ECI/ECIDemographic/Index',
                        controller: "eciDemographicController as ctrl",
                        cache: false,
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.ECI/ECIDemographic');
                            }],
                            regScriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Registration');
                            }]
                        },
                        data: {
                            permissionKey: 'ECI-Registration-Demographics'
                        }
                    }
                )
                .state('eciregistration.demographics.historylog', {
                    title: 'Demographics Change Log',
                    url: '/HistoryLog',
                    views: {
                        '@eciregistration': {
                            templateUrl: '/HistoryLog/Index',
                            controller: "historyLogController as ctrl",
                        }
                    },
                    resolve: {
                        scriptPromise: ['$http', function ($http) {
                            return lazyLoader.getScriptPromise($http, '/bundles/historyLog');
                        }]
                    },
                    data: {
                        permissionKey: 'ECI-Registration-Demographics'
                    }
                })
                .state('eciregistration.additionaldemographics',
                    {
                        title: 'Additional Demographics',
                        url: '/AdditionalDemographics/{ContactID:int}',
                        templateUrl: '/Plugins/ECI/ECIAdditionalDemographic/Index',
                        controller: "eciAdditionalDemographicController as ctrl",
                        cache: false,
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.ECI/ECIAdditionalDemographic');
                            }]
                        },
                        data: {
                            permissionKey: 'ECI-Registration-AdditionalDemographics'
                        }
                    }
                )
                .state('eciregistration.family',
                    {
                        title: 'Family Information',
                        url: '/FamilyInformation/{ContactID:int}',
                        templateUrl: function () {
                            return '/Collateral/Index';
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Collateral');
                            }],
                            contactTypeId: function () {
                                return CONTACT_TYPE.Family_Relationship;
                            }
                        },
                        controller: "collateralController as ctrl",
                        data: {
                            permissionKey: 'ECI-Registration-Collateral'
                        }
                    }
                )
                .state('eciregistration.referral',
                    {
                        title: 'Referral Detail',
                        url: '/Referral/{ContactID:int}',
                        templateUrl: '/Referral/Index',
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Referral');
                            }],
                            isECIClient: function () {
                                return true;
                            },
                        },
                        controller: "referralDetailController as ctrl",
                        data: {
                            permissionKey: 'ECI-Registration-Referral'
                        }
                    }
                )
                .state('eciregistration.benefits',
                    {
                        title: 'Payors',
                        url: '/Benefits/{ContactID:int}',
                        templateUrl: function () {
                            return '/ContactBenefit/Index';
                        },                    
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Benefits');
                            }],
                            collateralContactTypeID: function () {
                                return CONTACT_TYPE.Family_Relationship;
                            }
                        },
                        controller: "ContactBenefitController as ctrl",
                        data: {
                            permissionKey: 'ECI-Registration-Payors'
                        }
                    }
                )
                .state('eciregistration.financial',
                    {
                        title: 'Household Income',
                        url: '/Financial/{ContactID:int}',
                        templateUrl: function () {
                            return '/FinancialAssessment/Index';
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Financial');
                            }],
                            action: [function() {return ACTION.UPDATE}],
                        },
                        controller: "financialAssessmentController as ctrl",
                        data: {
                            permissionKey: 'ECI-Registration-HouseholdIncome'
                        }
                    }
                )
                //End of registration-based states
                //Patient profile-based states
                .state('patientprofile.chart.ifsps',
                    {
                        title: 'IFSP',
                        url: "/IFSP",
                        params: { IFSPTypeID: null },
                        views: {
                            '@patientprofile': {
                                templateUrl: "/Plugins/ECI/IFSP/Index",
                                controller: "ifspController as ctrl"
                            }
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, "/bundles/Plugins/Axis.Plugins.ECI/IFSP");
                            }]
                        },
                        data: {
                            permissionKey: 'ECI-IFSP-IFSP'
                        }
                    }
                )
                .state('patientprofile.chart.ifsps.ifsp',
                    {
                        url: '/{IFSPID:int}',
                        views: {
                            '@patientprofile': {
                                template: '<div ui-view></div>'
                            },
                            'navigation@patientprofile': {
                                template: function ($stateParams) {
                                    return '<ifsp-navigation />';
                                }
                            }
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/assessment');
                            }]
                        },
                        data: {
                            permissionKey: 'ECI-IFSP-IFSP'
                        }
                    }
                )
                .state('patientprofile.chart.ifsps.ifsp.header',
                    {
                        title: 'IFSP Header',
                        url: '/IFSPHeader',
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Plugins/ECI/IFSP/IFSPHeader',
                                controller: 'ifspController as ctrl'
                            }
                        },
                        data: {
                            permissionKey: 'ECI-IFSP-IFSP'
                        }
                    }
                )
                .state('patientprofile.chart.ifsps.ifsp.section',
                    {
                        title: 'Assessment Section',
                        url: '/{SectionID:int}/{ResponseID:int}',
                        views: {
                            '@patientprofile': {
                                template: '<assessment-section permission-key="ECI-IFSP-IFSP" data-return-state="patientprofile.chart.ifsps" />'
                            }
                        },
                        data: {
                            permissionKey: 'ECI-IFSP-IFSP'
                        }
                    }
                )
                .state('patientprofile.chart.ifsps.ifsp.report',
                    {
                        title: 'IFSP Report',
                        url: '/IFSPReport/{ResponseID:int}',
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Plugins/ECI/IFSP/IFSPReport',
                                controller: 'ifspController as ctrl'
                            }
                        },
                        data: {
                            permissionKey: 'ECI-IFSP-IFSP'
                        }
                    }
                )
               .state('patientprofile.chart.initialcontact', {
                   title: 'Initial Contact Note',
                   url: '/AttemptNote/{ContactID:int}/{NoteTypeID:int}',
                   params: { NoteHeaderID: null },
                   views: {
                       '@patientprofile': {
                           templateUrl: '/Plugins/ECI/ProgressNote/AttemptIndex',
                           controller: 'progressNoteController as ctrl'
                       }
                   },
                   resolve: {
                       pinkyPromise: ['$http', function ($http) {
                           return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                       }],
                       scriptPromise: ['$http', function ($http) {
                           return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.ECI/ProgressNote');
                       }]
                   },
                   data: {
                       permissionKey: 'ECI-ProgressNote-ProgressNote'
                   }
               })
                 .state('patientprofile.chart.initialcontactattempt', {
                     title: 'Initial Contact Attempt Note',
                     url: '/ProgressNote/{ContactID:int}/{NoteTypeID:int}',
                     params: { NoteHeaderID: null },
                     views: {
                         '@patientprofile': {
                             templateUrl: '/Plugins/ECI/ProgressNote/Index',
                             controller: 'progressNoteController as ctrl'
                         }
                     },
                     resolve: {
                         pinkyPromise: ['$http', function ($http) {
                             return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                         }],
                         scriptPromise: ['$http', function ($http) {
                             return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.ECI/ProgressNote');
                         }]
                     },
                     data: {
                         permissionKey: 'ECI-ProgressNote-ProgressNote'
                     }
                 })
                .state('patientprofile.chart.screenings',
                    {
                        title: 'Screening',
                        url: '/Screening',
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Plugins/ECI/Screening/Index',
                                controller: 'screeningController as ctrl'
                            }
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.ECI/Screening');
                            }]
                        },
                        data: {
                            permissionKey: 'ECI-Screening-Screening'
                        }
                    }
                )
                .state('patientprofile.chart.eligibilities',
                    {
                        title: 'Eligibility',
                        url: '/Eligibility',
                        params: { EligibilityCategoryID: null },
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Plugins/ECI/EligibilityDetermination/Index',
                                controller: 'eligibilityDeterminationController as ctrl'
                            }
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.ECI/EligibilityDetermination');
                            }]
                        },
                        data: {
                            permissionKey: 'ECI-Eligibility-Eligibility'
                        }
                    }
                )
                .state('patientprofile.chart.eligibilities.eligibility',
                    {
                        url: '/{EligibilityID:int}',
                        views: {
                            '@patientprofile': {
                                template: '<div ui-view></div>'
                            },
                            'navigation@patientprofile': {
                                templateUrl: '/Plugins/ECI/EligibilityDetermination/EligibilityNavigation'
                            }
                        }
                    }
                )
                .state('patientprofile.chart.eligibilities.eligibility.header',
                    {
                        title: 'Header',
                        url: '/Header',
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Plugins/ECI/EligibilityDetermination/EligibilityHeader',
                                controller: 'eligibilityDeterminationController as ctrl'
                            },
                            'navigation@patientprofile': {
                                templateUrl: '/Plugins/ECI/EligibilityDetermination/EligibilityNavigation'
                            }
                        },
                        data: {
                            permissionKey: 'ECI-Eligibility-Eligibility'
                        }
                    }
                )
                .state('patientprofile.chart.eligibilities.eligibility.calculation',
                    {
                        title: 'Calculation',
                        url: '/Calculation',
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Plugins/ECI/EligibilityCalculation/Calculation',
                                controller: 'eligibilityCalculationController as ctrl'
                            }
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.ECI/EligibilityCalculation');
                            }]
                        },
                        data: {
                            permissionKey: 'ECI-Eligibility-Eligibility'
                        }
                    }
                )
                .state('patientprofile.chart.eligibilities.eligibility.notes',
                    {
                        title: 'Notes',
                        url: '/Notes',
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Plugins/ECI/EligibilityDetermination/Notes',
                                controller: 'eligibilityDeterminationController as ctrl'
                            }
                        },
                        data: {
                            permissionKey: 'ECI-Eligibility-Eligibility'
                        }
                    }
                )
                .state('patientprofile.chart.eligibilities.eligibility.signature',
                    {
                        title: 'Signature',
                        url: '/Signature',
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Plugins/ECI/EligibilityDetermination/EligibilitySignature',
                                controller: 'eligibilityDeterminationController as ctrl'
                            }
                        },
                        data: {
                            permissionKey: 'ECI-Eligibility-Eligibility'
                        }
                    }
                )
                .state('patientprofile.chart.eligibilities.eligibility.report',
                    {
                        title: 'Report',
                        url: '/Report',
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Plugins/ECI/EligibilityDetermination/Report',
                                controller: 'eligibilityDeterminationController as ctrl'
                            }
                        },
                        data: {
                            permissionKey: 'ECI-Eligibility-Eligibility'
                        }
                    }
                )
                .state('patientprofile.chart.screenings.screening',
                    {
                        url: '/{ScreeningID:int}',
                        views: {
                            '@patientprofile': {
                                template: '<div ui-view></div>'
                            },
                            'navigation@patientprofile': {
                                template: function () {
                                    return '<screening-navigation />';
                                }
                            }
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/assessment');
                            }]
                        }
                    }
                )
                .state('patientprofile.chart.screenings.screening.header',
                    {
                        title: 'Screening Header',
                        url: '/Header',
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Plugins/ECI/Screening/ScreeningHeader',
                                controller: 'screeningController as ctrl'
                            }
                        },
                        data: {
                            permissionKey: 'ECI-Screening-Screening'
                        }
                    }
                )
                .state('patientprofile.chart.screenings.screening.section',
                    {
                        title: 'Assessment Section',
                        url: '/{SectionID:int}/{ResponseID:int}',
                        views: {
                            '@patientprofile': {
                                template: '<assessment-section permission-key="ECI-Screening-Screening" data-return-state="patientprofile.chart.screenings" />'
                            }
                        },
                        data: {
                            permissionKey: 'ECI-Screening-Screening'
                        }
                    }
                )
                .state('patientprofile.chart.screenings.screening.report',
                    {
                        title: 'Report',
                        url: '/Report',
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Plugins/ECI/Screening/ScreeningReport',
                                controller: 'screeningController as ctrl'
                            }
                        },
                        data: {
                            permissionKey: 'ECI-Screening-Screening'
                        }
                    }
                )
                .state('patientprofile.chart.assessmentsGrid',
                    {
                        title: 'Assessments',
                        url: "/Assessments",
                        views: {
                            '@patientprofile': {
                                templateUrl: "/AssessmentGrid/Index",
                                controller: "assessmentsGridController as ctrl"
                            }
                        },
                        resolve: {
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, "/bundles/assessmentsGrid");
                            }]
                        },
                        data: {
                            permissionKey: 'ECI-Screening-Screening'
                        }
                    }
                )
                .state('patientprofile.general.ecidemographics',
                    {
                        //ToDo: Remove ECI from the title and URL...they are there for testing purposes
                        title: 'Demographics',
                        url: '/ECI/Demographics',
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Plugins/ECI/ECIDemographic/Index',
                                controller: 'eciDemographicController as ctrl'
                            }
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.ECI/ECIDemographic');
                            }]
                        },
                        data: {
                            permissionKey: 'General-General-Demographics'
                        }
                    }
                )
                .state('patientprofile.general.eciadditional',
                    {
                        //ToDo: Remove ECI from the title and URL...they are there for testing purposes
                        title: 'Additional Demographics',
                        url: '/ECI/Additional',
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Plugins/ECI/ECIAdditionalDemographic/Index',
                                controller: 'eciAdditionalDemographicController as ctrl'
                            }
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.ECI/ECIAdditionalDemographic');
                            }]
                        },
                        data: {
                            permissionKey: 'General-General-AdditionalDemographics'
                        }
                    }
                )
                .state('patientprofile.general.ecireferral',
                    {
                        title: 'Referral',
                        url: '/ECI/Referral',
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Referral/Index',
                                controller: 'referralDetailController as ctrl'
                            }
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Referral');
                            }]
                        },
                        data: {
                            permissionKey: 'General-General-Referral'
                        }
                    }
                )
            .state('patientprofile.general.family',
                    {
                        title: 'Family Information',
                        url: '/FamilyInformation',
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Collateral/Index',
                                controller: 'collateralController as ctrl'
                            }
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Collateral');
                            }],
                            contactTypeId: function () {
                                return CONTACT_TYPE.Family_Relationship;
                            }
                        },
                        data: {
                            permissionKey: 'General-General-Collateral'
                        }
                    }
                )
            .state('patientprofile.benefits.ecibenefits',
                    {
                        title: 'Payors',
                        url: '/EciPayor',
                        views: {
                            '@patientprofile': {
                                templateUrl: '/ContactBenefit/Index',
                                controller: "ContactBenefitController as ctrl"
                            }
                        },
                        params: {
                            ContactPayorID: null
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Benefits');
                            }],
                            collateralContactTypeID: function () {
                                return CONTACT_TYPE.Family_Relationship;
                            }
                        },
                        data: {
                            permissionKey: 'Benefits-Payors-Payors'
                        }
                    }
                );

            //End of patient profile-based states

        }
    ]).run([
        '$rootScope', '$state', '$document', '$http', '$q', '$injector', '$timeout',
                function ($rootScope, $state, $document, $http, $q, $injector, $timeout) {

                    $rootScope.$on("$stateChangeSuccess", function (event, toState, toParams, fromState, fromParams) {
                        //default header redirections
                        if ((toState != null) && toState.name && (toState.name === 'patientprofile.chart.screenings.screening'))
                            $state.go('patientprofile.chart.screenings.screening.header');
                        if ((toState != null) && toState.name && (toState.name === 'patientprofile.chart.ifsps.ifsp'))
                            $state.go('patientprofile.chart.ifsps.ifsp.header');
                        //end default header redirections

                        if ((toState != null) && toState.name && (toState.name.indexOf('eciregistration') === 0)) {
                            if (!((fromState != null) && fromState.name && (fromState.name.indexOf('eciregistration.') === 0) && (fromParams.ContactID === toParams.ContactID))) {
                       
                                //experiment
                                var defaultValidationState = ' ';
                                var validationState = defaultValidationState;
                                var validState = 'valid';
                                var warningState = 'warning';
                                var contactId = toParams.ContactID ? toParams.ContactID : 0;
                                $timeout(function () {
                                    var workflowActions = $("xen-workflow-action[data-state-name^=eciregistration]").map(function () { return this.attributes['data-state-name'].value; }).get();
                                    if (contactId != 0) {
                                        var processQ = [];
                                        if ($injector.has('eciDemographicService')) {
                                            var eciDemographicService = $injector.get('eciDemographicService');
                                            processQ.push(eciDemographicService.get(contactId));
                                        }
                                        if ($injector.has('eciAdditionalDemographicService')) {
                                            var eciAdditionalDemographicService = $injector.get('eciAdditionalDemographicService');
                                            processQ.push(eciAdditionalDemographicService.getAdditionalDemographic(contactId));
                                        }
                                        if ($injector.has('referralAdditionalDetailService')) {
                                            var referralAdditionalDetailService = $injector.get('referralAdditionalDetailService');
                                            processQ.push(referralAdditionalDetailService.getReferral(contactId));
                                        }
                                        if ($injector.has('collateralService')) {
                                            var collateralService = $injector.get('collateralService');
                                            processQ.push(collateralService.get(contactId, CONTACT_TYPE.Family_Relationship, false));
                                        }
                                        if ($injector.has('contactBenefitService')) {
                                            var contactBenefitService = $injector.get('contactBenefitService');
                                            processQ.push(contactBenefitService.get(contactId));
                                        }
                                        if ($injector.has('financialAssessmentService')) {
                                            var financialAssessmentService = $injector.get('financialAssessmentService');
                                            processQ.push(financialAssessmentService.get(contactId));
                                        }
                                        $q.all(processQ).then(function (processData) {
                                            var indx = 0;
                                            angular.forEach(workflowActions, function (state) {
                                                var data = processData.slice(indx, indx + 1);
                                                if ((data != undefined && data.length > 0 && data[0] != null) && ((data[0].hasOwnProperty('DataItems') && data[0].DataItems.length > 0) || (data[0].hasOwnProperty('ID') && data[0].ID !== 0))) {
                                                    validationState = validState;
                                                    /// Fix for client 
                                                    if (data[0].hasOwnProperty('DataItems') && data[0].DataItems.length > 0 && data[0].DataItems[0].hasOwnProperty("ID") && data[0].DataItems[0].ID === 0)
                                                        validationState = warningState;
                                                    //fir for eci additionql demographices
                                                    if (hasData(data[0]) && data[0].DataItems[0].hasOwnProperty("AdditionalDemographicID") && data[0].DataItems[0].AdditionalDemographicID === 0) 
                                                        validationState = "invalid";
                                                } else {
                                                    validationState = warningState;
                                                }
                                                var stateObject = { stateName: state, validationState: validationState };                                                
                                                $rootScope.$broadcast('rightNavigationECIRegistrationHandler', stateObject);
                                                indx = indx + 1;
                                            });
                                        });
                                    }
                                    else if (!((fromState != null) && fromState.name && (fromState.name === 'eciregistration.initialdemographics'))) {
                                        //default everything to a neutral state and stop all spinners
                                        for (var iIdx = 0; iIdx < workflowActions.length; iIdx++) {
                                            $rootScope.$broadcast(workflowActions[iIdx], { validationState: ' ' });
                                        }
                                    }

                                });
                            }
                            $document.scrollTop(0);
                        }
                    });

                }
    ]);
