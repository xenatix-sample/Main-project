angular.module('xenatixApp')
    .factory('selfPayService', ["$http", "$q", '$state', 'settings', 'offlineData', function ($http, $q, $state, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/registration/selfPay/",
            offlineApiUrl: '/selfPay/'
        };

        var editStateFunc = function editStateSettings() {
            return {
                description: 'Self pay',
                state: 'patientprofile.benefits.selfPay',
                stateParams: { ContactID: this.ContactID, ReadOnly: 'edit' }
            };
        };

        function getSelfPay(contactID) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetSelfPayDetails', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'contactID', { childKey: 'SelfPayID' }), params: { selfPayID: contactID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function addSelfPay(selfPay) {
            var dfd = $q.defer();
            if (!('SelfPayID' in selfPay))
                selfPay.SelfPayID = 0;
            var data = $.extend(true, {}, selfPay, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (selfPay.ContactID || 0).toString() + '/' + (selfPay.SelfPayID || 0).toString(), 'SelfPayID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'], editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'addSelfPay', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function updateSelfPay(selfPay) {
            var dfd = $q.defer();

            var data = $.extend(true, {}, selfPay, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (selfPay.ContactID || 0).toString() + '/' + (selfPay.SelfPayID || 0).toString(), 'SelfPayID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'], editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateSelfPay', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function deleteSelfPay(ContactID,selfPayID) {
            var dfd = $q.defer();
            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'DeleteSelfPay', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (ContactID || 0).toString(), 'selfPayID', { parentKey: 'ContactID', referenceKeys: ['ResponseID']  }), params: { selfPayID: selfPayID, modifiedOn: moment.utc().format() } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            getSelfPay: getSelfPay,
            addSelfPay: addSelfPay,
            updateSelfPay: updateSelfPay,
            deleteSelfPay: deleteSelfPay
        };
    }]);