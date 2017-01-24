 angular.module('xenatixApp')
    .factory('staffManagementService', ["$http", "$q", 'settings', function ($http, $q, settings) {
        var CONFIG = {
            apiControllerRoot: '/data/staffManagement/'
        };

        function get(searchText) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetStaff', { params: { searchText: searchText } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function deleteUser(userID){
            var dfd = $q.defer();
            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'DeleteUser', { params: { userID: userID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function activate(userID) {
            var dfd = $q.defer();

            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'ActivateUser', {}, { params: { userID: userID } })
           .then(function (data, status, header, config) {
               dfd.resolve(data);
           }, function (data, status, header, config) {
               dfd.reject(status);
           }, function (notification) {
               dfd.notify(notification);
           });

            return dfd.promise;
        };

        function reset(primaryEmail) {
            var dfd = $q.defer();
            var resetPasswordControllerAction = '/account/forgotPassword/';

            $http.post(settings.webApiBaseUrl + resetPasswordControllerAction + 'sendResetLink', { email: primaryEmail })
           .then(function (data, status, header, config) {
               dfd.resolve(data);
           }, function (data, status, header, config) {
               dfd.reject(status);
           }, function (notification) {
               dfd.notify(notification);
           });

            return dfd.promise;
        };

        function unlock(userID) {
            var dfd = $q.defer();

            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UnlockUser', {}, { params: { userID: userID } })
           .then(function (data, status, header, config) {
               dfd.resolve(data);
           }, function (data, status, header, config) {
               dfd.reject(status);
           }, function (notification) {
               dfd.notify(notification);
           });

            return dfd.promise;
        };

        return {
            get: get,
            deleteUser: deleteUser,
            activate: activate,
            reset: reset,
            unlock: unlock
        };
    }]);