
// DEVELOPER: Chris Reed

(function () {
    angular.module('xenatixApp')
        .directive('multiSelect', ['$timeout', '_', '$window',

            /*#region Example HTML
            <multi-select name="param.Name" items="obj.ItemCollection"
                selected="obj.SelectedItems" options="{ Fields: { Text: 'Label', Value: 'Value' } }">
            </multi-select>
            #endregion*/

            function ($timeout, _, $window) {
                var defOptions = {
                    AnimSpeed: 500,
                    ErrorDispTime: 5000,
                    SelectOnBlur: true,
                    Editable: false,
                    Fields: {
                        Text: 'Name',
                        Value: 'Id'
                    }
                },
                sv = { // Static Values
                    buttonsHeight: 22,
                    listItemHeight: 26,
                    itemsToShow: 15
                };

                return {
                    restrict: 'E',
                    replace: true,
                    scope: {
                        items: '=',
                        name: '@',
                        selected: '=?',
                        selectedValues: '=?',
                        options: '=?',
                        asyncSearch: '=?',
                        onChange: '=?'
                    },
                    templateUrl: '/GetPartial?path=MultiSelect',
                    link: function (scope, elem, attrs) {
                        var $w = angular.element($window);

                        scope.items = scope.items || [];
                        scope.selected = scope.selected || [];
                        scope.selectedValues = scope.selectedValues || [];
                        scope.search = { text: '' };
                        scope.loading = false;

                        //scope.Op = $.extend(true, {}, defOptions, scope.options || {});
                        scope.Op = angular.copy(defOptions);
                        _.extend(scope.Op, scope.options);

                        if (!scope.name)
                            scope.Op.Name = 'ms_' + moment().unix();
                        else
                            scope.Op.Name = scope.name;

                        scope.SelectItem = function (item) {
                            if (!item) return;

                            var i = scope.items.indexOf(item);

                            if (i >= 0)
                                scope.selected.push(angular.copy(item));

                            if (scope.Op.Events && scope.Op.Events.Change)
                                scope.Op.Events.Change();
                        };

                        scope.DeselectItem = function (item) {
                            if (!item) return;

                            var i = scope.selected.indexOf(item);

                            if (i >= 0)
                                scope.selected.splice(i, 1);

                            if (scope.Op.Events && scope.Op.Events.Change)
                                scope.Op.Events.Change();
                        };

                        scope.SelectItem();

                        // Error or No Results
                        scope.NoResults = '';

                        scope.$watch('NoResults', function () {
                            $timeout(function () {
                                scope.NoResults = false;
                            }, scope.Op.ErrorDispTime);
                        }, true);

                        elem.find('p.error-block').css('z-index', 100);
                    }
                };
            }
        ]);
}());