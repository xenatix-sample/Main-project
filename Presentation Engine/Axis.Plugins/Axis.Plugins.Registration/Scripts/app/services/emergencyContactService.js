angular.module('xenatixApp')
    .factory('emergencyContactService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {

        var CONFIG = {
            apiControllerRoot: "/data/plugins/registration/EmergencyContact/",
            offlineApiUrl: '/Registration/Emergency/'
        };

        var editStateFunc = function editStateSettings() {
            return {
                description: 'Emergency Contacts',
                state: 'registration.emergcontacts',
                stateParams: { ContactID: this.ContactID }
            };
        };

        function get(contactID, contactTypeID) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetEmergencyContacts', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ParentContactID', { childKey: 'ContactRelationshipID' }), params: { contactID: contactID, contactTypeID: contactTypeID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function add(emergencyContact) {
            var dfd = $q.defer();
            if (!('ContactRelationshipID' in emergencyContact))
                emergencyContact.ContactRelationshipID = 0;
            var data = $.extend(true, {}, emergencyContact, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (emergencyContact.ParentContactID || 0).toString() + '/0', 'ContactRelationshipID', { parentKey: 'ParentContactID', editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddEmergencyContact', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function update(emergencyContact) {
            var dfd = $q.defer();

            var data = $.extend(true, {}, emergencyContact, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (emergencyContact.ParentContactID || 0).toString() + '/' + (emergencyContact.ContactRelationshipID || 0).toString(), 'ContactRelationshipID', { parentKey: 'ParentContactID', editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateEmergencyContact', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function remove(contactID, contactRelationshipID) {
            var dfd = $q.defer();

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'DeleteEmergencyContact', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (contactRelationshipID || 0).toString(), 'ContactRelationshipID', { parentKey: 'ParentContactID' }), params: { Id: contactRelationshipID, modifiedOn: moment.utc().format() } })
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
    }]);
