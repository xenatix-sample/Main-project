(function () {
    angular.module('xenatixApp')
    .factory('intakeFormsService', ["$http", "$q", "settings", "offlineData", function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/registration/IntakeForms/",
            offlineApiUrl: '/Registration/IntakeForms/'
        }

        var editStateFunc = function editStateSettings() {
            return {
                description: 'Forms ',
                state: 'patientprofile.intake.formnavi.forms',
                stateParams: { ContactID: this.ContactID, ContactFormsID: this.ContactFormsID, ReadOnly: 'Edit' }
            };
        };

        function getIntakeForm(contactFormsID) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetIntakeForm', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactFormsID || 0).toString(), 'contactFormsID', { childKey: 'contactFormsID' }), params: { contactFormsID: contactFormsID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function get(contactID) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetIntakeFormsByContactID', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { childKey: 'contactFormsID' }), params: { contactID: contactID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function add(FormsModel) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, FormsModel, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (FormsModel.ContactID || 0).toString() + '/' + (FormsModel.ContactFormsID || 0).toString(), 'ContactFormsID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'], editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddIntakeForms', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function update(FormsModel) {
            var dfd = $q.defer();
            var data = $.extend(true, {}, FormsModel, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (FormsModel.ContactID || 0).toString() + '/' + (FormsModel.ContactFormsID || 0).toString(), 'ContactFormsID', { parentKey: 'ContactID', referenceKeys: ['ResponseID'], editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateIntakeForms', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function remove(contactID, contactFormsID) {
            var dfd = $q.defer();
            var url = {
                data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (contactFormsID || 0).toString(), 'ContactFormsID', { parentKey: 'ContactID' }),
                params: { contactFormsID: contactFormsID, modifiedOn: moment.utc().format() }
            };

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "DeleteIntakeForms", url)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        return {
            getIntakeForm:getIntakeForm,
            get: get,
            add: add,
            update: update,
            remove: remove
        };
    }])

}());