angular.module("xenatixApp")
    .filter("toSSN", [function () {
        return function (value) {
            if (value) {
                if (value.length == 9) {
                    return value.substr(0, 3) + "-" + value.substr(3, 2) + "-" + value.substr(5);
                }
                else {
                    var ssn = maskX(value, 9);
                    return ssn.substr(0, 3) + "-" + ssn.substr(3, 2) + "-" + ssn.substr(5);
                }
            }
            return value;
        };
    }])
    .filter("toMaskSSN", [function () {
        return function (value) {
            if (value) {
                if (value.length == 9) {
                    return "***-**-"+ value.substr(5);
                }
                else {
                    var ssn = maskX(value, 9);
                    return "***-**-" + ssn.substr(5);
                }
            }
            return value;
        };
    }]);

function maskX(number, length) {

    var str = '' + number;
    while (str.length < length) {
        str = 'X' + str;
    }
    return str;
}
function pad(number, length) {

    var str = '' + number;
    while (str.length < length) {
        str = '0' + str;
    }
    return str;
}