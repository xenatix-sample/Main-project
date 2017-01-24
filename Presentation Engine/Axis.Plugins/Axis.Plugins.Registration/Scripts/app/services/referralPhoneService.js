angular.module('xenatixApp')
    .factory('referralPhoneService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/registration/referralPhone/",
            offlineApiUrl: '/referralPhone/'
        };

        function getPhones(referralID, contactTypeID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetPhones', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referralID || 0).toString(), 'ReferralID', { childKey: 'ReferralPhoneID' }), params: { referralID: referralID, contactTypeID: contactTypeID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function addUpdate(referral) {
            var dfd = $q.defer();
            if (!('ReferralPhoneID' in referral))
                referral.ReferralPhoneID = 0;
            var data = $.extend(true, {}, referral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referral.ReferralID || 0).toString() + '/' + (referral.ReferralPhoneID || 0).toString(), 'ReferralPhoneID', { parentKey: 'ReferralID' }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddUpdatePhones', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function deleteReferralPhone(referralPhoneID) {
            var dfd = $q.defer();

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'DeleteReferralPhone', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referralPhoneID || 0).toString(), 'ReferralPhoneID', { parentKey: 'ReferralID' }), params: { referralPhoneID: referralPhoneID, modifiedOn: moment.utc().format() } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            getPhones: getPhones,
            addUpdate: addUpdate,
            deleteReferralPhone: deleteReferralPhone
        };
    }]);