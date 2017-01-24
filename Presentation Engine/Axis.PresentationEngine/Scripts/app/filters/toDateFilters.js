function isLeapYear(year) {
    var d = new Date(year, 1, 28);
    d.setDate(d.getDate() + 1);
    return d.getMonth() == 1;
}

function getAge(date, dateToCompare) {
    var d = new Date(date),
        now = dateToCompare ? new Date(dateToCompare) : new Date();
    var years = now.getFullYear() - d.getFullYear();
    d.setFullYear(d.getFullYear() + years);
    if (d > now) {
        years--;
        d.setFullYear(d.getFullYear() - 1);
    }
    var days = (now.getTime() - d.getTime()) / (3600 * 24 * 1000);
    return years + days / (isLeapYear(now.getFullYear()) ? 366 : 365);
}

function getIsDateMax(date, maxLimit, dateToCompare) {
    var d = new moment(date).toDate();
    var now = new Date();
    var years = now.getFullYear() - d.getFullYear();
    d.setFullYear(d.getFullYear() + years);
    if (d > now) {
        years--;
        d.setFullYear(d.getFullYear() - 1);
    }
    var days = Math.floor((now.getTime() - d.getTime()) / (1000 * 3600 * 24));
    return (years + days / (isLeapYear(now.getFullYear()) ? 366 : 365) > maxLimit);
}

function isDateGreaterThanMonths(inputDate, totalMonths, dateToCompare) {
    var iDate = moment(inputDate);
    var now = dateToCompare ? moment(dateToCompare) : moment(new Date());
    var years = now.diff(iDate, 'year');
    iDate.add(years, 'years');

    var months = now.diff(iDate, 'months');
    iDate.add(months, 'months');

    var days = now.diff(iDate, 'days');
    var daysInMonth = moment(iDate).daysInMonth();
    return ((years * 12 + months + days / daysInMonth) > totalMonths);
}
angular.module('xenatixApp')
// To be used while saving Dates
.filter('toMMDDYYYYDate', function () {
    //This should probably be renamed, it affects the time because of uselocal and so it should not be used with date only fields.
    return function (value, format, useLocal) {
        if (!value) {
            return '';
        }

        if (!format) {
            format = 'MM/DD/YYYY';
        }

        //var utcDate = moment.utc(value);
        //var utcDate = moment(value);

        //We need to consider removing this useLocal and update the logic here for formatting the dates while display and save
        //if (useLocal === 'useLocal') {
        //    utcDate = moment.utc(utcDate).toDate();
        //}

        //return moment(utcDate).format(format);
        return moment(value).format(format);
    }
})
.filter('formatDate', function () {
    //This should be used with the Date Only Fields we do not need to add time stamp to date only fields
    return function (value, format) {
        if (!value) {
            return '';
        }

        if (!format) {
            format = 'MM/DD/YYYY';
        }

        return moment(value).format(format);
    }
})
.filter('toAdjustedAge', function () {
    return function (value) {
        var dob = moment(value);
        var today = moment();
        var ageInMonths = today.diff(dob, 'months');

        if (ageInMonths !== null && ageInMonths !== undefined) {
            if (ageInMonths > 18) {
                return 'N/A';
            } else {
                return ageInMonths + ' Months';
            }
        } else {
            return 'N/A';
        }
    }
})
.filter('toAdjustedAgeCaluclation', function () {
    return function (value, gestationalAge) {
        var adjustedAge = 0;
        var dob = moment(value);
        var today = moment();
        var ageInMonths = today.diff(dob, 'months');

        if (ageInMonths < 18) {
            if (gestationalAge !== null && gestationalAge !== undefined && gestationalAge > 0) {
                if (gestationalAge >= 37) {
                    return 'N/A';
                }
                else {
                    var prematurityAdjusment = (40 - gestationalAge) / 4;
                    var prematurityAdjustedAge = prematurityAdjusment > 4
                                                ? Math.floor(ageInMonths - 4)
                                                : Math.floor(ageInMonths - prematurityAdjusment);

                    if (!((ageInMonths >= 18) || (gestationalAge >= 37) || (prematurityAdjustedAge < 0))) {
                        if (prematurityAdjusment > 4) {
                            adjustedAge = Math.floor(ageInMonths - 4);
                        }
                        else {
                            adjustedAge = Math.floor(ageInMonths - prematurityAdjusment);
                        }
                    }
                }
            }
            else {
                return 'N/A';
            }
        }
        else {
            return 'N/A';
        }

        return adjustedAge < 0 ? 'O Months' : adjustedAge + ' Months';
    }
})
.filter('calculate120years', function () {
    return function () {
        var value = new Date();
        var year = value.getYear() - 120;
        if (year < 0) {
            year = 1900 + year;
        }
        value.setYear(year);
        return value;
    }
})
.filter('toAge', function () {
    return function (value) {
        if (value == undefined) {
            return "";
        }
        return Math.floor(getAge(value));
    }
})

.filter('isDateMaxLimit', function () {
    return function (value, maxLimit, dateToCompare) {
        return value ? getIsDateMax(value, maxLimit, dateToCompare) : true;
    }
})

.filter('isDateMaxLimitInMonths', function () {
    return function (value, maxLimit, dateToCompare) {
        return value ? isDateGreaterThanMonths(value, maxLimit, dateToCompare) : true;
    }
})

.filter('ageToShow', function () {
    var maxAgeInMonth = 3; // max age to show in month
    return function (value, dateToCompare) {
        if (value == undefined || new Date().getTime() < new Date(value).getTime()) {
            return "";
        }
        var todate = dateToCompare ? new Date(dateToCompare) : new Date();
        var age = [],
            fromdate = new Date(value),
            y = [todate.getFullYear(), fromdate.getFullYear()],
            ageYears = Math.floor(getAge(value, dateToCompare));

        var m = [todate.getMonth(), fromdate.getMonth()];
        if (m[0] < m[1]) {
            m[0] += 12;
        }
        var mdiff = m[0] - m[1],
        d = [todate.getDate(), fromdate.getDate()],
        ddiff = d[0] - d[1];
        if (ageYears >= maxAgeInMonth && !(ageYears == maxAgeInMonth && ddiff == 0 && mdiff == 0)) {
            return ageYears + 'y';
        }
        if (mdiff == 0 && y[0] != y[1] && ddiff < 0) {
            mdiff = 12;
        }
        mdiff = mdiff + (12 * ageYears);
        if (ddiff < 0) {
            fromdate.setMonth(m[1] + 1, 0);
            ddiff = fromdate.getDate() - d[1] + d[0];
            --mdiff;
        }
        if (mdiff > 0) age.push(mdiff + 'm ');
        if (ddiff > 0) { age.push(ddiff + 'd'); }
        else if (mdiff == 0 && ageYears == 0) { age.push(ddiff + 'd'); }
        if (age.length > 1) age.splice(age.length - 1, 0, '');
        return age.join('');
    }
})
.filter('getDaysDifference', function () {
    return function (fromDate, toDate) {
        if (!fromDate || fromDate === '' || !toDate || toDate === '') {
            return '';
        }

        var from = moment(fromDate);
        var to = moment(toDate);
        
        return to.diff(from,'days')+1;
    }
})

function pad(number, length) {

    var str = '' + number;
    while (str.length < length) {
        str = '0' + str;
    }
    return str;
}
