angular.module('xenatixApp')
   .factory('divisionProgramService', ["$http", "$q", 'settings', function ($http, $q, settings) {
       var CONFIG = {
           apiControllerRoot: '/data/DivisionProgram/'
       };

       function getDivisionPrograms(userID, isMyProfile) {
           var dfd = $q.defer();

           $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getDivisionPrograms', { params: { userID: userID, isMyProfile: isMyProfile } })
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       };

       function saveDivisionProgram(divisionPrograms, isMyProfile) {
           var dfd = $q.defer();

           $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'saveDivisionProgram', divisionPrograms, { params: { isMyProfile: isMyProfile } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

           return dfd.promise;
       }

       return {
           getDivisionPrograms: getDivisionPrograms,
           saveDivisionProgram: saveDivisionProgram
       };


   }]);