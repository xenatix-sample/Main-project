(function () {

    angular.module('xenatixApp')
        .factory('contactEmailService', ["$http", "$q", 'settings', 'offlineData', '$timeout', function ($http, $q, settings, offlineData, $timeout) {
            var CONFIG = {
                apiControllerRoot: "/data/plugins/registration/ContactEmail/",
                offlineApiUrl: '/Registration/ContactEmails/'
            }

            var editStateFunc = function editStateSettings() {
                return {
                    description: 'Contact Email ',
                    state: 'patientprofile.general.emails',
                    stateParams: { ContactID: this.ContactID }
                };
            };

            function get(contactID, contactTypeID) {
                var dfd = $q.defer();
                $timeout(function () {
                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetEmails', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { childKey: 'ContactEmailID' }), params: { contactID: contactID, contactTypeID: contactTypeID } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });
                }, 500);//TODO : for offline multiple get
                return dfd.promise;
            };

            function addUpdate(email) {
                var dfd = $q.defer();
                var data = $.extend(true, {}, email, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (email.ContactID || 0).toString() + '/' + (email.ContactEmailID || 0).toString(), 'ContactEmailID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
                $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddUpdateEmail', data)
                    .then(function (data, status, header, config) {
                        dfd.resolve(data);
                    }, function (data, status, header, config) {
                        dfd.reject(status);
                    }, function (notification) {
                        dfd.notify(notification);
                    });

                return dfd.promise;
            };

            function remove(contactEmailID, contactID) {
                var dfd = $q.defer();
                var url = {
                    data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (contactEmailID || 0).toString(), 'ContactEmailID', { parentKey: 'ContactID' }),
                    params: { contactEmailID: contactEmailID, modifiedOn: moment.utc().format() }
                };

                $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "DeleteEmail", url)
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
                addUpdate: addUpdate,
                remove: remove
            };
        }]);


}());