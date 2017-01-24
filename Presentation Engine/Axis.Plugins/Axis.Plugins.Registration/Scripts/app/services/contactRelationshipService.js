(function () {
    angular.module('xenatixApp')
        .factory('contactRelationshipService', ["$http", "$q", 'settings', 'offlineData', '$timeout', function ($http, $q, settings, offlineData, $timeout) {
            var CONFIG = {
                apiControllerRoot: "/data/plugins/registration/ContactRelationship/",
                offlineApiUrl: '/Registration/ContactRelationship/'
            }

            var editStateFunc = function editStateSettings() {
                return {
                    description: 'Contact Relationship',
                    stateParams: { ContactID: this.ContactID}
                };
            };
            function get(contactID,parentContactID) {
                var dfd = $q.defer();
                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetContactRelationship', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { parentKey: 'ParentContactID' }), params: { contactID: contactID, parentContactID: parentContactID } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });
                return dfd.promise;
            };

            function addContactRelationship(relationship) {
                var dfd = $q.defer();
                var data = $.extend(true, {}, relationship, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (relationship.ContactID || 0).toString() + '/' + (relationship.ContactRelationshipTypeID || 0).toString(), 'ContactRelationshipTypeID', { parentKey: 'ContactID', referenceKeys: ['ContactID', 'ParentContactID'], editState: editStateFunc.toString() }));
                $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddContactRelationship', data)
                    .then(function (data, status, header, config) {
                        dfd.resolve(data);
                    }, function (data, status, header, config) {
                        dfd.reject(status);
                    }, function (notification) {
                        dfd.notify(notification);
                    });

                return dfd.promise;
            };

            function updateContactRelationship(relationship) {
                var dfd = $q.defer();

                var data = $.extend(true, {}, relationship, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (relationship.ContactID || 0).toString() + '/' + (relationship.ContactRelationshipTypeID || 0).toString(), 'ContactRelationshipTypeID', { parentKey: 'ContactID', referenceKeys: ['ContactID', 'ParentContactID'], editState: editStateFunc.toString() }));
                $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateContactRelationship', data)
                    .then(function (data, status, header, config) {
                        dfd.resolve(data);
                    }, function (data, status, header, config) {
                        dfd.reject(status);
                    }, function (notification) {
                        dfd.notify(notification);
                    });

                return dfd.promise;
            };

            function remove(contactRelationshipTypeID, contactID) {
                var dfd = $q.defer();
                var url = {
                    data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (contactRelationshipTypeID || 0).toString(), 'ContactRelationshipTypeID', { parentKey: 'ContactID', editState: editStateFunc.toString() }),
                    params: { contactRelationshipTypeID: contactRelationshipTypeID, modifiedOn: moment.utc().format() }
                };

                $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "DeleteContactRelationship", url)
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
                addContactRelationship: addContactRelationship,
                updateContactRelationship: updateContactRelationship,
                remove: remove
            };
        }]);
}());