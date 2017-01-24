angular.module('xenatixApp')
    .factory('socialRelationshipHistoryService', ["$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/Clinical/SocialRelationshipHistory/",
            offlineApiUrl: '/Clinical/SocialRelationshipHistory/',
            offlineDetailsApiUrl: '/Clinical/SocialRelationshipHistory/Details/'
        };

        var editStateFunc = function editStateSettings() {
            return {
                description: 'Social Relationship History',
                state: 'patientprofile.chart.intake.socialrelationship.socialrelationshiphistory',
                stateParams: { ContactID: this.ContactID, socialRelationshipID: this.socialRelationshipID }
            };
        };


        function get(contactID, socialRelationshipID) {
            var dfd = $q.defer();

            $http.get(
                settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetSocialRelationship',
                {
                    data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactID || 0).toString() + '/' + (socialRelationshipID || 0).toString(), 'ContactID', { childKey: 'SocialRelationshipID' }),
                    params: { socialRelationshipID: socialRelationshipID }
                })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getDetails(contactID, socialRelationshipID) {
            var dfd = $q.defer();

            $http.get(
                settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetSocialRelationshipDetails',
                {
                    data: offlineData.getOfflineSettings(CONFIG.offlineDetailsApiUrl + (contactID || 0).toString() + '/' + (socialRelationshipID || 0).toString(), 'SocialRelationshipID', { childKey: 'SocialRelationshipDetailID' }),
                    params: { socialRelationshipID: socialRelationshipID }
                })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function add(model) {
            var dfd = $q.defer();
            if (!('FamilyRelationshipID' in model))
                model.FamilyRelationshipID = 0;
            var data = $.extend(true, {}, model, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (model.ContactID || 0).toString() + '/' + (model.SocialRelationshipID || 0).toString(), 'SocialRelationshipID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddSocialRelationHistory', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function update(model) {
            var dfd = $q.defer();

            var data = $.extend(true, {}, model, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (model.ContactID || 0).toString() + '/' + (model.SocialRelationshipID || 0).toString(), 'SocialRelationshipID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateSocialRelationHistory', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function addDetail(model) {
            var dfd = $q.defer();

            model.SocialRelationshipDetailID = 0;
            var data = $.extend(true, {}, model, offlineData.getOfflineSettings(CONFIG.offlineDetailsApiUrl + (model.ContactID || 0).toString() + '/' + (model.SocialRelationshipID || 0).toString() + '/' + (model.SocialRelationshipDetailID || 0).toString(), 'SocialRelationshipDetailID', { parentKey: 'SocialRelationshipID', editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddSocialRelationshipDetail', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function updateDetail(model) {
            var dfd = $q.defer();

            var data = $.extend(true, {}, model, offlineData.getOfflineSettings(CONFIG.offlineDetailsApiUrl + (model.ContactID || 0).toString() + '/' + (model.SocialRelationshipID || 0).toString() + '/' + (model.SocialRelationshipDetailID).toString(), 'SocialRelationshipDetailID', { parentKey: 'SocialRelationshipID', editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateSocialRelationshipDetail', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function remove(contactId, socialRelationshipID, socialRelationshipDetailID) {
            var dfd = $q.defer();

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'DeleteSocialRelationHistory', { data: offlineData.getOfflineSettings(CONFIG.offlineDetailsApiUrl + (contactId || 0).toString() + '/' + (socialRelationshipID).toString() + '/' + (socialRelationshipDetailID || 0).toString(), 'SocialRelationshipDetailID', { parentKey: 'SocialRelationshipID' }), params: { Id: socialRelationshipDetailID, modifiedOn: moment.utc().format() } })
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
            getDetails: getDetails,
            add: add,
            update: update,
            addDetail: addDetail,
            updateDetail: updateDetail,
            remove: remove
        };

    }]);
