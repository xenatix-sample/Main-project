angular.module('xenatixApp')
    .factory('photoService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: "/data/photo/",
            offlineApiUrl: '/photo/',
        }
        function getPhoto(photoID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getPhoto', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (photoID || 0).toString(), 'PhotoID'), params: { photoID: photoID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function addPhoto(photo) {
            var dfd = $q.defer();
            if (!('PhotoID' in photo) || (photo.PhotoID === undefined))
                photo.PhotoID = 0;

            var data = $.extend(true, {}, photo, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (photo.PhotoID || 0).toString(), 'PhotoID'));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'addPhoto', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function updatePhoto(photo) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, photo, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (photo.PhotoID || 0).toString(), 'PhotoID'));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'updatePhoto', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function deletePhoto(photoID) {
            var dfd = $q.defer();

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'deletePhoto', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (photoID || 0).toString(), 'PhotoID'), params: { photoID: photoID, modifiedOn: moment.utc().format() } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            getPhoto: getPhoto,
            addPhoto: addPhoto,
            updatePhoto: updatePhoto,
            deletePhoto: deletePhoto
        };
    }]);