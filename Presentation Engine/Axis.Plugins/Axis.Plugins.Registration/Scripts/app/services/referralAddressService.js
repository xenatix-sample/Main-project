angular.module('xenatixApp')
    .factory('referralAddressService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/registration/ReferralAddress/",
            offlineApiUrl: '/referralAddress/'
        };

        function getAddresses(referralID, contactTypeID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetAddresses', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referralID || 0).toString(), 'ReferralID', { childKey: 'ReferralAddressID' }), params: { referralID: referralID, contactTypeID: contactTypeID } })
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
            if (!('ReferralAddressID' in referral))
                referral.ReferralAddressID = 0;
            var data = $.extend(true, {}, referral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referral.ReferralID || 0).toString() + '/' + (referral.ReferralAddressID || 0).toString(), 'ReferralAddressID', { parentKey: 'ReferralID' }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddUpdateAddresses', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function deleteAddress(referralAddressID) {
            var dfd = $q.defer();

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'DeleteAddress', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referralAddressID || 0).toString(), 'ReferralAddressID', { parentKey: 'ReferralID' }), params: { referralAddressID: referralAddressID, modifiedOn: moment.utc().format() } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            getAddresses: getAddresses,
            addUpdate: addUpdate,
            deleteAddress: deleteAddress
        };
    }]);