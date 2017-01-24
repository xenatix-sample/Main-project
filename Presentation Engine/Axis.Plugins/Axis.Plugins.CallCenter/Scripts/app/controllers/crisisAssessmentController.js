(function () {
    angular.module('xenatixApp')
    .controller('crisisAssessmentController', ['$scope', '$filter', '$q', 'alertService', '$stateParams', '$state', '$rootScope', 'registrationService', 'lookupService', 'callerInformationService', 'formService', 'assessmentService', 'contactPhoneService', 'sectionID', 'responseID', 'contactAddressService', 'additionalDemographyService', 'credentialSecurityService', 'contactBenefitService', 'contactSSNService', 'contactRaceService', 'navigationService', 'assessmentPrintService', 'cacheService', 'serviceRecordingService','WorkflowHeaderService','callCenterAssessmentPrintService',
        function ($scope, $filter, $q, alertService, $stateParams, $state, $rootScope, registrationService, lookupService, callerInformationService, formService, assessmentService, contactPhoneService, sectionID, responseID, contactAddressService, additionalDemographyService, credentialSecurityService, contactBenefitService, contactSSNService, contactRaceService, navigationService, printService, cacheService, serviceRecordingService, WorkflowHeaderService, callCenterAssessmentPrintService) {
            $scope.ContactID = $stateParams.ContactID;
            $scope.AssessmentID = ASSESSMENT_TYPE.CrisisAssessment;
            var cssrsAssessmentID = ASSESSMENT_TYPE.ColumbiaSuicideSeverityRatingScale;
            var reportModel = null;
            var cssrsSourceQuestionID = 756;
            var cssrsTargetQuestionID = 586;
            var crisSubTotalScoreQuestionID = 587;
            var crisTotalScoreQuestionID = 588;
            var sendMCOTDateQuestionID = 3530;
            var optionID = 17;
            $scope.inputType = {
                Button: 1,
                Checkbox: 2,
                Radio: 3,
                Textbox: 4,
                Select: 5,
                MultiSelect: 6,
                None: 7,
                DatePicker: 8,
                TextArea: 9
            };
            var cssrsScore = 0;
            $scope.init = function () {
                angular.extend($stateParams, { SectionID: sectionID, ResponseID: responseID });
                $state.transitionTo($state.current.name, $stateParams);
            };

            $scope.prepopulatedData = function () {
                var dfd = $q.defer();
                $scope.GET_DATE = $filter('toMMDDYYYYDate')(new Date());
                var promiseArr = [];
                promiseArr.push(navigationService.get());
                promiseArr.push(serviceRecordingService.getServiceRecording($stateParams.CallCenterHeaderID, SERVICE_RECORDING_SOURCE.CallCenter));
                promiseArr.push(credentialSecurityService.getUserCredentialSecurity());

                $q.all(promiseArr).then(function (response) {
                    if (hasData(response[0])) {
                        $scope.STAFF_NAME = response[0].DataItems[0].UserFullName;
                    }

                    if (hasData(response[1])) {
                        var serviceRecording = response[1].DataItems[0];
                        $scope.SERVICE_ENDDATE = serviceRecording.ServiceEndDate ? $filter('toMMDDYYYYDate')(serviceRecording.ServiceEndDate) : null;
                    }

                    if(hasData(response[2])) {
                        $scope.CredentialList = $filter('filter')(response[2].DataItems, { CredentialActionForm: "Crisis Assessment", CredentialAction: "Digital Signature" }, true);
                    }
                    dfd.resolve(true);
                });
                return dfd.promise;
            };

            $scope.postAssessmentReponseDetails = function () {
                $scope.isDisabled = (cacheService.get('IsReadOnlyScreens')) ? true : false;
                $scope.calculateCSSRSScore(cssrsSourceQuestionID, cssrsTargetQuestionID, crisSubTotalScoreQuestionID, crisTotalScoreQuestionID, optionID);
                $scope.populateAddress().finally(function () {
                    $rootScope.formReset($scope.ctrl.assessmentForm, $scope.ctrl.assessmentForm.$name);
                });
                $scope.setDefaultMCOTDate();              
            };

            //set default Send MCOT Date to Service end date  
            $scope.setDefaultMCOTDate = function () {
                if ($scope.responses)
                {
                    if (!$scope.responses[sendMCOTDateQuestionID][optionID]) {
                        $scope.responses[sendMCOTDateQuestionID][optionID] = $scope.SERVICE_ENDDATE;
                    }
                }                                                 
            };

            $scope.calculateCSSRSScore = function (cssrsSourceQuestionID, cssrsTargetQuestionID, crisSubTotalScoreQuestionID, crisTotalScoreQuestionID, optionID) {
                assessmentService.getAssessmentSections(cssrsAssessmentID).then(function (data) {
                    callerInformationService.getCallCenterAssessmentResponse($stateParams.CallCenterHeaderID, cssrsAssessmentID).then(function (response) {
                        if (hasData(response)) {
                            assessmentService.getAssessmentResponseDetails(response.DataItems[0].ResponseID, data.DataItems[0].AssessmentSectionID).then(function (responseDetails) {
                                var rData = responseDetails.data;
                                if (hasData(rData)) {
                                    angular.forEach(rData.DataItems, function (item) {
                                        if (item.QuestionID == cssrsSourceQuestionID && item.OptionsID == optionID) {
                                            cssrsScore = parseInt(item.ResponseText);
                                            // calculate cssrs & crisis score
                                            if (!$scope.responses[cssrsTargetQuestionID] || $scope.responses[cssrsTargetQuestionID][optionID] != cssrsScore) {
                                                if ($scope.responses[cssrsTargetQuestionID] == undefined)
                                                    $scope.responses[cssrsTargetQuestionID] = {};

                                                $scope.responses[cssrsTargetQuestionID][optionID] = cssrsScore;
                                                $scope.calculateCrisisScore(crisSubTotalScoreQuestionID, crisTotalScoreQuestionID, optionID);
                                            }
                                        }
                                    });
                                }
                            });
                        }
                    });
                });
            }

            $scope.populateAddress = function () {
                return registrationService.get($scope.ContactID).then(function (data) {
                    return contactAddressService.get($scope.ContactID, data.DataItems[0].ContactTypeID).then(function (clientAddressResponse) {
                        if (hasData(clientAddressResponse))
                            clientAddressResponse.DataItems = getPrimaryOrLatestData($filter('filter')(clientAddressResponse.DataItems, { AddressTypeID: ADDRESS_TYPE.ResidenceHome }, true));
                        if ($scope.responses[778] == undefined)
                            $scope.responses[778] = {};
                        if ($scope.responses[778][212] == true && clientAddressResponse.DataItems.length > 0) {
                            $scope.isAddressReadOnly = true;
                            if ($scope.responses[780] == undefined)
                                $scope.responses[780] = {};
                            if ($scope.responses[781] == undefined)
                                $scope.responses[781] = {};
                            if ($scope.responses[782] == undefined)
                                $scope.responses[782] = {};
                            if ($scope.responses[783] == undefined)
                                $scope.responses[783] = {};
                            if ($scope.responses[784] == undefined)
                                $scope.responses[784] = {};
                            $scope.responses[780][17] = clientAddressResponse.DataItems[0].Line1 + ' ' + clientAddressResponse.DataItems[0].Line2;
                            $scope.responses[781][17] = clientAddressResponse.DataItems[0].City;
                            $scope.responses[782][17] = clientAddressResponse.DataItems[0].StateProvince;
                            $scope.responses[783][17] = clientAddressResponse.DataItems[0].County;
                            $scope.responses[784][17] = clientAddressResponse.DataItems[0].Zip;
                        }
                        else if ($scope.responses[778][212] == true || (!$scope.responses[780] && !$scope.responses[781] && !$scope.responses[782] && !$scope.responses[783] && !$scope.responses[784])) {
                            $scope.responses[780] = {};
                            $scope.responses[781] = {};
                            $scope.responses[782] = {};
                            $scope.responses[783] = {};
                            $scope.responses[784] = {};
                            $scope.isAddressReadOnly = $('[data-ng-model="responses[778][212]"]').hasClass('disabled');
                        } else {
                            $scope.isAddressReadOnly = $('[data-ng-model="responses[778][212]"]').hasClass('disabled');
                        }
                    });
                });
            }

            $scope.calculateCrisisScore = function (crisSubTotalScoreQuestionID, crisTotalScoreQuestionID, optionID) {
                var crisScore = $scope.calculateTotalScore();
                if ($scope.responses[crisSubTotalScoreQuestionID] == undefined)
                    $scope.responses[crisSubTotalScoreQuestionID] = {};
                if ($scope.responses[crisTotalScoreQuestionID] == undefined)
                    $scope.responses[crisTotalScoreQuestionID] = {};
                $scope.responses[crisSubTotalScoreQuestionID][optionID] = crisScore;
                $scope.responses[crisTotalScoreQuestionID][optionID] = cssrsScore + crisScore;
            }

            $scope.initReport = function (response) {
                if (response != "-1") {
                    alertService.success('Assessment Response saved successfully.');
                    //save workflow Header details.
                    //$rootScope.$broadcast('onWorkflowHeaderSave', { workflowDataKey: $state.current.data.workflowDataKey, workflowHeaderID: $stateParams.CallCenterHeaderID });
                    WorkflowHeaderService.AddWorkflowHeader({ WorkflowDataKey: $state.current.data.workflowDataKey, RecordHeaderID: $stateParams.CallCenterHeaderID, ContactID: $stateParams.ContactID });
                }
                return printService.initReports($scope.AssessmentID, responseID, sectionID).then(onPrintReportReceived.bind(this));
            }

            var onPrintReportReceived = function (resp) {
                reportModel = resp;
                reportModel.HasLoaded = false;
                reportModel.ReportHeader = 'Crisis Assessment';
                reportModel.ReportName = 'CrisisAssessment';

                var deferred = $q.defer();
                var promises = [];

                var callerInformationDeferred = $q.defer();
                var callerDeferred = $q.defer();
                var clientDeferred = $q.defer();
                var callerPhoneDeferred = $q.defer();
                var clientPhoneDeferred = $q.defer();
                var clientAddressDeferred = $q.defer();
                var addDemoDeferred = $q.defer();
                var benefitDeferred = $q.defer();
                var contactRaceDeferred = $q.defer();
                var serviceRecordingDeferred = $q.defer();
                var workflowHeaderDetailsDeferred = $q.defer();

                promises.push(callerInformationDeferred.promise);
                promises.push(callerDeferred.promise);
                promises.push(clientDeferred.promise);
                promises.push(callerPhoneDeferred.promise);
                promises.push(clientPhoneDeferred.promise);
                promises.push(clientAddressDeferred.promise);
                promises.push(addDemoDeferred.promise);
                promises.push(benefitDeferred.promise);
                promises.push(serviceRecordingDeferred.promise);
                promises.push(workflowHeaderDetailsDeferred.promise);

                callerInformationService.get($stateParams.CallCenterHeaderID).then(function (response) {
                    var callerID = response.DataItems[0].CallerContactID;
                    var clientID = response.DataItems[0].ClientContactID;
                    callerInformationDeferred.resolve(response);

                    WorkflowHeaderService.GetWorkflowHeader($state.current.data.workflowDataKey, $stateParams.CallCenterHeaderID).then(function (headerDetailsResponse) {
                        workflowHeaderDetailsDeferred.resolve(headerDetailsResponse);
                    });

                    registrationService.get(callerID).then(function (callerResponse) {
                        callerDeferred.resolve(callerResponse);
                        contactPhoneService.get(callerID, callerResponse.DataItems[0].ContactTypeID).then(function (callerPhoneResponse) {
                            callerPhoneDeferred.resolve(callerPhoneResponse);
                        });
                    });

                    additionalDemographyService.getAdditionalDemographic(clientID).then(function (addDemoResponse) {
                        addDemoDeferred.resolve(addDemoResponse);
                    });

                    registrationService.get(clientID).then(function (clientResponse) {
                        clientDeferred.resolve(clientResponse);
                        contactPhoneService.get(clientID, clientResponse.DataItems[0].ContactTypeID).then(function (clientPhoneResponse) {
                            clientPhoneDeferred.resolve(clientPhoneResponse);
                        });

                        contactAddressService.getReportModel(clientID, clientResponse.DataItems[0].ContactTypeID).then(function (clientAddressResponse) {
                            clientAddressDeferred.resolve(clientAddressResponse);
                        });
                    });

                    contactBenefitService.getMedicaidNumber(clientID).then(function (data) {
                        benefitDeferred.resolve(data);
                    });

                    contactBenefitService.getMedicaidNumber(clientID).then(function (data) {
                        benefitDeferred.resolve(data);
                    });

                });

                //contact race.
                promises.push(contactRaceDeferred.promise);

                contactRaceService.get($stateParams.ContactID).then(function (data) {
                    contactRaceDeferred.resolve(data);
                });

                serviceRecordingService.getServiceRecording($stateParams.CallCenterHeaderID, SERVICE_RECORDING_SOURCE.CallCenter).then(function (data) {
                    serviceRecordingDeferred.resolve(data);
                });

                $q.all(promises).then(function (data) {

                    if (hasData(data[9])) {
                        contactData = data[9].DataItems[0];
                        callCenterAssessmentPrintService.getCallCenterPrintHeaderDetails(reportModel, contactData);
                    }
                   
                    reportModel.callerFirstName = data[1].DataItems[0].FirstName;
                    reportModel.callerLastName = data[1].DataItems[0].LastName;
                    reportModel.callDate = ($filter('toMMDDYYYYDate')(new Date(data[0].DataItems[0].CallStartTime), 'MM/DD/YYYY', 'useLocal')).toString();
                    reportModel.callTime = ($filter('toMMDDYYYYDate')(data[0].DataItems[0].CallStartTime, 'hh:mm A', 'useLocal')).toString();
                    if (hasData(data[3]) && data[3].DataItems[0].Number) {
                        reportModel.callerContactNumber = ($filter('toPhone')(getPrimaryOrLatestData(data[3].DataItems)[0].Number)).toString();

                    }                    
                    reportModel[cssrsTargetQuestionID.toString()] = cssrsScore;
                    reportModel.signatureDate = ($filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal')).toString();
                    reportModel.atRiskFirstName = data[2].DataItems[0].FirstName;
                    reportModel.atRiskLastName = data[2].DataItems[0].LastName;
                    reportModel.atRiskID = (data[2].DataItems[0].ContactID).toString();
                    if (data[2].DataItems[0].DOB) {
                        reportModel.atRiskDOB = moment(moment(data[2].DataItems[0].DOB).toDate()).format('MM/DD/YYYY');
                        
                    }
                    if (data[2].DataItems[0].GenderID)
                        reportModel.atRiskGender = lookupService.getText('Gender', data[2].DataItems[0].GenderID);

                    if (hasData(data[4]) && data[4].DataItems[0].Number) {
                        reportModel.atRiskContactNumber = ($filter('toPhone')(getPrimaryOrLatestData(data[4].DataItems)[0].Number)).toString();
                      
                    }

                    if (!$.isEmptyObject(data[5])) {
                    				//angular.extend(reportModel, data[5]);
                    				reportModel.residenceLine1 = data[5].contactAddressLine1;
                    				reportModel.residenceLine2 = data[5].contactAddressLine2;
                    				reportModel.residenceCity = data[5].contactCity;
                    				reportModel.residenceState = data[5].contactState;
                    				reportModel.residenceCounty = data[5].contactCounty;
                    				reportModel.residenceZip = data[5].contactZip;
                    }

                    if (data[6].DataItems[0] && data[6].DataItems[0].MaritalStatusID) {
                        reportModel.atRiskMaritalStatus = lookupService.getText('MaritalStatus', data[6].DataItems[0].MaritalStatusID);
                    }

                    if (hasData(data[8])) {
                        var serviceRecording = data[8].DataItems[0];
                        if (serviceRecording.OrganizationID) {
                            reportModel.contactProgramUnit = lookupService.getText('ProgramUnit', serviceRecording.OrganizationID)
                        }
                        if (!reportModel[sendMCOTDateQuestionID])
                            reportModel[sendMCOTDateQuestionID] = ($filter('toMMDDYYYYDate')(data[8].DataItems[0].ServiceEndDate, 'MM/DD/YYYY'));
                    }

                    
                    reportModel.screener = lookupService.getText('Users', data[0].DataItems[0].ProviderID);
                    reportModel.screeningDate = ($filter('toMMDDYYYYDate')(new Date(data[0].DataItems[0].CallStartTime), 'MM/DD/YYYY', 'useLocal')).toString();
                    reportModel.screeningTime = ($filter('toMMDDYYYYDate')(data[0].DataItems[0].CallStartTime, 'hh:mm A', 'useLocal')).toString();
                    reportModel.signatureDate = ($filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal')).toString();

                   
                }).finally(function () {
                    deferred.resolve(reportModel);
                });
                return deferred.promise;
            }

            $scope.init();
        }]);
}());
