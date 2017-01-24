(function () {
    angular.module('xenatixApp')
    .controller('lawLiaisonEnforcementController', ['$scope', '$filter', '$q', 'alertService', 'navigationService', '$stateParams', '$state', '$rootScope', 'formService', 'callerInformationService', 'registrationService', 'contactPhoneService', 'scopesService', 'contactSSNService', 'lookupService', '$timeout', 'cacheService', 'contactAddressService','WorkflowHeaderService',
       function ($scope, $filter, $q, alertService, navigationService, $stateParams, $state, $rootScope, formService, callerInformationService, registrationService, contactPhoneService, scopesService, contactSSNService, lookupService, $timeout, cacheService, contactAddressService, WorkflowHeaderService) {

           $scope.enableContactSearch = true;
           $scope.enableBreadCrum = true;
           $scope.isLawLiaison = true;
           $scope.isRequiredField = false;
           var userID = 0;
           var serviceName = 'Law Liaison';
           var OTHER_TXT = 'Other';
           $scope.CONTACT_TYPE = CONTACT_TYPE;
           $scope.endDate = new Date();
           $scope.startDate = $filter('calculate120years')();
           var ageLimit = DOB_AGE.MaxAge;
           var headerForm = 'programInfo';
           var lawLiaisonFollowUp = cacheService.get('lawLiaisonFollowUp');
           if (lawLiaisonFollowUp) {
               $scope.isFollowup = lawLiaisonFollowUp.followupRequired;
           }
           $scope.isReadOnly = cacheService.get('IsReadOnlyLLScreens');
           $scope.otherReferralAgency = REFERRAL_AGENCY.Other;

           navigationService.get().then(function (data) {
               userID = data.DataItems[0].UserID;
               $scope.headerDetails.ProviderID = userID;
           });

           $scope.headerDetails = {
               CallCenterHeaderID: $stateParams.CallCenterHeaderID ? $stateParams.CallCenterHeaderID : 0,
               ProviderID: userID,
               CallPriorityID: null,
               CallCenterTypeID: 2,
               ReasonCalled: '',
               CallStartTime: $filter('formatDate')(new Date(), 'MM/DD/YYYY hh:mm:ss A'),
               ModifiedOn: new Date(),
               ModifiedBy: userID,
               CallerContactID: null,
               ClientContactID: null,
               ContactTypeID: CONTACT_TYPE.New,
               ProgramUnitID: null,
               CountyID: null,
               SuicideHomicideID: null,
               CallCenterPriorityID: null,
               ReferralAgencyID: '',
               OtherReferralAgency: null,
               FollowUpRequired: false,
               ReferralAgencyName: ''
           };

           $scope.callerDetails = {
               ContactID: 0,
               FirstName: '',
               LastName: '',
               Middle: '',
               PreferredName: '',
               DOB: null,
               ContactTypeID: CONTACT_TYPE.Law_Liason,
               ModifiedOn: new Date()
           };

           $scope.clientDetails = {
               ContactID: 0,
               FirstName: '',
               LastName: '',
               Middle: '',
               PreferredName: '',
               ContactTypeID: null,
               ClientTypeID: null,
               DOB: null,
               GenderID: null,
               SSN: null
           };

           $scope.isOther = false;
           var anonymousText = 'Anonymous';
           $scope.changeContactType = function () {
               if ($scope.headerDetails.ContactTypeID == CONTACT_TYPE.Anonymous_Adult || $scope.headerDetails.ContactTypeID == CONTACT_TYPE.Anonymous_Child) {
                   if ($scope.clientDetails.FirstName.length <= 0) {
                       $scope.clientDetails.FirstName = anonymousText;
                   }
                   if ($scope.clientDetails.LastName.length <= 0) {
                       $scope.clientDetails.LastName = anonymousText;
                   }
               }
           };

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

           $scope.checkOther = function (value) {
               $scope.headerDetails.ReferralAgencyName = value.Name;
               $scope.headerDetails.ReferralAgencyID = value.ID;
               if ($scope.headerDetails.ReferralAgencyName == OTHER_TXT) {
                   $scope.isOther = true;
               }
               else {
                   $scope.isOther = false;
                   $scope.headerDetails.OtherReferralAgency = '';
               }
           };

           $scope.programUnitList = $filter('filter')(lookupService.getLookupsByType("ProgramUnit"), function (item) {
               return (item.ServiceName == serviceName)
           });

           if (hasDetails($scope.programUnitList))
               $scope.headerDetails.ProgramUnitID = $scope.programUnitList[0].ID;

           $scope.changeAgency = function () {
               if ($scope.headerDetails.ReferralAgencyName !== OTHER_TXT) {
                   $scope.isOther = false;
                   $scope.headerDetails.OtherReferralAgency = ''
               }
           }

           $scope.orderByPriority = function (state, viewValue) {
               var txt = $('#ReferralAgency').val();
               if (typeof state === 'object')
                   return state.Name.toLowerCase().indexOf(txt.toLowerCase());
           };

           var clearScopeServiceData = function () {
               scopesService.clear(LAWLIAISON_QUICK_REG_DATA.ClientData);
               scopesService.clear(LAWLIAISON_QUICK_REG_DATA.PhoneData);
           };

           $scope.save = function (isNext, mandatory, hasErrors, keepForm, next) {
               //Save Law Enforcement Officer details
               var isDirty = formService.isDirty() ||
                   formService.isDirty($scope.ctrl.lawEnforcementForm.AgencyForm.$name) ||
                   formService.isDirty($scope.ctrl.lawEnforcementForm.CallerForm.$name) ||
                   formService.isDirty($scope.ctrl.lawEnforcementForm.CallerPhoneForm.$name) ||
                   formService.isDirty($scope.ctrl.lawEnforcementForm.ContactTypeForm.$name) ||
                   formService.isDirty($scope.ctrl.lawEnforcementForm.ClientForm.$name) ||
                   formService.isDirty($scope.ctrl.lawEnforcementForm.ClientPhoneForm.$name) ||
                   formService.isDirty($scope.ctrl.lawEnforcementForm.ClientAddressForm.$name) ||
                   formService.isDirty(headerForm);
               if (isDirty && !hasErrors) {
                   var isAnonymousContact = (($scope.headerDetails.ContactTypeID == CONTACT_TYPE.Anonymous || $scope.headerDetails.ContactTypeID == CONTACT_TYPE.Anonymous_Adult || $scope.headerDetails.ContactTypeID == CONTACT_TYPE.Anonymous_Child)
                       && (($scope.clientDetails.FirstName === anonymousText || !$scope.clientDetails.FirstName) && ($scope.clientDetails.LastName === anonymousText || !$scope.clientDetails.LastName)));
                   if ((!$scope.clientDetails.ContactID) && !isAnonymousContact) {
                       registrationService.verifyDuplicateContacts($scope.clientDetails).then(function (data) {
                           if (hasData(data)) {
                               $scope.callDuplicateContactList = data.DataItems;
                           }
                           else
                               $scope.saveDetails(isNext);
                       });
                   }
                   else
                       $scope.saveDetails(isNext);
               }
               else if (!isDirty && isNext) {
                   $scope.next();
               }
           };

           $scope.onContactSearch = function (contactID) {
               $scope.headerDetails.ClientContactID = contactID;
               getContactDetails(contactID).finally(function () {
                   $scope.$parent.$parent['autoFocusReferralAgency'] = true; //for Focus
                   $scope.headerDetails.ContactTypeID = CONTACT_TYPE.Existing;
                   $timeout(function () {
                       //formService.initForm(false, $scope.ctrl.lawEnforcementForm.ClientForm.$name);
                       formService.initForm(true)
                       formService.initForm(true, $scope.ctrl.lawEnforcementForm.AgencyForm.$name);
                   });
               });
           }

           var get = function (callCenterHeaderID) {
               var dfr = $q.defer();
               callerInformationService.get(callCenterHeaderID).then(function (data) {
                   if (hasData(data)) {
                       $scope.headerDetails = data.DataItems[0];
                       $scope.isLinkToExistingContact = $scope.headerDetails.IsLinkedToContact;
                       if ($scope.headerDetails.ReferralAgencyID != null) {
                           $scope.headerDetails.ReferralAgencyName = lookupService.getText('ReferralAgency', $scope.headerDetails.ReferralAgencyID);
                           if ($scope.headerDetails.ReferralAgencyName == OTHER_TXT) {
                               $scope.isOther = true;
                           }
                       }
                       var getData = [];
                       getData.push(getCallerDetails($scope.headerDetails.CallerContactID));
                       getData.push(getContactDetails($scope.headerDetails.ClientContactID));
                       getData.push(getPhoneDetails($scope.headerDetails.CallerContactID, CONTACT_TYPE.Law_Liason));
                       $q.all(getData).then(function (data) {
                           resetForm();
                           dfr.resolve();
                       },
                        function (err) {
                            alertService.error('OOPs something went wrong ' + err + '.');
                        }).finally(function () {
                            setWorflowStatus(true);
                        });
                       $scope.$parent.$parent['autoFocusReferralAgency'] = true;
                   }
               }, function () {
                   setWorflowStatus(false);
               });

               return dfr.promise;
           }

           var getCallerDetails = function (contactID) {
               return registrationService.get(contactID).then(function (data) {
                   if (hasData(data)) {
                       var callerInformation = data.DataItems[0];
                       $scope.callerDetails.ContactID = callerInformation.ContactID;
                       $scope.callerDetails.FirstName = callerInformation.FirstName;
                       $scope.callerDetails.LastName = callerInformation.LastName;
                       $scope.callerDetails.Middle = callerInformation.Middle;
                       $scope.callerDetails.PreferredName = callerInformation.PreferredName;
                       getPhoneDetails($scope.callerDetails.ContactID, CONTACT_TYPE.Law_Liason)
                   }
               })
           }

           var getContactDetails = function (contactID) {
               var dfd = $q.defer();
               registrationService.get(contactID).then(function (data) {
                   if (hasData(data)) {
                       var contactData = data.DataItems[0];

                       if (contactData.DOB) {
                           contactData.DOB = $filter('formatDate')(contactData.DOB);
                       }
                       $rootScope.$broadcast('updateRegistrationData', { Data: contactData });
                       var isContactDirty;
                       if ($scope.ctrl.lawEnforcementForm.ClientForm) {
                           isContactDirty = formService.isDirty($scope.ctrl.lawEnforcementForm.ClientForm.$name);
                       }
                       
                       if (contactData.SSN && contactData.SSN.length > 0 && contactData.SSN.length < 9) {
                           contactSSNService.refreshSSN(contactID, contactData).then(function () {
                               $scope.clientDetails = contactData;
                               getContactOtherdetails($scope.clientDetails).then(function () {
                                   dfd.resolve($scope.clientDetails);
                                   if (!isContactDirty && $scope.ctrl.lawEnforcementForm.ClientForm)
                                       $rootScope.formReset($scope.ctrl.lawEnforcementForm.ClientForm, $scope.ctrl.lawEnforcementForm.ClientForm.$name);
                               });
                           });
                       }
                       else {
                           $scope.clientDetails = contactData;
                           getContactOtherdetails($scope.clientDetails).then(function () {
                               dfd.resolve($scope.clientDetails);
                               if (!isContactDirty && $scope.ctrl.lawEnforcementForm.ClientForm)
                                   $rootScope.formReset($scope.ctrl.lawEnforcementForm.ClientForm, $scope.ctrl.lawEnforcementForm.ClientForm.$name);
                           });
                       }
                   }
                   else {
                       dfd.resolve($scope.clientDetails);
                   }

               })
               return dfd.promise;
           }

           var getContactOtherdetails = function (model) {
               var dfd = $q.defer();
               var getDetails = [];
               getDetails.push(getContactPhoneDetails(model.ContactID, model.ContactTypeID));
               getDetails.push(getContactAddressDetails(model.ContactID, model.ContactTypeID));
               $q.all(getDetails).then(function (data) {
                   dfd.resolve(null);
               });
               return dfd.promise;
           }

           var getContactPhoneDetails = function (contactID, ContactTypeID) {
               return contactPhoneService.get(contactID, ContactTypeID).then(function (data) {
                   if (hasData(data)) {
                       $scope.clientPhone = getPrimaryOrLatestData(data.DataItems)[0];
                       if ($scope.ctrl.lawEnforcementForm.ClientPhoneForm)
                           $rootScope.formReset($scope.ctrl.lawEnforcementForm.ClientPhoneForm, $scope.ctrl.lawEnforcementForm.ClientPhoneForm.$name);
                   }
                   else {
                       $scope.clientPhone = null;
                   }
               })
           }

           var getContactAddressDetails = function (contactID, ContactTypeID) {
               return contactAddressService.get(contactID, ContactTypeID).then(function (data) {
                   if (hasData(data)) {
                       $scope.Addresses = getPrimaryOrLatestData(data.DataItems);
                       $scope.Addresses[0].IsGateCode = true;
                       $scope.Addresses[0].IsComplexName = true;
                   }
                   else {
                       $scope.Addresses = [{
                           AddressTypeID: ADDRESS_TYPE.ResidenceHome,
                           Line1: '',
                           Line2: '',
                           City: '',
                           StateProvince: null,
                           County: null,
                           Zip: '',
                           MailPermissionID: '',
                           IsPrimary: true,
                           IsGateCode: true,
                           IsComplexName: true
                       }];
                   }
               })
           }

           var getPhoneDetails = function (contactID, ContactTypeID) {
               return contactPhoneService.get(contactID, ContactTypeID).then(function (data) {
                   if (hasData(data)) {
                       $scope.callerPhone = data.DataItems[0];

                   }
               })
           }

           $scope.saveDetails = function (isNext) {
               var msg = ($scope.headerDetails.CallCenterHeaderID && $scope.headerDetails.CallCenterHeaderID != 0) ? 'updated' : 'saved';
               $scope.clientDetails.ModifiedOn = new Date();
               $scope.clientDetails.DOB = $scope.clientDetails.DOB ? $filter('formatDate')($scope.clientDetails.DOB) : null;
               $scope.headerDetails.ContactTypeID = $scope.headerDetails.ContactTypeID || CONTACT_TYPE.Law_Liason;
               var promiseArr = [];
               promiseArr.push(saveContact.call($scope.callerDetails, $scope.ctrl.lawEnforcementForm.CallerForm.$name));
               promiseArr.push(saveContact.call($scope.clientDetails, $scope.ctrl.lawEnforcementForm.ClientForm.$name, $scope.ctrl.lawEnforcementForm.ContactTypeForm.$name));

               $q.all(promiseArr).then(function (data) {
                   $scope.headerDetails.CallerContactID = data[0].ContactID;
                   $scope.headerDetails.ClientContactID = data[1].ContactID;


                   var detailPromise = [];

                   detailPromise.push(saveHeader());
                   detailPromise.push(savePhone.call($scope.callerPhone, $scope.ctrl.lawEnforcementForm.CallerPhoneForm.$name, $scope.headerDetails.CallerContactID));
                   detailPromise.push(savePhone.call($scope.clientPhone, $scope.ctrl.lawEnforcementForm.ClientPhoneForm.$name, $scope.headerDetails.ClientContactID));
                   detailPromise.push(saveClientAddress());
                   $q.all(detailPromise).then(function (data) {
                       resetForm();
                       callerInformationService.updateModifiedOn($scope.headerDetails.CallCenterHeaderID)
                       get($scope.headerDetails.CallCenterHeaderID).then(function () {
                           alertService.success('Law Liaison Enforcement has been ' + msg + ' successfully.');
                           //save workflow Header details.
                           //$rootScope.$broadcast('onWorkflowHeaderSave', { workflowDataKey: $state.current.data.workflowDataKey, workflowHeaderID: $scope.headerDetails.CallCenterHeaderID });
                           WorkflowHeaderService.AddWorkflowHeader({ WorkflowDataKey: $state.current.data.workflowDataKey, RecordHeaderID: $scope.headerDetails.CallCenterHeaderID, ContactID: $stateParams.ContactID });
                           //if (!$stateParams.ContactID)
                           changeState(isNext, $scope.headerDetails.CallCenterHeaderID, $scope.clientDetails.ContactID);

                       }, errorMsg);


                   });
               })
           };

           var errorMsg = function (error) {
               alertService.error('OOPs something went wrong ' + error + '.');
           }

           var saveContact = function (formName, contactTypeForm) {
               var currentContact = this;
               var contactDfd = $q.defer();
               if (formName == $scope.ctrl.lawEnforcementForm.CallerForm.$name) {
                   if (currentContact.FirstName.length === 0 && currentContact.LastName.length === 0) {
                       currentContact.FirstName = anonymousText;
                       currentContact.LastName = anonymousText;
                   }
               }
               if (contactTypeForm) {
                   var isDirty = formService.isDirty(formName) || formService.isDirty(contactTypeForm);
                   if (currentContact.FirstName.length === 0 && currentContact.LastName.length === 0) {
                       currentContact.FirstName = anonymousText;
                       currentContact.LastName = anonymousText;
                   }
               }
               else {
                   var isDirty = formService.isDirty(formName);
               }
               if (isDirty || $scope.headerDetails.CallCenterHeaderID == 0) {
                   if (currentContact.ContactID != 0 && isDirty) {
                       if (formName == $scope.ctrl.lawEnforcementForm.ClientForm.$name) {
                           if (currentContact.ContactTypeID != CONTACT_TYPE.Patient) {
                               currentContact.ContactTypeID = $scope.headerDetails.ContactTypeID;
                           }
                       }
                       registrationService.update(currentContact).then(function (data) {
                           contactDfd.resolve(currentContact);
                       })
                   }
                   else if (!currentContact.ContactID) {
                       if (formName == $scope.ctrl.lawEnforcementForm.ClientForm.$name) {
                           currentContact.ContactTypeID = $scope.headerDetails.ContactTypeID;
                       }
                       registrationService.add(currentContact).then(function (data) {
                           currentContact.ContactID = data.data.ID;
                           contactDfd.resolve(currentContact);
                       })
                   }
                   else {
                       contactDfd.resolve(currentContact);
                   }
               } else {//Incase of update 
                   contactDfd.resolve(currentContact);
               }
               return contactDfd.promise;
           }

           var saveHeader = function () {
               headerDfd = $q.defer();
               if (formService.isDirty() || formService.isDirty($scope.ctrl.lawEnforcementForm.AgencyForm.$name) || formService.isDirty($scope.ctrl.lawEnforcementForm.ContactTypeForm.$name) || formService.isDirty(headerForm)) {
                   $scope.headerDetails.ModifiedOn = new Date();
                   if ($scope.headerDetails.CallCenterHeaderID == 0) {
                       callerInformationService.add($scope.headerDetails).then(function (data) {
                           $scope.headerDetails.CallCenterHeaderID = data.ID;
                           headerDfd.resolve($scope.headerDetails);
                       });
                   }
                   else {
                       callerInformationService.update($scope.headerDetails).then(function (data) {
                           headerDfd.resolve($scope.headerDetails);
                       });
                   }
               } else {
                   headerDfd.resolve($scope.headerDetails);
               }

               return headerDfd.promise;
           }

           var saveClientAddress = function () {
               var addressDfd = $q.defer();
               if (formService.isDirty($scope.ctrl.lawEnforcementForm.ClientAddressForm.$name)) {
                   $scope.Addresses[0].ContactID = $scope.headerDetails.ClientContactID;
                   contactAddressService.addUpdate($scope.Addresses[0]).then(function (data) {
                       addressDfd.resolve(null);
                   })
               } else {
                   addressDfd.resolve(null);
               }
               return addressDfd.promise;

           }

           var savePhone = function (formName, contactID) {
               self = this;
               var phoneDfd = $q.defer();
               if (formService.isDirty(formName)) {
                   self.ContactID = contactID;
                   contactPhoneService.save(self).then(function (data) {
                       phoneDfd.resolve(null);
                   })
               } else {
                   phoneDfd.resolve(null);
               }
               return phoneDfd.promise;
           }

           $scope.next = function () {
               var stateParams = angular.copy($stateParams);
               angular.extend(stateParams, {
                   ContactID: $scope.clientDetails.ContactID,
                   CallCenterHeaderID: $stateParams.CallCenterHeaderID || $scope.headerDetails.CallCenterHeaderID
               });
               $scope.Goto('callcenter.lawliaison.screening', stateParams);
           };

           //$scope.ignoreReferralAgencyID = function (viewValue) {
           //    var filteredReferralAgency = [];
           //    angular.forEach($scope.ReferralAgencyName, function (ra) {
           //        filteredReferralAgency.push(ra);
           //    });
           //    return filteredFriends;
           //};

           var changeState = function (isNext, callCenterHeaderID, contactID) {
               var stateParams = angular.copy($stateParams);
               angular.extend(stateParams, { CallCenterHeaderID: callCenterHeaderID, ContactID: contactID });
               setWorflowStatus(true);

               $state.transitionTo('callcenter.lawliaison.lawenforcement', stateParams);
               if (isNext) {
                   $scope.next();
               }
           };

           var init = function () {
               initPhones();
               initAddress();
               if ($stateParams.CallCenterHeaderID != undefined) {
                   if ($scope.isFollowup) {
                       get(lawLiaisonFollowUp.parentCallCenterHeaderID);
                   }
                   else {
                       get($stateParams.CallCenterHeaderID);
                   }
               }
               else {
                   setWorflowStatus(false);
                   //Add watch for any model changes
                   clearScopeServiceData();
                   $scope.$watch('clientDetails', function (newvalue, oldValue) {
                       scopesService.store(LAWLIAISON_QUICK_REG_DATA.ClientData, angular.copy($scope.ctrl.lawEnforcementForm.ClientForm));
                   }, true);

                   $scope.$watch('clientPhone', function (newvalue, oldValue) {
                       scopesService.store(LAWLIAISON_QUICK_REG_DATA.PhoneData, angular.copy($scope.ctrl.lawEnforcementForm.ClientPhoneForm));
                   }, true);
               }
           };

           var initPhones = function () {
               $scope.callerPhone = defaultPhoneModel();
               $scope.clientPhone = defaultPhoneModel();
           };

           var defaultPhoneModel = function () {
               return {
                   ContactID: 0,
                   Index: 0,
                   PhoneTypeID: null,
                   Number: '',
                   PhonePermissionID: null,
                   EffectiveDate: $filter('formatDate')(new Date(), 'MM/DD/YYYY'),
                   ExpirationDate: null,
                   IsPrimary: true,
               };
           };

           var initAddress = function () {
               $scope.Addresses = [{
                   AddressTypeID: ADDRESS_TYPE.ResidenceHome,
                   Line1: '',
                   Line2: '',
                   City: '',
                   StateProvince: null,
                   County: null,
                   Zip: '',
                   MailPermissionID: '',
                   IsPrimary: true,
                   IsGateCode: true,
                   IsComplexName: true
               }];
           }

           var setWorflowStatus = function (value) {
               var stateDetail = {
                   stateName: 'callcenter.lawliaison.lawenforcement', validationState: value ? 'valid' : 'warning'
               };
               $rootScope.$broadcast('rightNavigationCallCenterHandler', stateDetail);
           }

           var resetForm = function () {
               $rootScope.formReset($scope.ctrl.lawEnforcementForm, $scope.ctrl.lawEnforcementForm.$name);
               $rootScope.formReset($scope.ctrl.lawEnforcementForm.AgencyForm, $scope.ctrl.lawEnforcementForm.AgencyForm.$name);
               $rootScope.formReset($scope.ctrl.lawEnforcementForm.CallerForm, $scope.ctrl.lawEnforcementForm.CallerForm.$name);
               $rootScope.formReset($scope.ctrl.lawEnforcementForm.CallerPhoneForm, $scope.ctrl.lawEnforcementForm.CallerPhoneForm.$name);
               $rootScope.formReset($scope.ctrl.lawEnforcementForm.ContactTypeForm, $scope.ctrl.lawEnforcementForm.ContactTypeForm.$name);
               $rootScope.formReset($scope.ctrl.lawEnforcementForm.ClientForm, $scope.ctrl.lawEnforcementForm.ClientForm.$name);
               $rootScope.formReset($scope.ctrl.lawEnforcementForm.ClientPhoneForm, $scope.ctrl.lawEnforcementForm.ClientPhoneForm.$name);
               $rootScope.formReset($scope.ctrl.lawEnforcementForm.ClientAddressForm, $scope.ctrl.lawEnforcementForm.ClientAddressForm.$name);
               $rootScope.formReset($scope.ctrl.lawEnforcementForm.programInfo, headerForm);
               $rootScope.setform(false);
           }

           init();

           $rootScope.$on('updateData', function (event, args) {
               getContactDetails(args.Data.ContactID);
           });

           //duplicat contact detection callback 
           $scope.callBackDuplicate = function (contactID) {
               $scope.LinkedContactID = contactID;
               $scope.headerDetails.IsLinkedToContact = true;
               $scope.clientDetails.ContactID = contactID;
               getContactDetails(contactID).then(function () {
                   // if duplicate contact(16971) disabled quick registration and contact type become existing.
                   if ($scope.clientDetails.MRN && $scope.headerDetails.IsLinkedToContact) {
                       $scope.isCallCenterConvertToRegistration = true;
                       $scope.headerDetails.ContactTypeID = CONTACT_TYPE.Existing;
                   }
                   $timeout(function () {
                       formService.initForm(true);
                       if ($scope.ctrl.lawEnforcementForm && $scope.ctrl.lawEnforcementForm.ClientForm)
                           formService.initForm(true, $scope.ctrl.lawEnforcementForm.ClientForm.$name);
                   });
               });
           };

       }]);
}());
