angular.module('xenatixApp')
.directive("defaultdatepicker", function () {
    return {
        restrict: "E",
        scope: {
            ngModel: "=",
            dateOptions: "=",
            opened: "=",
            newDate: "=",
            dateRequired: '=',
            endDate: '=',
            startDate: '='
        },
        link: function ($scope, element, attrs) {
            $scope.opencalendar = function (event) {
                event.preventDefault();
                event.stopPropagation();
                $scope.opened = true;
                $('body div.datepicker .dropdown-menu').show();
                $('.datepicker_cal').on('keydown', function (e) {
                    if (e.which == 9) {
                        $('body div.datepicker .dropdown-menu').hide();
                    }
                });
            };

            $scope.clear = function () {
                $scope.ngModel = null;
            };
        },
        templateUrl: '/GetPartial?path=Datepicker'
    }
});