angular.module('xenatixApp')
    .constant("CONFIG", {
        controllerAction: "/account/forgotPassword/"
    })
    .factory('forgotPasswordService', ["$http", "$q", "CONFIG", 'settings', function ($http, $q, CONFIG, settings) {
        function sendResetLink(email) {
            var dfd = $q.defer();

            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'sendResetLink', { email: email })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function verifyResetIdentifier(resetPassword) {
            var dfd = $q.defer();
            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'verifyResetIdentifier', resetPassword)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function verifySecurityDetails(resetPassword) {
            var dfd = $q.defer();
            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'verifySecurityDetails', resetPassword)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function resetPassword(resetPassword) {
            var dfd = $q.defer();
            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'resetPassword', resetPassword)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getSecurityQuestions() {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getSecurityQuestions')
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            sendResetLink: sendResetLink,
            verifySecurityDetails: verifySecurityDetails,
            verifyResetIdentifier: verifyResetIdentifier,
            resetPassword: resetPassword,
            getSecurityQuestions: getSecurityQuestions
        };
    }]);