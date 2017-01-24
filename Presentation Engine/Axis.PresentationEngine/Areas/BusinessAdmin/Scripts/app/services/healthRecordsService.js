(function () {
    angular.module('xenatixApp')
       .factory('healthRecordsService', ["$http", "$q", 'settings', function ($http, $q, settings) {
           var CONFIG = {
               apiControllerRoot: '/data/HealthRecords/'
           };

           function getHealthRecords() {
               var dfd = $q.defer();
               $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetHealthRecords')
               .success(function (data, status, header, config) {
                   dfd.resolve(data);
               })
               .error(function (data, status, header, config) {
                   dfd.reject(status);
               });

               return dfd.promise;
           };


           return {
               getHealthRecords: getHealthRecords
           };
       }]);
}());