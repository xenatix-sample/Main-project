(function () {
    angular.module('xenatixApp')
       .factory('serviceDefinitionService', ["$http", "$q", 'settings', function ($http, $q, settings) {
           var CONFIG = {
               apiControllerRoot: '/data/ServiceDefinition/'
           };

           function getServices(searchText) {
               var dfd = $q.defer();
               $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetServices', { params: { searchText: searchText || "" } })
               .success(function (data, status, header, config) {
                   dfd.resolve(data);
               })
               .error(function (data, status, header, config) {
                   dfd.reject(status);
               });

               return dfd.promise;
           };
        
           function getServiceDefinitionByID(serviceID) {
               var dfd = $q.defer();

               $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetServiceDefinitionByID', { params: { serviceID: serviceID } })
               .success(function (data, status, header, config) {
                   dfd.resolve(data);
               })
               .error(function (data, status, header, config) {
                   dfd.reject(status);
               });

               return dfd.promise;
           };

           function saveServiceDefinition(serviceDefinition) {
               var dfd = $q.defer();
               $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'SaveServiceDefinition', serviceDefinition)
                   .then(function (data, status, header, config) {
                       dfd.resolve(data);
                   }, function (data, status, header, config) {
                       dfd.reject(status);
                   }, function (notification) {
                       dfd.notify(notification);
                   });

               return dfd.promise;
           };

           
           return {
               getServices: getServices,
               getServiceDefinitionByID: getServiceDefinitionByID,
               saveServiceDefinition: saveServiceDefinition
              
              
           };
       }]);
}());