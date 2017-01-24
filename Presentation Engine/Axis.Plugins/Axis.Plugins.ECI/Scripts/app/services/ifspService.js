angular.module("xenatixApp")
    .factory("ifspService", ["$http", "$q", "settings", "offlineData", function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: "/data/Plugins/ECI/IFSP/",
            offlineApiUrl: "/ECI/IFSP/"
        };

        var editStateFunc = function editStateSettings() {
            return {
                description: 'ECI IFSP ',
                state: 'patientprofile.chart.ifsps.ifsp.header',
                stateParams: { ContactID: this.ContactID, IFSPID: this.IFSPID }
            };
        };

        function getList(contactID) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "GetIFSPList", { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { childKey: 'IFSPID' }), params: { contactID: contactID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            }).error(function (data, status, header, config) {
                dfd.reject(status);
            });
            return dfd.promise;
        };

        function get(contactID, ifspID) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "GetIFSP", { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (ifspID || 0).toString(), 'IFSPID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'] }), params: { ifspID: ifspID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            }).error(function (data, status, header, config) {
                dfd.reject(status);
            });
            return dfd.promise;
        };

        function add(ifspDetails) {
            var dfd = $q.defer();
            if (!("IFSPID" in ifspDetails))
                ifspDetails.IFSPID = 0;
            var data = $.extend(true, {}, ifspDetails, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (ifspDetails.ContactID || 0).toString() + '/' + (ifspDetails.IFSPID || 0).toString(), 'IFSPID', { parentKey: 'ContactID', referenceKeys: ['ResponseID', 'ParentGuardians.ContactID'], editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddIFSP', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (data, status, header, config) {
                    dfd.notify(status);
                });
            return dfd.promise;
        }

        function update(ifspDetails) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, ifspDetails, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (ifspDetails.ContactID || 0).toString() + '/' + (ifspDetails.IFSPID || 0).toString(), 'IFSPID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'], editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateIFSP', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function remove(contactID, ifspID) {
            var dfd = $q.defer();
            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "RemoveIFSP",
                {
                    data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (ifspID || 0).toString(), 'IFSPID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'] }),
                    params: { ifspID: ifspID, modifiedOn: moment.utc().format() }
                })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });
            return dfd.promise;
        };

        function getIFSPMembers(contactID) {
            var dfd = $q.defer();
            var teamMembersOfflineApiUrl = '/ECI/IFSP/TeamMembers/';
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "GetIFSPMembers", { data: offlineData.getOfflineSettings(teamMembersOfflineApiUrl + (contactID || 0).toString()), params: { contactID: contactID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getIFSPParentGuardians(contactID) {
            var dfd = $q.defer();
            var teamMembersOfflineApiUrl = '/ECI/IFSP/ParentGuardians/';
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "GetIFSPParentGuardians", { data: offlineData.getOfflineSettings(teamMembersOfflineApiUrl + (contactID || 0).toString()), params: { contactID: contactID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            get: get,
            getList: getList,
            add: add,
            update: update,
            remove: remove,
            getIFSPMembers: getIFSPMembers,
            getIFSPParentGuardians: getIFSPParentGuardians
        };
    }]);
