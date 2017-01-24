angular.module("xenatixApp")
    .controller("callCenterLawliaisonController", ["$scope", "$rootScope", "$stateParams", "$controller", "cacheService", function ($scope, $rootScope, $stateParams, $controller, cacheService) {
        $scope.callCenterWorkFlowOptions = { enableWorkflow: 0, enableAdditionalWorkflow: true };
        $controller('callCenterQuickRegistrationController', { $scope: $scope });
        $scope.init = function () {
            var workFlowItems = [
                { title: "Law Enforcement", stateName: "callcenter.lawliaison.lawenforcement", stateParams: "{ CallCenterHeaderID:$stateParams.CallCenterHeaderID,ContactID:$stateParams.ContactID }", initActive: "callcenter.lawliaison.initlawenforcement" },
                { title: "Law Liaison Screening", stateName: "callcenter.lawliaison.screening", stateParams: "{ CallCenterHeaderID:$stateParams.CallCenterHeaderID,ContactID:$stateParams.ContactID }" },
                { title: "Services", stateName: "callcenter.lawliaison.services", stateParams: "{ CallCenterHeaderID:$stateParams.CallCenterHeaderID,ContactID:$stateParams.ContactID }" },
                { title: "Progress Notes", stateName: "callcenter.lawliaison.progressnotes", stateParams: "{ CallCenterHeaderID:$stateParams.CallCenterHeaderID,ContactID:$stateParams.ContactID }" }           
            ];
            // commented for the release 1.1 - replace it back on the above along with array items.
            //            { title: "Appointment", stateName: "callcenter.lawliaison.appointment", stateParams: "{ CallCenterHeaderID:$stateParams.CallCenterHeaderID,ContactID:$stateParams.ContactID }" }
            $scope.workFlowModel = {};
            $scope.workFlowModel.workFlowItems = workFlowItems;
        };
        $scope.isReadOnly = cacheService.get('IsReadOnlyLLScreens');
        $scope.$on('quickRegMRN', function (event, args) {
            var lawLiaisonFollowUp = cacheService.get('lawLiaisonFollowUp');
            if (lawLiaisonFollowUp) {
                $scope.isCallCenterConvertToRegistration = lawLiaisonFollowUp.followupRequired ? true : false;
            }
            else {
                $scope.isCallCenterConvertToRegistration = args.Data;
            }
        });

        $scope.$on('rightNavigationCallCenterHandler', function (event, args) {
            if ($rootScope.workflowActions != undefined && $rootScope.workflowActions.hasOwnProperty(args.stateName)) {
                if (args.stateName == "callcenter.crisisline.callerinformation" && args.validationState == "valid") {
                    $scope.callCenterWorkFlowOptions.enableWorkflow = null;
                }
                else if (args.stateName == "callcenter.lawliaison.lawenforcement" && args.validationState == "valid") {
                    $scope.callCenterWorkFlowOptions.enableWorkflow = null;
                }
                else if (args.stateName == "callcenter.lawliaison.services") {
                    if (args.validationState !== VALIDATION_STATE.Valid) {
                        args.validationState = VALIDATION_STATE.Invalid;
                    }
                }
                $rootScope.workflowActions[args.stateName].validationState = args.validationState;
                }
            $rootScope.$broadcast(args.stateName, { validationState: args.validationState });
        });
        $scope.init();
    }]);
