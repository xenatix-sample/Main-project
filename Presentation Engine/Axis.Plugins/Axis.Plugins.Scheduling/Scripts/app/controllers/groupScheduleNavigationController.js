angular.module('xenatixApp')
    .controller('groupScheduleNavigationController', [
        '$scope', '$rootScope',
        function ($scope, $rootScope) {

            $scope.init = function () {
                $scope.initNavigation();
            };

            $scope.initNavigation = function () {
                var workFlowItems = [{ title: "Group Schedule", stateName: "scheduling.groupscheduling.details.groupschedule", stateParams: "{ GroupID: $stateParams.GroupID, ReadOnly: $stateParams.ReadOnly }", isEnabled: false },
									 { title: "Group Note", stateName: "scheduling.groupscheduling.details.groupnote", stateParams: "{ GroupID: $stateParams.GroupID, ReadOnly: $stateParams.ReadOnly }", isEnabled: false }
									    
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
			
            $scope.init();
        }
]);