angular.module("xenatixApp")
    .directive("xenWorkflows", ["$compile", "$stateParams", "$rootScope", '$timeout', function ($compile, $stateParams, $rootScope, $timeout) {
    return {
        restrict: 'E',
        transclude: true,
        replace: true,
        scope: {
            workFlowOptions: "=",
            workFlowModel: "="
        },
        template: '<ul class="list-group text-uppercase">' +
                        '<div ng-repeat="rightNavigation in workFlowModel.workFlowItems" class="work-flow">' +
                            '<xen-workflow-action ng-if="!rightNavigation.onWorkflowClick" data-title="{{rightNavigation.title}}" data-state-name="{{rightNavigation.stateName}}" ' +
                                'data-state-key="{{rightNavigation.stateKey}}" data-state-params="{{rightNavigation.stateParams}}" ' + 
                                'data-init-state="none" data-init-active="{{rightNavigation.initActive}}" is-active="rightNavigation.isActive">' +
                            '</xen-workflow-action>' +
                            '<xen-workflow-action ng-if="rightNavigation.onWorkflowClick" data-title="{{rightNavigation.title}}" ' +
                                'data-init-active="{{rightNavigation.initActive}}" is-active="rightNavigation.isActive" on-workflow-click="rightNavigation.onWorkflowClick()"  data-init-state="none" >' +
                            '</xen-workflow-action>' +
                        '</div>' +
                        '<div ng-transclude ng-if="workFlowOptions.enableAdditionalWorkflow" class="padding-top-small"></div>' +
                    '</ul>',
        link: function (scope, element, attrs, ctrl) {
            scope.$watch("workFlowOptions", function (newValue, oldValue) {
                if (scope.workFlowOptions.enableWorkflow == null) {
                    //enableWorkflow : null     - enable all
                    angular.forEach(scope.workFlowModel.workFlowItems, function (item, index) {
                        item.isActive = true;
                    });
                }
                else if (scope.workFlowOptions.enableWorkflow == 0) {
                    //enableWorkflow : 0        - disable all
                    angular.forEach(scope.workFlowModel.workFlowItems, function (item, index) {
                        if (index > 0) {
                            item.isActive = false;
                        }
                    });
                }
                else {
                    //enableWorkflow : [1,2,3]    - enable 1,2 & 3 step
                    angular.forEach(scope.workFlowOptions.enableWorkflow, function (item, index) {
                        scope.workFlowModel.workFlowItems[item - 1].isActive = true;
                    });
                }
            }, true);
        }
    }
}]);
