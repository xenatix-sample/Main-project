angular.module('xenatixApp')
    .controller('referralFollowupController', [
        '$scope', '$state', '$q', '$stateParams', '$modal', 'referralFollowupService', 'alertService', 'lookupService', '$filter', '$rootScope', 'formService', '$timeout', 'navigationService',
        function ($scope, $state, $q, $stateParams, $modal, referralFollowupService, alertService, lookupService, $filter, $rootScope, formService, $timeout, navigationService) {
            $scope.isLoading = true;
            $scope.startDate = new Date();
            var referralFollowupTable = $("#referralFollowupTable");
            $scope.newReferralFollowup = function () {
                $scope.referralFollowup = {};
                $scope.permissionID = 0;
                $scope.FollowupProviderID = null;
                resetForm();
            };
            $scope.permissionKey = $state.current.data.permissionKey;
            $scope.followupDateName = 'followupDate';

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
                        $scope.selectRow(e, row);
                    },
                    columns: [
                         {
                             field: 'FollowupExpected',
                             title: 'Follow Up Expected',
                             formatter: function (value, row, index) {
                                 if (value)
                                     return 'Yes' ;
                                 else
                                     return 'No';

                             }
                         },
                        {
                            field: 'FollowupProviderID',
                            title: 'Provider',
                            formatter: function (value, row, index) {
                                if (value)
                                    return lookupService.getText('Users', value);
                                else
                                    return "";

                            }
                        },

                        {
                            field: 'FollowupDate',
                            title: 'Followup Date',
                            formatter: function (value, row, index) {
                                if (value)
                                    return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY', 'useLocal');
                                else
                                    return '';
                            }
                        },
                       {
                           field: 'IsAppointmentNotified',
                           title: 'Client Notified',
                           formatter: function (value, row, index) {
                               if (value)
                                   return 'Yes';
                               else
                                   return 'No';
                           }
                       },
                       {
                           field: 'ReferralOutcomeDetailID',
                           title: '',
                           formatter: function (value, row, index) {
                               return '<a ng-hide="{{isReadOnlyForm}}" href="javascript:void(0)" data-default-action id="editReferralFollowup" name="editReferralFollowup" ' + 'title="Edit" security permission-key="Referrals-Referral-FollowUpOutcome" permission="update" space-key-press><i class="fa fa-pencil fa-fw" /></a>';
                           }
                       }
                    ]
                };
            };

            var resetForm = function () {
                $rootScope.formReset($scope.ctrl.referralForm);
            };

            $scope.validateFollowupDate = function () {
                var followupDateerror = angular.element("#followupDateerror");
                var followUpDate = angular.element("#followUpDate");
                if ($scope.referralFollowup.FollowupDate != null && $scope.referralFollowup.FollowupDate !== '' && $scope.referralFollowup.FollowupDate != "Invalid date") {
                    var date = new Date($scope.referralFollowup.FollowupDate);
                    var toDay = new Date();

                    toDay.setHours(0, 0, 0, 0);
                    if (date >= toDay) {
                        followupDateerror.addClass('ng-hide');
                        followUpDate.removeClass('has-error');
                        if ($scope.ctrl.referralForm.followupDate) {
                            $scope.ctrl.referralForm.followupDate.$valid = true;
                            $scope.ctrl.referralForm.followupDate.$invalid = false;
                        }
                    }
                    else {
                        followupDateerror.removeClass('ng-hide');
                        followUpDate.addClass('has-error');

                        if ($scope.ctrl.referralForm.followupDate) {
                            $scope.ctrl.referralForm.followupDate.$valid = false;
                            $scope.ctrl.referralForm.followupDate.$invalid = true;
                        }
                        $scope.ctrl.referralForm.$valid = false;
                        $scope.ctrl.referralForm.$invalid = true;
                    }
                }
                else {
                    followupDateerror.addClass('ng-hide');
                    followUpDate.removeClass('has-error');
                }
            };
            

            $scope.getReferralFollowups = function () {
                $scope.isLoading = true;
                var deferred = $q.defer();
                referralFollowupService.getReferralFollowups($scope.ReferralHeaderID).then(function (data) {
                    if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                        $scope.referralFollowups = data.DataItems;

                        referralFollowupTable.bootstrapTable('load', $scope.referralFollowups);
                        $rootScope.$broadcast('referrals.followup', { validationState: 'valid' });

                    } else {
                        $scope.referralFollowups = [];
                        referralFollowupTable.bootstrapTable('removeAll');
                        $rootScope.$broadcast('referrals.followup', { validationState: 'warning' });

                    }
                    $scope.permissionID = 0;
                    applyDropupOnGrid(false);
                    deferred.resolve(data);
                },
               function (errorStatus) {
                   alertService.error('Unable to get referral followups: ' + errorStatus);
                   deferred.reject(status);
               }).finally(function () {
                   $scope.isLoading = false;
               });
                return deferred.promise;
            };

            $scope.get = function (referralOutcomeDetailID) {
                $scope.isLoading = true;

                return referralFollowupService.getReferralFollowup($scope.ReferralHeaderID, referralOutcomeDetailID).then(function (data) {
                    if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                        $scope.referralFollowup = data.DataItems[0];
                        resetForm();
                    } else {
                        alertService.error('Unable to get referral followup!');
                    };
                },
                    function (errorStatus) {
                        alertService.error('Unable to get referralFollowup: ' + errorStatus);
                    }).finally(function () {
                        $scope.isLoading = false;
                    });
            };

            $scope.editReferralFollowup = function (referralOutcomeDetailID, event) {
            }

            $scope.selectRow = function (e) {
                var isDirty = formService.isDirty();
                if (isDirty) {
                    bootbox.dialog({
                        message: "You have unsaved data. What would you like to do?",
                        buttons: {
                            save: {
                                label: "SAVE",
                                className: "btn-success",
                                callback: function () {
                                    $rootScope.safeSubmit(false, false, false);
                                }
                            },
                            discard: {
                                label: "DISCARD",
                                className: "btn-danger",
                                callback: function () {
                                    $scope.setFields(e);
                                }
                            }
                        }
                    });
                } else {
                    $scope.setFields(e);
                }
            }

            $scope.setFields = function (e) {
                $scope.referralFollowup = angular.copy(e);
                $scope.permissionID = $scope.referralFollowup.ReferralOutcomeDetailID;
                $scope.referralFollowup.FollowupDate = $filter('toMMDDYYYYDate')($scope.referralFollowup.FollowupDate, 'MM/DD/YYYY');
                if ($scope.referralFollowup.FollowupProviderID != null) {
                    angular.forEach($scope.providers, function (item) {
                        if (item.ID === $scope.referralFollowup.FollowupProviderID) {
                            $scope.FollowupProviderID = item;
                        }
                    });
                }
                resetForm();
            }

            $scope.saveReferralFollowup = function (isUpdate) {
                if ($scope.referralFollowup.IsAppointmentNotified == false) {
                    $scope.referralFollowup.AppointmentNotificationMethod = null;
                }
                if ($scope.FollowupProviderID)
                    $scope.referralFollowup.FollowupProviderID = $scope.FollowupProviderID.ID;
                if (!isUpdate) {
                    return referralFollowupService.addReferralFollowup($scope.referralFollowup)
                }
                else {
                    return referralFollowupService.updateReferralFollowup($scope.referralFollowup);
                }
            };

            $scope.save = function (isNext, mandatory, hasErrors, keepForm, next) {
                var isDirty = formService.isDirty();

                if (!isDirty && isNext) {
                    $scope.next();
                }
                else if (!$scope.isReadOnlyForm) {
                    if (next === undefined) {
                        next = function () {
                            $scope.next();
                        }
                    }
                    if (!mandatory && isNext && !hasErrors)
                        next();

                    if ((!mandatory && !hasErrors && !isNext) || (mandatory && !hasErrors && !isNext) || (!mandatory && !hasErrors && isNext) || (mandatory && !hasErrors && isNext)) {
                        var deferred = $q.defer();

                        $scope.referralFollowup.ReferralHeaderID = $scope.ReferralHeaderID;
                        $scope.referralFollowup.ContactID = $stateParams.ContactID;
                        var isUpdate = $scope.referralFollowup.ReferralOutcomeDetailID != undefined && $scope.referralFollowup.ReferralOutcomeDetailID !== 0;
                        $scope.saveReferralFollowup(isUpdate).then(function (response) {
                            var action = isUpdate ? 'updated' : 'added';
                            $scope.getReferralFollowups().finally(function () {
                                $scope.postSave(response, action, isNext);
                                deferred.resolve(response);
                            });
                        },
                         function (errorStatus) {
                             alertService.error('OOPS! Something went wrong');
                             deferred.reject();
                         },
                         function (notification) {
                             alertService.warning(notification);
                         });
                        return deferred.promise;
                    }
                }

            };

            $scope.postSave = function (response, action, isNext) {
                var data = response;
                if (data.ResultCode !== 0) {
                    alertService.error(data.ResultMessage);
                    return $scope.promiseNoOp();
                } else {
                    alertService.success('Referral followup has been ' + action + ' successfully.');
                    $scope.referralFollowup.ReferralOutcomeDetailID =
                        (($scope.referralFollowup !== undefined) && ($scope.referralFollowup.ReferralOutcomeDetailID !== undefined) && ($scope.referralFollowup.ReferralOutcomeDetailID != 0))
                        ? $scope.referralFollowup.ReferralOutcomeDetailID
                        : response.ID;
                    resetForm();
                    if (isNext) {
                        $scope.next();
                    }
                    else {
                        $scope.FollowupProviderID = null;
                        $scope.init();
                        resetForm();
                        return true;
                    }
                }
            };

            $scope.next = function () {
                $state.go("referrals.appointment", $stateParams);
            };

            $scope.init = function () {
                $scope.ReferralHeaderID = $stateParams.ReferralHeaderID;
                $scope.$parent['autoFocus'] = true; //for Focus
                $scope.referralFollowup = {};
                $scope.referralFollowups = [];
                $scope.initializeBootstrapTable();
                $scope.providers = lookupService.getLookupsByType("Users");
                $scope.getReferralFollowups().then(function () {
                    resetForm();
                });
                $scope.$parent.initReferralParent($scope.ReferralHeaderID, $stateParams.ContactID);
                $scope.newReferralFollowup();
                $scope.dateOptions = {
                    formatYear: 'yy',
                    startingDay: 1,
                    showWeeks: false
                };
                window.scrollTo(0, 0);
            };

            $scope.$on('showDetails', function (event, args) {
                $scope.getReferralFollowups().then(function () {
                    setGridItem(referralFollowupTable, 'ReferralOutcomeDetailID', args.id);
                });
            });
            $scope.init();
        }
    ]);
