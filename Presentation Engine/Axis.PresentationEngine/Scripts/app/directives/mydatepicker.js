angular.module('xenatixApp')
    .directive("mydatepicker", ['alertService', '$timeout',
        '$compile', function (alertService, $timeout, $compile) {
            return {
                restrict: "E",
                scope: {
                    ngModel: "=",
                    dateOptions: "=",
                    opened: "=?",
                    newDate: "=",
                    dateRequired: '=',
                    callFunctionOnChange: '&',
                    endDate: '=',
                    startDate: '=',
                    dobName: '@',
                    eventFocus: '@',
                    isDisabled: '=?',
                    isHidden: '=?'
                },
                link: function ($scope, element, attrs) {
                    $scope.opencalendar = function(event) {
                        event.preventDefault();
                        $scope.opened = true;
                        $("input[name='" + $scope.dobName + "'] ~ .dropdown-menu").show();
                        $('.datepicker_cal').on('keydown', function(e) {
                            if (e.which == 9) {
                                $("input[name='" + $scope.dobName + "'] ~ .dropdown-menu").hide();
                            }
                        });
                    };

                    $scope.clear = function() {
                        $scope.ngModel = null;
                    };

                    var getDate = function (dateTime) {
                        var d = new Date(dateTime);
                        d.setHours(0, 0, 0, 0);
                        return d;
                    };

                    $scope.change = function () {
                        // Check for future/past date validation
                        var newdate = new Date($scope.newDate);

                        if ($scope.newDate && newdate !== 'Invalid Date') {
                            var cdtmp = new Date();
                            var curdate = new Date(cdtmp.getFullYear(), cdtmp.getMonth(), cdtmp.getDate());
                            if (attrs['futuredatevalidate'] && attrs['futuredatevalidate'] == 'true') {
                                if (getDate(newdate) > curdate) {
                                    $scope.newDate = null;
                                    alertService.error("Date cannot be in the future");
                                    return;
                                }
                            }
                            if (attrs['pastdatevalidate'] && attrs['pastdatevalidate'] == 'true') {
                                if (newdate < curdate) {
                                    $scope.newDate = null;
                                    alertService.error("Date cannot be in the past");
                                }
                            }
                            }

                        if (typeof $scope.callFunctionOnChange == 'function') {
                           $timeout(function () {
                                $scope.callFunctionOnChange();
                            });
                        }
                   }
                },
                templateUrl: '/GetPartial?path=Datepicker'
            }
        }
    ]);
