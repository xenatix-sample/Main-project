angular.module('xenatixApp')
    .factory('screeningService', ["$http", "$q", 'settings', 'offlineData', 'lookupService', function ($http, $q, settings, offlineData, lookupService) {
        var CONFIG = {
            apiControllerRoot: '/data/Plugins/ECI/Screening/',
            offlineApiUrl: '/ECI/Screening/'
        };
        var editStateFunc = function editStateSettings() {
            return {
                description: 'Screening ',
                state: 'patientprofile.chart.screenings.screening.header',
                stateParams: { ContactID: this.ContactID, ScreeningID: this.ScreeningID }
            };
        };
        function getList(contactID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetScreenings', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { childKey: 'ScreeningID' }), params: { contactID: contactID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function get(contactID, screeningID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetScreening', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (screeningID || 0).toString(), 'ScreeningID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'] }), params: { screeningID: screeningID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function add(screening) {
            var dfd = $q.defer();
            if (!('ScreeningID' in screening))
                screening.ScreeningID = 0;
            var data = $.extend(true, {}, screening, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (screening.ContactID || 0).toString() + '/' + (screening.ScreeningID || 0).toString(), 'ScreeningID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'], editState: editStateFunc.toString() }));
          
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddScreening', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function update(screening) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, screening, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (screening.ContactID || 0).toString() + '/' + (screening.ScreeningID || 0).toString(), 'ScreeningID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'], editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateScreening', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function remove(contactID, screeningID) {
            var dfd = $q.defer();

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'RemoveScreening',
                { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (screeningID || 0).toString(), 'ScreeningID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'] }), params: { screeningID: screeningID , modifiedOn: moment.utc().format()} })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getLookups(typeName) {
            return lookupService.getLookupsByType(typeName);
        };

        function getCoordinatorList(programID) {
            var dfd = $q.defer();
            var coordinatorListOfflineApiUrl = '/ECI/Screening/CoordinatorList/';

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'CoordinatorList', { data: offlineData.getOfflineSettings(coordinatorListOfflineApiUrl + (programID || 0).toString()), params: { programID: programID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            getLookups: getLookups,
            getCoordinatorList: getCoordinatorList,
            getList: getList,
            get: get,
            add: add,
            update: update,
            remove: remove
        };
    }]);
