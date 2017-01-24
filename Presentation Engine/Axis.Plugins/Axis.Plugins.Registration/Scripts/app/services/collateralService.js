angular.module('xenatixApp')
    .factory('collateralService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {

        var CONFIG = {
            apiControllerRoot: "/data/plugins/registration/collateral/",
            offlineApiUrl: '/Registration/Collateral/'
        };

        var collateralStateFunc = function editStateSettings() {
            return {
                description: 'Collateral ',
                state: 'registration.collateral',
                stateParams: { ContactID: this.ContactID }
            };
        };

        function get(contactID, contactTypeID, getContactDetails) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetCollaterals', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ParentContactID', { childKey: 'ContactID' }), params: { contactID: contactID, contactTypeID: contactTypeID, getContactDetails: getContactDetails } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function add(collateral) {
            var dfd = $q.defer();
            if (!('ContactID' in collateral))
                collateral.ContactID = 0;
            var data = $.extend(true, {}, collateral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (collateral.ParentContactID || 0).toString() + '/' + collateral.ContactID, 'ContactID', { parentKey: 'ParentContactID', editState: collateralStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddCollateral', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function update(collateral) {
            var dfd = $q.defer();
            collateral.ModifiedOn = moment.utc().toDate();
            var data = $.extend(true, {}, collateral, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (collateral.ParentContactID || 0).toString() + '/' + (collateral.ContactID || 0).toString(), 'ContactID', { parentKey: 'ParentContactID', editState: collateralStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateCollateral', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function remove(parentContactID, contactID) {
            var dfd = $q.defer();

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'DeleteCollateral', {
                data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (parentContactID || 0).toString() + '/' + (contactID || 0).toString(), 'ContactID', { parentKey: 'ParentContactID' }), params: {
                parentContactID: parentContactID, contactID: contactID, modifiedOn: moment.utc().format() }
        })
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
            update: update,
            remove: remove
        };
    }]);
