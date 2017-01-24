angular
  .module('xenatixApp')
  .provider('httpLoaderInterceptor', function () {
      var exceptions = [];
      var ignoreLoading = false;
      var isLoading = false;

      this.exceptions = function (pattern) {
          exceptions.push(pattern);
      };

      this.$get = [
        '$q',
        '$rootScope',
        '$timeout',
        'auditService',
        '$stateParams',
        function ($q, $rootScope, $timeout, auditService, $stateParams) {
            var totalConcurrentRequests = 0;


            var isException = function (config) {
                var hasExceptionUrl = false;
                for (var i = 0; i < exceptions.length; i++) {
                    if (config.url.indexOf(exceptions[i]) != -1) {
                        hasExceptionUrl = true;
                        break;
                    }
                }
                return config.lazyLoader || hasExceptionUrl ? true : false;
            };

            var timer;
            var processResponse = function (config) {

                //add screem audit.
                addScreeAuditData(config);

                if (config && !isException(config) && (--totalConcurrentRequests === 0) && !ignoreLoading) {
                    $timeout(function () {
                        if (totalConcurrentRequests === 0) {
                            isLoading = false;
                            $rootScope.$emit('triggerLoader', {
                                method: config.method, isLoading: isLoading
                            });
                        }
                    });
                }
            };

            return {
                request: function (config) {

                    if (config && !isException(config) && (totalConcurrentRequests++ === 0) && !isLoading && !ignoreLoading) {

                        isLoading = true;
                        $rootScope.$emit('triggerLoader', {
                            method: config.method, isLoading: isLoading
                        });

                    }

                    return config || $q.when(config);
                },

                response: function (response) {
                    if (response)
                        processResponse(response.config);

                    return response || $q.when(response);
                },

                responseError: function (response) {
                    processResponse(response.config);

                    return $q.reject(response);
                },

                ignore: function (value) {
                    ignoreLoading = value;
                    isLoading = false;
                    if (value)
                        $rootScope.$emit('forceStopLoader');
                },

                loading: function () {
                    return isLoading;
                },

                setLoading: function (value) {
                    isLoading = value;
                }


            };

            function addScreeAuditData(config) {
                if (config && config.data && !isException(config) && !config.data.hasOwnProperty("PageLevelAuditLogID") && (config.method !== "GET")) {

                    if ($.isPlainObject(config.data)) {
                        auditService.auditScreenModel.ContactID = $stateParams.ContactID;
                        auditService.auditScreenModel.TransactionLogID = config.data.TransactionID;
                        auditService.auditScreenModel.ActionTypeID = SCREEN_ACTIONTYPES.Save_Edit;
                        //check if datakey is for client merge, then format DataKey to represent Parent and Child MRNs as well eg: ParentMRN:12,ChildMRN:11|BusinessAdministration-ClientMerge-ClientMerge
                        if (config.data.ParentMRN && config.data.ChildMRN && auditService.auditScreenModel.DataKey === BusinessAdministrationPermissionKey.BusinessAdministration_ClientMerge_ClientMerge) {
                            auditService.auditScreenModel.DataKey = ('ParentMRN:' + (config.data.ParentMRN || '') + ',ChildMRN:' + (config.data.ChildMRN || '') + '|' + auditService.auditScreenModel.DataKey);
                        }

                        if (auditService.auditScreenModel.DataKey && !auditService.auditScreenModel.isCurrentRequestPending) {
                            auditService.addScreenAudit(auditService.auditScreenModel).then(function () {
                                auditService.resetScreenAuditModel();
                            });
                            auditService.auditScreenModel.isCurrentRequestPending = true;

                        }
                    }
                }

            }
        }
      ];
  })