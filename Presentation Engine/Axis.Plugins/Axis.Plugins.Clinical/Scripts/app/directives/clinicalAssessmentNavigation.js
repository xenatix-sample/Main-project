angular.module('xenatixApp')
    .directive('clinicalAssessmentNavigation', [
        'clinicalAssessmentService', 'assessmentService', '$stateParams', '$compile',
        function (clinicalAssessmentService, assessmentService, $stateParams, $compile) {
            return {
                restrict: 'E',
                replace: true,
                scope: false,
                template: '<ul class="list-group text-uppercase">' +
                    '<workflow-action data-title="clinical assessment header" data-state-name="patientprofile.chart.intake.clinicalAssessment.ca.header" data-state-params="{ ContactID: $stateParams.ContactID, ClinicalAssessmentID: $stateParams.ClinicalAssessmentID }" data-init-state="none"></workflow-action>' +
                    '<placeholder />',
                link: function (scope, el, attrs) {
                    clinicalAssessmentService.getClinicalAssessment($stateParams.ContactID, $stateParams.ClinicalAssessmentID).then(function (data) {
                        if (data.ResultCode === 0) {
                            var assessmentID = data.DataItems[0].AssessmentID;
                            var responseID = data.DataItems[0].ResponseID;
                            assessmentService.getAssessmentSections(assessmentID).then(function (data) {
                                if (data.ResultCode === 0) {
                                    var assessmentSections = '';
                                    for (var iIdx = 0; iIdx < data.DataItems.length; iIdx++) {
                                        assessmentSections += $("<workflow-action>").attr('data-title', data.DataItems[iIdx].SectionName).attr('data-state-name', 'patientprofile.chart.intake.clinicalAssessment.ca.section').attr('data-state-key', '-' + data.DataItems[iIdx].AssessmentSectionID).attr('data-state-params', '{ ContactID: $stateParams.ContactID, ClinicalAssessmentID: $stateParams.ClinicalAssessmentID, SectionID: ' + data.DataItems[iIdx].AssessmentSectionID + ', ResponseID: ' + responseID + ' }').attr('data-init-state', 'none').wrap('<div>').parent().html();
                                    }
                                    el.find('placeholder').replaceWith(assessmentSections);
                                    $compile($(el))(scope);
                                }
                            });
                        } else {
                            el.find('placeholder').replaceWith('');
                        }
                    });
                }
            }
        }
    ]);