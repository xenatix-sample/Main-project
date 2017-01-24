angular.module('xenatixApp')
    .directive("xenTimePicker", ['alertService',
        '$compile', '$filter', function (alertService, $compile, $filter) {
            return {
                restrict: "E",
                scope: {
                    ngModel: "=",
                    formName: '=',
                    timeRequired: '=',
                    elementName: '@',
                    elementId: '@',
                    label: '@',
                    validationMessage: '@',
                    isDisabled: '=?',
                    onTimeChange: '&'
                },
                require: 'ngModel',
                link: function ($scope, element, attrs, ctrl) {
                    $scope.time = '';
                    $scope.AMPM = '';
                    if ($scope.ngModel) {
                        // if time has 00:00:01 then reset time
                        var isAdjustedTime = searchString(moment($scope.ngModel).toDate(), "00:00:01");
                        if (!isAdjustedTime) {
                            $scope.time = $filter('formatDate')($scope.ngModel, 'hh:mm');
                            var timePart = moment($scope.ngModel).format("HH:mm");
                            $scope.AMPM = $filter('toStandardTimeAMPM')(timePart);
                        }
                    }

                    $scope.onChange = function () {
                        if (($scope.time && !$scope.AMPM)) {
                            $scope.formName[$scope.elementName].$setValidity("invalidTime", false);
                            return;
                        }
                        else if ($scope.time && $scope.AMPM) {
                            var dateTime = buildDateTime(moment($scope.time, "hhmm").format("hh:mm") + ' ' + $scope.AMPM);
                            var isValid = moment(moment($scope.time, "HHmm").format("HH:mm") + ' ' + $scope.AMPM, "LT", true).isValid();
                            if (isValid)
                                $scope.ngModel = dateTime;
                            $scope.formName[$scope.elementName].$setValidity("invalidTime", isValid);
                            return;
                        }
                        else if (!$scope.time) {
                            // set time to 00:00:01
                            var dateTime = buildDateTime('12:00:01 AM');
                            $scope.ngModel = dateTime;
                        }
                        $scope.formName[$scope.elementName].$setValidity("invalidTime", true);
                    }

                    var buildDateTime = function (time) {
                        var isValidDate = moment($filter('formatDate')($scope.ngModel, 'MM/DD/YYYY'), "MM/DD/YYYY", true).isValid();
                        var date = $scope.ngModel;
                        if (!isValidDate)
                            date = new Date();
                        date = $filter('formatDate')(date, 'MM/DD/YYYY');

                        return date + ' ' + time;
                    }
                },
                templateUrl: '/Scripts/app/Template/TimePicker.html'
            }
        }
    ]);