angular.module('xenatixApp')
    .factory('dateTimeValidatorService', ['$filter', function ($filter) {
        function validateFutureDateTime(errorControlBlock, errorControl, formControl, dateControl, time, ampm, selector, formName) {
            var datePart = $filter('formatDate')(dateControl, 'MM/DD/YYYY');
            var timeControl = moment($filter('formatDate')(datePart + ' ' + $filter('toStandardTime')(time) + ' ' + ampm, 'MM/DD/YYYY HH:mm')).toDate();
            var compareTimeControl = new Date();
            formatTimeToDate(datePart, time, selector, formName, errorControlBlock);
            if (dateControl
                && time
                && ampm
                && moment(timeControl).isValid()) {
                validateTime(errorControlBlock, errorControl, formControl, dateControl, timeControl, compareTimeControl);
            }
            else {
                validateTime(errorControlBlock, errorControl, formControl, dateControl, null, compareTimeControl);
            }
        };

        function formatTimeToDate(dateVal, timeVal, timeSelector, formName, errorControlBlock) {
            timeVal = timeVal || '';
            if (timeVal.indexOf(':') == -1) {
                timeVal = timeVal.substring(0, 2) + ':' + timeVal.substring(2, timeVal.length);
            }
            var hr = timeVal.substring(0, timeVal.indexOf(':'));

            var min = timeVal.substring(timeVal.indexOf(':') + 1, timeVal.length);
            validateTimeFormat(hr, min, timeSelector, formName, errorControlBlock);
        };

        function validateTimeFormat(hour, minute, timeSelector, formName, errorControlBlock) {
            var isValidTime = true;
            var hr = hour.toString();
            if (hr && hr.length > 0 && minute && minute.length > 0) {
                if (hour < 1 || hour > 12) {
                    isValidTime = false;
                }

                if (minute < 0 || minute > 59) {
                    isValidTime = false;
                }

                setFormValidations(timeSelector, 'pattern', isValidTime, formName, errorControlBlock);
            }
            else {
                setFormValidations(timeSelector, 'pattern', true, formName, errorControlBlock);
            }

            return isValidTime;
        };

        function setFormValidations(elemSelector, error, isValid, formName, errorControlBlock) {
            if (formName[elemSelector]) {
                formName[elemSelector].$setValidity(error, isValid);
                if ($.isEmptyObject(formName[elemSelector].$error)) {
                    errorControlBlock.removeClass('has-error');
                }
                else {
                    errorControlBlock.addClass('has-error');
                    formName[elemSelector].$invalid = true;
                    formName[elemSelector].$valid = false;
                }
            }
        }

        function validateCallTime(date, startTime, endTime, callStartAMPM, callEndAMPM) {
            var callStartDate = angular.isDate(date) ? $filter('formatDate')(date, 'MM/DD/YYYY') : date;
            var fullStartDateTime = moment(callStartDate + ' ' + $filter('toStandardTime')(startTime) + ' ' + callStartAMPM).toDate();
            var fullEndDateTime = moment(callStartDate + ' ' + $filter('toStandardTime')(endTime) + ' ' + callEndAMPM).toDate();
            var currentTime = new Date();

            //The currenttime needed to be formatted correctly
            var validTime = (!moment(fullStartDateTime).isValid() || !moment(fullEndDateTime).isValid())
                || ((fullStartDateTime <= fullEndDateTime) && (fullEndDateTime <= currentTime));

            return validTime;
        }

        function getCurrentMeridian (date) {
            var currentHour = date.getHours();
            var currentMinute = date.getMinutes();

            var period = currentHour >= 12 ? 'pm' : 'am';
            return $filter('toMilitaryTime')(pad(currentHour, 2) + ":" + pad(currentMinute, 2), period);
        };

        return {
            validateFutureDateTime: validateFutureDateTime,
            validateCallTime: validateCallTime,
            getCurrentMeridian: getCurrentMeridian
        };
    }]);