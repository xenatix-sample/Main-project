angular.module('xenatixApp')
    .factory('auditService', ["$q", 'settings', 'offlineData', '$injector', function ($q, settings, offlineData, $injector) {
        var CONFIG = {
            apiControllerRoot: "/data/ClientAudit/",
            offlineApiUrl: '/ClientAudit/'
        }

        function addAudit(clientAudit) {
            var dfd = $q.defer();
            var $http = $injector.get('$http');
            var data = $.extend(true, {}, clientAudit, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + "1", 'ClientAuditId'));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddClientAudit', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function addScreenAudit(screenAuditModel) {
            var dfd = $q.defer();
            var $http = $injector.get('$http');            
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddScreenAudit', screenAuditModel)
                .then(function (data, status, header, config) {                    
                    dfd.resolve(data);                   
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });           
            return dfd.promise;
        };

       

        var getUniqueId = function () {
            var dfd = $q.defer();
            var $http = $injector.get('$http');
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetUniqueId')
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });
            return dfd.promise;
        };

        var getUniqueIdentifier = function () {
            return guid;
        };

        function guid() {
            function s4() {
                return Math.floor((1 + Math.random()) * 0x10000)
                  .toString(16)
                  .substring(1);
            }
            return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
              s4() + '-' + s4() + s4() + s4();
        }

        var getHistoryDetails = function (screenId, primaryKey) {
            var dfd = $q.defer();
            var $http = $injector.get('$http');
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getHistoryDetails', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (primaryKey || 0).toString(), 'ContactID'), params: { screenId: screenId, primaryKey: primaryKey } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });
            return dfd.promise;
        };

        var getDemographyHistory = function (contactId) {
            var dfd = $q.defer();
            var $http = $injector.get('$http');
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetDemographyHistory', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactId || 0).toString(), 'ContactID'), params: { contactId: contactId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });
            return dfd.promise;
        };

        var getAliasHistory = function (contactId) {
            var dfd = $q.defer();
            var $http = $injector.get('$http');
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetAliasHistory', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactId || 0).toString(), 'ContactID'), params: { contactId: contactId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });
            return dfd.promise;
        };

        var getIdHistory = function (contactId) {
            var dfd = $q.defer();
            var $http = $injector.get('$http');
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetIdHistory', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactId || 0).toString(), 'ContactID'), params: { contactId: contactId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });
            return dfd.promise;
        };

        var resetScreenAuditModel =  function () {
            this.auditScreenModel.TransactionLogID = undefined,
            this.auditScreenModel.isCurrentRequestPending = false,
            this.auditScreenModel.ContactID = undefined,
            this.auditScreenModel.DataKey = undefined,
            this.auditScreenModel.ActionTypeID = undefined,
            this.auditScreenModel.IsCareMember = undefined,
            this.auditScreenModel.IsBreaktheGlassEnabled = undefined,
            this.auditScreenModel.SearchText = undefined
        }

        var auditScreenModel = {
            isCurrentRequestPending: false,
            PageLevelAuditLogID: undefined,
            TransactionLogID: undefined,
            ContactID: undefined,
            DataKey: undefined,
            ActionTypeID: undefined,
            IsCareMember: undefined,
            IsBreaktheGlassEnabled: undefined,
            SearchText: undefined
        };

        return {
            addAudit: addAudit,
            getUniqueId: getUniqueId,
            getUniqueIdentifier: getUniqueIdentifier,
            getHistoryDetails: getHistoryDetails,
            getDemographyHistory: getDemographyHistory,
            getAliasHistory: getAliasHistory,
            getIdHistory: getIdHistory,
            auditScreenModel: auditScreenModel,
            addScreenAudit: addScreenAudit,
            resetScreenAuditModel: resetScreenAuditModel
        };
    }]);