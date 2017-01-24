angular.module('xenatixApp')
   .factory('clientMergeService', ["$http", "$q", 'settings', function ($http, $q, settings) {
       var CONFIG = {
           apiControllerRoot: '/data/ClientMerge/'
       };

       function getClientMergeCounts() {
           var dfd = $q.defer();
           $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetClientMergeCounts')
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       };

       function mergeRecords(clientMerge) {
           var dfd = $q.defer();
           $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'MergeRecords', clientMerge)
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       };

       function getPotentialMatches() {
           var dfd = $q.defer();
           $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetPotentialMatches')
           .success(function (data, status, header, config) {
               dfd.resolve(data)
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });
           return dfd.promise;
       };

       function getMergedRecords() {
           var dfd = $q.defer();
           $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetMergedRecords')
           .success(function (data, status, header, config) {
               dfd.resolve(data)
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });
           return dfd.promise;
       };

       function unMergeRecords(mergedContactsMappingID) {
           var dfd = $q.defer();
           $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UnMergeRecords', mergedContactsMappingID)
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       };

       function getRefreshPotentialMatches() {
           var dfd = $q.defer();
           $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetRefreshPotentialMatches')
           .success(function (data, status, header, config) {
               dfd.resolve(data)
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });
           return dfd.promise;
       };

       function getPotentialMergeContactsLastRun() {
           var dfd = $q.defer();
           $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetPotentialMergeContactsLastRun')
           .success(function (data, status, header, config) {
               dfd.resolve(data)
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });
           return dfd.promise;
       };

       return {
           getClientMergeCounts: getClientMergeCounts,
           mergeRecords: mergeRecords,
           getPotentialMatches: getPotentialMatches,
           getMergedRecords: getMergedRecords,
           unMergeRecords: unMergeRecords,
           getRefreshPotentialMatches: getRefreshPotentialMatches,
           getPotentialMergeContactsLastRun: getPotentialMergeContactsLastRun
       };
   }]);