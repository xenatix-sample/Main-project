angular.module('xenatixApp')
    .controller('socialRelationshipController', ['$scope', '$filter', 'socialRelationshipService', 'alertService', 'lookupService', 'navigationService', '$stateParams', '$state', '$rootScope', 'formService', '$q',
        function ($scope, $filter, socialRelationshipService, alertService, lookupService, navigationService, $stateParams, $state, $rootScope, formService, $q) {
            var UserID = 0;
            $scope.init = function () {
                $scope.takenOnDetail = {
                    TakenOnDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
                    TakenOnTime: $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal')
                };
                $scope.endDate = new Date();
                $scope.$parent['autoFocus'] = true;
                $scope.ContactID = $stateParams.ContactID;
                $scope.socialRelationshipTable = $('#socialRelationshipTable');
                $scope.initializeBootstrapTable();
                setDefaultDatePickerSettings();
                $('#takenTime').timepicker({
                    minuteStep: 1,
                    showInputs: false,
                    //disableFocus: true
                });
                $scope.socialRelationship= { SocialRelationshipID: 0 };
                $scope.get();
            };

            $scope.remove = function (contactSocialRelationshipDetailID, event) {
                event.stopPropagation();
                bootbox.confirm('Are you sure you want to deactivate?', function (confirmed) {
                    if (confirmed) {
                        socialRelationshipService.remove($scope.contactID, contactSocialRelationshipDetailID).then(function (data) {
                            if (data.ResultCode === 0) {
                                alertService.success('Social Relationship has been successfully deleted.');
                                $scope.get();
                            } else {
                                alertService.error('Error while deactivating allergy');
                            }
                        });
                    }
                });
            };

            $scope.resetForm = function () {
                if ($scope != null && $scope.ctrl != null && $scope.ctrl.socialRelationshipForm != null)
                    $rootScope.formReset($scope.ctrl.socialRelationshipForm, $scope.ctrl.socialRelationshipForm.name);
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

            $scope.selectOwner = function (item) {
                $scope.socialRelationship.TakenBy = item.ID;
            };

            $scope.newSocialRelationship = function () {
                $scope.socialRelationship = {
                    SocialRelationshipID: 0,
                    SocialRelationships: '',
                    ContactID: $scope.ContactID,
                    EncounterID: null,
                    TakenBy: null,
                    TakenTime: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal')
                };
                $scope.takenOnDetail = {
                    TakenOnDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
                    TakenOnTime: $filter('toMMDDYYYYDate')(new Date(), 'hh:mm A', 'useLocal')
                };

                if (UserID <= 0) {
                    navigationService.get().then(function (data) {
                        if (data.DataItems != undefined && data.DataItems.length > 0) {
                            UserID = data.DataItems[0].UserID;
                            $scope.takenOnDetail.TakenBy = UserID;      //default taken by to the logged in user
                        }
                        $scope.resetForm();
                    },
                    function (errorStatus) {
                        $scope.resetForm();
                    });
                }
                else {
                    $scope.takenOnDetail.TakenBy = UserID;      //default taken by to the logged in user
                    $scope.resetForm();
                }
            };

            $scope.prepRowEditData = function (row) {
                $scope.socialRelationship = row;
                $scope.takenOnDetail.TakenBy = $scope.socialRelationship.TakenBy;

                $scope.takenOnDetail.TakenOnDate = $filter('toMMDDYYYYDate')($scope.socialRelationship.TakenTime, 'MM/DD/YYYY', 'useLocal');
                $scope.takenOnDetail.TakenOnTime = $filter('toMMDDYYYYDate')($scope.socialRelationship.TakenTime, 'hh:mm A', 'useLocal');

                $scope.resetForm();
            };

            $scope.rowEdit = function (row, e) {
                if (e != undefined) {
                    return;
                }
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
                                    $('#socialRelationshipTable' + " tr.success").removeClass('success');
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

            $scope.moveNextSocialRelationship = function (socialRelationshipID, event) {
                if (event != undefined) {
                    event.stopPropagation();
                }
                row = $filter('filter')($scope.socialRelationshipList, { SocialRelationshipID: socialRelationshipID })[0];
                $scope.editSocialRelationship(row);
            }

            $scope.editSocialRelationship = function (row) {
               $scope.socialRelationship.SocialRelationshipID = row.SocialRelationshipID;
                var params = {
                    socialRelationshipID: row.SocialRelationshipID
                }
                $state.go('patientprofile.chart.intake.socialrelationship.socialrelationshiphistory', params);
            }

            $scope.next = function () {
                if ($scope.socialRelationship &&
                        $scope.socialRelationship.SocialRelationshipID) {
                    $scope.editSocialRelationship($scope.socialRelationship);
                }
                else {
                    alertService.error('Please select a socialRelationship, before proceeding to the next screen');
                }
            };

            $scope.get = function () {
                $scope.isLoading = true;
                return socialRelationshipService.getSocialRelationships($scope.ContactID).then(function (data) {
                    if (data != undefined && data.DataItems != undefined) {
                        $scope.socialRelationshipList = data.DataItems;
                        $scope.socialRelationshipTable.bootstrapTable('load', $scope.socialRelationshipList);
                    } else {
                        $scope.socialRelationshipList = [];
                        $scope.socialRelationshipTable.bootstrapTable('removeAll');
                    }
                    $scope.resetForm();
                },
                    function (errorStatus) {
                        alertService.error('Unable to get socialRelationship: ' + errorStatus);
                    }).finally(function () {
                        $scope.isLoading = false;
                        $scope.newSocialRelationship();
                    });
            };

            $scope.saveSocialRelationship = function (isAdd) {
                if (isAdd) {
                    return socialRelationshipService.add($scope.socialRelationship);
                }
                else {
                    return socialRelationshipService.update($scope.socialRelationship);
                }
            };

            $scope.save = function (isNext, mandatory, hasErrors, keepForm, next) {
                if (isNext && next === undefined) {
                    next = function () { $scope.next(); }
                }
                if (!mandatory && isNext && hasErrors) {
                    next();
                }
                var isAdd = ($scope.socialRelationship.SocialRelationshipID === 0 || $scope.socialRelationship.SocialRelationshipID === undefined);
                if (!hasErrors) {
                    if (isAdd || formService.isDirty($scope.ctrl.socialRelationshipForm.name)) {
                        $scope.socialRelationship.TakenBy = $scope.takenOnDetail.TakenBy;

                        var datePart = $filter('formatDate')($scope.takenOnDetail.TakenOnDate);
                        var dateTime = $filter('toMMDDYYYYDate')(datePart + ' ' + $scope.takenOnDetail.TakenOnTime, 'MM/DD/YYYY HH:mm');

                        $scope.socialRelationship.TakenTime = new Date(dateTime);


                        $scope.saveSocialRelationship(isAdd).then(function (response) {
                            if (!((response !== undefined) && ('data' in response) && ('ResultCode' in response.data) && (response.data.ResultCode !== 0))) {
                                if (response.data.ResultCode == 0) {
                                    if ($scope.socialRelationship != undefined && $scope.socialRelationship.SocialRelationshipID != undefined) {
                                        var successMessage = 'SocialRelationship has been ' + (isAdd ? 'added' : 'updated') + ' successfully.';
                                    }
                                    $scope.socialRelationship.SocialRelationshipID = (($scope.socialRelationship !== undefined) && ($scope.socialRelationship.SocialRelationshipID !== undefined) && ($scope.socialRelationship.SocialRelationshipID != 0))
                                        ? $scope.socialRelationship.SocialRelationshipID : response.data.ID;
                                    alertService.success(successMessage);
                                    if (isNext) {
                                        next();
                                    }
                                    else {
                                        $scope.newSocialRelationship();
                                        $scope.get();
                                    }
                                }
                                else {
                                    alertService.error('Unable to save SocialRelationship');
                                }
                            }
                            else {
                                alertService.error('Unable to save SocialRelationship');
                            }
                        });
                    } else if (isNext) {
                        next();
                    }
                }
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
                            field: 'TakenBy',
                            title: 'Taken By',
                            formatter: function (value, row) {
                                return lookupService.getText('Users', value);
                            }
                        },
                        {
                            field: 'TakenTime',
                            title: 'Taken On',
                            formatter: function (value, row) {
                                if (value) {
                                    return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY hh:mm A', 'useLocal');
                                } else
                                    return '';
                            }
                        },
                        {
                            field: 'SocialRelationshipID',
                            title: '',
                            formatter: function (value, row) {
                                return '<a href="javascript:void(0)" data-default-no-action id="editSocialRelationship" name="editSocialRelationship" data-toggle="modal" data-target="#SocialRelationshipModel" ' +
                                        'ng-click="moveNextSocialRelationship(' + row.SocialRelationshipID + ',$event);" security permission-key="Clinical-SocialRelationshipHistory-SocialRelationshipHistory" permission="update" title="Edit" space-key-press><i class="fa fa-pencil fa-fw" /></a>' +
                                        '<a href="javascript:void(0)" data-default-no-action ng-click="remove(' + value + ',$event)" id="deleteSocialRelationship" name="deleteSocialRelationship" title="Delete"  security permission-key="Clinical-SocialRelationshipHistory-SocialRelationshipHistory" permission="delete"" ' +
                                        'space-key-press><i class="fa fa-trash fa-fw"></i></a>';
                            }
                        }
                    ]
                };
            };

            $scope.$on('showDetails', function (event, args) {
                $scope.get($scope.ContactID).then(function () {
                    setGridItem($scope.socialRelationship, 'SocialRelationshipID', args.id);
                });
            });

            $scope.init();

        }]);