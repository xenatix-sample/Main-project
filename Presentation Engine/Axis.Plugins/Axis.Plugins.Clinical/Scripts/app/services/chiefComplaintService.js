angular.module('xenatixApp')
    .factory('chiefComplaintService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/Clinical/ChiefComplaint/",
            offlineApiUrl: '/Clinical/ChiefComplaint/'
        };

        var editStateFunc = function editStateSettings() {
            return {
                description: 'Chief Complaint Service',
                state: 'patientprofile.chart.intake.chiefcomplaint',
                stateParams: { ContactID: this.ContactID }
            };
        };

        function get(contactID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetChiefComplaint',
                {
                    data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(),
                        'ContactID',
                        { childKey: 'ChiefComplaintID' }),
                    params: { contactID: contactID }
                })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getList(contactID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetChiefComplaints',
                {
                    data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(),
                        'ContactID',
                        { childKey: 'ChiefComplaintID' }),
                    params: { contactID: contactID }
                })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function add(chiefComplaint) {
            var dfd = $q.defer();

            if (!('ChiefComplaintID' in chiefComplaint))
                chiefComplaint.ChiefComplaintID = 0;

            var data = $.extend(true, {}, chiefComplaint, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (chiefComplaint.ContactID || 0).toString() + '/' + (chiefComplaint.ChiefComplaintID || 0).toString(), 'ChiefComplaintID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));

            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddChiefComplaint', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function update(chiefComplaint) {
            var dfd = $q.defer();

            var data = $.extend(true, {}, chiefComplaint, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (chiefComplaint.ContactID || 0).toString() + '/' + (chiefComplaint.ChiefComplaintID || 0).toString(), 'ChiefComplaintID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateChiefComplaint', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };


        function deleteChiefComplaint(contactID, chiefComplaintID) {
            var dfd = $q.defer();
            var url = {
                data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (chiefComplaintID || 0).toString(), 'chiefComplaintID', { parentKey: 'ContactID' }),
                params: { chiefComplaintID: chiefComplaintID, modifiedOn: moment.utc().format() }
            };

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "deleteChiefComplaint", url)
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
            deleteChiefComplaint: deleteChiefComplaint
        };
    }]);
