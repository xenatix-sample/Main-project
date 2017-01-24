angular.module('xenatixApp')
    .factory('financialAssessmentService', ["$http", "$q", 'settings', 'offlineData', 'lookupService','$filter', function ($http, $q, settings, offlineData, lookupService,$filter) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/registration/financialAssessment/",
            offlineApiUrl: '/Registration/Financial/',
            offlineDetailsApiUrl: '/Registration/Financial/Details/'
        };
        var editStateFunc = function editStateSettings() {
            return {
                description: 'Financial Assessment',
                state: 'registration.financial',
                stateParams: { ContactID: this.ContactID }
            };
        };

        // For now, we are treating it as a singular financial assessment

        function get(contactId, financialAssessmentID) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getFinancialAssessment', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactId || 0).toString(), + '/' + (financialAssessmentID || 0).toString(), 'FinancialAssessmentID', { childKey: 'ContactID' }), params: { contactId: contactId, financialAssessmentID: financialAssessmentID || 0 } })
                .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });
            return dfd.promise;
        };


        function getActiveFA(contactId) {
            var dfd = $q.defer();
            get(contactId, 0).then(function (data) {
                data.DataItems = $filter('filter')(data.DataItems, function (item) {
                   return !isHouseholdExpired(item.ExpirationDate);
               });
               
               dfd.resolve(data);
            }, function (data, status) {
                dfd.reject(status);
            })
            return dfd.promise;
        };


        function add(financialAssessment) {
            var dfd = $q.defer();
            if (!('FinancialAssessmentID' in financialAssessment) || (financialAssessment.FinancialAssessmentID === undefined))
                financialAssessment.FinancialAssessmentID = 0;
            var data = $.extend(true, {}, financialAssessment, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (financialAssessment.ContactID || 0).toString() + '/' + (financialAssessment.FinancialAssessmentID || 0).toString(), 'FinancialAssessmentID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddFinancialAssessment', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function update(financialAssessment) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, financialAssessment, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (financialAssessment.ContactID || 0).toString() + '/' + (financialAssessment.FinancialAssessmentID || 0).toString(), 'FinancialAssessmentID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateFinancialAssessment', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function getDetails(contactId, financialAssessmentID) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getFinancialAssessmentDetails', { data: offlineData.getOfflineSettings(CONFIG.offlineDetailsApiUrl + (contactId || 0).toString() + '/' + (financialAssessmentID || 0).toString(), 'FinancialAssessmentID', { childKey: 'FinancialAssessmentDetailsID' }), params: { contactId: contactId, financialAssessmentID: financialAssessmentID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });
            return dfd.promise;
        };

        function addDetail(financialAssessmentDetail) {
            var dfd = $q.defer();
            if (!('FinancialAssessmentDetailsID' in financialAssessmentDetail) || (financialAssessmentDetail.FinancialAssessmentDetailsID === undefined))
                financialAssessmentDetail.FinancialAssessmentDetailsID = 0;
            var data = $.extend(true, {}, financialAssessmentDetail, offlineData.getOfflineSettings(CONFIG.offlineDetailsApiUrl + (financialAssessmentDetail.ContactID || 0).toString() + '/' + (financialAssessmentDetail.FinancialAssessmentID || 0).toString() + '/0', 'FinancialAssessmentDetailsID', { parentKey: 'FinancialAssessmentID', referenceKeys: ['ContactID'], editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddFinancialAssessmentDetails', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function updateDetail(financialAssessmentDetail) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, financialAssessmentDetail, offlineData.getOfflineSettings(CONFIG.offlineDetailsApiUrl + (financialAssessmentDetail.ContactID || 0).toString() + '/' + (financialAssessmentDetail.FinancialAssessmentID || 0).toString() + '/' + (financialAssessmentDetail.FinancialAssessmentDetailsID || 0).toString(), 'FinancialAssessmentDetailsID', { parentKey: 'FinancialAssessmentID', referenceKeys: ['ContactID'], editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateFinancialAssessmentDetails', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function removeDetail(contactID, assessmentID, detailID) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, { FinancialAssessmentID: assessmentID, FinancialAssessmentDetailsID: detailID }, offlineData.getOfflineSettings(CONFIG.offlineDetailsApiUrl + (contactID || 0).toString() + '/' + (assessmentID || 0).toString() + '/' + (detailID || 0).toString(), 'FinancialAssessmentDetailsID', { parentKey: 'FinancialAssessmentID' }));
            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'DeleteFinancialAssessmentDetail', { data: data, params: { financialAssessmentDetailID: detailID, modifiedOn: moment.utc().format() } })
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function getLookups(typeName) {
            return lookupService.getLookupsByType(typeName);
        };

        return {
            getLookups: getLookups,
            get: get,
            getDetails: getDetails,
            add: add,
            update: update,
            addDetail: addDetail,
            updateDetail: updateDetail,
            removeDetail: removeDetail,
            getActiveFA: getActiveFA
        };
    }]);
