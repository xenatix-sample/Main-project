(function () {
    angular.module('xenatixApp')
    .factory('benefitsAssistanceProgressNoteService', ["$http", "$q", "$filter", "settings", "offlineData", "assessmentPrintService", "registrationService", "contactBenefitService", "eSignatureService", "lookupService","WorkflowHeaderService","$state","$stateParams",
        function ($http, $q, $filter, settings, offlineData, assessmentPrintService, registrationService, contactBenefitService, eSignatureService, lookupService, WorkflowHeaderService, $state, $stateParams) {
            var CONFIG = {
                apiControllerRoot: "/data/plugins/registration/BenefitsAssistance/",
                offlineApiUrl: '/Registration/BenefitsAssistance/'
            }

            var editStateFunc = function editStateSettings() {
                return {
                    description: 'Contact Email ',
                    state: 'patientprofile.benefits.bapn.benefitsAssistanceProgressNote',
                    stateParams: { ContactID: this.ContactID, benefitsAssistanceID: this.benefitsAssistanceID, ReadOnly: 'Edit'}
                };
            };

            function get(contactID, benefitsAssistanceID) {
                var dfd = $q.defer();
                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetBenefitsAssistance', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { parentKey: 'contactID' }), params: { contactID: contactID, benefitsAssistanceID: benefitsAssistanceID } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };

            function getByContactID(contactID) {
                var dfd = $q.defer();
                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetBenefitsAssistanceByContactID', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { childKey: 'benefitsAssistanceID' }), params: { contactID: contactID } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };

            function add(benefitsAssistance) {
                var dfd = $q.defer();
                var data = $.extend(true, {}, benefitsAssistance, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (benefitsAssistance.ContactID || 0).toString() + '/' + (benefitsAssistance.BenefitsAssistanceID || 0).toString(), 'benefitsAssistanceID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'], editState: editStateFunc.toString() }));
                $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddBenefitsAssistance', data)
                    .then(function (data, status, header, config) {
                        dfd.resolve(data);
                    }, function (data, status, header, config) {
                        dfd.reject(status);
                    }, function (notification) {
                        dfd.notify(notification);
                    });

                return dfd.promise;
            };

            function update(benefitsAssistance) {
                var dfd = $q.defer();
                var data = $.extend(true, {}, benefitsAssistance, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (benefitsAssistance.ContactID || 0).toString() + '/' + (benefitsAssistance.BenefitsAssistanceID || 0).toString(), 'benefitsAssistanceID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'], editState: editStateFunc.toString() }));
                $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateBenefitsAssistance', data)
                    .then(function (data, status, header, config) {
                        dfd.resolve(data);
                    }, function (data, status, header, config) {
                        dfd.reject(status);
                    }, function (notification) {
                        dfd.notify(notification);
                    });

                return dfd.promise;
            };

            function remove(contactID, benefitsAssistanceID) {
                var dfd = $q.defer();
                var url = {
                    data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (benefitsAssistanceID || 0).toString(), 'benefitsAssistanceID', { parentKey: 'ContactID' }),
                    params: { benefitsAssistanceID: benefitsAssistanceID, modifiedOn: moment.utc().format() }
                };

                $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "DeleteBenefitsAssistance", url)
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };

            function initReport(assessmentID, responseID, sectionID, contactID, serviceStartDate, displaySignature, serviceRecordingID, workflowDataKey, benefitsAssistanceID) {
                var dfd = $q.defer(), reportModel = {}, promiseArr = [];
                responseID = responseID ? responseID : 0;
                promiseArr.push(assessmentPrintService.initReports(assessmentID, responseID, sectionID));
                promiseArr.push(getBAPNReportModel(contactID, serviceStartDate, workflowDataKey, benefitsAssistanceID));
                if (displaySignature)
                    promiseArr.push(eSignatureService.getDocumentSignatures(DOCUMENT_TYPE.ServiceRecording, serviceRecordingID));
                $q.all(promiseArr).then(function (data) {
                    if (data[0]) {
                        angular.merge(reportModel, data[0]);
                    }
                    if (data[1]) {
                        angular.merge(reportModel, data[1]);
                    }
                    if (displaySignature && hasData(data[2])) {
                        var signatureData = data[2].DataItems[0];
                        reportModel.staffName = signatureData.EntityName;
                        reportModel.staffCredential = lookupService.getText('Credential', signatureData.CredentialID);
                        reportModel.staffSigUri = signatureData.SignatureBlob;
                        reportModel.dateStaffSigned = $filter('formatDate')(signatureData.ModifiedOn);
                    }
                }).finally(function () {
                    dfd.resolve(reportModel);
                });
                return dfd.promise;
            }

            var getBAPNReportModel = function (contactID, serviceStartDate, workflowDataKey, benefitsAssistanceID) {
                var reportModel = {}
                reportModel.medicaidNumber = 'N/A';
                reportModel[1800] = reportModel.startDate = serviceStartDate;
                var dfd = $q.defer();
                var servicesPromises = [];
                servicesPromises.push(getRegitrationData(contactID, workflowDataKey, benefitsAssistanceID));
                servicesPromises.push(getContactBenefitData(contactID));
                $q.all(servicesPromises).then(function (data) {
                    if (data[0]) {
                        angular.merge(reportModel, data[0]);
                    }
                    if (data[1]) {
                        angular.merge(reportModel, data[1]);
                    }
                    dfd.resolve(reportModel);
                });
                return dfd.promise;
            }

            var getRegitrationData = function (contactID, workflowDataKey, benefitsAssistanceID) {
                var dfd = $q.defer();
                WorkflowHeaderService.GetWorkflowHeader(workflowDataKey, benefitsAssistanceID).then(function (data) {

                //});
                //registrationService.get(contactID).then(function (data) {
                    if (hasData(data)) {
                        var registrationData = data.DataItems[0];
                        data.mrn = registrationData.MRN ? registrationData.MRN.toString() : '';
                        data.clientName = registrationData.FirstName + ' ' + registrationData.LastName;
                        data.dob = registrationData.DOB ? $filter('toMMDDYYYYDate')(registrationData.DOB, 'MM/DD/YYYY') : '';
                        data.medicaidNumber = registrationData.MedicaidID || 'N/A';
                        dfd.resolve(data);
                    }
                    else {
                        dfd.resolve(null);
                    }
                }).finally(function () {
                    dfd.resolve(null);
                });
                return dfd.promise;
            }

            var getContactBenefitData = function (contactID) {
                var dfd = $q.defer(), reportModel = {};
                contactBenefitService.get(contactID).then(function (data) {
                    if (hasData(data)) {
                        var payors = $filter('filter')(data.DataItems, function (itm) {
                            return itm.PayorName.toString().toLowerCase().indexOf('medicaid') > -1;
                        });
                        if (hasData(payors)) {
                            //data.medicaidNumber = payors[0].PolicyID;
                        }
                        dfd.resolve(data);
                    }
                    else {
                        dfd.resolve(null);
                    }
                });
                return dfd.promise;
            }

            return {
                get: get,
                getByContactID: getByContactID,
                add: add,
                update: update,
                remove: remove,
                initReport: initReport
            };
        }]);
}());