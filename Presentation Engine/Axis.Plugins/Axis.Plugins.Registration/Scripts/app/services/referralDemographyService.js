angular.module('xenatixApp')
    .factory('referralDemographyService', ["$http", "$q", 'settings', 'offlineData', 'lookupService', function ($http, $q, settings, offlineData, lookupService) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/registration/ReferralDemographics/",
            offlineApiUrl: '/ReferralDemographics/'
        };
        var editStateFunc = function editStateSettings() {
            return {
                description: 'Referral Detail',
                state: 'registration.referral',
                stateParams: { ContactID: this.ContactID }
            };
        };
        function getReferralDemographics(referralID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetReferralDemographics', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referralID || 0).toString(), 'ReferralID'), params: { referralID: referralID } })
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
            if (!('ReferralID' in referral))
                referral.ReferralID = 0;
            var data = $.extend(true, {}, referral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referral.ReferralID || 0).toString(), 'ReferralID', { editState: editStateFunc.toString()}));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddReferralDemographics', data)
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

            var data = $.extend(true, {}, referral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referral.ReferralID || 0).toString(), 'ReferralID', { editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateReferralDemographics', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function deleteReferralDemographics(referralID) {
            var dfd = $q.defer();

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'DeleteReferralDemographics', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referralID || 0).toString(), 'ReferralID'), params: { referralID: referralID, modifiedOn: moment.utc().format() } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            getReferralDemographics: getReferralDemographics,
            add: add,
            update: update,
            deleteReferralDemographics: deleteReferralDemographics
        };
    }]);
