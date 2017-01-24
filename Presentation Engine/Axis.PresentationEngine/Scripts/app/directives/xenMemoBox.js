angular.module('xenatixApp')
    .directive('xenMemobox', ['$stateParams',
      function ($stateParams) {
          return {
              restrict: 'E',
              replace: true,
              scope: {
                  ngModel: '=',
                  id: '@',
                  rows: '=',
                  maxlength: '=',
                  label: '@',
                  isDisabled: '=',
                  className: '@?',
                  onChange: '&',
                  onChangeFieldId: '@'
              },
              template:
                '<div class="xenmemobox">' +
                    '<label for="{{id}}_xen_memo">{{label}}</label>' +
                    '<span>' +
                        '{{ (maxlength - ngModel.length) >= 0 ? (maxlength - ngModel.length) : 0 }} ' +
                        'character{{(maxlength - ngModel.length == 1 ? "" : "s")}} ' +
                        'remaining' +
                    '</span>' +
                    '<textarea style="width:100%" ng-disabled="isDisabled" ng-trim="false" class="form-control {{className}}" id="{{id}}_xen_memo" rows="{{rows}}" xen-maxlength="{{maxlength}}" ng-model="ngModel">' +
                    '</textarea>' +
                '</div>',
              link: function (scope, elem, attrs) {
                  if (scope.onChange != undefined && elem[0].id == scope.onChangeFieldId) {
                      scope.$watch('ngModel', function (newValue, oldValue) {
                          if (newValue != undefined && oldValue != undefined && newValue != oldValue)
                              scope.onChange();
                      });
                  }
              }
          };
      }
    ]);
