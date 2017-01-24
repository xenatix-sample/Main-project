angular.module('xenatixApp')
    .factory('settingsService', ["$http", "$q", 'settings', function ($http, $q, settings) {
        var CONFIG = {
            controllerAction: "/configuration/settings/"
        };
        function get() {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'GetNonUserSettings')
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function update(setting) {
            var dfd = $q.defer();

            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'UpdateSetting', setting)
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
            update: update
        };
    }]);
