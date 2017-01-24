angular.module('xenatixApp')
    .controller('reviewOfSystemsController', [
        '$scope', '$state', '$q', '$stateParams', '$modal', 'reviewOfSystemsService', 'alertService', 'lookupService', '$filter', '$rootScope', 'formService', '$timeout', 'assessmentService', 'navigationService',
        function ($scope, $state, $q, $stateParams, $modal, reviewOfSystemsService, alertService, lookupService, $filter, $rootScope, formService, $timeout, assessmentService, navigationService) {
            $scope.isLoading = true;
            var reviewOfSystemsTable = $("#reviewOfSystemsTable");
            $scope.AssessmentID = 4;
            $scope.allowReview = false;
            var initDirty = false;

            $scope.setDefaultDatePickerSettings = function () {
                $scope.opened = false;
                $scope.dateOptions = {
                    formatYear: 'yy',
                    startingDay: 1,
                    showWeeks: false
                };
                $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'MM/dd/yyyy'];
                $scope.format = $scope.formats[3];
            };

            $scope.newReviewOfSystems = function () {
                $scope.reviewOfSystem = {
                    RoSID: 0
                };

                initDirty = true;

                $scope.takenOnDetail = {
                    TakenOnDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
                    TakenOnTime: $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal')
                };

                navigationService.get().then(function (data) {
                    $scope.takenOnDetail.TakenBy = data.DataItems[0].UserID;
                    resetForm();
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
                        $scope.selectRow(e, row);
                    },
                    columns: [
                        {
                            field: 'ReviewdByName',
                            title: 'Taken By',
                        },
                        {
                            field: 'DateEntered',
                            title: 'Taken Date',
                            formatter: function (value, row, index) {
                                if (value) {
                                    return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY hh:mm A', 'useLocal');
                                } else
                                    return '';
                            }
                        },
                        {
                            field: 'RoSID',
                            title: '',
                            formatter: function (value, row, index) {
                                return '<span class="text-nowrap">' +
                                    '<a href="javascript:void(0)" data-default-action id="editReviewOfSystems" name="editReviewOfSystems" ' + 'ng-click="editAssessment(' + value + ', ' + row.AssessmentID + ', ' + row.ResponseID + ', $event)" title="Edit" ' + 'security permission-key="Clinical-ReviewOfSystems-ReviewofSystems" permission="update" space-key-press><i class="fa fa-pencil fa-fw" /></a>' +
                                    '<a href="javascript:void(0)" data-default-no-action ng-click="remove(' + value + ',$event)" id="deactivateReviewOfSystems" name="deactivateReviewOfSystems" title="Deactivate" security permission-key="Clinical-ReviewOfSystems-ReviewofSystems" permission="delete" space-key-press><i class="fa fa-trash fa-fw"></i></a>' +
                                    '</span>';
                                ;

                            }
                        }
                    ]
                };
            };

            var resetForm = function () {
                $rootScope.formReset($scope.ctrl.reviewOfSystemsForm);
            };

            $scope.$watch("[takenOnDetail.TakenBy, takenOnDetail.TakenOnDate, takenOnDetail.TakenOnTime]", function (newValues, oldValues) {
                if (oldValues[0] != null && oldValues[0] != newValues[0]) {
                    initDirty = false;
                }
                else if (oldValues[1] != null && oldValues[1] != newValues[1]) {
                    initDirty = false;
                }
                else if (oldValues[2] != null && oldValues[2] != newValues[2]) {
                    initDirty = false;
                }
            });

            $scope.getReviewOfSystems = function () {
                $scope.isLoading = true;

                return reviewOfSystemsService.getReviewOfSystemsByContact($scope.ContactID).then(function (data) {
                    if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                        $scope.reviewOfSystems = data.DataItems;

                        reviewOfSystemsTable.bootstrapTable('load', $scope.reviewOfSystems);
                    } else {
                        $scope.reviewOfSystems = [];
                        reviewOfSystemsTable.bootstrapTable('removeAll');
                    }
                    resetForm();
                },
                    function (errorStatus) {
                        alertService.error('Unable to get reviewOfSystems: ' + errorStatus);
                    }).finally(function () {
                        $scope.isLoading = false;
                    });
            };

            $scope.get = function (rosID) {
                $scope.isLoading = true;

                return reviewOfSystemsService.getReviewOfSystem($scope.ContactID, rosID).then(function (data) {
                    if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                        $scope.reviewOfSystem = data.DataItems[0];
                        $scope.takenOnDetail.TakenBy = $scope.reviewOfSystem.ReviewdBy;
                        $scope.takenOnDetail.TakenOnDate = $filter('toMMDDYYYYDate')($scope.reviewOfSystem.DateEntered, 'MM/DD/YYYY', 'useLocal');
                        $scope.takenOnDetail.TakenOnTime = $filter('toMMDDYYYYDate')($scope.reviewOfSystem.DateEntered, 'hh:mm A', 'useLocal');
                        resetForm();
                    } else {
                        alertService.error('Unable to get reviewOfSystems!');
                    };
                },
                    function (errorStatus) {
                        alertService.error('Unable to get reviewOfSystems: ' + errorStatus);
                    }).finally(function () {
                        $scope.isLoading = false;
                    });
            };

            $scope.editReviewOfSystems = function (rosID, event) {
                $scope.reviewOfSystem = { RoSID: rosID };
                initDirty = false;
            }

            $scope.selectRow = function (e) {
                if (initDirty == false && formService.isDirty()) {
                    bootbox.dialog({
                        message: "You have unsaved data. What would you like to do?",
                        buttons: {
                            save: {
                                label: "SAVE",
                                className: "btn-success",
                                callback: function () {
                                    $scope.save(false, true, false, true, false).then(function () {
                                        $scope.setFields(e);
                                    });
                                }
                            },
                            discard: {
                                label: "DISCARD",
                                className: "btn-danger",
                                callback: function () {
                                    $scope.setFields(e);
                                }
                            }
                        }
                    });
                } else {
                    $scope.setFields(e);
                }
            }

            $scope.setFields = function (e) {
                $scope.reviewOfSystem = angular.copy(e);
                $scope.takenOnDetail.TakenBy = $scope.reviewOfSystem.ReviewdBy;
                $scope.takenOnDetail.TakenOnDate = $filter('toMMDDYYYYDate')($scope.reviewOfSystem.DateEntered, 'MM/DD/YYYY', 'useLocal');
                $scope.takenOnDetail.TakenOnTime = $filter('toMMDDYYYYDate')($scope.reviewOfSystem.DateEntered, 'hh:mm A', 'useLocal');

                resetForm();
            }

            $scope.saveReview = function (isNext, mandatory, hasErrors, keepForm, next) {
                if ((!mandatory && !hasErrors && !isNext) || (mandatory && !hasErrors && !isNext) || (!mandatory && !hasErrors && isNext) || (mandatory && !hasErrors && isNext)) {
                    //var isDirty = formService.isDirty();
                    //if (isDirty) {
                    var copyOfReviewOfSystem = angular.copy($scope.reviewOfSystem);
                    $scope.get($stateParams.RoSID).then(function () {
                        $scope.reviewOfSystem.IsReviewChanged = copyOfReviewOfSystem.IsReviewChanged;
                        return reviewOfSystemsService.updateReviewOfSystem($scope.reviewOfSystem).then(function (response) {
                            $scope.postSaveUpdate(response);
                            if (isNext) {
                                $state.go('patientprofile.chart.intake.medicalhistory');
                            }
                        },
                         function (errorStatus) {
                             alertService.error('OOPS! Something went wrong');
                         },
                         function (notification) {
                             alertService.warning(notification);
                         });
                    });
                    //}
                    //else
                    //{
                    if (isNext) {
                        $state.go('patientprofile.chart.intake.medicalhistory');
                    }
                    //}
                }
            }

            $scope.saveReviewOfSystems = function (isUpdate) {
                if (!isUpdate) {
                    var deferred = $q.defer();
                    $scope.ensureResponse().then(function (responseID) {
                        $scope.reviewOfSystem.AssessmentID = $scope.AssessmentID;
                        $scope.reviewOfSystem.ResponseID = $scope.newResponse.ResponseID;

                        reviewOfSystemsService.addReviewOfSystem($scope.reviewOfSystem).then(function (response) {
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
                    return reviewOfSystemsService.updateReviewOfSystem($scope.reviewOfSystem);
                }
            };

            $scope.ensureResponse = function () {
                var deferred = $q.defer();
                if (($scope.newResponse !== undefined) && (($scope.newResponse.ResponseID !== 0) && ($scope.newResponse.AssessmentID === $scope.AssessmentID))) {
                    $scope.promiseNoOp().then(function () {
                        deferred.resolve($scope.newResponse.ResponseID);
                    });
                } else {
                    var assessmentResponse = {
                        ResponseID: 0,
                        ContactID: $scope.ContactID,
                        AssessmentID: $scope.AssessmentID
                    };
                    assessmentService.addAssessmentResponse(assessmentResponse).then(function (response) {
                        if (response.data.ResultCode === 0) {
                            $scope.newResponse = { ResponseID: response.data.ID, AssessmentID: $scope.AssessmentID };

                            // Step 1: Get last active review of systems
                            reviewOfSystemsService.getLastActiveReviewOfSystems($scope.ContactID).then(function (lastRos) {
                                var lastActiveRos = lastRos.DataItems[0]
                                if (lastActiveRos != undefined) {
                                    var assessmentID = lastActiveRos.AssessmentID;
                                    var responseID = lastActiveRos.ResponseID;

                                    // Step 2: Get sections active review of systems
                                    assessmentService.getAssessmentSections(assessmentID).then(function (data) {
                                        angular.forEach(data.DataItems, function (section) {
                                            // Step 3: Get section wise assessment response details
                                            assessmentService.getAssessmentResponseDetails(responseID, section.AssessmentSectionID).then(function (responseDetails) {
                                                // Step 4: Colon assesssment response details
                                                if (responseDetails.data.DataItems != undefined && responseDetails.data.DataItems.length > 0) {
                                                    assessmentService.saveAssessmentResponseDetails(response.data.ID, section.AssessmentSectionID, responseDetails.data.DataItems);
                                                }
                                            });
                                        });
                                    });
                                }
                            });

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

            $scope.saveHeader = function (isNext, mandatory, hasErrors, keepForm) {
                $scope.save(isNext, mandatory, hasErrors, true, $scope.headerNext);
            };

            $scope.headerNext = function () {
                var nextState = $("li[data-state-name='patientprofile.chart.intake.reviewOfSystems.ros.header']").next();
                $state.go(nextState.attr('data-state-name'), $.extend({}, $stateParams, { SectionID: Math.abs(nextState.attr('data-state-key')), ResponseID: $scope.reviewOfSystem.ResponseID }));
            };

            $scope.save = function (isNext, mandatory, hasErrors, keepForm, next) {
                if (next === undefined) {
                    next = function () { $scope.next(); }
                }
                if (!mandatory && isNext && !hasErrors)
                    next();

                if ((!mandatory && !hasErrors && !isNext) || (mandatory && !hasErrors && !isNext) || (!mandatory && !hasErrors && isNext) || (mandatory && !hasErrors && isNext)) {
                    //var isDirty = formService.isDirty();
                    var deferred = $q.defer();
                    //if (isDirty) {
                    $scope.reviewOfSystem.ContactID = $scope.ContactID;
                    $scope.reviewOfSystem.ReviewdBy = $scope.takenOnDetail.TakenBy;

                    var takenBy = lookupService.getSelectedTextById($scope.reviewOfSystem.ReviewdBy, $scope.reviewByLookups);
                    if (takenBy != undefined && takenBy.length > 0) {
                        $scope.reviewOfSystem.ReviewdByName = takenBy[0].Name;
                    }

                    var datePart = $filter('formatDate')($scope.takenOnDetail.TakenOnDate);
                    var dateTime = $filter('toMMDDYYYYDate')(datePart + ' ' + $scope.takenOnDetail.TakenOnTime, 'MM/DD/YYYY HH:mm')

                    $scope.reviewOfSystem.DateEntered = new Date(dateTime);

                    var isUpdate = $scope.reviewOfSystem.RoSID != undefined && $scope.reviewOfSystem.RoSID !== 0;
                    $scope.saveReviewOfSystems(isUpdate).then(function (response) {
                        var action = isUpdate ? 'updated' : 'added';
                        $scope.postSaveCommon(response, action, isNext);
                        deferred.resolve(response);
                    },
                     function (errorStatus) {
                         alertService.error('OOPS! Something went wrong');
                         deferred.reject();
                     },
                     function (notification) {
                         alertService.warning(notification);
                     });
                    return deferred.promise;
                    //}
                }
            };

            $scope.postSaveAdd = function (response) {
                return $scope.postSaveCommon(response, 'added');
            };

            $scope.postSaveUpdate = function (response) {
                return $scope.postSaveCommon(response, 'updated');
            };

            $scope.postSaveCommon = function (response, action, isNext) {
                var data = response;
                if (data.ResultCode !== 0) {
                    alertService.error(data.ResultMessage);
                    return $scope.promiseNoOp();
                } else {
                    alertService.success('Review Of Systems has been ' + action + ' successfully.');
                    $scope.reviewOfSystem.RoSID = (($scope.reviewOfSystem !== undefined) && ($scope.reviewOfSystem.RoSID !== undefined) && ($scope.reviewOfSystem.RoSID != 0))
                                    ? $scope.reviewOfSystem.RoSID : response.ID;
                    if (isNext) {
                        $scope.next();
                    }
                    else {
                        $scope.init();
                        return true;
                    }
                }
            };

            $scope.editAssessment = function (rosID, assessmentID, responseID, event) {
                event.stopPropagation();
                navigateToRosAssessment(rosID, assessmentID, responseID);
            }

            var navigateToRosAssessment = function (rosID, assessmentID, responseID) {
                $scope.get(rosID).then(function () {
                    reviewOfSystemsService.setLastAssessmentOn($scope.reviewOfSystem.LastAssessmentOn);
                });

                assessmentService.getAssessmentSections(assessmentID).then(function (data) {
                    var params = {
                        ContactID: $scope.ContactID,
                        RoSID: rosID,
                        SectionID: data.DataItems[0].AssessmentSectionID,
                        ResponseID: responseID
                    }

                    $state.go('patientprofile.chart.intake.reviewOfSystems.ros.section', params);
                });
            };

            $scope.next = function () {
                if ($scope.reviewOfSystem &&
                    $scope.reviewOfSystem.RoSID &&
                    $scope.reviewOfSystem.RoSID !== 0) {
                    navigateToRosAssessment($scope.reviewOfSystem.RoSID, $scope.reviewOfSystem.AssessmentID, $scope.reviewOfSystem.ResponseID);
                }
            };

            $scope.remove = function (rosID, event) {
                event.stopPropagation();
                bootbox.confirm('Are you sure you want to deactivate?', function (confirmed) {
                    if (confirmed) {
                        var tempReviewOfSystems = $scope.reviewOfSystems;
                        $scope.reviewOfSystems = $filter('filter')($scope.reviewOfSystems, { RoSID: '!' + rosID });
                        reviewOfSystemsService.deleteReviewOfSystem($scope.ContactID, rosID).then(function (response) {
                            if (response.ResultCode === 0) {
                                alertService.success('Review Of Systems has been deleted successfully.');
                                $scope.init();
                            } else {
                                alertService.error('Unable to delete screening.');
                                $scope.reviewOfSystems = tempReviewOfSystems;
                            }
                        });
                    }
                });
            };

            $scope.initHeader = function () {
                $scope.ContactID = $stateParams.ContactID;
                $scope.get($stateParams.RoSID);
            };

            $scope.initReview = function () {
                var lastAssessmentOn = reviewOfSystemsService.getLastAssessmentOn();

                $scope.get($stateParams.RoSID).then(function () {
                    if (lastAssessmentOn != null) {
                        if (lastAssessmentOn != $scope.reviewOfSystem.LastAssessmentOn) {
                            $scope.allowReview = true;
                            $scope.reviewOfSystem.IsReviewChanged = true;
                        }
                        else {
                            $scope.allowReview = false;
                            $scope.reviewOfSystem.IsReviewChanged = false;
                        }
                    }
                    else {
                        $scope.allowReview = false;
                        $scope.reviewOfSystem.IsReviewChanged = false;
                    }
                });
            };

            $scope.init = function () {

                $scope.ContactID = $stateParams.ContactID;
                $scope.endDate = new Date();
                $scope.$parent['autoFocus'] = true; //for Focus
                $scope.setDefaultDatePickerSettings();
                $scope.takenOnDetail = {};
                $scope.newResponse = {};

                var isReviewChanged = false;
                if ($scope.reviewOfSystem && $scope.reviewOfSystem.IsReviewChanged) {
                    isReviewChanged = $scope.reviewOfSystem.IsReviewChanged;
                }
                $scope.reviewOfSystem = {
                    IsReviewChanged: isReviewChanged
                };
                $scope.reviewOfSystems = [];
                $scope.initializeBootstrapTable();
                $scope.reviewByLookups = reviewOfSystemsService.getLookups("Users");
                $scope.getReviewOfSystems();
                if ($stateParams.RoSID == undefined)
                    $scope.newReviewOfSystems();
                $('#takenTime').timepicker({
                    minuteStep: 1,
                    showInputs: false,
                    //disableFocus: true
                });
                $scope.reviewOfSystem = { RoSID: 0 };
            };

            $scope.$on('showDetails', function (event, args) {
                $scope.getReviewOfSystems().then(function () {
                    setGridItem(reviewOfSystemsTable, 'RoSID', args.id);
                });
            });
            $scope.init();
        }
    ]);