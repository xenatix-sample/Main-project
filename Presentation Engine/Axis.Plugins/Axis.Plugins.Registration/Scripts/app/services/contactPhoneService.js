angular.module('xenatixApp')
.factory('contactPhoneService', ["$http", "$q", 'settings', 'offlineData', 'lookupService', '$timeout', function ($http, $q, settings, offlineData, lookupService, $timeout) {
    var CONFIG = {
        apiControllerRoot: "/data/plugins/registration/ContactPhone/",
        offlineApiUrl: '/Registration/ContactPhone/'
    };
    var editStateFunc = function editStateSettings() {
        return {
            description: 'Contact Phone ',
            state: 'patientprofile.general.phone',
            stateParams: { ContactID: this.ContactID }
        };
    };
    function get(contactID, contactTypeID) {
        var dfd = $q.defer();
        $timeout(function () {
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetContactPhones', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { childKey: 'ContactPhoneID' }), params: { contactID: contactID, contactTypeID: contactTypeID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });
        }, 500);//TODO : for offline multiple get

        return dfd.promise;
    };

    function save(contactPhone) {
        var dfd = $q.defer();
        var data = $.extend(true, {}, contactPhone, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactPhone.ContactID || 0).toString() + '/' + (contactPhone.ContactPhoneID || 0).toString(), 'ContactPhoneID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
        $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddUpdateContactPhone', data)
        .success(function (data) {
            dfd.resolve(data);
        })
        .error(function (data, status) {
            dfd.reject(status);
        });

        return dfd.promise;
    };

    function remove(contactID,contactPhoneId) {
        var dfd = $q.defer();
        var url = {
            data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (contactPhoneId || 0).toString(), 'ContactPhoneID', { parentKey: 'ContactID' }),
            params: { contactPhoneID: contactPhoneId, modifiedOn: moment.utc().format() }
        };

        $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "DeleteContactPhone", url)
        .success(function (data) {
            dfd.resolve(data);
        })
        .error(function (data, status) {
            dfd.reject(status);
        });

        return dfd.promise;
    };


    return {
        get: get,
        save: save,
        remove: remove
    };
}]);