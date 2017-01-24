angular.module('xenatixApp')
    .controller('chiefComplaintController', [
        '$http', '$scope', '$filter', 'chiefComplaintService', 'alertService', 'lookupService', '$stateParams', '$state', '$rootScope', 'formService', 'navigationService',
        function ($http, $scope, $filter, chiefComplaintService, alertService, lookupService, $stateParams, $state, $rootScope, formService, navigationService) {

    var chiefComplaintTable = $('#chiefComplaintTable');
    var UserID = 0;

    $scope.resetForm = function () {
        if ($scope != null && $scope.ctrl != null && $scope.ctrl.chiefComplaintForm != null)
            $rootScope.formReset($scope.ctrl.chiefComplaintForm, $scope.ctrl.chiefComplaintForm.name);
    };

    $scope.init = function () {
        $scope.endDate = new Date();
        $scope.setDefaultDatePickerSettings();
        $scope.chiefComplaint = {};
        $scope.takenOnDetail = {};
        $scope.ContactID = $stateParams.ContactID;
        $scope.rollbackChiefComplaint = {};
        $scope.rollbackTakenOnDetail = {};
        $scope.getList();
        $scope.initializeBootstrapTable();
        $scope.newChiefComplaint();
        $scope.chiefComplaint = { ChiefComplaintID: 0 };
        $('#takenTime').timepicker({
            minuteStep: 1,
            showInputs: false,
            //disableFocus: true
        });
        $scope.$parent['autoFocus'] = true;
    };

    $scope.newChiefComplaint = function () {
        $scope.chiefComplaint = {
            ChiefComplaintID: 0,
            ContactID: $scope.ContactID,
            EncounterID: null,
            Complaint: ''
        };

        $scope.takenOnDetail = {
            TakenOnDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
            TakenOnTime: $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal')
        };
        $scope.rollbackChiefComplaint = angular.copy($scope.chiefComplaint);
        if (UserID <= 0) {
            navigationService.get().then(function (data) {
                UserID = data.DataItems[0].UserID;
                $scope.takenOnDetail.TakenBy = UserID; // default taken by to the logged in user
                $scope.rollbackTakenOnDetail = angular.copy($scope.takenOnDetail);
                $scope.resetForm();
            },
            function (errorStatus) {
                $scope.resetForm();
            });
        }
        else {
            $scope.takenOnDetail.TakenBy = UserID; // default taken by to the logged in user
            $scope.rollbackTakenOnDetail = angular.copy($scope.takenOnDetail);
            $scope.resetForm();
        }
    }

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

    $scope.save = function (isNext, mandatory, hasErrors) {
        var isDirty = formService.isDirty()

        if (isDirty && !hasErrors) {
            var modelToSave = angular.copy($scope.chiefComplaint);
            modelToSave.ContactID = $stateParams.ContactID;
            modelToSave.TakenBy = $scope.takenOnDetail.TakenBy;
            modelToSave.TakenDate = $filter('toMMDDYYYYDate')($scope.takenOnDetail.TakenOnDate, 'MM/DD/YYYY');

            var datePart = $filter('formatDate')($scope.takenOnDetail.TakenOnDate);
            var dateTime = $filter('toMMDDYYYYDate')(datePart + ' ' + $scope.takenOnDetail.TakenOnTime, 'MM/DD/YYYY HH:mm')

            modelToSave.TakenTime = new Date(dateTime);

            if (modelToSave.ChiefComplaintID !== undefined && modelToSave.ChiefComplaintID !== 0) {
                $scope.update(isNext, modelToSave);
            }
            else {
                $scope.add(isNext, modelToSave);
            }
        }
        else if (!isDirty && isNext) {
            $scope.next();
        }
    };

    $scope.add = function (isNext, modelToSave) {
        chiefComplaintService.add(modelToSave).then(function (response) {
            $scope.chiefComplaint.ChiefComplaintID = (($scope.chiefComplaint !== undefined) && ($scope.chiefComplaint.ChiefComplaintID !== undefined) && ($scope.chiefComplaint.ChiefComplaintID != 0))
                ? $scope.chiefComplaint.ChiefComplaintID : response.data.ID;
            alertService.success('Chief complaint has been saved.');
            $scope.init();
            if (isNext)
                $scope.next();
        },
        function (errorStatus) {
            alertService.error('Unable to save the chief complaint.');
        },
        function (notification) {
            alertService.warning(notification);
        }).then(function () {
        });
    };

    $scope.update = function (isNext, modelToSave) {
        chiefComplaintService.update(modelToSave).then(function (response) {
            alertService.success('Chief complaint has been updated.');
            $scope.init();
            if (isNext)
                $scope.next();
        },
        function (errorStatus) {
            alertService.error('Unable to update the chief complaint.');
        },
        function (notification) {
            alertService.warning(notification);
        }).then(function () {
        });
    };

    $scope.next = function () {
       
        var nextState = $("li[data-state-name].list-group-item.active").next("li[data-state-name].list-group-item");
        if (nextState.length !== 1) {
            nextState = $scope.returnState;
        } else {
            nextState = nextState.attr('data-state-name');
        }
        $state.go(nextState);
    };

    $scope.cancel = function () {
        if (formService.isDirty()) {
                    bootbox.confirm("You will lose the information entered, continue with CANCEL?", function (confirmed) {
                if (confirmed) {
                    $scope.newChiefComplaint();
                }
            });
        };
    };

    $scope.getLookupsByType = function (typeName) {
        return lookupService.getLookupsByType(typeName);
    };

    $scope.getList = function () {

        return chiefComplaintService.getList($stateParams.ContactID).then(function (data) {
            if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                $scope.chiefComplaintList = data.DataItems;
                chiefComplaintTable.bootstrapTable('load', $scope.chiefComplaintList);
               
            } else {
                $scope.chiefComplaintList = [];
                chiefComplaintTable.bootstrapTable('removeAll');
            }
            $scope.resetForm();
        },
            function (errorStatus) {
                alertService.error('Unable to get chief complaint: ' + errorStatus);
            }).finally(function () {
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
                $scope.rowClicked(e, row);
            },
            columns: [
                {
                    field: 'TakenBy',
                    title: 'Taken By',
                    formatter: function (value, row, index) {
                        return lookupService.getText('Users', value);
                    }
                },
                {
                    field: 'TakenTime',
                    title: 'Date',
                    formatter: function (value, row, index) {
                        return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY hh:mm A', 'useLocal');;
                    }
                },
                {
                    field: 'ChiefComplaintID',
                    title: '',
                    formatter: function (value, row, index) {
                        return '<a href="javascript:void(0)" data-default-action id="editChiefComplaint" name="editChiefComplaint" data-toggle="modal" data-target="#ChiefComplaintModel" ' +
                            'ng-click="editChiefComplaint(' + value + ', $event);" title="Edit" security permission-key="Clinical-ChiefComplaint-ChiefComplaint"  ' +
                                    'permission="update" space-key-press><i class="fa fa-pencil fa-fw" /></a>' +
                                     '<a href="javascript:void(0)" data-default-no-action ng-click="remove(' + value + ',$event)" id="deactivateChiefComplaint" name="deactivateChiefComplaint" title="Deactivate" security permission-key="Clinical-ChiefComplaint-ChiefComplaint" permission="delete" space-key-press><i class="fa fa-trash fa-fw"></i></a>';
                    }
                }
            ]
        };
    };

    $scope.rowClicked = function (row) {
        $scope.chiefComplaint.ChiefComplaintID = row.ChiefComplaintID;
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
                            $scope.setFields(row);
                            $('#ifspTable' + " tr.success").removeClass('success');
                        }
                    },
                    danger: {
                        label: "Discard",
                        className: "btn-danger",
                        callback: function () {
                            $scope.setFields(row);
                        }
                    }
                }
            });
        }
        else {
            $scope.setFields(row);
        }

        $scope.rollbackChiefComplaint = angular.copy($scope.chiefComplaint);
        $scope.rollbackTakenOnDetail = angular.copy($scope.takenOnDetail);
    };
    
    $scope.allowEnterKey = function () {
        $scope.ignoreEnter = true;
    }

    $scope.captureEnterKey = function () {
        $scope.ignoreEnter = false;
    }

    $scope.setFields = function (row) {
        $scope.chiefComplaint = angular.copy(row);
        $scope.takenOnDetail.TakenBy = $scope.chiefComplaint.TakenBy;

        $scope.takenOnDetail.TakenOnDate = $filter('toMMDDYYYYDate')($scope.chiefComplaint.TakenTime, 'MM/DD/YYYY', 'useLocal');
        $scope.takenOnDetail.TakenOnTime = $filter('toMMDDYYYYDate')($scope.chiefComplaint.TakenTime, 'hh:mm A', 'useLocal');

        $scope.resetForm();
    }

    $scope.remove = function (chiefComplaintID, event) {
                event.stopPropagation();
                bootbox.confirm('Are you sure you want to deactivate?', function (confirmed) {
                    if (confirmed) {
                        var tempChiefComplaint = $scope.chiefComplaintList;
                        $scope.chiefComplaintList = $filter('filter')($scope.chiefComplaintList, { ChiefComplaintID: '!' + chiefComplaintID });
                        chiefComplaintService.deleteChiefComplaint($scope.ContactID, chiefComplaintID).then(function (response) {
                            if (response.ResultCode === 0) {
                                alertService.success('Chief Complaint has been deleted successfully.');
                                $scope.init();
                            } else {
                                alertService.error('Unable to delete Chief Complaint.');
                                $scope.chiefComplaintList = tempChiefComplaint;
                            }
                        });
                    }
                });
            };
    $scope.$on('showDetails', function (event, args) {
        $scope.getList().then(function () {
            setGridItem(chiefComplaintTable, 'ChiefComplaintID', args.id);
        });
    });
    $scope.init();

}]);

