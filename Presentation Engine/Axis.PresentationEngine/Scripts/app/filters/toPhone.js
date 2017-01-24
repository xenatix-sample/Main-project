angular.module("xenatixApp").filter("toPhone", [function () {
    return function (value) {
        if (value) {
            if (value.length == 10) {
                return value.substr(0, 3) + "-" + value.substr(3, 3) + "-" + value.substr(6);
            }
        }
        return value;
    };
}]);