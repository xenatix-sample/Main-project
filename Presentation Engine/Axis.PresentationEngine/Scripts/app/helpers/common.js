var DateMeasure = function (ms) {
    var d, h, m, s;
    s = Math.floor(ms / 1000);
    m = Math.floor(s / 60);
    s = s % 60;
    h = Math.floor(m / 60);
    m = m % 60;
    d = Math.floor(h / 24);
    h = h % 24;

    this.days = d;
    this.hours = h;
    this.minutes = m;
    this.seconds = s;
};

var getTimeDifference = function (elapsed) {
    if (elapsed >= 0) {
        var data = new DateMeasure(elapsed);
        if (data.days == 0 && data.hours == 0 && data.minutes == 0) {
            return '0 mins';
        } else if (data.days != 0) {
            return (data.days + (data.days == 1 ? ' day' : ' days'));
        } else if (data.hours != 0) {
            return (data.hours + (data.hours == 1 ? ' hour' : ' hours'));
        } else {
            return (data.minutes + (data.minutes == 1 ? ' min' : ' mins'));
        }
    }
    else {
        return '';
    }
};

function toDateTime(dateTimeVal) {
    var _date, _time, _meridiem;

    if (dateTimeVal.toString().indexOf("00:00:01") > -1) //no time selected 
    {
        _date = moment(dateTimeVal).format('MM/DD/YYYY');
        _time = _meridiem = null;
    }
    else if (dateTimeVal.toString().indexOf("00:00:00") > -1) //12:00 AM entered
    {
        _date = moment(dateTimeVal).format('MM/DD/YYYY');
        _time = "12:00";
        _meridiem = "AM";
    }
    else if (dateTimeVal.toString().indexOf("12:00:00") > -1) //12:00 PM entered
    {
        _date = moment(dateTimeVal).format('MM/DD/YYYY');
        _time = "12:00";
        _meridiem = "PM";
    }
    else {
        _date = moment(dateTimeVal).format('MM/DD/YYYY');
        _time = moment(dateTimeVal).format('hh:mm');
        _meridiem = moment(pad(getCurrentTime(moment(dateTimeVal).toDate()), 4), 'HHmm').format('A');
    }

    return { date: _date, time: _time, meridiem: _meridiem };
}

function getCurrentTime(date) {
    var currentHour = date.getHours();
    var currentMinute = date.getMinutes();
    var period = currentHour >= 12 ? 'pm' : 'am';
    return moment(pad(pad(currentHour, 2) + ":" + pad(currentMinute, 2), 4) + period, 'hh:mmA').format('HHmm');
};

function toStringDateTime(dateTimeVal) {
    var _dateObj = toDateTime(dateTimeVal);
    return _dateObj.date + (_dateObj.time == null ? '' : ' ' + _dateObj.time) + (_dateObj.meridiem == null ? '' : ' ' + _dateObj.meridiem);
}

function calculateDuration(startTime, endTime) {
    try {
        if (startTime && endTime) {
            var timeInMillis = moment(endTime).toDate() - moment(startTime).toDate();
            if (timeInMillis < 0) //In case End Time is not selected
                return;

            var data = new DateMeasure(timeInMillis);
            return ((data.days > 0) ? data.days + 'day ' : '') + data.hours + 'hr ' + ' ' + data.minutes + 'mins';
        }
    }
    catch (err) {
        return '';
    }
}

var formatDurationToSort = function (duration) {
    if (duration) {
        var result = "000000";
        var days = "00";
        var hrs = "00";
        var mins = "00";
        var time = duration.split(" ");
        time.forEach(function (value) {
            if (value.indexOf("day") > 0)
                days = value.replace("day", "");
            else if (value.indexOf("hr") > 0)
                hrs = value.replace("hr", "");
            else if (value.indexOf("mins") > 0)
                mins = value.replace("mins", "");
        });
        result = days.replace(/^(\d)$/, '0$1') + hrs.replace(/^(\d)$/, '0$1') + mins.replace(/^(\d)$/, '0$1');
        return result;
    }
};

var getFormattedDate = function (dateVal, timeVal) {
    var hr = timeVal.substring(0, timeVal.indexOf(':'));
    if (timeVal.substring(timeVal.indexOf(' ') + 1, timeVal.length) == "PM" && hr != 12) { //checks if PM, adds 12 hours
        hr = +hr + +12;
    }
    var min = timeVal.substring(timeVal.indexOf(':') + 1, timeVal.indexOf(' '));
    return dateVal.setHours(hr, min);
};

