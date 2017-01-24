angular.module('xenatixApp')
    .directive('xenComment', ['$stateParams', '$filter',
      function ($stateParams, $filter) {
          return {
              restrict: 'E',
              replace: true,
              scope: {
                  commentModel: '=',
                  historyModel: '=',
                  id: '@',
                  name: '@',
                  rows: '=',
                  maxlength: '=',
                  label: '@',
                  isDisabled: '=',
                  className: '@?',
                  xenCommentRequired: '=',
                  commentText: '@?',
                  dontNeedToggle: '='
              },
              template:
                '<div>' +
                    '<i ng-hide="dontNeedToggle" ' +
                        'class="{{openHistory? \'fa fa-minus-circle prevent-disable\':\'fa fa-plus-circle prevent-disable\'}}" ' +
                        'ng-click="toggleHistory()">' +
                    '</i>' +
                    '<div ng-hide="!openHistory">' +
                        '<xen-memobox ng-Model="commentModel" id="{{id}}_xen_comment" name="{{name}}" rows="rows" maxlength="maxlength" label="{{label}}" is-disabled="isDisabled" ng-required="xenCommentRequired" class-name="{{className}} prevent-disable"></xen-memobox>' +
                        '<div class="row"><ul> ' +
                            '<li ng-repeat="hist in historyModel track by $index">' +
                                '<span class="ase-comment-attr break-word"><b>{{hist.UserName}}</b> {{commentText!=undefined ? "added" : "commented"}}  on {{hist.CommentDate|toMMDDYYYYDate:"MM/DD/YYYY hh:mm:ss A":"useLocal"}}</span>' +
                                '<p class="ase-comment-content break-word">{{hist.Comment}}</p>' +
                            '</li>' +
                        '</ul></div>' +
                    '</div>' +
                '</div>',
              link: function ($scope, element, attrs, ctrl) {
                  if ($scope.dontNeedToggle) {
                      $scope.openHistory = true;
                  }
                  $scope.toggleHistory = function () {
                      $scope.openHistory = !$scope.openHistory;
                  };
              }
          };
      }
    ]);
