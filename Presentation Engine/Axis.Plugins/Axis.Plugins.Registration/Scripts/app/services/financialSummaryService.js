angular.module('xenatixApp')
    .factory('financialSummaryService', ["$http", "$q", 'settings', 'offlineData', 'lookupService', '$filter', function ($http, $q, settings, offlineData, lookupService, $filter) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/registration/financialSummary/",
            offlineApiUrl: '/Registration/FinancialSummary/',
        };

        var editStateFunc = function editStateSettings() {
            return {
                description: 'Financial Assessment ',
                state: 'patientprofile.benefits.financialAssesments.financialSummary',
                stateParams: { ContactID: this.ContactID, FinancialSummaryID: this.FinancialSummaryID, ReadOnly: 'Edit' }
            };
        };

        function getFinancialSummaryById(contactID, financialSummaryID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getFinancialSummaryById', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (financialSummaryID || 0).toString(), 'FinancialSummaryID', { parentKey: 'ContactID' }), params: { financialSummaryID: financialSummaryID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getFinancialSummaries(contactID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getFinancialSummaries', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { childKey: 'FinancialSummaryID' }), params: { contactID: contactID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function addFinancialSummary(financialSummary) {
            var dfd = $q.defer();
            if (!('FinancialSummaryID' in financialSummary))
                financialSummary.FinancialSummaryID = 0;
            var data = $.extend(true, {}, financialSummary, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (financialSummary.ContactID || 0).toString() + '/' + (financialSummary.FinancialSummaryID || 0).toString(), 'FinancialSummaryID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'addFinancialSummary', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function updateFinancialSummary(financialSummary) {
            var dfd = $q.defer();

            var data = $.extend(true, {}, financialSummary, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (financialSummary.ContactID || 0).toString() + '/' + (financialSummary.FinancialSummaryID || 0).toString(), 'FinancialSummaryID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'updateFinancialSummary', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            getFinancialSummaryById: getFinancialSummaryById,
            getFinancialSummaries: getFinancialSummaries,
            addFinancialSummary: addFinancialSummary,
            updateFinancialSummary: updateFinancialSummary
        };
    }]);
