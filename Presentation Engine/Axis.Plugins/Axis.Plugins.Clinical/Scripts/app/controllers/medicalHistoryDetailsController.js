angular.module('xenatixApp')
    .controller('medicalHistoryDetailsController', ['$scope', '$filter', 'medicalHistoryService', 'alertService', 'lookupService', '$stateParams', '$state', '$rootScope', 'formService', 'navigationService', '$timeout', 'registrationService', 'globalObjectsService',
    function ($scope, $filter, medicalHistoryService, alertService, lookupService, $stateParams, $state, $rootScope, formService, navigationService, $timeout, registrationService, globalObjectsService) {

        $scope.init = function () {
            $scope.$parent['autoFocus'] = true;
            $scope.MedicalHistoryID = $stateParams.MedicalHistoryID;
            $scope.disableReviewedChanges = $scope.MedicalHistoryID > 0 ? false : true;
            $scope.ContactID = $stateParams.ContactID;
            $scope.getContactDetails($scope.ContactID);
            $scope.initLookups();
            $scope.endDate = new Date();
            $scope.initTakenDetails();
            $scope.conditions = {};
            $scope.medicalHistory = {};
            $scope.medicalHistoriesToBeSaved = [];
            $scope.medicalHistoryDetailsTable = $('#medicalHistory');
            $scope.get();
            $scope.SelfID = 39;
            $scope.medicalHistory =
                {
                    MedicalHistoryConditionID: 0
                };
        };

        $scope.getContactDetails = function (contactId) {
            registrationService.get(contactId).then(function (contactDemographic) {
                if ((contactDemographic.DataItems != null) && (contactDemographic.DataItems.length === 1)) {
                    $scope.userFirstName = contactDemographic.DataItems[0].FirstName;
                    $scope.userLastName = contactDemographic.DataItems[0].LastName;
                }
            });
        }

        $scope.relationshipChanged = function(detail) {
            if (detail.RelationshipTypeID === $scope.SelfID) {
                detail.FirstName = $scope.userFirstName;
                detail.LastName = $scope.userLastName;
            } else {
                detail.FirstName = '';
                detail.LastName = '';
            }
        };

        $scope.initData = function () {
            $scope.initTakenDetails();
            $scope.medicalHistory = {};
            $scope.get();
            $scope.resetForm();
        };

        $scope.initLookups = function () {
            $scope.ConditionList = lookupService.getLookupsByType('MedicalCondition');
            $scope.UserList = lookupService.getLookupsByType('Users');
            $scope.FamilyRelationshipList = lookupService.getLookupsByType('FamilyRelationship');
        };

        $scope.initTakenDetails = function () {
            $scope.takenOnDetail = {
                TakenBy: 0,
                TakenOnDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
                TakenOnTime: $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal')
            };

            $scope.endDate = new Date();

            medicalHistoryService.getMedicalHistory($scope.MedicalHistoryID, $scope.ContactID).then(function (data) {
                if (data && data.DataItems) {
                    var item = data.DataItems[0];
                    $scope.takenOnDetail = {
                        TakenBy: item.TakenBy,
                        TakenOnDate: new Date($filter('toMMDDYYYYDate') (item.TakenTime, 'MM/DD/YYYY', 'useLocal')),
                        TakenOnTime: $filter('toMMDDYYYYDate') (item.TakenTime, 'hh:mm A', 'useLocal')
                    };
                }
                $scope.resetForm();
            });
        };

        $scope.getText = function (value, list) {
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

        $scope.resetForm = function () {
            $scope.formReset($scope.ctrl.medicalHistoryDetailForm);
        };

        $scope.get = function () {
            $scope.modelData = [];
            $scope.disableReviewedChanges = true;
            medicalHistoryService.getAllMedicalConditions($scope.MedicalHistoryID, $scope.ContactID).then(function (condData) {
                if (condData && condData.DataItems && condData.ResultCode === 0) {
                    if (condData.DataItems.length > 0 && condData.DataItems[0].Conditions) {
                        $scope.modelData = angular.copy(condData.DataItems[0].Conditions);
                    }
                    else if (condData.DataItems.length === 0 && condData.ResultMessage === "OFFLINE") {
                        angular.forEach($scope.ConditionList, function (resp) {
                            resp.MedicalHistoryConditionID = 0;
                            resp.MedicalHistoryID = 0;
                            resp.IsActive = true;
                            resp.MedicalConditionID = resp.ID;
                            $scope.modelData.push(resp);
                        });
                    }
                    angular.forEach($scope.modelData, function (dataCondition) {
                        if (dataCondition.Details && dataCondition.Details.length > 0 && dataCondition.HasCondition == true) {
                            //dataCondition.Details = dataCondition.Details;
                            $scope.disableReviewedChanges = false;
                            $scope.medicalHistory.MedicalHistoryConditionID = dataCondition.MedicalHistoryConditionID;
                        } else {
                            var newMedicalConditionDetail = [{
                                MedicalHistoryConditionID: dataCondition.MedicalHistoryConditionID,
                                MedicalHistoryID: dataCondition.MedicalHistoryID,
                                MedicalConditionID: dataCondition.MedicalConditionID,
                                HasCondition: false,
                                IsActive: dataCondition.IsActive,
                                RowCount: 1,
                                MedicalHistoryConditionDetailID: 0,
                                FamilyRelationshipID: 0,
                                IsSelf: false,
                                Comments: '',
                                RelationshipTypeID: null,
                                FirstName: '',
                                LastName: '',
                                IsDeceased: false,
                                IsFirst: true,
                                IsLast: true,
                                RowNumber: 0
                            }];
                            dataCondition.Details = [];
                            dataCondition.Details = angular.copy(newMedicalConditionDetail);
                        }
                    });


                    //Why are we iterating through same collection hierarichally ??

                    //angular.forEach($scope.modelData, function (modelCondition) {
                    //    if (!('MedicalConditionID' in modelCondition)) modelCondition.MedicalConditionID = 0; 
                    //    angular.forEach(condData.DataItems[0].Conditions, function (dataCondition) {
                    //        if (!('MedicalConditionID' in dataCondition)) dataCondition.MedicalConditionID = 0;

                    //        if (dataCondition.MedicalConditionID == modelCondition.MedicalConditionID) {
                    //            if (dataCondition.Details && dataCondition.Details.length > 0) {
                    //                $scope.modelData.Details = angular.copy(dataCondition.Details);
                    //                $scope.disableReviewedChanges = false;
                    //            } else {
                    //                var newMedicalConditionDetail = [{
                    //                    MedicalHistoryConditionID: dataCondition.MedicalHistoryConditionID,
                    //                    MedicalHistoryID: dataCondition.MedicalHistoryID,
                    //                    MedicalConditionID: dataCondition.MedicalConditionID,
                    //                    HasCondition: false,
                    //                    IsActive: dataCondition.IsActive,
                    //                    RowCount: 1,
                    //                    MedicalHistoryConditionDetailID: 0,
                    //                    FamilyRelationshipID: 0,
                    //                    IsSelf: false,
                    //                    Comments: '',
                    //                    RelationshipTypeID: null,
                    //                    FirstName: '',
                    //                    LastName: '',
                    //                    IsDeceased: false,
                    //                    IsFirst: true,
                    //                    IsLast: true,
                    //                    RowNumber: 0
                    //                }];
                    //                modelCondition.Details = [];
                    //                modelCondition.Details = angular.copy(newMedicalConditionDetail);
                    //            }
                    //        }
                    //    });
                    //});
                }

                $scope.resetForm();
            });
        };

        $scope.addRow = function (medicalConditionID, event) {
            globalObjectsService.setViewContent();
            if (event !== null && event !== undefined) {
                event.stopPropagation();
            }

            var idx = 0;
            var medicalHistoryAdded = false;
            angular.forEach($scope.modelData, function (medicalCondition) {
                if (!medicalHistoryAdded) {
                    if (medicalCondition.MedicalConditionID == medicalConditionID) {

                        var newMedicalConditionDetail = {
                            MedicalConditionID: medicalCondition.MedicalConditionID, HasCondition: false, MedicalHistoryID: medicalCondition.MedicalHistoryID,
                            RowCount: 1, IsActive: true, MedicalHistoryConditionDetailID: 0, FamilyRelationshipID: 0, IsSelf: false, Comments: '',
                            RelationshipTypeID: null, FirstName: '', LastName: '', IsDeceased: false, IsFirst: false, IsLast: true, RowNumber: $scope.modelData[idx].Details.length
                    };

                        $scope.modelData[idx].Details.push(newMedicalConditionDetail);
                        $scope.modelData[idx].RowCount++;
                        $scope.modelData[idx].Details[$scope.modelData[idx].Details.length - 2].IsLast = false; // -2 is always the previous last, as the add will always be on the bottom of the details
                        medicalHistoryAdded = true;
                        $scope.ctrl.medicalHistoryDetailForm.$setDirty();
                    }

                    idx++;
                }
            });

        };

        $scope.deleteRow = function (medicalConditionID, rowNumber, event) {
            globalObjectsService.setViewContent();
            if (event !== null && event !== undefined) {
                event.stopPropagation();
            }

            var idx = 0;
            angular.forEach($scope.modelData, function (medicalCondition) {
                if (medicalCondition.MedicalConditionID == medicalConditionID) {                    
                    if ($scope.modelData[idx].Details[rowNumber].MedicalHistoryConditionDetailID != 0) {
                            // this is an existing medical condition, so we need to mark it as disabled in the db
                            $scope.modelData[idx].Details[rowNumber].IsActive = false;
                        } else {
                            // we are deleting a family relationship that hasn't been saved yet
                            $scope.modelData[idx].Details.splice(rowNumber, 1);
                        }

                    // we've removed the last medical condition's row, so replace it with a new row
                    var detailsInactive = true;
                    angular.forEach($scope.modelData[idx].Details, function(detail) {
                        if (detail.IsActive) {
                            detailsInactive = false;
                        } 
                    });

                    if ($scope.modelData[idx].Details.length === 0 || detailsInactive) {
                        var newMedicalConditionDetail = {
                            MedicalConditionID: medicalCondition.MedicalConditionID, HasCondition: false, MedicalHistoryID: medicalCondition.MedicalHistoryID,
                            RowCount: 1, IsActive: true, MedicalHistoryConditionDetailID: 0, FamilyRelationshipID: 0, IsSelf: false, Comments: '',
                            RelationshipTypeID: null, FirstName: '', LastName: '', IsDeceased: false, IsFirst: true, IsLast: true, RowNumber: 0
                        };

                        $scope.modelData[idx].Details.push(newMedicalConditionDetail);
                    } else {
                        $scope.modelData[idx].Details[$scope.modelData[idx].Details.length - 1].IsLast = true;
                    }

                    var rowIndex = 0;
                    var firstFound = false;
                    angular.forEach($scope.modelData[idx].Details, function(detail) {
                        detail.RowNumber = rowIndex;
                        if (detail.IsActive && !firstFound) {
                            detail.IsFirst = true;
                            firstFound = true;
                        }
                        rowIndex++;
                    });

                    //to set model dirty for autofocus on last row
                    var tmpDetails = angular.copy($scope.modelData[idx].Details);
                    $scope.modelData[idx].Details = tmpDetails;
                    $scope.ctrl.medicalHistoryDetailForm.$setDirty();
                }

                idx++;
            });
        };

        CBClicked = function () {
            $scope.ctrl.medicalHistoryDetailForm.$setDirty();
            $scope.$digest();
        }

        $scope.save = function (isNext, mandatory, hasErrors) {
            if (!mandatory && isNext && hasErrors) {
                $scope.postSave(isNext);
            }

            if (!formService.isDirty() && isNext && !hasErrors) {
                $scope.postSave(isNext);
            }

            if ($scope.ctrl.medicalHistoryDetailForm.$dirty && !hasErrors) {
                //update bundle taken time and then send the detail records under the current bundle
                var dateVal = new Date($scope.takenOnDetail.TakenOnDate);
                var timeVal = $scope.takenOnDetail.TakenOnTime;
                var hr = timeVal.substring(0, timeVal.indexOf(':'));
                if (timeVal.substring(timeVal.indexOf(' ') +1, timeVal.length) == "PM" && hr != 12)      //checks if PM, adds 12 hours
                    hr = +hr + +12;
                var min = timeVal.substring(timeVal.indexOf(':') +1, timeVal.indexOf(' '));
                var dateTime = dateVal.setHours(hr, min);

                $scope.medicalHistory.TakenTime = new Date(dateTime);
                $scope.medicalHistory.ContactID = $stateParams.ContactID;
                $scope.medicalHistory.TakenBy = $scope.takenOnDetail.TakenBy;
                $scope.medicalHistory.MedicalHistoryID = $scope.MedicalHistoryID;

                medicalHistoryService.updateMedicalHistory($scope.medicalHistory).then(function (dataMedicalHistory) {
                    if (dataMedicalHistory && dataMedicalHistory.ResultCode && dataMedicalHistory.ResultCode === 0) {
                        //alertService.success('Medical history saved successfully');
                    }
                });

                var conditionsToSave = [];
                angular.forEach($scope.modelData, function (condition) {
                    if (condition.HasCondition) {
                        var conditionToSave = angular.copy(condition);

                        angular.forEach(condition.Details, function (detail) {
                            if (detail.RelationshipTypeID > 0 || detail.FirstName != '' || detail.LastName != '' || detail.Comment != '' || detail.IsDeceased) {
                                conditionToSave.Details.push(detail);
                            }
                        });

                        conditionsToSave.push(condition);
                    } else if (condition.MedicalHistoryConditionID > 0) {
                        conditionsToSave.push(condition);
                    }
                });

                $scope.medicalHistory.Conditions = $scope.modelData;

                medicalHistoryService.saveMedicalHistoryConditions($scope.medicalHistory).then(function (response) {
                    if (response.data.ResultCode === 0) {
                        alertService.success('Medical history saved successfully');
                        $scope.postSave(isNext);
                    }
                });
            }
        };

        $scope.postSave = function (isNext) {
            if (isNext) {
                $scope.next();
            } else {
                $scope.initData();
            }
        };

        $scope.next = function () {
            var nextState = angular.element("li[data-state-name].list-group-item.active").next("li[data-state-name].list-group-item");
            if (nextState.length === 0)
                $scope.Goto('^');
            else {
                $timeout(function () {
                    $rootScope.setform(false);
                    nextState.find('a').trigger('click');
                });
            }
        };

        $scope.reviewNoChanges = function () {
            var isDirty = formService.isDirty();
            if (isDirty) {
                bootbox.dialog({
                    message: "Your changes will be discarded. What would you like to do?",
                    buttons: {
                        success: {
                            label: "Proceed",
                            className: "btn-success",
                            callback: function () {
                                $scope.get();
                                $scope.reviewNoChangesSaveHandler();
                            }
                        },
                        danger: {
                            label: "Discard",
                            className: "btn-danger",
                            callback: function () {
                                //Do nothing
                            }
                        }
                    }
                });
            }
            else {
                $scope.get();
                $scope.reviewNoChangesSaveHandler();
            }
        }

        $scope.reviewNoChangesSaveHandler = function () {
            $scope.medicalHistory.TakenBy = $scope.CurrentUserID;
            var tmpTime = $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal');
            var datePart = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY');
            var dateTime = $filter('toMMDDYYYYDate')(datePart + ' ' + tmpTime, 'MM/DD/YYYY HH:mm');
            $scope.medicalHistory.TakenTime = new Date(dateTime);
            $scope.medicalHistory.ReviewedNoChanges = true;

            medicalHistoryService.saveMedicalHistoryConditions($scope.medicalHistory).then(function (response) {
                if (!((response !== undefined) && ('data' in response) && ('ResultCode' in response.data) && (response.data.ResultCode !== 0))) {
                    if (response.data.ResultCode === 0) {
                        $scope.medicalHistory.MedicalConditionID = (($scope.medicalHistory !== undefined) && ($scope.medicalHistory.MedicalHistoryConditionDetailID !== undefined) &&
                            ($scope.medicalHistory.MedicalHistoryConditionDetailID != 0))
                            ? $scope.medicalHistory.MedicalHistoryConditionDetailID : response.data.ID;
                        $scope.medicalHistory.medicalHistoryID = parseInt(response.data.ID);
                        //save condition records
                        var conditionsToSave = [];
                        angular.forEach($scope.modelData, function (condition) {
                            if (condition.HasCondition) {
                                var conditionToSave = angular.copy(condition);

                                angular.forEach(condition.Details, function (detail) {
                                    if (detail.RelationshipTypeID > 0 || detail.FirstName != '' || detail.LastName != '' || detail.Comment != '' || detail.IsDeceased) {
                                        conditionToSave.Details.push(detail);
                                    }
                                });

                                conditionsToSave.push(condition);
                            } else if (condition.MedicalHistoryConditionID > 0) {
                                conditionsToSave.push(condition);
                            }

                            $scope.medicalHistory.Conditions = $scope.modelData;

                            return medicalHistoryService.saveMedicalHistoryConditions($scope.medicalHistory).then(function (response) {
                                if (response.data.ResultCode === 0) {
                                    alertService.success('Medical history saved successfully');

                                    $scope.postSave(isNext);
                                }
                            });
                        });

                        var state = angular.element("li[data-state-name].list-group-item.active");
                        if (state.length === 0)
                            $scope.Goto('^');
                        else {
                            $timeout(function () {
                                $rootScope.setform(false);
                                state.find('a').trigger('click');
                            });
                        }
                    }
                    else {
                        alertService.error('Unable to save Medical History');
                    }
                }
                else {
                    alertService.error('Unable to save Medical History History');
                }
            });
        };

        $scope.init();
    }]);