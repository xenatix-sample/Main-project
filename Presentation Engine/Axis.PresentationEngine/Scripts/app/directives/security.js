angular.module('xenatixApp')
.directive('security', ['$log', '$timeout', '$compile', '$filter', 'roleSecurityService', 'credentialSecurityService', 'navigationService', 'auditService', function ($log, $timeout, $compile, $filter, roleSecurityService, credentialSecurityService, navigationService, auditService) {
    return {
        restrict: "A",
        priority: 2,
        scope: {
            permissionKey: '@',
            permission: '@',
            programUnits: '@',
            permissionMode: '@',
            onAction: '&',
            modules: '@',
            credentialKey: '@',
            isDisabled: '@',
            dualPermission: '@'
        },
        link: function (scope, element, attrs) {
           
            attrs.$observe('isDisabled', function (newValue, oldValue) {
                if (newValue !== oldValue && newValue == 'true') {
                    applySecurity(false, element, attrs, scope);
                }
            });

            attrs.$observe('permission', function (newValue, oldValue) {               
                if (newValue != PERMISSION.NONE && newValue !== oldValue) {
                    scope.programUnits = (scope.programUnits == 'null' ? null : scope.programUnits);

                    var hasPermission = false;

                    if (scope.credentialKey) {
                        hasPermission = credentialSecurityService.hasCredentialPermissionByForm(scope.credentialKey, CREDENTIAL_ACTION.FilloutForms);
                        if (!hasPermission) {
                            applySecurity(false, element, attrs, scope);
                            return;
                        }
                    }

                    // for Module permission
                    if (scope.modules && scope.permission && !scope.programUnits) {
                        hasPermission = roleSecurityService.hasModulePermission(scope.modules, scope.permission);
                        if (scope.dualPermission == 'true') {
                            applySecurity(hasPermission, element, attrs, scope);
                        }
                        else if (!hasPermission) {
                            applySecurity(false, element, attrs, scope);
                        }
                    }
                    else if (scope.permissionKey && scope.permission && !scope.programUnits) {// for CRUD permission
                        hasPermission = roleSecurityService.hasPermission(scope.permissionKey, scope.permission);
                        if (scope.dualPermission == 'true') {
                            applySecurity(hasPermission, element, attrs, scope);
                        }
                        else if (!hasPermission) {
                            applySecurity(false, element, attrs, scope);
                        }
                    }
                    else if (scope.programUnits) {
                        // for program level security
                        // check for company permission, if hasCompanyPermission then do nothing                        
                        if (scope.modules) {
                            hasPermission = roleSecurityService.hasModulePermission(scope.modules, scope.permission, PERMISSION_LEVEL.Company);
                        }
                        else {
                            hasPermission = roleSecurityService.hasPermission(scope.permissionKey, scope.permission, PERMISSION_LEVEL.Company);
                        }
                        if (!hasPermission) {
                            // check for program unit permission, if hasCompanyPermission then match programUnits with user's program unit access.
                            if (scope.modules) {
                                hasPermission = roleSecurityService.hasModulePermission(scope.modules, scope.permission, PERMISSION_LEVEL.ProgramUnit);
                            }
                            else {
                                hasPermission = roleSecurityService.hasPermission(scope.permissionKey, scope.permission, PERMISSION_LEVEL.ProgramUnit)
                            }

                            if (hasPermission) {
                                var programUnits = scope.programUnits.split(',');

                                navigationService.get().then(function (data) {
                                    var userProgramUnits = $filter('filter')(data.DataItems[0].UserOrganizationStructures, {
                                        DataKey: 'ProgramUnit'
                                    }, true);
                                    var isProgramUnitMatched = false;
                                    if (programUnits && userProgramUnits) {
                                        for (var i = 0; i < programUnits.length; i++) {
                                            if (!isNaN(programUnits[i])) {
                                                for (var j = 0; j < userProgramUnits.length; j++) {
                                                    if (!isNaN(userProgramUnits[j].MappingID) && programUnits[i] == userProgramUnits[j].MappingID) {
                                                        isProgramUnitMatched = true;
                                                        break;
                                                    }
                                                }
                                                if (isProgramUnitMatched)
                                                    break;
                                            }
                                        }
                                    }
                                    // If match then do nothing else applySecurity                                   
                                    if (scope.dualPermission == 'true') {
                                        applySecurity(isProgramUnitMatched, element, attrs, scope);
                                    }
                                    else if (!isProgramUnitMatched) {
                                        applySecurity(false, element, attrs, scope);
                                    }
                                });
                            }
                            else {
                                // if nither company nor prgram unit permission, then applySecurity
                                applySecurity(false, element, attrs, scope);
                            }
                        }
                        else {
                            // if have access applySecurity
                            if (scope.dualPermission == 'true') {
                                applySecurity(hasPermission, element, attrs, scope);
                            }
                        }
                    }
                }
            });

            // call action method
            scope.callback = function () {
                scope.onAction();
            };

            // apply security
            function applySecurity(hasPermission, element, attrs, scope) {
                $log.debug('**********security attributes***********');
                $log.debug(attrs);
                $log.debug('hasPermission - ' + hasPermission);

                var permissionMode = (attrs.onAction ? 'attach-action' : scope.permissionMode);
                switch (permissionMode) {
                    case PERMISSION_MODE.READONLY:
                        setControlsState(hasPermission, element, scope);
                        if (!hasPermission) {
                            scope.$parent.$on('applysecurity', function (event, objects) {
                                setControlsState(false, objects.element, scope);
                            });
                        }
                        break;
                    case PERMISSION_MODE.ATTACH_ACTION:
                        if (!hasPermission) {
                            $timeout(function () {
                                angular.element(element)
                                       .removeAttr('ui-sref href');

                                attrs.$set('ui-sref', null);
                                attrs.$set('href', null);
                                angular.element(element).unbind('click');
                                attrs.$set('ng-click', 'callback()');
                                attrs.$set('security', null);
                                $compile(element)(scope);
                            });
                        }
                        break;
                    default:
                        if (hasPermission) {
                            angular.element(element).not(".no-security").show();
                        }
                        else {
                            angular.element(element).not(".no-security").hide();
                        }
                }
            };

            // disable controls
            function setControlsState(state, element, scope) {
                $log.debug('**********security attributes***********');
                $log.debug(attrs);
                $log.debug('hasPermission - ' + state);
                $log.debug('**********security element***********');
                $log.debug(element);

                $timeout(function () {
                    //Class no-securiy prevents the control from getting disabled.
                    angular.element(element).find(".no-security").find("input, select, textarea, button").addClass('no-security');

                    angular.element(element).find("input, select, textarea, button").not(".no-security").prop("disabled", !state);
                    var ctrl = angular.element(element).find('.xencheckbox,td[ng-click],a[ng-click],.xenradiobutton').not(".no-security");

                    if (!state) {
                        ctrl.addClass("disabled").attr("disabled", true);
                        angular.element(element).find(".fa-plus-circle,.fa-minus-circle").not(".no-security").hide();
                    }
                    else {
                        ctrl.removeClass("disabled").removeAttr("disabled");
                        angular.element(element).find(".fa-plus-circle,.fa-minus-circle").not(".no-security").show();
                    }
                });
            }
        },
        controller: ['$scope', function ($scope) {
            this.scope = $scope;
        }]
    };
}])
.directive('applySecurity', function () {
    return {
        restrict: "A",
        link: function (scope, element, attrs) {
            scope.$emit('applysecurity', { element: element });
        }
    }
});
angular.module('xenatixApp')
  .directive('ngClick', ['auditService', '$state', function (auditService, $state) {
      return {
          restrict: 'A',
          priority: 1, // give it higher priority than built-in ng-click
          link: function (scope, element, attr) {

              if (element.attr('permission-key')) // || !element.attr('permission-key'))
              {
                  element.bind('click', function () {
                      var statePermissionKey = "DefaultKey";
                      if ($state.current.data)
                      {
                          statePermissionKey = $state.current.data.permissionKey;
                      }
                      //set permission key for auditing purpose. 
                      auditService.auditScreenModel.DataKey = element.attr('permission-key') ? element.attr('permission-key') : statePermissionKey;
                  })
              }
          }
      }
  }])