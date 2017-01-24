angular.module('xenatixApp')
// To be used while saving Time
.filter('toMilitaryTime', function () {
    return function (value, ampm) {
        if (value == undefined || ampm == undefined) {
            return "";
        }

        return moment(pad(value, 4) + ampm, 'hh:mmA').format('HHmm');
    }
}).filter('toStandardTime', function () {
    return function (value) {
        if (value == undefined || value == 0) {
            return "";
        }
        return moment(pad(value, 4), 'HHmm').format('hh:mm');
    }
}).filter('toStandardTimeAMPM', function () {
    return function (value) {
        if (value == undefined || value == 0) {
            return "";
        }
        return moment(pad(value, 4), 'HHmm').format('A');
    };
}).filter('toStandardTimeNoPad', function () {
    return function (value) {
        if (value == undefined || value == 0) {
            return "";
        }
        return moment(value).format('hh:mm');
    };
}).filter('toFullStandardTimeAMPM', function () {
    return function (value) {
        if (value == undefined || value == 0) {
            return "";
        }
        return moment(pad(value, 4), 'HHmm').format('hh:mm A');
    };
}).filter('hourLength', function () {
    return function (value) {
        if (value % 1 != 0) {
            value = parseFloat(value).toFixed(2);
        }
        return value;
    };
}).filter('minLength', function () {
    return function (value) {
        return Math.round(value);
    };
}).filter('tohour', function () {
    return function (value) {
        var hour = 0;
        if (value) {
            value = parseInt(value);
            hour = value / 60;
        }

        return hour.toFixed(2);
    };
}).filter('tomin', function () {
    return function (value) {
        var min = 0;
        value = parseFloat(value);
        min = Math.round(value * 60);
        return min;
    };
});