angular.module('xenatixApp')
    .controller('eciRegistrationController', [
        '$scope', '$rootScope', '$state', '$q', '$stateParams', 'alertService', 'lookupService', '$filter', '$rootScope', 'formService', '$timeout', 'roleSecurityService',
        function ($scope, $rootScope, $state, $q, $stateParams, alertService, lookupService, $filter, $rootScope, formService, $timeout, roleSecurityService) {
            $scope.eciRegistrationWorkFlowOptions = { enableWorkflow: 0 };

            $scope.init = function () {
                $scope.initNavigation();
            };

            $scope.initNavigation = function () {
                var workFlowItems = [];
                if (roleSecurityService.hasPermission(ECIPermissionKey.ECI_Registration_Demographics, PERMISSION.READ))
                    workFlowItems.push({ title: "Demographics", stateName: "eciregistration.demographics", stateParams: "{ ContactID: $stateParams.ContactID }", initActive: "eciregistration.initialdemographics", isEnabled: false });
                if (roleSecurityService.hasPermission(ECIPermissionKey.ECI_Registration_AdditionalDemographics, PERMISSION.READ))
                    workFlowItems.push({ title: "Additional Demographics", stateName: "eciregistration.additionaldemographics", stateParams: "{ ContactID: $stateParams.ContactID }", isEnabled: false });
                if (roleSecurityService.hasPermission(ECIPermissionKey.ECI_Registration_Referral, PERMISSION.READ))
                    workFlowItems.push({ title: "Referral", stateName: "eciregistration.referral", stateParams: "{ ContactID: $stateParams.ContactID }", isEnabled: false });
                if (roleSecurityService.hasPermission(ECIPermissionKey.ECI_Registration_Collateral, PERMISSION.READ))
                    workFlowItems.push({ title: "Family/Collateral", stateName: "eciregistration.family", stateParams: "{ ContactID: $stateParams.ContactID }", isEnabled: false });
                if (roleSecurityService.hasPermission(ECIPermissionKey.ECI_Registration_Payors, PERMISSION.READ))
                    workFlowItems.push({ title: "Payors", stateName: "eciregistration.benefits", stateParams: "{ ContactID: $stateParams.ContactID }", isEnabled: false });
                if (roleSecurityService.hasPermission(ECIPermissionKey.ECI_Registration_HouseholdIncome, PERMISSION.READ))
                    workFlowItems.push({ title: "Household Income", stateName: "eciregistration.financial", stateParams: "{ ContactID: $stateParams.ContactID }", isEnabled: false });

                $scope.workFlowModel = {};
                $scope.workFlowModel.workFlowItems = workFlowItems;
            };

            $scope.$on('rightNavigationECIRegistrationHandler', function (event, args) {
                if ($rootScope.workflowActions != undefined && $rootScope.workflowActions.hasOwnProperty(args.stateName)) {
                    if ((args.stateName == "eciregistration.demographics" || args.stateName == "eciregistration.initialdemographics" || args.EnableAllWorkFlow === true) && args.validationState == "valid") {
                        $scope.eciRegistrationWorkFlowOptions.enableWorkflow = null;
                    }
                    if (args.stateName == "eciregistration.additionaldemographics" && args.validationState == "valid") {
                        $scope.eciRegistrationWorkFlowOptions.enableAdditionalWorkflow = true;
                    }
                    $rootScope.workflowActions[args.stateName].validationState = args.validationState;
                }
                $rootScope.$broadcast(args.stateName, { validationState: args.validationState });
            });

            $scope.init();
        }
    ]);