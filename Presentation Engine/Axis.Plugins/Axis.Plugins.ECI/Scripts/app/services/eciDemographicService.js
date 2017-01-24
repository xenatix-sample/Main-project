angular.module('xenatixApp')
    .factory('eciDemographicService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: '/data/Plugins/ECI/ECIDemographic/',
            offlineApiUrl: '/Registration/Demographics/',
            offlineApiAddressUrl: '/Registration/Address/'
        };

        var editStateFunc = function editStateSettings() {
            return {
                description: 'ECI Demographic ',
                state: 'patientprofile.general.ecidemographics',
                stateParams: { ContactID: this.ContactID }
            };
        };

        function get(contactID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetContactDemographics', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID'), params: { contactID: contactID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function add(newDemography) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, newDemography, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (newDemography.ContactID || 0).toString(), 'ContactID', { editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddContactDemographics', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function update(newDemography) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, newDemography, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (newDemography.ContactID || 0).toString(), 'ContactID', { editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateContactDemographics', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        return {
            get: get,
            add: add,
            update: update
        };
    }]);
