(function () {
    angular.module('xenatixApp')
    .factory('callCenterAssessmentPrintService', ["$http", "$q", 'assessmentPrintService', 'serviceRecordingService', 'callerInformationService',
        'registrationService', 'contactBenefitService', 'additionalDemographyService', 'contactRaceService', 'lookupService', '$filter', 'contactAddressService', 'contactPhoneService', 'navigationService', 'contactSSNService', '$stateParams', 'assessmentService', 'WorkflowHeaderService', '$state',
        function ($http, $q, assessmentPrintService, serviceRecordingService, callerInformationService,
            registrationService, contactBenefitService, additionalDemographyService, contactRaceService, lookupService, $filter, contactAddressService, contactPhoneService, navigationService, contactSSNService, $stateParams, assessmentService, WorkflowHeaderService, $state) {

            var adjustedTime = "12:00:01";
            function getPrintData(contactID, assessmentID, responseID, sectionId) {
                return assessmentPrintService.initReports(assessmentID, responseID, sectionId);
            };

            function getContactReprotInformation(contactID, callCenterHeaderID,workflowDataKey) {
                var reportModel = {};
                reportModel.HasLoaded = false;
                var deferred = $q.defer();
                var promises = [];

                promises.push(registrationService.get(contactID));
                promises.push(contactBenefitService.getMedicaidNumber(contactID));
                promises.push(additionalDemographyService.getAdditionalDemographic(contactID));
                promises.push(contactRaceService.get(contactID));               
                if (callCenterHeaderID) {
                    promises.push(serviceRecordingService.getServiceRecording(callCenterHeaderID, SERVICE_RECORDING_SOURCE.CallCenter));
                    promises.push(callerInformationService.get(callCenterHeaderID));                   
                    promises.push(WorkflowHeaderService.GetWorkflowHeader($state.current.data.workflowDataKey || workflowDataKey, $stateParams.CallCenterHeaderID || callCenterHeaderID));
                    reportModel.incidentID = callCenterHeaderID.toString();
                }
                $q.all(promises).then(function (data) {
                    if (callCenterHeaderID) {
                        if (hasData(data[6])) {
                            getCallCenterPrintHeaderDetails(reportModel, data[6].DataItems[0], callCenterHeaderID);
                        }
                    }
                    
                    reportModel.signatureDate = ($filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY')).toString();

                    assessmentService.getAssessmentResponses(contactID, ASSESSMENT_TYPE.CrisisAssessment).then(function (response) {
                        if (hasData(response)) {
                            assessmentService.getAssessmentResponseDetails(response.DataItems[0].ResponseID, ASSESSMENT_SECTION.RiskFactors).then(function (responseDetails) {
                                var rData = responseDetails.data;
                                if (hasData(rData)) {
                                    $filter('filter')(rData.DataItems, function (question) { if (question.QuestionID == '3530') reportModel.sentToMCOTDate = question.ResponseText; });
                                    $filter('filter')(rData.DataItems, function (question) { if (question.QuestionID == '3531') reportModel.sentToMCOTTime = searchString(question.ResponseText, adjustedTime) ? "" : ($filter('toMMDDYYYYDate')(question.ResponseText, 'hh:mm A', 'useLocal')).toString(); });
                                }
                            });
                        }
                    });
                    //Retrieve the Ethnicity Details
                    if (hasData(data[2])) {
                        var additionalDemoData = data[2].DataItems[0];

                        if (additionalDemoData.MaritalStatusID) {
                            reportModel.atRiskMaritalStatus = lookupService.getText('MaritalStatus', additionalDemoData.MaritalStatusID); 
                        }
                    }

                    var subPromises = [];
                    var contactTypeID = data[0].DataItems[0].ContactTypeID;
                    subPromises.push(contactAddressService.getReportModel(contactID, contactTypeID));
                    subPromises.push(contactPhoneService.get(contactID, contactTypeID));

                    if (callCenterHeaderID) {
                        if (hasData(data[4]))
                            reportModel.contactProgramUnit = data[4].DataItems[0].OrganizationID ? lookupService.getText('ProgramUnit', data[4].DataItems[0].OrganizationID) : '';
                        if (hasData(data[5])) {
                            var callerDetails = data[5].DataItems[0];
                            reportModel.screeningDate = $filter('toMMDDYYYYDate')(callerDetails.CallStartTime);
                            reportModel.screener = lookupService.getText('Users', callerDetails.ProviderID);
                            reportModel.screeningTime = $filter('toMMDDYYYYDate')(callerDetails.CallStartTime, 'hh:mm A');
                            subPromises.push(registrationService.get(callerDetails.CallerContactID));
                        }
                    }

                    $q.all(subPromises).then(function (response) {
                        //Get the Address details
                        //if (!$.isEmptyObject(response[0])) {
                        //    angular.extend(reportModel, response[0]);
                        //}

                        //Get the Contact Phone details
                        //if (hasData(response[1])) {
                        //    var phoneData = response[1].DataItems[0];

                        //    reportModel.contactPhone = reportModel.atRiskContactNumber = phoneData.Number ? ($filter('toPhone')(phoneData.Number)).toString() : '';
                        //}

                        if (hasData(response[2])) {
                            var caller = response[2].DataItems[0];
                            reportModel.callerFirstName = caller.FirstName;
                            reportModel.callerLastName = caller.LastName;
                            if (hasDetails(caller.Phones)) {
                                reportModel.callerContactNumber = caller.Phones[0].Number ? ($filter('toPhone')(caller.Phones[0].Number)).toString() : '';
                            }
                        }

                        return deferred.resolve(reportModel);
                    });
                });

                return deferred.promise;
            }

            function getCallCenterPrintHeaderDetails(reportModel, contactData, callCenterHeaderID)
            {
                if (contactData) {
                    reportModel.contactFirstName = contactData.FirstName;
                    reportModel.contactLastName = contactData.LastName;
                    reportModel.contactMiddleName = contactData.Middle ? contactData.Middle : '';
                    reportModel.contactID = (contactData.ContactID).toString();
                    reportModel.mrn = contactData.MRN ? (contactData.MRN).toString() : '';
                    if (contactData.SuffixID)
                        reportModel.contactSuffix = lookupService.getText('Suffix', contactData.SuffixID);

                    if (contactData.DOB)
                        reportModel.contactDOB = $filter('formatDate')(contactData.DOB);
                    reportModel.medicaidNum = contactData.MedicaidID || 'N/A';

                    reportModel.contactEthnicity = contactData.Ethnicity || '';
                    reportModel.contactRace = contactData.Race || '';
                    reportModel.thsaID = contactData.THSAID || '';
                    reportModel.contactAge = $filter('toAge')(contactData.DOB).toString() || '';
                    if($stateParams)
                    {
                        reportModel.incidentID = ($stateParams.CallCenterHeaderID) ? $stateParams.CallCenterHeaderID.toString() : '';
                    }
                    else{
                        reportModel.incidentID = (callCenterHeaderID) ? callCenterHeaderID.toString() :  '';
                    }
                    
                    reportModel.contactSSN = $filter('toMaskSSN')(contactData.SSN) || '';
                    reportModel.contactGateCode = contactData.GateCode ? contactData.GateCode : '';
                    reportModel.contactComplexName = contactData.ComplexName ? contactData.ComplexName : '';
                    reportModel.contactApartment = reportModel.contactComplexName + ', ' + reportModel.contactGateCode;
                    reportModel.contactAddressLine1 = contactData.Line1 ? contactData.Line1 : '';
                    reportModel.contactAddressLine2 = contactData.Line2 ? contactData.Line2 : '';
                    reportModel.contactCity = contactData.City ? contactData.City : '';
                    reportModel.contactState = contactData.StateProvince || ''; //? lookupService.getText('StateProvince', contactData.StateProvince) : '';
                    reportModel.contactZip = contactData.Zip ? contactData.Zip : '';
                    reportModel.contactCounty = contactData.County ? lookupService.getText('County', contactData.County) : '';
                   // reportModel.contactProgramUnit = lookupService.getText('ProgramUnit', serviceRecordingModel.OrganizationID);

                    //Print the PhoneType if exists
                    if (contactData.PhoneTypeID) {
                        var phoneType = $filter('filter')(lookupService.getLookupsByType('PhoneType'), { ID: contactData.PhoneTypeID }, true);
                        reportModel.phoneType = phoneType && phoneType.length > 0 ? phoneType[0].Name : '';
                    }

                    //Print the Phone Number if exists
                    reportModel.phoneNumber = contactData.Number ? ($filter('toPhone')(contactData.Number)).toString() : '';

                    //Print the Phone permission if exists
                    if (contactData.PhonePermissionID) {
                        var phonePermissions = $filter('filter')(lookupService.getLookupsByType('PhonePermission'), { ID: contactData.PhonePermissionID }, true);
                        reportModel.phonePermissions = phonePermissions && phonePermissions.length > 0 ? phonePermissions[0].Name : '';
                    }
                    else
                        reportModel.phonePermissions = '';
                }
            }
            

            return {
                getPrintData: getPrintData,
                getContactReprotInformation: getContactReprotInformation,
                getCallCenterPrintHeaderDetails: getCallCenterPrintHeaderDetails
            };
        }])

}());
