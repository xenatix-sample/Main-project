(function () {
    angular.module('xenatixApp')
    .controller('historyLogController', ['$scope', '$q', '$stateParams', '$filter', 'auditService',
        function ($scope, $q, $stateParams, $filter, auditService) {
            var demographicsTable = $("#demographicsTable");
            var aliasTable = $("#aliasTable");
            var idTable = $("#idTable");
            var strECI = DIVISION_NAME.ECS;
            var aliasId = 0;
            var aliasRowId = 0;
            $scope.aliasKeys = [];
            var identifierId = 0;
            var identifierRowId = 0;
            $scope.identifierKeys = [];
            var aliasParentKeys = [], idParentKeys = [];

            var init = function () {
                $scope.contactID = $stateParams.ContactID;
                $scope.isECI = false;
                initializeBootstrapTable();
                getHistoryDetails();
            };

            var initializeBootstrapTable = function () {
                $scope.demotableoptions = {
                    pagination: true,
                    pageSize: 10,
                    pageList: [10, 25, 50, 100],
                    search: false,
                    showColumns: true,
                    data: [],
                    undefinedText: '',
                    columns: [
                        {
                            field: 'ChangedDate',
                            title: 'Change Date',
                            formatter: function (value) {
                                return value ? $filter('formatDate')(value, 'MM/DD/YYYY hh:mm:ss sss') : '';
                            }
                        },
                        {
                            field: 'TransactionLogID',
                            title: 'Transaction'
                        },
                        {
                            field: 'UserFirstName',
                            title: 'User Name',
                            formatter: function (value, row) {
                                return value + ' ' + row['UserLastName'];
                            }
                        },
                        {
                            field: 'PresentingProblemType',
                            title: 'Presenting Problem'
                        },
                        {
                            field: 'EffectiveDate',
                            title: 'Problem Eff. Date',
                            formatter: function (value) {
                                return value ? $filter('formatDate')(value, 'MM/DD/YYYY') : '';
                            }
                        },
                        {
                            field: 'ExpirationDate',
                            title: 'Problem Exp Date',
                            formatter: function (value) {
                                return value ? $filter('formatDate')(value, 'MM/DD/YYYY') : '';
                            }
                        },
                        {
                            field: 'ClientType',
                            title: 'Division'
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
                            field: 'PreferredName',
                            title: 'Preferred Name'
                        },
                        {
                            field: 'Title',
                            title: 'Prefix'
                        },
                        {
                            field: 'Suffix',
                            title: 'Suffix'
                        },
                        {
                            field: 'Gender',
                            title: 'Gender'
                        },
                        {
                            field: 'IsPregnant',
                            title: 'Is Pregnant',
                            formatter: function (value, row) {
                                return (row["Gender"] == "Male" || row["Gender"] == "Unknown") ? '' : ((value) ? 'Yes' : 'No');
                            }
                        },
                        {
                            field: 'PreferredGender',
                            title: 'Preferred Gender'
                        },
                        {
                            field: 'DOB',
                            title: 'Date of Birth',
                            formatter: function (value) {
                                return value ? $filter('formatDate')(value, 'MM/DD/YYYY') : '';
                            }
                        },
                        {
                            field: 'DOBStatus',
                            title: 'DOB Status'
                        },
                        {
                            field: 'Age',
                            title: 'Age',
                            formatter: function (value, row) {
                                row.Age = row.DOB ? $filter('ageToShow')(row.DOB) : '';
                                return row.Age;
                            }
                        },
                        {
                            field: 'SSN',
                            title: 'SSN',
                            formatter: function (value) {
                                return $filter('toSSN')(value);
                            }
                        },
                        {
                            field: 'SSNStatus',
                            title: 'SSN Status'
                        },
                        {
                            field: 'DriverLicense',
                            title: 'Driver License'
                        },
                        {
                            field: 'DriverLicenseState',
                            title: 'Driver License State'
                        },
                        {
                            field: 'PreferredContactMethod',
                            title: 'Preferred Contact Method'
                        },
                        {
                            field: 'IsActive',
                            title: 'Is Active',
                            formatter: function (value) {
                                return value ? 'Yes' : 'No';
                            }
                        }
                    ]
                };
                $scope.aliastableoptions = {
                    pagination: true,
                    pageSize: 10,
                    pageList: [10, 25, 50, 100],
                    search: false,
                    showColumns: true,
                    data: [],
                    undefinedText: '',
                    rowStyle: function (row, index) {
                        if (aliasRowId == row.ContactAliasID) {
                            return {
                                classes: 'more-data'
                            };
                        } else {
                            aliasRowId = row.ContactAliasID;
                            return {};
                        }
                    },
                    columns: [
                        {
                            field: 'ContactAliasID',
                            title: '',
                            formatter: function (value, row, index) {
                                var parentAlias = $.grep(aliasParentKeys, function (item) {
                                    return item.id == value && item.transId == row.TransactionLogID;
                                });
                                var parentCheck = $.grep(aliasParentKeys, function (item) {
                                    return item.id == value;
                                });
                                if (parentAlias.length === 0) {
                                    if (aliasId == value) {
                                        $scope.aliasKeys.push({ id: aliasId, index: index, toggle: true });
                                        $("#aliasTable").bootstrapTable('hideRow', { index: index });
                                        return '';
                                    } else if (parentCheck.length === 0) {
                                        aliasId = value;
                                        aliasParentKeys.push({ id: value, transId: row.TransactionLogID });
                                        return '<a ng-if="((aliasKeys | filter : {id:' + value + '} : true).length > 0) ? true : false" href="javascript:void(0)" data-default-action id="groupBy" name="groupBy" ng-click="toggelAllias(' + value + ')" title="Group By" space-key-press><i ng-click="toggelAllias(' + value + ')" class="{{ ((aliasKeys | filter : {id:' + value + '} : true)[0] && (aliasKeys | filter : {id:' + value + '} : true)[0].toggle) ? \'fa fa-caret-right\' : \'fa fa-caret-down\' }} " /></a> ';
                                    } else {
                                        $("#aliasTable").bootstrapTable('hideRow', { index: index });
                                    }
                                }
                            }
                        },
                        {
                            field: 'ChangedDate',
                            title: 'Change Date',
                            formatter: function (value) {
                                return value ? $filter('formatDate')(value, 'MM/DD/YYYY hh:mm:ss sss') : '';
                            }
                        },
                        {
                            field: 'TransactionLogID',
                            title: 'Transaction'
                        },
                        {
                            field: 'UserFirstName',
                            title: 'User Name',
                            formatter: function (value, row) {
                                return value + ' ' + row['UserLastName'];
                            }
                        },
                        {
                            field: 'AliasFirstName',
                            title: 'Alias First'
                        },
                        {
                            field: 'AliasLastName',
                            title: 'Alias Last'
                        },
                        {
                            field: 'AliasMiddle',
                            title: 'Alias Middle'
                        },
                        {
                            field: 'Suffix',
                            title: 'Alias Suffix'
                        },
                        {
                            field: 'IsActive',
                            title: 'Is Active',
                            formatter: function (value) {
                                return value ? 'Yes' : 'No';
                            }
                        }
                    ]
                };

                $scope.idtableoptions = {
                    pagination: true,
                    pageSize: 10,
                    pageList: [10, 25, 50, 100],
                    search: false,
                    showColumns: true,
                    data: [],
                    undefinedText: '',
                    rowStyle: function (row, index) {
                        if (identifierRowId == row.ContactClientIdentifierID) {
                            return {
                                classes: 'more-data'
                            };
                        } else {
                            identifierRowId = row.ContactClientIdentifierID;
                            return {};
                        }
                    },
                    columns: [
                        {
                            field: 'ContactClientIdentifierID',
                            title: '',
                            formatter: function (value, row, index) {
                                var parentId = $.grep(idParentKeys, function (item) {
                                    return item.id == value && item.transId == row.TransactionLogID;
                                });
                                var parentIdCheck = $.grep(idParentKeys, function (item) {
                                    return item.id == value;
                                });
                                if (parentId.length === 0) {
                                    if (identifierId == value) {
                                        $scope.identifierKeys.push({ id: identifierId, index: index, toggle: true });
                                        $("#idTable").bootstrapTable('hideRow', { index: index });
                                        return '';
                                    } else if (parentIdCheck.length === 0) {
                                        identifierId = value;
                                        idParentKeys.push({ id: value, transId: row.TransactionLogID });
                                        return '<a ng-if="((identifierKeys | filter : {id:' + value + '} : true).length > 0) ? true : false" href="javascript:void(0)" data-default-action id="groupBy" name="groupBy" ng-click="toggelIdentifier(' + value + ')" title="Group By" space-key-press><i ng-click="toggelIdentifier(' + value + ')" class="{{ ((identifierKeys | filter : {id:' + value + '} : true)[0] && (identifierKeys | filter : {id:' + value + '} : true)[0].toggle) ? \'fa fa-caret-right\' : \'fa fa-caret-down\' }} " /></a> ';
                                    }
                                    else {
                                        $("#idTable").bootstrapTable('hideRow', { index: index });
                                    }
                                }
                            }
                        },
                        {
                            field: 'ChangedDate',
                            title: 'Change Date',
                            formatter: function (value) {
                                return value ? $filter('formatDate')(value, 'MM/DD/YYYY hh:mm:ss sss') : '';
                            }
                        },
                        {
                            field: 'TransactionLogID',
                            title: 'Transaction'
                        },
                        {
                            field: 'UserFirstName',
                            title: 'User Name',
                            formatter: function (value, row) {
                                return value + ' ' + row['UserLastName'];
                            }
                        },
                        {
                            field: 'ClientIdentifierType',
                            title: 'Other Id Type'
                        },
                        {
                            field: 'AlternateID',
                            title: 'Other Id Number'
                        },
                        {
                            field: 'EffectiveDate',
                            title: 'Other Id Eff. Date',
                            formatter: function (value) {
                                return value ? $filter('formatDate')(value, 'MM/DD/YYYY') : '';
                            }
                        },
                        {
                            field: 'ExpirationDate',
                            title: 'Other Id Exp. Date',
                            formatter: function (value) {
                                return value ? $filter('formatDate')(value, 'MM/DD/YYYY') : '';
                            }
                        },
                        {
                            field: 'IsActive',
                            title: 'Is Active',
                            formatter: function (value) {
                                return value ? 'Yes' : 'No';
                            }
                        }
                    ]
                };
            };

            var getHistoryDetails = function () {
                var arr = [];
                arr.push(auditService.getDemographyHistory($scope.contactID));
                arr.push(auditService.getAliasHistory($scope.contactID));
                arr.push(auditService.getIdHistory($scope.contactID));

                $q.all(arr).then(function (response) {
                    if (hasDetails(response)) {
                        //Load Demography history
                        if (hasData(response[0])) {
                            demographicsTable.bootstrapTable('load', response[0].DataItems);
                            applyDropupOnGrid();
                            $scope.UserName = response[0].DataItems[0].FirstName + ' ' + response[0].DataItems[0].LastName;
                            $scope.isECI = (response[0].DataItems[0].ClientType === strECI);
                        }
                        //Load Alias history
                        if (hasData(response[1])) {
                            aliasTable.bootstrapTable('load', response[1].DataItems);
                            applyDropupOnGrid();
                        }
                        //Load ID history
                        if (hasData(response[2])) {
                            idTable.bootstrapTable('load', response[2].DataItems);
                            applyDropupOnGrid();
                        }
                    }
                });
            };

            $scope.toggelAllias = function (id) {
                var childs = $filter('filter')($scope.aliasKeys, function (item) {
                    return item.id == id;
                }, true)

                angular.forEach(childs, function (item, index) { //update list with updated email item
                    if (item.toggle) {
                        aliasTable.bootstrapTable('showRow', { index: item.index });
                    }
                    else {
                        aliasTable.bootstrapTable('hideRow', { index: item.index });
                    }
                    item.toggle = !item.toggle;
                });
            }

            $scope.toggelIdentifier = function (id) {
                var childs = $filter('filter')($scope.identifierKeys, function (item) {
                    return item.id == id;
                }, true)

                angular.forEach(childs, function (item, index) { //update list with updated email item
                    if (item.toggle) {
                        idTable.bootstrapTable('showRow', { index: item.index });
                    }
                    else {
                        idTable.bootstrapTable('hideRow', { index: item.index });
                    }
                    item.toggle = !item.toggle;
                });
            }

            init();
        }])
}())
