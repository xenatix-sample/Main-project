(function () {
    angular.module('xenatixApp')
       .factory('organizationStructureService', ["$http", "$q", 'settings', function ($http, $q, settings) {
           var CONFIG = {
               apiControllerRoot: '/data/OrganizationStructure/'
           };

           function getOrganizationStructureByID(detailID) {
               var dfd = $q.defer();
               $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetOrganizationStructureByID', { params: { detailID: detailID } })
               .success(function (data, status, header, config) {
                   dfd.resolve(data);
               })
               .error(function (data, status, header, config) {
                   dfd.reject(status);
               });

               return dfd.promise;
           };


           function getServiceOrganizationDetailsByID(detailID) {
               var dfd = $q.defer();
               $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetServiceOrganizationDetailsByID', { params: { detailID: detailID } })
               .success(function (data, status, header, config) {
                   dfd.resolve(data);
               })
               .error(function (data, status, header, config) {
                   dfd.reject(status);
               });

               return dfd.promise;
           };

           return {
               getOrganizationStructureByID: getOrganizationStructureByID,
               getServiceOrganizationDetailsByID: getServiceOrganizationDetailsByID
           };
       }]);
}());