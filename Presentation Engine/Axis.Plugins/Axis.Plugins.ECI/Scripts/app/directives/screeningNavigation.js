angular.module('xenatixApp')
    .directive('screeningNavigation', [
        'screeningService', 'assessmentService', '$stateParams', '$compile',
        function (screeningService, assessmentService, $stateParams, $compile) {
            return {
                restrict: 'E',
                replace: true,
                scope: false,
                template: '<ul class="list-group text-uppercase">' +
                    '<workflow-action data-title="Screening header" data-state-name="patientprofile.chart.screenings.screening.header" data-state-params="{ ContactID: $stateParams.ContactID, ScreeningID: $stateParams.ScreeningID }" data-init-state="none"></workflow-action>' +
                    '<placeholder />' +
                    '<workflow-action data-title="Report" data-state-name="patientprofile.chart.screenings.screening.report" data-state-params="{ ContactID: $stateParams.ContactID, ScreeningID: $stateParams.ScreeningID }" data-init-state="none"></workflow-action>' +
                    '</ul>',
                link: function (scope, el, attrs) {
                    screeningService.get($stateParams.ContactID, $stateParams.ScreeningID).then(function (data) {
                        if (data.ResultCode === 0) {
                            var assessmentID = data.DataItems[0].AssessmentID;
                            var responseID = data.DataItems[0].ResponseID;
                            assessmentService.getAssessmentSections(assessmentID).then(function (data) {
                                if (data.ResultCode === 0) {
                                    var assessmentSections = '';
                                    for (var iIdx = 0; iIdx < data.DataItems.length; iIdx++) {
                                        assessmentSections += $("<workflow-action>").attr('data-title', data.DataItems[iIdx].SectionName).attr('data-state-name', 'patientprofile.chart.screenings.screening.section').attr('data-state-key', '-' + data.DataItems[iIdx].AssessmentSectionID).attr('data-state-params', '{ ContactID: $stateParams.ContactID, ScreeningID: $stateParams.ScreeningID, SectionID: ' + data.DataItems[iIdx].AssessmentSectionID + ', ResponseID: ' + responseID + ' }').attr('data-init-state', 'none').wrap('<div>').parent().html();
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