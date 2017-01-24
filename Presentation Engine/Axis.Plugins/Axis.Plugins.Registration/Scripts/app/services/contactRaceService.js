(function () {
    angular.module('xenatixApp')
        .factory('contactRaceService', ["$http", "$q", 'settings', 'offlineData', '$timeout', function ($http, $q, settings, offlineData, $timeout) {
            var CONFIG = {
                apiControllerRoot: "/data/plugins/registration/ContactRace/",
                offlineApiUrl: '/Registration/ContactRace/'
            }

            var editStateFunc = function editStateSettings() {
                return {
                    description: 'Contact Race',
                    stateParams: { ContactID: this.ContactID}
                };
            };
            function get(contactID) {
                var dfd = $q.defer();
                $timeout(function () {
                    $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetContactRace', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { childKey: 'ContactRaceID' }), params: { contactID: contactID } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });
                }, 500);//TODO : for offline multiple get
                return dfd.promise;
            };

            function addContactRace(Race) {
                var dfd = $q.defer();
                var data = $.extend(true, {}, Race, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (Race.ContactID || 0).toString() + '/' + (Race.ContactRaceID || 0).toString(), 'ContactRaceID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
                $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddContactRace', data)
                    .then(function (data, status, header, config) {
                        dfd.resolve(data);
                    }, function (data, status, header, config) {
                        dfd.reject(status);
                    }, function (notification) {
                        dfd.notify(notification);
                    });

                return dfd.promise;
            };

            function updateContactRace(Race) {
                var dfd = $q.defer();

                var data = $.extend(true, {}, Race, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (Race.ContactID || 0).toString() + '/' + (Race.ContactRaceID || 0).toString(), 'ContactRaceID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
                $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateContactRace', data)
                    .then(function (data, status, header, config) {
                        dfd.resolve(data);
                    }, function (data, status, header, config) {
                        dfd.reject(status);
                    }, function (notification) {
                        dfd.notify(notification);
                    });

                return dfd.promise;
            };

            function remove(contactRaceID, contactID) {
                var dfd = $q.defer();
                var url = {
                    data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (contactRaceID || 0).toString(), 'ContactRaceID', { parentKey: 'ContactID', editState: editStateFunc.toString() }),
                    params: { contactRaceID: contactRaceID, modifiedOn: moment.utc().format() }
                };

                $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "DeleteContactRace", url)
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };

            return {
                get: get,
                addContactRace: addContactRace,
                updateContactRace: updateContactRace,
                remove: remove
            };
        }]);
}());