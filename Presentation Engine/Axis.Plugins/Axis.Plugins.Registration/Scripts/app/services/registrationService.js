angular.module('xenatixApp')
    .factory('registrationService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiController: "/data/plugins/registration/registration/",
            offlineApiUrl: '/Registration/Demographics/',
            offlineApiAddressUrl: '/Registration/Address/',
            offlineDuplicateContactApiUrl: '/DuplicateContact/'
        };

        var demoStateFunc = function editStateSettings() {
            return {
                description: 'Demographics for ' + this.FirstName + ' ' + this.LastName,
                state: 'patientprofile.general.demographics',
                stateParams: { ContactID: this.ContactID }
            };
        };

        function get(contactID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiController + 'GetContactDemographics', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID'), params: { contactID: contactID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getContactAddress(contactID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiController + 'GetContactAddress', { data: offlineData.getOfflineSettings(CONFIG.offlineApiAddressUrl + (contactID || 0).toString(), 'ContactAddressID', { parentKey: 'ContactID' }), params: { contactID: contactID } })
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
            var data = $.extend(true, {}, newDemography, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (newDemography.ContactID || 0).toString(), 'ContactID', { editState: demoStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiController + 'AddContactDemographics', data)
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
            var data = $.extend(true, {}, newDemography, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (newDemography.ContactID || 0).toString(), 'ContactID', { editState: demoStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiController + 'UpdateContactDemographics', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function remove(id) {
            var dfd = $q.defer();

            $http.delete(settings.webApiBaseUrl + CONFIG.apiController + 'deletedemographic/',
                { params: { id: id, modifiedOn: moment.utc().format() }})
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };
    
        function verifyDuplicateContacts(contact) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, contact, offlineData.getOfflineSettings(CONFIG.offlineDuplicateContactApiUrl + (contact.ContactID || 0).toString(), 'ContactID', { editState: demoStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiController + 'VerifyDuplicateContacts',  data)
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
            getContactAddress: getContactAddress,
            add: add,
            update: update,
            remove: remove,
            verifyDuplicateContacts: verifyDuplicateContacts
        };
    }]);