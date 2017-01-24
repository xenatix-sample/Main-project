angular.module('xenatixApp')
   .factory('roleManagementService', ["$http", "$q", 'settings', function ($http, $q, settings) {
       var CONFIG = {
           apiControllerRoot: '/data/RoleManagement/'
       };

       function getRoleModuleDetails(ModuleID) {
           var dfd = $q.defer();

           $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetRoleModuleDetails', { params: { ModuleID: ModuleID } })
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       };

       function getRoleModuleComponentDetails(RoleModuleID) {
           var dfd = $q.defer();

           $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetRoleModuleComponentDetails', { params: { RoleModuleID: RoleModuleID } })
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       };

       function getPermissions() {
           var dfd = $q.defer();

           $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetPermissions')
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       };

       function saveModulePermissions(roleModuleSave) {
           var dfd = $q.defer();

           $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'SaveModulePermissions', roleModuleSave)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

           return dfd.promise;
       };

       return {
           getRoleModuleDetails: getRoleModuleDetails,
           getRoleModuleComponentDetails: getRoleModuleComponentDetails,
           getPermissions: getPermissions,
           saveModulePermissions: saveModulePermissions
       };
   }]);