(function () {
    angular.module('xenatixApp')
           .factory('helperService', ["$state", "$filter", "lookupService", 'alertService', 'formService', function ($state, $filter, lookupService, alertService, formService) {

               function isThisCurrentState(stateName) {
                   return $state.current.name.toLowerCase().indexOf(stateName) >= 0;
               };

               function replaceStateTitle(state, stateTitle) {
                   if (state && stateTitle) {
                       $state.get(state).title = stateTitle;
                   };
               };

               function getDate(date, defaultValue) {
                   return date ? $filter('toMMDDYYYYDate')(date) : defaultValue;
               };

               function getFormattedDate(date, defaultValue) {
                   return date ? $filter('formatDate')(date, 'MM/DD/YYYY') : defaultValue || '';
               }

               function getOrganizationDetails(type, id) {
                   if (id) {
                       var organization = $filter('filter')(lookupService.getLookupsByType('OrganizationDetails'), {
                           DataKey: type, ID: id
                       }, true);
                       return organization.length > 0 && organization[0].Name ? organization[0].Name : '';
                   }
                   else {
                       return $filter('filter')(lookupService.getLookupsByType('OrganizationDetails'), {
                           DataKey: type
                       }, true);
                   }
               }

               function validateSignature (formObj, dSignatureObj) {
                   var isSignatureValid = true;
                   if (formObj && formService.isDirty(formObj.$name) && dSignatureObj) {
                       $.each(dSignatureObj, function (key, value) {
                           if (!value.IsSigned) {
                               if (value.Password) {
                                   alertService.error('Document is not signed with valid credentials.');
                                   isSignatureValid = false;
                                   return false;
                               }
                               else if (value.CredentialID) {
                                   alertService.error('Please enter Digital Password.');
                                   isSignatureValid = false;
                                   return false;
                               }
                           }
                       })
                   }
                   return isSignatureValid;
               };

               function updateLookupList(activeList, allList, filterParams) {
                   var lastSavedData = $filter('filter')(activeList, filterParams, true);
                   if (!hasDetails(lastSavedData)) {
                       activeList.push($filter('filter')(allList, filterParams, true)[0]);
                   }
               };

               return {
                   isThisCurrentState: isThisCurrentState,
                   replaceStateTitle: replaceStateTitle,
                   getDate: getDate,
                   getFormattedDate: getFormattedDate,
                   getOrganizationDetails: getOrganizationDetails,
                   validateSignature: validateSignature,
                   updateLookupList: updateLookupList
               }             
           }]);
}());