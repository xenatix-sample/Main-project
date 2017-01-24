
// DEVELOPER: Chris Reed

// Allows for filtering by dynamic field or property
angular.module("xenatixApp")
    .filter('filterBy', function () {
        return function (item, field, value) {
            return item[field] == value;
        };
    });