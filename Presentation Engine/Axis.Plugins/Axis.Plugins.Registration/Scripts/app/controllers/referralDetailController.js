(function () {
	angular.module('xenatixApp')
        .controller('referralDetailController', [
    '$scope', '$q', '$state', '$stateParams', 'alertService', '$filter', '$rootScope', '$controller', 'formService', 'referralConcernDetailService', 'contactAddressService', 'contactEmailService', 'contactPhoneService', "referralAdditionalDetailService", "registrationService", '$timeout', 'referralHeaderService', 'globalObjectsService', 'referralClientInformationService', 'isECIClient', 'roleSecurityService', 'lookupService', 'contactRelationshipService',
    function ($scope, $q, $state, $stateParams, alertService, $filter, $rootScope, $controller, formService, referralConcernDetailService, contactAddressService, contactEmailService, contactPhoneService, referralAdditionalDetailService, registrationService, $timeout, referralHeaderService, globalObjectsService, referralClientInformationService, isECIClient, roleSecurityService, lookupService, contactRelationshipService) {
        $controller('baseContactController', { $scope: $scope });
        $scope.ShowPhoneDaggerOnly = true;
        $scope.ShowEmailDaggerOnly = true;
        $scope.isReferral = true;
        $scope.PhoneAccessCode = $scope.PHONE_ACCESS.ConditionalRequired | $scope.PHONE_ACCESS.Number;
        $scope.EmailAccessCode = $scope.EMAIL_ACCESS.ConditionalRequired | $scope.EMAIL_ACCESS.Email;
        $scope.ReferralSource = isECIClient ? "Contact Organization" : "Referral Source";
        $scope.permissionKey = $state.current.data.permissionKey;
        var gotoNext = false;
        var linkedContactID;
        var contactReferralTypeID = CONTACT_TYPE.Referral_Requestor;
        var isSaving = false;
        var closedReferralStatusId = REFERRAL_STATUS.Closed;
        var externalReferralTypeId = REFERRALTYPE.External;
        var tableDefaultOptions = {
            pagination: true,
            pageSize: 5,
            pageList: [10, 20, 30],
            search: false,
            showColumns: true,
            undefinedText: ''
        };

        var tableColumns = {
            columns: [
                {
                    field: "ReferralDate",
                    title: "Referral Date",
                    sortable: true,
                    formatter: function (value, row) {
                        if (value) {
                            return $filter("toMMDDYYYYDate")(value);
                        } else
                            return "";
                    }
                },
                {
                    field: "ReferralSourceID",
                    title: "Referral Source",
                    sortable: true,
                    formatter: function (value, row) {
                        if (value) {
                            return lookupService.getText("ReferralSource", value);
                        } else
                            return "";
                    }
                },
                {
                    field: "FirstName",
                    title: "Referral First Name",
                    sortable: true
                },
                {
                    field: "LastName",
                    title: "Referral Last Name",
                    sortable: true
                },
                {
                    field: "PhoneNumber",
                    title: "Referral Phone Number",
                    sortable: true,
                    formatter: function (value) {
                        return formatPDFPhone(value);
                    }
                },
                {
                    field: "ReferralConcern",
                    title: "Referral Concern",
                    sortable: true,
                    visible: isECIClient ? true : false,
                    formatter: function (value, row) {
                        return value ? value : '';
                    }

                },
                {
                    field: "ReferralHeaderID",
                    title: "",
                    formatter: function (value, row, index) {
                        //var hasEditPermission = roleSecurityService.hasPermission($scope.permissionKey, PERMISSION.UPDATE);
                        var mode = $scope.hasEditPermission ? "edit" : "view";
                        return (
                            '<span class="text-nowrap pull-right">' +
                            '<a href="javascript:void(0)" data-default-action ng-click="edit(' + value + ',' + row.ContactID + ')" alt="' + mode + '" security permission-key="{{permissionKey}}" permission="' + ($scope.hasEditPermission ? 'update' : 'read') + '" class="padding-left-small padding-right-small" >' +
                            '<i title="' + mode + '" class="fa ' + ($scope.hasEditPermission ? 'fa-pencil' : 'fa-eye') + ' fa-fw" /></a>' +
                            '<a data-default-no-action class="padding-left-small padding-right-small" href="javascript:void(0)" ng-click="removeReferral(' + row.ContactID + ',' + row.IsReferrerConvertedToCollateral + ' )" alt="Remove" ' +
                                    'security permission-key="{{permissionKey}}" permission="delete" space-key-press><i title="Remove" class="fa fa-trash fa-fw" /></a>'
                    + '</span>');
                    }
                }
            ]
        };
        var referralSummaryTable = $("#reffralSummaryTable");
        $scope.tableoptions = tableDefaultOptions;
        $scope.tableoptions.columns = tableColumns.columns;
        $scope.isECI = isECIClient;
        $scope.hasEditPermission = roleSecurityService.hasPermission($scope.permissionKey, PERMISSION.UPDATE);

        $scope.init = function () {
            $scope.convertToCollateral = true;
            $scope.collateralTypeFamily = COLLATERAL_TYPE.Family;
            $scope.otherRelationship = COLLATERAL_RELATIONSHIP_TYPE.OtherRelationship;
            $scope.otherPhysician = COLLATERAL_RELATIONSHIP_TYPE.OtherPhysician;
            $scope.otherProvider = COLLATERAL_RELATIONSHIP_TYPE.OtherProvider;
            $scope.policyHolderValues = [{ Value: true, Name: 'Yes' }, { Value: false, Name: 'No' }];
            $scope.LinkedContactID = false;
            $scope.isEmailDirty = false;
            $scope.permissionId = 0;
            $scope.isPhoneDirty = false;
            $scope.endDate = new Date();
            $scope.opened = false;
            $scope.referralDate = 'referralDate';
            $scope.ReferralCategorySourceLookUpType = 'ReferralCategorySource';
            $scope.startDate = $filter('toMMDDYYYYDate')(new Date());
            $scope.EnableFilter = true;
            initConcerns();
            initReferralDetail();
            initReferralAdditionalDetail();
            initContactRelationship();
            $scope.hideRelationshipRange = true;
            $scope.dateOptions = {
                formatYear: 'yy',
                startingDay: 1,
                showWeeks: false
            };
            initPhones();
            initEmails();
            initAddress();
            $scope.ReferralOrganization = '';
            $scope.noCancel = true;
            $scope.enterKeyStop = $state.current.name.toLowerCase().indexOf('patientprofile') >= 0;
            $scope.saveOnEnter = $state.current.name.toLowerCase().indexOf('patientprofile') >= 0;
            $scope.ReferralConcernTypeLookUpData = $rootScope.getLookupsByType('ReferralConcernType');
            $scope.emailPattern = /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/i;
            $scope.noReferralConcernDetailResults = false;
            $scope.OtherContactOrganisationOption = 'Other';
            $scope.ReferralAdditionalDetailID = 0;
            $scope.selectedReferralConcernDetails = [];
            $scope.ReferralConcernDetail = '';
            $scope.ReferralConcernDetailsChanged = false;
            if ($state.current.name.toLowerCase().indexOf('patientprofile') >= 0) {
                $scope.noNext = true;
            }
            else {
                $timeout(function () {
                    var nextState = angular.element("xen-workflow-action[data-state-name] > li.list-group-item.active").parents('.work-flow').next().find("xen-workflow-action[data-state-name]");
                    $scope.noNext = (nextState.length === 0);
                });
            }
            $scope.$parent['autoFocusEdit'] = true;
            resetForm();
        }
        
        var initContactRelationship = function () {
            $scope.ContactRelationships = [];
            $scope.ContactRelationships.push(objContactRelationship());
        };

        $scope.validateCollateralType = function () {
            $scope.ContactRelationships[0].IsPolicyHolder = '';
            var filteredItems = filterContactRelationship();
            if (filteredItems && filteredItems.length > 0) {
                setRequiredFields(true);
            }
            else if (!filteredItems || filteredItems.length == 0) {
                setRequiredFields();
            }
        };
        $scope.resetCollateralInformation = function () {
            //TO DO : xenCheckBox one value previous
            if ($scope.ReferralDetail.IsReferrerConvertedToCollateral) {
                initContactRelationship();
                $scope.ReferralDetail.DriverLicense = null;
                $scope.ReferralDetail.DriverLicenseStateID = null;
                setRequiredFields();
            }
        }
        var setRequiredFields = function (isPolicyHolder) {
            if ($scope.ReferralDetail) {
                isPolicyHolder ? setAddressRequiredStatus($scope.ReferralDetail.IsReferrerConvertedToCollateral) : setAddressRequiredStatus(false);
            }
        };

        var setAddressRequiredStatus = function (isRequired) {
            $scope.AddressAccessCode = isRequired ? $scope.ADDRESS_ACCESS.Required | $scope.ADDRESS_ACCESS.Line1 | $scope.ADDRESS_ACCESS.City | $scope.ADDRESS_ACCESS.State | $scope.ADDRESS_ACCESS.PostalCode | $scope.ADDRESS_ACCESS.County : 0;
        }

        $scope.validatePolicyHolder = function (relationship) {
            var currentPolicyHolder = relationship.IsPolicyHolder;
            var filteredItems = filterContactRelationship();
            if (currentPolicyHolder && currentPolicyHolder === true) {
                if (filteredItems.length > 1) {
                    angular.forEach($scope.ContactRelationships, function (item) {
                        if (item.IsActive)
                            item.IsPolicyHolder = false;
                    });
                    relationship.IsPolicyHolder = true;
                }
                setRequiredFields(true);
            }
            else if (!filteredItems || filteredItems.length == 0) {
                setRequiredFields();
            }
        };

        var objContactRelationship = function () {
            return {
                ContactRelationshipTypeID: 0,
                ContactID: 0,
                OtherRelationship: '',
                EffectiveDate: $filter('toMMDDYYYYDate')(new Date()),
                LivingWithClientStatus: null,
                ExpirationDate: '',
                ShowPlusButton: false,
                ShowMinusButton: false,
                IsActive: true
            }
        }


        var checkFormStatus = function (isValid) {           
            $rootScope.$broadcast($state.current.name,{validationState: (isValid ? 'valid' : 'warning')});            
        };

        $scope.CancelReferral = function () {
            //TODO
        };
        $scope.removeReferral = function (contactID, isReferrerConvertedToCollateral) {
            var warningMessage = isReferrerConvertedToCollateral ? "This referral has been converted to a collateral. Are you sure you want to delete the referral?" : "Are you sure that you want to delete this referral ?";
            bootbox.confirm(warningMessage,
                function (result) {
                    if (result) {
                        referralAdditionalDetailService.remove(contactID)
                            .then(function (data) {
                                if (data.ResultCode == 0) {
                                    $scope.getReferralDetails();
                                    alertService.success("Referral deleted sucessfully.");
                                    $scope.init();
                                } else {
                                    alertService.error(data.ResultMessage);
                                }
                            })
                    }
                    else {
                        //do nothing
                    }
                });
        };
        $scope.validateReferralDetailDate = function () {
            var errorControlBlock = "#referralDetailDateErrorBlock";
            var errorControl = "#referralDetailDateError";
            var formControl = $scope.ctrl.referralMainForm.referralDetailForm.referralDate;
            var dateControl = $scope.ReferralDetail.ReferralDate;
            var compareDateControl = new Date();
            validateFutureDate(errorControlBlock, errorControl, formControl, dateControl, compareDateControl);
        };

        var resetDetail = function () {
            if ($scope.ctrl.referralMainForm.referralDetailForm)
                $rootScope.formReset($scope.ctrl.referralMainForm.referralDetailForm,
                    $scope.ctrl.referralMainForm.referralDetailForm.$name);
        };
        var resetPhone = function () {
            if ($scope.ctrl.referralMainForm.referralPhoneForm)
                $rootScope.formReset($scope.ctrl.referralMainForm.referralPhoneForm,
                    $scope.ctrl.referralMainForm.referralPhoneForm.$name);
        };
        var resetEmail = function () {
            if ($scope.ctrl.referralMainForm.referralEmailForm)
                $rootScope.formReset($scope.ctrl.referralMainForm.referralEmailForm,
                    $scope.ctrl.referralMainForm.referralEmailForm.$name);
        };
        var resetAddress = function () {
            if ($scope.ctrl.referralMainForm.referralAddressForm)
                $rootScope.formReset($scope.ctrl.referralMainForm.referralAddressForm,
                    $scope.ctrl.referralMainForm.referralAddressForm.$name);
        };
        var resetForm = function () {
            if ($scope.ctrl.referralMainForm) {
                resetDetail();
                resetPhone();
                resetEmail();
                resetContactRelationship();
                resetAddress();
                $scope.ReferralConcernDetailsChanged = false;
                $rootScope.formReset($scope.ctrl.referralMainForm);
            }
        };

        var resetContactRelationship = function () {
            if ($scope.ctrl.referralMainForm.ContactRelationshipForm) {
                $rootScope.formReset($scope.ctrl.referralMainForm.ContactRelationshipForm, $scope.ctrl.referralMainForm.ContactRelationshipForm.$name);
                if ($scope.ctrl.referralMainForm.ContactRelationshipForm.relationshipForm)
                    $rootScope.formReset($scope.ctrl.referralMainForm.ContactRelationshipForm.relationshipForm, $scope.ctrl.referralMainForm.ContactRelationshipForm.relationshipForm.$name);
            }
        }

        var filterContactRelationship = function () {
            return $filter('filter')($scope.ContactRelationships, function (data) {
                return data.IsActive === true && data.IsPolicyHolder === true;
            });
        }

        $scope.selectReferralConcernDetail = function (item) {
            var idx = -1;
            for (var i = 0; i < $scope.selectedReferralConcernDetails.length; i++) {
                if ($scope.selectedReferralConcernDetails[i].ReferralConcernID === item.ID) {
                    idx = i;
                    break;
                }
            }
            if (idx === -1) {
                $scope.selectedReferralConcernDetails.push({ ReferralConcernID: item.ID, ReferralConcern: item.Name, ReferralConcernDetailID: 0, ReferralAdditionalDetailID: $scope.ReferralAdditionalDetailID, IsActive: true });
                $scope.ReferralConcernDetailsChanged = true;
            }
            else if ($scope.selectedReferralConcernDetails[idx].IsActive == undefined || $scope.selectedReferralConcernDetails[idx].IsActive == null || $scope.selectedReferralConcernDetails[idx].IsActive == false) {
                $scope.selectedReferralConcernDetails[idx].IsActive = true;
            }
            $scope.ReferralConcernDetail = '';
            setConcernErrorClass();
        };

        $scope.removeReferralConcernDetail = function (ReferralConcernDetail) {
            var idx = -1;
            for (var i = 0; i < $scope.selectedReferralConcernDetails.length; i++) {
                if ($scope.selectedReferralConcernDetails[i].ReferralConcernID === ReferralConcernDetail.ReferralConcernID) {
                    idx = i;
                    break;
                }
            }
            if (idx > -1) {
                if ($scope.selectedReferralConcernDetails[idx].ReferralConcernDetailID != 0) {
                    $scope.selectedReferralConcernDetails[idx].IsActive = false;
                }
                else
                    $scope.selectedReferralConcernDetails.splice(idx, 1);
                $scope.ReferralConcernDetailsChanged = true;
            }
            setConcernErrorClass();
        };

        var setConcernErrorClass = function () {
            var referralConcernErrorBlock = $('#referralConcernErrorBlock');
            if (referralConcernErrorBlock != undefined && referralConcernErrorBlock != null) {
                if (referralConcernErrorBlock.hasClass('has-error'))
                    referralConcernErrorBlock.removeClass('has-error');
                if ($scope.selectedReferralConcernDetails.length === 0)
                    referralConcernErrorBlock.addClass('has-error');
                else {
                    var activeConcerns = $filter('filter')($scope.selectedReferralConcernDetails,
                        function (data) {
                            return data.IsActive;
                        });
                    if (activeConcerns.length === 0)
                        referralConcernErrorBlock.addClass('has-error');
                }
            }
        };

        var referralData = [];

        $scope.getReferralDetails = function () {
            var contactID = $stateParams.ContactID;
            
            referralAdditionalDetailService.getReferralDetails(contactID).then(function (data) {
                referralData = $filter('orderBy')(data.DataItems, 'ModifiedOn', true);
                checkFormStatus(hasData(data));              
                $scope.permissionId = 0;
                referralSummaryTable.bootstrapTable('load', referralData);
                if ($stateParams.ReferralContactID && $stateParams.ReferralHeaderID) {
                    setGridItem(referralSummaryTable, 'ReferralHeaderID', $stateParams.ReferralHeaderID);
                    $stateParams.ReferralContactID = 0;
                }
            },
            function (errorStatus) {
                checkFormStatus(false);
                alertService.error('Unable to get referralDetail: ' + errorStatus);
            }).finally(function () {
                resetForm();               
            });
        };

        $scope.edit = function (referralHeaderId, contactId) {
            $scope.isLoading = true;
            //var referral = $filter('filter')(referralData, { ReferralHeaderID: referralHeaderId });
            //if (hasDetails(referral)) {
            var items = $filter('filter')(referralData, function (refer) {
                return (refer.ReferralHeaderID != referralHeaderId && refer.ContactID == contactId && refer.IsReferrerConvertedToCollateral);
            });
            $scope.convertToCollateral = !hasDetails(items);
            //} else {
            //    $scope.convertToCollateral = true;
            //}

            referralClientInformationService.getReferralClientInformation(referralHeaderId).then(function (data) {
                if (hasData(data)) {
                    $scope.ReferralAdditionalDetails = data.DataItems[0].referralClientAdditionalDetails;
                    $scope.ReferralAdditionalDetailID = $scope.ReferralAdditionalDetails.ReferralAdditionalDetailID;
                    getReferralHeaderDetail($scope.ReferralAdditionalDetails.ReferralHeaderID, contactId);
                    getConcerns($scope.ReferralAdditionalDetailID);
                    $scope.permissionId = $scope.ReferralAdditionalDetails.ReferralHeaderID;
                }
            },
            function (errorStatus) {
                alertService.error('Unable to get referralDetail: ' + errorStatus);
            }).finally(function () {
                $scope.isLoading = false;
                resetForm();               
            });
        };

        var getReferralHeaderDetail = function (headerID, contactID) {
            referralHeaderService.getReferralHeader(headerID, contactID).then(function (data) {
                if (hasData(data)) {
                    $scope.ReferralDetail = data.DataItems[0];
                    if ($scope.ReferralDetail.ContactRelationShip) {
                        $scope.ContactRelationships = [];
                        $scope.ContactRelationships.push($scope.ReferralDetail.ContactRelationShip);
                        if ($scope.ReferralDetail.ContactRelationShip.IsPolicyHolder)
                            setRequiredFields(true);
                        else
                            setRequiredFields();
                    } else {
                        setRequiredFields();
                    }
                    if ($scope.ReferralDetail.IsLinkedToContact) {
                        $scope.LinkedContactID = true;
                        linkedContactID = $scope.ReferralDetail.ContactID;
                    }
                    else {
                        $scope.LinkedContactID = false;
                }
                    $scope.ReferralDetail.disableCollateralToReferral = $scope.ReferralDetail.IsReferrerConvertedToCollateral;
                    $scope.ReferralDetail.ReferralDate = $filter('toMMDDYYYYDate')($scope.ReferralDetail.ReferralDate);
                    if ($scope.ReferralDetail.ReferralSourceID) {
                        var referralOrganization = $scope.getLookupsByType('ReferralSource');
                        $scope.noResults = !(referralOrganization);
                        //$scope.OtherContactOrganisationOption = $filter('filter')(referralOrganization, { Name: 'Other' }, true)[0].ID; 
                        angular.forEach(referralOrganization, function (item) {
                            if (item.ID == $scope.ReferralDetail.ReferralSourceID)
                                $scope.ReferralOrganization = item;
                    });
                }
                    bindReferralCategory();
                    getRegistrationDetail(contactID).then(function () {
                        resetForm();
                });
                }
            },
            function (errorStatus) {
                alertService.error('Unable to get referralHeader: ' + errorStatus);
            });
        };
        var bindReferralCategory = function () {
            var referralCategorySource = $rootScope.getLookupsByType($scope.ReferralCategorySourceLookUpType);
            if ($scope.ReferralDetail && $scope.ReferralDetail.ReferralCategorySourceID) {
                $scope.ReferralDetail.ReferralCategoryID = $filter('filter')(referralCategorySource, { ID: $scope.ReferralDetail.ReferralCategorySourceID }, true)[0].CategoryID;
            }
        };

        var getRegistrationDetail = function (contactID) {
            return registrationService.get(contactID).then(function (data) {
                if (hasData(data)) {
                    $scope.ReferralDetail.FirstName = data.DataItems[0].FirstName;
                    $scope.ReferralDetail.LastName = data.DataItems[0].LastName;
                    $scope.ReferralDetail.Middle = data.DataItems[0].Middle;
                    $scope.ReferralDetail.TitleID = data.DataItems[0].TitleID;
                    $scope.ReferralDetail.SuffixID = data.DataItems[0].SuffixID;
                    $scope.ReferralDetail.DriverLicense = data.DataItems[0].DriverLicense;
                    $scope.ReferralDetail.DriverLicenseStateID = data.DataItems[0].DriverLicenseStateID;
                    var contactTypeID = data.DataItems[0].ContactTypeID;
                    $scope.ReferralDetail.ContactID = contactID;
                    $scope.ReferralDetail.ContactTypeID = contactTypeID;
                    getAddress(contactID, contactTypeID);
                    getEmails(contactID, contactTypeID);
                    getPhones(contactID, contactTypeID);
                }
            },
            function (errorStatus) {
                $scope.isLoading = false;
            });
        };
        var getConcerns = function (referralAdditionalDetailID) {
            if ($scope.isECI) {
                initConcerns();
                referralConcernDetailService.getReferralConcernDetail(referralAdditionalDetailID).then(function (data) {
                    if (hasData(data)) {
                        $scope.selectedReferralConcernDetails = data.DataItems;
                        for (var i = 0; i < $scope.selectedReferralConcernDetails.length; i++) {
                            for (var j = 0; j < $scope.ReferralConcernTypeLookUpData.length; j++) {
                                if ($scope.selectedReferralConcernDetails[i].ReferralConcernID == $scope.ReferralConcernTypeLookUpData[j].ID) {
                                    $scope.selectedReferralConcernDetails[i].ReferralConcern = $scope.ReferralConcernTypeLookUpData[j].Name;
                                    break;
                                }
                            }
                        }
                    }
                    else {
                        alertService.error('No referral concern exists');
                    }
                },
                function (errorStatus) {
                    alertService.error('Unable to get referralConcerns: ' + errorStatus);
                });
            }
        };
        var initConcerns = function () {
            $scope.selectedReferralConcernDetails = [];
        };
        $scope.next = function () {
            var nextState = angular.element("xen-workflow-action[data-state-name] > li.list-group-item.active").parents('.work-flow').next().find("xen-workflow-action[data-state-name]");
            if (nextState.length > 0) {
                $timeout(function () {                    
                    $scope.Goto(nextState.attr('data-state-name'), { ContactID: $scope.contactID });
                });
            }
        };

        var initReferralDetail = function () {
            $scope.ReferralDetail = {};
            $scope.ReferralDetail.ReferralDate = $filter('toMMDDYYYYDate')(new Date());
        };

        var initReferralAdditionalDetail = function () {
            $scope.ReferralAdditionalDetails = {};
        };

        /********************************************* Start Email Functions ********************************************/

        var getEmails = function (contactID, contactTypeID) {
            initEmails();
            contactEmailService.get(contactID, contactTypeID).then(function (data) {
                if (hasData(data)) {
                    $scope.Emails = data.DataItems;
                    setAddMinusButtons($scope.Emails);
                }
            })
            .finally(function () {
                resetEmail();
            });
        };
        var initEmails = function () {
            $scope.Emails = [];
            $scope.Emails.push(objEmail());
        };

        var objEmail = function () {
            return {
                Email: '',
                ContactID: 0,
                ContactEmailID: 0,
                EmailPermissionID: null,
                IsPrimary: true,
                ShowPlusButton: true,
                ShowMinusButton: true,
                IsActive: true
            };
        };
        $scope.addNewEmail = function () {
            globalObjectsService.setViewContent();
            $scope.Emails = $filter("filter")($scope.Emails, function (obj) { obj.ShowPlusButton = false; return obj; });
            $scope.Emails.push(objEmail());
        };
        $scope.removeEmail = function (index) {
            globalObjectsService.setViewContent();
            $scope.Emails = removeControl($scope.Emails, index, 'ContactEmailID');
            $scope.Emails = setAddMinusButtons($scope.Emails);
        }

        /* End Email Functions */

        /******************************************** Start Address Functions ***********************************************/

        var getAddress = function (contactID, contactTypeID) {
            initAddress();
            contactAddressService.get(contactID, contactTypeID).then(function (data) {
                if (hasData(data)) {
                    $scope.Addresses = [];
                    $scope.Addresses.push(data.DataItems[0]);
                }
            }).finally(function () {
                resetAddress();
            });
        };

        var initAddress = function () {
            $scope.Addresses = [{
                ContactAddressID: 0,
                ContactID: 0,
                AddressTypeID: null,
                Line1: '',
                Line2: '',
                City: '',
                StateProvince: null,
                County: null,
                Zip: '',
                MailPermissionID: 1,
                IsPrimary: true,
                IsGateCode: false,
                IsComplexName: false
            }];
        };

        /* End Address Functions */

        /******************************************** Start Phone Functions ***********************************************/

        var getPhones = function (contactID, contactTypeID) {
            initPhones();
            contactPhoneService.get(contactID, contactTypeID).then(function (data) {
                if (hasData(data)) {
                    $scope.Phones = data.DataItems;
                    setAddMinusButtons($scope.Phones);
                }
            }).finally(function () {
                resetPhone();
            });
        };
        var initPhones = function () {
            $scope.Phones = [];
            $scope.Phones.push(objPhone());
        };

        var objPhone = function () {
            return {
                Index: null,
                ContactPhoneID: 0,
                ContactID: 0,
                PhoneTypeID: null,
                Number: '',
                PhonePermissionID: null,
                IsPrimary: true,
                ShowPlusButton: true,
                ShowMinusButton: true,
                IsActive: true
            };
        };
        $scope.addNewPhone = function () {
            globalObjectsService.setViewContent();
            $scope.Phones = $filter("filter")($scope.Phones, function (obj) { obj.ShowPlusButton = false; return obj; });
            var priPhones = $filter("filter")($scope.Phones, function (obj) { return (obj.IsPrimary == true); });
            var phoneToAdd = objPhone();
            if (priPhones && priPhones.length > 0)
                phoneToAdd.IsPrimary = false;
            $scope.Phones.push(phoneToAdd);
        };
        $scope.removePhone = function (index) {
            globalObjectsService.setViewContent();
            $scope.Phones = removeControl($scope.Phones, index, 'ContactPhoneID');
            $scope.Phones = setAddMinusButtons($scope.Phones);
        }

        /* End Phone Functions */

        var removeControl = function (collection, index, pkID) {
            var newCollection = $filter('filter')(collection, function (data) {
                return data.IsActive === true;
            });
            var obj = newCollection[index];
            //if (obj.ShowPlusButton == true) {
            //    collection[index - 1].ShowPlusButton = true;
            //}
            // If not saved data then just remove it from collection

            if (eval('obj.' + pkID) === 0) {
                newCollection.splice(index, 1);
            }
            else {
                obj.IsActive = false;
                if (pkID.indexOf('Phone') >= 0) {
                    $scope.isPhoneDirty = true;
                }
                else if (pkID.indexOf('Email') >= 0) {
                    $scope.isEmailDirty = true;
                }
            }
            return mergedCollection(collection, newCollection);
        };

        var setAddMinusButtons = function (model) {
            var activeCollection = $filter('filter')(model, function (data) {
                return data.IsActive === true;
            });
            if (activeCollection.length == 1) {
                activeCollection[0].ShowPlusButton = true;
                activeCollection[0].ShowMinusButton = true;
            }
            else if (activeCollection.length > 1) {
                angular.forEach(activeCollection, function (data, index) {
                    if (index == 0) {
                        data.ShowMinusButton = true;
                    }
                    else if (index == activeCollection.length - 1) {
                        data.ShowMinusButton = true;
                        data.ShowPlusButton = true;
                    }
                    else
                        data.ShowMinusButton = true;
                });
            }
            return mergedCollection(model, activeCollection);
        };
        var mergedCollection = function (collection, activeCollection) {
            var inActiveCollection = $filter('filter')(collection, function (data) {
                return data.IsActive === false;
            });
            if (inActiveCollection.length > 0) {
                // remove obj from activeCollection which exists in inActiveCollection
                activeCollection = activeCollection.filter(function (val) {
                    return inActiveCollection.indexOf(val) == -1;
                });
                activeCollection = activeCollection.concat(inActiveCollection);
            }
            return activeCollection;
        }

        /******************************Save form data ******************************/
        $scope.save = function (isNext, mandatory, hasErrors, keepForm) {
            if ($scope.ReferralDetail.IsReferrerConvertedToCollateral) {
                //Verify unique identification                
                if (($scope.Phones.length == 0 || ($scope.Phones.length > 0 && !$scope.Phones[0].Number)) &&
                    ($scope.Emails.length == 0 || ($scope.Emails.length > 0 && !$scope.Emails[0].Email)) &&
                    (!$scope.ReferralDetail.DriverLicense || ($scope.ReferralDetail.DriverLicense && $scope.ReferralDetail.DriverLicense.length == 0))) {
                    alertService.error('At least one form of unique identification is required: Driver License, Phone Number or Email.');
                    return;
                }
            }

            if ($scope.ReferralOrganization == undefined || $scope.ReferralOrganization == null) {
                isSaving = false;
                alertService.error("Please select proper value in " + $scope.ReferralSource + " field.");
            }

            var activeReferralConcerns = $filter('filter')($scope.selectedReferralConcernDetails, function (item) {
                return item.IsActive;
            });
            $scope.ReferralDetail.ReferralDate = $filter('toMMDDYYYYDate')($scope.ReferralDetail.ReferralDate);

            if (!isSaving) {
                var isMainDirty = formService.isDirty();
                var isDirtyDetail = formService.isDirty($scope.ctrl.referralMainForm.referralDetailForm.$name);
                if ((isMainDirty && !hasErrors) || $scope.ReferralConcernDetailsChanged || $scope.isEmailDirty || $scope.isPhoneDirty) {
                    //Validate the Referral concerns if the form is dirty
                    setConcernErrorClass();
                    if (activeReferralConcerns.length == 0 && $scope.isECI) {
                        alertService.error('Please select atleast one Referral Concern');
                        return;
                    }
                    
                    gotoNext = isNext;
                    isSaving = true;
                    (!$scope.ReferralDetail.ContactID) ? addRegistrationData(isDirtyDetail) : (isDirtyDetail ? updateRegistrationData(isDirtyDetail) : successMethod({ data: { ID: $scope.ReferralDetail.ReferralHeaderID } }));
                }
                else if (!isDirtyDetail && isNext) {
                    checkFormStatus(false);
                    $scope.next();
                }
            }
        };
        var addContactRelationships = function (contactId) {
            if ($scope.ReferralDetail.IsReferrerConvertedToCollateral && hasDetails($scope.ContactRelationships)) {
                $scope.ContactRelationships[0].ContactID = contactId;
                $scope.ContactRelationships[0].ContactRelationshipTypeID = isECIClient ? CONTACT_TYPE.Family_Relationship : CONTACT_TYPE.Collateral;
                $scope.ContactRelationships[0].ParentContactID = $stateParams.ContactID;
                $scope.ContactRelationships[0].LivingWithClientStatus = $scope.ReferralDetail.ContactRelationShip.LivingWithClientStatus;
                $scope.ReferralDetail.ContactRelationShip = $scope.ContactRelationships[0];
            }
            else {
                delete $scope.ReferralDetail.ContactRelationShip
            }
        }
        var addRegistrationData = function (isDirtyDetail) {
            if ($scope.ReferralOrganization == undefined || $scope.ReferralOrganization == null) {
                isSaving = false;
                return;
            }
            if (isDirtyDetail) {
                $scope.ReferralDetail.ContactID = 0;
                $scope.ReferralDetail.ContactTypeID = contactReferralTypeID;
                $scope.ReferralDetail.ReferralSourceID = $scope.ReferralOrganization.ID;
                $scope.ReferralDetail.ReferralStatusID = closedReferralStatusId;
                $scope.ReferralDetail.ReferralTypeID = externalReferralTypeId;
                registrationService.add($scope.ReferralDetail).then(function (response) {
                    if (response && response.data.ID != 0 && response.data.ResultCode === 0) {
                        $scope.ReferralDetail.ContactID = response.data.ID;
                        addReferralHeaderDetail(isDirtyDetail);
                    }
                    else {
                        isSaving = false;
                        alertService.error('OOPS! Something went wrong with saving Referral Contact');
                    }
                }, errorMethod, notifyMethod);
            }
            else {
                isSaving = false;
                alertService.error('Nothing to add in Referral Contact');
            }
        }

        var updateRegistrationData = function (isDirtyDetail) {
            if (isDirtyDetail) {
                if ($scope.ReferralDetail.ContactID == 0) {
                    isSaving = false;
                    alertService.error('Referral Contact is not available to update');
                    return;
                }
                if (!$scope.ReferralDetail.ContactTypeID)
                    $scope.ReferralDetail.ContactTypeID = contactReferralTypeID;
                $scope.ReferralDetail.ReferralSourceID = $scope.ReferralOrganization.ID;
                $scope.ReferralDetail.ReferralStatusID = closedReferralStatusId;
                $scope.ReferralDetail.ReferralTypeID = externalReferralTypeId;
                registrationService.update($scope.ReferralDetail).then(function (response) {
                    if (response && response.data.ResultCode === 0) {
                        $scope.ReferralDetail.ReferralHeaderID ? updateReferralHeaderDetail(isDirtyDetail) : addReferralHeaderDetail(isDirtyDetail);
                    }
                    else {
                        isSaving = false;
                        alertService.error('OOPS! Something went wrong with updating Referral Contact');
                    }
                }, errorMethod, notifyMethod);
            }
            else {
                isSaving = false;
                alertService.error('Nothing to add in Referral Contact');
            }
        };
        var addReferralHeaderDetail = function (isDirtyDetail) {
            if (isDirtyDetail) {
                $scope.ReferralDetail.ReferralHeaderID = 0;
                addContactRelationships($scope.ReferralDetail.ContactID);
                referralHeaderService.add($scope.ReferralDetail).then(function (responseHeaderData) {
                    $scope.ReferralDetail.ReferralHeaderID = responseHeaderData.ID;
                    addReferralAdditionalDetail(isDirtyDetail);
                }, errorMethod, notifyMethod);
            } else {
                isSaving = false;
                alertService.error("Nothing to add in Referral Detail");
            }
        };

        var updateReferralHeaderDetail = function (isDirtyDetail) {
            if (isDirtyDetail) {
                addContactRelationships($scope.ReferralDetail.ContactID);
                referralHeaderService.update($scope.ReferralDetail).then(function (responseHeaderData) {
                    updateReferralAdditionalDetail(isDirtyDetail);
                }, errorMethod, notifyMethod);
            } else {
                isSaving = false;
                alertService.error('Nothing to add in Referral Detail');
            }
        };
        var addReferralAdditionalDetail = function (isDirtyDetail) {
            if (isDirtyDetail) {                
                if (!$scope.ReferralAdditionalDetails)
                    $scope.ReferralAdditionalDetails = {};
                $scope.ReferralAdditionalDetails.ReferralHeaderID = $scope.ReferralDetail.ReferralHeaderID;
                $scope.ReferralAdditionalDetails.HeaderContactID = $scope.ReferralDetail.ContactID;
                $scope.ReferralAdditionalDetails.ContactID = $stateParams.ContactID;
                referralAdditionalDetailService.add($scope.ReferralAdditionalDetails).then(successMethod, errorMethod, notifyMethod);
            }
            else {
                isSaving = false;
                alertService.error('Nothing to add in Referral Detail');
            }
        };

        var updateReferralAdditionalDetail = function (isDirtyDetail) {
            if (isDirtyDetail) {
                if (!$scope.ReferralAdditionalDetails)
                    $scope.ReferralAdditionalDetails = {};
                $scope.ReferralAdditionalDetails.ReferralHeaderID = $scope.ReferralDetail.ReferralHeaderID;
                $scope.ReferralAdditionalDetails.HeaderContactID = $scope.ReferralDetail.ContactID;
                $scope.ReferralAdditionalDetails.ContactID = $stateParams.ContactID;
                referralAdditionalDetailService.update($scope.ReferralAdditionalDetails).then(successMethod, errorMethod, notifyMethod);
            }
            else {
                isSaving = false;
                alertService.error('Nothing to add in Referral Detail');
            }
        };
        var successMethod = function (response) {

            var isAdd = (!$scope.ReferralAdditionalDetails.ReferralAdditionalDetailID || $scope.ReferralAdditionalDetails.ReferralAdditionalDetailID === 0);

            if (response && (response.ID != 0 || $scope.ReferralAdditionalDetails.ReferralAdditionalDetailID > 0)) {
                $scope.permissionId = $scope.ReferralAdditionalDetails.ReferralHeaderID;
                if (isAdd) {
                    $scope.ReferralAdditionalDetails.ReferralAdditionalDetailID = response.ID;
                    $scope.ReferralAdditionalDetailID = response.ID;
                }

                var deferred = $q.defer();
                var referralContactID = $scope.ReferralDetail.ContactID;
                var isDirtyPhone = formService.isDirty($scope.ctrl.referralMainForm.referralPhoneForm.$name);
                var isDirtyEmail = formService.isDirty($scope.ctrl.referralMainForm.referralEmailForm.$name);
                var isDirtyAddress = formService.isDirty($scope.ctrl.referralMainForm.referralAddressForm.$name);
                var taskArray = [];

                // filter out blank concern
                var concerns = $filter('filter')($scope.selectedReferralConcernDetails, function (item) {
                    if (item.ReferralConcernID !== 0) {
                        return true;
                    }
                    else {
                        return false;
                    }
                });

                if (($scope.ReferralConcernDetailsChanged) && hasDetails(concerns)) {

                    angular.forEach(concerns, function (concern, i) {
                        concern.ReferralAdditionalDetailID = $scope.ReferralAdditionalDetailID;
                        if (concern.ReferralConcernDetailID == 0) {
                            taskArray.push([referralConcernDetailService.add, [concern]]);
                        }
                        else if (concern.IsActive == false && concern.ReferralConcernDetailID && concern.ReferralConcernDetailID != 0) {
                            taskArray.push([referralConcernDetailService.remove, [concern.ReferralConcernDetailID, concern.ReferralAdditionalDetailID]]);
                        }
                    });
                }

                // filter out blank address
                var addresses = $filter('filter')($scope.Addresses, function (item) {
                    if (item.ContactAddressID > 0 || item.AddressTypeID != null || item.Line1 != '' || item.Line2 != '' || item.City != '' || item.StateProvince != null || item.County != null || item.Zip != '') {
                        return true;
                    }
                    else {
                        return false;
                    }
                });

                if (isDirtyAddress && addresses != undefined && addresses != null && addresses.length > 0) {
                    angular.forEach($scope.Addresses, function (address) {
                        address.ContactID = referralContactID;
                        taskArray.push([contactAddressService.addUpdate, [address]]);
                    });
                }

                // filter out blank email
                var emails = $filter('filter')($scope.Emails, function (item) {
                    if (item.Email != '' || (item.ContactEmailID != null && item.ContactEmailID > 0)) {
                        return true;
                    }
                    else {
                        return false;
                    }
                });
                if ((isDirtyEmail || $scope.isEmailDirty) && emails != undefined && emails != null && emails.length > 0) {
                    angular.forEach(emails, function (email) {
                        email.ContactID = referralContactID;
                        if (email.Email != '' || email.ContactEmailID > 0) {
                            if (email.ContactEmailID > 0 && (email.Email == '' || email.Email == undefined || email.Email == null))
                                email.IsActive = false;
                            if (email.IsActive == true)
                                taskArray.push([contactEmailService.addUpdate, [email]]);
                            else if (email.ContactEmailID != 0)
                                taskArray.push([contactEmailService.remove, [email.ContactEmailID, email.ContactID]]);
                        }
                    });
                }

                // filter out blank phone
                var phones = $filter('filter')($scope.Phones, function (item) {
                    if (item.Number != '' || (item.ContactPhoneID != null && item.ContactPhoneID > 0)) {
                        return true;
                    }
                    else {
                        return false;
                    }
                });
                if ((isDirtyPhone || $scope.isPhoneDirty) && phones != undefined && phones != null && phones.length > 0) {
                    angular.forEach(phones, function (phone) {
                        phone.ContactID = referralContactID;
                        if (phone.Number != '' || phone.ContactPhoneID > 0) {
                            if (phone.ContactPhoneID > 0 && (phone.Number == '' || phone.Number == undefined || phone.Number == null))
                                phone.IsActive = false;
                            if (phone.IsActive == true) {
                                taskArray.push([contactPhoneService.save, [phone]]);
                            }
                            else if (phone.ContactPhoneID != 0) {
                                taskArray.push([contactPhoneService.remove, [phone.ContactID, phone.ContactPhoneID]]);
                            }
                        }
                    });
                }

                $q.serial(taskArray).then(function () {
                    deferred.resolve();
                    alertService.success('Referral has been ' + (isAdd ? 'added' : 'updated') + ' successfully.');
                    resetForm();
                    checkFormStatus(true);
                    //If convert to collateral successful then collateral state should also broadcast
                    if ($scope.ReferralDetail.IsReferrerConvertedToCollateral)
                        $rootScope.$broadcast('registration.collateral', { validationState: 'valid' });

                    if (gotoNext) {
                        $scope.next();
                    } else {
                        $scope.getReferralDetails();
                        $scope.init();
                    }
                    $scope.ReferralConcernDetailsChanged = false;
                    $timeout(function () { isSaving = false; });
                },
                function (error) {
                    $scope.isSaving = false;
                    alertService.error('OOPs something went wrong with Referral Contact Details' + error);
                    deferred.reject();
                });

                return deferred.promise;
            }
            else {
                isSaving = false;
                alertService.error('Unable to ' + (isAdd ? 'add' : 'update') + ' Referral Details');
                return;
            }
        };
        var errorMethod = function (err) {
            isSaving = false;
            alertService.error('OOPs something went wrong');
        };

        var notifyMethod = function (notify) {
            isSaving = false;
            alertService.warning(notify);
        };

        $scope.init();
        $scope.contactID = $stateParams.ContactID;
        if ($scope.contactID != 0) {
            $scope.getReferralDetails();
        } else {
            resetForm();
        }
        var previousData = {};

        var getPreviouData = function () {
            previousData = {
                ReferralDetail: angular.copy($scope.ReferralDetail),
                Phones: angular.copy($scope.Phones),
                Addresses: angular.copy($scope.Addresses),
                Emails: angular.copy($scope.Emails)
            };
        };
        
        $scope.onContactSelect = function (selectedContactID) {
            $scope.LinkedContactID = true;
            linkedContactID = selectedContactID;
            $scope.ReferralDetail.IsLinkedToContact = true;
            //Verify if already converted to collateral
            var items = $filter('filter')(referralData, { ContactID: selectedContactID, IsReferrerConvertedToCollateral: true });
            $scope.convertToCollateral = !hasDetails(items);
            getPreviouData();
            getRegistrationDetail(selectedContactID);
        }

        $scope.setShortcutKey = function (enterKeyStop, stopNext, saveOnEnter, stopSave) {
            $scope.enterKeyStop = enterKeyStop;
            $scope.stopNext = stopNext;
            $scope.saveOnEnter = saveOnEnter;
            $scope.stopSave = stopSave;
        };

        $scope.setFocus = function (autoFocus) {
            $rootScope.setFocusToGrid('contactsTable');
            if (autoFocus) {
                $('#txtClientSearch').focus();
                $scope.setShortcutKey(true, false, false, true);
            }
        };

        $scope.setCancelFunction = function (cancelFunction) {
            if (cancelFunction)
                $scope.cancel = cancelFunction;
        };

        $scope.GetLinkedContact = function () {
            if (!$scope.ReferralDetail.IsLinkedToContact) {
                getPreviouData();
                getRegistrationDetail(linkedContactID);
            }
            else {
                $scope.ReferralDetail.FirstName = previousData.ReferralDetail.FirstName;
                $scope.ReferralDetail.LastName = previousData.ReferralDetail.LastName;
                $scope.ReferralDetail.Middle = previousData.ReferralDetail.Middle;
                $scope.ReferralDetail.TitleID = previousData.ReferralDetail.TitleID;
                $scope.ReferralDetail.SuffixID = previousData.ReferralDetail.SuffixID;
                $scope.Phones = previousData.Phones;
                $scope.Addresses = previousData.Addresses;
                $scope.Emails = previousData.Emails;
            }
        }
    }
])
}());