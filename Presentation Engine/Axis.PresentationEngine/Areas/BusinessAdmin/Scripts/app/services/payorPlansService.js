(function () {
    angular.module('xenatixApp')
       .factory('payorPlansService', ["$http", "$q", 'settings', function ($http, $q, settings) {
           var CONFIG = {
               apiControllerRoot: '/data/PayorPlans/'
           };

          
           function getPayorPlans(payorId) {
               var dfd = $q.defer();

               $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetPayorPlans', { params: { payorID: payorId } })
               .success(function (data, status, header, config) {
                   dfd.resolve(data);
               })
               .error(function (data, status, header, config) {
                   dfd.reject(status);
               });

               return dfd.promise;
           };

           function getPayorPlanByID(payorPlanID) {
               var dfd = $q.defer();

               $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getPayorPlanByID', { params: { payorPlanID: payorPlanID } })
               .success(function (data, status, header, config) {
                   dfd.resolve(data);
               })
               .error(function (data, status, header, config) {
                   dfd.reject(status);
               });

               return dfd.promise;
           };

           function addPayorPlan(payorPlanDetails) {
               var dfd = $q.defer();
               $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddPayorPlan', payorPlanDetails)
                   .then(function (data, status, header, config) {
                       dfd.resolve(data);
                   }, function (data, status, header, config) {
                       dfd.reject(status);
                   }, function (notification) {
                       dfd.notify(notification);
                   });

               return dfd.promise;
           };

           function updatePayorPlan(payorPlanDetails) {
               var dfd = $q.defer();
               $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdatePayorPlan', payorPlanDetails)
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
               getPayorPlans: getPayorPlans,
               getPayorPlanByID: getPayorPlanByID,
               addPayorPlan:addPayorPlan,
               updatePayorPlan: updatePayorPlan

           };
       }]);
}());