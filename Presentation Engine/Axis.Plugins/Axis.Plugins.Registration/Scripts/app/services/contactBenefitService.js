angular.module('xenatixApp')
    .factory('contactBenefitService', ["$http", "$q", 'settings', 'offlineData', '$filter', function ($http, $q, settings, offlineData, $filter) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/registration/ContactBenefit/",
            offlineApiUrl: '/Registration/Benefits/'
        };
        var editStateFunc = function editStateSettings() {
            return {
                description: 'Benefits ',
                state: 'patientprofile.benefits.benefits',
                stateParams: { ContactID: this.ContactID }
            };
        };

        function getPayors(searchText) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetPayors', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + encodeURI(searchText)), params: { searchText: searchText} })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };


        function get(contactId) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetContactBenefits', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactId || 0).toString(), 'ContactID', { childKey: 'ContactPayorID' }), params: { contactId: contactId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function add(benefit) {
            var dfd = $q.defer();
            if (!('ContactPayorID' in benefit))
                benefit.ContactPayorID = 0;
            benefit.ModifiedOn = moment().format("YYYY-MM-DD HH:mm:ss");
            var data = $.extend(true, {}, benefit, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (benefit.ContactID || 0).toString() + '/0', 'ContactPayorID', { parentKey: 'ContactID', referenceKeys: ['PolicyHolderID'], editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddContactBenefit', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function update(benefit) {
            var dfd = $q.defer();
            benefit.ModifiedOn = moment().format("YYYY-MM-DD HH:mm:ss");
            var data = $.extend(true, {}, benefit, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (benefit.ContactID || 0).toString() + '/' + (benefit.ContactPayorID || 0).toString(), 'ContactPayorID', { parentKey: 'ContactID', referenceKeys: ['PolicyHolderID'], editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateContactBenefit', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function remove(contactId, contactBenefitId) {
            var dfd = $q.defer();

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'DeleteContactBenefit', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactId || 0).toString() + '/' + (contactBenefitId || 0).toString(), 'ContactPayorID', { parentKey: 'ContactID' }), params: { contactBenefitId: contactBenefitId, modifiedOn: moment.utc().format() } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getPayorPlans(payorId) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetPayorPlans', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + 'Payor/' + (payorId || 0).toString()), params: { payorID: payorId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };


        function getGroupsAndAddressesForPlan(planId) {
            var dfd = $q.defer();

            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getGroupsAndAddressesForPlan', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + 'Plan/' + (planId || 0).toString()), params: { planID: planId } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getMedicaidNumber(contactID) {
            var benefitDeferred = $q.defer();
            get(contactID).then(function (data) {
                var MEDICAID_NUMBER = 'N/A';
                if (data && data.DataItems.length > 0) {
                    var payors = $filter('filter')(data.DataItems, function (itm) {
                        return itm.PayorName.toString().toLowerCase().indexOf('medicaid') > -1;
                    })
                    if (payors && payors.length > 0) {
                        MEDICAID_NUMBER = payors[0].PolicyID;
                    }
                }
                benefitDeferred.resolve(MEDICAID_NUMBER);
            });
            return benefitDeferred.promise;
        };

        return {
            get: get,
            getPayors:getPayors,
            add: add,
            update: update,
            remove: remove,
            getPayorPlans: getPayorPlans,
            getGroupsAndAddressesForPlan: getGroupsAndAddressesForPlan,
            getMedicaidNumber: getMedicaidNumber
        };
    }]);
