(function () {
    angular.module('xenatixApp')
        .controller('callCenterReportController', ['$scope', '$q', '$stateParams', '$filter', 'assessmentPrintService', 'callerInformationService', 'callCenterAssessmentPrintService', 'lookupService', 'registrationService', 'contactPhoneService', 'serviceRecordingService',
            function ($scope, $q, $stateParams, $filter, printService, callerInformationService, callCenterPrintService, lookupService, registrationService, contactPhoneService, serviceRecordingService) {

                $scope.printCrisisLine = function () {
                    var dfd = $q.defer();
                    var reportModel = {
                        HasLoaded: false,
                        ReportHeader: 'CrisisLineAssessments'
                    };
                    
                    var responseArr = [];
                    responseArr.push(callerInformationService.getCallCenterAssessmentResponse($stateParams.CallCenterHeaderID, ASSESSMENT_TYPE.ColumbiaSuicideSeverityRatingScale));
                    responseArr.push(callerInformationService.getCallCenterAssessmentResponse($stateParams.CallCenterHeaderID, ASSESSMENT_TYPE.CrisisAssessment));
                    responseArr.push(callerInformationService.getCallCenterAssessmentResponse($stateParams.CallCenterHeaderID, ASSESSMENT_TYPE.CrisisAdultScreening));
                    responseArr.push(callerInformationService.getCallCenterAssessmentResponse($stateParams.CallCenterHeaderID, ASSESSMENT_TYPE.CrisisChildScreening));

                    var reportArr = [];
                    $q.all(responseArr).then(function (response) {
                        reportArr.push(hasData(response[0]) ? printService.initReports(ASSESSMENT_TYPE.ColumbiaSuicideSeverityRatingScale, response[0].DataItems[0].ResponseID) : true);
                        reportArr.push(hasData(response[1]) ? printService.initReports(ASSESSMENT_TYPE.CrisisAssessment, response[1].DataItems[0].ResponseID) : true);
                        reportArr.push(hasData(response[2]) ? printService.initReports(ASSESSMENT_TYPE.CrisisAdultScreening, response[2].DataItems[0].ResponseID) : true);
                        reportArr.push(hasData(response[3]) ? printService.initReports(ASSESSMENT_TYPE.CrisisChildScreening, response[3].DataItems[0].ResponseID) : true);
                        reportArr.push(callCenterPrintService.getContactReprotInformation($stateParams.ContactID));
                        reportArr.push(serviceRecordingService.getServiceRecording($stateParams.CallCenterHeaderID, SERVICE_RECORDING_SOURCE.CallCenter));

                        $q.all(reportArr).then(function (data) {
                            reportModel.incidentID = $stateParams.CallCenterHeaderID.toString();

                            if (hasData(data[5])) {
                                reportModel.contactProgramUnit = data[5].DataItems[0].OrganizationID ? lookupService.getText('ProgramUnit', data[5].DataItems[0].OrganizationID) : '';
                            }
                            if (data[4]) {
                                angular.extend(reportModel, data[4]);
                            }

                            if (data[0]["974"] || data[1]["3527"] || data[2]["3542"] || data[3]["3552"]) {
                                callerInformationService.get($stateParams.CallCenterHeaderID).then(function (resp) {
                                    if (hasData(resp)) {
                                        var callerDetails = resp.DataItems[0];
                                        if (data[0]["974"]) {       //if cssrs is signed
                                            reportModel.printCSSRS = true;
                                            angular.extend(reportModel, data[0]);
                                            reportModel.cssrs = {
                                                screeningDate: ($filter('toMMDDYYYYDate')(callerDetails.CallStartTime, 'MM/DD/YYYY')).toString(),
                                                screener: lookupService.getText('Users', callerDetails.ProviderID),
                                                screeningTime: ($filter('toMMDDYYYYDate')(callerDetails.CallStartTime, 'hh:mm A')).toString(),
                                                sentToMCOTDate: data[1][3530] ? data[1][3530] : '',
                                                sentToMCOTTime: data[1][3531] ? data[1][3531] : '',
                                            };
                                        }
                                        if (data[2]["3542"]) {       //if adult screening is signed
                                            reportModel.printAdultScreening = true;
                                            angular.extend(reportModel, data[2]);
                                            reportModel.adultScreening = {
                                                screeningDate: ($filter('toMMDDYYYYDate')(callerDetails.CallStartTime, 'MM/DD/YYYY')).toString(),
                                                screener: lookupService.getText('Users', callerDetails.ProviderID),
                                                screeningTime: ($filter('toMMDDYYYYDate')(callerDetails.CallStartTime, 'hh:mm A')).toString(),
                                            };
                                        }
                                        if (data[3]["3552"]) {       //if child screening is signed
                                            reportModel.printChildScreening = true;
                                            angular.extend(reportModel, data[3]);
                                            reportModel.childScreening = {
                                                screeningDate: ($filter('toMMDDYYYYDate')(callerDetails.CallStartTime, 'MM/DD/YYYY')).toString(),
                                                screener: lookupService.getText('Users', callerDetails.ProviderID),
                                                screeningTime: ($filter('toMMDDYYYYDate')(callerDetails.CallStartTime, 'hh:mm A')).toString(),
                                            };
                                        }
                                        reportModel.ReportHeader = 'CrisisLineAssessments';
                                        reportModel.ReportName = 'CrisisLineAssessments';
                                        if (data[1]["3527"]) {
                                            reportModel.printCrisisAssessment = true;
                                            angular.extend(reportModel, data[1]);
                                            reportModel.crisisAssessment = {
                                                atRiskFirstName: data[4].contactFirstName,
                                                atRiskLastName: data[4].contactLastName,
                                                atRiskID: data[4].contactID,
                                                atRiskDOB: data[4].contactDOB ? data[4].contactDOB : '',
                                                atRiskAge: data[4].contactAge ? data[4].contactAge : '',
                                                atRiskGender: data[4].atRiskGender ? data[4].atRiskGender : '',
                                                atRiskMaritalStatus: data[4].atRiskMaritalStatus ? data[4].atRiskMaritalStatus : '',
                                                atRiskContactNumber: reportModel.contactPhone ? reportModel.contactPhone : '',
                                                residenceLine1: reportModel.contactAddressLine1 ? reportModel.contactAddressLine1 : '',
                                                residenceLine2: reportModel.contactAddressLine2 ? reportModel.contactAddressLine2 : '',
                                                residenceCity: reportModel.contactCity ? reportModel.contactCity : '',
                                                residenceState: reportModel.contactState ? reportModel.contactState : '',
                                                residenceZip: reportModel.contactZip ? reportModel.contactZip : '',
                                                residenceCounty: reportModel.contactCounty ? reportModel.contactCounty : '',
                                                callDate: callerDetails.CallStartTime ? ($filter('toMMDDYYYYDate')(callerDetails.CallStartTime, 'MM/DD/YYYY')).toString() : '',
                                                callTime: callerDetails.CallStartTime ? ($filter('toMMDDYYYYDate')(callerDetails.CallStartTime, 'hh:mm')).toString() : ''
                                            };
                                            reportModel.ReportHeader = 'CrisisLineAssessments';
                                            reportModel.ReportName = 'CrisisLineAssessments';
                                            registrationService.get(callerDetails.CallerContactID).then(function (callerResp) {
                                                if (hasData(callerResp)) {
                                                    var caller = callerResp.DataItems[0];
                                                    reportModel.crisisAssessment.callerFirstName = caller.FirstName;
                                                    reportModel.crisisAssessment.callerLastName = caller.LastName;
                                                    contactPhoneService.get(callerDetails.CallerContactID, caller.ContactTypeID).then(function (phoneResp) {
                                                        if (hasData(phoneResp)) {
                                                            var phone = phoneResp.DataItems[0];
                                                            reportModel.crisisAssessment.callerContactNumber = phone.Number ? ($filter('toPhone')(phone.Number)).toString() : '';
                                                            dfd.resolve(reportModel);
                                                        }
                                                    });
                                                } else {
                                                    dfd.resolve(reportModel);
                                                }
                                            });
                                        } else {
                                            dfd.resolve(reportModel);
                                        }
                                    } else {
                                        dfd.resolve(reportModel);
                                    }
                                })
                            } else {
                                dfd.resolve(reportModel);
                            }

                            if (data[1]["3527"]) {      //only if Crisis Assessment is signed
                                //Region yet to populate
                                reportModel.callerRelationship = '';       //In report callerRelationship is in reportModel not in crisisAssessment of reportModel
                                //reportModel.crisisAssessment.callerRelationship = '';       //Kyle to confirm from Rachel C
                            }

                            if (data[0]["756"]) { //only to get latest cssrs scroe value in criss assessement
                                data[1]["586"] = data[0]["756"];
                            }

                        });
                    });
                    reportModel.ReportName = 'CrisisLineAssessments';
                    reportModel.ReportHeader = 'CrisisLineAssessments';
                    return dfd.promise;
                };

            }]);
}());
