angular.module('xenatixApp')
    .factory('referralForwardedService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/registration/referralforwarded/",
            offlineApiUrl: '/referralForwarded/'
        };

        var editStateFunc = function editStateSettings() {
            return {
                description: 'Referral Forwarded To',
                state: 'referrals.forwardedto',
                stateParams: { ContactID: this.ContactID, ReferralHeaderID: this.ReferralHeaderID, ReadOnly: 'edit' }
            };
        };

        function getReferralForwardedDetails(referralHeaderID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetReferralForwardedDetails', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referralHeaderID || 0).toString(), 'ReferralHeaderID', { childKey: 'ReferralForwardedDetailID' }), params: { referralHeaderID: referralHeaderID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getReferralForwardedDetail(ReferralHeaderID, referralForwardedDetailID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetReferralForwardedDetail', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (ReferralHeaderID || 0).toString() + '/' + (referralForwardedDetailID || 0).toString(), 'ReferralForwardedDetailID', { parentKey: 'ReferralHeaderID' }), params: { ReferralForwardedDetailID: referralForwardedDetailID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function addReferralForwardedDetail(referral) {
            var dfd = $q.defer();
            if (!('referralForwardedDetailID' in referral))
                referral.referralForwardedDetailID = 0;
            var data = $.extend(true, {}, referral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referral.ReferralHeaderID || 0).toString() + '/' + (referral.referralForwardedDetailID || 0).toString(), 'referralForwardedDetailID', { parentKey: 'ReferralHeaderID', editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddReferralForwardedDetail', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function updateReferralForwardedDetail(referral) {
            var dfd = $q.defer();

            var data = $.extend(true, {}, referral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referral.ReferralHeaderID || 0).toString() + '/' + (referral.ReferralForwardedDetailID || 0).toString(), 'ReferralForwardedDetailID', { parentKey: 'ReferralHeaderID', editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateReferralForwardedDetail', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            getReferralForwardedDetails: getReferralForwardedDetails,//
            getReferralForwardedDetail: getReferralForwardedDetail,
            addReferralForwardedDetail: addReferralForwardedDetail,
            updateReferralForwardedDetail: updateReferralForwardedDetail
        };
    }]);