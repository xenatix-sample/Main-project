(function () {
    angular.module('xenatixApp')
        .factory('userDirectReportsService', ["$http", "$q", 'settings', function ($http, $q, settings) {
            var CONFIG = {
                apiControllerRoot: "/data/UserDirectReports/"
            }

            function get(userID, isMyProfile) {
                var dfd = $q.defer();
                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetUsers', { params: { userID: userID, isMyProfile: isMyProfile } })
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                });
                return dfd.promise;
            };

            function getUsersByCriteria(strSearch, isMyProfile) {
                var dfd = $q.defer();
                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetUsersByCriteria', { params: { strSearch: strSearch, isMyProfile: isMyProfile } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });
                return dfd.promise;
            };

            function add(userDetail, isMyProfile) {
                var dfd = $q.defer();
                if (!userDetail.MappingID) {
                    userDetail.MappingID = 0;
                }
                $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'Add', userDetail, { params: { isMyProfile: isMyProfile } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });
                return dfd.promise;
            }

            function remove(id, isMyProfile) {
                var dfd = $q.defer();
                $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'Remove', { params: { id: id, modifiedOn: moment.utc().format(), isMyProfile: isMyProfile } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });
                return dfd.promise;
            }

            return {
                get: get,
                getUsersByCriteria: getUsersByCriteria,
                add: add,
                remove: remove
            };
        }])
}());