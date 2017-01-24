(function () {
    angular.module("xenatixApp")
        .controller("organizationStructureNavigationController", ["$scope", 'roleSecurityService',
    function ($scope, roleSecurityService) {
        $scope.organizationStructureWorkFlowOptions = { enableWorkflow: null };
        var init = function () {
            var workFlowItems = [];
            if (roleSecurityService.hasPermission(BusinessAdministrationPermissionKey.BusinessAdministration_Configuration_Company, PERMISSION.READ))
                workFlowItems.push({ title: "Company", stateName: "businessadministration.configuration.organizationStructure.company" });
            if (roleSecurityService.hasPermission(BusinessAdministrationPermissionKey.BusinessAdministration_Configuration_Division, PERMISSION.READ))
                workFlowItems.push({ title: "Divisions", stateName: "businessadministration.configuration.organizationStructure.division" });
            if (roleSecurityService.hasPermission(BusinessAdministrationPermissionKey.BusinessAdministration_Configuration_Program, PERMISSION.READ))
                workFlowItems.push({ title: "Programs", stateName: "businessadministration.configuration.organizationStructure.programs" });
            if (roleSecurityService.hasPermission(BusinessAdministrationPermissionKey.BusinessAdministration_Configuration_ProgramUnit, PERMISSION.READ))
                workFlowItems.push({ title: "Program Units", stateName: "businessadministration.configuration.organizationStructure.programUnits" });

            $scope.workFlowModel = {
                workFlowItems: workFlowItems
            };
        }

        init();
    }]);
}());