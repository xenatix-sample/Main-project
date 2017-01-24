angular.module('xenatixApp')
    .controller('staffManagementNavigationController', [
        '$scope', '$rootScope', 'roleSecurityService',
        function ($scope, $rootScope, roleSecurityService) {
            $scope.init = function () {
                $scope.initNavigation();
            };

            $scope.initNavigation = function () {
                var workFlowItems = [];
                if (roleSecurityService.hasPermission(SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, PERMISSION.READ))
                    workFlowItems.push({ title: "User Details", stateName: "siteadministration.staffmanagement.user.details", stateParams: "{ UserID: $stateParams.UserID }", isEnabled: false });
                if (roleSecurityService.hasPermission(SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserRoles, PERMISSION.READ))
                    workFlowItems.push({ title: "User Roles", stateName: "siteadministration.staffmanagement.user.roles", stateParams: "{ UserID: $stateParams.UserID }", isEnabled: false });
                if (roleSecurityService.hasPermission(SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_Credentials, PERMISSION.READ))
                    workFlowItems.push({ title: "Credentials", stateName: "siteadministration.staffmanagement.user.credentials", stateParams: "{ UserID: $stateParams.UserID }", isEnabled: false });
                if (roleSecurityService.hasPermission(SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_DivisionPrograms, PERMISSION.READ))
                    workFlowItems.push({ title: "Division & Programs", stateName: "siteadministration.staffmanagement.user.divisionprogram", stateParams: "{ UserID: $stateParams.UserID }", isEnabled: false });
                if (roleSecurityService.hasPermission(SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_Scheduling, PERMISSION.READ))
                    workFlowItems.push({ title: "Scheduling", stateName: "siteadministration.staffmanagement.user.scheduling", stateParams: "{ UserID: $stateParams.UserID }", isEnabled: false });
                if (roleSecurityService.hasPermission(SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_BlockedTime, PERMISSION.READ))
                    workFlowItems.push( { title: "Blocked Time", stateName: "siteadministration.staffmanagement.user.blockedtime", stateParams: "{ UserID: $stateParams.UserID }", isEnabled: false });
                if (roleSecurityService.hasPermission(SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_DirectReports, PERMISSION.READ))
                    workFlowItems.push({ title: "Direct Reports", stateName: "siteadministration.staffmanagement.user.directreports", stateParams: "{ UserID: $stateParams.UserID }", isEnabled: false });
                if (roleSecurityService.hasPermission(SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserProfile, PERMISSION.READ))
                    workFlowItems.push({ title: "User Profile", stateName: "siteadministration.staffmanagement.user.profile", stateParams: "{ UserID: $stateParams.UserID }", isEnabled: false });
                if (roleSecurityService.hasPermission(SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserPhoto, PERMISSION.READ))
                    workFlowItems.push({ title: "User Photos", stateName: "siteadministration.staffmanagement.user.photos", stateParams: "{ UserID: $stateParams.UserID }", isEnabled: false });
                if (roleSecurityService.hasPermission(SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_AdditionalDetails, PERMISSION.READ))
                    workFlowItems.push({ title: "Additional Details", stateName: "siteadministration.staffmanagement.user.additionaldetails", stateParams: "{ UserID: $stateParams.UserID }", isEnabled: false });

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