angular.module('xenatixApp')
.directive("contactSearch", function () {
    var controller =
        ['$scope', '$rootScope', '$state', '$stateParams', '$modal', '$filter', 'clientSearchService', 'alertService', 'auditService',
        function ($scope, $rootScope, $state, $stateParams, $modal, $filter, clientSearchService, alertService, auditService) {
            var contactsTable = $("#contactsTable");
            var contactListModel = $('#contactListModel');
            var searchPermissionKey = '';
            if ($rootScope.$state.current.data && $rootScope.$state.current.data.permissionKey) {
                searchPermissionKey = $rootScope.$state.current.data.permissionKey;
            }
            $scope.allowFocusEvent = true;
            $scope.initializeBootstrapTable = function () {
                $scope.tableoptions = {
                    pagination: true,
                    pageSize: 10,
                    pageList: [10, 25, 50, 100],
                    search: false,
                    showColumns: true,
                    data: [],
                    undefinedText: '',
                    columns: [
                        {
                            field: 'MRN',
                            title: 'MRN'
                        },
                        {
                            field: 'FirstName',
                            title: 'First Name'
                        },
                        {
                            field: 'LastName',
                            title: 'Last Name'
                        },
                        {
                            field: 'Middle',
                            title: 'Middle Name'
                        },
                        {
                            field: 'DOB',
                            title: 'Date Of Birth',
                            formatter: function (value, row, index) {
                                if (value) {
                                    var formattedDate = $filter('formatDate')(value, 'MM/DD/YYYY');
                                    return formattedDate;
                                } else
                                    return '';
                            }
                        },
                        {
                            field: 'ContactGenderText',
                            title: 'Gender'
                        },
                        {
                            field: 'SSN',
                            title: 'SSN',
                            formatter: function (value, row, index) {
                                var formattedSNN = $filter('toMaskSSN')(value);
                                return formattedSNN;
                            }
                        },
                        {
                            field: 'ContactID',
                            title: '',
                            formatter: function (value, row, index) {
                                return '<a href="javascript:void(0)" data-default-action data-ng-click="selectContact(' + value + ', ' + row.ContactTypeID + ')" alt="View Contact" security permission-key="Registration-Registration-Demographics" permission="update" title="Edit" audit-on="click" audit-key="Contact" audit-value="' + value + '"><i class="fa fa-pencil fa-fw padding-left-small padding-right-small" /></a>';
                            }
                        }
                    ]
                };
            };


            $scope.searchContact = function (searchText, isSearch) {
                $scope.disableShortcuts();
                var searchString = searchText.split('=');
                if (searchString.length > 1) {
                    if (!validateSearchPattern(searchString))
                        alertService.warning("Please use the correct format for your search criteria: SSN=NNNN/NNNNNNNNN, DOB=MM/DD/YYYY, DL#=NNNNNNNN, MRN=NNNNNNNNN");
                    else {
                        $scope.allowFocusEvent = false;
                        $scope.getClientSummary(searchText);
                    }
                }
                else if (searchText && searchText != "" && isSearch) {
                    $scope.allowFocusEvent = false;
                    $scope.getClientSummary(searchText);
                }
            }

            $scope.enableShortcuts = function () {
                $scope.setShortcutKey({ enterKeyStop: false, stopNext: false, saveOnEnter: false, stopSave: false });
            }

            $scope.disableShortcuts = function () {
                $scope.setShortcutKey({ enterKeyStop: true, stopNext: false, saveOnEnter: false, stopSave: true });
            }

            //Get the contact detail based on the search text
            $scope.getClientSummary = function (searchText) {
                if (searchText && searchText != '') {
                    clientSearchService.getClientSummary(searchText, $scope.contactType).then(function (data) {
                        $scope.contactList = bindDataModel(data.DataItems, false);
                        if ($scope.contactList && $scope.contactList.length > 0) {
                            contactsTable.bootstrapTable('load', $scope.contactList);
                            contactListModel.modal('show');
                            contactListModel.on('shown.bs.modal', function () {
                                $scope.setFocus();
                            });
                        }
                        else {
                            contactsTable.bootstrapTable('removeAll');
                            $scope.allowFocusEvent = true;
                        }
                    }, function (errorStatus) {
                        $scope.allowFocusEvent = true;
                        alertService.error('Unable to connect to server');
                    }).finally(function () {
                        var contactID = $stateParams.ContactID ? $stateParams.ContactID : null;
                        var pageAudit = {
                            DataKey: searchPermissionKey,
                            ActionTypeID: SCREEN_ACTIONTYPES.View,
                            SearchText: searchText,
                            ContactID: contactID
                        }
                        auditService.addScreenAudit(pageAudit);
                    });
                } else {
                    $scope.allowFocusEvent = true;
                    $scope.stopEnterKey();
                }
            };

            bindDataModel = function (model, showCurrentUser) {
                var listToBind = model;
                angular.forEach(listToBind, function (collateral) {
                    collateral.DOB = collateral.DOB ? $filter('toMMDDYYYYDate')(collateral.DOB, 'MM/DD/YYYY') : "";
                });
                return listToBind;
            };

            $scope.selectContact = function (contactID, contactTypeID) {
                $scope.onContactSelect({ contactID: contactID });
                contactListModel.modal('hide');
                $scope.enableShortcuts();
            };

            $scope.cancelModel = function () {
                $scope.disableShortcuts();
                if (contactListModel && contactListModel.hasClass('in')) {
                    contactListModel.modal('hide');
                    $('#txtClientSearch').focus();
                    $scope.allowFocusEvent = true;
                }
            }

            $scope.initializeBootstrapTable();
            if ($stateParams.ContactID == undefined) {
                $scope.setFocus({ autoFocus: true });
            }
            $scope.setCancelFunction({ cancelFunction: $scope.cancelModel });

        }];

    return {
        restrict: "E",
        scope: {
            searchTitle: "@",
            contactType: "@",
            onContactSelect: "&",
            setShortcutKey: "&",
            setFocus: "&",
            setCancelFunction: '&',
            isDisabled: '='
        },
        controller: controller,
        controllerAs: 'contactSearchVM',
        templateUrl: "/Plugins/ClientSearch/ContactSearch"
    };
});