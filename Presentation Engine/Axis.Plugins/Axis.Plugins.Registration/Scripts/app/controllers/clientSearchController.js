angular.module('xenatixApp').
    controller('clientSearchController', [
        '$scope', '$state', '$window', 'alertService', 'clientSearchService', 'settings', '$filter', 'offlineData', '$timeout', '$q', 'lookupService',
        'registrationService', 'additionalDemographyService', 'referralAdditionalDetailService', 'contactBenefitService', 'financialAssessmentService', 'collateralService', 'consentService',
        'contactAddressService', 'contactEmailService', 'contactPhoneService', 'assessmentService', '$injector', 'referralHeaderService', 'referralConcernDetailService', 'auditService',
        function ($scope, $state, $window, alertService, clientSearchService, settings, $filter, offlineData, $timeout, $q, lookupService,
            registrationService, additionalDemographyService, referralAdditionalDetailService, contactBenefitService, financialAssessmentService, collateralService, consentService,
            contactAddressService, contactEmailService, contactPhoneService, assessmentService, $injector, referralHeaderService, referralConcernDetailService, auditService) {
            var contactsTable = $("#contactsTable");
            var offlineContactsDbKey = '/ClientSearch/OfflineContacts';
            var indexedDbProvider = offlineData.indexedDbProvider;
            var contactTypeSearch = '1';
            $scope.$parent['autoFocus'] = true;
            $scope.indexedDbReady = false;
            $scope.cachingContactIds = [];
            var searchPermissionKey = 'Registration-Registration-ContactSearch';

            var registrationNavigationState = [{ Name: 'Demographics', State: 'registration.demographics', PermissionKey: RegistrationPermissionKey.Registration_Demography },
                                            { Name: 'Additional Demographics', State: 'registration.additional', PermissionKey: RegistrationPermissionKey.Registration_AdditionalDemography },
                                            { Name: 'Referral', State: 'registration.referral', PermissionKey: RegistrationPermissionKey.Registration_Registration_Referral },
                                            { Name: 'Collateral', State: 'registration.collateral', PermissionKey: RegistrationPermissionKey.Registration_Collateral },
                                            { Name: 'Payors', State: 'registration.benefits', PermissionKey: RegistrationPermissionKey.Registration_Payors },
                                            { Name: 'Household Income', State: 'registration.financial', PermissionKey: RegistrationPermissionKey.Registration_HouseholdIncome }];

            var eciRegistrationNavigationState = [{ Name: 'Demographics', State: 'eciregistration.demographics', PermissionKey: ECIPermissionKey.ECI_Registration_Demographics },
                                            { Name: 'Additional Demographics', State: 'eciregistration.additionaldemographics', PermissionKey: ECIPermissionKey.ECI_Registration_AdditionalDemographics },
                                            { Name: 'Referral', State: 'eciregistration.referral', PermissionKey: ECIPermissionKey.ECI_Registration_Referral },
                                            { Name: 'Collateral', State: 'eciregistration.family', PermissionKey: ECIPermissionKey.ECI_Registration_Collateral },
                                            { Name: 'Payors', State: 'eciregistration.benefits', PermissionKey: ECIPermissionKey.ECI_Registration_Payors },
                                            { Name: 'Household Income', State: 'eciregistration.financial', PermissionKey: ECIPermissionKey.ECI_Registration_HouseholdIncome }];

            var getPermittedState = function (obj) {
                var roleSecurityService = $injector.get('roleSecurityService');
                for (var i = 0; i < obj.length; i++) {
                    if (roleSecurityService.hasPermission(obj[i].PermissionKey, PERMISSION.READ)) {
                        return obj[i].State;
                    }
                }
                return null;
            }

            var profileState = 'general';

            $scope.initializeBootstrapTable = function () {

                $scope.tableoptions = {
                    pagination: true,
                    pageSize: 10,
                    pageList: [10, 25, 50, 100],
                    search: false,
                    showColumns: true,
                    data: [],
                    undefinedText: '',
                    //onClickRow: function (e, row, $element) {
                    //    row.find("[data-default-action]").triggerHandler('click');
                    //},
                    columns: [
                        {
                            field: 'Color',
                            title: '',
                            sortable: false,
                            formatter: function (value, row, index) {
                                return '<div class="searchrankbar ' + value + '" />';
                            }
                        },
                        {
                            field: 'MRN',
                            title: 'MRN',
                            formatter: function (value, row, index) {
                                var isMerged = row["IsMerged"];
                                return (isMerged ? row["MRN"] + "," + row["MergedMRN"] : row["MRN"]);
                            }
                        },
                        {
                            field: 'FirstName',
                            title: 'First Name'
                        },
                        {
                            field: 'LastName',
                            title: 'Last Name'
                        },
                        {
                            field: 'Middle',
                            title: 'Middle Name'
                        },
                        {
                            field: 'Suffix',
                            title: 'Suffix'
                        },
                        {
                            field: 'DOB',
                            title: 'Date Of Birth',
                            formatter: function (value, row, index) {
                                if (value) {
                                    var formattedDate = $filter('formatDate')(value, 'MM/DD/YYYY');
                                    return formattedDate;
                                } else
                                    return '';
                            }
                        },
                        {
                            field: 'ContactGenderText',
                            title: 'Gender'
                        },
                        {
                            field: 'SSN',
                            title: 'SSN',
                            formatter: function (value, row, index) {
                                var formattedSNN = $filter('toMaskSSN')(value);
                                return formattedSNN;
                            }
                        },
                        {
                            field: 'DriverLicense',
                            title: 'DL #'
                        },

                        {
                            field: 'PreferredName',
                            title: 'Preferred Name'
                        },
                        {
                            field: 'ClientTypeText',
                            title: 'Division'
                        },
                        {
                            field: 'ContactID',
                            title: '',
                            formatter: function (value, row, index) {
                                //ToDo: Place all of this formatter logic in a template url or directive
                                var hasMRN = false;
                                if (row["MRN"] !== null && row["MRN"] !== undefined) {
                                    hasMRN = true;
                                }

                                var clientTypeID = null;
                                if (row["ClientTypeID"] !== null && row["ClientTypeID"] !== undefined) {
                                    var tmpClientTypeID = parseInt(row["ClientTypeID"]);
                                    if (tmpClientTypeID !== NaN) {
                                        clientTypeID = tmpClientTypeID;
                                    }
                                }

                                var initialRegistrationState = '.demographics';
                                var clientTypes = lookupService.getLookupsByType('ClientType');
                                var toState = 'registration.demographics';
                                var permittedState = null;
                                if (clientTypeID !== null) {
                                    var tmpClientTypeValue = clientTypes[clientTypes.map(function (e) { return e.ID; }).indexOf(clientTypeID)];
                                    if (tmpClientTypeValue !== null && tmpClientTypeValue !== undefined && tmpClientTypeValue.Division == "ECS") {
                                        toState = tmpClientTypeValue.RegistrationState;
                                        permittedState = getPermittedState(eciRegistrationNavigationState);
                                    }
                                    else {
                                        permittedState = getPermittedState(registrationNavigationState);
                                    }
                                }
                                else {
                                    permittedState = getPermittedState(registrationNavigationState);
                                }
                                if (permittedState)
                                    toState = permittedState;
                                //registration/profile
                                return (hasMRN
                                        ? '<span class="text-nowrap"><a data-default-action ui-sref="patientprofile.' + profileState + '({ ContactID: ' + value + ' })" alt="Profile" ' +
                                            'security modules="General|Benefits|Consents|Intake|Chart" permission="read" program-units="' + row.ProgramUnit + '" on-action="openFlyout(' + value + ')" audit-on="click" audit-key="Contact" audit-value="' + value + '" space-key-press>' +
                                            '<i  title="Profile" class="fa fa-user fa-fw border-left margin-left padding-left-small padding-right-small" audit-on="click" customData="ABC"/></a>'
                                        : '<span class="text-nowrap"><a data-default-action ng-click=edit("' + toState + '",' + value + ',"' + ((toState.toLowerCase().indexOf('eci') >= 0) ? 'ECI' : 'Registration') + '") alt="View Contact" ' +
                                            'security modules="' + ((toState.toLowerCase().indexOf('eci') >= 0) ? 'ECI' : 'Registration') + '" permission="create|update" program-units="' + row.ProgramUnit + '" on-action="openFlyout(' + value + ')" audit-on="click" audit-key="Contact" audit-value="' + value + '" space-key-press>' +
                                            '<i  title="Edit" class="fa fa-pencil fa-fw border-left margin-left padding-left-small padding-right-small" /></a>') +
                                        //charts
                                        (hasMRN
                                        ? '<a data-default-no-action ui-sref="patientprofile.chart({ ContactID: ' + value + ' })" alt="Chart" ng-if="false" security permission-key="Registration-Registration-Demographics|ECI-Registration-Demographics" permission="update" audit-on="click" audit-key="Contact" audit-value="' + value + '" space-key-press><i class="fa fa-file-text fa-fw border-left margin-left padding-left-small padding-right-small" title="Chart" /></a>'
                                        : '<a data-default-no-action ui-sref="' + toState + '({ ContactID: ' + value + ' })" alt="Registration" ng-if="false" security permission-key="Registration-Registration-Demographics|ECI-Registration-Demographics" permission="update" audit-on="click" audit-key="Contact" audit-value="' + value + '" space-key-press><i class="fa fa-file-text fa-fw border-left margin-left padding-left-small padding-right-small" title="Registration" /></a>')
                                        +
                                       //offline
                                        ($scope.indexedDbReady ? '<a data-default-no-action ng-click="toggleForOfflineUse(' + value + ',' + row.ClientTypeID + ',' + '\'' + row.ClientTypeText + '\'' + ')" href="javascript:void(0)" alt="Save For Offline Use" security permission-key="Registration-Registration-Demographics|ECI-Registration-Demographics" permission="update" style="cursor: pointer;" audit-on="click" audit-key="Contact" audit-value="' + value + '" space-key-press><i class="border-left margin-left padding-left-small" /><i ng-class="isSavedForOfflineUse(' + value + ') ? \'fa-hdd-o\' : isSavingForOfflineUse(' + value + ') ? \'fa-spinner fa-spin\' : \'fa-cloud-download\'" class="fa fa-fw" /><i class="padding-right-small" /></a>' : '') +
                                        '</span>';
                            }
                        }
                    ]
                };
            };
            $scope.edit = function (navigationState, id, module) {
                var roleSecurityService = $injector.get('roleSecurityService');
                if (roleSecurityService.hasModulePermission(module, 'create|update')) {
                    $state.go(navigationState, { ContactID: id }).then(function () {
                        var $rootScope = $injector.get('$rootScope');
                        $timeout(function () {
                            var stateDetail = { stateName: navigationState, EnableAllWorkFlow: true, validationState: 'valid' };
                            $rootScope.$broadcast(((navigationState.toLowerCase().indexOf('eci') >= 0) ? 'rightNavigationECIRegistrationHandler' : 'rightNavigationRegistrationHandler'), stateDetail);
                        });

                    });
                }
            }

            //Get the patients detail based on the search text
            $scope.getClientSummary = function (searchText) {
                //if (searchText.toLowerCase() === 'go offline')
                //    $scope.setFakeOffline(true);
                //else if (searchText.toLowerCase() === 'go online')
                //    $scope.setFakeOffline(false);
                //else {
                if (searchText != '' && searchText.length < 3) {
                    alertService.warning("Please use at least 3 characters when performing a search.");
                    return;
                }
                var searchString = searchText.split('=');
                if (searchString.length > 1) {
                    if (!validateSearchPattern(searchString))
                        alertService.warning("Please use the correct format for your search criteria: SSN=NNNN/NNNNNNNNN, DOB=MM/DD/YYYY, DL#=NNNNNNNN, MRN=NNNNNNNNN");
                    else
                        getClientSearch(searchText, contactTypeSearch);
                }
                else {
                    getClientSearch(searchText, contactTypeSearch);
                }
                //}
            };

            var getClientSearch = function (searchText, contactTypeSearch) {
                clientSearchService.getClientSummary(searchText, contactTypeSearch).then(function (data) {
                    if (data && data.DataItems && searchText != '') {
                        var hasProfileItems = $filter('filter')(data.DataItems, function (itm) {
                            return (itm.MRN !== null && itm.MRN !== undefined)
                        })
                        if (hasProfileItems && hasProfileItems.length > 0 && $injector.has('roleSecurityService')) {
                            var roleSecurityService = $injector.get('roleSecurityService');
                            var profileNavigationState = [{ Name: 'General', State: 'general' },
                                                          { Name: 'Chart', State: 'chart' },
                                                          { Name: 'Benefits', State: 'benefits' },
                                                          { Name: 'Intake', State: 'intake' },
                                                          { Name: 'Consents', State: 'consents' },
                                                          { Name: 'Appointments', State: 'appointments' }];
                            for (var i = 0; i < profileNavigationState.length; i++) {
                                if (roleSecurityService.hasModulePermission(profileNavigationState[i].Name, PERMISSION.READ)) {
                                    profileState = profileNavigationState[i].State;
                                    break;
                                }
                            }
                        }
                        contactsTable.bootstrapTable('load', data.DataItems);
                        $scope.$parent['autoFocus'] = true;
                    } else {
                        contactsTable.bootstrapTable('removeAll');
                        $scope.$parent['autoFocus'] = true;
                    }
                    if (searchText == '') {
                        if (contactsTable.find('tr').hasClass('no-records-found')) {
                            contactsTable.find('tr[class="no-records-found"]').remove();
                        }
                    }
                    applyDropupOnGrid(true);
                }, function (errorStatus) {
                    alertService.error('Unable to connect to server');
                }).finally(function () {
                    var pageAudit = {
                        DataKey: searchPermissionKey,
                        ActionTypeID: SCREEN_ACTIONTYPES.View,
                        SearchText: searchText
                    }
                    auditService.addScreenAudit(pageAudit);
                });
            };

            $scope.toggleForOfflineUse = function (contactId, clientTypeID, clientTypeText) {
                var existingIndex = $scope.offlineContacts.indexOf(contactId);
                if (existingIndex >= 0) {
                    $scope.offlineContacts.splice(existingIndex, 1);
                    indexedDbProvider.update({ entityUrl: offlineContactsDbKey, data: JSON.stringify($scope.offlineContacts) });
                    //TODO: Clear IndexedDB?
                } else if (offlineData.isOnline()) {
                    $scope.cachingContactIds.push(contactId);
                    var cachePromises = [];

                    //registration plugin
                    cachePromises.push(registrationService.get(contactId));
                    cachePromises.push(contactAddressService.get(contactId));
                    cachePromises.push(contactEmailService.get(contactId));
                    cachePromises.push(contactPhoneService.get(contactId, 1));
                    cachePromises.push(additionalDemographyService.getAdditionalDemographic(contactId));
                    cachePromises.push(getAllReferral(contactId, clientTypeID));
                    cachePromises.push(contactBenefitService.get(contactId));

                    var contactTypeId = 0;
                    var contactTypeIDArray = $scope.getLookupsByType('ClientType').filter(function (obj) { return obj.Name === clientTypeText; });//[0].ID;
                    if (contactTypeIDArray.length > 0) {
                        contactTypeId = contactTypeIDArray[0].ID;
                    }

                    cachePromises.push(collateralService.get(contactId, contactTypeId === 1 ? $scope.familyRelationship : $scope.collateralContactTypeId, false));
                    var faDeferred = $q.defer();
                    cachePromises.push(faDeferred.promise);
                    financialAssessmentService.getActiveFA(contactId).then(function (data) {
                        var innerPromises = [];
                        angular.forEach(data.DataItems, function (financialAssessment) {
                            innerPromises.push($scope.promiseWrap(financialAssessmentService.getDetails(contactId, financialAssessment.FinancialAssessmentID)));
                        });
                        $q.all(innerPromises);
                        faDeferred.resolve();
                    });
                    cachePromises.push(consentService.getConsentSignature(contactId));

                    //eci plugin
                    if ($injector.has('eciDemographicService')) {
                        var eciDemographicService = $injector.get('eciDemographicService');
                        cachePromises.push(eciDemographicService.get(contactId));
                    }
                    if ($injector.has('eciAdditionalDemographicService')) {
                        var eciAdditionalDemographicService = $injector.get('eciAdditionalDemographicService');
                        cachePromises.push(eciAdditionalDemographicService.getAdditionalDemographic(contactId));
                    }
                    if ($injector.has('screeningService')) {
                        var screeningService = $injector.get('screeningService');
                        var screeningDeferred = $q.defer();
                        cachePromises.push(screeningDeferred.promise);
                        screeningService.getList(contactId).then(function (data) {
                            if (data.DataItems.length > 0)
                                $q.all(data.DataItems.map(function (screening) { return (screening.AssessmentID && screening.ResponseID) ? $scope.getAssessmentCommon(contactId, screening.AssessmentID, screening.ResponseID) : undefined; }));
                            screeningDeferred.resolve();
                        });
                    }
                    if ($injector.has('eligibilityDeterminationService') && $injector.has('eligibilityCalculationService')) {
                        var eligibilityDeterminationService = $injector.get('eligibilityDeterminationService');
                        var eligibilityCalculationService = $injector.get('eligibilityCalculationService');
                        var edDeferred = $q.defer();
                        cachePromises.push(edDeferred.promise);
                        eligibilityDeterminationService.get(contactId).then(function (data) {
                            var innerEdPromises = [];
                            angular.forEach(data.DataItems, function (eligRecord) {
                                //load all of the necessary data for each eligibility record as needed for editing/reporting
                                innerEdPromises.push(eligibilityCalculationService.get(eligRecord.EligibilityID));
                                innerEdPromises.push(eligibilityDeterminationService.getEligibilityNote(eligRecord.EligibilityID));
                            });
                            $q.all(innerEdPromises);
                            edDeferred.resolve();
                        });
                    }
                    if ($injector.has('ifspService')) {
                        var ifspService = $injector.get('ifspService');
                        var ifspDeferred = $q.defer();
                        cachePromises.push(ifspDeferred.promise);
                        ifspService.getList(contactId).then(function (data) {
                            if (data.DataItems.length > 0)
                                $q.all(data.DataItems.map(function (ifsp) { return (ifsp.AssessmentID && ifsp.ResponseID) ? $scope.getAssessmentCommon(contactId, ifsp.AssessmentID, ifsp.ResponseID) : undefined; }));
                            ifspDeferred.resolve();
                        });
                    }

                    //clinical plugin
                    //allergy
                    if ($injector.has('allergyService')) {
                        var allergyService = $injector.get('allergyService');
                        var allergyDeferred = $q.defer();
                        cachePromises.push(allergyService.getTopAllergies(contactId));
                        cachePromises.push(allergyDeferred.promise);
                        var allergyTypes = lookupService.getLookupsByType('AllergyType');
                        var innerAllergyPromises = [];
                        angular.forEach(allergyTypes, function (allergyType) {
                            allergyService.getAllergyBundle(contactId, allergyType.ID).then(function (data) {
                                if (data.DataItems.length > 0) {
                                    innerAllergyPromises.push(allergyService.getAllergyDetails(contactId, data.DataItems[0].ContactAllergyID, allergyType.ID));
                                }
                            });
                        });
                        $q.all(innerAllergyPromises);
                        allergyDeferred.resolve();
                    }
                    //endallergy
                    //socialrelationship
                    if ($injector.has('socialRelationshipService') && $injector.has('socialRelationshipHistoryService')) {
                        var socialRelationshipService = $injector.get('socialRelationshipService');
                        var socialRelationshipHistoryService = $injector.get('socialRelationshipHistoryService');
                        var socialDeferred = $q.defer();
                        cachePromises.push(socialDeferred.promise);
                        socialRelationshipService.getSocialRelationships(contactId).then(function (data) {
                            var innerSocialPromises = [];
                            angular.forEach(data.DataItems, function (socialRelationship) {
                                innerSocialPromises.push(socialRelationshipHistoryService.get(contactId, socialRelationship.SocialRelationshipID));
                                innerSocialPromises.push(socialRelationshipHistoryService.getDetails(contactId, socialRelationship.SocialRelationshipID));
                            });

                            $q.all(innerSocialPromises);
                            socialDeferred.resolve();
                        });
                    }
                    //endsocialrelationship
                    if ($injector.has('clinicalAssessmentService')) {
                        var clinicalAssessmentService = $injector.get('clinicalAssessmentService');
                        var clinicalAssessDeferred = $q.defer();
                        cachePromises.push(clinicalAssessDeferred.promise);
                        clinicalAssessmentService.getClinicalAssessmentByContact(contactId).then(function (data) {
                            if (data.DataItems.length > 0)
                                $q.all(data.DataItems.map(function (clinAssess) { return (clinAssess.AssessmentID && clinAssess.ResponseID) ? $scope.getAssessmentCommon(contactId, clinAssess.AssessmentID, clinAssess.ResponseID) : undefined; }));
                            clinicalAssessDeferred.resolve();
                        });
                    }
                    if ($injector.has('vitalService')) {
                        var vitalService = $injector.get('vitalService');
                        cachePromises.push(vitalService.getContactVital(contactId));
                    }

                    $q.all(cachePromises).then(function () {
                        $scope.cachingContactIds.splice($scope.cachingContactIds.indexOf(contactId));
                        $scope.offlineContacts.push(contactId);
                        indexedDbProvider.update({ entityUrl: offlineContactsDbKey, data: JSON.stringify($scope.offlineContacts) });
                    });
                }
            };

            $scope.getAssessmentCommon = function (contactId, assessmentId, responseId) {
                var promiseArray = [];
                var deferred = $q.defer();
                if (assessmentId && responseId) {
                    promiseArray.push(assessmentService.getAssessmentResponse(contactId, assessmentId, responseId));
                    assessmentService.getAssessmentSections(assessmentId).then(function (data) {
                        angular.forEach(data.DataItems, function (section) {
                            promiseArray.push(assessmentService.getAssessmentResponseDetails(responseId, section.AssessmentSectionID));
                        });
                        $q.all(promiseArray);
                        deferred.resolve();
                    });
                } else {
                    $q.when(true).then(function () {
                        deferred.resolve();
                    });
                }
                return deferred.promise;
            };

            $scope.isSavedForOfflineUse = function (contactId) {
                return $scope.offlineContacts.indexOf(contactId) >= 0;
            };

            $scope.isSavingForOfflineUse = function (contactId) {
                return $scope.cachingContactIds.indexOf(contactId) >= 0;
            };

            $scope.initializeOfflineContacts = function () {
                $scope.offlineContacts = [];
                if (indexedDbProvider && (typeof (indexedDbProvider.ready) == 'function') && (indexedDbProvider.ready() === true)) {
                    indexedDbProvider.get(offlineContactsDbKey).then(function (result) {
                        if ((result !== undefined) && ('data' in result))
                            $scope.offlineContacts = JSON.parse(result.data);
                        else {
                            indexedDbProvider.add({ entityUrl: offlineContactsDbKey, data: JSON.stringify($scope.offlineContacts) });
                        }
                    });
                    $scope.indexedDbReady = true;
                } else {
                    $timeout($scope.initializeOfflineContacts, 2000);
                }
            };

            $scope.init = function () {
                $scope.$parent['autoFocus'] = true;
                $scope.searchText = '';
                $scope.initializeBootstrapTable();
                $scope.initializeOfflineContacts();
                var contactTypes = lookupService.getLookupsByType('ContactType');
                $scope.emergencyContactTypeId = contactTypes.filter(function (obj) { return (obj.Name === 'Emergency'); })[0].ID;
                $scope.familyRelationship = contactTypes.filter(function (obj) { return (obj.Name === 'Family Relationship'); })[0].ID;
                $scope.collateralContactTypeId = contactTypes.filter(function (obj) { return (obj.Name === 'Collateral'); })[0].ID;
                $scope.getClientSummary('');
            };
            getAllReferral = function (contactID, clientTypeID) {
                var dfd = $q.defer();
                referralAdditionalDetailService.getReferral(contactID).then(function (data) {
                    if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                        var headerID = data.DataItems[0].ReferralHeaderID;

                        var headerContactID = data.DataItems[0].HeaderContactID;
                        var referralAdditionalDetailID = data.DataItems[0].ReferralAdditionalDetailID;
                        var pArray = [];
                        pArray.push(referralHeaderService.getReferralHeader(headerID, headerContactID));
                        pArray.push(registrationService.get(headerContactID));
                        pArray.push(contactAddressService.get(headerContactID, CONTACT_TYPE.Referral_Requestor));
                        pArray.push(contactEmailService.get(headerContactID, CONTACT_TYPE.Referral_Requestor));
                        pArray.push(contactPhoneService.get(headerContactID, CONTACT_TYPE.Referral_Requestor));
                        if (clientTypeID == PROGRAM_TYPE.ECI)
                            pArray.push(referralConcernDetailService.getReferralConcernDetail(referralAdditionalDetailID));
                        $q.all(pArray).finally(function () {
                            dfd.resolve();
                        });
                    }
                    else {
                        dfd.resolve();
                    }
                }, function (error) {
                    dfd.resolve();
                });
                dfd.promise;
            }
            $scope.init();
        }
    ]);
