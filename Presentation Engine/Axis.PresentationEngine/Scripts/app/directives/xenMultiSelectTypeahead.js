angular.module('xenatixApp')
    .directive("xenMultiSelectTypeahead", ['$document', '$timeout', '$filter', function ($document, $timeout, $filter) {
        return {
            restrict: "E",
            scope: {
                ngModel: "=",
                elementId: '@',
                elementName: '@',
                label: '@',
                formName: '=',
                onChange: '&',
                selectedValues: '=',
                isRequired: '@',
                validationName: '@',
                showSelected: '@',
                targetTextKey: '@',  //Default would be text
                targetValueKey: '@', //Defalut would be value
                sourceTextKey: '@',  //Default would be text
                sourceValueKey: '@',  //Default would be value
                customSelectAllText: '@' // Default would be Select All
            },
            require: 'ngModel',
            link: function ($scope, element, attrs, ctrl) {
                $scope.targetTextKey = $scope.targetTextKey || 'text';
                $scope.targetValueKey = $scope.targetValueKey || 'value';
                $scope.sourceTextKey = $scope.sourceTextKey || 'text';
                $scope.sourceValueKey = $scope.sourceValueKey || 'value';
                $scope.customSelectAllText = $scope.customSelectAllText || 'Select All';

                var onItemChange = function () {
                    $timeout(function () {
                        $scope.onChange();
                    });
                }

                var getTargetModel = function (item) {
                    var retModel = {};
                    retModel[$scope.targetTextKey] = item[$scope.sourceTextKey];
                    retModel[$scope.targetValueKey] = item[$scope.sourceValueKey];
                    return retModel;
                }

                $timeout(function () {
                    $('#' + $scope.elementId).focus(function () { $(this).select(); });
                });

                $scope.$watch('ngModel', function (newValue, oldValue) {
                    $scope.isAllSelected = false;

                    if (newValue && newValue.length > 0) {
                        angular.forEach($scope.ngModel, function (item) {
                            if (item.isSelected == undefined) {
                                item.isSelected = false;
                            }
                        })
                        $scope.searchText = 'Select';
                    }
                    else {
                        $scope.searchText = 'No any items';
                    }

                    $scope.selectedItemsInCSV = null;
                    $scope.formName[$scope.elementName].$setValidity('invalidSearch', true);
                });

                $scope.$watch('selectedValues', function (newValue, oldValue) {
                    if (hasDetails(newValue) && newValue !== oldValue) {
                        angular.forEach($scope.ngModel, function (modelItem) {
                            $filter('filter')(newValue, function (selectedItem) {
                                if (modelItem[$scope.sourceValueKey] === selectedItem[$scope.targetValueKey]) {
                                    modelItem.isSelected = true;
                                }
                            });
                        })
                        loadItems();
                    }
                });

                $scope.items = [];
                $scope.toggle = function () {
                    if ($scope.ngModel.length > 0) {
                        $scope.isVisible = !$scope.isVisible;
                    }
                    if ($scope.isVisible) {
                        $('#' + $scope.elementId).focus();
                        loadItems();
                    }
                }

                $scope.search = function () {
                    $scope.items = $filter('filter')($scope.ngModel, function (item) {
                        return item[$scope.sourceTextKey].toLowerCase().includes($scope.searchText.toLowerCase());
                    });

                    if ($scope.items.length == 0) {
                        $scope.formName[$scope.elementName].$setValidity('invalidSearch', false);
                    }
                    else {
                        $scope.formName[$scope.elementName].$setValidity('invalidSearch', true);
                    }
                    getSelectedItemsCSV();
                    $scope.isVisible = $scope.items.length > 0;
                }

                var eventName = 'click.multiselecttyahead' + $scope.elementId;
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
                    var selectedValues = [];
                    angular.forEach($scope.ngModel, function (item) {
                        if (item.isSelected) {
                            selectedValues.push(getTargetModel(item));
                        } else {
                            $scope.isAllSelected = false;
                        }
                    })
                    $scope.selectedValues = selectedValues;
                    loadItems();
                    onItemChange();
                }

                $scope.removeItem = function (selectedItem) {
                    var modelItem = $filter('filter')($scope.ngModel, function (item) {
                        return item[$scope.sourceValueKey] === selectedItem[$scope.targetValueKey];
                    })[0];
                    modelItem.isSelected = false;
                    var index = $scope.selectedValues.indexOf(selectedItem);
                    $scope.selectedValues.splice(index, 1);
                    $scope.isAllSelected = false;
                    loadItems();
                };

                $scope.checkAll = function () {
                    var selectedValues = [];
                    angular.forEach($scope.ngModel, function (item) {
                        item.isSelected = $scope.isAllSelected;
                        item.isSelected && selectedValues.push(getTargetModel(item));
                    });
                    $scope.selectedValues = selectedValues;
                    loadItems();
                    onItemChange();
                }

                var loadItems = function () {
                    var selectedItems = getSelectedItems();
                    var unSelectedItems = getUnSelectedItems();
                    $scope.items = selectedItems.concat(unSelectedItems);
                    var selectedCount = countSelectedItems();
                    if (selectedCount > 0)
                        $scope.searchText = 'Items Selected (' + selectedCount + ')';
                    else {
                        $scope.searchText = 'Select';
                        $scope.formName[$scope.elementName + "_search"].$pristine = true;
                        $scope.formName[$scope.elementName].$pristine = true;
                    }

                    getSelectedItemsCSV();

                    $scope.formName[$scope.elementName].$setValidity('invalidSearch', true);
                }

                var getSelectedItemsCSV = function () {
                    $scope.selectedItemsInCSV = null;
                    if ($scope.selectedValues) {
                        $scope.selectedItemsInCSV = $scope.selectedValues.map(function (elem) {
                            return elem[$scope.sourceValueKey];
                        }).join(',');                        
                    }

                    if (!$scope.formName[$scope.elementName + "_search"].$pristine)
                    {
                        $scope.selectedItemsInCSV = $scope.selectedItemsInCSV + $scope.searchText;
                    }

                    console.log('selectedItemsInCSV');
                    console.log($scope.selectedItemsInCSV);
                    console.log('$pristine');
                    console.log($scope.formName[$scope.elementName + "_search"].$pristine);
                }

                var countSelectedItems = function () {
                    return getSelectedItems().length;
                }

                var getSelectedItems = function () {
                    return $filter('filter')($scope.ngModel, function (item) {
                        return item.isSelected;
                    });
                }

                var getUnSelectedItems = function () {
                    return $filter('filter')($scope.ngModel, function (item) {
                        return !item.isSelected;
                    });
                }
            },
            templateUrl: '/Scripts/app/Template/MultiSelectTypeahead.html'
        }
    }]);