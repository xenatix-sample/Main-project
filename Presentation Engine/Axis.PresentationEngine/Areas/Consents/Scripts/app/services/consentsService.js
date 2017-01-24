(function () {
    angular.module('xenatixApp')
    .factory('consentsService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: '/data/consents/',
            offlineApiUrl: '/consents/',
            offlineExpireApiUrl: '/consentsExpire/',
        };

        var editStateFunc = function editStateSettings() {
            return {
                description: 'Consents',
                state: 'patientprofile.consents.agency.agencyView',
                stateParams: {
                    ContactID: this.ContactID
                }
            };
        };

        function get(contactID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetConsents',
                        {
                            data: offlineData.getOfflineSettings(
                            CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { childKey: 'ContactConsentID' }), params: { contactID: contactID }
                        })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        var add = function (consent) {
            var dfd = $q.defer();
            if (!consent.ContactConsentID)
                consent.ContactConsentID = 0;
            var data = $.extend(true, {}, consent, offlineData.getOfflineSettings(
                                                    CONFIG.offlineApiUrl + (consent.ContactID || 0).toString() + '/' + (consent.ContactConsentID || 0).toString(),
                                                    'ContactConsentID', {
                                                        parentKey: 'ContactID',
                                                        referenceKeys: ['ContactConsentID', 'ResponseID'],
                                                        editState: editStateFunc.toString()
                                                    }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddConsent', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        }

        var update = function (consent, isExpire) {
            var dfd = $q.defer();
            //var offlineURL = isExpire ? CONFIG.offlineExpireApiUrl : CONFIG.offlineApiUrl;
            var data = $.extend(true, {}, consent, offlineData.getOfflineSettings(
                                                    CONFIG.offlineApiUrl + (consent.ContactID || 0).toString() + '/' + (consent.ContactConsentID || 0).toString(),
                                                    'ContactConsentID', {
                                                        parentKey: 'ContactID',
                                                        referenceKeys: ['ContactConsentID', 'ResponseID'],
                                                        editState: editStateFunc.toString()
                                                    }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateConsent', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            if (isExpire) {
                var dataExpire = $.extend(true, {}, consent, offlineData.getOfflineSettings(
                                                                    CONFIG.offlineExpireApiUrl + (consent.ContactID || 0).toString() + '/' + (consent.ContactConsentID || 0).toString(),
                                                                    'ContactConsentID', {
                                                                        parentKey: 'ContactID',
                                                                        referenceKeys: ['ContactConsentID', 'ResponseID', 'ExpiredResponseID'],
                                                                        editState: editStateFunc.toString()
                                                                    }));
                $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateConsent', dataExpire);
            }
            return dfd.promise;
        }

        return {
            get: get,
            update: update,
            add: add
        };
    }]);
}());