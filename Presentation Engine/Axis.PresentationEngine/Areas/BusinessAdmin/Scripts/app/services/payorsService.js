(function () {
    angular.module('xenatixApp')
       .factory('payorsService', ["$http", "$q", 'settings', function ($http, $q, settings) {
           var CONFIG = {
               apiControllerRoot: '/data/Payors/'
           };

           function getPayors(searchText) {
               var dfd = $q.defer();
               $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetPayors', { params: { searchText: searchText || "" } })
               .success(function (data, status, header, config) {
                   dfd.resolve(data);
               })
               .error(function (data, status, header, config) {
                   dfd.reject(status);
               });

               return dfd.promise;
           };

           function addPayor(payorDetails) {
               var dfd = $q.defer();
               $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddPayor', payorDetails)
                   .then(function (data, status, header, config) {
                       dfd.resolve(data);
                   }, function (data, status, header, config) {
                       dfd.reject(status);
                   }, function (notification) {
                       dfd.notify(notification);
                   });

               return dfd.promise;
           };

           function updatePayor(payorDetails) {
               var dfd = $q.defer();
               $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdatePayor', payorDetails)
                   .then(function (data, status, header, config) {
                       dfd.resolve(data);
                   }, function (data, status, header, config) {
                       dfd.reject(status);
                   }, function (notification) {
                       dfd.notify(notification);
                   });

               return dfd.promise;
           };

           function getPayorByID(payorId) {
               var dfd = $q.defer();

               $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetPayorByID', { params: { payorId: payorId } })
               .success(function (data, status, header, config) {
                   dfd.resolve(data);
               })
               .error(function (data, status, header, config) {
                   dfd.reject(status);
               });

               return dfd.promise;
           };


           return {
               getPayors: getPayors,
               addPayor: addPayor,
               updatePayor: updatePayor,
               getPayorByID: getPayorByID


           };
       }]);
}());