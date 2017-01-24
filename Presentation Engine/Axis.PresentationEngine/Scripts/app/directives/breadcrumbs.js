angular.module('xenatixApp')
    .directive('breadcrumbs', [ '$state', '$compile', '$rootScope',
        function($state, $compile, $rootScope) {
            return {
                restrict: 'E',
                replace: true,
                template: '<div class="breadcrumb_nav" ng-show="areBreadcrumbsVisible"><ul class="breadcrumb"></ul>​<a class="pull-right close close-view"></a></div>',
                link: function (scope, el, attrs) {
                    var minimumBreadcrumbs = parseInt('0' + (attrs.minBreadcrumbs || ''));
                    scope.refreshBreadcrumbs = function() {
                        var state = $state.current;
                        var parent = '^';
                        var stateHtml = '';
                        var lastState = '';
                        if (state.title)
                            stateHtml = $('<ul><li>').children('li').addClass('current').attr('ng-bind-template', state.title).parent().html();
                        state = $state.get(parent);
                        parent = parent + '.^';
                        while (state.name !== '') {
                            if (state.title) {
                                if (lastState === '')
                                    lastState = state.name;
                                stateHtml = $("<ul><li><a>").children('li').children('a').attr('href', 'javascript:void(0)').attr('ng-click', 'Goto(\'' + (attrs.goto || (state.name == 'patientprofile' ? 'patientprofile.general' : state.name)) + '\')').attr('ng-bind-template', state.title).parent().parent().html() + stateHtml;
                            }
                            state = $state.get(parent);
                            parent = parent + '.^';
                        }
                        $(el).find('ul').html(stateHtml);
                        var closeHtml = '<i class="fa fa-times-circle padding-small" href="javascript:void(0)" ng-click="Goto(\'' + (attrs.goto || lastState) + '\')"><span class="sr-only">Close Section and Return</span></i>';
                        $(el).find('a.close').html(closeHtml);

                        scope.areBreadcrumbsVisible = $(el).find('li').length >= minimumBreadcrumbs;
                        $compile(el.contents())(scope);
                    };

                    scope.areBreadcrumbsVisible = false;
                    scope.refreshBreadcrumbs();
                    $rootScope.$on("$stateChangeSuccess", scope.refreshBreadcrumbs);
                }
            };
        }
    ]);