angular
    .module("xenatixApp")
    .config(['$stateProvider', 'lazyLoaderProvider', function ($stateProvider, lazyLoader) {
        $stateProvider
            .state('contacts', {
                title: 'Contacts',
                url: '/Contacts',
                templateUrl: '/ClientSearch/Index',
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/ClientSearch');
                    }]
                },
                controller: 'clientSearchController'
            })
            .state('program', {
                title: 'Program',
                url: '/Registration/Program',
                templateUrl: '/Program/Index',
                cache: false,
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Program');
                    }]
                },
                controller: "programController as ctrl"
            })
            .state('contactprogram', {
                title: 'Program',
                url: '/Registration/Program/{OtherContactID:int}/:ProgramID',
                templateUrl: '/Program/Index',
                cache: false,
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Program');
                    }]
                },
                controller: "programController as ctrl"
            })
            .state('registration', {
                title: 'Registration',
                url: '/Registration',
                templateUrl: '/Registration/Main',
                cache: false,
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Registration');
                    }],
                    initFunction: ['$rootScope', function ($rootScope) {
                        $rootScope.workflowActions = {};
                        return null;
                    }],
                    scriptHeader: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/miniHeader');
                    }]
                },
                controller: "registrationNavigationController as ctrl"
            })
            .state('registration.initialdemographics', {
                title: 'Demographics',
                url: '/Demographics/Initial/{ClientTypeID:int}/:OtherContactID',
                //params: {
                //    ContactID: {value: 0}
                //},
                templateUrl: function () {
                    return '/Registration/Index';
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Registration');
                    }]
                },
                controller: "RegistrationController as ctrl",
                data: {
                    permissionKey: 'Registration-Registration-Demographics'
                }
            })
            .state('registration.demographics', {
                title: 'Demographics',
                url: '/Demographics/{ContactID:int}',
                templateUrl: function () {
                    return '/Registration/Index';
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Registration');
                    }]
                },
                controller: "RegistrationController as ctrl",
                data: {
                    permissionKey: 'Registration-Registration-Demographics'
                }
            })
            .state('registration.additional', {
                title: 'Additional Demographics',
                url: '/Additional/{ContactID:int}',
                templateUrl: function () {
                    return '/AdditionalDemographic/Index';
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/AdditionalDemographic');
                    }]
                },
                controller: "additionalDemographyController as ctrl",
                data: {
                    permissionKey: 'Registration-Registration-AdditionalDemographics'
                }
            })
            .state('registration.referral', {
                title: 'Referral',
                url: '/Referral/{ContactID:int}',
                templateUrl: function () {
                    return '/Referral/Index';
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Referral');
                    }],
                    isECIClient: function () {
                        return false;
                    },
                },
                controller: "referralDetailController as ctrl",
                data: {
                    permissionKey: 'Registration-Registration-Referral'
                }
            })
            .state('registration.emergcontacts', {
                title: 'EmergencyContacts',
                url: '/EmergencyContacts/{ContactID:int}',
                templateUrl: function () {
                    return '/EmergencyContact/Index';
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/EmergencyContacts');
                    }]
                },
                controller: "emergencyContactController as ctrl",
                data: {
                    permissionKey: 'Registration-Registration-Demographics'
                }
            })
            .state('registration.benefits', {
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
                        return CONTACT_TYPE.Collateral;
                    }
                },
                controller: "ContactBenefitController as ctrl",
                data: {
                    permissionKey: 'Registration-Registration-Payors'
                }
            })
            .state('registration.financial', {
                title: 'Household Income Details',
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
                    action: [function () { return ACTION.UPDATE }],
                },
                controller: "financialAssessmentController as ctrl",
                data: {
                    permissionKey: 'Registration-Registration-HouseholdIncome'
                }
            })

            .state('registration.collateral', {
                title: 'Collateral',
                url: '/Collateral/{ContactID:int}',
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
                        return CONTACT_TYPE.Collateral;
                    }
                },
                controller: "collateralController as ctrl",
                data: {
                    permissionKey: 'Registration-Registration-Collateral'
                }
            })
            .state('registration.consent', {
                title: 'Consent',
                url: '/Consent/{ContactID:int}',
                templateUrl: function () {
                    return '/Consent/Index';
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Consent');
                    }]
                },
                controller: "consentController as ctrl",
                data: {
                    permissionKey: 'Registration-Registration-Demographics'
                }
            })
            .state('registration.fareport', {
                title: 'Financial Report',
                url: '/FinancialReport/{ContactID:int}',
                templateUrl: function () {
                    return '/FinancialReport/Index';
                },
                controller: "consentController as ctrl",
                data: {
                    permissionKey: 'Registration-Registration-Demographics'
                }
            })
            .state('referrals', {
                title: 'Referrals',
                url: '/Referral/:ReadOnly',
                templateUrl: '/ReferralSearch/ReferralNavigation',
                controller: 'referralParentController as ctrl',
                cache: false,
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/ReferralCommon');
                    }]
                }
            })
            .state('referrals.main', {
                title: 'Referrer',
                url: '/Requestor',
                templateUrl: '/ReferralRequestor/Index',
                controller: 'referralRequestorController as ctrl',
                cache: false,
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/ReferralRequestor');
                    }]
                },
                data: {
                    permissionKey: 'Referrals-Referral-Referrer'
                }
            })
            .state('referrals.requestor', {
                title: 'Referrer',
                url: '/Requestor/{ContactID:int}/{ReferralHeaderID:int}',
                templateUrl: '/ReferralRequestor/Index',
                controller: 'referralRequestorController as ctrl',
                cache: false,
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/ReferralRequestor');
                    }]
                },
                data: {
                    permissionKey: 'Referrals-Referral-Referrer'
                }
            })
            .state('referrals.referredto', {
                title: 'Referred Information',
                url: '/ReferredToInformation/{ContactID:int}/{ReferralHeaderID:int}',
                templateUrl: '/ReferralReferredInformation/Index',
                cache: false,
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/ReferredInformation');
                    }]
                },
                controller: "referralReferredInformationController as ctrl",
                data: {
                    permissionKey: 'Referrals-Referral-ReferredTo'
                }
            })
            .state('referrals.client', {
                title: 'Contact',
                url: '/Client/{ContactID:int}/{ReferralHeaderID:int}',
                templateUrl: function () {
                    return '/ReferralClientInformation/Index';
                },
                cache: false,
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/ReferralClientInformation');
                    }]
                },
                controller: "referralClientController as ctrl",
                data: {
                    permissionKey: 'Referrals-Referral-Contact'
                }
            })
            .state('referrals.followup', {
                title: 'Follow Up',
                url: '/Followup/{ContactID:int}/{ReferralHeaderID:int}',
                templateUrl: '/ReferralFollowup/Index',
                cache: false,
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/ReferralFollowup');
                    }]
                },
                controller: "referralFollowupController as ctrl",
                data: {
                    permissionKey: 'Referrals-Referral-FollowUpOutcome'
                }
            })
            .state('referrals.forwardedto', {
                title: 'Forwarded To',
                url: '/Forwarded/{ContactID:int}/{ReferralHeaderID:int}',
                templateUrl: '/ReferralForwarded/Index',
                cache: false,
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/ReferralForwarded');
                    }]
                },
                controller: "referralForwardedController as ctrl",
                data: {
                    permissionKey: 'Referrals-Referral-ForwardedTo'
                }
            })
            .state('referrals.disposition', {
                title: 'Disposition',
                url: '/Disposition/{ContactID:int}/{ReferralHeaderID:int}',
                templateUrl: '/ReferralDisposition/Index',
                controller: "referralDispositionController as ctrl",
                cache: false,
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/ReferralDisposition');
                    }]
                },
                data: {
                    permissionKey: 'Referrals-Referral-Disposition'
                }
            })
            .state('referrals.appointment', {
                title: 'Appointment',
                url: '/Appointment/{ReferralHeaderID:int}/{ContactID:int}',
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
                        return { module: 'registration', feature: 'contact' };
                    }]
                },
                data: {
                    permissionKey: 'Referrals-Referral-SingleAppointment'
                }
            })
            .state('patientprofile', {
                title: '{{header.FullName}}',
                url: '/Contact/{ContactID:int}',
                templateUrl: '/PatientProfile/Main',
                controller: 'patientProfileController as ctrl',
                abstract: true,
                cache: false,
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/PatientProfile');
                    }],
                    photoScriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/photo');
                    }],
                    isECIClient: ['$stateParams', 'registrationService', function ($stateParams, registrationService) {
                        return registrationService.get($stateParams.ContactID).then(function (data) {
                            if (hasData(data)) {
                                return data.DataItems[0].ClientTypeID == PROGRAM_TYPE.ECI
                            }
                            else
                                return false;
                        });
                    }]
                },
                data: {
                    permissionKey: 'General-General-Demographics'
                }
            })
            .state('patientprofile.general', {
                title: 'General',
                url: '/General',
                templateUrl: '/Tiles/Index',
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/RegistrationTile');
                    }]
                },
                controller: "registrationTileController as ctrl",
                data: {
                    permissionKey: 'General-General-Demographics'
                }
            })
            .state('patientprofile.general.emails', {
                title: 'Email',
                url: '/Emails',
                views: {
                    '@patientprofile': {
                        templateUrl: '/ContactEmail/Index',
                        controller: "contactEmailController as ctrl"
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/ContactEmail');
                    }]
                },
                data: {
                    permissionKey: 'General-General-Email'
                }
            })
            .state('patientprofile.general.address', {
                title: 'Address',
                url: '/Address',
                views: {
                    '@patientprofile': {
                        templateUrl: '/ContactAddress/Index',
                        controller: "contactAddressController as ctrl"
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Address');
                    }]
                },
                data: {
                    permissionKey: 'General-General-Address'
                }
            })
            .state('patientprofile.general.phone', {
                title: 'Phone',
                url: '/Phone',
                views: {
                    '@patientprofile': {
                        templateUrl: '/ContactPhone/Index',
                        controller: "contactPhoneController as ctrl"
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/ContactPhone');
                    }]
                },
                data: {
                    permissionKey: 'General-General-Phone'
                }
            })
            .state('patientprofile.general.demographics', {
                title: 'Demographics',
                url: '/Demographics',
                views: {
                    '@patientprofile': {
                        templateUrl: '/Registration/Index',
                        controller: "RegistrationController as ctrl"
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Registration');
                    }]
                },
                data: {
                    permissionKey: 'General-General-Demographics'
                }
            })
            .state('historylog', {
                title: 'Demographics Change Log',
                url: '/HistoryLog/{ContactID:int}',
                templateUrl: '/HistoryLog/Index',
                controller: "historyLogController as ctrl",
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/historyLog');
                    }]
                },
                data: {
                    permissionKey: 'General-General-Demographics'
                }
            })
            .state('patientprofile.general.additional', {
                title: 'Additional Demographics',
                url: '/Additional',
                views: {
                    '@patientprofile': {
                        templateUrl: '/AdditionalDemographic/Index',
                        controller: "additionalDemographyController as ctrl"
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/AdditionalDemographic');
                    }]
                },
                data: {
                    permissionKey: 'General-General-AdditionalDemographics'
                }
            })
            .state('patientprofile.benefits.benefits', {
                title: 'Payors',
                url: '/Payor',
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
                        return CONTACT_TYPE.Collateral;
                    }
                },
                data: {
                    permissionKey: 'Benefits-Payors-Payors'
                }
            })
            .state('patientprofile.benefits.financial', {
                title: 'Household Income',
                url: '/Financial',
                views: {
                    '@patientprofile': {
                        templateUrl: '/FinancialAssessment/HouseholdInfo',
                        controller: "financialDetailsController as ctrl"
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Financial');
                    }],
                },
                data: {
                    permissionKey: 'Benefits-HouseholdIncome-HouseholdIncome'
                }
            })
            .state('patientprofile.benefits.financial.financialdetails', {
                title: 'Details',
                url: '/Financial/:ReadOnly/{FinancialAssessmentID:int}',
                views: {
                    '@patientprofile': {
                        templateUrl: '/FinancialAssessment/Index',
                        controller: "financialAssessmentController as ctrl"
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Financial');
                    }],
                    action: [function () { return ACTION.UPDATE }],
                },
                data: {
                    permissionKey: 'Benefits-HouseholdIncome-HouseholdIncome'
                }
            })
            .state('patientprofile.benefits.financial.initial', {
                title: 'Details',
                url: '/HouseholdIncome/:FinancialAssessmentID',
                views: {
                    '@patientprofile': {
                        templateUrl: '/FinancialAssessment/Index',
                        controller: "financialAssessmentController as ctrl"
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Financial');
                    }],
                    action: [function () { return ACTION.CREATE }],
                },
                data: {
                    permissionKey: 'Benefits-HouseholdIncome-HouseholdIncome'
                }
            })

            .state('patientprofile.benefits.financialAssesments', {
                title: 'Financial Assessments',
                url: '/FinancialAssesments',
                views: {
                    '@patientprofile': {
                        templateUrl: '/FinancialAssessments/Index',
                        controller: "financialAssessmentsController as ctrl"
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/FinancialAssessments');
                    }],
                    registrationPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/FinancialSummary');
                    }]
                },
                data: {
                    permissionKey: 'Benefits-FinancialAssessment-FinancialAssessment',
                    workflowDataKey: 'Benefits-FinancialAssessment-FinancialAssessment'
                }
            })
            .state('patientprofile.benefits.financialAssesments.newFinancialSummary', {
                title: 'Financial Assessment',
                url: '/FinancialSummary',
                views: {
                    '@patientprofile': {
                        templateUrl: '/FinancialSummary/Index',
                        controller: "financialSummaryController as ctrl"
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/FinancialSummary');
                    }],
                    admissionScriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Admission');
                    }],
                    selfPayScriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/SelfPay');
                    }],
                    organizationID: ['$stateParams', 'admissionService', function ($stateParams, admissionService) {
                        return admissionService.get($stateParams.ContactID).then(function (data) {
                            if (data && data.DataItems && data.DataItems.length > 0)
                                return data.DataItems[0].CompanyID;
                            else
                                return null;
                        });
                    }]
                },
                data: {
                    permissionKey: 'Benefits-FinancialAssessment-FinancialAssessment'
                }
            })
            .state('patientprofile.benefits.financialAssesments.financialSummary', {
                title: 'Financial Assessment',
                url: '/FinancialSummary/{FinancialSummaryID:int}',
                params: {
                    ReadOnly: null
                },
                views: {
                    '@patientprofile': {
                        templateUrl: '/FinancialSummary/Index',
                        controller: "financialSummaryController as ctrl"
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/FinancialSummary');
                    }],
                    admissionScriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Admission');
                    }],
                    selfPayScriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/SelfPay');
                    }],
                    organizationID: ['$stateParams', 'admissionService', function ($stateParams, admissionService) {
                        return admissionService.get($stateParams.ContactID).then(function (data) {
                            if (data && data.DataItems && data.DataItems.length > 0)
                                return data.DataItems[0].CompanyID;
                            else
                                return null;
                        });
                    }]
                },
                data: {
                    permissionKey: 'Benefits-FinancialAssessment-FinancialAssessment'
                }
            })
            .state('patientprofile.general.collateral', {
                title: 'Collateral',
                url: '/Collateral',
                views: {
                    '@patientprofile': {
                        templateUrl: '/Collateral/Index',
                        controller: "collateralController as ctrl"
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Collateral');
                    }],
                    contactTypeId: ['$stateParams', 'registrationService', function ($stateParams, registrationService) {
                        return registrationService.get($stateParams.ContactID).then(function (data) {
                            if (data && data.DataItems && data.DataItems.length > 0)
                                return data.DataItems[0].ClientTypeID == PROGRAM_TYPE.ECI ? CONTACT_TYPE.Family_Relationship : CONTACT_TYPE.Collateral;
                        })
                    }]
                },
                data: {
                    permissionKey: 'General-General-Collateral'
                }
            })
            .state('patientprofile.general.referral', {
                title: 'Referral',
                url: '/Referral',
                params: { ReferralContactID: null, ReferralHeaderID: null },
                views: {
                    '@patientprofile': {
                        templateUrl: "/Referral/Index",
                        controller: "referralDetailController as ctrl"
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Referral');
                    }],
                    isECIClient: ['$stateParams', 'registrationService', function ($stateParams, registrationService) {
                        return registrationService.get($stateParams.ContactID).then(function (data) {
                            if (hasData(data)) {
                                return data.DataItems[0].ClientTypeID == PROGRAM_TYPE.ECI
                            }
                            else
                                return false;
                        });
                    }]
                },
                data: {
                    permissionKey: 'General-General-Referral'
                }
            })
            .state('patientprofile.consents', {
                title: 'Consents',
                url: '/Consents',
                templateUrl: '/Tiles/Index',
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/ConsentTile');
                    }]
                },
                controller: "consentTileController as ctrl",
                data: {
                    permissionKey: 'Consents-Assessment-Agency'
                }
            })
            .state('referralsearch', {
                title: 'Referral Search',
                url: '/ReferralSearch',
                templateUrl: "ReferralSearch/Index",
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/ReferralSearch');
                    }]
                },
                controller: "referralSearchController as ctrl",
                data: {
                    permissionKey: 'Referrals-Referral-Referrer'
                }
            })
            .state('patientprofile.consents.consentView', {
                title: 'HIPAA',
                url: '/Consent',
                views: {
                    '@patientprofile': {
                        templateUrl: '/Consent/Index',
                        controller: "consentController as ctrl"
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Consent');
                    }]
                },
                data: {
                    permissionKey: 'Registration-Registration-Demographics'
                }
            })
            .state('patientprofile.general.admissionDischarge', {
                url: '/AdmissionDischarge',
                views: {
                    'navigation@patientprofile': {
                        template: '<xen-workflows work-flow-options="admissionWorkFlowOptions" work-flow-model="workFlowModel"></xen-workflows>',
                        controller: "admissionDischargeController as ctrl"
                    }
                },
                resolve: {
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/DischargeCompany');
                    }],
                    DocumentTypeID: [function () {
                        return DOCUMENT_TYPE.ContactDischargeNote;
                    }]
                }
            })
            .state('patientprofile.general.admissionDischarge.dischargeCompany', {
                title: 'Discharge Company',
                url: '/DischargeCompany',
                views: {
                    '@patientprofile': {
                        templateUrl: '/DischargeCompany/Index',
                        controller: "dischargeCompanyController as ctrl"
                    }
                },
                resolve: {
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/DischargeCompany');
                    }]
                },
                data: {
                    permissionKey: 'General-General-CompanyDischarge'
                }
            })
            .state('patientprofile.general.admissionDischarge.dischargeProgramUnit', {
                title: 'Discharge Program Unit',
                url: '/DischargeProgramUnit',
                views: {
                    '@patientprofile': {
                        templateUrl: '/DischargeProgramUnit/Index',
                        controller: "dischargeProgramUnitController as ctrl"
                    }
                },
                resolve: {
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/DischargeCompany');
                    }]
                },
                data: {
                    permissionKey: 'General-General-ProgramUnitDischarge'
                }
            })
            .state('patientprofile.general.admissionDischarge.admission', {
                title: 'Admission',
                url: '/Admission',
                params: { ContactAdmissionID: null },
                views: {
                    '@patientprofile': {
                        templateUrl: '/Admission/Index',
                        controller: "admissionController as ctrl"
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Admission');
                    }]
                },
                data: {
                    permissionKey: 'General-General-Admission'
                }
            })
            .state('patientprofile.benefits.selfPay', {
                title: 'Self Pay',
                url: '/SelfPay',
                params: { SelfPayID: null },
                views: {
                    '@patientprofile': {
                        templateUrl: "/SelfPay/Index",
                        controller: "benefitSelfPayController as ctrl"
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/SelfPay');
                    }]
                },
                data: {
                    permissionKey: 'Benefits-SelfPay-SelfPay'
                }
            })
            .state('patientprofile.benefits.bapn', {
                url: '/Benefits',
                abstract: true,
                views: {
                    'navigation@patientprofile': {
                        template: '<ul class="list-group text-uppercase">' +
                                '<workflow-action data-title="Benefits Assistance" data-state-name="patientprofile.benefits.bapn.benefitsAssistanceProgressNote" data-state-params="{ ContactID: $stateParams.ContactID }" data-init-state="none"></workflow-action>' +
                            '</ul>'
                    }
                },
                data: {
                    permissionKey: 'Benefits-BenefitsAssistanceProgressNote-BenefitsAssistanceProgressNote'
                }
            })
            .state('patientprofile.benefits.bapn.benefitsAssistanceProgressNote', {
                title: 'Benefits Assistance Progress Note',
                url: '/BenefitsAssistanceProgressNote',
                views: {
                    '@patientprofile': {
                        templateUrl: "/BenefitsAssistanceProgressNote/Index",
                        controller: "benefitsAssistanceProgressNoteController as ctrl"
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/BenefitsAssistanceProgressNote');
                    }],
                    responseID: ['$stateParams', 'assessmentService', function ($stateParams, assessmentService) {
                        return assessmentService.ensureResponseID($stateParams.ContactID, ASSESSMENT_TYPE.BenefitAssessmentsProgressNote).then(function (responseID) {
                            angular.extend($stateParams, { ResponseID: responseID });
                            return responseID;
                        });
                    }]
                },
                data: {
                    permissionKey: 'Benefits-BenefitsAssistanceProgressNote-BenefitsAssistanceProgressNote',
                    permissionVoidKey: 'Benefits-BenefitsAssistanceProgressNote-Void',
                    workflowDataKey: 'Benefits-BenefitsAssistanceProgressNote-BenefitsAssistanceProgressNote'
                },
                onEnter: ['cacheService', function (cacheService) {
                    cacheService.remove('IsVoidedRecord');
                }]
            })
            .state('patientprofile.benefits.bapn.benefitsAssistanceProgressNote.bapnNavigation', {
                url: '/{ResponseID:int}/{SectionID:int}/{BenefitsAssistanceID:int}/:ReadOnly/{DocumentStatusID:int}',
                abstract: true,
                views: {
                    '@patientprofile': {
                        template: '<div ui-view></div>'
                    },
                    'navigation@patientprofile': {
                        templateUrl: '/BenefitsAssistanceProgressNote/BAPNNavigation',
                        controller: "bapnNavigationController as ctrl"
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptUIPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/bapnDetails');
                    }],
                    scriptNavigationPromis: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/BapnNavigation');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/RecordingService');
                    }]
                }
            })
            .state('patientprofile.benefits.bapn.benefitsAssistanceProgressNote.bapnNavigation.section', {
                title: 'Benefits Assessment Progress Note Section',
                views: {
                    '@patientprofile': {
                        template: '<assessment-section permission-key="Benefits-BenefitsAssistanceProgressNote-BenefitsAssistanceProgressNote" data-return-state="patientprofile.benefits.bapn.benefitsAssistanceProgressNote.bapnNavigation.section" prepopulatedData="prepopulatedData" savemethod="saveRecordedService" no-access-to-other="{{noAccessToOther}}" on-print-report="initNoteReport" on-post-assessment-response="disableBAPN"/>',
                        controller: 'bapnDetailsController as ctrl'
                    }
                },
                resolve: {
                    titlePromise: ['$stateParams', 'assessmentService', function ($stateParams, assessmentService) {
                        var _this = this;
                        return assessmentService.getAssessmentSections(ASSESSMENT_TYPE.BenefitAssessmentsProgressNote).then(function (data) {
                            if (data.ResultCode === 0) {
                                angular.forEach(data.DataItems, function (item) {
                                    if (item.AssessmentSectionID == $stateParams.SectionID) {
                                        _this.self.title = item.SectionName;
                                    }
                                });
                            }
                        });
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/assessment');
                    }]
                },
                data: {
                    permissionKey: 'Benefits-BenefitsAssistanceProgressNote-BenefitsAssistanceProgressNote'
                }
            })
            .state('bapnService', {
                title: 'Services',
                parent: 'patientprofile.benefits.bapn.benefitsAssistanceProgressNote.bapnNavigation',
                url: '/ServiceRecording',
                templateUrl: "/RecordingService/Index",
                controller: "bapnDetailsController as ctrl",
                data: {
                    permissionKey: BenifitsPermissionKey.Benefits_BAPN_BAPN
                }
            })
            .state('initializeBapnService', {
                title: 'Services',
                parent: 'patientprofile.benefits.bapn.benefitsAssistanceProgressNote.bapnNavigation',
                url: '/ServiceRecording',
                templateUrl: "/RecordingService/Index",
                controller: "bapnDetailsController as ctrl",
                data: {
                    permissionKey: 'Benefits-BenefitsAssistanceProgressNote-BenefitsAssistanceProgressNote'
                }
            })
            .state('patientprofile.chart.recordedservices', {
                title: 'Recorded Services',
                url: '/RecordedServices',
                views: {
                    '@patientprofile': {
                        templateUrl: "/ServiceRecordingList/Index",
                        controller: "recordingServiceListController as ctrl",
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/ServiceList');
                    }],
                },
                data: {
                    permissionKey: 'Chart-Chart-RecordedServices'
                },
                data: {
                    permissionKey: 'Benefits-BenefitsAssistanceProgressNote-BenefitsAssistanceProgressNote'
                }
            })
            .state('patientprofile.benefits.bapn.benefitsAssistanceProgressNote.bapnNavigation.benefitsScreening', {
                title: 'Benefits Screening',
                url: '/BenefitsScreening/:ReadOnly',
                params: {
                    SectionID: null,
                    ResponseID: null
                },
                views: {
                    '@patientprofile': {
                        template: '<assessment-section permission-key="Benefits-BenefitsAssistanceProgressNote-BenefitsAssistanceProgressNote" data-return-state="patientprofile.benefits.bapn.benefitsAssistanceProgressNote.bapnNavigation.section" savemethod="saveRecordedService"></assessment-section>',
                        controller: 'bapnController as ctrl'
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/BAPN');
                    }],
                    assessmentScripts: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/assessment');
                    }],
                    sectionID: ['$stateParams', 'assessmentService', function ($stateParams, assessmentService) {
                        return assessmentService.ensureAssessmentSectionID(ASSESSMENT_TYPE.BenefitAssessmentsProgressNote).then(function (data) {
                            angular.extend($stateParams, { SectionID: data });
                            return data;
                        });
                    }],
                    responseID: ['$stateParams', 'assessmentService', function ($stateParams, assessmentService) {
                        return assessmentService.ensureResponseID($stateParams.ContactID, ASSESSMENT_TYPE.BenefitAssessmentsProgressNote).then(function (responseID) {
                            angular.extend($stateParams, { ResponseID: responseID });
                            return responseID;
                        });
                    }]
                },
                data: {
                    permissionKey: 'Benefits-BenefitsAssistanceProgressNote-BenefitsAssistanceProgressNote'
                }
            })
            .state('patientprofile.benefits', {
                title: 'Benefits',
                url: '/Benefits',
                templateUrl: '/Tiles/Index',
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/BenefitsTile');
                    }]
                },
                controller: "benefitsTileController as ctrl",
                data: {
                    permissionKey: BenifitsPermissionKey.Benefits_FinancialAssessment_FinancialAssessment
                }
            })
            .state('patientprofile.intake', {
                title: 'Intake',
                url: '/Intake',
                templateUrl: '/Tiles/Index',
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/IntakeTile');
                    }]
                },
                controller: "intakeTileController as ctrl",
                data: {
                    permissionKey: LettersPermissionKey.Intake_IDDForms_Forms
                }
            })
            .state('patientprofile.intake.navi', {
                abstract: true,
                views: {
                    'navigation@patientprofile': {
                        template: '<ul class="list-group text-uppercase">' +
                            '<workflow-action data-title="Letters" data-state-name="patientprofile.intake.navi.letters" data-init-state="none"></workflow-action>' +
                            '</ul>'
                    }
                }
            })
            .state('patientprofile.intake.formnavi', {
                abstract: true,
                views: {
                    'navigation@patientprofile': {
                        template: '<ul class="list-group text-uppercase">' +
                            '<workflow-action data-title="Forms" data-state-name="patientprofile.intake.formnavi.forms" data-init-state="none"></workflow-action>' +
                            '</ul>'
                    }
                }
            })
            .state('patientprofile.intake.navi.letters', {
                title: 'IDD Letters',
                url: '/Letters',
                views: {
                    '@patientprofile': {
                        templateUrl: '/Letters/Index',
                        controller: "lettersController as ctrl"
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Letters');
                    }]
                },
                data: {
                    permissionKey: 'Intake-IDDLetters-Letters'
                }
            })
            .state('patientprofile.intake.navi.letters.letternavi', {
                abstract: true,
                views: {
                    '@patientprofile': {
                        template: '<div ui-view></div>'
                    },
                    'navigation@patientprofile': {
                        template: '<letter-navigation />'
                    }
                }
            })
            .state('patientprofile.intake.navi.letters.letternavi.lettersSection', {
                title: 'Section',
                url: '/{AssessmentID:int}/{ResponseID:int}/{ReadOnly}/{SectionID:int}/{ContactLettersID:int}',
                views: {
                    '@patientprofile': {
                        template: '<assessment-section permission-key="Intake-IDDLetters-Letters" data-return-state="patientprofile.intake.navi.letters.letternavi.lettersSection" no-access-to-other="{{hasLetterSentDate}}" savemethod="saveletters" prepopulatedData="prepopulatedData" on-print-report="initLetterReport" is-mandatory="true"/>',
                        controller: 'lettersDetailsController as ctrl'
                    }
                },
                resolve: {
                    titlePromise: ['$stateParams', 'assessmentService', function ($stateParams, assessmentService) {
                        var _this = this;
                        return assessmentService.getAssessment().then(function (data) {
                            if (data.ResultCode === 0) {
                                angular.forEach(data.DataItems, function (item) {
                                    if (item.AssessmentID == $stateParams.AssessmentID) {
                                        _this.self.title = item.AssessmentName;
                                    }
                                });
                            }
                        });
                    }],

                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/assessment');
                    }],
                    scriptUIPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/LettersDetails');
                    }]
                },
                data: {
                    permissionKey: 'Intake-IDDLetters-Letters'
                }
            })
            .state('patientprofile.intake.formnavi.forms', {
                title: 'IDD Forms',
                url: '/Forms',
                views: {
                    '@patientprofile': {
                        templateUrl: '/Letters/Index',
                        controller: "lettersController as ctrl"
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Letters');
                    }]
                },
                data: {
                    permissionKey: 'Intake-IDDForms-Forms',
                    permissionVoidKey: 'Intake-IDDForms-Void',
                    workflowDataKey: 'Intake-IDDForms-Forms'

                },
                onEnter: ['cacheService', function (cacheService) {
                    cacheService.remove('IsVoidedRecord');
                }]
            })
            .state('patientprofile.intake.formnavi.forms.formsnavi', {
                abstract: true,
                url: '/{DocumentStatusID:int}',
                views: {
                    '@patientprofile': {
                        template: '<div ui-view></div>'
                    },
                    'navigation@patientprofile': {
                        template: '<xen-workflows work-flow-options="intakeWorkFlowOptions" work-flow-model="workFlowModel"></xen-workflows>',
                        controller: "intakeFormNavigationController as ctrl"
                    }
                },
                resolve: {
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/RecordingService');
                    }],
                    scriptUIPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/FormsDetails');
                    }]
                },
            })
            .state('formservice', {
                title: 'Services',
                parent: 'patientprofile.intake.formnavi.forms.formsnavi',
                url: '/ServiceRecording/{AssessmentID:int}/{SectionID:int}/{ReadOnly}/{ResponseID:int}/{ContactFormsID:int}',
                templateUrl: "/RecordingService/Index",
                controller: "formsDetailsController as ctrl",
                data: {
                    permissionKey: LettersPermissionKey.Intake_IDDForms_Forms
                }
            })
            .state('initializeformservice', {
                title: 'Services',
                parent: 'patientprofile.intake.formnavi.forms.formsnavi',//patientprofile.intake.navi.letters.letternavi',
                url: '/ServiceRecording/{AssessmentID:int}/{SectionID:int}/{ReadOnly}/{ResponseID:int}',
                templateUrl: "/RecordingService/Index",
                controller: "formsDetailsController as ctrl",
                data: {
                    permissionKey: 'Intake-IDDForms-Forms'
                }
            })
            .state('patientprofile.intake.formnavi.forms.formsnavi.formsSection', {
                title: 'Section',
                url: '/{AssessmentID:int}/{SectionID:int}/{ReadOnly}/{ResponseID:int}/{ContactFormsID:int}',
                views: {
                    '@patientprofile': {
                        template: '<assessment-section  data-return-state="patientprofile.intake.formnavi.forms" prepopulatedData="prepopulatedData" on-print-report="initFormsReport" savemethod="saveRecordedService" no-access-to-other="{{noAccessToOther}}" on-post-assessment-response="disableForm" />',
                        controller: 'formsDetailsController as ctrl'
                    }
                },
                resolve: {
                    titlePromise: ['$stateParams', 'assessmentService', function ($stateParams, assessmentService) {
                        var _this = this;
                        return assessmentService.getAssessmentSections(ASSESSMENT_TYPE.IDDIntakeForms).then(function (data) {
                            if (data.ResultCode === 0) {
                                angular.forEach(data.DataItems, function (item) {
                                    if (item.AssessmentSectionID == $stateParams.SectionID) {
                                        _this.self.title = item.SectionName;
                                    }
                                });
                            }
                        });
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/assessment');
                    }]
                }
            })
            .state('patientprofile.consents.agency', {
                abstract: true,
                views: {
                    'navigation@patientprofile': {
                        template: '<xen-workflows work-flow-options="concentsWorkFlowOptions" work-flow-model="workFlowModel"></xen-workflows>',
                        controller: "consentsNavigationController as ctrl"
                    }
                },
                resolve: {
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/consents');
                    }]
                }
            })
            .state('patientprofile.consents.agency.agencyView', {
                title: 'Agency',
                url: '/Agency',
                views: {
                    '@patientprofile': {
                        templateUrl: '/Consents/Consents/Index',
                        controller: "consentsController as ctrl"
                    }
                },
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }]
                },
                data: {
                    permissionKey: 'Consents-Assessment-Agency'
                }
            })
            .state('consentexpire', {
                title: 'Expire',
                parent: 'patientprofile.consents.agency.agencyView',
                url: '/{ReadOnly}/{AssessmentID:int}/{SectionID:int}/{ResponseID:int}/{ExpireAssessmentID:int}/{ContactConsentID:int}',
                views: {
                    '@patientprofile': {
                        template: '<assessment-section permission-key="Consents-Assessment-Agency" data-return-state="patientprofile.consents.agency.agencyView" ' +
                                    ' template-class-name="ase-template-consent" savemethod="saveConsent" prepopulatedData="prepopulatedData" no-access-to-other="{{noAccessToOther}}" on-post-assessment-response="noAccessToOtherUser"></assessment-section>',
                        controller: 'consentDetailsController as ctrl'
                    }
                },
                resolve: {
                    formNamePromise: ['$stateParams', 'assessmentService', function ($stateParams, assessmentService) {
                        var _thisState = this;
                        return assessmentService.getAssessment($stateParams.ExpireAssessmentID).then(function (data) {
                            // Changed state title dynamically
                            if (hasData(data)) {
                                _thisState.self.title = '';
                                var title = 'Expire (' + data.DataItems[0].AssessmentName + ')';
                                _thisState.self.title = title;
                                return data.DataItems[0].AssessmentName;
                            }
                        });
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/assessment');
                    }],
                    scriptUIPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/consentDetails');
                    }]
                },
                data: {
                    permissionKey: 'Consents-Assessment-Agency'
                }
            })
            .state('agencyViewSection', {
                title: 'Section',
                parent: 'patientprofile.consents.agency.agencyView',
                url: '/{ReadOnly}/{AssessmentID:int}/{SectionID:int}/{ResponseID:int}/{ContactConsentID:int}',
                views: {
                    '@patientprofile': {
                        template: '<assessment-section permission-key="Consents-Assessment-Agency" data-return-state="patientprofile.consents.agency.agencyView"  on-print-report="initAgencyReport" template-class-name="ase-template-consent" savemethod="saveConsent" prepopulatedData="prepopulatedData"  no-access-to-other="{{noAccessToOther}}" on-post-assessment-response="noAccessToOtherUser" custom-print="consentPrintReport" />',
                        controller: 'consentDetailsController as ctrl'
                    }
                },
                resolve: {
                    formNamePromise: ['$stateParams', 'assessmentService', function ($stateParams, assessmentService) {
                        var _this = this;
                        return assessmentService.getAssessment($stateParams.AssessmentID).then(function (data) {
                            // Changed state title dynamically
                            if (hasData(data)) {
                                _this.self.title = '';
                                _this.self.title = data.DataItems[0].AssessmentName;
                                return data.DataItems[0].AssessmentName;
                            }
                        });
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/assessment');
                    }],
                    scriptUIPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/consentDetails');
                    }]
                },
                data: {
                    permissionKey: 'Consents-Assessment-Agency'
                }
            })
            .state('lawliaison', {
                title: 'Law Liaison',
                url: '/LawLiaison',
                templateUrl: '/LawLiaison/Index',
                controller: 'lawLiaisonSummaryController as ctrl',
                cache: false,
                resolve: {
                    pinkyPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                    }],
                    registrationPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Registration/Registration');
                    }],
                    scriptPromise: ['$http', function ($http) {
                        return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.CallCenter/LawLiaisonSummary');
                    }]
                },
                onEnter: ['cacheService', function (cacheService) {
                    cacheService.remove('lawLiaisonFollowUp');
                }],
                onExist: ['cacheService', function (cacheService) {
                    cacheService.remove('IsReadOnlyLLScreens');
                }]
            })
    }])
    .run(['$rootScope', '$http', '$document', '$timeout', 'roleSecurityService', 'assessmentService', '$q',
        function ($rootScope, $http, $document, $timeout, roleSecurityService, assessmentService, $q) {
            $rootScope.$on("$stateChangeSuccess", function (event, toState, toParams, fromState, fromParams) {
                $('#sortModal').remove(); // Remove sort model to refresh every time
                $document.scrollTop(0);

                if ((toState != null) && toState.name && (toState.name.indexOf('registration') == 0)) {
                    if (!((fromState != null) && fromState.name && (fromState.name.indexOf('registration.') == 0) && (fromParams.ContactID === toParams.ContactID))) {
                        $timeout(function () {
                            var workflowActions = $("xen-workflow-action[data-state-name^=registration]").map(function () { return this.attributes['data-state-name'].value; }).get();
                            var contactId = toParams.ContactID ? toParams.ContactID : 0;
                            if (contactId > 0) {
                                $http.get('/Registration/NavigationValidationStates', {
                                    params: { contactId: toParams.ContactID, workflowActions: workflowActions }
                                })
                                .then(function (response) {
                                    var data = eval(response.data);
                                    for (var state in data) {
                                        var stateObject = {
                                            stateName: state, validationState: data[state]
                                        };

                                        $rootScope.$broadcast('rightNavigationRegistrationHandler', stateObject);

                                        if (workflowActions.indexOf(state) >= 0)
                                            workflowActions.splice(workflowActions.indexOf(state), 1);
                                    }
                                }).finally(function () { // whether it succeeded or not, stop any remaining spinners ...
                                    for (var iIdx = 0; iIdx < workflowActions.length; iIdx++) {
                                        var stateObject = {
                                            stateName: workflowActions[iIdx], validationState: ' '
                                        };
                                        $rootScope.$broadcast('rightNavigationRegistrationHandler', stateObject);
                                    }
                                });
                            } else if (!((fromState != null) && fromState.name && (fromState.name === 'registration.initialdemographics'))) {
                                for (var iIdx = 0; iIdx < workflowActions.length; iIdx++) {
                                    var stateObject = {
                                        stateName: workflowActions[iIdx], validationState: ' '
                                    };
                                    $rootScope.$broadcast('rightNavigationRegistrationHandler', stateObject);
                                }
                            }
                        });
                    }
                }
                else if ((toState != null) && toState.name && (toState.name.indexOf('referral') == 0)) {
                    $rootScope.ReferralHeaderID = toParams.ReferralHeaderID;
                    $rootScope.ReferralContactID = toParams.ContactID;
                }
                if ((toState != null) && toState.name && ((toState.name.toLowerCase().indexOf('bapnservice') == 0) || toState.name.toLowerCase().indexOf('bapnnavigation.section') > 0)) {
                    if (!((fromState != null) && fromState.name && (fromParams.BenefitsAssistanceID === toParams.BenefitsAssistanceID))) {
                        $timeout(function () {
                            var workflowActions = $("xen-workflow-action[data-state-name^=bapnService]").map(function () { return this.attributes['data-state-name'].value; }).get();
                            var benefitsAssistanceID = toParams.BenefitsAssistanceID ? toParams.BenefitsAssistanceID : 0;
                            var responseID = toParams.ResponseID ? toParams.ResponseID : 0;
                            var arrPromise = [];
                            var sectionStateName = "patientprofile.benefits.bapn.benefitsAssistanceProgressNote.bapnNavigation.section";

                            if (benefitsAssistanceID) {
                                assessmentService.getAssessmentSections(ASSESSMENT_TYPE.BenefitAssessmentsProgressNote).then(function (data) {
                                    if (data.ResultCode === 0) {
                                        for (var iIdx = 0; iIdx < data.DataItems.length; iIdx++) {
                                            if (!data.DataItems[iIdx].PermissionKey || roleSecurityService.hasPermission(data.DataItems[iIdx].PermissionKey, 'read')) {
                                                if (benefitsAssistanceID) {
                                                    arrPromise.push(assessmentService.getAssessmentResponseDetails(responseID, data.DataItems[iIdx].AssessmentSectionID));
                                                }
                                            }
                                        }
                                        $q.all(arrPromise)
                                            .then(function (responseData) {
                                                var totalResponses = responseData.length;
                                                for (iIdx = 0; iIdx < totalResponses; iIdx++) {
                                                    $rootScope.$broadcast(sectionStateName + responseData[iIdx].config.params.sectionId, { validationState: hasData(responseData[iIdx].data) ? 'valid' : 'warning' });
                                                }
                                            })
                                            .finally(function () {
                                                if (benefitsAssistanceID) {
                                                    var stateDetail = { stateName: "bapnService", validationState: 'valid' };
                                                    $rootScope.$broadcast('rightNavigationIntakeFormHandler', stateDetail);
                                                }
                                            });
                                    }
                                });
                            }

                        }, 1);
                    }
                }
                if ((toState != null) && toState.name && ((toState.name.indexOf('formservice') == 0) || toState.name.indexOf('formsSection') > 0)) {
                    if (!((fromState != null) && fromState.name && (fromParams.ContactFormsID === toParams.ContactFormsID))) {
                        $timeout(function () {
                            var workflowActions = $("xen-workflow-action[data-state-name^=formservice]").map(function () { return this.attributes['data-state-name'].value; }).get();
                            var contactFormsID = toParams.ContactFormsID ? toParams.ContactFormsID : 0;
                            var responseID = toParams.ResponseID ? toParams.ResponseID : 0;
                            var arrPromise = [];
                            var sectionStateName = "patientprofile.intake.formnavi.forms.formsnavi.formsSection";
                            if (contactFormsID) {
                                assessmentService.getAssessmentSections(ASSESSMENT_TYPE.IDDIntakeForms).then(function (data) {
                                    if (data.ResultCode === 0) {
                                        for (var iIdx = 0; iIdx < data.DataItems.length; iIdx++) {
                                            if (!data.DataItems[iIdx].PermissionKey || roleSecurityService.hasPermission(data.DataItems[iIdx].PermissionKey, 'read')) {
                                                if (contactFormsID) {
                                                    arrPromise.push(assessmentService.getAssessmentResponseDetails(responseID, data.DataItems[iIdx].AssessmentSectionID));
                                                }
                                            }
                                        }
                                        $q.all(arrPromise).then(function (responseData) {
                                            var totalResponses = responseData.length;
                                            for (iIdx = 0; iIdx < totalResponses; iIdx++) {
                                                $rootScope.$broadcast(sectionStateName + responseData[iIdx].config.params.sectionId, { validationState: hasData(responseData[iIdx].data) ? 'valid' : 'warning' });
                                            }
                                        }).finally(function () {
                                            if (contactFormsID) {
                                                var stateDetail = { stateName: "formservice", validationState: 'valid' };
                                                $rootScope.$broadcast('rightNavigationIntakeFormHandler', stateDetail);
                                            }
                                        });

                                    }
                                });
                            }
                        });
                    }
                }


            });
        }
    ]);
