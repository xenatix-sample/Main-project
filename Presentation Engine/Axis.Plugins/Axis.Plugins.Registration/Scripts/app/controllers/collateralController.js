angular.module('xenatixApp')
    .controller('collateralController', ['$scope', '$modal', '$filter', '$timeout', 'collateralService', 'alertService', 'settings', 'lookupService', '$stateParams', '$state', 'clientSearchService', '$rootScope', 'contactAddressService', 'formService', 'contactTypeId', 'globalObjectsService', 'contactSSNService', '$controller', 'contactRelationshipService', '$q', 'roleSecurityService','contactBenefitService',
        function ($scope, $modal, $filter, $timeout, collateralService, alertService, settings, lookupService, $stateParams, $state, clientSearchService, $rootScope, contactAddressService, formService, contactTypeId, globalObjectsService, contactSSNService, $controller, contactRelationshipService, $q, roleSecurityService, contactBenefitService) {

            $controller('baseContactController', { $scope: $scope });
            var editMode = false;
            $scope.isLoading = true;
            $scope.$parent['autoFocus'] = true; //for Focus
            $scope.collateral = {};
            $scope.dateOptions = {
                formatYear: 'yy',
                startingDay: 1,
                showWeeks: false
            };
            $scope.BeginDate = 'BeginDate';
            $scope.EndDate = 'EndDate';
            $scope.effectiveDate = 'EffectiveDate';
            $scope.expirationDate = 'ExpirationDate';
            $scope.permissionKey = $state.current.data.permissionKey;
            $scope.endDate = new Date();
            $scope.contactID = $stateParams.ContactID;
            $scope.ClientTypeID = $stateParams.ClientTypeID;
            var isClientIdentifierDirty = false;
            var ageLimit = 120;
            var contactTypeSearch = '1,4,8';
            var isContactRelationshipDirty = false;
            $scope.endDate = new Date();
            $scope.formats = ['dd-MMMM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'MM/dd/yyyy'];
            $scope.format = $scope.formats[3];
            $scope.emailPattern = /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/i;
            $scope.selIdx = -1;
            $scope.isChecked = false;
            $scope.isPrevChecked = false;
            $scope.contactAddress = [];
            $scope.parentAddressID = 0;
            $scope.disableAddress = false;
            $scope.alreadyChecked = false;
            $scope.dirtyAddress = false;
            $scope.emptyAddress = true;
            $scope.policyHolderValues = [{ Value: true, Name: 'Yes' }, { Value: false, Name: 'No' }];
            $scope.getLookupsByType = function (typeName) {
                return lookupService.getLookupsByType(typeName);
            };
            $scope.otherRelationship = COLLATERAL_RELATIONSHIP_TYPE.OtherRelationship;
            $scope.otherPhysician = COLLATERAL_RELATIONSHIP_TYPE.OtherPhysician;
            $scope.otherProvider = COLLATERAL_RELATIONSHIP_TYPE.OtherProvider;
            var contactsTable = $("#contactsTable");
            var collateralsTable = $("#collateralsTable");
            var demographic;
            $scope.controlsVisible = true;
            $scope.collateralTypeFamily = COLLATERAL_TYPE.Family;
            $scope.dob = "dob";
            $scope.contactBenefitList = {};
            var setAddressRequiredStatus = function (isRequired) {
                $scope.AddressAccessCode = isRequired ? $scope.ADDRESS_ACCESS.Required | $scope.ADDRESS_ACCESS.Line1 | $scope.ADDRESS_ACCESS.City | $scope.ADDRESS_ACCESS.State | $scope.ADDRESS_ACCESS.PostalCode | $scope.ADDRESS_ACCESS.County : 0;
            }

            $scope.setDisableAddress = function () {
                var hasPermission = roleSecurityService.hasPermission($scope.permissionKey, $rootScope.resolvePermission($scope.permissionId));
                if (!hasPermission) {
                    return true;
                }
                else {
                    return false;
                }
            };

            var resetContactRelationship = function () {
                if ($scope.ctrl.contactForm.ContactRelationshipForm)
                    $rootScope.formReset($scope.ctrl.contactForm.ContactRelationshipForm, $scope.ctrl.contactForm.ContactRelationshipForm.$name);                
            }

            $scope.$watch("[isChecked,collateral.ReceiveCorrespondenceID]", function (newValues, oldValues) {
                if (newValues !== oldValues) {
                    $scope.validateCollateralType();
                }

            });

            var setRequiredFields = function (isPolicyHolder) {
                if ($scope.collateral) {
                    if (isPolicyHolder || $scope.collateral.ReceiveCorrespondenceID) {
                        setAddressRequiredStatus(!$scope.isChecked);
                    }
                    else {
                        setAddressRequiredStatus(false);
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
                            field: 'DOB',
                            title: 'Date Of Birth',
                            formatter: function (value, row, index) {
                                if (value) {
                                    var formattedDate = $filter('toMMDDYYYYDate')(value, 'MM/DD/YYYY');
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
                            field: 'ContactID',
                            title: '',
                            formatter: function (value, row, index) {
                                return '<a href="javascript:void(0)" data-default-action data-ng-click="populateContact(' + value + ', ' + row.ContactTypeID + ')" alt="View Contact" security permission-key="Registration-Registration-Collateral" permission="update" title="Edit"><i class="fa fa-pencil fa-fw padding-left-small padding-right-small" /></a>';
                            }
                        }
                    ]
                };

                $scope.collateralTableoptions = {
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
                            field: 'CollateralTypes',
                            title: 'Collateral Type(s)'
                        },
                        {
                            field: 'Relationships',
                            title: 'Relationship(s)'
                        },
                        {
                            field: 'GenderText',
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
                            field: 'ContactID',
                            title: '',
                            formatter: function (value, row, index) {
                                var hasUpdatePermission = roleSecurityService.hasPermission($scope.permissionKey, PERMISSION.UPDATE);
                                return hasUpdatePermission ? '<a href="javascript:void(0)" data-default-action security permission-key="{{permissionKey}}" permission="update" data-ng-click="edit(' + value + ')" alt="Edit Contact" title="Edit" space-key-press><i class="fa fa-pencil fa-fw" /></a>'
                                    : '<a href="javascript:void(0)" data-default-action security permission-key="{{permissionKey}}" permission="read" data-ng-click="edit(' + value + ')" alt="View Contact" title="View" space-key-press><i class="fa fa-eye fa-fw" /></a>'
                                    +'<a href="javascript:void(0)" data-default-no-action security permission-key="{{permissionKey}}" permission="delete" data-ng-click="remove(' + value + ')" title="Delete" space-key-press><i class="fa fa-trash fa-fw"></i></a>';
                            }
                        }
                    ]
                };
            };

            var resetForm = function () {
                $rootScope.formReset($scope.ctrl.contactForm);
                isClientIdentifierDirty = false;
                resetContactRelationship();
                isContactRelationshipDirty = false;
            };

            var getIdByText = function (text, list) {
                var ContactTypeDetails = $scope.getLookupsByType('ContactType')
                return ContactTypeDetails.filter(
                      function (obj, value) {
                          return (obj.Name == text);
                      }
                  );
            };

            $scope.initCollateral = function () {
                $scope.collateral = {
                    CollateralEffectiveDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal')
                };
                $scope.initAddresses();
                $scope.initPrevAddresses();
                $scope.initPhones();
                $scope.initEmails();
                $scope.initAlternateIDs();
                $scope.formatAlternateIDs();
                initContactRelationship();
            };

            $scope.initAddresses = function () {
                $scope.emptyAddress = true;
                $scope.Addresses = [{
                    AddressTypeID: null,
                    Line1: '',
                    Line2: '',
                    City: '',
                    StateProvince: null,
                    County: null,
                    Zip: '',
                    MailPermissionID: '',
                    IsPrimary: true,
                    IsGateCode: false,
                    IsComplexName: false,
                    IsAddressPermissions: true
                }];
            };

            $scope.initPrevAddresses = function () {
                $scope.PrevAddresses = [{
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

            $scope.initPhones = function () {
                $scope.Phones = [{
                    Index: null,
                    PhoneTypeID: null,
                    Number: '',
                    PhonePermissionID: null,
                    IsPrimary: false
                }];
            };

            $scope.initEmails = function () {
                $scope.Emails = [{
                    Email: '',
                    EmailPermissionID: null,
                    IsPrimary: false
                }];
            };

            $scope.initAlternateIDs = function () {
                $scope.ClientAlternateIDs = [];
            };

            objAlternateID = function () {
                var obj = {
                    Index: 0,
                    ContactClientIdentifierID: 0,
                    ClientIdentifierTypeID: 0,
                    EffectiveDate: new Date(),
                    AlternateID: '',
                    IsRequired: false,
                    IsActive: true,
                    ShowPlusButton: true,
                    ShowMinusButton: true
                };
                return obj;
            }

            $scope.resetEnterKey = function () {
                $scope.enterKeyStop = true;
                $scope.stopNext = false;
                $scope.saveOnEnter = false;
            }
            $scope.get = function (contactID, contactTypeID) {

                $scope.isLoading = true;
                if ($state.current.name.toLowerCase().indexOf('patientprofile') >= 0) {
                    $scope.controlsVisible = false;
                    $scope.enterKeyStop = true;
                    $scope.stopNext = false;
                    $scope.saveOnEnter = true;

                }
                else {
                    $scope.controlsVisible = true;
                    $scope.enterKeyStop = false;
                    $scope.stopNext = false;
                    $scope.saveOnEnter = true;
                    $timeout(function () {
                        var nextState = angular.element("xen-workflow-action[data-state-name] > li.list-group-item.active").parents('.work-flow').next().find("xen-workflow-action[data-state-name]");
                        $scope.controlsVisible = (nextState.length > 0);
                    });
                }
                contactBenefitService.get($scope.contactID).then(function (data) {
                    if(hasData(data))
                       $scope.contactBenefitList = data.DataItems;
                });
                return collateralService.get(contactID, contactTypeID, false).then(function (data) {
                    $scope.initCollateral()
                    $scope.collateralList = bindDataModel(data.DataItems, 1);
                    isClientIdentifierDirty = false;
                    $scope.permissionId = 0;
                    $scope.disableAddress = $scope.setDisableAddress();

                    if ($scope.collateralList != null) {
                        collateralsTable.bootstrapTable('load', $scope.collateralList);
                        applyDropupOnGrid(false);
                    } else {
                        collateralsTable.bootstrapTable('removeAll');
                    }

                    resetForm();
                    editMode = false;
                    //checkFormStatus();
                    $rootScope.$broadcast($state.current.name,
                    { validationState: (($scope.collateralList != null && Object.keys($scope.collateralList).length > 0) ? 'valid' : 'warning') });

                }).then(function () {
                    $scope.isLoading = false;
                });
            };
            $scope.getParentAddressID = function () {
                $scope.isLoading = true;
                contactAddressService.get($scope.contactID).then(function (data) {
                    if (hasData(data)) {
                        var address = getPrimaryOrLatestData(data.DataItems);    //$filter('filter')(data.DataItems, { IsPrimary: true });
                        address[0].IsEffectiveDate = false;
                        address[0].IsExpirationDate = false;
                        address[0].IsAddressPermissions = true;
                        $scope.parentAddressID = address[0].AddressID;
                    }
                    $scope.isLoading = false;
                },
                 function (errorStatus) {
                     $scope.isLoading = false;
                     alertService.error('Unable to connect to server');
                 });

            };

            $scope.clearOther = function (index) {
                if ($scope.ContactRelationships && $scope.ContactRelationships[index]) {
                    var selectedCollateral = $scope.ContactRelationships[index].RelationshipTypeID;
                    if (selectedCollateral == $scope.otherRelationship || selectedCollateral == $scope.otherPhysician || selectedCollateral == $scope.otherProvider) {
                        $scope.ContactRelationships[index].OtherRelationship = '';
                    }
                }
               
            }

            $scope.formatAlternateIDs = function () {
                if ($scope.ClientAlternateIDs.length == 0)
                    $scope.ClientAlternateIDs.push(objAlternateID());
                else {
                    angular.forEach($scope.ClientAlternateIDs, function (alternateIdentifier) {
                        if (alternateIdentifier.EffectiveDate)
                            alternateIdentifier.EffectiveDate = $filter('toMMDDYYYYDate')(alternateIdentifier.EffectiveDate, 'MM/DD/YYYY');
                        if (alternateIdentifier.ExpirationDate)
                            alternateIdentifier.ExpirationDate = $filter('toMMDDYYYYDate')(alternateIdentifier.ExpirationDate, 'MM/DD/YYYY');
                    });
                }
                $scope.ClientAlternateIDs = setAddMinusButtons($scope.ClientAlternateIDs);
            }

            $scope.addNewAlternateID = function () {
                globalObjectsService.setViewContent();
                $scope.ClientAlternateIDs = $filter("filter")($scope.ClientAlternateIDs, function (obj) {
                    obj.ShowPlusButton = false;
                    return obj;
                });
                $scope.ClientAlternateIDs.push(objAlternateID());
            }

            $scope.removeAlternateID = function (removeIndex) {
                globalObjectsService.setViewContent();
                $scope.ClientAlternateIDs = removeControl($scope.ClientAlternateIDs, removeIndex, 'ContactClientIdentifierID');
                $scope.ClientAlternateIDs = setAddMinusButtons($scope.ClientAlternateIDs);
            }

            var checkFormStatus = function () {
                $scope.$watch('ctrl.contactForm.$valid', function (newValue) {
                    if (newValue !== undefined)
                        $rootScope.$broadcast($state.current.name,
                        {
                            validationState: (($scope.collateralList != null && $scope.collateralList.length > 0) ? 'valid' : 'warning')
                        });
                });
            };

            var bindDataModel = function (model, showCurrentUser) {
                var listToBind = model;
                if ($scope.GenderList == undefined || $scope.GenderList == null)
                    $scope.GenderList = $scope.getLookupsByType('Gender');
                if ($scope.CollateralTypeList == undefined || $scope.CollateralTypeList == null)
                    $scope.CollateralTypeList = $scope.getLookupsByType('CollateralType');
                if (!showCurrentUser) {
                    listToBind = removeContactFromList(listToBind, $scope.contactID);
                }
                angular.forEach(listToBind, function (collateral) {
                    collateral.DOB = collateral.DOB ? $filter('toMMDDYYYYDate')(collateral.DOB, 'MM/DD/YYYY') : "";
                    if (collateral.GenderID > 0)
                        collateral.GenderText = getGenderText(collateral.GenderID)[0].Name;
                });
                return listToBind;
            };

            var getGenderText = function (genderID) {
                var result = $scope.GenderList.filter(
                      function (obj, value) {
                          return (obj.ID == genderID);
                      }
                  );
                return result;
            }


            var removeContactFromList = function (contactList, contactId) {
                var obj = contactList.filter(function (obj, value) {
                    if (obj.ContactID == contactId) {
                        var index = contactList.indexOf(obj);
                        if (index != -1) {
                            contactList.splice(index, 1);
                        }
                    }
                    else {
                        //var indexCollateral = contactList.indexOf(obj);
                        //if (indexCollateral != -1) {
                        //    contactList.splice(indexCollateral, 1);
                        //}
                    }
                });
                //ToDo: Remove all collaterals already assigned to contact

                return contactList;
            }

            $scope.add = function (isNext, modelToSave) {
                $scope.isLoading = true;
                collateralService.add(modelToSave).then(
                    function (response) {
                        isContactRelationshipDirty = true;
                        saveContactRelationShip(response, 'added', isNext, response.data.ID)
                    },
                    function (errorStatus) {
                        $scope.isLoading = false;
                        alertService.error('OOPS! Something went wrong');
                    },
                    function (notification) {
                        alertService.warning(notification);
                    }).then(function () {
                        $scope.isLoading = false;
                    }
                );
            };

            var saveContactRelationShip = function (response, saveType, isNext, contactID) {
                var promiseArray = [];
                var deferred = $q.defer();
                var isDirtyContactRelationShip = formService.isDirty($scope.ctrl.contactForm.ContactRelationshipForm.$name);

                if ((isDirtyContactRelationShip || isContactRelationshipDirty) && $scope.ContactRelationships && $scope.ContactRelationships.length > 0) {
                    angular.forEach($scope.ContactRelationships, function (contactRelationship) {
                        contactRelationship.ContactID = contactID;
                        contactRelationship.ParentContactID = $scope.contactID;
                        if (contactRelationship.EffectiveDate) {
                            contactRelationship.EffectiveDate = $filter("formatDate")(contactRelationship.EffectiveDate);
                        }
                        if (contactRelationship.ExpirationDate) {
                            contactRelationship.ExpirationDate = $filter("formatDate")(contactRelationship.ExpirationDate);
                        }
                        if (!(contactRelationship.RelationshipTypeID == $scope.otherRelationship || contactRelationship.RelationshipTypeID == $scope.otherPhysician || contactRelationship.RelationshipTypeID == $scope.otherProvider)) {
                            contactRelationship.OtherRelationship = null;
                        }
                        if (contactRelationship.IsActive == true) {
                            if (contactRelationship.ContactRelationshipTypeID == 0)
                                promiseArray.push([contactRelationshipService.addContactRelationship, [contactRelationship]]);
                            else
                                promiseArray.push([contactRelationshipService.updateContactRelationship, [contactRelationship]]);
                        }
                        else if (contactRelationship.ContactRelationshipTypeID != 0)
                            promiseArray.push([contactRelationshipService.remove, [contactRelationship.ContactRelationshipTypeID, contactRelationship.ContactID]]);
                    });
                }

                $q.serial(promiseArray).then(function () {
                    deferred.resolve();
                    var data = response.data;
                    if (data.ResultCode == 0) {
                        alertService.success('Collateral has been successfully ' + saveType + ".");
                        if ($state.current.name.toLowerCase().indexOf('patientprofile') >= 0) {
                            $scope.$parent.getPatientProfileData();
                        }
                        $scope.isChecked = false;
                        $scope.isPrevChecked = false;
                        $scope.disableAddress = $scope.setDisableAddress();;
                        $scope.dirtyAddress = false;
                        isContactRelationshipDirty = false;
                        $scope.get($scope.contactID, $scope.collateral.ContactTypeID).then(function () {
                            if (isNext)
                                $scope.next();
                        });
                    } else {
                        alertService.error('OOPS! Something went wrong');
                    }
                },
                function (error) {
                    alertService.error('OOPs something went wrong ' + error);
                    deferred.reject();
                });

                return deferred.promise;
            }

            $scope.reset = function () {
                $scope.isChecked = false;
                $scope.isPrevChecked = false;
                $scope.dirtyAddress = false;
                $scope.initCollateral();
                editMode = false;
                resetForm();
                $scope.permissionId = 0;
                $scope.disableAddress = $scope.setDisableAddress();;
                $scope.$parent['autoFocusEdit'] = true;
            };

            $scope.checkCollateralAsPayor = function (expirationDate) {
                if (expirationDate && $scope.collateral.CollateralExpirationDate && (moment(moment(expirationDate).format('MM/DD/YYYY')) > moment(moment($scope.collateral.CollateralExpirationDate).format('MM/DD/YYYY')))) {
                    alertService.error('You can not select Expiration Date greater than Collateral Expiration Date.');
                    expirationDate = '';
                }
                var familyGroupRelations = $filter('filter')($scope.ContactRelationships, function (obj) {
                    return obj.RelationshipGroupID === $scope.collateralTypeFamily;
                }, true);
                if ($scope.contactBenefitList.length > 0)
                {
                    angular.forEach($scope.contactBenefitList, function (item) {
                        if (familyGroupRelations.length > 0)
                        {
                            if (familyGroupRelations[0].ExpirationDate && (moment(moment(familyGroupRelations[0].ExpirationDate).format('MM/DD/YYYY')) < moment(moment().format('MM/DD/YYYY'))) && item.PolicyHolderID == familyGroupRelations[0].ContactID) {
                                alertService.error('Please update Policy Holder on Payors screen before expiring Collateral Type.');
                                familyGroupRelations[0].ExpirationDate = '';
                                return;
                            }
                        }
                    })
                }
            };

            $scope.expireCollateral = function () {
                if ($scope.ctrl.contactForm && $scope.ctrl.contactForm.expirationDate && $scope.collateral && $scope.collateral.CollateralExpirationDate) {
                    angular.forEach($scope.ContactRelationships, function (item) {
                        if (item.IsActive == true &&
                            (((item.ExpirationDate && (moment(moment(item.ExpirationDate).format('MM/DD/YYYY')) > moment(moment($scope.collateral.CollateralExpirationDate).format('MM/DD/YYYY')))) && (moment(moment(item.EffectiveDate).format('MM/DD/YYYY')) > moment(moment($scope.collateral.CollateralExpirationDate).format('MM/DD/YYYY')))) ||
                            (moment(moment(item.EffectiveDate).format('MM/DD/YYYY')) > moment(moment($scope.collateral.CollateralExpirationDate).format('MM/DD/YYYY')))))
                        {
                            alertService.error('Please update Collateral Type Effective Date.');
                            $scope.collateral.CollateralExpirationDate = '';
                            return;
                        }
                        if (item.ExpirationDate == '' || item.ExpirationDate == null || (new Date(item.ExpirationDate) > new Date($scope.collateral.CollateralExpirationDate)))
                        {
                            item.ExpirationDate = $scope.collateral.CollateralExpirationDate;
                            if ($scope.contactBenefitList.length > 0) {
                                angular.forEach($scope.contactBenefitList, function (contactBenefitItem) {
                                    if (item.ExpirationDate && (moment(moment(item.ExpirationDate).format('MM/DD/YYYY')) < moment(moment().format('MM/DD/YYYY'))) && (contactBenefitItem.PolicyHolderID == item.ContactID)) {
                                        alertService.error('Please update Policy Holder on Payors screen before expiring Collateral Type.');
                                        item.ExpirationDate = $scope.collateral.CollateralExpirationDate = '';
                                        return;
                                    }
                                })
                            }
                        }
                    });
                }
            };

            $scope.save = function (isNext, mandatory, hasErrors) {
                // if there are errors, the screen is optional, and the user is trying to go to the next screen, don't bother trying to save anything
                // Vipul Singhal - As per Json if user modified the optional screen and click enter then it should display the validation error 
                // and if user don't don any modification then user can move to next screen.
                if (!mandatory && isNext && hasErrors) {
                    $('#contactListModel').modal('hide');
                    $scope.next();
                }

                var isDirty = formService.isDirty();

                if ((isDirty || isContactRelationshipDirty || isClientIdentifierDirty) && !hasErrors) {
                    $scope.collateral.ContactTypeID = contactTypeId;
                    var modelToSave = angular.copy($scope.collateral);

                    if (modelToSave.DOB) {
                        modelToSave.DOB = $filter("formatDate")(modelToSave.DOB);
                        var dob = new Date(modelToSave.DOB);
                        if (dob >= $scope.endDate) {
                            alertService.error('Please check date of birth.');
                            $scope.promiseNoOp();
                            return false;
                        }
                    }

                    if (modelToSave.SchoolBeginDate) {
                        modelToSave.SchoolBeginDate = $filter("formatDate")(modelToSave.SchoolBeginDate);
                        var schoolBeginDate = new Date(modelToSave.SchoolBeginDate);
                        if (schoolBeginDate >= $scope.endDate) {
                            alertService.error('Please check begin date.');
                            $scope.promiseNoOp();
                            return false;
                        }
                    }

                    if (modelToSave.SchoolEndDate) {
                        modelToSave.SchoolEndDate = $filter("formatDate")(modelToSave.SchoolEndDate);
                        var schoolEndDate = new Date(modelToSave.SchoolEndDate);
                        if (schoolBeginDate > schoolEndDate) {
                            alertService.error('Please check end date.');
                            $scope.promiseNoOp();
                            return false;
                        }
                    }
                    if (modelToSave.CollateralEffectiveDate) {
                        modelToSave.CollateralEffectiveDate = $filter("formatDate")(modelToSave.CollateralEffectiveDate);
                    }
                    if (modelToSave.CollateralExpirationDate) {
                        modelToSave.CollateralExpirationDate = $filter("formatDate")(modelToSave.CollateralExpirationDate);
                    }
                    // filter out blank address
                    modelToSave.Addresses = $filter('filter')($scope.Addresses, function (item) {
                        if (item.Line1 != '' || item.Line2 != '' || item.City != '' || (item.StateProvince != null && item.StateProvince != 0) || (item.County != null && item.County != 0) || item.Zip != '') {
                            item.ContactID = modelToSave.ContactID;
                            return true;
                        }
                        else {
                            return false;
                        }
                    });


                    //filter blank addresses from prev address
                    var prevadd = angular.copy($scope.PrevAddresses);
                    prevadd = $filter('filter')($scope.PrevAddresses, function (item) {
                        if (item.Line1 != '' || item.Line2 != '' || item.City != '' || (item.StateProvince != null && item.StateProvince != 0) || (item.County != null && item.County != 0) || item.Zip != '') {
                            return true;
                        }
                        else {
                            return false;
                        }
                    });

                    // filter out blank phone
                    modelToSave.Phones = $filter('filter')($scope.Phones, function (item) {
                        if (item.Number != '' || (item.ContactPhoneID != undefined && item.ContactPhoneID != 0)) {
                            item.ContactID = modelToSave.ContactID;
                            if (item.ContactPhoneID != 0 && item.Number == '') {
                                item.IsActive = false;
                            }
                            return true;
                        }
                        else {
                            return false;
                        }
                    });

                    // filter out blank email
                    modelToSave.Emails = $filter('filter')($scope.Emails, function (item) {
                        if (item.Email != '' || (item.ContactEmailID != undefined && item.ContactEmailID != 0)) {
                            item.ContactID = modelToSave.ContactID;
                            if (item.ContactEmailID != 0 && item.Email == '') {
                                item.IsActive = false;
                            }
                            return true;
                        }
                        else {
                            return false;
                        }
                    });
                    if ((modelToSave.Phones.length == 0 || (modelToSave.Phones.length > 0 && !modelToSave.Phones[0].Number)) && (modelToSave.Emails.length == 0 || (modelToSave.Emails.length > 0 && !modelToSave.Emails[0].Email)) && (!$scope.collateral.DriverLicense || ($scope.collateral.DriverLicense && $scope.collateral.DriverLicense.length == 0))) {
                        alertService.error('At least one form of unique identification is required: Driver License, Phone Number or Email.');
                        return;
                    }

                    modelToSave.ClientAlternateIDs = $filter('filter')($scope.ClientAlternateIDs, function (item) {
                        if (item.EffectiveDate) {
                            item.EffectiveDate = $filter("formatDate")(item.EffectiveDate);
                        }
                        if (item.ExpirationDate) {
                            item.ExpirationDate = $filter("formatDate")(item.ExpirationDate);
                        }

                        return item.AlternateID !== '';
                    });

                    if ($scope.isChecked) {
                        modelToSave.CopyContactAddress = true;
                        //angular.forEach(modelToSave.Addresses, function (address) {
                        //    address.AddressID = null;
                        //});
                    }
                    else {
                        modelToSave.CopyContactAddress = false;

                        if (modelToSave.Addresses.length == 0) {
                            modelToSave.Addresses = angular.forEach(prevadd, function (address) {
                                //if (address.ContactID == $scope.contactID)
                                if (address.AddressID == $scope.parentAddressID) {
                                    address.IsDeleted = true;
                                    return true;
                                }
                                else {
                                    return false;
                                }
                                //address.AddressID = null;
                            });
                        }
                    }

                    //reset to null, if Zip is Empty
                    //angular.forEach(modelToSave.Addresses, function (address) {
                    //    if (address.Zip == '') { address.Zip = null; }
                    //});

                    if (editMode) {
                        $scope.update(isNext, modelToSave);
                    }
                    else {
                        //modelToSave.ContactID = $scope.contactID;
                        modelToSave.ParentContactID = $scope.contactID;
                        $scope.add(isNext, modelToSave);
                    }
                }
                else if (isNext) {
                    $scope.next();
                }

                $scope.selIdx = -1;

            };

            $scope.edit = function (index, rowIndex) {
                $scope.selectedID = index;
                $scope.selIdx = rowIndex;
                $scope.isChecked = false;
                $scope.isPrevChecked = false;
                $scope.dirtyAddress = false;
                $scope.permissionId = index;
                $scope.isEdit = true;
                $scope.alreadyChecked = false;
                angular.forEach($scope.collateralList, function (collateral) {
                    if (collateral.ContactID == index) {
                        demographic = angular.copy(collateral);
                        //$scope.Addresses = angular.copy(collateral.Addresses);
                        //$scope.PrevAddresses = angular.copy(collateral.Addresses);
                        getContactRelationship(demographic.ContactID);
                        if (demographic.DOB)
                            demographic.DOB = $filter('toMMDDYYYYDate')(demographic.DOB, 'MM/DD/YYYY');

                        if (demographic.SchoolBeginDate)
                            demographic.SchoolBeginDate = $filter('toMMDDYYYYDate')(demographic.SchoolBeginDate, 'MM/DD/YYYY');

                        if (demographic.SchoolEndDate)
                            demographic.SchoolEndDate = $filter('toMMDDYYYYDate')(demographic.SchoolEndDate, 'MM/DD/YYYY');

                        if (demographic.CollateralEffectiveDate)
                            demographic.CollateralEffectiveDate = $filter('toMMDDYYYYDate')(demographic.CollateralEffectiveDate, 'MM/DD/YYYY');

                        if (demographic.CollateralExpirationDate)
                            demographic.CollateralExpirationDate = $filter('toMMDDYYYYDate')(demographic.CollateralExpirationDate, 'MM/DD/YYYY');

                        $scope.Addresses = getPrimaryOrLatestData(demographic.Addresses); //angular.copy($filter('filter')(demographic.Addresses, { IsPrimary: true }));
                        $scope.PrevAddresses = getPrimaryOrLatestData(demographic.Addresses); //angular.copy($filter('filter')(demographic.Addresses, { IsPrimary: true }));

                        demographic.LivingWithClientStatus = demographic.LivingWithClientStatus ? demographic.LivingWithClientStatus.toString() : '';

                        if ($scope.Addresses.length == 0) {
                            $scope.initAddresses();
                            $scope.initPrevAddresses();
                        }
                        else {
                            $scope.emptyAddress = false;
                            $scope.isChecked = false;
                            $scope.isPrevChecked = false;
                            $scope.Addresses[0].IsAddressPermissions = true;
                        }
                        //$scope.Phones = angular.copy(collateral.Phones);
                        $scope.Phones = getPrimaryOrLatestData(demographic.Phones);
                        angular.forEach($scope.Phones, function (phone) {
                            phone.EffectiveDate = phone.EffectiveDate ? $filter('toMMDDYYYYDate')(phone.EffectiveDate, 'MM/DD/YYYY') : '';
                            phone.ExpirationDate = phone.ExpirationDate ? $filter('toMMDDYYYYDate')(phone.ExpirationDate, 'MM/DD/YYYY') : '';
                        });
                        if ($scope.Phones.length == 0) {
                            $scope.initPhones();
                        }
                        //$scope.Emails = angular.copy(collateral.Emails);
                        $scope.Emails = getPrimaryOrLatestData(demographic.Emails);
                        angular.forEach($scope.Emails, function (email) {
                            email.EffectiveDate = email.EffectiveDate ? $filter('toMMDDYYYYDate')(email.EffectiveDate, 'MM/DD/YYYY') : '';
                            email.ExpirationDate = email.ExpirationDate ? $filter('toMMDDYYYYDate')(email.ExpirationDate, 'MM/DD/YYYY') : '';
                        });
                        if ($scope.Emails.length == 0) {
                            $scope.initEmails();
                        }

                        $scope.ClientAlternateIDs = angular.copy(demographic.ClientAlternateIDs);
                        $scope.formatAlternateIDs();

                        if (demographic.SSN && demographic.SSN.length > 0 && demographic.SSN.length < 9) {
                            contactSSNService.refreshSSN(demographic.ContactID, demographic).then(function () {
                                if (demographic != undefined) {
                                    $scope.collateral = demographic;
                                    resetForm();
                                    demographic = undefined;
                                }
                            })
                        }
                        else {
                            $scope.collateral = demographic;
                            resetForm();
                            demographic = undefined;

                        }
                        if ($scope.Addresses.length > 0 && ($scope.Addresses[0].AddressID == $scope.parentAddressID)) {
                            $scope.isChecked = true;
                            $scope.alreadyChecked = true;
                            $scope.isPrevChecked = true;
                            $scope.dirtyAddress = true;
                        }
                        $scope.$parent['autoFocusEdit'] = true;
                        //  $scope.resetMask(angular.element('#SSN'), $scope);
                        editMode = true;
                        return;
                    }
                });

                $scope.disableAddress = $scope.setDisableAddress() ? true : $scope.isChecked;
            }

            $scope.update = function (isNext, modelToSave) {
                $scope.isLoading = true;
                collateralService.update(modelToSave).then(
                    function (response) {
                        saveContactRelationShip(response, 'updated', isNext, modelToSave.ContactID);
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

            $scope.remove = function (id) {
                bootbox.confirm("Selected collateral will be removed.\n Do you want to continue?", function (result) {
                    if (result == true) {
                        collateralService.remove($scope.contactID, id).then(function (data) {
                            $scope.isLoading = false;
                            alertService.success('Collateral has been deleted.');
                            $scope.isChecked = false;
                            $scope.isPrevChecked = false;
                            $scope.disableAddress = $scope.setDisableAddress();;
                            $scope.dirtyAddress = false;
                            if ($state.current.name.toLowerCase().indexOf('patientprofile') >= 0) {
                                $scope.$parent.getPatientProfileData();
                            }
                            $scope.get($scope.contactID, contactTypeId).then(function () {
                                $scope.selIdx = -1;
                            });
                        },
                            function (errorStatus) {
                                alertService.error('OOPS! Something went wrong');
                            });
                    }
                });
            };

            $scope.cancel = function () {
                bootbox.confirm("You will lose the information entered.\n Do you want to continue?", function (result) {
                    if (result == true) {
                        $scope.initCollateral();
                        editMode = false;
                        $scope.ctrl.contactForm.$setPristine();
                        $scope.$apply();
                        $scope.$parent['autoFocusEdit'] = true;
                        $scope.isChecked = false;
                        $scope.isPrevChecked = false;
                        $scope.disableAddress = $scope.setDisableAddress();;
                        $scope.dirtyAddress = false;
                    }
                });
            };

            $scope.cancelModel = function () {
                editMode = false;
                $('#contactListModel').modal('hide');
                $scope.searchText = '';
                $rootScope.formReset($scope.searchForm, $scope.searchForm.$name);
            };

            $scope.next = function () {
                var nextState = angular.element("xen-workflow-action[data-state-name] > li.list-group-item.active").parents('.work-flow').next().find("xen-workflow-action[data-state-name]");
                if (nextState.length > 0) {
                    $timeout(function () {
                        $scope.Goto(nextState.attr('data-state-name'), { ContactID: $scope.contactID });
                    }, 400);
                }
            };

            $scope.calculateAge = function () {
                if ($scope.collateral) {
                    if ($scope.collateral.DOB != null && $scope.collateral.DOB !== '') {
                        var date = new Date($scope.collateral.DOB);
                        if (date <= $scope.endDate) {
                            $scope.collateral.Age = parseInt($filter('toAge')($scope.collateral.DOB));
                            var isDatePastLimit = $filter('isDateMaxLimit')($scope.collateral.DOB, ageLimit);
                            if (isDatePastLimit) {
                                $scope.collateral.Age = $scope.collateral.DOB = null;
                                alertService.error("Age Can't Be greater than " + ageLimit + " years.");
                            }
                            else {
                                $scope.collateral.Age = $filter('ageToShow')($scope.collateral.DOB);
                            }
                            $('#doberrortd').removeClass('has-error');
                            $('#doberror').addClass('ng-hide');
                        }
                        else {
                            $scope.collateral.Age = null;
                            $('#doberror').removeClass('ng-hide');
                            $('#doberrortd').addClass('has-error');
                        }
                    }
                    else {
                        $scope.collateral.Age = '';
                    }
                }
            };

            $scope.validateBeginDate = function () {
                var beginDateError = angular.element("#beginDateError");
                var beginDate = angular.element("#beginDateErrortd");

                if ($scope.ctrl.contactForm != undefined && $scope.ctrl.contactForm.BeginDate && $scope.collateral != undefined && $scope.collateral.SchoolBeginDate) {
                    var date = new Date($scope.collateral.SchoolBeginDate);
                    var toDay = new Date();

                    toDay.setHours(0, 0, 0, 0);
                    if (date > toDay || date == 'Invalid Date') {
                        beginDateError.removeClass('ng-hide');
                        beginDate.addClass('has-error');
                    }
                    else {
                        beginDateError.addClass('ng-hide');
                        beginDate.removeClass('has-error');
                    }
                }
            };

            $scope.validateBeginDateLessThanEndDate = function () {
                if ($scope.ctrl.contactForm != undefined && $scope.ctrl.contactForm.BeginDate && $scope.ctrl.contactForm.EndDate && $scope.collateral != undefined && $scope.collateral.SchoolBeginDate && $scope.collateral.SchoolEndDate) {
                    var startEndDateError = angular.element("#startEndDateError");
                    var endDateErrortd = angular.element("#endDateErrortd");
                    var beginDate = new Date($scope.collateral.SchoolBeginDate);
                    var endDate = new Date($scope.collateral.SchoolEndDate);
                    if (endDate != 'Invalid Date') {
                        if (beginDate > endDate) {
                            startEndDateError.removeClass('ng-hide');
                            endDateErrortd.addClass('has-error');
                        }
                        else {
                            startEndDateError.addClass('ng-hide');
                            endDateErrortd.removeClass('has-error');
                        }
                    }
                    else {
                        endDateErrortd.addClass('has-error');
                        startEndDateError.addClass('ng-hide');
                    }
                }
            };

            $scope.formatPhone = function (phoneText) {
                if (phoneText == null || phoneText == undefined)
                    return '';
                return phoneText.replace(/(\d{3})(\d{3})(\d{4})/, '$1-$2-$3');
            }

            //Get the contact detail based on the search text
            $scope.getClientSummary = function (searchText) {
                if (searchText != undefined && searchText != null && searchText != '') {

                    clientSearchService.getClientSummary(searchText, contactTypeSearch).then(function (data) {
                        $scope.contactList = bindDataModel(data.DataItems, false);

                        if ($scope.contactList != null && $scope.contactList.length > 0) {

                            $scope.collateralList.forEach(function (element, pos) {

                                var idx = $scope.contactList.map(function (contact) {
                                    return contact.ContactID;
                                }).indexOf(element.ContactID);
                                if (idx > -1)
                                    $scope.contactList.splice(idx, 1);
                            });

                            $('#contactListModel').on('hidden.bs.modal', function () {
                                $scope.stopEnterKey();
                                var focus = $('#FirstName').is(":focus");
                                if (!focus) {
                                    $('#txtClientSearch').focus();
                                }
                            })
                            if ($scope.contactList != null && $scope.contactList.length > 0) {
                                contactsTable.bootstrapTable('load', $scope.contactList);
                                $('#contactListModel').modal('show');
                                $('#contactListModel').on('shown.bs.modal', function () {
                                    $scope.resetEnterKey();
                                    $rootScope.setFocusToGrid('contactsTable');
                                })

                            }
                            else {
                                contactsTable.bootstrapTable('removeAll');
                            }
                        }
                        else {
                            alertService.warning("No matching records found");
                            contactsTable.bootstrapTable('removeAll');
                        }
                    }, function (errorStatus) {
                        alertService.error('Unable to connect to server');
                    });
                } else {
                    $scope.stopEnterKey();
                }
            };

            $scope.populateContact = function (contactID, contactTypeID) {
                $scope.selIdx = -1;
                getContactDetails(contactID, contactTypeID);
                $('#FirstName').focus();
                $('#contactListModel').modal('hide');
            }


            var getContactRelationship = function (contactID) {
                $scope.ContactRelationships = [];
                contactRelationshipService.get(contactID, $scope.contactID).then(function (data) {
                    if (hasData(data)) {
                        $scope.ContactRelationships = data.DataItems;
                        angular.forEach($scope.ContactRelationships, function (item) {
                            if (item.EffectiveDate)
                                item.EffectiveDate = $filter('toMMDDYYYYDate')(item.EffectiveDate, 'MM/DD/YYYY');
                            if (item.ExpirationDate)
                                item.ExpirationDate = $filter('toMMDDYYYYDate')(item.ExpirationDate, 'MM/DD/YYYY');
                        });
                        setAddMinusButtons($scope.ContactRelationships);
                        $scope.validateCollateralType();
                    }
                    else
                        initContactRelationship();
                }).finally(function () {
                    resetContactRelationship();
                    if ($scope.isReadOnlyForm) {
                        $scope.ContactRelationships = $filter("filter")($scope.ContactRelationships, function (obj) {
                            obj.ShowPlusButton = false;
                            obj.ShowMinusButton = false;
                            return obj;
                        });
                    }
                });
            }

            var initContactRelationship = function () {
                $scope.ContactRelationships = [];
                $scope.ContactRelationships.push(objContactRelationship());
            };

            var objContactRelationship = function () {
                var obj = {
                    ContactRelationshipTypeID: 0,
                    ContactID: 0,
                    OtherRelationship: '',
                    EffectiveDate: $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal'),
                    ExpirationDate : '',
                    ShowPlusButton: true,
                    ShowMinusButton: true,
                    IsActive: true
                }
                return obj;
            }

            $scope.addNewRelationship = function () {
                globalObjectsService.setViewContent();
                $scope.ContactRelationships = $filter("filter")($scope.ContactRelationships, function (obj) {
                    obj.ShowPlusButton = false;
                    return obj;
                });
                $scope.ContactRelationships.push(objContactRelationship());
                isContactRelationshipDirty = true;
            }

            $scope.removeRelationship = function (index) {
                var showMinus = false;
                var contactRelationshipTypeId;
                var familyGroupRelations = $filter('filter')($scope.ContactRelationships, function (obj) {
                    return obj.RelationshipGroupID === $scope.collateralTypeFamily;
                }, true);
                if (familyGroupRelations.length > 0)
                    contactRelationshipTypeId = familyGroupRelations[0].ContactRelationshipTypeID;
                if ($scope.contactBenefitList.length > 0) {
                    angular.forEach($scope.contactBenefitList, function (item) {
                        if (item.PolicyHolderID == $scope.ContactRelationships[index].ContactID && ($scope.ContactRelationships[index].ContactRelationshipTypeID == contactRelationshipTypeId)) {
                            alertService.error('Please update Policy Holder on Payors screen before expiring Collateral Type.');
                            showMinus = true;
                        }
                    })
                }
                if (!showMinus)
                {
                    globalObjectsService.setViewContent();
                    $scope.ContactRelationships = removeControl($scope.ContactRelationships, index, 'ContactRelationshipTypeID');
                    $scope.ContactRelationships = setAddMinusButtons($scope.ContactRelationships);
                    $scope.validateCollateralType();
                }
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

            $scope.validateCollateralType = function () {
                var filteredItems = filterContactRelationship();
                if (filteredItems && filteredItems.length > 0) {
                    setRequiredFields(true);
                }
                else if (!filteredItems || filteredItems.length == 0) {
                    setRequiredFields();
                }
            };

            var filterContactRelationship = function () {
                return $filter('filter')($scope.ContactRelationships, function (data) {
                    return data.IsActive === true && data.IsPolicyHolder === true;
                });
            }

            var getContactDetails = function (contactId, contactTypeID) {
                $scope.isLoading = true;
                $scope.isChecked = false;
                $scope.dirtyAddress = false;
                collateralService.get(contactId, contactTypeID, true).then(function (data) {
                    $scope.disableAddress = false;
                    // registrationService.get(contactId, contactTypeID).then(function (data) {
                    if (hasData(data)) {
                        data.DataItems[0].RelationshipTypeID = data.DataItems[0].RelationshipTypeID == 0 ? undefined : data.DataItems[0].RelationshipTypeID;

                        $scope.collateral = data.DataItems[0];
                        if ($scope.collateral.SSN && $scope.collateral.SSN.length > 0 && $scope.collateral.SSN.length < 9) {
                            contactSSNService.refreshSSN($scope.collateral.ContactID, $scope.collateral);
                        }
                        if ($scope.collateral.DOB)
                            $scope.collateral.DOB = $filter('toMMDDYYYYDate')($scope.collateral.DOB, 'MM/DD/YYYY');

                        if ($scope.collateral.DeceasedDate)
                            $scope.collateral.DeceasedDate = $filter('toMMDDYYYYDate')($scope.collateral.DeceasedDate, 'MM/DD/YYYY');

                        //$scope.Addresses = angular.copy($scope.collateral.Addresses);
                        $scope.Addresses = getPrimaryOrLatestData($scope.collateral.Addresses);

                        if ($scope.Addresses.length == 0) {
                            $scope.initAddresses();
                            $scope.initPrevAddresses();
                        }
                        else {
                            $scope.emptyAddress = false;
                            $scope.isChecked = false;
                            $scope.Addresses[0].IsEffectiveDate = false;
                            $scope.Addresses[0].IsExpirationDate = false;
                            $scope.Addresses[0].IsAddressPermissions = true;
                        }
                        //$scope.Phones = angular.copy($scope.collateral.Phones);
                        $scope.Phones = getPrimaryOrLatestData($scope.collateral.Phones);
                        if ($scope.Phones && $scope.Phones != undefined) {
                            angular.forEach($scope.Phones, function (phone) {
                                phone.EffectiveDate = phone.EffectiveDate ? $filter('toMMDDYYYYDate')(phone.EffectiveDate, 'MM/DD/YYYY') : '';
                                phone.ExpirationDate = phone.ExpirationDate ? $filter('toMMDDYYYYDate')(phone.ExpirationDate, 'MM/DD/YYYY') : '';
                            });
                        }
                        if ($scope.Phones.length == 0) {
                            $scope.initPhones();
                        }
                        //$scope.Emails = angular.copy($scope.collateral.Emails);
                        $scope.Emails = getPrimaryOrLatestData($scope.collateral.Emails);
                        if ($scope.Emails && $scope.Emails != undefined) {
                            angular.forEach($scope.Emails, function (email) {
                                email.EffectiveDate = email.EffectiveDate ? $filter('toMMDDYYYYDate')(email.EffectiveDate, 'MM/DD/YYYY') : '';
                                email.ExpirationDate = email.ExpirationDate ? $filter('toMMDDYYYYDate')(email.ExpirationDate, 'MM/DD/YYYY') : '';
                            });
                        }
                        if ($scope.Emails.length == 0) {
                            $scope.initEmails();
                        }

                        $scope.ClientAlternateIDs = $scope.collateral.ClientAlternateIDs;
                        $scope.formatAlternateIDs();
                        $scope.collateral.CollateralEffectiveDate = $scope.collateral.CollateralEffectiveDate?  $filter('toMMDDYYYYDate')($scope.collateral.CollateralEffectiveDate) : $filter('toMMDDYYYYDate')(new Date(), 'MM/DD/YYYY', 'useLocal');
                        
                        if ($scope.Addresses.length > 0 && ($scope.Addresses[0].AddressID == $scope.parentAddressID)) {
                            $scope.alreadyChecked = true;
                            $scope.isChecked = true;
                            $scope.isPrevChecked = true;
                            $scope.dirtyAddress = true;

                            $timeout(function () {
                                $scope.disableAddress = true;
                            }, 2000);
                        }
                        editMode = false;
                    }
                    $scope.isLoading = false;
                },
                    function (errorStatus) {
                        $scope.isLoading = false;
                        alertService.error('Unable to connect to server');
                    });

                $scope.$watch('ctrl.collateralForm.$valid', function (newValue) {
                    if (newValue !== undefined)
                        $rootScope.$broadcast($state.current.name, { validationState: (newValue ? 'valid' : 'invalid') });
                });
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

            $scope.EnableDisableEnterKey = function (searchText, isSearch) {
                if (searchText != undefined && searchText != null && searchText != "") {
                    $scope.resetEnterKey();
                    if (isSearch)
                        $scope.getClientSummary(searchText);
                }
                else if ($state.current.name.toLowerCase().indexOf('patientprofile') >= 0) {
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

            $scope.getParentAddressID();

            $scope.get($scope.contactID, contactTypeId);
            $scope.initializeBootstrapTable();
            $scope.$on('showDetails', function (event, args) {
                $scope.get($scope.contactID, contactTypeId).then(function () {
                    setGridItem(collateralsTable, 'ContactRelationshipID', args.id);
                });
            });


            var removeControl = function (collection, index, pkID) {
                var newCollection = $filter('filter')(collection, function (data) {
                    return data.IsActive === true;
                });
                var obj = newCollection[index];

                if (eval('obj.' + pkID) === 0) {
                    newCollection.splice(index, 1);
                }
                else {
                    obj.IsActive = false;

                    if (pkID.indexOf('ContactAlias') >= 0) {
                        isContactAliasDirty = true;
                    }
                    else if (pkID.indexOf('ContactRelationship') >= 0) {
                        isContactRelationshipDirty = true;
                    }
                    else if (pkID.indexOf('ClientIdentifier') >= 0) {
                        isClientIdentifierDirty = true;
                    }
                }
                return mergedCollection(collection, newCollection);
            }

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
            }

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
        }]);
