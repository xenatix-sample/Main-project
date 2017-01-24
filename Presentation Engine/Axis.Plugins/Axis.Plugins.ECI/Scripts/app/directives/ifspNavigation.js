angular.module('xenatixApp')
    .directive('ifspNavigation', [
        'ifspService', 'assessmentService', '$stateParams', '$compile',
        function (ifspService, assessmentService, $stateParams, $compile) {
            return {
                restrict: 'E',
                replace: true,
                scope: false,
                template: '<ul class="list-group text-uppercase">' +
                    '<workflow-action data-title="IFSP" data-state-name="patientprofile.chart.ifsps.ifsp.header" data-state-params="{ ContactID: $stateParams.ContactID, IFSPID: $stateParams.IFSPID }" data-init-state="none"></workflow-action>' +
                    '<placeholder />' +
                    '<workflow-action data-title="Report" data-state-name="patientprofile.chart.ifsps.ifsp.report" data-state-params="{ ContactID: $stateParams.ContactID, IFSPID: $stateParams.IFSPID, ResponseID: $stateParams.ResponseID }" data-init-state="none"></workflow-action>' +
                    '</ul>',
                link: function (scope, el, attrs) {
                    ifspService.get($stateParams.ContactID, $stateParams.IFSPID).then(function (data) {
                        if (data.ResultCode === 0) {
                            var assessmentID = data.DataItems[0].AssessmentID;
                            var responseID = data.DataItems[0].ResponseID;
                            assessmentService.getAssessmentSections(assessmentID).then(function (data) {
                                if (data.ResultCode === 0) {
                                    var assessmentSections = '';
                                    for (var iIdx = 0; iIdx < data.DataItems.length; iIdx++) {
                                        assessmentSections += $("<workflow-action>").attr('data-title', data.DataItems[iIdx].SectionName).attr('data-state-name', 'patientprofile.chart.ifsps.ifsp.section').attr('data-state-key', '-' + data.DataItems[iIdx].AssessmentSectionID).attr('data-state-params', '{ ContactID: $stateParams.ContactID, IFSPID: $stateParams.IFSPID, SectionID: ' + data.DataItems[iIdx].AssessmentSectionID + ', ResponseID: ' + responseID + ', AssessmentID: ' + assessmentID + ' }').attr('data-init-state', 'none').wrap('<div>').parent().html();
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