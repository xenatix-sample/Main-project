(function () {
    angular.module('xenatixApp')
           .factory('contactSSNService', ["$http", "$q", "settings", function ($http, $q, settings) {
               var CONFIG = {
                   apiController: "/data/plugins/registration/registration/"
               };
               // This service will work only in online mode because it's totally depends on SQL server.
               // Get Data
               function get(contactID) {
                   var dfd = $q.defer();
                   $http.get(settings.webApiBaseUrl + CONFIG.apiController + 'GetSSN', { params: { contactID: contactID } })
                   .success(function (data, status, header, config) {
                       dfd.resolve(data);
                   })
                   .error(function (data, status, header, config) {
                       dfd.reject(status);
                   });

                   return dfd.promise;
               };

               function refreshSSN(contactID, model) {
                   return get(contactID).then(function (data) {
                       if (hasData(data) > 0)
                           model.SSN = data.DataItems[0];
                   });
               }

               return {
                   get: get,
                   refreshSSN: refreshSSN
               }
           }]);
}());
