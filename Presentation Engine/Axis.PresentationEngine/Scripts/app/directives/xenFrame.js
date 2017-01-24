angular.module('xenatixApp')
    .directive('xenFrame', [
        function () {
            return {
                restrict: 'E',
                replace: true,
                scope: {
                    url: '=',
                    classes: '@?',
                    ngClass: '=',
                    fullWidth: '@?'
                },
                template: '<iframe class="xen-frame {{classes}}" ng-class="ngClass" style="width: 100%;" ng-src="{{url}}"></iframe>',
                link: function (scope, elem, attrs) {

                    var resize = function () {
                        if (scope.fullWidth)
                            elem.width(elem.parent().innerWidth());
                    };

                    //scope.$watch('url', function () {
                    //    console.log('url: ' + scope.url);
                    //});
                    //console.log('url: ' + scope.url);

                    elem.parent().on('resize', resize);
                    elem.css({
                        margin: 0,
                        padding: 0
                    });
                }
            };
        }
    ]);
