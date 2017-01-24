(function () {
    angular.module('xenatixApp')
        .controller('baseContactController', ['$scope', '$controller', 'lookupService', '$filter', function ($scope, $controller, lookupService, $filter) {

            $controller('baseController', { $scope: $scope });

            $scope.ADDRESS_ACCESS = {
                NotRequired: 0,
                Required: 1,
                ConditionalRequired: 2,
                Type: 4,
                Line1: 8,
                Line2: 16,
                City: 32,
                State: 64,
                County: 128,
                PostalCode: 256,
                ComplexName: 512,
                GateCode: 1024,
                EffectiveDate: 2048,
                ExpirationDate: 4096,
                Permission: 8192,

            }
            $scope.AddressAccessCode = 0;

            $scope.PHONE_ACCESS = {
                NotRequired: 0,
                Required: 1,
                ConditionalRequired: 2,
                Type: 4,
                Number: 8,
                Permission: 16
            }

            $scope.PhoneAccessCode = 0;

            $scope.EMAIL_ACCESS = {
                NotRequired: 0,
                Required: 1,
                ConditionalRequired: 2,
                Email: 4,
                Permission: 8
            }

            $scope.EmailAccessCode = 0;

            $scope.defaultStateProvinceID = $filter('filter')(lookupService.getLookupsByType('StateProvince'), { Name: 'Texas' }, true)[0].ID;
            $scope.defaultCountyID = $filter('filter')(lookupService.getLookupsByType('County'), { Name: 'Tarrant' }, true)[0].ID;           

        }]);
})();

