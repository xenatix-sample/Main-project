angular.module('xenatixApp')
    .factory('referralHeaderService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/registration/referralHeader/",
            offlineApiUrl: '/referralHeader/'
        };
        

        var editStateFunc = function editStateSettings() {
            return {
                description: 'Referral Requestor',
                state: 'referrals.requestor',
                stateParams: { ContactID: this.ContactID, ReferralHeaderID: this.ReferralHeaderID }
            };
        };
        function getReferralHeader(referralHeaderID, contactID) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetReferralHeader', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (referralHeaderID || 0).toString(), 'ContactID', { childKey: 'ReferralHeaderID' }), params: { referralHeaderID: referralHeaderID } })
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
            if (!('ReferralHeaderID' in referral))
                referral.ReferralHeaderID = 0;
            var data = $.extend(true, {}, referral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referral.ContactID || 0).toString() + '/' + (referral.ReferralHeaderID || 0).toString(), 'ReferralHeaderID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddReferralHeader', data)
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
            if (!('ReferralHeaderID' in referral))
                referral.ReferralHeaderID = 0;
            var data = $.extend(true, {}, referral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referral.ContactID || 0).toString() + '/' + (referral.ReferralHeaderID || 0).toString(), 'ReferralHeaderID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateReferralHeader', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function deleteReferralHeader(referralHeaderID) {
            var dfd = $q.defer();

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'DeleteReferralHeader', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referralHeaderID || 0).toString(), 'ReferralHeaderID', { parentKey: 'ContactID' }), params: { referralHeaderID: referralHeaderID, modifiedOn: moment.utc().format() } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            getReferralHeader: getReferralHeader,
            add: add,
            update: update,
            deleteReferralHeader: deleteReferralHeader
        };
    }]);