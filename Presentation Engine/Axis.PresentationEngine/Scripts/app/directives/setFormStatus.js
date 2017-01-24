(function () {
    angular.module("xenatixApp")
    .directive('setFormStatus', ['$rootScope', function ($rootScope) {
        return {
            restrict: 'EA',
            require: '^form',
            link: function (scope, elem, attr, ctrl) {
                scope.$watch(function () { return ctrl.modified; }, function () {
                    if (attr.setFormStatus == '' || attr.name == '') {
                        $rootScope.setform(ctrl.modified);
                    } else {
                        $rootScope.setform(ctrl.modified, attr.name);
                    }
                }, true);
            }
        }
    }])
    .directive('setFormMode', ['$rootScope', '$stateParams', '$timeout', '$state', 'cacheService', function ($rootScope, $stateParams, $timeout, $state, cacheService) {
        return {
            restrict: 'A',
            compile: function (elem, attr) {
                return {
                    post: function (scope, elem, attr, ctrl) {
                        var disableControls = function () {
                            // Temp fix for followup. Readonly directive will be refactored and implemented
                            if (($stateParams.ReadOnly && $stateParams.ReadOnly.toString().toLowerCase() == "view")) {  // || ((cacheService.get('FollowUp') == true) && ($state.current.name == 'callcenter.crisisline.callerinformation'))) {
                                $timeout(function () {
                                    $(elem).find(":input , textarea, button").not(".prevent-disable").prop("disabled", true); //Exclude element 
                                    $(elem).find('.xencheckbox,td[ng-click],a[ng-click],.xenradiobutton').not(".prevent-disable").unbind("click").addClass("disabled");
                                    $(elem).find(".fa-plus-circle,.fa-minus-circle").not(".prevent-disable").remove();
                                    $(elem).find(":button").not(".prevent-disable").unbind("click");

                                    if (!scope.preventStopSave) //True in case cancel appointment
                                    {
                                        scope.isReadOnlyForm = true;
                                        scope.stopSave = ($stateParams.ReadOnly && $stateParams.ReadOnly.toString().toLowerCase() == "view");
                                    }
                                });
                            }
                        };
                        disableControls();
                        scope.$on('getComplete', function () { disableControls() });

                        scope.$on('$destroy', function () {
                            cleanReadOnlyField(scope);
                        });

                        var cleanReadOnlyField = function (item) {
                            delete item.isReadOnlyForm;
                            if (item.$parent) {
                                cleanReadOnlyField(item.$parent);
                            }
                            return;
                        }
                    }
                }
            },
        }
    }]);
}());