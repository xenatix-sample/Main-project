angular.module('xenatixApp')
.directive('xenProviders', ['$http', '$q', '$filter', 'providersService', function ($http, $q, $filter, providersService) {
    return {
        restrict: 'E',
        replace: true,
        scope: {
            providerModel: '=',
            providerLists: '=',
            elementName: '@',
            elementId: '@',
            elementClass: '@',
            validationName: '@',
            isDisabled: '=',
            isRequired: '=',
            filterCriteria: '@',
            primaryKey: '=',
            elementType: '@',
            onSelect: '&'
        },
        templateUrl: function (element, attr) {
            if (attr.elementType == 'select')
                return '/Scripts/app/Template/SelectProviders.html';
            else if (attr.elementType == 'typeahead')
                return '/Scripts/app/Template/TypeaheadProviders.html';
            else if (attr.elementType == 'multiselect')
                return '/Scripts/app/Template/MultiSelectProviders.html';
        },
        link: function ($scope, $element, attrs) {
            $scope.$watch('providerModel', function (newValue, oldValue) {
                if ($scope.elementType != 'select' && newValue != oldValue) {
                    bindProviders();
                }
            });

            $scope.onProviderSelected = function (item) {
                $scope.providerModel = item.ID;
                if ($scope.onSelect)
                    $scope.onSelect({ item: item });
            }
 
            $scope.addProvider = function (item) {
                var copyOfItem = angular.copy(item);
                var isAlreadyInList = $filter('filter')($scope.providerLists, function (provider) {
                    return (provider.ID == copyOfItem.ID) && provider.IsActive == 1;
                }, true);

                if (copyOfItem && isAlreadyInList.length == 0) {
                    copyOfItem.UserID = copyOfItem.ID;
                    copyOfItem.IsActive = true;
                    copyOfItem.IsNew = true;
                    $scope.providerLists.push(copyOfItem);
                }
                $scope.providerModel = null;
            };

            $scope.removeProvider = function (item) {
                var index = $scope.providerLists.indexOf(item);
                if (index !== -1 && $scope.providerLists[index]) {
                    if (!$scope.providerLists[index].IsNew) {
                        $scope.providerLists[index].IsActive = false;
                    }
                    else {
                        $scope.providerLists.splice(index, 1);
                    }
                }
            }

            $scope.$watch('selectedProvider', function (newValue, oldValue) {
                if ($scope.elementType == 'typeahead' && newValue != oldValue && newValue==null) {
                    $scope.providerModel = null;
                }
            });
            $scope.triggerTypeahead = function (element) {
                $('[name="' + element + '"]').focus();
                $('[name="' + element + '"]').trigger('forcelyOpenTypeaheadPopup');
            };

            $scope.$watch('primaryKey', function (newValue, oldValue) {
                if (newValue != oldValue) {
                    bindProviders();
                }
            });

            var bindProviders = function () {
                var activeProviders = $filter('filter')($scope.providers, function (provider) { return provider.IsActive != false }, true);

                var selectedProvider = $filter('filter')(activeProviders, function (provider) { return provider.ID == $scope.providerModel }, true);

                if ($scope.primaryKey && !(selectedProvider && selectedProvider.length > 0)) {
                    providersService.getProviderbyid($scope.providerModel).then(function (response) {
                        if (hasData(response)) {
                            var provider = response.DataItems[0];
                            var inActiveProvider = {
                                ID: provider.ID,
                                Name: provider.Name,
                                IsActive: false,
                                Number: provider.Number,
                                PhoneID: provider.PhoneID,
                                ContactNumberID: provider.PhoneID
                            };

                            activeProviders.push(inActiveProvider);
                            setSelectedProvider(inActiveProvider);
                        }
                        $scope.providers = activeProviders;
                    })
                }
                else {
                    setSelectedProvider(selectedProvider[0]);
                    $scope.providers = activeProviders;
                }
            }

            var setSelectedProvider = function (selectedProvider) {
                if ($scope.elementType == 'typeahead' && selectedProvider) {
                    $scope.selectedProvider = selectedProvider;
                }
            }

            var getProviders = function () {
                providersService.getProviders($scope.filterCriteria).then(function (response) {
                    $scope.providers = response.DataItems;
                });
            }

            getProviders();
        }
    }
}]);