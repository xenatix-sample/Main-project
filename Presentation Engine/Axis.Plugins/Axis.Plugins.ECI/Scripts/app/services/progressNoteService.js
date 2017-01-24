angular.module("xenatixApp")
    .factory("progressNoteService", ["$http", "$q", "settings", "offlineData", function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: "/data/Plugins/ECI/ProgressNote/",
            offlineApiUrl: "/ECI/ProgressNote/"
        };
        var editStateFunc = function editStateSettings() {
            return {
                description: 'Progress Note ',
                state: 'patientprofile.chart.' + angular.lowercase(lookupService.getText('ProgressNoteType', this.NoteTypeID).replaceAll(' ','')),
                stateParams: { ContactID: this.ContactID, NoteTypeID: this.NoteTypeID, NoteHeaderID: this.NoteHeaderID }
            };
        };
        function getList(noteTypeId, contactId) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "GetProgressNotes", { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (noteTypeId || 0).toString() + '/' + (contactId || 0).toString(), 'NoteHeaderID', { childKey: 'NoteHeaderID' }), params: { noteTypeID: noteTypeId, contactID: contactId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            }).error(function (data, status, header, config) {
                dfd.reject(status);
            });
            return dfd.promise;
        };

        function get(noteHeaderId, noteTypeId, contactId) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "GetProgressNote", { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (noteTypeId || 0).toString() + '/' + (contactId || 0).toString() + '/' + (noteHeaderId || 0).toString(), 'NoteHeaderID', { parentKey: 'ContactID' }), params: { progressNoteId: noteHeaderId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            }).error(function (data, status, header, config) {
                dfd.reject(status);
            });
            return dfd.promise;
        };

        function add(noteHeader) {
            var dfd = $q.defer();
            if (!("NoteHeaderID" in noteHeader))
                noteHeader.NoteHeaderID = 0;
            var data = $.extend(true, {}, noteHeader, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (noteHeader.NoteTypeID || 0).toString() + '/' + (noteHeader.ContactID || 0).toString() + '/' + (noteHeader.NoteHeaderID || 0).toString(), 'NoteHeaderID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddProgressNote', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (data, status, header, config) {
                    dfd.notify(status);
                });
            return dfd.promise;
        }

        function update(noteHeader) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, noteHeader, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (noteHeader.NoteTypeID || 0).toString() + '/' + (noteHeader.ContactID || 0).toString() + '/' + (noteHeader.NoteHeaderID || 0).toString(), 'NoteHeaderID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateProgressNote', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function remove(noteTypeId, contactId, Id) {
            var dfd = $q.defer();
            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "DeleteProgressNote",
            {
                data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (noteTypeId || 0).toString() + '/' + (contactId || 0).toString() + '/' + (Id || 0).toString(), 'ProgressNoteID', { parentKey: 'ContactID' }),
                params: { Id: Id, modifiedOn: moment.utc().format() }
            }).success(function (data, status, header, config) {
                dfd.resolve(data);
            }).error(function (data, status, header, config) {
                dfd.reject(status);
            });
            return dfd.promise;
        };

        return {
            getList: getList,
            get: get,
            add: add,
            update: update,
            remove: remove
        };

    }]);
