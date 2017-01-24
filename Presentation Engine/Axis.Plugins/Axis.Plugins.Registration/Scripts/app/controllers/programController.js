angular.module('xenatixApp')
    .controller('programController', [
        '$scope', '$state', '$stateParams', '$modal', 'lookupService', '$timeout', '$filter', '$rootScope', 'roleSecurityService',
        function ($scope, $state, $stateParams, $modal, lookupService, $timeout, $filter, $rootScope, roleSecurityService) {
            $scope.init = function () {
                $scope.selectedText = 'Selected';
                $scope.unselectedText = 'Select';
                $scope.initProgram();
                $scope.filterPrograms();
                $scope.referralContactID = $stateParams.OtherContactID;

            };

            $scope.initProgram = function () {
                $scope.program = {};
                $scope.setWordsToNotInitialize();
                var hasECI = roleSecurityService.hasModulePermission('eci', PERMISSION.READ, PERMISSION_LEVEL.Company);
                var hasRegistration = roleSecurityService.hasModulePermission('registration', PERMISSION.READ, PERMISSION_LEVEL.Company);
                var clientTypeList;
                if (hasECI && hasRegistration)
                    clientTypeList = $scope.getLookupsByType('ClientType');
                else if (hasECI)
                    clientTypeList = $filter('securityFilter')($scope.getLookupsByType('ClientType'), 'Division', 'OrganizationDetailID', ECIPermissionKey.ECI_Registration_Demographics);
                else if (hasRegistration)
                    clientTypeList = $filter('securityFilter')($scope.getLookupsByType('ClientType'), 'Division', 'OrganizationDetailID', RegistrationPermissionKey.Registration_Demography);
                else
                    clientTypeList = [];
                
                $scope.clientTypeList = $filter('filter')(clientTypeList, function (data) {
                    return data.ID !== 0 && (roleSecurityService.hasModulePermission(data.Division == "ECS" ? 'eci' : 'registration', PERMISSION.READ, PERMISSION_LEVEL.Company));
                });

                $scope.changeClientTypeList();
                var idx = 0;
                angular.forEach($scope.clientTypeList, function (tmpProgram) {
                    tmpProgram['Abbreviation'] = tmpProgram.Division;
                    tmpProgram['InnerText'] = $scope.unselectedText;
                    idx === 0 ? tmpProgram['isFocused'] = true : tmpProgram['isFocused'] = false;
                    idx++;
                });
            };

            $scope.changeClientTypeList = function () {
                //if redirect from referral search don't show eci and always get contact id
                if ($stateParams.OtherContactID != undefined && $stateParams.ProgramID == "") {
                    $scope.clientTypeList = $filter('filter')($scope.clientTypeList, function (data) {
                        return data.ID !== 1; // 1 for eci program
                    });
                }

                // if redirect from call center search
                if ($stateParams.OtherContactID != undefined && $stateParams.ProgramID != "") { 
                    var unkownProgram = { Abbreviation: "UN", ID: 0, InnerText: "Select", Name: "Unknown", RegistrationState: "registration.initialdemographics", isFocused: false };
                    var checkUnknownAlreadyExists = $filter('filter')($scope.clientTypeList, function (data) {
                        return data.ID == 0;
                    });
                    if (checkUnknownAlreadyExists.length == 0)
                        $scope.clientTypeList.push(unkownProgram);
                }
            };

            $scope.getLookupsByType = function (typeName) {
                return lookupService.getLookupsByType(typeName);
            };

            $scope.setWordsToNotInitialize = function () {
                $scope.ignoreWords = [];
                $scope.ignoreWords.push('and');
            };

            $scope.GenerateAbbreviation = function (name) {
                var abbreviation = '';
                var arrWords = name.split(' ');
                for (var i = 0; i < arrWords.length; i++) {
                    if ($scope.ignoreWords.indexOf(arrWords[i]) > -1) {
                        arrWords.splice(i, 1);
                        i--;
                    } else {
                        abbreviation = abbreviation + arrWords[i].charAt(0);
                    }
                }

                return abbreviation;
            };

            $scope.filterPrograms = function () {
                //We may want to place this logic at a higher level so that it doesn't visit the program screen for a split second
                //ToDo: Add logic to filter the program list..this logic will set the isSingleProgramAvailable object
                $scope.isSingleProgramAvailable = { isSingleProgram: false, ClientTypeID: 4 };//default to a non-ECI ClientTypeID

                //if a single program is available...set the ClientTypeID to that value and trigger the click event on the next button
                if ($scope.isSingleProgramAvailable.isSingleProgram) {
                    $scope.program.ClientTypeID = $scope.isSingleProgramAvailable.ClientTypeID;
                    //trigger the click event on the next button
                    $timeout(function () {
                        angular.element('#btnNextState').trigger('click');
                    });
                }
            };

            $scope.init();
        }
    ]);