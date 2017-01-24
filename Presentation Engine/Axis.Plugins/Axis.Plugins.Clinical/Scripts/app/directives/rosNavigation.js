angular.module('xenatixApp')
    .directive('rosNavigation', [
        'reviewOfSystemsService', 'assessmentService', 'registrationService', '$stateParams', '$compile',
        function (reviewOfSystemsService, assessmentService, registrationService, $stateParams, $compile) {
            return {
                restrict: 'E',
                replace: true,
                scope: false,
                template: '<ul class="list-group text-uppercase">' +
                    '<workflow-action data-title="ros header" data-state-name="patientprofile.chart.intake.reviewOfSystems.ros.header" data-state-params="{ ContactID: $stateParams.ContactID, RoSID: $stateParams.RoSID }" data-init-state="none"></workflow-action>' +
                    '<placeholder />' +
                    '<workflow-action data-title="Review" data-state-name="patientprofile.chart.intake.reviewOfSystems.ros.review" data-state-params="{ ContactID: $stateParams.ContactID, RoSID: $stateParams.RoSID }" data-init-state="none"></workflow-action>' +
                    '</ul>',
                link: function (scope, el, attrs) {

                    registrationService.get($stateParams.ContactID).then(function (data) {
                        if (data != undefined && data.DataItems != undefined && data.DataItems.length > 0) {
                            if (data.DataItems[0].ClientTypeID == PROGRAM_TYPE.ECI || data.DataItems[0].GenderID == 1)
                                scope.isEnabledFemaleOnlyWorkFlow = true;

                            reviewOfSystemsService.getReviewOfSystem($stateParams.ContactID, $stateParams.RoSID).then(function (data) {
                                if (data.ResultCode === 0) {
                                    var assessmentID = data.DataItems[0].AssessmentID;
                                    var responseID = data.DataItems[0].ResponseID;
                                    assessmentService.getAssessmentSections(assessmentID).then(function (data) {
                                        if (data.ResultCode === 0) {
                                            var assessmentSections = '';
                                            for (var iIdx = 0; iIdx < data.DataItems.length; iIdx++) {
                                                if (!(scope.isEnabledFemaleOnlyWorkFlow && data.DataItems[iIdx].SectionName == "Female Only"))
                                                    assessmentSections += $("<workflow-action>").attr('data-title', data.DataItems[iIdx].SectionName).attr('data-state-name', 'patientprofile.chart.intake.reviewOfSystems.ros.section').attr('data-state-key', '-' + data.DataItems[iIdx].AssessmentSectionID).attr('data-state-params', '{ ContactID: $stateParams.ContactID, RoSID: $stateParams.RoSID, SectionID: ' + data.DataItems[iIdx].AssessmentSectionID + ', ResponseID: ' + responseID + ' }').attr('data-init-state', 'none').wrap('<div>').parent().html();
                                            }
                                            el.find('placeholder').replaceWith(assessmentSections);
                                            $compile($(el))(scope);
                                        }
                                    });
                                } else {
                                    el.find('placeholder').replaceWith('');
                                }
                            }); // end of review of system get

                        }
                    }); // end of registration get


                }
            }
        }
    ]);