//This file needs to be updated with the changes made by me for the bug 13763
var formatTimeToDate = function (dateVal, timeVal) { //format should be hh:mm tt
    var d = new Date(dateVal);
    if (timeVal.indexOf(':') == -1) {
        timeVal = timeVal.substring(0, 2) + ':' + timeVal.substring(2, timeVal.length);
    }
    var hr = timeVal.substring(0, timeVal.indexOf(':'));
    if (timeVal.substring(timeVal.indexOf(' ') + 1, timeVal.length) == "PM" && hr != 12)      //checks if PM, adds 12 hours
        hr = +hr + +12;
    var min = timeVal.substring(timeVal.indexOf(':') + 1, timeVal.length);
    return new Date(d.setHours(hr, min));
};

function prepareXenCommentHistory(previousData, newData, userName) {
    if (newData) {
        var newCommentData = { UserName: userName || 'None', CommentDate: new Date(), Comment: newData };
        if (previousData && previousData.length > 0) {
            previousData.unshift(newCommentData);
        }
        else {
            previousData = [];
            previousData.push(newCommentData);
        }
    }
    return JSON.stringify(previousData);
}

var isFutureDate = function (dt) {
    return (moment(moment(dt).format('MM/DD/YYYY')) > new Date());
}

var isExpireDate = function (dt) {
    return (moment(moment(dt).format('MM/DD/YYYY')) < new Date());
};

var isHouseholdExpired = function (dt) {
    if (!dt) return false;
    return isExpireDate(dt);
}

var isValidDateRange = function (inputDate, fromDate, toDate) {
    var inputDate = moment(moment(inputDate).format('MM/DD/YYYY'));
    var fromDate = moment(moment(fromDate).format('MM/DD/YYYY'));
    if (toDate) {
        var toDate = moment(moment(toDate).format('MM/DD/YYYY'));
        return (inputDate >= fromDate && inputDate <= toDate)
    }
    else {
        return fromDate <= inputDate;
    }
}
// Saving the extra work of determining that expDateModelName & effDateModelName are passed so it's mandatory while using this function to pass all three parameters
var filterFutureOrExpiredRecords = function (arr, expDateModelName, effDateModelName) {
    var filteredArr = $.grep(arr, function (item) {
        return (item[expDateModelName] ? !isExpireDate(item[expDateModelName]) : true) && (item[effDateModelName] ? !isFutureDate(item[effDateModelName]) : true)
    });
    return filteredArr;
}

var removeNullFromArray = function (obj) {
    var returnObj = [];
    if (obj) {
        angular.forEach(obj, function (itm) {
            if (itm)
                returnObj.push(itm);
        });
    }
    return returnObj;
}

// ****************** Helper Functions ************************//

function getQueryStringParams(sParam) {
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split('&');
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split('=');
        if (sParameterName[0].toLowerCase() == sParam.toLowerCase()) {
            return sParameterName[1];
        }
    }
}

//Concatenate string with provided separator.
concatWithSeparator = function (str, toAdd, separator) {
    if (str.trim().length > 0 && toAdd.trim().length > 0) {
        if (separator != undefined) {
            str = str.trim() + separator + toAdd.trim();
        }
        else {
            str = str.trim() + toAdd.trim();
        }

    }
    else if (toAdd.trim().length > 0) {
        str = toAdd.trim();
    }
    return str;
}

function capLock(e) {
    kc = e.keyCode ? e.keyCode : e.which;
    sk = e.shiftKey ? e.shiftKey : ((kc == 16) ? true : false);
    if (((kc >= 65 && kc <= 90) && !sk) || ((kc >= 97 && kc <= 122) && sk))
        document.getElementById('divCapsCheck').style.visibility = 'visible';
    else
        document.getElementById('divCapsCheck').style.visibility = 'hidden';
}

function toTitleCase(str) {
    return str.replace(/\w+/g, function (txt) { return txt.charAt(0).toUpperCase() + txt.substr(1).toLowerCase(); });
}

function ClearGridSelection(attrs) {
    if (attrs['tableId'] != undefined) {
        $('#' + attrs['tableId'] + " tr.success").removeClass('success');
    }
}

function getRaceCSVNames(raceLookupList, raceList) {
    var str = '';
    raceList.forEach(function (item, index) {
        if (item.RaceID) {
            var raceName = raceLookupList.find(function (raceItem) {
                return raceItem.ID === item.RaceID
            }).Name;
            str += raceName + ', ';
        }
    });
    return str.replace(/,\s*$/, "");
}

