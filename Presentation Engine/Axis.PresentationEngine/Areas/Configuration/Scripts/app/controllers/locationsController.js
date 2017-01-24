angular.module('xenatixApp')
    .controller('locationsController', ['$scope', '$modal', '$rootScope', '$filter', function ($scope, $modal, $rootScope, $filter) {
        $scope.LocationName = "Fake Location 1";
        $scope.Locations = true;
    }]);