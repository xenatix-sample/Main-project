(function () {
    angular.module('xenatixApp')
        .controller('referralRequestorController', [
            '$scope', '$state', '$q', '$stateParams', 'alertService', '$filter', '$rootScope', 'formService', 'referralHeaderService', 'registrationService', 'contactAddressService', 'contactEmailService', 'contactPhoneService', '$timeout', 'globalObjectsService', '$controller',
            function ($scope, $state, $q, $stateParams, alertService, $filter, $rootScope, formService, referralHeaderService, registrationService, contactAddressService, contactEmailService, contactPhoneService, $timeout, globalObjectsService, $controller) {
                $controller('baseContactController', { $scope: $scope });
                $scope.EnableFilter = true;
                $scope.referralTypeInternal = 1;
                $scope.referralTypeExternal = 2;
                $scope.otherOrganizationOption = 61;
                $scope.otherReferralSourceOption = 117;
                var gotoNext = false;
                var contactTypeID = 7;
                var currentUIState = 'referrals.requestor';
                var isSaving = false;
                var isEmailDirty = false;
                var isPhoneDirty = false;
                $scope.IsReferralScreen = true;
                $scope.referralSourceLookUpType = '';
                $scope.permissionKey = $state.current.data.permissionKey;

                initScope = function () {
                    $scope.enableReferralStatus = true;
                    $scope.dateOptions = {
                        formatYear: 'yy',
                        startingDay: 1,
                        showWeeks: false
                    };
                    initHeader();
                    initDemographics();
                    initPhones();
                    initEmails();
                    initAddress();
                    $scope.enterKeyStop = false;
                    $scope.$parent.initReferralParent($stateParams.ReferralHeaderID, $stateParams.ContactID);
                    $scope.emailPattern = /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/i;
                    //$scope.$parent['autoFocus'] = true;                    
                }
                var phoneAccessCode = $scope.PHONE_ACCESS.ConditionalRequired | $scope.PHONE_ACCESS.Number;
                $scope.PhoneAccessCode = phoneAccessCode;
                $scope.$watch("Emails[0].Email", function (newValue) {
                        if (newValue) {
                            $scope.PhoneAccessCode = 0;
                        }
                        else {
                            $scope.PhoneAccessCode = phoneAccessCode;
                        }
                    }
                );
                $scope.init = function () {
                    if ($stateParams.ReferralHeaderID && $stateParams.ReferralHeaderID != 0) {
                        $scope.get();
                    }
                    else {
                        $scope.permissionID = 0;
                        resetForm();
                        var stateDetail = { stateName: currentUIState, validationState: ' ' };                        
                        $rootScope.$broadcast('rightNavigationReferralHandler', stateDetail);
                        $scope.$parent.referralStatusID = 1;
                        setReferralSourceLookUpType();
                    }
                };

                $scope.changeReferralSource = function () {
                    $scope.ReferralSource = null;
                    setReferralSourceLookUpType();
                };

                var setReferralSourceLookUpType = function () {
                    $scope.referralSourceLookUpType = '';
                    if ($scope.Header.ReferralTypeID == $scope.referralTypeInternal) {
                        $scope.referralSourceLookUpType = LOOKUPTYPE.ClientType;
                    }
                    else if ($scope.Header.ReferralTypeID == $scope.referralTypeExternal) {
                        $scope.referralSourceLookUpType = LOOKUPTYPE.ReferralSource;
                    }

                }


                $scope.changeReferralOrganization = function () {
                    $scope.Header.OtherOrganization = '';
                }

                var resetHeader = function () {
                    $rootScope.formReset($scope.ctrl.referralMainForm.referralHeaderForm, $scope.ctrl.referralMainForm.referralHeaderForm.$name);
                    $rootScope.formReset($scope.ctrl.referralMainForm.referralBottomForm, $scope.ctrl.referralMainForm.referralBottomForm.$name);
                }
                var resetDemo = function () {
                    $rootScope.formReset($scope.ctrl.referralMainForm.referralDemographicsForm, $scope.ctrl.referralMainForm.referralDemographicsForm.$name);
                }

                var resetPhone = function () {
                    $rootScope.formReset($scope.ctrl.referralMainForm.referralPhoneForm, $scope.ctrl.referralMainForm.referralPhoneForm.$name);
                }

                var resetEmail = function () {
                    $rootScope.formReset($scope.ctrl.referralMainForm.referralEmailForm, $scope.ctrl.referralMainForm.referralEmailForm.$name);
                }
                var resetAddress = function () {
                    $rootScope.formReset($scope.ctrl.referralMainForm.referralAddressForm, $scope.ctrl.referralMainForm.referralAddressForm.$name);
                }
                var resetForm = function () {
                    resetHeader();
                    resetDemo();
                    resetPhone();
                    resetEmail();
                    resetAddress();
                    isEmailDirty = false;
                    isPhoneDirty = false;
                };

                resetFields = function () {
                    if ($scope.Header.ReferralTypeID == $scope.referralTypeExternal) {

                    } else if ($scope.Header.ReferralTypeID == $scope.referralTypeExternal) {

                    }
                }

                bindReferralSource = function () {
                    var referralSource = $rootScope.getLookupsByType($scope.referralSourceLookUpType);
                    if ($scope.Header.ReferralSourceID != null) {
                        $scope.ReferralSource = $filter('filter')(referralSource, { ID: $scope.Header.ReferralSourceID }, true)[0];
                    }

                    if ($scope.Header.ProgramID != null) {
                        $scope.ReferralSource = $filter('filter')(referralSource, { ID: $scope.Header.ProgramID }, true)[0];
                    }
                };

                $scope.get = function () {
                    $scope.isLoading = true;
                    var headerID = $stateParams.ReferralHeaderID;
                    referralHeaderService.getReferralHeader(headerID, $stateParams.ContactID).then(function (data) {
                        if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                            $scope.Header = data.DataItems[0];
                            setReferralSourceLookUpType();
                            $scope.initReferralParent($stateParams.ReferralHeaderID, $stateParams.ContactID);
                            var contactID = $scope.Header.ContactID;
                            var processQ = [];
                            processQ.push(getDemographics(contactID));
                            processQ.push(getAddress(contactID));
                            processQ.push(getEmails(contactID));
                            processQ.push(getPhones(contactID));
                            processQ.push(bindReferralSource());
                            $q.all(processQ).finally(function () {
                                resetForm();
                            });

                        } else {
                            alertService.error('Unable to get referral Requestor!');
                            resetForm();
                        };

                    },
                    function (errorStatus) {
                        alertService.error('Unable to get referralRequestor: ' + errorStatus);
                    }).finally(function () {
                        $scope.isLoading = false;
                        resetForm();
                    });
                };

                getDemographics = function (contactID) {
                    return registrationService.get(contactID).then(function (data) {
                        if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                            $scope.Demographics = data.DataItems[0];
                        }
                    });
                }



                $scope.next = function () {
                    $state.go("referrals.client", { ReferralHeaderID: $scope.Header.ReferralHeaderID, ContactID: $scope.Header.ContactID });
                };

                initHeader = function () {
                    $scope.Header = {};
                };

                initDemographics = function () {
                    $scope.Demographics = {};
                };

                /********************************************* Start Email Functions ********************************************/

                getEmails = function (contactID) {
                    initEmails();
                    contactEmailService.get(contactID, contactTypeID).then(function (data) {
                        if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                            $scope.Emails = data.DataItems;
                            setAddMinusButtons($scope.Emails);
                        }

                    }).finally(function () {
                        resetEmail();
                        if ($scope.isReadOnlyForm) {
                            $scope.Emails = $filter("filter")($scope.Emails, function (obj) {
                                obj.ShowPlusButton = false;
                                obj.ShowMinusButton = false;
                                return obj;
                            });
                        }
                    });
                }

                initEmails = function () {
                    $scope.Emails = [];
                    $scope.Emails.push(objEmail());
                };

                objEmail = function () {
                    var obj = {
                        Email: '',
                        ContactID: 0,
                        ContactEmailID: 0,
                        EmailPermissionID: null,
                        IsPrimary: true,
                        ShowPlusButton: true,
                        ShowMinusButton: true,
                        IsActive: true
                    };
                    return obj;
                }

                $scope.addNewEmail = function () {
                    globalObjectsService.setViewContent();
                    $scope.Emails = $filter("filter")($scope.Emails, function (obj) {
                        obj.ShowPlusButton = false;
                        return obj;
                    });
                    $scope.Emails.push(objEmail());
                }

                $scope.removeEmail = function (index) {
                    globalObjectsService.setViewContent();
                    $scope.Emails = removeControl($scope.Emails, index, 'ContactEmailID');
                    $scope.Emails = setAddMinusButtons($scope.Emails);
                }

                /* End Email Functions */

                /******************************************** Start Address Functions ***********************************************/

                getAddress = function (contactID) {
                    initAddress();
                    contactAddressService.get(contactID, contactTypeID).then(function (data) {
                        if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                            $scope.Addresses = [];
                            $scope.Addresses.push(data.DataItems[0]);
                        }
                    }).finally(function () {

                        resetAddress();
                    });
                };

                initAddress = function () {
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

                getPhones = function (contactID) {
                    initPhones();
                    contactPhoneService.get(contactID, contactTypeID).then(function (data) {
                        if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                            $scope.Phones = data.DataItems;
                            setAddMinusButtons($scope.Phones);
                        }
                    }).finally(function () {
                        resetPhone();
                        if ($scope.isReadOnlyForm) {
                            $scope.Phones = $filter("filter")($scope.Phones, function (obj) {
                                obj.ShowPlusButton = false;
                                obj.ShowMinusButton = false;
                                obj.HidePrimaryCheckbox = true;
                                return obj;
                            });
                        }
                    });
                }

                initPhones = function () {
                    $scope.Phones = [];
                    $scope.Phones.push(objPhone());

                };

                objPhone = function () {
                    var obj = {
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
                    return obj;
                }

                $scope.addNewPhone = function () {
                    globalObjectsService.setViewContent();
                    $scope.Phones = $filter("filter")($scope.Phones, function (obj) {
                        obj.ShowPlusButton = false;
                        return obj;
                    });
                    var priPhones = $filter("filter")($scope.Phones, function (obj) {
                        return (obj.IsPrimary == true);
                    });
                    var phoneToAdd = objPhone();
                    if (priPhones && priPhones.length > 0)
                        phoneToAdd.IsPrimary = false;
                    $scope.Phones.push(phoneToAdd);
                }

                $scope.removePhone = function (index) {
                    globalObjectsService.setViewContent();
                    $scope.Phones = removeControl($scope.Phones, index, 'ContactPhoneID');
                    $scope.Phones = setAddMinusButtons($scope.Phones);
                }

                /* End Phone Functions */

                removeControl = function (collection, index, pkID) {
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
                            isPhoneDirty = true;
                        }
                        else if (pkID.indexOf('Email') >= 0) {
                            isEmailDirty = true;
                        }
                    }
                    return mergedCollection(collection, newCollection);
                }
                setAddMinusButtons = function (model) {
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

                mergedCollection = function (collection, activeCollection) {
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


                $scope.validateDate = function () {
                    if ($scope.$parent.ReferralDate != undefined && $scope.$parent.ReferralDate != null && $scope.$parent.ReferralDate !== '') {
                        var date = new Date($scope.$parent.ReferralDate);
                        if (date > $scope.$parent.endDate) {
                            $scope.referralStatusForm.referralDate.$setValidity('date', false);
                        }
                    } else {
                        $("input[name='referralDate']").removeClass('ng-invalid').removeClass('ng-invalid-date');
                    }
                };


                /******************************Save form data ******************************/
                $scope.save = function (isNext, mandatory, hasErrors, keepForm) {
                    if (!isSaving) {
                        var isMainDirty = formService.isDirty();
                        var isDirtyDemo = formService.isDirty($scope.ctrl.referralMainForm.referralDemographicsForm.$name);
                        if ((isMainDirty && !hasErrors) || isEmailDirty || isPhoneDirty) {
                            var Phones = $filter('filter')($scope.Phones, function (item) {
                                return (item.IsActive == true && item.Number !== '');
                            });
                            var EmailsCount = $filter('filter')($scope.Emails, function (item) {
                                return (item.IsActive == true && item.Email != '')
                            });
                            if ((Phones.length == 0) && (EmailsCount.length == 0)) {
                                alertService.error('Please add at least one contact method (Phone / Email)');
                                return;
                            }
                            var isAdd = ($scope.Demographics.ContactID === undefined || $scope.Demographics.ContactID === 0);
                            gotoNext = isNext;
                            isSaving = true;
                            isAdd ? addDemographics(isDirtyDemo) : isDirtyDemo ?
                                    registrationService.update($scope.Demographics).then(successMethod, errorMethod, notifyMethod) : successMethod({ data: { ID: $scope.Demographics.ContactID } });
                        }
                        else if (!isDirtyDemo && isNext) {
                            $scope.next();
                        }
                    }
                };

                addDemographics = function (isDirtyDemo) {
                    if (isDirtyDemo) {
                        $scope.Demographics.ContactID = 0;
                        $scope.Demographics.ContactTypeID = contactTypeID;
                        registrationService.add($scope.Demographics).then(successMethod, errorMethod, notifyMethod)
                    }
                    else {
                        isSaving = false;
                        alertService.error('Nothing to add in demographics');
                    }
                };

                successMethod = function (response) {
                    var isDirtyHeader = formService.isDirty($scope.ctrl.referralMainForm.referralHeaderForm.$name);
                    isDirtyHeader = isDirtyHeader || formService.isDirty($scope.ctrl.referralMainForm.referralBottomForm.$name)
                    var isAdd = ($scope.Header.ReferralHeaderID === undefined || $scope.Header.ReferralHeaderID === 0);
                    $scope.permissionID = response.data.ID;
                    if (response && (response.data.ID != 0 || $scope.Demographics.ContactID > 0)) {

                        if ($scope.Header.ReferralTypeID == $scope.referralTypeInternal && $scope.ReferralSource != null) {
                            $scope.Header.ProgramID = $scope.ReferralSource.ID;
                        }
                        else if ($scope.Header.ReferralTypeID == $scope.referralTypeExternal && $scope.ReferralSource != null) {
                            $scope.Header.ProgramID = null;
                            $scope.Header.ReferralSourceID = $scope.ReferralSource.ID;
                        } else {
                            $scope.Header.ProgramID = null;
                        }

                        $scope.Header.ReferralStatusID = $scope.$parent.referralStatusID;
                        $scope.Header.ReferralDate = $filter('formatDate')($scope.$parent.ReferralDate, 'MM/DD/YYYY');

                        if (isAdd) {
                            $scope.Demographics.ContactID = response.data.ID;
                            $scope.Header.ContactID = $scope.Demographics.ContactID;
                            $scope.Header.ReferralHeaderID = 0;
                            referralHeaderService.add($scope.Header).then(function (responseHeaderData) {
                                $scope.Header.ReferralHeaderID = responseHeaderData.ID;
                                addUpdateDetails(isAdd).then(function () {
                                    resetHeader();
                                    resetDemo();
                                });
                            },
                            function (error) { isSaving = false; alertService.error('OOPs something went wrong with Referral Header ' + error); });
                        }
                        else if (!isAdd) {
                            if (isDirtyHeader) {
                                referralHeaderService.update($scope.Header).then(function (responseHeaderData) {
                                    resetHeader();
                                    resetDemo();
                                },
                                function (error) { isSaving = false; alertService.error('OOPs something went wrong with Referral Header ' + error); });
                            }
                            addUpdateDetails(isAdd);
                        }
                        else if (response.ResultCode != 0) {
                            isSaving = false;
                            alertService.error('Unable to ' + (isAdd ? 'added' : 'updated') + ' Referral');
                            return;
                        }
                    }
                    else {
                        isSaving = false;
                        alertService.error('Unable to ' + (isAdd ? 'added' : 'updated') + ' Referral');
                        return;
                    }
                }

                errorMethod = function (err) {
                    isSaving = false;
                    alertService.error('OOPs something went wrong');
                };

                notifyMethod = function (notify) {
                    isSaving = false;
                    //TODO: if needed then implement for notifications
                };

                addUpdateDetails = function (isAdd) {
                    var deferred = $q.defer();
                    var isDirtyPhone = formService.isDirty($scope.ctrl.referralMainForm.referralPhoneForm.$name);
                    var isDirtyEmail = formService.isDirty($scope.ctrl.referralMainForm.referralEmailForm.$name);
                    var isDirtyAddress = formService.isDirty($scope.ctrl.referralMainForm.referralAddressForm.$name);
                    var promiseArray = [];

                    // filter out blank address
                    var addresses = $filter('filter')($scope.Addresses, function (item) {
                        if (item.ContactAddressID == 0 || item.ContactAddressID != 0 || item.AddressTypeID != null || item.Line1 != '' || item.Line2 != '' || item.City != '' || item.StateProvince != null || item.County != null || item.Zip != '') {
                            return true;
                        }
                        else {
                            return false;
                        }
                    });

                    if (isDirtyAddress && addresses != undefined && addresses != null && addresses.length > 0) {
                        angular.forEach($scope.Addresses, function (address) {
                            address.ContactID = $scope.Demographics.ContactID;
                            promiseArray.push(contactAddressService.addUpdate(address));
                        });
                    }

                    // filter out blank email
                    var emails = $filter('filter')($scope.Emails, function (item) {
                        if (item.Email != '' || item.ContactEmailID != 0) {
                            return true;
                        }
                        else {
                            return false;
                        }
                    });
                    if ((isDirtyEmail || isEmailDirty) && emails != undefined && emails != null && emails.length > 0) {
                        angular.forEach(emails, function (email) {
                            email.ContactID = $scope.Demographics.ContactID;
                            if (email.Email != '' || email.ContactEmailID != 0) {
                                if (email.ContactEmailID != 0 && (!email.Email || email.Email == ''))
                                    email.IsActive = false;
                                if (email.IsActive == true)
                                    promiseArray.push(contactEmailService.addUpdate(email));
                                else if (email.ContactEmailID != 0)
                                    promiseArray.push(contactEmailService.remove(email.ContactEmailID, email.ContactID));
                            }
                        });
                    }

                    // filter out blank phone
                    var phones = $filter('filter')($scope.Phones, function (item) {
                        if (item.Number != '' || item.ContactPhoneID != 0) {
                            return true;
                        }
                        else {
                            return false;
                        }
                    });
                    if ((isDirtyPhone || isPhoneDirty) && phones != undefined && phones != null && phones.length > 0) {
                        angular.forEach(phones, function (phone) {
                            phone.ContactID = $scope.Demographics.ContactID;
                            if ((phone.Number && phone.Number != '') || phone.ContactPhoneID != 0) {
                                if (phone.ContactPhoneID != 0 && (!phone.Number || phone.Number == ''))
                                    phone.IsActive = false;
                                if (phone.IsActive == true) {
                                    promiseArray.push(contactPhoneService.save(phone));
                                }
                                else if (phone.ContactPhoneID != 0) {
                                    promiseArray.push(contactPhoneService.remove(phone.ContactID, phone.ContactPhoneID));
                                }
                            }
                        });
                    }

                    $q.all(promiseArray).then(function () {
                        if (isAdd && gotoNext)
                            $scope.$parent.initNavigation(null, null, true);
                        else if (isAdd) {
                            var stateDetail = { stateName: currentUIState, validationState: 'valid' };
                            $rootScope.$broadcast('rightNavigationReferralHandler', stateDetail);
                        }
                        deferred.resolve();
                        resetForm();
                        alertService.success('Referral has been ' + (isAdd ? 'added' : 'updated') + ' successfully.');

                        if (gotoNext) {
                            $scope.next();
                        }
                        else {

                            if (isDirtyAddress)
                                getAddress($scope.Demographics.ContactID);

                            if (isDirtyEmail || isEmailDirty)
                                getEmails($scope.Demographics.ContactID);

                            if (isDirtyPhone || isPhoneDirty)
                                getPhones($scope.Demographics.ContactID);
                            isEmailDirty = false;
                            isPhoneDirty = false;
                            $state.transitionTo(currentUIState, { ContactID: $scope.Header.ContactID, ReferralHeaderID: $scope.Header.ReferralHeaderID, ReadOnly: 'edit' }, {
                                notify: false
                            });
                        }
                    },
                    function (error) {
                        alertService.error('OOPs something went wrong with Referral Contact Details' + error);
                        deferred.reject();
                    }).finally(function () {
                        $timeout(function () {
                            isSaving = false;
                        });
                    });;

                    return deferred.promise;
                }

                initScope();
            }
        ])
}());