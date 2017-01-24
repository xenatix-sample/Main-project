(function () {
    angular.module('xenatixApp')
           .factory('commonService', ["$http", "$q", "settings", function ($http, $q, settings) {

               // Get Data
               function get(controller, action, params) {
                   var dfd = $q.defer();

                   $http.get(settings.webApiBaseUrl +controller + action, { params: params })
                   .success(function (data, status, header, config) {
                       dfd.resolve(data);
                   })
                   .error(function (data, status, header, config) {
                       dfd.reject(status);
                   });

                   return dfd.promise;
               };

               // Perform add operation
               function add(controller, action, model) {
                   var dfd = $q.defer();
                   $http.post(settings.webApiBaseUrl + controller + action, model)
                   .success(function (data, status, header, config) {
                       dfd.resolve(data);
                   })
                   .error(function (data, status, header, config) {
                       dfd.reject(status);
                   });

                   return dfd.promise;
               };

               // For Update 
               function update(controller, action, model) {
                   var dfd = $q.defer();

                   $http.put(settings.webApiBaseUrl + controller + action, model)
                   .success(function (data, status, header, config) {
                       dfd.resolve(data);
                   })
                   .error(function (data, status, header, config) {
                       dfd.reject(status);
                   });

                   return dfd.promise;
               };

               // For Remove/Delete 
               function remove(controller, action, params) {
                   var dfd = $q.defer();

                   $http.delete(settings.webApiBaseUrl + controller + action, { params: params })
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
                   add: add,
                   update: update,
                   remove:remove
               };
           }]);
}());