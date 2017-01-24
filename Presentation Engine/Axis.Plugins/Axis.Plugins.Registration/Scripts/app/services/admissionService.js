(function () {

    angular.module('xenatixApp')
    .factory('admissionService', ["$http", "$q", 'settings', 'offlineData', 'navigationService', '$filter', function ($http, $q, settings, offlineData, navigationService, $filter) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/registration/Admission/",
            offlineApiUrl: '/Registration/Admission/'
        }
        var editStateFunc = function editStateSettings() {
            return {
                description: 'Admission',
                state: 'patientprofile.general.admission',
                stateParams: { ContactID: this.ContactID }
            };
        };


        var defaultCompanyID = 1;

        function get(contactID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetAdmission', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { childKey: 'ContactAdmissionID' }), params: { contactId: contactID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function add(admission) {
            var dfd = $q.defer();

            if (!('ContactAdmissionID' in admission))
                admission.ContactAdmissionID = 0;

            var data = $.extend(true, {}, admission, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (admission.ContactID || 0).toString() + '/' + (admission.ContactAdmissionID || 0).toString(), 'ContactAdmissionID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));

            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddAdmission', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function update(admission) {
            var dfd = $q.defer();

            var data = $.extend(true, {}, admission, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (admission.ContactID || 0).toString() + '/' + (admission.ContactAdmissionID || 0).toString(), 'ContactAdmissionID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateAdmission', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function initAdmission(contactID) {
            var dfd = $q.defer();
            navigationService.get().then(function (data) {
                var userID = data.DataItems[0].UserID;
                var defaultAdmission = {
                    ContactAdmissionID: 0,
                    ProgramUnitID: defaultCompanyID,
                    ContactID: contactID,
                    EffectiveDate: $filter('formatDate')(new Date(), 'MM/DD/YYYY HH:mm'),
                    UserID: userID,
                    IsDocumentationComplete: '',
                    Comments: '',
                    IsCompanyActiveForProgramUnit: 1,
                    CompanyType: '',
                    CompanyID: defaultCompanyID,
                    DivisionID: null,
                    ProgramID: null,
                    IsCompanyActive: 1
                };
                dfd.resolve(defaultAdmission);
            });
            return dfd.promise;
        };


        function admissionToCompany(contactID) {
            return initAdmission(contactID).then(add);
        };

        return {
            get: get,
            add: add,
            update: update,
            initAdmission: initAdmission,
            admissionToCompany: admissionToCompany
        };
    }]);


}());