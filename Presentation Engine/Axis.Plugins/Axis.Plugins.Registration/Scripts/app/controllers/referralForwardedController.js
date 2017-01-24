angular.module('xenatixApp')
    .controller('referralForwardedController', [
        '$scope', '$state', '$q', '$stateParams', '$modal', 'referralForwardedService', 'alertService', 'lookupService', '$filter', '$rootScope', 'formService', '$timeout', 'navigationService',
        function ($scope, $state, $q, $stateParams, $modal, referralForwardedService, alertService, lookupService, $filter, $rootScope, formService, $timeout, navigationService) {
            $scope.isLoading = true;
            var referralForwardedTable = $("#referralForwardedTable");

            $scope.newReferralForwardedDetail = function () {
                $scope.referralForwardedDetail = {};
                $scope.permissionID = 0;
                $scope.initDateandUserDetails();
                resetForm();
            };
            $scope.permissionKey = $state.current.data.permissionKey;
            $scope.initLookups = function () {
                $scope.referralStatus = lookupService.getLookupsByType("ReferralStatus");
                $scope.staffList = lookupService.getLookupsByType('Users');
                //$scope.organizationList = lookupService.getLookupsByType('Organizations');
                $scope.referralToList = lookupService.getLookupsByType('Users');
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
                        $scope.selectRow(e, row);
                    },
                    columns: [{
                        field: 'UserID',
                        title: 'Staff Name',
                        formatter: function (value, row, index) {
                            if (value) {
                                return lookupService.getText('Users', value);
                            }
                            else {
                                return "";
                            }
                        }
                    },

                     {
                         field: 'SendingReferralToID',
                         title: 'Sending Referral To',
                         formatter: function (value, row, index) {
                             if (value)
                                 return lookupService.getText('UserFacility', value);
                             else
                                 return "";

                         }
                     },

                          {
                              field: 'OrganizationID',
                              title: 'Program Unit',
                              formatter: function (value, row, index) {
                                  if (value) {
                                      return lookupService.getText('Organizations', value);
                                  }
                                  else {
                                      return "";
                                  }
                              }
                          },

                     {
                         field: 'ReferralSentDate',
                         title: 'Date Referral Sent',
                         formatter: function (value, row, index) {
                             if (value) {
                                 return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY', 'useLocal');
                             } else
                                 return '';
                         }
                     },
                      {
                          field: 'ReferralForwardedDetailID',
                          title: '',
                          formatter: function (value, row, index) {
                              return '<a ng-hide="{{isReadOnlyForm}}" href="javascript:void(0)" data-default-action id="editReferralForwarded" name="editReferralForwarded" ' + 'ng-click="editReferralForwarded(' + value + ', $event)" title="Edit" ' + 'security permission-key="Referrals-Referral-ForwardedTo" permission="update" space-key-press ><i class="fa fa-pencil fa-fw" /></a>';
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

            $scope.initDateandUserDetails = function () {
                $scope.endDate = new Date();
                $scope.referralForwardedDetail.ReferralSentDate = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal');
                navigationService.get().then(function (data) {
                    $scope.CurrentUserID = data.DataItems[0].UserID;
                    $scope.referralForwardedDetail.UserID = $scope.CurrentUserID;
                    resetForm();
                });
            };

            var resetForm = function () {
                $rootScope.formReset($scope.ctrl.referralForm);
            };

            $scope.getReferralForwardedDetail = function (referralForwardedDetailID) {
                $scope.isLoading = true;
                return referralForwardedService.getReferralForwardedDetail(referralForwardedDetailID).then(function (data) {
                    if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                        $scope.ReferralForwardedDetail = data.DataItems[0];
                        resetForm();
                    } else {
                        alertService.error('Unable to get ReferralForwardedDetail !');
                    }
                },
                function (errorStatus) {
                    alertService.error('Unable to get ReferralForwardedDetail: ' + errorStatus);
                }).finally(function () {
                    $scope.isLoading = false;
                });
            };

            $scope.get = function () {

                return referralForwardedService.getReferralForwardedDetails($scope.ReferralHeaderID).then(function (data) {
                    if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                        $scope.ReferralForwardedDetails = data.DataItems;

                        referralForwardedTable.bootstrapTable('load', $scope.ReferralForwardedDetails);
                        $rootScope.$broadcast('referrals.forwardedto', { validationState: 'valid' });
                    } else {
                        $scope.ReferralForwardedDetails = [];
                        referralForwardedTable.bootstrapTable('removeAll');
                        $rootScope.$broadcast('referrals.forwardedto', { validationState: 'warning' });
                    }
                    $scope.permissionID = 0;
                    resetForm();
                    applyDropupOnGrid(true);
                },
                function (errorStatus) {
                    alertService.error('Unable to get  ReferralForwardedDetails: ' + errorStatus);
                }).finally(function () {
                    $scope.isLoading = false;
                });
            };

            $scope.selectRow = function (e) {
                if (formService.isDirty()) {
                    bootbox.dialog({
                        message: "You have unsaved data. What would you like to do?",
                        buttons: {
                            save: {
                                label: "SAVE",
                                className: "btn-success",
                                callback: function () {
                                    $scope.save(false, true, false, true, false).then(function () {
                                        $scope.setFields(e);
                                    });
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
                $scope.referralForwardedDetail = angular.copy(e);
                $scope.permissionID = $scope.referralForwardedDetail.ReferralForwardedDetailID;
                $scope.referralForwardedDetail.ReferralSentDate = $filter('toMMDDYYYYDate')($scope.referralForwardedDetail.ReferralSentDate, 'MM/DD/YYYY');
                if ($scope.referralForwardedDetail.SendingReferralToID != null) {
                    angular.forEach($scope.referralToList, function (item) {
                        if (item.ID === $scope.referralForwardedDetail.SendingReferralToID) {
                            $scope.SendingReferralToID = item;
                        }
                    });
                }
                resetForm();
                $scope.$digest();
            }

            $scope.validateDate = function () {
                if ($scope.referralForwardedDetail.ReferralSentDate != undefined && $scope.referralForwardedDetail.ReferralSentDate != null && $scope.referralForwardedDetail.ReferralSentDate !== '') {
                    var date = new Date($scope.referralForwardedDetail.ReferralSentDate);
                    if (date > $scope.endDate) {
                        $scope.ctrl.referralForm.referralSentDateCalander.$setValidity('date', false);
                    }
                } else {
                    $("input[name='referralSentDate']").removeClass('ng-invalid').removeClass('ng-invalid-date');
                }
            };

            $scope.save = function (isNext, mandatory, hasErrors, keepForm) {
                var isDirty = formService.isDirty();
                if (isDirty && !hasErrors) {
                    var deferred = $q.defer();
                    $scope.referralForwardedDetail.ReferralHeaderID = $scope.ReferralHeaderID;
                    $scope.referralForwardedDetail.ContactID = $stateParams.ContactID;
                    if ($scope.SendingReferralToID && $scope.SendingReferralToID.ID)
                    $scope.referralForwardedDetail.SendingReferralToID = $scope.SendingReferralToID.ID;
                    else
                        $scope.referralForwardedDetail.SendingReferralToID = null;
                    var isUpdate = $scope.referralForwardedDetail.ReferralForwardedDetailID != undefined && $scope.referralForwardedDetail.ReferralForwardedDetailID !== 0;
                    $scope.saveReferralForwarded(isUpdate).then(function (response) {
                        var action = isUpdate ? 'updated' : 'added';
                        $scope.get().finally(function () {
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
                else if (!isDirty && isNext) {
                    $scope.next();
                }
            };

            $scope.saveReferralForwarded = function (isUpdate) {
                if (!isUpdate) {
                    return referralForwardedService.addReferralForwardedDetail($scope.referralForwardedDetail)
                }
                else {
                    return referralForwardedService.updateReferralForwardedDetail($scope.referralForwardedDetail);
                }
            };

            $scope.postSave = function (response, action, isNext) {
                var data = response;
                if (data.ResultCode !== 0) {
                    alertService.error('OOPS! Something went wrong');
                    return $scope.promiseNoOp();
                } else {
                    alertService.success('Referral Forwarded detail has been ' + action + ' successfully.');
                    $scope.referralForwardedDetail.ReferralForwardedDetailID =
                        (($scope.referralForwardedDetail !== undefined) && ($scope.referralForwardedDetail.ReferralForwardedDetailID !== undefined) && ($scope.referralForwardedDetail.ReferralForwardedDetailID != 0))
                        ? $scope.referralForwardedDetail.ReferralForwardedDetailID
                        : response.ID;

                    if (isNext) {
                        $scope.next();
                    }
                    else {
                        $scope.SendingReferralToID = null;
                        $scope.init();
                        return true;
                    }
                }
            };

            $scope.next = function () {
                var params = {
                    ReferralHeaderID: $scope.ReferralHeaderID,
                    ContactID: $stateParams.ContactID
                }
                $state.go("referrals.referredto", params);
            };

            $scope.init = function () {
                $scope.ReferralHeaderID = $stateParams.ReferralHeaderID;
                $scope.$parent['autoFocus'] = true; //for Focus
                $scope.referralForwardedDetail = {};
                $scope.referralForwardedDetails = [];
                $scope.initializeBootstrapTable();
                $scope.setDefaultDatePickerSettings();
                $scope.initLookups();
                $scope.initDateandUserDetails();
                $scope.get();
                if ($stateParams.ReferralHeaderID == undefined)
                    $scope.newReferralForwardedDetail();
                resetForm();
            };

            $scope.init();
        }
    ]);
