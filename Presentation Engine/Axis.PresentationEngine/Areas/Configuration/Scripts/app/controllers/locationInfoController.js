angular.module('xenatixApp')
    .controller('locationInfoController', ['$scope', '$modal', '$rootScope', '$filter', function ($scope, $modal, $rootScope, $filter) {
        $scope.LocationName = "Fake Location 2";
        $scope.LocationInfo = true;
        var stateDetail = { stateName: "siteadministration.configuration.locations.details.general", validationState: 'valid' };
        $rootScope.locationRightNavigationHandler(stateDetail);
    }]);