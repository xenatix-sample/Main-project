angular.module('xenatixApp')
    .factory('allergyService', [
        "$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
            var CONFIG = {
                apiControllerRoot: '/data/plugins/Clinical/Allergy/',
                offlineApiUrl: '/Clinical/Allergy/',
                offlineApiDetailsUrl: '/Clinical/AllergyDetails/',
                allergyHeaderApiUrl: '/Clinical/AllergyHeader/'
        };

            var allergyStateFunc = function editStateSettings() {
                return {
                    description: 'Allergy',
                    state: 'patientprofile.chart.intake.allergy',
                    stateParams: { AllergyTypeID: this.AllergyTypeID }
                };
            };


            function getAllergyBundle(contactID, allergyTypeID) {
                var dfd = $q.defer();

                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetAllergyBundle', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { childKey: 'ContactAllergyID' }), params: { contactID: contactID, allergyTypeID: allergyTypeID } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };

            function getAllergyDetails(contactID, contactAllergyID, allergyTypeID) {
                var dfd = $q.defer();
                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetAllergyDetails', {
                    data: offlineData.getOfflineSettings(CONFIG.offlineApiDetailsUrl + (contactID || 0).toString() + '/' + (contactAllergyID || 0).toString(), 'ContactAllergyID', { childKey: 'ContactAllergyDetailID' }), params: { contactAllergyID: contactAllergyID, allergyTypeID: allergyTypeID } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };

            function getTopAllergies(contactID) {
                var dfd = $q.defer();
                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetTopAllergies', { data: offlineData.getOfflineSettings(CONFIG.allergyHeaderApiUrl + (contactID || 0).toString()), params: { contactID: contactID } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            }

            function addAllergy(allergy) {
                var dfd = $q.defer();
                
                var data = $.extend(true, {}, allergy, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (allergy.ContactID || 0).toString() + '/' + (allergy.ContactAllergyID || 0).toString(), 'ContactAllergyID', { parentKey: 'ContactID', editState: allergyStateFunc.toString() }));
                $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddAllergy', data)
               .then(function (data, status, header, config) {
                   dfd.resolve(data);
               }, function (data, status, header, config) {
                   dfd.reject(status);
               }, function (notification) {
                   dfd.notify(notification);
               });

                return dfd.promise;
            }

            function addAllergyDetail(allergy) {
                var dfd = $q.defer();
                
                allergy.ContactAllergyDetailID = 0;
                var data = $.extend(true, {}, allergy, offlineData.getOfflineSettings(CONFIG.offlineApiDetailsUrl + (allergy.ContactID || 0).toString() + '/' + (allergy.ContactAllergyID || 0).toString() + '/' + (allergy.ContactAllergyDetailID || 0).toString(), 'ContactAllergyDetailID', { parentKey: 'ContactAllergyID', editState: allergyStateFunc.toString() }));
                $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddAllergyDetail', data)
               .then(function (data, status, header, config) {
                   dfd.resolve(data);
               }, function (data, status, header, config) {
                   dfd.reject(status);
               }, function (notification) {
                   dfd.notify(notification);
               });

                return dfd.promise;
            }

            function updateAllergy(allergy) {
                var dfd = $q.defer();

                var data = $.extend(true, {}, allergy, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (allergy.ContactID || 0).toString() + '/' + (allergy.ContactAllergyID || 0).toString(), 'ContactAllergyID', { parentKey: 'ContactID', editState: allergyStateFunc.toString() }));

                $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateAllergy', data)
               .then(function (data, status, header, config) {
                   dfd.resolve(data);
               }, function (data, status, header, config) {
                   dfd.reject(status);
               }, function (notification) {
                   dfd.notify(notification);
               });

                return dfd.promise;
            }

            function updateAllergyDetail(allergy) {
                var dfd = $q.defer();
                
                var data = $.extend(true, {}, allergy, offlineData.getOfflineSettings(CONFIG.offlineApiDetailsUrl + (allergy.ContactID || 0).toString() + '/' + (allergy.ContactAllergyID || 0).toString() + '/' + (allergy.ContactAllergyDetailID).toString(), 'ContactAllergyDetailID', { parentKey: 'ContactAllergyID', editState: allergyStateFunc.toString() }));

                $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateAllergyDetail', data)
               .then(function (data, status, header, config) {
                   dfd.resolve(data);
               }, function (data, status, header, config) {
                   dfd.reject(status);
               }, function (notification) {
                   dfd.notify(notification);
               });

                return dfd.promise;
            }

            function deleteAllergyDetail(contactID, allergyId, contactAllergyDetailID, reasonForDeletion) {
                var dfd = $q.defer();
                $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'DeleteAllergyDetail', { data: offlineData.getOfflineSettings(CONFIG.offlineApiDetailsUrl + (contactID || 0).toString() + '/' + (allergyId || 0).toString() + '/' + (contactAllergyDetailID || 0).toString(), 'ContactAllergyDetailID', { parentKey: 'ContactAllergyID' }), params: { contactAllergyDetailID: contactAllergyDetailID, reasonForDeletion: reasonForDeletion, modifiedOn: moment.utc().format() } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };

            return {
                getAllergyBundle: getAllergyBundle,
                getAllergyDetails: getAllergyDetails,
                getTopAllergies: getTopAllergies,
                addAllergy: addAllergy,
                addAllergyDetail: addAllergyDetail,
                updateAllergy: updateAllergy,
                updateAllergyDetail: updateAllergyDetail,
                deleteAllergyDetail: deleteAllergyDetail
            };
        }
    ]);
