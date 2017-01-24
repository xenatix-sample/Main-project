(function () {
    angular.module('xenatixApp')
        .factory('consentService', [
            "$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
                var CONFIG = {
                    apiControllerRoot: "/data/Plugins/Registration/Consent/",
                    offlineApiUrl: '/Registration/Consent/'
                };
                var editStateFunc = function editStateSettings() {
                    return {
                        description: 'Consent ',
                        state: 'registration.consent',
                        stateParams: { ContactID: this.ContactID }
                    };
                };
                function addConsentSignature(consentModel) {
                    var dfd = $q.defer();
                    var data = $.extend(true, {}, consentModel, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (consentModel.ContactID || 0), '', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
                    $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddConsentSignature', data)
                        .then(function (data, status, header, config) {
                            dfd.resolve(data);
                        }, function (data, status, header, config) {
                            dfd.reject(status);
                        }, function (notification) {
                            dfd.notify(notification);
                        });

                    return dfd.promise;
                };

                function getConsentSignature(contactId) {
                    var dfd = $q.defer();
                    $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetConsentSignature', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactId || 0).toString()), params: { contactId: contactId } })
                    .success(function (data, status, header, config) {
                        dfd.resolve(data);
                    })
                    .error(function (data, status, header, config) {
                        dfd.reject(status);
                    });

                    return dfd.promise;
                };

                return {
                    addConsentSignature: addConsentSignature,
                    getConsentSignature: getConsentSignature
                };
            }
        ]);
}());