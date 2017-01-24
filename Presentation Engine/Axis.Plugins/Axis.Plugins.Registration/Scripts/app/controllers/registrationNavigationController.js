angular.module('xenatixApp')
    .controller('registrationNavigationController', ['$scope', '$rootScope', 'roleSecurityService',
        function ($scope, $rootScope, roleSecurityService) {
            $scope.registrationWorkFlowOptions = { enableWorkflow: 0 };

            var initNavigation = function () {
                var workFlowItems = [];
                if (roleSecurityService.hasPermission(RegistrationPermissionKey.Registration_Demography, PERMISSION.READ))
                    workFlowItems.push({ title: "Demographics", stateName: "registration.demographics", stateParams: "{ ContactID: $stateParams.ContactID }", initActive: "registration.initialdemographics" });
                if (roleSecurityService.hasPermission(RegistrationPermissionKey.Registration_AdditionalDemography, PERMISSION.READ))
                    workFlowItems.push({ title: "Additional Demographics", stateName: "registration.additional", stateParams: "{ ContactID: $stateParams.ContactID }" });
                if (roleSecurityService.hasPermission(RegistrationPermissionKey.Registration_Registration_Referral, PERMISSION.READ))
                    workFlowItems.push({ title: "Referral", stateName: "registration.referral", stateParams: "{ ContactID: $stateParams.ContactID }" });
                if (roleSecurityService.hasPermission(RegistrationPermissionKey.Registration_Collateral, PERMISSION.READ))
                    workFlowItems.push({ title: "Collateral", stateName: "registration.collateral", stateParams: "{ ContactID: $stateParams.ContactID }" });
                if (roleSecurityService.hasPermission(RegistrationPermissionKey.Registration_Payors, PERMISSION.READ))
                    workFlowItems.push({ title: "Payors", stateName: "registration.benefits", stateParams: "{ ContactID: $stateParams.ContactID }" });
                if (roleSecurityService.hasPermission(RegistrationPermissionKey.Registration_HouseholdIncome, PERMISSION.READ))
                    workFlowItems.push({ title: "Household Income", stateName: "registration.financial", stateParams: "{ ContactID: $stateParams.ContactID }" });

                $scope.workFlowModel = {
                    workFlowItems: workFlowItems
                };
            };

            $scope.$on('rightNavigationRegistrationHandler', function (event, args) {
                if ($rootScope.workflowActions != undefined && $rootScope.workflowActions.hasOwnProperty(args.stateName)) {
                    if (args.validationState == "valid" && (args.stateName == "registration.demographics" ||
                        args.stateName == "registration.initialdemographics" || args.EnableAllWorkFlow === true)) {
                        $scope.registrationWorkFlowOptions.enableWorkflow = null;
                    }
                    if ((args.stateName == "registration.additional" && args.validationState == "valid")) {
                        $scope.registrationWorkFlowOptions.enableAdditionalWorkflow = true;
                    }
                    $rootScope.workflowActions[args.stateName].validationState = args.validationState;
                }
                $rootScope.$broadcast(args.stateName, { validationState: args.validationState });
            });

            initNavigation();
        }]);