function ExplicitlyModifiedTheForm(scope, formname) {
    var formSubNames = [];
    if ((formname != undefined && formname != null && formname != '') && (scope.isChecked != scope.isPrevChecked)) {
        if (formname.indexOf('.') >= 0) {
            formSubNames = formname.split('.');
            if (formSubNames != null && formSubNames.length == 2) {
                scope[formSubNames[0]][formSubNames[1]].modified = true;
            }
        }
        else {
            scope[formname].modified = true;
        }
    }
}

var searchString = function (value, pattern) {
    if (value && pattern)
        return value.toString().indexOf(pattern) >= 0
}

function pad(number, length) {
    var paddedNumber = '' + number;
    while (paddedNumber.length < length) {
        paddedNumber = '0' + paddedNumber;
    }

    return paddedNumber;
}

function buildURL(contactID, url, params, isParentState) {
    var finalURL = isParentState ? "" : ".";
    finalURL = finalURL + url + "({ ContactID: " + contactID + "*extraParams* })";
    var extraParams = "";
    for (var key in params) {
        if (params.hasOwnProperty(key)) {
            extraParams = extraParams + ", " + key + ": " + params[key];
        }
    }

    return finalURL.replace("*extraParams*", extraParams);
}

// Initialize Tiles
initTiles = function (
    tileName, url, contactID, showShortcuts, tileAddDetails,
    permissionKey, permission, params, isParentState, tileDefaultUrl) {
    if (tileAddDetails == undefined)
        tileAddDetails = [];

    return {
        TileName: tileName,
        hideShortCutKeys: !(url && url != ""),
        tileDefaultUrl: tileDefaultUrl ? buildURL(contactID, tileDefaultUrl, params, false) : null,
        Url: buildURL(contactID, url, params, isParentState),
        TileDetails: [],
        EditDetails: [],
        ShowShortcuts: showShortcuts,
        TileAddDetails: tileAddDetails,
        PermissionKey: permissionKey,
        Permission: permission,
        IsLoaded: false
    };
}

// Check model 
checkModel = function (value) {
    if (value != undefined && value != null && value !== "") {
        return true;
    }
    else
        return false;
}

// Create Tile Detail Model Based on lookup type
getTileDetailsModel = function (lookUpType, value, label) {
    var tileDetailsModel = {};
    if (lookUpType != undefined && lookUpType != null) {
        tileDetailsModel.LookUpType = lookUpType;
    }
    tileDetailsModel.Value = value;
    tileDetailsModel.Label = label;
    return tileDetailsModel;
}


// Set Grid item
setGridItem = function (table, compareFieldName, value) {
    var tableData = table.bootstrapTable('getData');
    angular.forEach(tableData, function (obj, index) {
        if (eval('obj.' + compareFieldName) == value) {
            setTimeout(function () {
                var pageSize = table.bootstrapTable('getOptions').pageSize;
                var pageNumber = Math.ceil((index + 1) / (pageSize));
                var pageIndex = (index + 1) % (pageSize);
                if (pageNumber > 1)
                    table.bootstrapTable('selectPage', pageNumber)
                table.find("tr:eq(" + (pageIndex == 0 ? pageSize : pageIndex) + ")").find("[data-default-action]").click();
            }, 1000);

            return false;
        }
    });
}

function dataURItoBlob(dataURI) {
    var array = dataURI.split(','),
                    data = array[1],
                    mime = array[0].match(/:(.*?);/)[1];
    return { data: data, type: mime };
}

function addNoneToCredentials(obj) {
    var noneExists = obj.find(function (item) { return item.CredentialName.toLowerCase() === "none"; })
    if (!noneExists || noneExists.length === 0) {
        obj.push({ CredentialID: 1, CredentialName: 'None' });
    }
    return obj;
}

function getBrowserName() {
    var userAgent = window.navigator.userAgent;

    var browsers = { chrome: /chrome/i, safari: /safari/i, firefox: /firefox/i, ie: /msie|Trident.*rv[ :]*11\./gi };

    for (var key in browsers) {
        if (browsers[key].test(userAgent)) {
            return key;
        }
    };

    return 'unknown';
}
// This function will change menu dropdown to show upper right hand side on the grid
function applyDropupOnGrid(applyZindex) {
    var dropdownToggleElement = angular.element('.keep-open');
    if (!dropdownToggleElement.hasClass('dropup')) {
        dropdownToggleElement.addClass('dropup');
        if (applyZindex)
            angular.element('.keep-open ul').css('z-index', '9999');
    }
}


