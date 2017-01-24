angular.module('xenatixApp')
    .controller('screeningController', [
        '$http', '$scope', '$state', '$modal', '$filter', 'screeningService', 'alertService', 'lookupService', '$stateParams', '$rootScope', 'formService', 'assessmentService', '$q', 'registrationService', 'navigationService',
        function ($http, $scope, $state, $modal, $filter, screeningService, alertService, lookupService, $stateParams, $rootScope, formService, assessmentService, $q, registrationService, navigationService) {
            $scope.inputType = {
                Button: 1,
                Checkbox: 2,
                Radio: 3,
                Textbox: 4,
                Select: 5,
                MultiSelect: 6,
                None: 7,
                DatePicker: 8,
                TextArea: 9
            };

            $scope.loggedInUser  = {};

            var screeningsTable = $('#screeningsTable');

            $scope.selectInitialCoordinator = function (item) {
                $scope.screening.InitialServiceCoordinatorID = item.ID;
            };

            $scope.selectPrimaryCoordinator = function (item) {
                $scope.screening.PrimaryServiceCoordinatorID = item.ID;
            };

            $scope.resetMainForm = function () {
                if ($scope != null && $scope.ctrl != null && $scope.ctrl.screeningsForm != null)
                    $rootScope.formReset($scope.ctrl.screeningsForm, $scope.ctrl.screeningsForm.name);
            };

            var checkFormStatus = function () {
                $scope.$watch('ctrl.screeningsForm.$valid', function (newValue) {
                    $rootScope.$broadcast('patientprofile.screening', { validationState: ($scope.ContactID !== 0 || newValue) ? 'valid' : 'invalid' });
                });
            };

            $scope.initializeBootstrapTable = function () {
                $scope.tableoptions = {
                    pagination: true,
                    pageSize: 10,
                    pageList: [10, 25, 50, 100],
                    search: false,
                    showColumns: true,
                    data: [],
                    undefinedText: '',
                    onClickRow: function (e, row) {
                    },
                    columns: [
                        {
                            field: 'AssessmentName',
                            title: 'Screening Name'
                        },
                        {
                            field: 'ScreeningType',
                            title: 'Screening Type'
                        },
                        {
                            field: 'ScreeningDate',
                            title: 'Screening Date',
                            formatter: function (value, row, index) {
                                return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY', 'useLocal');
                            }
                        },
                        {
                            field: 'ScreeningResult',
                            title: 'Result'
                        },
                        {
                            field: 'ScreeningScore',
                            title: 'Score'
                        },
                        {
                            field: 'SubmittedBy',
                            title: 'Submitted By'
                        },
                        {
                            field: 'ScreeningID',
                            title: '',
                            formatter: function (value, row, index) {
                                return (
                                    '<span class="text-nowrap">' +

                                    '<a data-default-action ng-click="editAssessment(' + value + ',' + row.AssessmentID + ',' + row.ResponseID + ', $event)" ' +
                                    'alt="Edit Screening" security permission-key="ECI-Screening-Screening" permission="update" space-key-press>' +
                                    '<i  title="Edit" class="fa fa-pencil fa-fw border-left margin-left padding-left-small padding-right-small" /></a>') +

                                    '<a href="javascript:void(0)" data-default-no-action ng-click="print(' + value + ', $event)" id="printScreening" name="printScreening" title="Print" security permission-key="ECI-Screening-Screening" permission="read" ' +
                                    'space-key-press><i class="fa fa-print fa-fw"></i></a>' +

                                    '<a href="javascript:void(0)" data-default-no-action ng-click="remove(' + value + ', $event)" id="deleteScreening" name="deleteScreening" title="Delete" security permission-key="ECI-Screening-Screening" permission="delete" ' +
                                    ' space-key-press><i class="fa fa-trash fa-fw"></i></a>' +

                                    '</span>';
                            }
                        }
                    ]
                };
            };

            $scope.screeningTypeSelected = function () {
                // this will clear the screening name when the screening type changes
                $scope.screening.ScreeningNameID = null;
                $scope.screening.ScreeningName = null;
            }

            $scope.editScreening = function (screeningID, event) {
            }

            $scope.editAssessment = function (screeningID, assessmentID, responseID, event) {
                $scope.screening.ScreeningID = screeningID;
                event.stopPropagation();
                $scope.navigateToScreeningAssessment(screeningID, assessmentID, responseID);
            }

            $scope.print = function (screeningID, event) {
                event.stopPropagation();
                $state.go('.screening.report', $.extend({}, $stateParams, { ScreeningID: screeningID }));
            }

            $scope.getList = function () {
                $scope.isLoading = true;


                return screeningService.getList($scope.ContactID).then(function (data) {
                    if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                        if (data.ResultMessage == "OFFLINE") {
                            angular.forEach(data.DataItems, function (item, index) {
                                if (parseInt(item.ScreeningID) < 0) {
                                    item.SubmittedBy = $scope.loggedInUser.UserFullName;

                                    var screeningType = lookupService.getSelectedTextById(item.ScreeningTypeID, $scope.screeningTypeLookups);
                                    if (screeningType != undefined && screeningType.length > 0)
                                        item.ScreeningType = screeningType[0].Name;


                                    var assessmentType = lookupService.getSelectedTextById(item.AssessmentID, $scope.screeningNameLookups);
                                    if (assessmentType != undefined && assessmentType.length > 0)
                                        item.AssessmentName = assessmentType[0].Name;
                                }

                            });
                        }

                        $scope.screeningList = data.DataItems;
                        screeningsTable.bootstrapTable('load', $scope.screeningList);
                    }
                    else {
                       
                        $scope.screeningList = [];
                        screeningsTable.bootstrapTable('removeAll');
                    }
                    $scope.resetMainForm();
                    checkFormStatus();
                },
                    function (errorStatus) {
                        alertService.error('Unable to get screening: ' + errorStatus);
                    }).finally(function () {
                        $scope.isLoading = false;
                    });
            };

            $scope.get = function (screeningID) {
                $scope.isLoading = true;

                return screeningService.get($scope.ContactID, screeningID).then(function (data) {
                    if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                        $scope.screening = data.DataItems[0];
                        $scope.screening.InitialContactDate = $filter('toMMDDYYYYDate')($scope.screening.InitialContactDate, 'MM/DD/YYYY');
                        $scope.screening.ScreeningDate = $filter('toMMDDYYYYDate')($scope.screening.ScreeningDate, 'MM/DD/YYYY');
                    } else {
                        alertService.error('Unable to get screening!');
                    };
                    $scope.resetMainForm();
                    checkFormStatus();
                },
                    function (errorStatus) {
                        alertService.error('Unable to get screening: ' + errorStatus);
                    }).finally(function () {
                        $scope.isLoading = false;
                    });
            };

            $scope.ensureResponse = function () {
                var deferred = $q.defer();
                if (($scope.newResponse !== undefined) && (($scope.newResponse.ResponseID !== 0) && ($scope.newResponse.AssessmentID === $scope.screening.AssessmentID))) {
                    $scope.promiseNoOp().then(function () {
                        deferred.resolve($scope.newResponse.ResponseID);
                    });
                } else {
                    var assessmentResponse = {
                        ResponseID: 0,
                        ContactID: $scope.ContactID,
                        AssessmentID: $scope.screening.AssessmentID
                    };
                    assessmentService.addAssessmentResponse(assessmentResponse).then(function (response) {
                        if (response.data.ResultCode === 0) {
                            $scope.newResponse = { ResponseID: response.data.ID, AssessmentID: $scope.screening.AssessmentID };
                            deferred.resolve(response.data.ID);
                        } else {
                            deferred.reject();
                        }
                    }, function () {
                        deferred.reject();
                    });
                }
                return deferred.promise;
            };

            $scope.saveScreening = function (isAdd) {
                if ($scope.loggedInUser ) {
                    $scope.screening.SubmittedBy = $scope.loggedInUser.UserFullName;
                    $scope.screening.SubmittedByID = $scope.loggedInUser.UserID;
                }
                $scope.screening.InitialContactDate = $filter('formatDate')($scope.screening.InitialContactDate);
                $scope.screening.ScreeningDate = $filter('formatDate')($scope.screening.ScreeningDate);

                if (isAdd) {
                    var deferred = $q.defer();
                    $scope.ensureResponse().then(function (responseID) {
                        $scope.screening.ResponseID = responseID;
                        screeningService.add($scope.screening).then(function (response) {
                            $scope.newResponse = {};
                            deferred.resolve(response);
                        },
                            function () {
                                deferred.reject();
                            });
                    }, function () {
                        deferred.reject();
                    });
                    return deferred.promise;
                } else {
                    return screeningService.update($scope.screening);
                }
            };

            $scope.initHeader = function () {
                $scope.ContactID = $stateParams.ContactID;
                $scope.get($stateParams.ScreeningID);
            };

            $scope.initReport = function () {
                $scope.isReportReady = false;
                $scope.ContactID = $stateParams.ContactID;
                registrationService.get($scope.ContactID).then(function (data) {
                    $scope.reportModel = {
                        childName: data.DataItems[0].FirstName + ' ' + data.DataItems[0].LastName,
                        childAge: $filter('ageToShow')(data.DataItems[0].DOB)
                    };
                    navigationService.get().then(function (data) {
                        $scope.reportModel.evaluatorName = data.DataItems[0].UserFullName;
                        //$scope.reportModel.evaluatorName = ''; //TODO: Need to resolve into submitter's name
                        $scope.get($stateParams.ScreeningID).then(function () {
                            $scope.reportModel.screeningDate = $scope.screening.ScreeningDate;
                            $scope.reportName = $scope.screening.AssessmentID === 1 ? 'RiskFactors' : 'ECINeeds'; //TODO: Refactor!!!
                            assessmentService.getAssessmentSections($scope.screening.AssessmentID).then(function (data) {
                                var promises = [];
                                angular.forEach(data.DataItems, function (section) {
                                    var deferred = $q.defer();
                                    promises.push(deferred.promise);
                                    assessmentService.getAssessmentQuestions(section.AssessmentSectionID).then(function (qData) {
                                        assessmentService.getAssessmentResponseDetails($scope.screening.ResponseID, qData.DataItems[0].AssessmentSectionID).then(function (rData) {
                                            rData = rData.data;
                                            angular.forEach(rData.DataItems, function (responseDetail) {
                                                var inputTypeId = $filter('filter')(qData.DataItems, function (question) { return parseInt(question.QuestionID) === parseInt(responseDetail.QuestionID); })[0].InputTypeID;
                                                if (inputTypeId === $scope.inputType.Textbox || inputTypeId === $scope.inputType.TextArea || inputTypeId === $scope.inputType.DatePicker) {
                                                    if (!(responseDetail.QuestionID in $scope.reportModel))
                                                        $scope.reportModel[responseDetail.QuestionID] = {};
                                                    $scope.reportModel[responseDetail.QuestionID][responseDetail.OptionsID] = responseDetail.ResponseText;
                                                } else if (inputTypeId === $scope.inputType.Radio || inputTypeId === $scope.inputType.Select) {
                                                    $scope.reportModel[responseDetail.QuestionID] = responseDetail.OptionsID;
                                                } else if (inputTypeId === $scope.inputType.Checkbox || inputTypeId === $scope.inputType.MultiSelect) {
                                                    if (!(responseDetail.QuestionID in $scope.reportModel))
                                                        $scope.reportModel[responseDetail.QuestionID] = {};
                                                    $scope.reportModel[responseDetail.QuestionID][responseDetail.OptionsID] = true;
                                                }
                                            });
                                            deferred.resolve();
                                        });
                                    });
                                });
                                $q.all(promises).finally(function () {
                                    $scope.isReportReady = true;
                                });
                            });
                        });
                    });
                });
            };

            $scope.saveHeader = function (isNext, mandatory, hasErrors, keepForm) {
                $scope.save(isNext, mandatory, hasErrors, true, $scope.headerNext);
            };

            $scope.headerNext = function () {
                var nextState = $("li[data-state-name='patientprofile.chart.screenings.screening.header']").next();
                $state.go(nextState.attr('data-state-name'), $.extend({}, $stateParams, { SectionID: Math.abs(nextState.attr('data-state-key')), ResponseID: $scope.screening.ResponseID }));
            };

            $scope.save = function (isNext, mandatory, hasErrors, keepForm, next) {
                if (next === undefined) {
                    next = function () { $scope.next(); }
                }
                if (!mandatory && isNext && hasErrors)
                    next();

                if (!hasErrors) {
                    if (formService.isDirty($scope.ctrl.screeningsForm.name)) {
                        var lstTxtById = lookupService.getSelectedTextById($scope.screening.ScreeningTypeID, $scope.screeningTypeLookups);
                        if (lstTxtById != undefined && lstTxtById.length > 0)
                            $scope.screening.ScreeningType = lstTxtById[0].Name;

                        var lstTxt = lookupService.getSelectedTextById($scope.screening.AssessmentID, $scope.screeningNameLookups);
                        if (lstTxt != undefined && lstTxt.length > 0)
                            $scope.screening.AssessmentName = lstTxt[0].Name;
                        var isAdd = !($scope.screening.ScreeningID != undefined && $scope.screening.ScreeningID !== 0);
                        $scope.saveScreening(isAdd).then(function (response) {
                            if (!((response !== undefined) && ('data' in response) && ('ResultCode' in response.data) && (response.data.ResultCode !== 0))) {
                                if (('data' in response)) {
                                    if (response.data.ResultCode == 0) {
                                        if ($scope.screening != undefined && $scope.screening.ScreeningID != undefined) {
                                            var successMessage = 'Screening has been ' + (($scope.screening.ScreeningID === null || $scope.screening.ScreeningID === undefined ||
                                                $scope.screening.ScreeningID === 0) ? 'added' : 'updated') + ' successfully.';
                                        }
                                        $scope.screening.ScreeningID = (($scope.screening !== undefined) && ($scope.screening.ScreeningID !== undefined) && ($scope.screening.ScreeningID != 0))
                                            ? $scope.screening.ScreeningID : response.data.ID;
                                        alertService.success(successMessage);
                                        $scope.ctrl.screeningsForm.$setPristine();

                                        screeningService.getList($scope.ContactID).then(function () {
                                            if (!keepForm)
                                                $scope.postSave(isAdd);
                                            if (isNext)
                                                next();
                                        });
                                    } else {
                                        alertService.error('Unable to save screening');
                                    }
                                }
                            }
                        });
                    } else if (isNext)
                        next();
                }
            };

            $scope.postSave = function (isAdd) {
                $scope.getList().then(function () {
                    if (isAdd) // we only want to clear the screen and reset focus is the user was adding a new screening
                    {
                        $scope.newScreening();
                    }
                });
            };

            $scope.newScreening = function () {                 
                $scope.resetMainForm();
                $scope.$parent['autoFocusInitialContactDate'] = true;
                $scope.screening = {
                    ContactID: $scope.ContactID,
                    ProgramID: $scope.ProgramID, // ECI screenings will always fall under the ECI program
                    ScreeningID: 0,
                    ResponseID: 0,
                    InitialContactDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY'),
                    ScreeningDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY'),
                    InitialServiceCoordinatorID: null,
                    PrimaryServiceCoordinatorID: null,
                    ScreeningTypeID: null,
                    AssessmentID: null,
                    ScreeningScore: null,
                    ScreeningResultsID: null
                };
            };

            $scope.next = function () {
                if ($scope.screening &&
                    $scope.screening.ScreeningID &&
                    $scope.screening.ScreeningID !== 0) {

                    $scope.navigateToScreeningAssessment($scope.screening.ScreeningID, $scope.screening.AssessmentID, $scope.screening.ResponseID);
                } else {
                    alertService.error('Please select a screening with a screening name provided, before proceeding to the next screen');
                }
            };

            $scope.navigateToScreeningAssessment = function (screeningID, assessmentID, responseID) {
                // Create params object
                var params = {
                    ContactID: $scope.ContactID,
                    ScreeningID: screeningID,
                    AssessmentID: assessmentID,
                    ResponseID: responseID
                };

                // Use the assessment service to navigate to correct section
                assessmentService.navigateToSection('patientprofile.chart.screenings.screening.section', params);
            }

            $scope.remove = function (screeningID, event) {
                event.stopPropagation();
                bootbox.confirm('Are you sure you want to deactivate?', function (confirmed) {
                    if (confirmed) {
                        var tempScreeningList = $scope.screeningList;
                        $scope.screeningList = $filter('filter')($scope.screeningList, { ScreeningID: '!' + screeningID });
                        screeningService.remove($scope.ContactID, screeningID).then(function (response) {
                            if (response.ResultCode === 0) {
                                alertService.success('Screening has been deleted successfully.');
                                $scope.newScreening();
                                $scope.getList();

                            } else {
                                alertService.error('Unable to delete screening.');
                                $scope.screeningList = tempScreeningList;
                            }
                        });
                    }
                });
            };

            $scope.init = function () {               
                $scope.ContactID = $stateParams.ContactID;
                $scope.ProgramID = 1;
                $scope.$parent['autoFocus'] = true; //for Focus
                $scope.InitialContactDateOpened = false;
                $scope.ScreeningDateOpened = false;

                $scope.dateOptions = { // used by datepickers
                    formatYear: 'yy',
                    startingDay: 1,
                    showWeeks: false
                };

                $scope.newScreening();
                $scope.newResponse = {};
                $scope.screeningList = [];
                $scope.initializeBootstrapTable();


                $scope.initialServiceCoordinatorLookups = screeningService.getLookups('ServiceCoordinator');
                $scope.primaryServiceCoordinatorLookups = screeningService.getLookups('ServiceCoordinator');
                $scope.screeningTypeLookups = screeningService.getLookups("ScreeningType");
                $scope.screeningNameLookups = screeningService.getLookups('ScreeningName');
                $scope.screeningResultLookups = screeningService.getLookups('ScreeningResult');

                navigationService.get().then(function (data) {
                    $scope.loggedInUser  = data.DataItems[0];
                    $scope.getList();
                });

            };

            $scope.init();
        }
    ]);