angular.module("xenatixApp").directive("workFlowsSet", ["$compile", "$stateParams", "$rootScope", function ($compile, $stateParams, $rootScope) {
    return {
        scope: {
            workFlowActiveOption: "=",
            workFlowModel: "=",
            workFlowReady: "="
        },
        link: function (scope, element, attrs, ctrl) {
            var rootScopeClone;
            scope.$watch("workFlowActiveOption", function (newValue, oldValue) {
                if (newValue.isDisabled) {
                    disableSpecificWorkFlowSet(newValue.workFlowEnableAction, newValue.alertMessage);
                }
                else if (newValue.workFlowEnableAction == 0) {
                    activeAllWorkFlowSet();
                }
                else if (angular.isArray(newValue.workFlowEnableAction)) {
                    activeSpecificWorkFlowSets(newValue.workFlowEnableAction);
                }
                else
                    activeSpecificWorkFlow(newValue.workFlowEnableAction);
            });

            var disableSpecificWorkFlowSet = function (activeDecision, alertMessage) {
                if (scope.workFlowModel.workFlowItems != undefined && scope.workFlowModel.workFlowItems.length > 0) {
                    var rightNavigationItem = scope.workFlowModel.workFlowItems[activeDecision - 1];
                    if (rightNavigationItem != undefined) {
                        rightNavigationItem.isEnabled = true;
                        var workFlowAction = rightNavigationItem.stateName;
                        var elem = angular.element("li[data-state-name='" + workFlowAction + "']");
                        angular.element(elem.find("a")).off('click');
                        if (alertMessage)
                        {
                            angular.element(elem.find("a")).attr("ng-click", 'alertMessage("' + alertMessage + '")');
                            rootScopeClone && rootScopeClone.$destroy();
                            rootScopeClone = $rootScope.$new();
                            $compile(element)($rootScope);
                        }
                    }
                }
            };

            var activeViewButtons = function () {
                rootScopeClone && rootScopeClone.$destroy();
                rootScopeClone = $rootScope.$new();
                angular.element("div.btn-enable").attr("ng-show", scope.enableViewButtons);
                $compile(element)(rootScopeClone);
            };

            var activeSpecificWorkFlowSets = function (activeList) {
                if (scope.workFlowModel.workFlowItems != undefined && scope.workFlowModel.workFlowItems.length > 0) {
                    if (angular.isArray(activeList)) {
                        angular.forEach(activeList, function (item, index) {
                            var rightNavigationItem = scope.workFlowModel.workFlowItems[item - 1];
                            if (rightNavigationItem != undefined) {
                                rightNavigationItem.isEnabled = true;
                                var workFlowAction = rightNavigationItem.stateName;
                                var element = angular.element("li[data-state-name='" + workFlowAction + "']");
                                angular.element(element.find("a")).off('click');
                                if (rightNavigationItem.stateParams != "")
                                    angular.element(element.find("a")).attr("ng-click", 'Goto("' + rightNavigationItem.stateName + '",' + rightNavigationItem.stateParams + ')');
                                else
                                    angular.element(element.find("a")).attr("ng-click", 'Goto("' + rightNavigationItem.stateName + '",{})');

                            }
                        });

                        rootScopeClone && rootScopeClone.$destroy();
                        rootScopeClone = $rootScope.$new();
                        $compile(element)($rootScope);
                    }
                }
            };

            var activeSpecificWorkFlow = function (activeDecision) {
                if (scope.workFlowModel.workFlowItems != undefined && scope.workFlowModel.workFlowItems.length > 0) {
                    var rightNavigationItem = scope.workFlowModel.workFlowItems[activeDecision - 1];
                    if (rightNavigationItem != undefined) {
                        rightNavigationItem.isEnabled = true;
                        var workFlowAction = rightNavigationItem.stateName;
                        var element = angular.element("li[data-state-name='" + workFlowAction + "']");
                        angular.element(element.find("a")).off('click');
                        if (rightNavigationItem.stateParams != "")
                            angular.element(element.find("a")).attr("ng-click", 'Goto("' + rightNavigationItem.stateName + '",' + rightNavigationItem.stateParams + ')');
                        else
                            angular.element(element.find("a")).attr("ng-click", 'Goto("' + rightNavigationItem.stateName + '",{})');
                        $compile(element)($rootScope);
                    }
                }
            };

            var activeAllWorkFlowSet = function (activeDecision) {
                if (scope.workFlowModel.workFlowItems != undefined) {
                    angular.forEach(scope.workFlowModel.workFlowItems, function (rightNavigation, index) {
                            rightNavigation.isEnabled = true;
                            var element = angular.element("li[data-state-name='" + rightNavigation.stateName + "']");
                            angular.element(element.find("a")).off('click');
                            if (rightNavigation.stateParams != "")
                                angular.element(element.find("a")).attr("ng-click", 'Goto("' + rightNavigation.stateName + '",' + rightNavigation.stateParams + ')');
                            else
                                angular.element(element.find("a")).attr("ng-click", 'Goto("' + rightNavigation.stateName + '",{})');
                    });
                    
                    rootScopeClone && rootScopeClone.$destroy();
                    rootScopeClone = $rootScope.$new();
                    angular.element("div.btn-enable").attr("ng-show", scope.workFlowActiveOption.enableViewButtons);
                    $compile(element)(rootScopeClone);
                }
            };

            var InitilizeWorkFlowSet = function (scope) {
                var assessmentSections = '';
                if (scope.workFlowModel) {
                    if (scope.workFlowModel.workFlowItems != undefined && scope.workFlowModel.workFlowItems.length > 0) {
                        var assessmentSections = "<ul class='list-group text-uppercase'>";
                        angular.forEach(scope.workFlowModel.workFlowItems, function (rightNavigation, index) {
                            var enabledLink = (scope.workFlowActiveOption.workFlowEnableAction == 0) ? true : scope.workFlowActiveOption.workFlowEnableAction === index + 1 ? true : false;

                            if (!rightNavigation.isEnabled)
                                rightNavigation.isEnabled = enabledLink;

                            if (index == 0)
                                assessmentSections += $("<workflow-action>").attr('data-title', rightNavigation.title).attr('data-state-name', rightNavigation.stateName).attr('data-state-params', rightNavigation.stateParams).attr('data-init-active', rightNavigation.initActive).attr('data-init-state', 'none').wrap('<div>').parent().html();
                            else
                                assessmentSections += $("<workflow-action>").attr('data-title', rightNavigation.title).attr('data-state-name', rightNavigation.stateName).attr('data-state-params', rightNavigation.stateParams).attr('data-state-enabled', rightNavigation.isEnabled).attr('data-init-state', 'none').wrap('<div>').parent().html();
                        });
                        assessmentSections += "</ul>";
                        var enableButtons = false;
                        if (scope.workFlowModel.additonalWorkFlowItems != undefined && scope.workFlowModel.additonalWorkFlowItems.length > 0) {

                            assessmentSections += '<div  class="btn-group btn-enable"  ng-show=' + enableButtons + '>';
                            scope.workFlowModel.additonalWorkFlowItems.forEach(function (viewButton) {
                                if (viewButton.stateParams != "") {
                                    assessmentSections += '<a href=# ng-click="Goto(\'' + viewButton.stateName + '\',' + viewButton.stateParams + ',false,' + enableButtons + ')" class="btn btn-default"><span tab-action>' + viewButton.title + '</span> <i class="fa ' + (viewButton.iconClass || 'fa-user') + ' pull-right padding-left-small"></i> </a>';
                                }
                                else {
                                    assessmentSections += '<a href=# ng-click="Goto(\'' + viewButton.stateName + '\',{},false,' + enableButtons + ')" class="btn btn-default"><span tab-action>' + viewButton.title + '</span> <i class="fa ' + (viewButton.iconClass || 'fa-calendar') + ' pull-right padding-left-small"></i> </a>';
                                }
                            });
                            assessmentSections += '</div>';
                        }
                        element.html('');
                        element.append(assessmentSections);
                        $compile(element)($rootScope);
                    }
                }
            };

            var Init = function (scope) {
                InitilizeWorkFlowSet(scope);
            };
            scope.workFlowModel = {
                Init: function () { Init(scope); }
            };
            scope.workFlowReady = true;

        }
    }
}]);