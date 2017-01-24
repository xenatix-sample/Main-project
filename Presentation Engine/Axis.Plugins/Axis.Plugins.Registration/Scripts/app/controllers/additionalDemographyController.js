angular.module('xenatixApp')
    .controller('additionalDemographyController', ['$scope', '$modal', '$filter', 'additionalDemographyService', 'registrationService', 'alertService', 'lookupService', '$stateParams', '$state', '$rootScope', 'formService', '$q', '$controller', '$timeout',
 function ($scope, $modal, $filter, additionalDemographyService, registrationService, alertService, lookupService, $stateParams, $state, $rootScope, formService, $q, $controller, $timeout) {
     $controller('raceDetailsController', { $scope: $scope });
     $scope.isLoading = true;
     $scope.contactName = '';
     //TODO : will be replace with javascript enum
     $scope.permissionKey = $state.current.data.permissionKey;
     $scope.eciProgram = 1;
     $scope.otherEthnicityOption = 6;
     $scope.otherLegalstatusOption = 8;
     $scope.otherLanguagesOption = 12;
     $scope.otherCitizenshipOption = 3;
     $scope.otherEducationOption = 9;
     $scope.otherLivingArrangementOption = 9;
     $scope.otherVeteranStatusOption = VETERAN_STATUS.Other;
     $scope.otherEmploymentStatusOption = 6;
     $scope.otherReligionOption = 11;
     $scope.unemployedOption = Employment_Status.Unemployed;
     $scope.$parent['autoFocus'] = false; //for focusing race field
     $scope.$parent['autoFocusSchool'] = false; //for focusing school district field
     $scope.schoolDistrictAgeLimit = 22;
     $scope.schoolDistrictRequiredAgeLimit = 3;
     $scope.BeginDate = 'BeginDate';
     $scope.EndDate = 'EndDate';
     $scope.EmploymentBeginDate = 'EmploymentBeginDate';
     $scope.EmploymentEndDate = 'EmploymentEndDate';
     $scope.endDate = new Date();
     $scope.dateOptions = {
         formatYear: 'yy',
         startingDay: 1,
         showWeeks: false
     };
     $scope.controlsVisible = true;
     var legalStatusAge = 18;
     $scope.init = function () {
         $scope.contactID = $stateParams.ContactID;
         registrationService.get($scope.contactID).then(function (regdata) {
             if (regdata.DataItems && regdata.DataItems.length === 1) {
                 $scope.ProgramID = regdata.DataItems[0].ClientTypeID;
             }
         });
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

     }

     var checkFormStatus = function (state) {
         var stateDetail = { stateName: "registration.additional", validationState: state.valid ? 'valid' : 'invalid' };         
         $rootScope.$broadcast('rightNavigationRegistrationHandler', stateDetail);
     };

     resetForm = function () {
         $rootScope.formReset($scope.ctrl.additionalDemographicForm);
         $scope.raceDetailsChanged = false;
     };

     $scope.getLookupsByType = function (typeName) {
         return lookupService.getLookupsByType(typeName);
     };

     $scope.get = function () {
         $scope.isLoading = true;
         return additionalDemographyService.getAdditionalDemographic($scope.contactID).then(function (data) {
             $scope.additionalDemographic = {};
             if (hasData(data)) {
                 checkFormStatus({ valid: data.DataItems[0].AdditionalDemographicID != 0 ? true : false });
                 $scope.additionalDemographic = data.DataItems[0];
             }
             else {
                 checkFormStatus({ valid: false });
                 $scope.additionalDemographic.AdditionalDemographicID = 0;
             }
             $rootScope.$broadcast('updateRegistrationData', { Type: 'MRN', Data: $scope.additionalDemographic.MRN });
             $scope.getRace($scope.contactID);
             if (data.ResultMessage === 'OFFLINE') {
                 registrationService.get($scope.contactID).then(function (regdata) {
                     if ((hasData(regdata)) && ($scope.additionalDemographic.DOB !== regdata.DataItems[0].DOB)) {
                         $scope.additionalDemographic.DOB = regdata.DataItems[0].DOB;
                     }
                     $scope.postGet();
                 });
             } else {
                 $scope.postGet();
             }
         },
             function (errorStatus) {
                 $scope.isLoading = false;
                 checkFormStatus({ valid: false });
                 alertService.error('Unable to connect to server');
             });
     };

     $scope.postGet = function () {
         $scope.calculateAge();
         $scope.contactName = $scope.additionalDemographic.Name;

         if ($scope.additionalDemographic.FullCodeDNR !== undefined && $scope.additionalDemographic.FullCodeDNR != null) {
             $scope.additionalDemographic.FullCodeDNR = $scope.additionalDemographic.FullCodeDNR.toString();
         } else {
             $scope.additionalDemographic.FullCodeDNR = 'false';
         }

         if ($scope.additionalDemographic.LivingWill !== undefined && $scope.additionalDemographic.LivingWill != null) {
             $scope.additionalDemographic.LivingWill = $scope.additionalDemographic.LivingWill.toString();
         } else {
             $scope.additionalDemographic.LivingWill = null;
         }

         if ($scope.additionalDemographic.PowerOfAttorney !== undefined && $scope.additionalDemographic.PowerOfAttorney != null) {
             $scope.additionalDemographic.PowerOfAttorney = $scope.additionalDemographic.PowerOfAttorney.toString();
         } else {
             $scope.additionalDemographic.PowerOfAttorney = null;
         }

         if ($scope.additionalDemographic.AdditionalDemographicID == 0 || $scope.additionalDemographic.AdditionalDemographicID == undefined) {
             $scope.additionalDemographic.PrimaryLanguageID = LANGUAGE.English;
             $scope.additionalDemographic.SecondaryLanguageID = LANGUAGE.English;
             if ($scope.age >= legalStatusAge) {
                 $scope.additionalDemographic.LegalStatusID = LEGAL_STATUS.AdultNoGuardian;
             }
             else {
                 $scope.additionalDemographic.LegalStatusID = LEGAL_STATUS.Minor;
             }
         }

         if ($scope.additionalDemographic.EmploymentBeginDate)
             $scope.additionalDemographic.EmploymentBeginDate = $filter('toMMDDYYYYDate')($scope.additionalDemographic.EmploymentBeginDate, 'MM/DD/YYYY');

         if ($scope.additionalDemographic.EmploymentEndDate)
             $scope.additionalDemographic.EmploymentEndDate = $filter('toMMDDYYYYDate')($scope.additionalDemographic.EmploymentEndDate, 'MM/DD/YYYY');

         if ($scope.additionalDemographic.SchoolBeginDate)
             $scope.additionalDemographic.SchoolBeginDate = $filter('toMMDDYYYYDate')($scope.additionalDemographic.SchoolBeginDate, 'MM/DD/YYYY');

         if ($scope.additionalDemographic.SchoolEndDate)
             $scope.additionalDemographic.SchoolEndDate = $filter('toMMDDYYYYDate')($scope.additionalDemographic.SchoolEndDate, 'MM/DD/YYYY');

         $scope.isLoading = false;

         if ($scope.additionalDemographic.SchoolDistrictID != null) {
             var schoolDistrict = $scope.getLookupsByType('SchoolDistrict');
             $scope.noResults = !(schoolDistrict);
             angular.forEach(schoolDistrict, function (item) {
                 if (item.ID == $scope.additionalDemographic.SchoolDistrictID)
                     $scope.additionalDemographic.SchoolDistrict = item;
             });
         }
         if ($scope.age <= $scope.schoolDistrictAgeLimit) {
             if ($scope.$parent && 'autoFocusSchool' in $scope.$parent)
                 $scope.$parent['autoFocusSchool'] = true; //default focus should be school district when it is being displayed
         } else {
             if ($scope.$parent && 'autoFocus' in $scope.$parent)
                 $scope.$parent['autoFocus'] = true; //default focus should be race
         }
         $scope.isLoading = false;
         resetForm();
     };

     $scope.calculateAge = function () {
         if (($scope.additionalDemographic !== undefined) && ('DOB' in $scope.additionalDemographic)) {
             $scope.additionalDemographic.DOB = $filter('toMMDDYYYYDate')($scope.additionalDemographic.DOB, 'MM/DD/YYYY', 'useLocal');
             $scope.age = parseInt($filter('toAge')($scope.additionalDemographic.DOB));
         }
     };

     $scope.initAdditionalDemographics = function () {
         $scope.additionalDemographic = {};
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
                 if ($scope.age <= $scope.schoolDistrictAgeLimit) {
                     if ($scope.$parent && 'autoFocusSchool' in $scope.$parent)
                         $scope.$parent['autoFocusSchool'] = true; //default focus should be school district when it is being displayed
                 } else {
                     if ($scope.$parent && 'autoFocus' in $scope.$parent)
                         $scope.$parent['autoFocus'] = true; //default focus should be race
                 }
             }
         });
     };

     $scope.orderByPriority = function (state, viewValue) {
         var txt = $('#schoolDistrict').val();
         if (typeof state === 'object')
             return state.Name.toLowerCase().indexOf(txt.toLowerCase());
     };

     $scope.saveCommon = function (isUpdate) {
         if (isUpdate)
             return additionalDemographyService.updateAdditionalDemographic($scope.additionalDemographic);
         else
             return additionalDemographyService.addAdditionalDemographic($scope.additionalDemographic);
     };

     $scope.save = function (isNext, mandatory, hasErrors) {
         // if there are errors, the screen is optional, and the user is trying to go to the next screen, don't bother trying to save anything
         // Vipul Singhal - As per Json if user modified the optional screen and click enter then it should display the validation error 
         // and if user don't don any modification then user can move to next screen.
         if (!mandatory && isNext && hasErrors) {
             $scope.next();
         }

         if ((!mandatory && !hasErrors && !isNext) || (mandatory && !hasErrors && !isNext) || (!mandatory && !hasErrors && isNext) || (mandatory && !hasErrors && isNext)) {
             // save
             var activeRace = $filter('filter')($scope.selectedRaceDetails, function (item) {
                 return item.IsActive;
             });
             setRaceErrorClass();
             if (activeRace.length == 0) {
                 alertService.error('Please select atleast one Race.');
                 return;
             }
             var isDirty = formService.isDirty();

             $scope.additionalDemographic.ContactID = $scope.contactID;

             if ($scope.additionalDemographic.EmploymentBeginDate) {
                 $scope.additionalDemographic.EmploymentBeginDate = $filter("formatDate")($scope.additionalDemographic.EmploymentBeginDate);
             }

             if ($scope.additionalDemographic.EmploymentEndDate) {
                 $scope.additionalDemographic.EmploymentEndDate = $filter("formatDate")($scope.additionalDemographic.EmploymentEndDate);
             }

             if ($scope.additionalDemographic.SchoolBeginDate)
             {
                 $scope.additionalDemographic.SchoolBeginDate = $filter("formatDate")($scope.additionalDemographic.SchoolBeginDate);
             }

             if ($scope.additionalDemographic.SchoolEndDate)
             {
                 $scope.additionalDemographic.SchoolEndDate = $filter("formatDate")($scope.additionalDemographic.SchoolEndDate);
             }

             if ($scope.additionalDemographic.SchoolDistrict != null)
                 $scope.additionalDemographic.SchoolDistrictID = $scope.additionalDemographic.SchoolDistrict.ID;
             else
                 $scope.additionalDemographic.SchoolDistrictID = null;

             if ($scope.age > $scope.schoolDistrictAgeLimit) {
                 $scope.additionalDemographic.SchoolDistrictID = null;
             }

             if (!$scope.additionalDemographic.AdvancedDirective) {
                 $scope.additionalDemographic.AdvancedDirectiveTypeID = '';
             }

             if (!isDirty && !$scope.raceDetailsChanged && isNext) {
                 $scope.next();
             } else if (isDirty || $scope.raceDetailsChanged) {
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
             } else if ((!mandatory && hasErrors && isNext) || (mandatory && !hasErrors && isNext)) {
                 // don't save, just go to next screen
                 $scope.next();
             }
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

                     if ($state.current.name == 'patientprofile.general.additional') {
                         $scope.$parent.getPatientProfileData();
                     }
                     $scope.get().then(function () {
                         alertService.success('Additional Demographics has been ' + action + ' successfully.');
                         $scope.raceDetailsChanged = false;
                     });
                 },
                function (error) {
                    $scope.isSaving = false;
                    alertService.error('OOPs something went wrong with Referral Contact Details' + error);
                });
             });
         }
     };

     $scope.delete = function (id) {
         additionalDemographyService.deleteAdditionalDemographic(id).then(function (data) {
             alertService.success('Additional Demographics has been deleted.');
         },

         function (errorSttaus) {
             alertService.error('OOPS! Something went wrong');
         });
     };

     $scope.next = function () {
         checkFormStatus({ valid: true });
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
