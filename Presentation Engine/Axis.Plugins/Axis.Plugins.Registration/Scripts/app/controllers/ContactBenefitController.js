angular.module('xenatixApp')
    .controller('ContactBenefitController', ['$scope', '$modal', '$filter', 'contactBenefitService', 'alertService', 'settings', 'lookupService', '$stateParams', '$state', '$rootScope', 'formService', 'collateralService', 'registrationService', 'collateralContactTypeID', 'contactRelationshipService', 'contactAddressService', '$timeout', '$q', 'roleSecurityService',
function ($scope, $modal, $filter, contactBenefitService, alertService, settings, lookupService, $stateParams, $state, $rootScope, formService, collateralService, registrationService, collateralContactTypeID, contactRelationshipService, contactAddressService, $timeout, $q, roleSecurityService) {

    $scope.isLoading = true;
    $scope.$parent['autoFocus'] = true; //for Focus
    $scope.RetroOpened = false;
    $scope.ExpirationOpened = false;
    $scope.effectiveOpened = false;
    var expirationDate = 'ExpirationDate';
    var effectiveDate = 'EffectiveDate'
    $scope.payors = lookupService.getLookupsByType('Payor');
    $scope.otherPayorExpirationReasonOption = PAYOR_EXPIRATION_REASON.Other;
    $scope.dateOptions = {
        formatYear: 'yy',
        startingDay: 1,
        showWeeks: false
    };
    $scope.DisableLinkedDetails = true;
    $scope.searchText = '';
    $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'MM/dd/yyyy'];
    $scope.payorRankList = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
    $scope.format = $scope.formats[3];
    $scope.expirationStartDate = new Date();
    $scope.contactID = $stateParams.ContactID;
    $scope.policyHolderList = [];
    $scope.enablePHolder = false;
    $scope.enableBillingContactSection = true; //[Bug:11977]
    var ageLimit = 120; //TODO: should be configurable
    $scope.controlsVisible = true;
    $scope.endDate = new Date();
    $scope.startDate = $filter('calculate120years')();
    var _isNext;
    var _mandatory;
    var _hasErrors;
    var payorsTable = $("#payorsTable");
    var familyGroupID = RELATIONSHIP_TYPE_GROUPID.Family;
    $scope.permissionKey = $state.current.data.permissionKey;
    $scope.noID = 2;
    $scope.yesID = 1;
    var otherpayor = false;
    var otherID = 0;
    var finalSave = false;
    var isPatientProfile = false;
    var resetForm = function () {
        $rootScope.formReset($scope.ctrl.payorDetailsForm);
        $rootScope.formReset($scope.ctrl.policyHolderForm, 'ctrl.policyHolderForm');
        if ($scope.ctrl.payorDetailsForm) {
            $scope.ctrl.payorDetailsForm.modifiedModels = [];
        }
    };
    var contactHasAddress = false;
    var benefitsTable = $("#benefitsTable");
    $scope.payorDemography = {};
    $scope.validateAge = function () {
        if ($scope.payorDemography.DOB != null) {
            var date = new Date($scope.payorDemography.DOB);
            if (date <= $scope.endDate) {
                $scope.payorDemography.Age = parseInt($filter('toAge')($scope.payorDemography.DOB));
                var isDateMaxLimit = $filter('isDateMaxLimit')($scope.payorDemography.DOB, ageLimit);
                if (isDateMaxLimit) {
                    $scope.payorDemography.Age = $scope.payorDemography.DOB = null;
                    alertService.error("Age Can't Be greater than " + ageLimit + " years.");
                }
                $('#pdoberrortd').removeClass('has-error');
                $('#pdoberror').addClass('ng-hide');
            }
            else {
                $('#pdoberror').removeClass('ng-hide');
                $('#pdoberrortd').addClass('has-error');
                $scope.ctrl.policyHolderForm.$valid = false;
            }
        }
    };

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
                    field: 'PayorName',
                    title: 'Payor'
                },
                {
                    field: 'GroupName',
                    title: 'Group Name'
                },
                {
                    field: 'PlanName',
                    title: 'Plan Name'
                },
                {
                    field: 'PolicyHolderName',
                    title: 'Policy Holder',
                    formatter: function (value, row) {
                        var policyHolderName = row.PolicyHolderFirstName + (row.PolicyHolderMiddleName ? ' ' + row.PolicyHolderMiddleName : '') + (row.PolicyHolderLastName ? ' ' + row.PolicyHolderLastName : '') + (row.PolicyHolderSuffixID ? ' ' + lookupService.getText("Suffix", row.PolicyHolderSuffixID) : '');
                        var isSelf = row.ContactID == row.PolicyHolderID;
                        return isSelf ? "Self (" + policyHolderName + ")" : policyHolderName;
                    }
                },
                {
                    field: 'EffectiveDate',
                    title: 'Effective Date',
                    formatter: function (value, row, index) {
                        if (value) {
                            //toMMDDYYYYDate -> formateDate because its a date only field
                            var formattedDate = $filter('formatDate')(value, 'MM/DD/YYYY');
                            return formattedDate;
                        } else
                            return '';
                    }
                },
                {
                    field: 'ExpirationDate',
                    title: 'Expiration Date',
                    formatter: function (value, row, index) {
                        if (value) {
                            //toMMDDYYYYDate -> formateDate because its a date only field
                            var formattedDate = $filter('formatDate')(value, 'MM/DD/YYYY');
                            return formattedDate;
                        } else
                            return '';
                    }
                },
                {
                    field: 'ContactPayorID',
                    title: '',
                    formatter: function (value, row, index) {
                        var hasUpdatePermission = roleSecurityService.hasPermission($scope.permissionKey, PERMISSION.UPDATE);
                        return (hasUpdatePermission ? '<a href="javascript:void(0)" data-default-action security permission-key="{{permissionKey}}" permission="update" id="editBenefit" name="editBenefit" data-toggle="modal" ng-click="edit(' + value + ')" title="Edit" space-key-press><i class="fa fa-pencil fa-fw" /></a>'
                               : '<a href="javascript:void(0)" data-default-action security permission-key="{{permissionKey}}" permission="read" id="viewBenefit" name="viewBenefit" data-toggle="modal" ng-click="edit(' + value + ')" title="View" space-key-press><i class="fa fa-eye fa-fw" /></a>')
                            +'<a href="javascript:void(0)" data-default-no-action ng-click="remove(' + value + ',$event)" security permission-key="{{permissionKey}}" permission="delete" id="deactivateBenefit" name="deactivateBenefit" title="Deactivate" space-key-press><i class="fa fa-trash fa-fw"></i></a>';
                    }
                }
            ]
        };

        $scope.payorTableoptions = {
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
                    field: 'PayorName',
                    title: 'Payor Name'
                },
                {
                    field: 'PlanName',
                    title: 'Payor Plan'
                },
                {
                    field: 'Line1',
                    title: 'Payor Address',
                    formatter: function (value, row, index) {
                        return ('<span class="fixed-width-col">' + (checkModel(row.Line1) ? row.Line1 + '<br/>' : " ") + (checkModel(row.Line2) ? row.Line2 + '<br/>' : " ") + (checkModel(row.City) ? row.City + ',' : "") + ' ' + (checkModel(row.StateProvinceName) ? row.StateProvinceName + ',' : "") + ' ' + (checkModel(row.County) ? row.County + ',' : "") + ' ' + (checkModel(row.Zip) ? row.Zip : "") + '</span>')
                    }
                },
                {
                    field: 'ElectronicPayorID',
                    title: 'Electronic Payor ID',

                },
                   {
                       field: 'PayorID',
                       title: '',
                       formatter: function (value, row, index) {
                           return '<a href="javascript:void(0)" data-default-action security permission-key="Registration-Registration-Payors" permission="update" id="editBenefit" name="editBenefit" data-toggle="modal" ng-click="selectPayor(' + index + ')" title="View/Edit" space-key-press><i class="fa fa-pencil fa-fw" /></a>'

                       }
                   },
            ]
        };

    };

    $scope.selectPayor = function (indx, row) {
        var tableData = payorsTable.bootstrapTable('getData');
        var obj = tableData[indx];

        $scope.contactBenefit.PayorID = obj.PayorID;
        $scope.contactBenefit.Payor = obj.PayorName;
        $scope.contactBenefit.PayorCode = obj.PayorCode;
        $scope.contactBenefit.PlanName = obj.PlanName;
        $scope.contactBenefit.PlanID = obj.PlanID;
        $scope.contactBenefit.PayorPlanID = obj.PayorPlanID;

        $scope.contactBenefit.ElectronicPayorID = obj.ElectronicPayorID;
        $scope.contactBenefit.PayorAddressID = obj.PayorAddressID;

        $scope.Addresses[0].AddressID = obj.AddressID;
        $scope.Addresses[0].Line1 = obj.Line1;
        $scope.Addresses[0].Line2 = obj.Line2;
        $scope.Addresses[0].City = obj.City;
        $scope.Addresses[0].StateProvince = obj.StateProvince;
        $scope.Addresses[0].County = obj.County;
        $scope.Addresses[0].Zip = obj.Zip;
        $scope.contactBenefit.GroupName = obj.GroupName;
        $scope.contactBenefit.PayorGroupID = obj.PayorGroupID;

        $('#payorListModel').modal('hide');
        $scope.searchText = '';
    }


    var checkContactAddress = function () {
        contactAddressService.get($scope.contactID).then(function (addressData) {
            if (hasData(addressData)) {
                contactHasAddress = true;
            }
        });
    };

    //Get the contact detail based on the search text
    $scope.getPayors = function (searchText) {
        if (searchText != undefined && searchText != null && searchText != '') {

            contactBenefitService.getPayors(searchText).then(function (data) {
                $scope.payorList = data.DataItems;

                if ($scope.payorList != null && $scope.payorList.length > 0) {

                    $('#payorListModel').on('hidden.bs.modal', function () {
                        $scope.stopEnterKey();
                        var focus = $('#PayorName').is(":focus");
                        if (!focus) {
                            $('#txtClientSearch').focus();
                        }
                    })
                    if ($scope.payorList != null && $scope.payorList.length > 0) {
                        payorsTable.bootstrapTable('load', $scope.payorList);
                        payorsTable.bootstrapTable('selectPage', 1);
                        $('#payorListModel').modal('show');
                        $('#payorListModel').on('shown.bs.modal', function () {
                            $scope.resetEnterKey();
                            $rootScope.setFocusToGrid('payorsTable');
                        })
                        $('#payorsTable').on('all.bs.table', function (e, name, args) {
                            $('.fixed-table-body').scrollLeft(0);
                        })

                    }
                    else {
                        payorsTable.bootstrapTable('removeAll');
                    }
                }
                else {
                    alertService.warning("No matching records found");
                    payorsTable.bootstrapTable('removeAll');
                }
            }, function (errorStatus) {
                alertService.error('Unable to connect to server');
            });
        } else {
            $scope.stopEnterKey();
        }
    };


    $scope.closeModel = function () {

        $('#payorListModel').modal('hide');
        $scope.searchText = '';
    };


    $scope.stopEnterKey = function () {
        if (!$('#contactListModel').hasClass('in')) {
            if ($state.current.name.toLowerCase().indexOf('patientprofile') >= 0) {
                $scope.enterKeyStop = true;
                $scope.stopNext = false;
                $scope.saveOnEnter = true;
            }
            else {
                $scope.enterKeyStop = false;
                $scope.stopNext = false;
                $scope.saveOnEnter = true;
            }
        }
        else
            $scope.resetEnterKey();
    }

    $scope.resetEnterKey = function () {
        $scope.enterKeyStop = true;
        $scope.stopNext = false;
        $scope.saveOnEnter = false;
    }


    $scope.getLookupsByType = function (typeName) {
        return lookupService.getLookupsByType(typeName);
    };

    $scope.initAddresses = function () {
        $scope.Addresses = [{
            AddressID: null,
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
            IsComplexName: false,
            HideType: true
        }];
    };

    $scope.initContactBenefit = function () {
        $scope.PayorPlanID = null;
        $scope.cardNameMatchList = [{ ID: $scope.yesID, Name: 'Yes' }, { ID: $scope.noID, Name: 'No' }];
        $scope.contactBenefit = { HasSameCardName: null, Address: '', HasPolicyHolderSameCardName: $scope.yesID };
        $scope.contactBenefit.ContactPayorID = 0;
        $scope.$parent['autoFocus'] = true;
        $scope.initAddresses();
        $scope.resetGroupPlan();
        checkContactAddress();
        resetForm();

        if ($state.current.name.toLowerCase().indexOf('patientprofile') >= 0) {
            $scope.controlsVisible = false;
            $scope.enterKeyStop = true;
            $scope.stopNext = false;
            $scope.saveOnEnter = true;
            isPatientProfile = true;
        }
        else {
            $scope.controlsVisible = true;
            $scope.enterKeyStop = false;
            $scope.stopNext = false;
            $scope.saveOnEnter = false;
            isPatientProfile = false;
            $timeout(function () {
                var nextState = angular.element("xen-workflow-action[data-state-name] > li.list-group-item.active").parents('.work-flow').next().find("xen-workflow-action[data-state-name]");
                $scope.controlsVisible = (nextState.length > 0);
            });
        }
    };
    
    var getPolicyHolder = function () {
        $scope.policyHolderList = [];
        var deferred = $q.defer();
        var promiseArray = [];
        collateralService.get($scope.contactID, collateralContactTypeID, false).then(function (data) {
            model = data.DataItems;
            if (!checkPolicyHolderExists($scope.contactID))
                $scope.policyHolderList.push(policyHolder("Self", $scope.parentContact, contactHasAddress));
            if (hasData(data)) {
                angular.forEach(model, function (item) {
                    if (!checkPolicyHolderExists(item.ContactID))
                        promiseArray.push(getContactRelationShip(item));
                });
                $q.all(promiseArray).finally(function (data) {
                    deferred.resolve();
                    $scope.policyHolderList;
                });
            }
            else
                deferred.resolve();
        });
        return deferred.promise;
    };

    var getContactRelationShip = function (item) {
        return contactRelationshipService.get(item.ContactID, $scope.contactID).then(function (relationshipData) {
            if (hasData(relationshipData)) {
                var familyGroupRelations = $filter('filter')(filterFutureOrExpiredRecords(relationshipData.DataItems, expirationDate, effectiveDate), function (obj) {
                    return obj.RelationshipGroupID === familyGroupID;
                }, true);
                if (familyGroupRelations && familyGroupRelations.length)
                    $scope.policyHolderList.push(policyHolder(lookupService.getText("RelationshipType", familyGroupRelations[0].RelationshipTypeID), item, item.Addresses.length > 0));
            }
        });
    }

    var policyHolder = function (relation, model, hasAddress) {
        return { ID: model.ContactID, Name: relation + " (" + model.FirstName + " " + model.LastName + ") ", colName: name, Relation: relation, FirstName: model.FirstName, LastName: model.LastName, Middle: model.Middle, SuffixID: model.SuffixID, HasAddress: hasAddress, HasDob: model.DOB ? true : false };
    };

    var checkPolicyHolderExists = function (policyID) {
        var policyHolder = $filter('filter')($scope.policyHolderList, function (item) {
            return item.ID == policyID;
        });
        return policyHolder.length > 0;
    }

    $scope.get = function (contactId) {
        $scope.isLoading = true;

        var deferred = $q.defer();

        $scope.initContactBenefit();

        return contactBenefitService.get(contactId).then(function (data) {
            $scope.contactBenefitList = data.DataItems;
            if ($scope.contactBenefitList != null) {
                var sortedData = ($filter('orderBy')($scope.contactBenefitList, 'ModifiedOn', true));
                benefitsTable.bootstrapTable('load', sortedData);
                $rootScope.$broadcast('updateRegistrationData', { Type: 'payor', Data: $scope.contactBenefitList });
            } else {
                benefitsTable.bootstrapTable('removeAll');
            }

            //-----------------------------------------------------
            //to be changed when contact name will come from header
            //-----------------------------------------------------

            //if (data.ResultMessage == "OFFLINE" && data.AdditionalResult == undefined) {
            //    registrationService.get(contactId).then(function (contactDemographic) {
            //        if ((contactDemographic.DataItems != null) && (contactDemographic.DataItems.length > 0)) {
            //            var model = contactDemographic.DataItems[0];
            //            if (model != undefined && model != null) {
            //                $scope.ContactFullName = model.FirstName + " " + model.LastName;
            //            }
            //        }
            //    });
            //}
            //else {
            //    $scope.ContactFullName = data.AdditionalResult;
            //}
            registrationService.get(contactId).then(function (contactDemographic) {
                if (hasData(contactDemographic))
                    $scope.parentContact = contactDemographic.DataItems[0];
            });


            $scope.isLoading = false;
            resetForm();
            //checkFormStatus();
            $rootScope.$broadcast($state.current.name,
                     {
                         validationState: (($scope.contactBenefitList != null && Object.keys($scope.contactBenefitList).length > 0) ? 'valid' : 'warning')
                     });
            if ($state.current.name.toLowerCase().indexOf('patientprofile') > -1) {
                $scope.$parent.getPatientProfileData();
            }
            getPolicyHolder().then(function () {
                return deferred.resolve();
            });
            return deferred.promise;
        },
        function (errorStatus) {
            $scope.isLoading = false;
            alertService.error('Unable to connect to server');
        });
    };

    var checkFormStatus = function () {
        $scope.$watch('ctrl.contactForm.$valid', function (newValue) {
            if (newValue !== undefined)
                $rootScope.$broadcast($state.current.name,
                    {
                        validationState: (($scope.contactBenefitList != null && Object.keys($scope.contactBenefitList).length > 0) ? 'valid' : 'warning')
                    });
        });
    };

    $scope.getGroupsAndAddressesForPlan = function (planID) {
        $scope.isLoading = true;
        $scope.groupsPlanList = null;
        return contactBenefitService.getGroupsAndAddressesForPlan(planID).then(function (data) {
            $scope.initAddresses();
            if (data.DataItems != null && data.DataItems.length > 0) {
                $scope.groupsPlanList = data.DataItems[0].PlanGroups;
                if (data.DataItems[0].PayorAddresses.length > 0) {
                    $scope.Addresses = data.DataItems[0].PayorAddresses;
                    $scope.Addresses[0].HideType = true;
                }
            }
            $scope.isLoading = false;
        },
        function (errorStatus) {
            $scope.isLoading = false;
            alertService.error('Unable to connect to server');
        });
    }

    $scope.getPayorPlans = function (payorID) {
        $scope.isLoading = true;
        var payor = $filter('filter')($scope.payors, function (item) {
            return item.ID == payorID;
        })[0];
        $scope.contactBenefit.PayorCode = payor.PayorCode;
        return contactBenefitService.getPayorPlans(payorID).then(function (data) {
            $scope.initAddresses();
            if (data.DataItems != null && data.DataItems.length > 0) {
                $scope.payorPlans = data.DataItems;
            }
            $scope.isLoading = false;
        },
        function (errorStatus) {
            $scope.isLoading = false;
            alertService.error('Unable to connect to server');
        });

    };

    $scope.populatePolicyHolder = function (policyHolderID) {
        if (policyHolderID != null) {
            $scope.contactBenefit.PolicyHolderRelationship = '';
            $scope.enablePHolder = false;
            var obj = policyHolderInfo(policyHolderID);

            $scope.contactBenefit.PolicyHolderRelationship = obj.Relation;

            $scope.contactBenefit.PolicyHolderFirstName = obj.FirstName;
            $scope.contactBenefit.PolicyHolderLastName = obj.LastName;
            $scope.contactBenefit.PolicyHolderMiddleName = obj.Middle;
            $scope.contactBenefit.PolicyHolderSuffixID = obj.SuffixID;
            $scope.contactBenefit.HasSameCardName = null;

            //[Bug:11977] Billing Contact should be COnditional required and only enter when Policy holder other then self.
            if (obj.Relation.indexOf("Self") > -1) {
                $scope.enableBillingContactSection = false;
                clearBillingValues();
            }
            else {
                $scope.enableBillingContactSection = true;
            }

            $scope.updateCardMatch($scope.contactBenefit.HasSameCardName);
            warningMessage(obj);
        }
        else {
            $scope.contactBenefit.PolicyHolderRelationship = '';
            $scope.contactBenefit.PolicyHolderFirstName = '';
            $scope.contactBenefit.PolicyHolderLastName = '';
            $scope.contactBenefit.PolicyHolderMiddleName = '';
            $scope.contactBenefit.PolicyHolderSuffixID = '';
            $scope.enableBillingContactSection = true;
            clearBillingValues();
        }
    };

    var warningMessage = function (obj) {
        if (!obj.HasAddress && !obj.HasDob) {
            alertService.warning("Policy holder address and DOB needs to be entered");
        }
        else if (!obj.HasAddress) {
            alertService.warning("Policy holder address needs to be entered");
        }
        else if (!obj.HasDob) {
            alertService.warning("Policy holder DOB needs to be entered");
        }
    };



    $scope.setBillingValues = function () {
        if ($scope.contactBenefit.HasSameCardName == $scope.yesID) {
            // [Bug:11978] when choose yes shoudl populate with the Contact name not the Policy contact name
            //$scope.contactBenefit.BillingFirstName = $scope.contactBenefit.PolicyHolderFirstName;
            //$scope.contactBenefit.BillingMiddleName = $scope.contactBenefit.PolicyHolderMiddleName;
            //$scope.contactBenefit.BillingLastName = $scope.contactBenefit.PolicyHolderLastName;
            //$scope.contactBenefit.BillingSuffixID = $scope.contactBenefit.PolicyHolderSuffixID;
            var billingContact = policyHolderInfo($scope.contactID);

            $scope.contactBenefit.BillingFirstName = billingContact.FirstName;
            $scope.contactBenefit.BillingMiddleName = billingContact.Middle;
            $scope.contactBenefit.BillingLastName = billingContact.LastName;
            $scope.contactBenefit.BillingSuffixID = billingContact.SuffixID;
        }
    };


    //Obsolete  Method
    var getPayorSuccess = function (data, policyHolderID, isFinalSave) {
        finalSave = isFinalSave;
        if (data && data.DataItems && data.DataItems.length > 0) {
            $scope.isSSNexists = false;
            $scope.isDOBexists = false;
            data.DataItems = $filter('filter')(data.DataItems, { ContactID: policyHolderID })
            // reset error state
            $('#pdoberrortd').removeClass('has-error');
            $('#pdoberror').addClass('ng-hide');
            //Show popup if SSN or DOB is not there
            $scope.payorDemography = data.DataItems[0];
            $rootScope.formReset($scope.ctrl.policyHolderForm, 'ctrl.policyHolderForm');
            if (!$scope.payorDemography.DOB || !$scope.payorDemography.SSN || ($scope.payorDemography.DOB.length == 0 || $scope.payorDemography.SSN.length == 0)) {
                if ($scope.payorDemography.SSN && $scope.payorDemography.SSN.length >= 0) {
                    $scope.isSSNexists = true;
                }
                if ($scope.payorDemography.DOB && $scope.payorDemography.DOB.length >= 0) {
                    $scope.isDOBexists = true;
                    $scope.payorDemography.DOB = $filter('formatDate')($scope.payorDemography.DOB, 'MM/DD/YYYY');
                    if (isFinalSave) // call comes from save and user has all required field information
                        saveConfirm(_isNext, _mandatory, _hasErrors)
                }
                else {
                    $scope.payorDemography.DOB = null;
                    $('#policyHolderModal').modal('show');
                    $('#policyHolderModal').on('shown.bs.modal', function (data) {
                        $scope.saveMethod = $scope.updatePolicyHolder;
                        if (!isPatientProfile)
                            $scope.enterKeyStop = true;
                    });

                    $('#policyHolderModal').on('hide.bs.modal', function (data) {
                        $scope.saveMethod = null;
                        if (!isPatientProfile)
                            $scope.enterKeyStop = false;
                    });
                }
            }
            else {
                $scope.isSSNexists = true;
                $scope.isDOBexists = true;
                if (isFinalSave) // call comes from save and user has all required field information
                    saveConfirm(_isNext, _mandatory, _hasErrors)
            }

        }
        else {
            alertService.error('Unable to verify Policy Holder details.');
        }
    }

    var errorMethod = function (data) {
        alertService.error('Unable to get Payor details');
    };

    $scope.cancelModel = function () {

        $('#policyHolderModal').modal('hide');
        //$rootScope.onCancel(function (result) {
        //    if (result) {
        //        $('#policyHolderModal').modal('hide');
        //    }
        //});
    };

    var declinedSsnStatus = [1, 2, 3];

    $scope.checkSSN = function () {
        if ($scope.payorDemography.SSN && $scope.payorDemography.SSN != "") {
            var items = $filter('filter')(declinedSsnStatus, function (item) {
                return item == $scope.payorDemography.SSNStatusID;
            });
            if (items.length > 0) {
                alertService.error("Invalid SSN Status because SSN is already provided");
                $scope.payorDemography.SSNStatusID = 0;
                return false;
            }
        }
    }

    $scope.updatePolicyHolder = function () {
        iterateErrorFields($scope.ctrl.policyHolderForm.$error.required, null, 'required', alertService);
        iterateErrorFields($scope.ctrl.policyHolderForm.$error.date, null, 'date', alertService);

        if (formService.isDirty('ctrl.policyHolderForm') && ($scope.ctrl.policyHolderForm.$valid == true)) {
            $scope.payorDemography.DOB = $filter("formatDate")($scope.payorDemography.DOB);
            collateralService.update($scope.payorDemography).then(policyHolderUpdateSuccess, policyHolderUpdateFailure);
        }
    };

    var policyHolderUpdateSuccess = function (data) {
        alertService.success('Policy Holder details has been updated.');
        $rootScope.formReset($scope.ctrl.policyHolderForm, 'ctrl.policyHolderForm');
        $scope.cancelModel();
        $scope.isSSNexists = true;
        $scope.isDOBexists = true;
        if (finalSave) {
            saveConfirm(false, _mandatory, _hasErrors);
        }
    };

    var policyHolderUpdateFailure = function () {
        alertService.error('Unable to update Policy Holder');
    }

    $scope.resetPlanDetails = function () {
        $scope.contactBenefit.PayorPlanID = null;
        $scope.contactBenefit.PlanName = null;
        $scope.contactBenefit.PlanID = null;
        $scope.contactBenefit.PolicyHolderID = "";
        $scope.payorPlans = null;
        $scope.contactBenefit.Address = null;
    };

    $scope.resetGroupDetails = function () {
        $scope.contactBenefit.GroupName = null;
        $scope.contactBenefit.GroupID = null;
        $scope.contactBenefit.PayorGroupID = null;
        $scope.groupsPlanList = null;
    };

    $scope.populateGroupDetails = function (groupModel) {
        $scope.contactBenefit.GroupName = groupModel.GroupName;
        $scope.contactBenefit.GroupID = groupModel.GroupID;
        $scope.contactBenefit.PayorGroupID = groupModel.PayorGroupID;
    };

    $scope.populatePlanDetails = function (planModel) {
        $scope.contactBenefit.PayorPlanID = planModel.PayorPlanID;
        $scope.contactBenefit.PlanName = planModel.PlanName;
        $scope.contactBenefit.PlanID = planModel.PlanID;
    };

    $scope.updateCardMatch = function (val) {
        $scope.setBillingValues();
        if (val == $scope.noID) {
            clearBillingValues();
        }
    };

    var clearBillingValues = function () {
        $scope.contactBenefit.BillingFirstName = '';
        $scope.contactBenefit.BillingMiddleName = '';
        $scope.contactBenefit.BillingLastName = '';
        $scope.contactBenefit.BillingSuffixID = '';


    }

    $scope.resetGroupPlan = function () {
        $scope.resetPlanDetails();
        $scope.resetGroupDetails();
        $scope.contactBenefit.Address = null;
        //TFS#5493
        $scope.contactBenefit.PolicyHolderRelationship = null;
    };

    $scope.add = function (isNext, modelToSave) {
        $scope.isLoading = true;
        contactBenefitService.add(modelToSave)
            .then(
                function (response) {
                    var data = response.data;
                    $scope.initContactBenefit();
                    if (modelToSave.PayorID == null)
                        lookupService.getLatestLookupsByType('Payor');
                    $scope.get($scope.contactID).then(function () {
                        alertService.success('Payor has been added.');
                        if (isNext)
                            $scope.next();
                    });
                },
                function (errorStatus) {
                    $scope.isLoading = false;
                    alertService.error('OOPS! Something went wrong');
                },
                function (notification) {
                    alertService.warning(notification);
                }).then(function () {
                    $scope.isLoading = false;
                });
    };

    $scope.update = function (isNext, modelToSave) {
        $scope.isLoading = true;
        contactBenefitService.update(modelToSave)
            .then(
                function (response) {
                    var data = response.data;
                    $scope.initContactBenefit();
                    if (modelToSave.PayorID == null)
                        lookupService.getLatestLookupsByType('Payor');
                    $scope.get($scope.contactID).then(function () {
                        alertService.success('Payor has been updated.');
                        if (isNext)
                            $scope.next();
                    }
                    );
                },
                function (errorStatus) {
                    $scope.isLoading = false;
                    alertService.error('OOPS! Something went wrong');
                },
                function (notification) {
                    alertService.warning(notification);
                }).then(function () {
                    $scope.isLoading = false;
                });
    };

    var hasSameCard = function (model) {

        //var obj = policyHolderInfo($scope.contactBenefit.PolicyHolderID);
        //return (model.BillingFirstName === obj.FirstName
        //&& model.BillingMiddleName === obj.Middle
        //&& model.BillingLastName === obj.LastName
        //&& model.BillingSuffixID === obj.SuffixID)

        //get contact info
        var billingContact = policyHolderInfo($scope.contactID);

        return (model.BillingFirstName === billingContact.FirstName
       && model.BillingMiddleName === billingContact.Middle
       && model.BillingLastName === billingContact.LastName
       && model.BillingSuffixID === billingContact.SuffixID)
    }


    $scope.edit = function (contactPayorID, index) {
        $scope.selectedID = contactPayorID;
        angular.forEach($scope.contactBenefitList, function (contactBenefit) {
            if (contactBenefit.ContactPayorID == contactPayorID) {
                $scope.contactBenefit = angular.copy(contactBenefit);

                $scope.contactBenefit.ContactPayorID = 0;
                $scope.contactBenefit.AdditionalInformation = contactBenefit.AdditionalInformation;
                $scope.contactBenefit.PayorCode = contactBenefit.PayorCode;
                $scope.contactBenefit.ElectronicPayorID = contactBenefit.ElectronicPayorID;
                $scope.contactBenefit.HasPolicyHolderSameCardName = contactBenefit.HasPolicyHolderSameCardName === true ? $scope.yesID : $scope.noID;

                $scope.contactBenefit.HasSameCardName = hasSameCard(contactBenefit) ? $scope.yesID : $scope.noID;
                $scope.contactBenefit.PolicyHolderRelationship = policyHolderInfo($scope.contactBenefit.PolicyHolderID).Relation;
                $scope.Addresses[0].AddressID = contactBenefit.AddressID;
                $scope.Addresses[0].Line1 = contactBenefit.Line1;
                $scope.Addresses[0].Line2 = contactBenefit.Line2;
                $scope.Addresses[0].City = contactBenefit.City;
                $scope.Addresses[0].StateProvince = contactBenefit.StateProvince;
                $scope.Addresses[0].County = contactBenefit.County;
                $scope.Addresses[0].Zip = contactBenefit.Zip;
                $scope.Addresses[0].HideType = true;
                $scope.contactBenefit.EffectiveDate = (contactBenefit.EffectiveDate && (contactBenefit.EffectiveDate.length > 0)) ? $filter('formatDate')(contactBenefit.EffectiveDate, 'MM/DD/YYYY') : '';
                $scope.contactBenefit.ExpirationDate = (contactBenefit.ExpirationDate && (contactBenefit.ExpirationDate.length > 0)) ? $filter('formatDate')(contactBenefit.ExpirationDate, 'MM/DD/YYYY') : '';
                $scope.contactBenefit.AddRetroDate = (contactBenefit.AddRetroDate && (contactBenefit.AddRetroDate.length > 0)) ? $filter('formatDate')(contactBenefit.AddRetroDate, 'MM/DD/YYYY') : '';

                ////[Bug:11977]
                if ($scope.contactBenefit.PolicyHolderRelationship.indexOf("Self") > -1) {
                    $scope.enableBillingContactSection = false;
                    $scope.cardNameMatchList = [{ ID: $scope.yesID, Name: 'Yes' }, { ID: $scope.noID, Name: 'No' }];
                    $scope.contactBenefit.HasSameCardName = null;
                    clearBillingValues();
                }
                else
                    $scope.enableBillingContactSection = true;

                $scope.contactBenefit.Payor = contactBenefit.PayorName;
                //if ($scope.contactBenefit.PolicyHolderName != undefined && $scope.contactBenefit.PolicyHolderName != '' && $scope.contactBenefit.PolicyHolderID == undefined) {
                //    $scope.contactBenefit.PolicyHolderID = otherID;
                //}
                $scope.$parent['autoFocus'] = true;
                $timeout(function () {
                    $scope.contactBenefit.ContactPayorID = contactBenefit.ContactPayorID;
                });
                resetForm();
                return;
            }
        }, this);

    };

    $scope.remove = function (id) {
        bootbox.confirm("Selected benefit will be deactivated.\n Do you want to continue?", function (result) {
            if (result == true) {
                contactBenefitService.remove($scope.contactID, id).then(function () {
                    $scope.get($scope.contactID).then(function () {
                        alertService.success('Contact Benefit has been deactivated.');
                    });
                },
                    function (errorStatus) {
                        alertService.error('OOPS! Something went wrong');
                    }).then(function () {
                        $scope.$apply();
                    });
            }
        });
    };

    $scope.cancel = function () {
        bootbox.confirm("You will lose the information entered.\n Do you want to continue?", function (result) {
            if (result == true) {
                $scope.initContactBenefit();
                $scope.$apply();
            }
        });
    };

    var policyHolderInfo = function (contactID) {
        var result = $.grep($scope.policyHolderList, function (e) {
            return e.ID == contactID;
        })[0];
        return result;
    }

    $scope.save = function (isNext, mandatory, hasErrors) {
        // if there are errors, the screen is optional, and the user is trying to go to the next screen, don't bother trying to save anything
        // Vipul Singhal - As per Json if user modified the optional screen and click enter then it should display the validation error 
        // and if user don't don any modification then user can move to next screen.
        _isNext = isNext;
        _mandatory = mandatory;
        _hasErrors = hasErrors;

        var policyHolder = policyHolderInfo($scope.contactBenefit.PolicyHolderID);

        if (policyHolder) {
            warningMessage(policyHolder);
            saveConfirm(isNext, mandatory, hasErrors);
        }
        else if (policyHolder == undefined && isNext && !formService.isDirty()) {
            $scope.next();
        }
    };

    $scope.updatePolicyHolderCardMatch = function (val) {
        if (val == $scope.yesID) {
            $scope.populatePolicyHolder($scope.contactBenefit.PolicyHolderID);
        }
    };

    var saveConfirm = function (isNext, mandatory, hasErrors) {
        if (!mandatory && isNext && hasErrors) {
            $scope.next();
        }

        var isDirty = formService.isDirty();

        if ($scope.contactBenefit != null && $scope.contactBenefit.Payor != null) {
            if ($scope.contactBenefit.Payor.ID != null) {
                $scope.contactBenefit.PayorID = $scope.contactBenefit.Payor.ID;
                $scope.contactBenefit.PayorName = $scope.contactBenefit.Payor.Name;
            }
            else
                $scope.contactBenefit.PayorName = $scope.contactBenefit.Payor;
        }

        if (isDirty && !hasErrors) {

            var modelToSave = angular.copy($scope.contactBenefit);

            //if (modelToSave.PolicyHolderID == otherID) modelToSave.PolicyHolderID = null;
            // filter out blank address
            modelToSave.Addresses = $filter('filter')($scope.Addresses, function (item) {
                modelToSave.AddressTypeID = item.AddressTypeID;
                modelToSave.Line1 = item.Line1;
                modelToSave.Line2 = item.Line2;
                modelToSave.City = item.City;
                modelToSave.StateProvince = item.StateProvince;
                modelToSave.County = item.County;
                modelToSave.Zip = item.Zip;
                return;
            });
            //formateDate is a good check here before Saving
            modelToSave.EffectiveDate = $filter('formatDate')(modelToSave.EffectiveDate);
            modelToSave.ExpirationDate = $filter('formatDate')(modelToSave.ExpirationDate);
            modelToSave.AddRetroDate = $filter('formatDate')(modelToSave.AddRetroDate);
            //if (modelToSave.PayorID > 0 && modelToSave.PayorPlanID == '') {
            //    alertService.error('Please select Group Name');
            //    return;
            //}

            modelToSave.HasPolicyHolderSameCardName = modelToSave.HasPolicyHolderSameCardName === $scope.yesID ? 1 : 0;

            if (modelToSave.ExpirationDate == null || modelToSave.ExpirationDate == "" || modelToSave.ExpirationDate == undefined) {
                modelToSave.PayorExpirationReasonID = null;
                modelToSave.ExpirationReason = null;
            }

            if (modelToSave.PayorExpirationReasonID != $scope.otherPayorExpirationReasonOption) {
                modelToSave.ExpirationReason = null;
            }

            modelToSave.ContactID = $scope.contactID;
            if (modelToSave.ContactPayorID !== undefined && modelToSave.ContactPayorID !== 0) {
                $scope.update(isNext, modelToSave);
            }
            else {
                $scope.add(isNext, modelToSave);
            }
        }
        else if (!isDirty && isNext) {
            $scope.next();
        }
    }

    $scope.$watch('noResults', function () {
        if ($scope.noResults) {
            $scope.contactBenefit.PayorCode = '';
        }
    });

    $scope.$watch('contactBenefit.Payor', function () {
        if (!$scope.contactBenefit.Payor) {
            $scope.contactBenefit.PayorCode = '';
        }
    });

    $scope.getLookupsByType = function (typeName) {
        return lookupService.getLookupsByType(typeName);
    };

    $scope.next = function () {
        var nextState = angular.element("xen-workflow-action[data-state-name] > li.list-group-item.active").parents('.work-flow').next().find("xen-workflow-action[data-state-name]");
        if (nextState.length === 0)
            $scope.Goto('^', {
                ContactID: $scope.contactID
            });
        else {
            $scope.Goto(nextState.attr('data-state-name'), {
                ContactID: $scope.contactID
            });
        }
    };

    $scope.checkExpiration = function () {
        if ($scope.contactBenefit) {
            if (!$scope.contactBenefit.ExpirationDate) {
                $scope.contactBenefit.PayorExpirationReasonID = null;
                $scope.contactBenefit.ExpirationReason = null;
            }
        }
    };

    $scope.changeState = function () {
        var gotoState = 'registration.collateral';
        var currentState = $state.current.name.toString().toLowerCase();
        if (currentState.indexOf('patientprofile') >= 0) {
            gotoState = 'patientprofile.general.collateral';
        }
        else if (currentState.indexOf('eciregistration') >= 0) {
            gotoState = 'eciregistration.family';
        }
        $rootScope.Goto(gotoState, { ContactID: $scope.contactID });
    };

    $scope.get($scope.contactID).then(function () {
        $scope.contactBenefit.ContactPayorID = 0;
        if ($stateParams.ContactPayorID) {
            setGridItem(benefitsTable, 'ContactPayorID', $stateParams.ContactPayorID);
        }
    });

    $scope.initializeBootstrapTable();
}]);

