angular.module('xenatixApp')
    .factory('socialRelationshipService', ["$http", "$q", 'settings', 'offlineData', 'lookupService', function ($http, $q, settings, offlineData, lookupService) {
        var CONFIG = {
            apiControllerRoot: "/data/plugins/Clinical/SocialRelationship/",
            offlineApiUrl: '/SocialRelationship/'
        };

        var editStateFunc = function editStateSettings() {
            return {
                description: 'Social Relationship ',
                state: 'patientprofile.chart.intake.socialrelationship',
                stateParams: { ContactID: this.ContactID }
            };
        };

        function getSocialRelationships(contactId) {
            var dfd = $q.defer();

            $http.get(
                settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetSocialRelationshipsByContact',
                {
                    data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactId || 0).toString(), 'ContactID', { childKey: 'SocialRelationshipID' }),
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

        function add(socialRelationship) {
            var dfd = $q.defer();
            if (!('SocialRelationshipID' in socialRelationship))
                socialRelationship.SocialRelationshipID = 0;
            var data = $.extend(true, {}, socialRelationship, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (socialRelationship.ContactID || 0).toString() + '/' + (socialRelationship.SocialRelationshipID || 0).toString(), 'SocialRelationshipID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddSocialRelationship', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function update(socialRelationship) {
            var dfd = $q.defer();

            var data = $.extend(true, {}, socialRelationship, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (socialRelationship.ContactID || 0).toString() + '/' + (socialRelationship.SocialRelationshipID || 0).toString(), 'SocialRelationshipID', { parentKey: 'ContactID', editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateSocialRelationship', data)
                .then(function (data, status, header, config) {
                    dfd.resolve(data);
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });

            return dfd.promise;
        };

        function remove(contactId, SocialRelationshipId) {
            var dfd = $q.defer();

            $http.delete(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'DeleteSocialRelationship', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (contactId || 0).toString() + '/' + (SocialRelationshipId || 0).toString(), 'SocialRelationshipID', { parentKey: 'ContactID' }), params: { Id: SocialRelationshipId, modifiedOn: moment.utc().format() } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        
        return {
            getSocialRelationships: getSocialRelationships,
            add: add,
            update: update,
            remove: remove
        };

    }]);
