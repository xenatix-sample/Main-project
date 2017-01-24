angular.module('xenatixApp')
    .controller('potentialMatchesController', ['$scope', 'clientMergeService', '$filter', 'alertService', 'httpLoaderInterceptor',
function ($scope, clientMergeService, $filter, alertService, httpLoaderInterceptor) {

    // Private variables
    var potentialMatchesTable = $('#potentialMatchesTable');
    var selectedContacts = [];

    // Private functions
    var init = function () {
        initializeBootstrapTable();
        getPotentialMergeContactsLastRun();
        $scope.getPotentialMatches();
    }

    var initializeBootstrapTable = function () {
        $scope.potentialMatchTableOptions = {
            pagination: true,
            pageSize: 10,
            pageList: [10, 25, 50, 100],
            search: false,
            showColumns: true,
            data: [],
            undefinedText: '',
            checkboxHeader: false,
            onCheck: function (row) {
                selectedContacts.push(row);
            },
            onUncheck: function (row) {
                selectedContacts = $.grep(selectedContacts, function (item) {
                    return item.ContactID !== row.ContactID;
                });
            },
            columns: [
                {
                    field: 'check',
                    checkbox: true
                },
                {
                    field: 'MRN',
                    title: 'MRN',
                    sortable: true
                },
                {
                    field: 'FirstName',
                    title: 'First Name',
                    sortable: true
                },
                {
                    field: 'LastName',
                    title: 'Last Name',
                    sortable: true
                },
                {
                    field: 'DOB',
                    title: 'DOB',
                    sortable: true,
                    formatter: function (value, row) {
                        if (value) {
                            return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY');
                        } else
                            return '';
                    }
                },
                {
                    field: 'SSN',
                    title: 'SSN',
                    sortable: true,
                    formatter: function (value, row, index) {
                        return value ? $filter('toMaskSSN')(value) : '';
                    }
                },
                {
                    field: 'DriverLicense',
                    title: 'Driver’s License Number',
                    sortable: true
                },
                {
                    field: 'PhoneNumber',
                    title: 'Preferred Phone Number',
                    sortable: true,
                    formatter: function (value, row) {
                        return value ? $filter('toPhone')(value) : '';
                    }
                },
                {
                    field: 'Email',
                    title: 'Email Address',
                    sortable: true
                }
            ]
        };
    };
    // Angular methods
    $scope.navigateToCompareRecord = function () {
        (selectedContacts.length !== 2) ? alertService.error("Please select two contact") : $scope.Goto("businessadministration.clientmerge.clientmergeNavigation.potentialmatches.compareRecords", { contactID: selectedContacts[0].ContactID, compareContactID: selectedContacts[1].ContactID });
    };

    $scope.getPotentialMatches = function () {
        clientMergeService.getPotentialMatches().then(function (data) {
            hasData(data) ? potentialMatchesTable.bootstrapTable('load', data.DataItems) : potentialMatchesTable.bootstrapTable('removeAll');
        });
    };

    $scope.getRefreshPotentialMatches = function () {
        httpLoaderInterceptor.ignore(false);
        clientMergeService.getRefreshPotentialMatches().then(function (data) {
            if (data.ResultCode == 0) {
                clientMergeService.getPotentialMatches().then(function (potentialMatchData) {
                    hasData(potentialMatchData) ? potentialMatchesTable.bootstrapTable('load', potentialMatchData.DataItems) : potentialMatchesTable.bootstrapTable('removeAll');
                });
            }
        });
    };


    var getPotentialMergeContactsLastRun = function () {
        clientMergeService.getPotentialMergeContactsLastRun().then(function (data) {
            $scope.MergeContactLastRun = hasData(data) ? data.DataItems[0].LastRunDate : "";
        });
    };

    // Main function
    init();

}]);