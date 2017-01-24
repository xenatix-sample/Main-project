(function () {
    angular.module('xenatixApp')
        .controller('healthRecordsController', ['$scope', '$filter', '$rootScope', 'alertService', '$q', '$state', 'formService', 'healthRecordsService',
            function ($scope, $filter, $rootScope, alertService, $q, $state, formService, healthRecordsService) {
                $scope.init = function () {
                    $scope.$parent['autoFocus'] = true;
                }

                $scope.add = function () {
                    $scope.Goto("siteadministration.rolemanagement.role.roledetails");
                };

                $scope.getHealthRecords = function () {
                    healthRecordsService.getHealthRecords().then(function (data) {
                        if (data != null && data.DataItems != null) {
                            $scope.healthRecordItems = data.DataItems;
                        }
                    },
                    function (errorStatus) {
                        alertService.error('Unable to connect to server');
                    });
                };

                $scope.getHealthRecords();
            }]);
}());