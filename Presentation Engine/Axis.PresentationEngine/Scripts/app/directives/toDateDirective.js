angular.module('xenatixApp').directive('dateDirective', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelController) {
            ngModelController.$parsers.push(function (data) {
                //convert data from view format to model format
                return data; //converted
            });

            ngModelController.$formatters.push(function (data) {
                //convert data from model format to view format

                if (data == undefined) {
                    return "";
                }

                var localDateTime;
                if (data.indexOf('Date') > -1)
                    localDateTime = new Date(parseInt(data.replace(/\/Date\((.*?)\)\//gi, "$1")));
                else
                    localDateTime = new Date(data);

                localDateTime.setTime(localDateTime.getTime() - localDateTime.getTimezoneOffset() * 60 * 1000);
                var finalDate = pad(localDateTime.getDate(), 2) + '/' + pad((localDateTime.getMonth() + 1), 2) + '/' + localDateTime.getFullYear();

                return finalDate;
            });
        }
    }
});

angular.module('xenatixApp').directive('datemdyDirective', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelController) {
            ngModelController.$parsers.push(function (data) {
                //convert data from view format to model format
                return data; //converted
            });

            ngModelController.$formatters.push(function (data) {
                //convert data from model format to view format

                if (data == undefined || data == "") {
                    return "";
                }

                var localDateTime;
                if (data.indexOf('Date') > -1)
                    localDateTime = new Date(parseInt(data.replace(/\/Date\((.*?)\)\//gi, "$1")));
                else
                    localDateTime = new Date(data);                

                localDateTime.setTime(localDateTime.getTime() - localDateTime.getTimezoneOffset() * 60 * 1000);
                var finalDate = pad((localDateTime.getMonth() + 1), 2) + '/' + pad(localDateTime.getDate(), 2) + '/' + localDateTime.getFullYear();

                return finalDate;
            });
        }
    }
});