// This function will change menu dropdown to show lower right hand side on the grid
function applyDropdownOnGrid() {
    var dropdownToggleElement = angular.element('.keep-open');
    if (dropdownToggleElement.hasClass('dropup')) {
        dropdownToggleElement.removeClass('dropup');
    }
}

var getValidationName = function (elementName, attributeName) {
    var elemObj = $("[name='" + elementName + "']");

    if (checkModel(elemObj)) {
        var validationNameAttr = elemObj.attr(attributeName || 'data-validation-name');

        if (checkModel(validationNameAttr)) {
            return validationNameAttr;
        }
    }
    return elementName;
}

var getCompareValidationName = function (elementName) {
    var elemObj = $("[name='" + elementName + "']");

    if (checkModel(elemObj)) {
        var compareValidationNameAttr = elemObj.attr('data-compare-element-name');

        if (checkModel(compareValidationNameAttr)) {
            return getValidationName(compareValidationNameAttr);
        }
    }
    return '';
}

var formatDateMMDDYYYY = function (dateField) {
    return dateField.getMonth() + '/' + dateField.getDate() + '/' + dateField.getFullYear();
};

var iterateErrorFields = function (fieldCollection, fieldToFocus, errorType, alertService) {
    angular.forEach(fieldCollection, function (field) {
        if (field && ('$error' in field) && (errorType in field.$error) && (Object.prototype.toString.call(field.$error[errorType]) === '[object Array]')) {
            iterateErrorFields(field.$error[errorType], fieldToFocus, errorType, alertService);
        } else {
            if (field.$name !== undefined && field.$name !== null) {
                if (field.xenErrorMessage) {
                    errorMessage = field.xenErrorMessage;
                }
                else {
                    var errorMessage = '';
                    switch (errorType) {
                        case 'required':
                            if (!handleError(field, errorType)) {
                                errorMessage = 'Please fill out the ' + getValidationName(field.$name) + ' field.';
                            }
                            break;
                        case 'date':
                        case 'invalidDate':
                            if (!handleError(field, errorType)) {
                                var fieldName = getValidationName(field.$name);
                                errorMessage = fieldName + ' is not a valid ' + ((fieldName.toLowerCase().indexOf('time') > -1) ? 'time.' : 'date.');
                            }
                            break;
                        case "lessThanMinValidDate":
                            if (!handleError(field, errorType)) {
                                errorMessage = getValidationName(field.$name).split("|") + " should be greater than " + formatDateMMDDYYYY(new Date(70, 1, 1));// (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
                            }
                            break;
                        case 'futureDate':
                            if (!handleError(field, errorType)) {
                                errorMessage = getValidationName(field.$name) + ' can not be in the future.';
                            }
                            break;
                        case 'pastDate':
                            if (!handleError(field, errorType)) {
                                errorMessage = getValidationName(field.$name) + ' can not be in the past.';
                            }
                            break;
                        case 'greaterThanDate':
                            if (!handleError(field, errorType)) {
                                errorMessage = getValidationName(field.$name) + ' can not be greater than ' + getCompareValidationName(field.$name) + '.';
                            }
                            break;
                        case 'lessThanDate':
                            if (!handleError(field, errorType)) {
                                errorMessage = getValidationName(field.$name).split('|') + ' can not be less than ' + getCompareValidationName(field.$name) + '.';
                            }
                            break;
                        case 'invalidTime':
                            if (!handleError(field, errorType)) {
                                errorMessage = 'Please select valid ' + getValidationName(field.$name) + ' .';
                            }
                            break;
                        case 'maxLimit':
                            if (!handleError(field, errorType)) {
                                errorMessage = getValidationName(field.$name) + ' can not be greater than ' + getValidationName(field.$name, 'max-limit') + ' years.';
                            }
                            break;
                        case 'min':
                            errorMessage = getValidationName(field.$name) + ' can not be less than min value.';
                            break;
                        case 'max':
                            errorMessage = getValidationName(field.$name) + ' can not be greater than max value.';
                            break;
                        case 'maxlength':
                            errorMessage = getValidationName(field.$name) + ' has exceeded the maximum length.';
                            break;
                        case 'maximum':
                            errorMessage = 'Field ' + getValidationName(field.$name) + ' has exceeded the maximum length.';
                            break;
                        case 'minlength':
                            errorMessage = 'Field ' + getValidationName(field.$name) + ' has not met the minimum length.';
                            break;
                        case 'minimum':
                            errorMessage = 'Field ' + getValidationName(field.$name) + ' has not met the minimum length.';
                            break;
                        case 'mask':
                            errorMessage = 'Field ' + getValidationName(field.$name) + ' is not in the expected format.';
                            break;
                        case 'passwordPattern':
                        case 'pattern':
                            errorMessage = getValidationName(field.$name) + ' is not in the expected format.';
                            break;
                        case 'editable':
                            var isdisabled = (field.$name == '') ? '' : $('#' + field.$name + ' > div > input').attr('disabled');
                            if (isdisabled == 'disabled') {
                                field.$valid = true;
                                field.$pristine = true;
                            }
                            else if (!field.$viewValue.trim()) {
                                errorMessage = 'Please fill out the ' + getValidationName(field.$name) + ' field.';
                            }
                            else
                                errorMessage = getValidationName(field.$name) + ' is not valid.';
                            break;
                        case 'invalidSearch':
                            errorMessage = 'Please enter valid value in ' + getValidationName(field.$name) + ' field.';
                            break;
                        default:

                    }
                }
                if (errorMessage && (!field.$valid || field.$invalid)) {
                    alertService.error(errorMessage);
                    if (fieldToFocus === null) {
                        fieldToFocus = field;
                    }
                }
            }
            if (!field.$valid)
                field.$setDirty();
        }
    });
};

