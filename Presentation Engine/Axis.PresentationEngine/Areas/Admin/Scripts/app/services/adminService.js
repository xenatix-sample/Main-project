angular.module('xenatixApp')
    .factory('adminService', ["$http", "$q", 'settings', function ($http, $q, settings) {
        var CONFIG = {
            controllerAction: "/admin/admin/"
        };
        function get(userSch) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'GetUsers', { params: { userSch: userSch } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function add(user) {
            var dfd = $q.defer();
            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'AddUser', user)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        }

        function update(user) {
            var dfd = $q.defer();
            
            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'UpdateUser', user)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        }

        function remove(user) {
            var dfd = $q.defer();
            user.ModifiedOn = moment.utc();
            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'RemoveUser', user)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });
            
            return dfd.promise;
        }

        function activate(user) {
            var dfd = $q.defer();

            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'ActivateUser', user)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        }

        function reset(primaryEmail) {
            var dfd = $q.defer();
            var resetPasswordControllerAction = '/account/forgotPassword/';

            $http.post(settings.webApiBaseUrl + resetPasswordControllerAction + 'sendResetLink', { email: primaryEmail })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        }

        function unlock(user) {
            var dfd = $q.defer();

            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'UnlockUser', user)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        }

        function getUserRoles(user) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'GetUserRoles', { params: { userID: user.UserID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        }

        function getUserCredentials(user) {
            var dfd = $q.defer();

            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'GetUserCredentials', user)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        }

        function addUserCredential(userCredential) {
            var dfd = $q.defer();

            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'AddUserCredential', userCredential)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        }

        function updateUserCredential(userCredential) {
            var dfd = $q.defer();

            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'UpdateUserCredential', userCredential)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        }

        function removeUserCredential(userCredentialID) {
            var dfd = $q.defer();

            $http.delete(settings.webApiBaseUrl + CONFIG.controllerAction + 'DeleteUserCredential',
                { params: { userCredentialID: userCredentialID, modifiedOn: moment.utc().format() } })
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
            add: add,
            update: update,
            remove: remove,
            reset: reset,
            unlock: unlock,
            getUserRoles: getUserRoles,
            getUserCredentials: getUserCredentials,
            addUserCredential: addUserCredential,
            updateUserCredential: updateUserCredential,
            removeUserCredential: removeUserCredential,
            activate: activate
        };
    }]);
