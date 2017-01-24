angular.module('xenatixApp')
    .factory('medicalHistoryService', [
        "$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
            var CONFIG = {
                apiControllerRoot: '/data/plugins/Clinical/MedicalHistory/',
                offlineApiUrl: '/MedicalHistory/',
                offlineDetailsURL: '/MedicalHistoryDetails/'
            };

            var editStateFunc = function editStateSettings() {
                return {
                    description: 'Medical History ',
                    state: 'patientprofile.chart.intake.medicalhistory',
                    stateParams: { ContactID: this.ContactID }
                };
            };

            function getMedicalHistoryBundle(contactID) {
                var dfd = $q.defer();
                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetMedicalHistoryBundle', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'MedicalHistoryID', { childKey: "MedicalHistoryID" }), params: { contactID: contactID } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };

            function getMedicalHistory(medicalHistoryID, contactID) {
                var dfd = $q.defer();
                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetMedicalHistory', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + medicalHistoryID, 'MedicalHistoryID', { childKey: "MedicalHistoryID" }), params: { medicalHistoryID: medicalHistoryID } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            }

            function getMedicalHistoryConditionDetails(medicalHistoryID, contactID) {
                var dfd = $q.defer();

                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetMedicalHistoryConditionDetails', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (medicalHistoryID || 0).toString(), 'MedicalHistoryID', { parentKey: 'MedicalHistoryID' }), params: { medicalHistoryID: medicalHistoryID } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };

            function getAllMedicalConditions(medicalHistoryID, contactID) {
                var dfd = $q.defer();

                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetAllMedicalConditions', { data: offlineData.getOfflineSettings(CONFIG.offlineDetailsURL + (contactID || 0).toString() + '/' + (medicalHistoryID || 0).toString(), 'MedicalHistoryID'), params: { medicalHistoryID: medicalHistoryID } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };

            function addMedicalHistory(medicalHistory) {
                var dfd = $q.defer();

                if (!('MedicalHistoryID' in medicalHistory))
                    medicalHistory.MedicalHistoryID = 0;

                var data = $.extend(true, {}, medicalHistory, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (medicalHistory.ContactID || 0).toString() + '/' + (medicalHistory.MedicalHistoryID || 0).toString(), 'MedicalHistoryID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
                $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddMedicalHistory', data)
               .then(function (data, status, header, config) {
                   dfd.resolve(data);
               }, function (data, status, header, config) {
                   dfd.reject(status);
               }, function (notification) {
                   dfd.notify(notification);
               });

                return dfd.promise;
            }

            function updateMedicalHistory(medicalHistory) {
                var dfd = $q.defer();

                var data = $.extend(true, {}, medicalHistory, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (medicalHistory.ContactID || 0).toString() + '/' + (medicalHistory.MedicalHistoryID || 0).toString(), 'MedicalHistoryID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
                $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateMedicalHistory', data)
               .then(function (data, status, header, config) {
                   dfd.resolve(data);
               }, function (data, status, header, config) {
                   dfd.reject(status);
               }, function (notification) {
                   dfd.notify(notification);
               });

                return dfd.promise;
            }

            function saveMedicalHistoryConditions(medicalHistory) {
                var dfd = $q.defer();

                var data = $.extend(true, {}, medicalHistory, offlineData.getOfflineSettings(CONFIG.offlineDetailsURL + (medicalHistory.ContactID || 0).toString() + '/' + (medicalHistory.MedicalHistoryID || 0).toString(), 'MedicalHistoryID', { parentKey: 'MedicalHistoryID', referenceKeys: ['ContactID'], editState: editStateFunc.toString() }));
                $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'SaveMedicalHistoryDetails', data)
               .then(function (data, status, header, config) {
                   dfd.resolve(data);
               }, function (data, status, header, config) {
                   dfd.reject(status);
               }, function (notification) {
                   dfd.notify(notification);
               });

                return dfd.promise;
            }

            function deleteMedicalHistory(medicalHistoryID, contactId) {
                var dfd = $q.defer();
                $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'DeleteMedicalHistory',
                    {
                        data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactId || 0).toString() + '/' + (medicalHistoryID || 0).toString(), 'MedicalHistoryID', { parentKey: 'ContactID' }),
                        params: { medicalHistoryID: medicalHistoryID, modifiedOn: moment.utc().format() }
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
                getMedicalHistoryBundle: getMedicalHistoryBundle,
                getMedicalHistoryConditionDetails: getMedicalHistoryConditionDetails,
                getAllMedicalConditions: getAllMedicalConditions,
                deleteMedicalHistory: deleteMedicalHistory,
                addMedicalHistory: addMedicalHistory,
                updateMedicalHistory: updateMedicalHistory,
                saveMedicalHistoryConditions: saveMedicalHistoryConditions,
                getMedicalHistory: getMedicalHistory
            };
        }
    ]);
