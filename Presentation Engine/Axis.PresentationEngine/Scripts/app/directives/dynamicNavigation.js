angular.module('xenatixApp')
    .directive("dynamicNavigation", ['$compile', function ($compile) {
    return {
        restrict: 'E',
        transclude: true,
        template: function (elem, attr) {
            return '<ul class="list-group text-uppercase">' +
                          '<placeholder />'+
                          '</ul>';
        },
        replace: true,
        link: function (scope, el, attrs) {
            var sections = '';
            if (attrs.linkarray) {
                var linkarray = JSON.parse(attrs.linkarray);
                if (linkarray && linkarray.length > 0) {
                    
                    angular.forEach(linkarray, function (item, key) {
                        sections += $("<workflow-action>").attr('data-title', item.title).attr('data-state-name', item.state).attr('data-state-key', item.title).attr('data-state-params', item.paramarray).attr('data-init-state', 'none').wrap('<div>').parent().html();
                    })
                }
                el.find('placeholder').replaceWith(sections);
                $compile($(el))(scope);
            }
        }
    };
}])