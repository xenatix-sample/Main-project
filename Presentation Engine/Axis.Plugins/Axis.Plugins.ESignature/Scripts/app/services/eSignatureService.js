angular.module('xenatixApp')
    .factory('eSignatureService', [
        "$http", "$q", 'settings', 'offlineData', function ($http, $q, settings, offlineData) {
            var CONFIG = {
                apiControllerRoot: "/data/plugins/eSignature/eSignature/",
                controllerAction: "/admin/admin/",
                offlineApiUrl: '/eSignature/'
            };

            function getDocumentSignatures(documentTypeId, documentId) {
                var dfd = $q.defer();
                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetDocumentSignatures', { data: offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (documentTypeId || 0).toString() + '/' + (documentId || 0).toString(), 'DocumentID', { childKey: 'DocumentEntitySignatureID' }), params: { documentTypeId: documentTypeId, documentId: documentId } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };

            function saveDocumentSignature(signatureModel) {
                var dfd = $q.defer();
                var data = $.extend(true, {}, signatureModel, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (signatureModel.DocumentTypeID || 0).toString() + '/' + (signatureModel.DocumentID || 0).toString() + '/' + (signatureModel.DocumentEntitySignatureID || 0).toString(), 'DocumentEntitySignatureID', { parentKey: 'DocumentID' }));
                $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'SaveDocumentSignature', data)
                    .then(function (data, status, header, config) {
                        dfd.resolve(data);
                    }, function (data, status, header, config) {
                        dfd.reject(status);
                    }, function (notification) {
                        dfd.notify(notification);
                    });

                return dfd.promise;
            };

            function addEntitySignature(userProfile) {
                var dfd = $q.defer();

                $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddEntitySignature', userProfile)
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };

            function updateEntitySignature(userProfile) {
                var dfd = $q.defer();

                $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateEntitySignature', userProfile)
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };

            function getEntitySignature(entityId, entityTypeId, signatureTypeId) {
                var dfd = $q.defer();

                $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetEntitySignature', { params: { entityId: entityId, entityTypeId: entityTypeId, signatureTypeId: signatureTypeId } })
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };

            function addSignature(signatureModel) {
                var dfd = $q.defer();

                $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddSignature', signatureModel)
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };

            function verifySecurityDetails(resetPassword) {
                var dfd = $q.defer();
                $http.post(settings.webApiBaseUrl + CONFIG.controllerAction + 'verifySecurityDetails', resetPassword)
                .success(function (data, status, header, config) {
                    dfd.resolve(data);
                })
                .error(function (data, status, header, config) {
                    dfd.reject(status);
                });

                return dfd.promise;
            };

            return {
                saveDocumentSignature: saveDocumentSignature,
                getDocumentSignatures: getDocumentSignatures,
                getEntitySignature: getEntitySignature,
                updateEntitySignature: updateEntitySignature,
                addEntitySignature: addEntitySignature,
                addSignature: addSignature,
                verifySecurityDetails: verifySecurityDetails
            };
        }]);