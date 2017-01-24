angular.module("xenatixApp")
    .controller("consentsNavigationController", ["$scope", "$rootScope", "$stateParams", function ($scope, $rootScope, $stateParams) {
        $scope.concentsWorkFlowOptions = { enableWorkflow: null };
        $scope.init = function () {
            var workFlowItems = [{ title: "Agency", stateName: "patientprofile.consents.agency.agencyView", stateParams: "{ ContactID:$stateParams.ContactID }", initActive: "patientprofile.consents.agency.agencyView" }];
            $scope.workFlowModel = {};
            $scope.workFlowModel.workFlowItems = workFlowItems;
        };

        $scope.init();
    }]);