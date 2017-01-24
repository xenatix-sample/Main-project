(function () {
    angular.module('xenatixApp')
        .controller('financialAssessmentsController', ['$scope', '$q', '$stateParams', '$timeout', '$filter', 'financialSummaryService', 'registrationService', 'contactBenefitService', 'lookupService', 'navigationService', 'WorkflowHeaderService', '$state',
    function ($scope, $q, $stateParams, $timeout, $filter, financialSummaryService, registrationService, contactBenefitService, lookupService, navigationService, WorkflowHeaderService, $state) {

        var financialAssessmentsTable = $("#financialAssessmentsTable");
        $scope.$parent['autoFocus'] = true;
        var signatureStatusType = lookupService.getLookupsByType('SignatureStatus');

        var initializeBootstrapTable = function () {
            $scope.tableoptions = {
                pagination: true,
                pageSize: 10,
                pageList: [10, 25, 50, 100],
                search: false,
                showColumns: true,
                data: [],
                undefinedText: '',
                columns: [
                    // If a field is date only field in the database, do not use use local
                    {
                        field: "DateSigned", //Date Only Field
                        title: "Date",
                        formatter: function (value, row, index) {
                            return value ? $filter('formatDate')(value, 'MM/DD/YYYY') : '';
                        }
                    },
                    {
                        field: "SignatureStatusID",
                        title: "Status",
                        formatter: function (value, row, index) {
                            return getText(value, signatureStatusType);
                        }
                    },
                    {
                        field: "EffectiveDate", //Date Only Field
                        title: "Effective Date",
                        formatter: function (value, row, index) {
                            return value ? $filter('formatDate')(value, 'MM/DD/YYYY') : '';
                        }
                    },
                    {
                        field: "ExpirationDate", //Date Only Field
                        title: "Expiration Date",
                        formatter: function (value, row, index) {
                            return value ? $filter('formatDate')(value, 'MM/DD/YYYY') : '';
                        }
                    },
                    {
                        field: "FinancialSummaryID",
                        title: "Actions",
                        formatter: function (value, row, index) {
                            var condition = (row["SignatureStatusID"] == SIGNATURE_STATUS.Signed || (row["ExpirationDate"] && new Date(row["ExpirationDate"]) <= new Date()));
                            return (condition
                                ? '' : '<a href="javascript:void(0)" data-default-action id="editFA" security permission-key="Benefits-FinancialAssessment-FinancialAssessment" permission="update" name="editFA" data-toggle="modal" title="Edit" ui-sref="patientprofile.benefits.financialAssesments.financialSummary({  ReadOnly: \'edit\',FinancialSummaryID: ' + value + ',ContactID: ' + row.ContactID + ' })" alt="Edit" space-key-press><i class="fa fa-pencil fa-fw" /></a>') +
                                    '<a href="javascript:void(0)" ' + (condition ? 'data-default-action' : 'data-default-no-action') + '  security permission-key="Benefits-FinancialAssessment-FinancialAssessment" permission="read" id="viewFA" name="viewFA" title="View" ui-sref="patientprofile.benefits.financialAssesments.financialSummary({  ReadOnly: \'view\',FinancialSummaryID: ' + value + ',ContactID: ' + row.ContactID + ' })" alt="View" space-key-press><i class="fa fa-eye fa-fw"></i></a>' +
                                    '<a href="javascript:void(0)" data-default-no-action  security permission-key="Benefits-FinancialAssessment-FinancialAssessment" permission="read" id="printFA" name="printFA" title="Print" ng-click="initReport(' + value + ')" space-key-press><i class="fa fa-print fa-fw"></i></a>'
                        }
                    }
                ]
            };
        };

        var init = function () {
            $scope.contactID = $stateParams.ContactID;
            initializeBootstrapTable();
            get();
            getStaff();
        };

        var getStaff = function () {
            navigationService.get().then(function (data) {
                if (hasData(data)) {
                    //Logged in user
                    $scope.UserID = data.DataItems[0].UserID;
                    $scope.UserPhoneID = data.DataItems[0].UserPhoneID;
                    $scope.PhoneNumber = data.DataItems[0].ContactNumber;
                    $scope.CredentialID = getStaffCredential(data.DataItems[0]);
                    $scope.Extension = data.DataItems[0].Extension;
                }
            });
        }

        var get = function () {
            financialSummaryService.getFinancialSummaries($scope.contactID).then(function (data) {
                if (hasData(data)) {
                    var sortedFinancialAssessmentsList = $filter('orderBy')(data.DataItems, function (item) {
                        return new Date(item.EffectiveDate);
                    }, true);
                    var financialAssessmentsList = data.DataItems;
                    if (financialAssessmentsList) {
                        financialAssessmentsTable.bootstrapTable('load', sortedFinancialAssessmentsList);
                    }
                    else {
                        financialAssessmentsTable.bootstrapTable('removeAll');
                    }
                }
            });
        }

        $scope.initReport = function (financialSummaryID) {
            financialSummaryService.getFinancialSummaryById($scope.contactID, financialSummaryID).then(function (data) {
                $scope.reportModel = {
                    HasLoaded: false,
                    ReportHeader: 'Financial Assessment',
                    ReportName: 'Financial Assessment'
                };
                $scope.financialSummary = data.DataItems[0];
                if ($scope.financialSummary.StaffSignature) {
                    $scope.staffSignaturedUserID = $scope.financialSummary.StaffSignature.EntityId;
                }
                else {
                    $scope.staffSignaturedUserID = $scope.UserID;
                }
                // staff xmlDetails who signs the FA
                if (!$scope.financialSummary.FAStaffName) {
                    $scope.financialSummary.FAStaffName = lookupService.getText("Users", $scope.UserID);
                    $scope.financialSummary.FAStaffCredential = lookupService.getText("Credential", $scope.CredentialID);
                    $scope.financialSummary.FAStaffPhone = $scope.PhoneNumber;
                    $scope.financialSummary.FAStaffExtension = $scope.Extension;
                }
                var deferred = $q.defer();
                var promises = [];
                //promises.push(registrationService.get($scope.contactID));
                promises.push(WorkflowHeaderService.GetWorkflowHeader($state.current.data.workflowDataKey, financialSummaryID));
                $q.all(promises).then(function (data) {
                    var reportModel = data[1] || {};

                    if (data[0]) {
                        var suffix = lookupService.getText("Suffix", data[0].DataItems[0].SuffixID);
                        reportModel.clientName = data[0].DataItems[0].FirstName + (data[0].DataItems[0].Middle ? ' ' + data[0].DataItems[0].Middle : '') + ' ' + data[0].DataItems[0].LastName + (suffix ? ' ' + suffix : '');
                        if (data[0].DataItems[0].DOB)
                            reportModel.dob = ($filter('formatDate')(data[0].DataItems[0].DOB, 'MM/DD/YYYY')).toString();
                        reportModel.mrn = data[0].DataItems[0].MRN;
                        reportModel.medicaidNumber = data[0].DataItems[0].MedicaidID || 'N/A';
                    }
                    //Date Only Field
                    if ($scope.financialSummary.EffectiveDate)
                        reportModel.assessmentStartDate = ($filter('toMMDDYYYYDate')(new Date($scope.financialSummary.EffectiveDate), 'MM/DD/YYYY')).toString();
                    //Last time when FA was saved date
                    reportModel.lastAssessmentDate = $scope.financialSummary.ModifiedOn ? $filter('formatDate')($scope.financialSummary.ModifiedOn, 'MM/DD/YYYY') : '';
                    //Date Only Field
                    if ($scope.financialSummary.AssessmentEndDate)
                        reportModel.assessmentEndDate = ($filter('formatDate')(new Date($scope.financialSummary.AssessmentEndDate), 'MM/DD/YYYY')).toString();

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
                            var category = $filter('filter')(lookupService.getLookupsByType('Category'), { ID: item.CategoryID } , true);
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
                            var selfPayMonthlyAbility = item.IsPercent ? $filter('number')(item.SelfPayAmount || '', 2) + '%' : '$' + $filter('number')(item.SelfPayAmount || '', 2);
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
                            effectiveDate: item.EffectiveDate ? ($filter('formatDate')(new Date(item.EffectiveDate), 'MM/DD/YYYY')).toString() : '', //Date Only Field
                            expirationDate: item.ExpirationDate ? ($filter('formatDate')(new Date(item.ExpirationDate), 'MM/DD/YYYY')).toString() : '', //Date Only Field
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

                    if ($scope.financialSummary.ClientSignature && $scope.financialSummary.ClientSignature.SignatureBlob) {
                        reportModel.contactSigUri = $scope.financialSummary.ClientSignature.SignatureBlob;
                        reportModel.contactSigDate = $filter('toMMDDYYYYDate')($scope.financialSummary.ClientSignature.ModifiedOn, 'MM/DD/YYYY');
                    }

                    if ($scope.financialSummary.StaffSignature && $scope.financialSummary.StaffSignature.SignatureBlob) {
                        reportModel.staffSigUri = $scope.financialSummary.StaffSignature.SignatureBlob;//dataURI
                        reportModel.staffSigDate = $filter('toMMDDYYYYDate')($scope.financialSummary.StaffSignature.ModifiedOn, 'MM/DD/YYYY');
                    }

                    if ($scope.financialSummary.LegalAuthRepresentativeSignature && $scope.financialSummary.LegalAuthRepresentativeSignature.SignatureBlob) {
                        reportModel.repSigUri = $scope.financialSummary.LegalAuthRepresentativeSignature.SignatureBlob;//dataURI
                        reportModel.repSigDate = $filter('toMMDDYYYYDate')($scope.financialSummary.LegalAuthRepresentativeSignature.ModifiedOn, 'MM/DD/YYYY');
                        reportModel.repName = $scope.financialSummary.LegalAuthRepresentativeSignature.EntityName;
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
                               // reportModel.medicaidNumber = payors[0].PolicyID;
                            }
                        }
                        $scope.reportModel = reportModel;
                        $scope.reportModel.ReportHeader = 'Financial Assessment';
                        $scope.reportModel.ReportName = 'Financial Assessment';
                        $scope.reportModel.HasLoaded = true;
                        $('#financialSummaryReportModal').modal('show');
                        return deferred.resolve(reportModel);
                    });
                });
                return deferred.promise;
            });
        }

        var credentialRequired = {
            'Benefits Specialist': 49,
            'Service Coordinator': 74,
            'Qualified Mental Health Professional': 37
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
                return vaildCredentials[0].CredentialID;
            }
            else {
                return 0;
            }
        }


        init();
    }]);
}());