var handleError = function (field, errorType) {
    var isdisabled;
    if (errorType == "required") {
        isdisabled = (field.$name == '') ? '' : getDisabledAttrFromField(field.$name);
        if (isdisabled == 'disabled') {
            field.$valid = true;
            field.$invalid = false;
            field.$pristine = true;
        }
        else {
            field.$valid = false;
            field.$invalid = true;
        }
    }
    else {
        isdisabled = (field.$name == '') ? '' : $('#' + field.$name + ' > div > input').attr('disabled');
        if (isdisabled == 'disabled') {
            field.$valid = true;
            field.$invalid = false;
            field.$pristine = true;
        }
        else {
            field.$valid = false;
            field.$invalid = true;
        }
    }
    return isdisabled == "disabled";
}


var getDisabledAttrFromField = function (fieldname) {
    var ctrl = ctrl = $('#' + fieldname + ' > div > input');
    if (ctrl == null || ctrl.length == 0)
        ctrl = $('#' + fieldname);
    if (ctrl == null || ctrl.length == 0)
        return '';
    else
        return ctrl.attr('disabled');
}

var checkForAllDisabledCtrls = function (fieldCollection) {
    var ret = true;
    angular.forEach(fieldCollection, function (field) {
        if (field.$name !== undefined && field.$name !== null) {
            var dis = (field.$name == '') ? '' : getDisabledAttrFromField(field.$name);
            if (dis != 'disabled') {
                ret = false;
                return;
            }
        }
        else {
            ret = false;
            return;
        }
    });
    return ret;
};

var validateFutureDate = function (errorControlBlock, errorControl, formControl, dateControl, compareDateControl) {
    var errorBlock = angular.element(errorControlBlock);
    var error = angular.element(errorControl);
    if (formControl && dateControl) {
        var date = new Date(dateControl);
        compareDateControl.setHours(0, 0, 0, 0);
        date.setHours(0, 0, 0, 0);
        if (date > compareDateControl) {
            error.removeClass('ng-hide');
            errorBlock.addClass('has-error');
            if (formControl) {
                formControl.$setValidity('date', false);
            }
        }
        else {
            error.addClass('ng-hide');
            errorBlock.removeClass('has-error');
            if (formControl) {
                formControl.$setValidity('date', true);
            }
        }
    }
};

var validateTime = function (errorControlBlock, errorControl, formControl, dateControl, timeControl, compareTimeControl) {
    var errorBlock = angular.element(errorControlBlock);
    var error = angular.element(errorControl);
    if (dateControl && timeControl && compareTimeControl) {
        if (timeControl.valueOf() > compareTimeControl.valueOf()) {
            error.removeClass('ng-hide');
            errorBlock.addClass('has-error');
            if (formControl) {
                formControl.$setValidity('date', false);
            }
        }
        else {
            clearTimeError(error, errorBlock, formControl);
        }
    }
    else {
        clearTimeError(error, errorBlock, formControl);
    }
};


