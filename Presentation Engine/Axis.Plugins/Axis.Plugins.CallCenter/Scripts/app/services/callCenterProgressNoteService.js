(function () {
angular.module("xenatixApp")
    .factory("callCenterProgressNoteService", ["$http", "$q", "$injector", "$filter", "settings", "offlineData", "assessmentPrintService", "lookupService", "WorkflowHeaderService", "$state", "$stateParams","callCenterAssessmentPrintService",
        function ($http, $q, $injector, $filter, settings, offlineData, assessmentPrintService, lookupService, WorkflowHeaderService, $state, $stateParams, callCenterAssessmentPrintService) {
        var CONFIG = {
            apiControllerRoot: "/data/Plugins/CallCenter/CallCenterProgressNote/",
            offlineApiUrl: "/CallerInformation/"
        };

        var editStateFunc = function editStateSettings() {
            return {
                description: 'CallCenter Progress Note ',
                state: 'callcenter.lawliaison.progressnotes',
                stateParams: { ContactID: this.ContactID, CallCenterHeaderID: this.CallCenterHeaderID, SectionID: this.SectionID, ResponseID: this.ResponseID,ReadOnly:'Edit' }
            };
        };

        function get(callCenterHeaderID) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetCallCenterProgressNote', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (callCenterHeaderID || 0).toString(), 'CallCenterHeaderID', { childKey: 'CallCenterHeaderID' }), params: { callCenterHeaderID: callCenterHeaderID } })
             .success(function (data, status, header, config) {
                 dfd.resolve(data);
             })
             .error(function (data, status, header, config) {
                 dfd.reject(status);
             });
            return dfd.promise;
        };

        function add(callCenter) {
            var dfd = $q.defer();
            if (!('CallCenterHeaderID' in callCenter))
                callCenter.CallCenterHeaderID = 0;
            var data = $.extend(true, {}, callCenter, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (callCenter.CallCenterHeaderID || 0).toString(), 'CallCenterHeaderID', { parentKey: 'CallCenterHeaderID', referenceKeys: ['CallCenterHeaderID', 'CallerContactID', 'ClientContactID'], editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddCallCenterProgressNote', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });
            return dfd.promise;
        };

        function update(callCenter) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, callCenter, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (callCenter.CallCenterHeaderID || 0).toString(), 'CallCenterHeaderID', { parentKey: 'CallCenterHeaderID', referenceKeys: ['CallCenterHeaderID'], editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateCallCenterProgressNote', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        var initReport = function (assessmentID, responseID, sectionID, sourceHeaderID, signature, printType, serviceRecordingModel, callerInformation, workflowDataKey, CallCenterHeaderID) {
            var dfd = $q.defer(), reportModel = {}, promiseArr = [];
            promiseArr.push(assessmentPrintService.initReports(assessmentID, responseID, sectionID));
            promiseArr.push(getCallCenterProgressNoteModel(reportModel, sourceHeaderID, signature, serviceRecordingModel, callerInformation, printType, workflowDataKey, CallCenterHeaderID));
            $q.all(promiseArr).then(function (data) {
                if (data[0]) {
                    angular.merge(reportModel, data[0]);
                }
                if (data[1]) {
                    angular.merge(reportModel, data[1]);
                }
            }).finally(function () {
                reportModel.ReportHeader = (printType == 'crisisProgressNote') ? 'Crisis Line Progress Note' : 'Law Liaison Progress Note';
                reportModel.ReportName = (printType == 'crisisProgressNote') ? 'CrisisLineProgressNote' : 'LawLiaisonProgressNote';
                dfd.resolve(reportModel);
            });
            return dfd.promise;
        };

        var getCallCenterProgressNoteModel = function (reportModel, sourceHeaderID, signature, serviceRecordingModel, callerInformation, printType, workflowDataKey, CallCenterHeaderID) {
            var dfd = $q.defer();
            var callerInformationService = $injector.get('callerInformationService');
            var parentPromiseArr = [];
            var callCenterProgressNoteService = $injector.get('callCenterProgressNoteService');
            parentPromiseArr.push(callerInformationService.get(sourceHeaderID, true));
            parentPromiseArr.push(callCenterProgressNoteService.get(sourceHeaderID));
            $q.all(parentPromiseArr).then(function (resp) {
                var progressNoteDetails;
                if (hasData(resp[1])) {
                    progressNoteDetails = resp[1].DataItems[0];
                    if (progressNoteDetails.Comments)
                        reportModel.comments = printCommentsFromJSON(progressNoteDetails.Comments);
                    if (progressNoteDetails.FollowupPlan) {
                        if (printType === 'crisisProgressNote') {
                            reportModel.followUpPlan = progressNoteDetails.FollowupPlan;
                        } else {
                            reportModel.planOfAction = printCommentsFromJSON(progressNoteDetails.FollowupPlan);
                        }
                    }
                    if (progressNoteDetails.NatureofCall)
                        reportModel.natureOfPhoneCall = (printType === 'crisisProgressNote') ? progressNoteDetails.NatureofCall : printCommentsFromJSON(progressNoteDetails.NatureofCall);

                    if (progressNoteDetails.BehavioralCategoryID)
                        reportModel.behaviorCategory = lookupService.getText("BehavioralCategory", progressNoteDetails.BehavioralCategoryID);
                    reportModel.callerSameContact = progressNoteDetails.IsCallerSame ? progressNoteDetails.IsCallerSame : false;
                    if (progressNoteDetails.CallTypeID)
                        reportModel.typeOfCall = lookupService.getText("CallType", progressNoteDetails.CallTypeID);
                    reportModel.describeOther = progressNoteDetails.CallTypeOther || "";
                    reportModel.contactStatus = lookupService.getText("ClientStatus", progressNoteDetails.ClientStatusID);
                    //if (progressNoteDetails.FollowupPlan)
                    //    reportModel.followUpPlan = progressNoteDetails.FollowupPlan;
                }

                if (hasData(resp[0])) {
                    var contactSSNService = $injector.get('contactSSNService');
                    var contactRaceService = $injector.get('contactRaceService');
                    var additionalDemographyService = $injector.get('additionalDemographyService');
                    var contactBenefitService = $injector.get('contactBenefitService');
                    var registrationService = $injector.get('registrationService');
                    var navigationService = $injector.get('navigationService');

                    var contactID = resp[0].DataItems[0].ClientContactID;
                    var callerID = (printType == "crisisProgressNote") ? resp[0].DataItems[0].CallerContactID : (progressNoteDetails.IsCallerSame ? resp[0].DataItems[0].CallerContactID: progressNoteDetails.NewCallerID);

                    var promises = [];
                    promises.push(registrationService.get(contactID));
                    promises.push(contactBenefitService.getMedicaidNumber(contactID));
                    promises.push(additionalDemographyService.getAdditionalDemographic(contactID));
                    promises.push(contactRaceService.get(contactID));
                    promises.push(callerID ? registrationService.get(callerID) : 1);
                    promises.push(callerInformationService.get(sourceHeaderID));
                    promises.push(WorkflowHeaderService.GetWorkflowHeader($state.current.data.workflowDataKey || workflowDataKey, $stateParams.CallCenterHeaderID || sourceHeaderID));
                    //promises.push(navigationService.get());

                    $q.all(promises).then(function (data) {
                        if (hasData(data[6])) {
                            contactData = data[6].DataItems[0];
                            callCenterAssessmentPrintService.getCallCenterPrintHeaderDetails(reportModel, contactData);
                        }

                        reportModel.date = $filter('formatDate')(resp[0].DataItems[0].CallStartTime);

                        if (hasData(data[4])) {
                            var callerInformation = data[4].DataItems[0];
                            reportModel.callerFirstName = callerInformation.FirstName;
                            reportModel.callerLastName = callerInformation.LastName;

                            if (hasDetails(callerInformation.Phones)) {
                                var phoneData = callerInformation.Phones[0];

                                //Print the PhoneType if exists
                                if (phoneData.PhoneTypeID) {
                                    var phoneType = $filter('filter')(lookupService.getLookupsByType('PhoneType'), { ID: phoneData.PhoneTypeID }, true);
                                    reportModel.phoneType = phoneType && phoneType.length > 0 ? phoneType[0].Name : '';
                                }

                                //Print the Phone Number if exists
                                reportModel.phoneNumber = phoneData.Number ? ($filter('toPhone')(phoneData.Number)).toString() : '';

                                //Print the Phone permission if exists
                                if (phoneData.PhonePermissionID) {
                                    var phonePermissions = $filter('filter')(lookupService.getLookupsByType('PhonePermission'), { ID: phoneData.PhonePermissionID }, true);
                                    reportModel.phonePermissions = phonePermissions && phonePermissions.length > 0 ? phonePermissions[0].Name : '';
                                }
                                else
                                    reportModel.phonePermissions = '';
                            }
                        }

                        if (hasData(data[5])) {
                            var callerDetails = data[5].DataItems[0];
                            reportModel.callReason = callerDetails.ReasonCalled || '';
                            if (callerDetails.CountyID)
                                reportModel.countyOfResidence = lookupService.getText("County", callerDetails.CountyID);
                            if (callerDetails.Disposition)
                                reportModel.disposition = callerDetails.Disposition;
                            var referringAgency = $filter('filter')(lookupService.getLookupsByType('ReferralAgency'), {
                                ID: callerDetails.ReferralAgencyID
                            }, true);
                            reportModel.referralAgency = hasDetails(referringAgency) ? referringAgency[0].Name : '';

                            if (callerDetails.CallCenterHeaderID)
                                reportModel.incidentID = callerDetails.CallCenterHeaderID.toString();
                            reportModel.contactProvider = reportModel.screener = lookupService.getText('Users', callerDetails.ProviderID);
                            reportModel.screeningDate = $filter('formatDate')(callerDetails.CallStartTime);
                            reportModel.startTime = reportModel.screeningTime = $filter('formatDate')(callerDetails.CallStartTime, 'hh:mm A');
                            reportModel.provider = lookupService.getText("Users", resp[0].DataItems[0].ProviderID);
                            if (callerDetails.CallEndTime) {
                                var adjustedTime = "00:00:01";
                                reportModel.endTime = searchString(callerDetails.CallEndTime, adjustedTime) ? "" : $filter('formatDate')(callerDetails.CallEndTime, 'hh:mm A');
                            }
                        }

                        if (serviceRecordingModel) {
                            if (serviceRecordingModel.UserID)
                                reportModel.provider = lookupService.getText("Users", serviceRecordingModel.UserID);
                            if (serviceRecordingModel.OrganizationID) {
                                reportModel.contactProgramUnit = lookupService.getText('ProgramUnit', serviceRecordingModel.OrganizationID);
                            }
                        }

                        var subPromise = [];
                        if (signature && signature.EntityId) {
                            reportModel.credential = (signature.CredentialID) ? lookupService.getText("Credential", signature.CredentialID) : '';
                            reportModel.signatureDate = $filter('formatDate')(signature.ModifiedOn);
                            reportModel.isSigned = true;
                            var userProfileService = $injector.get('userProfileService');
                            subPromise.push(userProfileService.getByID(signature.EntityId, true));
                        } else {
                            subPromise.push(1);
                        }
                        var contactSSNService = $injector.get('contactSSNService');
                        subPromise.push((contactData && contactData.SSN && contactData.SSN.length > 0 && contactData.SSN.length < 9) ? contactSSNService.refreshSSN(contactID, contactData) : 1);

                        $q.all(subPromise).then(function (response) {
                            if (hasData(response[0])) {
                                var userData = response[0].DataItems[0];
                                reportModel.mySignature = userData.PrintSignature;
                            }

                           
                        }).finally(function () {
                            dfd.resolve(reportModel);
                        });
                    });
                }
                else {
                    dfd.resolve(reportModel);
                }
            });
            return dfd.promise;
        }

        //TODO: remove after verifying what all details are already available on controller and pass them as parameter
        //var printProgressNoteDummy = function (resp, isLawLiaison, isCrisisLine, contactID, sourceHeaderID) {
        //    var reportModel = resp;
        //    reportModel.HasLoaded = false;
        //    if (isLawLiaison) {
        //        reportModel.ReportHeader = 'Law Liaison Progress Note';
        //        reportModel.ReportName = 'LawLiaisonProgressNote';
        //    }
        //    else {
        //        reportModel.ReportHeader = 'Crisis Line Progress Note';
        //        reportModel.ReportName = 'CrisisLineProgressNote';
        //    }

        //    var deferred = $q.defer();
        //    var promises = [];
        //    promises.push(registrationService.get(contactID));
        //    promises.push(contactBenefitService.getMedicaidNumber(contactID));
        //    promises.push(callerInformationService.get(sourceHeaderID));
        //    promises.push(additionalDemographyService.getAdditionalDemographic(contactID));
        //    promises.push(contactRaceService.get(contactID));
        //    promises.push(serviceRecordingService.getServiceRecording(sourceHeaderID, ((isLawLiaison) ? SERVICE_RECORDING_SOURCE.LawLiaison : SERVICE_RECORDING_SOURCE.CallCenter)));

        //    if ($scope.signature && $scope.signature.CredentialID && $scope.signature.EntityId)
        //        promises.push(userProfileService.getByID($scope.signature.EntityId, true));
        //    $q.all(promises).then(function (data) {
        //        if (hasData(data[0])) {
        //            var contactData = data[0].DataItems[0];

        //            reportModel.contactFirstName = contactData.FirstName;
        //            reportModel.contactLastName = contactData.LastName;
        //            reportModel.contactMiddleName = contactData.Middle ? contactData.Middle : '';
        //            if (contactData.MRN)
        //                reportModel.mrn = contactData.MRN.toString();

        //            //Retrieve the SSN details
        //            if (contactData.SSN && contactData.SSN.length > 0 && contactData.SSN.length < 9) {
        //                contactSSNService.refreshSSN(contactID, contactData).then(function (response) {
        //                    if (contactData.SSN)
        //                        reportModel.contactSSN = contactData.SSN; // $filter('toMaskSSN')(contactData.SSN);
        //                });
        //            }

        //            //Retrieve the DOB/Age details
        //            if (contactData.DOB) {
        //                reportModel.contactDOB = ($filter('formatDate')(contactData.DOB, 'MM/DD/YYYY')).toString(); //Remove the new keyword
        //                reportModel.contactAge = $filter('toAge')(contactData.DOB).toString();
        //            }
        //        }
        //        reportModel.date = $filter('formatDate')(self.providerDetail.ProviderDate, 'MM/DD/YYYY').toString();
        //        reportModel.callerFirstName = self.callerInformation.FirstName;
        //        reportModel.callerLastName = self.callerInformation.LastName;
        //        reportModel.callReason = self.callerDetails.ReasonCalled || '';
        //        if (self.ProgressNoteDetails.Comments)
        //            reportModel.comments = printCommentsFromJSON(self.ProgressNoteDetails.Comments);
        //        if (self.ProgressNoteDetails.preFollowupPlan)
        //            reportModel.planOfAction = printCommentsFromJSON(JSON.stringify(self.ProgressNoteDetails.preFollowupPlan));
        //        if (self.ProgressNoteDetails.preNatureofCall)
        //            reportModel.natureOfPhoneCall = printCommentsFromJSON(JSON.stringify(self.ProgressNoteDetails.preNatureofCall));
        //        if (self.ProgressNoteDetails.BehavioralCategoryID)
        //            reportModel.behaviorCategory = lookupService.getText("BehavioralCategory", self.ProgressNoteDetails.BehavioralCategoryID);
        //        reportModel.callerSameContact = self.ProgressNoteDetails.IsCallerSame

        //        if (self.ProgressNoteDetails.CallTypeID)
        //            reportModel.typeOfCall = lookupService.getText("CallType", self.ProgressNoteDetails.CallTypeID);
        //        reportModel.describeOther = self.ProgressNoteDetails.CallTypeOther || "";
        //        if (self.callerDetails.CountyID)
        //            reportModel.countyOfResidence = lookupService.getText("County", self.callerDetails.CountyID)
        //        //if (self.callerDetails.ProgramUnitID)
        //        //    reportModel.contactProgramUnit = lookupService.getText('ProgramUnit', self.callerDetails.ProgramUnitID)

        //        reportModel.contactStatus = lookupService.getText("ClientStatus", self.ProgressNoteDetails.ClientStatusID);
        //        if (self.clientDetails.THSAID)
        //            reportModel.thsaID = self.clientDetails.THSAID;
        //        /// Crisis line 
        //        if (self.callerDetails.Disposition)
        //            reportModel.disposition = self.callerDetails.Disposition;
        //        if (self.ProgressNoteDetails.FollowupPlan)
        //            reportModel.followUpPlan = self.ProgressNoteDetails.FollowupPlan;
        //        if (self.ProgressNoteDetails.NatureofCall && isCrisisLine)
        //            reportModel.natureOfPhoneCall = self.ProgressNoteDetails.NatureofCall;
        //        /// End Crisis line

        //        var referringAgency = $filter('filter')(lookupService.getLookupsByType('ReferralAgency'), { ID: self.callerDetails.ReferralAgencyID }, true);
        //        reportModel.referralAgency = referringAgency && referringAgency.length > 0 ? referringAgency[0].Name : '';

        //        if (self.Phones && self.Phones.length > 0) {
        //            //Print the PhoneType if exists
        //            if (self.Phones[0].PhoneTypeID) {
        //                var phoneType = $filter('filter')(lookupService.getLookupsByType('PhoneType'), { ID: self.Phones[0].PhoneTypeID }, true);
        //                reportModel.phoneType = phoneType && phoneType.length > 0 ? phoneType[0].Name : '';
        //            }

        //            //Print the Phone Number if exists
        //            reportModel.phoneNumber = self.Phones[0].Number ? ($filter('toPhone')(self.Phones[0].Number)).toString() : '';

        //            //Print the Phone permission if exists
        //            if (self.Phones[0].PhonePermissionID) {
        //                var phonePermissions = $filter('filter')(lookupService.getLookupsByType('PhonePermission'), { ID: self.Phones[0].PhonePermissionID }, true);
        //                reportModel.phonePermissions = phonePermissions && phonePermissions.length > 0 ? phonePermissions[0].Name : '';
        //            }
        //            else
        //                reportModel.phonePermissions = '';
        //        }
        //        //Get the Mediclaim details
        //        reportModel.medicaidNum = data[1];

        //        //Retrieve the Caller information
        //        if (hasData(data[2])) {
        //            var programData = data[2].DataItems[0];

        //            if (programData.CallCenterHeaderID)
        //                reportModel.incidentID = programData.CallCenterHeaderID.toString();
        //            reportModel.screener = lookupService.getText('Users', programData.ProviderID);
        //            reportModel.screeningDate = ($filter('formatDate')(new Date(programData.CallStartTime), 'MM/DD/YYYY')).toString();
        //            reportModel.screeningTime = ($filter('formatDate')(programData.CallStartTime, 'hh:mm A')).toString();
        //        }

        //        //Retrieve the Ethnicity Details
        //        if (hasData(data[3])) {
        //            var additionalDemoData = data[3].DataItems[0];

        //            if (additionalDemoData.EthnicityID)
        //                reportModel.contactEthnicity = lookupService.getText('Ethnicity', additionalDemoData.EthnicityID);
        //        }

        //        //Retrieve the Race details
        //        if (hasData(data[4])) {
        //            var raceLookupList = lookupService.getLookupsByType('Race');
        //            reportModel.contactRace = getRaceCSVNames(raceLookupList, data[4].DataItems);
        //        }

        //        if (self.callerDetails.ProgramUnitID)
        //            reportModel.contactProgramUnit = lookupService.getText('ProgramUnit', self.callerDetails.ProgramUnitID)

        //        if (self.providerDetail.ProviderBy)
        //            reportModel.provider = lookupService.getText("Users", self.providerDetail.ProviderBy);

        //        //Retrieve the serviceRecording details
        //        if (hasData(data[5])) {
        //            var serviceRecording = data[5].DataItems[0];
        //            reportModel.provider = lookupService.getText("Users", serviceRecording.UserID);
        //            reportModel.contactProvider = reportModel.provider;
        //            if (serviceRecording.OrganizationID) {
        //                reportModel.contactProgramUnit = lookupService.getText('ProgramUnit', serviceRecording.OrganizationID)
        //            }
        //        }


        //        reportModel.startTime = self.providerDetail.ProviderStartTime.toString();
        //        reportModel.endTime = self.providerDetail.ProviderEndTime;

        //        //Signature details
        //        if ($scope.signature && $scope.signature.CredentialID && hasData(data[6])) {
        //            let credential = $filter('filter')($scope.userCredentials, function (itm) {
        //                return itm.CredentialID == $scope.signature.CredentialID
        //            }, true);
        //            if (credential && credential.length) {
        //                reportModel.credential = credential[0].CredentialName;
        //            }
        //            reportModel.signatureDate = $scope.signature.DateSigned.toString();
        //            var userData = data[6].DataItems[0];
        //            reportModel.isSigned = true;
        //            reportModel.mySignature = userData.PrintSignature;
        //        }

        //        var subPromises = [];
        //        var contactTypeID = data[0].DataItems[0].ContactTypeID;

        //        subPromises.push(contactAddressService.getReportModel(contactID, contactTypeID));
        //        subPromises.push(contactPhoneService.get(contactID, contactTypeID));

        //        $q.all(subPromises).then(function (response) {
        //            //Get the Address details
        //            if (!$.isEmptyObject(response[0])) {
        //                angular.extend(reportModel, response[0]);
        //            }

        //            //Get the Contact Phone details
        //            if (hasData(response[1])) {
        //                var phoneData = getPrimaryOrLatestData(response[1].DataItems);

        //                reportModel.contactPhone = phoneData[0].Number
        //                                                ? ($filter('toPhone')(phoneData[0].Number)).toString()
        //                                                : '';
        //            }
        //            reportModel.HasLoaded = true;
        //            $scope.reportModel = reportModel;
        //            $('#reportModal').modal('show');
        //            return deferred.resolve(reportModel);
        //        });
        //    });

        //    return deferred.promise;
        //}

        return {
            get: get,
            add: add,
            update: update,
            initReport: initReport
        };

    }]);
})();