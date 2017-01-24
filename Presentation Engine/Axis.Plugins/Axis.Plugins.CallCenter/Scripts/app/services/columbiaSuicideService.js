(function () {
    angular.module('xenatixApp')
        .service('columbiaSuicideService', ['$q', '$injector', '$filter', 'assessmentPrintService', 'lookupService', function ($q, $injector, $filter, assessmentPrintService, lookupService) {

            var initReport = function (contactID, assessmentID, responseID, sectionID, sourceHeaderID) {
                var dfd = $q.defer(), reportModel = {}, promiseArr = [];
                promiseArr.push(assessmentPrintService.initReports(assessmentID, responseID, sectionID));
                promiseArr.push(colubiaSuicideReport(sourceHeaderID, contactID));
                $q.all(promiseArr).then(function (data) {
                    if (data[0]) {
                        angular.merge(reportModel, data[0]);
                    }
                    if (data[1]) {
                        angular.merge(reportModel, data[1]);
                    }
                }).finally(function () {
                    reportModel.ReportHeader = 'Columbia Suicide Scale';
                    reportModel.ReportName = 'Columbia-SuicideSeverityRatingScale';
                    dfd.resolve(reportModel);
                });
                return dfd.promise;
            }

            var colubiaSuicideReport = function (headerID, contactID) {
                var deferred = $q.defer();
                var reportModel = {};
                reportModel.HasLoaded = false;
                reportModel.ReportHeader = 'Columbia Suicide Scale';
                reportModel.ReportName = 'Columbia-SuicideSeverityRatingScale';
                var promises = [];

                var registrationService = $injector.get('registrationService');
                var callerInformationService = $injector.get('callerInformationService');
                var contactBenefitService = $injector.get('contactBenefitService');
                var additionalDemographyService = $injector.get('additionalDemographyService');
                var contactRaceService = $injector.get('contactRaceService');
                var serviceRecordingService = $injector.get('serviceRecordingService');
                var assessmentService = $injector.get('assessmentService');


                promises.push(registrationService.get(contactID));
                promises.push(callerInformationService.get(headerID));
                promises.push(contactBenefitService.getMedicaidNumber(contactID));
                promises.push(additionalDemographyService.getAdditionalDemographic(contactID));
                promises.push(contactRaceService.get(contactID));
                promises.push(serviceRecordingService.getServiceRecording(headerID, SERVICE_RECORDING_SOURCE.CallCenter));
                promises.push(assessmentService.getAssessmentResponses(contactID, ASSESSMENT_TYPE.CrisisAssessment));

                $q.all(promises).then(function (data) {
                    if (hasData(data[0])) {
                        var contactData = data[0].DataItems[0];
                        reportModel.contactFirstName = contactData.FirstName;
                        reportModel.contactLastName = contactData.LastName;
                        reportModel.contactMiddleName = contactData.Middle ? contactData.Middle : '';
                        reportModel.contactID = contactID.toString();
                        reportModel.mrn = contactData.MRN ? (contactData.MRN).toString() : '';
                        reportModel.contactSuffix = contactData.SuffixID ? lookupService.getText('Suffix', contactData.SuffixID) : '';
                        reportModel.contactDOB = contactData.DOB ? $filter('formatDate')(contactData.DOB) : '';
                        reportModel.contactAge = contactData.DOB ? $filter('toAge')(contactData.DOB).toString() : '';
                        if (hasDetails(contactData.Phones)) {
                            reportModel.contactPhone = $filter('toPhone')(contactData.Phones.Number);
                        }

                        if (hasDetails(contactData.Addresses)) {
                            var address = contactData.Addresses[0];
                            reportModel.contactGateCode = address.GateCode ? address.GateCode : '';
                            reportModel.contactComplexName = address.ComplexName ? address.ComplexName : '';
                            reportModel.contactAddressLine1 = address.Line1 ? address.Line1 : '';
                            reportModel.contactAddressLine2 = address.Line2 ? address.Line2 : '';
                            reportModel.contactCity = address.City ? address.City : '';
                            reportModel.contactState = address.StateProvince ? lookupService.getText('StateProvince', address.StateProvince) : '';
                            reportModel.contactZip = address.Zip ? address.Zip : '';
                        }
                    }

                    if (hasData(data[1])) {
                        var callerInformation = data[1].DataItems[0];
                        reportModel.screener = lookupService.getText('Users', callerInformation.ProviderID);
                        reportModel.screeningDate = ($filter('toMMDDYYYYDate')(moment(callerInformation.CallStartTime).toDate(), 'MM/DD/YYYY')).toString();
                        reportModel.screeningTime = ($filter('toMMDDYYYYDate')(callerInformation.CallStartTime, 'hh:mm A')).toString();
                        reportModel.signatureDate = ($filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY')).toString();

                        if (data[1].DataItems[0].CallCenterHeaderID)
                            reportModel.incidentID = data[1].DataItems[0].CallCenterHeaderID.toString();
                    }

                    reportModel.medicaidNum = data[2];
                    if (hasData(data[4])) {
                        var raceLookupList = lookupService.getLookupsByType('Race');
                        reportModel.contactRace = getRaceCSVNames(raceLookupList, data[4].DataItems);
                    }

                    if (hasData(data[3])) {
                        if (data[3].DataItems[0].EthnicityID)
                            reportModel.contactEthnicity = lookupService.getText('Ethnicity', data[3].DataItems[0].EthnicityID);
                    }

                    if (hasData(data[5])) {
                        var serviceRecording = data[5].DataItems[0];
                        if (serviceRecording.OrganizationID) {
                            reportModel.contactProgramUnit = lookupService.getText('ProgramUnit', serviceRecording.OrganizationID);
                        }
                    }

                    var subPromiseArr = [];
                    var contactSSNService = $injector.get('contactSSNService');
                    subPromiseArr.push((contactData && contactData.SSN && contactData.SSN.length > 0 && contactData.SSN.length < 9) ? contactSSNService.refreshSSN(contactID, contactData) : 1);
                    subPromiseArr.push(hasData(data[6]) ? assessmentService.getAssessmentResponseDetails(data[6].DataItems[0].ResponseID, ASSESSMENT_SECTION.RiskFactors) : 1);
                    $q.all(subPromiseArr).then(function (response) {
                        if (response[0] !== 1 && contactData.SSN) {
                            reportModel.contactSSN = $filter('toMaskSSN')(contactData.SSN);
                        }
                        if (response[1] && hasData(response[1].data)) {
                            var rData = response[1].data;
                            $filter('filter')(rData.DataItems, function (question) {
                                if (question.QuestionID == '3530')
                                    reportModel.sentToMCOTDate = question.ResponseText ? ($filter('toMMDDYYYYDate')(question.ResponseText, 'MM/DD/YYYY')).toString() : '';
                                if (question.QuestionID == '3531')
                                    reportModel.sentToMCOTTime = searchString(question.ResponseText, adjustedTime) ? "" : ($filter('toMMDDYYYYDate')(question.ResponseText, 'hh:mm A')).toString();
                            });
                        }

                    }).finally(function () {
                        return deferred.resolve(reportModel);
                    });
                });
                return deferred.promise;
            }

            return {
                initReport: initReport
            }
        }])
})();