angular.module('xenatixApp')
    .factory('vitalService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/Clinical/Vital/",
            offlineApiUrl: '/Registration/Vitals/'
        };

        var editStateFunc = function editStateSettings() {
            return {
                description: 'Vitals ',
                state: 'patientprofile.chart.intake.vitals',
                stateParams: { ContactID: this.ContactID }
            };
        };

        function getContactVital(contactId) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetContactVitals', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactId || 0).toString(), 'ContactID', { childKey: 'VitalID' }), params: { contactId: contactId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

       

        function add(vital) {
            var dfd = $q.defer();
            if (!('VitalID' in vital))
                vital.VitalID = 0;
            var data = $.extend(true, {}, vital, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (vital.ContactID || 0).toString() + '/0', 'VitalID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddVital', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function update(vital) {
            var dfd = $q.defer();

            var data = $.extend(true, {}, vital, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (vital.ContactID || 0).toString() + '/' + (vital.VitalID || 0).toString(), 'VitalID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateVital', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function remove(contactId, vitalId) {
            var dfd = $q.defer();

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'DeleteVital', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactId || 0).toString() + '/' + (vitalId || 0).toString(), 'VitalID', { parentKey: 'ContactID' }), params: { vitalId: vitalId, modifiedOn: moment.utc().format() } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            getContactVital: getContactVital,
            add: add,
            update: update,
            remove: remove            
        };
    }]);
