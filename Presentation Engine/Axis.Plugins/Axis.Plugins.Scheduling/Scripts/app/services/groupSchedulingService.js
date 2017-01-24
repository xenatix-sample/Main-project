angular.module('xenatixApp')
    .factory('groupSchedulingService', ["$http", "$q", 'settings', function ($http, $q, settings) {
        var CONFIG = {
            controllerAction: "/data/plugins/scheduling/groupscheduling/"
        };

        function getGroupByID(groupID) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getGroupByID', { params: { GroupID: groupID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getGroupSchedulingResource(groupID) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getGroupSchedulingResource', { params: { GroupID: groupID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getAppointmentByGroupID(groupID) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'getAppointmentByGroupID', { params: { GroupID: groupID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getAllContactResourceNamesByAppointment(appointmentID) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.controllerAction + 'GetAllContactResourceNamesByAppointment', { params: { appointmentID: appointmentID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function addGroupData(group) {
            var dfd = $q.defer();
            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'AddGroupData', group)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function addResources(resources) {
            var dfd = $q.defer();
            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'AddResources', resources)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function updateGroupData(group) {
            var dfd = $q.defer();
            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'UpdateGroupData', group)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function updateResources(resources) {
            var dfd = $q.defer();
            $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'UpdateResources', resources)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function deleteGroupSchedulingResource(groupId, groupResourceId) {
            var dfd = $q.defer();
            $http.delete(settings.webApiBaseUrl + CONFIG.controllerAction + 'deleteGroupSchedulingResource', { params: { id: groupResourceId, modifiedOn: moment.utc().format() } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

            return dfd.promise;
        };

        return {
            getGroupByID: getGroupByID,
            getGroupSchedulingResource: getGroupSchedulingResource,
            getAppointmentByGroupID: getAppointmentByGroupID,
            getAllContactResourceNamesByAppointment: getAllContactResourceNamesByAppointment,
            addGroupData: addGroupData,
            addResources: addResources,
            updateGroupData: updateGroupData,
            updateResources: updateResources,
            deleteGroupSchedulingResource: deleteGroupSchedulingResource
        };
    }]);