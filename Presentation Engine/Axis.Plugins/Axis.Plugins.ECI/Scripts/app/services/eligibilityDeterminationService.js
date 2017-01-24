angular.module('xenatixApp')
    .factory('eligibilityDeterminationService', ["$http", "$q", 'settings', 'offlineData', 'lookupService', function ($http, $q, settings, offlineData, lookupService) {
        var CONFIG = {
            apiControllerRoot: '/data/Plugins/ECI/EligibilityDetermination/',
            offlineApiUrl: '/ECI/EligibilityDetermination/',
            offlineNoteApiUrl: '/ECI/EligibilityDetermination/Note/'
        };

        var editStateFunc = function editStateSettings() {
            return {
                description: 'Eligibility Determination ',
                state: 'patientprofile.chart.eligibilities.eligibility.notes',
                stateParams: { ContactID: this.ContactID }
            };
        };

        function get(contactID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetEligibilityDetermination', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { childKey: 'EligibilityID' }), params: { contactID: contactID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getEligibility(contactID, eligibilityID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetEligibility', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (eligibilityID || 0).toString(), 'EligibilityID', { parentKey: 'ContactID' }), params: { eligibilityID: eligibilityID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getContactEligibilityMembers(contactID) {
            var dfd = $q.defer();
            var eligibilityMembersOfflineApiUrl = '/ECI/EligibilityDetermination/EligibilityMembers/';

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetContactEligibilityMembers', { data: offlineData.getOfflineSettings(eligibilityMembersOfflineApiUrl + (contactID || 0).toString()), params: { contactID: contactID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getEligibilityNote(eligibilityID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl +CONFIG.apiControllerRoot + 'GetEligibilityNote', { data : offlineData.getOfflineSettings(CONFIG.offlineNoteApiUrl +(eligibilityID || 0).toString(), 'EligibilityID', { parentKey: 'ContactID' }), params: {
                eligibilityID: eligibilityID
            }
            })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function add(eligibility) {
            var dfd = $q.defer();
            if (!('EligibilityID' in eligibility))
                eligibility.EligibilityID = 0;
            var data = $.extend(true, {}, eligibility, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (eligibility.ContactID || 0).toString() + '/' + (eligibility.EligibilityID || 0).toString(), 'EligibilityID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));

            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddEligibility', data)
           .then(function (data, status, header, config) {
               dfd.resolve(data);
           }, function (data, status, header, config) {
               dfd.reject(status);
           }, function (notification) {
               dfd.notify(notification);
           });

            return dfd.promise;
        };

        function update(eligibility) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, eligibility, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (eligibility.ContactID || 0).toString() + '/' + (eligibility.EligibilityID || 0).toString(), 'EligibilityID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateEligibility', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function deactivate(contactID, eligibilityID) {
            var dfd = $q.defer();
            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'DeactivateEligibility', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (eligibilityID || 0).toString(), 'EligibilityID', { parentKey: 'ContactID' }), params: { eligibilityID: eligibilityID , modifiedOn: moment.utc().format() } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function saveEligibilityNote(eligibility) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, eligibility, offlineData.getOfflineSettings(CONFIG.offlineNoteApiUrl + (eligibility.EligibilityID || 0).toString(), 'EligibilityID', { parentKey: 'EligibilityID', editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'saveEligibilityNote', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        return {
            get: get,
            getEligibility: getEligibility,
            getContactEligibilityMembers: getContactEligibilityMembers,
            getEligibilityNote: getEligibilityNote,
            add: add,
            update: update,
            deactivate: deactivate,
            saveEligibilityNote: saveEligibilityNote
        };
    }]);
