angular.module('xenatixApp')
    .factory('referralEmailService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/registration/ReferralEmail/",
            offlineApiUrl: '/referralEmail/'
        };

        function getEmails(referralID, contactTypeID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetEmails', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referralID || 0).toString(), 'ReferralID', { childKey: 'ReferralEmailID' }), params: { referralID: referralID, contactTypeID: contactTypeID } })
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
            if (!('ReferralEmailID' in referral))
                referral.ReferralEmailID = 0;
            var data = $.extend(true, {}, referral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referral.ReferralID || 0).toString() + '/' + (referral.ReferralEmailID || 0).toString(), 'ReferralEmailID', { parentKey: 'ReferralID' }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddUpdateEmails', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function deleteEmail(referralEmailID) {
            var dfd = $q.defer();

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'DeleteEmail', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referralEmailID || 0).toString(), 'ReferralEmailID', { parentKey: 'ReferralID' }), params: { referralEmailID: referralEmailID, modifiedOn: moment.utc().format() } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            getEmails: getEmails,
            addUpdate: addUpdate,
            deleteEmail: deleteEmail
        };
    }]);