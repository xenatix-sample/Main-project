angular.module('xenatixApp')
    .controller('mergedContactsController', ['$scope', '$filter', 'clientMergeService', 'alertService', '_',
        function ($scope, $filter, clientMergeService, alertService, _) {

            // Private variables
            var mergedRecordTable = $('#mergedRecordsTable');
            $scope.disabledUnMergeButton = true;

            // Private functions
            var init = function () {
                initializeBootstrapTable();
                getMergedRecords();
            }
            var initializeBootstrapTable = function () {

                $scope.tableoptions = {
                    pagination: true,
                    pageSize: 10,
                    pageList: [10, 25, 50, 100],
                    search: false,
                    showColumns: true,
                    checkboxHeader: false,
                    data: [],
                    undefinedText: '',
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
                            title: 'Driver’sLicenseNumber',
                            sortable: true
                        },
                        {
                            field: 'Phone',
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
                        },
                        {
                            field: 'MergedDate',
                            title: 'Merged Date',
                            sortable: true,
                            formatter: function (value, row) {
                                if (value) {
                                    return $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY');
                                } else
                                    return '';
                            }
                        }

                    ]
                };
            };

            var getMergedRecords = function () {
                clientMergeService.getMergedRecords().then(function (data) {
                    $scope.mergedContactList = [{ mergeContactList: []}];
                    if (hasData(data)) {
                        var mergeContactList = [];
                        var mergedList = _.groupBy(data.DataItems, "MergedContactsMappingID");
                        angular.forEach(mergedList, function (item) {
                            var parentContact = item[0];
                            parentContact.isChecked = false;
                            parentContact.childrens = [];
                            parentContact.childrens.push(item[1]);
                            mergeContactList.push(parentContact);
                        });
                        $scope.mergedContactList = [{ mergeContactList: mergeContactList }];
                    }
                });
            };

            $scope.unMergeContact = function () {
                var selectedContacts = $filter('filter')($scope.mergedContactList[0].mergeContactList, { isChecked: true }, true);
                if (selectedContacts.length !== 1)
                    alertService.error("Please select one contact at a time to un-merge.");
                else {
                    bootbox.confirm("<b>Are you sure you want to Unmerge?</b> <br/> This record will revert back to the two records pre-merge.", function (result) {
                        if (result == true) {
                            clientMergeService.unMergeRecords(selectedContacts[0].MergedContactsMappingID).then(function (data) {
                                alertService.success("Unmerged record completed successfully.");
                                getMergedRecords();
                            }, function () {
                                alertService.error('Something went wrong with un-merge, Please try again or contact administrator');
                            });
                        }
                    });
                }
            };

            $scope.mergeCheckboxCallBack = function (id) {
                var mergeContactList = $scope.mergedContactList[0].mergeContactList;
                var item = $filter('filter')(mergeContactList, { MergedContactsMappingID: id }, true)[0];
                item.isChecked = !item.isChecked;
                $scope.disabledUnMergeButton = ($filter('filter')(mergeContactList, { isChecked: true }, true).length > 0) ? false : true;
            };

            // Main function
            init();

        }]);