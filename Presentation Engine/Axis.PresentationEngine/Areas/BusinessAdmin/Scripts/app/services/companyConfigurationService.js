(function () {
    angular.module('xenatixApp')
       .factory('companyConfigurationService', ["$http", "$q", 'settings', function ($http, $q, settings) {
           var CONFIG = {
               apiControllerRoot: '/data/Company/'
           };

           function getCompanies() {
               var dfd = $q.defer();
               $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetCompanies')
               .success(function (data, status, header, config) {
                   dfd.resolve(data);
               })
               .error(function (data, status, header, config) {
                   dfd.reject(status);
               });

               return dfd.promise;
           };

           function getCompanyByID(companyID) {
               var dfd = $q.defer();
               $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetCompanyByID', { params: { companyID: companyID } })
               .success(function (data, status, header, config) {
                   dfd.resolve(data);
               })
               .error(function (data, status, header, config) {
                   dfd.reject(status);
               });

               return dfd.promise;
           };

           function saveCompany(company) {
               var dfd = $q.defer();
               $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'SaveCompany', company)
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
               getCompanies: getCompanies,
               getCompanyByID: getCompanyByID,
               saveCompany: saveCompany
           };
       }]);
}());