(function() {
    angular.module("xenatixApp").component("syncProgress", {
        template: '' +
            '<div ng-init="$ctrl.init()">' +
            '<table id="syncTable" data-toggle="table" data-filter-control="false" data-mobile-responsive="true" data-search="false" data-show-columns="false" data-sortable="false" data-classes="table table-stripe-borders" bootstrap-table="$ctrl.tableOptions">' +
            '<thead><tr><th>Description</th><th>Status</th><th>Last Synched</th><th>Attempts</th></tr></thead>' +
            '</table>' +
            '</div>',
        controller: [
            'offlineData', '$interval', '$state', '$q', function (offlineData, $interval, $state, $q) {
                this.intervalTimer = null;

                this.initializeBootstrapTable = function() {
                    this.tableOptions = {
                        pagination: true,
                        pageSize: 50,
                        filterControl: false,
                        mobileResponsive: true,
                        search: false,
                        showColumns: false,
                        sortable: false,
                        formatNoMatches: function() {
                            return "You're all caught up!";
                        },
                        onDblClickRow: function (row) {
//                            if (row && row.Status && (row.Status === 'Errored')) { // only for errored attempts or for any offline record?
                            if (row && row.EditState)
                                $state.go(row.EditState.state, row.EditState.stateParams);
//                            }
                        },
                        data: [],
                        columns: [
                            {
                                field: 'DataUrl',
                                title: 'Description',
                                formatter: function (value, row) {
                                    if (row && row.EditState)
                                        return row.EditState.description;
                                    else
                                        return 'Unable to load description!';
                                }
                            },
                            {
                                field: 'Status',
                                title: 'Status',
                                formatter: function (value, row) {
                                    if ((value === 'Errored') && (row.ErrorMessage)) {
                                        return angular.element('<span>').append(angular.element('<a>').attr('href', '#').attr('data-toggle', 'tooltip').attr('data-placement', 'top').attr('title', row.ErrorMessage).html(value)).html();
                                    }
                                    else
                                        return value || 'Pending';
                                }
                            },
                            {
                                field: 'LastSync',
                                title: 'Last Synched',
                                formatter: function (value) {
                                    return value ? value.toLocaleString() : '';
                                }
                            },
                            {
                                field: 'AttemptCount',
                                title: 'Attempts',
                                formatter: function(value) {
                                    return value || '';
                                }
                            }
                        ]
                    };
                };

                this.loadGridData = function (data) {
                    this.syncTable.bootstrapTable('load', data);
                    $('[data-toggle="tooltip"]').tooltip();
                };

                this.getEditState = function (datum) {
                    var deferred = $q.defer();
                    offlineData.getEditState(datum.DataUrl).then(function (editState) {
                        datum.EditState = editState;
                        deferred.resolve();
                    });
                    return deferred.promise;
                };

                this.refreshCachedData = function () {
                    var deferred = $q.defer();
                    this.indexedDbProvider.get(this.idbConfigKey).then(function (data) {
                        var promiseArray = [];
                        data = (data && data.data) || [];
                        for (var iIdx = 0; iIdx < data.length; iIdx++) {
                            promiseArray.push(this.getEditState(data[iIdx]));
                        }
                        $q.all(promiseArray).then(function() {
                            deferred.resolve(data);
                        });
                    }.bind(this));
                    return deferred.promise;
                };

                this.init = function () {
                    this.syncTable = angular.element('#syncTable');
                    this.idbConfigKey = '/Configs';
                    this.indexedDbProvider = offlineData.indexedDbProvider;
                    this.initializeBootstrapTable();
                    this.intervalTimer = $interval(function() {
                        this.refreshCachedData().then(this.loadGridData.bind(this));
                    }.bind(this), 5000);
                    this.refreshCachedData().then(this.loadGridData.bind(this));
                };
            }
        ]
    });
})();