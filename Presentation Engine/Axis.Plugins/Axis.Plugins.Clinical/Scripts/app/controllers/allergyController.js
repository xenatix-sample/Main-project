angular.module('xenatixApp')
.controller('allergyController', [
    '$scope', '$filter', 'allergyService', 'alertService', 'lookupService', 'navigationService', '$stateParams', '$state', '$rootScope', 'formService', '$q', '$timeout', 'drugService',
    function ($scope, $filter, allergyService, alertService, lookupService, navigationService, $stateParams, $state, $rootScope, formService, $q, $timeout, drugService) {

        $scope.init = function () {
            $scope.$parent['autoFocus'] = true;
            $scope.isTakenDateDisabled = true;
            $scope.isSymptomsChanged = false;
            $scope.contactID = $stateParams.ContactID;
            $scope.initTakenDetails();
            $scope.initAllergyType();
            $scope.initialAllergyBundleID = 0;
            $scope.currentAllergyBundleID = 0;
            $scope.initLookups();
            $scope.initAllergy();
            $scope.initAllergyDetails();
            $scope.allergyTable = $('#allergyTable');
            $scope.initializeBootstrapTable();
            $scope.allergyDetails = { ContactAllergyID: 0 };
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
            //offCanvasNav.init();
        };

        var resetForm = function () {
            $rootScope.formReset($scope.ctrl.allergyForm);
            $scope.isSymptomsChanged = false;
        };

        $scope.initAllergyType = function () {
            $scope.allergyTypeID = $stateParams.AllergyTypeID;

            if ($scope.allergyTypeID === 1) {
                $scope.allergyLabel = 'Allergy';
                $scope.allergyLookupName = 'Allergy';
            } else if ($scope.allergyTypeID === 2) {
                $scope.allergyLabel = 'Drug Allergy';
                $scope.allergyLookupName = 'Drug';
            } else {
                $scope.allergyLabel = 'Drug Intolerance';
                $scope.allergyLookupName = 'Drug';
            }
        };

        $scope.initLookups = function () {
            $scope.AllergyList = lookupService.getLookupsByType('Allergy');
            $scope.DrugList = drugService.getDrugData();
            $scope.AllergySymptomList = lookupService.getLookupsByType('AllergySymptom');
            $scope.AllergySeverityList = lookupService.getLookupsByType('AllergySeverity');
            $scope.UserList = lookupService.getLookupsByType('Users');
        };

        $scope.initAllergy = function () {
            $scope.allergy = {};
            $scope.initSymptoms();
        };

        $scope.initAllergyDetails = function () {
            $scope.allergyDetails = {};
            $scope.allergyDetailsGridData = {};
        };

        $scope.initSymptoms = function () {
            $scope.AllergySymptoms = [{ AllergySymptomID: 0, Name: '' }];//object used to reset typeahead
            $scope.selectedSymptoms = [{ AllergySymptomID: 0, Name: '' }];//holds the current pills displayed
            $scope.isSymptomsChanged = false;
        };

        $scope.initTakenDetails = function () {
            $scope.endDate = new Date();
            $scope.takenOnDetail = {};
            $scope.takenOnDetail.TakenOnDate = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal');
            $scope.takenOnDetail.TakenOnTime = $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal');
            navigationService.get().then(function (data) {
                $scope.CurrentUserID = data.DataItems[0].UserID;
                $scope.takenOnDetail.TakenBy = $scope.CurrentUserID;
                resetForm();
            });
        };

        $scope.validateAllergy = function (allergy) {
            //if not in edit mode or in edit mode(but don't verify against id being edited)
            var editedAllergyID = -1;
            var tmpAllergyDetailsGridData = angular.copy($scope.allergyDetailsGridData);
            if (Object.prototype.toString.call(tmpAllergyDetailsGridData) === '[object Array]') {
                var existingAllergy = $filter('filter')(tmpAllergyDetailsGridData, { AllergyID: allergy.ID }, true);

                if ($scope.allergyDetails.ContactAllergyDetailID !== undefined && $scope.allergyDetails.ContactAllergyDetailID !== null) {
                    var tmpEditRecord = $filter('filter')(tmpAllergyDetailsGridData, { ContactAllergyDetailID: $scope.allergyDetails.ContactAllergyDetailID });
                    editedAllergyID = tmpEditRecord[0].AllergyID;
                    if (existingAllergy.length > 0 && existingAllergy[0].AllergyID !== editedAllergyID) {
                        $scope.allergyDetails.AllergyModel = '';
                        alertService.error('Allergy has already been added');
                    }
                }
                else {
                    if (existingAllergy.length > 0) {
                        $scope.allergyDetails.AllergyModel = '';
                        alertService.error('Allergy has already been added');
                    }
                }

            }
        };

        $scope.selectSymptom = function (item) {
            var idx = -1;
            for (var i = 0; i < $scope.selectedSymptoms.length; i++) {
                if ($scope.selectedSymptoms[i].AllergySymptomID === item.ID) {
                    idx = i;
                }
            }
            if (idx === -1) {
                $scope.selectedSymptoms.push({ AllergySymptomID: item.ID, Name: item.Name });
                $scope.isSymptomsChanged = true;
            }

            //Clear the typeahead
            $scope.AllergySymptoms = [{ AllergySymptomID: 0, Name: '' }];
        };

        $scope.removeSymptom = function (symptom) {
            var idx = -1;
            for (var i = 0; i < $scope.selectedSymptoms.length; i++) {
                if ($scope.selectedSymptoms[i].AllergySymptomID === symptom.AllergySymptomID) {
                    idx = i;
                }
            }
            if (idx > -1) {
                $scope.selectedSymptoms.splice(idx, 1);
                $scope.isSymptomsChanged = true;
            }
        };

        $scope.get = function () {
            var dfd = $q.defer();

            allergyService.getAllergyBundle($scope.contactID, $scope.allergyTypeID).then(function (bundleData) {
                if (bundleData.ResultCode === 0) {
                    if (bundleData.DataItems.length > 0) {
                        //filter list to current allergytypeid for offline use
                        var bundlesDataByType = $filter('filter')(bundleData.DataItems, { AllergyTypeID: $scope.allergyTypeID });
                        var bundleModels = $filter('orderBy')(bundlesDataByType, 'TakenTime', true);
                        var bundleModel = null;
                        if (bundleModels.length > 1) {
                            bundleModel = $filter('orderBy')(bundlesDataByType, 'ModifiedOn', true)[0];
                        } else {
                            bundleModel = bundleModels[0];
                        }
                        if (bundleModel !== null && bundleModel !== undefined) {
                            $scope.allergyBundle = bundleModel;
                            $scope.takenBy = bundleModel.TakenBy;
                            $scope.takenOn = bundleModel.TakenTime;
                            $scope.allergy.NoKnownAllergy = bundleModel.NoKnownAllergy;
                            $scope.currentAllergyBundleID = bundleModel.ContactAllergyID;
                            if ($scope.initialAllergyBundleID === 0) {
                                $scope.initialAllergyBundleID = bundleModel.ContactAllergyID;
                            }
                            allergyService.getAllergyDetails($scope.contactID, bundleModel.ContactAllergyID, $scope.allergyTypeID).then(function (data) {
                                if (data.ResultCode === 0) {
                                    $scope.allergyDetailsGridData = data.DataItems;
                                    angular.forEach($scope.allergyDetailsGridData, function (item) {
                                        item.AllergyModel = item.AllergyID;
                                    })
                                    if (data.DataItems != undefined && data.DataItems.length > 0) {
                                        $scope.allergyTable.bootstrapTable('load', $scope.allergyDetailsGridData);
                                    } else {
                                        $scope.allergyTable.bootstrapTable('removeAll');
                                    }
                                    resetForm();
                                    dfd.resolve(true);
                                } else {
                                    alertService.error('Error while loading allergy details');
                                    dfd.reject(false);
                                }
                            });
                        }
                    }
                    applyDropupOnGrid(false);
                } else {
                    alertService.error('Error while getting allergy bundle');
                    dfd.reject(false);
                }
            },
          function (errorStatus) {
              alertService.error('Error while getting allergy bundle: ' + errorStatus);
              dfd.reject(errorStatus);
          });

            return dfd.promise;
        };

        $scope.delete = function (contactAllergyDetailID, event) {
            if (event !== null && event !== undefined) {
                event.stopPropagation();
            }

            bootbox.confirm('Are you sure you want to deactivate this allergy?', function (confirmed) {
                if (confirmed) {
                    //always create new bundle with current taken details, copy all detail records, then make deletion on copied detail id
                    $scope.allergy.TakenTime = new Date();
                    $scope.allergy.TakenBy = $scope.CurrentUserID;

                    $scope.allergy.ContactID = $scope.contactID;
                    $scope.allergy.ContactAllergyID = 0;
                    $scope.allergy.AllergyTypeID = $scope.allergyTypeID;
                    $scope.allergy.ReviewedNoChanges = false; //control still needs to be implemented
                    return allergyService.addAllergy($scope.allergy).then(function (data) {
                        if (data.data.ResultCode === 0) {
                            var tmpNewBundleID = data.data.ID;
                            //copy any detail records into the new bundle
                            var deferred = $q.defer();
                            var taskArray = [];
                            if ($scope.allergyDetailsGridData.length > 0) {
                                var tmpAllergyDetailsGridData = angular.copy($scope.allergyDetailsGridData);
                                angular.forEach(tmpAllergyDetailsGridData, function (item) {
                                    //only add the items that are not being deleted to the new bundle
                                    if (item.ContactAllergyDetailID !== contactAllergyDetailID) {
                                        if (typeof item.AllergyModel === 'object') {
                                            item.AllergyID = item.AllergyModel.ID;
                                        }
                                        item.ContactAllergyID = tmpNewBundleID;
                                        item.AllergyTypeID = $scope.allergyTypeID;
                                        //save all detail records in this loop
                                        item.ContactID = $scope.contactID;
                                        item.ContactAllergyDetailID = 0;
                                        taskArray.push([allergyService.addAllergyDetail, [item]]);
                                    }
                                });
                            }
                            $q.serial(taskArray).finally(function () {
                                deferred.resolve();
                                alertService.success('Allergy deactivated successfully');
                                $scope.postSave(false);
                            },
                            function (error) {
                                deferred.reject();
                            });

                            return deferred.promise;
                        } else {
                            alertService.error('Error while adding allergy bundle');
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

            if (formService.isDirty() && !hasErrors || $scope.isSymptomsChanged) {
                $scope.populateSymptoms();

                var datePart = $filter('toMMDDYYYYDate')($scope.takenOnDetail.TakenOnDate, 'MM/DD/YYYY');
                var dateTime = $filter('toMMDDYYYYDate')(datePart + ' ' + $scope.takenOnDetail.TakenOnTime, 'MM/DD/YYYY HH:mm');

                $scope.allergy.TakenTime = new Date(dateTime);

                $scope.allergy.ContactID = $scope.contactID;
                $scope.allergy.TakenBy = $scope.takenOnDetail.TakenBy;

                if ($scope.allergy.NoKnownAllergy !== true) {
                    if ($scope.allergyDetails.Symptoms.length > 0) {
                        if ($scope.currentAllergyBundleID === $scope.initialAllergyBundleID) {
                            //new bundle must be created
                            $scope.allergy.ContactID = $scope.contactID;
                            $scope.allergy.ContactAllergyID = 0;
                            $scope.allergy.AllergyTypeID = $scope.allergyTypeID;
                            $scope.allergy.ReviewedNoChanges = false; //control still needs to be implemented
                            return allergyService.addAllergy($scope.allergy).then(function (data) {
                                if (data.data.ResultCode === 0) {
                                    alertService.success('Allergy bundle created successfully');
                                    var deferred = $q.defer();
                                    var tmpNewBundleID = data.data.ID;
                                    //copy any detail records into the new bundle
                                    var taskArray = [];
                                    if ($scope.allergyDetailsGridData.length > 0) {
                                        var tmpAllergyDetailsGridData = angular.copy($scope.allergyDetailsGridData);
                                        var loopIndex = 0;
                                        angular.forEach(tmpAllergyDetailsGridData, function (item) {
                                            if (loopIndex === 0) {
                                                $scope.hasItemBeenEdited = false;
                                            }

                                            if (typeof item.AllergyModel === 'object') {
                                                item.AllergyID = item.AllergyModel.ID;
                                            }
                                            item.ContactAllergyID = tmpNewBundleID;
                                            item.AllergyTypeID = $scope.allergyTypeID;
                                            //save all detail records in this loop

                                            //section to catch if the edit in this portion of the recursive calls is the one being edited
                                            if (item.ContactAllergyDetailID === $scope.allergyDetails.ContactAllergyDetailID) {
                                                //item being edited
                                                item = $scope.allergyDetails;
                                                if (typeof item.AllergyModel === 'object') {
                                                    item.AllergyID = item.AllergyModel.ID;
                                                }
                                                item.ContactID = $scope.contactID;
                                                item.ContactAllergyID = tmpNewBundleID;
                                                item.AllergyTypeID = $scope.allergyTypeID;
                                                $scope.hasItemBeenEdited = true;
                                            }

                                            item.ContactID = $scope.contactID;
                                            item.ContactAllergyDetailID = 0;
                                            loopIndex++;
                                            taskArray.push([allergyService.addAllergyDetail, [item]]);
                                        });
                                    }
                                    $q.serial(taskArray).finally(function () {
                                        deferred.resolve();
                                        if ($scope.allergyDetails.ContactAllergyDetailID === 0 || $scope.allergyDetails.ContactAllergyDetailID === undefined || $scope.allergyDetails.ContactAllergyDetailID == null) {
                                            if (!$scope.hasItemBeenEdited) {
                                                //add detail record
                                                $scope.allergyDetails.ContactID = $scope.contactID;
                                                $scope.allergyDetails.ContactAllergyDetailID = 0;
                                                $scope.allergyDetails.ContactAllergyID = tmpNewBundleID;
                                                $scope.allergyDetails.AllergyTypeID = $scope.allergyTypeID;
                                                if (typeof $scope.allergyDetails.AllergyModel === 'object') {
                                                    $scope.allergyDetails.AllergyID = $scope.allergyDetails.AllergyModel.ID;
                                                }

                                                allergyService.addAllergyDetail($scope.allergyDetails).then(function () {
                                                    $scope.postSave(isNext);
                                                    $scope.allergyDetails.ContactAllergyID = 0;
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
                                    alertService.error('Error while adding allergy bundle');
                                    return;
                                }
                            });
                        } else {
                            //bundle must be updated
                            $scope.allergy.ContactAllergyID = $scope.currentAllergyBundleID;
                            $scope.allergy.AllergyTypeID = $scope.allergyTypeID;
                            $scope.allergy.ReviewedNoChanges = false; //control still needs to be implemented

                            return allergyService.updateAllergy($scope.allergy).then(function (response) {
                                if (response.data.ResultCode === 0) {
                                    alertService.success('Allergy bundle updated successfully');
                                    //add or update a detail record?
                                    $scope.allergyDetails.ContactAllergyID = $scope.currentAllergyBundleID;
                                    if (typeof $scope.allergyDetails.AllergyModel === 'object') {
                                        $scope.allergyDetails.AllergyID = $scope.allergyDetails.AllergyModel.ID;
                                    }
                                    $scope.allergyDetails.ContactID = $scope.contactID;
                                    if ($scope.allergyDetails.ContactAllergyDetailID !== null && $scope.allergyDetails.ContactAllergyDetailID !== undefined && $scope.allergyDetails.ContactAllergyDetailID !== 0) {
                                        //update detail record
                                        allergyService.updateAllergyDetail($scope.allergyDetails).then(function () {
                                            $scope.postSave(isNext);
                                            $scope.allergyDetails.ContactAllergyID = 0;
                                        });
                                    } else {
                                        //add detail record
                                        $scope.allergyDetails.ContactID = $scope.contactID;
                                        $scope.allergyDetails.ContactAllergyDetailID = 0;
                                        allergyService.addAllergyDetail($scope.allergyDetails).then(function () {
                                            $scope.postSave(isNext);
                                            $scope.allergyDetails.ContactAllergyID = 0;
                                        });
                                    }
                                }
                            });
                        }
                    } else {
                        alertService.error('Please add a related symptom');
                    }
                } else {
                    //save to a new bundle with no detail records

                    $scope.allergy.AllergyTypeID = $scope.allergyTypeID;
                    $scope.allergy.ReviewedNoChanges = false; //control still needs to be implemented
                    $scope.allergy.ContactID = $scope.contactID;

                    return allergyService.addAllergy($scope.allergy).then(function (data) {
                        if (data.data.ResultCode === 0) {
                            $scope.allergy.ContactAllergyID = 0;
                            alertService.success('Allergy bundle created successfully');
                        } else {
                            alertService.error('Error while saving allergy bundle');
                        }

                        $scope.postSave(isNext);

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
                $scope.initAllergy();
                $scope.initAllergyDetails();
                $scope.initTakenDetails();
                $scope.isSymptomsChanged = false;
                $scope.get();
            }
        };

        $scope.populateSymptoms = function () {
            $scope.allergyDetails.Symptoms = [];//declare property before adding any symptoms
            $filter('filter')($scope.selectedSymptoms, function (item) {
                if (item.AllergySymptomID > 0) {
                    $scope.allergyDetails.Symptoms.push(item.AllergySymptomID);
                }
            });
        };

        $scope.rowEdit = function (row) {
            $scope.allergyDetails.ContactAllergyID = row.ContactAllergyID;
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
                                $('#allergyTable' + " tr.success").removeClass('success');
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
            $scope.allergyDetails = angular.copy(row);
            if ($scope.allergyDetails.AllergyID != null) {
                var allergies = $scope.allergyTypeID === 1 ? $scope.AllergyList : $scope.DrugList;
                $scope.noResults = !(allergies);
                angular.forEach(allergies, function (item) {
                    if (item.ID === $scope.allergyDetails.AllergyID) {
                        $scope.allergyDetails.AllergyModel = item;
                    }
                });
            }

            //TakenBy Model
            $scope.takenOnDetail.TakenBy = $scope.takenBy;
            $scope.takenOnDetail.TakenOnDate = $filter('toMMDDYYYYDate')($scope.takenOn, 'MM/DD/YYYY', 'useLocal');
            $scope.takenOnDetail.TakenOnTime = $filter('toMMDDYYYYDate')($scope.takenOn, 'hh:mm A', 'useLocal');


            //symptoms model
            $scope.selectedSymptoms = [{ AllergySymptomID: 0, Name: '' }];//reset the selected symptoms object
            $filter('filter')(row.Symptoms, function (item) {
                var symptomData = $scope.AllergySymptomList[$scope.AllergySymptomList.map(function (el) { return el.ID; }).indexOf(item)];
                $scope.selectedSymptoms.push({ AllergySymptomID: symptomData.ID, Name: symptomData.Name });
            });

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

        function formatSymptoms(symptomList) {
            var symptomArr = [];
            angular.forEach(symptomList, function (item) {
                symptomArr.push(getText(item, $scope.AllergySymptomList));
            });

            return symptomArr.join();
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
                    $scope.rowEdit(e);
                },
                columns: [
                    {
                        field: 'TakenTime',
                        title: 'Taken On',
                        formatter: function (value, row) {
                            return $filter('toMMDDYYYYDate')($scope.takenOn, 'MM/DD/YYYY hh:mm A', 'useLocal');
                        }
                    },
                    {
                        field: 'TakenBy',
                        title: 'Taken By',
                        formatter: function (value, row) {
                            return getText($scope.takenBy, $scope.UserList);
                        }
                    },
                    {
                        field: 'AllergyID',
                        title: 'Allergy',
                        formatter: function (value, row) {
                            return getText(value, $scope.allergyTypeID === 1 ? $scope.AllergyList : $scope.DrugList);
                        }
                    },
                    {
                        field: 'SeverityID',
                        title: 'Severity',
                        formatter: function (value, row) {
                            return getText(value, $scope.AllergySeverityList);
                        }
                    },
                    {
                        field: 'Symptoms',
                        title: 'Related Symptoms',
                        formatter: function (value, row) {
                            return formatSymptoms(value);
                        }
                    },
                    {
                        field: 'ContactAllergyDetailID',
                        title: '',
                        formatter: function (value, row) {
                            return '<a href="javascript:void(0)" data-default-action id="editAllergyDetail" name="editAllergyDetail"' +
                                'title="Edit" ng-click="editAllergyDetail(' + value + ', $event);" space-key-press><i class="fa fa-pencil fa-fw" /></a>'
                                +
                                '<a href="javascript:void(0)" data-default-no-action ng-click="delete(' + value + ', $event)" id="deleteAllergy" name="deleteAllergy" title="Deactivate Allergy" ' +
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
            $scope.endDate = new Date();
            //$scope.startDate = new Date();
        };

        $scope.$on('showDetails', function (event, args) {
            $scope.contactID = $stateParams.ContactID;
            $scope.allergyTypeID = $stateParams.AllergyTypeID;
            $scope.get().then(function () {
                setGridItem($scope.allergyTable, 'ContactAllergyDetailID', args.id);
            });
        });
        $scope.init();
    }
]);