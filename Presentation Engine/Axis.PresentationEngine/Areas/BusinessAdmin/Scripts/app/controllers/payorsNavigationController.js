(function () {
    angular.module("xenatixApp")
        .controller("payorsNavigationController", ["$scope", "$state", "helperService",
    function ($scope, $state, helperService) {
        $scope.payorsWorkFlowOptions = { enableWorkflow: null };
        var initPlanAddressNavigation = function () {
            var workFlowItems = [];
            workFlowItems.push({ title: "Plan Address", stateName: $state.current.name }
                );

            $scope.workFlowModel = {
                workFlowItems: workFlowItems
            };
        };
        var initPlanDetailsNavigation = function () {
            var workFlowItems = [];
            workFlowItems.push({ title: "Plan Details", stateName: $state.current.name }
                );

            $scope.workFlowModel = {
                workFlowItems: workFlowItems
            };
        };
        var initPayorDetailsNavigation = function () {
            var workFlowItems = [];
            workFlowItems.push({ title: "Payor Plan", stateName: $state.current.name }
                );

            $scope.workFlowModel = {
                workFlowItems: workFlowItems
            };
        };
        var initPayorsNavigation = function () {
            var workFlowItems = [];
            workFlowItems.push({ title: "Payors", stateName: $state.current.name }
                );

            $scope.workFlowModel = {
                workFlowItems: workFlowItems
            };
        };
        

        var init = function () {
            if (isThisCurrentState('plandetails.addressdetails') || isThisCurrentState('plandetails.initial')) {
                initPlanAddressNavigation();
            }
            else if (isThisCurrentState('payorplans.plandetails') || isThisCurrentState('payorplans.initial')) {
                initPlanDetailsNavigation();
            }
            else if (isThisCurrentState('payors.payorplans') || isThisCurrentState('payors.initial')) {
                initPayorDetailsNavigation();
            }
            else {
                initPayorsNavigation();
            }
        }

        var isThisCurrentState = function (stateName) {
            return helperService.isThisCurrentState(stateName);
        };
        init();
    }]);
}());