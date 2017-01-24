angular.module('xenatixApp')
    .controller('noteController', ['$scope', '$filter', 'noteService', 'alertService', 'lookupService', '$stateParams', '$state', '$rootScope', 'formService', 'navigationService', '$q',
    function ($scope, $filter, noteService, alertService, lookupService, $stateParams, $state, $rootScope, formService, navigationService, $q) {
        var UserID = 0;
        $scope.initLookups = function () {
            $scope.NoteTypeList = lookupService.getLookupsByType('NoteType');
            $scope.UserList = lookupService.getLookupsByType('Users');
            $scope.NoteStatusList = lookupService.getLookupsByType('NoteStatus');
        };

        $scope.getLookupsByType = function (typeName) {
            return lookupService.getLookupsByType(typeName);
        };

        $scope.resetForm = function (isInit) {
            if ($scope != null && $scope.ctrl != null && $scope.ctrl.noteForm != null && isInit == undefined)
                $rootScope.formReset($scope.ctrl.noteForm, $scope.ctrl.noteForm.name);
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
        };

        $scope.selectOwner = function (item) {
            $scope.note.TakenBy = item.ID;
        };

        $scope.newNote = function (isInit) {
            $scope.note = {
                NoteID: 0,
                Notes: '',
                NoteTypeID: null,
                ContactID: $scope.ContactID,
                EncounterID: null,
                TakenBy: null,
                TakenTime: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
                NoteStatusID: null
            };
            $scope.takenOnDetail = {
                TakenOnDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
                TakenOnTime: $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal')
            };

            if (UserID <= 0) {
                navigationService.get().then(function (data) {
                    UserID = data.DataItems[0].UserID;
                    $scope.takenOnDetail.TakenBy = UserID;      //default taken by to the logged in user
                }).finally(function () {
                    $scope.resetForm(isInit);
                });
            }
            else {
                $scope.takenOnDetail.TakenBy = UserID;      //default taken by to the logged in user
                $scope.resetForm(isInit);
            }
        };

        $scope.prepRowEditData = function (row) {
            $scope.note = row;
            $scope.takenOnDetail.TakenBy = $scope.note.TakenBy;

            $scope.takenOnDetail.TakenOnDate = $filter('toMMDDYYYYDate')($scope.note.TakenTime, 'MM/DD/YYYY', 'useLocal');
            $scope.takenOnDetail.TakenOnTime = $filter('toMMDDYYYYDate')($scope.note.TakenTime, 'hh:mm A', 'useLocal');

            $scope.resetForm();
        };

        $scope.rowEdit = function (row) {
            var isDirty = formService.isDirty();
            if (isDirty) {
                bootbox.dialog({
                    message: "You have unsaved data. What would you like to do?",
                    buttons: {
                        success: {
                            label: "Save",
                            className: "btn-success",
                            callback: function () {
                                $rootScope.safeSubmit(false, true, true);
                                $('#noteTable' + " tr.success").removeClass('success');
                            }
                        },
                        danger: {
                            label: "Discard",
                            className: "btn-danger",
                            callback: function () {
                                $scope.prepRowEditData(row);
                            }
                        }
                    }
                });
            }
            else {
                $scope.prepRowEditData(row);
            }
        };

        $scope.editNote = function (noteID) {
            if (event)
                event.stopPropagation();
            var params = $filter('filter')($scope.noteList, function (note) {
                return note.NoteID == noteID;
            })[0];
            noteService.setData(params);
            $state.go('patientprofile.chart.intake.note.notedetail', params);
        }

        $scope.next = function () {
            if ($scope.note &&
                    $scope.note.NoteID &&
                    $scope.note.NoteTypeID !== 0) {
                $scope.editNote($scope.note.NoteID);
            }
            else {
                alertService.error('Please select a note, before proceeding to the next screen');
            }
        };

        $scope.get = function (isInit) {
            $scope.isLoading = true;
            return noteService.getNotes($scope.ContactID).then(function (data) {
                if (data != undefined && data.DataItems != undefined) {
                    $scope.noteList = data.DataItems;
                    $scope.noteTable.bootstrapTable('load', $scope.noteList);
                } else {
                    $scope.noteList = [];
                    $scope.noteTable.bootstrapTable('removeAll');
                }
            },function (errorStatus) {
                alertService.error('Unable to get note: ' + errorStatus);
            }).finally(function () {
                $scope.isLoading = false;
                $scope.resetForm();
            });
        };

        $scope.saveNote = function (isAdd) {
            if (isAdd) {
                return noteService.add($scope.note);
            }
            else {
                return noteService.update($scope.note);
            }
        };

        $scope.save = function (isNext, mandatory, hasErrors, keepForm, next) {
            if (isNext && next === undefined) {
                next = function () { $scope.next(); }
            }
            if (!mandatory && isNext && hasErrors) {
                next();
            }
            if (!hasErrors) {
                var isAdd = ($scope.note.NoteID === 0 || $scope.note.NoteID === undefined);
                if (formService.isDirty($scope.ctrl.noteForm.name) || isAdd) {
                    $scope.note.TakenBy = $scope.takenOnDetail.TakenBy;

                    var datePart = $filter('formatDate')($scope.takenOnDetail.TakenOnDate);
                    var dateTime = $filter('toMMDDYYYYDate')(datePart + ' ' + $scope.takenOnDetail.TakenOnTime, 'MM/DD/YYYY HH:mm');
                    $scope.note.TakenTime = new Date(dateTime);
                    $scope.saveNote(isAdd).then(function (response) {
                        if (!((response !== undefined) && ('data' in response) && ('ResultCode' in response.data) && (response.data.ResultCode !== 0))) {
                            if (response.data.ResultCode == 0) {
                                if ($scope.note != undefined && $scope.note.NoteID != undefined) {
                                    var successMessage = 'Note has been ' + (isAdd ? 'added' : 'updated') + ' successfully.';
                                }
                                $scope.note.NoteID = (($scope.note !== undefined) && ($scope.note.NoteID !== undefined) && ($scope.note.NoteID != 0))
                                    ? $scope.note.NoteID : response.data.ID;
                                alertService.success(successMessage);
                                if (isNext) {
                                    noteService.getNotes($scope.ContactID).then(function (data) {
                                        if (data != undefined && data.DataItems != undefined) {
                                            $scope.noteList = data.DataItems;
                                            next();
                                        }
                                    }, function (errorStatus) {
                                        alertService.error('Unable to get note: ' + errorStatus);
                                    }).finally(function () {
                                        $scope.isLoading = false;
                                    });
                                }
                                else {
                                    $scope.newNote();
                                    $scope.get();
                                }
                            }
                            else {
                                alertService.error('Unable to save Note');
                            }
                        }
                        else {
                            alertService.error('Unable to save Note');
                        }
                    });
                } else if (isNext) {
                    next();
                }
            }
        };

        $scope.remove = function (noteID, event) {
            event.stopPropagation();
            bootbox.confirm('Selected Note will be removed.\n Do you want to continue?', function (confirmed) {
                if (confirmed) {
                    var tempNoteList = $scope.noteList;
                    $scope.noteList = $filter('filter')($scope.noteList, { NoteID: '!' + noteID });
                    noteService.remove($scope.ContactID, noteID).then(function (response) {
                        if (response.ResultCode === 0) {
                            alertService.success('Note has been successfully deleted.');
                            $scope.get();
                        } else {
                            alertService.error('Unable to delete Note.');
                            $scope.noteList = tempNoteList;
                        }
                    });
                }
            });
        };

        $scope.updateNoteDetails = function (isNext, mandatory, hasErrors, keepForm, next) {
            if (isNext && next === undefined) {
                next = function () { $scope.next(); }
            }
            if (!mandatory && isNext && hasErrors) {
                next();
            }

            if (!hasErrors) {
                if (formService.isDirty($scope.ctrl.noteForm.name)) {
                    noteService.updateNoteDetails($scope.noteDetails).then(function (response) {
                        if (!((response !== undefined) && ('data' in response) && ('ResultCode' in response.data) && (response.data.ResultCode !== 0))) {
                            if (response.data.ResultCode == 0) {
                                if ($scope.note != undefined && $scope.note.NoteID != undefined) {
                                    var successMessage = 'Note has been updated successfully.';
                                }
                                $scope.note.NoteID = (($scope.note !== undefined) && ($scope.note.NoteID !== undefined) && ($scope.note.NoteID != 0))
                                    ? $scope.note.NoteID : response.data.ID;
                                alertService.success(successMessage);
                            }
                            else {
                                alertService.error('Unable to save Note');
                            }
                        }
                        else {
                            alertService.error('Unable to save Note');
                        }
                    });
                } else if (isNext) {
                    next();
                }
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
                onClickRow: function (e, row, $element) {
                    $scope.rowEdit(e);
                },
                columns: [
                    {
                        field: "NoteTypeID",
                        title: "Note Type",
                        formatter: function (value, row, index) {
                            if (value) {
                                var lstTxtById = lookupService.getSelectedTextById(value, $scope.NoteTypeList);
                                if (lstTxtById != undefined && lstTxtById.length > 0)
                                    return lstTxtById[0].Name;
                                else
                                    return "";
                            }
                            else { return ""; }
                        }
                    },
                    {
                        field: "DocumentStatusID",
                        title: "Note Status",
                        formatter: function (value, row, index) {
                            if (value) {
                                var lstTxtById = lookupService.getSelectedTextById(value, $scope.NoteStatusList);
                                if (lstTxtById != undefined && lstTxtById.length > 0)
                                    return lstTxtById[0].Name;
                                else
                                    return "";
                            }
                            else { return ""; }
                        }
                    },
                    {
                        field: "TakenTime",
                        title: "Note Date",
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
                                var lstTxtById = lookupService.getSelectedTextById(value, $scope.UserList);
                                if (lstTxtById != undefined && lstTxtById.length > 0)
                                    return lstTxtById[0].Name;
                                else
                                    return "";
                            }
                            else { return ""; }
                        }
                    },
                    {
                        field: "NoteID",
                        title: "",
                        formatter: function (value, row, index) {

                            return (
                                '<a href="javascript:void(0)" data-default-no-action id="editNote" name="editNote" data-toggle="modal" data-target="#NoteModel" ' +
                                    'ng-click="editNote(' + value + ');" security permission-key="Clinical-Note-Note" permission="update" title="Edit" space-key-press><i class="fa fa-pencil fa-fw" /></a>' +
                                '<a href="javascript:void(0)" data-default-no-action ng-click="remove(' + value + ', $event)" id="deleteNote" name="deleteNote" title="Delete" security permission-key="Clinical-Note-Note" permission="delete" ' +
                                'space-key-press><i class="fa fa-trash fa-fw"></i></a>'
                                );
                        }
                    }
                ]
            };
        };

        $scope.init = function () {
            $scope.note = { NoteID: 0 };
            $scope.endDate = new Date();
            $scope.ContactID = $stateParams.ContactID;
            $scope.initLookups();
            $scope.setDefaultDatePickerSettings();
            $scope.newNote(true);
            $scope.noteTable = $("#noteTable");
            $scope.initializeBootstrapTable();
            $scope.note.NoteTypeID = parseInt($stateParams.NTypeID);
            $scope.get(true);
            $('#takenTime').timepicker({
                minuteStep: 1,
                showInputs: false,
                //disableFocus: true
            });
           
            $scope.resetForm();
            $scope.$parent['autoFocus'] = true;
        };

        $scope.$on('showDetails', function (event, args) {
            $scope.get().then(function () {
                setGridItem($scope.noteTable, 'NoteID', args.id);
            });
        });

        $scope.init();
    }]);