angular.module('xenatixApp')
   .factory('userSecurityService', ["$http", "$q", 'settings', function ($http, $q, settings) {
       var CONFIG = {
           apiControllerRoot: '/account/userSecurity/'
       };

       function get(userID) {
           var dfd = $q.defer();

           $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetUserSecurityQuestions', { params: { userID: userID } })
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       };

       function save(securityQuestions) {
           var dfd = $q.defer();

           $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'SaveUserSecurityQuestions', securityQuestions)
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       };

       function saveUserPassword(userProfile) {
           var dfd = $q.defer();

           $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'SaveUserPassword', userProfile)
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       };

       function updateUserSignatureDetails(userProfile) {
           var dfd = $q.defer();

           $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateUserSignatureDetails', userProfile)
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
           save: save,
           saveUserPassword: saveUserPassword,           
           updateUserSignatureDetails: updateUserSignatureDetails
       };
   }]);