angular.module('xenatixApp')
    .factory('referralClientInformationService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData)
    {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/registration/ReferralClientInformation/",
            offlineApiUrl: '/referralClient/'
        };

        var editStateFunc = function editStateSettings() {
            return {
                description: 'Referral Client Information',
                state: 'referrals.client',
                stateParams: { ContactID: this.ContactID, ReferralHeaderID: this.ReferralHeaderID,ReadOnly:'edit' }
            };
        };

        function getReferralClientInformation(referralHeaderID)
        {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetClientInformation', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referralHeaderID || 0).toString(), 'ReferralHeaderID', { childKey: 'ContactID' }), params: { referralHeaderID: referralHeaderID } })
            .success(function (data, status, header, config)
            {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config)
            {
                dfd.reject(status);
            });

            return dfd.promise;
        };


        function addReferralClientInformation(referral){
            var dfd = $q.defer();
           
            var data = $.extend(true, {}, referral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referral.ReferralHeaderID || 0).toString() + '/' + (referral.clientDemographicsModel.ContactID || 0).toString(), 'ContactID', { parentKey: 'ReferralHeaderID', editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddClientInformation', data)
            .success(function (data, status, header, config)
            {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config)
            {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function updateReferralClientInformation(referral)
        {
            var dfd = $q.defer();

            var data = $.extend(true, {}, referral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (referral.ReferralHeaderID || 0).toString() + '/' + (referral.clientDemographicsModel.ContactID || 0).toString(), 'ContactID', { parentKey: 'ReferralHeaderID', editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateClientInformation', data)
            .success(function (data, status, header, config)
            {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config)
            {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            getReferralClientInformation: getReferralClientInformation,
            addReferralClientInformation: addReferralClientInformation,
            updateReferralClientInformation: updateReferralClientInformation
        };


    }]);