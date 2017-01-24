angular.module("xenatixApp")
    .factory("lawLiaisonSummaryService", ["$http", "$q", "settings", "offlineData", function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: "/data/Plugins/CallCenter/LawLiaisonSummary/",
            offlineApiUrl: "/LawLiaisonSummary/"
        };

        function get(searchStr, userID, searchTypeFilter, callStatusId, userIDFilter, isNormalView) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "GetLawLiaisonCallCenterSummary", { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + encodeURI(searchStr ? searchStr.toUpperCase() : "")), params: { searchStr: searchStr, userID: userID, searchTypeFilter: searchTypeFilter, callStatusID: callStatusId, userIDFilter: userIDFilter, isNormalView: isNormalView } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                }).error(function (data, status, header, config) {
                    dfd.reject(status);
                });
            return dfd.promise;
        };

        function getLawLiaisonIncident(callCenterHeaderID) {
            var dfd = $q.defer()
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "GetLawLiaisonIncident", { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + '/' + (callCenterHeaderID || 0).toString(), 'CallCenterHeaderID'), params: { callCenterHeaderID: callCenterHeaderID } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });
            return dfd.promise;
        };

        function remove(id) {
            var dfd = $q.defer()
            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "Delete", { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + '/' + (id || 0).toString(), 'CallCenterID'), params: { id: id, modifiedOn: moment.utc().format() } })
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
            remove: remove,
            getLawLiaisonIncident: getLawLiaisonIncident
        };

    }]);
