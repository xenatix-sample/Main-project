angular.module('xenatixApp')
    .controller('financialSummaryController', [
        '$scope', '$modal', '$timeout', 'alertService', 'financialAssessmentService', 'financialSummaryService', 'registrationService', 'additionalDemographyService', 'financialAssessmentService', 'contactBenefitService', 'collateralService', 'selfPayService', 'navigationService', 'lookupService', '$filter', '$q', '$stateParams', '$state', '$rootScope', 'formService', 'organizationID', 'contactRelationshipService', 'WorkflowHeaderService',
        function ($scope, $modal, $timeout, alertService, financialAssessmentService, financialSummaryService, registrationService, additionalDemographyService, financialAssessmentService, contactBenefitService, collateralService, selfPayService, navigationService, lookupService, $filter, $q, $stateParams, $state, $rootScope, formService, organizationID, contactRelationshipService, WorkflowHeaderService) {

            $scope.unemployedOption = Employment_Status.Unemployed;
            $scope.categoryTypeLookups = lookupService.getLookupsByType('CategoryType');
            $scope.assessmentStartDate = 'assessmentStartDate';
            $scope.assessmentEndDate = 'assessmentEndDate';
            var expirationDate = 'ExpirationDate';
            var effectiveDate = 'EffectiveDate';
            $scope.financialSummary = {};
            $scope.documentTypeID = DOCUMENT_TYPE.ConfirmationStatement;
            var credentialRequired = {
                'Benefits Specialist': 49,
                'Service Coordinator': 74,
                'Qualified Mental Health Professional': 37
            }

            var loggedInUserCredentialID = null;

            var SINGLE_SPACE = ' ';
            //signature details
            var signature = function (name) {
                return {
                    IsActive: true,
                    modelNumber: '',
                    name: name,
                    b64ImageData: '',
                    DeviceMessage: 'You do not have application permissions to electronically sign!'
                };
            }
            $scope.clientSignature = signature('clientSignature');
            $scope.staffSignature = signature('staffSignature');
            $scope.legalAuthRepresentativeSignature = signature('legalAuthRepresentativeSignature');
            $scope.initialsSignature = [];

            // Signature - Topaz Device
            $scope.OnTopazReady = function (value) {
                if (value === true) {
                    initSignaturePad();
                }
            };

            $scope.dateOptions = {
                formatYear: 'yy',
                startingDay: 1,
                showWeeks: false
            };

            $scope.init = function () {
                initInitialsSignature();

                $scope.isTopazReady = false;
                $scope.$watch('isTopazReady', $scope.OnTopazReady);

                if ($stateParams.FinancialSummaryID && $stateParams.FinancialSummaryID != 0) {
                    $scope.get();
                }
                else {
                    $scope.financialSummary.FinancialSummaryID = 0;
                    resetForm();
                    getData();
                }
            };

            var initInitialsSignature = function () {
                $scope.confirmationStatements = $filter('filter')(lookupService.getLookupsByType('ConfirmationStatement'), { OrganizationID: organizationID, DocumentTypeID: $scope.documentTypeID }, true)
                $scope.clientSignature.ModifiedOn = $scope.staffSignature.ModifiedOn = $scope.legalAuthRepresentativeSignature.ModifiedOn = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal');
                angular.forEach($scope.confirmationStatements, function (item) {
                    if (item.IsSignatureRequired) {
                        var sign = signature('initialsSignature' + item.ID);
                        sign.ConfirmationStatementID = item.ID;
                        sign.ContactID = $scope.contactID;
                        sign.DateSigned = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal');
                        sign.SignatureStatusID = SIGNATURE_STATUS.NotSigned;
                        if (!sign.EntitySignatureId)
                            sign.EntitySignatureId = 0;
                        if (!sign.FinancialSummaryConfirmationStatementID)
                            sign.FinancialSummaryConfirmationStatementID = 0;
                        if (!sign.EntitySignatureID)
                            sign.EntitySignatureID = 0;
                        $scope.initialsSignature[item.ID] = sign;
                    }
                });
            };

            var initSignaturePad = function () {
                $scope.clientSignature.Init();
                $scope.clientSignature.ImageCallback = $scope.individualSignImageCallback.bind({}, $scope.clientSignature);
                $scope.clientSignature.ClearCallback = $scope.individualSignImageCallback.bind({}, $scope.clientSignature);
                $scope.clientSignature.NoPointsCallback = $scope.noPointsCallback;

                $scope.staffSignature.Init();
                $scope.staffSignature.ImageCallback = $scope.individualSignImageCallback.bind({}, $scope.staffSignature);
                $scope.staffSignature.ClearCallback = $scope.individualSignImageCallback.bind({}, $scope.staffSignature);
                $scope.staffSignature.NoPointsCallback = $scope.noPointsCallback;

                $scope.legalAuthRepresentativeSignature.Init();
                $scope.legalAuthRepresentativeSignature.ImageCallback = $scope.individualSignImageCallback.bind({}, $scope.legalAuthRepresentativeSignature);
                $scope.legalAuthRepresentativeSignature.ClearCallback = $scope.individualSignImageCallback.bind({}, $scope.legalAuthRepresentativeSignature);
                $scope.legalAuthRepresentativeSignature.NoPointsCallback = $scope.noPointsCallback;

                angular.forEach($scope.initialsSignature, function (item) {
                    if (item) {
                        item.Init();
                        if (item.EntitySignatureID && item.EntitySignatureID != 0)
                            item.b64ImageData = item.SignatureBLOB;
                        item.ImageCallback = $scope.sigImageCallback.bind({}, item);
                        item.ClearCallback = $scope.sigImageCallback.bind({}, item);
                        item.NoPointsCallback = $scope.noPointsCallback;
                    }
                });
            };

            $scope.noPointsCallback = function () {
                alertService.error("Please sign before submitting the form");
            };

            $scope.individualSignImageCallback = function (item, str) {
                item.b64ImageData = item.SignatureBlob = str;
                item.SignatureStatusID = (str != null) ? SIGNATURE_STATUS.Signed : SIGNATURE_STATUS.NotSigned;
                if (!formService.isDirty())
                    $rootScope.setform(true, 'default');
            };

            $scope.sigImageCallback = function (item, str) {
                item.b64ImageData = item.SignatureBLOB = str;
                item.SignatureStatusID = SIGNATURE_STATUS.Signed;
                if (!formService.isDirty())
                    $rootScope.setform(true, 'default');
            };

            var resetForm = function () {
                $rootScope.formReset($scope.ctrl.financialSummaryForm);
            };

            var getPromises = [];
            $scope.get = function () {
                $scope.isLoading = true;
                $scope.contactID = $stateParams.ContactID;
                $scope.financialSummary.FinancialSummaryID = $stateParams.FinancialSummaryID;
                var deferred = $q.defer();

                return financialSummaryService.getFinancialSummaryById($scope.contactID, $scope.financialSummary.FinancialSummaryID).then(function (data) {
                    if (hasData(data) > 0) {
                        $scope.financialSummary = data.DataItems[0];

                        $scope.isExpired = ($scope.financialSummary.ExpirationDate && (new Date($scope.financialSummary.ExpirationDate) <= new Date())) ? true : false;
                        $scope.lastCompletedFADate = $scope.financialSummary.ModifiedOn ? $filter('formatDate')($scope.financialSummary.ModifiedOn, 'MM/DD/YYYY') : null;
                        // Set Initials Signature
                        angular.forEach($scope.financialSummary.FinancialSummaryConfirmationStatements, function (item) {
                            if (item) {
                                var initialsSign = $scope.initialsSignature[item.ConfirmationStatementID];
                                if (initialsSign) {
                                    initialsSign.FinancialSummaryConfirmationStatementID = item.FinancialSummaryConfirmationStatementID;
                                    if (!initialsSign.FinancialSummaryConfirmationStatementID)
                                        initialsSign.FinancialSummaryConfirmationStatementID = 0;
                                    if (!initialsSign.EntitySignatureID)
                                        initialsSign.EntitySignatureID = 0;
                                    initialsSign.FinancialSummaryID = item.FinancialSummaryID;
                                    initialsSign.ContactID = item.ContactID;
                                    initialsSign.ConfirmationStatementID = item.ConfirmationStatementID;
                                    initialsSign.DateSigned = item.DateSigned;
                                    initialsSign.SignatureStatusID = item.SignatureStatusID;
                                    initialsSign.EntitySignatureID = item.EntitySignatureID;
                                    initialsSign.SignatureBLOB = item.SignatureBLOB;
                                    initialsSign.b64ImageData = item.SignatureBLOB || "";
                                }
                            }

                        });

                        // Set Client Signature
                        if ($scope.financialSummary.ClientSignature) {
                            cloneSignature($scope.financialSummary.ClientSignature, $scope.clientSignature);
                            $scope.clientSignature.ModifiedOn = $filter('toMMDDYYYYDate')($scope.financialSummary.ClientSignature.ModifiedOn, 'MM/DD/YYYY');
                        }

                        if ($scope.financialSummary.StaffSignature) {
                            $scope.staffSignaturedUserID = $scope.financialSummary.StaffSignature.EntityId;
                            cloneSignature($scope.financialSummary.StaffSignature, $scope.staffSignature);
                            $scope.isStaffSigned = true;
                            $scope.staffSignature.ModifiedOn = $filter('toMMDDYYYYDate')($scope.financialSummary.StaffSignature.ModifiedOn, 'MM/DD/YYYY');
                        }

                        if ($scope.financialSummary.LegalAuthRepresentativeSignature) {
                            cloneSignature($scope.financialSummary.LegalAuthRepresentativeSignature, $scope.legalAuthRepresentativeSignature);
                            $scope.isReadOnlyLegalAuthRepresentative = true;
                            $scope.legalAuthRepresentativeSignature.ModifiedOn = $filter('toMMDDYYYYDate')($scope.financialSummary.LegalAuthRepresentativeSignature.ModifiedOn, 'MM/DD/YYYY');
                        }

                        getContactName();
                        getStaff();

                        angular.forEach($scope.financialSummary.Payors, function (payor) {
                            if (payor.EffectiveDate)
                                payor.EffectiveDate = $filter('formatDate')(payor.EffectiveDate, 'MM/DD/YYYY');
                            if (payor.ExpirationDate)
                                payor.ExpirationDate = $filter('formatDate')(payor.ExpirationDate, 'MM/DD/YYYY');
                        });
                        if ($scope.financialSummary.EffectiveDate)
                            $scope.financialSummary.EffectiveDate = $filter('formatDate')($scope.financialSummary.EffectiveDate, 'MM/DD/YYYY');
                        if ($scope.financialSummary.AssessmentEndDate)
                            $scope.financialSummary.AssessmentEndDate = $filter('formatDate')($scope.financialSummary.AssessmentEndDate, 'MM/DD/YYYY');

                        $q.all(getPromises).then(function (data) {
                            resetForm();
                            return deferred.resolve(data);
                        });
                        return deferred.promise;
                    } else {
                        // If no financial summary exists then pull data from house hold, payor, additional demography screen
                        getData();
                    };
                },
                function (errorStatus) {
                    alertService.error('Unable to get financial assessment: ' + errorStatus);
                }).finally(function () {
                    $scope.isLoading = false;
                });
            };

            var cloneSignature = function (fromSignature, toSignature) {
                toSignature.DocumentId = fromSignature.DocumentId;
                toSignature.DocumentTypeId = fromSignature.DocumentTypeId;
                toSignature.EntitySignatureId = fromSignature.EntitySignatureId;
                toSignature.EntityId = fromSignature.EntityId;
                toSignature.EntityName = fromSignature.EntityName;
                toSignature.EntityTypeId = fromSignature.EntityTypeId;
                toSignature.SignatureBlob = fromSignature.SignatureBlob;
                toSignature.CredentialID = fromSignature.CredentialID;
                toSignature.SignatureID = fromSignature.SignatureID;
                toSignature.b64ImageData = fromSignature.SignatureBlob || "";
            };

            var getData = function () {
                var promisesArray = [];

                $scope.financialSummary.ContactID = $scope.contactID = $stateParams.ContactID;
                $scope.financialSummary.DateSigned = $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal');
                $scope.financialSummary.SignatureStatusID = SIGNATURE_STATUS.NotSigned; // 1 - Not Signed, 2 - Signed

                $scope.financialSummary.OrganizationID = organizationID;

                //Add Demographic
                promisesArray.push(additionalDemographyService.getAdditionalDemographic($scope.contactID));

                //Add Financial assessment
                promisesArray.push(financialAssessmentService.getActiveFA($scope.contactID));

                //Add Contact benefit
                promisesArray.push(contactBenefitService.get($scope.contactID));

                //Add SelfPay
                promisesArray.push(selfPayService.getSelfPay($scope.contactID));

                //Add Contact Name
                var contactID = $scope.clientSignature.EntityId || $scope.contactID;
                promisesArray.push(registrationService.get(contactID));

                //Add Staff Name
                promisesArray.push(navigationService.get());

                //Final Response
                $q.all(promisesArray).then(function (data) {
                    //Additional Demographics
                    if (hasData(data[0]) && data[0].DataItems.length === 1) {
                        var additionalDemographicsData = data[0].DataItems[0];
                        $scope.financialSummary.EmploymentStatusID = additionalDemographicsData.EmploymentStatusID;
                        $scope.financialSummary.LookingForWork = additionalDemographicsData.LookingForWork;
                    }

                    //Financial Assessment Data
                    if (hasData(data[1]) && data[1].DataItems.length === 1) {
                        var financialAssessmentData = data[1].DataItems[0];
                        $scope.financialSummary.FinancialAssessment = {};
                        $scope.financialSummary.FinancialAssessment.FinancialAssessmentID = financialAssessmentData.FinancialAssessmentID;
                        $scope.financialSummary.FinancialAssessment.TotalIncome = financialAssessmentData.TotalIncome;
                        $scope.financialSummary.FinancialAssessment.TotalExpenses = financialAssessmentData.TotalExpenses;
                        $scope.financialSummary.FinancialAssessment.TotalExtraOrdinaryExpenses = financialAssessmentData.TotalExtraOrdinaryExpenses;
                        $scope.financialSummary.FinancialAssessment.AdjustedGrossIncome = financialAssessmentData.AdjustedGrossIncome;
                        $scope.financialSummary.FinancialAssessment.FamilySize = financialAssessmentData.FamilySize;
                        financialAssessmentService.getDetails($scope.contactID, financialAssessmentData.FinancialAssessmentID).then(function (financialAssessmentDetailsData) {
                            if (financialAssessmentDetailsData.DataItems.length > 0) {
                                $scope.financialSummary.FinancialAssessmentDetails = financialAssessmentDetailsData.DataItems;
                                resetForm();
                            }
                        });
                    }

                    //Contact Benefit Data
                    if (hasData(data[2])) {
                        $scope.financialSummary.Payors = $filter('filter')(data[2].DataItems, function (payor) {
                            return payor.ExpirationDate ? !isExpireDate(payor.ExpirationDate) : true;
                        }, true);
                        var familyGroupID = RELATIONSHIP_TYPE_GROUPID.Family;

                        angular.forEach($scope.financialSummary.Payors, function (payor) {
                            if (payor.EffectiveDate)
                                payor.EffectiveDate = $filter('formatDate')(payor.EffectiveDate, 'MM/DD/YYYY');
                            if (payor.ExpirationDate)
                                payor.ExpirationDate = $filter('formatDate')(payor.ExpirationDate, 'MM/DD/YYYY');

                            if (payor.PolicyHolderID == payor.ContactID) {
                                var relationshipLookups = financialAssessmentService.getLookups('RelationshipType');// self
                                payor.RelationshipTypeID = $filter('filter')(relationshipLookups, { Name: 'Self' }, true)[0].ID; //130; //Scott told me that the value got
                                payor.PolicyHolderName = new String('Self').concat(SINGLE_SPACE, '(', payor.PolicyHolderFirstName, SINGLE_SPACE, (payor.PolicyHolderMiddleName) ? payor.PolicyHolderMiddleName : '', SINGLE_SPACE, payor.PolicyHolderLastName, ')');
                            }
                            else {
                                collateralService.get($scope.contactID, 4, false).then(function (data) {
                                    var policyHolder = $filter('filter')(data.DataItems, function (item) {
                                        return item.ContactID == payor.PolicyHolderID;

                                    });
                                    contactRelationshipService.get(payor.PolicyHolderID, $stateParams.ContactID).then(function (relationshipData) {
                                        if (hasData(relationshipData)) {
                                            var familyGroupRelations = $filter('filter')(relationshipData.DataItems, function (obj) {
                                                return obj.RelationshipGroupID === familyGroupID;
                                            }, true);
                                            payor.RelationshipTypeID = familyGroupRelations[0].RelationshipTypeID;
                                            if (familyGroupRelations && familyGroupRelations.length > 0)
                                                payor.PolicyHolderName = lookupService.getText("RelationshipType", familyGroupRelations[0].RelationshipTypeID)
                                                                          .concat(SINGLE_SPACE, '(', payor.PolicyHolderFirstName, SINGLE_SPACE, (payor.PolicyHolderMiddleName) ? payor.PolicyHolderMiddleName : '', SINGLE_SPACE, payor.PolicyHolderLastName, ')');
                                            resetForm();
                                        }
                                    });
                                });
                            }
                        });
                    }

                    //SelfPay Data
                    if (hasData(data[3])) {
                        var selfPayData = filterFutureOrExpiredRecords(data[3].DataItems, expirationDate, effectiveDate);

                        $scope.financialSummary.SelfPay = {
                            MonthlyAbilities: [],
                            ISChildInConservatorship: false,
                            IsNotAttested: false,
                            IsEnrolledInPublicBenefits: false,
                            IsRequestingReconsideration: false,
                            IsNotGivingConsent: false,
                            IsOtherChildEnrolled: false,
                            IsApplyingForPublicBenefits: false,
                            IsReconsiderationOfAdjustment: false
                        };

                        angular.forEach(selfPayData, function (item) {
                            $scope.financialSummary.SelfPay.MonthlyAbilities.push({
                                SelfPayID: item.SelfPayID,
                                OrganizationDetailID: item.OrganizationDetailID,
                                ContactID: item.ContactID,
                                SelfPayAmount: item.SelfPayAmount,
                                IsPercent: item.IsPercent,
                                EffectiveDate: item.EffectiveDate,
                                ExpirationDate: item.ExpirationDate
                            });

                            $scope.financialSummary.SelfPay.ISChildInConservatorship = $scope.financialSummary.SelfPay.ISChildInConservatorship || item.ISChildInConservatorship;
                            $scope.financialSummary.SelfPay.IsNotAttested = $scope.financialSummary.SelfPay.IsNotAttested || item.IsNotAttested;
                            $scope.financialSummary.SelfPay.IsEnrolledInPublicBenefits = $scope.financialSummary.SelfPay.IsEnrolledInPublicBenefits || item.IsEnrolledInPublicBenefits;
                            $scope.financialSummary.SelfPay.IsRequestingReconsideration = $scope.financialSummary.SelfPay.IsRequestingReconsideration || item.IsRequestingReconsideration;
                            $scope.financialSummary.SelfPay.IsNotGivingConsent = $scope.financialSummary.SelfPay.IsNotGivingConsent || item.IsNotGivingConsent;
                            $scope.financialSummary.SelfPay.IsOtherChildEnrolled = $scope.financialSummary.SelfPay.IsOtherChildEnrolled || item.IsOtherChildEnrolled;
                            $scope.financialSummary.SelfPay.IsApplyingForPublicBenefits = $scope.financialSummary.SelfPay.IsApplyingForPublicBenefits || item.IsApplyingForPublicBenefits;
                            $scope.financialSummary.SelfPay.IsReconsiderationOfAdjustment = $scope.financialSummary.SelfPay.IsReconsiderationOfAdjustment || item.IsReconsiderationOfAdjustment;
                        });
                    }

                    //Get Contact Name
                    if (hasData(data[4])) {
                        var contactNameData = data[4].DataItems[0];
                        var suffix = lookupService.getText("Suffix", contactNameData.SuffixID);
                        $scope.clientSignature.ClientName = contactNameData.FirstName + (contactNameData.Middle ? ' ' + contactNameData.Middle : '') + ' ' + contactNameData.LastName + (suffix ? ' ' + suffix : '');
                    }

                    //Get Staff Name
                    if (hasData(data[5])) {
                        var credentialID = getStaffCredential(data[5].DataItems[0]);
                        if (credentialID) {
                            $scope.CredentialID = credentialID;
                            $scope.financialSummary.CredentialID = $scope.CredentialID;
                        };

                        var staffNameData = data[5].DataItems[0];
                        //Logged in user
                        $scope.UserID = staffNameData.UserID;
                        $scope.staffSignaturedUserID = staffNameData.UserID;
                        $scope.UserPhoneID = staffNameData.UserPhoneID;
                        $scope.PhoneNumber = staffNameData.ContactNumber;
                        $scope.Extension = staffNameData.Extension;

                        // Assessment User
                        if ($scope.financialSummary.FinancialSummaryID == undefined) {
                            $scope.financialSummary.UserID = $scope.UserID;
                            $scope.financialSummary.CredentialID = $scope.CredentialID;
                            $scope.financialSummary.UserPhoneID = $scope.UserPhoneID;
                        }
                        // staff xmlDetails who signs the FA
                        if (!$scope.financialSummary.FAStaffName) {
                            $scope.financialSummary.FAStaffName = lookupService.getText("Users", $scope.UserID);
                            $scope.financialSummary.FAStaffCredential = lookupService.getText("Credential", $scope.CredentialID);
                            $scope.financialSummary.FAStaffPhone = $scope.PhoneNumber;
                            $scope.financialSummary.FAStaffExtension = $scope.Extension;

                        }
                    }

                    //Reset the form
                    resetForm();
                });
            };

            var getContactName = function () {
                var contactID = $scope.clientSignature.EntityId || $scope.contactID;
                var registrationDeferred = $q.defer();
                getPromises.push(registrationDeferred.promise);
                registrationService.get(contactID).then(function (data) {
                    if (data.DataItems && data.DataItems.length > 0) {
                        var suffix = lookupService.getText("Suffix", data.DataItems[0].SuffixID);
                        $scope.clientSignature.ClientName = data.DataItems[0].FirstName + (data.DataItems[0].Middle ? ' ' + data.DataItems[0].Middle : '') + ' ' + data.DataItems[0].LastName + (suffix ? ' ' + suffix : '');
                    }
                    registrationDeferred.resolve(data);
                });
            }

            var getStaff = function () {
                var navigationDeferred = $q.defer();
                getPromises.push(navigationDeferred.promise);
                navigationService.get().then(function (data) {
                    if (data.DataItems != undefined && data.DataItems.length > 0) {
                        //Logged in user
                        $scope.UserID = data.DataItems[0].UserID;
                        $scope.staffSignaturedUserID = $scope.staffSignaturedUserID ? $scope.staffSignaturedUserID : $scope.UserID;
                        $scope.UserPhoneID = data.DataItems[0].UserPhoneID;
                        $scope.PhoneNumber = data.DataItems[0].ContactNumber;
                        $scope.Extension = data.DataItems[0].Extension;
                        // Assessment User
                        if (!$scope.financialSummary.CredentialID || ($scope.financialSummary.CredentialID != credentialRequired['Benefits Specialist'] &&
                                    $scope.financialSummary.CredentialID != credentialRequired['Service Coordinator'] &&
                                    $scope.financialSummary.CredentialID != credentialRequired['Qualified Mental Health Professional'])) {
                            $scope.financialSummary.CredentialID = null;
                            $scope.CredentialID = null;
                            var credentialID = getStaffCredential(data.DataItems[0]);
                            if (credentialID) {
                                $scope.CredentialID = credentialID;
                                $scope.financialSummary.CredentialID = $scope.CredentialID;
                                $scope.financialSummary.UserID = $scope.UserID;
                            };
                        }
                        else {
                            $scope.CredentialID = $scope.financialSummary.CredentialID;
                            getStaffCredential(data.DataItems[0]);
                        }

                        if ($scope.financialSummary.FinancialSummaryID == undefined) {
                            $scope.financialSummary.UserPhoneID = $scope.UserPhoneID;
                        }

                        // staff xmlDetails who signs the FA
                        if (!$scope.financialSummary.FAStaffName) {
                            $scope.financialSummary.FAStaffName = lookupService.getText("Users", $scope.financialSummary.UserID);
                            $scope.financialSummary.FAStaffCredential = lookupService.getText("Credential", $scope.CredentialID);
                            $scope.financialSummary.FAStaffPhone = $scope.PhoneNumber;
                            $scope.financialSummary.FAStaffExtension = $scope.Extension;
                        }
                    }
                    navigationDeferred.resolve(data);
                });
            }

            function getStaffCredential(user) {
                var staffCredentials = null;
                staffCredentials = user.UserCredentials;
                var vaildCredentials = $filter('filter')(staffCredentials, function (credential) {
                    return credential.CredentialID == credentialRequired['Benefits Specialist'] ||
                        credential.CredentialID == credentialRequired['Service Coordinator'] ||
                        credential.CredentialID == credentialRequired['Qualified Mental Health Professional'];
                }, true);

                if (vaildCredentials && vaildCredentials.length > 0) {
                    $scope.staffSignature.hideSignatureBtns = false;
                    loggedInUserCredentialID = vaildCredentials[0].CredentialID;
                    return vaildCredentials[0].CredentialID;
                }
                else {
                    $scope.staffSignature.hideSignatureBtns = true;
                    loggedInUserCredentialID = null;
                    return 0;
                }
            }

            $scope.saveFinancialAssessmentSummary = function (isUpdate) {
                if (isAssessmentSignedByClientAndUser()) {
                    return $scope.forcedExpiration().then(function (response) {
                        return addUpdateFinancialSummary(isUpdate);
                    });
                }
                else {
                    return addUpdateFinancialSummary(isUpdate);
                }
            };

            //Add/Update the Financial Summary Data
            var addUpdateFinancialSummary = function (isUpdate) {
                return isUpdate
                            ? financialSummaryService.updateFinancialSummary($scope.financialSummary)
                            : financialSummaryService.addFinancialSummary($scope.financialSummary);
            }

            $scope.save = function (isNext, mandatory, hasErrors, keepForm, next) {
                var isDirty = formService.isDirty();

                if ($scope.staffSignature.b64ImageData != null && $scope.staffSignature.b64ImageData) {
                    if (moment($scope.financialSummary.EffectiveDate).toDate() > new Date()) {
                        alertService.error('User is not allowed to sign the assessment for a future start date');
                        return false;
                    }
                }
                if ($scope.financialSummary.EffectiveDate) {
                    $scope.financialSummary.EffectiveDate = $filter("formatDate")($scope.financialSummary.EffectiveDate);
                } else {
                    $scope.financialSummary.EffectiveDate = $filter("formatDate")(new Date());
                }
                if ($scope.financialSummary.AssessmentEndDate) {
                    $scope.financialSummary.AssessmentEndDate = $filter("formatDate")($scope.financialSummary.AssessmentEndDate);
                }

                //Don't save xmlstaff details, if not signed
                if (isDirty && !$scope.staffSignature.b64ImageData) {
                    $scope.financialSummary.FAStaffName = null;
                    $scope.financialSummary.FAStaffCredential = null;
                    $scope.financialSummary.FAStaffPhone = null;
                    $scope.financialSummary.FAStaffExtension = null;
                }
                // Last Assessment User Details for Save
                if (isDirty && $scope.financialSummary.FinancialSummaryID != undefined) {
                    $scope.financialSummary.UserID = $scope.UserID;
                    $scope.financialSummary.UserPhoneID = $scope.UserPhoneID;
                    $scope.financialSummary.CredentialID = loggedInUserCredentialID;
                }

                $scope.financialSummary.ModifiedOn = $filter("formatDate")(moment().toDate());

                // Signatures
                $scope.financialSummary.FinancialSummaryConfirmationStatements = angular.copy($scope.initialsSignature);
                if (!$scope.clientSignature.EntitySignatureId)
                    $scope.clientSignature.EntitySignatureId = 0;
                $scope.financialSummary.ClientSignature = angular.copy($scope.clientSignature);
                if (!$scope.staffSignature.EntitySignatureId)
                    $scope.staffSignature.EntitySignatureId = 0;
                $scope.financialSummary.StaffSignature = angular.copy($scope.staffSignature);
                if ($scope.financialSummary.StaffSignature.SignatureBlob)
                    $scope.financialSummary.SignatureStatusID = SIGNATURE_STATUS.Signed;
                if (!$scope.legalAuthRepresentativeSignature.EntitySignatureId)
                    $scope.legalAuthRepresentativeSignature.EntitySignatureId = 0;
                $scope.financialSummary.LegalAuthRepresentativeSignature = angular.copy($scope.legalAuthRepresentativeSignature);

                if ((!mandatory && !hasErrors && !isNext) || (mandatory && !hasErrors && !isNext) || (!mandatory && !hasErrors && isNext) || (mandatory && !hasErrors && isNext)) {
                    var deferred = $q.defer();
                    if (isDirty) {
                        var isUpdate = $scope.financialSummary.FinancialSummaryID != undefined && $scope.financialSummary.FinancialSummaryID !== 0;
                        $scope.saveFinancialAssessmentSummary(isUpdate).then(function (response) {
                            var action = isUpdate ? 'updated' : 'added';
                            if (!isUpdate && response && response.ID) {
                                transitTo(response.ID, ($scope.financialSummary.SignatureStatusID == SIGNATURE_STATUS.Signed ? 'view' : 'edit'));
                            }
                            // if signature is completed the disable
                            if (isUpdate && $scope.financialSummary.SignatureStatusID == SIGNATURE_STATUS.Signed) {
                                transitTo($scope.financialSummary.FinancialSummaryID, 'view');
                            }
                            $scope.get().finally(function () {
                                $scope.postSave(response, action, isNext);
                                deferred.resolve(response);
                            });
                        },
                         function (errorStatus) {
                             alertService.error('OOPS! Something went wrong');
                             deferred.reject();
                         },
                         function (notification) {
                             alertService.warning(notification);
                         });
                    }
                    else
                        deferred.resolve(-1);
                    return deferred.promise;
                }
            };

            var transitTo = function (financialSummaryID, mode) {
                angular.extend($stateParams, { FinancialSummaryID: financialSummaryID, ReadOnly: mode });
                $state.transitionTo('patientprofile.benefits.financialAssesments.financialSummary',
                                    $stateParams,
                                    { notify: true, reload: true });
            }

            $scope.forcedExpiration = function () {
                return financialSummaryService.getFinancialSummaries($scope.contactID).then(function (data) {
                    if (hasData(data)) {
                        //Retrieve all the active Financial assessment data
                        var financialSummries = $filter('filter')(data.DataItems, function (item) {
                            return !item.ExpirationDate
                        }, true);
                        var taskArray = [];

                        //Update the EffectiveDate of the newly created Assessment
                        //[Bug:12681] do not change new F.A effective date.
                        //if (financialSummries.length > 0) {
                        //    $scope.financialSummary.EffectiveDate = $filter("formatDate")(new Date());
                        //}

                        //Update the Previous Financial Assessment active records
                        angular.forEach(financialSummries, function (item) {
                            //This needs to have the filter applied
                            item.ExpirationDate = moment($scope.financialSummary.EffectiveDate).subtract(1, "days").format("MM-DD-YYYY");
                            taskArray.push([financialSummaryService.updateFinancialSummary, [item]]);
                        });
                        return $q.serial(taskArray);
                    }
                });
            }


            $scope.postSave = function (response, action, isNext) {
                var data = response;
                if (data.ResultCode !== 0) {
                    alertService.error(data.ResultMessage);
                    return $scope.promiseNoOp();
                } else {
                    alertService.success('Financial Assessment has been ' + action + ' successfully.');
                    $scope.financialSummary.FinancialSummaryID =
                        (($scope.financialSummary !== undefined) && ($scope.financialSummary.FinancialSummaryID !== undefined) && ($scope.financialSummary.FinancialSummaryID != 0))
                        ? $scope.financialSummary.FinancialSummaryID
                        : response.ID;

                    //$rootScope.$broadcast('onWorkflowHeaderSave', { workflowDataKey: $state.current.data.workflowDataKey, workflowHeaderID: $scope.financialSummary.FinancialSummaryID });
                    WorkflowHeaderService.AddWorkflowHeader({ WorkflowDataKey: $state.current.data.workflowDataKey, RecordHeaderID: $scope.financialSummary.FinancialSummaryID, ContactID: $stateParams.ContactID });
                    resetForm();
                }
            };

            //Check if both the Client and aXis user has signed the document
            var isAssessmentSignedByClientAndUser = function () {
                return ($scope.financialSummary.StaffSignature
                            && checkModel($scope.financialSummary.StaffSignature.SignatureBlob)
                            && (($scope.financialSummary.ClientSignature && checkModel($scope.financialSummary.ClientSignature.SignatureBlob))
                                    || ($scope.financialSummary.LegalAuthRepresentativeSignature && checkModel($scope.financialSummary.LegalAuthRepresentativeSignature.SignatureBlob))));
            }

            $scope.validateStartDateLessThanEndDate = function (index) {
                if ($scope.ctrl.financialSummaryForm && $scope.ctrl.financialSummaryForm.assessmentStartDate && $scope.financialSummary.EffectiveDate && $scope.ctrl.financialSummaryForm && $scope.ctrl.financialSummaryForm.assessmentEndDate && $scope.financialSummary.AssessmentEndDate) {
                    var assessmentStartDateError = angular.element("#assessmentStartDateError" + index);
                    var assessmentStartDateErrortd = angular.element("#assessmentStartDateErrortd" + index);

                    var effectiveDate = new Date($scope.financialSummary.EffectiveDate);
                    var assessmentEndDate = new Date($scope.financialSummary.AssessmentEndDate);

                    effectiveDate.setHours(0, 0, 0, 0);
                    effectiveDate.setHours(0, 0, 0, 0);

                    if (effectiveDate > assessmentEndDate) {
                        assessmentStartDateError.removeClass('ng-hide');
                        assessmentStartDateErrortd.addClass('has-error');
                        if ($scope.ctrl.financialSummaryForm.assessmentEndDate) {
                            $scope.ctrl.financialSummaryForm.assessmentEndDate.$setValidity('date', false);
                        }
                    }
                    else {
                        assessmentStartDateError.addClass('ng-hide');
                        assessmentStartDateErrortd.removeClass('has-error');
                        if ($scope.ctrl.financialSummaryForm.assessmentEndDate) {
                            $scope.ctrl.financialSummaryForm.assessmentEndDate.$setValidity('date', true);
                        }
                    }
                }
            };

            $scope.initReport = function () {
                $scope.save(false, false).then(function () {
                    $rootScope.reportModel = {
                        HasLoaded: false,
                        ReportHeader: 'Financial Assessment',
                        ReportName: 'Financial Assessment'
                    };
                    var deferred = $q.defer();
                    var promises = [];
                    //promises.push(registrationService.get($scope.contactID));
                    promises.push(WorkflowHeaderService.GetWorkflowHeader($state.current.data.workflowDataKey, $stateParams.FinancialSummaryID));
                    $q.all(promises).then(function (data) {
                        var reportModel = data[1] || {};

                        if (data[0]) {
                            var suffix = lookupService.getText("Suffix", data[0].DataItems[0].SuffixID);
                            reportModel.clientName = data[0].DataItems[0].FirstName + (data[0].DataItems[0].Middle ? ' ' + data[0].DataItems[0].Middle : '') + ' ' + data[0].DataItems[0].LastName + (suffix ? ' ' + suffix : '');
                            if (data[0].DataItems[0].DOB)
                                reportModel.dob = ($filter('toMMDDYYYYDate')(data[0].DataItems[0].DOB, 'MM/DD/YYYY')).toString();
                            reportModel.mrn = data[0].DataItems[0].MRN;
                            reportModel.medicaidNumber = data[0].DataItems[0].MedicaidID || 'N/A';
                        }

                        if ($scope.financialSummary.EffectiveDate)
                            reportModel.assessmentStartDate = ($filter('toMMDDYYYYDate')(new Date($scope.financialSummary.EffectiveDate), 'MM/DD/YYYY', 'useLocal')).toString();
                        if ($scope.financialSummary.AssessmentEndDate)
                            reportModel.assessmentEndDate = ($filter('toMMDDYYYYDate')(new Date($scope.financialSummary.AssessmentEndDate), 'MM/DD/YYYY', 'useLocal')).toString();

                        //Last time when FA was saved date
                        reportModel.lastAssessmentDate = $scope.lastCompletedFADate ? $scope.lastCompletedFADate : ''

                        if ($scope.financialSummary.OrganizationID) {
                            var companyName = $filter('filter')(lookupService.getOrganizationByDataKey('Company'), { ID: $scope.financialSummary.OrganizationID });
                            reportModel.company = companyName && companyName.length > 0 ? companyName[0].Name : '';
                        }

                        if ($scope.financialSummary.EmploymentStatusID) {
                            var employmentStatus = $filter('filter')(lookupService.getLookupsByType('EmploymentStatus'), { ID: $scope.financialSummary.EmploymentStatusID });
                            reportModel.employmentStatus = employmentStatus && employmentStatus.length > 0 ? employmentStatus[0].Name : '';
                        }

                        if ($scope.financialSummary.EmploymentStatusID == Employment_Status.Unemployed) {
                            reportModel.checkLookingForWorkYes = $scope.financialSummary.LookingForWork ? true : false;
                            reportModel.checkLookingForWorkNo = !$scope.financialSummary.LookingForWork;
                        }

                        if ($scope.financialSummary.FinancialAssessment) {
                            reportModel.incomeTotal = $filter('currency')($scope.financialSummary.FinancialAssessment.TotalIncome);
                            reportModel.expenseTotal = $filter('currency')($scope.financialSummary.FinancialAssessment.TotalExpenses);
                            reportModel.extraordinaryExpensesTotal = $filter('currency')($scope.financialSummary.FinancialAssessment.TotalExtraOrdinaryExpenses);
                            reportModel.adjustedGrossIncome = $filter('currency')($scope.financialSummary.FinancialAssessment.AdjustedGrossIncome);
                            reportModel.familySize = $scope.financialSummary.FinancialAssessment.FamilySize ? $scope.financialSummary.FinancialAssessment.FamilySize.toString() : '';
                        }

                        reportModel.allIncome = [];
                        reportModel.allExpenses = [];
                        reportModel.allExtraordinaryExpenses = [];
                        reportModel.allOther = [];
                        angular.forEach($scope.financialSummary.FinancialAssessmentDetails, function (item) {
                            if ($scope.financialSummary.FinancialAssessmentDetails) {
                                var type = $filter('filter')(lookupService.getLookupsByType('CategoryType'), { ID: item.CategoryTypeID }, true);
                                var category = $filter('filter')(lookupService.getLookupsByType('Category'), { ID: item.CategoryID }, true);
                                var frequency = $filter('filter')(lookupService.getLookupsByType('FinanceFrequency'), { ID: item.FinanceFrequencyID }, true);

                                var financialAssessmentDetail = {
                                    Type: type && type.length > 0 ? type[0].Name : '',
                                    Category: category && category.length > 0 ? category[0].Name : '',
                                    Amount: $filter('currency')(item.Amount),
                                    Frequency: frequency && frequency.length > 0 ? frequency[0].Name : ''
                                };
                                switch (item.CategoryTypeID) {
                                    case 1:
                                        reportModel.allIncome.push(financialAssessmentDetail);
                                        break;
                                    case 2:
                                        reportModel.allExpenses.push(financialAssessmentDetail);
                                        break;
                                    case 3:
                                        reportModel.allExtraordinaryExpenses.push(financialAssessmentDetail);
                                        break;
                                    case 4:
                                        reportModel.allOther.push(financialAssessmentDetail);
                                        break;
                                }
                            }
                        });

                        if ($scope.financialSummary.SelfPay && $scope.financialSummary.SelfPay.MonthlyAbilities) {
                            angular.forEach($scope.financialSummary.SelfPay.MonthlyAbilities, function (item) {
                                var selfPayMonthlyAbility = item.IsPercent ? $filter('number')(item.SelfPayAmount ||'', 2) + '%' : '$' + $filter('number')(item.SelfPayAmount||'', 2);
                                switch (item.OrganizationDetailID) {
                                    case 3:
                                        reportModel.monthlyAbilityToPayECI = selfPayMonthlyAbility;
                                        break;
                                    case 4:
                                        reportModel.monthlyAbilityToPayIDD = selfPayMonthlyAbility;
                                        break;
                                    case 5:
                                        reportModel.monthlyAbilityToPayMH = selfPayMonthlyAbility;
                                        break;
                                    case 6:
                                        reportModel.monthlyAbilityToPayADS = selfPayMonthlyAbility;
                                        break;
                                    case 7:
                                        reportModel.monthlyAbilityToPaySF = selfPayMonthlyAbility;
                                        break;
                                }
                            });
                        }

                        reportModel.checkChildInConservatorship = $scope.financialSummary.SelfPay && $scope.financialSummary.SelfPay.ISChildInConservatorship ? $scope.financialSummary.SelfPay.ISChildInConservatorship : '';
                        reportModel.checkFamilyChoice = $scope.financialSummary.SelfPay && $scope.financialSummary.SelfPay.IsNotAttested ? $scope.financialSummary.SelfPay.IsNotAttested : '';
                        reportModel.checkEnrolledInPublicBenefits = $scope.financialSummary.SelfPay && $scope.financialSummary.SelfPay.IsEnrolledInPublicBenefits ? $scope.financialSummary.SelfPay.IsEnrolledInPublicBenefits : '';
                        reportModel.checkRequestingReconsideration = $scope.financialSummary.SelfPay && $scope.financialSummary.SelfPay.IsRequestingReconsideration ? $scope.financialSummary.SelfPay.IsRequestingReconsideration : '';
                        reportModel.checkNotGivingConsent = $scope.financialSummary.SelfPay && $scope.financialSummary.SelfPay.IsNotGivingConsent ? $scope.financialSummary.SelfPay.IsNotGivingConsent : '';
                        reportModel.checkOtherChildEnrolled = $scope.financialSummary.SelfPay && $scope.financialSummary.SelfPay.IsOtherChildEnrolled ? $scope.financialSummary.SelfPay.IsOtherChildEnrolled : '';
                        reportModel.checkApplyingForPublicBenefits = $scope.financialSummary.SelfPay && $scope.financialSummary.SelfPay.IsApplyingForPublicBenefits ? $scope.financialSummary.SelfPay.IsApplyingForPublicBenefits : '';
                        reportModel.checkReconsiderationOfAdjustment = $scope.financialSummary.SelfPay && $scope.financialSummary.SelfPay.IsReconsiderationOfAdjustment ? $scope.financialSummary.SelfPay.IsReconsiderationOfAdjustment : '';
                        reportModel.payorInformation = [];
                        angular.forEach($scope.financialSummary.Payors, function (item) {
                            var relationshipToPolicyHolder = '';
                            if (item.RelationshipTypeID)
                                relationshipToPolicyHolder = $filter('filter')(lookupService.getLookupsByType('RelationshipType'), { ID: item.RelationshipTypeID }, true);

                            reportModel.payorInformation.push({
                                policyHolder: item.PolicyHolderName ? item.PolicyHolderName : '',
                                relationshipToPolicyHolder: relationshipToPolicyHolder && relationshipToPolicyHolder.length > 0 ? relationshipToPolicyHolder[0].Name : '',
                                effectiveDate: item.EffectiveDate ? ($filter('toMMDDYYYYDate')(new Date(item.EffectiveDate), 'MM/DD/YYYY', 'useLocal')).toString() : '',
                                expirationDate: item.ExpirationDate ? ($filter('toMMDDYYYYDate')(new Date(item.ExpirationDate), 'MM/DD/YYYY', 'useLocal')).toString() : '',
                                nameOfPayor: item.PayorName ? item.PayorName : '',
                                groupName: item.GroupName ? item.GroupName : '',
                                groupID: item.GroupID ? item.GroupID : '',
                                planName: item.PlanName ? item.PlanName : '',
                                planID: item.PlanID ? item.PlanID : '',
                                policyID: item.PolicyID ? item.PolicyID : ''
                            });
                        });
                        if ($scope.staffSignaturedUserID) {
                            var staffName = $filter('filter')(lookupService.getLookupsByType('Users'), { ID: $scope.staffSignaturedUserID }, true);
                            reportModel.staffName = staffName && staffName.length > 0 ? staffName[0].Name : '';
                        }

                        if ($scope.CredentialID) {
                            var credentialName = $filter('filter')(lookupService.getLookupsByType('Credential'), { CredentialID: $scope.CredentialID }, true);
                            reportModel.staffCredential = credentialName && credentialName.length > 0 ? credentialName[0].CredentialName : '';
                        }

                        if ($scope.PhoneNumber)
                            reportModel.staffPhone = $scope.PhoneNumber;

                        if ($scope.financialSummary.UserID) {
                            var lastAssessmentCompletedBy = $filter('filter')(lookupService.getLookupsByType('Users'), { ID: $scope.financialSummary.UserID }, true);
                            reportModel.lastAssessmentCompletedBy = lastAssessmentCompletedBy && lastAssessmentCompletedBy.length > 0 ? lastAssessmentCompletedBy[0].Name : '';
                        }

                        if ($scope.financialSummary.FAStaffName) {
                            reportModel.FAStaffName = $scope.financialSummary.FAStaffName ? $scope.financialSummary.FAStaffName : '';
                            reportModel.FAStaffCredential = $scope.financialSummary.FAStaffCredential ? $scope.financialSummary.FAStaffCredential : '';
                            reportModel.FAStaffPhone = $scope.financialSummary.FAStaffPhone ? $scope.financialSummary.FAStaffPhone : '';
                            reportModel.FAStaffExtension = $scope.financialSummary.FAStaffExtension ? $scope.financialSummary.FAStaffExtension : '';
                        }

                        if ($scope.financialSummary.CredentialID) {
                            var lastAssessmentCredential = $filter('filter')(lookupService.getLookupsByType('Credential'), { CredentialID: $scope.financialSummary.CredentialID }, true);
                            reportModel.lastAssessmentCredential = lastAssessmentCredential && lastAssessmentCredential.length > 0 ? lastAssessmentCredential[0].CredentialName : '';
                        }

                        angular.forEach($scope.financialSummary.FinancialSummaryConfirmationStatements, function (item) {
                            if (item && item.ConfirmationStatementID) {
                                switch (item.ConfirmationStatementID) {
                                    case 1:
                                    case 2:
                                        break;
                                    case 3:
                                        reportModel.medicareStatusInitial = item.SignatureBLOB;
                                        break;
                                    case 4:
                                        reportModel.assignmentOfBenefitsInitial = item.SignatureBLOB;
                                        break;
                                    case 5:
                                        reportModel.balanceMyResponsibilityInitial = item.SignatureBLOB;
                                        break;
                                    case 6:
                                        reportModel.authorizeReleaseToMedicaidInitial = item.SignatureBLOB;
                                        break;
                                    case 7:
                                        reportModel.informationTrueAndCorrectInitial = item.SignatureBLOB;
                                        break;
                                    case 8:
                                        reportModel.statementFeesChangeInitial = item.SignatureBLOB;
                                        break;
                                    case 9:
                                        reportModel.receivedFeeScheduleInitial = item.SignatureBLOB;
                                        break;
                                    default:
                                }
                            }
                        });

                        if ($scope.clientSignature && $scope.clientSignature.SignatureBlob) {
                            reportModel.contactSigUri = $scope.clientSignature.SignatureBlob;
                            reportModel.contactSigDate = $filter('toMMDDYYYYDate')($scope.clientSignature.ModifiedOn, 'MM/DD/YYYY');
                        }

                        if ($scope.staffSignature && $scope.staffSignature.SignatureBlob) {
                            reportModel.staffSigUri = $scope.staffSignature.SignatureBlob;//dataURI
                            reportModel.staffSigDate = $filter('toMMDDYYYYDate')($scope.staffSignature.ModifiedOn, 'MM/DD/YYYY');
                        }

                        if ($scope.legalAuthRepresentativeSignature && $scope.legalAuthRepresentativeSignature.SignatureBlob) {
                            reportModel.repSigUri = $scope.legalAuthRepresentativeSignature.SignatureBlob;
                            reportModel.repSigDate = $filter('toMMDDYYYYDate')($scope.legalAuthRepresentativeSignature.ModifiedOn, 'MM/DD/YYYY');
                            reportModel.repName = $scope.legalAuthRepresentativeSignature.EntityName;
                        }
                        //reportModel.repName = ;//Name of Legally Authorized Representative
                        //reportModel.repSigUri = ; //dataURI for authorized rep signature
                        //reportModel.repSigDate = ;

                        //reportModel.medicaidNumber = 'N/A';
                        contactBenefitService.get($scope.contactID).then(function (data) {
                            if (data && data.DataItems && data.DataItems.length > 0) {
                                var payors = $filter('filter')(data.DataItems, function (itm) {
                                    return itm.PayorName.toString().toLowerCase().indexOf('medicaid') > -1;
                                })
                                if (payors && payors.length > 0) {
                                    //reportModel.medicaidNumber = payors[0].PolicyID;
                                }
                            }
                            $rootScope.reportModel = reportModel;
                            $rootScope.reportModel.ReportHeader = 'Financial Assessment';
                            $rootScope.reportModel.ReportName = 'Financial Assessment';
                            $rootScope.reportModel.HasLoaded = true;
                            $('#financialSummaryReportModal').modal('show');
                            return deferred.resolve(reportModel);

                        });
                    });
                    return deferred.promise;
                });
            }

            $scope.$on('$destroy', function () {
                $rootScope.reportModel = null;
            });

            $scope.init();
        }]);
