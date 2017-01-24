angular.module('xenatixApp')
    .factory('referralDispositionService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/registration/referraldisposition/",
            offlineApiUrl: '/referralDisposition/'
        };

        var editStateFunc = function editStateSettings() {
            return {
                description: 'Referrals Disposition',
                state: 'referrals.disposition',
                stateParams: { ContactID: this.ContactID, ReferralHeaderID: this.ReferralHeaderID,ReadOnly:'edit' }
            };
        };

        function getReferralDispositionDetail(referralHeaderID) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getReferralDispositionDetail', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referralHeaderID || 0).toString(), 'ReferralHeaderID', { childKey: 'ReferralDispositionDetailID' }), params: { referralHeaderID: referralHeaderID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function addReferralDispositionDetail(referral) {
            var dfd = $q.defer();
            if (!('ReferralDispositionDetailID' in referral))
                referral.ReferralDispositionDetailID = 0;
            var data = $.extend(true, {}, referral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referral.ReferralHeaderID || 0).toString() + '/' + (referral.ReferralDispositionDetailID || 0).toString(), 'ReferralDispositionDetailID', { parentKey: 'ReferralHeaderID', editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'addReferralDispositionDetail', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function updateReferralDispositionDetail(referral) {
            var dfd = $q.defer();

            var data = $.extend(true, {}, referral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referral.ReferralHeaderID || 0).toString() + '/' + (referral.ReferralDispositionDetailID || 0).toString(), 'ReferralDispositionDetailID', { parentKey: 'ReferralHeaderID', editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'updateReferralDisposition', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            getReferralDispositionDetail: getReferralDispositionDetail,
            addReferralDispositionDetail: addReferralDispositionDetail,
            updateReferralDispositionDetail: updateReferralDispositionDetail
        };
    }]);