angular.module('xenatixApp')
    .factory('userPhotoService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {

        var CONFIG = {
            apiControllerRoot: "/data/userPhoto/",
            offlineApiUrl: '/admin/userPhoto/'
        };

        function getUserPhoto(userID, isMyProfile) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getUserPhoto', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (userID || 0).toString(), 'UserID', { childKey: 'UserPhotoID' }), params: { userID: userID, isMyProfile: isMyProfile } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getUserPhotoById(userID, userPhotoID, isMyProfile) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getUserPhotoById', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (userID || 0).toString() + '/' + (userPhotoID || 0).toString(), 'UserPhotoID', { parentKey: 'UserID' }), params: { userPhotoID: userPhotoID, isMyProfile: isMyProfile } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function addUserPhoto(userPhoto, isMyProfile) {
            var dfd = $q.defer();
            if (!('UserPhotoID' in userPhoto))
                userPhoto.UserPhotoID = 0;
            var data = $.extend(true, {}, userPhoto, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (userPhoto.UsertID || 0).toString() + '/' + (userPhoto.UserPhotoID || 0).toString(), 'UserPhotoID', { parentKey: 'UserID', referenceKeys: ['PhotoID'] }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'addUserPhoto', data, { params: { isMyProfile: isMyProfile } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function updateUserPhoto(userPhoto, isMyProfile) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, userPhoto, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (userPhoto.UserID || 0).toString() + '/' + (userPhoto.UserPhotoID || 0).toString(), 'UserPhotoID', { parentKey: 'UserID' }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'updateUserPhoto', data, { params: { isMyProfile: isMyProfile } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function deleteUserPhoto(userID, userPhotoID, isMyProfile) {
            var dfd = $q.defer();

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'deleteUserPhoto', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (userID || 0).toString() + '/' + (userPhotoID || 0).toString(), 'UserPhotoID', { parentKey: 'UserID' }), params: { userPhotoID: userPhotoID, modifiedOn: moment.utc().format(), isMyProfile: isMyProfile } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            getUserPhoto: getUserPhoto,
            getUserPhotoById: getUserPhotoById,
            addUserPhoto: addUserPhoto,
            updateUserPhoto: updateUserPhoto,
            deleteUserPhoto: deleteUserPhoto
        };
    }]);
