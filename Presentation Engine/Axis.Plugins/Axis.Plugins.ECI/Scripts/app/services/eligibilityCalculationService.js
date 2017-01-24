angular.module('xenatixApp')
    .factory('eligibilityCalculationService', ["$http", "$q", 'settings', 'offlineData', 'lookupService', function ($http, $q, settings, offlineData, lookupService) {
        var CONFIG = {
            apiControllerRoot: '/data/Plugins/ECI/EligibilityCalculation/',
            offlineApiUrl: '/ECI/EligibilityCalculation/'
        };
        var editStateFunc = function editStateSettings() {
            return {
                description: 'ECI Eligibility Calculation',
                state: 'patientprofile.chart.eligibilities.eligibility.calculation',
                stateParams: { ContactID: this.ContactID, EligibilityID: this.EligibilityID }
            };
        };

        function get(eligibilityID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetEligibilityCalculations', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (eligibilityID || 0).toString(), 'EligibilityID', { childKey: 'EligibilityCalculationID' }), params: { eligibilityID: eligibilityID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function add(calculationModel) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, calculationModel, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (calculationModel.EligibilityID || 0).toString(), 'EligibilityCalculationID', { parentKey: 'EligibilityID', editState: editStateFunc.toString() }));

            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddEligibilityCalculations', data)
           .then(function (data, status, header, config) {
               dfd.resolve(data);
           }, function (data, status, header, config) {
               dfd.reject(status);
           }, function (notification) {
               dfd.notify(notification);
           });

            return dfd.promise;
        };

        function update(calculationModel) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, calculationModel, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (calculationModel.EligibilityID || 0).toString(), 'EligibilityCalculationID', { parentKey: 'EligibilityID', editState: editStateFunc.toString() }));

            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateEligibilityCalculations', data)
           .then(function (data, status, header, config) {
               dfd.resolve(data);
           }, function (data, status, header, config) {
               dfd.reject(status);
           }, function (notification) {
               dfd.notify(notification);
           });

            return dfd.promise;
        };

        return {
            get: get,
            add: add,
            update: update
        };
    }]);
