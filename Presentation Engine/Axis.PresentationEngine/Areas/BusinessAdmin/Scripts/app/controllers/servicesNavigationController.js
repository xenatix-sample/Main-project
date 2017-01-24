(function () {
    angular.module("xenatixApp")
        .controller("servicesNavigationController", ['$scope', '$rootScope','$state', 'roleSecurityService', 'helperService',
    function ($scope, $rootScope, $state, roleSecurityService, helperService) {
        $scope.servicesWorkFlowOptions = { enableWorkflow: 0 };
        var initServiceDetails = function () {
            var workFlowItems = [];
            if (roleSecurityService.hasPermission(BusinessAdministrationPermissionKey.BusinessAdministration_Configuration_Company, PERMISSION.READ)) {
                workFlowItems.push({ title: "Service Definition", stateName: "servicedefinition", stateParams: "{ ServicesID:$stateParams.ServicesID }", initActive: "servicedefinitioninitial" },
                    { title: "Service Details", stateName: "servicedetails", stateParams: "{ ServicesID:$stateParams.ServicesID }" });

                if (!($state.current.name == "servicedefinitioninitial")) {
                    //arrPromise = [];
                    //arrPromise.push(a.service(data));
                    //$q.all(arrPromise).then(function (responseData) {
                    //    var totalResponses = responseData.length;
                    //    for (iIdx = 0; iIdx < totalResponses; iIdx++) {
                    //        $rootScope.$broadcast(sectionStateName + responseData[iIdx].config.params.sectionId, { validationState: hasData(responseData[iIdx].data) ? 'valid' : 'warning' });
                    //    }
                    //}).finally(function () {
                    //    var stateDetail = { stateName: 'servicedefinition' };
                    //    $rootScope.$broadcast('rightNavigationServicesHandler', stateDetail);
                    //});
                    //var stateDetail = { stateName: 'servicedetails', validationState: 'valid' };
                    //$rootScope.$broadcast('rightNavigationServicesHandler', stateDetail);
                }

                $scope.workFlowModel = {
                    workFlowItems: workFlowItems
                };
            }
        }
        var initServices = function () {
            var workFlowItems = [];
            workFlowItems.push({ title: "Services", stateName: "businessadministration.configuration.services" });

            $scope.workFlowModel = {
                workFlowItems: workFlowItems
            };
        }
        var init = function () {
            if (helperService.isThisCurrentState('servicedefinition') || helperService.isThisCurrentState('servicedefinitioninitial') || helperService.isThisCurrentState('servicedetails')) {
                initServiceDetails();
            }
            else {
                initServices();
            }
        }

        $scope.$on('rightNavigationServicesHandler', function (event, args) {
            if ($rootScope.workflowActions != undefined && $rootScope.workflowActions.hasOwnProperty(args.stateName)) {
                if ((args.stateName == "servicedefinition" || args.stateName == "servicedetails") && args.validationState == VALIDATION_STATE.Valid) {
                    $scope.servicesWorkFlowOptions.enableWorkflow = null;
                }
                $rootScope.workflowActions[args.stateName].validationState = args.validationState;
            }
            $rootScope.$broadcast(args.stateName, { validationState: args.validationState });
        });

        init();
    }]);
}());