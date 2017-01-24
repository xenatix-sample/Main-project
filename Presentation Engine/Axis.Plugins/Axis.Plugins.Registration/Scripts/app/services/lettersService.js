(function () {
    angular.module('xenatixApp') 
    .factory('lettersService', ["$http", "$q", "settings", "offlineData", function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/registration/Letters/",
            offlineApiUrl: '/Registration/Letters/'
        }

        var editStateFunc = function editStateSettings() {
            return {
                description: 'Letters ',
                state: 'patientprofile.intake.navi.letters',
                stateParams: { ContactID: this.ContactID, ContactLettersID: this.ContactLettersID, ReadOnly: 'Edit' }
            };
        };

        function get(contactID) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetLetters', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { childKey: 'contactLettersID' }), params: { contactID: contactID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function add(lettersModel) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, lettersModel, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (lettersModel.ContactID || 0).toString() + '/' + (lettersModel.ContactLettersID || 0).toString(), 'ContactLettersID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'], editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddLetters', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function update(lettersModel) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, lettersModel, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (lettersModel.ContactID || 0).toString() + '/' + (lettersModel.ContactLettersID || 0).toString(), 'ContactLettersID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'], editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateLetters', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function remove(contactID, contactLettersID) {
            var dfd = $q.defer();
            var url = {
                data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (contactLettersID || 0).toString(), 'ContactLettersID', { parentKey: 'ContactID' }),
                params: { ContactLettersID: contactLettersID, modifiedOn: moment.utc().format() }
            };

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "DeleteLetters", url)
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
            add: add,
            update: update,
            remove: remove
        };
    }])

}());