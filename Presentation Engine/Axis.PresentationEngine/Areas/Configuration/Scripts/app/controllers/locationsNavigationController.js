angular.module('xenatixApp')
    .controller('locationsNavigationController', [
        '$scope', '$rootScope',
        function ($scope, $rootScope) {

            $scope.init = function () {
                $scope.initNavigation();
            };

            $scope.initNavigation = function () {
                var workFlowItems = [{ title: "General Info", stateName: "siteadministration.configuration.locations.details.general", stateParams: "{ LocationID: $stateParams.LocationID }", initActive: "siteadministration.configuration.locations.details.general", isEnabled: false },
									    { title: "Phone Numbers", stateName: "siteadministration.configuration.locations.details.phonenumbers", stateParams: "{ LocationID: $stateParams.LocationID }", isEnabled: false },
									    { title: "Office Schedule", stateName: "siteadministration.configuration.locations.details.officeschedule", stateParams: "{ LocationID: $stateParams.LocationID }", isEnabled: false },
                                        { title: "Blocked Time", stateName: "siteadministration.configuration.locations.details.blockedtime", stateParams: "{ LocationID: $stateParams.LocationID }", isEnabled: false },
                                        { title: "Rooms", stateName: "siteadministration.configuration.locations.details.rooms", stateParams: "{ LocationID: $stateParams.LocationID }", isEnabled: false },
                                        { title: "Room Schedule", stateName: "siteadministration.configuration.locations.details.roomschedule", stateParams: "{ LocationID: $stateParams.LocationID }", isEnabled: false }
									    
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