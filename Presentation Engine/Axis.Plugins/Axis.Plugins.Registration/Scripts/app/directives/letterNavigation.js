angular.module('xenatixApp')
    .directive('letterNavigation', ['$stateParams', '$compile', 'assessmentService',
        function ($stateParams, $compile, assessmentService) {
            return {
                restrict: 'E',
                replace: true,
                scope: false,
                template: '<ul class="list-group text-uppercase"><placeholder /></ul>',

                link: function (scope, el, attrs) {
                    var responseID = $stateParams.ResponseID;
                    assessmentService.getAssessmentSections($stateParams.AssessmentID).then(function (data) {
                        if (data.ResultCode === 0) {
                            var assessmentSections = '';
                            for (var iIdx = 0; iIdx < data.DataItems.length; iIdx++) {
                                assessmentSections += $("<workflow-action>")
                                    .attr('data-title', data.DataItems[iIdx].SectionName)
                                    .attr('data-state-name', 'patientprofile.intake.navi.letters.letternavi.lettersSection')
                                    .attr('data-state-key', '-' + data.DataItems[iIdx].AssessmentSectionID)
                                    .attr('data-state-params', '{ ContactID: $stateParams.ContactID, ResponseID: $stateParams.ResponseID, SectionID: ' +
                                                                data.DataItems[iIdx].AssessmentSectionID + ',AssessmentID:' + $stateParams.AssessmentID + ', ReadOnly:$stateParams.ReadOnly,ContactLettersID:$stateParams.ContactLettersID }').
                                    attr('data-init-state', 'none').wrap('<div>').parent().html();
                            }
                            el.find('placeholder').replaceWith(assessmentSections);
                            $compile($(el))(scope);
                        }
                    });
                }
            }
        }
    ]);