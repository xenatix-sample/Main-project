angular.module("xenatixApp")
    .config([
        '$stateProvider', 'lazyLoaderProvider', function ($stateProvider, lazyLoader) {
            $stateProvider
                .state('patientprofile.chart.intake',
                    {
                        url: '/Intake',
                        views: {
                            'navigation@patientprofile': {
                                template: '<ul class="list-group text-uppercase">' +
                                    '<workflow-action data-title="Chief Complaint" data-state-name="patientprofile.chart.intake.chiefcomplaint" data-state-params="{ ContactID: $stateParams.ContactID }" data-init-state="none"></workflow-action>' +
                                    '<workflow-action data-title="Vitals" data-state-name="patientprofile.chart.intake.vitals" data-state-params="{ ContactID: $stateParams.ContactID }" data-init-state="none"></workflow-action>' +
                                    '<workflow-action data-title="Food/Env Allergies" data-state-name="patientprofile.chart.intake.allergy" data-state-params="{ ContactID: $stateParams.ContactID, AllergyTypeID: 1 }" data-init-state="none"></workflow-action>' +
                                    '<workflow-action data-title="Drug Allergies" data-state-name="patientprofile.chart.intake.drugallergy" data-state-params="{ ContactID: $stateParams.ContactID, AllergyTypeID: 2 }" data-init-state="none"></workflow-action>' +
                                    '<workflow-action data-title="Drug Intolerances" data-state-name="patientprofile.chart.intake.drugintolerance" data-state-params="{ ContactID: $stateParams.ContactID, AllergyTypeID: 3 }" data-init-state="none"></workflow-action>' +
                                    '<workflow-action data-title="Review of Systems" data-state-name="patientprofile.chart.intake.reviewOfSystems" data-state-params="{ ContactID: $stateParams.ContactID }" data-init-state="none"></workflow-action>' +
                                    '<workflow-action data-title="Medical History" data-state-name="patientprofile.chart.intake.medicalhistory" data-init-active="patientprofile.chart.intake.medicalhistory.medicalhistorydetails" data-state-params="{ ContactID: $stateParams.ContactID }" data-init-state="none"></workflow-action>' +
                                    '<workflow-action data-title="Social Relationship History" data-state-name="patientprofile.chart.intake.socialrelationship" data-init-active="patientprofile.chart.intake.socialrelationship.socialrelationshiphistory" data-state-params="{ ContactID: $stateParams.ContactID }" data-init-state="none"></workflow-action>' +
                                    '<workflow-action data-title="History of Present Illness" data-state-name="patientprofile.chart.intake.presentillness" data-init-active="patientprofile.chart.intake.presentillness" data-state-params="{ ContactID: $stateParams.ContactID }" data-init-state="none"></workflow-action>' +
                                    '<workflow-action data-title="Clinical Assessment" data-state-name="patientprofile.chart.intake.clinicalAssessment" data-state-params="{ ContactID: $stateParams.ContactID }" data-init-state="none"></workflow-action>' +
                                    '<workflow-action data-title="Note" data-state-name="patientprofile.chart.intake.note" data-init-active="patientprofile.chart.intake.note.notedetail" data-state-params="{ ContactID: $stateParams.ContactID }" data-init-state="none"></workflow-action>' +
                             '</ul>'
                            }
                        }
                    }
                )
                .state('patientprofile.chart.intake.vitals',
                    {
                        title: 'Vitals',
                        url: '/Vitals',
                        views: {
                            '@patientprofile': {
                                templateUrl: "/Plugins/Clinical/Vital/Index",
                                controller: "vitalController as ctrl"
                            }
                        },
                        resolve: {
                            chartsPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/chartJs');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Clinical/Vital');
                            }]
                        },
                        data: {
                            permissionKey: 'Clinical-Vitals-Vitals'
                        }
                    }
                )
                .state('patientprofile.chart.intake.socialrelationship',
                   {
                       title: 'Social Relationship',
                       url: "/SocialRelationship",
                       views: {
                           '@patientprofile': {
                               templateUrl: "/Plugins/Clinical/SocialRelationship/Index",
                               controller: "socialRelationshipController as ctrl"
                           }
                       },
                       resolve: {
                           pinkyPromise: ['$http', function ($http) {
                               return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                           }],
                           scriptPromise: ['$http', function ($http) {
                               return lazyLoader.getScriptPromise($http, "/bundles/Plugins/Axis.Plugins.Clinical/SocialRelationship");
                           }]
                       },
                       data: {
                           permissionKey: 'Clinical-SocialRelationshipHistory-SocialRelationshipHistory'
                       }
                   }
                )
                .state('patientprofile.chart.intake.socialrelationship.socialrelationshiphistory',
                    {
                        title: 'Social Relationship History',
                        url: "/SocialRelationshipHistory/{socialRelationshipID:int}",
                        views: {
                            '@patientprofile': {
                                templateUrl: "/Plugins/Clinical/SocialRelationshipHistory/Index",
                                controller: "socialRelationshipHistoryController as ctrl"
                            }
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, "/bundles/Plugins/Axis.Plugins.Clinical.SocialRelationshipHistory");
                            }]
                        },
                        data: {
                            permissionKey: 'Clinical-SocialRelationshipHistory-SocialRelationshipHistory'
                        }
                    }
                )
                .state('patientprofile.chart.intake.note',
                    {
                        title: 'Notes',
                        url: "/Note",
                        params: { NTypeID: null },
                        views: {
                            '@patientprofile': {
                                templateUrl: "/Plugins/Clinical/Note/Index",
                                controller: "noteController as ctrl"
                            }
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, "/bundles/Plugins/Axis.Plugins.Clinical/Note");
                            }]
                        },
                        data: {
                            permissionKey: 'Clinical-Note-Note'
                        }
                    }
                )
                .state('patientprofile.chart.intake.note.notedetail',
                    {
                        title: 'Note Detail',
                        url: "/Note/Notedetail",
                        views: {
                            '@patientprofile': {
                                templateUrl: "/Plugins/Clinical/Note/NoteDetail",
                                controller: "noteDetailController as ctrl"
                            }
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, "/bundles/Plugins/Axis.Plugins.Clinical/NoteDetail");
                            }]
                        },
                        data: {
                            permissionKey: 'Clinical-Note-Note'
                        }
                    }
                )
                .state('patientprofile.chart.intake.allergy',
                    {
                        title: 'Food & Environment Allergies',
                        url: '/Allergy/{AllergyTypeID:int}',
                        params: { AllergyTypeID: null },
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Plugins/Clinical/Allergy/Index',
                                controller: 'allergyController as ctrl'
                            }
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Clinical/Allergy');
                            }]
                        },
                        data: {
                            permissionKey: 'Clinical-Allergy-Allergy'
                        }
                    }
                )
                .state('patientprofile.chart.intake.drugallergy',
                    {
                        title: 'Drug Allergies',
                        url: '/DrugAllergy/{AllergyTypeID:int}',
                        params: { AllergyTypeID: null },
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Plugins/Clinical/Allergy/Index',
                                controller: 'allergyController as ctrl'
                            }
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Clinical/Allergy');
                            }]
                        },
                        data: {
                            permissionKey: 'Clinical-Allergy-Allergy'
                        }
                    }
                )
                .state('patientprofile.chart.intake.drugintolerance',
                    {
                        title: 'Drug Intolerances',
                        url: '/DrugIntolerance/{AllergyTypeID:int}',
                        params: { AllergyTypeID: null },
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Plugins/Clinical/Allergy/Index',
                                controller: 'allergyController as ctrl'
                            }
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/Plugins/Axis.Plugins.Clinical/Allergy');
                            }]
                        },
                        data: {
                            permissionKey: 'Clinical-Allergy-Allergy'
                        }
                    }
                )
                .state('patientprofile.chart.intake.reviewOfSystems',
                    {
                        title: 'Review Of Systems',
                        url: "/ReviewOfSystems",
                        views: {
                            '@patientprofile': {
                                templateUrl: "/Plugins/Clinical/ReviewOfSystems/Index",
                                controller: "reviewOfSystemsController as ctrl"
                            }
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, "/bundles/Plugins/Axis.Plugins.Clinical/ReviewOfSystems");
                            }]
                        },
                        data: {
                            permissionKey: 'Clinical-ReviewOfSystems-ReviewofSystems'
                        }
                    })
                .state('patientprofile.chart.intake.reviewOfSystems.ros',
                    {
                        url: '/{RoSID:int}',
                        views: {
                            '@patientprofile': {
                                template: '<div ui-view></div>'
                            },
                            'navigation@patientprofile': {
                                template: function ($stateParams) {
                                    return '<ros-navigation />';
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
                            permissionKey: 'Clinical-ReviewOfSystems-ReviewofSystems'
                        }
                    }
                )
                .state('patientprofile.chart.intake.reviewOfSystems.ros.header',
                    {
                        title: 'Ros Header',
                        url: '/RosHeader',
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Plugins/Clinical/ReviewOfSystems/ReviewOfSystemHeader',
                                controller: 'reviewOfSystemsController as ctrl'
                            }
                        },
                        data: {
                            permissionKey: 'Clinical-ReviewOfSystems-ReviewofSystems'
                        }
                    }
                )
                .state('patientprofile.chart.intake.reviewOfSystems.ros.section',
                    {
                        title: 'Ros Section',
                        url: '/{SectionID:int}/{ResponseID:int}',
                        views: {
                            '@patientprofile': {
                                template: '<assessment-section permission-key="Clinical-ReviewOfSystems-ReviewofSystems" data-return-state="patientprofile.chart.intake.reviewOfSystems" />'
                            }
                        },
                        
                        data: {
                            permissionKey: 'Clinical-ReviewOfSystems-ReviewofSystems'
                        }
                    }
                )
                .state('patientprofile.chart.intake.reviewOfSystems.ros.review',
                    {
                        title: 'Review',
                        url: "/Review",
                        views: {
                            '@patientprofile': {
                                templateUrl: "/Plugins/Clinical/ReviewOfSystems/Review",
                                controller: "reviewOfSystemsController as ctrl"
                            }
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, "/bundles/Plugins/Axis.Plugins.Clinical/ReviewOfSystems");
                            }]
                        },
                        data: {
                            permissionKey: 'Clinical-ReviewOfSystems-ReviewofSystems'
                        }
                    }
                )
                 .state('patientprofile.chart.intake.clinicalAssessment',
                    {
                        title: 'Assessment',
                        url: "/clinicalAssessment",
                        views: {
                            '@patientprofile': {
                                templateUrl: "/Plugins/Clinical/clinicalAssessment/Index",
                                controller: "clinicalAssessmentController as ctrl"
                            }
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, "/bundles/Plugins/Axis.Plugins.Clinical/clinicalAssessment");
                            }]
                        },
                        data: {
                            permissionKey: 'Clinical-Assessment-Assessment'
                        }

                    }
                )
                .state('patientprofile.chart.intake.clinicalAssessment.ca',
                    {
                        url: '/{ClinicalAssessmentID:int}',
                        views: {
                            '@patientprofile': {
                                template: '<div ui-view></div>'
                            },
                            'navigation@patientprofile': {
                                template: function ($stateParams) {
                                    return '<clinical-assessment-navigation />';
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
                            permissionKey: 'Clinical-Assessment-Assessment'
                        }
                    }
                )
                .state('patientprofile.chart.intake.clinicalAssessment.ca.header',
                    {
                        title: 'Clinical Assessment Header',
                        url: '/Header',
                        views: {
                            '@patientprofile': {
                                templateUrl: '/Plugins/Clinical/clinicalAssessment/ClinicalAssessmentHeader',
                                controller: 'clinicalAssessmentController as ctrl'
                            }
                        },
                        data: {
                            permissionKey: 'Clinical-Assessment-Assessment'
                        }
                    }
                )
                .state('patientprofile.chart.intake.clinicalAssessment.ca.section',
                    {
                        title: 'Clinical Assessment Section',
                        url: '/{SectionID:int}/{ResponseID:int}',
                        views: {
                            '@patientprofile': {
                                template: '<assessment-section permission-key="Clinical-Assessment-Assessment" data-return-state="patientprofile.chart.intake.note" />'
                            },
                            data: {
                                permissionKey: 'Clinical-Assessment-Assessment'
                            }
                        }
                    }
                )
                .state('patientprofile.chart.intake.chiefcomplaint',
                    {
                        title: 'Chief Complaint',
                        url: "/ChiefComplaint",
                        views: {
                            '@patientprofile': {
                                templateUrl: "/Plugins/Clinical/ChiefComplaint/Index",
                                controller: "chiefComplaintController as ctrl"
                            }
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, "/bundles/Plugins/Axis.Plugins.Clinical/ChiefComplaint");
                            }]
                        },
                        data: {
                            permissionKey: 'Clinical-ChiefComplaint-ChiefComplaint'
                        }

                    }
                ).state('patientprofile.chart.intake.medicalhistory',
                    {
                        title: 'Medical History',
                        url: "/MedicalHistory",
                        views: {
                            '@patientprofile': {
                                templateUrl: "/Plugins/Clinical/MedicalHistory/Index",
                                controller: "medicalHistoryController as ctrl"
                            }
                        },
                        resolve: {
                            pinkyPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                            }],
                            scriptPromise: ['$http', function ($http) {
                                return lazyLoader.getScriptPromise($http, "/bundles/Plugins/Axis.Plugins.Clinical/MedicalHistory");
                            }]
                        },
                        data: {
                            permissionKey: 'Clinical-SocialRelationshipHistory-SocialRelationshipHistory'
                        }
                    }
                )
                .state('patientprofile.chart.intake.medicalhistory.medicalhistorydetails',
                   {
                       title: 'Medical History Details',
                       url: "/MedicalHistoryDetails/{MedicalHistoryID:int}",
                       params: { MedicalHistoryID: null },
                       views: {
                           '@patientprofile': {
                               templateUrl: "/Plugins/Clinical/MedicalHistory/MedicalHistoryDetails",
                               controller: "medicalHistoryDetailsController as ctrl"
                           }
                       },
                       resolve: {
                           pinkyPromise: ['$http', function ($http) {
                               return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                           }],
                           scriptPromise: ['$http', function ($http) {
                               return lazyLoader.getScriptPromise($http, "/bundles/Plugins/Axis.Plugins.Clinical/MedicalHistoryDetails");
                           }]
                       },
                       data: {
                           permissionKey: 'Clinical-SocialRelationshipHistory-SocialRelationshipHistory'
                       }
                   }
                )
            .state('patientprofile.chart.intake.presentillness',
                   {
                       title: 'Present Illness',
                       url: "/PresentIllness",
                       views: {
                           '@patientprofile': {
                               templateUrl: "/Plugins/Clinical/PresentIllness/Index",
                               controller: "presentIllnessController as ctrl"
                           }
                       },
                       resolve: {
                           pinkyPromise: ['$http', function ($http) {
                               return lazyLoader.getScriptPromise($http, '/bundles/angular-forms');
                           }],
                           scriptPromise: ['$http', function ($http) {
                               return lazyLoader.getScriptPromise($http, "/bundles/Plugins/Axis.Plugins.Clinical/PresentIllness");
                           }]
                       },
                       data: {
                           permissionKey: 'Clinical-PresentIllness-PresentIllness'
                       }
                   }
                )
            ;
        }
    ])
    .run([
        '$rootScope', '$http', '$document', '$filter', function ($rootScope, $http, $document, $filter) {
            var setValidationStatus = function (formName, status) {
                formName.takenDetailsForm.takenOnDateCalander.$valid = status;
                formName.takenDetailsForm.takenOnDateCalander.$pristine = status;
                formName.takenDetailsForm.takenOnDateCalander.$invalid = !status;
                var noError = angular.equals({}, formName.$error);
                if (formName.$error.length != 0 && status == false) {
                    formName.$valid = status;
                    formName.$invalid = !status;
                }

                if (noError && status == true) {
                    formName.$valid = true
                    formName.$invalid = false;
                }
                if (!noError) {

                    formName.$valid = false;
                    formName.$invalid = true;
                }



            };

            $rootScope.validateFutureDate = function (currentCtrl) {
                var formName = currentCtrl.ctrl[Object.keys(currentCtrl.ctrl)[0]];
                if (formName.takenDetailsForm.takenOnDateCalander != undefined) {
                    var datePart = $filter('toMMDDYYYYDate')(formName.takenDetailsForm.takenOnDateCalander.$modelValue, 'MM/DD/YYYY');
                    var dateTime = new Date($filter('toMMDDYYYYDate')(datePart + ' ' + formName.takenDetailsForm.takenTime.$modelValue, 'MM/DD/YYYY HH:mm'));
                    if (dateTime.valueOf() <= (new Date()).valueOf()) {
                        setValidationStatus(formName, true);
                    }
                    else {
                        setValidationStatus(formName, false);
                    }
                }
            };
            $rootScope.$on("$stateChangeSuccess", function (event, toState, toParams, fromState, fromParams) {
                if ((toState != null) && toState.name && (toState.name.indexOf('patientprofile.chart.intake.reviewOfSystems') == 0)) {
                    $rootScope.$evalAsync(function ($rootScope) {
                        var contactId = toParams.ContactID ? toParams.ContactID : 0;
                        if (contactId > 0) {
                            $http.get('/data/plugins/clinical/ReviewOfSystems/NavigationValidationStates', { params: { contactId: toParams.ContactID } })
                            .then(function (response) {
                                var data = response.data.DataItems;
                                for (var state in data) {
                                    $rootScope.$broadcast(data[state].Key, { validationState: data[state].Value });
                                }
                            }).finally(function () {

                            });
                        }
                    });
                }
            });
        }
    ]);