function validateSearchPattern(searchString) {
    var searchType = searchString[0];
    var searchNumber = searchString[1];
    var pattern;
    switch (searchType.toLowerCase()) {
        case "dob":
            return moment(searchNumber, ["MM-DD-YYYY", "MM/DD/YYYY"], true).isValid();
            break;
        case "mrn":
            pattern = /^[0-9]{1,9}$/;
            break;
        case "ssn":
            pattern = /^(\d{4}|\d{9})$/;
            break;
        case "dl#":
            pattern = /^[0-9]{1,8}$/;
            break;
        default:
            return false;
    }
    return (searchNumber && pattern.test(searchNumber));
};

var clearTimeError = function (error, errorBlock, formControl) {
    error.addClass('ng-hide');
    errorBlock.removeClass('has-error');
    if (formControl) {
        formControl.$setValidity('date', true);
    }
};

function formatPDFPhone(phonenum) {
    var regexObj = /^(?:\+?1[-. ]?)?(?:\(?([0-9]{3})\)?[-. ]?)?([0-9]{3})[-. ]?([0-9]{4})$/;
    if (regexObj.test(phonenum)) {
        var parts = phonenum.match(regexObj);
        var phone = '';
        phone += parts[1] + "-" + parts[2] + "-" + parts[3];
        return phone;
    }
    else {
        //invalid phone number
        return phonenum;
    }
}

var hasData = function (data) {
    if (data && data.DataItems && data.DataItems.length > 0)
        return true;
    return false;
};

var hasDetails = function (data) {
    if (data && data.length > 0)
        return true;
    return false;
};

var phoneThreeDigitsCheck = function (phoneControl, phoneModel) {
    if (phoneControl)
        var phoneNumber = phoneControl.$viewValue;
    //Prepending zeros only for 1-1 numbers BUGFIX - 11478 & 11482
    if (phoneModel && phoneNumber && phoneNumber.length == 3 && (/11$/.test(phoneNumber))) {
        phoneModel.Number = '0000000' + phoneNumber;
    }
};

var getDefaultFormName = function () {
    return $("form[is-default]").attr('name');
}
var defaultExpirationDate = 'ExpirationDate';
var defaultEffectiveDate = 'EffectiveDate';
var getPrimaryOrLatestData = function (value) {
    value = filterFutureOrExpiredRecords(value, defaultExpirationDate, defaultEffectiveDate);
    var primaryData = value.filter(function (item) {
        return (item.IsPrimary === true);
    });

    if (primaryData.length > 0) {
        return primaryData;
    }
    else {
        return value.sort(function (first, second) {
            return first.ModifiedOn < second.ModifiedOn ? 1 : -1;
        }).slice(0, 1);
    }
}

var getPrimaryOrLatestDataItems = function (value, expDateName, effDateName) {
    expDateName = expDateName ? expDateName : defaultExpirationDate;
    effDateName = effDateName ? effDateName : defaultEffectiveDate;
    var sortedActiveList = sortBasedModifiedOn(filterFutureOrExpiredRecords(value, expDateName, effDateName));
    var sortedExpiredList = sortBasedModifiedOn(value.filter(function (item) {
        return ((item[expDateName] ? isExpireDate(item[expDateName]) : false) || (item[effDateName] ? isFutureDate(item[effDateName]) : false));
    }));

    var sortedDataItems = sortedActiveList.concat(sortedExpiredList)
    return sortedDataItems;
}

var sortBasedModifiedOn = function (value) {
    return value.sort(function (first, second) {
        return (moment(moment(first.ModifiedOn)).toDate() < moment(moment(second.ModifiedOn)).toDate()) ? 1 : -1
    });
}

var parseJSON = function (value) {
    try {
        return JSON.parse(value);
    } catch (e) {
        //return undefined;
        console.log(e);
    }
};

var getDocumentStatus = function (statusID) {
    var status = '';
    switch (statusID) {
        case DOCUMENT_STATUS.Draft:
            status = "Draft";
            break;
        case DOCUMENT_STATUS.Completed:
            status = "Completed";
            break;
        case DOCUMENT_STATUS.Void:
            status = "Void";
            break;
        default:
            status = "Pending";
    }
    return status;
}

