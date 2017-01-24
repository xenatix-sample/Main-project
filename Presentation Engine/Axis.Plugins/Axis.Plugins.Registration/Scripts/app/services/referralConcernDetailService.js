angular.module('xenatixApp')
    .factory('referralConcernDetailService', [
        "$http", "$q", 'settings', 'offlineData', function($http, $q, settings, offlineData) {
            var CONFIG = {
                apiControllerRoot: "/data/plugins/Registration/ReferralConcernDetail/",
                offlineApiUrl: '/Registration/ReferralConcernDetail/'
            };
            var editStateFunc = function editStateSettings() {
                return {
                    description: 'Referral Detail',
                    state: 'registration.referral',
                    stateParams: { ContactID: this.ContactID }
                };
            };
            function getReferralConcernDetail(referralAdditionalDetailID) {
                var dfd = $q.defer();
                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetReferralConcernDetail', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referralAdditionalDetailID || 0).toString(), 'ReferralAdditionalDetailID', { childKey: 'ReferralConcernDetailID' }), params: { referralAdditionalDetailID: referralAdditionalDetailID } })
                    .success(function(data, status, header, config) {
                        dfd.resolve(data);
                    })
                    .error(function(data, status, header, config) {
                        dfd.reject(status);
                    });

                return dfd.promise;
            };

            function add(referral) {
                var dfd = $q.defer();
                if (!('ReferralConcernDetailID' in referral))
                    referral.ReferralConcernDetailID = 0;
                var data = $.extend(true, {}, referral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referral.ReferralAdditionalDetailID || 0).toString() + '/' + (referral.ReferralConcernDetailID || 0).toString(), 'ReferralConcernDetailID', { parentKey: 'ReferralConcernDetailID', referenceKeys: ['ReferralAdditionalDetailID'], editState: editStateFunc.toString() }));
                $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddReferralConcernDetail', data)
                    .success(function(data, status, header, config) {
                        dfd.resolve(data);
                    })
                    .error(function(data, status, header, config) {
                        dfd.reject(status);
                    });

                return dfd.promise;
            };

            function update(referral) {
                var dfd = $q.defer();
                var data = $.extend(true, {}, referral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referral.ReferralAdditionalDetailID || 0).toString() + '/' + (referral.ReferralConcernDetailID || 0).toString(), 'ReferralConcernDetailID', { parentKey: 'ReferralAdditionalDetailID', editState: editStateFunc.toString() }));
                $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateReferralConcernDetail', data)
                    .success(function(data, status, header, config) {
                        dfd.resolve(data);
                    })
                    .error(function(data, status, header, config) {
                        dfd.reject(status);
                    });

                return dfd.promise;
            };

            function remove(referralConcernDetailID, referralAdditionalDetailID) {
                var dfd = $q.defer();
                var url = {
                    data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referralAdditionalDetailID || 0).toString() + '/' + (referralConcernDetailID || 0).toString(), 'ReferralConcernDetailID', { parentKey: 'ReferralAdditionalDetailID' }),
                    params: { referralConcernDetailID: referralConcernDetailID, modifiedOn: moment.utc().format() }
                };

                $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "DeleteReferralConcernDetail", url)
                    .success(function(data, status, header, config) {
                        dfd.resolve(data);
                    })
                    .error(function(data, status, header, config) {
                        dfd.reject(status);
                    });
                return dfd.promise;
            };

            return {
                getReferralConcernDetail: getReferralConcernDetail,
                add: add,
                update: update,
                remove: remove
            };
        }
    ]);