angular.module('xenatixApp')
    .controller('eciAdditionalDemographicController', ['$scope', '$q', '$modal', '$filter', 'eciAdditionalDemographicService', 'alertService', 'lookupService', '$stateParams', '$state', '$rootScope', 'formService', 'navigationService', '$timeout', 'contactRaceService', '$controller', 'providersService',
 function ($scope, $q, $modal, $filter, eciAdditionalDemographicService, alertService, lookupService, $stateParams, $state, $rootScope, formService, navigationService, $timeout, contactRaceService, $controller, providersService) {
     $controller('raceDetailsController', { $scope: $scope });
     $scope.isLoading = true;
     $scope.contactName = '';
     $scope.otherOptions = Other_TYPE;
     $scope.$parent['autoFocus'] = true;
     $scope.permissionKey = $state.current.data.permissionKey;
     $scope.controlsVisible = true;
     $scope.providerKey = PROVIDER_KEY.ECI_Registration_AdditionalDemography;
     $scope.init = function () {
         $scope.contactID = $stateParams.ContactID;
         $scope.get();
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
             $scope.saveOnEnter = false;
             $timeout(function () {
                 var nextState = angular.element("xen-workflow-action[data-state-name] > li.list-group-item.active").parents('.work-flow').next().find("xen-workflow-action[data-state-name]");
                 $scope.controlsVisible = (nextState.length > 0);
             });
         }
         $scope.endDate = new Date();
     }

     var checkFormStatus = function (state) {
         var stateDetail = { stateName: "eciregistration.additionaldemographics", validationState: state.valid ? 'valid' : 'invalid' };
         $rootScope.$broadcast('rightNavigationECIRegistrationHandler', stateDetail);
     };

     $scope.resetDischargeDate = function () {
         if (!$scope.additionalDemographic.IsChildHospitalized)
             $scope.additionalDemographic.ExpectedHospitalDischargeDate = null;
     };
     
     var resetForm = function () {
         $rootScope.formReset($scope.ctrl.additionalDemographicForm);
         $scope.raceDetailsChanged = false;
     };

     $scope.getLookupsByType = function (typeName) {
         return lookupService.getLookupsByType(typeName);
     };
     var getServiceCoordinatorPhone = function (serviceCoordinatorID) {
         if (serviceCoordinatorID) {
             return providersService.getProviderbyid(serviceCoordinatorID).then(function (response) {
                 if (hasData(response)) {
                     var providerData = response.DataItems;
                     $scope.selectedCoordinatorPhone = providerData[0].Number;
                     $scope.additionalDemographic.ServiceCoordinatorPhoneID = providerData[0].ContactNumberID;
                     $scope.selectCoordinatorID = providerData[0].ID;
                 }
             })
         }
         else {
             return $scope.promiseNoOp();
         }
     };
     $scope.get = function () {
         return eciAdditionalDemographicService.getAdditionalDemographic($scope.contactID).then(function (data) {
             $scope.initAdditionalDemographics();
             if (hasData(data)) {
                 $scope.additionalDemographic = data.DataItems[0];
                 $rootScope.$broadcast('updateRegistrationData', { Type: 'MRN', Data: $scope.additionalDemographic.MRN });
                 if ($scope.additionalDemographic.ExpectedHospitalDischargeDate)
                     $scope.additionalDemographic.ExpectedHospitalDischargeDate = $filter('toMMDDYYYYDate')($scope.additionalDemographic.ExpectedHospitalDischargeDate, 'MM/DD/YYYY');

                 if ($scope.additionalDemographic.TransferDate)
                     $scope.additionalDemographic.TransferDate = $filter('toMMDDYYYYDate')($scope.additionalDemographic.TransferDate, 'MM/DD/YYYY');

                 var schoolDistrictTypeList = $rootScope.getLookupsByType(LOOKUPTYPE.SchoolDistrict);
                 angular.forEach(schoolDistrictTypeList, function (item) {
                     if (item.ID == $scope.additionalDemographic.SchoolDistrictID)
                         $scope.selectedSchoolDistrict = item.Name;
                 });
                 var eciAdditionalProm = [];
                 eciAdditionalProm.push($scope.getRace($scope.contactID));
                 eciAdditionalProm.push(getServiceCoordinatorPhone($scope.additionalDemographic.ServiceCoordinatorID));
                 $q.all(eciAdditionalProm).then(function () {
                     resetForm();
                 });
                 if (data.DataItems && data.DataItems.length === 1) {
                     checkFormStatus({ valid: data.DataItems[0].AdditionalDemographicID != 0 ? true : false });
                 }
                 else
                     checkFormStatus({ valid: false });
             } else {
                 $scope.additionalDemographic.AdditionalDemographicID = 0;
                 $scope.currentLoggedUserDetail();
             }
         },
             function (errorStatus) {
                 checkFormStatus({ valid: false });
                 alertService.error('Unable to get additional demography: ' + errorStatus);
             }).finally(function () {
                 var stateDetail = { stateName: $state.current.name, validationState: (($scope.additionalDemographic != null && $scope.additionalDemographic.AdditionalDemographicID != undefined && $scope.additionalDemographic.AdditionalDemographicID != 0) ? 'valid' : 'invalid') };
                 $rootScope.$broadcast('rightNavigationECIRegistrationHandler', stateDetail);
             });
     };

     $scope.$watch("serviceCoordinator", function (newValue, oldValue) {
         if (newValue == "") {
             $scope.additionalDemographic.ServiceCoordinatorPhoneID = null;
             $scope.selectedCoordinatorPhone = '';
             $scope.selectCoordinatorID = null;
         }
     })

     $scope.selectCoordinator = function (item) {
         if (item.ID) {
             providersService.getProviderbyid(item.ID).then(function (response) {
                 if (hasData(response)) {
                     var provider = response.DataItems[0];

                     $scope.additionalDemographic.ServiceCoordinatorPhoneID = provider.PhoneID;
                     $scope.selectedCoordinatorPhone = provider.Number;
                     $scope.selectCoordinatorID = item.ID;
                 }
             });
         }
     };

     $scope.initAdditionalDemographics = function () {
         $scope.additionalDemographic = {
             ReferralDispositionStatusID: 1, //show by default referral
             LanguageID: LANGUAGE.English //show by default english language
         };
     };

     $scope.currentLoggedUserDetail = function () {
         navigationService.get().then(function (data) {
             if (data.DataItems != undefined && data.DataItems.length > 0) {
                 var user = data.DataItems[0];
                 $scope.serviceCoordinator = user.UserFullName;
                 $scope.selectedCoordinatorPhone = user.ContactNumber;
                 $scope.additionalDemographic.ServiceCoordinatorPhoneID = user.ContactNumberID;
                 $scope.selectCoordinatorID = user.UserID;
                 resetForm();
             }
         });
     };

     $scope.add = function () {
         $scope.editMode = false;
         $scope.initAdditionalDemographics();
     };

     $scope.edit = function (additionalDemographic) {
         $scope.editMode = true;
         $scope.additionalDemographic = additionalDemographic;
     };

     $scope.cancel = function () {
         bootbox.confirm("You will lose the information entered.\n Do you want to continue?", function (result) {
             if (result == true) {
                 $scope.initAdditionalDemographics();
                 $scope.additionalDemographic.Name = $scope.contactName;
                 $scope.editMode = false;
                 $scope.ctrl.additionalDemographicForm.$setPristine();
                 $scope.$apply();
             }
         });
     };

     $scope.saveCommon = function (isUpdate) {
         $scope.additionalDemographic.ExpectedHospitalDischargeDate = $filter("formatDate")($scope.additionalDemographic.ExpectedHospitalDischargeDate);
         $scope.additionalDemographic.TransferDate = $filter("formatDate")($scope.additionalDemographic.TransferDate);

         if (!$scope.additionalDemographic.IsChildHospitalized)
             $scope.additionalDemographic.ExpectedHospitalDischargeDate = null;
         if (!$scope.additionalDemographic.IsTransfer)
             $scope.additionalDemographic.TransferFrom = $scope.additionalDemographic.TransferDate = null;

         if (isUpdate)
             return eciAdditionalDemographicService.updateAdditionalDemographic($scope.additionalDemographic);
         else
             return eciAdditionalDemographicService.addAdditionalDemographic($scope.additionalDemographic);
     };

     $scope.save = function (isNext, mandatory, hasErrors) {
         var activeRace = $filter('filter')($scope.selectedRaceDetails, function (item) {
             return item.IsActive;
         });
         setRaceErrorClass();
         if (activeRace.length == 0) {
             alertService.error('Please select atleast one Race.');
             return;
         }

         var isDirty = formService.isDirty();
         if ((isDirty && !hasErrors) || $scope.raceDetailsChanged) {
             // save
             $scope.additionalDemographic.ContactID = $scope.contactID;
             if ($scope.selectedSchoolDistrict)
                 $scope.additionalDemographic.SchoolDistrictID = $scope.selectedSchoolDistrict.ID;

             else
                 $scope.additionalDemographic.SchoolDistrictID = null;

             if (!$scope.additionalDemographic.ServiceCoordinatorID) {
                 $scope.selectedCoordinatorPhone = null;
                 $scope.additionalDemographic.ServiceCoordinatorPhoneID = null;
             }
             var isUpdate = $scope.additionalDemographic.AdditionalDemographicID != null && $scope.additionalDemographic.AdditionalDemographicID !== 0;
             $scope.saveCommon(isUpdate)
                 .then(isUpdate ? $scope.postSaveUpdate : $scope.postSaveAdd,
                     function (errorStatus) {
                         alertService.error('OOPS! Something went wrong');
                     },
                     function (notification) {
                         alertService.warning(notification);
                     })
                 .then(function () {
                     if ($scope.next != null && isNext)
                         $scope.next();
                 });
         } else if (!isDirty && isNext) {
             $scope.next();
         }
     };

     $scope.postSaveAdd = function (response) {
         return $scope.postSaveCommon(response, 'added');
     };

     $scope.postSaveUpdate = function (response) {
         return $scope.postSaveCommon(response, 'updated');
     };

     $scope.postSaveCommon = function (response, action) {
         var data = response.data;
         if (data.ResultCode !== 0) {
             alertService.error(data.ResultMessage);
             return $scope.promiseNoOp();
         } else {
             $scope.saveRace($scope.additionalDemographic.ContactID).then(function () {
                 $q.serial($scope.taskArray).then(function () {
                     if ($state.current.name.indexOf('patientprofile') === 0) {
                         $scope.$parent.getPatientProfileData();
                     }

                     return $scope.get().then(function () {
                         alertService.success('Additional Demographics has been ' + action + ' successfully.');
                         $scope.raceDetailsChanged = false;
                     });
                 },
                   function (error) {
                       $scope.isSaving = false;
                       alertService.error('OOPs something went wrong with Additional Demographics Details' + error);
                   });
             });
         }
     };

     $scope.next = function () {
         var nextState = angular.element("xen-workflow-action[data-state-name] > li.list-group-item.active").parents('.work-flow').next().find("xen-workflow-action[data-state-name]");
         if (nextState.length > 0) {
             $timeout(function () {
                 $rootScope.setform(false);
                 $scope.Goto(nextState.attr('data-state-name'), { ContactID: $scope.contactID });
             });
         }
     };

     $scope.init();
 }]);
