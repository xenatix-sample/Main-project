angular.module('xenatixApp')
    .factory('dischargeService', [
        "$http", "$q", 'settings', 'offlineData', '$state', function ($http, $q, settings, offlineData, $state) {
            var CONFIG = {
                apiControllerRoot: "/data/Plugins/Registration/ContactDischargeNote/",
                offlineApiUrl: '/Registration/ContactDischargeNote/'
            };

            var editStateFunc = function editStateSettings() {
                return {
                    description: 'Discharge',
                    state: $state.current.name,
                    stateParams: { ContactID: this.ContactID }
                };
            };

            function getDischargeNote(contactDischargeNoteID) {
                var dfd = $q.defer();
                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetContactDischargeNote', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactDischargeNoteID || 0).toString(), 'ContactAdmissionID'), params: { contactDischargeNoteID: contactDischargeNoteID} })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };

            function getDischargeNotes(contactDischargeNoteID,noteTypeID) {
                var dfd = $q.defer();
                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetContactDischargeNotes', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactDischargeNoteID || 0).toString(), 'ContactAdmissionID'), params: { contactDischargeNoteID: contactDischargeNoteID, noteTypeID: noteTypeID } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };

            function addContactDischargeNote(contactDischargeNote) {
                var dfd = $q.defer();
                var data = $.extend(true, {}, contactDischargeNote, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactDischargeNote.ContactDischargeNoteID || 0).toString(), 'ContactDischargeNoteID', { parentKey: 'ContactAdmissionID', editState: editStateFunc.toString() }));
                $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddContactDischargeNote', data)
                    .then(function (data, status, header, config) {
                        dfd.resolve(data);
                    }, function (data, status, header, config) {
                        dfd.reject(status);
                    }, function (notification) {
                        dfd.notify(notification);
                    });

                return dfd.promise;
            };



            function updateContactDischargeNote(contactDischargeNote) {
                var dfd = $q.defer();
                var data = $.extend(true, {}, contactDischargeNote, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactDischargeNote.ContactDischargeNoteID || 0).toString(), 'ContactDischargeNoteID', { parentKey: 'ContactAdmissionID', editState: editStateFunc.toString() }));
                $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateContactDischargeNote', data)
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };

            return {
                getDischargeNote: getDischargeNote,
                getDischargeNotes: getDischargeNotes,
                addContactDischargeNote: addContactDischargeNote,
                updateContactDischargeNote: updateContactDischargeNote
            };
        }]);