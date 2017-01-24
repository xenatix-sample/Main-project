angular.module('xenatixApp')
.factory('credentialSecurityService', ['$filter', "$http", "$q", 'scopesService', 'settings',
    function ($filter, $http, $q, scopesService, settings) {

        var servicedata ;
        var CONFIG =  {
            controllerAction: "/security/credentialsecurity/"
        };

        function getUserCredentialSecurity() {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getUserCredentialSecurity')

            .success(function (data, status, header, config) {
                servicedata = data;
                scopesService.store('localCredentialSecurityServiceData', data);
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function applyCredentialSecurity(isCacheRefresh) {
            if (isCacheRefresh==true)
            {
                getUserCredentialSecurity();
            }
            else {
                servicedata = scopesService.get('localCredentialSecurityServiceData');
            }
        };

        function hasCredentialPermission(credentialName, credentialActionID) {
            var permissions = $filter('filter')(servicedata.DataItems, { CredentialName: credentialName, CredentialActionID: credentialActionID });
            return permissions.length > 0;
        };

        function hasCredentialPermissionByForm(formName, credentialActionID) {
            var permissions = $filter('filter')(servicedata.DataItems, { CredentialActionForm: formName, CredentialActionID: credentialActionID });
            return permissions.length > 0;
        };

        function hasCredentialActionPermissionByForm(credentialName, formName, credentialActionID) {
            var permissions = $filter('filter')(servicedata.DataItems, { CredentialName: credentialName, CredentialActionForm: formName, CredentialActionID: credentialActionID });
            return permissions.length > 0;
        };

        function hasCredentialActionServicePermissionByForm(credentialName, formName, credentialActionID, serviceID) {
            var permissions = permissions = $filter('filter')(servicedata.DataItems, { CredentialName: credentialName, CredentialActionForm: formName, CredentialActionID: credentialActionID, ServicesID: serviceID });
            return permissions.length > 0;
        };

        return {
            getUserCredentialSecurity: getUserCredentialSecurity,
            applyCredentialSecurity: applyCredentialSecurity,
            hasCredentialPermission: hasCredentialPermission,
            hasCredentialPermissionByForm: hasCredentialPermissionByForm,
            hasCredentialActionPermissionByForm: hasCredentialActionPermissionByForm,
            hasCredentialActionServicePermissionByForm: hasCredentialActionServicePermissionByForm
        };
    }
]);
