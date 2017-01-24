angular.module('xenatixApp')
    .factory('patientProfileService', [
            "$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {             
                var CONFIG = {
                    apiControllerRoot: "/data/plugins/registration/PatientProfile/",
                    offlineApiUrl: '/Registration/PatientProfile/'
                };
                function get(contactID) {
                    var dfd = $q.defer();

                    $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetPatientProfile', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString()), params: { contactID: contactID } })
                    .success(function (data, status, header, config) {
                        dfd.resolve(data);
                    })
                    .error(function (data, status, header, config) {
                        dfd.reject(status);
                    });

                    return dfd.promise;
                };

                return {
                    get: get
                };
            }
    ]
    );