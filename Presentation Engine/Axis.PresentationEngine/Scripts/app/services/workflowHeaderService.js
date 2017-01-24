angular.module('xenatixApp')
    .factory('WorkflowHeaderService', ["$q", 'settings', 'offlineData', '$injector', function ($q, settings, offlineData, $injector) {
        var CONFIG = {
            apiControllerRoot: "/data/WorkflowHeader/",
            offlineApiUrl: '/WorkflowHeader/'
        }

        var AddWorkflowHeader = function (workflowHeaderModel) {
            var dfd = $q.defer();
            var $http = $injector.get('$http');            
            $http.post(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'AddWorkflowHeader', workflowHeaderModel)
                .then(function (data, status, header, config) {                    
                    dfd.resolve(data);                   
                }, function (data, status, header, config) {
                    dfd.reject(status);
                }, function (notification) {
                    dfd.notify(notification);
                });           
            return dfd.promise;
        };
      
        var GetWorkflowHeader = function (workflowKey, headerID) {
            var dfd = $q.defer();
            var $http = $injector.get('$http');
            $http.get(settings.webApiBaseUrl + CONFIG.apiControllerRoot + 'GetWorkflowHeader', {params: { workflowKey: workflowKey, headerID: headerID } })
            .success(function (data, status, header, config) {
                dfd.resolve(data);
            })
            .error(function (data, status, header, config) {
                dfd.reject(status);
            });
            return dfd.promise;
        };

        return {
            AddWorkflowHeader: AddWorkflowHeader,
            GetWorkflowHeader: GetWorkflowHeader
        };
    }]);