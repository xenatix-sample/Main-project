
// DEVELOPER: Chris Reed

(function () {
    angular.module('xenatixApp')
        .directive('inputLabel', ['_',
            function (_) {

                return {
                    restrict: 'A',
                    require: 'ngModel',
                    link: function (scope, elem, attrs, ngModel) {
                        var name = attrs.name || 'inputLabel_' + moment().unix();

                        var parent = elem.parent();
                        parent.css('position', 'relative');

                        elem.attr('name', name);

                        scope.Label = $('<label>')
                            .attr('for', name)
                            .addClass('input-label')
                            .insertBefore(elem);

                        var posCheck = function () {
                            if (elem.is(':focus') === false && elem.val().length < 1)
                                scope.Label.removeClass('shrink');
                            else
                                scope.Label.addClass('shrink');
                        };

                        //#region Watches FNs

                        elem.on('focus', posCheck)
                        .on('blur', posCheck);

                        scope.$watch(function () {
                            return ngModel.$modelValue;
                        }, posCheck, true);

                        scope.$watch(attrs.inputLabel, function (val) {
                            scope.Label.text(val);
                        }, true);

                        //#endregion

                        // Init
                        posCheck();
                    }
                };
            }
        ]);
}());

//function (newValue) 
//{
//    if (newValue.length < 1)
//        elem.addClass('empty');
//    else
//        elem.removeClass('empty');
//}