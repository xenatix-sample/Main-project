(function () {
    angular.module('xenatixApp')
        .service('lawLiaisonscreeningService', ['$q', '$injector', '$filter', 'assessmentPrintService', 'lookupService', '$state', '$stateParams', 'callCenterAssessmentPrintService',
            function ($q, $injector, $filter, assessmentPrintService, lookupService, $state, $stateParams, callCenterAssessmentPrintService) {

                var initReport = function (assessmentID, responseID, sectionID, sourceHeaderID, contactID, providerDate, callerInformation, callerDetails, providerStartTime, providerCallStartAMPM, providerName,workflowHeaderID) {
                    var dfd = $q.defer(), reportModel = {}, promiseArr = [];
                    promiseArr.push(assessmentPrintService.initReports(assessmentID, responseID, sectionID));
                    promiseArr.push(lawLiaisonPrintReport(sourceHeaderID, contactID, providerDate, callerInformation, callerDetails, providerStartTime, providerCallStartAMPM, providerName, workflowHeaderID));
                    $q.all(promiseArr).then(function (data) {
                        if (data[0]) {
                            angular.merge(reportModel, data[0]);
                        }
                        if (data[1]) {
                            angular.merge(reportModel, data[1]);
                        }
                    }).finally(function () {
                        reportModel.HasLoaded = true;
                        dfd.resolve(reportModel);
                    });
                    return dfd.promise;
                }

                var lawLiaisonPrintReport = function (headerID, contactID, providerDate, callerInformation, callerDetails, providerStartTime, providerCallStartAMPM, providerName, workflowHeaderID) {
                    var reportModel = {}, deferred = $q.defer(), promises = [];

                    reportModel.HasLoaded = false;
                    reportModel.ReportHeader = 'Law Liaison Screening';
                    reportModel.ReportName = 'LawLiaisonScreening';
                    reportModel.startTime = (providerStartTime ? providerStartTime : '') + ' ' + (providerCallStartAMPM ? providerCallStartAMPM : '');
                    reportModel.startDate = providerDate ? providerDate : '';

                    var registrationService = $injector.get('registrationService');
                    var contactBenefitService = $injector.get('contactBenefitService');
                    var callerInformationService = $injector.get('callerInformationService');
                    var additionalDemographyService = $injector.get('additionalDemographyService');
                    var contactRaceService = $injector.get('contactRaceService');
                    var serviceRecordingService = $injector.get('serviceRecordingService');
                    var WorkflowHeaderService = $injector.get('WorkflowHeaderService');

                    promises.push(registrationService.get(contactID));
                    promises.push(contactBenefitService.getMedicaidNumber(contactID));
                    promises.push(callerInformationService.get(headerID));
                    promises.push(additionalDemographyService.getAdditionalDemographic(contactID));
                    promises.push(contactRaceService.get(contactID));
                    promises.push(serviceRecordingService.getServiceRecording(headerID, SERVICE_RECORDING_SOURCE.LawLiaison));
                    promises.push(WorkflowHeaderService.GetWorkflowHeader($state.current.data.workflowDataKey || workflowHeaderID, $stateParams.CallCenterHeaderID || headerID));
                    $q.all(promises).then(function (data) {

                        if (hasData(data[6])) {
                            contactData = data[6].DataItems[0];
                            callCenterAssessmentPrintService.getCallCenterPrintHeaderDetails(reportModel, contactData);
                        }

                        // Caller phone info
                        if (hasDetails(callerInformation.Phones)) {
                            var phoneData = callerInformation.Phones[0];
                            reportModel.callerPhone = phoneData.Number ? ($filter('toPhone')(phoneData.Number)).toString() : '';

                            //Print the PhoneType if exists
                            if (phoneData.PhoneTypeID) {
                                var phoneType = $filter('filter')(lookupService.getLookupsByType('PhoneType'), { ID: phoneData.PhoneTypeID }, true);
                                reportModel.phoneType = phoneType && phoneType.length > 0 ? phoneType[0].Name : '';
                            }

                            //Print the Phone permission if exists
                            if (phoneData.PhonePermissionID) {
                                var phonePermissions = $filter('filter')(lookupService.getLookupsByType('PhonePermission'), { ID: phoneData.PhonePermissionID }, true);
                                reportModel.phonePermissions = phonePermissions && phonePermissions.length > 0 ? phonePermissions[0].Name : '';
                            }
                            else
                                reportModel.phonePermissions = '';
                           
                        }
                        reportModel.provider = providerName ? providerName : "";
                        reportModel.dateCrisisReceived = providerDate.toString();
                        reportModel.callerFirstName = callerInformation.FirstName;
                        reportModel.callerLastName = callerInformation.LastName;
                        reportModel.incidentDate = $filter('formatDate')(callerDetails.DateOfIncident);
                        reportModel.callReason = callerDetails.ReasonCalled || '';

                        var referringAgency = $filter('filter')(lookupService.getLookupsByType('ReferralAgency'), { ID: callerDetails.ReferralAgencyID }, true);
                        reportModel.referralAgency = referringAgency && referringAgency.length > 0 ? referringAgency[0].Name : '';
                        reportModel.otherReferralAgency = callerDetails.OtherReferralAgency;
                        reportModel.signatureDate = $filter('formatDate')(new Date(), 'MM/DD/YYYY');


                        //Retrieve the Caller information
                        if (hasData(data[2])) {
                            var programData = data[2].DataItems[0];

                            if (programData.CallCenterHeaderID)
                                reportModel.incidentID = programData.CallCenterHeaderID.toString();
                            //if (programData.ProgramUnitID)
                            //    reportModel.contactProgramUnit = lookupService.getText('ProgramUnit', programData.ProgramUnitID);

                            reportModel.screener = lookupService.getText('Users', programData.ProviderID);
                            reportModel.screeningDate = ($filter('formatDate')(new Date(programData.CallStartTime), 'MM/DD/YYYY')).toString();
                            reportModel.screeningTime = ($filter('formatDate')(programData.CallStartTime, 'hh:mm A')).toString();
                        }

                        //service recording details
                        if (hasData(data[5])) {
                            var serviceRecording = data[5].DataItems[0];
                            //reportModel.provider = lookupService.getText("Users", serviceRecording.UserID);
                            reportModel.contactProvider = reportModel.provider;

                            if (serviceRecording.ServiceEndDate) {
                                reportModel.endTime = $filter('toMMDDYYYYDate')(serviceRecording.ServiceEndDate, 'hh:mm A');
                                reportModel.endDate = $filter('toMMDDYYYYDate')(serviceRecording.ServiceEndDate, 'MM/DD/YYYY');
                            }
                            if (serviceRecording.OrganizationID) {
                                reportModel.contactProgramUnit = lookupService.getText('ProgramUnit', serviceRecording.OrganizationID);
                            }
                        }
                    }).finally(function () {
                        deferred.resolve(reportModel);
                    });
                    return deferred.promise;
                }

                return {
                    initReport: initReport
                }
            }])
})();