var printCommentsFromJSON = function (model) {
    var printModel = [];
    var commentData = JSON.parse(model);
    var timezone = new Date().toString().match(/\(([A-Za-z\s].*)\)/)[1];
    var commentDate = '';

    for (index = 0; index < commentData.length; index++) {
        commentDate = new Date(commentData[index].CommentDate);
        printModel.push(
        { text: [{ text: commentData[index].UserName, bold: true, fontSize: 9 }, { text: ' added on ' + commentDate.toLocaleString().replace('', '', '') + ' (' + timezone + ')' }], fontSize: 8 },
        { text: commentData[index].Comment + '\n\n', fontSize: 9 }
        )
    }
    printModel.push({ text: '', pageBreak: 'after' })
    return printModel
}

var repositionElement = function (reposElement) {
    windowHeight = $(window)[0].screen.availHeight;
    elementHeight = reposElement[0].getBoundingClientRect().height;
    var top = reposElement.parent()[0].getBoundingClientRect().top + 37;
    var left = reposElement.parent()[0].getBoundingClientRect().left;

    if (top + elementHeight < windowHeight) {
        reposElement.css('top', top + 'px');
        reposElement.css('left', left + 'px');
    }
    else {
        var bottom = reposElement.parent()[0].getBoundingClientRect().top - elementHeight;
        reposElement.css('top', bottom + 'px');
        reposElement.css('left', left + 'px');
    }
}

// **************************** Events ********************************** //

$(document).ready(function () {
    $(document.body)
.on('show.bs.modal', function () {
    $('#navigationBar').css('z-index', -1);
})
.on('hide.bs.modal', function () {
    $('#navigationBar').css('z-index', 1040);
});
    String.prototype.replaceAll = function (s, r) {
        return this.split(s).join(r)
    };
});

// Used for typehead smooth scrolling 
if (!Element.prototype.scrollIntoViewIfNeeded) {
    Element.prototype.scrollIntoViewIfNeeded = function (centerIfNeeded) {
        centerIfNeeded = arguments.length === 0 ? true : !!centerIfNeeded;

        var parent = this.parentNode,
                        parentComputedStyle = window.getComputedStyle(parent, null),
                        parentBorderTopWidth = parseInt(parentComputedStyle.getPropertyValue('border-top-width')),
                        parentBorderLeftWidth = parseInt(parentComputedStyle.getPropertyValue('border-left-width')),
                        overTop = this.offsetTop - parent.offsetTop < parent.scrollTop,
                        overBottom = (this.offsetTop - parent.offsetTop + this.clientHeight - parentBorderTopWidth) >= (parent.scrollTop + parent.clientHeight - 10),
                        overLeft = this.offsetLeft - parent.offsetLeft < parent.scrollLeft,
                        overRight = (this.offsetLeft - parent.offsetLeft + this.clientWidth - parentBorderLeftWidth) > (parent.scrollLeft + parent.clientWidth),
                        alignWithTop = overTop && !overBottom;

        if ((overTop || overBottom) && centerIfNeeded) {
            parent.scrollTop = this.offsetTop - parent.offsetTop - parent.clientHeight / 2 - parentBorderTopWidth + this.clientHeight / 2;
        }

        if ((overLeft || overRight) && centerIfNeeded) {
            parent.scrollLeft = this.offsetLeft - parent.offsetLeft - parent.clientWidth / 2 - parentBorderLeftWidth + this.clientWidth / 2;
        }

        if ((overTop || overBottom || overLeft || overRight) && !centerIfNeeded) {
            this.scrollIntoView(alignWithTop);
        }
    };
}

//We are using this function to give the defination of the CustomEvent in case of IE that is used in the Offlinedata.js
(function () {
    function CustomEvent(event, params) {
        params = params || { bubbles: false, cancelable: false, detail: undefined };
        var evt = document.createEvent('CustomEvent');
        evt.initCustomEvent(event, params.bubbles, params.cancelable, params.detail);
        return evt;
    };

    CustomEvent.prototype = window.Event.prototype;
    window.CustomEvent = CustomEvent;
})();

var getLookupFieldById = function (lookupList, ID, prop) {
    if (ID && lookupList && lookupList.length) {
        for (var i = 0; i < lookupList.length; i++) {
            if (lookupList[i].ID == ID) {
                return prop ? lookupList[i].prop : lookupList[i].Name;
            }
        }
        return '';
    }
    else {
        return '';
    }
}

var getTextorEmpty = function (val) {
    return val ? val : '';
}
var getValueOrEmpty = function (val) {
    return val ? val.toString() : '';
}

