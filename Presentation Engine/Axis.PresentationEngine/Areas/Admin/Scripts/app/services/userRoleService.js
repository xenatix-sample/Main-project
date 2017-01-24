angular.module('xenatixApp')
   .factory('userRoleService', ["$http", "$q", 'settings', function ($http, $q, settings) {
       var CONFIG = {
           apiControllerRoot: '/data/userRole/'
       };

       function get(userID) {
           var dfd = $q.defer();

           $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetUserRoles', { params: { userID: userID } })
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       };

       function update(user) {
           var dfd = $q.defer();

           $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'SaveUserRoles', user)
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       }

       return {
           get: get,
           update: update
       };
   }]);