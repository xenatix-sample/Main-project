angular.module('xenatixApp')
    .factory('clinicalAssessmentService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/Clinical/clinicalAssessment/",
            offlineApiUrl: '/Clinical/Assessment/'
        };

        var editStateFunc = function editStateSettings() {
            return {
                description: 'Clinical Assessment Service ',
                state: 'patientprofile.chart.intake.clinicalAssessment',
                stateParams: { ContactID: this.ContactID }
            };
        };

        function getClinicalAssessmentByContact(contactID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetClinicalAssessmentsByContact', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { childKey: 'ClinicalAssessmentID' }), params: { contactID: contactID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getClinicalAssessment(contactID, clinicalAssessmentID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetClinicalAssessments', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (clinicalAssessmentID || 0).toString(), 'ClinicalAssessmentID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'] }), params: { clinicalAssessmentID: clinicalAssessmentID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function add(assessment) {
            var dfd = $q.defer();
            if (!('ClinicalAssessmentID' in assessment))
                assessment.ClinicalAssessmentID = 0;
            var data = $.extend(true, {}, assessment, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (assessment.ContactID || 0).toString() + '/0', 'ClinicalAssessmentID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'], editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddAssessment', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function update(assessment) {
            var dfd = $q.defer();

            var data = $.extend(true, {}, assessment, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (assessment.ContactID || 0).toString() + '/' + (assessment.ClinicalAssessmentID || 0).toString(), 'ClinicalAssessmentID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'], editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateAssessment', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function remove(contactId, assessmentID) {
            var dfd = $q.defer();

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'Deleteassessment', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactId || 0).toString() + '/' + (assessmentID || 0).toString(), 'ClinicalAssessmentID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'] }), params: { Id: assessmentID, modifiedOn: moment.utc().format() } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            getClinicalAssessmentByContact: getClinicalAssessmentByContact,
            getClinicalAssessment:getClinicalAssessment,
            add: add,
            update: update,
            remove: remove
        };

    }]);
