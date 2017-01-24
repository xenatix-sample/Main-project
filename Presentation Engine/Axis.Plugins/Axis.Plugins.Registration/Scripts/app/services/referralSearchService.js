angular.module("xenatixApp")
    .factory("referralSearchService", ["$http", "$q", "settings", "offlineData", function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: "/data/Plugins/Registration/ReferralSearch/",
            offlineApiUrl: "/ReferralSearch/"
        };

        function get(searchStr, searchType,userID) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "GetReferrals", { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + encodeURI(searchStr ? searchStr.toUpperCase() : "")), params: { searchStr: searchStr, searchType: searchType, userID: userID } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                }).error(function (data, status, header, config) {
                    dfd.reject(status);
                });
            return dfd.promise;
        };

        function remove(id, reasonForDelete) {
            var dfd = $q.defer()
            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "DeleteReferral", { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + '/' + (id || 0).toString(), 'ReferralHeaderID'), params: { id: id, reasonForDelete: reasonForDelete, modifiedOn: moment.utc().format() } })
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
            remove: remove
        };

    }]);