// by using a directive for form valiation, we are making a declarative, rather than imperative, solution to error handling
angular.module('xenatixApp')
    .directive('xenSubmit', ['$rootScope', '$document', 'alertService', 'roleSecurityService', 'formService', 'httpLoaderInterceptor', 'auditService', function ($rootScope, $document, alertService, roleSecurityService, formService, httpLoaderInterceptor, auditService) {
        return {
            restrict: 'E',
            scope: {
                id: '@',
                name: '@',
                value: '@',
                style: '@',
                permissionKey: '@',
                permission: '@',
                credentialKey: '@',
                isHidden: '=',
                isDisabled: '=',
                preventDisable: '=',
                isNext: '=',
                mandatory: '=',
                keepForm: '=',
                isPrint: '=',
                dualPermission:'@'
            },
            template: '<button type="button" id="{{id}}_xen_submit" name="{{name}}" ng-click="submit()" class="{{style}}" ng-if="!isHidden" ng-class="{ \'prevent-disable\': preventDisable }" security permission-key="{{permissionKey}}" ng-disabled="isDisabled" permission="{{permission}}" credential-key="{{credentialKey}}" dual-permission="{{dualPermission}}">{{value}}</button>',
            require: '^xenCheckForm',
            link: function (scope, elem, attr, controller) {
                if (!scope.isNext || scope.isPrint) {
                    scope.credentialKey = controller.scope.credentialKey;
                }
                scope.submit = function (next) {
                    var hasPermission = true;
                    if (controller.scope.permissionKey && controller.scope.permission)
                        hasPermission = roleSecurityService.hasPermission(controller.scope.permissionKey, controller.scope.permission);
                    if (!hasPermission) {
                        formService.reset();
                    }
                    if ((isSave() && !controller.scope.stopSave) || isNext() || isPrint()) {
                        next = controller.scope.stopNext ? false : (next || scope.isNext);
                        //capture data for screen auditing.
                        setScreenAuditData();

                        controller.submit(next, scope.mandatory, scope.keepForm, scope.isPrint);
                    }
                }

                //Clean attached form's event
                scope.$on('$destroy', function () {
                    $document.off(eventName);
                });

                var eventName = 'keydown.' + controller.scope.name + scope.name;
                // page shortcuts
                $document.off(eventName).on(eventName, function (e) {
                    if (httpLoaderInterceptor.loading() || $rootScope.isStopKeyEvents == true) {
                        e.preventDefault();
                        return;
                    }

                    // save - ctrl+s
                    if (e.ctrlKey && e.which === 83) {
                        if (isDefaultForm() && isSave()) {
                            scope.submit();
                            scope.$apply();
                        }
                        e.preventDefault();
                        return false;
                    }
                    // next - ctrl+e
                    if (e.ctrlKey && e.which === 69) {
                        if (isDefaultForm() && isNext()) {
                            scope.submit();
                            scope.$apply();
                        }
                        e.preventDefault();
                        return false;
                    }
                    // next - enter
                    if (e.which === 13) {
                        var action = getAttribute();
                        if (isDefaultForm() && (action.save.isExists && action.save.isExists == isSave() || !action.save.isExists && action.next.isExists == isNext())) {
                            scope.mandatory = action.next.isExists ? JSON.parse(action.next.isMandatory) : JSON.parse(action.save.isMandatory);
                            scope.submit(action.next.isExists);
                            scope.$apply();
                        }
                        e.preventDefault();
                        return false;
                    }
                });

                function isSave() {
                    return !scope.isNext && !scope.isPrint;
                }

                function isNext() {
                    return scope.isNext && !scope.isPrint;
                }

                function isPrint() {
                    return scope.isPrint;
                }

                function isDefaultForm() {
                    return $rootScope.defaultFormName === controller.scope.name;
                }

                function getAttribute() {
                    var saveAttribute = getActionAttribute('false');
                    var nextAttribute = getActionAttribute('true');
                    return {
                        save: saveAttribute,
                        next: nextAttribute
                    };
                }

                function getActionAttribute(action) {
                    var element = getElement(action);
                    var isExists = element.find('button').is(":visible");
                    if (isExists)
                        isExists = !(element.find('button').is(":disabled"));

                    var isMandatory = element.attr("mandatory");

                    return {
                        isExists: isExists,
                        isMandatory: isMandatory
                    };
                }

                function getElement(action) {
                    return $('[name="' + controller.scope.name + '"] xen-submit[is-next="' + action + '"]');
                }

                function setScreenAuditData()
                {
                    if (isPrint())
                    {
                        auditService.auditScreenModel.ActionTypeID = SCREEN_ACTIONTYPES.PrintView;                       
                    }
                    else {
                        auditService.auditScreenModel.ActionTypeID = undefined;
                    }
                }
            }
        }
    }]);
