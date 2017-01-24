angular.module('xenatixApp')
    .factory('navigationService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
        var CONFIG = {
            controllerAction: "/Account/Account/",
            offlineApiUrl: '/Account/NavigationItems/'
        };
        var navigationData = null;
        function get(needLatest) {
            var dfd = $q.defer();

            if (!hasData(navigationData) || needLatest) {
            var dfd = $q.defer();
                $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'GetNavigationItems', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl) })
            .success(function (data, status, header, config) {
                    navigationData = data;
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });
            }
            else {
                dfd.resolve(navigationData);
            }

            return dfd.promise;
        };
        function getUserDetails() {
            return navigationData;
        }
        return {
            get: get,
            userDetails: getUserDetails
        };
    }]);