angular.module('xenatixApp')
    .factory('userProfileService', [
            "$http", "$q", 'settings', function($http, $q, settings) {
                var CONFIG = {
                    controllerAction: "/account/userprofile/"
                };
                function get(isMyProfile) {
                    var dfd = $q.defer();
                    $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'GetUserProfile', { params: { isMyProfile: isMyProfile } })
                    .success(function (data, status, header, config) {
                        dfd.resolve(data);
                    })
                    .error(function (data, status, header, config) {
                        dfd.reject(status);
                    });

                    return dfd.promise;
                }

                function getByID(userID, isMyProfile) {
                    var dfd = $q.defer();
                    $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'GetUserProfileByID', { params: { userID: userID, isMyProfile: isMyProfile } })
                    .success(function (data, status, header, config) {
                        dfd.resolve(data);
                    })
                    .error(function (data, status, header, config) {
                        dfd.reject(status);
                    });

                    return dfd.promise;
                }

                function save(userProfile, isMyProfile) {
                    var dfd = $q.defer();
                    $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'SaveUserProfile', userProfile,{ params: { isMyProfile: isMyProfile } })
                    .success(function (data) {
                        dfd.resolve(data);
                    })
                    .error(function (data, status) {
                        dfd.reject(status);
                    });

                    return dfd.promise;
                }

                return {
                    get: get,
                    getByID: getByID,
                    save: save
                };
            }
        ]
    );