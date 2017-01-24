
// DEVELOPER: Chris Reed

(function () {
    angular.module('xenatixApp')
        .directive('checkboxDropdown', ['$timeout', 'scopesService', '_', '$window',
            function ($timeout, scopesService, _, $window) {
                var defOptions = {
                    AnimSpeed: 500,
                    ExclusiveSelectValue: null,
                    Events: {
                        Change: null
                    },
                    Labels: {
                        NoneSelected: 'None Selected',
                        MultiSelected: '{0} Selected',
                        AllSelected: 'All Selected ({0})'
                    },
                    NoneSelectedText: 'SELECT',
                    Fields: {
                        Text: 'Name',
                        Value: 'Id'
                    }
                };

                return {
                    restrict: 'E',
                    replace: true,
                    scope: {
                        items: '=',
                        selected: '=?',
                        options: '=?',
                        searchFn: '=?',
                        name: '@'
                    },
                    templateUrl: '/GetPartial?path=CheckboxDropdown',
                    link: function (scope, elem, attrs) {
                        var $w = angular.element($window);

                        //#region MODEL MODS/RESETS

                        // Model Resets
                        scope.items = scope.items || [];
                        scope.selected = scope.selected || [];

                        // DOM Elements
                        scope.panel = elem.find('.items-panel');
                        scope.list = elem.find('.list-panel');

                        scope.Search = { Text: '' };

                        // Options
                        scope.Op = $.extend(true, {}, defOptions, scope.options || {});
                        scope.Op.Name = scope.name || 'cbdd' + moment().unix();

                        // Selected Text
                        scope.SelText = {
                            // scope.SelText.Value
                            Value: '',
                            // scope.SelText.Update
                            Update: function () {
                                if (scope.selected.length < 1)
                                    scope.SelText.Value = scope.Op.Labels.NoneSelected + '';
                                else if (scope.selected.length == 1)
                                    scope.SelText.Value = scope.selected[0][scope.Op.Fields.Text];
                                else if (scope.selected.length == scope.items.length)
                                    scope.SelText.Value = scope.Op.Labels.AllSelected.replace('{0}', scope.selected.length);
                                else if (scope.selected.length > 1)
                                    scope.SelText.Value = scope.Op.Labels.MultiSelected.replace('{0}', scope.selected.length);
                            }
                        };

                        // DropDown List
                        scope.Lst = {
                            // scope.Lst.IsOpen
                            IsOpen: false,
                            // scope.Lst.Selected
                            Selected: [],
                            // scope.Lst.Unselected
                            Unselected: [],
                            // scope.Lst.Init
                            Init: function () {
                                scope.Lst.Selected = angular.copy(scope.selected);
                                _.forEach(scope.Lst.Selected, function (item) {
                                    item.Selected = true;
                                });

                                var cObj = {};
                                scope.Lst.Unselected = _.filter(scope.items, function (item) {
                                    cObj[scope.Op.Fields.Value] = item[scope.Op.Fields.Value];
                                    return !_.find(scope.selected, cObj);
                                });
                            },
                            // scope.Lst.Clear
                            Clear: function () {
                                scope.Lst.Selected = [];
                                scope.Lst.Unselected = [];
                            }
                        };

                        //#endregion


                        //#region PVT FNs

                        var hasFocus = function () {
                            return (elem.find(':focus').length + elem.find(':hover').length) > 0;
                        };

                        //#endregion


                        //#region Public FNs

                        scope.Open = function () {
                            if (scope.Lst.IsOpen !== true) {
                                scope.Lst.Init();

                                scope.Lst.IsOpen = true;
                                scopesService.store('cbdd_open', scope.Op.Name);
                            }
                        };

                        scope.Close = function () {
                            if (scope.Lst.IsOpen !== false) {
                                scope.Lst.IsOpen = false;

                                if (scopesService.get('cbdd_open') == scope.Op.Name)
                                    scopesService.store('cbdd_open', '');

                                scope.Lst.Clear();
                            }
                        };

                        scope.ToggleList = function () {
                            scope[scope.Lst.IsOpen ? 'Close' : 'Open'].apply();
                        };

                        scope.SelectAll = function () {
                            scope.selected = angular.copy(scope.items);
                            scope.Lst.Init();
                            scope.SelText.Update();
                        };

                        scope.DeselectAll = function () {
                            scope.selected = [];
                            scope.Lst.Init();
                            scope.SelText.Update();
                        };

                        scope.ToggleSelected = function (item) {
                            if (!item) return;

                            if (item.Selected === true) {
                                item.Selected = false;

                                // Find in selected items
                                var cObj = {};
                                cObj[scope.Op.Fields.Value] = item[scope.Op.Fields.Value];
                                var index = _.findIndex(scope.selected, cObj);

                                // If in selected items, remove
                                if (index >= 0)
                                    scope.selected.splice(index, 1);
                            }
                            else {
                                item.Selected = true;

                                // Find in items
                                var cObj = {};
                                cObj[scope.Op.Fields.Value] = item[scope.Op.Fields.Value];
                                var index = _.findIndex(scope.items, cObj);

                                // If in items, add to selected
                                if (index >= 0)
                                    scope.selected.push(angular.copy(item));
                            }

                            scope.SelText.Update();

                            if (scope.Op.Events && scope.Op.Events.Change)
                                scope.Op.Events.Change();
                        };

                        //#endregion


                        //#region WATCHERS/EVENTS

                        scope.SearchKeyPress = function (e) {
                            switch (e.keyCode) {
                                case 27:
                                    scope.Search.Text = '';
                                    scope.Lst.Init();
                                    break;
                            }
                        };

                        scope.$watch('Search.Text', function () {
                            if (scope.Lst.IsOpen && scope.Search.Text.length < 1)
                                scope.Lst.Init();
                        });

                        elem.find('input[type=text]')
                            .on('blur', function () {
                                if (!hasFocus())
                                    scope.$apply(scope.Close);
                            });


                        elem.find('.items-panel')
                            .on('mouseleave', function () {
                                if (!hasFocus())
                                    scope.$apply(scope.Close);
                            });

                        scopesService.subscribe('cbdd_open', function () {
                            if (scopesService.get('cbdd_open') != scope.Op.Name)
                                scope.Close();
                        });

                        //#endregion

                        // Init
                        scope.SelText.Update();
                    }
                };
            }
        ]);
}());