angular.module('xenatixApp')
    .factory('reviewOfSystemsService', ["$http", "$q", 'settings', 'offlineData', 'lookupService', function ($http, $q, settings, offlineData, lookupService) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/Clinical/reviewOfSystems/",
            offlineApiUrl: '/ReviewOfSystems/',
            offlineLastActiveRos: '/LastActiveRos'
        };

        var editStateFunc = function editStateSettings() {
            return {
                description: 'Review Of Systems',
                state: 'patientprofile.chart.intake.reviewOfSystems.ros.header',
                stateParams: { ContactID: this.ContactID }
            };
        };

        function getReviewOfSystemsByContact(contactID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getReviewOfSystemsByContact', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { childKey: 'RoSID' }), params: { contactID: contactID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getReviewOfSystem(contactID, rosID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getReviewOfSystem', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (rosID || 0).toString(), 'RoSID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'] }), params: { rosID: rosID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getLastActiveReviewOfSystems(contactID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getLastActiveReviewOfSystems', { data: offlineData.getOfflineSettings(CONFIG.offlineLastActiveRos + (contactID || 0).toString(), 'ContactID', { childKey: 'RoSID' }), params: { contactID: contactID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getLookups(typeName) {
            return lookupService.getLookupsByType(typeName);
        };

        function addReviewOfSystem(ros) {
            var dfd = $q.defer();
            if (!('RoSID' in ros))
                ros.RoSID = 0;
            var data = $.extend(true, {}, ros, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (ros.ContactID || 0).toString() + '/' + (ros.RoSID || 0).toString(), 'RoSID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'], editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'addReviewOfSystem', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function updateReviewOfSystem(ros) {
            var dfd = $q.defer();

            var data = $.extend(true, {}, ros, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (ros.ContactID || 0).toString() + '/' + (ros.RoSID || 0).toString(), 'RoSID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'], editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'updateReviewOfSystem', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function deleteReviewOfSystem(contactID, rosID) {
            var dfd = $q.defer();

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'deleteReviewOfSystem', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (rosID || 0).toString(), 'RoSID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'] }), params: { rosID: rosID, modifiedOn: moment.utc().format() } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getLastAssessmentOn() {
            return lastAssessmentOn;
        };

        function setLastAssessmentOn(value) {
            lastAssessmentOn = value;
        };

        return {
            getReviewOfSystemsByContact: getReviewOfSystemsByContact,
            getReviewOfSystem: getReviewOfSystem,
            getLastActiveReviewOfSystems: getLastActiveReviewOfSystems,
            getLookups: getLookups,
            getLastAssessmentOn: getLastAssessmentOn,
            setLastAssessmentOn: setLastAssessmentOn,

            addReviewOfSystem: addReviewOfSystem,
            updateReviewOfSystem: updateReviewOfSystem,
            deleteReviewOfSystem: deleteReviewOfSystem
        };
    }]);
