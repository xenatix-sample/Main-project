(function () {
    angular.module('xenatixApp')
        .factory('contactAliasService', ["$http", "$q", 'settings', 'offlineData', '$timeout', function ($http, $q, settings, offlineData, $timeout) {
            var CONFIG = {
                apiControllerRoot: "/data/plugins/registration/ContactAlias/",
                offlineApiUrl: '/Registration/ContactAlias/'
            }
            var editStateFunc = function editStateSettings() {
                return {
                    description: 'Contact Alias',
                    stateParams: { ContactID: this.ContactID }
                };
            };

            function get(contactID) {
                var dfd = $q.defer();
                $timeout(function () {
                    $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetContactAlias', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { childKey: 'ContactAliasID' }), params: { contactID: contactID } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });
                }, 500);//TODO : for offline multiple get
                return dfd.promise;
            };

            function addContactAlias(alias) {
                var dfd = $q.defer();
                var data = $.extend(true, {}, alias, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (alias.ContactID || 0).toString() + '/' + (alias.ContactAliasID || 0).toString(), 'ContactAliasID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
                $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddContactAlias', data)
                    .then(function (data, status, header, config) {
                        dfd.resolve(data);
                    }, function (data, status, header, config) {
                        dfd.reject(status);
                    }, function (notification) {
                        dfd.notify(notification);
                    });

                return dfd.promise;
            };

            function updateContactAlias(alias) {
                var dfd = $q.defer();

                var data = $.extend(true, {}, alias, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (alias.ContactID || 0).toString() + '/' + (alias.ContactAliasID || 0).toString(), 'ContactAliasID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
                $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateContactAlias', data)
                    .then(function (data, status, header, config) {
                        dfd.resolve(data);
                    }, function (data, status, header, config) {
                        dfd.reject(status);
                    }, function (notification) {
                        dfd.notify(notification);
                    });

                return dfd.promise;
            };

            function remove(contactAliasID, contactID) {
                var dfd = $q.defer();
                var url = {
                    data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (contactAliasID || 0).toString(), 'ContactAliasID', { parentKey: 'ContactID', editState: editStateFunc.toString() }),
                    params: { contactAliasID: contactAliasID, modifiedOn: moment.utc().format() }
                };

                $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "DeleteContactAlias", url)
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
                addContactAlias: addContactAlias,
                updateContactAlias: updateContactAlias,
                remove: remove
            };
        }]);
}());