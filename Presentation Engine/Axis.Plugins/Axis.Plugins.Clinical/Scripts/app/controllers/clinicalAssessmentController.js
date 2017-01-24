angular.module('xenatixApp')
    .controller('clinicalAssessmentController', ['$q', '$scope', '$filter', 'clinicalAssessmentService', 'alertService', 'lookupService', '$stateParams', '$state', '$rootScope', 'formService', 'navigationService', 'assessmentService', '$location', '$anchorScroll',
function ($q, $scope, $filter, clinicalAssessmentService, alertService, lookupService, $stateParams, $state, $rootScope, formService, navigationService, assessmentService, $location, $anchorScroll) {

    $scope.initLookups = function () {

    };

    $scope.getLookupsByType = function (typeName) {
        return lookupService.getLookupsByType(typeName);
    };

    $scope.resetForm = function () {
        if ($scope != null && $scope.ctrl != null && $scope.ctrl.assForm != null)
            $rootScope.formReset($scope.ctrl.assForm, $scope.ctrl.assForm.name);
    };

    $scope.setDefaultDatePickerSettings = function () {
        $scope.opened = false;
        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1,
            showWeeks: false
        };
        $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'MM/dd/yyyy'];
        $scope.format = $scope.formats[3];
        $scope.resetForm();
    };

    $scope.print = function (clinicalAssessmentID, event) {
        event.stopPropagation();
        $state.go('.clinicalAssessment.report', $.extend({}, $stateParams, { clinicalAssessmentID: clinicalAssessmentID }));
    }

    $scope.selectOwner = function (item) {
        $scope.clinicalAssessment.TakenBy = item.ID;
    };

    $scope.newAssessment = function () {
        $scope.endDate = new Date();

        $scope.clinicalAssessment = {
            ClinicalAssessmentID: 0,
            AssessmentID: null,
            ContactID: $scope.ContactID,
            EncounterID: null,
            TakenBy: null,
            TakenTime: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal')
        };

        $scope.takenOnDetail = {
            TakenOnDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
            TakenOnTime: $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal')
        };

        navigationService.get().then(function (data) {
            $scope.takenOnDetail.TakenBy = data.DataItems[0].UserID;// default taken by to the logged in user
             $scope.resetForm();
        });
        $scope.resetForm();
    };

    $scope.next = function () {
        if ($scope.clinicalAssessment &&
              $scope.clinicalAssessment.ClinicalAssessmentID &&
              $scope.clinicalAssessment.ClinicalAssessmentID !== 0) {
            $scope.navigateToClinicalAssessment($scope.clinicalAssessment.ClinicalAssessmentID, $scope.clinicalAssessment.AssessmentID, $scope.clinicalAssessment.ResponseID);
        }
    };

    $scope.getClinicalAssessment = function () {
        $scope.isLoading = true;
        return clinicalAssessmentService.getClinicalAssessmentByContact($scope.ContactID).then(function (data) {
            if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                $scope.clinicalAssessmentList = data.DataItems;
                $scope.assessmentTable.bootstrapTable('load', $scope.clinicalAssessmentList);
            } else {
                $scope.clinicalAssessmentList = [];
                $scope.assessmentTable.bootstrapTable('removeAll');
            }
            $scope.resetForm();
        },
            function (errorStatus) {
                alertService.error('Unable to get assessment: ' + errorStatus);
            }).finally(function () {
                $scope.isLoading = false;
            });
    };

    $scope.get = function (clinicalAssessmentID) {
        $scope.isLoading = true;
        return clinicalAssessmentService.getClinicalAssessment($scope.ContactID, clinicalAssessmentID).then(function (data) {
            if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                $scope.clinicalAssessment = data.DataItems[0];
                $scope.takenOnDetail.TakenBy = $scope.clinicalAssessment.TakenBy;
                
                $scope.takenOnDetail.TakenOnDate = $filter('toMMDDYYYYDate')($scope.clinicalAssessment.TakenTime, 'MM/DD/YYYY', 'useLocal');
                $scope.takenOnDetail.TakenOnTime = $filter('toMMDDYYYYDate')($scope.clinicalAssessment.TakenTime, 'hh:mm A', 'useLocal');
                $scope.resetForm();
            } else {
                alertService.error('Unable to get assessment!');
            };
        },
            function (errorStatus) {
                alertService.error('Unable to get assessment: ' + errorStatus);
            }).finally(function () {
                $scope.isLoading = false;
                $scope.resetForm();
            });
    };

    $scope.saveAssessment = function (isAdd) {
        if (isAdd) {
            var deferred = $q.defer();
            $scope.ensureResponse().then(function (responseID) {
                $scope.clinicalAssessment.ResponseID = responseID;
                $scope.clinicalAssessment.ContactID = $scope.ContactID;
                return clinicalAssessmentService.add($scope.clinicalAssessment).then(function (response) {
                    $scope.newResponse = {};
                    deferred.resolve(response);;
                },
                  function () {
                      deferred.reject();
                  });
            }, function () {
                deferred.reject();
            });
            return deferred.promise;
        }
        else {
            return clinicalAssessmentService.update($scope.clinicalAssessment);
        }
    };

    $scope.ensureResponse = function () {
        var deferred = $q.defer();
        if (($scope.newResponse !== undefined) && (($scope.newResponse.ResponseID !== 0) && ($scope.newResponse.AssessmentID === $scope.clinicalAssessment.AssessmentID))) {
            $scope.promiseNoOp().then(function () {
                deferred.resolve($scope.newResponse.ResponseID);
            });
        } else {
            var assessmentResponse = {
                ResponseID: 0,
                ContactID: $scope.ContactID,
                AssessmentID: $scope.clinicalAssessment.AssessmentID
            };
            assessmentService.addAssessmentResponse(assessmentResponse).then(function (response) {
                if (response.data.ResultCode === 0) {
                    $scope.newResponse = { ResponseID: response.data.ID, AssessmentID: $scope.clinicalAssessment.AssessmentID };
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

    $scope.save = function (isNext, mandatory, hasErrors, keepForm, next) {
        if (next === undefined) {
            next = function () { $scope.next(); }
        }
        if (!mandatory && isNext && !hasErrors)
            next();

        if ((!mandatory && !hasErrors && !isNext) || (mandatory && !hasErrors && !isNext) || (!mandatory && !hasErrors && isNext) || (mandatory && !hasErrors && isNext)) {
            //var isDirty = formService.isDirty();
            //if (isDirty) {
                $scope.clinicalAssessment.TakenBy = $scope.takenOnDetail.TakenBy;
                
                var datePart = $filter('formatDate')($scope.takenOnDetail.TakenOnDate);
                var dateTime = $filter('toMMDDYYYYDate')(datePart + ' ' + $scope.takenOnDetail.TakenOnTime, 'MM/DD/YYYY HH:mm')

                $scope.clinicalAssessment.TakenTime = new Date(dateTime);

                var isAdd = ($scope.clinicalAssessment.ClinicalAssessmentID === 0 || $scope.clinicalAssessment.ClinicalAssessmentID === undefined);

                if ($scope.clinicalAssessment.TakenBy > 0 && $scope.clinicalAssessment.AssessmentID > 0) {
                    $scope.saveAssessment(isAdd).then(function (response) {
                        if ($scope.clinicalAssessment.ClinicalAssessmentID === 0)
                            $scope.clinicalAssessment.ClinicalAssessmentID = response.data.ID;
                        var action = isAdd ? 'added' : 'updated';
                        $scope.postSaveCommon(response, action, isNext);
                    },
                         function (errorStatus) {
                             alertService.error('OOPS! Something went wrong');
                         },
                         function (notification) {
                             alertService.warning(notification);
                         });
                }
            //}
        }
    };

    $scope.initHeader = function () {
        $scope.ContactID = $stateParams.ContactID;
        $scope.get($stateParams.ClinicalAssessmentID);
        $location.hash('top');
        $anchorScroll();
    };

    $scope.postSaveAdd = function (response) {
        return $scope.postSaveCommon(response, 'added');
    };

    $scope.postSaveUpdate = function (response) {
        return $scope.postSaveCommon(response, 'updated');
    };

    $scope.postSaveCommon = function (response, action, isNext) {
        var data = response;
        if (data.data.ResultCode !== 0) {
            alertService.error(data.ResultMessage);
            return $scope.promiseNoOp();
        } else {
            $scope.resetForm();
            alertService.success('Clinical assessment has been ' + action + ' successfully.');
            if (isNext) {
                $scope.next();
            }
            return $scope.getClinicalAssessment();
        }
    };

    $scope.editAssessment = function (clinicalAssessmentID, assessmentID, responseID, event) {
        event.stopPropagation();
        $scope.navigateToClinicalAssessment(clinicalAssessmentID, assessmentID, responseID);
       
    }

    $scope.navigateToClinicalAssessment = function (clinicalAssessmentID, assessmentID, responseID) {

        // Create params object
        var params = {
            ContactID: $scope.ContactID,
            ClinicalAssessmentID: clinicalAssessmentID,
            AssessmentID: assessmentID,
            ResponseID: responseID
        };

        // Use the assessment service to navigate to correct section
        assessmentService.navigateToSection('patientprofile.chart.intake.clinicalAssessment.ca.section', params);        
    };

    $scope.remove = function (assessmentID, event) {
        event.stopPropagation();
        bootbox.confirm('Selected assessment will be removed.\n Do you want to continue?', function (confirmed) {
            if (confirmed) {
                var tempClinicalAssessmentList = $scope.clinicalAssessmentList;
                $scope.clinicalAssessmentList = $filter('filter')($scope.clinicalAssessmentList, { ClinicalAssessmentID: '!' + assessmentID });
                clinicalAssessmentService.remove($scope.ContactID, assessmentID).then(function (response) {
                    if (response.ResultCode === 0) {
                        alertService.success('assessment has successfully been deleted.');
                        $scope.newAssessment();
                        $scope.getClinicalAssessment();
                    } else {
                        alertService.error('Unable to delete assessment.');
                        $scope.clinicalAssessmentList = tempClinicalAssessmentList;
                    }
                });
            }
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
            onClickRow: function (e, row, $element) {
            },
            columns: [
                {
                    field: "AssessmentID",
                    title: "Assessment Name",
                    formatter: function (value, row, index) {
                        if (value) {
                            return lookupService.getText('ClinicalAssessment', value);
                        }
                        else { return ""; }
                    }
                },
                {
                    field: "TakenTime",
                    title: "Assessment Date",
                    formatter: function (value, row, index) {
                        if (value) {
                            return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY hh:mm A', 'useLocal');
                        } else
                            return '';
                    }
                },
                {
                    field: "TakenBy",
                    title: "Owner",
                    formatter: function (value, row, index) {
                        if (value) {
                            return lookupService.getText('Users', value);
                        }
                        else { return ""; }
                    }
                },
                {
                    field: "ClinicalAssessmentID",
                    title: "",
                    formatter: function (value, row, index) {
                        return (
                            '<span class="text-nowrap"><a href="javascript:void(0)" data-default-action ng-click="editAssessment(' + value + ',' + row.AssessmentID + ',' + row.ResponseID + ', $event)" ' +
                            'alt="Edit Clinical Assessment" security permission-key="Clinical-Assessment-Assessment" permission="update" space-key-press>' +
                            '<i  title="Edit" class="fa fa-pencil fa-fw border-left margin-left padding-left-small padding-right-small" /></a>') +

                            '<a href="javascript:void(0)" data-default-no-action ng-click="remove(' + value + ', $event)" id="deleteAssessment" name="deleteAssessment" security permission-key="Clinical-Assessment-Assessment" permission="delete" title="Delete" ' +
                            'space-key-press><i class="fa fa-trash fa-fw"></i></a>' +

                            '</span>';
                    }
                }
            ]
        };
    };

    $scope.init = function () {
        $scope.ContactID = $stateParams.ContactID;
        $scope.initLookups();
        $scope.setDefaultDatePickerSettings();
        $scope.newAssessment();
        $scope.newResponse = {};
        $scope.assessmentTable = $("#assessmentTable");
        $scope.initializeBootstrapTable();
        $scope.getClinicalAssessment();
        $scope.clinicalAssessment = { ClinicalAssessmentID: 0 };
        if ($scope.$parent != undefined && $scope.$parent != null)
            $scope.$parent['autoFocus'] = true;
        $('#takenTime').timepicker({
            minuteStep: 5,
            showInputs: false,
            //disableFocus: true
        });
        $scope.resetForm();
    };

    $scope.$on('showDetails', function (event, args) {
        $scope.getClinicalAssessment($scope.ContactID).then(function () {
            setGridItem($scope.assessmentTable, 'ClinicalAssessmentID', args.id);
        });
    });
    $scope.init();

}]);