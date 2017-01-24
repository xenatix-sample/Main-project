(function () {

    angular.module('xenatixApp')
        .factory('referralReferredInformationService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {

            var CONFIG = {
                apiControllerRoot: "/data/Plugins/registration/ReferralReferredInformation/",
                offlineApiUrl: '/referral/information/'
            };

            var editStateFunc = function editStateSettings() {
                return {
                    description: 'Referral Referred To',
                    state: 'referrals.referredto',
                    stateParams: { ContactID: this.ContactID, ReferralHeaderID: this.ReferralHeaderID, ReadOnly: 'edit' }
                };
            };

            function get(referralHeaderID) {
                var dfd = $q.defer();

                var dfd = $q.defer();
                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetReferredInformation', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referralHeaderID || 0).toString(), 'ReferralHeaderID', { childKey: 'ReferredToDetailID' }), params: { referralHeaderID: referralHeaderID } })
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

                if (!('ReferredToDetailID' in referral))
                    referral.ReferredToDetailID = 0;

                var data = $.extend(true, {}, referral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referral.ReferralHeaderID || 0).toString() + '/' + (referral.ReferredToDetailID || 0).toString(), 'ReferredToDetailID', { parentKey: 'ReferralHeaderID', editState: editStateFunc.toString() }));
                $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'addReferredInformation', data)
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

                var data = $.extend(true, {}, referral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referral.ReferralHeaderID || 0).toString() + '/' + (referral.ReferredToDetailID || 0).toString(), 'ReferredToDetailID', { parentKey: 'ReferralHeaderID', editState: editStateFunc.toString() }));
                $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'updateReferredInformation', data)
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };

            return {
                get: get,
                add: add,
                update: update
            };
        }]);


}());