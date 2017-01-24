angular.module("xenatixApp")
    .controller("admissionDischargeController", ["$scope", "$rootScope", "$stateParams", 'roleSecurityService', function ($scope, $rootScope, $stateParams, roleSecurityService) {
        $scope.admissionWorkFlowOptions = { enableWorkflow: null };

        $scope.init = function () {
            var workFlowItems = [];
            if (roleSecurityService.hasPermission(GeneralPermissionKey.General_General_Admission, PERMISSION.READ))
                workFlowItems.push({ title: "Admission", stateName: "patientprofile.general.admissionDischarge.admission", stateParams: "{ ContactID:$stateParams.ContactID || 1 }", initActive: "admissionsDischarge.dischargeCompany", isEnabled: false });
            if (roleSecurityService.hasPermission(GeneralPermissionKey.General_General_ProgramUnitDischarge, PERMISSION.READ))
                workFlowItems.push({ title: "Discharge Program Unit", stateName: "patientprofile.general.admissionDischarge.dischargeProgramUnit", stateParams: "{ContactID:$stateParams.ContactID || 1 }", isEnabled: false });
            if (roleSecurityService.hasPermission(GeneralPermissionKey.General_General_CompanyDischarge, PERMISSION.READ))
                workFlowItems.push({ title: "Discharge Company", stateName: "patientprofile.general.admissionDischarge.dischargeCompany", stateParams: "{ContactID:$stateParams.ContactID || 1 }", isEnabled: false });

            $scope.workFlowModel = {};
            $scope.workFlowModel.workFlowItems = workFlowItems;
        };
        
        $scope.$on('rightNavigationAdmissionHandler', function (event, args) {
            if ($rootScope.workflowActions != undefined && $rootScope.workflowActions.hasOwnProperty(args.stateName)) {
                if ((args.stateName == "patientprofile.general.admissionDischarge.admission") && args.validationState == "valid") {
                    $scope.admissionWorkFlowOptions.enableWorkflow = null;
                }
                $rootScope.workflowActions[args.stateName].validationState = args.validationState;
            }
            $rootScope.$broadcast(args.stateName, { validationState: args.validationState });
        });

        $scope.init();
    }]);