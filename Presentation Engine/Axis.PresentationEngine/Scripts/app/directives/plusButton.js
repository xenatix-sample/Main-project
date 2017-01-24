(function () {
    angular.module('xenatixApp')
      .directive('plusButton', ['formService',
    function (formService) {
        return {
            restrict: 'E',
            replace: true,
            scope: {
                title: '@',
                save: '&',
                reset: '&',
                permissionKey: '@',
                permission: '@',
                isDisabled: '@',
                iconClass:'@',
                ignoreDirty:'='
            },
            template: '<a ng-hide="{{isDisabled}}" href="javascript:void(0)" class="plain font-size-xlarge" role="button" ><i security permission-key="{{permissionKey}}" permission="{{permission}}" class="fa {{iconClass?iconClass:\'fa-plus-circle\'}}"></i></a>',
                link: function ($scope, elem, attrs) {
                    elem.off('keydown').on('keydown', function (e) {

                        if (e.which == 32) {
                            elem.click();
                            e.preventDefault();
                        }
                    });
                    elem.off('click').on('click', function (e) {
                        var isDirty = formService.isDirty();

                        var form = elem.inheritedData('$formController');
                        if (form != undefined && form.$name.indexOf("financialAssessmentForm") >= 0) {
                            isDirty = formService.isDirty(form.financialAssessmentDetailsForm.$name);
                        }

                        var ignoreDirtyMessage = $scope.ignoreDirty || false;
                        if (isDirty && !ignoreDirtyMessage) {
                            bootbox.confirm("Any unsaved data will be lost. Do you want to continue?", function (result) {
                                if (result == true) {
                                    $scope.reset();
                                }
                            });
                        }
                        else {
                            $scope.reset();
                        }

                        //to remove green color from grid row if button is clicked.
                        ClearGridSelection(attrs);
                        //
                    });

                }
            };
    }
      ]);
}());