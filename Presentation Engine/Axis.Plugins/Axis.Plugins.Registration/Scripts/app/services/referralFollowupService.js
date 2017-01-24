angular.module('xenatixApp')
    .factory('referralFollowupService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/registration/referralfollowup/",
            offlineApiUrl: '/referralFollowup/'
        };
        var editStateFunc = function editStateSettings() {
            return {
                description: 'Referral Followup',
                state: 'referrals.followup',
                stateParams: { ContactID: this.ContactID, ReferralHeaderID: this.ReferralHeaderID, ReadOnly: 'edit' }
            };
        };
        function getReferralFollowups(referralHeaderID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getReferralFollowups', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referralHeaderID || 0).toString(), 'ReferralHeaderID', { childKey: 'ReferralOutcomeDetailID' }), params: { referralHeaderID: referralHeaderID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getReferralFollowup(referralHeaderID, referralOutcomeDetailID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getReferralFollowup', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referralHeaderID || 0).toString() + '/' + (referralOutcomeDetailID || 0).toString(), 'ReferralOutcomeDetailID', { parentKey: 'ReferralHeaderID' }), params: { referralOutcomeDetailID: referralOutcomeDetailID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function addReferralFollowup(referral) {
            var dfd = $q.defer();
            if (!('ReferralOutcomeDetailID' in referral))
                referral.ReferralOutcomeDetailID = 0;
            var data = $.extend(true, {}, referral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referral.ReferralHeaderID || 0).toString() + '/' + (referral.ReferralOutcomeDetailID || 0).toString(), 'ReferralOutcomeDetailID', { parentKey: 'ReferralHeaderID', editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'addReferralFollowup', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function updateReferralFollowup(referral) {
            var dfd = $q.defer();

            var data = $.extend(true, {}, referral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referral.ReferralHeaderID || 0).toString() + '/' + (referral.ReferralOutcomeDetailID || 0).toString(), 'ReferralOutcomeDetailID', { parentKey: 'ReferralHeaderID', editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'updateReferralFollowup', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            getReferralFollowups: getReferralFollowups,
            getReferralFollowup: getReferralFollowup,
            addReferralFollowup: addReferralFollowup,
            updateReferralFollowup: updateReferralFollowup
        };
    }]);