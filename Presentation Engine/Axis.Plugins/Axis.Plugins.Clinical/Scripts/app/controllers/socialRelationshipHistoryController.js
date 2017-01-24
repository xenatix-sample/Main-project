angular.module('xenatixApp')
    .controller('socialRelationshipHistoryController', ['$scope', 'socialRelationshipHistoryService', '$filter', 'alertService', 'lookupService', '$stateParams', '$state', '$rootScope', '$timeout', 'formService', 'navigationService', 'socialRelationshipService',
    function ($scope, socialRelationshipHistoryService, $filter, alertService, lookupService, $stateParams, $state, $rootScope, $timeout, formService, navigationService, socialRelationshipService) {

        $scope.init = function () {
            $scope.$parent['autoFocus'] = true;
            $scope.endDate = new Date();
            $scope.ContactID = $stateParams.ContactID;
            $scope.SocialRelationshipID = $stateParams.socialRelationshipID;
            $scope.socialHistory = { SocialRelationshipID: 0 };
            $scope.disableReviewedChanges = true;
            $scope.siblingRelationshipTypeID = 32;
            $scope.setDefaultDatePickerSettings();
            $scope.historyTable = $("#historyTable");
            $scope.initializeBootstrapTable();
            
            $('#takenTime').timepicker({
                minuteStep: 1,
                showInputs: false,
                //disableFocus: true
            });
            $scope.initTakenDetails();
            $scope.isSocialRelationshipHistoryExists = false;
            $scope.getSocialRelationshipHistory().finally(function () {
                $scope.disableReviewedChanges = !$scope.isSocialRelationshipHistoryExists;
            });
            
        };

        $scope.resetForm = function () {
            if ($scope != null && $scope.ctrl != null && $scope.ctrl.socialRelationshipHistoryForm != null) {
                $rootScope.formReset($scope.ctrl.socialRelationshipHistoryForm, $scope.ctrl.socialRelationshipHistoryForm.name);
                $rootScope.formReset($scope.ctrl.socialRelationshipHistoryForm.socialRelationshipDetailsForm, $scope.ctrl.socialRelationshipHistoryForm.socialRelationshipDetailsForm.name);
            }
        };

        $scope.initTakenDetails = function () {
            $scope.endDate = new Date();
            $scope.takenOnDetail = {
                TakenOnDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
                TakenOnTime: $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal')
            };

            socialRelationshipService.getSocialRelationships($stateParams.ContactID).then(function (data) {

                if (data != null && data.DataItems != null) {
                    var socialRelationShipItem = data.DataItems.filter(function (obj) {
                        return obj.SocialRelationshipID === $stateParams.socialRelationshipID;
                    })[0];
                    $scope.CurrentUserID = socialRelationShipItem.ContactID;
                    $scope.takenOnDetail.TakenBy = socialRelationShipItem.TakenBy;
                    $scope.takenOnDetail.TakenOnDate = $filter('toMMDDYYYYDate')(socialRelationShipItem.TakenTime, 'MM/DD/YYYY', 'useLocal'),
                    $scope.takenOnDetail.TakenOnTime = $filter('toMMDDYYYYDate')(socialRelationShipItem.TakenTime, 'hh:mm A', 'useLocal');
                }
                $scope.resetForm();
            });
        };

        $scope.getLookupsByType = function (typeName) {
            return lookupService.getLookupsByType(typeName);
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

        $scope.newFamily = function () {
            $scope.socialHistoryDetail = {};
            $scope.socialHistoryDetail.FamilyRelationshipID = 0;
            $scope.socialHistoryDetail.RelationshipTypeID = $scope.siblingRelationshipTypeID;
            $scope.socialHistoryDetail.ContactID = $scope.ContactID;
            $scope.socialHistoryDetail.FirstName = null;
            $scope.socialHistoryDetail.LastName = null;
            $scope.socialHistoryDetail.IsDeceased = false;
            $scope.socialHistoryDetail.IsInvolved = false;
            $scope.socialHistoryDetail.IsDetailsDirty = false;
            $scope.socialHistoryDetail.IsSocialRelationshipDirty = false;
            $scope.socialHistoryDetail.ReviewedNoChanges = false;
            $scope.socialHistoryDetail.SocialRelationshipID = $scope.SocialRelationshipID;
            $scope.resetForm();
        };

        $scope.getSocialRelationshipHistory = function () {
            $scope.isLoading = true;
            return socialRelationshipHistoryService.get($scope.ContactID, $scope.SocialRelationshipID).then(function (data) {
                $scope.isSocialRelationshipHistoryExists = false;
                if (data.DataItems.length > 0) {
                    $scope.socialHistory = data.DataItems[0];//sets the parent level model fields                    
                    $scope.isSocialRelationshipHistoryExists = (($scope.socialHistory.ChildhoodHistory && $scope.socialHistory.ChildhoodHistory != "") ||
                        ($scope.socialHistory.RelationShipHistory && $scope.socialHistory.RelationShipHistory != "") || ($scope.socialHistory.FamilyHistory && $scope.socialHistory.FamilyHistory != ""));
                    if (!$scope.isSocialRelationshipHistoryExists)
                        $scope.disableReviewedChanges = true;
                    return socialRelationshipHistoryService.getDetails($scope.ContactID, $scope.SocialRelationshipID).then(function (detailsData) {
                        //load the bootstrap table and set the model
                        if (detailsData.DataItems.length > 0) {
                            $scope.isSocialRelationshipHistoryExists = true;
                            $scope.detailList = detailsData.DataItems;
                            $scope.historyTable.bootstrapTable('load', $scope.detailList);
                        } else {
                            $scope.socialHistory.SocialRelationshipID = 0;
                            $scope.historyTable.bootstrapTable('removeAll');
                        }
                    });
                } else {
                    $scope.socialHistory = { SocialRelationshipID: $scope.SocialRelationshipID, ContactID: $scope.ContactID };
                }
            },
            function (errorStatus) {
                alertService.error('Unable to connect to server: ' + errorStatus);
            }).finally(function () {
                $scope.isLoading = false;
                $scope.newFamily();
            });
        };

        $scope.prepRowEditData = function (row) {
            $scope.socialHistoryDetail.FamilyRelationshipID = row.FamilyRelationshipID;
            $scope.socialHistoryDetail.RelationshipTypeID = row.RelationshipTypeID;
            $scope.socialHistoryDetail.ContactID = $scope.ContactID;
            $scope.socialHistoryDetail.FirstName = row.FirstName;
            $scope.socialHistoryDetail.LastName = row.LastName;
            $scope.socialHistoryDetail.IsDeceased = row.IsDeceased;
            $scope.socialHistoryDetail.IsInvolved = row.IsInvolved;
            $scope.socialHistoryDetail.SocialRelationshipID = $scope.SocialRelationshipID;
            $scope.socialHistoryDetail.SocialRelationshipDetailID = row.SocialRelationshipDetailID;
            $scope.resetForm();
        };

        $scope.rowEdit = function (row) {
            var isDirty = formService.isDirty($scope.socialRelationshipDetailsForm.name);
            if (isDirty) {
                bootbox.dialog({
                    message: "You have unsaved data. What would you like to do?",
                    buttons: {
                        success: {
                            label: "Save",
                            className: "btn-success",
                            callback: function () {
                                $rootScope.safeSubmit(false, true, true);
                                $('#historyTable' + " tr.success").removeClass('success');
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

        $scope.editHistory = function (socialRelationshipDetailID) {
            angular.forEach($scope.detailList, function (detail) {
                if (detail.SocialRelationshipDetailID === socialRelationshipDetailID) {
                    var copiedDetailRow = angular.copy(detail);
                    $scope.rowEdit(copiedDetailRow);
                }
            });
        }

        $scope.save = function (isNext, mandatory, hasErrors, keepForm, next) {
            if (isNext && next === undefined) {
                next = function () { $scope.next(); }
            }
            if (!mandatory && isNext && hasErrors) {
                next();
            }
            if (!hasErrors) {
                if (formService.isDirty($scope.ctrl.socialRelationshipHistoryForm.name) || $scope.socialHistory.ReviewedNoChanges) {
                    $scope.socialHistory.TakenBy = $scope.takenOnDetail.TakenBy;
                    var datePart = $filter('toMMDDYYYYDate')($scope.takenOnDetail.TakenOnDate, 'MM/DD/YYYY');
                    var dateTime = $filter('toMMDDYYYYDate')(datePart + ' ' + $scope.takenOnDetail.TakenOnTime, 'MM/DD/YYYY HH:mm');
                    $scope.socialHistory.TakenTime = new Date(dateTime);
                    $scope.socialHistoryDetail.RelationshipTypeID = $scope.siblingRelationshipTypeID; //TODO: Update with correct RelationshipTypeID from lookups in the init func
                    var isAdd = ($scope.socialHistory.SocialRelationshipID === 0 || $scope.socialHistory.SocialRelationshipID === undefined);
                    $scope.saveSocialHistory(isAdd).then(function (response) {
                        if (!((response !== undefined) && ('data' in response) && ('ResultCode' in response.data) && (response.data.ResultCode !== 0))) {
                            if (response.data.ResultCode === 0) {
                                var successMessage = 'Social Relationship History has been ' + (isAdd ? 'added' : 'updated') + ' successfully.';
                                $scope.socialHistory.ReviewedNoChanges = false;
                                $scope.socialHistory.SocialRelationshipDetailID = (($scope.socialHistory !== undefined) && ($scope.socialHistory.SocialRelationshipDetailID !== undefined) && ($scope.socialHistory.SocialRelationshipDetailID != 0))
                                    ? $scope.socialHistory.SocialRelationshipDetailID : response.data.ID;
                                $scope.socialHistory.SocialRelationshipID = (($scope.socialHistory !== undefined) && ($scope.socialHistory.SocialRelationshipID !== undefined) && ($scope.socialHistory.SocialRelationShipID != 0))
                                    ? $scope.socialHistory.SocialRelationshipID : parseInt(response.data.ID);
                                alertService.success(successMessage);
                                //save detail records
                                $scope.socialHistoryDetail.SocialRelationshipID = $scope.socialHistory.SocialRelationshipID;
                                var isDetailAdd = ($scope.socialHistoryDetail.SocialRelationshipDetailID === 0 || $scope.socialHistoryDetail.SocialRelationshipDetailID === undefined);
                                $scope.saveSocialHistoryDetails(isDetailAdd).then(function () {
                                    $scope.postSave(isNext);
                                });
                            }
                            else {
                                alertService.error('Unable to save Social Relationship History');
                            }
                        }
                        else {
                            alertService.error('Unable to save Social Relationship History');
                        }
                    });
                } else if (isNext) {
                    next();
                }
            }
        };

        $scope.saveSocialHistory = function (isAdd, isReviewedNoChanges) {
            var bypassDirtyCheck = isReviewedNoChanges || false;

            if (formService.isDirty($scope.ctrl.socialRelationshipHistoryForm.name) || bypassDirtyCheck) {
                if (isAdd) {
                    return socialRelationshipHistoryService.add($scope.socialHistory);
                } else {
                    return socialRelationshipHistoryService.update($scope.socialHistory);
                }
            } else {
                return $scope.promiseNoOp();
            }
        };

        $scope.saveSocialHistoryDetails = function (isDetailAdd, isReviewedNoChanges) {
            var bypassDirtyCheck = isReviewedNoChanges || false;

            if (formService.isDirty($scope.socialRelationshipDetailsForm) || bypassDirtyCheck) {
                if (isDetailAdd) {
                    return socialRelationshipHistoryService.addDetail($scope.socialHistoryDetail);
                } else {
                    return socialRelationshipHistoryService.updateDetail($scope.socialHistoryDetail);
                }
            } else {
                return $scope.promiseNoOp();
            }
        };

        $scope.postSave = function (isNext) {
            if (isNext) {
                $scope.next();
            } else {
                $scope.newFamily();
                $scope.getSocialRelationshipHistory();
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

        $scope.remove = function (socialRelationshipDetailID, event) {
            event.stopPropagation();
            bootbox.confirm('Selected social relationship history will be removed.\n Do you want to continue?', function (confirmed) {
                if (confirmed) {
                    socialRelationshipHistoryService.remove($scope.ContactID, $scope.SocialRelationshipID, socialRelationshipDetailID).then(function (response) {
                        if (response.ResultCode === 0) {
                            alertService.success('Social Relationship History has been successfully deleted.');
                            $scope.postSave(false);
                        } else {
                            alertService.error('Unable to delete Social Relationship History.');
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
                //onClickRow: function (e, row, $element) {
                //    $scope.rowEdit(e);
                //},
                columns: [
                    {
                        field: "FirstName",
                        title: "Siblings",
                        formatter: function (value, row, index) {
                            return ((row.FirstName == null ? '' : row.FirstName) + ' ' + (row.LastName == null ? '' : row.LastName));
                        }
                    },
                    {
                        field: "IsDeceased",
                        title: "Living/Deceased",
                        formatter: function (value, row, index) {
                            return (value ? 'Deceased' : 'Living');
                        }
                    },
                    {
                        field: "IsInvolved",
                        title: "Involved in Client's Life",
                        formatter: function (value, row, index) {
                            return (value ? 'Yes' : 'No');
                        }
                    },
                    {
                        field: "SocialRelationshipDetailID",
                        title: "",
                        formatter: function (value, row, index) {
                            return (
                                '<a href="javascript:void(0)" data-default-action id="edit" name="edit"' +
                                'ng-click="editHistory(' + value + ');" security permission-key="Clinical-SocialRelationshipHistory-SocialRelationshipHistory" permission="update" title="Edit" space-key-press><i class="fa fa-pencil fa-fw" /></a>' +
                                '<a href="javascript:void(0)" data-default-no-action ng-click="remove(' + value + ', $event)" id="delete" name="delete" title="Delete" security permission-key="Clinical-SocialRelationshipHistory-SocialRelationshipHistory" permission="delete" ' +
                                'space-key-press><i class="fa fa-trash fa-fw"></i></a>'
                                );
                        }
                    }
                ]
            };
        };

        $scope.$on('showDetails', function (event, args) {
            $scope.get().then(function () {
                setGridItem($scope.historyTable, 'FamilyRelationshipID', args.id);
            });
        });

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
                                $scope.getSocialRelationshipHistory().then(function () {
                                    $scope.reviewNoChangesSaveHandler();
                                });
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
                $scope.getSocialRelationshipHistory().then(function () {
                    $scope.reviewNoChangesSaveHandler();
                });
            }
        }

        $scope.reviewNoChangesSaveHandler = function () {
            $scope.socialHistory.TakenBy = $scope.CurrentUserID;
            var tmpTime = $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal');
            var datePart = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY');
            var dateTime = $filter('toMMDDYYYYDate')(datePart + ' ' + tmpTime, 'MM/DD/YYYY HH:mm');
            $scope.socialHistory.TakenTime = new Date(dateTime);
            $scope.socialHistory.ReviewedNoChanges = true;

            $scope.saveSocialHistory(true, true).then(function (response) {
                if (!((response !== undefined) && ('data' in response) && ('ResultCode' in response.data) && (response.data.ResultCode !== 0))) {
                    if (response.data.ResultCode === 0) {
                        $scope.socialHistory.SocialRelationshipDetailID = (($scope.socialHistory !== undefined) && ($scope.socialHistory.SocialRelationshipDetailID !== undefined) && ($scope.socialHistory.SocialRelationshipDetailID != 0))
                            ? $scope.socialHistory.SocialRelationshipDetailID : response.data.ID;
                        $scope.socialHistory.SocialRelationshipID = parseInt(response.data.ID);
                        //save detail records
                        angular.forEach($scope.detailList, function (existingDetail) {
                            $scope.socialHistoryDetail = angular.copy(existingDetail);
                            $scope.socialHistoryDetail.SocialRelationshipID = $scope.socialHistory.SocialRelationshipID;
                            $scope.socialHistoryDetail.SocialRelationshipDetailID = 0;
                            $scope.socialHistoryDetail.ContactID = $scope.ContactID;
                            $scope.socialHistoryDetail.RelationshipTypeID = $scope.siblingRelationshipTypeID; //TODO: Update with correct RelationshipTypeID from lookups in the init func
                            $scope.saveSocialHistoryDetails(true, true).then(function (detailResponse) {
                                if (detailResponse.data.ResultCode !== 0) {
                                    alertService.error('Unable to save Social Relationship History detail record');
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
                        alertService.error('Unable to save Social Relationship History');
                    }
                }
                else {
                    alertService.error('Unable to save Social Relationship History');
                }
            });
        };

        $scope.init();
    }]);