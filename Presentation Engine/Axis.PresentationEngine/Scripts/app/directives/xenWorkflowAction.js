angular.module('xenatixApp')
    .directive('xenWorkflowAction', ['$rootScope', '$compile', '$state', '$timeout', 'alertService', 
        function ($rootScope, $compile, $state, $timeout, alertService) {
            function refreshContent(active, workflowAction) {
                var element = workflowAction.element;
                var validationState = workflowAction.validationState;
                element.children('a').removeClass().addClass((active ? 'btn-block ' : validationState == 'invalid' ? 'text-danger' : validationState == 'warning' ? 'text-warning' : validationState == 'init' ? 'text-muted' : 'text-success'));
                element.children('a').children('i').removeClass('fa-pencil fa-check-circle fa-times-circle fa-warning ').addClass(active ? 'fa-pencil ' : validationState == 'valid' ? 'fa-check-circle' : validationState == 'invalid' ? 'fa-times-circle' : validationState == 'warning' ? 'fa-warning' : '');

                if (active)
                    element.addClass('active');
                else
                    element.removeClass('active');
            };

            function onValidationHandler(event, args) {
                if (args.validationState) {
                    var stateName = event.name;
                    var workflowAction = resolveWorkflowAction(stateName, args.stateKey || '');
                    if (workflowAction) {
                        if (!(args.validationState === 'init'))
                            workflowAction.element.children('a').children('i').removeClass('fa-spinner fa-spin');
                        workflowAction.validationState = args.validationState;
                        refreshContent($state.is(stateName) || $state.is(workflowAction.initActive) || $state.is(workflowAction.stateName, $rootScope.$eval(workflowAction.stateParams)), workflowAction);
                    }
                }
            };

            function resolveWorkflowAction(stateName, stateKey) {
                if ($rootScope.workflowActions !== null) {
                    if (stateName in $rootScope.workflowActionMappings)
                        return $rootScope.workflowActions[$rootScope.workflowActionMappings[stateName]];
                    else if (stateName + stateKey in $rootScope.workflowActions)
                        return $rootScope.workflowActions[stateName + stateKey];
                    else
                        return $rootScope.workflowActions[stateName];
                } else
                    return undefined;
            };

            function onStateChangeHandler(event, toState, toParams, fromState, fromParams) {
                $timeout(function () {
                    var fromStateKey = $("li[data-state-name='" + fromState.name + "'],li[data-init-active='" + fromState.name + "']").children("a").children("i.fa-pencil").parent().parent().map(function () {
                        return this.attributes['data-state-name'].value + (this.attributes['data-state-key'] ? this.attributes['data-state-key'].value : '');
                    }).get();
                    if (fromStateKey.length == 0) {
                        fromStateKey=  $("xen-workflow-action[data-state-name='" + fromState.name + "'],li[data-init-active='" + fromState.name + "']").children("li").children("a").children("i.fa-pencil").parent().parent().parent().map(function () {
                            return this.attributes['data-state-name'].value + (this.attributes['data-state-key'] ? this.attributes['data-state-key'].value : '');
                        }).get();
                    }
                    if (fromStateKey.length == 0) {
                        fromStateKey = fromState.name;
                    }
                    var workflowAction = resolveWorkflowAction(fromStateKey);
                    if (workflowAction)
                        refreshContent(false, workflowAction);
                });

                $timeout(function () {
                    var toStateKey = $("li[data-state-name='" + toState.name + "'].active,li[data-init-active='" + toState.name + "'].active").map(function () {
                        return this.attributes['data-state-name'].value + (this.attributes['data-state-key'] ? this.attributes['data-state-key'].value : '');
                    }).get();
                    if (toStateKey.length == 0) {
                        toStateKey = $("xen-workflow-action[data-state-name='" + toState.name + "'],li[data-init-active='" + toState.name + "']").children("li").children("a").children("i.fa-pencil").parent().parent().parent().map(function () {
                            return this.attributes['data-state-name'].value + (this.attributes['data-state-key'] ? this.attributes['data-state-key'].value : '');
                        }).get();
                    }
                    if (toStateKey.length == 0) {
                        toStateKey = toState.name;
                    }
                    var workflowAction = resolveWorkflowAction(toStateKey);
                    if (workflowAction)
                        refreshContent(true, workflowAction);
                });
            };

            return {
                restrict: 'E',
                scope: {
                    isActive: '=',
                    onWorkflowClick: '&?'
                },
                template: '<li class="list-group-item"><a><i class="fa fa-fw pull-right fa-spinner fa-spin" /></a></li>',
                link: function (scope, el, attrs) {
                    var stateName = attrs.stateName;
                    var stateKey = (attrs.stateKey && attrs.stateKey.length > 0) ? attrs.stateKey : '';
                    var stateParams = (attrs.stateParams && attrs.stateParams.length > 0) ? attrs.stateParams : '{}';
                    $rootScope.workflowActions = $rootScope.workflowActions || {};
                    $rootScope.workflowActionMappings = $rootScope.workflowActionMappings || {};
                    $rootScope.workflowActions[stateName + stateKey] = {
                        element: el.find('li'),
                        validationState: '',
                        initActive: attrs.initActive,
                        initState: attrs.initState,
                        stateParams: stateParams,
                        stateName: stateName
                    };
                    if (attrs.initActive && (attrs.initActive.length > 0)) {
                        el.find('li').attr('ng-class', "{active: $state.is('" + stateName + "'," + stateParams + ") || $state.is('" + attrs.initActive + "')}");
                        $rootScope.workflowActionMappings[attrs.initActive] = stateName;
                    }
                    else
                        el.find('li').attr('ng-class', "{active: $state.is('" + stateName + "'," + stateParams + ")}");
                    
                    el.find('a').prepend(attrs.title);

                    $rootScope.$on(stateName + stateKey, onValidationHandler.bind(scope));
                    if ($rootScope.workflowActionsStateChangeHandler)
                        $rootScope.workflowActionsStateChangeHandler();
                    $rootScope.workflowActionsStateChangeHandler = $rootScope.$on('$stateChangeSuccess', onStateChangeHandler);
                    $compile(el.contents())(scope);
                    refreshContent($state.is(stateName) || (attrs.initActive && $state.is(attrs.initActive)), resolveWorkflowAction(stateName, stateKey));
                    if (attrs.initState)
                        $rootScope.$broadcast(stateName + stateKey, { validationState: attrs.initState, stateKey: stateKey });

                    el.children('a').on('keydown', function (e) {
                        if (e.keyCode == 32) {
                            $(this).click();
                        }
                    });

                    scope.$watch('isActive', function (newValue, oldValue) {
                        if (newValue == true) {
                            angular.element(el).find('a').off('click');
                            angular.element(el).find('a').attr('ng-click', scope.onWorkflowClick ? 'callOnClick()' : 'Goto("' + stateName + '",' + stateParams + ')');
                            $compile(el.contents())($rootScope);
                        }
                        else if(newValue == false){
                            angular.element(el).find('a').attr('ng-click', 'alertMessage()');
                            $compile(el.contents())(scope);
                        }
                    });

                    $rootScope.callOnClick = function () {
                        scope.onWorkflowClick();
                    }

                    scope.alertMessage = function (txt) {
                        alertService.error(txt ? txt : "The required fields on this screen must be completed before moving to the next screen.");
                    };
                }
            };
        }
    ]);
