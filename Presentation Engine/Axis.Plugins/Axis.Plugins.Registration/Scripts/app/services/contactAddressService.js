(function () {

    angular.module('xenatixApp')
    .factory('contactAddressService', ["$http", "$q", 'settings', 'offlineData', 'lookupService', function ($http, $q, settings, offlineData, lookupService) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/registration/ContactAddress/",
            offlineApiUrl: '/Registration/ContactAddress/'
        }
        var editStateFunc = function editStateSettings() {
            return {
                description: 'Contact Address ',
                state: 'patientprofile.general.address',
                stateParams: { ContactID: this.ContactID }
            };
        };

        function get(contactID, contactTypeID) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetAddresses', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString(), 'ContactID', { childKey: 'ContactAddressID' }), params: { contactID: contactID, contactTypeID: contactTypeID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getReportModel(contactID, contactTypeID) {
            var dfd = $q.defer();
            var reportModel = {};
            get(contactID, contactTypeID).then(function (data) {
                if (hasData(data)) {
                    var addressData = getPrimaryOrLatestData(data.DataItems)[0];
                    reportModel.contactGateCode = addressData.GateCode ? addressData.GateCode : '';
                    reportModel.contactComplexName = addressData.ComplexName ? addressData.ComplexName : '';
                    reportModel.contactApartment = reportModel.contactComplexName + ', ' + reportModel.contactGateCode;
                    reportModel.contactAddressLine1 = addressData.Line1 ? addressData.Line1 : '';
                    reportModel.contactAddressLine2 = addressData.Line2 ? addressData.Line2 : '';
                    reportModel.contactCity = addressData.City ? addressData.City : '';
                    reportModel.contactState = addressData.StateProvince ? lookupService.getText('StateProvince', addressData.StateProvince) : '';
                    reportModel.contactCounty = addressData.County ? lookupService.getText('County', addressData.County) : '';
                    reportModel.contactZip = addressData.Zip ? addressData.Zip : '';
                }
                dfd.resolve(reportModel);
            });
            return dfd.promise;
        };

        function addUpdate(addressModel) {
            var dfd = $q.defer();           
            var data = $.extend(true, {}, addressModel, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (addressModel.ContactID || 0).toString() + '/' + (addressModel.ContactAddressID || 0).toString(), 'ContactAddressID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddUpdateAddress', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };


        function remove(contactAddressID, contactID) {
            
            var dfd = $q.defer();
            var url = {
                data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (contactAddressID || 0).toString(), 'ContactAddressID', { parentKey: 'ContactID' }),
                params: { "contactAddressID": contactAddressID, modifiedOn: moment.utc().format() }
            };

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + "DeleteAddress", url)
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
            getReportModel : getReportModel,
            addUpdate: addUpdate,
            remove: remove,
        };
    }]);


}());