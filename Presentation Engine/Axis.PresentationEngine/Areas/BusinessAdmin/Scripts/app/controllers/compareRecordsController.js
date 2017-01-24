angular.module('xenatixApp')
    .controller('compareRecordsController', ['$scope', '$q', '$filter', 'alertService', '$stateParams', 'registrationService', 'contactAddressService', 'contactPhoneService', 'contactEmailService', 'collateralService', 'lookupService', 'clientMergeService', 'contactBenefitService', 'contactRelationshipService',
function ($scope, $q, $filter, alertService, $stateParams, registrationService, contactAddressService, contactPhoneService, contactEmailService, collateralService, lookupService, clientMergeService, contactBenefitService, contactRelationshipService) {

    // Private global variables
    $scope.isMaster = false;

    // Private functions
    var init = function () {
        var promiseAll = [];
        promiseAll.push(getData($stateParams.contactID));
        promiseAll.push(getData($stateParams.compareContactID));
        $q.all(promiseAll).then(function (data) {
            $scope.contactDetails = data;
        });

    };

    var getData = function (contactID) {
        var dfd = $q.defer();
        var promises = [];
        promises.push(registrationService.get(contactID));
        promises.push(contactAddressService.get(contactID));
        promises.push(contactPhoneService.get(contactID, 1));
        promises.push(contactEmailService.get(contactID));
        promises.push(collateralService.get(contactID, CONTACT_TYPE.Collateral, false).then(function (data) {
            return getContactRelationShip(data,contactID);
        }));
        promises.push(contactBenefitService.get(contactID).then(getMedicaidID));
        $q.all(promises).then(function (data) {
            dfd.resolve(data);
        });
        return dfd.promise;
    };

    var getMrn = function (index) {
        return $scope.contactDetails[index][0].DataItems[0].MRN;
    };

    var getMedicaidID = function (data) {
        if (hasData(data)) {
            var payors = $filter('filter')(data.DataItems, function (itm) {
                return itm.PayorName.toString().toLowerCase().indexOf('medicaid') > -1;
            });
            if (payors && payors.length > 0) {
                return payors[0].PolicyID;
            }
        }
    };

    var getContactRelationShip = function (data, contactID) {
        var dfd = $q.defer();
        var innerPromises = [];
        angular.forEach(data.DataItems, function (item) {
            innerPromises.push(contactRelationshipService.get(item.ContactID, contactID));
        });
        $q.all(innerPromises).then(function (collateralRelationShipData) {
            angular.forEach(data.DataItems, function (item, index) {
                if (hasData(collateralRelationShipData[index]))
                item.RelationshipGroupID = collateralRelationShipData[index].DataItems[0].RelationshipGroupID;
            });
            return dfd.resolve(data);
        });
        return dfd.promise;
    };

    // Angular methods
    $scope.getText = function (type, id) {
        return lookupService.getText(type, id);
    };

    $scope.getCollateralType = function (type, id) {
        var collateralTypes = $filter('filter')(lookupService.getLookupsByType(type), {
            RelationshipGroupID: id
        });
        return collateralTypes.length > 0 ? collateralTypes[0].Name : "";
    };

    $scope.selectMasterCallback = function (value) {
        $scope.isMaster = value;
    };

    $scope.notMatchRecord = function () {
        $scope.Goto("businessadministration.clientmerge.clientmergeNavigation.potentialmatches");
    };

    $scope.mergeClient = function () {
        bootbox.confirm("<b>Are you sure you want to merge?</b> <br/> This action can only be undone if the merged record has not been edited", function (result) {
            if (result == true) {
                var parentMRN = getMrn($scope.isMaster ? 1 : 0);
                var childMRN = getMrn($scope.isMaster ? 0 : 1);
                clientMergeService.mergeRecords({ ParentMRN: parentMRN, ChildMRN: childMRN }).then(function (data) {
                    alertService.success("Client merge completed successfully.");
                }, function () {
                    alertService.error('OOPS! Something went wrong');
                });
            }
        });

    };

    // Main function
    init();

}]);