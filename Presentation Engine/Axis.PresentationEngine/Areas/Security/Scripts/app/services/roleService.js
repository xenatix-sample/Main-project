angular.module('xenatixApp')
    .factory('roleService', ["$http", "$q", 'settings', function ($http, $q, settings) {
        var CONFIG =  {
            controllerAction: "/security/role/"
        };

        function getRoles(roleName) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getRoles', { params: { roleName: roleName } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getRoleById(roleId) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getRoleById', { params: { id: roleId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function add(role) {
            var dfd = $q.defer();
            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'addRole', role)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function update(role) {
            var dfd = $q.defer();
            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'updateRole', role)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function remove(id) {
            var dfd = $q.defer();
            $http.delete(settings.webApiBaseUrl + CONFIG.controllerAction + 'deleteRole' , { params: { id: id, modifiedOn: moment.utc().format() } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getModule() {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getModule')
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function assignModuleToRole(role) {
            var dfd = $q.defer();
            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'assignModuleToRole', role)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getModuleByRoleId(roleId) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getModuleByRoleId', { params: { id: roleId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getAssignedPermissionByModuleId(moduleId, roleId) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getAssignedPermissionByModuleId', { params: { id: moduleId, roleId: roleId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getAssignedPermissionByFeatureId(featureId, roleId) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getAssignedPermissionByFeatureId', { params: { id: featureId, roleId: roleId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getFeaturePermissionByModuleId(moduleId, roleId) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getFeaturePermissionByModuleId', { params: { id: moduleId, roleId: roleId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getFeaturePermissionByRoleId(roleId) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getFeaturePermissionByRoleId', { params: { roleId: roleId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getModulePermissionByRoleId(roleId) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getModulePermissionByRoleId', { params: { roleId: roleId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function assignRolePermission(role) {
            var dfd = $q.defer();
            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'assignRolePermission', role)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            getRoles: getRoles,
            getRoleById: getRoleById,
            add: add,
            update: update,
            remove: remove,
            getModule: getModule,
            assignModuleToRole: assignModuleToRole,
            getModuleByRoleId: getModuleByRoleId,
            getAssignedPermissionByModuleId: getAssignedPermissionByModuleId,
            getAssignedPermissionByFeatureId: getAssignedPermissionByFeatureId,
            getFeaturePermissionByModuleId: getFeaturePermissionByModuleId,
            getFeaturePermissionByRoleId: getFeaturePermissionByRoleId,
            getModulePermissionByRoleId: getModulePermissionByRoleId,
            assignRolePermission: assignRolePermission
        };
    }]);