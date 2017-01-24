

angular.module('xenatixApp')
    .controller('presentIllnessController', [
        '$scope', '$filter', 'presentIllnessService', 'alertService', 'lookupService', 'navigationService', '$stateParams', '$state', '$rootScope', 'formService', '$q', '$timeout',
        function ($scope, $filter, presentIllnessService, alertService, lookupService, navigationService, $stateParams, $state, $rootScope, formService, $q, $timeout) {


            $scope.init = function () {
                $scope.$parent['autoFocus'] = true;
                $scope.contactID = $stateParams.ContactID;
                $scope.initTakenDetails();
                $scope.initialHPIBundleID = 0;
                $scope.currentHPIBundleID = 0;
                $scope.initLookups();
                $scope.initHPI();
                $scope.hpi = { HPIID: 0 };
                $scope.initHPIDetails();
                $scope.hpiTable = $('#presentIllnessTable');
                $scope.initializeBootstrapTable();
                $scope.setDefaultDatePickerSettings();
                $scope.get();
                $('#takenTime').timepicker({
                    minuteStep: 1,
                    showInputs: false,
                    //disableFocus: true,
                    change: function (time) {
                        $rootScope.validateFutureDate($scope);
                    }
                });
            };

            var resetForm = function () {
                $rootScope.formReset($scope.ctrl.hpiForm);
            };

            $scope.initTakenDetails = function () {
                $scope.endDate = new Date();
                $scope.takenOnDetail = {};
                $scope.takenOnDetail.TakenOnDate = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
                $scope.takenOnDetail.TakenOnTime = $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal')
                navigationService.get().then(function (data) {
                    $scope.CurrentUserID = data.DataItems[0].UserID;
                    $scope.takenOnDetail.TakenBy = $scope.CurrentUserID;
                    resetForm();
                });
            };

            $scope.initLookups = function () {
                $scope.UserList = lookupService.getLookupsByType('Users');
            };

            $scope.initHPI = function () {
                $scope.hpi = {};
            };

            $scope.initHPIDetails = function () {
                $scope.hpiDetails = {};
                $scope.hpiDetailsGridData = {};
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
                             field: "TakenBy",
                             title: "Taken By",
                             formatter: function (value, row, index) {
                                 return getText($scope.takenBy, $scope.UserList);
                             }
                         },
                        {
                            field: 'TakenTime',
                            title: 'Date',
                            formatter: function (value, row, index) {
                                return $filter('toMMDDYYYYDate')($scope.takenOn, 'MM/DD/YYYY hh:mm A', 'useLocal');
                            }
                        },
                        {
                            field: 'Location',
                            title: 'Location',
                            formatter: function (value, row, index) {
                                return value;
                            }
                        },

                        {
                            field: 'HPIDetailID',
                            title: '',
                            formatter: function (value, row) {
                                return '<a href="javascript:void(0)" data-default-action id="editHPIDetail" name="editHPIDetail"' +
                                    'title="Edit" security permission-key="Clinical-PresentIllness-PresentIllness" permission="update" ng-click="editHPIDetail(' + value + ', $event);" space-key-press><i class="fa fa-pencil fa-fw" /></a>'
                                    +
                                    '<a href="javascript:void(0)" data-default-no-action ng-click="delete(' + value + ', $event)" id="deleteHPI" name="deleteHPI"security permission-key="Clinical-PresentIllness-PresentIllness" permission="delete" title="Deactivate HPI" ' +
                                    'space-key-press><i class="fa fa-trash fa-fw"></i></a>';
                            }
                        }
                    ]
                };
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


            $scope.get = function () {
                var dfd = $q.defer();
                return presentIllnessService.getHPIBundle($scope.contactID).then(function (bundleData) {
                    if (bundleData.ResultCode === 0) {
                        if (bundleData.DataItems.length > 0) {
                            var bundleModel = $filter('orderBy')(bundleData.DataItems, 'TakenTime', true)[0];
                            $scope.hpiBundle = bundleModel;
                            $scope.takenBy = bundleModel.TakenBy;
                            $scope.takenOn = bundleModel.TakenTime;
                            $scope.currentHPIBundleID = bundleModel.HPIID;
                            if ($scope.initialHPIBundleID === 0) {
                                $scope.initialHPIBundleID = bundleModel.HPIID;
                            }

                            presentIllnessService.getHPIDetails(bundleModel.HPIID, $scope.contactID).then(function (data) {
                                if (data.ResultCode === 0) {
                                    $scope.hpiDetailsGridData = data.DataItems;
                                    if (data.DataItems != undefined && data.DataItems.length > 0) {
                                        $scope.hpiTable.bootstrapTable('load', $scope.hpiDetailsGridData);
                                    } else {
                                        $scope.hpiTable.bootstrapTable('removeAll');
                                    }
                                    resetForm();
                                    dfd.resolve(true);
                                } else {
                                    alertService.error('Error while loading hpi details');
                                    dfd.reject(false);
                                }
                            });
                        }
                        $scope.hpi.HPIID = 0;
                        applyDropupOnGrid(false);
                    } else {
                        alertService.error('Error while getting hpi bundle');
                        dfd.reject(false);
                    }
                },
               function (errorStatus) {
                   alertService.error('Error while getting hpi bundle: ' + errorStatus);
                   dfd.reject(errorStatus);
               });

                return dfd.promise;
            };

            $scope.delete = function (hpiDetailID, event) {
                if (event !== null && event !== undefined) {
                    event.stopPropagation();
                }

                bootbox.confirm('Are you sure you want to deactivate this hpi?', function (confirmed) {
                    if (confirmed) {
                        //always create new bundle with current taken details, copy all detail records, then make deletion on copied detail id
                        $scope.hpi.TakenTime = new Date();
                        $scope.hpi.TakenBy = $scope.CurrentUserID;

                        $scope.hpi.ContactID = $scope.contactID;
                        $scope.hpi.HPIID = 0;

                        return presentIllnessService.addHPI($scope.hpi).then(function (data) {
                            if (data.data.ResultCode === 0) {
                                alertService.success('HPI bundle created successfully');
                                var tmpNewBundleID = data.data.ID;
                                //copy any detail records into the new bundle
                                var deferred = $q.defer();
                                var taskArray = [];
                                if ($scope.hpiDetailsGridData.length > 0) {
                                    var tmphpiDetailsGridData = angular.copy($scope.hpiDetailsGridData);
                                    angular.forEach(tmphpiDetailsGridData, function (item) {
                                        //only add the items that are not being deleted to the new bundle
                                        if (item.HPIDetailID !== hpiDetailID) {
                                            if (typeof item.HPIID === 'object') {
                                                item.HPIID = item.HPIID.ID;
                                            }
                                            item.HPIID = tmpNewBundleID;
                                            //save all detail records in this loop                                            
                                            item.ContactID = $scope.contactID;
                                            item.HPIDetailID = 0;
                                            taskArray.push([presentIllnessService.addHPIDetail, [item]]);
                                        }
                                    });
                                }
                                $q.serial(taskArray).finally(function () {
                                    deferred.resolve();
                                    alertService.success('Present Illness deactivated successfully');

                                    $scope.postSave(false);
                                },
                            function (error) {
                                deferred.reject();
                            });

                                return deferred.promise;
                            } else {
                                alertService.error('Error while adding hpi bundle');
                            }
                        });
                    }
                });
            };

            $scope.save = function (isNext, mandatory, hasErrors) {
                if (!mandatory && isNext && hasErrors) {
                    $scope.postSave(isNext);
                }

                if (!formService.isDirty() && isNext && !hasErrors) {
                    $scope.postSave(isNext);
                }

                if (formService.isDirty() && !hasErrors) {
                    //$scope.populateSymptoms();

                    var datePart = $filter('formatDate')($scope.takenOnDetail.TakenOnDate);
                    var dateTime = $filter('toMMDDYYYYDate')(datePart + ' ' + $scope.takenOnDetail.TakenOnTime, 'MM/DD/YYYY HH:mm');

                    $scope.hpi.TakenTime = new Date(dateTime);

                    $scope.hpi.ContactID = $scope.contactID;
                    $scope.hpi.TakenBy = $scope.takenOnDetail.TakenBy;

                    if ($scope.currentHPIBundleID === $scope.initialHPIBundleID) {
                        //new bundle must be created
                        $scope.hpi.ContactID = $scope.contactID;
                        $scope.hpi.HPIID = 0;
                        return presentIllnessService.addHPI($scope.hpi).then(function (data) {
                            if (data.data.ResultCode === 0) {
                                alertService.success('HPI bundle created successfully');
                                var deferred = $q.defer();
                                var tmpNewBundleID = data.data.ID;
                                //copy any detail records into the new bundle
                                var taskArray = [];
                                if ($scope.hpiDetailsGridData.length > 0) {
                                    var tmphpiDetailsGridData = angular.copy($scope.hpiDetailsGridData);
                                    var loopIndex = 0;
                                    angular.forEach(tmphpiDetailsGridData, function (item) {
                                        if (loopIndex === 0) {
                                            $scope.hasItemBeenEdited = false;
                                        }

                                        item.HPIID = tmpNewBundleID;
                                        //save all detail records in this loop

                                        //section to catch if the edit in this portion of the recursive calls is the one being edited
                                        if (item.HPIDetailID === $scope.hpiDetails.HPIDetailID) {
                                            //item being edited
                                            item = $scope.hpiDetails;

                                            item.ContactID = $scope.contactID;
                                            item.HPIID = tmpNewBundleID;
                                            $scope.hasItemBeenEdited = true;
                                        }

                                        item.ContactID = $scope.contactID;
                                        item.HPIDetailID = 0;
                                        loopIndex++;
                                        taskArray.push([presentIllnessService.addHPIDetail, [item]]);
                                    });
                                }
                                $q.serial(taskArray).finally(function () {
                                    deferred.resolve();
                                    if ($scope.hpiDetails.HPIDetailID === 0 || $scope.hpiDetails.HPIDetailID === undefined || $scope.hpiDetails.HPIDetailID == null) {
                                        if (!$scope.hasItemBeenEdited) {
                                            $scope.hpiDetails.ContactID = $scope.contactID;
                                            $scope.hpiDetails.HPIDetailID = 0;
                                            $scope.hpiDetails.HPIID = tmpNewBundleID;
                                            if (typeof $scope.hpiDetails.HPIID === 'object') {
                                                $scope.hpiDetails.HPIID = $scope.hpiDetails.HPIID.ID;
                                            }
                                            presentIllnessService.addHPIDetail($scope.hpiDetails).then(function () {
                                                $scope.postSave(isNext);
                                            });
                                        } else {
                                            $scope.postSave(isNext);
                                        }
                                    } else {
                                        $scope.postSave(isNext);
                                    }
                                },
                                    function (error) {
                                        deferred.reject();
                                    });

                                return deferred.promise;
                            } else {
                                alertService.error('Error while adding hpi bundle');
                                return;
                            }
                        });
                    } else {
                        //bundle must be updated
                        $scope.hpi.HPIID = $scope.currentHPIBundleID;
                        $scope.hpi.ReviewedNoChanges = false; //control still needs to be implemented

                        return presentIllnessService.updateHPI($scope.hpi).then(function (response) {
                            if (response.data.ResultCode === 0) {
                                alertService.success('HPI bundle updated successfully');
                                //add or update a detail record?
                                $scope.hpiDetails.HPIID = $scope.currentHPIBundleID;
                                if (typeof $scope.hpiDetails.HPIID === 'object') {
                                    $scope.hpiDetails.HPIID = $scope.hpiDetails.HPIID.ID;
                                }
                                $scope.hpiDetails.ContactID = $scope.contactID;
                                if ($scope.hpiDetails.HPIDetailID !== null && $scope.hpiDetails.HPIDetailID !== undefined && $scope.hpiDetails.HPIDetailID > 0) {
                                    //update detail record
                                    presentIllnessService.updateHPIDetail($scope.hpiDetails).then(function () {
                                        $scope.postSave(isNext);
                                    });
                                } else {
                                    //add detail record
                                    $scope.hpiDetails.ContactID = $scope.contactID;
                                    $scope.hpiDetails.HPIDetailID = 0;
                                    presentIllnessService.addHPIDetail($scope.hpiDetails).then(function () {
                                        $scope.postSave(isNext);
                                    });
                                }
                            }
                        });
                    }
                }
            };

            $scope.postSave = function (isNext) {
                $scope.$parent.getPatientProfileData();
                if (isNext) {
                    var nextState = angular.element("li[data-state-name].list-group-item.active").next("li[data-state-name].list-group-item");
                    if (nextState.length === 0)
                        $scope.Goto('^');
                    else {
                        $timeout(function () {
                            $rootScope.setform(false);
                            nextState.find('a').trigger('click');
                        });
                    }
                } else {
                    $scope.takenOnDetail = {};
                    $scope.initHPI();
                    $scope.initHPIDetails();
                    $scope.initTakenDetails();
                    $scope.get();
                }
            };

            $scope.rowEdit = function (row) {
                $scope.hpi.HPIID = row.HPIID;
                var isDirty = formService.isDirty();
                if (isDirty) {
                    bootbox.dialog({
                        message: "You have unsaved data. What would you like to do?",
                        buttons: {
                            success: {
                                label: "SAVE",
                                className: "btn-success",
                                callback: function () {
                                    $rootScope.safeSubmit(false, true);
                                    $('#presentIllnessTable' + " tr.success").removeClass('success');
                                }
                            },
                            danger: {
                                label: "DISCARD",
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

            $scope.prepRowEditData = function (row) {
                //detail model
                $scope.hpiDetails = angular.copy(row);

                resetForm();
                $scope.$digest();
            };

            function getText(value, list) {
                if (value) {
                    var formattedValue = lookupService.getSelectedTextById(value, list);
                    if (formattedValue != undefined && formattedValue.length > 0) {
                        return formattedValue[0].Name;
                    } else {
                        return '';
                    }
                } else {
                    return '';
                }
            };

            $scope.$on('showDetails', function (event, args) {
                $scope.contactID = $stateParams.ContactID;
                $scope.get().then(function () {
                    setGridItem($scope.hpiTable, 'HPIDetailID', args.id);
                });
            });

            $scope.init();
        }
    ]);