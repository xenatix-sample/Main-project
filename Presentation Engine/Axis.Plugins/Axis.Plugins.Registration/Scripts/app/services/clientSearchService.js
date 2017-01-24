angular.module('xenatixApp')
    .factory('clientSearchService', [
        '$http', '$q', 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
            var CONFIG = {
                apiControllerRoot: "/data/plugins/registration/ClientSearch/",
                offlineApiUrl: '/ClientSearch/'
            };

            //Get the patients detail based on the search text
            function getClientSummary(searchCriteria,contactType) {
                var deferred = $q.defer();
                searchCriteria = searchCriteria.trim();
                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetClientSummary', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + contactType.toString() + '/' + encodeURI(searchCriteria.toUpperCase())), params: { searchCriteria: searchCriteria, contactType: contactType } })
                    .success(function(data, status, headers, config) {
                        deferred.resolve(data);
                    })
                    .error(function(data, status, headers, config) {
                        deferred.reject(status);
                    });
                return deferred.promise;
            };


            return {
                getClientSummary: getClientSummary
            };
        }
    ]);