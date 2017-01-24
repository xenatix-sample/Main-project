angular.module('xenatixApp')
    .controller('vitalController', [
        '$scope', '$filter', 'vitalService', 'alertService', 'lookupService', '$stateParams', '$state', 'formService', 'registrationService', 'navigationService', '$timeout',
        function ($scope, $filter, vitalService, alertService, lookupService, $stateParams, $state, formService, registrationService, navigationService, $timeout) {
            $scope.isLoading = true;
            $scope.gender = 0;
            $scope.LMPOpened = false;
            $scope.VitalsOpened = false;
            $scope.dateOptions = {
                formatYear: 'yy',
                startingDay: 1,
                showWeeks: false
            };
            $scope.endDate = new Date();
            $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'MM/dd/yyyy'];
            $scope.format = $scope.formats[3];
            $scope.ContactID = $stateParams.ContactID;
            $scope.controlsVisible = true;
            $scope.chartModelData = {};
            $scope.defaultTakenById = 0;

            $scope.resetForm = function () {
                $scope.formReset($scope.ctrl.vitalForm);
            };

            var vitalsTable = $("#vitalsTable");

            $scope.initializeBootstrapTable = function () {

                $scope.tableoptions = {
                    pagination: true,
                    pageSize: 10,
                    pageList: [10, 25, 50, 100],
                    search: false,
                    showColumns: true,
                    data: [],
                    onClickRow: function (e, row, $element) {
                        if (e.VitalID != undefined)
                            $scope.editVital(e.VitalID);
                    },
                    undefinedText: '',
                    columns: [
                        {
                            field: 'TakenTime',
                            title: 'Date',
                            formatter: function (value, row, index) {
                                if (value) {
                                    return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY', 'useLocal');
                                } else
                                    return '';
                            }
                        },
                        {
                            field: 'TakenTime',
                            title: 'Time',
                            formatter: function (value, row, index) {
                                if (value) {
                                    return $filter('toMMDDYYYYDate')(value, 'hh:mm A', 'useLocal');
                                } else
                                    return '';
                            }
                        },
                        {
                            field: "TakenBy",
                            title: "Taken By",
                            formatter: function (value, row, index) {
                                if (value) {
                                    return $filter('filter')($scope.getLookupsByType('Users'), { ID: value })[0].Name;
                                } else return '';
                            }
                        },
                        {
                            field: 'Systolic',
                            title: 'BP',
                            formatter: function (value, row, index) {
                                if (value) {
                                    return row.Systolic + ' / ' + row.Diastolic;
                                } else return '';
                            }
                        },
                        {
                            field: 'Pulse',
                            title: 'Pulse'
                        },
                        {
                            field: 'Temperature',
                            title: 'Temp'
                        },
                        {
                            field: 'OxygenSaturation',
                            title: 'SpO2'
                        },
                        {
                            field: 'RespiratoryRate',
                            title: 'RR'
                        },
                        {
                            field: 'Glucose',
                            title: 'Glucose'
                        },
                        {
                            field: 'VitalID',
                            title: '',
                            formatter: function (value, row, index) {
                                return '<span class="text-nowrap"><a href="javascript:void(0)" data-default-action alt="Edit Vitals" title="Edit" security permission-key="Clinical-Vitals-Vitals" permission="update" space-key-press><i class="fa fa-pencil fa-fw" /></a>' +
                                    '<a href="javascript:void(0)" data-default-no-action ng-click="remove(' + value + ',$event)" id="deactivateVital" name="deactivateVital"security permission-key="Clinical-Vitals-Vitals" permission="delete" title="Deactivate" space-key-press><i class="fa fa-trash fa-fw"></i></a></span>';
                            }
                        }
                    ]
                };
            };

            $scope.editVital = function (vitalId) {
                $scope.vital.VitalID = vitalId;
                var row = $filter('filter')($scope.vitalList, { VitalID: vitalId })[0];
                if (formService.isDirty()) {
                    bootbox.dialog({
                        message: "You have unsaved data. What would you like to do?",
                        buttons: {
                            save: {
                                label: "SAVE",
                                className: "btn-success",
                                callback: function () {
                                    $scope.safeSubmit(false, true, true);
                                    $scope.setFields(row);
                                    $scope.$parent['autoFocus'] = true;
                                }
                            },
                            discard: {
                                label: "DISCARD",
                                className: "btn-danger",
                                callback: function () {
                                    $scope.setFields(row);
                                    $scope.$parent['autoFocus'] = true;
                                }
                            }
                        }
                    });
                } else {
                    $scope.setFields(row);
                    $scope.$parent['autoFocus'] = true;
                }
            }

            $scope.setFields = function (e) {
                $scope.vital = angular.copy(e);
                $scope.takenOnDetail = {
                    TakenBy: $scope.vital.TakenBy,
                    TakenOnDate: $filter('toMMDDYYYYDate')($scope.vital.TakenTime, 'MM/DD/YYYY', 'useLocal'),
                    TakenOnTime: $filter('toMMDDYYYYDate')($scope.vital.TakenTime, 'hh:mm A', 'useLocal')
                };
                $scope.resetForm();
            };

            $scope.initVital = function () {
                $scope.vital = {};
                $scope.vital.VitalID = 0;
                $scope.takenOnDetail.TakenBy = $scope.defaultTakenById;
                $scope.takenOnDetail.TakenOnDate = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal');
                $scope.takenOnDetail.TakenOnTime = $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal');
                $scope.$parent['autoFocus'] = true;
                $scope.resetForm();
            };

            $scope.init = function () {
                $scope.takenOnDetail = {}
                $scope.takenOnDetail.TakenOnDate = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
                $scope.takenOnDetail.TakenOnTime = $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal')
                $scope.BPMethods = $scope.getLookupsByType('BPMethod');
                $scope.vitalList = [];
                $scope.initTakenById().then(function () {
                    $scope.initVital();
                });
                $scope.initializeBootstrapTable();
                if ($state.current.name.toLowerCase().indexOf('patientprofile') >= 0) {
                    $scope.controlsVisible = false;
                    $scope.enterKeyStop = true;
                    $scope.stopNext = false;
                    $scope.saveOnEnter = true;
                } else {
                    $scope.controlsVisible = true;
                    $scope.enterKeyStop = false;
                    $scope.stopNext = false;
                    $scope.saveOnEnter = false;
                }
                $scope.getContactDetails($scope.ContactID);
                $scope.get($scope.ContactID);
                $scope.$watchGroup(['vital.HeightFeet', 'vital.HeightInches', 'vital.WeightLbs', 'vital.WeightOz'], function () {
                    if ($scope.ctrl.vitalForm.heightFeet.$valid && $scope.ctrl.vitalForm.heightInches.$valid && $scope.ctrl.vitalForm.weightLbs.$valid && $scope.ctrl.vitalForm.weightOz.$valid) {
                        $scope.vital.BMI = ((($scope.vital.WeightLbs + ($scope.vital.WeightOz / 16.0)) * 703) / Math.pow(($scope.vital.HeightFeet * 12) + $scope.vital.HeightInches, 2)).toFixed(1);
                    } else if ($scope.vital)
                        $scope.vital.BMI = '--';
                });
                $('#takenTime').timepicker({
                    minuteStep: 1,
                    showInputs: false,
                    //disableFocus: true
                });
            };

            $scope.initTakenById = function () {
                return navigationService.get().then(function (data) {
                    $scope.defaultTakenById = data.DataItems[0].UserID;
                });
            };

            $scope.makeChartModel = function () {
                $scope.chartModelData = {
                    labels: $scope.vitalList.map(function (vital) { return $filter('toMMDDYYYYDate')(vital.TakenTime, 'MM/DD/YYYY hh:mm A', 'useLocal'); }),
                    datasets: [
                        {
                            label: 'HT',
                            data: $scope.vitalList.map(function (vital) { return (parseInt(vital.HeightFeet) * 12) + parseInt(vital.HeightInches); })
                        },
                        {
                            label: 'WT',
                            data: $scope.vitalList.map(function (vital) { return parseFloat(vital.WeightLbs) + (parseInt(vital.WeightOz) / 16.0); })
                        },
                        {
                            label: 'BMI',
                            data: $scope.vitalList.map(function (vital) { return vital.BMI; })
                        },
                        {
                            label: 'SYS',
                            data: $scope.vitalList.map(function (vital) { return vital.Systolic; })
                        },
                        {
                            label: 'DIA',
                            data: $scope.vitalList.map(function (vital) { return vital.Diastolic; })
                        },
                        {
                            label: 'SpO2',
                            data: $scope.vitalList.map(function (vital) { return vital.OxygenSaturation || 0; })
                        },
                        {
                            label: 'RR',
                            data: $scope.vitalList.map(function (vital) { return vital.RespiratoryRate || 0; })
                        },
                        {
                            label: 'PULSE',
                            data: $scope.vitalList.map(function (vital) { return vital.Pulse || 0; })
                        },
                        {
                            label: 'TEMP',
                            data: $scope.vitalList.map(function (vital) { return vital.Temperature || 0; })
                        },
                        {
                            label: 'GLUC',
                            data: $scope.vitalList.map(function (vital) { return vital.Glucose || 0; })
                        },
                        {
                            label: 'WAIST',
                            data: $scope.vitalList.map(function (vital) { return vital.WaistCircumference || 0; })
                        }
                    ]
                };
            };

            $scope.openChartModal = function () {
                $scope.makeChartModel();
            };

            $scope.closeChartModal = function () {
                angular.element('#vitalsChartModal').modal('hide');
            };

            $scope.get = function (contactId) {
                $scope.isLoading = true;

                return vitalService.getContactVital(contactId).then(function (data) {
                    $scope.vitalList = data.DataItems || [];
                    if ($scope.vitalList.length > 0) {
                        vitalsTable.bootstrapTable('load', $scope.vitalList);
                    } else {
                        vitalsTable.bootstrapTable('removeAll');
                    }
                    angular.element("a[data-target=#vitalsChartModal]").prependTo('form .bootstrap-table > .fixed-table-toolbar');

                    $scope.makeChartModel();

                    applyDropupOnGrid(false);
                    $scope.resetForm();
                },
                    function (errorStatus) {
                        alertService.error('Unable to connect to server');
                    }).finally(function () {
                        $scope.isLoading = false;
                    });
            };


            $scope.getContactDetails = function (contactId) {
                registrationService.get(contactId).then(function (contactDemographic) {
                    if ((contactDemographic.DataItems != null) && (contactDemographic.DataItems.length === 1)) {
                        $scope.gender = contactDemographic.DataItems[0].GenderID;
                    }
                });
            }

            $scope.add = function (isNext, modelToSave) {
                $scope.isLoading = true;
                $scope.ctrl.vitalForm.$setPristine();
                vitalService.add(modelToSave)
                    .then(
                        function (response) {
                            var data = response.data;
                            $scope.initVital();
                            $scope.get($scope.ContactID).then(function () {
                                alertService.success('Vital has been added.');
                                if (isNext)
                                    $scope.next();
                            });
                        },
                        function (errorStatus) {
                            $scope.isLoading = false;
                            alertService.error('OOPS! Something went wrong');
                        },
                        function (notification) {
                            alertService.warning(notification);
                        }).then(function () {
                            $scope.isLoading = false;
                        });
            };

            $scope.update = function (isNext, modelToSave) {
                $scope.isLoading = true;
                vitalService.update(modelToSave)
                    .then(
                        function (response) {
                            var data = response.data;
                            $scope.initVital();
                            $scope.get($scope.ContactID).then(function () {
                                alertService.success('Vital has been updated.');
                                if (isNext)
                                    $scope.next();
                            }
                            );
                        },
                        function (errorStatus) {
                            $scope.isLoading = false;
                            alertService.error('OOPS! Something went wrong');
                        },
                        function (notification) {
                            alertService.warning(notification);
                        }).then(function () {
                            $scope.isLoading = false;
                        });
            };


            $scope.remove = function(id) {
                bootbox.confirm("Selected Vital will be deactivated.\n Do you want to continue?", function(result) {
                    if (result == true) {
                        vitalService.remove($scope.ContactID, id).then(function() {
                                $scope.get($scope.ContactID).then(function() {
                                    alertService.success('Vital has been deactivated.');
                                    $scope.initVital();
                                });
                            },
                            function(errorStatus) {
                                alertService.error('OOPS! Something went wrong');
                            }).then(function() {
                            
                        });
                    }
                });
            };

            $scope.validateVitalFutureDate = function () {
                if ($scope.vital != undefined && $scope.vital.LMP != null && $scope.vital.LMP !== '') {
                    var date = new Date($scope.vital.LMP);
                    if (date <= $scope.endDate) {
                        $('#lmpErrortd').removeClass('has-error');
                        $('#lmpError').addClass('ng-hide');
                        $scope.ctrl.vitalForm.$invalid = false;
                        $scope.ctrl.vitalForm.$valid = true;
                    }
                    else {
                        $('#lmpError').removeClass('ng-hide');
                        $('#lmpErrortd').addClass('has-error');
                        $scope.ctrl.vitalForm.$invalid = true;
                        $scope.ctrl.vitalForm.$valid = false;
                    }
                }
            };

            $scope.cancel = function () {
                bootbox.confirm("You will lose the information entered.\n Do you want to continue?", function (result) {
                    if (result == true) {
                        $scope.initVital();
                        $scope.$apply();
                    }
                });
            };

            $scope.save = function (isNext, mandatory, hasErrors) {
                // if there are errors, the screen is optional, and the user is trying to go to the next screen, don't bother trying to save anything
                // Vipul Singhal - As per Json if user modified the optional screen and click enter then it should display the validation error 
                // and if user don't don any modification then user can move to next screen.
                if (!mandatory && isNext && hasErrors) {
                    $scope.next();
                }

                var isDirty = formService.isDirty();

                if (isDirty && !hasErrors) {

                    var modelToSave = angular.copy($scope.vital);

                    modelToSave.ContactID = $scope.ContactID;
                    modelToSave.TakenBy = $scope.takenOnDetail.TakenBy;

                    var datePart = $filter('formatDate')($scope.takenOnDetail.TakenOnDate);
                    var dateTime = $filter('toMMDDYYYYDate')(datePart + ' ' + $scope.takenOnDetail.TakenOnTime, 'MM/DD/YYYY HH:mm');

                    modelToSave.TakenTime = new Date(dateTime);

                    if (modelToSave.VitalID !== undefined && modelToSave.VitalID !== 0) {
                        $scope.update(isNext, modelToSave);
                    } else {
                        $scope.add(isNext, modelToSave);
                    }
                } else if (!isDirty && isNext) {
                    $scope.next();
                }
            };

            $scope.getLookupsByType = function (typeName) {
                return lookupService.getLookupsByType(typeName);
            };

            $scope.next = function () {
                var nextState = angular.element("li[data-state-name].list-group-item.active").next("li[data-state-name].list-group-item");
                if (nextState.length === 0)
                    $scope.Goto('^');
                else {
                    $timeout(function() {
                        nextState.find('a').trigger('click');
                    });
                }
            };

            $scope.$on('showDetails', function (event, args) {
                $scope.get($scope.ContactID).then(function () {
                    setGridItem(vitalsTable, 'VitalID', args.id);
                });
            });

            $scope.init();
        }
    ]);
