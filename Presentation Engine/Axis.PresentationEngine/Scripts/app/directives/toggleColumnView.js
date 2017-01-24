angular.module('xenatixApp')
    .directive('toggleColumnView', ['$state', '$rootScope', '$filter',
        function ($state, $rootScope, $filter) {

            var showHideColumnView = function (currentColumnViewState) {
                if (currentColumnViewState) {
                    $('.tiles .row').removeClass('tile-view').addClass('list-view');
                    $('.tile-toggle i').removeClass('fa-list-ul').addClass('fa-th');
                }
                else {
                    $('.tiles .row').removeClass('list-view').addClass('tile-view');
                    $('.tile-toggle i').removeClass('fa-th').addClass('fa-list-ul');
                }
            };

            return {
                restrict: 'A',
                replace: false,
                link: function (scope, elem) {
                    var currentState = $state.current.name;
                    var currentStateColumnView = $filter('filter')($rootScope.TileColumnViewStates, { stateName: currentState });
                    var currentColumnViewState = false;
                    if (currentStateColumnView != undefined && currentStateColumnView.length > 0) {
                        currentColumnViewState = currentStateColumnView[0].IsColumnView;
                    } 
                    elem.off('click').on('click', function (e) {
                        if (currentStateColumnView != undefined && currentStateColumnView.length > 0) {
                            currentStateColumnView[0].IsColumnView = !currentStateColumnView[0].IsColumnView;
                            //update the current state's IsColumnView to match currentStateColumnView[0].IsColumnView below
                            var index = $rootScope.TileColumnViewStates.map(function (e) { return e.stateName; }).indexOf(currentState);
                            $rootScope.TileColumnViewStates[index].IsColumnView = currentStateColumnView[0].IsColumnView;
                            currentColumnViewState = currentStateColumnView[0].IsColumnView;
                        }
                        else {
                            $rootScope.TileColumnViewStates.push({ stateName: $state.current.name, IsColumnView: true });
                            currentStateColumnView.push({ stateName: $state.current.name, IsColumnView: true });
                            currentColumnViewState = true;
                        }
                        showHideColumnView(currentColumnViewState);
                        e.preventDefault();
                    });
                    showHideColumnView(currentColumnViewState);
                }
            };
        }
    ]);