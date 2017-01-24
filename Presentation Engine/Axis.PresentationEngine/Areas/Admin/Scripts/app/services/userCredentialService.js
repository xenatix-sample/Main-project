angular.module('xenatixApp')
   .factory('userCredentialService', ["$http", "$q", 'settings', function ($http, $q, settings) {
       var CONFIG = {
           apiControllerRoot: '/data/userCredential/'
       };

       function get(userID, isMyProfile) {
           var dfd = $q.defer();
           isMyProfile = isMyProfile || false;
           $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetUserCredentials', { params: { userID: userID, isMyProfile: isMyProfile } })
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       };

       function getwithServiceID(userID, moduleComponentID) {
           var dfd = $q.defer();
           $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetUserCredentialsWithServiceID', { params: { userID: userID, moduleComponentID: moduleComponentID } })
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       };

       function update(userCredential, isMyProfile) {
           var dfd = $q.defer();

           $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'SaveUserCredentials', userCredential, { params: { isMyProfile: isMyProfile } })
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
           getwithServiceID: getwithServiceID,
           update: update
       };
   }]);