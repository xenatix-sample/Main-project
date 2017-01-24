angular.module('xenatixApp').directive('plusButtonGridClear', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            //to remove green color from grid row if button is clicked.
            if (attrs['tableId'] != undefined) {
                element.off('click').on('click', function (e) {
                    //to remove green color from grid row if button is clicked.
                    ClearGridSelection(attrs);
                    //
                });
            }
        }
    }
});