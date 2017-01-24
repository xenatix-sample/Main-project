angular.module('xenatixApp')
    .factory('presentIllnessService', [
        "$http", "$q", 'settings', 'offlineData','lookupService', function ($http, $q, settings, offlineData, lookupService) {
            var CONFIG = {
                apiControllerRoot: '/data/plugins/Clinical/PresentIllness/',
                offlineApiUrl: '/Clinical/PresentIllness/',
                offlineApiDetailsUrl: '/Clinical/PresentIllnessDetails/'
            };

            var editStateFunc = function editStateSettings() {
                return {
                    description: 'Present Illness',
                    state: 'patientprofile.chart.intake.presentillness',
                    stateParams: { ContactID: this.ContactID }
                };
            };

            function getHPIBundle(contactID) {
                var dfd = $q.defer();
                
                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetHPIBundle', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { childKey: 'HPIID' }), params: { contactID: contactID }} )
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };

            function getHPIDetails(hpiID, contactID) {//add contactid
                var dfd = $q.defer();

                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetHPIDetails', { data: offlineData.getOfflineSettings(CONFIG.offlineApiDetailsUrl + (contactID || 0).toString() + '/' + (hpiID || 0).toString(), 'HPIID', { childKey: 'HPIDetailID' }), params: { hpiID: hpiID } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };

            function addHPI(HPI) {
                var dfd = $q.defer();

                var data = $.extend(true, {}, HPI, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (HPI.ContactID || 0).toString() + '/' + (HPI.HPIID || 0).toString(), 'HPIID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
                $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddHPI', data)
               .then(function (data, status, header, config) {
                   dfd.resolve(data);
               }, function (data, status, header, config) { 
                   dfd.reject(status);
               }, function (notification) {
                   dfd.notify(notification);
               });

                return dfd.promise;
            }

            function addHPIDetail(HPI) {
                var dfd = $q.defer();

                var data = $.extend(true, {}, HPI, offlineData.getOfflineSettings(CONFIG.offlineApiDetailsUrl + (HPI.ContactID || 0).toString() + '/' + (HPI.HPIID || 0).toString() + '/' + (HPI.HPIDetailID).toString(), 'HPIDetailID', { parentKey: 'HPIID', editState: editStateFunc.toString() }));
                $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddHPIDetail', data)
               .then(function (data, status, header, config) {
                   dfd.resolve(data);
               }, function (data, status, header, config) {
                   dfd.reject(status);
               }, function (notification) {
                   dfd.notify(notification);
               });

                return dfd.promise;
            }

            function updateHPI(HPI) {
                var dfd = $q.defer();

                var data = $.extend(true, {}, HPI, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (HPI.ContactID || 0).toString() + '/' + (HPI.HPIID || 0).toString(), 'HPIID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
                $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateHPI', data)
               .then(function (data, status, header, config) {
                   dfd.resolve(data);
               }, function (data, status, header, config) {
                   dfd.reject(status);
               }, function (notification) {
                   dfd.notify(notification);
               });

                return dfd.promise;
            }

            function updateHPIDetail(HPI) {
                var dfd = $q.defer();

                var data = $.extend(true, {}, HPI, offlineData.getOfflineSettings(CONFIG.offlineApiDetailsUrl + (HPI.ContactID || 0).toString() + '/' + (HPI.HPIDetailID || 0).toString(), 'HPIDetailID', { parentKey: 'HPIID' }));
                $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateHPIDetail', data)
               .then(function (data, status, header, config) {
                   dfd.resolve(data);
               }, function (data, status, header, config) {
                   dfd.reject(status);
               }, function (notification) {
                   dfd.notify(notification);
               });

                return dfd.promise;
            }

            function deleteHPIDetail(contactID, hpiID,HPIDetailID) {
                var dfd = $q.defer();
                $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'DeleteHPIDetail', { data: offlineData.getOfflineSettings(CONFIG.offlineApiDetailsUrl + (contactID || 0).toString() + '/' + (hpiID || 0).toString() + '/' + (HPIDetailID || 0).toString(), 'HPIDetailID', { parentKey: 'HPIID' }), params: { HPIDetailID: HPIDetailID, modifiedOn: moment.utc().format() } })
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


            return {
                getHPIBundle: getHPIBundle,
                getHPIDetails: getHPIDetails,
                addHPI: addHPI,
                addHPIDetail: addHPIDetail,
                updateHPI: updateHPI,
                updateHPIDetail: updateHPIDetail,
                deleteHPIDetail: deleteHPIDetail
            };
        }
    ]);





               

