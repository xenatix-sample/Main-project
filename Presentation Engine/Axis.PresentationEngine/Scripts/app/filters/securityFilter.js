angular.module('xenatixApp')
.filter('securityFilter', ['$filter', 'navigationService', 'roleSecurityService', function ($filter, navigationService, roleSecurityService) {
    return function (collection, datakey, compareKey, permissionKey) {
        var userData = navigationService.userDetails();
        if (hasData(userData)) {
            //if getting permission key, check for company permission. If has, do not apply filter
            if (permissionKey) {
                var hasCompanyPermission = roleSecurityService.hasPermission(permissionKey, PERMISSION.READ, PERMISSION_LEVEL.Company);
                if (hasCompanyPermission)
                    return collection;
            }
            compareKey = compareKey || 'ID';
            var objData = userData.DataItems[0].UserOrganizationStructures;
            var premissionCollection = $filter('filter')(objData, { DataKey: datakey }, true);
            var retCollection = [];
            if (collection) {
                angular.forEach(premissionCollection, function (item) {
                    for (var ctr = 0; ctr < collection.length; ctr++) {
                        if (item.MappingID == collection[ctr][compareKey]) {
                            retCollection.push(collection[ctr]);
                            break;
                        }
                    }
                })
            }
            return retCollection;
        }
    };
}]);
