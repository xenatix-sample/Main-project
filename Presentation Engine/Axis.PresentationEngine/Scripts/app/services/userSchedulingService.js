angular.module('xenatixApp')
   .factory('userSchedulingService', ["$http", "$q", 'settings', function ($http, $q, settings) {
       var CONFIG = {
           apiControllerRoot: '/data/userscheduling/'
       };

       function getUserFacilities(userID, isMyProfile) {
           var dfd = $q.defer();

           $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetUserFacilities', { params: { userID: userID, isMyProfile: isMyProfile } })
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       };

       function getUserFacilitySchedule(userID, facilityID, isMyProfile) {
           var dfd = $q.defer();

           $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetUserFacilitySchedule', { params: { userID: userID, facilityID: facilityID, isMyProfile: isMyProfile } })
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       };

       function saveUserFacilitySchedule(userFacilitySchedule, isMyProfile) {
           var dfd = $q.defer();

           $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'SaveUserFacilitySchedule', userFacilitySchedule, { params: { isMyProfile: isMyProfile } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

           return dfd.promise;
       }

       return {
           getUserFacilities: getUserFacilities,
           getUserFacilitySchedule: getUserFacilitySchedule,
           saveUserFacilitySchedule: saveUserFacilitySchedule
       };


   }]);