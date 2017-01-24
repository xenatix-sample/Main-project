angular.module('xenatixApp')
    .controller('clientMergeController', ['$scope', '$filter','$rootScope', 'alertService',  '$q', '$state', 'formService',  'clientMergeService',
        function ($scope, $filter,$rootScope, alertService, $q, $state, formService, clientMergeService) {

            var resetForm = function () {
                $rootScope.formReset($scope.ctrl.clientMergeForm, $scope.ctrl.clientMergeForm.$name);
            };

            $scope.getLookupsByType = function (typeName) {
                return lookupService.getLookupsByType(typeName);
            };

            $scope.init = function () {
                
                $scope.$parent['autoFocus'] = true;
                $scope.getClientMergeCounts();
       
            };


            $scope.getClientMergeCounts = function () {
                clientMergeService.getClientMergeCounts().then(function (response) {
                    if (response != null && response.ResultCode === 0 && response.DataItems != null) {
                        $scope.clientMergeCounts = response.DataItems[0];
                    }
                },
                function (errorStatus) {
                    alertService.error('Unable to get the counts for Merge');
                });
            };

                
            $scope.init();

        }]);