// ******************************PDFMAKE****************************** //
function formatPDFPhone(phonenum) {
    var regexObj = /^(?:\+?1[-. ]?)?(?:\(?([0-9]{3})\)?[-. ]?)?([0-9]{3})[-. ]?([0-9]{4})$/;
    if (regexObj.test(phonenum)) {
        var parts = phonenum.match(regexObj);
        var phone = '';
        phone += parts[1] + "-" + parts[2] + "-" + parts[3];
        return phone;
    }
    else {
        //invalid phone number
        return phonenum;
    }
}

function formatPDFSSN(ssn) {
    var regexObj = /^(?:\+?1[-. ]?)?(?:\(?([0-9]{3})\)?[-. ]?)?([0-9]{2})[-. ]?([0-9]{4})$/;
    if (regexObj.test(ssn)) {
        var parts = ssn.match(regexObj);
        var ssnFormatted = '';
        ssnFormatted += parts[1] + "-" + parts[2] + "-" + parts[3];
        return ssnFormatted;
    }
    else {
        //invalid ssn
        return ssn;
    }
}

function GetContactLettersWorkFlowDataKey(assessmentID) {
    var workflowDatakey = '';
    switch (assessmentID) {
        case 33:
            workflowDatakey = 'Intake-IDDLetters-DidNotKeepAppointmentLetter';
            break;
        case 34:
            workflowDatakey = 'Intake-IDDLetters-Intake10DayLetterNotification';
            break;
        case 35:
            workflowDatakey = 'Intake-IDDLetters-IDDIntakeNewAppointmentLetter';

            break;
        default:
            workflowDatakey = '';
            break;
    }
    return workflowDatakey;
}
function GetContactConsentsWorkflowDataKey(assessmentID) {
    var workflowDatakey = '';
    switch (assessmentID) {

        case 13:
            workflowDatakey = 'Consents-Agency-EHRPhotoID';
            break;
        case 14:
            workflowDatakey = 'Consents-Agency-HIPAAAcknowledgement';
            break;
        case 15:
            workflowDatakey = 'Consents-Agency-AssignmentofBenefits';
            break;
        case 16:
            workflowDatakey = 'Consents-Agency-InformedConsentforServices';
            break;
        case 30:
            workflowDatakey = 'Consents-Agency-AgainstProfessionalAdvice';
        case 20:
            workflowDatakey = 'Consents-Agency-AuthorizationtoDiscloseHealthInformation';
            break;
        case 29:
            workflowDatakey = 'Consents-Agency-ConsentforAudioRecording';
            break;
        case 28:
            workflowDatakey = 'Consents-Agency-ConsentforPublication';
            break;
        case 18:
            workflowDatakey = 'Consents-Agency-ConsenttoSearch';
        case 23:
            workflowDatakey = 'Consents-Agency-ConsumerRights';
            break;
        case 27:
            workflowDatakey = 'Consents-Agency-DeclinationofVoterRegistration';
            break;
        case 25:
            workflowDatakey = 'Consents-Agency-EmergencyMedicalCare';
            break;
        case 17:
            workflowDatakey = 'Consents-Agency-ExplanationofIDDServicesandSupports';
            break;
        case 19:
            workflowDatakey = 'Consents-Agency-GeneralRelease';
            break;
        case 26:
            workflowDatakey = 'Consents-Agency-OpportunitytoRegistertoVote';
            break;
        case 22:
            workflowDatakey = 'Consents-Agency-ProtectedHealthInformationAmendment';
            break;
        case 24:
            workflowDatakey = 'Consents-Agency-RightsAbuseandNeglectComplaintProceduresandTelephoneNumber';
            break;
        default:
            workflowDatakey = '';
            break;
    }

    return workflowDatakey;
}


function pdfmakeDefaults() {
    //MHMR Tarrant logo
    var agencyLogo = 'data:image/png;base64,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';

    var logoWidth = 180;
    var agencyName = 'MHMR Tarrant';
    var agencyAddressLine1 = '3840 Hulen St';
    var agencyAddressLine2 = '';
    var agencyCity = 'Fort Worth';
    var agencyState = 'TX';
    var agencyZip = '76107';

    return {
        agencyLogo: agencyLogo,
        logoWidth: logoWidth,
        agencyName: agencyName,
        agencyAddressLine1: agencyAddressLine1,
        agencyAddressLine2: agencyAddressLine2,
        agencyCity: agencyCity,
        agencyState: agencyState,
        agencyZip: agencyZip
    };
}
// ******************************PDFMAKE****************************** //