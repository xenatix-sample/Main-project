(function () {
    angular.module('xenatixApp')
    .controller('consentDetailsController', ['$scope', '$q', 'consentsPrintService', '$stateParams', 'alertService', '$state', '$filter', 'navigationService', 'lookupService', '$injector', 'contactBenefitService', 'formNamePromise', 'consentsService', 'formService', '$timeout', 'contactRelationshipService', 'admissionService', '$rootScope', 'roleSecurityService', 'WorkflowHeaderService',
        function ($scope, $q, printService, $stateParams, alertService, $state, $filter, navigationService, lookupService, $injector, contactBenefitService, formNamePromise, consentsService, formService, $timeout, contactRelationshipService, admissionService, $rootScope, roleSecurityService,WorkflowHeaderService) {
            var isECIClient = false;
            var strECI = DIVISION_NAME.ECS;
            ////Currently present all the parameters need to be populated dynamically
            //TODO: Populate as per required for consent
            $scope.enableSave = true;
            $scope.enablePrint = true;
            $scope.AGENCY_ADDRESS = AGENCY_Data.ADDRESS;
            $scope.AGENCY_CITY = AGENCY_Data.CITY;
            $scope.AGENCY_NAME = AGENCY_Data.NAME;
            $scope.AGENCY_STATE = AGENCY_Data.STATE;
            $scope.AGENCY_ZIP = AGENCY_Data.ZIP;
            $scope.ASSESSMENT_ID = $stateParams.AssessmentID;
            $scope.ASSESSMENT_SECTION_ID = $stateParams.SectionID;
            $scope.EXPIRE_ASSESSMENT_ID = $stateParams.ExpireAssessmentID;
            $scope.ALWAYS_TRUE = true;
            $scope.ASSESSMENT_TYPE = ASSESSMENT_TYPE;
            //$scope.AGENCY_PHONE =
            var userID;
            var credentialList = [];
            var hasEditPermission = roleSecurityService.hasPermission("Consents-Assessment-Agency", PERMISSION.UPDATE);
            //$scope.CLIENT_DOCTOR_MEDICAL_FACILITY =
            //$scope.CLIENT_DOCTOR_NAME =
            //$scope.CLIENT_DOCTOR_PHONE =
            //$scope.CLIENT_DOCTOR_SPECIALTY =

            $scope.GET_DATE = $filter('toMMDDYYYYDate')(new Date, 'MM/DD/YYYY', 'useLocal');
            $scope.MEDICAID_NUMBER = 'N/A';

            contactBenefitService.get($scope.contactID).then(function (data) {
                if (data && data.DataItems.length > 0) {
                    var payors = $filter('filter')(data.DataItems, function (itm) {
                        return itm.PayorName.toString().toLowerCase().indexOf('medicaid') > -1;
                    })
                    if (payors && payors.length > 0) {
                        $scope.MEDICAID_NUMBER = payors[0].PolicyID;
                    }
                }
            });

            var pushProgramUnit = function (programUnitID) {
                var filteredProgramUnit = $filter('filter')($rootScope.getOrganizationByDataKey('ProgramUnit'), { ID: programUnitID }, true);
                if (hasDetails(filteredProgramUnit))
                    $scope.programUnitList.push({ ID: filteredProgramUnit[0].ID, Name: filteredProgramUnit[0].Name });
                $scope.programUnitList = $filter('orderBy')($scope.programUnitList, 'Name');
            };

            var getPostProgramUnit = function () {
                var consentToSearchPU = $scope.responses[1051];
                console.log(isSigned());
                console.log($stateParams.ReadOnly == 'view');
                console.log($scope.noAccessToOther);
                if ($stateParams.ContactConsentID && consentToSearchPU && ($scope.noAccessToOther || !hasEditPermission)) {
                    var isExistProgramUnit = $filter('filter')($scope.programUnitList, function (existPUID) {
                        return existPUID.ID === consentToSearchPU[17];
                    }, true);
                    if (isExistProgramUnit.length == 0)
                        pushProgramUnit(consentToSearchPU[17]);
                }
            };

            var getDefaultProgramUnit = function () {
                admissionService.get($stateParams.ContactID).then(function (data) {
                    $scope.programUnitList = [];
                    if (hasData(data)) {
                        var admittedProgramUnit = $filter('filter')(data.DataItems, { DataKey: 'ProgramUnit', IsDischarged: false }, true);
                        if (hasDetails(admittedProgramUnit)) {
                            angular.forEach(admittedProgramUnit, function (item) {
                                pushProgramUnit(item.ProgramUnitID);
                            })
                            if ($scope.programUnitList && $scope.programUnitList.length == 1) {
                                $scope.PROGRAM_UNIT_ID = $scope.programUnitList[0].ID;
                            }
                        }
                    }
                })
            };

            navigationService.get().then(function (response) {
                if (response && response.DataItems && response.DataItems.length > 0) {
                    $scope.STAFF_NAME = response.DataItems[0].UserFullName;
                    userID = response.DataItems[0].UserID;
                    $scope.CredentialList = $filter('orderBy')(response.DataItems[0].UserCredentials, 'CredentialName');
                    if ($scope.CredentialList && $scope.CredentialList.length == 1) {
                        $scope.CREDENTIAL_ID = $scope.CredentialList[0].CredentialID;
                }
                }
            });

            var setDemographicScope = function (registrationData) {
                var dfd = $q.defer();
                var prefix = registrationData.TitleID ? lookupService.getText("PrefixType", registrationData.TitleID) + ' ' : '';
                var middle = registrationData.Middle ? ' ' + registrationData.Middle : '';
                var sufix = registrationData.SuffixID ? ' ' + lookupService.getText("Suffix", registrationData.SuffixID) : '';
                $scope.CLIENT_FULLNAME = prefix + registrationData.FirstName + middle + ' ' + registrationData.LastName + sufix;
                $scope.CLIENT_NAME = registrationData.FirstName + middle + ' ' + registrationData.LastName + sufix;
                $scope.CONSENT_NAME = formNamePromise;
                if (registrationData.ClientTypeID) {
                    var clientType = lookupService.getLookupsByType('ClientType').filter(function (obj) { return obj.ID === registrationData.ClientTypeID; });
                    if (clientType && clientType.length > 0)
                        $scope.CLIENT_PROGRAM_UNIT = clientType[0].Name;
                }
                $scope.DOB = $filter('formatDate')(registrationData.DOB, 'MM/DD/YYYY');
                $scope.MRN = registrationData.MRN ? registrationData.MRN : 'PENDING';
                var collateralService = $injector.get('collateralService');
                var additionalDemographyService = $injector.get('additionalDemographyService');

                var contactTypeText = isECIClient ? 'Family Relationship' : 'Collateral';
                var contactTypeId = lookupService.getLookupsByType('ContactType').filter(function (obj) { return obj.Name === contactTypeText; })[0].ID;
                
                collateralService.get($stateParams.ContactID, contactTypeId, false).then(function (resp) {
                    if (hasData(resp)) {
                        resp.DataItems = $filter('orderBy')(resp.DataItems, "ModifiedOn", true);
                        var emergencyCollateralList = resp.DataItems.filter(function (obj) { return obj.EmergencyContact === true; });
                        if (hasDetails(emergencyCollateralList)) {
                            $scope.LIST_JSON = [];
                            var prmoiseArr = [];
                            angular.forEach(emergencyCollateralList, function (emergencyCollateral, idx) {
                                //var emergencyCollateral = emergencyCollateralList[0];
                                var EMERGENCY_CONTACT_NAME = emergencyCollateral.FirstName + ' ' + emergencyCollateral.LastName;
                                var EMERGENCY_CONTACT_PHONE;
                                var EMERGENCY_CONTACT_ADDRESS;
                                if (hasDetails(emergencyCollateral.Phones)) {
                                    var ecPhone = emergencyCollateral.Phones[0];
                                    if (ecPhone) {
                                        EMERGENCY_CONTACT_PHONE = $filter('toPhone')(ecPhone.Number);
                                    }
                                }
                                var ecAddress = emergencyCollateral.Addresses;
                                if (hasDetails(ecAddress)) {
                                    // Calculate Address
                                    addModel = ecAddress[0];
                                    var addressLineFormat = ((checkModel(addModel.Line1)) ? addModel.Line1 + " " : "") +
                                          ((checkModel(addModel.Line2)) ? addModel.Line2 : "");

                                    var addressCityFormat = ((checkModel((addModel.City)) ? addModel.City : "") + ((checkModel(addModel.StateProvince) && (addModel.StateProvince > 0)) ?
                                    ((checkModel(addModel.City)) ? ", " + lookupService.getText("StateProvince", addModel.StateProvince) :
                                        lookupService.getText("StateProvince", addModel.StateProvince)) : "") + ((checkModel(addModel.Zip)) ? " " + addModel.Zip : ""));

                                    EMERGENCY_CONTACT_ADDRESS = (checkModel(addressLineFormat) ? addressLineFormat : "") + " " + addressCityFormat;
                                }
                                prmoiseArr.push(getContactRelationshipDetails(emergencyCollateral,
                                                                            registrationData.ContactID,
                                                                            EMERGENCY_CONTACT_NAME,
                                                                            EMERGENCY_CONTACT_PHONE,
                                                                            EMERGENCY_CONTACT_ADDRESS));
                            });
                            if (prmoiseArr.length > 0) {
                                $q.all(prmoiseArr).then(function (response) {
                                    dfd.resolve(true);
                                });
                            } else {
                                dfd.resolve(true);
                            }
                        } else {
                            dfd.resolve(true);
                        }
                    } else {
                        dfd.resolve(true);
                    }
                });
                return dfd.promise;
            }

            var getContactRelationshipDetails = function (emergencyCollateral, contactID, contactName, contactPhone, contactAddress) {
                var dfd = $q.defer();
                contactRelationshipService.get(emergencyCollateral.ContactID, contactID).then(function (data) {
                    var EMERGENCY_CONTACT_RELATIONSHIP;
                    if (hasData(data)) {
                        var contactRelationships = data.DataItems[0];
                        var relationships = lookupService.getLookupsByType('RelationshipType').filter(function (obj) { return obj.ID === contactRelationships.RelationshipTypeID; }, true);
                        if (hasDetails(relationships))
                            EMERGENCY_CONTACT_RELATIONSHIP = relationships[0].Name;
                    }
                    $scope.LIST_JSON.push({
                            LabelText: contactName +
                                    (EMERGENCY_CONTACT_RELATIONSHIP ? (', ' + EMERGENCY_CONTACT_RELATIONSHIP) : '') +
                                    (contactAddress ? (', ' + contactAddress): '') +
                                    (contactPhone ? (', ' + contactPhone): ''),
                            IsSelected: false,
                            Id: 'emergencyContact' + emergencyCollateral.ContactID,
                            ContactID: emergencyCollateral.ContactID
                            //EMERGENCY_CONTACT_RELATIONSHIP: EMERGENCY_CONTACT_RELATIONSHIP,
                            //EMERGENCY_CONTACT_ADDRESS: contactAddress,
                            //EMERGENCY_CONTACT_PHONE: contactPhone
                        });
                    dfd.resolve(true);
                });
                return dfd.promise;
            }

            $scope.prepopulatedData = function () {
                var dfd = $q.defer();
                //get dropdown list
                $scope.hideNextAssessment = true;       //Need to hide next for consent
                getDefaultProgramUnit();
                if ($state.current.name == "consentexpire") {   //Only in case of expire consent
                    $scope.isPrintReportRequired = false;

                    $scope.AssessmentExpireReasons = lookupService.getLookupsByType('AssessmentExpirationReason');
                    if ($stateParams.ExpireAssessmentID != ASSESSMENT_TYPE.AuthorizationtoDiscloseHealthInformation && $stateParams.ExpireAssessmentID != ASSESSMENT_TYPE.ConsentforPublication) {
                        //remove revoke option from dropdown list
                        $scope.AssessmentExpireReasons = $filter('filter')($scope.AssessmentExpireReasons, function (item) {
                            return (item.ID !== Assessment_Expire_Reason.Revoke);
                        });
                    }
                    $scope.Assessment_Expire_Reason = Assessment_Expire_Reason;
                };

                if ($injector.has('registrationService')) {
                    var registrationService = $injector.get('registrationService');
                    registrationService.get($scope.contactID).then(function (data) {
                        if (!hasData(data)) {
                            //no data so let's try the ecidemographicservice
                            if ($injector.has('eciDemographicService')) {
                                var eciDemographicService = $injector.get('eciDemographicService');
                                eciDemographicService.get($scope.contactID).then(function (ecidata) {
                                    if (hasData(ecidata)) {
                                        demographicData = ecidata.DataItems[0];
                                        isECIClient = true;
                                        setDemographicScope(demographicData).then(function () {
                                            dfd.resolve(true);
                                        });
                                    } else {
                                        dfd.resolve(true);
                                    }
                                });
                            } else {
                                dfd.resolve(true);
                            }
                        } else {
                            demographicData = data.DataItems[0];
                            if (demographicData.ClientTypeID === $scope.ClientTypeList.filter(function (obj) { return obj.Name === strECI; })[0].ID) {
                                isECIClient = true;
                            }
                            setDemographicScope(demographicData).then(function () {
                                dfd.resolve(true);
                            });
                        }
                    });
                } else {
                    dfd.resolve(true);
                }
                return dfd.promise;
            }

            $scope.initAgencyReport = function (response) {
                // saveConsentForGrid(response)
                let workflowDataKey = GetContactConsentsWorkflowDataKey($stateParams.AssessmentID);
                return printService.initReports($stateParams.AssessmentID, $stateParams.SectionID, $scope.responseId, workflowDataKey, $stateParams.ContactConsentID);
            }

            var textMsg;
            var signed = false;

            $scope.saveConsent = function (isNext, mandatory, hasErrors, keepForm, next) {
                var isDirty = formService.isDirty();
                if (isDirty && !hasErrors) {
                    if ($state.current.name == "consentexpire") {   //Only in case of expire consent
                        bootbox.confirm("Are you sure you want to Expire this Consent?", function (result) {
                            if (result === true) {
                                textMsg = 'expired';
                                saveConsentDetails(isNext, mandatory, hasErrors, keepForm, next);
                            }
                        });
                    }
                    else {
                        saveConsentDetails(isNext, mandatory, hasErrors, keepForm, next);
                    }
                }
            };

            var dataSave = function (isNext, mandatory, hasErrors, keepForm, next) {
                return $scope.saveAssessment(isNext, mandatory, hasErrors, keepForm, next).then(function (response) {
                    return saveConsentForGrid(response);
                });
            }

            var saveConsentDetails = function (isNext, mandatory, hasErrors, keepForm, next) {
                var dfd = $q.defer();
                if (isSigned() && $state.current.name != "consentexpire") {
                    bootbox.dialog({
                        message: "Are you sure you want to complete this consent?",
                        buttons: {
                            success: {
                                label: "Yes",
                                className: "btn-success",
                                callback: function () {
                                    dataSave(isNext, mandatory, hasErrors, keepForm, next).then(function (response) {
                                        return dfd.resolve(response);
                                    });
                                }
                            },
                            danger: {
                                label: "No",
                                className: "btn-danger",
                                callback:function(){
                                    return dfd.resolve();
                                }
                            }
                        }
                    });
                }
                else {
                    dataSave(isNext, mandatory, hasErrors, keepForm, next).then(function (response) {
                        return dfd.resolve(response);
                    });
                }
                return dfd.promise;
            };

            var saveConsentForGrid = function (response) {
                if (response != "-1") {
                    //$stateParams.ExpireAssessmentID
                    //Get all values from $scope.responses and populate in respective properties of object
                    signed = isSigned();
                    var consent = {
                        ContactConsentID: $stateParams.ContactConsentID ? $stateParams.ContactConsentID : 0,
                        ContactID: $stateParams.ContactID,
                        AssessmentID: $stateParams.AssessmentID, //only add
                        AssessmentSectionID: $stateParams.SectionID,                            //only add
                        ResponseID: $stateParams.ResponseID ? $stateParams.ResponseID : $scope.responseId,                                          //only add
                        DateSigned: signed ? new Date() : null,                                 //Specify condition and set date
                        EffectiveDate: $scope.responses.ModifiedOn ? $scope.responses.ModifiedOn : new Date(),                  //Update it with date of consent, currently assessment does not save Modified on
                        ExpirationDate: (textMsg == 'expired') ? ($scope.responses[1752][17] ? $filter('formatDate')($scope.responses[1752][17]) : null) : null,      //only update
                        ExpirationReasonID: (textMsg == 'expired') ? (($scope.responses[1753]) ? $scope.responses[1753][17] : null) : null,             //only update
                        ExpiredResponseID: (textMsg == 'expired') ? $scope.responseId : null,   //only update
                        ExpiredBy: (textMsg == 'expired') ? userID : null,                                        //Specify condition
                        SignatureStatusID: signed ? SIGNATURE_STATUS.Signed : SIGNATURE_STATUS.NotSigned,     //Specify condition
                        ModifiedOn: $scope.ModifiedOn ? $scope.ModifiedOn : new Date(),
                        ModifiedBy: userID
                    };

                    if (consent.ContactConsentID === 0) {
                        textMsg = 'saved';
                        return consentsService.add(consent).then(addUpdateSuccess, addUpdateFailure);

                    } else {
                        textMsg = (textMsg == "expired") ? 'expired' : 'updated';
                        return consentsService.update(consent).then(addUpdateSuccess, addUpdateFailure);
                    }
                    }
                else {
                    return $scope.promiseNoOp();
                }
            }

            var addUpdateSuccess = function (data) {
                if (data && data.ResultCode == 0) {
                    alertService.success('Consent ' + textMsg + ' successfully.');
                    formService.reset();
                    saveWorkflowHeader($stateParams.AssessmentID, $stateParams.ContactConsentID || data.ID);
                    if (textMsg == 'expired') {
                        $scope.Goto('patientprofile.consents.agency.agencyView', $stateParams);        //If expired, take user to grid screen
                    }
                    else {
                        angular.extend($stateParams, {
                            ResponseID: $stateParams.ResponseID || $scope.responseId,
                            ContactConsentID: $stateParams.ContactConsentID || data.ID,
                            ReadOnly: signed ? "view" : 'edit'
                        });
                        $state.transitionTo($state.current.name, $stateParams, { notify: false });
                    }
                }
                else {
                    alertService.error('OOPS! Something went wrong');
                }
            };

            var addUpdateFailure = function () {
                alertService.error('OOPS! Something went wrong');
            };

            $scope.noAccessToOtherUser = function () {
                $timeout(function () {
                    $scope.noAccessToOther = isSigned() || $stateParams.ReadOnly == 'view';
                    $scope.disableAssessmentSave = isSigned() || $stateParams.ReadOnly == 'view';
                    getPostProgramUnit();
                });
            }

            var isSigned = function () {
                var staffSig = false;
                angular.forEach($scope.responses, function (value, key) {
                    if (value) {
                        if (value['3701']) {
                            staffSig = true;
                        }
                    }
                });
                return staffSig;
            }
            var printData = function () {
                $scope.initAgencyReport().then(function (data) {
                    $scope.reportModel = data;
                    $scope.reportModel.HasLoaded = true;
                    $('#reportModal').modal('show');
                });
            }
            $scope.consentPrintReport = function (isNext, mandatory, hasErrors, keepForm, next) {
                var isDirty = formService.isDirty();
                if (isDirty) {
                    saveConsentDetails(isNext, mandatory, hasErrors, keepForm, next).then(function () {
                        printData();
                    });
                }
                else {
                    printData();
                }
            }

            var saveWorkflowHeader = function (assessmentID, headerID) {
                var workflowDatakey = GetContactConsentsWorkflowDataKey(assessmentID, headerID);

                //$rootScope.$broadcast('onWorkflowHeaderSave', { workflowDataKey: workflowDatakey, workflowHeaderID: headerID });
                WorkflowHeaderService.AddWorkflowHeader({ WorkflowDataKey: workflowDatakey, RecordHeaderID: headerID, ContactID: $stateParams.ContactID });
            }

        }]);
}())
