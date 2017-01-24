angular.module('xenatixApp')
   .factory('voidService', ["$http", "$q", 'settings', 'offlineData', '$filter', function ($http, $q, settings, offlineData, $filter) {
       var CONFIG = {
           controllerAction: '/RecordedServices/VoidService/',
           offlineApiUrl: '/VoidService/'
       };

       function voidRecordedService(voidServiceModel) {
           var dfd = $q.defer();
           if (!voidServiceModel.ServiceRecordingVoidID)
               voidServiceModel.ServiceRecordingVoidID = 0;
           voidServiceModel.ModifiedOn = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY HH:mm:ss');
           var data = $.extend(true, {}, voidServiceModel, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (voidServiceModel.ServiceRecordingVoidID || 0).toString(), 'ServiceRecordingVoidID'));
           $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'VoidRecordedService', data)
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       };

       function voidCallCenterRecordedService(voidServiceModel) {
           var dfd = $q.defer();
           if (!voidServiceModel.ServiceRecordingVoidID)
               voidServiceModel.ServiceRecordingVoidID = 0;
           voidServiceModel.ModifiedOn = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY HH:mm:ss');
           var data = $.extend(true, {}, voidServiceModel, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (voidServiceModel.ServiceRecordingVoidID || 0).toString(), 'ServiceRecordingVoidID'));
           $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'VoidServiceCallCenter', data)
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       };

       function getVoidRecordedService(serviceRecordingID) {
           var dfd = $q.defer();

           $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'GetVoidRecordedService', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (serviceRecordingID || 0).toString(), 'serviceRecordingID'), params: { serviceRecordingID: serviceRecordingID } })
           .success(function (data, status, header, config) {
               dfd.resolve(data);
           })
           .error(function (data, status, header, config) {
               dfd.reject(status);
           });

           return dfd.promise;
       };

       return {
           voidRecordedService: voidRecordedService,
           voidCallCenterRecordedService: voidCallCenterRecordedService,
           getVoidRecordedService: getVoidRecordedService
       };
   }]);