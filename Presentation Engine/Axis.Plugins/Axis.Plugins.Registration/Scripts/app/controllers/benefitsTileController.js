(function () {
    angular.module('xenatixApp')
        .controller('benefitsTileController', [
            '$scope', '$rootScope', '$q', '$injector', '$filter', 'alertService', '$stateParams', 'financialAssessmentService', 'roleSecurityService', 'contactBenefitService', 'registrationService', 'selfPayService', 'financialSummaryService', 'benefitsAssistanceProgressNoteService', 'httpLoaderInterceptor',
            function ($scope, $rootScope, $q, $injector, $filter, alertService, $stateParams, financialAssessmentService, roleSecurityService, contactBenefitService, registrationService, selfPayService, financialSummaryService, benefitsAssistanceProgressNoteService, httpLoaderInterceptor) {
                httpLoaderInterceptor.ignore(true);

                $scope.listLimit = 5;

                //Page Variables

                var documentStatus = DOCUMENT_STATUS.Draft;
                var getFinancialAssessmentTile = function (contactID) {
                    var results = [];
                    var formattedDate;
                    var permissionKey = BenifitsPermissionKey.Benefits_FinancialAssessment_FinancialAssessment;
                    if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                        var financialSummaryTileInfo = initTiles("Financial Assessment", "financialAssesments", contactID, true, null, permissionKey, PERMISSION.CREATE);

                        financialSummaryService.getFinancialSummaries(contactID).then(function (financialSummaryResponse) {
                            if (financialSummaryResponse
                                   && financialSummaryResponse.DataItems
                                   && financialSummaryResponse.DataItems.length > 0) {
                                var filteredFSData = $filter('filter')(financialSummaryResponse.DataItems, function (item) {
                                    return !item.ExpirationDate;
                                }, true);

                                var activeFS = $filter('filter')(filteredFSData, { SignatureStatusID: SIGNATURE_STATUS.Signed }, true)
                                if (activeFS && activeFS.length == 0) {
                                    activeFS = $filter('orderBy')(filteredFSData, function (item) {
                                        return item.EffectiveDate;
                                    }, true);
                                }

                                if (activeFS && activeFS.length > 0) {
                                    //Get the Date Signed
                                    var model = activeFS[0];
                                    if (checkModel(model.DateSigned)) {
                                        if (model.SignatureStatusID == SIGNATURE_STATUS.NotSigned)
                                            formattedDate = 'Not Signed';
                                        else
                                            formattedDate = $filter('toMMDDYYYYDate')(model.DateSigned, 'MM/DD/YYYY');
                                        results.push(getTileDetailsModel(null, formattedDate, "Date Signed"));
                                    }
                                    else {
                                        results.push(getTileDetailsModel(null, "No signed Financial Assessment on file.", "Date Signed"));
                                    }

                                    //Get the Current Effective Date
                                    if (checkModel(model.EffectiveDate)) {
                                        formattedDate = $filter('toMMDDYYYYDate')(model.EffectiveDate, 'MM/DD/YYYY');
                                        results.push(getTileDetailsModel(null, formattedDate, "Start Date"));
                                    }

                                    //Get the Expiration Date
                                    if (checkModel(model.ExpirationDate)) {
                                        formattedDate = $filter('toMMDDYYYYDate')(model.ExpirationDate, 'MM/DD/YYYY');
                                        results.push(getTileDetailsModel(null, formattedDate, "End Date"));
                                    }
                                    var readOnly = (model.SignatureStatusID == SIGNATURE_STATUS.Signed || (model.ExpirationDate && (new Date(model.ExpirationDate) <= new Date()))) ? 'view' : 'edit';

                                    //Edit the Financial Assessments details
                                    financialSummaryTileInfo.EditDetails.push({
                                        state: ".financialAssesments.financialSummary",
                                        contactID: contactID,
                                        params: { ReadOnly: readOnly, FinancialSummaryID: model.FinancialSummaryID, ContactID: contactID }
                                    });
                                }
                            }
                            else {
                                results.push({
                                    CustomMessage: "No signed Financial Assessment on file."
                                });
                            }

                            financialSummaryTileInfo.TileDetails = results;
                            financialSummaryTileInfo.IsLoaded = true;
                        },
                        function (errorStatus) {
                            financialSummaryTileInfo.IsLoaded = true;
                            financialSummaryTileInfo.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                        });
                    }
                    //Return the Financial Summary Model
                    return financialSummaryTileInfo;
                };

                var getHouseholdIncomeTile = function (contactID) {
                    var permissionKey = BenifitsPermissionKey.Benefits_HouseholdIncome_HouseholdIncome;
                    if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                        var tileInfoModel = initTiles("Household Income", "financial", contactID, true, null, permissionKey, PERMISSION.CREATE);
                        financialAssessmentService.get(contactID, 0).then(function (financialAssessmentData) {
                            var result = [];
                            if (hasData(financialAssessmentData)) {
                                var sortedData = ($filter('orderBy')(financialAssessmentData.DataItems, ['ExpirationDate','ModifiedOn'], true));
                                var model = sortedData[0];
                                if (checkModel(model.AdjustedGrossIncome)) {
                                    result.push(getTileDetailsModel(null, $filter('currency')(model.AdjustedGrossIncome), "Adjusted Gross Income"));
                                }

                                if (checkModel(model.TotalIncome)) {
                                    result.push(getTileDetailsModel(null, $filter('currency')(model.TotalIncome), "Total Income"));
                                }

                                if (checkModel(model.TotalExpenses) || checkModel(model.TotalExtraOrdinaryExpenses)) {
                                    var totalExpenses = (model.TotalExpenses || 0) + (model.TotalExtraOrdinaryExpenses || 0);
                                    result.push(getTileDetailsModel(null, $filter('currency')(totalExpenses), "Total Of All Expenses"));
                                }

                                if (checkModel(model.TotalOther)) {
                                    result.push(getTileDetailsModel(null, $filter('currency')(model.TotalOther), "Total Other"));
                                }


                                if (checkModel(model.AssessmentDate)) {
                                    if (checkModel(model.ExpirationDate) && isHouseholdExpired(model.ExpirationDate)) {
                                        model.AssessmentDate = $filter('toMMDDYYYYDate')(model.AssessmentDate) + " (Expired)-" + $filter('toMMDDYYYYDate')(model.ExpirationDate);
                                    }
                                    else {
                                        model.AssessmentDate = $filter('toMMDDYYYYDate')(model.AssessmentDate);
                                    }
                                    result.push(getTileDetailsModel(null, model.AssessmentDate, "Assessment Date"));
                                }
                            }
                            tileInfoModel.TileDetails = result;
                            tileInfoModel.IsLoaded = true;
                            if (model) {
                                tileInfoModel.ShowShortcuts = true;
                                isView = (isHouseholdExpired(model.ExpirationDate) || model.IsViewFinanicalAssessment);
                                tileInfoModel.EditDetails.push({
                                    state: 'patientprofile.benefits.financial.financialdetails',
                                    contactID: $stateParams.ContactID,
                                    id: model.FinancialAssessmentID,
                                    params: { FinancialAssessmentID: model.FinancialAssessmentID, ReadOnly: isView ? 'view' : 'edit' }
                                });
                            }

                        },
                            function (errorStatus) {
                                tileInfoModel.IsLoaded = true;
                                tileInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                            });
                    }
                    return tileInfoModel;
                };

                var getSelfPaySectionTile = function (contactID) {
                    var permissionKey = BenifitsPermissionKey.Benefits_SelfPay_SelfPay;
                    if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                        var selfPayInfoModel = initTiles("Self Pay", "selfPay", contactID, true, null, permissionKey, PERMISSION.CREATE);
                        selfPayService.getSelfPay(contactID).then(function (selfPayData) {
                            var result = [];
                            if ((selfPayData.DataItems != null) && selfPayData.DataItems.length > 0) {
                                var model = selfPayData.DataItems[0];
                                if (checkModel(model.OrganizationDetailID)) {
                                    result.push(getTileDetailsModel(null, $filter('filter')($filter('securityFilter')($rootScope.getOrganizationByDataKey('Division'), 'Division', 'ID', permissionKey), { ID: model.OrganizationDetailID }, true)[0].Name, "Division"));
                                }

                                if (checkModel(model.EffectiveDate)) {
                                    result.push(getTileDetailsModel(null, $filter('toMMDDYYYYDate')(model.EffectiveDate, 'MM/DD/YYYY'), "Effective Date"));
                                }

                                if (checkModel(model.ExpirationDate)) {
                                    result.push(getTileDetailsModel(null, $filter('toMMDDYYYYDate')(model.ExpirationDate, 'MM/DD/YYYY'), "Expiration Date"));
                                }
                                if (checkModel(model.SelfPayAmount)) {
                                    var selfPayAmount = model.IsPercent ? $filter('number')(model.SelfPayAmount, 2) + "%" : "$" + $filter('number')(model.SelfPayAmount, 2);
                                    result.push(getTileDetailsModel(null, selfPayAmount, "Amount"));
                                }
                                angular.forEach(selfPayData.DataItems, function (item) {
                                    selfPayInfoModel.EditDetails.push({
                                        modifiedDate: $filter('toMMDDYYYYDate')(item.EffectiveDate, 'MM/DD/YYYY', 'useLocal'),
                                        item: $filter('filter')($filter('securityFilter')($rootScope.getOrganizationByDataKey('Division'), 'Division', 'ID', permissionKey), { ID: item.OrganizationDetailID }, true)[0].Name,
                                        state: "patientprofile.benefits.selfPay",
                                        contactID: contactID,
                                        id: item.SelfPayID,
                                        params: { ContactID: contactID, SelfPayID: item.SelfPayID }
                                    });
                                });
                            }
                            selfPayInfoModel.TileDetails = result;
                            selfPayInfoModel.IsLoaded = true;
                        },
                            function (errorStatus) {
                                selfPayInfoModel.IsLoaded = true;
                                selfPayInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                            });
                    }
                    return selfPayInfoModel;
                }

                var getPayorsTile = function (contactID, isEciContact) {
                    var permissionKey = BenifitsPermissionKey.Benefits_Payors_Payors;
                    var benefitStateName = "benefits";
                    if (isEciContact)
                        benefitStateName = "ecibenefits"
                    if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                        var tileInfoModel = initTiles("Payors", benefitStateName, contactID, true, null, permissionKey, PERMISSION.CREATE);
                        contactBenefitService.get(contactID).then(function (contactBenefitData) {
                            var result = [];
                            if (hasData(contactBenefitData)) {
                                var activePayors = $filter('filter')(contactBenefitData.DataItems, function (item) {
                                    return !item.ExpirationDate || $filter('formatDate')(item.ExpirationDate, 'MM/DD/YYYY') >= $filter('formatDate')(new Date(), 'MM/DD/YYYY')
                                });
                                if (activePayors && activePayors.length > 0) {
                                     var sortedData= ($filter('orderBy')(activePayors, 'ModifiedOn', true));
                                     model = sortedData[0];
                                    if (checkModel(model.PayorName)) {
                                        result.push(getTileDetailsModel(null, model.PayorName, "Payor Name"));
                                    }
                                    if (checkModel(model.PlanName)) {
                                        result.push(getTileDetailsModel(null, model.PlanName, "Plan Name"));
                                    }

                                    if (checkModel(model.GroupName)) {
                                        result.push(getTileDetailsModel(null, model.GroupName, "Group Name"));
                                    }
                                    angular.forEach(sortedData, function (item) {
                                        tileInfoModel.EditDetails.push({
                                            modifiedDate: $filter('toMMDDYYYYDate')(item.ModifiedOn, 'MM/DD/YYYY', 'useLocal'),
                                            item: item.PayorName,
                                            state: "patientprofile.benefits." + benefitStateName,
                                            contactID: contactID,
                                            id: item.ContactPayorID,
                                            params: { ContactPayorID: item.ContactPayorID }
                                        });
                                    });
                                    tileInfoModel.TileDetails = result;
                                }
                                else if (activePayors.length == 0 && contactBenefitData.DataItems.length > 0) {
                                    tileInfoModel.TileDetails.push({ CustomMessage: "No Active Payors" });
                                }

                            }
                            tileInfoModel.IsLoaded = true;
                        },
                            function (errorStatus) {
                                tileInfoModel.IsLoaded = true;
                                tileInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                            });
                    }
                    return tileInfoModel;
                };

                var getBenefitsAssistanceProgressNote = function (contactID) {
                    var permissionKey = BenifitsPermissionKey.Benefits_BAPN_BAPN;
                    if (roleSecurityService.hasPermission(permissionKey, PERMISSION.READ)) {
                        var params = {
                            ResponseID: 0
                        };
                        var tileInfoModel = initTiles(
                            "Benefits Assistance Progress Note",
                            ".bapn.benefitsAssistanceProgressNote",
                            contactID,
                            true,
                            null,
                            permissionKey,
                            PERMISSION.CREATE,
                            params,
                            true,
                            "bapn.benefitsAssistanceProgressNote"
                        );
                        benefitsAssistanceProgressNoteService.getByContactID(contactID).then(function (data) {
                            var result = [];
                            if (hasData(data)) {
                                tileData = $filter('orderBy')(data.DataItems, ['DocumentStatusID', '-ServiceStartDate'], false);
                                tileData = tileData.slice(0, 5);
                                angular.forEach(tileData, function (item, key) {
                                    result.push(getTileDetailsModel(null, (item.ServiceStartDate ? $filter('formatDate')(item.ServiceStartDate, 'MM/DD/YYYY') : '') + " (" + (item.DocumentStatusID == DOCUMENT_STATUS.Draft ? "Draft" : (item.DocumentStatusID == DOCUMENT_STATUS.Completed) ? "Completed" : "Void") + ")", (key == 0) ? "Recent" : ""));
                                });

                                angular.forEach(tileData, function (item) {
                                    tileInfoModel.EditDetails.push({
                                        modifiedDate: item.DateEntered ? $filter('formatDate')(item.DateEntered, 'MM/DD/YYYY') : '',
                                        item: (item.DocumentStatusID == DOCUMENT_STATUS.Draft) ? "Draft" : (item.DocumentStatusID == DOCUMENT_STATUS.Completed) ? "Completed" : "Void",
                                        state: "bapnService",
                                        contactID: contactID,
                                        id: item.BenefitsAssistanceID,
                                        params: {
                                            ContactID: contactID,
                                            ResponseID: item.ResponseID || 0,
                                            SectionID: 0,
                                            ReadOnly: (item.DocumentStatusID == DOCUMENT_STATUS.Draft) ? "edit" : "view",
                                            BenefitsAssistanceID: item.BenefitsAssistanceID,
                                            DocumentStatusID: item.DocumentStatusID
                                        }
                                    });
                                });
                            }
                            tileInfoModel.TileDetails = result;
                            tileInfoModel.IsLoaded = true;
                        },
                            function (errorStatus) {
                                tileInfoModel.IsLoaded = true;
                                tileInfoModel.TileDetails = [getTileDetailsModel(null, "Unable to connect to server", "Error")];
                            });
                    }
                    return tileInfoModel;
                }

                $scope.get = function (contactID) {
                    var isECIClient = false;
                    var strECI = DIVISION_NAME.ECS;
                    var registrationPromise = [];
                    var contactDemographic = null;
                    registrationPromise.push(registrationService.get(contactID));
                    $q.all(registrationPromise).then(function (demographic) {
                        if (demographic !== undefined && demographic !== null && demographic[0].DataItems.length > 0) {
                            contactDemographic = demographic[0];
                            preferredMethodID = contactDemographic.DataItems[0].ContactMethodID;
                            var tmpClientTypeID = demographic[0].DataItems[0].ClientTypeID;
                            if (getText(tmpClientTypeID, $scope.getLookupsByType('ClientType')) === strECI) {
                                if ($injector.has('eciRegistrationTileService')) {
                                    isECIClient = true;
                                }
                            }
                        }//if there is no data available in the regular registration service call then we will assume that the client is an eci patient if the service is available
                        else if ($injector.has('eciRegistrationTileService')) {
                            isECIClient = true;
                        }

                        var tileModel = [];
                        var tilePromises = [];
                        //Add the Tile promises
                        if (isECIClient) {
                            tilePromises.push(getPayorsTile(contactID, true));
                        }
                        else {
                            tilePromises.push(getPayorsTile(contactID, false));
                        }
                        tilePromises.push(getHouseholdIncomeTile(contactID));
                        tilePromises.push(getSelfPaySectionTile(contactID));
                        tilePromises.push(getFinancialAssessmentTile(contactID));
                        tilePromises.push(getBenefitsAssistanceProgressNote(contactID));
                        //Resolve the Tile promises
                        $q.all(tilePromises).then(function (infoModels) {
                            tileModel.push({ SectionName: "Financial", TileInfo: infoModels.slice(0) });
                            tileModel[0].TileInfo = removeNullFromArray(tileModel[0].TileInfo);
                            $scope.Sections = tileModel;
                        });
                    });
                    //Check for Permission
                };

                $scope.get($stateParams.ContactID);
            }
        ]);
}());
