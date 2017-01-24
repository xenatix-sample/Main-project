angular.module('xenatixApp')
   .factory('userDetailService', ["$http", "$q", 'settings', function ($http, $q, settings) {
       var CONFIG = {
           apiControllerRoot: '/data/userDetail/',
           apiControllerAddtlDetails: '/data/userAdditionalDetails/'
       };

       function get(userID) {
           var dfd = $q.defer();

           $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetUser', { params: { userID: userID } })
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       };

       function add(userDetail) {
           var dfd = $q.defer();
           $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddUser', userDetail)
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       }

       function update(userDetail) {
           var dfd = $q.defer();

           $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateUser', userDetail)
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       }

       function getCoSignatures(userID) {
           var dfd = $q.defer();

           $http.get(settings.webApiBaseUrl + CONFIG.apiControllerAddtlDetails + 'GetCoSignatures', { params: { userID: userID } })
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       };

       function getUserAdditionalDetails(userID) {
           var dfd = $q.defer();

           $http.get(settings.webApiBaseUrl + CONFIG.apiControllerAddtlDetails + 'GetUserAdditionalDetails', { params: { userID: userID } })
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       };

       function getUserIdentifierDetails(userID) {
           var dfd = $q.defer();

           $http.get(settings.webApiBaseUrl + CONFIG.apiControllerAddtlDetails + 'GetUserIdentifierDetails', { params: { userID: userID } })
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       };

       function addCoSignatures(userDetail) {
           var dfd = $q.defer();
           $http.post(settings.webApiBaseUrl + CONFIG.apiControllerAddtlDetails + 'AddCoSignatures', userDetail)
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       }

       function addUserIdentifierDetails(userDetail) {
           var dfd = $q.defer();
           $http.post(settings.webApiBaseUrl + CONFIG.apiControllerAddtlDetails + 'AddUserIdentifierDetails', userDetail)
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       }

       function addUserAdditionalDetails(userDetail) {
           var dfd = $q.defer();
           $http.post(settings.webApiBaseUrl + CONFIG.apiControllerAddtlDetails + 'AddUserAdditionalDetails', userDetail)
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       }

       function updateCoSignatures(sig) {
           var dfd = $q.defer();

           $http.put(settings.webApiBaseUrl + CONFIG.apiControllerAddtlDetails + 'UpdateCoSignatures', sig)
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       }

       function updateUserIdentifierDetails(userDetail) {
           var dfd = $q.defer();

           $http.put(settings.webApiBaseUrl + CONFIG.apiControllerAddtlDetails + 'UpdateUserIdentifierDetails', userDetail)
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       }

       function updateUserAdditionalDetails(userDetail) {
           var dfd = $q.defer();

           $http.put(settings.webApiBaseUrl + CONFIG.apiControllerAddtlDetails + 'UpdateUserAdditionalDetails', userDetail)
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       }


       function deleteCoSignatures(id) {
           var dfd = $q.defer();

           $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerAddtlDetails + 'DeleteCoSignatures',
               { params: { id: id, modifiedOn: moment.utc().format() } })
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       }

       function deleteUserAdditionalDetails(id) {
           var dfd = $q.defer();

           $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerAddtlDetails + 'DeleteUserAdditionalDetails',
               { params: { id: id, modifiedOn: moment.utc().format() } })
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       }

       function deleteUserIdentifierDetails(id) {
           var dfd = $q.defer();

           $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerAddtlDetails + 'DeleteUserIdentifierDetails',
               { params: { id: id, modifiedOn: moment.utc().format() } })
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
           add: add,
           update: update,
           getCoSignatures:getCoSignatures,
           getUserAdditionalDetails: getUserAdditionalDetails,
           getUserIdentifierDetails: getUserIdentifierDetails,
           addCoSignatures : addCoSignatures,
           addUserIdentifierDetails: addUserIdentifierDetails,
           addUserAdditionalDetails : addUserAdditionalDetails,
           updateCoSignatures: updateCoSignatures,
           updateUserIdentifierDetails :updateUserIdentifierDetails,
           updateUserAdditionalDetails : updateUserAdditionalDetails,
           deleteCoSignatures: deleteCoSignatures,
           deleteUserAdditionalDetails :deleteUserAdditionalDetails,
           deleteUserIdentifierDetails: deleteUserIdentifierDetails
       };
   }]);