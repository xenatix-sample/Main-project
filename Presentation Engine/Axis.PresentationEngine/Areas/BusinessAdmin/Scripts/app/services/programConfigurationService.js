(function () {
    angular.module('xenatixApp')
       .factory('programConfigurationService', ["$http", "$q", 'settings', function ($http, $q, settings) {
           var CONFIG = {
               apiControllerRoot: '/data/Program/'
           };

           function getPrograms() {
               var dfd = $q.defer();
               $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetPrograms')
               .success(function (data, status, header, config) {
                   dfd.resolve(data);
               })
               .error(function (data, status, header, config) {
                   dfd.reject(status);
               });

               return dfd.promise;
           };

           function getProgramByID(programID) {
               var dfd = $q.defer();
               $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetProgramByID', { params: { programID: programID } })
               .success(function (data, status, header, config) {
                   dfd.resolve(data);
               })
               .error(function (data, status, header, config) {
                   dfd.reject(status);
               });

               return dfd.promise;
           };

           function saveProgram(program) {
               var dfd = $q.defer();
               $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'SaveProgram', program)
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
               getPrograms: getPrograms,
               getProgramByID: getProgramByID,
               saveProgram: saveProgram
           };
       }]);
}());