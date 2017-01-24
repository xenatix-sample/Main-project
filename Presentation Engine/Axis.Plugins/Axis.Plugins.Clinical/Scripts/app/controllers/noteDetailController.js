angular.module('xenatixApp')
    .controller('noteDetailController', ['$scope', '$filter', 'noteService', 'alertService', 'lookupService', '$stateParams', '$state', '$rootScope', 'formService',
    function ($scope, $filter, noteService, alertService, lookupService, $stateParams, $state, $rootScope, formService) {

        $scope.resetForm = function () {
            if ($scope != null && $scope.ctrl != null && $scope.ctrl.noteDetailForm != null)
                $rootScope.formReset($scope.ctrl.noteDetailForm, $scope.ctrl.noteDetailForm.name);
        };

        $scope.updateNoteDetails = function (isNext, mandatory, hasErrors, keepForm, next) {
            if (isNext && next === undefined) {
                next = function () { $scope.next(); }
            }
            if (!mandatory && isNext && hasErrors) {
                next();
            }

            if (!hasErrors) {
                if (formService.isDirty($scope.ctrl.noteDetailForm.name)) {
                    var dateTime = $filter('toMMDDYYYYDate')($scope.noteDetails.TakenTime, 'MM/DD/YYYY HH:mm', 'useLocal');
                    $scope.noteDetails.TakenTime = new Date(dateTime);
                    noteService.update($scope.noteDetails).then(function (response) {
                        if (!((response !== undefined) && ('data' in response) && ('ResultCode' in response.data) && (response.data.ResultCode !== 0))) {
                            if (response.data.ResultCode == 0) {
                                alertService.success('Note has been updated successfully.');
                                $scope.resetForm();
                            }
                            else {
                                alertService.error('Unable to save Note.');
                            }
                        }
                        else {
                            alertService.error('Unable to save Note.');
                        }
                    });
                } else if (isNext) {
                    next();
                }
            }
        }

        $scope.newNote = function () {
            $scope.noteDetails = {
                NoteID: 0,
                Notes: '',
                NoteTypeID: null
            };
            $scope.resetForm();
        };

        $scope.get = function () {
            //$scope.isLoading = true;
            //return noteService.getNote($scope.NoteID).then(function (data) {
            //    if (data != undefined && data.DataItems != undefined && data.DataItems > 0) {
            //        $scope.noteDetails = data.DataItems[0];
            //        $scope.noteDetails.NoteType = lookupService.getText($scope.noteDetails.NoteTypeID);
            //    } else {
            //        $scope.noteList = [];
            //        $scope.noteTable.bootstrapTable('removeAll');
            //    }
            //    $scope.resetForm();
            //    checkFormStatus();
            //},
            //    function (errorStatus) {
            //        alertService.error('Unable to get note: ' + errorStatus);
            //    }).finally(function () {
            //        $scope.isLoading = false;
            //    });
        };

        $scope.init = function () {
            $scope.ContactID = $stateParams.ContactID;
            $scope.newNote();
            var params = noteService.getData();
            if (params.Notes == "") {
                $scope.noteDetails.NoteID = 0;
            }
            else
            {
                $scope.noteDetails.NoteID = params.NoteID;
            }
           
            $scope.noteDetails = params;
            $scope.noteDetails.NoteType = lookupService.getText('NoteType', $scope.noteDetails.NoteTypeID);
            $scope.resetForm();
            $scope.controlsVisible = false;
            $scope.enterKeyStop = true;
            $scope.saveOnEnter = false;
        };

        $scope.init();

    }]);