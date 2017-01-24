angular.module("xenatixApp")
    .factory("groupSchedulingSearchService", ["$http", "$q", "settings", "offlineData", function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: "/data/Plugins/Scheduling/GroupSchedulingSearch/",
            offlineApiUrl: "/GroupScheduling/"
        };

        function get(searchStr) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "GetGroupSchedules", { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + encodeURI(searchStr ? searchStr.toUpperCase() : "")), params: { searchStr: searchStr } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                }).error(function (data, status, header, config) {
                    dfd.reject(status);
                });
            return dfd.promise;
        };

        function cancelGroupAppointment(appointment) {
            var dfd = $q.defer();
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'CancelGroupAppointment', appointment)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };


        return {
            get: get,
            cancelGroupAppointment: cancelGroupAppointment
        };

    }]);