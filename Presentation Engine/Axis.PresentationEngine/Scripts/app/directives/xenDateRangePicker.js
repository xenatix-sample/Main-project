angular.module('xenatixApp')
    .directive("xenDateRangePicker", ['$timeout', function ($timeout) {
        return {
            restrict: "E",
            scope: {
                onChange: '&',
                elementId: '@',
            },
            link: function ($scope, element, attrs, ctrl) {
                var start = moment().startOf('month');//Start of month
                var end = moment().endOf('month');//end of month
                function setDate(start, end) {
                    $('.date-range-picker').html(start.format('MM/DD/YYYY') + ' - ' + end.format('MM/DD/YYYY'));
                    $scope.onChange({ startDate: start.format('MMMM D, YYYY'), endDate: end.format('MMMM D, YYYY') });
                    $('.date-range-events').css('cursor', 'pointer');
                    //$('.daterangepicker').css('top', '517px');
                }

                $('.date-range-events').daterangepicker({
                    startDate: start,
                    endDate: end,
                    ranges: {
                        'Today': [moment(), moment()],
                        'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
                        'Last 7 Days': [moment().subtract(6, 'days'), moment()],
                        'Last 30 Days': [moment().subtract(29, 'days'), moment()],
                        'This Month': [moment().startOf('month'), moment().endOf('month')],
                        'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                    }
                }, setDate);

                setDate(start, end);
            },
            templateUrl: '/Scripts/app/Template/DateRangePicker.html'
        }
    }]);