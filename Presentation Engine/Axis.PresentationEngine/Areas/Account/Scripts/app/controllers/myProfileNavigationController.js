angular.module('xenatixApp')
    .controller('myProfileNavigationController', [
        '$scope', '$timeout',
        function ($scope, $timeout) {

            $scope.init = function () {
                $scope.initNavigation();
            };

            $scope.initNavigation = function () {
                var workFlowItems = [{ title: "User Profile", stateName: "myprofile.nav.profile", stateParams: "{}", isEnabled: false },
									 { title: "Security", stateName: "myprofile.nav.security", stateParams: "{}", isEnabled: false },
                                     { title: "Digital Signature", stateName: "myprofile.nav.digitalsignature", stateParams: "{}", isEnabled: false },
									 { title: "Credentials", stateName: "myprofile.nav.credentials", stateParams: "{}", isEnabled: false },
									 { title: "Division & Programs", stateName: "myprofile.nav.divisionprograms", stateParams: "{}", isEnabled: false },
									 { title: "Scheduling", stateName: "myprofile.nav.scheduling", stateParams: "{}", isEnabled: false },
									 { title: "Direct Reports", stateName: "myprofile.nav.directreports", stateParams: "{}", isEnabled: false },
                                     { title: "User Photos", stateName: "myprofile.nav.userphotos", stateParams: "{}", isEnabled: false }
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