(function () {
    angular.module('xenatixApp')
       .factory('planAddressesService', ["$http", "$q", 'settings', function ($http, $q, settings) {
           var CONFIG = {
               apiControllerRoot: '/data/PlanAddresses/'
           };

           
           function getPlanAddresses(payorPlanID) {
               var dfd = $q.defer();

               $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetPlanAddresses', { params: { payorPlanID: payorPlanID } })
               .success(function (data, status, header, config) {
                   dfd.resolve(data);
               })
               .error(function (data, status, header, config) {
                   dfd.reject(status);
               });

               return dfd.promise;
           };

           function getPlanAddress(payorAddressID) {
               var dfd = $q.defer();
               $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetPlanAddress', { params: { payorAddressID: payorAddressID } })
               .success(function (data, status, header, config) {
                   dfd.resolve(data);
               })
               .error(function (data, status, header, config) {
                   dfd.reject(status);
               });

               return dfd.promise;
           };

           function addPlanAddress(payorPlanDetails) {
               var dfd = $q.defer();
               $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddPlanAddress', payorPlanDetails)
                   .then(function (data, status, header, config) {
                       dfd.resolve(data);
                   }, function (data, status, header, config) {
                       dfd.reject(status);
                   }, function (notification) {
                       dfd.notify(notification);
                   });

               return dfd.promise;
           };

           function updatePlanAddress(payorPlanDetails) {
               var dfd = $q.defer();
               $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdatePlanAddress', payorPlanDetails)
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
               getPlanAddresses: getPlanAddresses,
               getPlanAddress: getPlanAddress,
               addPlanAddress: addPlanAddress,
               updatePlanAddress: updatePlanAddress
              
           };
       }]);
}());