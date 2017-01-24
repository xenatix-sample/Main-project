angular.module("xenatixApp")
    .controller("referralSearchController", [
        "$scope", "$filter", "$timeout", "referralSearchService", "alertService", "lookupService", "$stateParams", "$state", "$rootScope", "formService", 'registrationService', 'contactAddressService', 'contactEmailService', 'contactPhoneService', 'roleSecurityService', 'navigationService',
        function ($scope, $filter, $timeout, referralSearchService, alertService, lookupService, $stateParams, $state, $rootScope, formService, registrationService, contactAddressService, contactEmailService, contactPhoneService, roleSecurityService, navigationService) {
            var contactTypeID = 7;
            var searchType = "1";// for incoming option
            var logedUserID = 0;
            $scope.searchType = searchType;
            
            $scope.get = function (searchStr) {
                $scope.isLoading = true;
                
                return referralSearchService.get(searchStr ? searchStr : '', $scope.searchType, logedUserID).then(function (data) {
                    if (data.DataItems && data.DataItems != undefined) {
                        $scope.referrals = data.DataItems;
                        $scope.referralsTable.bootstrapTable('load', $scope.referrals);
                        $scope.referralsTable.bootstrapTable('hideColumn', Number($scope.searchType) != 1 ? 'SubmittedBy' : 'ForwardedTo');
                        $scope.referralsTable.bootstrapTable('showColumn', Number($scope.searchType) == 1 ? 'SubmittedBy' : 'ForwardedTo');
                        angular.forEach($scope.referrals, function (referral) {
                            saveForOfflineUse(referral.HeaderContactID);
                        });
                    } else {
                        $scope.referrals = [];
                        $scope.referralsTable.bootstrapTable('removeAll');
                    }

                    applyDropupOnGrid(true);

                }
                ).finally(function () {
                    //Store data for offline use for each record in $scope.referrals
                    $scope.isLoading = false;
                });
            };

            saveForOfflineUse = function (contactId) {
                //Gets all data in cache for quick registration
                //No need to get referral header
                registrationService.get(contactId);
                contactAddressService.get(contactId, contactTypeID);
                contactEmailService.get(contactId, contactTypeID);
                contactPhoneService.get(contactId, contactTypeID);
            };

            $scope.remove = function (id) {
                $scope.reasonForDelete = "";
                $scope.deleteId = id;
                removeErrorClass();
                $('#deleteModel').modal('show');
                $timeout(function () {
                    $('#reasonForDelete').focus();
                }, 1000);
            };

            $scope.cancelModel = function () {
                $('#deleteModel').modal('hide');
            }

            $scope.addNew = function () {
                $state.go('referrals.main', { ReadOnly: 'edit' });
            }

            $scope.checkReasonforDelete = function () {
                removeErrorClass();
                if ($scope.referraldeleteForm.reasonForDelete.$invalid) {
                    $('.modal-body').addClass('has-error');
                    return false;
                }
                return true;
            }

            var removeErrorClass = function () {
                if ($('.modal-body').hasClass('has-error'))
                    $('.modal-body').removeClass('has-error');
            }

            $scope.deleteReferral = function (id, reasonForDelete) {
                if (reasonForDelete == undefined || reasonForDelete == null || !$scope.checkReasonforDelete() || reasonForDelete == "") {
                    return;
                }
                $('#deleteModel').modal('hide');

                referralSearchService.remove(id, reasonForDelete).then(function (data) {
                    $scope.isLoading = false;
                    alertService.success("Referral has been deleted successfully.");
                    $scope.get('');
                },
                function (errorStatus) {
                    alertService.error("OOPS! Something went wrong");
                }).then(function () {
                    $scope.$apply();
                });
            };

            $scope.quickRegistration = function (row) {
                //TODO: Move to quick registration screen
            };

            $scope.sendMail = function (row) {
                //TODO: send message to respective staff members
            };

            $scope.prepRowEditData = function (row) {
                $state.go("referrals.requestor", "{ ReferralHeaderID: " + row.ReferralHeaderID + ", ReadOnly: true }");
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
                        //$scope.prepRowEditData(e);
                    },
                    columns: [
                        {
                            field: "MRN",
                            title: "MRN"
                        },
                        {
                            field: "FirstName",
                            title: "First Name"
                        },
                        {
                            field: "LastName",
                            title: "Last Name"
                        },
                        {
                            field: "Contact",
                            title: "Client Contact",
                            formatter: function (value, row) {
                                if (value) {
                                    return $filter('toPhone')(value);
                                } else
                                    return '';
                            }
                        },
                        {
                            field: "RequestorName",
                            title: "Referrer Name"
                        },
                        {
                            field: "RequestorContact",
                            title: "Referrer Contact",
                            formatter: function (value, row) {
                                if (value) {
                                    return $filter('toPhone')(value);
                                } else
                                    return '';
                            }
                        },
                        {
                            field: "TransferReferralDate",
                            title: "Referral Date",
                            formatter: function (value, row) {
                                if (value) {
                                    return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY', 'useLocal');
                                } else
                                    return '';
                            }
                        },
                        {
                            field: "ReferralStatus",
                            title: "Referral Status"
                        },
                        {
                             field: "ProgramUnit",
                             title: "Program Unit"
                        },
                        {
                            field: "ForwardedTo",
                            title: "Forwarded To"
                        },
                        {
                            field: "SubmittedBy",
                            title: "Submitted By"
                        },
                        {
                            field: "ReferralHeaderID",
                            title: "",
                            formatter: function (value, row, index) {
                                var permissionKey = "Referrals-Referral-Referrer";
                                var hasEditPermission = roleSecurityService.hasPermission(permissionKey, PERMISSION.UPDATE);
                                return (
                                    '<span class="text-nowrap"><a ' + (hasEditPermission ? 'data-default-no-action' : 'data-default-action') + '  ui-sref="referrals.requestor({  ReadOnly: \'view\',ReferralHeaderID: ' + value + ',ContactID: ' + row.HeaderContactID + ' })" alt="View" ' +
                                            'security permission-key="Referrals-Referral-Referrer" permission="read" space-key-press>' +
                                            '<i security permission-key="Referrals-Referral-Referrer" permission="update"  title="View" class="fa fa-eye fa-fw border-left margin-left padding-left-small padding-right-small" /></a>' +

                                    '<a '+(hasEditPermission?'data-default-action':'data-default-no-action')+' ui-sref="referrals.requestor({  ReadOnly: \'edit\',ReferralHeaderID: ' + value + ',ContactID: ' + row.HeaderContactID + ' })" alt="Edit" ' +
                                            'security permission-key="Referrals-Referral-Referrer" permission="update" space-key-press>' +
                                            '<i  title="Edit" class="fa fa-pencil fa-fw border-left margin-left padding-left-small padding-right-small" /></a>' +

                                    '<a data-default-no-action href="javascript:void(0)"  alt="Quick Registration" ' + ((row.ContactID && row.ContactID != 0) ? 'ui-sref="contactprogram({  OtherContactID: ' + row.ContactID + '})"' : '') +
                                            'security permission-key="Referrals-Referral-Referrer" permission="update" space-key-press>' +
                                            '<i  title="' + ((row.ContactID && row.ContactID != 0) ? 'Quick Registration' : 'Referral Client doesn\'t exists') + '" class="fa fa-sign-in fa-fw border-left margin-left padding-left-small padding-right-small" /></a>' +

                                    '<a data-default-no-action href="javascript:void(0)" ng-click="remove(' + value + ', $event)" alt="Remove" ' +
                                            'security permission-key="Referrals-Referral-Referrer" permission="delete" space-key-press>' +
                                            '<i title="Remove" class="fa fa-trash fa-fw border-left margin-left padding-left-small padding-right-small" /></a>'
                                    + '</span>'
                                    );
                            }
                        }
                    ]
                };
            };

            $scope.init = function () {
                navigationService.get().then(function (data) {
                    if (data && data.DataItems && data.DataItems.length > 0) {
                        logedUserID = data.DataItems[0].UserID;
                        $scope.get('');
                    }
                })
                $scope.$parent['autoFocus'] = true;
                $scope.referralsTable = $("#referralsTable");
                $scope.initializeBootstrapTable();                
            };

            $scope.init();

        }]);