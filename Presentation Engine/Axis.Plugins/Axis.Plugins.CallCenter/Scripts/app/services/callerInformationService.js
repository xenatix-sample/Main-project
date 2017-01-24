angular.module("xenatixApp")
    .factory("callerInformationService", ["$http", "$q", "settings", "offlineData", "assessmentService", "$filter", function ($http, $q, settings, offlineData, assessmentService, $filter) {
        var CONFIG = {
            apiControllerRoot: "/data/Plugins/CallCenter/CallerInformation/",
            offlineApiUrl: "/CallerInformation/",
            offlineCallerApiUrl: "/CallerInfo/", //Only used to update/get caller information
            offlineCallCenterAssessmentResponseApiUrl: "/CallerInfo/" //Only used to update/get caller information
        };

        var editStateFunc = function editStateSettings() {
            return {
                description: 'Caller Information ',
                state: 'callcenter.crisisline.callerinformation',
                stateParams: { ContactID: this.ContactID, CallCenterHeaderID: this.CallCenterHeaderID, ReadOnly: 'Edit' }
            };
        };

        function get(callCenterHeaderID, isCaller) {
            var dfd = $q.defer();
            var offlineUrl = isCaller ? CONFIG.offlineCallerApiUrl : CONFIG.offlineApiUrl
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetCallerInformation', {
                data: offlineData.getOfflineSettings(offlineUrl + (callCenterHeaderID || 0).toString(), 'CallerContactID', { childKey: 'CallCenterHeaderID' }), params: {
                    callCenterHeaderID: callCenterHeaderID
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

        function add(callCenter) {
            var dfd = $q.defer();
            if (!('CallCenterHeaderID' in callCenter))
                callCenter.CallCenterHeaderID = 0;
            var data = $.extend(true, {}, callCenter, offlineData.getOfflineSettings(CONFIG.offlineApiUrl + (callCenter.CallCenterHeaderID || 0).toString(), 'CallCenterHeaderID', { parentKey: 'CallCenterHeaderID', referenceKeys: ['CallerContactID', 'ClientContactID'], editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddCallerInformation', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function update(callCenter, isCaller) {
            var dfd = $q.defer();
            var offlineUrl = isCaller ? CONFIG.offlineCallerApiUrl : CONFIG.offlineApiUrl
            var data = $.extend(true, {}, callCenter, offlineData.getOfflineSettings(offlineUrl + (callCenter.CallCenterHeaderID || 0).toString(), 'CallCenterHeaderID', { parentKey: 'CallCenterHeaderID', referenceKeys: ['CallerContactID', 'ClientContactID'], editState: editStateFunc.toString() }));
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateCallerInformation', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };


        function updateModifiedOn(callCenterHeaderID) {
            var dfd = $q.defer();
            $http.put(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'UpdateModifiedOn', { CallCenterHeaderID: callCenterHeaderID, ModifiedOn: new Date() })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function getCallCenterAssessmentResponse(callCenterHeaderID, assessmentID) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetCallCenterAssessmentResponse', {
                data: offlineData.getOfflineSettings(CONFIG.offlineCallCenterAssessmentResponseApiUrl + (callCenterHeaderID || 0).toString(), 'CallerContactID', { childKey: 'CallCenterHeaderID' }), params: {
                    callCenterHeaderID: callCenterHeaderID,
                    assessmentID: assessmentID,
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


        function getCallCenterAssessmentResponseByContactID(contactID) {
            var dfd = $q.defer();
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'getCallCenterAssessmentResponseByContactID', {
                data: offlineData.getOfflineSettings(CONFIG.offlineCallCenterAssessmentResponseApiUrl + (contactID || 0).toString(), 'contactID', { childKey: 'contactID' }), params: {
                    contactID: contactID
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

        function addCallCenterAssessmentResponse(callCenter) {
            var dfd = $q.defer();
            if (!('CallCenterHeaderID' in callCenter))
                callCenter.CallCenterHeaderID = 0;
            var data = $.extend(true, {}, callCenter, offlineData.getOfflineSettings(CONFIG.offlineCallCenterAssessmentResponseApiUrl + (callCenter.CallCenterHeaderID || 0).toString(), 'CallCenterHeaderID', { parentKey: 'CallCenterAssessmentResponseID', referenceKeys: ['ResponseID', 'CallCenterHeaderID'], editState: editStateFunc.toString() }));
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddCallCenterAssessmentResponse', data)
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });

            return dfd.promise;
        };

        function ensureResponseID(callCenterHeaderID, contactID, assessmentID) {
            return getCallCenterAssessmentResponse(callCenterHeaderID, assessmentID).then(function (response) {
                if (hasData(response) && filterAssessmentResponse(response.DataItems, callCenterHeaderID).length > 0) {
                    return filterAssessmentResponse(response.DataItems, callCenterHeaderID)[0].ResponseID;
                }
                else {
                    var assessmentResponse = {
                        ResponseID: 0,
                        ContactID: contactID,
                        AssessmentID: assessmentID
                    };
                    return assessmentService.addAssessmentResponse(assessmentResponse).then(function (assessmentResponse) {
                        if (assessmentResponse.data.ResultCode === 0) {
                            var responseID = assessmentResponse.data.ID;
                            var callCenterAssessmentResponse = {
                                CallCenterHeaderID: callCenterHeaderID,
                                AssessmentID: assessmentID,
                                ResponseID: responseID
                            };
                            return addCallCenterAssessmentResponse(callCenterAssessmentResponse).then(function () {
                                return responseID;
                            });
                        }
                    });
                }
            });
        }

        function filterAssessmentResponse(items, callCenterHeaderID) {
            return $filter('filter')(items, { CallCenterHeaderID: callCenterHeaderID }, true);
        }

        return {
            get: get,
            add: add,
            getCallCenterAssessmentResponseByContactID:getCallCenterAssessmentResponseByContactID,
            update: update,
            updateModifiedOn: updateModifiedOn,
            getCallCenterAssessmentResponse: getCallCenterAssessmentResponse,
            addCallCenterAssessmentResponse: addCallCenterAssessmentResponse,
            ensureResponseID: ensureResponseID
        };
    }]);