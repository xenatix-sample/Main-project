(function () {
    angular.module('xenatixApp')
       .factory('serviceDetailsService', ["$http", "$q", 'settings', function ($http, $q, settings) {
           var CONFIG = {
               apiControllerRoot: '/data/ServiceDetails/'
           };


           function getServiceWorkflows(servicesID) {
               var dfd = $q.defer();
               $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetServiceWorkflows', { params: { servicesID: servicesID } })
               .success(function (data, status, header, config) {
                   dfd.resolve(data);
               })
               .error(function (data, status, header, config) {
                   dfd.reject(status);
               });

               return dfd.promise;
           };

           function saveServiceDetails(serviceDetails) {
               var dfd = $q.defer();
               $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'SaveServiceDetails', serviceDetails)
                   .then(function (data, status, header, config) {
                       dfd.resolve(data);
                   }, function (data, status, header, config) {
                       dfd.reject(status);
                   }, function (notification) {
                       dfd.notify(notification);
                   });

               return dfd.promise;
           };

           function getServiceDetails(servicesID,moduleComponentID) {
               var dfd = $q.defer();
               $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetServiceDetails', { params: { servicesID: servicesID, moduleComponentID: moduleComponentID } })
               .success(function (data, status, header, config) {
                   dfd.resolve(data);
               })
               .error(function (data, status, header, config) {
                   dfd.reject(status);
               });

               return dfd.promise;
           };


           return {
               getServiceWorkflows: getServiceWorkflows,
               saveServiceDetails: saveServiceDetails,
               getServiceDetails: getServiceDetails
           };
       }]);
}());