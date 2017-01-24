angular.module('xenatixApp')
    .directive("assessmentSection", ['$parse', '$compile', 'formService', function ($parse, $compile, formService) {
        return {
            restrict: "E",
            scope: false,
            link: function (scope, element, attrs) {
                scope.startDate = attrs.startDate;
                scope.endDate = attrs.endDate;
                scope.returnState = attrs.returnState;
                scope.permissionKey = attrs.permissionKey;
                scope.templateClassName = attrs.templateClassName;
                scope.isMandatory = !!(attrs.isMandatory);

                attrs.$observe('noAccessToOther', function (newValue, oldValue) {
                    if (newValue !== oldValue && newValue == 'true') {
                        scope.assessmentNoAccess = true;
                    }
                });
                
                scope.saveMethod = $parse(attrs.savemethod)(scope);
                if (scope.saveMethod) {
                    scope.saveData = scope.saveMethod;
                }
                else {
                    scope.saveData = scope.save;
                }

                attrs.$observe('pkid', function (newValue, oldValue) {
                    if (newValue != PERMISSION.NONE && newValue !== oldValue) {
                        scope.pkid = $parse(attrs.pkid)(scope);
                    }
                });

                scope.preDefinedData = $parse(attrs.prepopulateddata)(scope);
                scope.onPostAssessmentResponse = $parse(attrs.onPostAssessmentResponse)(scope);
                var customPrint = $parse(attrs.customPrint)(scope);
                scope.printReport = function (isNext, mandatory, hasErrors, keepForm, next) {
                    if (!customPrint) {
                        var isDirty = formService.isDirty();
                        scope.saveAssessment().then(function (response) {
                            var callback = $parse(attrs.onPrintReport)(scope);
                            callback(response, isDirty).then(function (data) {
                                if (!data.alreadyExecuted) {
                                    scope.reportModel = data;
                                    scope.reportModel.HasLoaded = true;
                                    $('#reportModal').modal('show');
                                }
                            });
                    });
                }
                    else {
                        customPrint();
                    }
                }


            },
            controller: 'assessmentController',
            controllerAs: 'assessmentController',
            templateUrl: '/Assessment/Assessment/Section'
        }
    }]);
