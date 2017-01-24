angular.module('xenatixApp')
    .factory('contactPhotoService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {

        var CONFIG = {
            apiControllerRoot: "/data/plugins/registration/contactPhoto/",
            offlineApiUrl: '/registration/contactPhoto/'
        };


        var editStateFunc = function editStateSettings() {
            return {
                description: 'Contact Profile',
                state: 'patientprofile.general',
                stateParams: { ContactID: this.ContactID }
            };
        };

        function getContactPhoto(contactID) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getContactPhoto', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { childKey: 'ContactPhotoID' }), params: { contactID: contactID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getContactPhotoById(contactID, contactPhotoID) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getContactPhotoById', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl +(contactID || 0).toString() + '/' + (contactPhotoID || 0).toString(), 'ContactPhotoID', { parentKey: 'ContactID' }), params: { contactPhotoID: contactPhotoID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function addContactPhoto(contactPhoto) {
            var dfd = $q.defer();
            if (!('ContactPhotoID' in contactPhoto))
                contactPhoto.ContactPhotoID = 0;
            var data = $.extend(true, {}, contactPhoto, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactPhoto.ContactID || 0).toString() + '/' + (contactPhoto.ContactPhotoID || 0).toString(), 'ContactPhotoID', { parentKey: 'ContactID', referenceKeys: ['PhotoID'], editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'addContactPhoto', data)
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function updateContactPhoto(contactPhoto) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, contactPhoto, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactPhoto.ContactID || 0).toString() + '/' + (contactPhoto.ContactPhotoID || 0).toString(), 'ContactPhotoID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'updateContactPhoto', data)
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function deleteContactPhoto(contactID, contactPhotoID) {
            var dfd = $q.defer();

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'deleteContactPhoto', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (contactPhotoID || 0).toString(), 'ContactPhotoID', { parentKey: 'ContactID' }), params: { contactPhotoID: contactPhotoID, modifiedOn: moment.utc().format() } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            getContactPhoto: getContactPhoto,
            getContactPhotoById : getContactPhotoById,
            addContactPhoto: addContactPhoto,
            updateContactPhoto: updateContactPhoto,
            deleteContactPhoto: deleteContactPhoto
        };
    }]);
