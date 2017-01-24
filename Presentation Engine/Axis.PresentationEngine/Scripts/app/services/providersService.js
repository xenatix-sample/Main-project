(function () {
    angular.module('xenatixApp')
           .service('providersService', ["$http", "$q", "settings", function ($http, $q, settings) {
               // Get Data
               var CONFIG = {
                   apiControllerRoot: "/data/Providers/",
               }
               function getProviders(filterCriteria) {
                   var dfd = $q.defer();

                   $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getProviders', { params: { filterCriteria: filterCriteria } })
                   .success(function (data, status, header, config) {
                       dfd.resolve(data);
                   })
                   .error(function (data, status, header, config) {
                       dfd.reject(status);
                   });

                   return dfd.promise;
               };


               function getProviderbyid(providerID) {
                   var dfd = $q.defer();

                   $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getProviderbyid', { params: { providerID: providerID } })
                   .success(function (data, status, header, config) {
                       dfd.resolve(data);
                   })
                   .error(function (data, status, header, config) {
                       dfd.reject(status);
                   });

                   return dfd.promise;
               };

               return {
                   getProviders: getProviders,
                   getProviderbyid: getProviderbyid
               };
           }]);
}());