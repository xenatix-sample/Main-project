angular.module('xenatixApp')
    .factory('noteService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/Clinical/Note/",
            offlineApiUrl: '/Clinical/Note/'
        };


        var editStateFunc = function editStateSettings() {
            return {
                description: 'Note ',
                state: 'patientprofile.chart.intake.note',
                stateParams: { ContactID: this.ContactID, NTypeID: this.NTypeID }
            };
        };
        function getNotes(contactId) {
            var dfd = $q.defer();

            $http.get(
                settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetNotesAsync',
                {
                    data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactId || 0).toString(), 'ContactID', { childKey: 'NoteID' }),
                    params: { ContactID: contactId }
                })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function add(note) {
            var dfd = $q.defer();
            if (!('NoteID' in note))
                note.NoteID = 0;
            var data = $.extend(true, {}, note, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (note.ContactID || 0).toString() + '/0', 'NoteID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddNote', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function update(note) {
            var dfd = $q.defer();

            var data = $.extend(true, {}, note, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (note.ContactID || 0).toString() + '/' + (note.NoteID || 0).toString(), 'NoteID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateNote', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function updateNoteDetails(note) {
            var dfd = $q.defer();

            var data = $.extend(true, {}, note, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (note.ContactID || 0).toString() + '/Notes/' + (note.NoteID || 0).toString(), 'NoteID', { parentKey: 'NoteID', editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateNoteDetails', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function remove(contactId, NoteId) {
            var dfd = $q.defer();

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'DeleteNote', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactId || 0).toString() + '/' + (NoteId || 0).toString(), 'NoteID', { parentKey: 'ContactID' }), params: { Id: NoteId, modifiedOn: moment.utc().format() } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        var savedData = {}

        function setData(data) {
            savedData = data;
        }

        function getData(data) {
            return savedData;
        }

        return {
            getNotes: getNotes,
            add: add,
            update: update,
            remove: remove,
            updateNoteDetails: updateNoteDetails,
            setData: setData,
            getData: getData
        };

    }]);
