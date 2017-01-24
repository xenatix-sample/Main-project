angular.module('xenatixApp')
    .directive("xenMultiSelect", ['$document', '$timeout', function ($document, $timeout) {
        return {
            restrict: "E",
            scope: {
                ngModel: "=",
                onChange: '&',
                selectedValues: '='
            },
            require: 'ngModel',
            link: function ($scope, element, attrs, ctrl) {
                var onItemChange = function () {
                    $timeout(function () {
                        $scope.onChange();
                    });
                }

                $scope.toggle = function () {
                    if ($scope.ngModel.length > 0)
                        $scope.isVisible = !$scope.isVisible;
                }

                $scope.$watch('ngModel', function () {
                    $scope.isAllSelected = true;
                    $scope.checkAll();
                });

                var eventName = 'click.multiselect';
                $scope.$on('$destroy', function () {
                    $document.off(eventName);
                });

                element.bind('click', function (event) {
                    event.stopPropagation();
                });

                $document.off(eventName).on(eventName, function (e) {
                    $scope.isVisible = false;
                    $scope.$apply();
                })

                $scope.onSelect = function () {
                    $scope.isAllSelected = true;
                    $scope.selectedValues = [];
                    angular.forEach($scope.ngModel, function (item) {
                        if (item.isSelected) {
                            $scope.selectedValues.push(item);
                        } else {
                            $scope.isAllSelected = false;
                        }
                    })
                    onItemChange();
                }

                $scope.checkAll = function () {
                    $scope.selectedValues = [];
                    angular.forEach($scope.ngModel, function (item) {
                        item.isSelected = $scope.isAllSelected;
                    })
                    if ($scope.isAllSelected)
                        $scope.selectedValues = $scope.ngModel;
                    onItemChange();
                }
            },
            templateUrl: '/Scripts/app/Template/MultiSelect.html'
        }
    }]);