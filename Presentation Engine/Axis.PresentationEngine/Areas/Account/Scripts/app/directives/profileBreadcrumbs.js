angular.module('xenatixApp')
    .directive('profileBreadcrumbs', ['$state', '$compile', '$rootScope',
        function ($state, $compile, $rootScope) {
            return {
                restrict: 'E',
                replace: true,
                template: '<div class="breadcrumb_nav"><ul class="breadcrumb"><li class="current">{{currentStateTitle}}</li></ul>​</div>',
                link: function (scope, el, attrs) {
                    scope.refreshBreadcrumbs = function () {
                        var myProfileTitle = $state.current.title || '';
                        scope.currentStateTitle = attrs.ngTitle != undefined ? attrs.ngTitle : 'My Profile / ' + myProfileTitle;
                        $compile(el.contents())(scope);
                    };
                    scope.refreshBreadcrumbs();
                    $rootScope.$on("$stateChangeSuccess", scope.refreshBreadcrumbs);
                }
            };
        }
    ]);