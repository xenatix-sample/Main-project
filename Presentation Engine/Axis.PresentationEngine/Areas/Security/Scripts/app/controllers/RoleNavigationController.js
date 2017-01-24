angular.module('xenatixApp')
    .controller('RoleNavigationController', [
        '$scope', '$rootScope','alertService',
        function ($scope, $rootScope, alertService) {
            
            $scope.init = function () {
                $scope.initNavigation();
            };
            $scope.GotoNext = function (state, para) {
                if (para.RoleId > 0) {
                    $scope.Goto("siteadministration.rolemanagement.role.assignmodules", { RoleId: para.RoleId });
                } else {
                    alertService.error('The required fields must be completed before moving to next screen ');
                }
            };
            $scope.initNavigation = function () {
                var workFlowItems = [{ title: "Role Details", stateName: "siteadministration.rolemanagement.role.roledetails", stateParams: "{ RoleId: $stateParams.RoleId, ReadOnly: $stateParams.ReadOnly }", isEnabled: false },
									 { title: "Assign Modules", stateName: "siteadministration.rolemanagement.role.assignmodules", stateParams: "{ RoleId: $stateParams.RoleId, ReadOnly: $stateParams.ReadOnly }", isEnabled: false }

                ];

                $scope.workFlowReady = false;
                $scope.OnWorkFlowReady = function (value) {
                    if (value === true) {
                        $scope.workFlowModel.workFlowItems = workFlowItems;
                        $scope.workFlowModel.Init();
                    }
                };
                $scope.$watch('workFlowReady', $scope.OnWorkFlowReady);
            };

           
        }
    ]);
