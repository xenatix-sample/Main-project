(function () {
    angular.module("xenatixApp")
        .controller("progressNoteController", [
            "progressNoteService", "$scope", "$filter", "alertService", "$stateParams", "$state", "formService", "$rootScope", "navigationService", "lookupService",
        function (progressNoteService, $scope, $filter, alertService, $stateParams, $state, formService, $rootScope, navigationService, lookupService) {

            var noteTable = $("#progressNoteTable");
            var collateralGroupID = RELATIONSHIP_TYPE_GROUPID.Collateral;

            init = function () {
                $scope.ContactID = $stateParams.ContactID;
                $scope.NoteTypeID = $stateParams.NoteTypeID;
                $scope.endDate = new Date();
                $scope.noNext = true;
                $scope.noCancel = true;
                $scope.initForm();
                $scope.showOtherContact = false;
                initTakenDetails();
                initializeBootstrapTable();
                initDropdowns();
                getList().then(function () {
                    if ($stateParams.NoteHeaderID != undefined)
                        setGridItem(noteTable, 'NoteHeaderID', parseInt($stateParams.NoteHeaderID));
                });
                $('#endtime,#starttime,#notetime').timepicker({
                    minuteStep: 1,
                    showInputs: false,
                    //disableFocus: true,
                    change: function (time) {
                        $scope.calcDuration();
                    }
                });
                $scope.calcDuration();
                resetForm();
            };

            $scope.initForm = function () {
                $scope.noteHeader = {
                    NoteHeaderID: 0,
                    ContactID: $scope.ContactID,
                    NoteTypeID: $scope.NoteTypeID,
                    TakenBy: $scope.CurrentUserID,
                    TakenTime: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
                    Discharge: {
                        DischargeID: 0,
                        ProgressNoteID: null,
                        DischargeTypeID: null,
                        TakenBy: $scope.CurrentUserID,
                        DischargeDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal')
                    },
                    ProgressNote: {
                        ProgressNoteID: 0,
                        ProgressNoteDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
                        NoteHeaderID: null,
                        ContactMethodID: null,
                        ContactMethodOther: null,
                        FirstName: null,
                        LastName: null,
                        RelationshipTypeID: null,
                        Summary: null,
                        ReviewedSourceConcerns: null,
                        ReviewedECIProcess: false,
                        ReviewedECIServices: false,
                        ReviewedECIRequirements: false,
                        IsSurrogateParentNeeded: false,
                        Comments: null,
                        IsDischarged: false,
                        ContactID: null,
                        NoteTypeID: $scope.NoteTypeID,
                        StartTime: $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal'),
                        EndTime: $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal')
                    },
                    ProgressNoteAssessment: {
                        ScheduleNoteAssessmentID: null,
                        ProgressNoteID: null,
                        NoteAssessmentDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
                        NoteAssessmentTime: $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal'),
                        LocationID: null,
                        Location:null,
                        ProviderID: null,
                        MembersInvited: null
                    }
                };
                setDefaultDatePickerSettings();
                resetForm();
            };

            initTakenDetails = function () {
                $scope.noteHeader.TakenTime = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal');

                navigationService.get().then(function (data) {
                    if (data && data.DataItems) {
                        $scope.CurrentUserID = data.DataItems[0].UserID;
                        $scope.noteHeader.TakenBy = $scope.CurrentUserID;
                        $scope.noteHeader.Discharge.TakenBy = $scope.CurrentUserID;
                    }
                    resetForm();
                });
            };

            setDefaultDatePickerSettings = function () {
                $scope.opened = false;
                $scope.dateOptions = {
                    formatYear: 'yy',
                    startingDay: 1,
                    showWeeks: false
                };
                $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'MM/dd/yyyy'];
                $scope.format = $scope.formats[3];
            };

            initDropdowns = function () {
                $scope.ContactMethodList = $rootScope.getLookupsByType('ContactMethod');
                $scope.DischargeReasonList = $rootScope.getLookupsByType('DischargeReason');

                var relationshipLookups = $rootScope.getLookupsByType('RelationshipType');
                $scope.RelationshipTypeList = $filter('filter')(relationshipLookups, { RelationshipGroupID: collateralGroupID });
            };

            resetForm = function () {
                if ($scope && $scope.ctrl && $scope.ctrl.progressNoteForm)
                    $rootScope.formReset($scope.ctrl.progressNoteForm);
            };

            getList = function () {
                return progressNoteService.getList($scope.NoteTypeID, $scope.ContactID).then(getListSuccessMethod, failureMethod, notificationMethod);
            };

            get = function () {
                return progressNoteService.get($scope.noteHeader.ProgressNoteID, $scope.NoteTypeID, $scope.ContactID).then(getSuccessMethod, failureMethod, notificationMethod);
            };

            $scope.save = function (isNext, mandatory, hasErrors, keepForm) {
                var isDirty = formService.isDirty();
                if (isDirty && !hasErrors) {
                    var isAdd = ($scope.noteHeader.ProgressNote.ProgressNoteID === undefined || $scope.noteHeader.ProgressNote.ProgressNoteID === 0);
                    $scope.noteHeader.ProgressNote.StartTimeSecs = timespanToSeconds($scope.noteHeader.ProgressNote.StartTime);
                    $scope.noteHeader.ProgressNoteAssessment.NoteAssessmentTimeSecs = timespanToSeconds($scope.noteHeader.ProgressNoteAssessment.NoteAssessmentTime);
                    $scope.noteHeader.ProgressNote.EndTimeSecs = timespanToSeconds($scope.noteHeader.ProgressNote.EndTime);
                    if (!$scope.showOtherContact)
                        $scope.noteHeader.ProgressNote.ContactMethodOther = '';
                    isAdd ? progressNoteService.add($scope.noteHeader).then(saveSuccessMethod, failureMethod, notificationMethod) :
                            progressNoteService.update($scope.noteHeader).then(saveSuccessMethod, failureMethod, notificationMethod);
                }
            };

            $scope.remove = function (noteHeaderID) {
                bootbox.confirm('Are you sure you want to deactivate?', function (confirmed) {
                    if (confirmed) {
                        progressNoteService.remove($scope.noteHeader.NoteTypeID, $scope.ContactID, noteHeaderID).then(function (data) {
                            if (data.ResultCode === 0) {
                                alertService.success('Note has been successfully deleted.');
                                $scope.initForm();
                                getList();
                            } else {
                                alertService.error('Error while deactivating allergy');
                            }
                        });
                    }
                });
            };

            getListSuccessMethod = function (response) {
                if (response && response.DataItems)
                {
                    $scope.noteHeaderList = response.DataItems;
                    if ($scope.noteHeaderList.length > 0)
                    {
                        angular.forEach($scope.noteHeaderList, function (noteHeader) {
                            noteHeader.ProgressNote.StartTime = secondsToHMS(noteHeader.ProgressNote.StartTimeSecs);
                            noteHeader.ProgressNote.EndTime = secondsToHMS(noteHeader.ProgressNote.EndTimeSecs);
                            if (noteHeader.ProgressNoteAssessment)
                                noteHeader.ProgressNoteAssessment.NoteAssessmentTime = secondsToHMS(noteHeader.ProgressNoteAssessment.NoteAssessmentTimeSecs);
                        });
                        noteTable.bootstrapTable('load', $scope.noteHeaderList);
                        $scope.permissionID = $scope.noteHeader.ProgressNote.ProgressNoteID = response.DataItems[0].ProgressNote.ProgressNoteID;
                    }
                    else
                    {
                        $scope.permissionID = $scope.noteHeader.ProgressNote.ProgressNoteID = 0;

                        noteTable.bootstrapTable('removeAll');
                    }
                }
                else
                {
                    alertService.error('OOPs something went wrong');
                }
            };

            failureMethod = function (response) {
                alertService.error('OOPs something went wrong');
            };

            notificationMethod = function (response) {
                //Add code if required
            };

            saveSuccessMethod = function (response) {
                var isAdd = ($scope.noteHeader.ProgressNote.ProgressNoteID === undefined || $scope.noteHeader.ProgressNote.ProgressNoteID === 0);
                if (response && (!isAdd || response.data.ID != 0)) {
                    alertService.success('Note has been ' + (isAdd ? 'added' : 'updated') + ' successfully.');
                    $scope.initForm();
                    resetForm();
                }
                else {
                    alertService.error('Unable to ' + (isAdd ? 'added' : 'updated') + ' Note');
                    return;
                }
                getList();
            };

            $scope.$watch('noteHeader.ProgressNote.ContactMethodID', function () {
                $scope.checkOtherContactVisibility();
            });

            $scope.checkOtherContactVisibility = function () {
                var otherel = lookupService.getSelectedTextByText('Other', $scope.ContactMethodList);
                if (otherel != undefined && otherel.length > 0) {
                    if (otherel[0].ID === $scope.noteHeader.ProgressNote.ContactMethodID) {
                        $scope.showOtherContact = true;
                    }
                    else {
                        $scope.showOtherContact = false;
                    }
                }
            }

            //This should also call the servcieRecoringService.js calcDuration to calculate the final Duration. Not making the change now to make sure we do not introduce any new bugs
            $scope.calcDuration = function () {
                if ($scope.noteHeader.ProgressNote.StartTime && $scope.noteHeader.ProgressNote.EndTime) {
                    try {
                        //Clear out the duration at the beginning so that if its an invalid date or duration is negative we do not retain the old calculated duration in the model
                        $scope.noteHeader.ProgressNote.Duration = '';
                        var start = formatTimeToDate($scope.noteHeader.ProgressNote.StartTime);
                        var end = formatTimeToDate($scope.noteHeader.ProgressNote.EndTime);
                        var elapsed = end - start; // time in milliseconds
                        $scope.smallTimeError = elapsed >= 0 ? false : true;
                        if (elapsed >= 0) {
                            $scope.ctrl.progressNoteForm.endtime.$setValidity('date', true);
                            elapsed = elapsed / 1000;
                            var seconds = Math.floor(elapsed % 60);
                            elapsed = elapsed / 60;
                            var minutes = Math.floor(elapsed % 60);
                            elapsed = elapsed / 60;
                            var hours = Math.floor(elapsed % 24);
                            //display minutes at the interval of 15.
                            //show 0 if mins < 8.
                            //show 15 if mins >= 8 < (15 + 8).
                            //var diff = minutes / 15;
                            var dispMinutes = Math.floor(minutes / 15) * 15;
                            var pendingMins = minutes % 15;
                            if (pendingMins >= 8)
                                dispMinutes += 15;
                            if (dispMinutes == 60) {
                                dispMinutes = 0;
                                hours += 1;
                            }
                            if (hours == 0 && dispMinutes == 0) {
                                $scope.noteHeader.ProgressNote.Duration = '';
                            }
                            else {
                                $scope.noteHeader.ProgressNote.Duration = hours > 0 ? (hours + 'hr ' + dispMinutes + 'mins') : dispMinutes + 'mins';
                            }
                        }
                        else {
                            $scope.ctrl.progressNoteForm.endtime.$setValidity('date', false);
                            $scope.noteHeader.ProgressNote.Duration = '';
                        }
                    }
                    catch (err) {
                        $scope.noteHeader.ProgressNote.Duration = '';
                    };
                }
            };

            formatTimeToDate = function (timeVal) { //format should be hh:mm tt
                var d = new Date();
                var hr = timeVal.substring(0, timeVal.indexOf(':'));
                if (timeVal.substring(timeVal.indexOf(' ') + 1, timeVal.length) == "PM" && hr != 12)      //checks if PM, adds 12 hours
                    hr = +hr + +12;
                var min = timeVal.substring(timeVal.indexOf(':') + 1, timeVal.indexOf(' '));
                return new Date(d.setHours(hr, min));
            };

            $scope.prepRowEditData = function (row) {
                if (row.TakenTime)
                    row.TakenTime = new Date(row.TakenTime);
                if (row.Discharge && row.Discharge.DischargeDate)
                    row.Discharge.DischargeDate = new Date(row.Discharge.DischargeDate);
                if (row.ProgressNoteAssessment && row.ProgressNoteAssessment.NoteAssessmentDate)
                    row.ProgressNoteAssessment.NoteAssessmentDate = new Date(row.ProgressNoteAssessment.NoteAssessmentDate);
                $scope.noteHeader = row;
                $scope.permissionID = $scope.noteHeader.ProgressNote.ProgressNoteID 

                $scope.checkOtherContactVisibility();
                $scope.calcDuration();
                resetForm();
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
                                    $scope.prepRowEditData(row);
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

            initializeBootstrapTable = function () {
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
                            field: 'TakenTime',
                            title: 'Date',
                            formatter: function (value, row, index) {
                                if (value) {
                                    var formattedDate = $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY', 'useLocal');
                                    return formattedDate;
                                } else
                                    return '';
                            }
                        },
                        {
                            field: 'StartTime',
                            title: 'Start Time',
                            formatter: function (value, row, index) {
                                return row.ProgressNote.StartTime;
                            }
                        },
                        {
                            field: 'ProgressNote.EndTime',
                            title: 'End Time',
                            formatter: function (value, row, index) {
                                return row.ProgressNote.EndTime;
                            }
                        },
                        {
                            field: 'ContactMethodID',
                            title: 'Contact Method',
                            formatter: function (value, row, index) {
                                var obj = lookupService.getSelectedTextById(row.ProgressNote.ContactMethodID, $scope.ContactMethodList);
                                if (obj.length > 0)
                                    return obj[0].Name;
                                return row.ProgressNote.ContactMethodID;
                            }
                        },
                        {
                            field: 'TakenBy',
                            title: 'Entered By',
                            formatter: function (value, row, index) {
                                var obj = lookupService.getSelectedText('Users', value);
                                if (obj.length > 0)
                                    return obj[0].Name;
                                return row.ProgressNote.ContactMethodID;
                            }
                        },
                        {
                            field: 'IsDischarged',
                            title: 'Discharged',
                            formatter: function (value, row, index) {
                                return row.ProgressNote.IsDischarged ? "Yes" : "No";
                            }
                        },
                        {
                            field: 'DischargeReasonID',
                            title: 'Discharge Reason',
                            formatter: function (value, row, index) {
                                if (row.ProgressNote.IsDischarged) {
                                    var obj = lookupService.getSelectedTextById(row.Discharge.DischargeReasonID, $scope.DischargeReasonList);
                                    if (obj.length > 0)
                                        return obj[0].Name;
                                    return row.Discharge.DischargeReasonID;
                                }
                            }
                        },
                        {
                            field: 'NoteHeaderID',
                            title: '',
                            formatter: function (value, row, index) {
                                return '<a href="javascript:void(0)" data-default-action security permission-key="ECI-ProgressNote-ProgressNote" permission="update" id="editBenefit" name="editNote" data-toggle="modal" ng-click="edit(' + value + ')" title="View/Edit" space-key-press><i class="fa fa-pencil fa-fw" /></a>' +
                                        '<a href="javascript:void(0)" data-default-no-action ng-click="remove(' + value + ')" id="deleteNote" security permission-key="ECI-ProgressNote-ProgressNote" permission="delete" name="deleteNote" title="Delete" space-key-press><i class="fa fa-trash fa-fw"></i></a>';
                            }
                        }
                    ]
                };
            };

            secondsToHMS = function (seconds) {
                //This function can also convert milliseconds to Hour, minutes, seconds
                //// 1- Convert to seconds:
                //var seconds = ms / 1000;
                // 2- Extract hours:
                var hours = parseInt(seconds / 3600); // 3,600 seconds in 1 hour
                seconds = seconds % 3600; // seconds remaining after extracting hours
                // 3- Extract minutes:
                var minutes = parseInt(seconds / 60); // 60 seconds in 1 minute
                // 4- Keep only seconds not extracted to minutes:
                seconds = seconds % 60;
                var tt = 'AM';
                if (hours > 12) {   //if hours > 12, it will be PM          //|| (hours == 12 && minutes > 0)
                    tt = 'PM';
                    hours -= 12;
                }
                return ("0" + hours).slice(-2) + ":" + ("0" + minutes).slice(-2) + " " + tt;
            }

            timespanToSeconds = function (timeVal) {       //format should be hh:mm tt
                var hr = timeVal.substring(0, timeVal.indexOf(':'));
                if (timeVal.substring(timeVal.indexOf(' ') + 1, timeVal.length) == "PM" && hr != 12)      //checks if PM, adds 12 hours
                    hr = +hr + +12;
                var min = timeVal.substring(timeVal.indexOf(':') + 1, timeVal.indexOf(' '));
                return (hr*3600 + min*60);    //1 Hour = 3600 secs, 1 minute = 60 secs
            }

            init();

        }]);
}());