angular.module('xenatixApp')
    .factory('referralAdditionalDetailService', [
        "$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
            var CONFIG = {
                apiControllerRoot: "/data/plugins/Registration/ReferralAdditionalDetail/",
            offlineApiUrl: '/Registration/ReferralAdditionalDetail/'
        };

        var editStateFunc = function editStateSettings() {
            return {
                description: 'Referral Additional Detail',
                state: 'eciregistration.referral',
                stateParams: { ContactID: this.ContactID }
            };
        };
        function getReferral(contactID) {
            var dfd = $q.defer();

                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetReferralAdditionalDetail', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { childKey: 'ReferralAdditionalDetailID' }), params: { contactID: contactID } })
                    .success(function (data, status, header, config) {
                        dfd.resolve(data);
                    })
                    .error(function (data, status, header, config) {
                        dfd.reject(status);
                    });

            return dfd.promise;
        };

            function getReferralDetails(contactID) {
                var dfd = $q.defer();
                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getReferralsDetails', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { childKey: 'ReferralAdditionalDetailID' }), params: { contactID: contactID } })
                    .success(function (data, status, header, config) {
                        dfd.resolve(data);
                    })
                    .error(function (data, status, header, config) {
                        dfd.reject(status);
                    });
                return dfd.promise;
            };

            function add(referral) {
                var dfd = $q.defer();
                if (!('ReferralAdditionalDetailID' in referral))
                    referral.ReferralAdditionalDetailID = 0;
                referral.ModifiedOn = moment.utc().toDate();
                var data = $.extend(true, {}, referral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + referral.ContactID + '/' + (referral.ReferralAdditionalDetailID || 0).toString(), 'ReferralAdditionalDetailID', {
                    parentKey: 'ContactID', referenceKeys: ["HeaderContactID", "ReferralHeaderID"], editState: editStateFunc.toString()
                }));
                $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddReferralAdditionalDetail', data)
                    .success(function (data, status, header, config) {
                        dfd.resolve(data);
                    })
                    .error(function (data, status, header, config) {
                        dfd.reject(status);
                    });

            return dfd.promise;
        };

            function update(referral) {
                var dfd = $q.defer();
                referral.ModifiedOn = moment.utc().toDate();

            var data = $.extend(true, {}, referral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + referral.ContactID + '/' + (referral.ReferralAdditionalDetailID || 0).toString(), 'ReferralAdditionalDetailID', { parentKey: 'ContactID', referenceKeys: ["HeaderContactID", "ReferralHeaderID"], editState: editStateFunc.toString() }));
                $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateReferralAdditionalDetail', data)
                    .success(function (data, status, header, config) {
                        dfd.resolve(data);
                    })
                    .error(function (data, status, header, config) {
                        dfd.reject(status);
                    });

                return dfd.promise;
            };

            function remove(contactID) {
                var dfd = $q.defer();

                $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'DeleteReferralDetails', {
                    data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID'), params: {
                        contactID: contactID
                    }
                })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };
            return {
                getReferral: getReferral,
                add: add,
                update: update,
                getReferralDetails: getReferralDetails,
                remove: remove
            };
        }
    ]);
