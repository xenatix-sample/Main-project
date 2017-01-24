angular.module('xenatixApp')
    .factory('eciAdditionalDemographicService', ["$http", "$q", 'settings', 'offlineData', '$rootScope', function ($http, $q, settings, offlineData, $rootScope) {

        var CONFIG = {
            apiControllerRoot: "/data/Plugins/ECI/eciAdditionalDemographic/",
            offlineApiUrl: '/ECI/Additional/'
        };

        var editStateFunc = function editStateSettings() {
            return {
                description: 'ECI Additional Demographic ',
                state: 'patientprofile.general.eciadditional',
                stateParams: { ContactID: this.ContactID }
            };
        };

        function getAdditionalDemographic(contactId) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getAdditionalDemographic', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactId || 0).toString()), params: { contactId: contactId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function addAdditionalDemographic(additional) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, additional, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (additional.ContactID || 0).toString(), 'AdditionalDemographicID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'addAdditionalDemographic', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function updateAdditionalDemographic(additional) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, additional, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + additional.ContactID.toString(), 'AdditionalDemographicID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'updateAdditionalDemographic', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function deleteAdditionalDemographic(id) {
            var dfd = $q.defer();

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'deleteAdditionalDemographic/',
                { params: { id: id, modifiedOn: moment.utc().format() } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            getAdditionalDemographic: getAdditionalDemographic,
            addAdditionalDemographic: addAdditionalDemographic,
            updateAdditionalDemographic: updateAdditionalDemographic,
            deleteAdditionalDemographic: deleteAdditionalDemographic
        };
    }]);
